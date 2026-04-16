using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeviceManager.Api.Models; // This makes DeviceDto work!
using DeviceManager.Api.Services;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]

public class AiController : ControllerBase
{
    private readonly AiServiceInterface _aiService;

    // The constructor injects the service so the controller doesn't have to do the work
    public AiController(AiServiceInterface aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Generates a professional description based on device specs.
    /// </summary>
    /// <param name="deviceDto">The device specs from the frontend form</param>
    /// <returns>A JSON object containing the AI-generated description</returns>
    [HttpPost("generate-description")]
    public async Task<IActionResult> GenerateDescription([FromBody] DeviceDto deviceDto)
    {
        try
        {
            // Hand the work off to the Service
            var description = await _aiService.GenerateDescriptionAsync(deviceDto);

            // Send the "suitcase" back to Angular
            return Ok(new { description });
        }
        catch (Exception ex)
        {
            // If the AI service is down or fails, we tell the frontend exactly why
            return StatusCode(500, new { message = "AI Generation failed", details = ex.Message });
        }
    }
}