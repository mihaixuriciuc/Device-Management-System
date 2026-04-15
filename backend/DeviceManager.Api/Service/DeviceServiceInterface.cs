using DeviceManager.Api.Models;

namespace DeviceManager.Api.Services;

public interface DeviceServiceInterface
{
    // ==========================================
    // ZONE 1: NORMAL USER METHODS
    // ==========================================
    Task<IEnumerable<Device>> GetDevicesByUserIdAsync(string userId);
    Task AssignDeviceAsync(int deviceId, string userId);
    Task UnassignDeviceAsync(int deviceId, string userId);

    // ==========================================
    //  ZONE 2: ADMIN ONLY METHODS
    // ==========================================
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceByIdAsync(int id);
    Task<Device> CreateDeviceAsync(DeviceDto deviceDto);
    Task<Device?> UpdateDeviceAsync(int id, DeviceDto deviceDto);
    Task<bool> DeleteDeviceAsync(int id);
    
    // ==========================================
    // UTILITY METHODS
    // ==========================================
    Task<bool> CheckSerialNumberExistsAsync(string serialNumber, int? excludeDeviceId = null);
}