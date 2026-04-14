using System.Collections.Concurrent;
using FormInspector.Application.Interfaces;
using FormInspector.Domain.Snapshots;

namespace FormInspector.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of ISnapshotRepository for MVP.
/// Stores the latest snapshot per form type, replacing previous entries.
/// Thread-safe via ConcurrentDictionary.
/// </summary>
public class InMemorySnapshotRepository : ISnapshotRepository
{
    private readonly ConcurrentDictionary<string, Snapshot> _storeByFormType = new();
    private readonly ConcurrentDictionary<string, Snapshot> _storeById = new();

    public Task SaveAsync(Snapshot snapshot)
    {
        if (snapshot is null) throw new ArgumentNullException(nameof(snapshot));

        _storeByFormType[snapshot.Form.FormType.Value] = snapshot;
        _storeById[snapshot.SnapshotId] = snapshot;

        return Task.CompletedTask;
    }

    public Task<Snapshot?> GetLatestAsync(string formType)
    {
        _storeByFormType.TryGetValue(formType, out var snapshot);
        return Task.FromResult(snapshot);
    }

    public Task<IReadOnlyList<Snapshot>> GetAllAsync()
    {
        var snapshots = _storeByFormType.Values
            .OrderByDescending(s => s.CapturedAt)
            .ToList();

        return Task.FromResult<IReadOnlyList<Snapshot>>(snapshots);
    }

    public Task<Snapshot?> GetByIdAsync(string snapshotId)
    {
        _storeById.TryGetValue(snapshotId, out var snapshot);
        return Task.FromResult(snapshot);
    }
}
