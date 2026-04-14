using FormInspector.Domain.ValueObjects;

namespace FormInspector.Domain.Snapshots;

/// <summary>
/// Represents metadata for a column within a matrix or grid on an SAP Business One form.
/// </summary>
public record ColumnMetadata
{
    /// <summary>Unique identifier of the column within the matrix.</summary>
    public string ColumnUid { get; init; }

    /// <summary>Type of the column (e.g., "EditText", "CheckBox", "ComboBox").</summary>
    public string ColumnType { get; init; }

    /// <summary>Data binding for the column, if any.</summary>
    public DataBinding? DataBinding { get; init; }

    /// <summary>Whether the column is editable.</summary>
    public bool Editable { get; init; }

    /// <summary>Display caption of the column.</summary>
    public string? Caption { get; init; }

    /// <summary>Visual width of the column.</summary>
    public int Width { get; init; }

    public ColumnMetadata(
        string columnUid,
        string columnType,
        DataBinding? dataBinding = null,
        bool editable = true,
        string? caption = null,
        int width = 0)
    {
        if (string.IsNullOrWhiteSpace(columnUid))
            throw new ArgumentException("ColumnUid cannot be null or empty.", nameof(columnUid));
        if (string.IsNullOrWhiteSpace(columnType))
            throw new ArgumentException("ColumnType cannot be null or empty.", nameof(columnType));

        ColumnUid = columnUid;
        ColumnType = columnType;
        DataBinding = dataBinding;
        Editable = editable;
        Caption = caption;
        Width = width;
    }
}
