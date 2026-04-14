using System;

namespace SapB1.Addon.FormInspector.Utilities;

/// <summary>
/// Provides centralized access to the SAP Business One Application object.
/// Initialized once during add-on startup, then used by all inspectors.
/// </summary>
public static class SapContext
{
    /// <summary>
    /// The SAP UI API Application object.
    /// Set during add-on startup via <see cref="Initialize"/>.
    /// </summary>
#if SAP_UI_SDK
    public static SAPbouiCOM.Application? Application { get; private set; }
#else
    public static object? Application { get; private set; }
#endif

    /// <summary>
    /// Whether the SAP context has been initialized.
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// Initializes the SAP context with the Application object.
    /// Called once during add-on startup.
    /// </summary>
#if SAP_UI_SDK
    public static void Initialize(SAPbouiCOM.Application application)
#else
    public static void Initialize(object application)
#endif
    {
        Application = application ?? throw new ArgumentNullException(nameof(application));
        IsInitialized = true;
    }

    /// <summary>
    /// Resets the context (used in tests or during shutdown).
    /// </summary>
    public static void Reset()
    {
        Application = null;
        IsInitialized = false;
    }

#if SAP_UI_SDK
    /// <summary>
    /// Resolves a form by its UniqueID.
    /// Throws if the context is not initialized or the form is not found.
    /// </summary>
    public static SAPbouiCOM.Form GetForm(string formUid)
    {
        if (!IsInitialized || Application == null)
            throw new InvalidOperationException("SapContext is not initialized. Call Initialize() during startup.");

        try
        {
            return Application.Forms.Item(formUid);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Form with UniqueID '{formUid}' not found or inaccessible.", ex);
        }
    }

    /// <summary>
    /// Attempts to resolve a form by its UniqueID.
    /// Returns null if the form is not found or the context is not initialized.
    /// </summary>
    public static SAPbouiCOM.Form? TryGetForm(string formUid)
    {
        if (!IsInitialized || Application == null)
            return null;

        try
        {
            return Application.Forms.Item(formUid);
        }
        catch
        {
            return null;
        }
    }
#endif
}
