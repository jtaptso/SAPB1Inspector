using FormInspector.Domain.Enums;
using FormInspector.Domain.ValueObjects;

namespace FormInspector.Domain.Snapshots;

/// <summary>
/// Represents metadata for a single UI item on an SAP Business One form.
/// </summary>
public record ItemMetadata
{
    /// <summary>Unique identifier of the item within the form.</summary>
    public string ItemUid { get; init; }

    /// <summary>Type of the item (e.g., "EditText", "Button", "CheckBox", "Matrix").</summary>
    public ItemType ItemType { get; init; }

    /// <summary>Position and size of the item on the form.</summary>
    public Layout Layout { get; init; }

    /// <summary>Whether the item is visible.</summary>
    public bool Visible { get; init; }

    /// <summary>Whether the item is enabled (interactive).</summary>
    public bool Enabled { get; init; }

    /// <summary>Data binding to a database table/column, if any.</summary>
    public DataBinding? DataBinding { get; init; }

    /// <summary>FromPane - the pane level at which this item starts being visible.</summary>
    public int FromPane { get; init; }

    /// <summary>ToPane - the pane level at which this item stops being visible.</summary>
    public int ToPane { get; init; }

    /// <summary>Display caption or description of the item.</summary>
    public string? Description { get; init; }

    /// <summary>Matrix metadata if this item is a matrix/grid type.</summary>
    public MatrixMetadata? MatrixMetadata { get; init; }

    public ItemMetadata(
        string itemUid,
        ItemType itemType,
        Layout layout,
        bool visible = true,
        bool enabled = true,
        DataBinding? dataBinding = null,
        int fromPane = 0,
        int toPane = 0,
        string? description = null,
        MatrixMetadata? matrixMetadata = null)
    {
        if (string.IsNullOrWhiteSpace(itemUid))
            throw new ArgumentException("ItemUid cannot be null or empty.", nameof(itemUid));

        ItemUid = itemUid;
        ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
        Layout = layout ?? throw new ArgumentNullException(nameof(layout));
        Visible = visible;
        Enabled = enabled;
        DataBinding = dataBinding;
        FromPane = fromPane;
        ToPane = toPane;
        Description = description;
        MatrixMetadata = matrixMetadata;
    }
}
