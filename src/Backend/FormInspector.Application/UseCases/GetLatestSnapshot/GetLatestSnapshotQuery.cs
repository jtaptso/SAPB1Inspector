namespace FormInspector.Application.UseCases.GetLatestSnapshot;

/// <summary>
/// Query to retrieve the latest snapshot for a given form type.
/// </summary>
public record GetLatestSnapshotQuery
{
    /// <summary>The SAP form type to look up (e.g., "139").</summary>
    public string FormType { get; init; } = string.Empty;
}

/// <summary>
/// Query to retrieve all stored snapshots (summary view).
/// </summary>
public record GetAllSnapshotsQuery;

/// <summary>
/// Query to retrieve a specific snapshot by ID.
/// </summary>
public record GetSnapshotByIdQuery
{
    /// <summary>The unique snapshot ID.</summary>
    public string SnapshotId { get; init; } = string.Empty;
}
