using SapB1.Addon.FormInspector.Configuration;

namespace SapB1.Addon.FormInspector.Events;

/// <summary>
/// Filters SAP UI events based on configuration.
/// Determines which form types and events should be processed.
/// </summary>
public class FormEventFilters
{
    private readonly InspectorSettings _settings;

    public FormEventFilters(InspectorSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Determines whether a form event should be processed.
    /// </summary>
    public bool ShouldProcess(string formType, string eventType)
    {
        // Check if the form type is in the allowed list
        if (_settings.IncludedFormTypes?.Count > 0 &&
            !_settings.IncludedFormTypes.Contains(formType))
            return false;

        // Check if the form type is excluded
        if (_settings.ExcludedFormTypes?.Count > 0 &&
            _settings.ExcludedFormTypes.Contains(formType))
            return false;

        // Check if the event type is enabled
        return eventType switch
        {
            "FormLoad" => _settings.TrackFormLoad,
            "FormActivate" => _settings.TrackFormActivate,
            "FormVisible" => _settings.TrackFormVisible,
            "FormModeChange" => _settings.TrackFormModeChange,
            "PaneChange" => _settings.TrackPaneChanges,
            _ => false
        };
    }
}
