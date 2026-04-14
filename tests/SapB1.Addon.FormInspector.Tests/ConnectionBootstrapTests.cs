using System;
using SapB1.Addon.FormInspector.Startup;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

[Collection("SapContext")]
public class ConnectionBootstrapTests
{
    public ConnectionBootstrapTests()
    {
        // Ensure clean state before each test
        SapContext.Reset();
    }

    [Fact]
    public void IsConnected_BeforeConnect_ReturnsFalse()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap();

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void IsConnected_AfterDisconnect_ReturnsFalse()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap();

        // Act
        bootstrap.Disconnect();

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void Disconnect_WhenNotConnected_DoesNotThrow()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap();

        // Act
        var act = () => bootstrap.Disconnect();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Connect_WithoutSdk_DoesNotThrow()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap();

        // Act
        var act = () => bootstrap.Connect();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void IsConnected_DelegatesToSapContext()
    {
        // Arrange
        var bootstrap = new ConnectionBootstrap();
        bootstrap.IsConnected.Should().BeFalse();

        // Act — manually initialize SapContext to simulate a successful connection
        SapContext.Initialize(new object());

        // Assert — IsConnected reflects SapContext state
        bootstrap.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void Disconnect_ResetsSapContext()
    {
        // Arrange
        SapContext.Initialize(new object());
        var bootstrap = new ConnectionBootstrap();
        bootstrap.IsConnected.Should().BeTrue();

        // Act
        bootstrap.Disconnect();

        // Assert
        bootstrap.IsConnected.Should().BeFalse();
        SapContext.IsInitialized.Should().BeFalse();
    }
}
