#if B1UP_SDK
using SwissAddonFramework.Hosting;
#endif
using System;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Manages the connection lifecycle to the SAP Business One UI API.
/// Uses SwissAddonFramework or direct SAP UI API to establish and maintain the connection.
/// </summary>
public class ConnectionBootstrap
{
    /// <summary>Indicates whether the add-on is connected to the SAP client.</summary>
    public bool IsConnected => SapContext.IsInitialized;

    /// <summary>
    /// Establishes the connection to the SAP Business One client.
    /// </summary>
    public void Connect()
    {
#if B1UP_SDK
        try
        {
            var connection = SwissAddonFramework.Hosting.Startup.Initialize();
            if (connection != null)
            {
                SapContext.Initialize(connection.Application);
            }
        }
        catch (Exception)
        {
            // SwissAddonFramework connection failed
        }
#endif
#if SAP_UI_SDK
        if (!SapContext.IsInitialized)
        {
            try
            {
                var sboGuiApi = new SAPbouiCOM.SboGuiApi();
                sboGuiApi.Connect(GetConnectionString());
                var application = sboGuiApi.GetApplication();
                SapContext.Initialize(application);
            }
            catch (Exception)
            {
                // Direct SAP UI API connection failed
            }
        }
#endif
        // Without SDK DLLs, this is a no-op for testing
    }

    /// <summary>
    /// Disconnects from the SAP Business One client gracefully.
    /// </summary>
    public void Disconnect()
    {
        SapContext.Reset();
    }

#if SAP_UI_SDK
    /// <summary>
    /// Builds the connection string for the SAP UI API.
    /// </summary>
    private static string GetConnectionString()
    {
        return string.Empty;
    }
#endif
}
