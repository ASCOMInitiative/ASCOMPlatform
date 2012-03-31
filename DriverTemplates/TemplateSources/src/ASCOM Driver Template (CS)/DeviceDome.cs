// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceDome
{
    Util util = new Util(); TraceLogger tl = new TraceLogger();

    #region IDome Implementation
    public void AbortSlew()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public double Altitude
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool AtHome
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool AtPark
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double Azimuth
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanFindHome
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanPark
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSetAltitude
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSetAzimuth
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSetPark
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSetShutter
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSlave
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSyncAzimuth
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void CloseShutter()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void FindHome()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void OpenShutter()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void Park()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void SetPark()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public ShutterState ShutterStatus
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool Slaved
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException();
        }
    }

    public void SlewToAltitude(double Altitude)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public void SlewToAzimuth(double Azimuth)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public bool Slewing
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void SyncToAzimuth(double Azimuth)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    #endregion

    //ENDOFINSERTEDFILE
}