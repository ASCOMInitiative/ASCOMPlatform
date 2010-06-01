//-----------------------------------------------------------------------
// <summary>Defines the IDome Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    /// <summary>
    /// Defines the IDome Interface
    /// </summary>
    public interface IDome : IAscomDriver, IDeviceControl
    {
        /// <summary>
        /// Immediately cancel current dome operation.
        /// Calling this method will immediately disable hardware slewing (Dome.Slaved will become False).
        /// Raises an error if a communications failure occurs, or if the command is known to have failed. 
        /// </summary>
        void AbortSlew();

        /// <summary>
        /// The dome altitude (degrees, horizon zero and increasing positive to 90 zenith).
        /// Raises an error only if no altitude control. If actual dome altitude can not be read,
        /// then reports back the last slew position. 
        /// </summary>
        double Altitude { get; }

        /// <summary>
        /// True if the dome is in the Home position.
        /// Set only following a Dome.FindHome operation and reset with any azimuth slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        bool AtHome { get; }

        /// <summary>
        /// True if the dome is in the programmed park position.
        /// Set only following a Dome.Park operation and reset with any slew operation.
        /// Raises an error if not supported. 
        /// </summary>
        bool AtPark { get; }

        /// <summary>
        /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
        /// </summary>
        double Azimuth { get; }

        /// <summary>
        /// True if driver can do a search for home position.
        /// </summary>
        bool CanFindHome { get; }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        bool CanPark { get; }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        bool CanSetAltitude { get; }

        /// <summary>
        /// True if driver is capable of setting dome azimuth.
        /// </summary>
        bool CanSetAzimuth { get; }

        /// <summary>
        /// True if driver can set the dome park position.
        /// </summary>
        bool CanSetPark { get; }

        /// <summary>
        /// True if driver is capable of automatically operating shutter.
        /// </summary>
        bool CanSetShutter { get; }

        /// <summary>
        /// True if the dome hardware supports slaving to a telescope.
        /// </summary>
        bool CanSlave { get; }

        /// <summary>
        /// True if driver is capable of synchronizing the dome azimuth position using the Dome.SyncToAzimuth method.
        /// </summary>
        bool CanSyncAzimuth { get; }

        /// <summary>
        /// Close shutter or otherwise shield telescope from the sky.
        /// </summary>
        void CloseShutter();

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Description and version information about this ASCOM dome driver.
        /// </summary>
        void FindHome();

        /// <summary>
        /// Open shutter or otherwise expose telescope to the sky.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        void OpenShutter();

        /// <summary>
        /// Rotate dome in azimuth to park position.
        /// After assuming programmed park position, sets Dome.AtPark flag. Raises an error if Dome.Slaved is True,
        /// or if not supported, or if a communications failure has occurred. 
        /// </summary>
        void Park();

        /// <summary>
        /// Set the current azimuth, altitude position of dome to be the park position.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        void SetPark();

        /// <summary>
        /// Status of the dome shutter or roll-off roof.
        /// Raises an error only if no shutter control.
        /// If actual shutter status can not be read, 
        /// then reports back the last shutter state. 
        /// </summary>
        ShutterState ShutterStatus { get; }

        /// <summary>
        /// True if the dome is slaved to the telescope in its hardware, else False.
        /// Set this property to True to enable dome-telescope hardware slaving,
        /// if supported (see Dome.CanSlave). Raises an exception on any attempt to set 
        /// this property if hardware slaving is not supported).
        /// Always returns False if hardware slaving is not supported. 
        /// </summary>
        bool Slaved { get; set; }
        
        /// <summary>
        /// True if any part of the dome is currently moving, False if all dome components are steady.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        bool Slewing { get; }

        /// <summary>
        /// Slew the dome to the given altitude position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated altitude. 
        /// </summary>
        /// <param name="Altitude">Target dome altitude (degrees, horizon zero and increasing positive to 90 zenith)</param>
        void SlewToAltitude(double Altitude);

        /// <summary>
        /// Slew the dome to the given azimuth position.
        /// Raises an error if Dome.Slaved is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        void SlewToAzimuth(double Azimuth);

        /// <summary>
        /// Synchronize the current position of the dome to the given azimuth.
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        void SyncToAzimuth(double Azimuth);

    }
}
