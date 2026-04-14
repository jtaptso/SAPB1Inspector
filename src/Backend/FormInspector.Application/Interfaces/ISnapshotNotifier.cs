namespace FormInspector.Application.Interfaces;

/// <summary>
/// Notifier interface (port) for broadcasting snapshot updates.
/// Implemented by Infrastructure layer using SignalR; Application layer remains transport-agnostic.
/// </summary>
public interface ISnapshotNotifier
{
    /// <summary>Notifies all connected clients that a snapshot has been updated.</summary>
    Task NotifyUpdatedAsync(string snapshotId, string formType);
}
