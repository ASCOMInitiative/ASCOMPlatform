//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.Helper;

namespace ASCOM.DriverAccess
{
    #region FilterWheel wrapper
    /// <summary>
    /// Provides universal access to FilterWheel drivers
    /// </summary>
    public class FilterWheel : ASCOM.Interface.IFilterWheel, IDisposable
    {
        object objFilterWheelLateBound;
		ASCOM.Interface.IFilterWheel IFilterWheel;
		Type objTypeFilterWheel;

        /// <summary>
        /// Creates a FilterWheel object with the given Prog ID
        /// </summary>
        /// <param name="filterWheelID"></param>
        public FilterWheel(string filterWheelID)
		{
			// Get Type Information 
            objTypeFilterWheel = Type.GetTypeFromProgID(filterWheelID);

            // Create an instance of the FilterWheel object
            objFilterWheelLateBound = Activator.CreateInstance(objTypeFilterWheel);

            // Try to see if this driver has an ASCOM.FilterWheel interface
			try
			{
                IFilterWheel = (ASCOM.Interface.IFilterWheel)objFilterWheelLateBound;
			}
			catch (Exception)
			{
                IFilterWheel = null;
			}

		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a FilterWheel
        /// </summary>
        /// <param name="filterWheelID">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen FilterWheel or null for none</returns>
        public static string Choose(string filterWheelID)
        {
            try
            {
                Chooser oChooser = new Chooser();
                oChooser.DeviceTypeV = "FilterWheel";			// Requires Helper 5.0.3 (May '07)
                return oChooser.Choose(filterWheelID);
            }
            catch
            {
                return "";
            }
		}
        #region IFilterWheel Members

        /// <summary>
        /// Controls the link between the driver and the filter wheel. Set True to enable
        /// the link. Set False to disable the link. You can also read the property to check
        /// whether it is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.Connected;
                else
                    return Convert.ToBoolean(objTypeFilterWheel.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { }));
            }
            set
            {
                if (IFilterWheel != null)
                    IFilterWheel.Connected = value;
                else
                    objTypeFilterWheel.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objFilterWheelLateBound, new object[] { value });
            }
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
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.FocusOffsets;
                else
                {
                    return (int[])objTypeFilterWheel.InvokeMember("FocusOffsets",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
                }
            }
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
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.Names;
                else
                {
                    return (string[])objTypeFilterWheel.InvokeMember("Names",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
                }
            }
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
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.Position;
                else
                    return Convert.ToInt16(objTypeFilterWheel.InvokeMember("Position",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { }));
            }
            set
            {
                if (IFilterWheel != null)
                    IFilterWheel.Position = value;
                else
                    objTypeFilterWheel.InvokeMember("Position",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objFilterWheelLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (IFilterWheel != null)
                IFilterWheel.SetupDialog();
            else
                objTypeFilterWheel.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { });
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
			if (this.objFilterWheelLateBound != null)
			{
				try { Marshal.ReleaseComObject(objFilterWheelLateBound); }
				catch (Exception) { }
				objFilterWheelLateBound = null;
			}
		}

        #endregion
    }
    #endregion
}
