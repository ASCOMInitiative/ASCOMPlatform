// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;

class DeviceFocuser
{
    #region IFocuser Implementation

    /// <summary>
    /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
    /// </summary>
    public bool Absolute
    {
        get
        {
            try
            {
                CheckConnected("Absolute");
                bool absolute = FocuserHardware.Absolute;
                LogMessage("Absolute", absolute.ToString());
                return absolute;
            }
            catch (Exception ex)
            {
                LogMessage("Absolute", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

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
                try { deviceState.Add(new StateValue(nameof(IFocuserV4.IsMoving), IsMoving)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IFocuserV4.Position), Position)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IFocuserV4.Temperature), Temperature)); } catch { }
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
    /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
    /// </summary>
    public void Halt()
    {
        try
        {
            CheckConnected("Halt");
            LogMessage("Halt", $"Calling method.");
            FocuserHardware.Halt();
            LogMessage("Halt", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Halt", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
    /// </summary>
    public bool IsMoving
    {
        get
        {
            try
            {
                CheckConnected("IsMoving");
                bool isMoving = FocuserHardware.IsMoving;
                LogMessage("IsMoving", isMoving.ToString());
                return isMoving;
            }
            catch (Exception ex)
            {
                LogMessage("IsMoving", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// State of the connection to the focuser.
    /// </summary>
    public bool Link
    {
        get
        {
            try
            {
                CheckConnected("Link Get");
                bool link = Connected;
                LogMessage("Link Get", link.ToString());
                return link;
            }
            catch (Exception ex)
            {
                LogMessage("Link Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        set
        {
            try
            {
                CheckConnected("Link Set");
                LogMessage("Link Set", value.ToString());
                Connected = value;
            }
            catch (Exception ex)
            {
                LogMessage("Link Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Maximum increment size allowed by the focuser;
    /// i.e. the maximum number of steps allowed in one move operation.
    /// </summary>
    public int MaxIncrement
    {
        get
        {
            try
            {
                CheckConnected("MaxIncrement");
                int maxIncrement = FocuserHardware.MaxIncrement;
                LogMessage("MaxIncrement", maxIncrement.ToString());
                return maxIncrement;
            }
            catch (Exception ex)
            {
                LogMessage("MaxIncrement", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Maximum step position permitted.
    /// </summary>
    public int MaxStep
    {
        get
        {
            try
            {
                CheckConnected("MaxStep");
                int maxStep = FocuserHardware.MaxStep;
                LogMessage("MaxStep", maxStep.ToString());
                return maxStep;
            }
            catch (Exception ex)
            {
                LogMessage("MaxStep", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
    /// </summary>
    /// <param name="position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
    public void Move(int position)
    {
        try
        {
            CheckConnected("Move");
            LogMessage("Move", $"Calling method.");
            FocuserHardware.Move(position);
            LogMessage("Move", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Move", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Current focuser position, in steps.
    /// </summary>
    public int Position
    {
        get
        {
            try
            {
                CheckConnected("Position");
                int position = FocuserHardware.Position;
                LogMessage("Position", position.ToString());
                return position;
            }
            catch (Exception ex)
            {
                LogMessage("Position", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }


    /// <summary>
    /// Step size (microns) for the focuser.
    /// </summary>
    public double StepSize
    {
        get
        {
            try
            {
                CheckConnected("StepSize");
                double stepSize = FocuserHardware.StepSize;
                LogMessage("StepSize", stepSize.ToString());
                return stepSize;
            }
            catch (Exception ex)
            {
                LogMessage("StepSize", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The state of temperature compensation mode (if available), else always False.
    /// </summary>
    public bool TempComp
    {
        get
        {
            try
            {
                CheckConnected("TempComp Get");
                bool tempComp = FocuserHardware.TempComp;
                LogMessage("TempComp Get", tempComp.ToString());
                return tempComp;
            }
            catch (Exception ex)
            {
                LogMessage("TempComp Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        set
        {
            try
            {
                CheckConnected("TempComp Set");
                LogMessage("TempComp Set", value.ToString());
                FocuserHardware.TempComp = value;
            }
            catch (Exception ex)
            {
                LogMessage("TempComp Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// True if focuser has temperature compensation available.
    /// </summary>
    public bool TempCompAvailable
    {
        get
        {
            try
            {
                CheckConnected("TempCompAvailable");
                bool tempCompAvailable = FocuserHardware.TempCompAvailable;
                LogMessage("TempCompAvailable", tempCompAvailable.ToString());
                return tempCompAvailable;
            }
            catch (Exception ex)
            {
                LogMessage("TempCompAvailable", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Current ambient temperature in degrees Celsius as measured by the focuser.
    /// </summary>
    public double Temperature
    {
        get
        {
            try
            {
                CheckConnected("Temperature");
                double temperature = FocuserHardware.Temperature;
                LogMessage("Temperature", temperature.ToString());
                return temperature;
            }
            catch (Exception ex)
            {
                LogMessage("Temperature", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy Connected method because this is referenced in the Link method
    /// </summary>
    private bool Connected
    {
        get { return false; }
        set { }
    }

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