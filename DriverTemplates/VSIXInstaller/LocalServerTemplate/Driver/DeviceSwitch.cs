// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.AxHost;

class DeviceSwitch
{
    #region ISwitch Implementation

    /// <summary>
    /// Return the device's state in one call
    /// </summary>
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
                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"GetSwitch{i}", GetSwitch(i))); } catch { }
                }

                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"GetSwitchValue{i}", GetSwitchValue(i))); } catch { }
                }

                for (short i = 0; i < MaxSwitch; i++)
                {
                    try { deviceState.Add(new StateValue($"StateChangeComplete{i}", StateChangeComplete(i))); } catch { }
                }

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

    /// <summary>
    /// The number of switches managed by this driver
    /// </summary>
    /// <returns>The number of devices managed by this driver.</returns>
    public short MaxSwitch
    {
        get
        {
            try
            {
                CheckConnected("MaxSwitch");
                short maxSwitch = SwitchHardware.MaxSwitch;
                LogMessage("MaxSwitch", maxSwitch.ToString());
                return maxSwitch;
            }
            catch (Exception ex)
            {
                LogMessage("MaxSwitch", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Return the name of switch device n.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The name of the device</returns>
    public string GetSwitchName(short id)
    {
        try
        {
            CheckConnected("GetSwitchName");
            LogMessage("GetSwitchName", $"Calling method.");
            string switchName = SwitchHardware.GetSwitchName(id);
            LogMessage("GetSwitchName", switchName.ToString());
            return switchName;
        }
        catch (Exception ex)
        {
            LogMessage("GetSwitchName", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Set a switch device name to a specified value.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="name">The name of the device</param>
    public void SetSwitchName(short id, string name)
    {
        try
        {
            CheckConnected("SetSwitchName");
            LogMessage("SetSwitchName", $"Calling method.");
            SwitchHardware.SetSwitchName(id, name);
            LogMessage("SetSwitchName", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetSwitchName", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Gets the description of the specified switch device. This is to allow a fuller description of
    /// the device to be returned, for example for a tool tip.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>
    /// String giving the device description.
    /// </returns>
    public string GetSwitchDescription(short id)
    {
        try
        {
            CheckConnected("GetSwitchDescription");
            LogMessage("GetSwitchDescription", $"Calling method.");
            string switchDescription = SwitchHardware.GetSwitchDescription(id);
            LogMessage("GetSwitchDescription", switchDescription.ToString());
            return switchDescription;
        }
        catch (Exception ex)
        {
            LogMessage("GetSwitchDescription", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Reports if the specified switch device can be written to, default true.
    /// This is false if the device cannot be written to, for example a limit switch or a sensor.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>
    /// <c>true</c> if the device can be written to, otherwise <c>false</c>.
    /// </returns>
    public bool CanWrite(short id)
    {
        try
        {
            CheckConnected("CanWrite");
            LogMessage("CanWrite", $"Calling method.");
            bool canWrite = SwitchHardware.CanWrite(id);
            LogMessage("CanWrite", canWrite.ToString());
            return canWrite;
        }
        catch (Exception ex)
        {
            LogMessage("CanWrite", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #region Boolean members

    /// <summary>
    /// Return the state of switch device id as a boolean
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>True or false</returns>
    public bool GetSwitch(short id)
    {
        try
        {
            CheckConnected("GetSwitch");
            LogMessage("GetSwitch", $"Calling method.");
            bool getSwitch = SwitchHardware.GetSwitch(id);
            LogMessage("GetSwitch", getSwitch.ToString());
            return getSwitch;
        }
        catch (Exception ex)
        {
            LogMessage("GetSwitch", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Sets a switch controller device to the specified state, true or false.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="state">The required control state</param>
    public void SetSwitch(short id, bool state)
    {
        try
        {
            CheckConnected("SetSwitch");
            LogMessage("SetSwitch", $"Calling method.");
            SwitchHardware.SetSwitch(id, state);
            LogMessage("SetSwitch", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetSwitch", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #endregion

    #region Analogue members

    /// <summary>
    /// Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The maximum value to which this device can be set or which a read only sensor will return.</returns>
    public double MaxSwitchValue(short id)
    {
        try
        {
            CheckConnected("MaxSwitchValue");
            LogMessage("MaxSwitchValue", $"Calling method.");
            double maxSwitchValue = SwitchHardware.MaxSwitchValue(id);
            LogMessage("MaxSwitchValue", maxSwitchValue.ToString());
            return maxSwitchValue;
        }
        catch (Exception ex)
        {
            LogMessage("MaxSwitchValue", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The minimum value to which this device can be set or which a read only sensor will return.</returns>
    public double MinSwitchValue(short id)
    {
        try
        {
            CheckConnected("MinSwitchValue");
            LogMessage("MinSwitchValue", $"Calling method.");
            double maxSwitchValue = SwitchHardware.MinSwitchValue(id);
            LogMessage("MinSwitchValue", maxSwitchValue.ToString());
            return maxSwitchValue;
        }
        catch (Exception ex)
        {
            LogMessage("MinSwitchValue", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Returns the step size that this device supports (the difference between successive values of the device).
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The step size for this device.</returns>
    public double SwitchStep(short id)
    {
        try
        {
            CheckConnected("SwitchStep");
            LogMessage("SwitchStep", $"Calling method.");
            double switchStep = SwitchHardware.SwitchStep(id);
            LogMessage("SwitchStep", switchStep.ToString());
            return switchStep;
        }
        catch (Exception ex)
        {
            LogMessage("SwitchStep", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Returns the value for switch device id as a double
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The value for this switch, this is expected to be between <see cref="MinSwitchValue"/> and
    /// <see cref="MaxSwitchValue"/>.</returns>
    public double GetSwitchValue(short id)
    {
        try
        {
            CheckConnected("GetSwitchValue");
            LogMessage("GetSwitchValue", $"Calling method.");
            double switchValue = SwitchHardware.GetSwitchValue(id);
            LogMessage("GetSwitchValue", switchValue.ToString());
            return switchValue;
        }
        catch (Exception ex)
        {
            LogMessage("GetSwitchValue", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Set the value for this device as a double.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="value">The value to be set, between <see cref="MinSwitchValue"/> and <see cref="MaxSwitchValue"/></param>
    public void SetSwitchValue(short id, double value)
    {
        try
        {
            CheckConnected("SetSwitchValue");
            LogMessage("SetSwitchValue", $"Calling method.");
            SwitchHardware.SetSwitchValue(id, value);
            LogMessage("SetSwitchValue", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetSwitchValue", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #endregion

    #region Async members

    /// <summary>
    /// Set a boolean switch's state asynchronously
    /// </summary>
    /// <exception cref="MethodNotImplementedException">When CanAsync(id) is false.</exception>
    /// <param name="id">Switch number.</param>
    /// <param name="state">New boolean state.</param>
    /// <remarks>
    /// <p style="color:red"><b>This is an optional method and can throw a <see cref="MethodNotImplementedException"/> when <see cref="CanAsync(short)"/> is <see langword="false"/>.</b></p>
    /// </remarks>
    public void SetAsync(short id, bool state)
    {
        try
        {
            CheckConnected("SetAsync");
            LogMessage("SetAsync", $"Calling method.");
            SwitchHardware.SetAsync(id, state);
            LogMessage("SetAsync", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetAsync", $"Threw an exception: {ex.Message}\r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Set a switch's value asynchronously
    /// </summary>
    /// <param name="id">Switch number.</param>
    /// <param name="value">New double value.</param>
    /// <p style="color:red"><b>This is an optional method and can throw a <see cref="MethodNotImplementedException"/> when <see cref="CanAsync(short)"/> is <see langword="false"/>.</b></p>
    /// <exception cref="MethodNotImplementedException">When CanAsync(id) is false.</exception>
    /// <remarks>
    /// <p style="color:red"><b>This is an optional method and can throw a <see cref="MethodNotImplementedException"/> when <see cref="CanAsync(short)"/> is <see langword="false"/>.</b></p>
    /// </remarks>
    public void SetAsyncValue(short id, double value)
    {
        try
        {
            CheckConnected("SetAsyncValue");
            LogMessage("SetAsyncValue", $"Calling method.");
            SwitchHardware.SetAsyncValue(id, value);
            LogMessage("SetAsyncValue", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetAsyncValue", $"Threw an exception: {ex.Message}\r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Flag indicating whether this switch can operate asynchronously.
    /// </summary>
    /// <param name="id">Switch number.</param>
    /// <returns>True if the switch can operate asynchronously.</returns>
    /// <exception cref="MethodNotImplementedException">When CanAsync(id) is false.</exception>
    /// <remarks>
    /// <p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p>
    /// </remarks>
    public bool CanAsync(short id)
    {
        try
        {
            CheckConnected("CanAsync");
            LogMessage("CanAsync", $"Calling method.");
            bool canAsync = SwitchHardware.CanAsync(id);
            LogMessage("CanAsync", canAsync.ToString());
            return canAsync;
        }
        catch (Exception ex)
        {
            LogMessage("CanAsync", $"Threw an exception: {ex.Message}\r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Completion variable for asynchronous switch state change operations.
    /// </summary>
    /// <param name="id">Switch number.</param>
    /// <exception cref="OperationCancelledException">When an in-progress operation is cancelled by the <see cref="CancelAsync(short)"/> method.</exception>
    /// <returns>False while an asynchronous operation is underway and true when it has completed.</returns>
    /// <remarks>
    /// <p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p>
    /// </remarks>
    public bool StateChangeComplete(short id)
    {
        try
        {
            CheckConnected("StateChangeComplete");
            LogMessage("StateChangeComplete", $"Calling method.");
            bool stateChangeComplete = SwitchHardware.StateChangeComplete(id);
            LogMessage("StateChangeComplete", stateChangeComplete.ToString());
            return stateChangeComplete;
        }
        catch (Exception ex)
        {
            LogMessage("StateChangeComplete", $"Threw an exception: {ex.Message}\r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Cancels an in-progress asynchronous state change operation.
    /// </summary>
    /// <param name="id">Switch number.</param>
    /// <exception cref="MethodNotImplementedException">When it is not possible to cancel an asynchronous change.</exception>
    /// <remarks>
    /// <p style="color:red"><b>This is an optional method and can throw a <see cref="MethodNotImplementedException"/>.</b></p>
    /// This method must be implemented if it is possible for the device to cancel an asynchronous state change operation, otherwise it must throw a <see cref="MethodNotImplementedException"/>.
    /// </remarks>
    public void CancelAsync(short id)
    {
        try
        {
            CheckConnected("CancelAsync");
            LogMessage("CancelAsync", $"Calling method...");
            SwitchHardware.CancelAsync(id);
            LogMessage("CancelAsync", "Returned from method OK.");
        }
        catch (Exception ex)
        {
            LogMessage("CancelAsync", $"Threw an exception: {ex.Message}\r\n{ex}");
            throw;
        }
    }

    #endregion

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