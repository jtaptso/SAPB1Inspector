namespace FormInspector.Presentation.Models;

/// <summary>
/// Response model returned after successfully receiving a snapshot.
/// </summary>
public record SnapshotResponseModel
{
    public string SnapshotId { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime ReceivedAt { get; init; }
}

/// <summary>
/// Error response model for API errors.
/// </summary>
public record ErrorResponseModel
{
    public string Error { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
