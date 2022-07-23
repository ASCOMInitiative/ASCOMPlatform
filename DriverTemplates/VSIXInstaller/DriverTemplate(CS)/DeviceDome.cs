// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceDome
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

    #region IDome Implementation

    private bool domeShutterState = false; // Variable to hold the open/closed status of the shutter, true = Open

	/// <summary>
	/// Immediately stops any and all movement of the dome.
	/// </summary>
	public void AbortSlew()
    {
        // This is a mandatory parameter but we have no action to take in this simple driver
        tl.LogMessage("AbortSlew", "Completed");
    }

	/// <summary>
	/// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
	/// </summary>
	public double Altitude
    {
        get
        {
            tl.LogMessage("Altitude Get", "Not implemented");
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
	public bool AtHome
    {
        get
        {
            tl.LogMessage("AtHome Get", "Not implemented");
            throw new PropertyNotImplementedException("AtHome", false);
        }
    }

	/// <summary>
	/// <see langword="true" /> if the dome is in the programmed park position.
	/// </summary>
	public bool AtPark
    {
        get
        {
            tl.LogMessage("AtPark Get", "Not implemented");
            throw new PropertyNotImplementedException("AtPark", false);
        }
    }

	/// <summary>
	/// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
	/// </summary>
	public double Azimuth
    {
        get
        {
            tl.LogMessage("Azimuth Get", "Not implemented");
            throw new PropertyNotImplementedException("Azimuth", false);
        }
    }

	/// <summary>
	/// <see langword="true" /> if driver can perform a search for home position.
	/// </summary>
	public bool CanFindHome
    {
        get
        {
            tl.LogMessage("CanFindHome Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver is capable of parking the dome.
	/// </summary>
	public bool CanPark
    {
        get
        {
            tl.LogMessage("CanPark Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// <see langword="true" /> if driver is capable of setting dome altitude.
	/// </summary>
	public bool CanSetAltitude
    {
        get
        {
            tl.LogMessage("CanSetAltitude Get", false.ToString());
            return false;
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
            tl.LogMessage("CanSetAzimuth Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the driver can set the dome park position.
	/// </summary>
	public bool CanSetPark
    {
        get
        {
            tl.LogMessage("CanSetPark Get", false.ToString());
            return false;
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
            tl.LogMessage("CanSetShutter Get", true.ToString());
            return true;
        }
    }

	/// <summary>
	/// <see langword="true" /> if the dome hardware supports slaving to a telescope.
	/// </summary>
	public bool CanSlave
    {
        get
        {
            tl.LogMessage("CanSlave Get", false.ToString());
            return false;
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
            tl.LogMessage("CanSyncAzimuth Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Close the shutter or otherwise shield the telescope from the sky.
	/// </summary>
	public void CloseShutter()
    {
        tl.LogMessage("CloseShutter", "Shutter has been closed");
        domeShutterState = false;
    }

	/// <summary>
	/// Start operation to search for the dome home position.
	/// </summary>
	public void FindHome()
    {
        tl.LogMessage("FindHome", "Not implemented");
        throw new MethodNotImplementedException("FindHome");
    }

	/// <summary>
	/// Open shutter or otherwise expose telescope to the sky.
	/// </summary>
	public void OpenShutter()
    {
        tl.LogMessage("OpenShutter", "Shutter has been opened");
        domeShutterState = true;
    }

	/// <summary>
	/// Rotate dome in azimuth to park position.
	/// </summary>
    public void Park()
    {
        tl.LogMessage("Park", "Not implemented");
        throw new MethodNotImplementedException("Park");
    }

	/// <summary>
	/// Set the current azimuth position of dome to the park position.
	/// </summary>
	public void SetPark()
    {
        tl.LogMessage("SetPark", "Not implemented");
        throw new MethodNotImplementedException("SetPark");
    }

	/// <summary>
	/// Gets the status of the dome shutter or roof structure.
	/// </summary>
	public ShutterState ShutterStatus
    {
        get
        {
            tl.LogMessage("ShutterStatus Get", false.ToString());
            if (domeShutterState)
            {
                tl.LogMessage("ShutterStatus", ShutterState.shutterOpen.ToString());
                return ShutterState.shutterOpen;
            }
            else
            {
                tl.LogMessage("ShutterStatus", ShutterState.shutterClosed.ToString());
                return ShutterState.shutterClosed;
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
            tl.LogMessage("Slaved Get", false.ToString());
            return false;
        }
        set
        {
            tl.LogMessage("Slaved Set", "not implemented");
            throw new PropertyNotImplementedException("Slaved", true);
        }
    }

	/// <summary>
	/// Ensure that the requested viewing altitude is available for observing.
	/// </summary>
	/// <param name="Altitude">
	/// The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
	/// </param>
	public void SlewToAltitude(double Altitude)
    {
        tl.LogMessage("SlewToAltitude", "Not implemented");
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
	public void SlewToAzimuth(double Azimuth)
    {
        tl.LogMessage("SlewToAzimuth", "Not implemented");
        throw new MethodNotImplementedException("SlewToAzimuth");
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
            tl.LogMessage("Slewing Get", false.ToString());
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
	public void SyncToAzimuth(double Azimuth)
    {
        tl.LogMessage("SyncToAzimuth", "Not implemented");
        throw new MethodNotImplementedException("SyncToAzimuth");
    }

    #endregion

    //ENDOFINSERTEDFILE
}