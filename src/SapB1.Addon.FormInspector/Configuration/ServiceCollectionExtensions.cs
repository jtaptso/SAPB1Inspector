using Microsoft.Extensions.DependencyInjection;
using SapB1.Addon.FormInspector.Events;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Publishing;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Startup;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Configuration;

/// <summary>
/// Extension methods for registering all Form Inspector services
/// with <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Form Inspector services into the DI container.
    /// Call once at application startup.
    /// </summary>
    /// <param name="services">The service collection to register into.</param>
    /// <param name="settings">Configuration settings (registered as singleton).</param>
    /// <returns>The service collection, for chaining.</returns>
    public static IServiceCollection AddFormInspector(this IServiceCollection services, InspectorSettings settings)
    {
        // Configuration — singleton
        services.AddSingleton(settings);

        // Core SAP context — singleton (holds the Application reference)
        services.AddSingleton<ISapContext, SapContext>();

        // Inspectors — singletons (stateless except for ISapContext dependency)
        services.AddSingleton<MatrixInspector>();
        services.AddSingleton<ItemInspector>();
        services.AddSingleton<DataSourceInspector>();
        services.AddSingleton<FormInspectorService>();
        services.AddSingleton<SapHelpers>();

        // Snapshot pipeline — singletons
        services.AddSingleton<SnapshotBuilder>();
        services.AddSingleton<HttpPublisher>();
        services.AddSingleton<SnapshotPublisher>();

        // Throttling — singleton (tracks per-form-type timestamps)
        services.AddSingleton<Throttler>();

        // Startup & events — singletons
        services.AddSingleton<ConnectionBootstrap>();
        services.AddSingleton<FormEventDispatcher>();
        services.AddSingleton<AddonStartup>();

        return services;
    }
}
