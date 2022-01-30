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

    public void AbortSlew()
    {
        // This is a mandatory parameter but we have no action to take in this simple driver
        tl.LogMessage("AbortSlew", "Completed");
    }

    public double Altitude
    {
        get
        {
            tl.LogMessage("Altitude Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Altitude", false);
        }
    }

    public bool AtHome
    {
        get
        {
            tl.LogMessage("AtHome Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("AtHome", false);
        }
    }

    public bool AtPark
    {
        get
        {
            tl.LogMessage("AtPark Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("AtPark", false);
        }
    }

    public double Azimuth
    {
        get
        {
            tl.LogMessage("Azimuth Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Azimuth", false);
        }
    }

    public bool CanFindHome
    {
        get
        {
            tl.LogMessage("CanFindHome Get", false.ToString());
            return false;
        }
    }

    public bool CanPark
    {
        get
        {
            tl.LogMessage("CanPark Get", false.ToString());
            return false;
        }
    }

    public bool CanSetAltitude
    {
        get
        {
            tl.LogMessage("CanSetAltitude Get", false.ToString());
            return false;
        }
    }

    public bool CanSetAzimuth
    {
        get
        {
            tl.LogMessage("CanSetAzimuth Get", false.ToString());
            return false;
        }
    }

    public bool CanSetPark
    {
        get
        {
            tl.LogMessage("CanSetPark Get", false.ToString());
            return false;
        }
    }

    public bool CanSetShutter
    {
        get
        {
            tl.LogMessage("CanSetShutter Get", true.ToString());
            return true;
        }
    }

    public bool CanSlave
    {
        get
        {
            tl.LogMessage("CanSlave Get", false.ToString());
            return false;
        }
    }

    public bool CanSyncAzimuth
    {
        get
        {
            tl.LogMessage("CanSyncAzimuth Get", false.ToString());
            return false;
        }
    }

    public void CloseShutter()
    {
        tl.LogMessage("CloseShutter", "Shutter has been closed");
        domeShutterState = false;
    }

    public void FindHome()
    {
        tl.LogMessage("FindHome", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("FindHome");
    }

    public void OpenShutter()
    {
        tl.LogMessage("OpenShutter", "Shutter has been opened");
        domeShutterState = true;
    }

    public void Park()
    {
        tl.LogMessage("Park", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("Park");
    }

    public void SetPark()
    {
        tl.LogMessage("SetPark", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SetPark");
    }

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
            throw new ASCOM.PropertyNotImplementedException("Slaved", true);
        }
    }

    public void SlewToAltitude(double Altitude)
    {
        tl.LogMessage("SlewToAltitude", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToAltitude");
    }

    public void SlewToAzimuth(double Azimuth)
    {
        tl.LogMessage("SlewToAzimuth", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToAzimuth");
    }

    public bool Slewing
    {
        get
        {
            tl.LogMessage("Slewing Get", false.ToString());
            return false;
        }
    }

    public void SyncToAzimuth(double Azimuth)
    {
        tl.LogMessage("SyncToAzimuth", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SyncToAzimuth");
    }

    #endregion

    //ENDOFINSERTEDFILE
}