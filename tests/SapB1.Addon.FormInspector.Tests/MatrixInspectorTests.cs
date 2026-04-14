using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class MatrixInspectorTests
{
    private readonly MatrixInspector _inspector;

    public MatrixInspectorTests()
    {
        // Ensure SapContext is not initialized (non-SDK code path)
        SapContext.Reset();
        _inspector = new MatrixInspector();
    }

    [Fact]
    public void InspectMatrix_WithoutSdk_ReturnsMatrixDtoWithMatrixUid()
    {
        // Act
        var result = _inspector.InspectMatrix("form-1", "matrix-38");

        // Assert
        result.Should().NotBeNull();
        result.MatrixUid.Should().Be("matrix-38");
    }

    [Fact]
    public void InspectMatrix_WithoutSdk_ReturnsZeroRowCount()
    {
        // Act
        var result = _inspector.InspectMatrix("form-1", "matrix-38");

        // Assert
        result.RowCount.Should().Be(0);
    }

    [Fact]
    public void InspectMatrix_WithoutSdk_ReturnsEditableTrue()
    {
        // Act
        var result = _inspector.InspectMatrix("form-1", "matrix-38");

        // Assert
        result.Editable.Should().BeTrue();
    }

    [Fact]
    public void InspectMatrix_WithoutSdk_ReturnsEmptyColumnsList()
    {
        // Act
        var result = _inspector.InspectMatrix("form-1", "matrix-38");

        // Assert
        result.Columns.Should().NotBeNull();
        result.Columns.Should().BeEmpty();
    }

    [Fact]
    public void InspectAllMatrices_WithoutSdk_ReturnsEmptyList()
    {
        // Act
        var result = _inspector.InspectAllMatrices("form-1");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
