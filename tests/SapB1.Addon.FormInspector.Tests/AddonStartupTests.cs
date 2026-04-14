using System;
using Moq;
using SapB1.Addon.FormInspector.Configuration;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Publishing;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Startup;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Tests;

public class AddonStartupTests
{
    private readonly Mock<FormEventDispatcher> _eventDispatcherMock;
    private readonly Mock<ConnectionBootstrap> _connectionBootstrapMock;
    private readonly InspectorSettings _settings;
    private readonly AddonStartup _startup;
    private readonly SapContext _sapContext;

    public AddonStartupTests()
    {
        // Fresh instance — no shared static state
        _sapContext = new SapContext();

        _eventDispatcherMock = new Mock<FormEventDispatcher>(
            new FormInspectorService(_sapContext),
            new SnapshotBuilder(
                new ItemInspector(new MatrixInspector(_sapContext), _sapContext),
                new DataSourceInspector(_sapContext),
                new SapHelpers(_sapContext)),
            new SnapshotPublisher(new HttpPublisher(new InspectorSettings())),
            new InspectorSettings(),
            new Throttler(new InspectorSettings()));

        _connectionBootstrapMock = new Mock<ConnectionBootstrap>(_sapContext);
        _settings = new InspectorSettings();

        _startup = new AddonStartup(
            _eventDispatcherMock.Object,
            _settings,
            _connectionBootstrapMock.Object,
            _sapContext);
    }

    [Fact]
    public void Start_CallsConnectionBootstrapConnect()
    {
        // Act
        _startup.Start();

        // Assert
        _connectionBootstrapMock.Verify(b => b.Connect(), Times.Once);
    }

    [Fact]
    public void Start_CallsEventDispatcherRegisterHandlers()
    {
        // Act
        _startup.Start();

        // Assert
        _eventDispatcherMock.Verify(d => d.RegisterHandlers(), Times.Once);
    }

    [Fact]
    public void Stop_CallsEventDispatcherUnregisterHandlers()
    {
        // Act
        _startup.Stop();

        // Assert
        _eventDispatcherMock.Verify(d => d.UnregisterHandlers(), Times.Once);
    }

    [Fact]
    public void Stop_CallsConnectionBootstrapDisconnect()
    {
        // Act
        _startup.Stop();

        // Assert
        _connectionBootstrapMock.Verify(b => b.Disconnect(), Times.Once);
    }

    [Fact]
    public void Start_DoesNotThrow()
    {
        // Act
        var act = () => _startup.Start();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Stop_DoesNotThrow()
    {
        // Act
        var act = () => _startup.Stop();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ConvenienceConstructor_WiresDependencies()
    {
        // Act — use the convenience constructor
        var startup = new AddonStartup(_settings);

        // Assert — does not throw on Start/Stop
        var startAct = () => startup.Start();
        var stopAct = () => startup.Stop();
        startAct.Should().NotThrow();
        stopAct.Should().NotThrow();
    }
}
