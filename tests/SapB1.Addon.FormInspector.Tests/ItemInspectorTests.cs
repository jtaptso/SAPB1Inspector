using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class ItemInspectorTests
{
    private readonly ItemInspector _inspector;
    private readonly Mock<MatrixInspector> _matrixInspectorMock;
    private readonly SapContext _sapContext;

    public ItemInspectorTests()
    {
        // Fresh instance — no shared static state
        _sapContext = new SapContext();

        _matrixInspectorMock = new Mock<MatrixInspector>(_sapContext);
        _inspector = new ItemInspector(_matrixInspectorMock.Object, _sapContext);
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsItemDtoWithItemUid()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.Should().NotBeNull();
        result.ItemUid.Should().Be("item-4");
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsEmptyItemType()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.ItemType.Should().BeEmpty();
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsDefaultLayout()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.Layout.Should().NotBeNull();
        result.Layout.Top.Should().Be(0);
        result.Layout.Left.Should().Be(0);
        result.Layout.Width.Should().Be(0);
        result.Layout.Height.Should().Be(0);
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsVisibleTrue()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.Visible.Should().BeTrue();
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsEnabledTrue()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.Enabled.Should().BeTrue();
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsNullDataBinding()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.DataBinding.Should().BeNull();
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsZeroFromPane()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.FromPane.Should().Be(0);
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsZeroToPane()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.ToPane.Should().Be(0);
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsNullDescription()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.Description.Should().BeNull();
    }

    [Fact]
    public void InspectItem_WithoutSdk_ReturnsNullMatrixMetadata()
    {
        // Act
        var result = _inspector.InspectItem("form-1", "item-4");

        // Assert
        result.MatrixMetadata.Should().BeNull();
    }

    [Fact]
    public void InspectAllItems_WithoutSdk_ReturnsEmptyList()
    {
        // Act
        var result = _inspector.InspectAllItems("form-1");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
