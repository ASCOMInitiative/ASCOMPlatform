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
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

    #region ICamera Implementation

    private const int ccdWidth = 1394; // Constants to define the CCD pixel dimensions
    private const int ccdHeight = 1040;
    private const double pixelSize = 6.45; // Constant for the pixel physical dimension

    private int cameraNumX = ccdWidth; // Initialise variables to hold values required for functionality tested by Conform
    private int cameraNumY = ccdHeight;
    private int cameraStartX = 0;
    private int cameraStartY = 0;
    private DateTime exposureStart = DateTime.MinValue;
    private double cameraLastExposureDuration = 0.0;
    private bool cameraImageReady = false;
    private int[,] cameraImageArray;
    private object[,] cameraImageArrayVariant;

	/// <summary>
	/// Aborts the current exposure, if any, and returns the camera to Idle state.
	/// </summary>
	public void AbortExposure()
    {
        tl.LogMessage("AbortExposure", "Not implemented");
        throw new MethodNotImplementedException("AbortExposure");
    }

	/// <summary>
	/// Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
	/// </summary>
	/// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
	public short BayerOffsetX
    {
        get
        {
            tl.LogMessage("BayerOffsetX Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", false);
        }
    }

	/// <summary>
	/// Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
	/// </summary>
	/// <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
	public short BayerOffsetY
    {
        get
        {
            tl.LogMessage("BayerOffsetY Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", true);
        }
    }

	/// <summary>
	/// Sets the binning factor for the X axis, also returns the current value.
	/// </summary>
	/// <value>The X binning value</value>
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

	/// <summary>
	/// Sets the binning factor for the Y axis, also returns the current value.
	/// </summary>
	/// <value>The Y binning value.</value>
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

	/// <summary>
	/// Returns the current CCD temperature in degrees Celsius.
	/// </summary>
	/// <value>The CCD temperature.</value>
	public double CCDTemperature
    {
        get
        {
            tl.LogMessage("CCDTemperature Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("CCDTemperature", false);
        }
    }

	/// <summary>
	/// Returns the current camera operational state
	/// </summary>
	/// <value>The state of the camera.</value>
	public CameraStates CameraState
    {
        get
        {
            tl.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString());
            return CameraStates.cameraIdle;
        }
    }

	/// <summary>
	/// Returns the width of the CCD camera chip in unbinned pixels.
	/// </summary>
	/// <value>The size of the camera X.</value>
	public int CameraXSize
    {
        get
        {
            tl.LogMessage("CameraXSize Get", ccdWidth.ToString());
            return ccdWidth;
        }
    }

	/// <summary>
	/// Returns the height of the CCD camera chip in unbinned pixels.
	/// </summary>
	/// <value>The size of the camera Y.</value>
	public int CameraYSize
    {
        get
        {
            tl.LogMessage("CameraYSize Get", ccdHeight.ToString());
            return ccdHeight;
        }
    }

	/// <summary>
	/// Returns <c>true</c> if the camera can abort exposures; <c>false</c> if not.
	/// </summary>
	/// <value>
	public bool CanAbortExposure
    {
        get
        {
            tl.LogMessage("CanAbortExposure Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Returns a flag showing whether this camera supports asymmetric binning
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
	/// </value>
	public bool CanAsymmetricBin
    {
        get
        {
            tl.LogMessage("CanAsymmetricBin Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Camera has a fast readout mode
	/// </summary>
	/// <returns><c>true</c> when the camera supports a fast readout mode</returns>
	public bool CanFastReadout
    {
        get
        {
            tl.LogMessage("CanFastReadout Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// If <c>true</c>, the camera's cooler power setting can be read.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
	/// </value>
	public bool CanGetCoolerPower
    {
        get
        {
            tl.LogMessage("CanGetCoolerPower Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Returns a flag indicating whether this camera supports pulse guiding
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
	/// </value>
	public bool CanPulseGuide
    {
        get
        {
            tl.LogMessage("CanPulseGuide Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Returns a flag indicating whether this camera supports setting the CCD temperature
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
	/// </value>
	public bool CanSetCCDTemperature
    {
        get
        {
            tl.LogMessage("CanSetCCDTemperature Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Returns a flag indicating whether this camera can stop an exposure that is in progress
	/// </summary>
	/// <value>
	/// <c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
	/// </value>
	public bool CanStopExposure
    {
        get
        {
            tl.LogMessage("CanStopExposure Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Turns on and off the camera cooler, and returns the current on/off state.
	/// </summary>
	/// <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
	public bool CoolerOn
    {
        get
        {
            tl.LogMessage("CoolerOn Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("CoolerOn", false);
        }
        set
        {
            tl.LogMessage("CoolerOn Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("CoolerOn", true);
        }
    }

	/// <summary>
	/// Returns the present cooler power level, in percent.
	/// </summary>
	/// <value>The cooler power.</value>
	public double CoolerPower
    {
        get
        {
            tl.LogMessage("CoolerPower Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("CoolerPower", false);
        }
    }

	/// <summary>
	/// Returns the gain of the camera in photoelectrons per A/D unit.
	/// </summary>
	/// <value>The electrons per ADU.</value>
	public double ElectronsPerADU
    {
        get
        {
            tl.LogMessage("ElectronsPerADU Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ElectronsPerADU", false);
        }
    }

	/// <summary>
	/// Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
	/// </summary>
	/// <returns>The maximum exposure time, in seconds, that the camera supports</returns>
	public double ExposureMax
    {
        get
        {
            tl.LogMessage("ExposureMax Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ExposureMax", false);
        }
    }

	/// <summary>
	/// Minimum exposure time
	/// </summary>
	/// <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
	public double ExposureMin
    {
        get
        {
            tl.LogMessage("ExposureMin Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ExposureMin", false);
        }
    }

	/// <summary>
	/// Exposure resolution
	/// </summary>
	/// <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
	public double ExposureResolution
    {
        get
        {
            tl.LogMessage("ExposureResolution Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ExposureResolution", false);
        }
    }

	/// <summary>
	/// Gets or sets Fast Readout Mode
	/// </summary>
	/// <value><c>true</c> for fast readout mode, <c>false</c> for normal mode</value>
	public bool FastReadout
    {
        get
        {
            tl.LogMessage("FastReadout Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("FastReadout", false);
        }
        set
        {
            tl.LogMessage("FastReadout Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("FastReadout", true);
        }
    }

	/// <summary>
	/// Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
	/// </summary>
	/// <value>The full well capacity.</value>
	public double FullWellCapacity
    {
        get
        {
            tl.LogMessage("FullWellCapacity Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("FullWellCapacity", false);
        }
    }


	/// <summary>
	/// The camera's gain (GAIN VALUE MODE) OR the index of the selected camera gain description in the <see cref="Gains" /> array (GAINS INDEX MODE)
	/// </summary>
	/// <returns><para><b> GAIN VALUE MODE:</b> The current gain value.</para>
	/// <p style="color:red"><b>OR</b></p>
	/// <b>GAINS INDEX MODE:</b> Index into the Gains array for the current camera gain
	/// </returns>
	public short Gain
    {
        get
        {
            tl.LogMessage("Gain Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Gain", false);
        }
        set
        {
            tl.LogMessage("Gain Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Gain", true);
        }
    }

	/// <summary>
	/// Maximum <see cref="Gain" /> value of that this camera supports
	/// </summary>
	/// <returns>The maximum gain value that this camera supports</returns>
	public short GainMax
    {
        get
        {
            tl.LogMessage("GainMax Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GainMax", false);
        }
    }

    public short GainMin
    {
        get
        {
            tl.LogMessage("GainMin Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GainMin", true);
        }
    }

	/// <summary>
	/// List of Gain names supported by the camera
	/// </summary>
	/// <returns>The list of supported gain names as an ArrayList of strings</returns>
	public ArrayList Gains
    {
        get
        {
            tl.LogMessage("Gains Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Gains", true);
        }
    }

	/// <summary>
	/// Returns a flag indicating whether this camera has a mechanical shutter
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance has shutter; otherwise, <c>false</c>.
	/// </value>
	public bool HasShutter
    {
        get
        {
            tl.LogMessage("HasShutter Get", false.ToString());
            return false;
        }
    }

	/// <summary>
	/// Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius.
	/// </summary>
	/// <value>The heat sink temperature.</value>
	public double HeatSinkTemperature
    {
        get
        {
            tl.LogMessage("HeatSinkTemperature Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("HeatSinkTemperature", false);
        }
    }

	/// <summary>
	/// Returns a safearray of int of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
	/// </summary>
	/// <value>The image array.</value>
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

	/// <summary>
	/// Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
	/// </summary>
	/// <value>The image array variant.</value>
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

	/// <summary>
	/// Returns a flag indicating whether the image is ready to be downloaded from the camera
	/// </summary>
	/// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
	public bool ImageReady
    {
        get
        {
            tl.LogMessage("ImageReady Get", cameraImageReady.ToString());
            return cameraImageReady;
        }
    }

	/// <summary>
	/// Returns a flag indicating whether the camera is currently in a <see cref="PulseGuide">PulseGuide</see> operation.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
	/// </value>
	public bool IsPulseGuiding
    {
        get
        {
            tl.LogMessage("IsPulseGuiding Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
        }
    }

	/// <summary>
	/// Reports the actual exposure duration in seconds (i.e. shutter open time).
	/// </summary>
	/// <value>The last duration of the exposure.</value>
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

	/// <summary>
	/// Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
	/// The start time must be UTC.
	/// </summary>
	/// <value>The last exposure start time in UTC.</value>
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

	/// <summary>
	/// Reports the maximum ADU value the camera can produce.
	/// </summary>
	/// <value>The maximum ADU.</value>
	public int MaxADU
    {
        get
        {
            tl.LogMessage("MaxADU Get", "20000");
            return 20000;
        }
    }

	/// <summary>
	/// Returns the maximum allowed binning for the X camera axis
	/// </summary>
	/// <value>The max bin X.</value>
	public short MaxBinX
    {
        get
        {
            tl.LogMessage("MaxBinX Get", "1");
            return 1;
        }
    }

	/// <summary>
	/// Returns the maximum allowed binning for the Y camera axis
	/// </summary>
	/// <value>The max bin Y.</value>
	public short MaxBinY
    {
        get
        {
            tl.LogMessage("MaxBinY Get", "1");
            return 1;
        }
    }

	/// <summary>
	/// Sets the subframe width. Also returns the current value.
	/// </summary>
	/// <value>The num X.</value>
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

	/// <summary>
	/// Sets the subframe height. Also returns the current value.
	/// </summary>
	/// <value>The num Y.</value>
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

	/// <summary>
	/// The camera's offset (OFFSET VALUE MODE) OR the index of the selected camera offset description in the <see cref="Offsets" /> array (OFFSETS INDEX MODE)
	/// </summary>
	/// <returns><para><b> OFFSET VALUE MODE:</b> The current offset value.</para>
	/// <p style="color:red"><b>OR</b></p>
	/// <b>OFFSETS INDEX MODE:</b> Index into the Offsets array for the current camera offset
	/// </returns>
	public int Offset
    {
        get
        {
            tl.LogMessage("Offset Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Offset", false);
        }
        set
        {
            tl.LogMessage("Offset Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Offset", true);
        }
    }

	/// <summary>
	/// Maximum <see cref="Offset" /> value that this camera supports
	/// </summary>
	/// <returns>The maximum offset value that this camera supports</returns>
	public int OffsetMax
    {
        get
        {
            tl.LogMessage("OffsetMax Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("OffsetMax", false);
        }
    }

	/// <summary>
	/// Minimum <see cref="Offset" /> value that this camera supports
	/// </summary>
	/// <returns>The minimum offset value that this camera supports</returns>
	public int OffsetMin
    {
        get
        {
            tl.LogMessage("OffsetMin Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("OffsetMin", true);
        }
    }

	/// <summary>
	/// List of Offset names supported by the camera
	/// </summary>
	/// <returns>The list of supported offset names as an ArrayList of strings</returns>
	public ArrayList Offsets
    {
        get
        {
            tl.LogMessage("Offsets Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Offsets", true);
        }
    }

	/// <summary>
	/// Percent completed, Interface Version 2 and later
	/// </summary>
	/// <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
	public short PercentCompleted
    {
        get
        {
            tl.LogMessage("PercentCompleted Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("PercentCompleted", false);
        }
    }

	/// <summary>
	/// Returns the width of the CCD chip pixels in microns.
	/// </summary>
	/// <value>The pixel size X.</value>
	public double PixelSizeX
    {
        get
        {
            tl.LogMessage("PixelSizeX Get", pixelSize.ToString());
            return pixelSize;
        }
    }

	/// <summary>
	/// Returns the height of the CCD chip pixels in microns.
	/// </summary>
	/// <value>The pixel size Y.</value>
	public double PixelSizeY
    {
        get
        {
            tl.LogMessage("PixelSizeY Get", pixelSize.ToString());
            return pixelSize;
        }
    }

	/// <summary>
	/// Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
	/// </summary>
	/// <param name="Direction">The direction of movement.</param>
	/// <param name="Duration">The duration of movement in milli-seconds.</param>
	public void PulseGuide(GuideDirections Direction, int Duration)
    {
        tl.LogMessage("PulseGuide", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("PulseGuide");
    }

	/// <summary>
	/// Readout mode, Interface Version 2 and later
	/// </summary>
	/// <value></value>
	/// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
	/// the camera's current readout mode.</returns>
	public short ReadoutMode
    {
        get
        {
            tl.LogMessage("ReadoutMode Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ReadoutMode", false);
        }
        set
        {
            tl.LogMessage("ReadoutMode Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ReadoutMode", true);
        }
    }

	/// <summary>
	/// List of available readout modes, Interface Version 2 and later
	/// </summary>
	/// <returns>An ArrayList of readout mode names</returns>
	public ArrayList ReadoutModes
    {
        get
        {
            tl.LogMessage("ReadoutModes Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ReadoutModes", false);
        }
    }

	/// <summary>
	/// Sensor name, Interface Version 2 and later
	/// </summary>
	/// <returns>The name of the sensor used within the camera.</returns>
	public string SensorName
    {
        get
        {
            tl.LogMessage("SensorName Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SensorName", false);
        }
    }

	/// <summary>
	/// Type of colour information returned by the camera sensor, Interface Version 2 and later
	/// </summary>
	/// <value>The type of sensor used by the camera.</value>
	public SensorType SensorType
    {
        get
        {
            tl.LogMessage("SensorType Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SensorType", false);
        }
    }

	/// <summary>
	/// Sets the camera cooler setpoint in degrees Celsius, and returns the current setpoint.
	/// </summary>
	/// <value>The set CCD temperature.</value>
	public double SetCCDTemperature
    {
        get
        {
            tl.LogMessage("SetCCDTemperature Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", false);
        }
        set
        {
            tl.LogMessage("SetCCDTemperature Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", true);
        }
    }

	/// <summary>
	/// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
	/// </summary>
	/// <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
	/// <param name="Light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
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

	/// <summary>
	/// Sets the subframe start position for the X axis (0 based) and returns the current value.
	/// </summary>
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
            tl.LogMessage("StartX Set", value.ToString());
        }
    }

	/// <summary>
	/// Sets the subframe start position for the Y axis (0 based). Also returns the current value.
	/// </summary>
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

	/// <summary>
	/// Stops the current exposure, if any.
	/// </summary>
	public void StopExposure()
    {
        tl.LogMessage("StopExposure", "Not implemented");
        throw new MethodNotImplementedException("StopExposure");
    }

	/// <summary>
	/// Camera's sub-exposure interval
	/// </summary>
	public double SubExposureDuration
    {
        get
        {
            tl.LogMessage("SubExposureDuration Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SubExposureDuration", false);
        }
        set
        {
            tl.LogMessage("SubExposureDuration Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SubExposureDuration", true);
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}