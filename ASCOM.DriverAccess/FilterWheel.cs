//-----------------------------------------------------------------------
// <summary>Defines the FilterWheel class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to FilterWheel drivers
    /// </summary>
    public class FilterWheel : AscomDriver, IFilterWheelV2, IFilterWheelV3
    {
        private MemberFactory memberFactory;

        #region FilterWheel constructors

        /// <summary>
        /// Creates a FilterWheel object with the given Prog ID
        /// </summary>
        /// <param name="filterWheelId">ProgID of the filterwheel device to be accessed.</param>
        public FilterWheel(string filterWheelId)
            : base(filterWheelId)
        {
            //memberFactory = new MemberFactory(filterWheelID);
            memberFactory = base.MemberFactory; //Get the member factory created by the base class
        }

        #region IDisposable Members
        // No member here, we are relying on Dispose in the base class
        #endregion

        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a FilterWheel
        /// </summary>
        /// <param name="filterWheelId">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen FilterWheel or null for none</returns>
        public static string Choose(string filterWheelId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "FilterWheel";
                return chooser.Choose(filterWheelId);
            }
        }

        /// <summary>
        /// FilterWheel device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public FilterWheelState FilterWheelState
        {
            get
            {
                // Create a state object to return.
                FilterWheelState filterWheelState = new FilterWheelState(DeviceState, TL);
                TL.LogMessage(nameof(FilterWheelState), $"Returning: '{filterWheelState.Position}' '{filterWheelState.TimeStamp}'");

                // Return the device specific state class
                return filterWheelState;
            }
        }

        #endregion

        #region IFilterWheel Members

        /// <inheritdoc/>
        public int[] FocusOffsets
        {
            get { return (int[])memberFactory.CallMember(1, "FocusOffsets", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
		public string[] Names
        {
            get { return (string[])memberFactory.CallMember(1, "Names", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public short Position
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Position", new Type[] { typeof(Int16) }, new object[] { value }); }
        }

        #endregion
    }
}
