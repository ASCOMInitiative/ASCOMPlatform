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

    private const int ccdWidth = 1394;
    private const int ccdHeight = 1040;
    private const double pixelSize = 6.45;
    private int cameraNumX = ccdWidth;
    private int cameraNumY = ccdHeight;
    private int cameraStartX = 0;
    private int cameraStartY = 0;
    private DateTime exposureStart = DateTime.MinValue;
    private double cameraLastExposureDuration = 0.0;
    private bool cameraImageReady = false;
    private int[,] cameraImageArray;
    private object[,] cameraImageArrayVariant;

    public void AbortExposure()
    {
        tl.LogMessage("AbortExposure", "Not implemented");
        throw new MethodNotImplementedException("AbortExposure");
    }

    public short BayerOffsetX
    {
        get { throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", false); }
    }

    public short BayerOffsetY
    {
        get { throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", true); }
    }

    public short BinX
    {
        get
        {
            tl.LogMessage("BinX Get", "1");
            return 1;
        }
        set
        {
            tl.LogMessage("BinX Set", value.ToString());
            if (value != 1) throw new ASCOM.InvalidValueException("BinX", value.ToString(), "1"); // Only 1 is valid in this simple template
        }
    }

    public short BinY
    {
        get
        {
            tl.LogMessage("BinY Get", "1");
            return 1;
        }
        set
        {
            tl.LogMessage("BinY Set", value.ToString());
            if (value != 1) throw new ASCOM.InvalidValueException("BinY", value.ToString(), "1"); // Only 1 is valid in this simple template
        }
    }

    public double CCDTemperature
    {
        get { throw new ASCOM.PropertyNotImplementedException("CCDTemperature", false); }
    }

    public CameraStates CameraState
    {
        get
        {
            tl.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString());
            return CameraStates.cameraIdle;
        }
    }

    public int CameraXSize
    {
        get
        {
            tl.LogMessage("CameraXSize Get", ccdWidth.ToString());
            return ccdWidth;
        }
    }

    public int CameraYSize
    {
        get
        {
            tl.LogMessage("CameraYSize Get", ccdHeight.ToString());
            return ccdHeight;
        }
    }

    public bool CanAbortExposure
    {
        get
        {
            tl.LogMessage("CanAbortExposure Get", false.ToString());
            return false;
        }
    }

    public bool CanAsymmetricBin
    {
        get
        {
            tl.LogMessage("CanAsymmetricBin Get", false.ToString());
            return false;
        }
    }

    public bool CanFastReadout
    {
        get
        {
            tl.LogMessage("CanFastReadout Get", false.ToString());
            return false;
        }
    }

    public bool CanGetCoolerPower
    {
        get
        {
            tl.LogMessage("CanGetCoolerPower Get", false.ToString());
            return false;
        }
    }

    public bool CanPulseGuide
    {
        get
        {
            tl.LogMessage("CanPulseGuide Get", false.ToString());
            return false;
        }
    }

    public bool CanSetCCDTemperature
    {
        get
        {
            tl.LogMessage("CanSetCCDTemperature Get", false.ToString());
            return false;
        }
    }

    public bool CanStopExposure
    {
        get
        {
            tl.LogMessage("CanStopExposure Get", false.ToString());
            return false;
        }
    }

    public bool CoolerOn
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException("CoolerOn", false);
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException("CoolerOn", true);
        }
    }

    public double CoolerPower
    {
        get { throw new ASCOM.PropertyNotImplementedException("CoolerPower", false); }
    }

    public double ElectronsPerADU
    {
        get { throw new ASCOM.PropertyNotImplementedException("ElectronsPerADU", false); }
    }

    public double ExposureMax
    {
        get { throw new ASCOM.PropertyNotImplementedException("ExposureMax", false); }
    }

    public double ExposureMin
    {
        get { throw new ASCOM.PropertyNotImplementedException("ExposureMin", false); }
    }

    public double ExposureResolution
    {
        get { throw new ASCOM.PropertyNotImplementedException("ExposureResolution", false); }
    }

    public bool FastReadout
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException("FastReadout", false);
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException("FastReadout", true);
        }
    }

    public double FullWellCapacity
    {
        get { throw new ASCOM.PropertyNotImplementedException("FullWellCapacity", false); }
    }

    public short Gain
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException("Gain", false);
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException("Gain", true);
        }
    }

    public short GainMax
    {
        get { throw new ASCOM.PropertyNotImplementedException("GainMax", false); }
    }

    public short GainMin
    {
        get { throw new ASCOM.PropertyNotImplementedException("GainMin", true); }
    }

    public ArrayList Gains
    {
        get { throw new ASCOM.PropertyNotImplementedException("Gains", true); }
    }

    public bool HasShutter
    {
        get
        {
            tl.LogMessage("HasShutter Get", false.ToString());
            return false;
        }
    }

    public double HeatSinkTemperature
    {
        get { throw new ASCOM.PropertyNotImplementedException("HeatSinkTemperature", false); }
    }

    public object ImageArray
    {
        get
        {
            if (!cameraImageReady)
            {
                tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!");
                throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!");
            }

            cameraImageArray = new int[cameraNumX, cameraNumY];
            return cameraImageArray;
        }
    }

    public object ImageArrayVariant
    {
        get
        {
            if (!cameraImageReady)
            {
                tl.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!");
                throw new ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!");
            }
            cameraImageArrayVariant = new object[cameraNumX, cameraNumY];
            for (int i = 0; i < cameraImageArray.GetLength(1); i++)
            {
                for (int j = 0; j < cameraImageArray.GetLength(0); j++)
                {
                    cameraImageArrayVariant[j, i] = cameraImageArray[j, i];
                }

            }

            return cameraImageArrayVariant;
        }
    }

    public bool ImageReady
    {
        get
        {
            tl.LogMessage("ImageReady Get", cameraImageReady.ToString());
            return cameraImageReady;
        }
    }

    public bool IsPulseGuiding
    {
        get { throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false); }
    }

    public double LastExposureDuration
    {
        get
        {
            if (!cameraImageReady)
            {
                tl.LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!");
                throw new ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!");
            }
            tl.LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString());
            return cameraLastExposureDuration;
        }
    }

    public string LastExposureStartTime
    {
        get
        {
            if (!cameraImageReady)
            {
                tl.LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!");
                throw new ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!");
            }
            string exposureStartString = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss");
            tl.LogMessage("LastExposureStartTime Get", exposureStartString.ToString());
            return exposureStartString;
        }
    }

    public int MaxADU
    {
        get
        {
            tl.LogMessage("MaxADU Get", "20000");
            return 20000;
        }
    }

    public short MaxBinX
    {
        get
        {
            tl.LogMessage("MaxBinX Get", "1");
            return 1;
        }
    }

    public short MaxBinY
    {
        get
        {
            tl.LogMessage("MaxBinY Get", "1");
            return 1;
        }
    }

    public int NumX
    {
        get
        {
            tl.LogMessage("NumX Get", cameraNumX.ToString());
            return cameraNumX;
        }
        set
        {
            cameraNumX = value;
            tl.LogMessage("NumX set", value.ToString());
        }
    }

    public int NumY
    {
        get
        {
            tl.LogMessage("NumY Get", cameraNumY.ToString());
            return cameraNumY;
        }
        set
        {
            cameraNumY = value;
            tl.LogMessage("NumY set", value.ToString());
        }
    }

    public short PercentCompleted
    {
        get { throw new ASCOM.PropertyNotImplementedException("PercentCompleted", false); }
    }

    public double PixelSizeX
    {
        get
        {
            tl.LogMessage("PixelSizeX Get", pixelSize.ToString());
            return pixelSize;
        }
    }

    public double PixelSizeY
    {
        get
        {
            tl.LogMessage("PixelSizeY Get", pixelSize.ToString());
            return pixelSize;
        }
    }

    public void PulseGuide(GuideDirections Direction, int Duration)
    {
        throw new ASCOM.MethodNotImplementedException("PulseGuide");
    }

    public short ReadoutMode
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException("ReadoutMode", false);
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException("ReadoutMode", true);
        }
    }

    public ArrayList ReadoutModes
    {
        get { throw new ASCOM.PropertyNotImplementedException("ReadoutModes", false); }
    }

    public string SensorName
    {
        get { throw new ASCOM.PropertyNotImplementedException("SensorName", false); }
    }

    public SensorType SensorType
    {
        get { throw new ASCOM.PropertyNotImplementedException("SensorType", false); }
    }

    public double SetCCDTemperature
    {
        get
        {
            throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", false);
        }
        set
        {
            throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", true);
        }
    }

    public void StartExposure(double Duration, bool Light)
    {
        if (Duration < 0.0) throw new InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards");
        if (cameraNumX > ccdWidth) throw new InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString());
        if (cameraNumY > ccdHeight) throw new InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString());
        if (cameraStartX > ccdWidth) throw new InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString());
        if (cameraStartY > ccdHeight) throw new InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString());

        cameraLastExposureDuration = Duration;
        exposureStart = DateTime.Now;
        System.Threading.Thread.Sleep((int)Duration * 1000);  // Sleep for the duration to simulate exposure 
        tl.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString());
        cameraImageReady = true;
    }

    public int StartX
    {
        get
        {
            tl.LogMessage("StartX Get", cameraStartX.ToString());
            return cameraStartX;
        }
        set
        {
            cameraStartX = value;
            tl.LogMessage("StartX set", value.ToString());
        }
    }

    public int StartY
    {
        get
        {
            tl.LogMessage("StartY Get", cameraStartY.ToString());
            return cameraStartY;
        }
        set
        {
            cameraStartY = value;
            tl.LogMessage("StartY set", value.ToString());
        }
    }

    public void StopExposure()
    {
        tl.LogMessage("StopExposure", "Not implemented");
        throw new MethodNotImplementedException("StopExposure");
    }

    #endregion

    //ENDOFINSERTEDFILE
}