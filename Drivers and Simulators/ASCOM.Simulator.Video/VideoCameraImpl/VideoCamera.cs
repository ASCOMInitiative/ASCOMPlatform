//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This file implements a simulated camera. This is the actual 
//			    implementation of the simulator
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video;
using ASCOM.Simulator.Properties;
using ASCOM.Simulator.Utils;

namespace Simulator.VideoCameraImpl
{
	public class VideoCameraFrame
	{
		public int[,] Pixels;
		public byte[] PreviewBitmapBytes;
		public long? FrameNumber;
		public DateTime? ExposureStartTime;
		public double? ExposureDuration;
		public string ImageInfo;
	}

	public class VideoCamera
	{
		private List<string> supportedUpperCaseActionNames = new List<string>();
		private List<string> supportedActionNames = new List<string>();

		private List<string> supportedIntegrationRates = new List<string>();
		private List<string> supportedUpperCaseExposureRates = new List<string>();

		private bool isConnected = false;
		private BitmapVideoPlayer bitmapPlayer = null;

		private List<string> supportedGains = new List<string>();
		private List<string> supportedGammas = new List<string>();

		private short freeRangeGainValue;
		private int selectedDiscreteGainsIndex;

		private short freeRangeGammaValue;
		private int selectedDiscreteGammaIndex;

		public string selectedIntegrationRate;

		public int selectedWhiteBalance = 0;

		private VideoCameraState cameraState = VideoCameraState.videoCameraIdle;

		private bool useBuffering;
		private int bufferSize;

		private int[,] alteredPixels;

        private AviTools aviTools;

		public VideoCamera()
		{
            aviTools = new AviTools();
			ReloadSimulatorSettings();

			isConnected = false;
		}

		public bool IsConnected
		{
			get { return isConnected; }
		}

		public void EnsureConnected()
		{
			if (bitmapPlayer == null)
				bitmapPlayer = new BitmapVideoPlayer(
					Settings.Default.UseEmbeddedVideoSource, 
					Settings.Default.SourceBitmapFilesLocation, 
					useBuffering ? bufferSize : 0);

			alteredPixels = new int[bitmapPlayer.Height, bitmapPlayer.Width];

			SetBitmapPlayerFrameRate();

			bitmapPlayer.Start();

			isConnected = true;

			cameraState = VideoCameraState.videoCameraRunning;
		}

		public void EnsureDisconnected()
		{
			if (cameraState == VideoCameraState.videoCameraRecording)
			{
				StopRecordingVideoFile();
			}

			if (bitmapPlayer != null)
			{
				cameraState = VideoCameraState.videoCameraIdle;

				bitmapPlayer.Stop();
			}

			alteredPixels = null;
			bitmapPlayer = null;
			isConnected = false;
		}

		public List<string> SupportedActions
		{
			get { return supportedActionNames; }
		}

		public List<string> SupportedIntegrationRates
		{
			get
			{
				if (Settings.Default.CameraType == SiumulatedCameraType.AnalogueNonIntegrating)
				{
					switch (Settings.Default.CameraFrameRate)
					{
						case AnalogueCameraFrameRate.PAL:
							return new List<string>(new string[] {"25.00"});

						case AnalogueCameraFrameRate.NTSC:
							return new List<string>(new string[] {"29.97"});

						default:
							throw new IndexOutOfRangeException();
					}
				}
				else
					return supportedIntegrationRates;
			}
		}

		public string IntegrationRate
		{
			get { return selectedIntegrationRate; }
		}

