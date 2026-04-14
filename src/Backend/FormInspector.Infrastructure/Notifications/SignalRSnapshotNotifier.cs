using FormInspector.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FormInspector.Infrastructure.Notifications;

/// <summary>
/// SignalR implementation of ISnapshotNotifier.
/// Broadcasts snapshot update notifications to all connected clients.
/// Keeps Application layer free from SignalR dependencies.
/// </summary>
public class SignalRSnapshotNotifier : ISnapshotNotifier
{
    private readonly IHubContext<SnapshotHub> _hubContext;

    public SignalRSnapshotNotifier(IHubContext<SnapshotHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task NotifyUpdatedAsync(string snapshotId, string formType)
    {
        await _hubContext.Clients.All.SendAsync("SnapshotUpdated", new
        {
            SnapshotId = snapshotId,
            FormType = formType,
            UpdatedAt = DateTime.UtcNow
        });
    }
}
