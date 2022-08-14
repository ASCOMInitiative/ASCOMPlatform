// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceSafetyMonitor
{
    #region ISafetyMonitor Implementation

    /// <summary>
    /// Indicates whether the monitored state is safe for use.
    /// </summary>
    /// <value>True if the state is safe, False if it is unsafe.</value>
    public bool IsSafe
    {
        get
        {
            try
            {
                CheckConnected("IsSafe");
                bool isSafe = SafetyMonitorHardware.IsSafe;
                LogMessage("IsSafe", isSafe.ToString());
                return isSafe;
            }
            catch (Exception ex)
            {
                LogMessage("IsSafe", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }

    /// <summary>
    /// Dummy CheckConnected class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    /// <param name="message"></param>
    private void CheckConnected(string message)
    {
    }
}