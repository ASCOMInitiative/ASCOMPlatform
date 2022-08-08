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
    AstroUtils astroUtilities = new AstroUtils();

    #region IRotator Implementation

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
            bool canReverse = RotatorHardware.CanReverse;
            LogMessage("CanReverse Get", canReverse.ToString());
            return canReverse;
        }
    }

    /// <summary>
    /// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
    /// </summary>
    public void Halt()
    {
        LogMessage("Halt", $"Calling method.");
        RotatorHardware.Halt();
        LogMessage("Halt", $"Completed.");
    }

    /// <summary>
    /// Indicates whether the rotator is currently moving
    /// </summary>
    /// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
    public bool IsMoving
    {
        get
        {
            bool isMoving = RotatorHardware.IsMoving;
            LogMessage("IsMoving Get", isMoving.ToString());
            return isMoving;
        }
    }

	/// <summary>
	/// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
	/// </summary>
	/// <param name="position">Relative position to move in degrees from current <see cref="Position" />.</param>
	public void Move(float position)
    {
        LogMessage("Move", $"Calling method.");
        RotatorHardware.Move(position);
        LogMessage("Move", $"Completed.");
    }


    /// <summary>
    /// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
    /// </summary>
    /// <param name="position">Absolute position in degrees.</param>
    public void MoveAbsolute(float position)
    {
        LogMessage("MoveAbsolute", $"Calling method.");
        RotatorHardware.MoveAbsolute(position);
        LogMessage("MoveAbsolute", $"Completed.");
    }

    /// <summary>
    /// Current instantaneous Rotator position, allowing for any sync offset, in degrees.
    /// </summary>
    public float Position
    {
        get
        {
            float position = RotatorHardware.Position;
            LogMessage("Position Get", position.ToString());
            return position;
        }
    }

	/// <summary>
	/// Sets or Returns the rotator’s Reverse state.
	/// </summary>
	public bool Reverse
    {
        get
        {
            bool canReverse = RotatorHardware.Reverse;
            LogMessage("Reverse Get", canReverse.ToString());
            return canReverse;
        }
        set
        {
            LogMessage("Reverse Set", value.ToString());
            RotatorHardware.Reverse = value;
        }
    }

	/// <summary>
	/// The minimum StepSize, in degrees.
	/// </summary>
	public float StepSize
    {
        get
        {
            float stepSize = RotatorHardware.StepSize;
            LogMessage("StepSize Get", stepSize.ToString());
            return stepSize;
        }
    }

	/// <summary>
	/// The destination position angle for Move() and MoveAbsolute().
	/// </summary>
	public float TargetPosition
    {
        get
        {
            float targetPosition = RotatorHardware.TargetPosition;
            LogMessage("TargetPosition Get", targetPosition.ToString());
            return targetPosition;
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
            float mechanicalPosition = RotatorHardware.MechanicalPosition;
            LogMessage("MechanicalPosition Get", mechanicalPosition.ToString());
            return mechanicalPosition;
        }
    }

	/// <summary>
	/// Moves the rotator to the specified mechanical angle. 
	/// </summary>
	/// <param name="position">Mechanical rotator position angle.</param>
	public void MoveMechanical(float position)
    {
        LogMessage("AbortExposure", $"Calling method.");
        RotatorHardware.MoveMechanical(position);
        LogMessage("AbortExposure", $"Completed.");
    }

    /// <summary>
    /// Syncs the rotator to the specified position angle without moving it. 
    /// </summary>
    /// <param name="position">Synchronised rotator position angle.</param>
    public void Sync(float position)
    {
        LogMessage("Sync", $"Calling method.");
        RotatorHardware.Sync(position);
        LogMessage("Sync", $"Completed.");
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