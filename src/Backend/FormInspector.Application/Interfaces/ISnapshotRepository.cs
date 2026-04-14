using FormInspector.Domain.Snapshots;

namespace FormInspector.Application.Interfaces;

/// <summary>
/// Repository interface (port) for snapshot persistence.
/// Implemented by Infrastructure layer; Application layer remains persistence-agnostic.
/// </summary>
public interface ISnapshotRepository
{
    /// <summary>Saves a snapshot, replacing any previous snapshot for the same form type.</summary>
    Task SaveAsync(Snapshot snapshot);

    /// <summary>Retrieves the latest snapshot for a given form type.</summary>
    Task<Snapshot?> GetLatestAsync(string formType);

    /// <summary>Retrieves all stored snapshots (for listing available forms).</summary>
    Task<IReadOnlyList<Snapshot>> GetAllAsync();

    /// <summary>Retrieves a specific snapshot by its ID.</summary>
    Task<Snapshot?> GetByIdAsync(string snapshotId);
}
