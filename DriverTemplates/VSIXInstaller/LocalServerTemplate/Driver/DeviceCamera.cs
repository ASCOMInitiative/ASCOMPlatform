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
    #region ICamera Implementation

	/// <summary>
	/// Aborts the current exposure, if any, and returns the camera to Idle state.
	/// </summary>
	public void AbortExposure()
    {
        LogMessage("AbortExposure", $"Calling method.");
        CameraHardware.AbortExposure();
        LogMessage("AbortExposure", $"Completed.");
    }

    /// <summary>
    /// Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
    /// </summary>
    /// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
    public short BayerOffsetX
    {
        get
        {
            short bayerOffsetX=CameraHardware.BayerOffsetX;
            LogMessage("BayerOffsetX Get", bayerOffsetX.ToString());
            return bayerOffsetX;
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
            short bayerOffsetY = CameraHardware.BayerOffsetY;
            LogMessage("BayerOffsetY Get", bayerOffsetY.ToString());
            return bayerOffsetY;
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
            short binX = CameraHardware.BinX;
            LogMessage("BinX Get", binX.ToString());
            return binX;
        }
        set
        {
            LogMessage("BinX Set", value.ToString());
            CameraHardware.BinX = value;
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
            short binY = CameraHardware.BinY;
            LogMessage("BinY Get", binY.ToString());
            return binY;
        }
        set
        {
            LogMessage("BinY Set", value.ToString());
            CameraHardware.BinY = value;
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
            double ccdTemperature = CameraHardware.CCDTemperature;
            LogMessage("CCDTemperature Get", ccdTemperature.ToString());
            return ccdTemperature;
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
            CameraStates cameraState = CameraHardware.CameraState;
            LogMessage("CameraState Get", cameraState.ToString());
            return cameraState;
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
            int cameraXSize= CameraHardware.CameraXSize;
            LogMessage("CameraXSize Get", cameraXSize.ToString());
            return cameraXSize;
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
            int cameraYSize = CameraHardware.CameraYSize;
            LogMessage("CameraYSize Get", cameraYSize.ToString());
            return cameraYSize;
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
            bool canAbortExposure= CameraHardware.CanAbortExposure;
            LogMessage("CanAbortExposure Get", canAbortExposure.ToString());
            return canAbortExposure;
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
            bool canAsymmetricBin = CameraHardware.CanAsymmetricBin;
            LogMessage("CanAsymmetricBin Get", canAsymmetricBin.ToString());
            return canAsymmetricBin;
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
            {
                bool canFastReadout = CameraHardware.CanFastReadout;
                LogMessage("CanFastReadout Get", canFastReadout.ToString());
                return canFastReadout;
            }
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
            bool canGetCoolerPower = CameraHardware.CanGetCoolerPower;
            LogMessage("CanGetCoolerPower Get", canGetCoolerPower.ToString());
            return canGetCoolerPower;
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
            bool canPulseGuide = CameraHardware.CanPulseGuide;
            LogMessage("CanPulseGuide Get", canPulseGuide.ToString());
            return canPulseGuide;
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
            bool canSetCCDTemperature = CameraHardware.CanSetCCDTemperature;
            LogMessage("CanSetCCDTemperature Get", canSetCCDTemperature.ToString());
            return canSetCCDTemperature;
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
            bool canStopExposure = CameraHardware.CanStopExposure;
            LogMessage("CanStopExposure Get", canStopExposure.ToString());
            return canStopExposure;
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
            bool coolerOn = CameraHardware.CoolerOn;
            LogMessage("CoolerOn Get", coolerOn.ToString());
            return coolerOn;
        }
        set
        {
            LogMessage("CoolerOn Set", value.ToString());
            CameraHardware.CoolerOn = value;
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
            double coolerPower = CameraHardware.CoolerPower;
            LogMessage("CoolerPower Get", coolerPower.ToString());
            return coolerPower;
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
            double electronsPerAdu = CameraHardware.ElectronsPerADU;
            LogMessage("ElectronsPerADU Get", electronsPerAdu.ToString());
            return electronsPerAdu;
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
            double exposureMax = CameraHardware.ExposureMax;
            LogMessage("ExposureMax Get", exposureMax.ToString());
            return exposureMax;
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
            double exposureMin = CameraHardware.ExposureMin;
            LogMessage("ExposureMin Get", exposureMin.ToString());
            return exposureMin;
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
            double exposureResolution = CameraHardware.ExposureResolution;
            LogMessage("ExposureResolution Get", exposureResolution.ToString());
            return exposureResolution;
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
            bool fastreadout = CameraHardware.FastReadout;
            LogMessage("FastReadout Get", fastreadout.ToString());
            return fastreadout;
        }
        set
        {
            LogMessage("FastReadout Set", value.ToString());
            CameraHardware.FastReadout = value;
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
            double fullWellCapacity = CameraHardware.FullWellCapacity;
            LogMessage("FullWellCapacity Get", fullWellCapacity.ToString());
            return fullWellCapacity;
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
            short gain = CameraHardware.Gain;
            LogMessage("Gain Get", gain.ToString());
            return gain;
        }
        set
        {
            LogMessage("Gain Set", value.ToString());
            CameraHardware.Gain = value;
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
            short gainMax = CameraHardware.GainMax;
            LogMessage("GainMax Get", gainMax.ToString());
            return gainMax;
        }
    }

	/// <summary>
	/// Minimum <see cref="Gain" /> value of that this camera supports
	/// </summary>
	/// <returns>The minimum gain value that this camera supports</returns>
	public short GainMin
    {
        get
        {
            short gainMin = CameraHardware.GainMin;
            LogMessage("GainMin Get", gainMin.ToString());
            return gainMin;
        }
    }

	/// <summary>
	/// Minimum <see cref="Gain" /> value of that this camera supports
	/// </summary>
	/// <returns>The minimum gain value that this camera supports</returns>
	public ArrayList Gains
    {
        get
        {
            ArrayList gains = CameraHardware.Gains;
            LogMessage("Gains Get", $"Received {gains.Count} gain values.");
            return gains;
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
            bool hasShutter = CameraHardware.HasShutter;
            LogMessage("HasShutter Get", hasShutter.ToString());
            return hasShutter;
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
            double heatSinkTemperature = CameraHardware.HeatSinkTemperature;
            LogMessage("HeatSinkTemperature Get", heatSinkTemperature.ToString());
            return heatSinkTemperature;
        }
    }

	/// <summary>
	/// Returns a safearray of integer of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
	/// </summary>
	/// <value>The image array.</value>
	public object ImageArray
    {
        get
        {
            LogMessage("ImageArray Get", $"Retrieving image array");
            return CameraHardware.ImageArray; 
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
            LogMessage("ImageArrayVariant Get", $"Retrieving image array");
            return CameraHardware.ImageArrayVariant;
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
            bool imageReady = CameraHardware.ImageReady;
            LogMessage("ImageReady Get", imageReady.ToString());
            return imageReady;
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
            bool isPulseGuiding = CameraHardware.IsPulseGuiding;
            LogMessage("IsPulseGuiding Get", isPulseGuiding.ToString());
            return isPulseGuiding;
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
            double lastExposureDuration = CameraHardware.LastExposureDuration;
            LogMessage("LastExposureDuration Get", lastExposureDuration.ToString());
            return lastExposureDuration;
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
            string lastExposureStartTime = CameraHardware.LastExposureStartTime;
            LogMessage("LastExposureStartTime Get", lastExposureStartTime.ToString());
            return lastExposureStartTime;
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
            int maxAdu = CameraHardware.MaxADU;
            LogMessage("MaxADU Get", maxAdu.ToString());
            return maxAdu;
        }
    }

	/// <summary>
	/// Returns the maximum allowed binning for the X camera axis
	/// </summary>
	/// <value>The maximum width of the image.</value>
	public short MaxBinX
    {
        get
        {
            short maxBinX = CameraHardware.MaxBinX;
            LogMessage("MaxBinX Get", maxBinX.ToString());
            return maxBinX;
        }
    }

	/// <summary>
	/// Returns the maximum allowed binning for the Y camera axis
	/// </summary>
	/// <value>The maximum height of the image.</value>
	public short MaxBinY
    {
        get
        {
            short maxBinY = CameraHardware.MaxBinY;
            LogMessage("MaxBinY Get", maxBinY.ToString());
            return maxBinY;
        }
    }

	/// <summary>
	/// Sets the subframe width. Also returns the current value.
	/// </summary>
	/// <value>The width of the image.</value>
	public int NumX
    {
        get
        {
            int numX = CameraHardware.NumX;
            LogMessage("NumX Get", numX.ToString());
            return numX;
        }
        set
        {
            LogMessage("NumX Set", value.ToString());
            CameraHardware.NumX = value;
        }
    }

	/// <summary>
	/// Sets the subframe height. Also returns the current value.
	/// </summary>
	/// <value>The height of the image.</value>
	public int NumY
    {
        get
        {
            int numY = CameraHardware.NumY;
            LogMessage("NumY Get", numY.ToString());
            return numY;
        }
        set
        {
            LogMessage("NumY Set", value.ToString());
            CameraHardware.NumY = value;
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
            int offset = CameraHardware.Offset;
            LogMessage("Offset Get", offset.ToString());
            return offset;
        }
        set
        {
            LogMessage("Offset Set", value.ToString());
            CameraHardware.Offset = value;
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
            int offsetMax = CameraHardware.OffsetMax;
            LogMessage("OffsetMax Get", offsetMax.ToString());
            return offsetMax;
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
            int offsetMin = CameraHardware.OffsetMin;
            LogMessage("OffsetMin Get", offsetMin.ToString());
            return offsetMin;
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
            LogMessage("Offsets Get", "Not implemented");
            throw new PropertyNotImplementedException("Offsets", true);
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
            short percentCompleted = CameraHardware.PercentCompleted;
            LogMessage("PercentCompleted Get", percentCompleted.ToString());
            return percentCompleted;
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
            double pixelSizeX = CameraHardware.PixelSizeX;
            LogMessage("PixelSizeX Get", pixelSizeX.ToString());
            return pixelSizeX;
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
            double pixelSizeY = CameraHardware.PixelSizeY;
            LogMessage("PixelSizeY Get", pixelSizeY.ToString());
            return pixelSizeY;
        }
    }

	/// <summary>
	/// Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
	/// </summary>
	/// <param name="direction">The direction of movement.</param>
	/// <param name="duration">The duration of movement in milli-seconds.</param>
	public void PulseGuide(GuideDirections direction, int duration)
    {
        LogMessage("PulseGuide", $"Direction: {direction}, Duration: {duration}");
        CameraHardware.PulseGuide(direction,duration);
    }

    /// <summary>
    /// Readout mode, Interface Version 2 only
    /// </summary>
    /// <value></value>
    /// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
    /// the camera's current readout mode.</returns>
    public short ReadoutMode
    {
        get
        {
            short readoutMode = CameraHardware.ReadoutMode;
            LogMessage("ReadoutMode Get", readoutMode.ToString());
            return readoutMode;
        }
        set
        {
            LogMessage("ReadoutMode Set", value.ToString());
            CameraHardware.ReadoutMode = value;
        }
    }

	/// <summary>
	/// List of available readout modes, Interface Version 2 only
	/// </summary>
	/// <returns>An ArrayList of readout mode names</returns>
	public ArrayList ReadoutModes
    {
        get
        {
            ArrayList readoutMode = CameraHardware.ReadoutModes;
            LogMessage("ReadoutModes Get", $"Received {readoutMode.Count} readout modes.");
            return readoutMode;
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
            string sensorName = CameraHardware.SensorName;
            LogMessage("SensorName Get", sensorName.ToString());
            return sensorName;
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
            SensorType sensorType = CameraHardware.SensorType;
            LogMessage("SensorType Get", sensorType.ToString());
            return sensorType;
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
            double setCcdTemperature = CameraHardware.SetCCDTemperature;
            LogMessage("SetCCDTemperature Get", setCcdTemperature.ToString());
            return setCcdTemperature;
        }
        set
        {
            LogMessage("SetCCDTemperature Set", value.ToString());
            CameraHardware.SetCCDTemperature = value;
        }
    }

	/// <summary>
	/// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
	/// </summary>
	/// <param name="duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
	/// <param name="light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
	public void StartExposure(double duration, bool light)
    {
        LogMessage("StartExposure", $"Duration: {duration}, Light: {light}");
        CameraHardware.StartExposure(duration, light);
    }

	/// <summary>
	/// Sets the subframe start position for the X axis (0 based) and returns the current value.
	/// </summary>
	public int StartX
    {
        get
        {
            int setCcdTemperature = CameraHardware.StartX;
            LogMessage("StartX Get", setCcdTemperature.ToString());
            return setCcdTemperature;
        }
        set
        {
            LogMessage("StartX Set", value.ToString());
            CameraHardware.StartX = value;
        }
    }

    /// <summary>
    /// Sets the subframe start position for the Y axis (0 based). Also returns the current value.
    /// </summary>
    public int StartY
    {
        get
        {
            int setCcdTemperature = CameraHardware.StartY;
            LogMessage("StartY Get", setCcdTemperature.ToString());
            return setCcdTemperature;
        }
        set
        {
            LogMessage("StartY Set", value.ToString());
            CameraHardware.StartY = value;
        }
    }

    /// <summary>
    /// Stops the current exposure, if any.
    /// </summary>
    public void StopExposure()
    {
        LogMessage("StopExposure", $"Method called");
        CameraHardware.StopExposure();
    }

    /// <summary>
    /// Camera's sub-exposure interval
    /// </summary>
    public double SubExposureDuration
    {
        get
        {
            double subExposureDuration = CameraHardware.SubExposureDuration;
            LogMessage("SubExposureDuration Get", subExposureDuration.ToString());
            return subExposureDuration;
        }
        set
        {
            LogMessage("SubExposureDuration Set", value.ToString());
            CameraHardware.SubExposureDuration = value;
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

}