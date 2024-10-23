//-----------------------------------------------------------------------
// <summary>Defines the Video class.</summary>
//-----------------------------------------------------------------------
// 29-Oct-13  	pwgs    6.1.0 - Created class.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

using System.Collections;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Defines the IVideo Interface. - NOW DEPRECATED, ONLY RETAINED FOR BACKWARD COMPATIBILITY
    /// </summary>
    public class Video : AscomDriver, IVideo, IVideoV2
    {
        private MemberFactory memberFactory;

        #region Video constructors

        /// <summary>
        /// Creates a Video object with the given Prog ID
        /// </summary>
        /// <param name="videoId">ProgID of the Video to be accessed.</param>
        public Video(string videoId)
            : base(videoId)
        {
            memberFactory = base.MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a Video device
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

        /// <summary>
        /// VideoState device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public VideoState VideoState
        {
            get
            {
                // Create a state object to return.
                VideoState videoState = new VideoState(DeviceState, TL);
                TL.LogMessage(nameof(VideoState), $"Returning: " +
                    $"CameraState: '{videoState.CameraState}', " +
                    $"Time stamp: '{videoState.TimeStamp}'");

                // Return the device specific state class
                return videoState;
            }
        }

        #endregion

        #region IVideo Members

        /// <inheritdoc/>
        public string VideoCaptureDeviceName
        {
            get { return (string)memberFactory.CallMember(1, "VideoCaptureDeviceName", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public double ExposureMax
        {
            get { return (double)memberFactory.CallMember(1, "ExposureMax", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public double ExposureMin
        {
            get { return (double)memberFactory.CallMember(1, "ExposureMin", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public VideoCameraFrameRate FrameRate
        {
            get { return (VideoCameraFrameRate)memberFactory.CallMember(1, "FrameRate", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public ArrayList SupportedIntegrationRates
        {
            get { return memberFactory.CallMember(1, "SupportedIntegrationRates", new Type[0], new object[0]).ComObjToArrayList(); }
        }

        /// <inheritdoc/>
		public int IntegrationRate
        {
            get { return (int)memberFactory.CallMember(1, "IntegrationRate", new Type[0], new object[0]); }

            set { memberFactory.CallMember(2, "IntegrationRate", new Type[0], new object[] { value }); }
        }

        /// <inheritdoc/>
		public IVideoFrame LastVideoFrame
        {
            get { return (IVideoFrame)memberFactory.CallMember(1, "LastVideoFrame", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public string SensorName
        {
            get { return (string)memberFactory.CallMember(1, "SensorName", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public SensorType SensorType
        {
            get { return (SensorType)memberFactory.CallMember(1, "SensorType", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public int Width
        {
            get { return (int)memberFactory.CallMember(1, "Width", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public int Height
        {
            get { return (int)memberFactory.CallMember(1, "Height", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public double PixelSizeX
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "PixelSizeX", new Type[0], new object[0])); }
        }

        /// <inheritdoc/>
		public double PixelSizeY
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "PixelSizeY", new Type[0], new object[0])); }
        }

        /// <inheritdoc/>
		public int BitDepth
        {
            get { return (int)memberFactory.CallMember(1, "BitDepth", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public string VideoCodec
        {
            get { return (string)memberFactory.CallMember(1, "VideoCodec", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public string VideoFileFormat
        {
            get { return (string)memberFactory.CallMember(1, "VideoFileFormat", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public int VideoFramesBufferSize
        {
            get { return (int)memberFactory.CallMember(1, "VideoFramesBufferSize", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public string StartRecordingVideoFile(string PreferredFileName)
        {
            return (string)memberFactory.CallMember(3, "StartRecordingVideoFile", new Type[] { typeof(string) }, new object[] { PreferredFileName });
        }

        /// <inheritdoc/>
		public void StopRecordingVideoFile()
        {
            memberFactory.CallMember(3, "StopRecordingVideoFile", new Type[0], new object[0]);
        }

        /// <inheritdoc/>
		public VideoCameraState CameraState
        {
            get { return (VideoCameraState)memberFactory.CallMember(1, "CameraState", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public short GainMax
        {
            get { return (short)memberFactory.CallMember(1, "GainMax", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public short GainMin
        {
            get { return (short)memberFactory.CallMember(1, "GainMin", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public short Gain
        {
            get { return (short)memberFactory.CallMember(1, "Gain", new Type[0], new object[0]); }

            set { memberFactory.CallMember(2, "Gain", new Type[0], new object[] { value }); }
        }

        /// <inheritdoc/>
		public ArrayList Gains
        {
            get { return memberFactory.CallMember(1, "Gains", new Type[0], new object[0]).ComObjToArrayList(); }
        }

        /// <inheritdoc/>
		public short GammaMax
        {
            get { return (short)memberFactory.CallMember(1, "GammaMax", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public short GammaMin
        {
            get { return (short)memberFactory.CallMember(1, "GammaMin", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public short Gamma
        {
            get { return (short)memberFactory.CallMember(1, "Gamma", new Type[0], new object[0]); }

            set { memberFactory.CallMember(2, "Gamma", new Type[0], new object[] { value }); }
        }

        /// <inheritdoc/>
		public ArrayList Gammas
        {
            get { return memberFactory.CallMember(1, "Gammas", new Type[0], new object[0]).ComObjToArrayList(); }
        }

        /// <inheritdoc/>
		public bool CanConfigureDeviceProperties
        {
            get { return (bool)memberFactory.CallMember(1, "CanConfigureDeviceProperties", new Type[0], new object[0]); }
        }

        /// <inheritdoc/>
		public void ConfigureDeviceProperties()
        {
            memberFactory.CallMember(3, "ConfigureDeviceProperties", new Type[0], new object[0]);
        }

        #endregion

    }
}
