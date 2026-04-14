using FormInspector.Application.Interfaces;
using FormInspector.Application.Mapping;
using FormInspector.Domain.Snapshots;

namespace FormInspector.Application.UseCases.ReceiveSnapshot;

/// <summary>
/// Handles incoming snapshots from the SAP add-on.
/// Validates schema version, persists the snapshot, and triggers notification.
/// </summary>
public class ReceiveSnapshotHandler
{
    private const string SupportedSchemaVersion = "1.0";

    private readonly ISnapshotRepository _repository;
    private readonly ISnapshotNotifier _notifier;

    public ReceiveSnapshotHandler(
        ISnapshotRepository repository,
        ISnapshotNotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    /// <summary>
    /// Processes an incoming snapshot command.
    /// </summary>
    /// <param name="command">The receive snapshot command.</param>
    /// <returns>The snapshot ID if successful.</returns>
    /// <exception cref="InvalidOperationException">Thrown when schema version is unsupported.</exception>
    public async Task<string> HandleAsync(ReceiveSnapshotCommand command)
    {
        ValidateSchemaVersion(command.Snapshot.SchemaVersion);

        var snapshot = command.Snapshot.ToDomain();

        await _repository.SaveAsync(snapshot);
        await _notifier.NotifyUpdatedAsync(snapshot.SnapshotId, snapshot.Form.FormType.Value);

        return snapshot.SnapshotId;
    }

    private static void ValidateSchemaVersion(string schemaVersion)
    {
        if (string.IsNullOrWhiteSpace(schemaVersion))
            throw new InvalidOperationException("Snapshot schema version is required.");

        if (!IsSupportedVersion(schemaVersion))
            throw new InvalidOperationException(
                $"Unsupported snapshot schema version '{schemaVersion}'. Supported: {SupportedSchemaVersion}");
    }

    private static bool IsSupportedVersion(string version)
    {
        // Allow major version match (e.g., "1.0", "1.1" both supported under "1.x")
        var majorPart = version.Split('.').First();
        var supportedMajor = SupportedSchemaVersion.Split('.').First();
        return majorPart == supportedMajor;
    }
}
