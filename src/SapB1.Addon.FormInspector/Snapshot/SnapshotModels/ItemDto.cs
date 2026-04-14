namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing metadata for a single UI item on an SAP Business One form.
/// </summary>
public record ItemDto
{
    public string ItemUid { get; init; } = string.Empty;
    public string ItemType { get; init; } = string.Empty;
    public LayoutDto Layout { get; init; } = new();
    public bool Visible { get; init; } = true;
    public bool Enabled { get; init; } = true;
    public DataBindingDto? DataBinding { get; init; }
    public int FromPane { get; init; }
    public int ToPane { get; init; }
    public string? Description { get; init; }
    public MatrixDto? MatrixMetadata { get; init; }
}
