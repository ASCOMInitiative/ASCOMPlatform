//-----------------------------------------------------------------------
// <summary>Defines the FilterWheel class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region FilterWheel wrapper

    /// <summary>
    /// Provides universal access to FilterWheel drivers
    /// </summary>
    public class FilterWheel : IFilterWheel, IDisposable
    {
        #region FilterWheel constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates a FilterWheel object with the given Prog ID
        /// </summary>
        /// <param name="filterWheelId"></param>
        public FilterWheel(string filterWheelId)
        {
            _memberFactory = new MemberFactory(filterWheelId);
        }

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a FilterWheel
        /// </summary>
        /// <param name="filterWheelId">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen FilterWheel or null for none</returns>
        public static string Choose(string filterWheelId)
        {
            try
            {
                var oChooser = new Chooser {DeviceType = "FilterWheel"};
                return oChooser.Choose(filterWheelId);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            _memberFactory.Dispose();
        }

        #endregion

        #region IFilterWheel Members

        /// <summary>
        /// Controls the link between the driver and the filter wheel. Set True to enable
        /// the link. Set False to disable the link. You can also read the property to check
        /// whether it is connected.
        /// </summary>
        public bool Connected
        {
            get { return (bool) _memberFactory.CallMember(1, "Connected", new Type[] {}, new object[] {}); }
            set { _memberFactory.CallMember(2, "Connected", new Type[] {}, new object[] {value}); }
        }

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
            get { return (int[]) _memberFactory.CallMember(1, "FocusOffsets", new Type[] {}, new object[] {}); }
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
            get { return (string[]) _memberFactory.CallMember(1, "Names", new Type[] {}, new object[] {}); }
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
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "Position", new Type[] {}, new object[] {})); }
            set { _memberFactory.CallMember(2, "Position", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            _memberFactory.CallMember(3, "SetupDialog", new Type[] {}, new object[] {});
        }

        #endregion
    }

    #endregion
}