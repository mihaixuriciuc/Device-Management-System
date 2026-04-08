using Microsoft.AspNetCore.Mvc;
using DeviceManager.Api.Models;
using DeviceManager.Api.Services;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceServiceInterface _deviceService;

    // Inject the Service, NOT the DbContext
    public DevicesController(DeviceServiceInterface deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetDevice(int id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        
        if (device == null) return NotFound();
        
        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(DeviceDto deviceDto)
    {
        try
        {
            var device = await _deviceService.CreateDeviceAsync(deviceDto);
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }
        catch (InvalidOperationException ex)
        {
            // Catch our custom duplicate serial error
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(int id, DeviceDto deviceDto)
    {
        try
        {
            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, deviceDto);
            
            if (updatedDevice == null) return NotFound();
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var success = await _deviceService.DeleteDeviceAsync(id);
        
        if (!success) return NotFound();
        
        return NoContent();
    }

    // The endpoint for your Angular Async Validator
    [HttpGet("check-serial")]
    public async Task<IActionResult> CheckSerialNumberExists([FromQuery] string sn)
    {
        var exists = await _deviceService.CheckSerialNumberExistsAsync(sn);
        return Ok(exists);
    }
}