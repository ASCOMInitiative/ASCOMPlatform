//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow
//
// Description:	This is the implementation of the DirectShow Capture functionality 
//              used by the driver. This class is based on a number of examples from
//              the DirectShowNet project (http://directshownet.sourceforge.net/)
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 21-Mar-2013	HDP	6.0.0	Initial commit
// 22-Mar-2013	HDP	6.0.0	Added support for XviD and Huffyuv codecs
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl;
using ASCOM.Utilities.Video.DirectShowVideo;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl
{
	internal class DirectShowCapture : ISampleGrabberCB, IDisposable
	{
		[DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
		private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] uint Length);

		//A (modified) definition of OleCreatePropertyFrame found here: http://groups.google.no/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/db794e9779144a46/55dbed2bab4cd772?lnk=st&q=[DllImport(%22olepro32.dll%22)]&rnum=1&hl=no#55dbed2bab4cd772
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int OleCreatePropertyFrame(
			IntPtr hwndOwner,
			int x,
			int y,
			[MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
			int cObjects,
			[MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] 
			ref object ppUnk,
			int cPages,
			IntPtr lpPageClsID,
			int lcid,
			int dwReserved,
			IntPtr lpvReserved);


		private IFilterGraph2 filterGraph;
		private IMediaControl mediaCtrl;
		private ISampleGrabber samplGrabber;
		private ICaptureGraphBuilder2 capBuilder;
		private IBaseFilter deviceFilter = null;

		private bool isRunning = false;
        private bool firstFrameReceived = false;

		private int videoWidth;
		private int videoHeight;
		private int stride;
		private long frameCounter;

		Bitmap latestBitmap = null;
		Rectangle fullRect;

		private object syncRoot = new object();
		
		// NOTE: If the graph doesn't show up in GraphEdit then see this: http://sourceforge.net/p/directshownet/discussion/460697/thread/67dbf387
		private DsROTEntry rot = null;

		private CrossbarHelper crossbarHelper;

		public DirectShowCapture(DirectShowVideoSettings settings)
		{
			crossbarHelper = new CrossbarHelper(settings);
		}

		public void SetupFileRecorderGraph(DsDevice dev, SystemCodecEntry compressor, VideoFormatHelper.SupportedVideoFormat selectedFormat, ref float iFrameRate, ref int iWidth, ref int iHeight, string fileName)
		{
			try
			{
				SetupGraphInternal(dev, compressor, selectedFormat, ref iFrameRate, ref iWidth, ref iHeight, fileName);

				latestBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
				fullRect = new Rectangle(0, 0, latestBitmap.Width, latestBitmap.Height);
			}
			catch
			{
				CloseResources();
				throw;
			} 
		}

		public void SetupPreviewOnlyGraph(DsDevice dev, VideoFormatHelper.SupportedVideoFormat selectedFormat, ref float iFrameRate, ref int iWidth, ref int iHeight)
		{
			try
			{
				SetupGraphInternal(dev, null, selectedFormat, ref iFrameRate, ref iWidth, ref iHeight, null);

				latestBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
				fullRect = new Rectangle(0, 0, latestBitmap.Width, latestBitmap.Height);
			}
			catch
			{
				CloseResources();
				throw;
			}
		}

		public bool IsRunning
		{
			get { return isRunning;}
		}

		public void Start()
		{
			if (!isRunning)
			{
				frameCounter = 0;
                firstFrameReceived = false;

				int hr = mediaCtrl.Run();
				DsError.ThrowExceptionForHR(hr);

				isRunning = true;                
			}
		}

		public void Pause()
		{
			if (isRunning)
			{
				int hr = mediaCtrl.Pause();
				DsError.ThrowExceptionForHR(hr);

				isRunning = false;
			}
		}

		public Bitmap GetNextFrame(out long frameId)
		{
			if (latestBitmap == null)
			{
				frameId = -1;
				return null;
			}

            if (!firstFrameReceived)
            {
				crossbarHelper.SetupTunerAndCrossbar(capBuilder, deviceFilter);
                firstFrameReceived = true;
            }

			Bitmap rv = null;

			NonBlockingLock.Lock(
				NonBlockingLock.LOCK_ID_GetNextFrame,
				() =>
				{
					rv = (Bitmap)latestBitmap.Clone();
				});

			frameId = frameCounter;
			return rv;
		}

		private IBaseFilter CreateFilter(Guid category, string friendlyname)
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

		private void SetupGraphInternal(DsDevice dev, SystemCodecEntry compressor, VideoFormatHelper.SupportedVideoFormat selectedFormat, ref float iFrameRate, ref int iWidth, ref int iHeight, string fileName)
		{
			filterGraph = (IFilterGraph2)new FilterGraph();
			mediaCtrl = filterGraph as IMediaControl;

			capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

			samplGrabber = (ISampleGrabber)new SampleGrabber();

			int hr = capBuilder.SetFiltergraph(filterGraph);
			DsError.ThrowExceptionForHR(hr);

			if (rot != null)
			{
				rot.Dispose();
				rot = null;
			}
			rot = new DsROTEntry(filterGraph);

			if (fileName != null)
				deviceFilter = BuildFileCaptureGraph(dev, compressor.Device, selectedFormat, fileName, ref iFrameRate, ref iWidth, ref iHeight);
			else
				deviceFilter = BuildPreviewOnlyCaptureGraph(dev, selectedFormat, ref iFrameRate, ref iWidth, ref iHeight);

			// Now that sizes are fixed/known, store the sizes
			SaveSizeInfo(samplGrabber);
		}

		private IBaseFilter BuildPreviewOnlyCaptureGraph(DsDevice dev, VideoFormatHelper.SupportedVideoFormat selectedFormat, ref float iFrameRate, ref int iWidth, ref int iHeight)
		{
			// Capture Source (Capture/Video) --> (Input) Sample Grabber (Output) --> (In) Null Renderer

			IBaseFilter nullRenderer = null;

			try
			{
				IBaseFilter capFilter;

				// Add the video device
				int hr = filterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
				DsError.ThrowExceptionForHR(hr);

				if (capFilter != null)
					SetConfigParms(capBuilder, capFilter, selectedFormat, ref iFrameRate, ref iWidth, ref iHeight);

				IBaseFilter baseGrabFlt = (IBaseFilter)samplGrabber;
				ConfigureSampleGrabber(samplGrabber);

				hr = filterGraph.AddFilter(baseGrabFlt, "ASCOM Video Grabber");
				DsError.ThrowExceptionForHR(hr);

				// Connect the video device output to the sample grabber
				IPin videoCaptureOutputPin = FindPin(capFilter, PinDirection.Output, MediaType.Video, PinCategory.Capture, "Capture");				
				IPin grabberInputPin = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);
				hr = filterGraph.Connect(videoCaptureOutputPin, grabberInputPin);
				DsError.ThrowExceptionForHR(hr);
				Marshal.ReleaseComObject(videoCaptureOutputPin);
				Marshal.ReleaseComObject(grabberInputPin);

				// Add the frame grabber to the graph
				nullRenderer = (IBaseFilter)new NullRenderer();
				hr = filterGraph.AddFilter(nullRenderer, "ASCOM Video Null Renderer");
				DsError.ThrowExceptionForHR(hr);

				// Connect the sample grabber to the null renderer (so frame samples will be coming through)
				IPin grabberOutputPin = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Output, 0);
				IPin renderedInputPin = DsFindPin.ByDirection(nullRenderer, PinDirection.Input, 0);
				hr = filterGraph.Connect(grabberOutputPin, renderedInputPin);
				DsError.ThrowExceptionForHR(hr);
				Marshal.ReleaseComObject(grabberOutputPin);
				Marshal.ReleaseComObject(renderedInputPin);
				
				return capFilter;
			}
			finally
			{				
				if (nullRenderer != null)
					Marshal.ReleaseComObject(nullRenderer);
			}
		}

		private IBaseFilter BuildFileCaptureGraph(DsDevice dev, DsDevice compressor, VideoFormatHelper.SupportedVideoFormat selectedFormat, string fileName, ref float iFrameRate, ref int iWidth, ref int iHeight)
		{
			// Capture Source (Capture/Video) --> (Input) Sample Grabber (Output) --> (Input) Video Compressor (Output) --> (Input 01/Video/) AVI Mux (Output) --> (In) FileSink

			IBaseFilter muxFilter = null;
			IFileSinkFilter fileWriterFilter = null;
			IBaseFilter compressorFilter = null;

			try
			{
				IBaseFilter capFilter;

				// Add the video device
				int hr = filterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
				DsError.ThrowExceptionForHR(hr);

				if (capFilter != null)
					SetConfigParms(capBuilder, capFilter, selectedFormat, ref iFrameRate, ref iWidth, ref iHeight);

				IBaseFilter baseGrabFlt = (IBaseFilter)samplGrabber;
				ConfigureSampleGrabber(samplGrabber);

				hr = filterGraph.AddFilter(baseGrabFlt, "ASCOM Video Grabber");
				DsError.ThrowExceptionForHR(hr);

				// Connect the video device output to the sample grabber
				IPin videoCaptureOutputPin = FindPin(capFilter, PinDirection.Output, MediaType.Video, Guid.Empty, "Capture");
				IPin smartTeeInputPin = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);
				hr = filterGraph.Connect(videoCaptureOutputPin, smartTeeInputPin);
				DsError.ThrowExceptionForHR(hr);
				Marshal.ReleaseComObject(videoCaptureOutputPin);
				Marshal.ReleaseComObject(smartTeeInputPin);

				// Create the file writer and AVI Mux (already connected to each other)
				hr = capBuilder.SetOutputFileName(MediaSubType.Avi, fileName, out muxFilter, out fileWriterFilter);
				DsError.ThrowExceptionForHR(hr);

				if (compressor != null)
					// Create the compressor
					compressorFilter = CreateFilter(FilterCategory.VideoCompressorCategory, compressor.Name);

				if (compressorFilter != null)
				{
					hr = filterGraph.AddFilter(compressorFilter, "ASCOM Video Compressor");
					DsError.ThrowExceptionForHR(hr);

					// Connect the sample grabber Output pin to the compressor
					IPin grabberOutputPin = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Output, 0);
					IPin compressorInputPin = DsFindPin.ByDirection(compressorFilter, PinDirection.Input, 0);
					hr = filterGraph.Connect(grabberOutputPin, compressorInputPin);
					DsError.ThrowExceptionForHR(hr);
					Marshal.ReleaseComObject(grabberOutputPin);
					Marshal.ReleaseComObject(compressorInputPin);

					// Connect the compressor output to the AVI Mux
					IPin compressorOutputPin = DsFindPin.ByDirection(compressorFilter, PinDirection.Output, 0);
					IPin aviMuxVideoInputPin = DsFindPin.ByDirection(muxFilter, PinDirection.Input, 0);
					hr = filterGraph.Connect(compressorOutputPin, aviMuxVideoInputPin);
					DsError.ThrowExceptionForHR(hr);
					Marshal.ReleaseComObject(compressorOutputPin);
					Marshal.ReleaseComObject(aviMuxVideoInputPin);
				}
				else
				{
					// Connect the sample grabber Output pin to the AVI Mux
					IPin grabberOutputPin = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Output, 0);
					IPin aviMuxVideoInputPin = DsFindPin.ByDirection(muxFilter, PinDirection.Input, 0);
					hr = filterGraph.Connect(grabberOutputPin, aviMuxVideoInputPin);
					DsError.ThrowExceptionForHR(hr);
					Marshal.ReleaseComObject(grabberOutputPin);
					Marshal.ReleaseComObject(aviMuxVideoInputPin);					
				}

				return capFilter;
			}
			finally
			{
				if (fileWriterFilter != null)
					Marshal.ReleaseComObject(fileWriterFilter);

				if (muxFilter != null)
					Marshal.ReleaseComObject(muxFilter);

				if (compressorFilter != null)
					Marshal.ReleaseComObject(compressorFilter);
			}
		}

		private IPin FindPin(IBaseFilter filter, PinDirection direction, Guid mediaType, Guid pinCategory, string preferredName)
		{
			if (Guid.Empty != pinCategory)
			{
				int idx = 0;

				do
				{
					IPin pinByCategory = DsFindPin.ByCategory(filter, pinCategory, idx);

					if (pinByCategory != null)
					{
						if (IsMatchingPin(pinByCategory, direction, mediaType))
							return pinByCategory;

						Marshal.ReleaseComObject(pinByCategory);
					}
					else
						break;

					idx++;
				}
				while (true);
			}

			if (!string.IsNullOrEmpty(preferredName))
			{
				IPin pinByName = DsFindPin.ByName(filter, preferredName);
				if (pinByName != null && IsMatchingPin(pinByName, direction, mediaType))
					return pinByName;

				Marshal.ReleaseComObject(pinByName);
			}

			IEnumPins pinsEnum;
			IPin[] pins = new IPin[1];

			int hr = filter.EnumPins(out pinsEnum);
			DsError.ThrowExceptionForHR(hr);

			while (pinsEnum.Next(1, pins, IntPtr.Zero) == 0)
			{
				IPin pin = pins[0];
				if (pin != null)
				{
					if (IsMatchingPin(pin, direction, mediaType))
						return pin;

					Marshal.ReleaseComObject(pin);
				}
			}

			return null;
		}


		private bool IsMatchingPin(IPin pin, PinDirection direction, Guid mediaType)
		{
			PinDirection pinDirection;
			int hr = pin.QueryDirection(out pinDirection);
			DsError.ThrowExceptionForHR(hr);

			if (pinDirection != direction)
				// The pin lacks direction
				return false;

			IPin connectedPin;
			hr = pin.ConnectedTo(out connectedPin);
			if ((uint)hr != 0x80040209 /* Pin is not connected */)
				DsError.ThrowExceptionForHR(hr);

			if (connectedPin != null)
			{
				// The pin is already connected
				Marshal.ReleaseComObject(connectedPin);
				return false;
			}

			IEnumMediaTypes mediaTypesEnum;
			hr = pin.EnumMediaTypes(out mediaTypesEnum);
			DsError.ThrowExceptionForHR(hr);

			AMMediaType[] mediaTypes = new AMMediaType[1];

			while (mediaTypesEnum.Next(1, mediaTypes, IntPtr.Zero) == 0)
			{
				Guid majorType = mediaTypes[0].majorType;
				DsUtils.FreeAMMediaType(mediaTypes[0]);

				if (majorType == mediaType)
				{
					// We have found the pin we were looking for
					return true;
				}
			}

			return false;
		}

		private void SaveSizeInfo(ISampleGrabber sampGrabber)
		{
			AMMediaType media = new AMMediaType();
			int hr = sampGrabber.GetConnectedMediaType(media);
			DsError.ThrowExceptionForHR(hr);

			if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
			{
				throw new NotSupportedException("Unknown Grabber Media Format");
			}

			VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
			videoWidth = videoInfoHeader.BmiHeader.Width;
			videoHeight = videoInfoHeader.BmiHeader.Height;
			stride = videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

			DsUtils.FreeAMMediaType(media);
		}

		private void SetConfigParms(ICaptureGraphBuilder2 capBuilder, IBaseFilter capFilter, VideoFormatHelper.SupportedVideoFormat selectedFormat, ref float iFrameRate, ref int iWidth, ref int iHeight)
		{
			object o;
			IAMStreamConfig videoStreamConfig;
			IAMVideoControl videoControl = capFilter as IAMVideoControl;

			int hr = capBuilder.FindInterface(PinCategory.Capture, MediaType.Video, capFilter, typeof(IAMStreamConfig).GUID, out o);

			videoStreamConfig = o as IAMStreamConfig;
			try
			{
				if (videoStreamConfig == null)
				{
					throw new Exception("Failed to get IAMStreamConfig");
				}

				int iCount = 0, iSize = 0;
				hr = videoStreamConfig.GetNumberOfCapabilities(out iCount, out iSize);
				DsError.ThrowExceptionForHR(hr);

				VideoInfoHeader vMatching = null;
				VideoFormatHelper.SupportedVideoFormat entry = null;

				IntPtr taskMemPointer = Marshal.AllocCoTaskMem(iSize);

				AMMediaType pmtConfig = null;
				for (int iFormat = 0; iFormat < iCount; iFormat++)
				{
					IntPtr ptr = IntPtr.Zero;

					hr = videoStreamConfig.GetStreamCaps(iFormat, out pmtConfig, taskMemPointer);
					DsError.ThrowExceptionForHR(hr);

					vMatching = (VideoInfoHeader)Marshal.PtrToStructure(pmtConfig.formatPtr, typeof(VideoInfoHeader));

					if (vMatching.BmiHeader.BitCount > 0)
					{
						entry = new VideoFormatHelper.SupportedVideoFormat()
						{
							Width = vMatching.BmiHeader.Width,
							Height = vMatching.BmiHeader.Height,
							BitCount = vMatching.BmiHeader.BitCount,
							FrameRate = 10000000.0 / vMatching.AvgTimePerFrame
						};

						if (entry.Matches(selectedFormat))
						{
							// WE FOUND IT !!!
							break;
						}
					}

					vMatching = null;
				}

				if (vMatching != null)
				{
					hr = videoStreamConfig.SetFormat(pmtConfig);
					DsError.ThrowExceptionForHR(hr);

					iFrameRate = 10000000 / vMatching.AvgTimePerFrame;
					iWidth = vMatching.BmiHeader.Width;
					iHeight = vMatching.BmiHeader.Height;
				}
				else
				{
					AMMediaType media;
					hr = videoStreamConfig.GetFormat(out media);
					DsError.ThrowExceptionForHR(hr);

					// Copy out the videoinfoheader
					VideoInfoHeader v = new VideoInfoHeader();
					Marshal.PtrToStructure(media.formatPtr, v);

					// If overriding the framerate, set the frame rate
					if (iFrameRate > 0)
					{
						v.AvgTimePerFrame = (int)Math.Round(10000000 / iFrameRate);
					}
					else
						iFrameRate = 10000000 / v.AvgTimePerFrame;

					// If overriding the width, set the width
					if (iWidth > 0)
					{
						v.BmiHeader.Width = iWidth;
					}
					else
						iWidth = v.BmiHeader.Width;

					// If overriding the Height, set the Height
					if (iHeight > 0)
					{
						v.BmiHeader.Height = iHeight;
					}
					else
						iHeight = v.BmiHeader.Height;

					// Copy the media structure back
					Marshal.StructureToPtr(v, media.formatPtr, false);

					// Set the new format
					hr = videoStreamConfig.SetFormat(media);
					DsError.ThrowExceptionForHR(hr);

					DsUtils.FreeAMMediaType(media);
					media = null;
				}

				Marshal.FreeCoTaskMem(taskMemPointer);
				DsUtils.FreeAMMediaType(pmtConfig);
				pmtConfig = null;
			}
			finally
			{
				Marshal.ReleaseComObject(videoStreamConfig);
			}
		}

		private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
		{
			AMMediaType media = new AMMediaType();

			// Set the media type to Video/RBG24
			media.majorType = MediaType.Video;
			media.subType = MediaSubType.RGB24;
			media.formatType = FormatType.VideoInfo;
			int hr = sampGrabber.SetMediaType(media);
			DsError.ThrowExceptionForHR(hr);

			DsUtils.FreeAMMediaType(media);
			media = null;

			// Configure the samplegrabber callback
			hr = sampGrabber.SetCallback(this, 1);
			DsError.ThrowExceptionForHR(hr);
		}

        public void CloseResources()
        {
            CloseInterfaces();

			lock (syncRoot)
	        {
		        if (latestBitmap != null)
		        {
					latestBitmap.Dispose();
			        latestBitmap = null;
		        }

				if (samplGrabber != null)
				{
					Marshal.ReleaseComObject(samplGrabber);
					samplGrabber = null;
				}

				if (capBuilder != null)
				{
					Marshal.ReleaseComObject(capBuilder);
					capBuilder = null;
				}

				if (rot != null)
				{
					rot.Dispose();
					rot = null;
				}
			}
        }

		public void Dispose()
		{
			CloseResources();
		}

		~DirectShowCapture()
        {
            CloseInterfaces();

			GC.SuppressFinalize(this);
        }

		private void CloseInterfaces()
		{
			try
			{
				Thread.Sleep(50);

				if (mediaCtrl != null)
				{
					NonBlockingLock.ExclusiveLock(
						NonBlockingLock.LOCK_ID_CloseInterfaces,
						() =>
						{
							Application.DoEvents();

							// Stop the graph
							int hr = mediaCtrl.Stop();
							DsError.ThrowExceptionForHR(hr);
						});

					mediaCtrl = null;
					isRunning = false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

			if (filterGraph != null)
			{
				Marshal.ReleaseComObject(filterGraph);
				filterGraph = null;
			}

			if (deviceFilter != null)
			{
				Marshal.ReleaseComObject(deviceFilter);
				deviceFilter = null;
			}
		}

		int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
		{
			Marshal.ReleaseComObject(pSample);

			return 0;
		}

		/// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
		int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
		{
			NonBlockingLock.Lock(
				NonBlockingLock.LOCK_ID_BufferCB,
				() =>
				{
					CopyBitmap(pBuffer);

					frameCounter++;
				});

			Thread.Sleep(1);

			return 0;
		}

		private void CopyBitmap(IntPtr pBuffer)
		{
			if (latestBitmap != null)
			{
				BitmapData bmd = latestBitmap.LockBits(fullRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
				try
				{
					IntPtr ipSource = (IntPtr)(pBuffer.ToInt32() + stride * (videoHeight - 1));
					IntPtr ipDest = bmd.Scan0;

					for (int x = 0; x < videoHeight; x++)
					{
						CopyMemory(ipDest, ipSource, (uint)stride);
						ipDest = (IntPtr)(ipDest.ToInt32() + bmd.Stride);
						ipSource = (IntPtr)(ipSource.ToInt32() - stride);
					}
				}
				finally
				{
					latestBitmap.UnlockBits(bmd);
				}
			}
		}

		/// <summary>
		/// Displays a property page for a filter
		/// </summary>
		/// <param name="dev">The filter for which to display a property page</param>
		public static void DisplayPropertyPage(IBaseFilter dev, IntPtr hwndOwner)
		{
			//Get the ISpecifyPropertyPages for the filter
			ISpecifyPropertyPages pProp = dev as ISpecifyPropertyPages;
			int hr = 0;

			if (pProp == null)
			{
				//If the filter doesn't implement ISpecifyPropertyPages, try displaying IAMVfwCompressDialogs instead!
				IAMVfwCompressDialogs compressDialog = dev as IAMVfwCompressDialogs;
				if (compressDialog != null)
				{

                    try
                    {
                        compressDialog.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
                    }
                    catch(Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
				}
				return;
			}

			//Get the name of the filter from the FilterInfo struct
			FilterInfo filterInfo;
			hr = dev.QueryFilterInfo(out filterInfo);
			DsError.ThrowExceptionForHR(hr);

			// Get the propertypages from the property bag
			DsCAUUID caGUID;
			hr = pProp.GetPages(out caGUID);
			DsError.ThrowExceptionForHR(hr);

			// Create and display the OlePropertyFrame
			object oDevice = (object)dev;
			hr = OleCreatePropertyFrame(hwndOwner, 0, 0, filterInfo.achName, 1, ref oDevice, caGUID.cElems, caGUID.pElems, 0, 0, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

			// Release COM objects
			Marshal.FreeCoTaskMem(caGUID.pElems);
			Marshal.ReleaseComObject(pProp);
			if (filterInfo.pGraph != null)
			{
				Marshal.ReleaseComObject(filterInfo.pGraph);
			}
		}

		public void ShowDeviceProperties()
		{
			if (deviceFilter != null)
				DisplayPropertyPage(deviceFilter, IntPtr.Zero);
		}

	}
}
