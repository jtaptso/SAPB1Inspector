namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing metadata for a single UI item on an SAP Business One form.
/// </summary>
public class ItemDto
{
    public string ItemUid { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public LayoutDto Layout { get; set; } = new LayoutDto();
    public bool Visible { get; set; } = true;
    public bool Enabled { get; set; } = true;
    public DataBindingDto? DataBinding { get; set; }
    public int FromPane { get; set; }
    public int ToPane { get; set; }
    public string? Description { get; set; }
    public MatrixDto? MatrixMetadata { get; set; }
}
