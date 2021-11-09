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
        tl.LogMessage("GetSwitchName", $"GetSwitchName({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchName");
    }

    /// <summary>
    /// Sets a switch name to a specified value
    /// </summary>
    /// <param name="id">The number of the switch whose name is to be set</param>
    /// <param name="name">The name of the switch</param>
    public void SetSwitchName(short id, string name)
    {
        Validate("SetSwitchName", id);
        tl.LogMessage("SetSwitchName", $"SetSwitchName({id}) = {name} - not implemented");
        throw new MethodNotImplementedException("SetSwitchName");
    }

    /// <summary>
    /// Gets the description of the specified switch. This is to allow a fuller description of the switch to be returned, for example for a tool tip.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    /// <returns></returns>
    public string GetSwitchDescription(short id)
    {
        Validate("GetSwitchDescription", id);
        tl.LogMessage("GetSwitchDescription", $"GetSwitchDescription({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchDescription");
    }

    /// <summary>
    /// Reports whether the specified switch can be written to.
    /// Returns false if the switch cannot be written to, for example a limit switch or a sensor.
    /// </summary>
    /// <param name="id">The number of the switch whose write state is to be returned</param>
    /// <returns>
    /// <c>true</c> if the switch can be written to, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    public bool CanWrite(short id)
    {
        bool writable = true;
        Validate("CanWrite", id);
        // default behavour is to report true
        tl.LogMessage("CanWrite", $"CanWrite({id}): {writable}");
        return true;
    }

    #region Boolean switch members

    /// <summary>
    /// Return the state of switch n.
    /// A multi-value switch must throw a not implemented exception
    /// </summary>
    /// <param name="id">The switch number to return</param>
    /// <returns>
    /// True or false
    /// </returns>
    public bool GetSwitch(short id)
    {
        Validate("GetSwitch", id);
        tl.LogMessage("GetSwitch", $"GetSwitch({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitch");
    }

    /// <summary>
    /// Sets a switch to the specified state
    /// If the switch cannot be set then throws a MethodNotImplementedException.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    public void SetSwitch(short id, bool state)
    {
        Validate("SetSwitch", id);
        if (!CanWrite(id))
        {
            var str = $"SetSwitch({id}) - Cannot Write";
            tl.LogMessage("SetSwitch", str);
            throw new MethodNotImplementedException(str);
        }
        tl.LogMessage("SetSwitch", $"SetSwitch({id}) = {state} - not implemented");
        throw new MethodNotImplementedException("SetSwitch");
    }

    #endregion

    #region Analogue members

    /// <summary>
    /// Returns the maximum value for this switch
    /// Boolean switches must return 1.0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double MaxSwitchValue(short id)
    {
        Validate("MaxSwitchValue", id);
        tl.LogMessage("MaxSwitchValue", $"MaxSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("MaxSwitchValue");
    }

    /// <summary>
    /// Returns the minimum value for this switch
    /// Boolean switches must return 0.0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double MinSwitchValue(short id)
    {
        Validate("MinSwitchValue", id);
        tl.LogMessage("MinSwitchValue", $"MinSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("MinSwitchValue");
    }

    /// <summary>
    /// Returns the step size that this switch supports. This gives the difference between successive values of the switch.
    /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
    /// boolean switches must return 1.0, giving two states.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double SwitchStep(short id)
    {
        Validate("SwitchStep", id);
        tl.LogMessage("SwitchStep", $"SwitchStep({id}) - not implemented");
        throw new MethodNotImplementedException("SwitchStep");
    }

    /// <summary>
    /// Returns the analogue switch value for switch id.
    /// Boolean switches must return either 0.0 (false) or 1.0 (true).
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public double GetSwitchValue(short id)
    {
        Validate("GetSwitchValue", id);
        tl.LogMessage("GetSwitchValue", $"GetSwitchValue({id}) - not implemented");
        throw new MethodNotImplementedException("GetSwitchValue");
    }

    /// <summary>
    /// Set the analogue value for this switch.
    /// If the switch cannot be set then throws a MethodNotImplementedException.
    /// If the value is not between the maximum and minimum then throws an InvalidValueException
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetSwitchValue(short id, double value)
    {
        Validate("SetSwitchValue", id, value);
        if (!CanWrite(id))
        {
            tl.LogMessage("SetSwitchValue", $"SetSwitchValue({id}) - Cannot write");
            throw new ASCOM.MethodNotImplementedException($"SetSwitchValue({id}) - Cannot write");
        }
        tl.LogMessage("SetSwitchValue", $"SetSwitchValue({id}) = {value} - not implemented");
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

    /// <summary>
    /// Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
    /// Boolean switches must have 2 states and multi-value switches more than 2.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="id"></param>
    /// <param name="expectBoolean"></param>
    //private void Validate(string message, short id, bool expectBoolean)
    //{
    //    Validate(message, id);
    //    var ns = (int)(((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1);
    //    if ((expectBoolean && ns != 2) || (!expectBoolean && ns <= 2))
    //    {
    //        tl.LogMessage(message, string.Format("Switch {0} has the wriong number of states", id, ns));
    //        throw new MethodNotImplementedException(string.Format("{0}({1})", message, id));
    //    }
    //}

    #endregion

    //ENDOFINSERTEDFILE
}