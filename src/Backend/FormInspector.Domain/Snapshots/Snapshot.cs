using FormInspector.Domain.Common;

namespace FormInspector.Domain.Snapshots;

/// <summary>
/// Aggregate root representing one captured SAP Business One UI form state.
/// Contains form metadata and all item metadata captured at a specific point in time.
/// </summary>
public class Snapshot : EntityBase
{
    /// <summary>Unique identifier for this snapshot.</summary>
    public string SnapshotId { get; init; }

    /// <summary>Timestamp when the snapshot was captured.</summary>
    public DateTime CapturedAt { get; init; }

    /// <summary>Context information: user, machine, and client.</summary>
    public SnapshotContext Context { get; init; }

    /// <summary>Form-level metadata.</summary>
    public FormMetadata Form { get; init; }

    /// <summary>Collection of item-level metadata.</summary>
    public IReadOnlyList<ItemMetadata> Items { get; init; }

    /// <summary>Schema version for forward compatibility.</summary>
    public string SchemaVersion { get; init; }

    public Snapshot(
        string snapshotId,
        DateTime capturedAt,
        SnapshotContext context,
        FormMetadata form,
        IReadOnlyList<ItemMetadata>? items = null,
        string schemaVersion = "1.0")
    {
        if (string.IsNullOrWhiteSpace(snapshotId))
            throw new ArgumentException("SnapshotId cannot be null or empty.", nameof(snapshotId));

        SnapshotId = snapshotId;
        Id = snapshotId;
        CapturedAt = capturedAt;
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Form = form ?? throw new ArgumentNullException(nameof(form));
        Items = items ?? [];
        SchemaVersion = schemaVersion;
    }
}

/// <summary>
/// Context information about when and where a snapshot was captured.
/// </summary>
public record SnapshotContext
{
    /// <summary>SAP user name.</summary>
    public string? UserName { get; init; }

    /// <summary>Machine name where the SAP client is running.</summary>
    public string? MachineName { get; init; }

    /// <summary>SAP client connection identifier.</summary>
    public string? ClientId { get; init; }

    public SnapshotContext(string? userName = null, string? machineName = null, string? clientId = null)
    {
        UserName = userName;
        MachineName = machineName;
        ClientId = clientId;
    }
}
