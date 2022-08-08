// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;
using System.Diagnostics;
using ASCOM.DeviceInterface;
using System.Collections;

class DeviceVideo
{
    #region IVideo Implementation

	/// <summary>
	/// Reports the bit depth the camera can produce.
	/// </summary>
	/// <value>The bit depth per pixel. Typical analogue videos are 8-bit while some digital cameras can provide 12, 14 or 16-bit images.</value>
	public int BitDepth
    {
        get
        {
            int bitDepth = VideoHardware.BitDepth;
            LogMessage("BitDepth Get", bitDepth.ToString());
            return bitDepth;
        }
    }

	/// <summary>
	/// Returns the current camera operational state.
	/// </summary>
	public VideoCameraState CameraState
    {
        get 
        {
            VideoCameraState cameraState = VideoHardware.CameraState;
            LogMessage("CameraState Get", cameraState.ToString());
            return cameraState;
        }
    }

	/// <summary>
	/// Returns True if the driver supports custom device properties configuration via the <see cref="M:ASCOM.DeviceInterface.IVideo.ConfigureDeviceProperties"/> method.
	/// </summary>
	public bool CanConfigureDeviceProperties
    {
        get
        {
            bool canConfigureDeviceProperties = VideoHardware.CanConfigureDeviceProperties;
            LogMessage("CanConfigureDeviceProperties Get", canConfigureDeviceProperties.ToString());
            return canConfigureDeviceProperties;
        }
    }

	/// <summary>
	/// Displays a device properties configuration dialog that allows the configuration of specialized settings.
	/// </summary>
	public void ConfigureDeviceProperties()
    {
        LogMessage("ConfigureDeviceProperties", $"Calling method.");
        VideoHardware.ConfigureDeviceProperties();
        LogMessage("ConfigureDeviceProperties", $"Completed.");
    }

    /// <summary>
    /// The maximum supported exposure (integration time) in seconds.
    /// </summary>
    public double ExposureMax
    {
        get
        {
            double exposureMax = VideoHardware.ExposureMax;
            LogMessage("ExposureMax Get", exposureMax.ToString());
            return exposureMax;
        }
    }

	/// <summary>
	/// The minimum supported exposure (integration time) in seconds.
	/// </summary>
	public double ExposureMin
    {
        get
        {
            double exposureMin = VideoHardware.ExposureMin;
            LogMessage("ExposureMin Get", exposureMin.ToString());
            return exposureMin;
        }
    }

	/// <summary>
	/// The frame rate at which the camera is running.
	/// </summary>
	public VideoCameraFrameRate FrameRate
    {
        get
        {
            VideoCameraFrameRate frameRate = VideoHardware.FrameRate;
            LogMessage("FrameRate Get", frameRate.ToString());
            return frameRate;
        }
    }

	/// <summary>
	/// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> array for the selected camera gain.
	/// </summary>
	/// <value>Short integer index for the current camera gain in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> string array.</value>
	/// <returns>Index into the Gains array for the selected camera gain</returns>
	public short Gain
    {
        get
        {
            short gain = VideoHardware.Gain;
            LogMessage("Gain Get", gain.ToString());
            return gain;
        }

        set
        {
            LogMessage("Gain Set", value.ToString());
            VideoHardware.Gain = value;
        }
    }

	/// <summary>
	/// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
	/// </summary>
	public short GainMax
    {
        get
        {
            short gainMax = VideoHardware.GainMax;
            LogMessage("GainMax Get", gainMax.ToString());
            return gainMax;
        }
    }

	/// <summary>
	/// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
	/// </summary>
	/// <returns>The minimum gain value that this camera supports</returns>
	public short GainMin
    {
        get
        {
            short gainMin = VideoHardware.GainMin;
            LogMessage("GainMin Get", gainMin.ToString());
            return gainMin;
        }
    }

