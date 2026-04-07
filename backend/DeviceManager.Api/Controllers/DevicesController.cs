using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Data;
using DeviceManager.Api.Models;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // This sets the URL to: localhost:xxxx/api/devices
public class DevicesController : ControllerBase
{
  private readonly AppDbContext _context;

  public DevicesController(AppDbContext context)
  {
    _context = context;
  }

  // 1. GET: api/devices (READ ALL)
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
  {
    return await _context.Devices.ToListAsync();
  }

  // 2. GET: api/devices/5 (READ ONE)
  [HttpGet("{id}")]
  public async Task<ActionResult<Device>> GetDevice(int id)
  {
    var device = await _context.Devices.FindAsync(id);

    if (device == null)
    {
      return NotFound();
    }

    return device;
  }

  // 3. POST: api/devices (CREATE)
  [HttpPost]
  public async Task<ActionResult<Device>> PostDevice(DeviceDto deviceDto)
  {
    // You MUST map the DTO to the Model here
    var device = new Device
    {
      Name = deviceDto.Name,
      Manufacturer = deviceDto.Manufacturer,
      Type = deviceDto.Type,
      OperatingSystem = deviceDto.OperatingSystem,
      OsVersion = deviceDto.OsVersion,
      Processor = deviceDto.Processor,
      RamAmount = deviceDto.RamAmount, // Ensure this matches!
      Description = deviceDto.Description,
      Status = deviceDto.Status,
      DateAdded = DateTime.UtcNow // Set by server, not user
    };

    _context.Devices.Add(device);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> PutDevice(int id, DeviceDto deviceDto)
  {
    // 1. Find the existing device in the database
    var deviceInDb = await _context.Devices.FindAsync(id);

    // 2. If it doesn't exist, tell the user
    if (deviceInDb == null)
    {
      return NotFound($"Device with ID {id} not found.");
    }

    // 3. Map the DTO values to the existing database record
    // We do NOT touch 'Id' or 'DateAdded' here!
    deviceInDb.Name = deviceDto.Name;
    deviceInDb.Manufacturer = deviceDto.Manufacturer;
    deviceInDb.Type = deviceDto.Type;
    deviceInDb.OperatingSystem = deviceDto.OperatingSystem;
    deviceInDb.OsVersion = deviceDto.OsVersion;
    deviceInDb.Processor = deviceDto.Processor;
    deviceInDb.RamAmount = deviceDto.RamAmount;
    deviceInDb.Description = deviceDto.Description;
    deviceInDb.Status = deviceDto.Status;

    // 4. Save the changes
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!DeviceExists(id)) return NotFound();
      else throw;
    }

    // 5. Return 204 No Content (Standard for successful updates)
    return NoContent();
  }


  // 5. DELETE: api/devices/5 (DELETE)
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteDevice(int id)
  {
    var device = await _context.Devices.FindAsync(id);
    if (device == null)
    {
      return NotFound();
    }

    _context.Devices.Remove(device);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  // Helper method used by the PutDevice above
  private bool DeviceExists(int id)
  {
    return _context.Devices.Any(e => e.Id == id);
  }
}