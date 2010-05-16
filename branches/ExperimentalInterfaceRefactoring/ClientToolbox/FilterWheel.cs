//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interfaces;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    #region FilterWheel wrapper
    /// <summary>
    /// Provides universal access to FilterWheel drivers
    /// </summary>
    [ComVisible(true), Guid("4553D163-D217-4096-9D5E-70223FC0009C"), ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : IFilterWheel, IDisposable, IAscomDriver, IDeviceControl
    {
        object objFilterWheelLateBound;
		IFilterWheel IFilterWheel;
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
                IFilterWheel = (IFilterWheel)objFilterWheelLateBound;
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
                oChooser.DeviceType = "FilterWheel";			// Requires Helper 5.0.3 (May '07)
                return oChooser.Choose(filterWheelID);
            }
            catch
            {
                return "";
            }
		}

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

        #region IAscomDriver Members

        /// <summary>
        /// Set True to enable the
        /// link. Set False to disable the link (this does not switch off the cooler).
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
        bool IAscomDriver.Connected
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
        /// Returns a description of the driver, such as manufacturer and model
        /// number. Any ASCII characters may be used. The string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
        string IAscomDriver.Description
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.Description;
                else
                    return Convert.ToString(objTypeFilterWheel.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { }));
            }
        }
        /// <summary>
        /// Descriptive and version information about this ASCOM Telescope driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the telescope itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
        /// </summary>
        string IAscomDriver.DriverInfo
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.DriverInfo;
                else
                    return (string)objTypeFilterWheel.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
            }
        }
        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
        /// </summary>
        string IAscomDriver.DriverVersion
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.DriverVersion;
                else
                    return (string)objTypeFilterWheel.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
            }
        }
        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
        /// </summary>
        short IAscomDriver.InterfaceVersion
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.InterfaceVersion;
                else
                    return (short)objTypeFilterWheel.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        string IAscomDriver.LastResult
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.LastResult;
                else
                    return Convert.ToString(objTypeFilterWheel.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        string IAscomDriver.Name
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.Name;
                else
                    return (string)objTypeFilterWheel.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        void IAscomDriver.SetupDialog()
        {
            if (IFilterWheel != null)
                IFilterWheel.SetupDialog();
            else
                objTypeFilterWheel.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { });
        }
        #endregion

        #region IDeviceControl Members

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="ActionName">
        /// A well known name agreed by interested parties that represents the action
        /// to be carried out. 
        /// <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change.
        /// </example>
        /// </param>
        /// <param name="ActionParameters">
        /// List of required parameters or <see cref="String.Empty"/>  if none are required.
        /// </param>
        /// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
        string IDeviceControl.Action(string ActionName, string ActionParameters)
        {
            if (IFilterWheel != null)
                return IFilterWheel.Action(ActionName, ActionParameters);
            else
                return (string)objTypeFilterWheel.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        string[] IDeviceControl.SupportedActions
        {
            get
            {
                if (IFilterWheel != null)
                    return IFilterWheel.SupportedActions;
                else
                    return (string[])(objTypeFilterWheel.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFilterWheelLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        void IDeviceControl.CommandBlind(string Command, bool Raw)
        {
            if (IFilterWheel != null)
                IFilterWheel.CommandBlind(Command, Raw);
            else
                objTypeFilterWheel.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        bool IDeviceControl.CommandBool(string Command, bool Raw)
        {
            if (IFilterWheel != null)
                return IFilterWheel.CommandBool(Command, Raw);
            else
                return (bool)objTypeFilterWheel.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        string IDeviceControl.CommandString(string Command, bool Raw)
        {
            if (IFilterWheel != null)
                return IFilterWheel.CommandString(Command, Raw);
            else
                return (string)objTypeFilterWheel.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFilterWheelLateBound, new object[] { Command, Raw });
        }

        #endregion
    }
    #endregion
}
