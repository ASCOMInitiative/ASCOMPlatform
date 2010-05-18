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
	#region Focuser wrapper
	/// <summary>
	/// Provides universal access to Focuser drivers
	/// </summary>
    [ComVisible(true), Guid("E92E1FDC-125A-4926-887E-0BFA6D2A3914"), ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser, IDisposable, IAscomDriver, IDeviceControl
    {

        object objFocuserLateBound;
		IFocuser IFocuser;
		Type objTypeFocuser;
        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserID"></param>
        public Focuser(string focuserID)
		{
			// Get Type Information 
            objTypeFocuser = Type.GetTypeFromProgID(focuserID);
			
			// Create an instance of the Focuser object
            objFocuserLateBound = Activator.CreateInstance(objTypeFocuser);

			// Try to see if this driver has an ASCOM.Focuser interface
			try
			{
				IFocuser = (IFocuser)objFocuserLateBound;
			}
			catch (Exception)
			{
				IFocuser = null;
			}

		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Focuser
        /// </summary>
        /// <param name="focuserID">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string focuserID)
        {
			Chooser oChooser = new Chooser();
			oChooser.DeviceType = "Focuser";			// Requires Helper 5.0.3 (May '07)
			return oChooser.Choose(focuserID);
		}


        #region IFocuser Members

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Absolute;
				else
					return (bool)objTypeFocuser.InvokeMember("Absolute", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() method call.
        /// Some focusers may not support this function, in which case an exception will be raised. 
        /// Recommendation: Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface. 
        /// </summary>
        public void Halt()
        {
			if (IFocuser != null)
				IFocuser.Halt();
			else
				objTypeFocuser.InvokeMember("Halt", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objFocuserLateBound, new object[] { });
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.IsMoving;
				else
					return (bool)objTypeFocuser.InvokeMember("IsMoving", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }
        /// <summary>
        /// State of the connection to the focuser.
        /// et True to start the link to the focuser; set False to terminate the link. 
        /// The current link status can also be read back as this property. 
        /// An exception will be raised if the link fails to change state for any reason. 
        /// </summary>
        public bool Link
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Link;
				else
					return (bool)objTypeFocuser.InvokeMember("Link", 
						BindingFlags.Default | BindingFlags.GetProperty,
                    null, objFocuserLateBound, new object[] { });
            }
            set
            {
				if (IFocuser != null)
					IFocuser.Link = value;
				else
					objTypeFocuser.InvokeMember("Link", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objFocuserLateBound, new object[] { value });

            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; 
        /// i.e. the maximum number of steps allowed in one move operation.
        /// For most focusers this is the same as the MaxStep property.
        /// This is normally used to limit the Increment display in the host software. 
        /// </summary>
        public int MaxIncrement
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.MaxIncrement;
				else
					return (int)objTypeFocuser.InvokeMember("MaxIncrement", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit. 
        /// </summary>
        public int MaxStep
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.MaxStep;
				else
					return (int)objTypeFocuser.InvokeMember("MaxStep", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { }); 
            }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// </summary>
        /// <param name="val"></param>
        public void Move(int val)
        {
			if (IFocuser != null)
				IFocuser.Move(val);
			else
				objTypeFocuser.InvokeMember("Move", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { val });
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// Valid only for absolute positioning focusers (see the Absolute property).
        /// An exception will be raised for relative positioning focusers.   
        /// </summary>
        public int Position
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Position;
				else
					return (int)objTypeFocuser.InvokeMember("Position", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// 
        /// </summary>

        public double StepSize
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.StepSize;
				else
					return (double)objTypeFocuser.InvokeMember("StepSize", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// If the TempCompAvailable property is True, then setting TempComp to True
        /// puts the focuser into temperature tracking mode. While in temperature tracking mode,
        /// Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
        /// An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. 
        /// 
        /// </summary>
        public bool TempComp
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.TempComp;
				else
					return (bool)objTypeFocuser.InvokeMember("TempComp", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
            set
            {
				if (IFocuser != null)
					IFocuser.TempComp = value;
				else
					objTypeFocuser.InvokeMember("TempComp",
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objFocuserLateBound, new object[] { value } );

            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
        /// </summary>
        public bool TempCompAvailable
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.TempCompAvailable;
				else
					return (bool)objTypeFocuser.InvokeMember("TempCompAvailable", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Current ambient temperature as measured by the focuser.
        /// Raises an exception if ambient temperature is not available.
        /// Commonly available on focusers with a built-in temperature compensation mode. 
        /// </summary>
        public double Temperature
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Temperature;
				else
					return (double)objTypeFocuser.InvokeMember("Temperature",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
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
			if (this.objFocuserLateBound != null)
			{
				try { Marshal.ReleaseComObject(objFocuserLateBound); }
				catch (Exception) { }
				objFocuserLateBound = null;
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
                if (IFocuser != null)
                    return IFocuser.Connected;
                else
                    return Convert.ToBoolean(objTypeFocuser.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { }));
            }
            set
            {
                if (IFocuser != null)
                    IFocuser.Connected = value;
                else
                    objTypeFocuser.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objFocuserLateBound, new object[] { value });
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
                if (IFocuser != null)
                    return IFocuser.Description;
                else
                    return Convert.ToString(objTypeFocuser.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { }));
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
                if (IFocuser != null)
                    return IFocuser.DriverInfo;
                else
                    return (string)objTypeFocuser.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { });
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
                if (IFocuser != null)
                    return IFocuser.DriverVersion;
                else
                    return (string)objTypeFocuser.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { });
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
                if (IFocuser != null)
                    return IFocuser.InterfaceVersion;
                else
                    return (short)objTypeFocuser.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { });
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
                if (IFocuser != null)
                    return IFocuser.LastResult;
                else
                    return Convert.ToString(objTypeFocuser.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (IFocuser != null)
                    return IFocuser.Name;
                else
                    return (string)objTypeFocuser.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (IFocuser != null)
                IFocuser.SetupDialog();
            else
                objTypeFocuser.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { });
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
            if (IFocuser != null)
                return IFocuser.Action(ActionName, ActionParameters);
            else
                return (string)objTypeFocuser.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (IFocuser != null)
                    return IFocuser.SupportedActions;
                else
                    return (string[])(objTypeFocuser.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objFocuserLateBound, new object[] { }));
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
            if (IFocuser != null)
                IFocuser.CommandBlind(Command, Raw);
            else
                objTypeFocuser.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { Command, Raw });
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
            if (IFocuser != null)
                return IFocuser.CommandBool(Command, Raw);
            else
                return (bool)objTypeFocuser.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { Command, Raw });
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
            if (IFocuser != null)
                return IFocuser.CommandString(Command, Raw);
            else
                return (string)objTypeFocuser.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { Command, Raw });
        }

        #endregion

	}
	#endregion
}