		public void ChangeIntegrationRate(string NewIntegrationRate)
		{
			if (Settings.Default.CameraType != SiumulatedCameraType.AnalogueNonIntegrating)
			{
				if (!string.IsNullOrEmpty(NewIntegrationRate))
				{
					float frameRateValue;
					int integrationValue;
					if (float.TryParse(NewIntegrationRate, NumberStyles.Number, CultureInfo.InvariantCulture, out frameRateValue))
					{
						if (Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.NTSC)
							integrationValue = (int) Math.Round(frameRateValue * 29.97);
						else
							integrationValue = (int)Math.Round(frameRateValue * 25);

						bitmapPlayer.SetIntegration(integrationValue);
						selectedIntegrationRate = NewIntegrationRate;
					}
					else if (
						NewIntegrationRate.StartsWith("x", true, CultureInfo.InvariantCulture) &&
						int.TryParse(NewIntegrationRate, NumberStyles.Number, CultureInfo.InvariantCulture, out integrationValue))
					{
						bitmapPlayer.SetIntegration(integrationValue);
						selectedIntegrationRate = NewIntegrationRate;
					}
					else
					{
						throw new DriverException(string.Format("The exposure rate '{0}' is not a number and is not in the format xNNN.", NewIntegrationRate));
					}
				}
			}
		}

		public List<string> SupportedGains
		{
			get { return supportedGains; }
		}


		public List<string> SupportedGammas
		{
			get { return supportedGammas; }
		} 

		public void SetGain(short newGain)
		{
			freeRangeGainValue = newGain;
			selectedDiscreteGainsIndex = -1;
		}

		public short GetCurrentGain()
		{
			if (selectedDiscreteGainsIndex == -1)
				return freeRangeGainValue;
			else
				return (short)selectedDiscreteGainsIndex;
		}

		private short? maxSupportedGain = null;

		public short GetMaxGain()
		{
			if (!maxSupportedGain.HasValue)
			{
				short maxGain;
				if (!Settings.Default.SupportsGainRange &&
					supportedGains.Count > 0 &&
					short.TryParse(supportedGains[supportedGains.Count - 1], NumberStyles.Number, CultureInfo.InvariantCulture, out maxGain))
				{
					maxSupportedGain = maxGain;
					return maxSupportedGain.Value;
				}
				else if (Settings.Default.SupportsGainRange)
				{
					maxSupportedGain = Settings.Default.GainMax;
					return maxSupportedGain.Value;
				}
					
				return 0;
			}
			else
				return maxSupportedGain.Value;
		}

		public void SetDiscreteGain(string gainNameOrValue)
		{
			selectedDiscreteGainsIndex = supportedGains.IndexOf(gainNameOrValue);
			if (selectedDiscreteGainsIndex != -1)
			{
				freeRangeGainValue = short.MinValue;
			}
		}

		public void SetGamma(int newGammaIndex)
		{
			selectedDiscreteGammaIndex = newGammaIndex;

			double gammaVal = GetCurrentGammaValue();
            aviTools.SetNewGamma(gammaVal);
		}

		public int GetCurrentGamma()
		{
			if (selectedDiscreteGammaIndex == -1)
				return freeRangeGammaValue;
			else
				return (short)selectedDiscreteGammaIndex;
		}

		public void SetDiscreteGamma(string gammaNameOrValue)
		{
			selectedDiscreteGammaIndex = supportedGammas.IndexOf(gammaNameOrValue);
			if (selectedDiscreteGammaIndex != -1)
			{
				freeRangeGammaValue = short.MinValue;
				double gammaVal = GetCurrentGammaValue();
                aviTools.SetNewGamma(gammaVal);
			}
		}

		private static Regex GAMMA_REGEX = new Regex("\\((?<InBracketsValue>[^\\)]+)\\)");

