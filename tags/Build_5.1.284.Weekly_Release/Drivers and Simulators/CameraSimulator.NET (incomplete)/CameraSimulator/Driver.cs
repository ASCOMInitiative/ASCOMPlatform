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
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using Helper = ASCOM.HelperNET;

namespace ASCOM.Simulator
{
	//
	// Your driver's ID is ASCOM.Simulator.Camera
	//
	// The Guid attribute sets the CLSID for ASCOM.Simulator.Camera
	// The ClassInterface/None attribute prevents an empty interface called
	// _Camera from being created and used as the [default] interface
	//
	[Guid("12229c31-e7d6-49e8-9c5d-5d7ff05c3bfe")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Camera : ICamera
	{
		#region Camera Constructor
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.Simulator.Camera";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "Camera simulator";

		//
		// Constructor - Must be public for COM registration!
		//
		public Camera()
		{
			// TODO Implement your additional construction here
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

		short m_iBin = 1;

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
				return m_iBin;
			}
			set
			{
				m_iBin = value;
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
		}

		/// <summary>
		/// Returns the current CCD temperature in degrees Celsius. Only valid if
		/// CanControlTemperature is True.
		/// </summary>
		/// <exception>Must throw exception if data unavailable.</exception>
		public double CCDTemperature
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns the width of the CCD camera chip in unbinned pixels.
		/// </summary>
		/// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
		public int CameraXSize
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns the height of the CCD camera chip in unbinned pixels.
		/// </summary>
		/// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
		public int CameraYSize
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns True if the camera can abort exposures; False if not.
		/// </summary>
		public bool CanAbortExposure
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If True, the camera's cooler power setting can be read.
		/// </summary>
		public bool CanGetCoolerPower
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns True if the camera can send autoguider pulses to the telescope mount;
		/// False if not.  (Note: this does not provide any indication of whether the
		/// autoguider cable is actually connected.)
		/// </summary>
		public bool CanPulseGuide
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If True, the camera's cooler setpoint can be adjusted. If False, the camera
		/// either uses open-loop cooling or does not have the ability to adjust temperature
		/// from software, and setting the TemperatureSetpoint property has no effect.
		/// </summary>
		public bool CanSetCCDTemperature
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
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
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns a description of the camera model, such as manufacturer and model
		/// number. Any ASCII characters may be used. The string shall not exceed 68
		/// characters (for compatibility with FITS headers).
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
		public string Description
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
		/// multiple gain modes; these should be selected via the SetupDialog and thus are
		/// static during a session.)
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double ElectronsPerADU
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Reports the full well capacity of the camera in electrons, at the current camera
		/// settings (binning, SetupDialog settings, etc.)
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double FullWellCapacity
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If True, the camera has a mechanical shutter. If False, the camera does not have
		/// a shutter.  If there is no shutter, the StartExposure command will ignore the
		/// Light parameter.
		/// </summary>
		public bool HasShutter
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns the current heat sink temperature (called "ambient temperature" by some
		/// manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double HeatSinkTemperature
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If True, there is an image from the camera available. If False, no image
		/// is available and attempts to use the ImageArray method will produce an
		/// exception.
		/// </summary>
		/// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
		public bool ImageReady
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Reports the actual exposure duration in seconds (i.e. shutter open time).  This
		/// may differ from the exposure time requested due to shutter latency, camera timing
		/// precision, etc.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
		public double LastExposureDuration
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Reports the actual exposure start in the FITS-standard
		/// CCYY-MM-DDThh:mm:ss[.sss...] format.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
		public string LastExposureStartTime
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Reports the maximum ADU value the camera can produce.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public int MaxADU
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If AsymmetricBinning = False, returns the maximum allowed binning factor. If
		/// AsymmetricBinning = True, returns the maximum allowed binning factor for the X axis.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public short MaxBinX
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
		/// returns the maximum allowed binning factor for the Y axis.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public short MaxBinY
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
		}

		/// <summary>
		/// Returns the width of the CCD chip pixels in microns, as provided by the camera
		/// driver.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double PixelSizeX
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		/// <summary>
		/// Returns the height of the CCD chip pixels in microns, as provided by the camera
		/// driver.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
		public double PixelSizeY
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
		}

		/// <summary>
		/// Launches a configuration dialog box for the driver.  The call will not return
		/// until the user clicks OK or cancel manually.
		/// </summary>
		/// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
		}

		/// <summary>
		/// Starts an exposure. Use ImageReady to check when the exposure is complete.
		/// </summary>
		/// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
		/// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
		/// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
		public void StartExposure(double Duration, bool Light)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Sets the subframe start position for the X axis (0 based). Also returns the
		/// current value.  If binning is active, value is in binned pixels.
		/// </summary>
		public int StartX
		{
			get
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
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
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
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
	}
}
