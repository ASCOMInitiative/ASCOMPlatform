//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video - Simulator
//
// Description:	Helper class used by the Video Simulator
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities.Video;

namespace ASCOM.Utilities.Video
{
    internal class AviTools
    {
        private NativeHelpers nativeHelpers;

        public AviTools()
        {
            nativeHelpers = new NativeHelpers();
        }

        public  void SetNewGamma(double newGamma)
        {
            nativeHelpers.SetGamma(newGamma);
        }

        public  void SetNewWhiteBalance(int newWhiteBalance)
        {
            nativeHelpers.SetWhiteBalance(255 - newWhiteBalance);
        }

        public  void ApplyGammaBrightness(int[,] pixelsIn, int[,] pixelsOut, int width, int height, short brightness)
        {
            nativeHelpers.ApplyGammaBrightness(width, height, 8, ref pixelsIn, ref pixelsOut, brightness);
        }

        public  void InitFrameIntegration(int width, int height)
        {
            nativeHelpers.InitFrameIntegration(width, height, 8);
        }

        public  void AddIntegrationFrame(int[,] pixelsIn)
        {
            nativeHelpers.AddFrameForIntegration(ref pixelsIn);
        }

        public  int[,] GetResultingIntegratedFrame(int width, int height)
        {
            int[,] rv = new int[height, width];

            nativeHelpers.GetResultingIntegratedFrame(ref rv);

            return rv;
        }

        public  string GetLastAviErrorMessage()
        {
            string error = null;
            IntPtr buffer = IntPtr.Zero;

            try
            {
                byte[] errorMessage = new byte[200];
                buffer = Marshal.AllocHGlobal(errorMessage.Length + 1);
                Marshal.Copy(errorMessage, 0, buffer, errorMessage.Length);
                Marshal.WriteByte(buffer + errorMessage.Length, 0); // terminating null

                nativeHelpers.GetLastAviFileError(buffer);

                error = Marshal.PtrToStringAnsi(buffer);

                if (error != null)
                    error = error.Trim();
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

            return error;
        }

        private  void TraceLastNativeError()
        {
            string error = GetLastAviErrorMessage();

            Trace.WriteLine(error, "VideoNativeException");
        }

        public  void StartNewAviFile(string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog)
        {
            if (nativeHelpers.CreateNewAviFile(fileName, width, height, bpp, fps, showCompressionDialog) != 0)
            {
                TraceLastNativeError();
            }
        }

        public  void AddAviVideoFrame(int[,] pixels)
        {
            if (nativeHelpers.AviFileAddFrame(pixels) != 0)
            {
                TraceLastNativeError();
            }
        }

        public  void CloseAviFile()
        {
            if (nativeHelpers.AviFileClose() != 0)
            {
                TraceLastNativeError();
            }
        }

        public  string GetUsedAviFourCC()
        {
            uint fourcc = nativeHelpers.GetUsedAviCompression();

            if (fourcc == 0)
                return string.Empty;
            else
                return string.Concat(
                    Convert.ToString((char)(fourcc & 0xFF)),
                    Convert.ToString((char)((fourcc >> 8) & 0xFF)),
                    Convert.ToString((char)((fourcc >> 16) & 0xFF)),
                    Convert.ToString((char)((fourcc >> 24) & 0xFF))).ToUpper();
        }
    }
}
