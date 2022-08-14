using ASCOM.DeviceInterface;

namespace ASCOM.TEMPLATEDEVICENAME
{

	interface ITEMPLATEDEVICEINTERFACE
	{
		// Dummy interface just to stop compile errors during development.
		// This file is not needed and is deleted by the setup wizard.
	}
}

//Dummy implementation to stop compile errors in the Driver template solution
class AxisRates : IAxisRates 
{ 
    public AxisRates(TelescopeAxes Axis)
    {
    }

    public int Count
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Dispose()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public IRate this[int index]
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }
}

//Dummy implementation to stop compile errors in the Driver template solution
class TrackingRates : ITrackingRates
{
    public int Count
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void Dispose()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public DriveRates this[int index]
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }
}