		public double GetCurrentGammaValue()
		{
			double gammaVal;

			if (selectedDiscreteGammaIndex >=0 && selectedDiscreteGammaIndex < supportedGammas.Count)
			{
				string gamma = supportedGammas[selectedDiscreteGammaIndex];
				if (double.TryParse(gamma, NumberStyles.Number, CultureInfo.InvariantCulture, out gammaVal))
					return gammaVal;

				if (gamma != null)
				{
					if (gamma.StartsWith("HI", StringComparison.InvariantCultureIgnoreCase))
						return 0.35;
					else if (gamma.StartsWith("LO", StringComparison.InvariantCultureIgnoreCase))
						return 0.45;
					else if (gamma.StartsWith("OFF", StringComparison.InvariantCultureIgnoreCase))
						return 1.00;

					Match regexMatch = GAMMA_REGEX.Match(gamma);
					if (regexMatch.Success && 
						regexMatch.Groups["InBracketsValue"].Success)
					{
						if (double.TryParse(regexMatch.Groups["InBracketsValue"].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out gammaVal))
							return gammaVal;
					}
				}
			}

			return 1.0;
		}

		private static string[] LIST_ITEM_SEPARATOR = new string[] { "#;" };

		internal void ReloadSimulatorSettings()
		{
			supportedActionNames.Clear();
			supportedUpperCaseActionNames.Clear();

			if (!string.IsNullOrEmpty(Settings.Default.SupportedActionsList))
			{
				supportedActionNames.AddRange(
					Settings.Default.SupportedActionsList
						.Split(LIST_ITEM_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => x.Trim())
					);

				supportedUpperCaseActionNames.AddRange(
					supportedActionNames
						.Select(x => x.ToUpper())
					);
			}

			supportedIntegrationRates.Clear();

			if (!string.IsNullOrEmpty(Settings.Default.SupportedExposuresList))
			{
				supportedIntegrationRates.AddRange(
					Settings.Default.SupportedExposuresList
						.Split(LIST_ITEM_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => x.Trim())
					);

				supportedUpperCaseExposureRates.AddRange(
					supportedIntegrationRates
						.Select(x => x.ToUpper()));
			}

			supportedGains.Clear();

			if (!string.IsNullOrEmpty(Settings.Default.Gains))
			{
				supportedGains.AddRange(
					Settings.Default.Gains
						.Split(LIST_ITEM_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => x.Trim())
					);
			}

			supportedGammas.Clear();

			if (!string.IsNullOrEmpty(Settings.Default.Gammas))
			{
				supportedGammas.AddRange(
					Settings.Default.Gammas
						.Split(LIST_ITEM_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => x.Trim())
					);
			}

			useBuffering = Settings.Default.UseBuffering;
			bufferSize = Settings.Default.BufferSize;

			SetBitmapPlayerFrameRate();
		}

		private void SetBitmapPlayerFrameRate()
		{
			if (bitmapPlayer != null)
			{
				if (Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.PAL)
					bitmapPlayer.SetFrameWaitTime((int)Math.Round(1000.0 / 25.0));
				else if (Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.NTSC)
					bitmapPlayer.SetFrameWaitTime((int)Math.Round(1000.0 / 29.97));
			}			
		}

		public VideoCameraState GetCurrentCameraState()
		{
			return cameraState;
		}

		public string VideoCaptureDeviceName
		{
			get { return Settings.Default.VideoCaptureDeviceName; }
		}

		public bool IsActionSupported(string actionName)
		{
			return
				supportedUpperCaseActionNames.IndexOf(actionName.ToUpper()) > -1;
		}

		public string PerformAction(string actionName, string actionParameters)
		{
			if (IsActionSupported(actionName))
			{
				return string.Format("ECHO: {0} {1}", actionName, actionParameters);
			}
			else
				throw new NotSupportedException();
		}

		public double ExposureMin
		{
			get { return Settings.Default.ExposureMin; }
		}

		public double ExposureMax
		{
			get { return Settings.Default.ExposureMax; }
		}

		public VideoCameraFrameRate FrameRate
		{
			get
			{
				switch (Settings.Default.CameraType)
				{
					case SiumulatedCameraType.AnalogueIntegrating:
					case SiumulatedCameraType.AnalogueNonIntegrating:
						return Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.PAL
						       	? VideoCameraFrameRate.PAL
						       	: VideoCameraFrameRate.NTSC;

					case SiumulatedCameraType.Digital:
					case SiumulatedCameraType.VideoSystem:
						return VideoCameraFrameRate.Digital;

					default:
						throw new IndexOutOfRangeException();
				}
			}
		}

