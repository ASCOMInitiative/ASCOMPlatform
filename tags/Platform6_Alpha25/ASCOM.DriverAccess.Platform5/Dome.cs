//-----------------------------------------------------------------------
// <summary>Defines the Dome class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.
//
using System;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region Dome wrapper

    /// <summary>
    /// Provides universal access to ASCOM Dome drivers
    /// </summary>
    public class Dome : IDome, IDisposable
    {
        #region IDome constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Constructor for Dome class. Creates a Dome based on the ProgID in the DomeID string.
        /// </summary>
        /// <param name="domeId">The progID of the dome to be instantiated</param>
        public Dome(string domeId)
        {
            _memberFactory = new MemberFactory(domeId);
        }

        /// <summary>
        /// Shows the ASCOM Chooser to select a Dome.
        /// </summary>
        /// <param name="domeId">Prog ID of the default dome to select. Null if no default is to be set.</param>
        /// <returns>The Prog ID of the Dome chosen, or Null if no dome is chose, or the dialog is canceled.</returns>
        public static string Choose(string domeId)
        {
            var oChooser = new Chooser {DeviceType = "Dome"};
            return oChooser.Choose(domeId);
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

        #region IDome Members

        /// <summary>
        /// Immediately cancel current dome operation.
        /// Calling this method will immediately disable hardware slewing (Dome.Slaved will become False).
        /// Raises an error if a communications failure occurs, or if the command is known to have failed. 
        /// </summary>
        public void AbortSlew()
        {
            _memberFactory.CallMember(3, "AbortSlew", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// The dome altitude (degrees, horizon zero and increasing positive to 90 zenith).
        /// Raises an error only if no altitude control. If actual dome altitude can not be read,
        /// then reports back the last slew position. 
        /// </summary>
        public double Altitude
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "Altitude", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// True if the dome is in the Home position.
        /// Set only following a Dome.FindHome operation and reset with any azimuth slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        public bool AtHome
        {
            get { return (bool) _memberFactory.CallMember(1, "AtHome", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if the dome is in the programmed park position.
        /// Set only following a Dome.Park operation and reset with any slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        public bool AtPark
        {
            get { return (bool) _memberFactory.CallMember(1, "AtPark", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
        /// </summary>
        public double Azimuth
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "Azimuth", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// True if driver can do a search for home position.
        /// </summary>
        public bool CanFindHome
        {
            get { return (bool) _memberFactory.CallMember(1, "CanFindHome", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanPark
        {
            get { return (bool) _memberFactory.CallMember(1, "CanPark", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanSetAltitude
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSetAltitude", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver is capable of setting dome azimuth.
        /// </summary>
        public bool CanSetAzimuth
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSetAzimuth", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver can set the dome park position.
        /// </summary>
        public bool CanSetPark
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSetPark", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver is capable of automatically operating shutter.
        /// </summary>
        public bool CanSetShutter
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSetShutter", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if the dome hardware supports slaving to a telescope.
        /// </summary>
        public bool CanSlave
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSlave", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// True if driver is capable of synchronizing the dome azimuth position using the Dome.SyncToAzimuth method.
        /// </summary>
        public bool CanSyncAzimuth
        {
            get { return (bool) _memberFactory.CallMember(1, "CanSyncAzimuth", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// Close shutter or otherwise shield telescope from the sky.
        /// </summary>
        public void CloseShutter()
        {
            _memberFactory.CallMember(3, "CloseShutter", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Send a string command directly to the dome without expecting response data.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="command"></param>
        public void CommandBlind(string command)
        {
            _memberFactory.CallMember(3, "CommandBlind", new[] {typeof (string)}, new object[] {command});
        }


        /// <summary>
        /// Send a string command directly to the dome, returning a True / False response.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="command">Raw command string to be sent to the dome.</param>
        /// <returns>True if the response indicated True or success, else False.</returns>
        public bool CommandBool(string command)
        {
            return (bool) _memberFactory.CallMember(3, "CommandBool", new[] {typeof (string)}, new object[] {command});
        }

        /// <summary>
        /// Send a string command directly to the dome, returning the response string.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="command">Raw command string to be sent to the dome.</param>
        /// <returns>Response string from controller.</returns>
        public string CommandString(string command)
        {
            return
                (string) _memberFactory.CallMember(3, "CommandString", new[] {typeof (string)}, new object[] {command});
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            _memberFactory.CallMember(3, "CommandBlind", new[] { typeof(string), typeof(bool) }, new object[] { command });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string command, bool raw)
        {
            return (bool)_memberFactory.CallMember(3, "CommandBool", new[] { typeof(string), typeof(bool) }, new object[] { command });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="command">The literal command string to be transmitted.</param>
        /// <param name="raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            return (string)_memberFactory.CallMember(3, "CommandString", new[] { typeof(string), typeof(bool) }, new object[] { command });
        }

        /// <summary>
        /// True if driver has established communication to dome control.
        /// Set to True to establish the link and set to False to terminate the link.
        /// Raises an error if connect fails. 
        /// </summary>
        public bool Connected
        {
            get { return (bool) _memberFactory.CallMember(1, "Connected", new Type[] {}, new object[] {}); }
            set { _memberFactory.CallMember(2, "Connected", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        /// A long description of the dome hardware / software or whatever.
        /// </summary>
        public string Description
        {
            get { return (string) _memberFactory.CallMember(1, "Description", new[] {typeof (string)}, new object[] {}); }
        }

        /// <summary>
        /// Description and version information about this ASCOM dome driver.
        /// </summary>
        public string DriverInfo
        {
            get { return (string) _memberFactory.CallMember(1, "DriverInfo", new[] {typeof (string)}, new object[] {}); }
        }

        /// <summary>
        /// Description and version information about this ASCOM dome driver.
        /// </summary>
        public void FindHome()
        {
            _memberFactory.CallMember(3, "FindHome", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// The ASCOM Standard interface version supported by this driver.
        /// Returns 1 for this interface version. 
        /// </summary>
        public short InterfaceVersion
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "InterfaceVersion", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// Short name for the dome.
        /// </summary>
        public string Name
        {
            get { return (string) _memberFactory.CallMember(1, "Name", new[] {typeof (string)}, new object[] {}); }
        }

        /// <summary>
        /// Open shutter or otherwise expose telescope to the sky.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        public void OpenShutter()
        {
            _memberFactory.CallMember(3, "OpenShutter", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Rotate dome in azimuth to park position.
        /// After assuming programmed park position, sets Dome.AtPark flag. Raises an error if Dome.Slaved is True,
        /// or if not supported, or if a communications failure has occurred. 
        /// </summary>
        public void Park()
        {
            _memberFactory.CallMember(3, "Park", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Set the current azimuth, altitude position of dome to be the park position.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        public void SetPark()
        {
            _memberFactory.CallMember(3, "SetPark", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Brings up a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// </summary>
        public void SetupDialog()
        {
            _memberFactory.CallMember(3, "SetupDialog", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Status of the dome shutter or roll-off roof.
        /// Raises an error only if no shutter control.
        /// If actual shutter status can not be read, 
        /// then reports back the last shutter state. 
        /// </summary>
        public ShutterState ShutterStatus
        {
            get { return (ShutterState) _memberFactory.CallMember(1, "ShutterStatus", new Type[] {}, new object[] {}); }
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
            get { return (bool) (_memberFactory.CallMember(1, "Slaved", new Type[] {}, new object[] {})); }
            set { _memberFactory.CallMember(2, "Slaved", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        /// Slew the dome to the given altitude position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated altitude. 
        /// </summary>
        /// <param name="altitude">Target dome altitude (degrees, horizon zero and increasing positive to 90 zenith)</param>
        public void SlewToAltitude(double altitude)
        {
            _memberFactory.CallMember(3, "SlewToAltitude", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Slew the dome to the given azimuth position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        /// <param name="azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SlewToAzimuth(double azimuth)
        {
            _memberFactory.CallMember(3, "SlewToAzimuth", new[] {typeof (double)}, new object[] {azimuth});
        }

        /// <summary>
        /// True if any part of the dome is currently moving, False if all dome components are steady.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        public bool Slewing
        {
            get { return (bool) (_memberFactory.CallMember(1, "Slewing", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// Synchronize the current position of the dome to the given azimuth.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SyncToAzimuth(double azimuth)
        {
            _memberFactory.CallMember(3, "SyncToAzimuth", new[] {typeof (double)}, new object[] {azimuth});
        }

        #endregion
    }

    #endregion
}