using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ASCOM.Interface;

/// -----------------------------------------------------------------------------------
/// TODO: The members in this class currently ONLY invoke via COM Interop.
/// While this will work, it results in 2 unnecessary round-trips through COM Interop
/// when a .NET driver is being used. This seems perilous and differs from the pattern
/// laid down in V1. However, this will probably be enough for proof-of-concept
/// and we can argue whether it is sufficient later.
/// -----------------------------------------------------------------------------------

namespace ASCOM.DriverAccess
{
	/// <summary>
	/// Camera V2 adds the additional members negotiated by the camera vendors,
	/// plus IAscomDriver and IDeviceControl (which are intrinsic to ICameraV2).
	/// </summary>
	class CameraV2 : Camera, ASCOM.Interface.ICameraV2
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CameraV2"/> class.
		/// </summary>
		/// <param name="cameraID">The COM ProgID of the underlying driver.</param>
		public CameraV2(string cameraID) : base(cameraID) { }

		#region Camera V2 Properties
		// the Version 2 properties are late bound only for now,
		// so only the late bound interface is implemented

		/// <summary>
		/// Returns the X offset of the Bayer matrix, as defined in Camera.SensorType.
		/// Value returned must be in the range 0 to M-1, where M is the width of the
		/// Bayer matrix. The offset is relative to the 0,0 pixel in the sensor array,
		/// and does not change to reflect subframe settings.
		/// </summary>
		/// <value>The bayer offset X.</value>
		/// <exception cref=" System.Exception">Must throw an exception if the information is not available.</exception>
		/// <exception cref=" System.Exception">Must throw an exception if not valid. </exception>
		public short BayerOffsetX
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("BayerOffsetX",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Returns the Y offset of the Bayer matrix, as defined in Camera.SensorType.
		/// Value returned must be in the range 0 to M-1, where M is the height of the
		/// Bayer matrix. The offset is relative to the 0,0 pixel in the sensor array,
		/// and does not change to reflect subframe settings.
		/// </summary>
		/// <value>The bayer offset Y.</value>
		/// <exception cref=" System.Exception">Must throw an exception if the information is not available.</exception>
		/// <exception cref=" System.Exception">Must throw an exception if not valid. </exception>
		public short BayerOffsetY
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("BayerOffsetY",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// If True, the Camera.FastReadout function is available.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance can do fast readout; otherwise, <c>false</c>.
		/// </value>
		public bool CanFastReadout
		{
			get
			{
				return Convert.ToBoolean(objTypeCamera.InvokeMember("CanFastReadout",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Descriptive and version information about this ASCOM Camera driver.
		/// This string may contain line endings and may be hundreds to thousands of characters long.
		/// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
		/// See the Description property for descriptive info on the camera itself.
		/// To get the driver version in a parseable string, use the DriverVersion property.
		/// </summary>
		/// <value>The driver info.</value>
		public string DriverInfo
		{
			get
			{
				//if (iCamera != null)
				//    return iCamera.DriverInfo;
				//else
				return (string)objTypeCamera.InvokeMember("DriverInfo",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { });
			}
		}

		/// <summary>
		/// Returns the maximum exposure time in seconds supported by Camera.StartExposure.
		/// </summary>
		/// <value>The maximum exposure in seconds.</value>
		public double ExposureMax
		{
			get
			{
				return Convert.ToDouble(objTypeCamera.InvokeMember("ExposureMax",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Returns the minimum exposure time in seconds supported by Camera.StartExposure.
		/// </summary>
		/// <value>The minimum exposure in seconds.</value>
		public double ExposureMin
		{
			get
			{
				return Convert.ToDouble(objTypeCamera.InvokeMember("ExposureMin",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Returns the smallest increment of exposure time in seconds supported by Camera.StartExposure
		/// </summary>
		/// <value>The exposure resolution in seconds</value>
		public double ExposureResolution
		{
			get
			{
				return Convert.ToDouble(objTypeCamera.InvokeMember("ExposureResolution",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Many cameras have a "fast mode" intended for use in focusing.
		/// When set to True, the camera will operate in Fast mode;
		/// when set False, the camera will operate normally.
		/// This property should default to False.
		/// </summary>
		/// <value><c>true</c> if fast readout is possible; otherwise, <c>false</c>.</value>
		public bool FastReadout
		{
			get
			{
				return Convert.ToBoolean(objTypeCamera.InvokeMember("FastReadout",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
			set
			{
				objTypeCamera.InvokeMember("FastReadout",
					BindingFlags.Default | BindingFlags.SetProperty,
					null, objCameraLateBound, new object[] { value });
			}
		}

		/// <summary>
		/// Set or get the camera gain. See the documentation for details.
		/// If <see cref="Gains"/> contains valid values then this provides an index into the array of <see cref="Gains"/> strings,
		/// otherwise and integer between <see cref="GainMin"/>  and <see cref="GainMax"/>
		/// </summary>
		/// <value>The gain.</value>
		public short Gain
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("Gain",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
			set
			{
				objTypeCamera.InvokeMember("Gain",
					BindingFlags.Default | BindingFlags.SetProperty,
					null, objCameraLateBound, new object[] { value });
			}
		}

		/// <summary>
		/// When specifying the gain setting with an integer value, this
		/// is used in conjunction with <see cref="GainMin"/> to specify the range of valid settings.
		/// if <see cref="Gains"/> contains valid data this must not be used
		/// </summary>
		/// <value>The maximum value of gain</value>
		public short GainMax
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("GainMax",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// When specifying the gain setting with an integer value, this
		/// is used in conjunction with <see cref="GainMax"/> to specify the range of valid settings.
		/// if <see cref="Gains"/> contains valid data this must not be used
		/// </summary>
		/// <value>The minimum value of gain</value>
		public short GainMin
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("GainMin",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// Gains provides a 0-based array of available gain settings.
		/// This is often used to specify ISO settings for DSLR cameras.
		/// Typically the application software will display the available
		/// gain settings in a drop list. The application will then supply
		/// the selected index to the driver via the <see cref="Gain"/> property.
		/// If this is valid <see cref="GainMin"/> and <see cref="GainMax"/> are not.
		/// </summary>
		public string[] Gains
		{
			get
			{
				return (string[])objTypeCamera.InvokeMember("Gains",
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
		/// <value>The interface version.</value>
		public short InterfaceVersion
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("InterfaceVersion",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// The short name of the camera, for display purposes
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				//if (iCamera != null)
				//    return iCamera.Name;
				//else
				return (string)objTypeCamera.InvokeMember("Name",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { });
			}
		}

		/// <summary>
		/// Returns an integer between 0 and 100, where 0 indicates 0%
		/// progress (function just started) and 100 indicates 100% progress
		/// (i.e. completion).
		/// </summary>
		/// <value>The percent completed.</value>
		public short PercentCompleted
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("PercentCompleted",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// ReadoutMode is an index into the array <see cref="ReadoutModes"/>., and selects
		/// the desired readout mode for the camera.  Defaults to 0 if not set.
		/// Throws an exception if the selected mode is not available.
		/// </summary>
		/// <value>The readout mode.</value>
		public short ReadoutMode
		{
			get
			{
				return Convert.ToInt16(objTypeCamera.InvokeMember("ReadoutMode",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { }));
			}
			set
			{
				objTypeCamera.InvokeMember("ReadoutMode",
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objCameraLateBound, new object[] { value });
			}
		}

		/// <summary>
		/// Return an array of strings, each of which describes an available
		/// readout mode of the camera.
		/// use <see cref="ReadoutMode"/> to set or get the current mode.
		/// </summary>
		/// <value>The readout modes.</value>
		public string[] ReadoutModes
		{
			get
			{
				return (string[])objTypeCamera.InvokeMember("ReadoutModes",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { });
			}
		}

		/// <summary>
		/// Returns the name (datasheet part number) of the sensor, e.g. ICX285AL.
		/// </summary>
		/// <value>The name of the sensor.</value>
		public string SensorName
		{
			get
			{
				return Convert.ToString(objTypeCamera.InvokeMember("SensorName",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { }));
			}
		}

		/// <summary>
		/// SensorType returns a value indicating whether the sensor is monochrome,
		/// or what Bayer matrix it encodes.
		/// <list type="bullet">
		/// 		<listheader>
		/// 			<description>Value  SensorType      Meaning</description>
		/// 		</listheader>
		/// 		<item>
		/// 			<description>0      Monochrome      Camera produces monochrome array with no Bayer encoding</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>1      Color           Camera produces color image directly,
		///													requiring no Bayer decoding</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>2      RGGB            Camera produces RGGB encoded Bayer array images</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>3      CMYG            Camera produces CMYG encoded Bayer array images</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>4      CMYG2           Camera produces CMYG2 encoded Bayer array images</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>5      LRGB            Camera produces Kodak TRUESENSE Bayer LRGB array image</description>
		/// 		</item>
		/// 		<item>
		/// 			<description>5      CameraError     Camera error condition serious enough to prevent
		/// further operations (link fail, etc.).</description>
		/// 		</item>
		/// 	</list>
		/// </summary>
		/// <value>The type of the sensor.</value>
		public  SensorType SensorType
		{
			get
			{
				return (SensorType) objTypeCamera.InvokeMember("SensorType",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objCameraLateBound, new object[] { });
			}
			set
			{
				objTypeCamera.InvokeMember("SensorType",
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objCameraLateBound, new object[] { value });
			}
		}

		private int? interfaceVersion;

		private int CheckInterfaceVersion()
		{
			if (this.interfaceVersion != null)
				return (int)interfaceVersion;
			try
			{
				this.interfaceVersion = Convert.ToInt16(objTypeCamera.InvokeMember("InterfaceVersion",
					BindingFlags.Default | BindingFlags.GetProperty,
					null, objCameraLateBound, new object[] { }));
			}
			catch (System.MissingFieldException)
			{
				this.interfaceVersion = 1;
			}
			return (int)this.interfaceVersion;
		}
		#endregion


		#region IAscomDriver Members

		public string DriverVersion
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IDeviceControl Members

		public string Action(string ActionName, string ActionParameters)
		{
			throw new NotImplementedException();
		}

		public string LastResult
		{
			get { throw new NotImplementedException(); }
		}

		public string[] SupportedActions
		{
			get { throw new NotImplementedException(); }
		}

		public void CommandBlind(string Command, bool Raw)
		{
			throw new NotImplementedException();
		}

		public bool CommandBool(string Command, bool Raw)
		{
			throw new NotImplementedException();
		}

		public string CommandString(string Command, bool Raw)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
