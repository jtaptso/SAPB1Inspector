using FormInspector.Application.DTOs;
using FormInspector.Application.Interfaces;
using FormInspector.Application.Mapping;

namespace FormInspector.Application.UseCases.GetLatestSnapshot;

/// <summary>
/// Handles queries for retrieving snapshots.
/// </summary>
public class GetLatestSnapshotHandler
{
    private readonly ISnapshotRepository _repository;

    public GetLatestSnapshotHandler(ISnapshotRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Gets the latest snapshot for a given form type.</summary>
    public async Task<SnapshotOutputDto?> HandleAsync(GetLatestSnapshotQuery query)
    {
        var snapshot = await _repository.GetLatestAsync(query.FormType);
        return snapshot?.ToOutputDto();
    }

    /// <summary>Gets all stored snapshots as summaries.</summary>
    public async Task<IReadOnlyList<SnapshotSummaryDto>> HandleAsync(GetAllSnapshotsQuery query)
    {
        var snapshots = await _repository.GetAllAsync();
        return snapshots.Select(s => s.ToSummaryDto()).ToList();
    }

    /// <summary>Gets a specific snapshot by ID.</summary>
    public async Task<SnapshotOutputDto?> HandleAsync(GetSnapshotByIdQuery query)
    {
        var snapshot = await _repository.GetByIdAsync(query.SnapshotId);
        return snapshot?.ToOutputDto();
    }
}
