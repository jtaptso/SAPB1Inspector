using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Publishing;

/// <summary>
/// High-level publisher that serializes a snapshot and sends it to the backend.
/// Coordinates between the SnapshotSerializer and HttpPublisher.
/// </summary>
public class SnapshotPublisher
{
    private readonly HttpPublisher _httpPublisher;

    public SnapshotPublisher(HttpPublisher httpPublisher)
    {
        _httpPublisher = httpPublisher;
    }

    /// <summary>
    /// Serializes and publishes a snapshot to the backend.
    /// </summary>
    public async Task PublishAsync(SnapshotDto snapshot)
    {
        var json = SnapshotSerializer.Serialize(snapshot);
        await _httpPublisher.PostAsync("/api/snapshot", json);
    }

    /// <summary>
    /// Publishes a snapshot with retry logic.
    /// </summary>
    public async Task PublishWithRetryAsync(SnapshotDto snapshot, int maxRetries = 3)
    {
        var json = SnapshotSerializer.Serialize(snapshot);

        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                await _httpPublisher.PostAsync("/api/snapshot", json);
                return;
            }
            catch (HttpRequestException)
            {
                if (attempt == maxRetries)
                    throw;

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }
    }
}
