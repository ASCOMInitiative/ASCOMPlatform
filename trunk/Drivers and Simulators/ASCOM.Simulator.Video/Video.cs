//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This file implements the IVideo COM interface for the Video Simulator
//              and is simply a wrapper to the VideoCamera and other main classes
//
// Implements:	ASCOM Video interface version: 1
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video;
using ASCOM.Simulator.Properties;
using ASCOM.Simulator.Utils;
using Microsoft.Win32;
using Simulator.VideoCameraImpl;

namespace ASCOM.Simulator
{
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IVideo))]
	[Guid("1D06B419-B330-42CA-BC41-992718C929F4")]
	[ProgId("ASCOM.Simulator.Video")]
	public class Video : IVideo
	{
		/// <summary>
		/// Category under which the device will be listed by the ASCOM Chooser
		/// </summary>
		private static string DRIVER_DEVICE_TYPE = "Video";

		/// <summary>
		/// ASCOM DeviceID (COM ProgID) for this driver.
		/// The DeviceID is used by ASCOM applications to load the driver at runtime.
		/// </summary>
		private static string DRIVER_ID = "ASCOM.Simulator.Video";

		/// <summary>
		/// Driver description that displays in the ASCOM Chooser.
		/// </summary>
		private static string DRIVER_DESCRIPTION = "Video Simulator";

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		/// <summary>
		/// Register or unregister the driver with the ASCOM Platform.
		/// This is harmless if the driver is already registered/unregistered.
		/// </summary>
		/// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
		private static void RegUnregASCOM(bool bRegister)
		{
			using (var P = new ASCOM.Utilities.Profile())
			{
				P.DeviceType = DRIVER_DEVICE_TYPE;
				if (bRegister)
				{
					P.Register(DRIVER_ID, DRIVER_DESCRIPTION);
				}
				else
				{
					P.Unregister(DRIVER_ID);
				}
			}
		}

		/// <summary>
		/// This function registers the driver with the ASCOM Chooser and
		/// is called automatically whenever this class is registered for COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is successfully built.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During setup, when the installer registers the assembly for COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually register a driver with ASCOM.
		/// </remarks>
		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
		{
			RegUnregASCOM(true);
		}

		/// <summary>
		/// This function unregisters the driver from the ASCOM Chooser and
		/// is called automatically whenever this class is unregistered from COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is cleaned or prior to rebuilding.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
		/// </remarks>
		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
		{
			RegUnregASCOM(false);
		}
		#endregion

		private VideoCamera camera;
        private AviTools aviTools;

		public Video()
		{
            aviTools = new AviTools();
			Properties.Settings.Default.Reload();

			camera = new VideoCamera();
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public bool Connected
		{
			get { return camera.IsConnected; }
			set
			{
				if (value != camera.IsConnected)
				{
					if (value)
					{
						camera.EnsureConnected();

						if (camera.SupportedIntegrationRates.Count > 0)
							camera.ChangeIntegrationRate(camera.SupportedIntegrationRates[0]);

						if (camera.SupportedGammas.Count > 0)
						{
							string offGamma = camera.SupportedGammas.FirstOrDefault(x => x.Contains("OFF"));
							if (!string.IsNullOrEmpty(offGamma))
								camera.SetGamma(camera.SupportedGammas.IndexOf(offGamma));
							else
							{
								offGamma = camera.SupportedGammas.FirstOrDefault(x => x == "1" || x == "1.0" || x == "1.00" || x == "1.000");
								if (!string.IsNullOrEmpty(offGamma))
									camera.SetGamma(camera.SupportedGammas.IndexOf(offGamma));
								else
									camera.SetGamma(0);
							}							
						}							
					}
					else
						camera.EnsureDisconnected();
				}
			}
		}

		/// <exception cref="T:ASCOM.NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public string Description
		{
			get { return DRIVER_DESCRIPTION; }
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public string DriverInfo
		{
			get
			{
				return string.Format(
					@"Video Simulator Driver ver {0}", 
					Assembly.GetExecutingAssembly().GetName().Version);
			}
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public string DriverVersion
		{
			get
			{
				Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
			}
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public short InterfaceVersion
		{
			get { return 1; }
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public string Name
		{
			get { return DRIVER_DESCRIPTION; }
		}

		public string VideoCaptureDeviceName
		{
			get { return camera.VideoCaptureDeviceName; }
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public void SetupDialog()
		{
			using (var setupDlg = new frmSetupDialog())
			{
				Form ownerForm = Application.OpenForms
					.Cast<Form>()
					.FirstOrDefault(x => x != null && x.GetType().FullName == "ASCOM.Utilities.ChooserForm");

				if (ownerForm == null)
					ownerForm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null && x.Owner == null);

				setupDlg.StartPosition = FormStartPosition.CenterParent;
				
				if (setupDlg.ShowDialog(ownerForm) == DialogResult.OK)
				{
					Properties.Settings.Default.Save();

					camera.ReloadSimulatorSettings();
					return;
				}
				Properties.Settings.Default.Reload();
			}
		}

		private void AssertConnected()
		{
			if (!camera.IsConnected)
				throw new ASCOM.NotConnectedException();			
		}

		/// <exception cref="T:ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
		/// <exception cref="T:ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
		/// of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
		/// perform an action that it does not support.</exception>
		/// <exception cref="T:ASCOM.NotConnectedException">If the driver is not connected.</exception>
		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public string Action(string ActionName, string ActionParameters)
		{
			if (!camera.IsActionSupported(ActionName))
				throw new ASCOM.ActionNotImplementedException();

			AssertConnected();

			try
			{
				return camera.PerformAction(ActionName, ActionParameters);	
			}
			catch(Exception ex)
			{
				throw new ASCOM.DriverException(string.Format("Error executing action '{0}' with parameters '{1}'.", ActionName, ActionParameters), ex);
			}
			
		}

		///	<exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public System.Collections.ArrayList SupportedActions
		{
			get
			{
				try
				{
					var rv = new ArrayList();
					camera.SupportedActions.ForEach(x => rv.Add(x));

					return rv;
				}
				catch (Exception ex)
				{
					throw new ASCOM.DriverException("Cannot get supported actions list.", ex);
				}
			}
		}

		public void Dispose()
		{
			if (camera != null && camera.IsConnected)
				camera.EnsureDisconnected();

			camera = null;
		}

		public double ExposureMax
		{
			get { return camera.ExposureMax; }
		}

		public double ExposureMin
		{
			get { return camera.ExposureMin; }
		}

		public VideoCameraFrameRate FrameRate
		{
			get { return camera.FrameRate; }
		}

		/// <exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
		/// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw exception if camera supports only one integration rate (exposure) that cannot be changed.</exception>		
		public System.Collections.ArrayList SupportedIntegrationRates
		{
			get
			{
				AssertConnected();

				try
				{
					var rv = new ArrayList();
					camera.SupportedIntegrationRates.ForEach(x => rv.Add(double.Parse(x)));

					return rv;
				}
				catch (Exception ex)
				{
					throw new ASCOM.DriverException("Cannot get supported exposures list.", ex);
				}

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
				AssertConnected();

				if (Settings.Default.CameraType == SiumulatedCameraType.AnalogueNonIntegrating)
					throw new PropertyNotImplementedException("The camera doesn't support integration rates.");

				if (camera.SupportedIntegrationRates.Count == 1)
					return 0;

				string integrationRate = camera.IntegrationRate;
				int integrationRateIndex = camera.SupportedIntegrationRates.IndexOf(integrationRate);
				return integrationRateIndex;
			}

			set
			{
				AssertConnected();

				if (value < 0 || value >= camera.SupportedIntegrationRates.Count - 1)
					throw new ASCOM.DriverException("Integration rate must correspond to the index of an element from the SupportedIntegrationRates.");

				try
				{
					camera.ChangeIntegrationRate(camera.SupportedIntegrationRates[value]);
				}
				catch (Exception ex)
				{
					throw new ASCOM.DriverException("Cannot change exposure.", ex);
				}				
			}			
		}

		/// <exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
		/// <exception cref="T:ASCOM.InvalidOperationException">If called before any video frame has been taken</exception>	
		public IVideoFrame LastVideoFrame
		{
			get
			{
				AssertConnected();

				VideoCameraFrame cameraFrame;

				if (camera.GetCurrentFrame(out cameraFrame))
				{					
					//return VideoFrame.FakeFrame(camera.ImageWidth, camera.ImageHeight);
					VideoFrame rv = VideoFrame.CreateFrame(camera.ImageWidth, camera.ImageHeight, cameraFrame);
					return rv;					
				}
				else
					throw new ASCOM.InvalidOperationException("No video frames are available.");
			}
		}

		public IVideoFrame LastVideoFrameImageArrayVariant
		{
			get
			{
				AssertConnected();

				VideoCameraFrame cameraFrame;

				if (camera.GetCurrentFrame(out cameraFrame))
				{
					VideoFrame rv = VideoFrame.CreateFrameVariant(camera.ImageWidth, camera.ImageHeight, cameraFrame);
					return rv;
				}
				else
					throw new ASCOM.InvalidOperationException("No video frames are available.");				
			}
		}


		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		public string SensorName
		{
			get
			{
				AssertConnected();

				return camera.SensorName;
			}
		}

		public SensorType SensorType
		{
			get { return camera.SensorType; }
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
		public int CameraXSize
		{
			get
			{
				AssertConnected();

				return camera.ImageWidth;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
		public int CameraYSize
		{
			get
			{
				AssertConnected();

				return camera.ImageHeight;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
		public double PixelSizeX
		{
			get
			{
				AssertConnected();

				return Settings.Default.PixelSizeX;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
		public int Width
		{
			get
			{
				AssertConnected();

				return camera.ImageWidth;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
		public int Height
		{
			get
			{
				AssertConnected();

				return camera.ImageHeight;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
		public double PixelSizeY
		{
			get
			{
				AssertConnected();

				return Settings.Default.PixelSizeY;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
		public int BitDepth
		{
			get
			{
				AssertConnected();

				return camera.BitDepth;
			}
		}

		public string VideoCodec
		{
			get
			{
                return aviTools.GetUsedAviFourCC();
			}
		}

		public string VideoFileFormat
		{
			get { return Settings.Default.VideoFileFormat; }
		}

		public int VideoFramesBufferSize
		{
			get
			{
				return Settings.Default.UseBuffering
				       	? Settings.Default.BufferSize
				       	: 1;
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if not connected.</exception>
		///	<exception cref="T:ASCOM.InvalidOperationException">Must throw exception if the current camera state doesn't allow to begin recording a file.</exception>
		///	<exception cref="T:ASCOM.DriverException">Must throw exception if there is any other problem as a result of which the recording cannot begin.</exception>
		public string StartRecordingVideoFile(string PreferredFileName)
		{
			AssertConnected();

			try
			{
				VideoCameraState currentState = camera.GetCurrentCameraState();

				if (currentState == VideoCameraState.videoCameraRecording)
					throw new InvalidOperationException("The camera is already recording.");
				else if (currentState != VideoCameraState.videoCameraRunning)
					throw new InvalidOperationException("The current state of the video camera doesn't allow a recording operation to begin right now.");

				if (Settings.Default.UseBuffering)
					throw new DriverException("The IVideo Simulator only supports recording in a non-buffering mode.");

				string directory = Path.GetDirectoryName(PreferredFileName);

				if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

				if (File.Exists(PreferredFileName))
					throw new DriverException(string.Format("File '{0}' already exists. Video can be recorded only in a non existing file.", PreferredFileName));

				return camera.StartRecordingVideoFile(PreferredFileName);
			}
			catch(Exception ex)
			{
				throw new DriverException("Error starting the recording. " + ex.Message, ex);		
			}
		}


		///	<exception cref="T:ASCOM.NotConnectedException">Must throw exception if not connected.</exception>
		///	<exception cref="T:ASCOM.InvalidOperationException">Must throw exception if the current camera state doesn't allow to stop recording the file or no file is currently being recorded.</exception>
		///	<exception cref="T:ASCOM.DriverException">Must throw exception if there is any other problem as result of which the recording cannot stop.</exception>
		public void StopRecordingVideoFile()
		{
			AssertConnected();

			try
			{
				VideoCameraState currentState = camera.GetCurrentCameraState();

				if (currentState != VideoCameraState.videoCameraRecording)
					throw new InvalidOperationException("The camera is currently not recording.");

				camera.StopRecordingVideoFile();

			}
			catch(Exception ex)
			{
				throw new DriverException("Error stopping the recording. " + ex.Message, ex);
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must return an exception if the camera status is unavailable.</exception>
		public VideoCameraState CameraState
		{
			get
			{
				AssertConnected();

				return camera.GetCurrentCameraState();
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
		public short GainMax
		{
			get
			{
				if (Settings.Default.SupportsGainRange)
					return Settings.Default.GainMax;
				else
					throw new PropertyNotImplementedException();
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
		public short GainMin
		{
			get
			{
				if (Settings.Default.SupportsGainRange)
					return Settings.Default.GainMin;
				else
					throw new PropertyNotImplementedException();
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
		public short Gain
		{
			get
			{
				AssertConnected();

				return camera.GetCurrentGain();
			}
			set
			{
				AssertConnected();

				if (Settings.Default.SupportsGainRange)
				{
					if (value < Settings.Default.GainMin || value > Settings.Default.GainMax)
						throw new InvalidValueException(string.Format("Value {0} is outside of the supported gain range {1} - {2}", value, Settings.Default.GainMin, Settings.Default.GainMax));

					camera.SetGain(value);
				}
				else
				{
					if (value < 0 || value >= camera.SupportedGains.Count)
						throw new InvalidValueException(string.Format("Value {0} is outside of the supported gain range {1} - {2}", value, 0, camera.SupportedGains.Count - 1));

					camera.SetDiscreteGain(camera.SupportedGains[value]);
				}
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if Gains is not supported</exception>
		public System.Collections.ArrayList Gains
		{
			get
			{
				AssertConnected();

				try
				{
					var rv = new ArrayList();
					camera.SupportedGains.ForEach(x => rv.Add(x));

					return rv;
				}
				catch (Exception ex)
				{
					throw new ASCOM.DriverException("Cannot get supported gains list.", ex);
				}
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
		public short GammaMax
		{
			get
			{
				if (Settings.Default.SupportsGammaRange)
					return Settings.Default.GammaMax;
				else
					throw new PropertyNotImplementedException();
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
		public short GammaMin
		{
			get
			{
				if (Settings.Default.SupportsGammaRange)
					return Settings.Default.GammaMin;
				else
					throw new PropertyNotImplementedException();
			}
		}


		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.InvalidValueException">Must throw an exception if not valid.</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gamma is not supported</exception>
		public int Gamma
		{
			get
			{
				AssertConnected();

				return camera.GetCurrentGamma();
			}
			set
			{
				AssertConnected();

				if (Settings.Default.SupportsGammaRange)
				{
					if (value < Settings.Default.GammaMin || value > Settings.Default.GammaMax)
						throw new InvalidValueException(string.Format("Value {0} is outside of the supported gamma range {1} - {2}", value, Settings.Default.GammaMin, Settings.Default.GammaMax));

					camera.SetGamma(value);
				}
				else
				{
					if (value < 0 || value >= camera.SupportedGammas.Count)
						throw new InvalidValueException(string.Format("Value {0} is outside of the supported gamma range {1} - {2}", value, 0, camera.SupportedGammas.Count - 1));

					camera.SetDiscreteGamma(camera.SupportedGammas[value]);
				}
			}
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
		///	active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
		public System.Collections.ArrayList Gammas
		{
			get
			{
				AssertConnected();

				try
				{
					var rv = new ArrayList();
					camera.SupportedGammas.ForEach(x => rv.Add(x));

					return rv;
				}
				catch (Exception ex)
				{
					throw new ASCOM.DriverException("Cannot get supported gammas list.", ex);
				}
			}
		}

		public bool CanConfigureDeviceProperties
		{
			get { return true; }
		}

		private frmImageSettings frmImageSettings = null;

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the camera is not connected.</exception>
		///	<exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if ConfigureImage is not supported.</exception>
		public void ConfigureDeviceProperties()
		{
			AssertConnected();

			if (frmImageSettings != null)
			{
				try
				{
					frmImageSettings.Show();
				}
				catch (Exception)
				{
					try
					{
						frmImageSettings.Close();
							
					}
					catch
					{ }
					finally
					{
						frmImageSettings = null;
					}
				}
			}

			if (frmImageSettings == null)
			{
				frmImageSettings = new frmImageSettings();
				frmImageSettings.Camera = camera;


				Form ownerForm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null && x.Owner == null);

				frmImageSettings.StartPosition = FormStartPosition.CenterParent;

				frmImageSettings.Show(ownerForm);
			}
			else
				frmImageSettings.Show();

		}
	}
}
