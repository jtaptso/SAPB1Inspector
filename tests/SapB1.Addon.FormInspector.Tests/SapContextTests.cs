using System;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class SapContextTests
{
    private readonly SapContext _sapContext;

    public SapContextTests()
    {
        // Fresh instance per test — no shared static state
        _sapContext = new SapContext();
    }

    [Fact]
    public void IsInitialized_BeforeInitialize_ReturnsFalse()
    {
        // Assert
        _sapContext.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public void IsInitialized_AfterInitialize_ReturnsTrue()
    {
        // Arrange & Act
        _sapContext.Initialize(new object());

        // Assert
        _sapContext.IsInitialized.Should().BeTrue();
    }

    [Fact]
    public void IsInitialized_AfterReset_ReturnsFalse()
    {
        // Arrange
        _sapContext.Initialize(new object());

        // Act
        _sapContext.Reset();

        // Assert
        _sapContext.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public void Initialize_NullArgument_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => _sapContext.Initialize(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("application");
    }

    [Fact]
    public void Application_BeforeInitialize_IsNull()
    {
        // Assert
        _sapContext.Application.Should().BeNull();
    }

    [Fact]
    public void Application_AfterInitialize_ReturnsSetObject()
    {
        // Arrange
        var app = new object();

        // Act
        _sapContext.Initialize(app);

        // Assert
        _sapContext.Application.Should().BeSameAs(app);
    }

    [Fact]
    public void Application_AfterReset_IsNull()
    {
        // Arrange
        _sapContext.Initialize(new object());

        // Act
        _sapContext.Reset();

        // Assert
        _sapContext.Application.Should().BeNull();
    }

    [Fact]
    public void Initialize_CalledTwice_OverwritesPrevious()
    {
        // Arrange
        var app1 = new object();
        var app2 = new object();

        // Act
        _sapContext.Initialize(app1);
        _sapContext.Initialize(app2);

        // Assert
        _sapContext.Application.Should().BeSameAs(app2);
    }

    [Fact]
    public void Instances_AreIsolated()
    {
        // Arrange — two independent instances
        var ctx1 = new SapContext();
        var ctx2 = new SapContext();

        // Act — initialize only ctx1
        ctx1.Initialize(new object());

        // Assert — ctx2 is unaffected
        ctx1.IsInitialized.Should().BeTrue();
        ctx2.IsInitialized.Should().BeFalse();
    }
}
