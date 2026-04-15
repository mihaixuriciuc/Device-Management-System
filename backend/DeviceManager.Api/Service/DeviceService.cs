using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Data;
using DeviceManager.Api.Models;
using DeviceManager.Api.DTOs; // Ensure your DTO namespace is correct

namespace DeviceManager.Api.Services;

public class DeviceService : DeviceServiceInterface
{
    private readonly AppDbContext _context;

    public DeviceService(AppDbContext context)
    {
        _context = context;
    }

    // GET ALL: Now includes the User and maps to DTO
    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices
            .Include(d => d.AssignedUser) // ⬅️ The "Magic" join
            .ToListAsync();
    }

    // GET BY ID: Changed FindAsync to FirstOrDefaultAsync to allow .Include()
    public async Task<Device?> GetDeviceByIdAsync(int id)
    {
        return await _context.Devices
            .Include(d => d.AssignedUser)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Device> CreateDeviceAsync(DeviceDto deviceDto)
    {
        if (await CheckSerialNumberExistsAsync(deviceDto.SerialNumber))
        {
            throw new InvalidOperationException($"Serial Number {deviceDto.SerialNumber} is already registered.");
        }

        var device = new Device
        {
            SerialNumber = deviceDto.SerialNumber,
            Name = deviceDto.Name,
            Manufacturer = deviceDto.Manufacturer,
            Type = deviceDto.Type,
            OperatingSystem = deviceDto.OperatingSystem,
            OsVersion = deviceDto.OsVersion,
            Processor = deviceDto.Processor,
            RamAmount = deviceDto.RamAmount,
            Description = deviceDto.Description,
            Status = deviceDto.Status, // Uses your 4-state status strings
            DateAdded = DateTime.UtcNow
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device?> UpdateDeviceAsync(int id, DeviceDto deviceDto)
    {
        var deviceInDb = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (deviceInDb == null) return null;

        if (await CheckSerialNumberExistsAsync(deviceDto.SerialNumber, id))
        {
            throw new InvalidOperationException($"Serial Number {deviceDto.SerialNumber} is already assigned.");
        }

        deviceInDb.SerialNumber = deviceDto.SerialNumber;
        deviceInDb.Name = deviceDto.Name;
        deviceInDb.Manufacturer = deviceDto.Manufacturer;
        deviceInDb.Type = deviceDto.Type;
        deviceInDb.OperatingSystem = deviceDto.OperatingSystem;
        deviceInDb.OsVersion = deviceDto.OsVersion;
        deviceInDb.Processor = deviceDto.Processor;
        deviceInDb.RamAmount = deviceDto.RamAmount;
        deviceInDb.Description = deviceDto.Description;
        deviceInDb.Status = deviceDto.Status;

        await _context.SaveChangesAsync();
        return deviceInDb;
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return false;

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckSerialNumberExistsAsync(string serialNumber, int? excludeDeviceId = null)
    {
        if (string.IsNullOrWhiteSpace(serialNumber)) return false;
        var query = _context.Devices.Where(d => d.SerialNumber.ToUpper() == serialNumber.ToUpper());
        if (excludeDeviceId.HasValue) query = query.Where(d => d.Id != excludeDeviceId.Value);
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(string userId)
    {
        return await _context.Devices
            .Where(d => d.AssignedUserId == userId)
            .Include(d => d.AssignedUser)
            .ToListAsync();
    }

    public async Task AssignDeviceAsync(int deviceId, string userId)
    {
        var device = await _context.Devices.FindAsync(deviceId);
        if (device == null) throw new InvalidOperationException("Device not found.");
        
        if (device.Status != "Available" || device.AssignedUserId != null)
            throw new InvalidOperationException("Device is already in use.");

        device.AssignedUserId = userId;
        device.Status = "In Use"; 
        await _context.SaveChangesAsync();
    }

    public async Task UnassignDeviceAsync(int deviceId, string userId)
    {
        var device = await _context.Devices.FindAsync(deviceId);
        if (device == null) throw new InvalidOperationException("Device not found.");

        if (device.AssignedUserId != userId)
            throw new UnauthorizedAccessException("Not your device.");

        device.AssignedUserId = null;
        device.Status = "Available";
        await _context.SaveChangesAsync();
    }
}