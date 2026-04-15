using Microsoft.AspNetCore.Mvc;
using DeviceManager.Api.Services;
using DeviceManager.Api.DTOs;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly AuthServiceInterface _authService;

    public AccountController(AuthServiceInterface authService) => _authService = authService;

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = true, // Set to false if testing without HTTPS, true for Production
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var (result, accessToken, refreshToken) = await _authService.RegisterAsync(model);
        if (!result.IsSuccess) return BadRequest(result);

        SetTokenCookies(accessToken!, refreshToken!);
        return Ok(result); 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var (result, accessToken, refreshToken) = await _authService.LoginAsync(model);
        if (!result.IsSuccess) return Unauthorized(result);

        SetTokenCookies(accessToken!, refreshToken!);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var oldRefreshToken = Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(oldRefreshToken)) return Unauthorized("No refresh token found.");

        var (result, newAccessToken, newRefreshToken) = await _authService.RefreshTokenAsync(oldRefreshToken);
        if (!result.IsSuccess) return Unauthorized(result);

        SetTokenCookies(newAccessToken!, newRefreshToken!);
        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // This tells the browser to instantly expire and delete the secure cookies
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");
        
        return Ok(new { message = "Logged out successfully" });
    }
}