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
    private TraceLogger tl = new TraceLogger();
    Util util = new Util();

    #region IVideo Implementation

    private const int deviceWidth = 720; // Constants to define the ccd pixel dimenstions
    private const int deviceHeight = 480;
    private const int bitDepth = 8;
    private const string videoFileFormat = "AVI";

    public int BitDepth
    {
        get
        {
            tl.LogMessage("BitDepth Get", bitDepth.ToString());
            return bitDepth;
        }
    }

    public VideoCameraState CameraState
    {
        get { return VideoCameraState.videoCameraError; }
    }

    public bool CanConfigureDeviceProperties
    {
        get
        {
            return false;
        }
    }

    public void ConfigureDeviceProperties()
    {
        throw new ASCOM.PropertyNotImplementedException();
    }

    public double ExposureMax
    {
        get
        {
            // Standard NTSC frame duration
            return 0.03337;
        }
    }

    public double ExposureMin
    {
        get
        {
            // Standard NTSC frame duration
            return 0.03337;
        }
    }

    public VideoCameraFrameRate FrameRate
    {
        get { return VideoCameraFrameRate.NTSC; }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
    public short Gain
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

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
    public short GainMax
    {

        get
        {
            throw new PropertyNotImplementedException("GainMax", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public short GainMin
    {

        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if Gains is not supported</exception>
    public ArrayList Gains
    {

        get
        {
            throw new PropertyNotImplementedException("Gains", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gamma is not supported</exception>
    public short Gamma
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

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
    public short GammaMax
    {

        get
        {
            throw new PropertyNotImplementedException("GainMax", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public short GammaMin
    {

        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public ArrayList Gammas
    {

        get
        {
            throw new PropertyNotImplementedException("Gammas", false);
        }
    }

    public int Height
    {
        get
        {
            tl.LogMessage("Height Get", deviceHeight.ToString());
            return deviceHeight;
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if the camera supports only one integration rate (exposure) that cannot be changed.</exception>
    public int IntegrationRate
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

    public IVideoFrame LastVideoFrame
    {
        get { throw new ASCOM.InvalidOperationException("There are no video frames available."); }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if not implemented.</exception>
    public double PixelSizeX
    {

        get
        {
            throw new PropertyNotImplementedException("PixelSizeX", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if not implemented.</exception>
    public double PixelSizeY
    {

        get
        {
            throw new PropertyNotImplementedException("PixelSizeY", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if not implemented.</exception>
    public string SensorName
    {

        get
        {
            throw new PropertyNotImplementedException("SensorName", false);
        }
    }

    public SensorType SensorType
    {
        get { return SensorType.Monochrome; }
    }

    public string StartRecordingVideoFile(string PreferredFileName)
    {
        tl.LogMessage("StartRecordingVideoFile", "Supplied file name: " + PreferredFileName);
        throw new InvalidOperationException("Cannot start recording a video file right now.");
    }

    public void StopRecordingVideoFile()
    {
        throw new InvalidOperationException("Cannot stop recording right now.");
    }

    /// <exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if camera supports only one integration rate (exposure) that cannot be changed.</exception>		
    public ArrayList SupportedIntegrationRates
    {

        get
        {
            throw new PropertyNotImplementedException("SupportedIntegrationRates", false);
        }
    }

    public string VideoCaptureDeviceName
    {
        get { return string.Empty; }
    }

    public string VideoCodec
    {
        get { return string.Empty; }
    }

    public string VideoFileFormat
    {
        get
        {
            tl.LogMessage("VideoFileFormat Get", videoFileFormat);
            return videoFileFormat;
        }
    }

    public int VideoFramesBufferSize
    {
        get { return 0; }
    }

    public int Width
    {
        get
        {
            tl.LogMessage("Height Width", deviceWidth.ToString());
            return deviceWidth;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}