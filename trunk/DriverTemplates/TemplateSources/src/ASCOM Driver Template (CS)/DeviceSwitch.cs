// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;

class DeviceSwitch
{
    private TraceLogger tl = new TraceLogger();

    #region ISwitchV2 Implementation

    private  short numSwitch = 0;

    /// <summary>
    /// The number of switches managed by this driver
    /// </summary>
    public short MaxSwitch
    {
        get 
        { 
            tl.LogMessage("MaxSwitch Get", numSwitch.ToString());
            return this.numSwitch; 
        }
    }

    /// <summary>
    /// Return the name of switch n
    /// </summary>
    /// <param name="id">The switch number to return</param>
    /// <returns>
    /// The name of the switch
    /// </returns>
    public string GetSwitchName(short id)
    {
        Validate("GetSwitchName", id);
        tl.LogMessage("GetSwitchName", string.Format("GetSwitchName({0}) - not implemented", id));
        throw new MethodNotImplementedException("GetSwitchName");
        // or
        tl.LogMessage("GetSwitchName", string.Format("GetSwitchName({0}) - default Switch{0}", id));
        return "Switch" + id.ToString();
    }

    /// <summary>
    /// Sets a switch name to a specified value
    /// </summary>
    /// <param name="id">The number of the switch whose name is to be set</param>
    /// <param name="name">The name of the switch</param>
    public void SetSwitchName(short id, string name)
    {
        Validate("SetSwitchName", id);
        tl.LogMessage("SetSwitchName", string.Format("SetSwitchName({0}) = {1} - not implemented", id, name));
        throw new MethodNotImplementedException("SetSwitchName");
    }

    /// <summary>
    /// Gets the switch description.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public string GetSwitchDescription(short id)
    {
        Validate("GetSwitchDescription", id);
        tl.LogMessage("GetSwitchDescription", string.Format("GetSwitchDescription({0}) - not implemented", id));
        throw new MethodNotImplementedException("GetSwitchDescription");
    }

    /// <summary>
    /// Reports if the specified switch can be written to.
    /// This is false if the switch cannot be written to, for example a limit switch or a sensor.
    /// The default is true.
    /// </summary>
    /// <param name="id">The number of the switch whose write state is to be returned</param><returns>
    ///   <c>true</c> if the switch can be written to, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    public bool CanWrite(short id)
    {
        Validate("CanWrite", id);
        // default behavour is to report true
        tl.LogMessage("CanWrite", string.Format("CanWrite({0}) - default true", id));
        return true;
        // implementation should report the correct behaviour
        //tl.LogMessage("CanWrite", string.Format("CanWrite({0}) - not implemented", id));
        //throw new MethodNotImplementedException("CanWrite");
    }

    #region boolean switch members

    /// <summary>
    /// Return the state of switch n
    /// an analogue switch will return true if the value is closer to the maximum than the minimum, otherwise false
    /// </summary>
    /// <param name="id">The switch number to return</param>
    /// <returns>
    /// True or false
    /// </returns>
    public bool GetSwitch(short id)
    {
        Validate("GetSwitch", id);
        tl.LogMessage("GetSwitch", string.Format("GetSwitch({0}) - not implemented", id));
        throw new MethodNotImplementedException("GetSwitch");
    }

    /// <summary>
    /// Sets a switch to the specified state
    /// If the switch cannot be set then throws a MethodNotImplementedException.
    /// Setting an analogue switch to true will set it to its maximim value and
    /// setting it to false will set it to its minimum value.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    public void SetSwitch(short id, bool state)
    {
        Validate("SetSwitch", id);
        if (!CanWrite(id))
        {
            var str = string.Format("SetSwitch({0}) - Cannot Write", id);
            tl.LogMessage("SetSwitch", str);
            throw new MethodNotImplementedException(str);
        }
        tl.LogMessage("SetSwitch", string.Format("SetSwitch({0}) = {1} - not implemented", id, state));
        throw new MethodNotImplementedException("SetSwitch");
    }

    #endregion

    #region analogue members

    /// <summary>
    /// returns the maximum value for this switch
    /// boolean switches must return 1.0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double MaxSwitchValue(short id)
    {
        Validate("MaxSwitchValue", id);
        // boolean switch implementation:
        return 1;
        // or
        //tl.LogMessage("MaxSwitchValue", string.Format("MaxSwitchValue({0}) - not implemented", id));
        //throw new MethodNotImplementedException("MaxSwitchValue");
    }

    /// <summary>
    /// returns the minimum value for this switch
    /// boolean switches must return 0.0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double MinSwitchValue(short id)
    {
        Validate("MinSwitchValue", id);
        // boolean switch implementation:
        return 0;
        // or
        //tl.LogMessage("MinSwitchValue", string.Format("MinSwitchValue({0}) - not implemented", id));
        //throw new MethodNotImplementedException("MinSwitchValue");
    }

    /// <summary>
    /// returns the step size that this switch supports. This gives the difference between
    /// successive values of the switch.
    /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
    /// boolean switches must return 1.0, giving two states.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double SwitchStep(short id)
    {
        Validate("SwitchStep", id);
        // boolean switch implementation:
        return 1;
        // or
        //tl.LogMessage("SwitchStep", string.Format("SwitchStep({0}) - not implemented", id));
        //throw new MethodNotImplementedException("SwitchStep");
    }

    /// <summary>
    /// returns the analogue switch value for switch id
    /// boolean switches will return 1.0 or 0.0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double GetSwitchValue(short id)
    {
        Validate("GetSwitchValue", id);
        // boolean switch implementation, return 0 or 1
        return GetSwitch(id) ? 1 : 0;
        // or
        //tl.LogMessage("GetSwitchValue", string.Format("GetSwitchValue({0}) - not implemented", id));
        //throw new MethodNotImplementedException("GetSwitchValue");
    }

    /// <summary>
    /// set the analogue value for this switch.
    /// If the switch cannot be set then throws a MethodNotImplementedException.
    /// If the value is not between the maximum and minimum then throws an InvalidValueException
    /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetSwitchValue(short id, double value)
    {
        Validate("SetSwitchValue", id, value);
        if (!CanWrite(id))
        {
            tl.LogMessage("SetSwitchValue", string.Format("SetSwitchValue({0}) - Cannot write", id));
            throw new ASCOM.MethodNotImplementedException(string.Format("SetSwitchValue({0}) - Cannot write", id));
        }
        tl.LogMessage("SetSwitchValue", string.Format("SetSwitchValue({0}) = {1} - not implemented", id, value));
        throw new MethodNotImplementedException("SetSwitchValue");
    }

    #endregion
    #endregion

    #region private methods

    /// <summary>
    /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="id">The id.</param>
    private void Validate(string message,short id)
    {
        if (id < 0 || id >= numSwitch)
        {
            tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, numSwitch -1));
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
    private void Validate(string message, short id, double value)
    {
        Validate(message, id);
        var min = MinSwitchValue(id);
        var max = MaxSwitchValue(id);
        if (value < min || value > max)
        {
            tl.LogMessage(message, string.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value,  min, max));
 	        throw new InvalidValueException(message, value.ToString(), string.Format("Switch({0}) range {1} to {2}", id, min, max));
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}