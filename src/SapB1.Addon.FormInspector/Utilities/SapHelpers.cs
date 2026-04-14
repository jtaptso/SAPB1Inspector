#if SAP_UI_SDK
using SAPbouiCOM;
#endif
#if B1UP_SDK
using SwissAddonFramework.Application;
#endif
using System;

namespace SapB1.Addon.FormInspector.Utilities;

/// <summary>
/// Helper utilities for accessing SAP Business One context information.
/// Provides user name, machine name, and client ID extraction.
/// </summary>
public class SapHelpers
{
    /// <summary>
    /// Gets the current SAP user name.
    /// </summary>
    public string? GetCurrentUserName()
    {
#if B1UP_SDK
        try
        {
            return SwissAddonFramework.Application.User.Name;
        }
        catch (Exception)
        {
            // SwissAddonFramework not available — fall through
        }
#endif
#if SAP_UI_SDK
        if (SapContext.IsInitialized && SapContext.Application != null)
        {
            try
            {
                return SapContext.Application.Company?.UserName;
            }
            catch (Exception)
            {
                // Company object may not be available — fall through
            }
        }
#endif
        return Environment.UserName;
    }

    /// <summary>
    /// Gets the SAP client connection identifier.
    /// </summary>
    public string? GetClientId()
    {
#if SAP_UI_SDK
        if (SapContext.IsInitialized && SapContext.Application != null)
        {
            try
            {
                // The Application doesn't expose a direct client ID,
                // but we can derive one from the connection cookie or session info
                var company = SapContext.Application.Company;
                if (company != null)
                {
                    return $"{company.CompanyName}@{company.Server}";
                }
            }
            catch (Exception)
            {
                // Company info may not be available
            }
        }
#endif
        return null;
    }

    /// <summary>
    /// Gets the SAP company database name.
    /// </summary>
    public string? GetCompanyDb()
    {
#if SAP_UI_SDK
        if (SapContext.IsInitialized && SapContext.Application != null)
        {
            try
            {
                return SapContext.Application.Company?.CompanyDB;
            }
            catch (Exception)
            {
                // Company object may not be available
            }
        }
#endif
        return null;
    }
}
