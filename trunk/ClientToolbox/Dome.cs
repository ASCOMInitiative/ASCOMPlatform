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
	#region Dome wrapper
	/// <summary>
	/// Provides universal access to ASCOM Dome drivers
	/// </summary>
	public class Dome : ASCOM.Interface.IDome, IDisposable
    {

        object objDomeLateBound;
		ASCOM.Interface.IDome IDome;
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
				IDome = (ASCOM.Interface.IDome)objDomeLateBound;
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
			oChooser.DeviceTypeV = "Dome";				// Requires Helper 5.0.3 (May '07)
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
        /// Send a string command directly to the dome without expecting response data.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="Command"></param>
        public void CommandBlind(string Command)
        {
			if (IDome != null)
				IDome.CommandBlind(Command);
			else
				objTypeDome.InvokeMember("CommandString", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command });
        }


        /// <summary>
        /// Send a string command directly to the dome, returning a True / False response.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="Command">Raw command string to be sent to the dome.</param>
        /// <returns>True if the response indicated True or success, else False.</returns>
        public bool CommandBool(string Command)
        {
			if (IDome != null)
				return IDome.CommandBool(Command);
			else
				return (bool)objTypeDome.InvokeMember("CommandString", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command });
        }

        /// <summary>
        /// Send a string command directly to the dome, returning the response string.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="Command">Raw command string to be sent to the dome.</param>
        /// <returns>Response string from controller.</returns>
        public string CommandString(string Command)
        {
			if (IDome != null)
				return IDome.CommandString(Command);
			else
				return (string)objTypeDome.InvokeMember("CommandString", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objDomeLateBound, new object[] { Command });
        }

        /// <summary>
        /// True if driver has established communication to dome control.
        /// Set to True to establish the link and set to False to terminate the link.
        /// Raises an error if connect fails. 
        /// </summary>
        public bool Connected
        {
            get
            {
				if (IDome != null)
					return IDome.Connected;
				else
					return (bool)objTypeDome.InvokeMember("Connected", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
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
        /// A long description of the dome hardware / software or whatever.
        /// </summary>
        public string Description
        {
            get
            {
				if (IDome != null)
					return IDome.Description;
				else
					return (string)objTypeDome.InvokeMember("Description", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objDomeLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Description and version information about this ASCOM dome driver.
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
        /// The ASCOM Standard interface version supported by this driver.
        /// Returns 1 for this interface version. 
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
        /// Short name for the dome.
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
        /// Brings up a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// </summary>
        public void SetupDialog()
        {
			if (IDome != null)
				IDome.SetupDialog();
			else
				objTypeDome.InvokeMember("SetupDialog", 
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
	}
	#endregion
}
