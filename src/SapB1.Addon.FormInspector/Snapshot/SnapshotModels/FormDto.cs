namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing form-level metadata captured from an SAP Business One form.
/// </summary>
public class FormDto
{
    public string FormType { get; set; } = string.Empty;
    public string UniqueId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Mode { get; set; } = "OK";
    public int PaneLevel { get; set; }
    public string? SapVersion { get; set; }
}
