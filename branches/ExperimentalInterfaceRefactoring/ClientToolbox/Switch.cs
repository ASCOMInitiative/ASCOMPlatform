//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interfaces;
using ASCOM.Utilities;
using System.Collections;

namespace ASCOM.DriverAccess
{
    #region Switch wrapper
    /// <summary>
    /// Provides universal access to Switch drivers
    /// </summary>
    [ComVisible(true), Guid("B9AAB7E3-0B8E-4c57-B875-0D5AB79305B5"), ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitch, IDisposable, IAscomDriver, IDeviceControl
    {
        object objSwitchLateBound;
		ISwitch ISwitch;
		Type objTypeSwitch;

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchID"></param>
        public Switch(string switchID)
		{
			// Get Type Information 
            objTypeSwitch = Type.GetTypeFromProgID(switchID);

            // Create an instance of the Switch object
            objSwitchLateBound = Activator.CreateInstance(objTypeSwitch);

			// Try to see if this driver has an ASCOM.Switch interface
			try
			{
                ISwitch = (ISwitch)objSwitchLateBound;
			}
			catch (Exception)
			{
                ISwitch = null;
			}

		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Switch
        /// </summary>
        /// <param name="switchID">Switch Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Switch or null for none</returns>
        public static string Choose(string switchID)
        {
			Chooser oChooser = new Chooser();
            oChooser.DeviceType = "Switch";			// Requires Helper 5.0.3 (May '07)
            return oChooser.Choose(switchID);
		}
    
        #region ISwitch Members


        public void SetSwitch(string Name)
        {
                if (ISwitch != null)
                    ISwitch.SetSwitch(Name);
                else
                    objTypeSwitch.InvokeMember("SetSwitch",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null, objSwitchLateBound, new object[] { Name });
        }

        /// <summary>
        /// Yields a collection of ISwitchDevice objects.
        /// </summary>
        /// <value></value>
        public ArrayList Switches
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.Switches;
                else
                    return (ArrayList)(objTypeSwitch.InvokeMember("Switches",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { }));
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
			if (this.objSwitchLateBound != null)
			{
				try { Marshal.ReleaseComObject(objSwitchLateBound); }
				catch (Exception) { }
				objSwitchLateBound = null;
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
        public bool Connected
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.Connected;
                else
                    return Convert.ToBoolean(objTypeSwitch.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { }));
            }
            set
            {
                if (ISwitch != null)
                    ISwitch.Connected = value;
                else
                    objTypeSwitch.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objSwitchLateBound, new object[] { value });
            }
        }
        /// <summary>
        /// Returns a description of the driver, such as manufacturer and model
        /// number. Any ASCII characters may be used. The string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
        public string Description
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.Description;
                else
                    return Convert.ToString(objTypeSwitch.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { }));
            }
        }
        /// <summary>
        /// Descriptive and version information about this ASCOM Telescope driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the telescope itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.DriverInfo;
                else
                    return (string)objTypeSwitch.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { });
            }
        }
        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
        /// </summary>
        public string DriverVersion
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.DriverVersion;
                else
                    return (string)objTypeSwitch.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { });
            }
        }
        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.InterfaceVersion;
                else
                    return (short)objTypeSwitch.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        public string LastResult
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.LastResult;
                else
                    return Convert.ToString(objTypeSwitch.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.Name;
                else
                    return (string)objTypeSwitch.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (ISwitch != null)
                ISwitch.SetupDialog();
            else
                objTypeSwitch.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSwitchLateBound, new object[] { });
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
        public string Action(string ActionName, string ActionParameters)
        {
            if (ISwitch != null)
                return ISwitch.Action(ActionName, ActionParameters);
            else
                return (string)objTypeSwitch.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSwitchLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (ISwitch != null)
                    return ISwitch.SupportedActions;
                else
                    return (string[])(objTypeSwitch.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objSwitchLateBound, new object[] { }));
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
        public void CommandBlind(string Command, bool Raw)
        {
            if (ISwitch != null)
                ISwitch.CommandBlind(Command, Raw);
            else
                objTypeSwitch.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSwitchLateBound, new object[] { Command, Raw });
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
        public bool CommandBool(string Command, bool Raw)
        {
            if (ISwitch != null)
                return ISwitch.CommandBool(Command, Raw);
            else
                return (bool)objTypeSwitch.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSwitchLateBound, new object[] { Command, Raw });
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
        public string CommandString(string Command, bool Raw)
        {
            if (ISwitch != null)
                return ISwitch.CommandString(Command, Raw);
            else
                return (string)objTypeSwitch.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objSwitchLateBound, new object[] { Command, Raw });
        }

        #endregion
    }
    #endregion
}
