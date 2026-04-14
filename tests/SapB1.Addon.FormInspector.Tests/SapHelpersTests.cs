using System;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class SapHelpersTests
{
    private readonly SapHelpers _helpers;

    public SapHelpersTests()
    {
        // Ensure SapContext is not initialized (non-SDK code path)
        SapContext.Reset();
        _helpers = new SapHelpers();
    }

    [Fact]
    public void GetCurrentUserName_WithoutSdk_ReturnsEnvironmentUserName()
    {
        // Act
        var result = _helpers.GetCurrentUserName();

        // Assert
        result.Should().Be(Environment.UserName);
    }

    [Fact]
    public void GetCurrentUserName_WithoutSdk_ReturnsNonNullValue()
    {
        // Act
        var result = _helpers.GetCurrentUserName();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetClientId_WithoutSdk_ReturnsNull()
    {
        // Act
        var result = _helpers.GetClientId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCompanyDb_WithoutSdk_ReturnsNull()
    {
        // Act
        var result = _helpers.GetCompanyDb();

        // Assert
        result.Should().BeNull();
    }
}
