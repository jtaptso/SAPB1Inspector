namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// Top-level snapshot DTO sent from the SAP add-on to the backend via HTTP POST.
/// Pure metadata — no SAP objects, no COM references.
/// </summary>
public record SnapshotDto
{
    public string SnapshotId { get; init; } = Guid.NewGuid().ToString();
    public string SchemaVersion { get; init; } = "1.0";
    public DateTime CapturedAt { get; init; } = DateTime.UtcNow;
    public string? UserName { get; init; }
    public string? MachineName { get; init; }
    public string? ClientId { get; init; }
    public FormDto Form { get; init; } = new();
    public List<ItemDto> Items { get; init; } = [];
}
