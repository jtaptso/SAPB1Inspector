using System.Text.Json;
using System.Text.Json.Serialization;
using FormInspector.Application.DTOs;

namespace FormInspector.Infrastructure.Serialization;

/// <summary>
/// Handles JSON serialization and deserialization of snapshot DTOs.
/// Provides consistent serialization settings across the system.
/// </summary>
public static class SnapshotJsonMapper
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>Serializes a snapshot input DTO to JSON.</summary>
    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, DefaultOptions);
    }

    /// <summary>Deserializes JSON to a snapshot input DTO.</summary>
    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, DefaultOptions);
    }

    /// <summary>Gets the shared JsonSerializerOptions for consistent serialization.</summary>
    public static JsonSerializerOptions GetOptions() => DefaultOptions;
}
