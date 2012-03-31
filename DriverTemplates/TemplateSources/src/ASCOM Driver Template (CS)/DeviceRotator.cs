// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceRotator
{
    Util util = new Util(); TraceLogger tl = new TraceLogger();

    #region IRotator Implementation
    public bool CanReverse
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Halt()
    {
        throw new ASCOM.PropertyNotImplementedException();
    }

    public bool IsMoving
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Move(float Position)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void MoveAbsolute(float Position)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public float Position
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool Reverse
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

    public float StepSize
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public float TargetPosition
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    #endregion

    //ENDOFINSERTEDFILE
}