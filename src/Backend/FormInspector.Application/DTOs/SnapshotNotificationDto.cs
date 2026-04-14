namespace FormInspector.Application.DTOs;

/// <summary>
/// Notification DTO for snapshot update events sent via SignalR.
/// Defined in Application layer so viewers can reference it without depending on Infrastructure.
/// </summary>
public record SnapshotUpdatedNotification
{
    /// <summary>The ID of the updated snapshot.</summary>
    public string SnapshotId { get; init; } = string.Empty;

    /// <summary>The form type of the updated snapshot.</summary>
    public string FormType { get; init; } = string.Empty;

    /// <summary>When the update occurred.</summary>
    public DateTime UpdatedAt { get; init; }
}
