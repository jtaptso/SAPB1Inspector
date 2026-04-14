using FormInspector.Application.DTOs;
using FormInspector.Domain.Enums;
using FormInspector.Domain.Snapshots;
using FormInspector.Domain.ValueObjects;

namespace FormInspector.Application.Mapping;

/// <summary>
/// Maps between Domain snapshot entities and Application DTOs.
/// Keeps Domain free of serialization concerns.
/// </summary>
public static class SnapshotMapper
{
    /// <summary>Maps an input DTO to a Domain Snapshot entity.</summary>
    public static Snapshot ToDomain(this SnapshotInputDto dto)
    {
        var formType = new FormType(dto.Form.FormType);
        var formMode = ParseFormMode(dto.Form.Mode);

        var form = new FormMetadata(
            formType,
            dto.Form.UniqueId,
            dto.Form.Title,
            formMode,
            dto.Form.PaneLevel,
            dto.Form.SapVersion);

        var context = new SnapshotContext(
            dto.Context?.UserName,
            dto.Context?.MachineName,
            dto.Context?.ClientId);

        var items = dto.Items?.Select(ToDomain).ToList() ?? [];

        return new Snapshot(
            dto.SnapshotId,
            dto.CapturedAt,
            context,
            form,
            items,
            dto.SchemaVersion ?? "1.0");
    }

    /// <summary>Maps a Domain Snapshot to an output DTO.</summary>
    public static SnapshotOutputDto ToOutputDto(this Snapshot snapshot)
    {
        return new SnapshotOutputDto
        {
            SnapshotId = snapshot.SnapshotId,
            SchemaVersion = snapshot.SchemaVersion,
            CapturedAt = snapshot.CapturedAt,
            Context = new SnapshotContextDto
            {
                UserName = snapshot.Context.UserName,
                MachineName = snapshot.Context.MachineName,
                ClientId = snapshot.Context.ClientId
            },
            Form = new FormMetadataDto
            {
                FormType = snapshot.Form.FormType.Value,
                UniqueId = snapshot.Form.UniqueId,
                Title = snapshot.Form.Title,
                Mode = snapshot.Form.Mode.ToString(),
                PaneLevel = snapshot.Form.PaneLevel,
                SapVersion = snapshot.Form.SapVersion
            },
            Items = snapshot.Items.Select(ToDto).ToList()
        };
    }

    /// <summary>Maps a Domain Snapshot to a summary DTO.</summary>
    public static SnapshotSummaryDto ToSummaryDto(this Snapshot snapshot)
    {
        return new SnapshotSummaryDto
        {
            SnapshotId = snapshot.SnapshotId,
            FormType = snapshot.Form.FormType.Value,
            Title = snapshot.Form.Title,
            CapturedAt = snapshot.CapturedAt,
            Mode = snapshot.Form.Mode.ToString(),
            ItemCount = snapshot.Items.Count
        };
    }

    private static ItemMetadata ToDomain(ItemMetadataDto dto)
    {
        var layout = new Layout(dto.Layout.Top, dto.Layout.Left, dto.Layout.Width, dto.Layout.Height);
        var itemType = new ItemType(dto.ItemType);
        var dataBinding = dto.DataBinding is not null
            ? new DataBinding(dto.DataBinding.TableName, dto.DataBinding.ColumnName)
            : null;
        var matrixMetadata = dto.MatrixMetadata is not null ? ToDomain(dto.MatrixMetadata) : null;

        return new ItemMetadata(
            dto.ItemUid,
            itemType,
            layout,
            dto.Visible,
            dto.Enabled,
            dataBinding,
            dto.FromPane,
            dto.ToPane,
            dto.Description,
            matrixMetadata);
    }

    private static MatrixMetadata ToDomain(MatrixMetadataDto dto)
    {
        var columns = dto.Columns?.Select(ToDomain).ToList() ?? [];
        return new MatrixMetadata(dto.MatrixUid, dto.RowCount, dto.Editable, columns);
    }

    private static ColumnMetadata ToDomain(ColumnMetadataDto dto)
    {
        var dataBinding = dto.DataBinding is not null
            ? new DataBinding(dto.DataBinding.TableName, dto.DataBinding.ColumnName)
            : null;

        return new ColumnMetadata(dto.ColumnUid, dto.ColumnType, dataBinding, dto.Editable, dto.Caption, dto.Width);
    }

    private static ItemMetadataDto ToDto(ItemMetadata item)
    {
        return new ItemMetadataDto
        {
            ItemUid = item.ItemUid,
            ItemType = item.ItemType.Value,
            Layout = new LayoutDto
            {
                Top = item.Layout.Top,
                Left = item.Layout.Left,
                Width = item.Layout.Width,
                Height = item.Layout.Height
            },
            Visible = item.Visible,
            Enabled = item.Enabled,
            DataBinding = item.DataBinding is not null
                ? new DataBindingDto { TableName = item.DataBinding.TableName, ColumnName = item.DataBinding.ColumnName }
                : null,
            FromPane = item.FromPane,
            ToPane = item.ToPane,
            Description = item.Description,
            MatrixMetadata = item.MatrixMetadata is not null ? ToDto(item.MatrixMetadata) : null
        };
    }

    private static MatrixMetadataDto ToDto(MatrixMetadata matrix)
    {
        return new MatrixMetadataDto
        {
            MatrixUid = matrix.MatrixUid,
            RowCount = matrix.RowCount,
            Editable = matrix.Editable,
            Columns = matrix.Columns.Select(ToDto).ToList()
        };
    }

    private static ColumnMetadataDto ToDto(ColumnMetadata column)
    {
        return new ColumnMetadataDto
        {
            ColumnUid = column.ColumnUid,
            ColumnType = column.ColumnType,
            DataBinding = column.DataBinding is not null
                ? new DataBindingDto { TableName = column.DataBinding.TableName, ColumnName = column.DataBinding.ColumnName }
                : null,
            Editable = column.Editable,
            Caption = column.Caption,
            Width = column.Width
        };
    }

    private static FormMode ParseFormMode(string mode)
    {
        return mode?.ToUpperInvariant() switch
        {
            "ADD" => Domain.Enums.FormMode.Add,
            "FIND" => Domain.Enums.FormMode.Find,
            "UPDATE" => Domain.Enums.FormMode.Update,
            "OK" => Domain.Enums.FormMode.Ok,
            _ => Domain.Enums.FormMode.Ok
        };
    }
}
