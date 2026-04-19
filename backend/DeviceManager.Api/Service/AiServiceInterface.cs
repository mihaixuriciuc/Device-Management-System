using DeviceManager.Api.DTOs; // Ensure your DTO namespace is correct
using DeviceManager.Api.Models;

namespace DeviceManager.Api.Services;
public interface AiServiceInterface
{
    Task<string> GenerateDescriptionAsync(DeviceDto specs);
}