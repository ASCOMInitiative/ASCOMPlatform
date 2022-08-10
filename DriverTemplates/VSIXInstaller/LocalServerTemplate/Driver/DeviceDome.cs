// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceDome
{
    #region IDome Implementation

	/// <summary>
	/// Immediately stops any and all movement of the dome.
	/// </summary>
	public void AbortSlew()
    {
        LogMessage("AbortSlew", $"Calling method.");
        DomeHardware.AbortSlew();
        LogMessage("AbortSlew", $"Completed.");
    }

    /// <summary>
    /// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
    /// </summary>
    public double Altitude
    {
        get
        {
            double altitude = DomeHardware.Altitude;
            LogMessage("Altitude Get", altitude.ToString());
            return altitude;
        }
    }

	/// <summary>
	/// <para><see langword="true" /> when the dome is in the home position. Raises an error if not supported.</para>
	/// <para>
	/// This is normally used following a <see cref="FindHome" /> operation. The value is reset
	/// with any azimuth slew operation that moves the dome away from the home position.
	/// </para>
	/// <para>
	/// <see cref="AtHome" /> may optionally also become true during normal slew operations, if the
	/// dome passes through the home position and the dome controller hardware is capable of
	/// detecting that; or at the end of a slew operation if the dome comes to rest at the home
	/// position.
	/// </para>
	/// </summary>
	public bool AtHome
    {
        get
        {
            bool atHome = DomeHardware.AtHome;
            LogMessage("AtHome Get", atHome.ToString());
            return atHome;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the dome is in the programmed park position.
	/// </summary>
	public bool AtPark
    {
        get
        {
            bool atPark = DomeHardware.AtPark;
            LogMessage("AtPark Get", atPark.ToString());
            return atPark;
        }
    }

	/// <summary>
	/// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
	/// </summary>
	public double Azimuth
    {
        get
        {
            double azimuth = DomeHardware.Azimuth;
            LogMessage("Azimuth Get", azimuth.ToString());
            return azimuth;
        }
    }

	/// <summary>
	/// <see langword="true" /> if driver can perform a search for home position.
	/// </summary>
	public bool CanFindHome
    {
        get
        {
            bool canFindHome = DomeHardware.CanFindHome;
            LogMessage("CanFindHome Get", canFindHome.ToString());
            return canFindHome;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver is capable of parking the dome.
	/// </summary>
	public bool CanPark
    {
        get
        {
            bool canPark = DomeHardware.CanPark;
            LogMessage("CanPark Get", canPark.ToString());
            return canPark;
        }
    }

	/// <summary>
	/// <see langword="true" /> if driver is capable of setting dome altitude.
	/// </summary>
	public bool CanSetAltitude
    {
        get
        {
            bool canSetAltitude = DomeHardware.CanSetAltitude;
            LogMessage("CanSetAltitude Get", canSetAltitude.ToString());
            return canSetAltitude;
        }
    }

	/// <summary>
	/// <see langword="true" /> if driver is capable of rotating the dome. Must be <see "langword="false" /> for a 
	/// roll-off roof or clamshell.
	/// </summary>
	public bool CanSetAzimuth
    {
        get
        {
            bool canSetAzimuth = DomeHardware.CanSetAzimuth;
            LogMessage("CanSetAzimuth Get", canSetAzimuth.ToString());
            return canSetAzimuth;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver can set the dome park position.
	/// </summary>
	public bool CanSetPark
    {
        get
        {
            bool canSetPark = DomeHardware.CanSetPark;
            LogMessage("CanSetPark Get", canSetPark.ToString());
            return canSetPark;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver is capable of opening and closing the shutter or roof
	/// mechanism.
	/// </summary>
	public bool CanSetShutter
    {
        get
        {
            bool canSetShutter = DomeHardware.CanSetShutter;
            LogMessage("CanSetShutter Get", canSetShutter.ToString());
            return canSetShutter;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the dome hardware supports slaving to a telescope.
	/// </summary>
	public bool CanSlave
    {
        get
        {
            bool canSlave = DomeHardware.CanSlave;
            LogMessage("CanSlave Get", canSlave.ToString());
            return canSlave;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
	/// using the <see cref="SyncToAzimuth" /> method.
	/// </summary>
	public bool CanSyncAzimuth
    {
        get
        {
            bool canSyncAzimuth = DomeHardware.CanSyncAzimuth;
            LogMessage("CanSyncAzimuth Get", canSyncAzimuth.ToString());
            return canSyncAzimuth;
        }
    }

	/// <summary>
	/// Close the shutter or otherwise shield the telescope from the sky.
	/// </summary>
	public void CloseShutter()
    {
        LogMessage("CloseShutter", $"Calling method.");
        DomeHardware.CloseShutter();
        LogMessage("CloseShutter", $"Completed.");
    }

    /// <summary>
    /// Start operation to search for the dome home position.
    /// </summary>
    public void FindHome()
    {
        LogMessage("FindHome", $"Calling method.");
        DomeHardware.FindHome();
        LogMessage("FindHome", $"Completed.");
    }

    /// <summary>
    /// Open shutter or otherwise expose telescope to the sky.
    /// </summary>
    public void OpenShutter()
    {
        LogMessage("OpenShutter", $"Calling method.");
        DomeHardware.OpenShutter();
        LogMessage("OpenShutter", $"Completed.");
    }

    /// <summary>
    /// Rotate dome in azimuth to park position.
    /// </summary>
    public void Park()
    {
        LogMessage("Park", $"Calling method.");
        DomeHardware.Park();
        LogMessage("Park", $"Completed.");
    }

    /// <summary>
    /// Set the current azimuth position of dome to the park position.
    /// </summary>
    public void SetPark()
    {
        LogMessage("SetPark", $"Calling method.");
        DomeHardware.SetPark();
        LogMessage("SetPark", $"Completed.");
    }

    /// <summary>
    /// Gets the status of the dome shutter or roof structure.
    /// </summary>
    public ShutterState ShutterStatus
    {
        get
        {
            ShutterState shutterSttaus = DomeHardware.ShutterStatus;
            LogMessage("ShutterStatus Get", shutterSttaus.ToString());
            return shutterSttaus;
        }
    }

	/// <summary>
	/// <see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.
	/// </summary>
	public bool Slaved
    {
        get
        {
            bool slaved = DomeHardware.Slaved;
            LogMessage("Slaved Get", slaved.ToString());
            return slaved;
        }
        set
        {
            LogMessage("Slaved Set", value.ToString());
            DomeHardware.Slaved = value;
        }
    }

	/// <summary>
	/// Ensure that the requested viewing altitude is available for observing.
	/// </summary>
	/// <param name="altitude">
	/// The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
	/// </param>
	public void SlewToAltitude(double altitude)
    {
        LogMessage("SlewToAltitude", $"Calling method.");
        DomeHardware.SlewToAltitude(altitude);
        LogMessage("SlewToAltitude", $"Completed.");
    }

    /// <summary>
    /// Ensure that the requested viewing azimuth is available for observing.
    /// The method should not block and the slew operation should complete asynchronously.
    /// </summary>
    /// <param name="azimuth">
    /// Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
    /// 180 South, 270 West)
    /// </param>
    public void SlewToAzimuth(double azimuth)
    {
        LogMessage("SlewToAzimuth", $"Calling method.");
        DomeHardware.SlewToAzimuth(azimuth);
        LogMessage("SlewToAzimuth", $"Completed.");
    }

    /// <summary>
    /// <see langword="true" /> if any part of the dome is currently moving or a move command has been issued, 
    /// but the dome has not yet started to move. <see langword="false" /> if all dome components are stationary
    /// and no move command has been issued. /> 
    /// </summary>
    public bool Slewing
    {
        get
        {
            bool slewing = DomeHardware.Slewing;
            LogMessage("Slewing Get", slewing.ToString());
            return slewing;
        }
    }

	/// <summary>
	/// Synchronize the current position of the dome to the given azimuth.
	/// </summary>
	/// <param name="azimuth">
	/// Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
	/// 180 South, 270 West)
	/// </param>
	public void SyncToAzimuth(double azimuth)
    {
        LogMessage("SyncToAzimuth", $"Calling method.");
        DomeHardware.SyncToAzimuth(azimuth);
        LogMessage("SyncToAzimuth", $"Completed.");
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }
}