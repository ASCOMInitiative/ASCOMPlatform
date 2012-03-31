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
        get { throw new System.NotImplementedException(); }
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    public IRate this[int index]
    {
        get { throw new System.NotImplementedException(); }
    }
}

//Dummy implementation to stop compile errors in the Driver template solution
class TrackingRates : ITrackingRates
{
    public int Count
    {
        get { throw new System.NotImplementedException(); }
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    public DriveRates this[int index]
    {
        get { throw new System.NotImplementedException(); }
    }
}
