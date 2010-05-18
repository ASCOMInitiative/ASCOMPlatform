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
	#region Dome wrapper
	/// <summary>
	/// Provides universal access to ASCOM Dome drivers
	/// </summary>
	public class Dome : IDome, IDisposable, IDeviceControl, IAscomDriver
    {

        object objDomeLateBound;
		IDome IDome;
        Type objTypeDome;
        /// <summary>
        /// Constructor for Dome class. Creates a Dome based on the ProgID in the DomeID string.
        /// </summary>
        /// <param name="domeID">The progID of the dome to be instantiated</param>
        public Dome(string domeID)
		{
			// Get Type Information 
            objTypeDome = Type.GetTypeFromProgID(domeID);
			
			// Create an instance of the Focuser object
            objDomeLateBound = Activator.CreateInstance(objTypeDome);

			// Try to see if this driver has an ASCOM.Dome interface
			try {
				IDome = (IDome)objDomeLateBound;
			} catch(Exception) {
				IDome = null;
			}

		}

        /// <summary>
        /// Shows the ASCOM Chooser to select a Dome.
        /// </summary>
        /// <param name="domeID">Prog ID of the default dome to select. Null if no default is to be set.</param>
        /// <returns>The Prog ID of the Dome chosen, or Null if no dome is chose, or the dialog is canceled.</returns>
        public static string Choose(string domeID)
        {
			Chooser oChooser = new Chooser();
			oChooser.DeviceType = "Dome";				// Requires Helper 5.0.3 (May '07)
			return oChooser.Choose(domeID);
		}

        #region IDome Members

        /// <summary>
        /// Immediately cancel current dome operation.
        /// Calling this method will immediately disable hardware slewing (Dome.Slaved will become False).
        /// Raises an error if a communications failure occurs, or if the command is known to have failed. 
        /// </summary>
        public void AbortSlew()
        {
			if (IDome != null)
				IDome.AbortSlew();
			else
				objTypeDome.InvokeMember("AbortSlew", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// The dome altitude (degrees, horizon zero and increasing positive to 90 zenith).
        /// Raises an error only if no altitude control. If actual dome altitude can not be read,
        /// then reports back the last slew position. 
        /// </summary>
        public double Altitude
        {
            get
            {
				if (IDome != null)
					return IDome.Altitude;
				else
					return (double)objTypeDome.InvokeMember("Altitude", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// True if the dome is in the Home position.
        /// Set only following a Dome.FindHome operation and reset with any azimuth slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        public bool AtHome
        {
            get
            {
				if (IDome != null)
					return IDome.AtHome;
				else
					return (bool)objTypeDome.InvokeMember("AtHome", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the dome is in the programmed park position.
        /// Set only following a Dome.Park operation and reset with any slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        public bool AtPark
        {
            get
            {
				if (IDome != null)
					return IDome.AtPark;
				else
					return (bool)objTypeDome.InvokeMember("AtPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
        /// </summary>
        public double Azimuth
        {
            get
            {
				if (IDome != null)
					return IDome.Azimuth;
				else
					return (double)objTypeDome.InvokeMember("Azimuth", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver can do a search for home position.
        /// </summary>
        public bool CanFindHome
        {
            get
            {
				if (IDome != null)
					return IDome.CanFindHome;
				else
					return (bool)objTypeDome.InvokeMember("CanFindHome", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanPark
        {
            get
            {
				if (IDome != null)
					return IDome.CanPark;
				else
					return (bool)objTypeDome.InvokeMember("CanPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanSetAltitude
        {
            get
            {
				if (IDome != null)
					return IDome.CanSetAltitude;
				else
					return (bool)objTypeDome.InvokeMember("CanSetAltitude", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver is capable of setting dome azimuth.
        /// </summary>
        public bool CanSetAzimuth
        {
            get
            {
				if (IDome != null)
					return IDome.CanSetAzimuth;
				else
					return (bool)objTypeDome.InvokeMember("CanSetAzimuth", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver can set the dome park position.
        /// </summary>
        public bool CanSetPark
        {
            get
            {
				if (IDome != null)
					return IDome.CanSetPark;
				else
					return (bool)objTypeDome.InvokeMember("CanSetPark", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver is capable of automatically operating shutter.
        /// </summary>
        public bool CanSetShutter
        {
            get
            {
				if (IDome != null)
					return IDome.CanSetShutter;
				else
					return (bool)objTypeDome.InvokeMember("CanSetShutter", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the dome hardware supports slaving to a telescope.
        /// </summary>
        public bool CanSlave
        {
            get
            {
				if (IDome != null)
					return IDome.CanSlave;
				else
					return (bool)objTypeDome.InvokeMember("CanSlave", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if driver is capable of synchronizing the dome azimuth position using the Dome.SyncToAzimuth method.
        /// </summary>
        public bool CanSyncAzimuth
        {
            get
            {
				if (IDome != null)
					return IDome.CanSyncAzimuth;
				else
					return (bool)objTypeDome.InvokeMember("CanSyncAzimuth", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Close shutter or otherwise shield telescope from the sky.
        /// </summary>
        public void CloseShutter()
        {
			if (IDome != null)
				IDome.CloseShutter();
			else
				objTypeDome.InvokeMember("CloseShutter", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] {  });
        }

        /// <summary>
        /// Description and version information about this ASCOM dome driver.
        /// </summary>
        public void FindHome()
        {
			if (IDome != null)
				IDome.FindHome();
			else
				objTypeDome.InvokeMember("FindHome", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// Open shutter or otherwise expose telescope to the sky.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        public void OpenShutter()
        {
			if (IDome != null)
				IDome.OpenShutter();
			else
				objTypeDome.InvokeMember("OpenShutter", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// Rotate dome in azimuth to park position.
        /// After assuming programmed park position, sets Dome.AtPark flag. Raises an error if Dome.Slaved is True,
        /// or if not supported, or if a communications failure has occurred. 
        /// </summary>
        public void Park()
        {
			if (IDome != null)
				IDome.Park();
			else
				objTypeDome.InvokeMember("Park", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// Set the current azimuth, altitude position of dome to be the park position.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        public void SetPark()
        {
			if (IDome != null)
				IDome.SetPark();
			else
				objTypeDome.InvokeMember("SetPark", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// Status of the dome shutter or roll-off roof.
        /// Raises an error only if no shutter control.
        /// If actual shutter status can not be read, 
        /// then reports back the last shutter state. 
        /// </summary>
        public ShutterState ShutterStatus
        {
            get
            {
				if (IDome != null)
					return IDome.ShutterStatus;
				else
					return (ShutterState)objTypeDome.InvokeMember("ShutterStatus", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// True if the dome is slaved to the telescope in its hardware, else False.
        /// Set this property to True to enable dome-telescope hardware slaving,
        /// if supported (see Dome.CanSlave). Raises an exception on any attempt to set 
        /// this property if hardware slaving is not supported).
        /// Always returns False if hardware slaving is not supported. 
        /// </summary>
        public bool Slaved
        {
            get
            {
				if (IDome != null)
					return IDome.Slaved;
				else
					return (bool)objTypeDome.InvokeMember("Slaved", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
            set
            {
				if (IDome != null)
					IDome.Slaved = value;
				else
					objTypeDome.InvokeMember("Slaved", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objDomeLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Slew the dome to the given altitude position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated altitude. 
        /// </summary>
        /// <param name="Altitude">Target dome altitude (degrees, horizon zero and increasing positive to 90 zenith)</param>
        public void SlewToAltitude(double Altitude)
        {
			if (IDome != null)
				IDome.SlewToAltitude(Altitude);
			else
				objTypeDome.InvokeMember("SlewToAltitude", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Altitude });
        }

        /// <summary>
        /// Slew the dome to the given azimuth position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SlewToAzimuth(double Azimuth)
        {
			if (IDome != null)
				IDome.SlewToAzimuth(Azimuth);
			else
				objTypeDome.InvokeMember("SlewToAzimuth", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Azimuth });
        }

        /// <summary>
        /// True if any part of the dome is currently moving, False if all dome components are steady.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        public bool Slewing
        {
            get
            {
				if (IDome != null)
					return IDome.Slewing;
				else
					return (bool)objTypeDome.InvokeMember("Slewing",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }    
        }

        /// <summary>
        /// Synchronize the current position of the dome to the given azimuth.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SyncToAzimuth(double Azimuth)
        {
			if (IDome != null)
				IDome.SyncToAzimuth(Azimuth);
			else
				objTypeDome.InvokeMember("SyncToAzimuth", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
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
			if (this.objDomeLateBound != null)
			{
				try { Marshal.ReleaseComObject(objDomeLateBound); }
				catch (Exception) { }
				objDomeLateBound = null;
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
                if (IDome != null)
                    return IDome.Connected;
                else
                    return Convert.ToBoolean(objTypeDome.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { }));
            }
            set
            {
                if (IDome != null)
                    IDome.Connected = value;
                else
                    objTypeDome.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objDomeLateBound, new object[] { value });
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
                if (IDome != null)
                    return IDome.Description;
                else
                    return Convert.ToString(objTypeDome.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { }));
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
                if (IDome != null)
                    return IDome.DriverInfo;
                else
                    return (string)objTypeDome.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { });
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
                if (IDome != null)
                    return IDome.DriverVersion;
                else
                    return (string)objTypeDome.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { });
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
                if (IDome != null)
                    return IDome.InterfaceVersion;
                else
                    return (short)objTypeDome.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { });
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
                if (IDome != null)
                    return IDome.LastResult;
                else
                    return Convert.ToString(objTypeDome.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (IDome != null)
                    return IDome.Name;
                else
                    return (string)objTypeDome.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (IDome != null)
                IDome.SetupDialog();
            else
                objTypeDome.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
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
            if (IDome != null)
                return IDome.Action(ActionName, ActionParameters);
            else
                return (string)objTypeDome.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (IDome != null)
                    return IDome.SupportedActions;
                else
                    return (string[])(objTypeDome.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objDomeLateBound, new object[] { }));
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
            if (IDome != null)
                IDome.CommandBlind(Command, Raw);
            else
                objTypeDome.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command, Raw });
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
            if (IDome != null)
                return IDome.CommandBool(Command, Raw);
            else
                return (bool)objTypeDome.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command, Raw });
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
            if (IDome != null)
                return IDome.CommandString(Command, Raw);
            else
                return (string)objTypeDome.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command, Raw });
        }

        #endregion
	}
	#endregion
}
