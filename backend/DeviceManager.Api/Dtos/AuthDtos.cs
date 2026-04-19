using System.ComponentModel.DataAnnotations;

namespace DeviceManager.Api.DTOs;

public record RegisterDto(
    [Required(ErrorMessage = "Email is required")] 
    [EmailAddress(ErrorMessage = "Invalid email format")] 
    string Email, 
    
    [Required(ErrorMessage = "Password is required")] 
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    string Password, 
    
    [Required(ErrorMessage = "First name is required")] 
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    string FirstName, 
    
    [Required(ErrorMessage = "Last name is required")] 
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    string LastName
);

public record LoginDto(
    [Required(ErrorMessage = "Email is required")] 
    [EmailAddress] 
    string Email, 
    
    [Required(ErrorMessage = "Password is required")] 
    string Password
);

public record AuthResponseDto(
    bool IsSuccess, 
    string? Message, 
    string? Role,
    string? FirstName,   // ← Add this
    string? LastName     // ← Add this
);