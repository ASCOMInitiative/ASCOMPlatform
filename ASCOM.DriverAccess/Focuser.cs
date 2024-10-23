//-----------------------------------------------------------------------
// <summary>Defines the Focuser class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to Focuser drivers
    /// </summary>
    public class Focuser : AscomDriver, IFocuserV2, IFocuserV3, IFocuserV4
    {
        private MemberFactory memberFactory;

        #region Focuser constructors

        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserId">ProgID of the focuser device to be accessed.</param>
        public Focuser(string focuserId)
            : base(focuserId)
        {
            memberFactory = base.MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a Focuser
        /// </summary>
        /// <param name="focuserId">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string focuserId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Focuser";
                return chooser.Choose(focuserId);
            }
        }

        /// <summary>
        /// Focuser device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public FocuserState FocuserState
        {
            get
            {
                // Create a state object to return.
                FocuserState focuserState = new FocuserState(DeviceState, TL);
                TL.LogMessage(nameof(FocuserState), $"Returning: '{focuserState.IsMoving}' '{focuserState.Position}' '{focuserState.Temperature}' '{focuserState.TimeStamp}'");

                // Return the device specific state class
                return focuserState;
            }
        }

        #endregion

        #region IFocuser Members

        /// <inheritdoc/>
        public bool Absolute
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Absolute", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public bool IsMoving
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { })); }
        }
        /// <inheritdoc/>
		public bool Link
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Link", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Link", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
		public int MaxIncrement
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxIncrement", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public int MaxStep
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxStep", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public void Move(int Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(int) }, new object[] { Position });
        }

        /// <inheritdoc/>
		public int Position
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public double StepSize
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool TempComp
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempComp", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TempComp", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
		public bool TempCompAvailable
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempCompAvailable", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public double Temperature
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { })); }
        }

        #endregion

    }
}
