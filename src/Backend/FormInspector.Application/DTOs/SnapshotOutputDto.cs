namespace FormInspector.Application.DTOs;

/// <summary>
/// Output DTO for returning snapshot data to the Blazor viewer and API clients.
/// </summary>
public record SnapshotOutputDto
{
    public string SnapshotId { get; init; } = string.Empty;
    public string SchemaVersion { get; init; } = "1.0";
    public DateTime CapturedAt { get; init; }
    public SnapshotContextDto Context { get; init; } = new();
    public FormMetadataDto Form { get; init; } = new();
    public List<ItemMetadataDto> Items { get; init; } = [];
}

/// <summary>
/// Summary DTO for listing available snapshots without full item details.
/// </summary>
public record SnapshotSummaryDto
{
    public string SnapshotId { get; init; } = string.Empty;
    public string FormType { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public DateTime CapturedAt { get; init; }
    public string Mode { get; init; } = "OK";
    public int ItemCount { get; init; }
}
