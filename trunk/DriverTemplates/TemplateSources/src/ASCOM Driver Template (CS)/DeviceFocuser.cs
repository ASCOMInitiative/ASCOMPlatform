// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceFocuser
{
    Util util = new Util(); TraceLogger tl = new TraceLogger();

    #region IFocuser Implementation
    public bool Absolute
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Halt()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public bool IsMoving
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool Link
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
    }

    public int MaxIncrement
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public int MaxStep
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Move(int Position)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public int Position
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double StepSize
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool TempComp
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
    }

    public bool TempCompAvailable
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double Temperature
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    #endregion

    //ENDOFINSERTEDFILE
}