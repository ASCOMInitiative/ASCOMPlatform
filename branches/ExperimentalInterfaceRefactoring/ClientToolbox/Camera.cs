// 10-Jul-08	rbd		1.0.5 - ImageArray returns type object, remove ImageArrayV which
//						was a Chris Rowland test/experiment. Can cast the object returned
//						by ImageArray into int[,]. Add COM releasing to Dispose().
// 01-Jan-10  	cdr     1.0.6 - Add Camera V2 properties as late bound properties.

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interfaces;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
	#region Camera Wrapper
    /// <summary>
    /// Implements a camera class to access any registered ASCOM Camera
    /// </summary>
    public class Camera : ICamera, IDisposable, IAscomDriver, IDeviceControl
    {
        protected object objCameraLateBound;
		protected ICamera iCamera;
		protected Type objTypeCamera;

        public Camera()
        {
        }
        
        /// <summary>
        /// Creates an instance of the camera class.
        /// </summary>
        /// <param name="cameraID">The ProgID for the camera</param>
        public Camera(string cameraID)
		{
			// Get Type Information 
            objTypeCamera = Type.GetTypeFromProgID(cameraID);
			
			// Create an instance of the camera object
            objCameraLateBound = Activator.CreateInstance(objTypeCamera);

			// Try to see if this driver has an ASCOM.Camera interface
			try
			{
				iCamera = (ICamera)objCameraLateBound;
			}
			catch (Exception)
			{
				iCamera = null;
			}
		}

        /// <summary>
        /// The Choose() method returns the DriverID of the selected driver.
        /// Choose() allows you to optionally pass the DriverID of a "current" driver,
        /// and the corresponding camera type is pre-selected in the Chooser///s list.
        /// In this case, the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured.
        /// </summary>
        /// <param name="cameraID">Optional DriverID of the previously selected camera that is to be the pre-selected camera in the list.</param>
        /// <returns>
        /// The DriverID of the user selected camera. Null if the dialog is canceled.
        /// </returns>
        public static string Choose(string cameraID)
        {
            Chooser oChooser = new Chooser();
            oChooser.DeviceType = "Camera";
            return oChooser.Choose(cameraID);
        }

        #region ICamera Members

        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// Must throw exception if camera is not idle and abort is
        /// unsuccessful (or not possible, e.g. during download).
        /// Must throw exception if hardware or communications error
        /// occurs.
        /// Must NOT throw an exception if the camera is already idle.
        /// </summary>
        public void AbortExposure()
        {
            if (iCamera != null)
                iCamera.AbortExposure();
            else
                objTypeCamera.InvokeMember("AbortExposure",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { });
        }

        /// <summary>
        /// Sets the binning factor for the X axis.  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <value>BinX sets/gets the X binning value</value>
        /// <exception>Must throw an exception for illegal binning values</exception>
        public short BinX
        {
            get
            {
                if (iCamera != null)
                    return iCamera.BinX;
                else
                    return Convert.ToInt16(objTypeCamera.InvokeMember("BinX",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.BinX = value;
                else
                    objTypeCamera.InvokeMember("BinX",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <value>The bin Y.</value>
        /// <exception>Must throw an exception for illegal binning values</exception>
        public short BinY
        {
            get
            {
                if (iCamera != null)
                    return iCamera.BinY;
                else
                    return Convert.ToInt16(objTypeCamera.InvokeMember("BinY",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.BinY = value;
                else
                    objTypeCamera.InvokeMember("BinY",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius. Only valid if
        /// CanControlTemperature is True.
        /// </summary>
        /// <value>The CCD temperature.</value>
        /// <exception>Must throw exception if data unavailable.</exception>
        public double CCDTemperature
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CCDTemperature;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("CCDTemperature",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns one of the following status information:
        /// <list type="bullet">
        /// 		<listheader>
        /// 			<description>Value  State          Meaning</description>
        /// 		</listheader>
        /// 		<item>
        /// 			<description>0      CameraIdle      At idle state, available to start exposure</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
        /// filter wheel, etc.)</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>2      CameraExposing  Exposure currently in progress</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>3      CameraReading   CCD array is being read out (digitized)</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>4      CameraDownload  Downloading data to PC</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>5      CameraError     Camera error condition serious enough to prevent
        /// further operations (link fail, etc.).</description>
        /// 		</item>
        /// 	</list>
        /// </summary>
        /// <value>The state of the camera.</value>
        /// <exception cref="System.Exception">Must return an exception if the camera status is unavailable.</exception>
        public CameraStates CameraState
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CameraState;
                else
                    return (CameraStates)objTypeCamera.InvokeMember("CameraState",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        public int CameraXSize
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CameraXSize;
                else
                {
                    return Convert.ToInt32( objTypeCamera.InvokeMember("CameraXSize",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
                }
            }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        public int CameraYSize
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CameraYSize;
                else
                {
                    return Convert.ToInt32( objTypeCamera.InvokeMember("CameraYSize",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
                }
            }
        }

        /// <summary>
        /// Returns True if the camera can abort exposures; False if not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can abort exposure; otherwise, <c>false</c>.
        /// </value>
        public bool CanAbortExposure
        {
	        get 
            {
				if (iCamera != null)
					return iCamera.CanAbortExposure;
				else
					return Convert.ToBoolean(objTypeCamera.InvokeMember("CanAbortExposure", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If True, the camera can have different binning on the X and Y axes, as
        /// determined by BinX and BinY. If False, the binning must be equal on the X and Y
        /// axes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known (n.b. normally only
        /// occurs if no link established and camera must be queried)</exception>
        public bool CanAsymmetricBin
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CanAsymmetricBin;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CanAsymmetricBin",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If True, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        public bool  CanGetCoolerPower
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CanGetCoolerPower;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CanGetCoolerPower",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns True if the camera can send autoguider pulses to the telescope mount;
        /// False if not.  (Note: this does not provide any indication of whether the
        /// autoguider cable is actually connected.)
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        public bool CanPulseGuide
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CanPulseGuide;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CanPulseGuide",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If True, the camera's cooler setpoint can be adjusted. If False, the camera
        /// either uses open-loop cooling or does not have the ability to adjust temperature
        /// from software, and setting the TemperatureSetpoint property has no effect.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        public bool CanSetCCDTemperature
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CanSetCCDTemperature;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CanSetCCDTemperature",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Some cameras support StopExposure, which allows the exposure to be terminated
        /// before the exposure timer completes, but will still read out the image.  Returns
        /// True if StopExposure is available, False if not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        public bool CanStopExposure
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CanStopExposure;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CanStopExposure",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// Warning: turning the cooler off when the cooler is operating at high delta-T
        /// (typically &gt;20C below ambient) may result in thermal shock.  Repeated thermal
        /// shock may lead to damage to the sensor or cooler stack.  Please consult the
        /// documentation supplied with the camera for further information.
        /// </summary>
        /// <value><c>true</c> if [cooler on]; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
       public bool CoolerOn
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CoolerOn;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("CoolerOn",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.CoolerOn = value;
                else
                    objTypeCamera.InvokeMember("CoolerOn",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

       /// <summary>
       /// Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
       /// False.
       /// </summary>
       /// <value>The cooler power.</value>
       /// <exception cref=" System.Exception">not supported</exception>
       /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        public double CoolerPower
        {
            get
            {
                if (iCamera != null)
                    return iCamera.CoolerPower;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("CoolerPower",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
        /// multiple gain modes; these should be selected via the SetupDialog and thus are
        /// static during a session.)
        /// </summary>
        /// <value>The electrons per ADU.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double ElectronsPerADU
        {
            get
            {
                if (iCamera != null)
                    return iCamera.ElectronsPerADU;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("ElectronsPerADU",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera
        /// settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double FullWellCapacity
        {
            get
            {
                if (iCamera != null)
                    return iCamera.FullWellCapacity;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("FullWellCapacity",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If True, the camera has a mechanical shutter. If False, the camera does not have
        /// a shutter.  If there is no shutter, the StartExposure command will ignore the
        /// Light parameter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
       public bool HasShutter
        {
            get
            {
                if (iCamera != null)
                    return iCamera.HasShutter;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("HasShutter",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

       /// <summary>
       /// Returns the current heat sink temperature (called "ambient temperature" by some
       /// manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
       /// </summary>
       /// <value>The heat sink temperature.</value>
       /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double HeatSinkTemperature
        {
            get
            {
                if (iCamera != null)
                    return iCamera.HeatSinkTemperature;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("HeatSinkTemperature",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns a safearray of int of size NumX * NumY containing the pixel values from
        /// the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This is the
        /// preferred method for programs (not scripts) to download iamges since it requires
        /// much less memory.
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <value>The image array.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public object ImageArray
        {
            get
            {
                if (iCamera != null)
                    return iCamera.ImageArray;
                else
                    return (object)objTypeCamera.InvokeMember("ImageArray",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Returns a safearray of Variant of size NumX * NumY containing the pixel values
        /// from the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This property
        /// should only be used from scripts due to the extremely high memory utilization on
        /// large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
        /// or Double format.
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <value>The image array variant.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public object ImageArrayVariant
        {
            get
            {
                if (iCamera != null)
                    return iCamera.ImageArrayVariant;
                else
                    return (object)objTypeCamera.InvokeMember("ImageArrayVariant",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// If True, there is an image from the camera available. If False, no image
        /// is available and attempts to use the ImageArray method will produce an
        /// exception.
        /// </summary>
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        public bool ImageReady
        {
            get
            {
                if (iCamera != null)
                    return iCamera.ImageReady;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("ImageReady",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If True, pulse guiding is in progress. Required if the PulseGuide() method
        /// (which is non-blocking) is implemented. See the PulseGuide() method.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        public bool IsPulseGuiding
        {
            get
            {
                if (iCamera != null)
                    return iCamera.IsPulseGuiding;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("IsPulseGuiding",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Reports the last error condition reported by the camera hardware or communications
        /// link.  The string may contain a text message or simply an error code.  The error
        /// value is cleared the next time any method is called.
        /// </summary>
        /// <value>The last error.</value>
        /// <exception cref=" System.Exception">Must throw exception if no error condition.</exception>
        public string LastError
        {
            get
            {
                if (iCamera != null)
                    return iCamera.LastError;
                else
                    return Convert.ToString(objTypeCamera.InvokeMember("LastError",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).  This
        /// may differ from the exposure time requested due to shutter latency, camera timing
        /// precision, etc.
        /// </summary>
        /// <value>The last duration of the exposure.</value>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        public double LastExposureDuration
        {
            get
            {
                if (iCamera != null)
                    return iCamera.LastExposureDuration;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("LastExposureDuration",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard
        /// CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// </summary>
        /// <value>The last exposure start time.</value>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        public string LastExposureStartTime
        {
            get
            {
                if (iCamera != null)
                    return iCamera.LastExposureStartTime;
                else
                    return Convert.ToString(objTypeCamera.InvokeMember("LastExposureStartTime",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The max ADU.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public int MaxADU
        {
            get
            {
                if (iCamera != null)
                    return iCamera.MaxADU;
                else
                    return Convert.ToInt32(objTypeCamera.InvokeMember("MaxADU",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If AsymmetricBinning = False, returns the maximum allowed binning factor. If
        /// AsymmetricBinning = True, returns the maximum allowed binning factor for the X
        /// axis.
        /// </summary>
        /// <value>The max bin X.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public short MaxBinX
        {
            get
            {
                if (iCamera != null)
                    return iCamera.MaxBinX;
                else
                    return Convert.ToInt16(objTypeCamera.InvokeMember("MaxBinX",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
        /// returns the maximum allowed binning factor for the Y axis.
        /// </summary>
        /// <value>The max bin Y.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public short MaxBinY
        {
            get
            {
                if (iCamera != null)
                    return iCamera.MaxBinY;
                else
                    return Convert.ToInt16(objTypeCamera.InvokeMember("MaxBinY",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraXSize.
        /// </summary>
        /// <value>The num X.</value>
        public int NumX
        {
            get
            {
                if (iCamera != null)
                    return iCamera.NumX;
                else
                    return Convert.ToInt32(objTypeCamera.InvokeMember("NumX",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.NumX = value;
                else
                    objTypeCamera.InvokeMember("NumX",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraYSize.
        /// </summary>
        /// <value>The num Y.</value>
        public int NumY
        {
            get
            {
                if (iCamera != null)
                    return iCamera.NumY;
                else
                    return Convert.ToInt32(objTypeCamera.InvokeMember("NumY",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.NumY = value;
                else
                    objTypeCamera.InvokeMember("NumY",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <value>The pixel size X.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double PixelSizeX
        {
            get
            {
                if (iCamera != null)
                    return iCamera.PixelSizeX;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("PixelSizeX",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <value>The pixel size Y.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        public double PixelSizeY
        {
            get
            {
                if (iCamera != null)
                    return iCamera.PixelSizeY;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("PixelSizeY",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// This method returns only after the move has completed.
        /// symbolic Constants
        /// The (symbolic) values for GuideDirections are:
        /// Constant     Value      Description
        /// --------     -----      -----------
        /// guideNorth     0        North (+ declination/elevation)
        /// guideSouth     1        South (- declination/elevation)
        /// guideEast      2        East (+ right ascension/azimuth)
        /// guideWest      3        West (+ right ascension/azimuth)
        /// Note: directions are nominal and may depend on exact mount wiring.  guideNorth
        /// must be opposite guideSouth, and guideEast must be opposite guideWest.
        /// </summary>
        /// <param name="Direction">The direction.</param>
        /// <param name="Duration">The duration.</param>
        /// <exception cref=" System.Exception">PulseGuide command is unsupported</exception>
        /// <exception cref=" System.Exception">PulseGuide command is unsuccessful</exception>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (iCamera != null)
                iCamera.PulseGuide(Direction, Duration);
            else
                objTypeCamera.InvokeMember("PulseGuide",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { Direction, Duration });
        }

        /// <summary>
        /// Sets the camera cooler setpoint in degrees Celsius, and returns the current
        /// setpoint.
        /// Note:  camera hardware and/or driver should perform cooler ramping, to prevent
        /// thermal shock and potential damage to the CCD array or cooler stack.
        /// </summary>
        /// <value>The set CCD temperature.</value>
        /// <exception cref=" System.Exception">Must throw exception if command not successful.</exception>
        /// <exception cref=" System.Exception">Must throw exception if CanSetCCDTemperature is False.</exception>
        public double SetCCDTemperature
        {
            get
            {
                if (iCamera != null)
                    return iCamera.SetCCDTemperature;
                else
                    return Convert.ToDouble(objTypeCamera.InvokeMember("SetCCDTemperature",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.SetCCDTemperature = value;
                else
                    objTypeCamera.InvokeMember("SetCCDTemperature",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Starts an exposure. Use ImageReady to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">Duration of exposure in seconds</param>
        /// <param name="Light">True for light frame, False for dark frame (ignored if no shutter)</param>
        /// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
        /// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
        /// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
        public void StartExposure(double Duration, bool Light)
        {
            if (iCamera != null)
                iCamera.StartExposure(Duration, Light);
            else
                objTypeCamera.InvokeMember("StartExposure",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { Duration, Light });
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        /// <value>The start X.</value>
        public int StartX
        {
            get
            {
                if (iCamera != null)
                    return iCamera.StartX;
                else
                    return Convert.ToInt32( objTypeCamera.InvokeMember("StartX",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.StartX = value;
                else
                    objTypeCamera.InvokeMember("StartX",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        /// <value>The start Y.</value>
        public int StartY
        {
            get
            {
                if (iCamera != null)
                    return iCamera.StartY;
                else
                    return Convert.ToInt32(objTypeCamera.InvokeMember("StartY",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.StartY = value;
                else
                    objTypeCamera.InvokeMember("StartY",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Stops the current exposure, if any.  If an exposure is in progress, the readout
        /// process is initiated.  Ignored if readout is already in process.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if CanStopExposure is False</exception>
        /// <exception cref=" System.Exception">Must throw an exception if no exposure is in progress</exception>
        /// <exception cref=" System.Exception">Must throw an exception if the camera or link has an error condition</exception>
        /// <exception cref=" System.Exception">Must throw an exception if for any reason no image readout will be available.</exception>
        public void StopExposure()
        {
            if (iCamera != null)
                iCamera.StopExposure();
            else
                objTypeCamera.InvokeMember("StopExposure",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] {  });
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
        /// </summary>
        public void Dispose()
        {
            if (this.objCameraLateBound != null)
            {
				try { Marshal.ReleaseComObject(objCameraLateBound); }
				catch (Exception) { }
				objCameraLateBound = null;
            }
        }

        #endregion

        #region IAscomDriver Members

        /// <summary>
        /// Set True to enable the
        /// link. Set False to disable the link (this does not switch off the cooler).
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
        public bool Connected
        {
            get
            {
                if (iCamera != null)
                    return iCamera.Connected;
                else
                    return Convert.ToBoolean(objTypeCamera.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
            set
            {
                if (iCamera != null)
                    iCamera.Connected = value;
                else
                    objTypeCamera.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objCameraLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Returns a description of the driver, such as manufacturer and model
        /// number. Any ASCII characters may be used. The string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
        public string Description
        {
            get
            {
                if (iCamera != null)
                    return iCamera.Description;
                else
                    return Convert.ToString(objTypeCamera.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM Telescope driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the telescope itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                if (iCamera != null)
                    return iCamera.DriverInfo;
                else
                    return (string)objTypeCamera.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
        /// </summary>
        public string DriverVersion
        {
            get
            {
                if (iCamera != null)
                    return iCamera.DriverVersion;
                else
                    return (string)objTypeCamera.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                if (iCamera != null)
                    return iCamera.InterfaceVersion;
                else
                    return (short)objTypeCamera.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        public string LastResult
        {
            get
            {
                if (iCamera != null)
                    return iCamera.LastResult;
                else
                    return Convert.ToString(objTypeCamera.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (iCamera != null)
                    return iCamera.Name;
                else
                    return (string)objTypeCamera.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (iCamera != null)
                iCamera.SetupDialog();
            else
                objTypeCamera.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { });
        }

        #endregion

        #region IDeviceControl Members

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="ActionName">
        /// A well known name agreed by interested parties that represents the action
        /// to be carried out. 
        /// <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change.
        /// </example>
        /// </param>
        /// <param name="ActionParameters">
        /// List of required parameters or <see cref="String.Empty"/>  if none are required.
        /// </param>
        /// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
        public string Action(string ActionName, string ActionParameters)
        {
            if (iCamera != null)
                return iCamera.Action(ActionName, ActionParameters);
            else
                return (string)objTypeCamera.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (iCamera != null)
                    return iCamera.SupportedActions;
                else
                    return (string[])(objTypeCamera.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objCameraLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string Command, bool Raw)
        {
            if (iCamera != null)
                iCamera.CommandBlind(Command, Raw);
            else
                objTypeCamera.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string Command, bool Raw)
        {
            if (iCamera != null)
                return iCamera.CommandBool(Command, Raw);
            else
                return (bool)objTypeCamera.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string Command, bool Raw)
        {
            if (iCamera != null)
                return iCamera.CommandString(Command, Raw);
            else
                return (string)objTypeCamera.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objCameraLateBound, new object[] { Command, Raw });
        }

        #endregion
    }

    #endregion
}
