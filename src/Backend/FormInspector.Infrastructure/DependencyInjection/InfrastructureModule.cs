using FormInspector.Application.Interfaces;
using FormInspector.Infrastructure.Notifications;
using FormInspector.Infrastructure.Persistence;
using FormInspector.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace FormInspector.Infrastructure.DependencyInjection;

/// <summary>
/// Registers all Infrastructure layer services with the DI container.
/// Call this from the Presentation/Blazor Program.cs to wire up infrastructure.
/// </summary>
public static class InfrastructureModule
{
    /// <summary>
    /// Adds Infrastructure layer services to the DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Persistence
        services.AddSingleton<ISnapshotRepository, InMemorySnapshotRepository>();

        // Time
        services.AddSingleton<ITimeProvider, SystemTimeProvider>();

        // Notifications (SignalR-based) — requires AddSignalR() to be called first
        services.AddTransient<ISnapshotNotifier, SignalRSnapshotNotifier>();

        return services;
    }
}
