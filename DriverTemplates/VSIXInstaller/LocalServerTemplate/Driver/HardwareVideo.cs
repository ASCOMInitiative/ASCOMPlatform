// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.DeviceInterface;
using System.Collections;

static class VideoHardware
{
    #region IVideo Implementation

    private const int deviceWidth = 720; // Constants to define the ccd pixel dimenstions
    private const int deviceHeight = 480;
    private const int bitDepth = 8;
    private const string videoFileFormat = "AVI";

    /// <summary>
    /// Reports the bit depth the camera can produce.
    /// </summary>
    /// <value>The bit depth per pixel. Typical analogue videos are 8-bit while some digital cameras can provide 12, 14 or 16-bit images.</value>
    internal static int BitDepth
    {
        get
        {
            LogMessage("BitDepth Get", bitDepth.ToString());
            return bitDepth;
        }
    }

    /// <summary>
    /// Returns the current camera operational state.
    /// </summary>
    internal static VideoCameraState CameraState
    {
        get { return VideoCameraState.videoCameraError; }
    }

    /// <summary>
    /// Returns True if the driver supports custom device properties configuration via the <see cref="M:ASCOM.DeviceInterface.IVideo.ConfigureDeviceProperties"/> method.
    /// </summary>
    internal static bool CanConfigureDeviceProperties
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Displays a device properties configuration dialog that allows the configuration of specialized settings.
    /// </summary>
    internal static void ConfigureDeviceProperties()
    {
        throw new PropertyNotImplementedException();
    }

    /// <summary>
    /// The maximum supported exposure (integration time) in seconds.
    /// </summary>
    internal static double ExposureMax
    {
        get
        {
            // Standard NTSC frame duration
            return 0.03337;
        }
    }

    /// <summary>
    /// The minimum supported exposure (integration time) in seconds.
    /// </summary>
    internal static double ExposureMin
    {
        get
        {
            // Standard NTSC frame duration
            return 0.03337;
        }
    }

    /// <summary>
    /// The frame rate at which the camera is running.
    /// </summary>
    internal static VideoCameraFrameRate FrameRate
    {
        get { return VideoCameraFrameRate.NTSC; }
    }

    /// <summary>
    /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> array for the selected camera gain.
    /// </summary>
    /// <value>Short integer index for the current camera gain in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> string array.</value>
    /// <returns>Index into the Gains array for the selected camera gain</returns>
    internal static short Gain
    {

        get
        {
            throw new PropertyNotImplementedException("Gain", false);
        }


        set
        {
            throw new PropertyNotImplementedException("Gain", true);
        }
    }

    /// <summary>
    /// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
    /// </summary>
    internal static short GainMax
    {

        get
        {
            throw new PropertyNotImplementedException("GainMax", false);
        }
    }

