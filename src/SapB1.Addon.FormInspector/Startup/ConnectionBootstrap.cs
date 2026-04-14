#if B1UP_SDK
using SwissAddonFramework.Hosting;
#endif

namespace SapB1.Addon.FormInspector.Startup;

/// <summary>
/// Manages the connection lifecycle to the SAP Business One UI API.
/// Uses SwissAddonFramework to establish and maintain the connection.
/// </summary>
public class ConnectionBootstrap
{
    /// <summary>Indicates whether the add-on is connected to the SAP client.</summary>
    public bool IsConnected { get; private set; }

    /// <summary>
    /// Establishes the connection to the SAP Business One client.
    /// </summary>
    public void Connect()
    {
        // TODO: Implement SwissAddonFramework connection
        // var connection = SwissAddonFramework.Hosting.Startup.Initialize();
        // IsConnected = connection != null;
        IsConnected = true;
    }

    /// <summary>
    /// Disconnects from the SAP Business One client gracefully.
    /// </summary>
    public void Disconnect()
    {
        // TODO: Implement SwissAddonFramework disconnection
        IsConnected = false;
    }
}
