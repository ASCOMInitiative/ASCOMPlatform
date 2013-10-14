//-----------------------------------------------------------------------
// <summary>Defines the Video class.</summary>
//-----------------------------------------------------------------------
// 29-Oct-13  	pwgs    6.1.0 - Created class.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DriverAccess
{
    #region Video wrapper
    /// <summary>
    /// Provides universal access to Video drivers
    /// </summary>
    public class Video : AscomDriver, IVideo
    {
        //private Type comType;
        //private object comObject;

        #region Video constructors
        private MemberFactory memberFactory;

        /// <summary>
        /// Creates a Video object with the given Prog ID
        /// </summary>
        /// <param name="videoId">ProgID of the Video to be accessed.</param>
        public Video(string videoId)
            : base(videoId)
        {
            memberFactory = base.MemberFactory;
            //comType = Type.GetTypeFromProgID(videoId);
            //comObject = Activator.CreateInstance(comType);
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Video device
        /// </summary>
        /// <param name="videoId">Video Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Video or null for none</returns>
        public static string Choose(string videoId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Video";
                return chooser.Choose(videoId);
            }
        }

        #endregion

        #region IVideo Members
        /// <summary>
        /// 
        /// </summary>
        public string VideoCaptureDeviceName
        {
            get { return (string)memberFactory.CallMember(1, "VideoCaptureDeviceName", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToString(comType.InvokeMember("VideoCaptureDeviceName", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public double ExposureMax
        {
            get { return (double)memberFactory.CallMember(1, "ExposureMax", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToDouble(comType.InvokeMember("ExposureMax", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public double ExposureMin
        {
            get { return (double)memberFactory.CallMember(1, "ExposureMin", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToDouble(comType.InvokeMember("ExposureMin", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public VideoCameraFrameRate FrameRate
        {
            get { return (VideoCameraFrameRate)memberFactory.CallMember(1, "FrameRate", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(VideoCameraFrameRate)Convert.ToInt32(comType.InvokeMember("FrameRate", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList SupportedIntegrationRates
        {
            get { return (ArrayList)memberFactory.CallMember(1, "SupportedIntegrationRates", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(ArrayList)comType.InvokeMember("SupportedIntegrationRates", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int IntegrationRate
        {
            get { return (int)memberFactory.CallMember(1, "IntegrationRate", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("IntegrationRate", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
            set { memberFactory.CallMember(2, "IntegrationRate", new Type[0], new object[] { value }); }
            //{
            //TargetInvocationShield(() =>
            //comType.InvokeMember("IntegrationRate", BindingFlags.SetProperty, (Binder)null, comObject, new object[] { value }, CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public IVideoFrame LastVideoFrame
        {
            get { return (IVideoFrame)memberFactory.CallMember(1, "LastVideoFrame", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(IVideoFrame)comType.InvokeMember("LastVideoFrame", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public string SensorName
        {
            get { return (string)memberFactory.CallMember(1, "SensorName", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToString(comType.InvokeMember("SensorName", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public SensorType SensorType
        {
            get { return (SensorType)memberFactory.CallMember(1, "SensorType", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(SensorType)Convert.ToInt32(comType.InvokeMember("SensorType", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int CameraXSize
        {
            get { return (int)memberFactory.CallMember(1, "CameraXSize", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("CameraXSize", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int CameraYSize
        {
            get { return (int)memberFactory.CallMember(1, "CameraYSize", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("CameraYSize", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return (int)memberFactory.CallMember(1, "Width", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("Width", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return (int)memberFactory.CallMember(1, "Height", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("Height", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public double PixelSizeX
        {
            get { return (int)memberFactory.CallMember(1, "PixelSizeX", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToDouble(comType.InvokeMember("PixelSizeX", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public double PixelSizeY
        {
            get { return (int)memberFactory.CallMember(1, "PixelSizeY", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToDouble(comType.InvokeMember("PixelSizeY", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int BitDepth
        {
            get { return (int)memberFactory.CallMember(1, "BitDepth", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("BitDepth", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public string VideoCodec
        {
            get { return (string)memberFactory.CallMember(1, "VideoCodec", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToString(comType.InvokeMember("VideoCodec", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public string VideoFileFormat
        {
            get { return (string)memberFactory.CallMember(1, "VideoFileFormat", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToString(comType.InvokeMember("VideoFileFormat", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int VideoFramesBufferSize
        {
            get { return (int)memberFactory.CallMember(1, "VideoFramesBufferSize", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("VideoFramesBufferSize", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public string StartRecordingVideoFile(string PreferredFileName)
        {
            return (string)memberFactory.CallMember(3, "StartRecordingVideoFile", new Type[] { typeof(string) }, new object[] { PreferredFileName });
            //return TargetInvocationShield(() =>
            //Convert.ToString(comType.InvokeMember("StartRecordingVideoFile", BindingFlags.InvokeMethod, (Binder)null, comObject, new object[] { PreferredFileName }, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopRecordingVideoFile()
        {
            memberFactory.CallMember(3, "StopRecordingVideoFile", new Type[0], new object[0]);
            //TargetInvocationShield(() =>
            //comType.InvokeMember("StopRecordingVideoFile", BindingFlags.InvokeMethod, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 
        /// </summary>
        public VideoCameraState CameraState
        {
            get { return (VideoCameraState)memberFactory.CallMember(1, "CameraState", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(VideoCameraState)Convert.ToInt32(comType.InvokeMember("CameraState", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short GainMax
        {
            get { return (short)memberFactory.CallMember(1, "GainMax", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt16(comType.InvokeMember("GainMax", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short GainMin
        {
            get { return (short)memberFactory.CallMember(1, "GainMin", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt16(comType.InvokeMember("GainMin", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short Gain
        {
            get { return (short)memberFactory.CallMember(1, "Gain", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt16(comType.InvokeMember("Gain", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
            set { memberFactory.CallMember(2, "Gain", new Type[0], new object[] { value }); }
            //{
            //TargetInvocationShield(() =>
            //comType.InvokeMember("Gain", BindingFlags.SetProperty, (Binder)null, comObject, new object[] { value }, CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList Gains
        {
            get { return (ArrayList)memberFactory.CallMember(1, "Gains", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(ArrayList)(comType.InvokeMember("Gains", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short GammaMax
        {
            get { return (short)memberFactory.CallMember(1, "GammaMax", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt16(comType.InvokeMember("GammaMax", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short GammaMin
        {
            get { return (short)memberFactory.CallMember(1, "GammaMin", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt16(comType.InvokeMember("GammaMin", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public short Gamma
        {
            get { return (short)memberFactory.CallMember(1, "Gamma", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToInt32(comType.InvokeMember("Gamma", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
            set { memberFactory.CallMember(2, "Gamma", new Type[0], new object[] { value }); }
            //{
            //TargetInvocationShield(() =>
            //comType.InvokeMember("Gamma", BindingFlags.SetProperty, (Binder)null, comObject, new object[] { value }, CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList Gammas
        {
            get { return (ArrayList)memberFactory.CallMember(1, "Gammas", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //(ArrayList)comType.InvokeMember("Gammas", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanConfigureDeviceProperties
        {
            get { return (bool)memberFactory.CallMember(1, "CanConfigureDeviceProperties", new Type[0], new object[0]); }
            //{
            //return TargetInvocationShield(() =>
            //Convert.ToBoolean(comType.InvokeMember("CanConfigureDeviceProperties", BindingFlags.GetProperty, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture)));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConfigureDeviceProperties()
        {
            memberFactory.CallMember(3, "ConfigureDeviceProperties", new Type[0], new object[0]);
            //TargetInvocationShield(() =>
            //comType.InvokeMember("ConfigureDeviceProperties", BindingFlags.InvokeMethod, (Binder)null, comObject, new object[0], CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 
        /// </summary>
        private TReturnType TargetInvocationShield<TReturnType>(Func<TReturnType> func)
        {
            try
            {
                return func();
            }
            catch (TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TargetInvocationShield(Action action)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException tex)
            {
                throw tex.InnerException;
            }
        }

        #endregion

    }
    #endregion
}