		public string SensorName
		{
			get { return Settings.Default.SensorName; }
		}

		public SensorType SensorType
		{
			get
			{
				return Enum.GetValues(typeof (SensorType))
					.Cast<SensorType>()
					.SingleOrDefault(x => string.Equals(x.ToString(), Settings.Default.SensorType));
			}
		}

		public int ImageWidth
		{
			get { return bitmapPlayer.Width; }
		}

		public int ImageHeight
		{
			get { return bitmapPlayer.Height; }
		}

		public int BitDepth
		{
			get
			{
				switch (Settings.Default.CameraType)
				{
					case SiumulatedCameraType.Digital:
					case SiumulatedCameraType.VideoSystem:
						return Settings.Default.BitDepth;

					default:
						return 8;
				}
			}
		}

		public int GetWhiteBalance()
		{
			return selectedWhiteBalance;
		}

		public void SetWhiteBalance(int newWhiteBalance)
		{
			if (selectedWhiteBalance != newWhiteBalance &&
				newWhiteBalance >= 0 &&
				newWhiteBalance <= 255)
			{
				selectedWhiteBalance = newWhiteBalance;

                aviTools.SetNewWhiteBalance(selectedWhiteBalance);
			}
		}

		public bool GetCurrentFrame(out VideoCameraFrame cameraFrame)
		{
			cameraFrame = null;

			if (bitmapPlayer.Running)
			{
				long currentFrameNo;
				int[,] currentFrame;
				byte[] previewBitmapBytes;

				bitmapPlayer.GetCurrentImage(out currentFrame, out currentFrameNo, out previewBitmapBytes);

				short currentGain = GetCurrentGain();
				double currentGamma = GetCurrentGammaValue();
				int currentWhiteBalance = GetWhiteBalance();

				if (currentGain != 0 || currentGamma != 1.0 || currentWhiteBalance > 0)
				{
					short brightness = (short)Math.Round(150 * ((double)currentGain / GetMaxGain()));

                    aviTools.ApplyGammaBrightness(currentFrame, alteredPixels, bitmapPlayer.Width, bitmapPlayer.Height, brightness);

					cameraFrame = new VideoCameraFrame()
					{
						Pixels = alteredPixels,
						FrameNumber = currentFrameNo
					};
				}
				else
				{
					cameraFrame = new VideoCameraFrame()
					{
						Pixels = currentFrame,
						FrameNumber = currentFrameNo,
						PreviewBitmapBytes = previewBitmapBytes
					};	
				}

				return true;
			}
				
			return false;
		}

		public string StartRecordingVideoFile(string PreferredFileName)
		{
			if (cameraState == VideoCameraState.videoCameraRunning)
			{
				double fps = 25;

				switch(FrameRate)
				{
					case VideoCameraFrameRate.PAL:
						fps = 25.0;
						break;

					case VideoCameraFrameRate.NTSC:
						fps = 29.97;
						break;

					case VideoCameraFrameRate.Digital:
						throw new DriverException("The driver simulator can only save video files for simulated analogue video cameras.");

					default:
						throw new DriverException("Cannot determine FPS rate for video.");
				}

				bitmapPlayer.StartRecording(PreferredFileName, fps, Settings.Default.ShowCompressionDialog);

				cameraState = VideoCameraState.videoCameraRecording;

				return PreferredFileName;
			}
			else
				throw new ASCOM.InvalidOperationException();
		}

		public void StopRecordingVideoFile()
		{
			if (cameraState == VideoCameraState.videoCameraRecording)
			{
				bitmapPlayer.StopRecording();

				cameraState = VideoCameraState.videoCameraRunning;
			}
		}
	}
}