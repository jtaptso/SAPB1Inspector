using FormInspector.Domain.Snapshots;

namespace FormInspector.Infrastructure.Persistence;

/// <summary>
/// Cache layer for quick access to the most recently captured snapshots.
/// Wraps the repository to provide faster lookups for frequently accessed data.
/// </summary>
public class SnapshotCache
{
    private volatile Snapshot? _latestSnapshot;
    private volatile string? _latestFormType;
    private readonly object _lock = new();

    /// <summary>Updates the cache with the latest snapshot.</summary>
    public void Update(Snapshot snapshot)
    {
        lock (_lock)
        {
            _latestSnapshot = snapshot;
            _latestFormType = snapshot.Form.FormType.Value;
        }
    }

    /// <summary>Gets the latest snapshot if it matches the given form type.</summary>
    public Snapshot? TryGetLatest(string formType)
    {
        lock (_lock)
        {
            if (_latestFormType == formType && _latestSnapshot is not null)
                return _latestSnapshot;
            return null;
        }
    }

    /// <summary>Gets the latest snapshot regardless of form type.</summary>
    public Snapshot? TryGetLatest()
    {
        lock (_lock)
        {
            return _latestSnapshot;
        }
    }

    /// <summary>Clears the cache.</summary>
    public void Clear()
    {
        lock (_lock)
        {
            _latestSnapshot = null;
            _latestFormType = null;
        }
    }
}
