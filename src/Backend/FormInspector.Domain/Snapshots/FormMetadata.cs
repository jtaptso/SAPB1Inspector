using FormInspector.Domain.Enums;
using FormInspector.Domain.ValueObjects;

namespace FormInspector.Domain.Snapshots;

/// <summary>
/// Represents metadata about an SAP Business One form.
/// </summary>
public record FormMetadata
{
    /// <summary>SAP form type identifier (e.g., "139" for Sales Order).</summary>
    public FormType FormType { get; init; }

    /// <summary>Unique identifier of the form instance.</summary>
    public string UniqueId { get; init; }

    /// <summary>Form title / caption.</summary>
    public string Title { get; init; }

    /// <summary>Current mode of the form (Add, OK, Find, Update).</summary>
    public FormMode Mode { get; init; }

    /// <summary>Current pane level of the form.</summary>
    public int PaneLevel { get; init; }

    /// <summary>SAP client version string.</summary>
    public string? SapVersion { get; init; }

    public FormMetadata(
        FormType formType,
        string uniqueId,
        string title,
        FormMode mode = FormMode.Ok,
        int paneLevel = 0,
        string? sapVersion = null)
    {
        FormType = formType ?? throw new ArgumentNullException(nameof(formType));
        UniqueId = uniqueId ?? throw new ArgumentNullException(nameof(uniqueId));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Mode = mode;
        PaneLevel = paneLevel;
        SapVersion = sapVersion;
    }
}
