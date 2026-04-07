namespace DeviceManager.Api.Models;

public class Device
{
  // The Primary Key (matches [Id] in SQL)
  public int Id { get; set; }

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