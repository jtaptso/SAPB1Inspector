using FormInspector.Application.DTOs;

namespace FormInspector.Application.UseCases.ReceiveSnapshot;

/// <summary>
/// Command representing an incoming snapshot from the SAP add-on.
/// </summary>
public record ReceiveSnapshotCommand
{
    /// <summary>The snapshot input data received from the add-on.</summary>
    public SnapshotInputDto Snapshot { get; init; } = new();
}
