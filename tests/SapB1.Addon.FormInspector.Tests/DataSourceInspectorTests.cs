using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class DataSourceInspectorTests
{
    private readonly DataSourceInspector _inspector;

    public DataSourceInspectorTests()
    {
        // Ensure SapContext is not initialized (non-SDK code path)
        SapContext.Reset();
        _inspector = new DataSourceInspector();
    }

    [Fact]
    public void InspectDataSources_WithoutSdk_ReturnsEmptyList()
    {
        // Act
        var result = _inspector.InspectDataSources("form-1");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetDataBinding_WithoutSdk_ReturnsNull()
    {
        // Act
        var result = _inspector.GetDataBinding("form-1", "item-4");

        // Assert
        result.Should().BeNull();
    }
}
