using System.Collections.Generic;

namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// DTO representing metadata for a matrix or grid on an SAP Business One form.
/// </summary>
public class MatrixDto
{
    public string MatrixUid { get; set; } = string.Empty;
    public int RowCount { get; set; }
    public bool Editable { get; set; } = true;
    public List<ColumnDto> Columns { get; set; } = new List<ColumnDto>();
}

/// <summary>
/// DTO representing a column within a matrix or grid.
/// </summary>
public class ColumnDto
{
    public string ColumnUid { get; set; } = string.Empty;
    public string ColumnType { get; set; } = string.Empty;
    public DataBindingDto? DataBinding { get; set; }
    public bool Editable { get; set; } = true;
    public string? Caption { get; set; }
    public int Width { get; set; }
}

/// <summary>
/// DTO representing the layout (position and size) of a UI item.
/// </summary>
public class LayoutDto
{
    public int Top { get; set; }
    public int Left { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

/// <summary>
/// DTO representing a data binding to a database table and column.
/// </summary>
public class DataBindingDto
{
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
}
