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
    public class Switch : ISwitch, IDisposable, IAscomDriver, IDeviceControl
    {
        object objLateBound;
        ISwitch ascomInterface;
		Type objType;
        private MemberFactory memberFactory;

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchID"></param>
        public Switch(string switchID)
		{

            memberFactory = new MemberFactory(switchID);

			// Get Type Information 
            //objType = Type.GetTypeFromProgID(switchID);

            // Create an instance of the Switch object
            //objLateBound = Activator.CreateInstance(objType);

			// Try to see if this driver has an ASCOM.Switch interface
            //ascomInterface = objLateBound as ISwitch;

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
    
        #region ascomInterface Members


        public void SetSwitch(string Name)
        {
                if (ascomInterface != null)
                    ascomInterface.SetSwitch(Name);
                else
                    objType.InvokeMember("SetSwitch",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null, objLateBound, new object[] { Name });
        }

        /// <summary>
        /// Yields a collection of ascomInterfaceDevice objects.
        /// </summary>
        /// <value></value>
        public ArrayList Switches
        {
            get
            {
                if (ascomInterface != null)
                    return ascomInterface.Switches;
                else
                    return (ArrayList)(objType.InvokeMember("Switches",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { }));
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
			if (this.objLateBound != null)
			{
				try { Marshal.ReleaseComObject(objLateBound); }
				catch (Exception) { }
				objLateBound = null;
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
                return (bool)memberFactory.CallMember(1, "Connected", null, new object[] { });
            }
            set
            {
                memberFactory.CallMember(2, "Connected", null, new object[] { });
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
                return null;

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
                if (ascomInterface != null)
                    return ascomInterface.DriverInfo;
                else
                    return (string)objType.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { });
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
                return null;

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
                if (ascomInterface != null)
                    return ascomInterface.InterfaceVersion;
                else
                    return (short)objType.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { });
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
                if (ascomInterface != null)
                    return ascomInterface.LastResult;
                else
                    return Convert.ToString(objType.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (ascomInterface != null)
                    return ascomInterface.Name;
                else
                    return (string)objType.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (ascomInterface != null)
                ascomInterface.SetupDialog();
            else
                objType.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objLateBound, new object[] { });
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
            if (ascomInterface != null)
                return ascomInterface.Action(ActionName, ActionParameters);
            else
                return (string)objType.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (ascomInterface != null)
                    return ascomInterface.SupportedActions;
                else
                    return (string[])(objType.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objLateBound, new object[] { }));
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
            if (ascomInterface != null)
                ascomInterface.CommandBlind(Command, Raw);
            else
                objType.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objLateBound, new object[] { Command, Raw });
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
            if (ascomInterface != null)
                return ascomInterface.CommandBool(Command, Raw);
            else
                return (bool)objType.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objLateBound, new object[] { Command, Raw });
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
            if (ascomInterface != null)
                return ascomInterface.CommandString(Command, Raw);
            else
                return (string)objType.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objLateBound, new object[] { Command, Raw });
        }

        #endregion
    }
    #endregion
}
