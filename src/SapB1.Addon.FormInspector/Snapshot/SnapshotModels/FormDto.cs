namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing form-level metadata captured from an SAP Business One form.
/// </summary>
public record FormDto
{
    public string FormType { get; init; } = string.Empty;
    public string UniqueId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Mode { get; init; } = "OK";
    public int PaneLevel { get; init; }
    public string? SapVersion { get; init; }
}
