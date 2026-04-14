using System.Collections.Concurrent;
using SapB1.Addon.FormInspector.Configuration;

namespace SapB1.Addon.FormInspector.Utilities;

/// <summary>
/// Throttles form inspection to prevent excessive snapshot creation.
/// Ensures that the same form type is not inspected more frequently than
/// the configured throttle interval.
/// </summary>
public class Throttler
{
    private readonly InspectorSettings _settings;
    private readonly ConcurrentDictionary<string, DateTime> _lastProcessed = new();

    public Throttler(InspectorSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Determines whether a form type should be processed based on the throttle interval.
    /// Returns true if enough time has elapsed since the last processing.
    /// </summary>
    public bool ShouldProcess(string formType)
    {
        var now = DateTime.UtcNow;
        var interval = TimeSpan.FromMilliseconds(_settings.ThrottleIntervalMs);

        if (_lastProcessed.TryGetValue(formType, out var lastProcessed))
        {
            if (now - lastProcessed < interval)
                return false;
        }

        _lastProcessed[formType] = now;
        return true;
    }

    /// <summary>
    /// Resets the throttle state for a specific form type.
    /// </summary>
    public void Reset(string formType)
    {
        _lastProcessed.TryRemove(formType, out _);
    }

    /// <summary>
    /// Resets all throttle state.
    /// </summary>
    public void ResetAll()
    {
        _lastProcessed.Clear();
    }
}
