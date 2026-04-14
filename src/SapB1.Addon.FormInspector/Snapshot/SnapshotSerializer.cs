using System.Text.Json;
using System.Text.Json.Serialization;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Snapshot;

/// <summary>
/// Serializes snapshot DTOs to JSON for transmission to the backend.
/// Provides consistent, versioned serialization with camelCase naming.
/// </summary>
public static class SnapshotSerializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>Serializes a snapshot DTO to a JSON string.</summary>
    public static string Serialize(SnapshotDto snapshot)
    {
        return JsonSerializer.Serialize(snapshot, Options);
    }

    /// <summary>Deserializes a JSON string to a snapshot DTO.</summary>
    public static SnapshotDto? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<SnapshotDto>(json, Options);
    }
}
