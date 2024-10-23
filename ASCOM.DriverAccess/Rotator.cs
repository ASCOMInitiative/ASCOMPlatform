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
    /// <summary>
    /// Provides universal access to Rotator drivers
    /// </summary>
    public class Rotator : AscomDriver, IRotatorV2, IRotatorV3, IRotatorV4
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
        public static string Choose(string rotatorId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Rotator";
                return chooser.Choose(rotatorId);
            }
        }

        /// <summary>
        /// Rotator device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public RotatorState RotatorState
        {
            get
            {
                // Create a state object to return.
                RotatorState rotatorState = new RotatorState(DeviceState, TL);
                TL.LogMessage(nameof(RotatorState), $"Returning: " +
                    $"Cloud cover: '{rotatorState.IsMoving}', " +
                    $"Dew point: '{rotatorState.MechanicalPosition}', " +
                    $"Humidity: '{rotatorState.Position}', " +
                    $"Time stamp: '{rotatorState.TimeStamp}'");

                // Return the device specific state class
                return rotatorState;
            }
        }

        #endregion

        #region IRotatorV2 Members

        /// <inheritdoc/>
        public bool CanReverse
        {
            get { return (bool)memberFactory.CallMember(1, "CanReverse", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
		public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public bool IsMoving
        {
            get { return (bool)memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public void Move(float Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <inheritdoc/>
        public void MoveAbsolute(float Position)
        {
            memberFactory.CallMember(3, "MoveAbsolute", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <inheritdoc/>
		public float Position
        {
            get { return (float)memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
		public bool Reverse
        {
            get { return (bool)memberFactory.CallMember(1, "Reverse", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Reverse", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
		public float StepSize
        {
            get { return (float)memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
		public float TargetPosition
        {
            get { return (float)memberFactory.CallMember(1, "TargetPosition", new Type[] { }, new object[] { }); }
        }

        #endregion

        #region IRotatorV3 Members

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
}