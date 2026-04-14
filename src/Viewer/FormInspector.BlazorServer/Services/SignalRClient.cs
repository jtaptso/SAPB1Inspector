using FormInspector.Application.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace FormInspector.BlazorServer.Services;

/// <summary>
/// SignalR client service for receiving real-time snapshot update notifications.
/// Connects to the backend SnapshotHub and raises events when updates arrive.
/// </summary>
public class SignalRClient : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly ILogger<SignalRClient> _logger;

    /// <summary>Raised when a snapshot update notification is received.</summary>
    public event Func<SnapshotUpdatedNotification, Task>? OnSnapshotUpdated;

    /// <summary>Indicates whether the client is connected to the hub.</summary>
    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

    public SignalRClient(ILogger<SignalRClient> logger, IConfiguration configuration)
    {
        _logger = logger;

        var hubUrl = configuration["SnapshotHub:Url"] ?? "http://localhost:5000/snapshothub";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15) })
            .Build();

        _hubConnection.On<SnapshotUpdatedNotification>("SnapshotUpdated", async (notification) =>
        {
            if (OnSnapshotUpdated is not null)
                await OnSnapshotUpdated.Invoke(notification);
        });

        _hubConnection.Reconnecting += error =>
        {
            _logger.LogWarning(error, "SignalR reconnecting...");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            _logger.LogInformation("SignalR reconnected with ID: {ConnectionId}", connectionId);
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            _logger.LogError(error, "SignalR connection closed");
            return Task.CompletedTask;
        };
    }

    /// <summary>Starts the SignalR connection.</summary>
    public async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR connected. ConnectionId: {ConnectionId}", _hubConnection.ConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to SignalR hub");
        }
    }

    /// <summary>Stops the SignalR connection.</summary>
    public async Task StopAsync()
    {
        await _hubConnection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
