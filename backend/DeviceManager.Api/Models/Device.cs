namespace DeviceManager.Api.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(SerialNumber), IsUnique = true)] // Database-level uniqueness
public class Device
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string SerialNumber { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;

  public string Manufacturer { get; set; } = string.Empty;

  //only phone or tablet
  public string Type { get; set; } = string.Empty;

  public string OperatingSystem { get; set; } = string.Empty;

  public string OsVersion { get; set; } = string.Empty;

  public string Processor { get; set; } = string.Empty;

  public string RamAmount { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public string Status { get; set; } = "Available";

  public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}