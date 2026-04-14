namespace FormInspector.Infrastructure.Persistence;

/// <summary>
/// Configuration options for the persistence layer.
/// </summary>
public class PersistenceConfig
{
    /// <summary>The storage mechanism to use. Currently only "InMemory" is supported.</summary>
    public string StorageType { get; set; } = "InMemory";

    /// <summary>Connection string for database storage (future use).</summary>
    public string? ConnectionString { get; set; }

    /// <summary>Maximum number of snapshots to retain per form type.</summary>
    public int MaxSnapshotsPerFormType { get; set; } = 10;
}
