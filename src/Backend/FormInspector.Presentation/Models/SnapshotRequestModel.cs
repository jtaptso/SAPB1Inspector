namespace FormInspector.Presentation.Models;

/// <summary>
/// Request model for snapshot submission from the SAP add-on.
/// Used as an alternative to the Application DTO for presentation-specific formatting.
/// </summary>
public record SnapshotRequestModel
{
    public string SnapshotId { get; init; } = string.Empty;
    public string SchemaVersion { get; init; } = "1.0";
    public DateTime CapturedAt { get; init; }
    public string? UserName { get; init; }
    public string? MachineName { get; init; }
    public string FormType { get; init; } = string.Empty;
    public string UniqueId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Mode { get; init; } = "OK";
    public int PaneLevel { get; init; }
    public string? SapVersion { get; init; }
}