    /// <summary>
    /// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
    /// </summary>
    /// <returns>The minimum gain value that this camera supports</returns>
    internal static short GainMin
    {

        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    /// <summary>
    /// Gains supported by the camera.
    /// </summary>
    /// <returns>An ArrayList of gain names or values</returns>
    internal static ArrayList Gains
    {

        get
        {
            throw new PropertyNotImplementedException("Gains", false);
        }
    }

    /// <summary>
    /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> array for the selected camera gamma.
    /// </summary>
    /// <value>Short integer index for the current camera gamma in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> string array.</value>
    /// <returns>Index into the Gammas array for the selected camera gamma</returns>
    internal static short Gamma
    {

        get
        {
            throw new PropertyNotImplementedException("Gamma", false);
        }


        set
        {
            throw new PropertyNotImplementedException("Gamma", true);
        }
    }

    /// <summary>
    /// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
    /// </summary>
    /// <value>Short integer representing the maximum gamma value supported by the camera.</value>
    /// <returns>The maximum gain value that this camera supports</returns>
    internal static short GammaMax
    {

        get
        {
            throw new PropertyNotImplementedException("GainMax", false);
        }
    }

    /// <summary>
    /// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
    /// </summary>
    /// <returns>The minimum gamma value that this camera supports</returns>
    internal static short GammaMin
    {

        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    /// <summary>
    /// Gammas supported by the camera.
    /// </summary>
    /// <returns>An ArrayList of gamma names or values</returns>
    internal static ArrayList Gammas
    {

        get
        {
            throw new PropertyNotImplementedException("Gammas", false);
        }
    }

    /// <summary>
    /// Returns the height of the video frame in pixels.
    /// </summary>
    /// <value>The video frame height.</value>
    internal static int Height
    {
        get
        {
            LogMessage("Height Get", deviceHeight.ToString());
            return deviceHeight;
        }
    }

    /// <summary>
    /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> array for the selected camera integration rate.
    /// </summary>
    /// <value>Integer index for the current camera integration rate in the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> string array.</value>
    /// <returns>Index into the SupportedIntegrationRates array for the selected camera integration rate.</returns>
    internal static int IntegrationRate
    {

        get
        {
            throw new PropertyNotImplementedException("IntegrationRate", false);
        }


        set
        {
            throw new PropertyNotImplementedException("IntegrationRate", true);
        }
    }

    /// <summary>
    /// Returns an <see cref="DeviceInterface.IVideoFrame"/> with its <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/> property populated.
    /// </summary>
    /// <value>The current video frame.</value>
    internal static IVideoFrame LastVideoFrame
    {
        get { throw new InvalidOperationException("There are no video frames available."); }
    }

    /// <summary>
    /// Returns the width of the CCD chip pixels in microns.
    /// </summary>
    internal static double PixelSizeX
    {

        get
        {
            throw new PropertyNotImplementedException("PixelSizeX", false);
        }
    }

    /// <summary>
    /// Returns the height of the CCD chip pixels in microns.
    /// </summary>
    /// <value>The pixel size Y if known.</value>
    internal static double PixelSizeY
    {

        get
        {
            throw new PropertyNotImplementedException("PixelSizeY", false);
        }
    }

    /// <summary>
    /// Sensor name.
    /// </summary>
    /// <returns>The name of sensor used within the camera.</returns>
    internal static string SensorName
    {

        get
        {
            throw new PropertyNotImplementedException("SensorName", false);
        }
    }

    /// <summary>
    /// Type of colour information returned by the the camera sensor.
    /// </summary>
    /// <returns>The <see cref="DeviceInterface.SensorType"/> enum value of the camera sensor</returns>
    internal static SensorType SensorType
    {
        get { return SensorType.Monochrome; }
    }

    /// <summary>
    /// Starts recording a new video file.
    /// </summary>
    /// <param name="PreferredFileName">The file name requested by the client. Some systems may not allow the file name to be controlled directly and they should ignore this parameter.</param>
    /// <returns>The actual file name of the file that is being recorded.</returns>
    internal static string StartRecordingVideoFile(string PreferredFileName)
    {
        LogMessage("StartRecordingVideoFile", "Supplied file name: " + PreferredFileName);
        throw new InvalidOperationException("Cannot start recording a video file right now.");
    }

    /// <summary>
    /// Stops the recording of a video file.
    /// </summary>
    internal static void StopRecordingVideoFile()
    {
        throw new InvalidOperationException("Cannot stop recording right now.");
    }

    /// <summary>
    /// Returns the list of integration rates supported by the video camera.
    /// </summary>
    /// <value>The list of supported integration rates in seconds.</value>
    internal static ArrayList SupportedIntegrationRates
    {

        get
        {
            throw new PropertyNotImplementedException("SupportedIntegrationRates", false);
        }
    }

    /// <summary>
    /// The name of the video capture device when such a device is used.
    /// </summary>
    internal static string VideoCaptureDeviceName
    {
        get { return string.Empty; }
    }

    /// <summary>
    /// Returns the video codec used to record the video file.
    /// </summary>
    internal static string VideoCodec
    {
        get { return string.Empty; }
    }

    /// <summary>
    /// Returns the file format of the recorded video file, e.g. AVI, MPEG, ADV etc.
    /// </summary>
    internal static string VideoFileFormat
    {
        get
        {
            LogMessage("VideoFileFormat Get", videoFileFormat);
            return videoFileFormat;
        }
    }

    /// <summary>
    /// The size of the video frame buffer.
    /// </summary>
    internal static int VideoFramesBufferSize
    {
        get { return 0; }
    }

    /// <summary>
    /// Returns the width of the video frame in pixels.
    /// </summary>
    /// <value>The video frame width.</value>
    internal static int Width
    {
        get
        {
            LogMessage("Height Width", deviceWidth.ToString());
            return deviceWidth;
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