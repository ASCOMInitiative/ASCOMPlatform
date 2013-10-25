// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;
using System.Diagnostics;

class DeviceVideoUsingBaseClass
{
    private TraceLogger tl = new TraceLogger();

    #region IVideo Implementation

    /// <exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if camera supports only one integration rate (exposure) that cannot be changed.</exception>		
    public System.Collections.ArrayList SupportedIntegrationRates
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("SupportedIntegrationRates", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if the camera supports only one integration rate (exposure) that cannot be changed.</exception>
    public int IntegrationRate
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("IntegrationRate", false);
        }

        [DebuggerStepThrough]
        set
        {
            throw new PropertyNotImplementedException("IntegrationRate", true);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public string SensorName
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("SensorName", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
    public int CameraXSize
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("CameraXSize", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public int CameraYSize
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("CameraYSize", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public double PixelSizeX
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("PixelSizeX", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    /// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public double PixelSizeY
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("PixelSizeY", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
    public short GainMax
    {
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
    public short Gain
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("Gain", false);
        }

        [DebuggerStepThrough]
        set
        {
            throw new PropertyNotImplementedException("Gain", true);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if Gains is not supported</exception>
    public System.Collections.ArrayList Gains
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("Gains", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
    public short GammaMax
    {
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("GainMin", false);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gamma is not supported</exception>
    public short Gamma
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("Gamma", false);
        }

        [DebuggerStepThrough]
        set
        {
            throw new PropertyNotImplementedException("Gamma", true);
        }
    }

    ///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    public System.Collections.ArrayList Gammas
    {
        [DebuggerStepThrough]
        get
        {
            throw new PropertyNotImplementedException("Gammas", false);
        }
    }


    #endregion

    //ENDOFINSERTEDFILE
}