// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using System;
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
            try
            {
                CheckConnected("CanReverse");
                bool canReverse = RotatorHardware.CanReverse;
                LogMessage("CanReverse", canReverse.ToString());
                return canReverse;
            }
            catch (Exception ex)
            {
                LogMessage("CanReverse", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
    /// </summary>
    public void Halt()
    {
        try
        {
            CheckConnected("Halt");
            LogMessage("Halt", $"Calling method.");
            RotatorHardware.Halt();
            LogMessage("Halt", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Halt", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Indicates whether the rotator is currently moving
    /// </summary>
    /// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
    public bool IsMoving
    {
        get
        {
            try
            {
                CheckConnected("IsMoving");
                bool isMoving = RotatorHardware.IsMoving;
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
    /// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
    /// </summary>
    /// <param name="position">Relative position to move in degrees from current <see cref="Position" />.</param>
    public void Move(float position)
    {
        try
        {
            CheckConnected("Move");
            LogMessage("Move", $"Calling method.");
            RotatorHardware.Move(position);
            LogMessage("Move", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Move", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }


    /// <summary>
    /// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
    /// </summary>
    /// <param name="position">Absolute position in degrees.</param>
    public void MoveAbsolute(float position)
    {
        try
        {
            CheckConnected("MoveAbsolute");
            LogMessage("MoveAbsolute", $"Calling method.");
            RotatorHardware.MoveAbsolute(position);
            LogMessage("MoveAbsolute", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("MoveAbsolute", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Current instantaneous Rotator position, allowing for any sync offset, in degrees.
    /// </summary>
    public float Position
    {
        get
        {
            try
            {
                CheckConnected("Position");
                float position = RotatorHardware.Position;
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
    /// Sets or Returns the rotator’s Reverse state.
    /// </summary>
    public bool Reverse
    {
        get
        {
            try
            {
                CheckConnected("Reverse Get");
                bool canReverse = RotatorHardware.Reverse;
                LogMessage("Reverse Get", canReverse.ToString());
                return canReverse;
            }
            catch (Exception ex)
            {
                LogMessage("Reverse Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        set
        {
            try
            {
                CheckConnected("Reverse Set");
                LogMessage("Reverse Set", value.ToString());
                RotatorHardware.Reverse = value;
            }
            catch (Exception ex)
            {
                LogMessage("Reverse Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The minimum StepSize, in degrees.
    /// </summary>
    public float StepSize
    {
        get
        {
            try
            {
                CheckConnected("StepSize");
                float stepSize = RotatorHardware.StepSize;
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
    /// The destination position angle for Move() and MoveAbsolute().
    /// </summary>
    public float TargetPosition
    {
        get
        {
            try
            {
                CheckConnected("TargetPosition");
                float targetPosition = RotatorHardware.TargetPosition;
                LogMessage("TargetPosition", targetPosition.ToString());
                return targetPosition;
            }
            catch (Exception ex)
            {
                LogMessage("TargetPosition", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("MechanicalPosition");
                float mechanicalPosition = RotatorHardware.MechanicalPosition;
                LogMessage("MechanicalPosition", mechanicalPosition.ToString());
                return mechanicalPosition;
            }
            catch (Exception ex)
            {
                LogMessage("MechanicalPosition", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Moves the rotator to the specified mechanical angle. 
    /// </summary>
    /// <param name="position">Mechanical rotator position angle.</param>
    public void MoveMechanical(float position)
    {
        try
        {
            CheckConnected("AbortExposure");
            LogMessage("AbortExposure", $"Calling method.");
            RotatorHardware.MoveMechanical(position);
            LogMessage("AbortExposure", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("MoveMechanical", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Syncs the rotator to the specified position angle without moving it. 
    /// </summary>
    /// <param name="position">Synchronised rotator position angle.</param>
    public void Sync(float position)
    {
        try
        {
            CheckConnected("Sync");
            LogMessage("Sync", $"Calling method.");
            RotatorHardware.Sync(position);
            LogMessage("Sync", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Sync", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE

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