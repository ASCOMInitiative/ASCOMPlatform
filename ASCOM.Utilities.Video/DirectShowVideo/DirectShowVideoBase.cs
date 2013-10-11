//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow Capture
//
// Description:	This file implements the IVideo COM interface for the Video Capture Driver
//
// Implements:	ASCOM Video interface version: 1
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-Mar-2013	HDP	6.0.0	Initial commit
// 21-Mar-2013	HDP	6.0.0.	Implemented monochrome and colour grabbing
// 22-Mar-2013	HDP	6.0.0	Added support for XviD and Huffyuv codecs
// 19-Sep-2013  HDP 6.1.0   Renamed ConfigureImage to ConfigureDeviceProperties and CanConfigureImage to CanConfigureDeviceProperties
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video.DirectShowVideo;
using ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl;
using Microsoft.Win32;

namespace ASCOM.Utilities.Video.DirectShowVideo
{
	public abstract class DirectShowVideoBase
	{
		private VideoCaptureImpl.VideoCapture camera;
		private DirectShowVideoSettings settings;

		public DirectShowVideoBase()
		{
			camera = new VideoCaptureImpl.VideoCapture();
		}

		public void Initialize(DirectShowVideoSettings settings)
		{
			this.settings = settings;

			camera.Initialize(settings);
		}

		public virtual bool Connected
		{
			get { return camera.IsConnected; }
			set
			{
				if (value != camera.IsConnected)
				{
					if (value)
					{
						if (camera.LocateCaptureDevice())
							camera.EnsureConnected();						
					}
					else
						camera.EnsureDisconnected();
				}
			}
		}

		public virtual string VideoCaptureDeviceName
		{
			get
			{
				return camera.DeviceName;
			}
		}

		/// <exception cref="T:ASCOM.DriverException">Must throw an exception if the call was not successful</exception>
		public void SetupDialog()
		{
			string version = "1.0";
			try
			{
				version = (this as IVideo).DriverVersion;
			}
			catch
			{ }
			
			using (var setupDlg = new frmSetupDialog(settings, version))
			{
				Form ownerForm = Application.OpenForms
					.Cast<Form>()
					.FirstOrDefault(x => x != null && x.GetType().FullName == "ASCOM.Utilities.ChooserForm");

				if (ownerForm == null)
					ownerForm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null && x.Owner == null);

				setupDlg.StartPosition = FormStartPosition.CenterParent;

				if (setupDlg.ShowDialog(ownerForm) == DialogResult.OK)
				{
					settings.Save();

					camera.ReloadSettings();

					return;
				}
				settings.Reload();
			}
		}

		private void AssertConnected()
		{
			if (!camera.IsConnected)
			    throw new ASCOM.NotConnectedException();
		}

		public void Dispose()
		{
			if (camera != null && camera.IsConnected)
			    camera.EnsureDisconnected();

			camera = null;
		}

		private double GetCameraExposureFromFrameRate()
		{
			return 1000.0 / camera.FrameRate;
		}

		public double ExposureMax
		{
			get { return GetCameraExposureFromFrameRate(); }
		}

		public double ExposureMin
		{
			get { return GetCameraExposureFromFrameRate(); }
		}

		public VideoCameraFrameRate FrameRate
		{
			get
			{
				if (Math.Abs(camera.FrameRate - 29.97) < 0.5)
					return VideoCameraFrameRate.NTSC;
				else if (Math.Abs(camera.FrameRate - 25) < 0.5)
					return VideoCameraFrameRate.PAL;
				else
					return VideoCameraFrameRate.Variable;
			}
		}

		public virtual IVideoFrame LastVideoFrame
		{
			get
			{
				AssertConnected();

				VideoCameraFrame cameraFrame;

				if (camera.GetCurrentFrame(out cameraFrame))
				{
					VideoFrame rv = VideoFrame.CreateFrame(camera.ImageWidth, camera.ImageHeight, cameraFrame);
					return rv;
				}
				else
					throw new ASCOM.InvalidOperationException("No video frames are available.");
			}
		}

		public SensorType SensorType
		{
			get
			{
				AssertConnected();

				return camera.SimulatedSensorType;
			}
		}

		public virtual int Width
		{
			get
			{
				AssertConnected();

				return camera.ImageWidth;
			}
		}

		public virtual int Height
		{
			get
			{
				AssertConnected();

				return camera.ImageHeight;
			}
		}

		public virtual int BitDepth
		{
			get
			{
				AssertConnected();

				return camera.BitDepth;
			}
		}

		public virtual string VideoCodec
		{
			get
			{
				return camera.GetUsedAviFourCC();
			}
		}

		public virtual string VideoFileFormat
		{
			get { return "AVI"; }
		}

		public int VideoFramesBufferSize
		{
			get
			{
				return 1;
			}
		}

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

				string directory = Path.GetDirectoryName(PreferredFileName);
				string fileName = Path.GetFileName(PreferredFileName);

				if (!Directory.Exists(directory))
					Directory.CreateDirectory(fileName);

				if (File.Exists(PreferredFileName))
					throw new DriverException(string.Format("File '{0}' already exists. Video can be recorded only in a non existing file.", PreferredFileName));

				return camera.StartRecordingVideoFile(PreferredFileName);
			}
			catch (Exception ex)
			{
				throw new DriverException("Error starting the recording: " + ex.Message, ex);
			}
		}

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
			catch (Exception ex)
			{
				throw new DriverException("Error stopping the recording: " + ex.Message, ex);
			}
		}

		public VideoCameraState CameraState
		{
			get
			{
				AssertConnected();

				return camera.GetCurrentCameraState();
			}
		}

		public virtual bool CanConfigureDeviceProperties
		{
			get { return true; }
		}

		///	<exception cref="T:ASCOM.NotConnectedException">Must throw an exception if the camera is not connected.</exception>
		///	<exception cref="T:ASCOM.MethodNotImplementedException">Must throw an exception if ConfigureImage is not supported.</exception>
		[DebuggerStepThrough]
		public virtual void ConfigureDeviceProperties()
		{
			AssertConnected();

			camera.ShowDeviceProperties();
		}
	}
}