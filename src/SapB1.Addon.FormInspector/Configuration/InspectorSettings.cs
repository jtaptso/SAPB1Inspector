using System.Collections.Generic;

namespace SapB1.Addon.FormInspector.Configuration;

/// <summary>
/// Configuration settings for the SAP B1 Form Inspector add-on.
/// Controls which events to track, throttling, and backend connection.
/// </summary>
public class InspectorSettings
{
    /// <summary>Backend API URL where snapshots are published.</summary>
    public string BackendUrl { get; set; } = "http://localhost:5000";

    /// <summary>Timeout in seconds for HTTP publish operations.</summary>
    public int PublishTimeoutSeconds { get; set; } = 10;

    /// <summary>Minimum interval in milliseconds between snapshots for the same form type.</summary>
    public int ThrottleIntervalMs { get; set; } = 1000;

    /// <summary>Whether to track FormLoad events.</summary>
    public bool TrackFormLoad { get; set; } = true;

    /// <summary>Whether to track FormActivate events.</summary>
    public bool TrackFormActivate { get; set; } = true;

    /// <summary>Whether to track FormVisible events.</summary>
    public bool TrackFormVisible { get; set; } = true;

    /// <summary>Whether to track FormModeChange events.</summary>
    public bool TrackFormModeChange { get; set; } = true;

    /// <summary>Whether to track PaneChange events (optional, can be noisy).</summary>
    public bool TrackPaneChanges { get; set; } = false;

    /// <summary>List of form types to include (empty = all).</summary>
    public List<string>? IncludedFormTypes { get; set; }

    /// <summary>List of form types to exclude.</summary>
    public List<string>? ExcludedFormTypes { get; set; }

    /// <summary>Maximum number of retry attempts for publishing.</summary>
    public int MaxPublishRetries { get; set; } = 3;
}
