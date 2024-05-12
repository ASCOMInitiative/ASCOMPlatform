// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;

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
                // IsSafe is required to deliver false when the device is not connected so there is no need to test whether or not the driver is connected.
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

    public IStateValueCollection DeviceState
    {
        get
        {
            try
            {
                CheckConnected("DeviceState");

                // Create an array list to hold the IStateValue entries
                List<IStateValue> deviceState = new List<IStateValue>();

                // Add one entry for each operational state, if possible
                try { deviceState.Add(new StateValue(nameof(ISafetyMonitorV3.IsSafe), IsSafe)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                // Return the overall device state
                return new StateValueCollection(deviceState);
            }
            catch (Exception ex)
            {
                LogMessage("DeviceState", $"Threw an exception: {ex.Message}\r\n{ex}");
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