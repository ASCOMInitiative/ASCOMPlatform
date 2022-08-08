// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

static class DomeHardware
{
    #region IDome Implementation

    private static bool domeShutterState = false; // Variable to hold the open/closed status of the shutter, true = Open

    /// <summary>
    /// Immediately stops any and all movement of the dome.
    /// </summary>
    internal static void AbortSlew()
    {
        // This is a mandatory parameter but we have no action to take in this simple driver
        LogMessage("AbortSlew", "Completed");
    }

    /// <summary>
    /// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
    /// </summary>
    internal static double Altitude
    {
        get
        {
            LogMessage("Altitude Get", "Not implemented");
            throw new PropertyNotImplementedException("Altitude", false);
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
    internal static bool AtHome
    {
        get
        {
            LogMessage("AtHome Get", "Not implemented");
            throw new PropertyNotImplementedException("AtHome", false);
        }
    }

    /// <summary>
    /// <see langword="true" /> if the dome is in the programmed park position.
    /// </summary>
    internal static bool AtPark
    {
        get
        {
            LogMessage("AtPark Get", "Not implemented");
            throw new PropertyNotImplementedException("AtPark", false);
        }
    }

    /// <summary>
    /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
    /// </summary>
    internal static double Azimuth
    {
        get
        {
            LogMessage("Azimuth Get", "Not implemented");
            throw new PropertyNotImplementedException("Azimuth", false);
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver can perform a search for home position.
    /// </summary>
    internal static bool CanFindHome
    {
        get
        {
            LogMessage("CanFindHome Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver is capable of parking the dome.
    /// </summary>
    internal static bool CanPark
    {
        get
        {
            LogMessage("CanPark Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver is capable of setting dome altitude.
    /// </summary>
    internal static bool CanSetAltitude
    {
        get
        {
            LogMessage("CanSetAltitude Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if driver is capable of rotating the dome. Muste be <see "langword="false" /> for a 
    /// roll-off roof or clamshell.
    /// </summary>
    internal static bool CanSetAzimuth
    {
        get
        {
            LogMessage("CanSetAzimuth Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver can set the dome park position.
    /// </summary>
    internal static bool CanSetPark
    {
        get
        {
            LogMessage("CanSetPark Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver is capable of opening and closing the shutter or roof
    /// mechanism.
    /// </summary>
    internal static bool CanSetShutter
    {
        get
        {
            LogMessage("CanSetShutter Get", true.ToString());
            return true;
        }
    }

    /// <summary>
    /// <see langword="true" /> if the dome hardware supports slaving to a telescope.
    /// </summary>
    internal static bool CanSlave
    {
        get
        {
            LogMessage("CanSlave Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
    /// using the <see cref="SyncToAzimuth" /> method.
    /// </summary>
    internal static bool CanSyncAzimuth
    {
        get
        {
            LogMessage("CanSyncAzimuth Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// Close the shutter or otherwise shield the telescope from the sky.
    /// </summary>
    internal static void CloseShutter()
    {
        LogMessage("CloseShutter", "Shutter has been closed");
        domeShutterState = false;
    }

    /// <summary>
    /// Start operation to search for the dome home position.
    /// </summary>
    internal static void FindHome()
    {
        LogMessage("FindHome", "Not implemented");
        throw new MethodNotImplementedException("FindHome");
    }

    /// <summary>
    /// Open shutter or otherwise expose telescope to the sky.
    /// </summary>
    internal static void OpenShutter()
    {
        LogMessage("OpenShutter", "Shutter has been opened");
        domeShutterState = true;
    }

    /// <summary>
    /// Rotate dome in azimuth to park position.
    /// </summary>
    internal static void Park()
    {
        LogMessage("Park", "Not implemented");
        throw new MethodNotImplementedException("Park");
    }

    /// <summary>
    /// Set the current azimuth position of dome to the park position.
    /// </summary>
    internal static void SetPark()
    {
        LogMessage("SetPark", "Not implemented");
        throw new MethodNotImplementedException("SetPark");
    }

    /// <summary>
    /// Gets the status of the dome shutter or roof structure.
    /// </summary>
    internal static ShutterState ShutterStatus
    {
        get
        {
            LogMessage("ShutterStatus Get", false.ToString());
            if (domeShutterState)
            {
                LogMessage("ShutterStatus", ShutterState.shutterOpen.ToString());
                return ShutterState.shutterOpen;
            }
            else
            {
                LogMessage("ShutterStatus", ShutterState.shutterClosed.ToString());
                return ShutterState.shutterClosed;
            }
        }
    }

    /// <summary>
    /// <see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.
    /// </summary>
    internal static bool Slaved
    {
        get
        {
            LogMessage("Slaved Get", false.ToString());
            return false;
        }
        set
        {
            LogMessage("Slaved Set", "not implemented");
            throw new PropertyNotImplementedException("Slaved", true);
        }
    }

    /// <summary>
    /// Ensure that the requested viewing altitude is available for observing.
    /// </summary>
    /// <param name="Altitude">
    /// The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
    /// </param>
    internal static void SlewToAltitude(double Altitude)
    {
        LogMessage("SlewToAltitude", "Not implemented");
        throw new MethodNotImplementedException("SlewToAltitude");
    }

    /// <summary>
    /// Ensure that the requested viewing azimuth is available for observing.
    /// The method should not block and the slew operation should complete asynchronously.
    /// </summary>
    /// <param name="Azimuth">
    /// Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
    /// 180 South, 270 West)
    /// </param>
    internal static void SlewToAzimuth(double Azimuth)
    {
        LogMessage("SlewToAzimuth", "Not implemented");
        throw new MethodNotImplementedException("SlewToAzimuth");
    }

    /// <summary>
    /// <see langword="true" /> if any part of the dome is currently moving or a move command has been issued, 
    /// but the dome has not yet started to move. <see langword="false" /> if all dome components are stationary
    /// and no move command has been issued. /> 
    /// </summary>
    internal static bool Slewing
    {
        get
        {
            LogMessage("Slewing Get", false.ToString());
            return false;
        }
    }

    /// <summary>
    /// Synchronize the current position of the dome to the given azimuth.
    /// </summary>
    /// <param name="Azimuth">
    /// Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
    /// 180 South, 270 West)
    /// </param>
    internal static void SyncToAzimuth(double Azimuth)
    {
        LogMessage("SyncToAzimuth", "Not implemented");
        throw new MethodNotImplementedException("SyncToAzimuth");
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