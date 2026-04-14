using System;

namespace SapB1.Addon.FormInspector.Utilities;

/// <summary>
/// Abstraction for SAP Business One context access.
/// Injected into inspectors and startup components to decouple them
/// from static state, enabling isolated unit testing.
/// </summary>
public interface ISapContext
{
    /// <summary>Whether the SAP context has been initialized.</summary>
    bool IsInitialized { get; }

    /// <summary>Resets the context (used during shutdown or in tests).</summary>
    void Reset();

#if SAP_UI_SDK
    /// <summary>The SAP UI API Application object.</summary>
    SAPbouiCOM.Application? Application { get; }

    /// <summary>Initializes the context with the Application object.</summary>
    void Initialize(SAPbouiCOM.Application application);

    /// <summary>
    /// Resolves a form by its UniqueID.
    /// Throws if the context is not initialized or the form is not found.
    /// </summary>
    SAPbouiCOM.Form GetForm(string formUid);

    /// <summary>
    /// Attempts to resolve a form by its UniqueID.
    /// Returns null if the form is not found or the context is not initialized.
    /// </summary>
    SAPbouiCOM.Form? TryGetForm(string formUid);
#else
    /// <summary>The SAP Application object (untyped when SDK is not available).</summary>
    object? Application { get; }

    /// <summary>Initializes the context with the Application object.</summary>
    void Initialize(object application);
#endif
}

/// <summary>
/// Default implementation of <see cref="ISapContext"/>.
/// Holds the SAP Business One Application object and provides form resolution.
/// Injected as a singleton via constructor injection — no static state.
/// </summary>
public class SapContext : ISapContext
{
    /// <inheritdoc/>
#if SAP_UI_SDK
    public SAPbouiCOM.Application? Application { get; private set; }
#else
    public object? Application { get; private set; }
#endif

    /// <inheritdoc/>
    public bool IsInitialized { get; private set; }

    /// <inheritdoc/>
#if SAP_UI_SDK
    public void Initialize(SAPbouiCOM.Application application)
#else
    public void Initialize(object application)
#endif
    {
        Application = application ?? throw new ArgumentNullException(nameof(application));
        IsInitialized = true;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        Application = null;
        IsInitialized = false;
    }

#if SAP_UI_SDK
    /// <inheritdoc/>
    public SAPbouiCOM.Form GetForm(string formUid)
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

    /// <inheritdoc/>
    public SAPbouiCOM.Form? TryGetForm(string formUid)
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
