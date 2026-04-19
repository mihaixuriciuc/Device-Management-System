using DeviceManager.Api.DTOs;

namespace DeviceManager.Api.Services;

public interface AuthServiceInterface
{
    // Register returns the JSON result AND the raw tokens for automatic login
    Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> RegisterAsync(RegisterDto model);
    
    // Login returns the JSON result AND the raw tokens
    Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> LoginAsync(LoginDto model);
    
    // Refresh only takes the old token string, and returns the new set
    Task<(AuthResponseDto Result, string? AccessToken, string? RefreshToken)> RefreshTokenAsync(string oldRefreshToken);
}