// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System.Collections;
using System;
using System.Collections.Generic;

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
            try
            {
                CheckConnected("BitDepth");
                int bitDepth = VideoHardware.BitDepth;
                LogMessage("BitDepth", bitDepth.ToString());
                return bitDepth;
            }
            catch (Exception ex)
            {
                LogMessage("BitDepth", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the current camera operational state.
    /// </summary>
    public VideoCameraState CameraState
    {
        get
        {
            try
            {
                CheckConnected("CameraState");
                VideoCameraState cameraState = VideoHardware.CameraState;
                LogMessage("CameraState", cameraState.ToString());
                return cameraState;
            }
            catch (Exception ex)
            {
                LogMessage("CameraState", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns True if the driver supports custom device properties configuration via the <see cref="M:ASCOM.DeviceInterface.IVideo.ConfigureDeviceProperties"/> method.
    /// </summary>
    public bool CanConfigureDeviceProperties
    {
        get
        {
            try
            {
                CheckConnected("CanConfigureDeviceProperties");
                bool canConfigureDeviceProperties = VideoHardware.CanConfigureDeviceProperties;
                LogMessage("CanConfigureDeviceProperties", canConfigureDeviceProperties.ToString());
                return canConfigureDeviceProperties;
            }
            catch (Exception ex)
            {
                LogMessage("CanConfigureDeviceProperties", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Displays a device properties configuration dialog that allows the configuration of specialized settings.
    /// </summary>
    public void ConfigureDeviceProperties()
    {
        try
        {
            CheckConnected("ConfigureDeviceProperties");
            LogMessage("ConfigureDeviceProperties", $"Calling method.");
            VideoHardware.ConfigureDeviceProperties();
            LogMessage("ConfigureDeviceProperties", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("ConfigureDeviceProperties", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Return the device's state in one call.
    /// </summary>
    public IStateValueCollection DeviceState
    {
        get
        {
            try
            {
                CheckConnected("DeviceState");

                // Create an array list to hold the IStateValue entries
                List<IStateValue> deviceState = new List<IStateValue>();

                // Add one entry for each operational state, if possible
                try { deviceState.Add(new StateValue(nameof(IVideoV2.CameraState), CameraState)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                // Return the overall device state
                return new StateValueCollection(deviceState);
            }
            catch (Exception ex)
            {
                LogMessage("DeviceState", $"Threw an exception: {ex.Message}\r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The maximum supported exposure (integration time) in seconds.
    /// </summary>
    public double ExposureMax
    {
        get
        {
            try
            {
                CheckConnected("ExposureMax");
                double exposureMax = VideoHardware.ExposureMax;
                LogMessage("ExposureMax", exposureMax.ToString());
                return exposureMax;
            }
            catch (Exception ex)
            {
                LogMessage("ExposureMax", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The minimum supported exposure (integration time) in seconds.
    /// </summary>
    public double ExposureMin
    {
        get
        {
            try
            {
                CheckConnected("ExposureMin");
                double exposureMin = VideoHardware.ExposureMin;
                LogMessage("ExposureMin", exposureMin.ToString());
                return exposureMin;
            }
            catch (Exception ex)
            {
                LogMessage("ExposureMin", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The frame rate at which the camera is running.
    /// </summary>
    public VideoCameraFrameRate FrameRate
    {
        get
        {
            try
            {
                CheckConnected("FrameRate");
                VideoCameraFrameRate frameRate = VideoHardware.FrameRate;
                LogMessage("FrameRate", frameRate.ToString());
                return frameRate;
            }
            catch (Exception ex)
            {
                LogMessage("FrameRate", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Gain Get");
                short gain = VideoHardware.Gain;
                LogMessage("Gain Get", gain.ToString());
                return gain;
            }
            catch (Exception ex)
            {
                LogMessage("Gain Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        set
        {
            try
            {
                CheckConnected("Gain Set");
                LogMessage("Gain Set", value.ToString());
                VideoHardware.Gain = value;
            }
            catch (Exception ex)
            {
                LogMessage("Gain Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
    /// </summary>
    public short GainMax
    {
        get
        {
            try
            {
                CheckConnected("GainMax");
                short gainMax = VideoHardware.GainMax;
                LogMessage("GainMax", gainMax.ToString());
                return gainMax;
            }
            catch (Exception ex)
            {
                LogMessage("GainMax", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("GainMin");
                short gainMin = VideoHardware.GainMin;
                LogMessage("GainMin", gainMin.ToString());
                return gainMin;
            }
            catch (Exception ex)
            {
                LogMessage("GainMin", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Gains");
                ArrayList gains = VideoHardware.Gains;
                LogMessage("Gains", $"{gains.Count} gains were returned.");
                return gains;
            }
            catch (Exception ex)
            {
                LogMessage("Gains", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Gamma Get");
                short gamma = VideoHardware.Gamma;
                LogMessage("Gamma Get", gamma.ToString());
                return gamma;
            }
            catch (Exception ex)
            {
                LogMessage("Gamma Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        set
        {
            try
            {
                CheckConnected("Gamma Set");
                LogMessage("Gamma Set", value.ToString());
                VideoHardware.Gamma = value;
            }
            catch (Exception ex)
            {
                LogMessage("Gamma Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("GammaMax");
                short gammaMax = VideoHardware.GammaMax;
                LogMessage("GammaMax", gammaMax.ToString());
                return gammaMax;
            }
            catch (Exception ex)
            {
                LogMessage("GammaMax", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("GammaMin");
                short gammaMin = VideoHardware.GammaMin;
                LogMessage("GammaMin", gammaMin.ToString());
                return gammaMin;
            }
            catch (Exception ex)
            {
                LogMessage("GammaMin", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Gammas");
                ArrayList gammas = VideoHardware.Gammas;
                LogMessage("Gammas", $"{gammas.Count} gammas were returned.");
                return gammas;
            }
            catch (Exception ex)
            {
                LogMessage("Gammas", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Height");
                int height = VideoHardware.Height;
                LogMessage("Height", height.ToString());
                return height;
            }
            catch (Exception ex)
            {
                LogMessage("Height", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("IntegrationRate Get");
                int integrationRate = VideoHardware.IntegrationRate;
                LogMessage("IntegrationRate Get", integrationRate.ToString());
                return integrationRate;
            }
            catch (Exception ex)
            {
                LogMessage("IntegrationRate Get", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        set
        {
            try
            {
                CheckConnected("IntegrationRate Set");
                LogMessage("IntegrationRate Set", value.ToString());
                VideoHardware.IntegrationRate = value;
            }
            catch (Exception ex)
            {
                LogMessage("IntegrationRate Set", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("LastVideoFrame");
                IVideoFrame lastVideoFrame = VideoHardware.LastVideoFrame;
                LogMessage("LastVideoFrame", $"Returned frame number: {lastVideoFrame.FrameNumber}");
                return lastVideoFrame;
            }
            catch (Exception ex)
            {
                LogMessage("LastVideoFrame", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the width of the CCD chip pixels in microns.
    /// </summary>
    public double PixelSizeX
    {
        get
        {
            try
            {
                CheckConnected("PixelSizeX");
                double pixelSizeX = VideoHardware.PixelSizeX;
                LogMessage("PixelSizeX", pixelSizeX.ToString());
                return pixelSizeX;
            }
            catch (Exception ex)
            {
                LogMessage("PixelSizeX", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("PixelSizeY");
                double pixelSizeY = VideoHardware.PixelSizeY;
                LogMessage("PixelSizeY", pixelSizeY.ToString());
                return pixelSizeY;
            }
            catch (Exception ex)
            {
                LogMessage("PixelSizeY", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("SensorName");
                string sensorName = VideoHardware.SensorName;
                LogMessage("SensorName", sensorName.ToString());
                return sensorName;
            }
            catch (Exception ex)
            {
                LogMessage("SensorName", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("SensorType");
                SensorType sensorType = VideoHardware.SensorType;
                LogMessage("SensorType", sensorType.ToString());
                return sensorType;
            }
            catch (Exception ex)
            {
                LogMessage("SensorType", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Starts recording a new video file.
    /// </summary>
    /// <param name="PreferredFileName">The file name requested by the client. Some systems may not allow the file name to be controlled directly and they should ignore this parameter.</param>
    /// <returns>The actual file name of the file that is being recorded.</returns>
    public string StartRecordingVideoFile(string PreferredFileName)
    {
        try
        {
            CheckConnected("StartRecordingVideoFile");
            LogMessage("StartRecordingVideoFile", $"Calling method.");
            string startRecordingVideoFile = VideoHardware.StartRecordingVideoFile(PreferredFileName);
            LogMessage("StartRecordingVideoFile", startRecordingVideoFile.ToString());
            return startRecordingVideoFile;
        }
        catch (Exception ex)
        {
            LogMessage("StartRecordingVideoFile", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Stops the recording of a video file.
    /// </summary>
    public void StopRecordingVideoFile()
    {
        try
        {
            CheckConnected("StopRecordingVideoFile");
            LogMessage("StopRecordingVideoFile", $"Calling method.");
            VideoHardware.StopRecordingVideoFile();
            LogMessage("StopRecordingVideoFile", $"Completed.");
        }
        catch (Exception ex)
        {
            LogMessage("StopRecordingVideoFile", $"Threw an exception: \r\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Returns the list of integration rates supported by the video camera.
    /// </summary>
    /// <value>The list of supported integration rates in seconds.</value>
    public ArrayList SupportedIntegrationRates
    {
        get
        {
            try
            {
                CheckConnected("SupportedIntegrationRates");
                ArrayList supportedIntegrationRates = VideoHardware.SupportedIntegrationRates;
                LogMessage("SupportedIntegrationRates", $"Returned {supportedIntegrationRates.Count} rates.");
                return supportedIntegrationRates;
            }
            catch (Exception ex)
            {
                LogMessage("SupportedIntegrationRates", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The name of the video capture device when such a device is used.
    /// </summary>
    public string VideoCaptureDeviceName
    {
        get
        {
            try
            {
                CheckConnected("VideoCaptureDeviceName");
                string videoCaptureDeviceName = VideoHardware.VideoCaptureDeviceName;
                LogMessage("VideoCaptureDeviceName", videoCaptureDeviceName.ToString());
                return videoCaptureDeviceName;
            }
            catch (Exception ex)
            {
                LogMessage("VideoCaptureDeviceName", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the video codec used to record the video file.
    /// </summary>
    public string VideoCodec
    {
        get
        {
            try
            {
                CheckConnected("VideoCodec");
                string videoCodec = VideoHardware.VideoCodec;
                LogMessage("VideoCodec", videoCodec.ToString());
                return videoCodec;
            }
            catch (Exception ex)
            {
                LogMessage("VideoCodec", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the file format of the recorded video file, e.g. AVI, MPEG, ADV etc.
    /// </summary>
    public string VideoFileFormat
    {
        get
        {
            try
            {
                CheckConnected("VideoFileFormat");
                string videoFileFormat = VideoHardware.VideoFileFormat;
                LogMessage("VideoFileFormat", videoFileFormat.ToString());
                return videoFileFormat;
            }
            catch (Exception ex)
            {
                LogMessage("VideoFileFormat", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }
    }

    /// <summary>
    /// The size of the video frame buffer.
    /// </summary>
    public int VideoFramesBufferSize
    {
        get
        {
            try
            {
                CheckConnected("VideoFramesBufferSize");
                int videoFramesBufferSize = VideoHardware.VideoFramesBufferSize;
                LogMessage("VideoFramesBufferSize", videoFramesBufferSize.ToString());
                return videoFramesBufferSize;
            }
            catch (Exception ex)
            {
                LogMessage("VideoFramesBufferSize", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
            try
            {
                CheckConnected("Width");
                int width = VideoHardware.Width;
                LogMessage("Width", width.ToString());
                return width;
            }
            catch (Exception ex)
            {
                LogMessage("Width", $"Threw an exception: \r\n{ex}");
                throw;
            }
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

    /// <summary>
    /// Dummy CheckConnected class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    /// <param name="message"></param>
    private void CheckConnected(string message)
    {
    }
}