	/// <summary>
	/// Gains supported by the camera.
	/// </summary>
	/// <returns>An ArrayList of gain names or values</returns>
	public ArrayList Gains
    {
        get
        {
            ArrayList gains = VideoHardware.Gains;
            LogMessage("Gains Get", $"{gains.Count} gains were returned.");
            return gains;
        }
    }

	/// <summary>
	/// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> array for the selected camera gamma.
	/// </summary>
	/// <value>Short integer index for the current camera gamma in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> string array.</value>
	/// <returns>Index into the Gammas array for the selected camera gamma</returns>
	public short Gamma
    {
        get
        {
            short gamma = VideoHardware.Gamma;
            LogMessage("Gamma Get", gamma.ToString());
            return gamma;
        }

        set
        {
            LogMessage("Gamma Set", value.ToString());
            VideoHardware.Gamma = value;
        }
    }

	/// <summary>
	/// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
	/// </summary>
	/// <value>Short integer representing the maximum gamma value supported by the camera.</value>
	/// <returns>The maximum gain value that this camera supports</returns>
	public short GammaMax
    {
        get
        {
            short gammaMax = VideoHardware.GammaMax;
            LogMessage("GammaMax Get", gammaMax.ToString());
            return gammaMax;
        }
    }

	/// <summary>
	/// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
	/// </summary>
	/// <returns>The minimum gamma value that this camera supports</returns>
	public short GammaMin
    {
        get
        {
            short gammaMin = VideoHardware.GammaMin;
            LogMessage("GammaMin Get", gammaMin.ToString());
            return gammaMin;
        }
    }

	/// <summary>
	/// Gammas supported by the camera.
	/// </summary>
	/// <returns>An ArrayList of gamma names or values</returns>
	public ArrayList Gammas
    {
        get
        {
            ArrayList gammas = VideoHardware.Gammas;
            LogMessage("Gammas Get", $"{gammas.Count} gammas were returned.");
            return gammas;
        }
    }

	/// <summary>
	/// Returns the height of the video frame in pixels.
	/// </summary>
	/// <value>The video frame height.</value>
	public int Height
    {
        get
        {
            int height = VideoHardware.Height;
            LogMessage("Height Get", height.ToString());
            return height;
        }
    }

	/// <summary>
	/// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> array for the selected camera integration rate.
	/// </summary>
	/// <value>Integer index for the current camera integration rate in the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> string array.</value>
	/// <returns>Index into the SupportedIntegrationRates array for the selected camera integration rate.</returns>
	public int IntegrationRate
    {
        get
        {
            int integrationRate = VideoHardware.IntegrationRate;
            LogMessage("IntegrationRate Get", integrationRate.ToString());
            return integrationRate;
        }

        set
        {
            LogMessage("IntegrationRate Set", value.ToString());
            VideoHardware.IntegrationRate = value;
        }
    }

	/// <summary>
	/// Returns an <see cref="DeviceInterface.IVideoFrame"/> with its <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/> property populated.
	/// </summary>
	/// <value>The current video frame.</value>
	public IVideoFrame LastVideoFrame
    {
        get 
        {
            IVideoFrame lastVideoFrame = VideoHardware.LastVideoFrame;
            LogMessage("LastVideoFrame Get", $"Returned frame number: {lastVideoFrame.FrameNumber}");
            return lastVideoFrame;
        }
    }

	/// <summary>
	/// Returns the width of the CCD chip pixels in microns.
	/// </summary>
	public double PixelSizeX
    {
        get
        {
            double pixelSizeX = VideoHardware.PixelSizeX;
            LogMessage("PixelSizeX Get", pixelSizeX.ToString());
            return pixelSizeX;
        }
    }

	/// <summary>
	/// Returns the height of the CCD chip pixels in microns.
	/// </summary>
	/// <value>The pixel size Y if known.</value>
	public double PixelSizeY
    {
        get
        {
            double pixelSizeY = VideoHardware.PixelSizeY;
            LogMessage("PixelSizeY Get", pixelSizeY.ToString());
            return pixelSizeY;
        }
    }

