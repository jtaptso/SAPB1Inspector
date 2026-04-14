using Microsoft.AspNetCore.SignalR;

namespace FormInspector.Infrastructure.Notifications;

/// <summary>
/// SignalR hub for broadcasting snapshot updates to connected viewers.
/// The hub is a thin transport layer — it does not access repositories or domain objects directly.
/// Defined in Infrastructure because the SignalRSnapshotNotifier depends on IHubContext&lt;SnapshotHub&gt;.
/// The Presentation layer maps this hub via app.MapHub&lt;SnapshotHub&gt;().
/// </summary>
public class SnapshotHub : Hub
{
    /// <summary>Called when a client connects to the hub.</summary>
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    /// <summary>Called when a client disconnects from the hub.</summary>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
