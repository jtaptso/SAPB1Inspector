using SapB1.Addon.FormInspector.Events;

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Entry point for the SAP Business One add-on.
/// Initializes the SwissAddonFramework connection and registers event handlers.
/// </summary>
public class AddonStartup
{
    private readonly FormEventDispatcher _eventDispatcher;
    private readonly Configuration.InspectorSettings _settings;

    public AddonStartup(FormEventDispatcher eventDispatcher, Configuration.InspectorSettings settings)
    {
        _eventDispatcher = eventDispatcher;
        _settings = settings;
    }

    /// <summary>
    /// Starts the add-on. Called by the SwissAddonFramework runtime.
    /// </summary>
    public void Start()
    {
        // TODO: Initialize SwissAddonFramework connection
        // SwissAddonFramework.Hosting.Startup.Initialize();
        // Register the event dispatcher with the SAP UI API
        _eventDispatcher.RegisterHandlers();
    }

    /// <summary>
    /// Stops the add-on gracefully.
    /// </summary>
    public void Stop()
    {
        _eventDispatcher.UnregisterHandlers();
    }
}
