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
        // TODO: Use SwissAddonFramework.Application.User.Name
        // or SAPbouiCOM.Application.Company.UserName
        return Environment.UserName;
    }

    /// <summary>
    /// Gets the SAP client connection identifier.
    /// </summary>
    public string? GetClientId()
    {
        // TODO: Use SwissAddonFramework connection context
        return null;
    }

    /// <summary>
    /// Gets the SAP company database name.
    /// </summary>
    public string? GetCompanyDb()
    {
        // TODO: Use SAPbouiCOM.Application.Company.CompanyDB
        return null;
    }
}
