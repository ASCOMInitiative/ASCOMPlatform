// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceFocuser
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

    private bool Connected // Dummy Connected method because this is referenced in the Link method
    {
        get { return false; }
        set { }
    }

    #region IFocuser Implementation

    private int focuserPosition = 0; // Class level variable to hold the current focuser position
    private const int focuserSteps = 10000;

    public bool Absolute
    {
        get
        {
            tl.LogMessage("Absolute Get", true.ToString());
            return true; // This is an absolute focuser
        }
    }

    public void Halt()
    {
        tl.LogMessage("Halt", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("Halt");
    }

    public bool IsMoving
    {
        get
        {
            tl.LogMessage("IsMoving Get", false.ToString());
            return false; // This focuser always moves instantaneously so no need for IsMoving ever to be True
        }
    }

    public bool Link
    {
        get
        {
            tl.LogMessage("Link Get", this.Connected.ToString());
            return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
        }
        set
        {
            tl.LogMessage("Link Set", value.ToString());
            this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
        }
    }

    public int MaxIncrement
    {
        get
        {
            tl.LogMessage("MaxIncrement Get", focuserSteps.ToString());
            return focuserSteps; // Maximum change in one move
        }
    }

    public int MaxStep
    {
        get
        {
            tl.LogMessage("MaxStep Get", focuserSteps.ToString());
            return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
        }
    }

    public void Move(int Position)
    {
        tl.LogMessage("Move", Position.ToString());
        focuserPosition = Position; // Set the focuser position
    }

    public int Position
    {
        get
        {
            return focuserPosition; // Return the focuser position
        }
    }

    public double StepSize
    {
        get
        {
            tl.LogMessage("StepSize Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("StepSize", false);
        }
    }

    public bool TempComp
    {
        get
        {
            tl.LogMessage("TempComp Get", false.ToString());
            return false;
        }
        set
        {
            tl.LogMessage("TempComp Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TempComp", false);
        }
    }

    public bool TempCompAvailable
    {
        get
        {
            tl.LogMessage("TempCompAvailable Get", false.ToString());
            return false; // Temperature compensation is not available in this driver
        }
    }

    public double Temperature
    {
        get
        {
            tl.LogMessage("Temperature Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Temperature", false);
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}