using System;
using SapB1.Addon.FormInspector.Configuration;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class ThrottlerTests
{
    private DateTime _currentTime;
    private readonly InspectorSettings _settings;
    private readonly Throttler _throttler;

    public ThrottlerTests()
    {
        _currentTime = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        _settings = new InspectorSettings { ThrottleIntervalMs = 500 };
        _throttler = new Throttler(_settings, () => _currentTime);
    }

    [Fact]
    public void ShouldProcess_FirstCall_ReturnsTrue()
    {
        // Act
        var result = _throttler.ShouldProcess("139");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldProcess_SecondCallImmediately_ReturnsFalse()
    {
        // Arrange
        _throttler.ShouldProcess("139");

        // Act
        var result = _throttler.ShouldProcess("139");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldProcess_DifferentFormTypes_ReturnsTrue()
    {
        // Arrange
        _throttler.ShouldProcess("139");

        // Act
        var result = _throttler.ShouldProcess("142");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldProcess_AfterInterval_ReturnsTrue()
    {
        // Arrange
        _throttler.ShouldProcess("139");
        // Advance time past the throttle interval
        _currentTime = _currentTime.AddMilliseconds(501);

        // Act
        var result = _throttler.ShouldProcess("139");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldProcess_BeforeInterval_ReturnsFalse()
    {
        // Arrange
        _throttler.ShouldProcess("139");
        // Advance time but not enough
        _currentTime = _currentTime.AddMilliseconds(400);

        // Act
        var result = _throttler.ShouldProcess("139");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Reset_AllowsImmediateReProcessing()
    {
        // Arrange
        _throttler.ShouldProcess("139");
        // Without reset, second call would return false

        // Act
        _throttler.Reset("139");
        var result = _throttler.ShouldProcess("139");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ResetAll_AllowsImmediateReProcessing()
    {
        // Arrange
        _throttler.ShouldProcess("139");
        _throttler.ShouldProcess("142");

        // Act
        _throttler.ResetAll();
        var result139 = _throttler.ShouldProcess("139");
        var result142 = _throttler.ShouldProcess("142");

        // Assert
        result139.Should().BeTrue();
        result142.Should().BeTrue();
    }
}
