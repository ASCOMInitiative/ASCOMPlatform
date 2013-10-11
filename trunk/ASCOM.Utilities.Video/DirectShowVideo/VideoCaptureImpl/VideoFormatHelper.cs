using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl
{
	internal static class VideoFormatHelper
	{
		internal class SupportedVideoFormat
		{
			public SupportedVideoFormat()
			{ }

			private static Regex REGEX_FORMATTER = new Regex("^(\\d+) x (\\d+) @([\\d\\.]+) fps \\((\\d+) bpp\\)$");

			public SupportedVideoFormat(string stringRep)
			{
				if (!string.IsNullOrEmpty(stringRep))
				{
					Match regexMatch = REGEX_FORMATTER.Match(stringRep);
					if (regexMatch.Success)
					{
						Width = int.Parse(regexMatch.Groups[1].Value);
						Height = int.Parse(regexMatch.Groups[2].Value);
						FrameRate = double.Parse(regexMatch.Groups[3].Value, CultureInfo.InvariantCulture);
						BitCount = int.Parse(regexMatch.Groups[4].Value);
					}
				}
			}

			public bool Matches(SupportedVideoFormat compareTo)
			{
				return
					Width == compareTo.Width &&
					Height == compareTo.Height &&
					Math.Abs(FrameRate - compareTo.FrameRate) < 0.01 &&
					BitCount == compareTo.BitCount;
			}

			public int Width;
			public int Height;
			public int BitCount;
			public double FrameRate;

			public override string ToString()
			{
				return string.Format("{0} x {1} @{2} fps", Width, Height, FrameRate.ToString("0.00"));
			}

			public string AsSerialized()
			{
				return string.Format("{0} x {1} @{2} fps ({3} bpp)", Width, Height, FrameRate.ToString("0.00"), BitCount);
			}
		}

		public static void LoadSupportedVideoFormats(string deviceName, ComboBox cbxVideoFormats)
		{
			var allFormatsDict = new Dictionary<string, SupportedVideoFormat>();
			DoSupportedVideoFormatsOperation(
				deviceName,
				delegate(SupportedVideoFormat format)
				{
					string key = format.ToString();
					SupportedVideoFormat existing;
					allFormatsDict.TryGetValue(key, out existing);
					if (existing != null)
					{
						if (existing.BitCount < format.BitCount)
							allFormatsDict[key] = format;
					}
					else
						allFormatsDict.Add(key, format);
				});

			List<SupportedVideoFormat> uniqueFormats = allFormatsDict.Values.ToList();
			uniqueFormats.Sort(
				delegate(SupportedVideoFormat x, SupportedVideoFormat y)
				{
					int compareByFrameRate = x.FrameRate.CompareTo(y.FrameRate);
					if (compareByFrameRate == 0)
						return (x.Width + x.Height).CompareTo(y.Width + y.Height);
					else
						return compareByFrameRate;
				});

			uniqueFormats.ForEach(x => cbxVideoFormats.Items.Add(x));
		}

		private static IBaseFilter CreateFilter(Guid category, string friendlyname)
		{
			object source = null;
			Guid iid = typeof(IBaseFilter).GUID;
			foreach (DsDevice device in DsDevice.GetDevicesOfCat(category))
			{
				if (device.Name.CompareTo(friendlyname) == 0)
				{
					device.Mon.BindToObject(null, null, ref iid, out source);
					break;
				}
			}

			return (IBaseFilter)source;
		}

		internal delegate void SupportedVideoFormatCallback(SupportedVideoFormat format);

		private static void DoSupportedVideoFormatsOperation(string deviceName, SupportedVideoFormatCallback callback)
		{
			IGraphBuilder graphBuilder = null;
			ICaptureGraphBuilder2 captureGraphBuilder = null;
			IBaseFilter theDevice = null;

			try
			{
				graphBuilder = (IGraphBuilder)new FilterGraph();
				captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
				theDevice = CreateFilter(FilterCategory.VideoInputDevice, deviceName);

				// Attach the filter graph to the capture graph
				int hr = captureGraphBuilder.SetFiltergraph(graphBuilder);
				DsError.ThrowExceptionForHR(hr);

				// Add the Video input device to the graph
				hr = graphBuilder.AddFilter(theDevice, "source filter");
				DsError.ThrowExceptionForHR(hr);

				object o;
				// AMMediaType media;
				IAMStreamConfig videoStreamConfig;
				IAMVideoControl videoControl = theDevice as IAMVideoControl;

				hr = captureGraphBuilder.FindInterface(PinCategory.Capture, MediaType.Video, theDevice, typeof(IAMStreamConfig).GUID, out o);
				DsError.ThrowExceptionForHR(hr);

				videoStreamConfig = o as IAMStreamConfig;
				try
				{
					if (videoStreamConfig == null)
					{
						throw new Exception("Failed to get IAMStreamConfig");
					}

					int iCount = 0, iSize = 0;
					videoStreamConfig.GetNumberOfCapabilities(out iCount, out iSize);

					IntPtr taskMemPointer = Marshal.AllocCoTaskMem(iSize);

					AMMediaType pmtConfig = null;
					for (int iFormat = 0; iFormat < iCount; iFormat++)
					{
						IntPtr ptr = IntPtr.Zero;

						videoStreamConfig.GetStreamCaps(iFormat, out pmtConfig, taskMemPointer);

						var v2 = (VideoInfoHeader)Marshal.PtrToStructure(pmtConfig.formatPtr, typeof(VideoInfoHeader));

						if (v2.BmiHeader.BitCount > 0)
						{
							var entry = new SupportedVideoFormat()
							{
								Width = v2.BmiHeader.Width,
								Height = v2.BmiHeader.Height,
								BitCount = v2.BmiHeader.BitCount,
								FrameRate = 10000000.0 / v2.AvgTimePerFrame
							};

							callback(entry);

							Trace.WriteLine(entry.AsSerialized());
						}
					}

					Marshal.FreeCoTaskMem(taskMemPointer);
					DsUtils.FreeAMMediaType(pmtConfig);
				}
				finally
				{
					Marshal.ReleaseComObject(videoStreamConfig);
				}
			}
			finally
			{
				if (theDevice != null)
					Marshal.ReleaseComObject(theDevice);

				if (graphBuilder != null)
					Marshal.ReleaseComObject(graphBuilder);

				if (captureGraphBuilder != null)
					Marshal.ReleaseComObject(captureGraphBuilder);
			}
		}

		public static void FixFlippedVideo(IAMVideoControl videoControl, IPin pPin)
		{
			VideoControlFlags pCapsFlags;

			int hr = videoControl.GetCaps(pPin, out pCapsFlags);
			DsError.ThrowExceptionForHR(hr);

			if ((pCapsFlags & VideoControlFlags.FlipVertical) > 0)
			{
				hr = videoControl.GetMode(pPin, out pCapsFlags);
				DsError.ThrowExceptionForHR(hr);

				hr = videoControl.SetMode(pPin, pCapsFlags & ~VideoControlFlags.FlipVertical);
				DsError.ThrowExceptionForHR(hr);

				PinInfo pinInfo;
				hr = pPin.QueryPinInfo(out pinInfo);
				DsError.ThrowExceptionForHR(hr);

				Trace.WriteLine("Fixing 'FlipVertical' video for pin " + pinInfo.name);
			}

			if ((pCapsFlags & VideoControlFlags.FlipHorizontal) > 0)
			{
				hr = videoControl.GetMode(pPin, out pCapsFlags);
				DsError.ThrowExceptionForHR(hr);

				hr = videoControl.SetMode(pPin, pCapsFlags | VideoControlFlags.FlipHorizontal);
				DsError.ThrowExceptionForHR(hr);

				PinInfo pinInfo;
				hr = pPin.QueryPinInfo(out pinInfo);
				DsError.ThrowExceptionForHR(hr);

				Trace.WriteLine("Fixing 'FlipHorizontal' video for pin " + pinInfo.name);
			}
		}
	}
}
