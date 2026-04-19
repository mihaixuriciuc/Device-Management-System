using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using DeviceManager.Api.Models;
using DeviceManager.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DeviceManager.Api.Services;

public class AuthService : AuthServiceInterface
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    // --- 1. REGISTER ---
    public async Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> RegisterAsync(RegisterDto model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null) 
            return (new AuthResponseDto(false, "User already exists.", null, null, null), null, null);

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded) 
            return (new AuthResponseDto(false, "Registration failed.", null, null, null), null, null);

        // Generate tokens for automatic login
        var accessToken = await GenerateJwtToken(user); 
        var refreshToken = GenerateRefreshToken();

        // Save the refresh token to the database
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.AddToRoleAsync(user, "User");

        // Get role and return real user name
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        var responseDto = new AuthResponseDto(
            true, 
            "Registration and Login successful!", 
            userRole,
            user.FirstName,
            user.LastName
        );

        return (responseDto, accessToken, refreshToken);
    }

    // --- 2. LOGIN ---
    public async Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return (new AuthResponseDto(false, "Invalid credentials.", null, null, null), null, null);

        // Generate fresh tokens
        var accessToken = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        // Save the refresh token to the database
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        // Get role and return real user name
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        var responseDto = new AuthResponseDto(
            true, 
            "Login successful.", 
            userRole,
            user.FirstName,
            user.LastName
        );

        return (responseDto, accessToken, refreshToken);
    }

    // --- 3. REFRESH TOKEN ROTATION ---
    public async Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> RefreshTokenAsync(string oldRefreshToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == oldRefreshToken);

        // If no user matches the token, or it expired, force a fresh login
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return (new AuthResponseDto(false, "Invalid or expired token. Please log in again.", null, null, null), null, null);

        // Generate replacement tokens
        var newAccessToken = await GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        // Overwrite the old token in the DB (Rotation)
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
        await _userManager.UpdateAsync(user);

        // Get role and return real user name
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        var responseDto = new AuthResponseDto(
            true, 
            "Tokens rotated successfully.", 
            userRole,
            user.FirstName,
            user.LastName
        );

        return (responseDto, newAccessToken, newRefreshToken);
    }

    // --- HELPER: GENERATE JWT (ACCESS TOKEN) ---
    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

        // Add roles to token
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // --- HELPER: GENERATE SECURE STRING (REFRESH TOKEN) ---
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}