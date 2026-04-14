using System;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class SapContextTests
{
    public SapContextTests()
    {
        // Ensure clean state before each test
        SapContext.Reset();
    }

    [Fact]
    public void IsInitialized_BeforeInitialize_ReturnsFalse()
    {
        // Assert
        SapContext.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public void IsInitialized_AfterInitialize_ReturnsTrue()
    {
        // Arrange & Act
        SapContext.Initialize(new object());

        // Assert
        SapContext.IsInitialized.Should().BeTrue();
    }

    [Fact]
    public void IsInitialized_AfterReset_ReturnsFalse()
    {
        // Arrange
        SapContext.Initialize(new object());

        // Act
        SapContext.Reset();

        // Assert
        SapContext.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public void Initialize_NullArgument_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => SapContext.Initialize(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("application");
    }

    [Fact]
    public void Application_BeforeInitialize_IsNull()
    {
        // Assert
        SapContext.Application.Should().BeNull();
    }

    [Fact]
    public void Application_AfterInitialize_ReturnsSetObject()
    {
        // Arrange
        var app = new object();

        // Act
        SapContext.Initialize(app);

        // Assert
        SapContext.Application.Should().BeSameAs(app);
    }

    [Fact]
    public void Application_AfterReset_IsNull()
    {
        // Arrange
        SapContext.Initialize(new object());

        // Act
        SapContext.Reset();

        // Assert
        SapContext.Application.Should().BeNull();
    }

    [Fact]
    public void Initialize_CalledTwice_OverwritesPrevious()
    {
        // Arrange
        var app1 = new object();
        var app2 = new object();

        // Act
        SapContext.Initialize(app1);
        SapContext.Initialize(app2);

        // Assert
        SapContext.Application.Should().BeSameAs(app2);
    }
}
