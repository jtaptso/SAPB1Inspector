namespace FormInspector.Application.DTOs;

/// <summary>
/// Input DTO for receiving a snapshot from the SAP add-on via HTTP POST.
/// Represents the wire format sent by the SwissAddonFramework publisher.
/// </summary>
public record SnapshotInputDto
{
    public string SnapshotId { get; init; } = string.Empty;
    public string SchemaVersion { get; init; } = "1.0";
    public DateTime CapturedAt { get; init; }
    public SnapshotContextDto Context { get; init; } = new();
    public FormMetadataDto Form { get; init; } = new();
    public List<ItemMetadataDto> Items { get; init; } = [];
}

public record SnapshotContextDto
{
    public string? UserName { get; init; }
    public string? MachineName { get; init; }
    public string? ClientId { get; init; }
}

public record FormMetadataDto
{
    public string FormType { get; init; } = string.Empty;
    public string UniqueId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Mode { get; init; } = "OK";
    public int PaneLevel { get; init; }
    public string? SapVersion { get; init; }
}

public record ItemMetadataDto
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
    public MatrixMetadataDto? MatrixMetadata { get; init; }
}

public record LayoutDto
{
    public int Top { get; init; }
    public int Left { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
}

public record DataBindingDto
{
    public string TableName { get; init; } = string.Empty;
    public string ColumnName { get; init; } = string.Empty;
}

public record MatrixMetadataDto
{
    public string MatrixUid { get; init; } = string.Empty;
    public int RowCount { get; init; }
    public bool Editable { get; init; } = true;
    public List<ColumnMetadataDto> Columns { get; init; } = [];
}

public record ColumnMetadataDto
{
    public string ColumnUid { get; init; } = string.Empty;
    public string ColumnType { get; init; } = string.Empty;
    public DataBindingDto? DataBinding { get; init; }
    public bool Editable { get; init; } = true;
    public string? Caption { get; init; }
    public int Width { get; init; }
}
