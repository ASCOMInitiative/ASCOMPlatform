//-----------------------------------------------------------------------
// <summary>Defines the Rotator class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    #region Rotator wrapper
    /// <summary>
    /// Provides universal access to Rotator drivers
    /// </summary>
    public class Rotator : AscomDriver, IRotatorV3
    {
        private MemberFactory memberFactory;

        #region Rotator constructors

        /// <summary>
        /// Creates a rotator object with the given ProgID
        /// </summary>
        /// <param name="rotatorId">ProgID of the rotator to be accessed.</param>
        public Rotator(string rotatorId)
            : base(rotatorId)
        {
            memberFactory = base.MemberFactory;
        }
		#endregion

		#region Convenience Members
		/// <summary>
		/// Brings up the ASCOM Chooser Dialogue to choose a Rotator
		/// </summary>
		/// <param name="rotatorId">Rotator ProgID for default or null for None</param>
		/// <returns>ProgID for chosen Rotator or null for none</returns>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		public static string Choose(string rotatorId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Rotator";
                return chooser.Choose(rotatorId);
            }
        }

		#endregion

		#region IRotatorV2 Members

		/// <summary>
		/// Indicates whether the Rotator supports the <see cref="Reverse" /> method.
		/// </summary>
		/// <returns>
		/// True if the Rotator supports the <see cref="Reverse" /> method.
		/// </returns>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// </remarks>
		public bool CanReverse
        {
            get { return (bool)memberFactory.CallMember(1, "CanReverse", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
		/// </summary>
		/// <remarks>This is an optional method. Raises an error if not supported.</remarks>
		/// <exception cref="MethodNotImplementedException">Throw a MethodNotImplementedException if the rotator cannot halt.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

		/// <summary>
		/// Indicates whether the rotator is currently moving
		/// </summary>
		/// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <para>During rotation, <see cref="IsMoving" /> must be True, otherwise it must be False.</para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public bool IsMoving
        {
            get { return (bool)memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
		/// </summary>
		/// <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
		/// <exception cref="InvalidValueException">If Position is invalid.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <para>Calling <see cref="Move">Move</see> causes the <see cref="TargetPosition" /> property to change to the sum of the current angular position
		/// and the value of the <see cref="Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref="TargetPosition" />.</para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public void Move(float Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(float) }, new object[] { Position });
        }

		/// <summary>
		/// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
		/// </summary>
		/// <param name="Position">Absolute position in degrees.</param>
		/// <exception cref="InvalidValueException">If Position is invalid.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <p style="color:red"><b>SPECIFICATION REVISION - IRotatorV3 - Platform 6.5</b></p>
		/// <para>
		/// Calling <see cref="MoveAbsolute"/> causes the <see cref="TargetPosition" /> property to change to the value of the
		/// <see cref="Position" /> parameter, then starts rotation to <see cref="TargetPosition" />. 
		/// </para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public void MoveAbsolute(float Position)
        {
            memberFactory.CallMember(3, "MoveAbsolute", new Type[] { typeof(float) }, new object[] { Position });
        }

		/// <summary>
		/// Current instantaneous Rotator position, allowing for any sync offset, in degrees.
		/// </summary>
		/// <exception cref="InvalidValueException">If Position is invalid.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <p style="color:red"><b>SPECIFICATION REVISION - IRotatorV3 - Platform 6.5</b></p>
		/// <para>
		/// Position reports the synced position rather than the mechanical position. The synced position is defined 
		/// as the mechanical position plus an offset. The offset is determined when the <see cref="Sync(float)"/> method is called and must be persisted across driver starts and device reboots.
		/// </para>
		/// <para>
		/// The position is expressed as an angle from 0 up to but not including 360 degrees, counter-clockwise against the
		/// sky. This is the standard definition of Position Angle. However, the rotator does not need to (and in general will not)
		/// report the true Equatorial Position Angle, as the attached imager may not be precisely aligned with the rotator's indexing.
		/// It is up to the client to determine any offset between mechanical rotator position angle and the true Equatorial Position
		/// Angle of the imager, and compensate for any difference.
		/// </para>
		/// <para>
		/// The <see cref="Reverse" /> property is provided in order to manage rotators being used on optics with odd or
		/// even number of reflections. With the Reverse switch in the correct position for the optics, the reported position angle must
		/// be counter-clockwise against the sky.
		/// </para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public float Position
        {
            get { return (float)memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Sets or Returns the rotator’s Reverse state.
		/// </summary>
		/// <value>True if the rotation and angular direction must be reversed for the optics</value>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <para>See the definition of <see cref="Position" />.</para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public bool Reverse
        {
            get { return (bool)memberFactory.CallMember(1, "Reverse", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Reverse", new Type[] { }, new object[] { value }); }
        }

		/// <summary>
		/// The minimum StepSize, in degrees.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator does not know its step size.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Optional - can throw a not implemented exception</b></p>
		/// <para>Raises an exception if the rotator does not intrinsically know what the step size is.</para>
		/// </remarks>
		public float StepSize
        {
            get { return (float)memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// The destination position angle for Move() and MoveAbsolute().
		/// </summary>
		/// <value>The destination position angle for<see cref="Move">Move</see> and <see cref="MoveAbsolute">MoveAbsolute</see>.</value>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
		/// <para>Upon calling <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>, this property immediately changes to the position angle to which the rotator is moving. 
		/// The value is retained until a subsequent call to <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>.</para>
		/// <para><b>NOTE</b></para>
		/// <para>IRotatorV3, released in Platform 6.5, requires this method to be implemented, in previous interface versions implementation was optional.</para>
		/// </remarks>
		public float TargetPosition
        {
            get { return (float)memberFactory.CallMember(1, "TargetPosition", new Type[] { }, new object[] { }); }
        }

		#endregion

		#region IRotatorV3 Members

		/// <summary>
		/// This returns the raw mechanical position of the rotator in degrees.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented.</b></p>
		/// <p style="color:red"><b>Introduced in IRotatorV3.</b></p>
		/// Returns the mechanical position of the rotator, which is equivalent to the IRotatorV2 <see cref="Position"/> property. Other clients (beyond the one that performed the sync) 
		/// can calculate the current offset using this and the <see cref="Position"/> value.
		/// </remarks>
		public float MechanicalPosition
        {
            get
            {
                // Return the device's mechanical position for interface versions of 3 and higher otherwise return Position because earlier interfaces don't have an InstrumentalPosition method
                if (base.DriverInterfaceVersion >= 3)
                {
                    return (float)memberFactory.CallMember(1, "MechanicalPosition", new Type[] { }, new object[] { });
                }
                else
                {
                    return (float)memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { });
                }
            }

        }

		/// <summary>
		/// Syncs the rotator to the specified position angle without moving it. 
		/// </summary>
		/// <param name="Position">Synchronised rotator position angle.</param>
		/// <exception cref="InvalidValueException">If Position is invalid.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented.</b></p>
		/// <p style="color:red"><b>Introduced in IRotatorV3.</b></p>
		/// Once this method has been called and the sync offset determined, both the <see cref="MoveAbsolute(float)"/> method and the <see cref="Position"/> property must function in synced coordinates 
		/// rather than mechanical coordinates. The sync offset must persist across driver starts and device reboots.
		/// </remarks>
		public void Sync(float Position)
        {
            if (base.DriverInterfaceVersion >= 3)
            {
                memberFactory.CallMember(3, "Sync", new Type[] { typeof(float) }, new object[] { Position });
            }
            else
            {
                throw new MethodNotImplementedException("Sync", "Sync is not implemented because the driver is IRotatorV2 or earlier.");
            }
        }

		/// <summary>
		/// Moves the rotator to the specified mechanical angle. 
		/// </summary>
		/// <param name="Position">Mechanical rotator position angle.</param>
		/// <exception cref="InvalidValueException">If Position is invalid.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented.</b></p>
		/// <p style="color:red"><b>Introduced in IRotatorV3.</b></p>
		/// <para>Moves the rotator to the requested mechanical angle, independent of any sync offset that may have been set. This method is to address requirements that need a physical rotation
		/// angle such as taking sky flats.</para>
		/// <para>Client applications should use the <see cref="MoveAbsolute(float)"/> method in preference to this method when imaging.</para>
		/// </remarks>
		public void MoveMechanical(float Position)
        {
            if (base.DriverInterfaceVersion >= 3)
            {
                memberFactory.CallMember(3, "MoveMechanical", new Type[] { typeof(float) }, new object[] { Position });
            }
            else
            {
                throw new MethodNotImplementedException("MoveMechanical", "MoveMechanical is not implemented because the driver is IRotatorV2 or earlier.");
            }
        }

        #endregion

    }

    #endregion
}
