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
    public class Rotator : AscomDriver, IRotatorV2
    {
        #region Rotator constructors
        private MemberFactory memberFactory;

        /// <summary>
        /// Creates a rotator object with the given Prog ID
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
        /// Brings up the ASCOM Chooser Dialog to choose a Rotator
        /// </summary>
        /// <param name="rotatorId">Rotator Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Rotator or null for none</returns>
        public static string Choose(string rotatorId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Rotator";
                return chooser.Choose(rotatorId);
            }
        }

        #endregion

        #region IRotator Members

        /// <summary>
        /// Indicates whether the Rotator supports the <see cref="Reverse" /> method.
        /// </summary>
        /// <returns>
        /// True if the Rotator supports the <see cref="Reverse" /> method.
        /// </returns>
        public bool CanReverse
        {
            get { return (bool)memberFactory.CallMember(1, "CanReverse", new Type[] { }, new object[] { }); }
        }


        /// <summary>
        /// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
        /// </summary>
        /// <remarks>This is an optional method. Raises an error if not supported.</remarks>
        /// <exception cref="MethodNotImplementedException">Throw a MethodNotImplementedException if the rotator cannot halt.</exception>
        public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Indicates whether the rotator is currently moving
        /// </summary>
        /// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
        /// <remarks>Rotation is asynchronous, that is, when the <see cref="Move">Move</see> method is called, it starts the rotation, then returns 
        /// immediately. During rotation, <see cref="IsMoving" /> must be True, else it must be False.</remarks>
        public bool IsMoving
        {
            get { return (bool)memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
        /// </summary>
        /// <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
        /// <remarks>Calling <see cref="Move">Move</see> causes the <see cref="TargetPosition" /> property to change to the sum of the current angular position 
        /// and the value of the <see cref="Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref="TargetPosition" />.</remarks>
        public void Move(float Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <summary>
        /// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
        /// </summary>
        /// <param name="Position">Absolute position in degrees.</param>
        /// <remarks>Calling <see cref="MoveAbsolute">MoveAbsolute</see> causes the <see cref="TargetPosition" /> property to change to the value of the
        /// <see cref="Position" /> parameter, then starts rotation to <see cref="TargetPosition" />. </remarks>
        public void MoveAbsolute(float Position)
        {
            memberFactory.CallMember(3, "MoveAbsolute", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <summary>
        /// Current instantaneous Rotator position, in degrees.
        /// </summary>
        /// <remarks>
        /// The position is expressed as an angle from 0 up to but not including 360 degrees, counter-clockwise against the 
        /// sky. This is the standard definition of Position Angle. However, the rotator does not need to (and in general will not) 
        /// report the true Equatorial Position Angle, as the attached imager may not be precisely aligned with the rotator's indexing. 
        /// It is up to the client to determine any offset between mechanical rotator position angle and the true Equatorial Position 
        /// Angle of the imager, and compensate for any difference. 
        /// <para>The optional <see cref="Reverse" /> property is provided in order to manage rotators being used on optics with odd or 
        /// even number of reflections. With the Reverse switch in the correct position for the optics, the reported position angle must 
        /// be counter-clockwise against the sky.</para>
        /// </remarks>
        public float Position
        {
            get { return (float)memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sets or Returns the rotator’s Reverse state.
        /// </summary>
        /// <value>True if the rotation and angular direction must be reversed for the optics</value>
        /// <remarks>See the definition of <see cref="Position" />. Raises an error if not supported. </remarks>
        /// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator cannot reverse.</exception>
        public bool Reverse
        {
            get { return (bool)memberFactory.CallMember(1, "Reverse", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Reverse", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        /// <remarks>
        /// Raises an exception if the rotator does not intrinsically know what the step size is.
        /// </remarks>
        /// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator does not know its step size.</exception>
        public float StepSize
        {
            get { return (float)memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// The destination position angle for Move() and MoveAbsolute().
        /// </summary>
        /// <value>The destination position angle for<see cref="Move">Move</see> and <see cref="MoveAbsolute">MoveAbsolute</see>.</value>
        /// <remarks>Upon calling <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>, this property immediately 
        /// changes to the position angle to which the rotator is moving. The value is retained until a subsequent call 
        /// to <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>.
        /// </remarks>
        public float TargetPosition
        {
            get { return (float)memberFactory.CallMember(1, "TargetPosition", new Type[] { }, new object[] { }); }
        }

        #endregion

    }
    #endregion
}