	/// <summary>
	/// Sensor name.
	/// </summary>
	/// <returns>The name of sensor used within the camera.</returns>
	public string SensorName
    {
        get
        {
            string sensorName = VideoHardware.SensorName;
            LogMessage("SensorName Get", sensorName.ToString());
            return sensorName;
        }
    }

	/// <summary>
	/// Type of colour information returned by the the camera sensor.
	/// </summary>
	/// <returns>The <see cref="DeviceInterface.SensorType"/> enum value of the camera sensor</returns>
	public SensorType SensorType
    {
        get
        {
            SensorType sensorType = VideoHardware.SensorType;
            LogMessage("SensorType Get", sensorType.ToString());
            return sensorType;
        }
    }

	/// <summary>
	/// Starts recording a new video file.
	/// </summary>
	/// <param name="PreferredFileName">The file name requested by the client. Some systems may not allow the file name to be controlled directly and they should ignore this parameter.</param>
	/// <returns>The actual file name of the file that is being recorded.</returns>
	public string StartRecordingVideoFile(string PreferredFileName)
    {
        LogMessage("StartRecordingVideoFile", $"Calling method.");
        string startRecordingVideoFile=VideoHardware.StartRecordingVideoFile(PreferredFileName);
        LogMessage("StartRecordingVideoFile", startRecordingVideoFile.ToString());
        return startRecordingVideoFile;
    }

    /// <summary>
    /// Stops the recording of a video file.
    /// </summary>
    public void StopRecordingVideoFile()
    {
        LogMessage("StopRecordingVideoFile", $"Calling method.");
        VideoHardware.StopRecordingVideoFile();
        LogMessage("StopRecordingVideoFile", $"Completed.");
    }

    /// <summary>
    /// Returns the list of integration rates supported by the video camera.
    /// </summary>
    /// <value>The list of supported integration rates in seconds.</value>
    public ArrayList SupportedIntegrationRates
    {
        get
        {
            ArrayList supportedIntegrationRates = VideoHardware.SupportedIntegrationRates;
            LogMessage("SupportedIntegrationRates Get", $"Returned {supportedIntegrationRates.Count} rates.");
            return supportedIntegrationRates;
        }
    }

	/// <summary>
	/// The name of the video capture device when such a device is used.
	/// </summary>
	public string VideoCaptureDeviceName
    {
        get
        {
            string videoCaptureDeviceName = VideoHardware.VideoCaptureDeviceName;
            LogMessage("VideoCaptureDeviceName Get", videoCaptureDeviceName.ToString());
            return videoCaptureDeviceName;
        }
    }

	/// <summary>
	/// Returns the video codec used to record the video file.
	/// </summary>
	public string VideoCodec
    {
        get 
        {
            string videoCodec = VideoHardware.VideoCodec;
            LogMessage("VideoCodec Get", videoCodec.ToString());
            return videoCodec;
        }
    }

	/// <summary>
	/// Returns the file format of the recorded video file, e.g. AVI, MPEG, ADV etc.
	/// </summary>
	public string VideoFileFormat
    {
        get
        {
            string videoFileFormat = VideoHardware.VideoFileFormat;
            LogMessage("VideoFileFormat Get", videoFileFormat.ToString());
            return videoFileFormat;
        }
    }

	/// <summary>
	/// The size of the video frame buffer.
	/// </summary>
	public int VideoFramesBufferSize
    {
        get 
        {
            int videoFramesBufferSize = VideoHardware.VideoFramesBufferSize;
            LogMessage("VideoFramesBufferSize Get", videoFramesBufferSize.ToString());
            return videoFramesBufferSize;
        }
    }

	/// <summary>
	/// Returns the width of the video frame in pixels.
	/// </summary>
	/// <value>The video frame width.</value>
	public int Width
    {
        get
        {
            int width = VideoHardware.Width;
            LogMessage("Width Get", width.ToString());
            return width;
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