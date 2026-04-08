using Microsoft.EntityFrameworkCore;
using DeviceManager.Api.Data;
using DeviceManager.Api.Models;

namespace DeviceManager.Api.Services;

public class DeviceService : DeviceServiceInterface
{
    private readonly AppDbContext _context;

    public DeviceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices.ToListAsync();
    }

    public async Task<Device?> GetDeviceByIdAsync(int id)
    {
        return await _context.Devices.FindAsync(id);
    }

    public async Task<Device> CreateDeviceAsync(DeviceDto deviceDto)
    {
        // Check for duplicates using our reusable method
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
            Status = deviceDto.Status,
            DateAdded = DateTime.UtcNow
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        return device;
    }

    public async Task<Device?> UpdateDeviceAsync(int id, DeviceDto deviceDto)
    {
        var deviceInDb = await _context.Devices.FindAsync(id);
        if (deviceInDb == null) return null;

        // Check if another device is using this serial number
        if (await CheckSerialNumberExistsAsync(deviceDto.SerialNumber, id))
        {
            throw new InvalidOperationException($"Serial Number {deviceDto.SerialNumber} is already assigned to a different device.");
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

    // This handles both the Frontend validation AND our internal DB checks
    public async Task<bool> CheckSerialNumberExistsAsync(string serialNumber, int? excludeDeviceId = null)
    {
        if (string.IsNullOrWhiteSpace(serialNumber)) return false;

        var query = _context.Devices.Where(d => d.SerialNumber.ToUpper() == serialNumber.ToUpper());

        // If we are updating, we exclude the current device's ID from the check
        if (excludeDeviceId.HasValue)
        {
            query = query.Where(d => d.Id != excludeDeviceId.Value);
        }

        return await query.AnyAsync();
    }
}