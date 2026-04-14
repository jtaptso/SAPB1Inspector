#if SAP_UI_SDK
using SAPbouiCOM;
#endif
#if B1UP_SDK
using SwissAddonFramework.UI;
#endif
using System;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects SAP Business One forms at runtime using the SAP UI API.
/// Extracts form-level metadata (type, title, mode, pane level).
/// Read-only — does not modify the SAP UI.
/// </summary>
public class FormInspectorService
{
    private readonly ISapContext _sapContext;

    public FormInspectorService(ISapContext sapContext)
    {
        _sapContext = sapContext;
    }

    /// <summary>
    /// Inspects a form by its SAP UniqueID and returns form-level metadata.
    /// </summary>
    public FormDto InspectForm(string formUid)
    {
#if SAP_UI_SDK
        var form = _sapContext.TryGetForm(formUid);
        if (form != null)
        {
            try
            {
                var dto = new FormDto
                {
                    FormType = form.FormTypeEx ?? form.FormType ?? string.Empty,
                    UniqueId = form.UniqueID ?? formUid,
                    Title = form.Title ?? string.Empty,
                    Mode = MapFormMode(form.Mode),
                    PaneLevel = form.PaneLevel
                };

                System.Runtime.InteropServices.Marshal.ReleaseComObject(form);
                return dto;
            }
            catch (Exception)
            {
                // Form may have been closed or is busy — fall through to default
            }
        }
#endif
        return new FormDto
        {
            FormType = string.Empty,
            UniqueId = formUid,
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
#if SAP_UI_SDK
        if (_sapContext.IsInitialized && _sapContext.Application != null)
        {
            try
            {
                return _sapContext.Application.Company?.Version;
            }
            catch (Exception)
            {
                // Application or Company may not be available
            }
        }
#endif
        return null;
    }

#if SAP_UI_SDK
    /// <summary>
    /// Maps SAP BoFormMode enum to a string representation for the DTO.
    /// </summary>
    private static string MapFormMode(BoFormMode mode)
    {
        return mode switch
        {
            BoFormMode.fm_ADD => "ADD",
            BoFormMode.fm_FIND => "FIND",
            BoFormMode.fm_OK => "OK",
            BoFormMode.fm_VIEW => "VIEW",
            _ => mode.ToString()
        };
    }
#endif
}
