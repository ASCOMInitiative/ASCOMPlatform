// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;

static class SwitchHardware
{
    #region ISwitchV2 Implementation

    private static short numSwitch = 0;

    /// <summary>
    /// The number of switches managed by this driver
    /// </summary>
    /// <returns>The number of devices managed by this driver.</returns>
    internal static short MaxSwitch
    {
        get
        {
            LogMessage("MaxSwitch Get", numSwitch.ToString());
            return numSwitch;
        }
    }

    /// <summary>
    /// Return the name of switch device n.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The name of the device</returns>
    internal static string GetSwitchName(short id)
    {
        Validate("GetSwitchName", id);
        LogMessage("GetSwitchName", $"GetSwitchName({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchName");
    }

    /// <summary>
    /// Set a switch device name to a specified value.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="name">The name of the device</param>
    internal static void SetSwitchName(short id, string name)
    {
        Validate("SetSwitchName", id);
        LogMessage("SetSwitchName", $"SetSwitchName({id}) = {name} - not implemented");
        throw new MethodNotImplementedException("SetSwitchName");
    }

    /// <summary>
    /// Gets the description of the specified switch device. This is to allow a fuller description of
    /// the device to be returned, for example for a tool tip.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>
    /// String giving the device description.
    /// </returns>
    internal static string GetSwitchDescription(short id)
    {
        Validate("GetSwitchDescription", id);
        LogMessage("GetSwitchDescription", $"GetSwitchDescription({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchDescription");
    }

    /// <summary>
    /// Reports if the specified switch device can be written to, default true.
    /// This is false if the device cannot be written to, for example a limit switch or a sensor.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>
    /// <c>true</c> if the device can be written to, otherwise <c>false</c>.
    /// </returns>
    internal static bool CanWrite(short id)
    {
        bool writable = true;
        Validate("CanWrite", id);
        // default behavour is to report true
        LogMessage("CanWrite", $"CanWrite({id}): {writable}");
        return true;
    }

    #region Boolean switch members

    /// <summary>
    /// Return the state of switch device id as a boolean
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>True or false</returns>
    internal static bool GetSwitch(short id)
    {
        Validate("GetSwitch", id);
        LogMessage("GetSwitch", $"GetSwitch({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitch");
    }

    /// <summary>
    /// Sets a switch controller device to the specified state, true or false.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="state">The required control state</param>
    internal static void SetSwitch(short id, bool state)
    {
        Validate("SetSwitch", id);
        if (!CanWrite(id))
        {
            var str = $"SetSwitch({id}) - Cannot Write";
            LogMessage("SetSwitch", str);
            throw new MethodNotImplementedException(str);
        }
        LogMessage("SetSwitch", $"SetSwitch({id}) = {state} - not implemented");
        throw new MethodNotImplementedException("SetSwitch");
    }

    #endregion

    #region Analogue members

    /// <summary>
    /// Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The maximum value to which this device can be set or which a read only sensor will return.</returns>
    internal static double MaxSwitchValue(short id)
    {
        Validate("MaxSwitchValue", id);
        LogMessage("MaxSwitchValue", $"MaxSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("MaxSwitchValue");
    }

    /// <summary>
    /// Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The minimum value to which this device can be set or which a read only sensor will return.</returns>
    internal static double MinSwitchValue(short id)
    {
        Validate("MinSwitchValue", id);
        LogMessage("MinSwitchValue", $"MinSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("MinSwitchValue");
    }

    /// <summary>
    /// Returns the step size that this device supports (the difference between successive values of the device).
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The step size for this device.</returns>
    internal static double SwitchStep(short id)
    {
        Validate("SwitchStep", id);
        LogMessage("SwitchStep", $"SwitchStep({id}) - not implemented");
        throw new MethodNotImplementedException("SwitchStep");
    }

    /// <summary>
    /// Returns the value for switch device id as a double
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <returns>The value for this switch, this is expected to be between <see cref="MinSwitchValue"/> and
    /// <see cref="MaxSwitchValue"/>.</returns>
    internal static double GetSwitchValue(short id)
    {
        Validate("GetSwitchValue", id);
        LogMessage("GetSwitchValue", $"GetSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchValue");
    }

    /// <summary>
    /// Set the value for this device as a double.
    /// </summary>
    /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
    /// <param name="value">The value to be set, between <see cref="MinSwitchValue"/> and <see cref="MaxSwitchValue"/></param>
    internal static void SetSwitchValue(short id, double value)
    {
        Validate("SetSwitchValue", id, value);
        if (!CanWrite(id))
        {
            LogMessage("SetSwitchValue", $"SetSwitchValue({id}) - Cannot write");
            throw new ASCOM.MethodNotImplementedException($"SetSwitchValue({id}) - Cannot write");
        }
        LogMessage("SetSwitchValue", $"SetSwitchValue({id}) = {value} - not implemented");
        throw new MethodNotImplementedException("SetSwitchValue");
    }

    #endregion

    #endregion

    #region Private methods

    /// <summary>
    /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="id">The id.</param>
    private static void Validate(string message, short id)
    {
        if (id < 0 || id >= numSwitch)
        {
            LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, numSwitch - 1));
            throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", numSwitch - 1));
        }
    }

    /// <summary>
    /// Checks that the switch id and value are in range and throws an
    /// InvalidValueException if they are not.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="id">The id.</param>
    /// <param name="value">The value.</param>
    private static void Validate(string message, short id, double value)
    {
        Validate(message, id);
        var min = MinSwitchValue(id);
        var max = MaxSwitchValue(id);
        if (value < min || value > max)
        {
            LogMessage(message, string.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max));
            throw new InvalidValueException(message, value.ToString(), string.Format("Switch({0}) range {1} to {2}", id, min, max));
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

}