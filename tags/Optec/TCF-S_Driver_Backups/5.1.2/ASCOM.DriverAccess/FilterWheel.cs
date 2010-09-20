//-----------------------------------------------------------------------
// <summary>Defines the FilterWheel class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Conform;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to FilterWheel drivers
    /// </summary>
    public class FilterWheel : AscomDriver, IFilterWheel
    {
         private MemberFactory memberFactory;

        #region FilterWheel constructors

        /// <summary>
        /// Creates a FilterWheel object with the given Prog ID
        /// </summary>
        /// <param name="filterWheelID"></param>
        public FilterWheel(string filterWheelID) : base(filterWheelID)
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
        /// Brings up the ASCOM Chooser Dialog to choose a FilterWheel
        /// </summary>
        /// <param name="filterWheelID">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen FilterWheel or null for none</returns>
        public static string Choose(string filterWheelID)
        {
            Chooser oChooser = new Chooser();
            oChooser.DeviceType = "FilterWheel";			// Requires Helper 5.0.3 (May '07)
            return oChooser.Choose(filterWheelID);
        }

        #endregion

        #region IFilterWheel Members

        /// <summary>
        /// For each valid slot number (from 0 to N-1), reports the focus offset for
        /// the given filter position.  These values are focuser- and filter
        /// -dependent, and  would usually be set up by the user via the SetupDialog.
        /// The number of slots N can be determined from the length of the array.
        /// If focuser offsets are not available, then it should report back 0 for all
        /// array values.
        /// </summary>
        public int[] FocusOffsets
        {
            get { return (int[])memberFactory.CallMember(1, "FocusOffsets", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// For each valid slot number (from 0 to N-1), reports the name given to the
        /// filter position.  These names would usually be set up by the user via the
        /// SetupDialog.  The number of slots N can be determined from the length of
        /// the array.  If filter names are not available, then it should report back
        /// "Filter 1", "Filter 2", etc.
        /// </summary>
        public string[] Names
        {
            get { return (string[])memberFactory.CallMember(1, "Names", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Write number between 0 and N-1, where N is the number of filter slots (see
        /// Filter.Names). Starts filter wheel rotation immediately when written*. Reading
        /// the property gives current slot number (if wheel stationary) or -1 if wheel is
        /// moving. This is mandatory; valid slot numbers shall not be reported back while
        /// the filter wheel is rotating past filter positions.
        /// 
        /// Note that some filter wheels are built into the camera (one driver, two
        /// interfaces).  Some cameras may not actually rotate the wheel until the
        /// exposure is triggered.  In this case, the written value is available
        /// immediately as the read value, and -1 is never produced.
        /// </summary>
        public short Position
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Position", new Type[] { }, new object[] { value }); }
        }

        #endregion

    }
}
