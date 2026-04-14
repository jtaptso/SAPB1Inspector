using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class SnapshotBuilderTests
{
    private readonly SnapshotBuilder _builder;
    private readonly ItemInspector _itemInspector;
    private readonly DataSourceInspector _dataSourceInspector;
    private readonly SapHelpers _sapHelpers;

    public SnapshotBuilderTests()
    {
        // Ensure SapContext is not initialized (non-SDK code path)
        SapContext.Reset();

        _itemInspector = new ItemInspector(new MatrixInspector());
        _dataSourceInspector = new DataSourceInspector();
        _sapHelpers = new SapHelpers();
        _builder = new SnapshotBuilder(_itemInspector, _dataSourceInspector, _sapHelpers);
    }

    [Fact]
    public void Build_WithFormDto_ReturnsSnapshotDto()
    {
        // Arrange
        var formDto = new FormDto
        {
            FormType = "139",
            UniqueId = "form-1",
            Title = "Sales Order",
            Mode = "OK",
            PaneLevel = 1
        };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Build_WithFormDto_SetsSchemaVersion()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.SchemaVersion.Should().Be("1.0");
    }

    [Fact]
    public void Build_WithFormDto_SetsSnapshotId()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.SnapshotId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Build_WithFormDto_SetsCapturedAtToRecentTime()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };
        var before = DateTime.UtcNow.AddSeconds(-5);

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.CapturedAt.Should().BeOnOrAfter(before);
    }

    [Fact]
    public void Build_WithFormDto_CopiesFormData()
    {
        // Arrange
        var formDto = new FormDto
        {
            FormType = "139",
            UniqueId = "form-1",
            Title = "Sales Order",
            Mode = "OK",
            PaneLevel = 1
        };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.Form.FormType.Should().Be("139");
        result.Form.Title.Should().Be("Sales Order");
        result.Form.Mode.Should().Be("OK");
        result.Form.PaneLevel.Should().Be(1);
    }

    [Fact]
    public void Build_WithFormDto_SetsUserNameFromSapHelpers()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.Build(formDto);

        // Assert — without SDK, SapHelpers returns Environment.UserName
        result.UserName.Should().Be(Environment.UserName);
    }

    [Fact]
    public void Build_WithFormDto_SetsMachineName()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.MachineName.Should().Be(Environment.MachineName);
    }

    [Fact]
    public void Build_WithFormDto_ItemsIsEmptyWithoutSdk()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.Build(formDto);

        // Assert
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public void BuildWithContext_WithUserNameOverride_OverridesUserName()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.BuildWithContext(formDto, userName: "custom-user", machineName: null);

        // Assert
        result.UserName.Should().Be("custom-user");
    }

    [Fact]
    public void BuildWithContext_WithMachineNameOverride_OverridesMachineName()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };

        // Act
        var result = _builder.BuildWithContext(formDto, userName: null, machineName: "CUSTOM-MACHINE");

        // Assert
        result.MachineName.Should().Be("CUSTOM-MACHINE");
    }

    [Fact]
    public void BuildWithContext_WithNullOverrides_KeepsDefaults()
    {
        // Arrange
        var formDto = new FormDto { UniqueId = "form-1" };
        var baseResult = _builder.Build(formDto);

        // Act
        var result = _builder.BuildWithContext(formDto, userName: null, machineName: null);

        // Assert
        result.UserName.Should().Be(baseResult.UserName);
        result.MachineName.Should().Be(baseResult.MachineName);
    }
}
