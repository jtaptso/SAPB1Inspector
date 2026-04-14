#if B1UP_SDK
using SwissAddonFramework.UI.EventHandlers;
#endif
using System;
using System.Threading.Tasks;
using SapB1.Addon.FormInspector.Inspection;
using SapB1.Addon.FormInspector.Snapshot;
using SapB1.Addon.FormInspector.Publishing;
using SapB1.Addon.FormInspector.Configuration;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Events;

/// <summary>
/// Dispatches SAP UI form events to the inspection pipeline.
/// Subscribes only to structural events (FormLoad, FormActivate, FormVisible, FormModeChange).
/// Events like key presses or validations are intentionally excluded to avoid UI freezes.
/// </summary>
public class FormEventDispatcher
{
    private readonly FormInspectorService _formInspector;
    private readonly SnapshotBuilder _snapshotBuilder;
    private readonly SnapshotPublisher _publisher;
    private readonly InspectorSettings _settings;
    private readonly Throttler _throttler;

    public FormEventDispatcher(
        FormInspectorService formInspector,
        SnapshotBuilder snapshotBuilder,
        SnapshotPublisher publisher,
        InspectorSettings settings,
        Throttler throttler)
    {
        _formInspector = formInspector;
        _snapshotBuilder = snapshotBuilder;
        _publisher = publisher;
        _settings = settings;
        _throttler = throttler;
    }

    /// <summary>
    /// Registers SAP UI event handlers.
    /// </summary>
    public void RegisterHandlers()
    {
        // TODO: Register with SwissAddonFramework event system
        // SwissAddonFramework.UI.EventHandlers.FormLoad += OnFormLoad;
        // SwissAddonFramework.UI.EventHandlers.FormActivate += OnFormActivate;
        // SwissAddonFramework.UI.EventHandlers.FormVisible += OnFormVisible;
        // SwissAddonFramework.UI.EventHandlers.FormModeChange += OnFormModeChange;

        // Optional: PaneChange for pane-dependent items
        // if (_settings.TrackPaneChanges)
        //     SwissAddonFramework.UI.EventHandlers.PaneChange += OnPaneChange;
    }

    /// <summary>
    /// Unregisters SAP UI event handlers.
    /// </summary>
    public void UnregisterHandlers()
    {
        // TODO: Unregister from SwissAddonFramework event system
    }

    private async void OnFormLoad(object? sender, FormEventArgs e)
    {
        if (!_throttler.ShouldProcess(e.FormType)) return;

        await InspectAndPublishAsync(e.FormType, e.FormId);
    }

    private async void OnFormActivate(object? sender, FormEventArgs e)
    {
        if (!_throttler.ShouldProcess(e.FormType)) return;

        await InspectAndPublishAsync(e.FormType, e.FormId);
    }

    private async void OnFormVisible(object? sender, FormEventArgs e)
    {
        if (!_throttler.ShouldProcess(e.FormType)) return;

        await InspectAndPublishAsync(e.FormType, e.FormId);
    }

    private async void OnFormModeChange(object? sender, FormEventArgs e)
    {
        if (!_throttler.ShouldProcess(e.FormType)) return;

        await InspectAndPublishAsync(e.FormType, e.FormId);
    }

    private async void OnPaneChange(object? sender, FormEventArgs e)
    {
        if (!_settings.TrackPaneChanges) return;
        if (!_throttler.ShouldProcess(e.FormType)) return;

        await InspectAndPublishAsync(e.FormType, e.FormId);
    }

    private async Task InspectAndPublishAsync(string formType, int formId)
    {
        try
        {
            // Inspect the form using SAP UI API
            var formData = _formInspector.InspectForm(formId);

            // Build a snapshot from the inspection data
            var snapshot = _snapshotBuilder.Build(formData);

            // Publish the snapshot to the backend
            await _publisher.PublishAsync(snapshot);
        }
        catch (Exception)
        {
            // Swallow exceptions to prevent SAP UI freezes
            // Logging would be added in production
        }
    }
}

/// <summary>
/// Event arguments for SAP form events.
/// </summary>
public class FormEventArgs : EventArgs
{
    public string FormType { get; set; } = string.Empty;
    public int FormId { get; set; }
}
