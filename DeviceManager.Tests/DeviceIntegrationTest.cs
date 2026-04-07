using System.Net;
using System.Net.Http.Json;
using DeviceManager.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DeviceManager.Tests;

public class DeviceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public DeviceIntegrationTests(WebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Post_DuplicateSerialNumber_ReturnsConflict()
  {
    var testSerial = "DUPE-" + Guid.NewGuid().ToString().Substring(0, 5);
    var device = CreateDevice(testSerial);

    await _client.PostAsJsonAsync("/api/devices", device);
    var response = await _client.PostAsJsonAsync("/api/devices", device);

    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
  }

  [Fact]
  public async Task Post_NewDevice_CanBeRetrieved()
  {
    // Arrange
    var uniqueSerial = "SN-" + Guid.NewGuid().ToString().Substring(0, 8);
    var newDevice = CreateDevice(uniqueSerial);

    // Act
    var postResponse = await _client.PostAsJsonAsync("/api/devices", newDevice);

    // DEBUG: If this fails, it prints the status code
    postResponse.EnsureSuccessStatusCode();

    // DEBUG: Let's read the raw string first to see what the test sees
    var rawJson = await postResponse.Content.ReadAsStringAsync();
    // If the test fails here, look at the output to see if "status" has quotes or not
    Assert.Contains("\"status\":\"Available\"", rawJson.Replace(" ", ""));

    var createdDevice = await postResponse.Content.ReadFromJsonAsync<Device>();

    // Assert
    Assert.NotNull(createdDevice);
    var getResponse = await _client.GetAsync($"/api/devices/{createdDevice!.Id}");
    Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
  }
  [Fact]
  public async Task Post_DeviceWithMissingName_ReturnsBadRequest()
  {
    // Arrange: Name is just a space (should trigger [Required])
    var invalidDevice = CreateDevice("FAIL-1");
    invalidDevice.Name = " "; // Empty string triggers validation

    // Act
    var response = await _client.PostAsJsonAsync("/api/devices", invalidDevice);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact]
  public async Task Get_NonExistentDevice_ReturnsNotFound()
  {
    var response = await _client.GetAsync("/api/devices/999999");
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  // Helper to avoid repeating code
  private DeviceDto CreateDevice(string serial) => new DeviceDto
  {
    SerialNumber = serial,
    Name = "Test Name",
    Manufacturer = "Test Brand",
    Type = "Phone",
    OperatingSystem = "Android",
    OsVersion = "14",
    Processor = "Snapdragon",
    RamAmount = "12GB",
    Description = "Test Desc",
    Status = "Available"
  };
}