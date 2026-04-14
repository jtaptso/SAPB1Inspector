using System;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class FormInspectorServiceTests
{
    private readonly FormInspectorService _service;
    private readonly SapContext _sapContext;

    public FormInspectorServiceTests()
    {
        // Fresh instance — no shared static state
        _sapContext = new SapContext();
        _service = new FormInspectorService(_sapContext);
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsFormDtoWithFormUid()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.Should().NotBeNull();
        result.UniqueId.Should().Be("test-form-uid");
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsEmptyFormType()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.FormType.Should().BeEmpty();
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsEmptyTitle()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.Title.Should().BeEmpty();
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsModeOk()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.Mode.Should().Be("OK");
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsZeroPaneLevel()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.PaneLevel.Should().Be(0);
    }

    [Fact]
    public void InspectForm_WithoutSdk_ReturnsNullSapVersion()
    {
        // Act
        var result = _service.InspectForm("test-form-uid");

        // Assert
        result.SapVersion.Should().BeNull();
    }

    [Fact]
    public void GetSapVersion_WithoutSdk_ReturnsNull()
    {
        // Act
        var result = _service.GetSapVersion();

        // Assert
        result.Should().BeNull();
    }
}
