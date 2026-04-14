using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Snapshot;

/// <summary>
/// Builds a snapshot DTO from inspection data gathered from SAP UI API.
/// The builder assembles form metadata, item metadata, and matrix metadata
/// into a pure, serializable snapshot — no SAP objects or COM references.
/// </summary>
public class SnapshotBuilder
{
    private readonly ItemInspector _itemInspector;
    private readonly DataSourceInspector _dataSourceInspector;
    private readonly Utilities.SapHelpers _sapHelpers;

    public SnapshotBuilder(
        ItemInspector itemInspector,
        DataSourceInspector dataSourceInspector,
        Utilities.SapHelpers sapHelpers)
    {
        _itemInspector = itemInspector;
        _dataSourceInspector = dataSourceInspector;
        _sapHelpers = sapHelpers;
    }

    /// <summary>
    /// Builds a complete snapshot from a form inspection result.
    /// </summary>
    public SnapshotDto Build(FormDto formData)
    {
        var items = _itemInspector.InspectAllItems(int.Parse(formData.UniqueId));

        // Enrich items with matrix metadata where applicable
        foreach (var item in items.Where(i => i.ItemType == "Matrix" || i.ItemType == "Grid"))
        {
            // Matrix metadata is attached during item inspection
        }

        return new SnapshotDto
        {
            SnapshotId = Guid.NewGuid().ToString(),
            SchemaVersion = "1.0",
            CapturedAt = DateTime.UtcNow,
            UserName = _sapHelpers.GetCurrentUserName(),
            MachineName = Environment.MachineName,
            ClientId = _sapHelpers.GetClientId(),
            Form = formData,
            Items = items
        };
    }

    /// <summary>
    /// Builds a snapshot with additional context information.
    /// </summary>
    public SnapshotDto BuildWithContext(FormDto formData, string? userName, string? machineName)
    {
        var snapshot = Build(formData);

        return snapshot with
        {
            UserName = userName ?? snapshot.UserName,
            MachineName = machineName ?? snapshot.MachineName
        };
    }
}
