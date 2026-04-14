using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects SAP Business One forms at runtime using the SAP UI API.
/// Extracts form-level metadata (type, title, mode, pane level).
/// Read-only — does not modify the SAP UI.
/// </summary>
public class FormInspectorService
{
    /// <summary>
    /// Inspects a form by its SAP form ID and returns form-level metadata.
    /// </summary>
    public FormDto InspectForm(int formId)
    {
        // TODO: Use SAPbouiCOM.Form to extract metadata
        // var form = SwissAddonFramework.UI.Forms.GetForm(formId);
        // return new FormDto { ... };

        return new FormDto
        {
            FormType = string.Empty,
            UniqueId = formId.ToString(),
            Title = string.Empty,
            Mode = "OK",
            PaneLevel = 0,
            SapVersion = null
        };
    }

    /// <summary>
    /// Gets the SAP client version string.
    /// </summary>
    public string? GetSapVersion()
    {
        // TODO: Use SAPbouiCOM.Application.Company.Version
        return null;
    }
}
