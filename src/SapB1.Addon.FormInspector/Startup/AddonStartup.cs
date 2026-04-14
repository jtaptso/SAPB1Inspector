#if B1UP_SDK
using SwissAddonFramework.Hosting;
#endif
using System;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Publishing;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Entry point for the SAP Business One add-on.
/// Acts as the composition root — creates the <see cref="ISapContext"/> singleton
/// and wires all dependencies, then delegates to <see cref="ConnectionBootstrap"/>.
/// </summary>
public class AddonStartup
{
    private readonly FormEventDispatcher _eventDispatcher;
    private readonly Configuration.InspectorSettings _settings;
    private readonly ConnectionBootstrap _connectionBootstrap;
    private readonly ISapContext _sapContext;

    public AddonStartup(
        FormEventDispatcher eventDispatcher,
        Configuration.InspectorSettings settings,
        ConnectionBootstrap connectionBootstrap,
        ISapContext sapContext)
    {
        _eventDispatcher = eventDispatcher;
        _settings = settings;
        _connectionBootstrap = connectionBootstrap;
        _sapContext = sapContext;
    }

    /// <summary>
    /// Convenience constructor that creates a default <see cref="SapContext"/> instance
    /// and wires all dependencies automatically.
    /// </summary>
    public AddonStartup(Configuration.InspectorSettings settings)
    {
        _settings = settings;
        _sapContext = new SapContext();

        var matrixInspector = new MatrixInspector(_sapContext);
        var itemInspector = new ItemInspector(matrixInspector, _sapContext);
        var dataSourceInspector = new DataSourceInspector(_sapContext);
        var formInspector = new FormInspectorService(_sapContext);
        var sapHelpers = new Utilities.SapHelpers(_sapContext);
        var snapshotBuilder = new Snapshot.SnapshotBuilder(itemInspector, dataSourceInspector, sapHelpers);
        var httpPublisher = new Publishing.HttpPublisher(settings);
        var publisher = new Publishing.SnapshotPublisher(httpPublisher);
        var throttler = new Utilities.Throttler(settings);

        _connectionBootstrap = new ConnectionBootstrap(_sapContext);
        _eventDispatcher = new FormEventDispatcher(
            formInspector, snapshotBuilder, publisher, settings, throttler);
    }

    /// <summary>
    /// Starts the add-on. Called by the SwissAddonFramework runtime.
    /// Connects to SAP and registers event handlers.
    /// </summary>
    public void Start()
    {
        // Delegate connection setup to ConnectionBootstrap
        _connectionBootstrap.Connect();

        // Register the event dispatcher with the SAP UI API
        _eventDispatcher.RegisterHandlers();
    }

    /// <summary>
    /// Stops the add-on gracefully.
    /// </summary>
    public void Stop()
    {
        _eventDispatcher.UnregisterHandlers();
        _connectionBootstrap.Disconnect();
    }
}
