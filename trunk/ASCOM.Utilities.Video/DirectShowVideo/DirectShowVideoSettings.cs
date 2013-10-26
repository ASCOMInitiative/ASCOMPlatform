using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities.Video.DirectShowVideo;
using ASCOM.Utilities.Video;

namespace ASCOM.Utilities.Video.DirectShowVideo
{
	public class DirectShowVideoSettings : System.Configuration.ApplicationSettingsBase
	{
        private static DirectShowVideoSettings defaultInstance = ((DirectShowVideoSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DirectShowVideoSettings())));

        public static DirectShowVideoSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        static DirectShowVideoSettings()
        {
            defaultInstance = (DirectShowVideoSettings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new DirectShowVideoSettings());
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("")]
		public string PreferredCaptureDevice
		{
			get
			{
				return ((string)(this["PreferredCaptureDevice"]));
			}
			set
			{
				this["PreferredCaptureDevice"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("Uncompressed")]
		public string PreferredCompressorDevice
		{
			get
			{
				return ((string)(this["PreferredCompressorDevice"]));
			}
			set
			{
				this["PreferredCompressorDevice"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("Color")]
		public global::ASCOM.Utilities.Video.DirectShowVideo.VideoFrameLayout SimulatedImageLayout
		{
			get
			{
				return ((global::ASCOM.Utilities.Video.DirectShowVideo.VideoFrameLayout)(this["SimulatedImageLayout"]));
			}
			set
			{
				this["SimulatedImageLayout"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("R")]
		public global::ASCOM.Utilities.Video.DirectShowVideo.LumaConversionMode LumaConversionMode
		{
			get
			{
				return ((global::ASCOM.Utilities.Video.DirectShowVideo.LumaConversionMode)(this["LumaConversionMode"]));
			}
			set
			{
				this["LumaConversionMode"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("False")]
		public bool UsesTunerCrossbar
		{
			get
			{
				return ((bool)(this["UsesTunerCrossbar"]));
			}
			set
			{
				this["UsesTunerCrossbar"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("0")]
		public int CrossbarOutputPin
		{
			get
			{
				return ((int)(this["CrossbarOutputPin"]));
			}
			set
			{
				this["CrossbarOutputPin"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("0")]
		public int CrossbarInputPin
		{
			get
			{
				return ((int)(this["CrossbarInputPin"]));
			}
			set
			{
				this["CrossbarInputPin"] = value;
			}
		}

		[global::System.Configuration.UserScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.DefaultSettingValueAttribute("")]
		public string SelectedVideoFormat
		{
			get
			{
				return ((string)(this["SelectedVideoFormat"]));
			}
			set
			{
				this["SelectedVideoFormat"] = value;
			}
		}
	}
}
