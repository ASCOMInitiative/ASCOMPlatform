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

    private float rotatorPosition = 0; // Synced or mechanical position angle of the rotator
    private float mechanicalPosition = 0; // Mechanical position angle of the rotator

	/// <summary>
	/// Indicates whether the Rotator supports the <see cref="Reverse" /> method.
	/// </summary>
	/// <returns>
	/// True if the Rotator supports the <see cref="Reverse" /> method.
	/// </returns>
	public bool CanReverse
    {
        get
        {
            tl.LogMessage("CanReverse Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
	/// </summary>
	public void Halt()
    {
        tl.LogMessage("Halt", "Not implemented");
        throw new MethodNotImplementedException("Halt");
    }

	/// <summary>
	/// Indicates whether the rotator is currently moving
	/// </summary>
	/// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
	public bool IsMoving
    {
        get
        {
            tl.LogMessage("IsMoving Get", false.ToString()); // This rotator has instantaneous movement
            return false;
        }
    }

	/// <summary>
	/// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
	/// </summary>
	/// <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
	public void Move(float Position)
    {
        tl.LogMessage("Move", Position.ToString()); // Move by this amount
        rotatorPosition += Position;
        rotatorPosition = (float)astroUtilities.Range(rotatorPosition, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }


	/// <summary>
	/// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
	/// </summary>
	/// <param name="Position">Absolute position in degrees.</param>
	public void MoveAbsolute(float Position)
    {
        tl.LogMessage("MoveAbsolute", Position.ToString()); // Move to this position
        rotatorPosition = Position;
        rotatorPosition = (float)astroUtilities.Range(rotatorPosition, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }

	/// <summary>
	/// Current instantaneous Rotator position, allowing for any sync offset, in degrees.
	/// </summary>
	public float Position
    {
        get
        {
            tl.LogMessage("Position Get", rotatorPosition.ToString()); // This rotator has instantaneous movement
            return rotatorPosition;
        }
    }

	/// <summary>
	/// Sets or Returns the rotator’s Reverse state.
	/// </summary>
	public bool Reverse
    {
        get
        {
            tl.LogMessage("Reverse Get", "Not implemented");
            throw new PropertyNotImplementedException("Reverse", false);
        }
        set
        {
            tl.LogMessage("Reverse Set", "Not implemented");
            throw new PropertyNotImplementedException("Reverse", true);
        }
    }

	/// <summary>
	/// The minimum StepSize, in degrees.
	/// </summary>
	public float StepSize
    {
        get
        {
            tl.LogMessage("StepSize Get", "Not implemented");
            throw new PropertyNotImplementedException("StepSize", false);
        }
    }

	/// <summary>
	/// The destination position angle for Move() and MoveAbsolute().
	/// </summary>
	public float TargetPosition
    {
        get
        {
            tl.LogMessage("TargetPosition Get", rotatorPosition.ToString()); // This rotator has instantaneous movement
            return rotatorPosition;
        }
    }

	// IRotatorV3 methods

	/// <summary>
	/// This returns the raw mechanical position of the rotator in degrees.
	/// </summary>
	public float MechanicalPosition
    {
        get
        {
            tl.LogMessage("MechanicalPosition Get", mechanicalPosition.ToString());
            return mechanicalPosition;
        }
    }

	/// <summary>
	/// Moves the rotator to the specified mechanical angle. 
	/// </summary>
	/// <param name="Position">Mechanical rotator position angle.</param>
	public void MoveMechanical(float Position)
    {
        tl.LogMessage("MoveMechanical", Position.ToString()); // Move to this position

        // TODO: Implement correct sync behaviour. i.e. if the rotator has been synced the mechanical and rotator positions won't be the same
        mechanicalPosition = (float)astroUtilities.Range(Position, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
        rotatorPosition = (float)astroUtilities.Range(Position, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }

	/// <summary>
	/// Syncs the rotator to the specified position angle without moving it. 
	/// </summary>
	/// <param name="Position">Synchronised rotator position angle.</param>
	public void Sync(float Position)
    {
        tl.LogMessage("Sync", Position.ToString()); // Sync to this position

        // TODO: Implement correct sync behaviour. i.e. the rotator mechanical and rotator positions may not be the same
        rotatorPosition = (float)astroUtilities.Range(Position, 0.0, true, 360.0, false); // Ensure value is in the range 0.0..359.9999...
    }

    #endregion

    //ENDOFINSERTEDFILE
}