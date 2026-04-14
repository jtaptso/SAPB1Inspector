using FormInspector.Domain.Enums;
using FormInspector.Domain.Snapshots;
using FormInspector.Domain.ValueObjects;

namespace FormInspector.Domain.Tests;

public class SnapshotTests
{
    [Fact]
    public void Snapshot_CreatedWithValidData_SetsProperties()
    {
        // Arrange
        var formType = new FormType("139");
        var form = new FormMetadata(formType, "1", "Sales Order", FormMode.Ok, 1);
        var context = new SnapshotContext("manager", "WORKSTATION01", "SAP01");
        var capturedAt = DateTime.UtcNow;

        // Act
        var snapshot = new Snapshot("snap-001", capturedAt, context, form);

        // Assert
        Assert.Equal("snap-001", snapshot.SnapshotId);
        Assert.Equal(capturedAt, snapshot.CapturedAt);
        Assert.Equal("139", snapshot.Form.FormType.Value);
        Assert.Equal("Sales Order", snapshot.Form.Title);
        Assert.Equal(FormMode.Ok, snapshot.Form.Mode);
        Assert.Equal("1.0", snapshot.SchemaVersion);
        Assert.Empty(snapshot.Items);
    }

    [Fact]
    public void Snapshot_NullSnapshotId_ThrowsArgumentException()
    {
        // Arrange
        var form = new FormMetadata(new FormType("139"), "1", "Test");
        var context = new SnapshotContext();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Snapshot(null!, DateTime.UtcNow, context, form));
    }

    [Fact]
    public void Snapshot_EmptySnapshotId_ThrowsArgumentException()
    {
        // Arrange
        var form = new FormMetadata(new FormType("139"), "1", "Test");
        var context = new SnapshotContext();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Snapshot("", DateTime.UtcNow, context, form));
    }

    [Fact]
    public void Snapshot_NullForm_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new SnapshotContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Snapshot("snap-001", DateTime.UtcNow, context, null!));
    }

    [Fact]
    public void Snapshot_NullContext_ThrowsArgumentNullException()
    {
        // Arrange
        var form = new FormMetadata(new FormType("139"), "1", "Test");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Snapshot("snap-001", DateTime.UtcNow, null!, form));
    }
}

public class FormTypeTests
{
    [Fact]
    public void FormType_ValidValue_SetsValue()
    {
        var ft = new FormType("139");
        Assert.Equal("139", ft.Value);
    }

    [Fact]
    public void FormType_NullValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new FormType(null!));
    }

    [Fact]
    public void FormType_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new FormType(""));
    }

    [Fact]
    public void FormType_ImplicitConversionToString_ReturnsValue()
    {
        var ft = new FormType("139");
        string result = ft;
        Assert.Equal("139", result);
    }
}

public class ItemTypeTests
{
    [Fact]
    public void ItemType_ValidValue_SetsValue()
    {
        var it = new ItemType("EditText");
        Assert.Equal("EditText", it.Value);
    }

    [Fact]
    public void ItemType_NullValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ItemType(null!));
    }
}

public class LayoutTests
{
    [Fact]
    public void Layout_ValidValues_SetsProperties()
    {
        var layout = new Layout(10, 20, 100, 30);
        Assert.Equal(10, layout.Top);
        Assert.Equal(20, layout.Left);
        Assert.Equal(100, layout.Width);
        Assert.Equal(30, layout.Height);
    }

    [Fact]
    public void Layout_NegativeTop_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Layout(-1, 0, 100, 30));
    }

    [Fact]
    public void Layout_NegativeWidth_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Layout(0, 0, -1, 30));
    }
}

public class DataBindingTests
{
    [Fact]
    public void DataBinding_ValidValues_SetsProperties()
    {
        var binding = new DataBinding("ORDR", "DocNum");
        Assert.Equal("ORDR", binding.TableName);
        Assert.Equal("DocNum", binding.ColumnName);
    }

    [Fact]
    public void DataBinding_NullTableName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DataBinding(null!, "DocNum"));
    }

    [Fact]
    public void DataBinding_NullColumnName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DataBinding("ORDR", null!));
    }

    [Fact]
    public void DataBinding_ToString_ReturnsTableDotColumn()
    {
        var binding = new DataBinding("ORDR", "DocNum");
        Assert.Equal("ORDR.DocNum", binding.ToString());
    }
}

public class ItemMetadataTests
{
    [Fact]
    public void ItemMetadata_ValidData_SetsProperties()
    {
        var layout = new Layout(10, 20, 100, 30);
        var itemType = new ItemType("EditText");
        var binding = new DataBinding("ORDR", "DocNum");

        var item = new ItemMetadata("4", itemType, layout, true, true, binding, 0, 0, "Card Code");

        Assert.Equal("4", item.ItemUid);
        Assert.Equal("EditText", item.ItemType.Value);
        Assert.True(item.Visible);
        Assert.True(item.Enabled);
        Assert.Equal("ORDR", item.DataBinding?.TableName);
        Assert.Equal("Card Code", item.Description);
    }

    [Fact]
    public void ItemMetadata_NullItemUid_ThrowsArgumentException()
    {
        var layout = new Layout(0, 0, 100, 30);
        var itemType = new ItemType("Button");

        Assert.Throws<ArgumentException>(() => new ItemMetadata(null!, itemType, layout));
    }

    [Fact]
    public void ItemMetadata_NullItemType_ThrowsArgumentNullException()
    {
        var layout = new Layout(0, 0, 100, 30);

        Assert.Throws<ArgumentNullException>(() => new ItemMetadata("1", null!, layout));
    }
}

public class MatrixMetadataTests
{
    [Fact]
    public void MatrixMetadata_ValidData_SetsProperties()
    {
        var columns = new List<ColumnMetadata>
        {
            new("col1", "EditText", editable: true, caption: "Item Code"),
            new("col2", "CheckBox", editable: false, caption: "Active")
        };

        var matrix = new MatrixMetadata("38", 5, true, columns);

        Assert.Equal("38", matrix.MatrixUid);
        Assert.Equal(5, matrix.RowCount);
        Assert.True(matrix.Editable);
        Assert.Equal(2, matrix.Columns.Count);
    }

    [Fact]
    public void MatrixMetadata_NullMatrixUid_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MatrixMetadata(null!, 0));
    }

    [Fact]
    public void MatrixMetadata_NegativeRowCount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MatrixMetadata("38", -1));
    }
}
