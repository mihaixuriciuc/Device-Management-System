using DeviceManager.Api.Models;

namespace DeviceManager.Api.Services;

public interface DeviceServiceInterface
{
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceByIdAsync(int id);
    Task<Device> CreateDeviceAsync(DeviceDto deviceDto);
    Task<Device?> UpdateDeviceAsync(int id, DeviceDto deviceDto);
    Task<bool> DeleteDeviceAsync(int id);
    Task<bool> CheckSerialNumberExistsAsync(string serialNumber, int? excludeDeviceId = null);
}