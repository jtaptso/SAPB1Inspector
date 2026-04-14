using System;
using SapB1.Addon.FormInspector.Startup;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class ConnectionBootstrapTests
{
    private readonly SapContext _sapContext;

    public ConnectionBootstrapTests()
    {
        // Fresh instance per test — no shared static state
        _sapContext = new SapContext();
    }

    [Fact]
    public void IsConnected_BeforeConnect_ReturnsFalse()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap(_sapContext);

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void IsConnected_AfterDisconnect_ReturnsFalse()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap(_sapContext);

        // Act
        bootstrap.Disconnect();

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void Disconnect_WhenNotConnected_DoesNotThrow()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap(_sapContext);

        // Act
        var act = () => bootstrap.Disconnect();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Connect_WithoutSdk_DoesNotThrow()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap(_sapContext);

        // Act
        var act = () => bootstrap.Connect();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void IsConnected_DelegatesToSapContext()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap(_sapContext);
        bootstrap.IsConnected.Should().BeFalse();

        // Act — manually initialize SapContext to simulate a successful connection
        _sapContext.Initialize(new object());

        // Assert — IsConnected reflects SapContext state
        bootstrap.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void Disconnect_ResetsSapContext()
    {
        // Arrange
        _sapContext.Initialize(new object());
        var bootstrap = new ConnectionBootstrap(_sapContext);
        bootstrap.IsConnected.Should().BeTrue();

        // Act
        bootstrap.Disconnect();

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
        _sapContext.IsInitialized.Should().BeFalse();
    }
}
