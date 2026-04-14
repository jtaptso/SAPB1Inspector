#if B1UP_SDK
using SwissAddonFramework.Hosting;
#endif
using System;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Entry point for the SAP Business One add-on.
/// Delegates connection management to <see cref="ConnectionBootstrap"/> and registers event handlers.
/// </summary>
public class AddonStartup
{
    private readonly FormEventDispatcher _eventDispatcher;
    private readonly Configuration.InspectorSettings _settings;
    private readonly ConnectionBootstrap _connectionBootstrap;

    public AddonStartup(
        FormEventDispatcher eventDispatcher,
        Configuration.InspectorSettings settings,
        ConnectionBootstrap connectionBootstrap)
    {
        _eventDispatcher = eventDispatcher;
        _settings = settings;
        _connectionBootstrap = connectionBootstrap;
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
