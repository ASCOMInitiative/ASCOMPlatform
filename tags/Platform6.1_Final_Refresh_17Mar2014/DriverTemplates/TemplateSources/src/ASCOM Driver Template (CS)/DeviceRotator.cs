// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Astrometry.AstroUtils;

class DeviceRotator
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();
    AstroUtils astroUtilities = new AstroUtils();

    #region IRotator Implementation

    private float rotatorPosition = 0; // Absolute position angle of the rotator 

    public bool CanReverse
    {
        get
        {
            tl.LogMessage("CanReverse Get", false.ToString());
            return false;
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
            tl.LogMessage("IsMoving Get", false.ToString()); // This rotator has instantaneous movement
            return false;
        }
    }

    public void Move(float Position)
    {
        tl.LogMessage("Move", Position.ToString()); // Move by this amount
        rotatorPosition += Position;
        rotatorPosition = (float)astroUtilities.Range(rotatorPosition, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }

    public void MoveAbsolute(float Position)
    {
        tl.LogMessage("MoveAbsolute", Position.ToString()); // Move to this position
        rotatorPosition = Position;
        rotatorPosition = (float)astroUtilities.Range(rotatorPosition, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }

    public float Position
    {
        get
        {
            tl.LogMessage("Position Get", rotatorPosition.ToString()); // This rotator has instantaneous movement
            return rotatorPosition;
        }
    }

    public bool Reverse
    {
        get
        {
            tl.LogMessage("Reverse Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Reverse", false);
        }
        set
        {
            tl.LogMessage("Reverse Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Reverse", true);
        }
    }

    public float StepSize
    {
        get
        {
            tl.LogMessage("StepSize Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("StepSize", false);
        }
    }

    public float TargetPosition
    {
        get
        {
            tl.LogMessage("TargetPosition Get", rotatorPosition.ToString()); // This rotator has instantaneous movement
            return rotatorPosition;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}