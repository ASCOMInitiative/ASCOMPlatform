// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

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
            bool absolute = FocuserHardware.Absolute;
            LogMessage("Absolute Get", absolute.ToString());
            return absolute;
        }
    }

    /// <summary>
    /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
    /// </summary>
    public void Halt()
    {
        LogMessage("Halt", $"Calling method.");
        FocuserHardware.Halt();
        LogMessage("Halt", $"Completed.");
    }

    /// <summary>
    /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
    /// </summary>
    public bool IsMoving
    {
        get
        {
            bool isMoving = FocuserHardware.IsMoving;
            LogMessage("IsMoving Get", isMoving.ToString());
            return isMoving;
        }
    }

    /// <summary>
    /// State of the connection to the focuser.
    /// </summary>
    public bool Link
    {
        get
        {
            bool link = FocuserHardware.Link;
            LogMessage("Link Get", link.ToString());
            return link;
        }
        set
        {
            LogMessage("Link Set", value.ToString());
            FocuserHardware.Link = value;
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
            int maxIncrement = FocuserHardware.MaxIncrement;
            LogMessage("MaxIncrement Get", maxIncrement.ToString());
            return maxIncrement;
        }
    }

    /// <summary>
    /// Maximum step position permitted.
    /// </summary>
    public int MaxStep
    {
        get
        {
            int maxStep = FocuserHardware.MaxStep;
            LogMessage("MaxStep Get", maxStep.ToString());
            return maxStep;
        }
    }

    /// <summary>
    /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
    /// </summary>
    /// <param name="position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
    public void Move(int position)
    {
        LogMessage("Move", $"Calling method.");
        FocuserHardware.Move(position);
        LogMessage("Move", $"Completed.");
    }

    /// <summary>
    /// Current focuser position, in steps.
    /// </summary>
    public int Position
    {
        get
        {
            int position = FocuserHardware.Position;
            LogMessage("Position Get", position.ToString());
            return position;
        }
    }


    /// <summary>
    /// Step size (microns) for the focuser.
    /// </summary>
    public double StepSize
    {
        get
        {
            double stepSize = FocuserHardware.StepSize;
            LogMessage("StepSize Get", stepSize.ToString());
            return stepSize;
        }
    }

    /// <summary>
    /// The state of temperature compensation mode (if available), else always False.
    /// </summary>
    public bool TempComp
    {
        get
        {
            bool tempComp = FocuserHardware.TempComp;
            LogMessage("TempComp Get", tempComp.ToString());
            return tempComp;
        }
        set
        {
            LogMessage("TempComp Set", value.ToString());
            FocuserHardware.TempComp = value;
        }
    }

    /// <summary>
    /// True if focuser has temperature compensation available.
    /// </summary>
    public bool TempCompAvailable
    {
        get
        {
            bool tempCompAvailable = FocuserHardware.TempCompAvailable;
            LogMessage("TempCompAvailable Get", tempCompAvailable.ToString());
            return tempCompAvailable;
        }
    }

    /// <summary>
    /// Current ambient temperature in degrees Celsius as measured by the focuser.
    /// </summary>
    public double Temperature
    {
        get
        {
            double temperature = FocuserHardware.Temperature;
            LogMessage("Temperature Get", temperature.ToString());
            return temperature;
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

}