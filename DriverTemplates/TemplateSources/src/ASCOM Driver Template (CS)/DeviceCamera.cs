// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using System.Collections;
using ASCOM;
using ASCOM.Utilities;

class DeviceCamera
{
    Util util = new Util(); TraceLogger tl = new TraceLogger();

    #region ICamera Implementation
    public void AbortExposure()
    {
        throw new MethodNotImplementedException();
    }

    public short BayerOffsetX
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short BayerOffsetY
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short BinX
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

    public short BinY
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

    public double CCDTemperature
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public CameraStates CameraState
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public int CameraXSize
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public int CameraYSize
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanAbortExposure
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanAsymmetricBin
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanFastReadout
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanGetCoolerPower
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanPulseGuide
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanSetCCDTemperature
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CanStopExposure
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool CoolerOn
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

    public double CoolerPower
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double ElectronsPerADU
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double ExposureMax
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double ExposureMin
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double ExposureResolution
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool FastReadout
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

    public double FullWellCapacity
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short Gain
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

    public short GainMax
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short GainMin
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public ArrayList Gains
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool HasShutter
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double HeatSinkTemperature
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public object ImageArray
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public object ImageArrayVariant
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool ImageReady
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public bool IsPulseGuiding
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double LastExposureDuration
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public string LastExposureStartTime
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public int MaxADU
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short MaxBinX
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public short MaxBinY
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public int NumX
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

    public int NumY
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

    public short PercentCompleted
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double PixelSizeX
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double PixelSizeY
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public void PulseGuide(GuideDirections Direction, int Duration)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public short ReadoutMode
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

    public ArrayList ReadoutModes
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public string SensorName
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public SensorType SensorType
    {
        get { throw new ASCOM.PropertyNotImplementedException(); }
    }

    public double SetCCDTemperature
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

    public void StartExposure(double Duration, bool Light)
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    public int StartX
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

    public int StartY
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

    public void StopExposure()
    {
        throw new ASCOM.MethodNotImplementedException();
    }

    #endregion

    //ENDOFINSERTEDFILE
}