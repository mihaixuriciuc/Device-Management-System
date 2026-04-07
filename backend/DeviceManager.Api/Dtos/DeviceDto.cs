using System.ComponentModel.DataAnnotations;

namespace DeviceManager.Api.Models;

public class DeviceDto
{
  [Required(ErrorMessage = "Name is mandatory")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "Manufacturer is mandatory")]
  public string Manufacturer { get; set; } = string.Empty;

  [Required]
  [RegularExpression("^(Phone|Tablet)$",
      ErrorMessage = "Type must be 'Phone' or 'Tablet'")]
  public string Type { get; set; } = string.Empty;

  [Required]
  public string OperatingSystem { get; set; } = string.Empty;

  [Required]
  public string OsVersion { get; set; } = string.Empty;

  [Required]
  public string Processor { get; set; } = string.Empty;

  [Required]
  // This Regex ensures it ends in 'GB' and the number is 1-3 digits
  [RegularExpression(@"^([1-9]|[1-9][0-9]|[1-4][0-9][0-9]|512)GB$",
      ErrorMessage = "RAM must be between 1GB and 512GB (e.g., '16GB')")]
  public string RamAmount { get; set; } = string.Empty;

  [Required]
  public string Description { get; set; } = string.Empty;

  [Required]
  [RegularExpression("^(Available|Unavailable|In Service|Retired)$",
      ErrorMessage = "Status must be: Available, Unavailable, In Service, or Retired")]
  public string Status { get; set; } = "Available";
}