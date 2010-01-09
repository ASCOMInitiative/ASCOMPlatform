//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Camera driver for Simulator
//
// Description:	A very basic Camera simulator.
//
// Implements:	ASCOM Camera interface version: 1.0
// Author:		Bob Denny <rdenny@dc3.com>
//				using Matthias Busch's VB6 Camera Simulator and Chris Rowland's
//				C#.NET Camera Driver template.
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 14-Oct-2007	rbd	1.0.0	Initial edit, from ASCOM Camera Driver template
// 09-Jan-2009  cdr 6.0.0   Get the basic functionality working, some V2 properties in place but no interface
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using Helper = ASCOM.Utilities;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Your driver's ID is ASCOM.Simulator.Camera
	/// The Guid attribute sets the CLSID for ASCOM.Simulator.Camera
	/// The ClassInterface/None attribute prevents an empty interface called
	/// _Camera from being created and used as the [default] interface
    /// </summary>
	[Guid("12229c31-e7d6-49e8-9c5d-5d7ff05c3bfe")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Camera : ICamera
	{

        #region internal properties

        //Pixel
        internal double pixelSizeX;
        internal double pixelSizeY;
        internal double fullWellCapacity;
        internal int maxADU;
        internal double electronsPerADU;

        //CCD
        internal int cameraXSize;
        internal int cameraYSize;
        internal bool canAsymmetricBin;
        internal short maxBinX;
        internal short maxBinY;
        internal short binX;
        internal short binY;
        internal bool hasShutter;
        internal string sensorName;
        internal int sensorType;    // TODO make an Enum
        internal int bayerOffsetX;
        internal int bayerOffsetY;

        internal int startX;
        internal int startY;
        internal int numX;
        internal int numY;

        //cooling
        internal bool hasCooler;
        private bool coolerOn;
        internal bool canSetCcdTemperature;
        internal bool canGetCoolerPower;
        internal double coolerPower;
        internal double ccdTemperature;
        internal double heatSinkTemperature;
        internal double setCcdTemperature;

        // Gain
        internal string[] gains;
        internal int gainMin;
        internal int gainMax;

        // Exposure
        internal bool canAbortExposure;
        internal bool canStopExposure;
        internal double minExposure;
        internal double maxExposure;
        internal double exposureResolution;
        private double lastExposureDuration;
        private string lastExposureStartTime;

        internal bool imageReady = false;


        // simulation
        internal bool applyNoise;
        internal bool applyStars;

        internal bool connected=false;
        internal CameraStates cameraState = CameraStates.cameraIdle;

        private int[,] imageArray;
        private object[,] imageArrayVariant;

        private Timer exposureTimer;

        private string lastError = string.Empty;

        #endregion

		#region Camera Constructor
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
        internal static string s_csDriverID = "ASCOM.Simulator.Camera";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "Camera V2 simulator";
        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// Must be public for COM registration!
        /// </summary>
		public Camera()
		{
			// TODO Implement your additional construction here
            InitialiseSimulator();
		}
		#endregion

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
		{
			Helper.Profile P = new Helper.Profile();
			P.DeviceType = "Camera";					//  Requires Helper 5.0.3 or later
			if (bRegister)
				P.Register(s_csDriverID, s_csDriverDescription);
			else
				P.Unregister(s_csDriverID);
			try                        // In case Helper becomes native .NET
			{
				Marshal.ReleaseComObject(P);
			}
			catch (Exception) { }
			P = null;
		}

		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
		{
			RegUnregASCOM(true);
		}

		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
		{
			RegUnregASCOM(false);
		}
		#endregion


        //
		// PUBLIC COM INTERFACE ICamera IMPLEMENTATION
		//

		#region ICamera Members


		/// <summary>
		/// Aborts the current exposure, if any, and returns the camera to Idle state.
		/// Must throw exception if camera is not idle and abort is
		///  unsuccessful (or not possible, e.g. during download).
		/// Must throw exception if hardware or communications error
		///  occurs.
		/// Must NOT throw an exception if the camera is already idle.
		/// </summary>
		public void AbortExposure()
		{
			throw new MethodNotImplementedException("AbortExposure");
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
                if (!this.connected)
                    throw new NotConnectedException("Can't read BinX when not connected");
                return this.binX;
			}
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set BinX when not connected");
                if (value > this.maxBinX || value < 1)
                    throw new InvalidValueException("BinX", value.ToString("d2"), string.Format("1 to {0}", this.MaxBinX));
				this.binX = value;
                if (!this.canAsymmetricBin)
                    this.binY = value;
			}
		}

		/// <summary>
		/// Sets the binning factor for the Y axis  Also returns the current value.  Should
		/// default to 1 when the camera link is established.  Note:  driver does not check
		/// for compatible subframe values when this value is set; rather they are checked
		/// upon StartExposure.
		/// </summary>
		/// <exception>Must throw an exception for illegal binning values</exception>
		public short BinY
		{
			get
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't read BinY when not connected");
                return this.binY;
			}
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set BinY when not connected");
                if (value > this.maxBinY || value < 1)
                    throw new InvalidValueException("BinY", value.ToString("d2"), string.Format("1 to {0}", this.MaxBinY));
				this.binY = value;
                if (!this.canAsymmetricBin)
                    this.binX = value;
			}
		}

		/// <summary>
		/// Returns the current CCD temperature in degrees Celsius. Only valid if
		/// CanControlTemperature is True.
		/// </summary>
		/// <exception>Must throw exception if data unavailable.</exception>
		public double CCDTemperature
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read the CCD temperature when not connected");
                return this.ccdTemperature;
            }
		}

		/// <summary>
		/// Returns one of the following status information:
		/// <list type="bullet">
		///  <listheader>
		///   <description>Value  State          Meaning</description>
		///  </listheader>
		///  <item>
		///   <description>0      CameraIdle      At idle state, available to start exposure</description>
		///  </item>
		///  <item>
		///   <description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
		///                        filter wheel, etc.)</description>
		///  </item>
		///  <item>
		///   <description>2      CameraExposing  Exposure currently in progress</description>
		///  </item>
		///  <item>
		///   <description>3      CameraReading   CCD array is being read out (digitized)</description>
		///  </item>
		///  <item>
		///   <description>4      CameraDownload  Downloading data to PC</description>
		///  </item>
		///  <item>
		///   <description>5      CameraError     Camera error condition serious enough to prevent
		///                        further operations (link fail, etc.).</description>
		///  </item>
		/// </list>
		/// </summary>
		/// <exception cref="System.Exception">Must return an exception if the camera status is unavailable.</exception>
		public CameraStates CameraState
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read the camera state when not connected");
                return this.cameraState;
            }
		}

		/// <summary>
		/// Returns the width of the CCD camera chip in unbinned pixels.
		/// </summary>
		/// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
		public int CameraXSize
		{
			get
            { 
                if (!this.connected)
                    throw new NotConnectedException("Can't read the camera Xsize when not connected");
                return this.cameraXSize;
            }
		}

		/// <summary>
		/// Returns the height of the CCD camera chip in unbinned pixels.
		/// </summary>
		/// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
		public int CameraYSize
		{
			get
            { 
                if (!this.connected)
                    throw new NotConnectedException("Can't read the camera Ysize when not connected");
                return this.cameraYSize;
            }
		}

		/// <summary>
		/// Returns True if the camera can abort exposures; False if not.
		/// </summary>
		public bool CanAbortExposure
		{
			get 
            { 
                if (!this.connected)
                    throw new NotConnectedException("Can't read CanAbortExposure when not connected");
                return this.canAbortExposure; 
            }
		}

		/// <summary>
		/// If True, the camera can have different binning on the X and Y axes, as
		/// determined by BinX and BinY. If False, the binning must be equal on the X and Y
		/// axes.
		/// </summary>
		/// <exception cref="System.Exception">Must throw exception if the value is not known (n.b. normally only
		///            occurs if no link established and camera must be queried)</exception>
		public bool CanAsymmetricBin
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read CanAsymmetricBin when not connected");
                return this.canAsymmetricBin;
            }
        }

		/// <summary>
		/// If True, the camera's cooler power setting can be read.
		/// </summary>
        public bool CanGetCoolerPower
        {
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read CanGetCoolerPower when not connected");
                return this.canGetCoolerPower;
            }
        }

		/// <summary>
		/// Returns True if the camera can send autoguider pulses to the telescope mount;
		/// False if not.  (Note: this does not provide any indication of whether the
		/// autoguider cable is actually connected.)
		/// </summary>
		public bool CanPulseGuide
		{
			get { return false; }
		}

		/// <summary>
		/// If True, the camera's cooler setpoint can be adjusted. If False, the camera
		/// either uses open-loop cooling or does not have the ability to adjust temperature
		/// from software, and setting the TemperatureSetpoint property has no effect.
		/// </summary>
		public bool CanSetCCDTemperature
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read CanSetCCDTemperature when not connected");
                return this.canSetCcdTemperature;
            }
        }

		/// <summary>
		/// Some cameras support StopExposure, which allows the exposure to be terminated
		/// before the exposure timer completes, but will still read out the image.  Returns
		/// True if StopExposure is available, False if not.
		/// </summary>
		/// <exception cref=" System.Exception">not supported</exception>
		/// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
		public bool CanStopExposure
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read CanStopExposure when not connected");
                return this.canStopExposure;
            }
		}

		/// <summary>
		/// Controls the link between the driver and the camera. Set True to enable the
		/// link. Set False to disable the link (this does not switch off the cooler).
		/// You can also read the property to check whether it is connected.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
		public bool Connected
		{
			get
			{
				return this.connected;
			}
			set
			{
                this.connected = value;
			}
		}

		/// <summary>
		/// Turns on and off the camera cooler, and returns the current on/off state.
		/// Warning: turning the cooler off when the cooler is operating at high delta-T
		/// (typically >20C below ambient) may result in thermal shock.  Repeated thermal
		/// shock may lead to damage to the sensor or cooler stack.  Please consult the
		/// documentation supplied with the camera for further information.
		/// </summary>
		/// <exception cref=" System.Exception">not supported</exception>
		/// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
		public bool CoolerOn
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read CoolerOn when not connected");
                if (!this.hasCooler)
                    throw new PropertyNotImplementedException("CoolerOn", false);
                return this.coolerOn;
            }
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set CoolerOn when not connected");
                if (!this.hasCooler)
                    throw new PropertyNotImplementedException("CoolerOn", true);
                this.coolerOn = value;
                if (this.canSetCcdTemperature)
                {
                    // TODO implement CCD temperature control
                }
			}
		}

		/// <summary>
		/// Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
		/// False.
		/// </summary>
		/// <exception cref=" System.Exception">not supported</exception>
		/// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
		public double CoolerPower
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read Cooler Power when not connected");
                if (!this.hasCooler)
                    throw new PropertyNotImplementedException("CoolerPower", false);
                return this.coolerPower;
            }
        }

		/// <summary>
		/// Returns a description of the camera model, such as manufacturer and model
		/// number. Any ASCII characters may be used. The string shall not exceed 68
		/// characters (for compatibility with FITS headers).
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
		public string Description
		{
			get 
            {
                String strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return (s_csDriverDescription + " - Version " + strVersion);
            }
		}

		/// <summary>
		/// Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
		/// multiple gain modes; these should be selected via the SetupDialog and thus are
		/// static during a session.)
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double ElectronsPerADU
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read ElectronsPerADU when not connected");
                return this.electronsPerADU;
            }
        }

		/// <summary>
		/// Reports the full well capacity of the camera in electrons, at the current camera
		/// settings (binning, SetupDialog settings, etc.)
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double FullWellCapacity
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read FullWellCapacity when not connected");
                return this.fullWellCapacity;
            }
		}

		/// <summary>
		/// If True, the camera has a mechanical shutter. If False, the camera does not have
		/// a shutter.  If there is no shutter, the StartExposure command will ignore the
		/// Light parameter.
		/// </summary>
		public bool HasShutter
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read HasShutter when not connected");
                return this.hasShutter;
            }
        }

		/// <summary>
		/// Returns the current heat sink temperature (called "ambient temperature" by some
		/// manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double HeatSinkTemperature
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read HeatSinkTemperature when not connected");
                if (!this.canSetCcdTemperature)
                    throw new PropertyNotImplementedException("HeatSinkTemperature", false);
                return this.heatSinkTemperature;
            }
        }

		/// <summary>
		/// Returns a safearray of int of size NumX * NumY containing the pixel values from
		/// the last exposure. The application must inspect the Safearray parameters to
		/// determine the dimensions. Note: if NumX or NumY is changed after a call to
		/// StartExposure it will have no effect on the size of this array. This is the
		/// preferred method for programs (not scripts) to download images since it requires
		/// much less memory.
		///
		/// For color or multispectral cameras, will produce an array of NumX * NumY *
		/// NumPlanes.  If the application cannot handle multispectral images, it should use
		/// just the first plane.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public object ImageArray
		{
			get 
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read ImageArray when not connected");
                if (!this.imageReady)
                    throw new ASCOM.InvalidOperationException("There is no image available");

                return this.imageArray;
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
		///
		/// For color or multispectral cameras, will produce an array of NumX * NumY *
		/// NumPlanes.  If the application cannot handle multispectral images, it should use
		/// just the first plane.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public object ImageArrayVariant
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read ImageArrayVariant when not connected");
                if (!this.imageReady)
                    throw new ASCOM.InvalidOperationException("There is no image available");
                // convert to variant
                this.imageArrayVariant = new object[imageArray.GetLength(0), imageArray.GetLength(1)];
                for (int i = 0; i < imageArray.GetLength(1); i++)
                {
                    for (int j = 0; j < imageArray.GetLength(0); j++)
                    {
                        imageArrayVariant[j,i] = imageArray[j,i];
                    }
                    
                }
                //this.imageArray.CopyTo(iav, 0);
                return imageArrayVariant;
            }
		}

		/// <summary>
		/// If True, there is an image from the camera available. If False, no image
		/// is available and attempts to use the ImageArray method will produce an
		/// exception.
		/// </summary>
		/// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
		public bool ImageReady
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read ImageReady when not connected");
                return this.imageReady;
            }
        }

		/// <summary>
		/// If True, pulse guiding is in progress. Required if the PulseGuide() method
		/// (which is non-blocking) is implemented. See the PulseGuide() method.
		/// </summary>
		/// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
		public bool IsPulseGuiding
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Reports the last error condition reported by the camera hardware or communications
		/// link.  The string may contain a text message or simply an error code.  The error
		/// value is cleared the next time any method is called.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if no error condition.</exception>
		public string LastError
		{
			get 
            {
                if (this.lastError.Length == 0)
                    throw new ASCOM.InvalidOperationException("there is no last error");
                return this.lastError;
            }
		}

		/// <summary>
		/// Reports the actual exposure duration in seconds (i.e. shutter open time).  This
		/// may differ from the exposure time requested due to shutter latency, camera timing
		/// precision, etc.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
		public double LastExposureDuration
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read LastExposureDuration when not connected");
                if (!this.imageReady)
                    throw new NotConnectedException("Can't read LastExposureDuration when no image is ready");
                return this.lastExposureDuration;
            }
        }

		/// <summary>
		/// Reports the actual exposure start in the FITS-standard
		/// CCYY-MM-DDThh:mm:ss[.sss...] format.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
		public string LastExposureStartTime
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read LastExposureStartTime when not connected");
                if (!this.imageReady)
                    throw new NotConnectedException("Can't read LastExposureStartTime when no image is ready");
                return this.lastExposureStartTime;
            }
		}

		/// <summary>
		/// Reports the maximum ADU value the camera can produce.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public int MaxADU
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read MaxADU when not connected");
                return this.maxADU;
            }
		}

		/// <summary>
		/// If AsymmetricBinning = False, returns the maximum allowed binning factor. If
		/// AsymmetricBinning = True, returns the maximum allowed binning factor for the X axis.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public short MaxBinX
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read MaxBinX when not connected");
                return this.maxBinX;
            }
        }

		/// <summary>
		/// If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
		/// returns the maximum allowed binning factor for the Y axis.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public short MaxBinY
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read MaxBinY when not connected");
                return this.maxBinY;
            }
        }

		/// <summary>
		/// Sets the subframe width. Also returns the current value.  If binning is active,
		/// value is in binned pixels.  No error check is performed when the value is set.
		/// Should default to CameraXSize.
		/// </summary>
		public int NumX
		{
			get
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't read NumX when not connected");
                return this.numX;
			}
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set NumX when not connected");
                this.numX = value;
			}
		}

		/// <summary>
		/// Sets the subframe height. Also returns the current value.  If binning is active,
		/// value is in binned pixels.  No error check is performed when the value is set.
		/// Should default to CameraYSize.
		/// </summary>
		public int NumY
		{
			get
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't read NumY when not connected");
                return this.numY;
            }
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set NumY when not connected");
                this.numY = value;
			}
		}

		/// <summary>
		/// Returns the width of the CCD chip pixels in microns, as provided by the camera
		/// driver.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double PixelSizeX
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read PixelSizeX when not connected");
                return this.pixelSizeX;
            }
        }

		/// <summary>
		/// Returns the height of the CCD chip pixels in microns, as provided by the camera
		/// driver.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double PixelSizeY
		{
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read PixelSizeY when not connected");
                return this.pixelSizeY;
            }
        }

		/// <summary>
		/// This method returns only after the move has completed.
		///
		/// symbolic Constants
		/// The (symbolic) values for GuideDirections are:
		/// Constant     Value      Description
		/// --------     -----      -----------
		/// guideNorth     0        North (+ declination/elevation)
		/// guideSouth     1        South (- declination/elevation)
		/// guideEast      2        East (+ right ascension/azimuth)
		/// guideWest      3        West (+ right ascension/azimuth)
		///
		/// Note: directions are nominal and may depend on exact mount wiring.  guideNorth
		/// must be opposite guideSouth, and guideEast must be opposite guideWest.
		/// </summary>
		/// <param name="Direction">Direction of guide command</param>
		/// <param name="Duration">Duration of guide in milliseconds</param>
		/// <exception cref=" System.Exception">PulseGuide command is unsupported</exception>
		/// <exception cref=" System.Exception">PulseGuide command is unsuccessful</exception>
		public void PulseGuide(GuideDirections Direction, int Duration)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Sets the camera cooler setpoint in degrees Celsius, and returns the current
		/// setpoint.
		/// Note:  camera hardware and/or driver should perform cooler ramping, to prevent
		/// thermal shock and potential damage to the CCD array or cooler stack.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if command not successful.</exception>
		/// <exception cref=" System.Exception">Must throw exception if CanSetCCDTemperature is False.</exception>
		public double SetCCDTemperature
		{
			get
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't read SetCCDTemperature when not connected");
                if (!this.canSetCcdTemperature)
                    throw new PropertyNotImplementedException("SetCCDTemperature", false);
                return this.setCcdTemperature;
            }
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set SetCCDTemperature when not connected");
                if (!this.canSetCcdTemperature)
                    throw new PropertyNotImplementedException("SetCCDTemperature", true);
                this.setCcdTemperature = value;
                // does this turn cooling on?
			}
		}

		/// <summary>
		/// Launches a configuration dialog box for the driver.  The call will not return
		/// until the user clicks OK or cancel manually.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
		public void SetupDialog()
		{
            if (this.connected)
                throw new NotConnectedException("Can't set the CCD properties when connected");
			SetupDialogForm F = new SetupDialogForm();
            F.InitProperties(this);
			F.ShowDialog();
            this.ReadFromProfile();
		}

		/// <summary>
		/// Starts an exposure. Use ImageReady to check when the exposure is complete.
		/// </summary>
		/// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
		/// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
		/// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
		public void StartExposure(double Duration, bool Light)
		{
            if (!this.connected)
                throw new NotConnectedException("Can't set StartExposure when not connected");

            // check the duration
            if (Duration < this.minExposure || Duration > this.maxExposure)
            {
                this.lastError="Incorrect exposure duration";
                throw new ASCOM.InvalidValueException("StartExposure Duration",
                                                     Duration.ToString(),
                                                     string.Format("{0} to {1}", this.minExposure, this.maxExposure));
            }
   
            //  binning tests
            if ((this.binX > this.MaxBinX) || (this.BinX < 1) )
            {
                this.lastError="Incorrect bin X factor";
                throw new ASCOM.InvalidValueException("StartExposure BinX",
                                                    this.binX.ToString(),
                                                    string.Format("1 to {0}",this.maxBinX));
            }
            if ((this.binY > this.MaxBinY) || (this.BinY < 1) )
            {
                this.lastError="Incorrect bin Y factor";
                throw new ASCOM.InvalidValueException("StartExposure BinY",
                                                    this.binY.ToString(),
                                                    string.Format("1 to {0}",this.maxBinY));
            }

            // check the start position is in range
            // start is in binned pixels
            if (this.startX < 0 || this.startX * this.binX > this.cameraXSize)
            {
                this.lastError="Incorrect Start X position";
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    this.startX.ToString(),
                                                    string.Format("0 to {0}",cameraXSize/this.binX));
            }
            if (this.startY < 0 || this.startY * this.binY > this.cameraYSize)
            {
                this.lastError="Incorrect Start X position";
                throw new ASCOM.InvalidValueException("StartExposure StartX",
                                                    this.startX.ToString(),
                                                    string.Format("0 to {0}",cameraXSize/this.binX));
            }

            // check that the acquisition is at least 1 pixel in size and fits in the camera area
            if (this.numX < 1 || (this.numX + this.startX ) * this.binX > this.cameraXSize)
            {
                this.lastError="Incorrect Num X value";
                throw new ASCOM.InvalidValueException("StartExposure NumX",
                                                    this.numX.ToString(),
                                                    string.Format("1 to {0}",cameraXSize/this.binX));
            }
            if (this.numY < 1 || (this.numY + this.startY ) * this.binY > this.cameraYSize)
            {
                this.lastError="Incorrect Num Y value";
                throw new ASCOM.InvalidValueException("StartExposure NumY",
                                                    this.numY.ToString(),
                                                    string.Format("1 to {0}",cameraYSize/this.binY));
            }

            this.lastExposureDuration = Duration;
            this.lastExposureStartTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            if (this.exposureTimer == null)
                this.exposureTimer = new Timer();
            this.exposureTimer.Tick+=new Timer.TickEventHandler(exposureTimer_Tick);
            this.exposureTimer.Interval = (int)(Duration * 1000);
            this.cameraState = CameraStates.cameraExposing;
            this.exposureTimer.Enabled = true;
		}

        private void  exposureTimer_Tick()
        {
            this.exposureTimer.Enabled = false;
            this.cameraState = CameraStates.cameraDownload;

            // set the image array dimensions
            this.imageArray = new int[this.numX, this.numY];
            // fill the array
            for (int i = 0; i < this.imageArray.GetLength(1); i++)
            {
                for (int j = 0; j < this.imageArray.GetLength(0); j++)
                {
                    this.imageArray[j, i] = i + j;
                }
            }
            this.imageReady = true;
            this.cameraState = CameraStates.cameraIdle;
        }

		/// <summary>
		/// Sets the subframe start position for the X axis (0 based). Also returns the
		/// current value.  If binning is active, value is in binned pixels.
		/// </summary>
		public int StartX
		{
			get
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't read StartX when not connected");
                return this.startX;
            }
			set
			{
                if (!this.connected)
                    throw new NotConnectedException("Can't set StartX when not connected");
                this.startX = value;
			}
		}

		/// <summary>
		/// Sets the subframe start position for the Y axis (0 based). Also returns the
		/// current value.  If binning is active, value is in binned pixels.
		/// </summary>
        public int StartY
        {
            get
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't read StartY when not connected");
                return this.startY;
            }
            set
            {
                if (!this.connected)
                    throw new NotConnectedException("Can't set StartY when not connected");
                this.startY = value;
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
			throw new System.Exception("The method or operation is not implemented.");
		}

		#endregion

        #region private
        private Profile profile;

        private void ReadFromProfile()
        {
            profile = new Profile(true);
            profile.DeviceType = "Camera";
            // read properties from profile
            this.pixelSizeX = Convert.ToDouble(profile.GetValue(s_csDriverID, "PixelSizeX", string.Empty, "5.6"), CultureInfo.InvariantCulture);
            this.pixelSizeY = Convert.ToDouble(profile.GetValue(s_csDriverID, "PixelSizeY", string.Empty, "5.6"), CultureInfo.InvariantCulture);
            this.fullWellCapacity = Convert.ToDouble(profile.GetValue(s_csDriverID, "FullWellCapacity", string.Empty, "30000"), CultureInfo.InvariantCulture);
            this.maxADU = Convert.ToInt32(profile.GetValue(s_csDriverID, "maxADU", string.Empty, "4096"), CultureInfo.InvariantCulture);
            this.electronsPerADU = Convert.ToDouble(profile.GetValue(s_csDriverID, "ElectronsPerADU", string.Empty, "0.8"), CultureInfo.InvariantCulture);

            this.cameraXSize = Convert.ToInt32(profile.GetValue(s_csDriverID, "CameraXSize", string.Empty, "800"), CultureInfo.InvariantCulture);
            this.cameraYSize = Convert.ToInt32(profile.GetValue(s_csDriverID, "CameraYSize", string.Empty, "600"), CultureInfo.InvariantCulture);
            this.canAsymmetricBin = Convert.ToBoolean(profile.GetValue(s_csDriverID, "CanAsymmetricBin", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.maxBinX = Convert.ToInt16(profile.GetValue(s_csDriverID, "MaxBinX", string.Empty, "4"), CultureInfo.InvariantCulture);
            this.maxBinY = Convert.ToInt16(profile.GetValue(s_csDriverID, "MaxBinY", string.Empty, "4"), CultureInfo.InvariantCulture);
            this.hasShutter = Convert.ToBoolean(profile.GetValue(s_csDriverID, "HasShutter", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.sensorName = profile.GetValue(s_csDriverID, "SensorName", string.Empty, "");
            this.sensorType = Convert.ToInt32(profile.GetValue(s_csDriverID, "SensorType", string.Empty, "0"), CultureInfo.InvariantCulture);
            this.bayerOffsetX = Convert.ToInt32(profile.GetValue(s_csDriverID, "BayerOffsetX", string.Empty, "0"), CultureInfo.InvariantCulture);
            this.bayerOffsetY = Convert.ToInt32(profile.GetValue(s_csDriverID, "BayerOffsetY", string.Empty, "0"), CultureInfo.InvariantCulture);

            this.hasCooler = Convert.ToBoolean(profile.GetValue(s_csDriverID, "HasCooler", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.canSetCcdTemperature = Convert.ToBoolean(profile.GetValue(s_csDriverID, "CanSetCcdTemperature", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.canGetCoolerPower = Convert.ToBoolean(profile.GetValue(s_csDriverID, "CanGetCoolerPower", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.coolerPower = Convert.ToDouble(profile.GetValue(s_csDriverID, "CoolerPower", string.Empty, "0"), CultureInfo.InvariantCulture);
            this.ccdTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, "CCDTemperature", string.Empty, "0"), CultureInfo.InvariantCulture);
            this.heatSinkTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, "HeatSinkTemperature", string.Empty, "20"), CultureInfo.InvariantCulture);
            this.setCcdTemperature = Convert.ToDouble(profile.GetValue(s_csDriverID, "SetCCDTemperature", string.Empty, "-20"), CultureInfo.InvariantCulture);

            this.canAbortExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, "CanAbortExposure", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.canStopExposure = Convert.ToBoolean(profile.GetValue(s_csDriverID, "CanStopExposure", string.Empty, "false"), CultureInfo.InvariantCulture);
            this.minExposure = Convert.ToDouble(profile.GetValue(s_csDriverID, "MinExposure", string.Empty, "0.001"), CultureInfo.InvariantCulture);
            this.maxExposure = Convert.ToDouble(profile.GetValue(s_csDriverID, "MaxExposure", string.Empty, "3600"), CultureInfo.InvariantCulture);
            this.exposureResolution = Convert.ToDouble(profile.GetValue(s_csDriverID, "ExposureResolution", string.Empty, "0.001"), CultureInfo.InvariantCulture);
        }

        private void InitialiseSimulator()
        {
            this.ReadFromProfile();
            this.startX = 0;
            this.startY = 0;
            this.binX = 1;
            this.binY = 1;
            this.numX = this.cameraXSize;
            this.numY = this.cameraYSize;
            
            this.cameraState= CameraStates.cameraIdle;
            this.coolerOn = false;
        }

        #endregion
    }
}
