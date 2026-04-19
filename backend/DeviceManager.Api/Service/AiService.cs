using System.Text;
using System.Text.Json;
using DeviceManager.Api.DTOs;

namespace DeviceManager.Api.Services;

public class AiService : AiServiceInterface
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

public async Task<string> GenerateDescriptionAsync(DeviceDto specs)
{
    var apiKey = _config["Gemini:ApiKey"];
    if (string.IsNullOrEmpty(apiKey))
        throw new Exception("Gemini API key is missing in appsettings.json");

    // Best stable model right now
    var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent?key={apiKey}";

    var promptText = $"Write a concise, professional 2-sentence description for a device. " +
                     $"Specs: {specs.Manufacturer} {specs.Name}, {specs.Processor}, {specs.RamAmount} RAM, " +
                     $"running {specs.OperatingSystem}. Focus on its best use case.";

    var requestBody = new
    {
        contents = new[]
        {
            new { parts = new[] { new { text = promptText } } }
        }
    };

    var jsonRequest = JsonSerializer.Serialize(requestBody);
    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.PostAsync(url, content);

    var jsonResponse = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"Google Error ({response.StatusCode}): {jsonResponse}");
    }

    // Safer parsing with null checks and safety filter handling
    using var doc = JsonDocument.Parse(jsonResponse);
    var root = doc.RootElement;

    if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
    {
        var candidate = candidates[0];

        // Handle blocked responses (very common)
        if (candidate.TryGetProperty("finishReason", out var finishReason))
        {
            var reason = finishReason.GetString();
            if (reason == "SAFETY" || reason == "RECITATION" || reason == "BLOCKLIST")
            {
                return "AI generation was blocked by safety filters. Try changing the device specs slightly.";
            }
        }

        // Normal successful response
        if (candidate.TryGetProperty("content", out var contentProp) &&
            contentProp.TryGetProperty("parts", out var parts) &&
            parts.GetArrayLength() > 0)
        {
            var text = parts[0].GetProperty("text").GetString();
            return text?.Trim() ?? "Could not generate description.";
        }
    }

    return "Could not generate description. Please try again with different specs.";
}
}