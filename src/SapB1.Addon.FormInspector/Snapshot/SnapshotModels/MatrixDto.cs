namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing metadata for a matrix or grid on an SAP Business One form.
/// </summary>
public record MatrixDto
{
    public string MatrixUid { get; init; } = string.Empty;
    public int RowCount { get; init; }
    public bool Editable { get; init; } = true;
    public List<ColumnDto> Columns { get; init; } = [];
}

/// <summary>
/// DTO representing a column within a matrix or grid.
/// </summary>
public record ColumnDto
{
    public string ColumnUid { get; init; } = string.Empty;
    public string ColumnType { get; init; } = string.Empty;
    public DataBindingDto? DataBinding { get; init; }
    public bool Editable { get; init; } = true;
    public string? Caption { get; init; }
    public int Width { get; init; }
}

/// <summary>
/// DTO representing the layout (position and size) of a UI item.
/// </summary>
public record LayoutDto
{
    public int Top { get; init; }
    public int Left { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
}

/// <summary>
/// DTO representing a data binding to a database table and column.
/// </summary>
public record DataBindingDto
{
    public string TableName { get; init; } = string.Empty;
    public string ColumnName { get; init; } = string.Empty;
}
