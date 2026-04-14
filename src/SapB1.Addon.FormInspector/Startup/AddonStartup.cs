#if B1UP_SDK
using SwissAddonFramework.Hosting;
#endif
using System;
using Microsoft.Extensions.DependencyInjection;
using SapB1.Addon.FormInspector.Configuration;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Entry point for the SAP Business One add-on.
/// Uses <see cref="IServiceProvider"/> for dependency resolution.
/// The convenience constructor builds the container via
/// <see cref="ServiceCollectionExtensions.AddFormInspector"/>;
/// the parameterized constructor accepts pre-resolved dependencies for testing.
/// </summary>
public class AddonStartup : IDisposable
{
    private readonly FormEventDispatcher _eventDispatcher;
    private readonly Configuration.InspectorSettings _settings;
    private readonly ConnectionBootstrap _connectionBootstrap;
    private readonly ISapContext _sapContext;
    private readonly ServiceProvider? _serviceProvider;

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
        _serviceProvider = null;
    }

    /// <summary>
    /// Convenience constructor that builds a <see cref="ServiceProvider"/>
    /// via <see cref="ServiceCollectionExtensions.AddFormInspector"/>
    /// and resolves all dependencies automatically.
    /// </summary>
    public AddonStartup(Configuration.InspectorSettings settings)
    {
        _settings = settings;

        _serviceProvider = new ServiceCollection()
            .AddFormInspector(settings)
            .BuildServiceProvider(validateScopes: true);

        _sapContext = _serviceProvider.GetRequiredService<ISapContext>();
        _connectionBootstrap = _serviceProvider.GetRequiredService<ConnectionBootstrap>();
        _eventDispatcher = _serviceProvider.GetRequiredService<FormEventDispatcher>();
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

    /// <summary>
    /// Disposes the DI container and all disposable services.
    /// Ensures graceful shutdown (unregister handlers, disconnect) before disposing resources.
    /// </summary>
    public void Dispose()
    {
        Stop();
        _serviceProvider?.Dispose();
    }
}
