//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow
//
// Description:	This file implements the IVideoFrame interface for the Video Capture Driver
//
// Implements:	ASCOM Video Frame interface version: 1
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-Mar-2013	HDP	6.0.0	Initial commit
// 21-Mar-2013	HDP	6.0.0	Implemented monochrome and colour grabbing
// 19-Sep-2013  HDP 6.1.0   Added the PreviewBitmap property
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl;

namespace ASCOM.Utilities.Video.DirectShowVideo
{
	internal class VideoFrame : IVideoFrame
	{
		private long? frameNumber;
		private string imageInfo;
		private double? exposureDuration;
		private string exposureStartTime;
		private object pixels;
		private byte[] previewBitmapBytes;

		private static int s_Counter = 0;

		internal static VideoFrame FakeFrame(int width, int height)
		{
			var rv = new VideoFrame();
			s_Counter++;
			rv.frameNumber = s_Counter;

			rv.pixels = new int[0, 0];
			return rv;
		}

		internal static VideoFrame CreateFrame(int width, int height, VideoCameraFrame cameraFrame)
		{
			return InternalCreateFrame(width, height, cameraFrame);
		}

		private static VideoFrame InternalCreateFrame(int width, int height, VideoCameraFrame cameraFrame)
		{
			var rv = new VideoFrame();

			if (cameraFrame.ImageLayout == VideoFrameLayout.Monochrome)
			{
				rv.pixels = new int[height, width];

				rv.pixels = (int[,])cameraFrame.Pixels;
			}
			else if (cameraFrame.ImageLayout == VideoFrameLayout.Color)
			{
				rv.pixels = new int[height, width, 3];

				rv.pixels = (int[, ,])cameraFrame.Pixels;
			}
			else if (cameraFrame.ImageLayout == VideoFrameLayout.BayerRGGB)
			{
				throw new NotSupportedException();
			}
			else
				throw new NotSupportedException();

			rv.previewBitmapBytes = cameraFrame.PreviewBitmapBytes;
			rv.frameNumber = cameraFrame.FrameNumber;
			rv.exposureStartTime = null;
			rv.exposureDuration = null;
			rv.imageInfo = null;

			return rv;
		}

		public object ImageArray
		{
			get
			{
				return pixels;
			}
		}

		public byte[] PreviewBitmap
		{
			get
			{
				return previewBitmapBytes;
			}
		}

		/// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if not supported</exception>
		public long FrameNumber
		{
			get
			{
				if (frameNumber.HasValue)
					return frameNumber.Value;

				return -1;
			}
		}

		/// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if not supported</exception>
		public double ExposureDuration
		{
			[DebuggerStepThrough]
			get
			{
				if (exposureDuration.HasValue)
					return exposureDuration.Value;

				throw new ASCOM.PropertyNotImplementedException("Current camera doesn't support frame timing.");
			}
		}

		/// <exception cref="T:ASCOM.PropertyNotImplementedException">Must throw an exception if not supported</exception>
		public string ExposureStartTime
		{
			[DebuggerStepThrough]
			get
			{
				if (exposureStartTime != null)
					return exposureStartTime;

				throw new ASCOM.PropertyNotImplementedException("Current camera doesn't support frame timing.");
			}
		}

		public string ImageInfo
		{
			get { return imageInfo; }
		}

	}
}

