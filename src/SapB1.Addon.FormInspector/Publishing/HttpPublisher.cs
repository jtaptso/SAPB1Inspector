using System.Text;
using SapB1.Addon.FormInspector.Configuration;

namespace SapB1.Addon.FormInspector.Publishing;

/// <summary>
/// Low-level HTTP client for sending JSON payloads to the backend API.
/// Uses configuration for the backend URL and timeout settings.
/// </summary>
public class HttpPublisher
{
    private readonly HttpClient _httpClient;

    public HttpPublisher(InspectorSettings settings)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(settings.BackendUrl),
            Timeout = TimeSpan.FromSeconds(settings.PublishTimeoutSeconds)
        };

        _httpClient.DefaultRequestHeaders.Add("X-Snapshot-Source", "SapB1-FormInspector");
        _httpClient.DefaultRequestHeaders.Add("X-Schema-Version", "1.0");
    }

    /// <summary>
    /// Posts a JSON payload to the specified endpoint.
    /// </summary>
    public async Task PostAsync(string endpoint, string jsonPayload)
    {
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Gets a resource from the specified endpoint.
    /// </summary>
    public async Task<string> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
