using System.Net.Http.Json;
using FormInspector.Application.DTOs;

namespace FormInspector.BlazorServer.Services;

/// <summary>
/// HTTP client service for calling the backend REST API.
/// Provides typed methods for snapshot operations.
/// </summary>
public class SnapshotApiClient
{
    private readonly HttpClient _httpClient;

    public SnapshotApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>Gets all snapshots as summaries.</summary>
    public async Task<IReadOnlyList<SnapshotSummaryDto>?> GetAllSnapshotsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<SnapshotSummaryDto>>("api/snapshot");
    }

    /// <summary>Gets the latest snapshot for a given form type.</summary>
    public async Task<SnapshotOutputDto?> GetLatestSnapshotAsync(string formType)
    {
        return await _httpClient.GetFromJsonAsync<SnapshotOutputDto>($"api/snapshot/latest/{formType}");
    }

    /// <summary>Gets a specific snapshot by ID.</summary>
    public async Task<SnapshotOutputDto?> GetSnapshotByIdAsync(string snapshotId)
    {
        return await _httpClient.GetFromJsonAsync<SnapshotOutputDto>($"api/snapshot/{snapshotId}");
    }
}
