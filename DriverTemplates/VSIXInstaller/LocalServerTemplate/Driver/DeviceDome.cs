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
        try
        {
            CheckConnected("AbortSlew");
            LogMessage("AbortSlew", $"Calling method.");
            DomeHardware.AbortSlew();
            LogMessage("AbortSlew", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("AbortSlew", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
    /// </summary>
    public double Altitude
    {
        get
        {
            try
            {
                CheckConnected("Altitude");
                double altitude = DomeHardware.Altitude;
                LogMessage("Altitude", altitude.ToString());
                return altitude;
            }
            catch (Exception ex)
            {
                LogMessage("Altitude", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("AtHome");
                bool atHome = DomeHardware.AtHome;
                LogMessage("AtHome", atHome.ToString());
                return atHome;
            }
            catch (Exception ex)
            {
                LogMessage("AtHome", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if the dome is in the programmed park position.
    /// </summary>
    public bool AtPark
    {
        get
        {
            try
            {
                CheckConnected("AtPark");
                bool atPark = DomeHardware.AtPark;
                LogMessage("AtPark", atPark.ToString());
                return atPark;
            }
            catch (Exception ex)
            {
                LogMessage("AtPark", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
    /// </summary>
    public double Azimuth
    {
        get
        {
            try
            {
                CheckConnected("Azimuth");
                double azimuth = DomeHardware.Azimuth;
                LogMessage("Azimuth", azimuth.ToString());
                return azimuth;
            }
            catch (Exception ex)
            {
                LogMessage("Azimuth", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver can perform a search for home position.
    /// </summary>
    public bool CanFindHome
    {
        get
        {
            try
            {
                CheckConnected("CanFindHome");
                bool canFindHome = DomeHardware.CanFindHome;
                LogMessage("CanFindHome", canFindHome.ToString());
                return canFindHome;
            }
            catch (Exception ex)
            {
                LogMessage("CanFindHome", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver is capable of parking the dome.
    /// </summary>
    public bool CanPark
    {
        get
        {
            try
            {
                CheckConnected("CanPark");
                bool canPark = DomeHardware.CanPark;
                LogMessage("CanPark", canPark.ToString());
                return canPark;
            }
            catch (Exception ex)
            {
                LogMessage("CanPark", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver is capable of setting dome altitude.
    /// </summary>
    public bool CanSetAltitude
    {
        get
        {
            try
            {
                CheckConnected("CanSetAltitude");
                bool canSetAltitude = DomeHardware.CanSetAltitude;
                LogMessage("CanSetAltitude", canSetAltitude.ToString());
                return canSetAltitude;
            }
            catch (Exception ex)
            {
                LogMessage("CanSetAltitude", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver is capable of rotating the dome. Muste be <see "langword="false" /> for a 
    /// roll-off roof or clamshell.
    /// </summary>
    public bool CanSetAzimuth
    {
        get
        {
            try
            {
                CheckConnected("CanSetAzimuth");
                bool canSetAzimuth = DomeHardware.CanSetAzimuth;
                LogMessage("CanSetAzimuth", canSetAzimuth.ToString());
                return canSetAzimuth;
            }
            catch (Exception ex)
            {
                LogMessage("CanSetAzimuth", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver can set the dome park position.
    /// </summary>
    public bool CanSetPark
    {
        get
        {
            try
            {
                CheckConnected("CanSetPark");
                bool canSetPark = DomeHardware.CanSetPark;
                LogMessage("CanSetPark", canSetPark.ToString());
                return canSetPark;
            }
            catch (Exception ex)
            {
                LogMessage("CanSetPark", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("CanSetShutter");
                bool canSetShutter = DomeHardware.CanSetShutter;
                LogMessage("CanSetShutter", canSetShutter.ToString());
                return canSetShutter;
            }
            catch (Exception ex)
            {
                LogMessage("CanSetShutter", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true" /> if the dome hardware supports slaving to a telescope.
    /// </summary>
    public bool CanSlave
    {
        get
        {
            try
            {
                CheckConnected("CanSlave");
                bool canSlave = DomeHardware.CanSlave;
                LogMessage("CanSlave", canSlave.ToString());
                return canSlave;
            }
            catch (Exception ex)
            {
                LogMessage("CanSlave", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("CanSyncAzimuth");
                bool canSyncAzimuth = DomeHardware.CanSyncAzimuth;
                LogMessage("CanSyncAzimuth", canSyncAzimuth.ToString());
                return canSyncAzimuth;
            }
            catch (Exception ex)
            {
                LogMessage("CanSyncAzimuth", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Close the shutter or otherwise shield the telescope from the sky.
    /// </summary>
    public void CloseShutter()
    {
        try
        {
            CheckConnected("CloseShutter");
            LogMessage("CloseShutter", $"Calling method.");
            DomeHardware.CloseShutter();
            LogMessage("CloseShutter", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("CloseShutter", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Start operation to search for the dome home position.
    /// </summary>
    public void FindHome()
    {
        try
        {
            CheckConnected("FindHome");
            LogMessage("FindHome", $"Calling method.");
            DomeHardware.FindHome();
            LogMessage("FindHome", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("FindHome", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Open shutter or otherwise expose telescope to the sky.
    /// </summary>
    public void OpenShutter()
    {
        try
        {
            CheckConnected("OpenShutter");
            LogMessage("OpenShutter", $"Calling method.");
            DomeHardware.OpenShutter();
            LogMessage("OpenShutter", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("OpenShutter", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Rotate dome in azimuth to park position.
    /// </summary>
    public void Park()
    {
        try
        {
            CheckConnected("Park");
            LogMessage("Park", $"Calling method.");
            DomeHardware.Park();
            LogMessage("Park", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("Park", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Set the current azimuth position of dome to the park position.
    /// </summary>
    public void SetPark()
    {
        try
        {
            CheckConnected("SetPark");
            LogMessage("SetPark", $"Calling method.");
            DomeHardware.SetPark();
            LogMessage("SetPark", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SetPark", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Gets the status of the dome shutter or roof structure.
    /// </summary>
    public ShutterState ShutterStatus
    {
        get
        {
            try
            {
                CheckConnected("ShutterStatus");
                ShutterState shutterSttaus = DomeHardware.ShutterStatus;
                LogMessage("ShutterStatus", shutterSttaus.ToString());
                return shutterSttaus;
            }
            catch (Exception ex)
            {
                LogMessage("ShutterStatus", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// <see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.
    /// </summary>
    public bool Slaved
    {
        get
        {
            try
            {
                CheckConnected("Slaved Get");
                bool slaved = DomeHardware.Slaved;
                LogMessage("Slaved Get", slaved.ToString());
                return slaved;
            }
            catch (Exception ex)
            {
                LogMessage("Slaved Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
        set
        {
            try
            {
                CheckConnected("Slaved Set");
                LogMessage("Slaved Set", value.ToString());
                DomeHardware.Slaved = value;
            }
            catch (Exception ex)
            {
                LogMessage("Slaved Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
        try
        {
            CheckConnected("SlewToAltitude");
            LogMessage("SlewToAltitude", $"Calling method.");
            DomeHardware.SlewToAltitude(altitude);
            LogMessage("SlewToAltitude", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SlewToAltitude", $"Threw an exception: \r\n{ex}");
            throw;
        }
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
        try
        {
            CheckConnected("SlewToAzimuth");
            LogMessage("SlewToAzimuth", $"Calling method.");
            DomeHardware.SlewToAzimuth(azimuth);
            LogMessage("SlewToAzimuth", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SlewToAzimuth", $"Threw an exception: \r\n{ex}");
            throw;
        }
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
            try
            {
                CheckConnected("Slewing");
                bool slewing = DomeHardware.Slewing;
                LogMessage("Slewing", slewing.ToString());
                return slewing;
            }
            catch (Exception ex)
            {
                LogMessage("Slewing", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
        try
        {
            CheckConnected("SyncToAzimuth");
            LogMessage("SyncToAzimuth", $"Calling method.");
            DomeHardware.SyncToAzimuth(azimuth);
            LogMessage("SyncToAzimuth", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("SyncToAzimuth", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }

    /// <summary>
    /// Dummy CheckConnected class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    /// <param name="message"></param>
    private void CheckConnected(string message)
    {
    }
}