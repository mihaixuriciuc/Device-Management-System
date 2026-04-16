using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DeviceManager.Api.Models;
using DeviceManager.Api.Services;

namespace DeviceManager.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly DeviceServiceInterface _deviceService;

    public DevicesController(DeviceServiceInterface deviceService)
    {
        _deviceService = deviceService;
    }

    private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetAll() 
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        return device == null ? NotFound() : Ok(device);
    }

    [HttpGet("my-devices")]
    public async Task<IActionResult> GetMyDevices()
    {
        return Ok(await _deviceService.GetDevicesByUserIdAsync(GetCurrentUserId()));
    }

    [HttpPost("{id}/assign")]
    public async Task<IActionResult> Assign(int id)
    {
        try {
            await _deviceService.AssignDeviceAsync(id, GetCurrentUserId());
            return Ok(new { message = "Assigned" });
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/unassign")]
    public async Task<IActionResult> Unassign(int id)
    {
        try {
            await _deviceService.UnassignDeviceAsync(id, GetCurrentUserId());
            return Ok(new { message = "Unassigned" });
        } catch (UnauthorizedAccessException) {
            return Forbid();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(DeviceDto dto)
    {
        try {
            var device = await _deviceService.CreateDeviceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        } catch (InvalidOperationException ex) {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, DeviceDto dto)
    {
        try {
            var result = await _deviceService.UpdateDeviceAsync(id, dto);
            return result == null ? NotFound() : NoContent();
        } catch (InvalidOperationException ex) {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _deviceService.DeleteDeviceAsync(id) ? NoContent() : NotFound();
    }

    [HttpGet("check-serial")]
    public async Task<IActionResult> CheckSerialExists([FromQuery] string sn)
    {
        if (string.IsNullOrWhiteSpace(sn))
            return BadRequest(new { message = "Serial number is required" });

        var exists = await _deviceService.CheckSerialNumberExistsAsync(sn);
        return Ok(exists);
    }
}