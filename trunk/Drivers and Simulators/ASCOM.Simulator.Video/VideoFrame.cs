//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This file implements the IVideoFrame interface for the Video Simulator
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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using Simulator.VideoCameraImpl;

namespace ASCOM.Simulator
{
	public class VideoFrame : ASCOM.DeviceInterface.IVideoFrame
	{
		private long? frameNumber;
		private string imageInfo;
		private double? exposureDuration;
		private string exposureStartTime;
		private int[,] pixels;
		private object[,] pixelsVariant;
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

		internal static VideoFrame CreateFrameVariant(int width, int height, VideoCameraFrame cameraFrame)
		{
			return InternalCreateFrame(width, height, cameraFrame, true);
		}

		internal static VideoFrame CreateFrame(int width, int height, VideoCameraFrame cameraFrame)
		{
			return InternalCreateFrame(width, height, cameraFrame, false);
		}

		private static VideoFrame InternalCreateFrame(int width, int height, VideoCameraFrame cameraFrame, bool variant)
		{
			var rv = new VideoFrame();

			if (variant)
			{
				rv.pixelsVariant = new object[height, width];
				rv.pixels = null;
			}
			else
			{
				rv.pixels = new int[height, width];
				rv.pixelsVariant = null;
			}

			rv.previewBitmapBytes = cameraFrame.PreviewBitmapBytes;

			if (variant)
				Array.Copy(cameraFrame.Pixels, rv.pixelsVariant, cameraFrame.Pixels.Length);
			else
				rv.pixels = cameraFrame.Pixels;

			rv.frameNumber = cameraFrame.FrameNumber;
			rv.exposureStartTime = cameraFrame.ExposureStartTime != null
				? cameraFrame.ExposureStartTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff")
				: null;
			rv.exposureDuration = cameraFrame.ExposureDuration;
			rv.imageInfo = cameraFrame.ImageInfo;

			return rv;
		}

		public object ImageArray
		{
			get
			{
				Array rv = Array.CreateInstance(typeof(int), pixels.GetLength(0), pixels.GetLength(1));
				Array.Copy(pixels, rv, pixels.Length);

				return rv;
			}
		}

		public object ImageArrayVariant
		{
			get
			{
				Array rv = Array.CreateInstance(typeof(object), pixelsVariant.GetLength(0), pixelsVariant.GetLength(1));
				Array.Copy(pixelsVariant, rv, pixelsVariant.Length);

				return rv;
			}
		}

		public byte[] PreviewBitmap
		{
			get { return previewBitmapBytes; }
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
			get
			{
				if (exposureStartTime != null)
					return exposureStartTime;

				throw new ASCOM.PropertyNotImplementedException("Current camera doesn't support frame timing.");
			}
		}

		public ArrayList ImageMetadata
		{
			get { return new ArrayList(); }
		}

	}
}
