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
    public class FilterWheel : AscomDriver, IFilterWheelV3
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

        /// <summary>
        /// Focus offset of each filter in the wheel
        ///</summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// For each valid slot number (from 0 to N-1), reports the focus offset for
        /// the given filter position.  These values are focuser and filter
        /// dependent, and  would usually be set up by the user via the SetupDialog.
        /// The number of slots N can be determined from the length of the array.
        /// If focuser offsets are not available, then it should report back 0 for all
        /// array values.
        /// </remarks>
        public int[] FocusOffsets
        {
            get { return (int[])memberFactory.CallMember(1, "FocusOffsets", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Name of each filter in the wheel
		///</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// For each valid slot number (from 0 to N-1), reports the name given to the
		/// filter position.  These names would usually be set up by the user via the
		/// SetupDialog.  The number of slots N can be determined from the length of
		/// the array.  If filter names are not available, then it should report back
		/// "Filter 1", "Filter 2", etc.
		/// </remarks>
		public string[] Names
        {
            get { return (string[])memberFactory.CallMember(1, "Names", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sets or returns the current filter wheel position
        /// </summary>
        /// <exception cref="InvalidValueException">Must throw an InvalidValueException if an invalid position is set</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        ///<para>This is an asynchronous operation: Writing to Position must return as soon as the filter change operation has been successfully started. Reading the Position property must return -1 while the change
        /// is in progress. After the requested position has been successfully reached and motion stops, reading Position must return the requested new filter number.</para>
        /// <para>Write a position number between 0 and N-1, where N is the number of filter slots (see
        /// <see cref="Names"/>). Starts filter wheel rotation immediately when written. Reading
        /// the property gives current slot number (if wheel stationary) or -1 if wheel is
        /// moving.</para>
        /// <para>Returning a position of -1 is <b>mandatory</b> while the filter wheel is in motion; valid slot numbers must not be reported back while
        /// the filter wheel is rotating past filter positions.</para>
        /// <para><b>Note</b></para>
        /// <para>Some filter wheels are built into the camera (one driver, two
        /// interfaces).  Some cameras may not actually rotate the wheel until the
        /// exposure is triggered.  In this case, the written value is available
        /// immediately as the read value, and -1 is never produced.</para>
        /// </remarks>
        public short Position
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Position", new Type[] { typeof(Int16) }, new object[] { value }); }
        }

        #endregion
    }
}
