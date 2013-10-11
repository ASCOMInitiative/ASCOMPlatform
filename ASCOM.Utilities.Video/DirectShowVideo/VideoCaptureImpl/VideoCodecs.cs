//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow
//
// Description:	Imlements enumaration and working with the supported Codecs
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 22-Mar-2013	HDP	6.0.0	Initial commit
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl
{
	internal class DsDeviceNameAttribute : Attribute
	{
		public string DeviceName { get; set; }
		public string FourCC { get; set; }

		public DsDeviceNameAttribute(string deviceName, string fourCC)
		{
			DeviceName = deviceName;
			FourCC = fourCC;
		}
	}

	internal enum SupportedCodec
	{
		Uncompressed,

		[DsDeviceName("DV Video Encoder", "DVSD")]
		DV,

		[DsDeviceName("Xvid MPEG-4 Codec", "XVID")]
		XviD,

		[DsDeviceName("Huffyuv v2.1.1", "HFYU")]
		HuffYuv211,

        Unsupported
	}

	internal class SystemCodecEntry
	{
		private SupportedCodec codec = SupportedCodec.Unsupported;
		private DsDeviceNameAttribute codecAttribute = null;

		public SupportedCodec Codec
		{
			get { return codec; }
			set
			{
				if (codec != value)
				{
					codecAttribute = (DsDeviceNameAttribute)typeof(SupportedCodec)
						.GetMember(value.ToString())
					    .Single()
					    .GetCustomAttributes(typeof (DsDeviceNameAttribute), false)
					    .SingleOrDefault();

					codec = value;
				}
			}
		}

		public string DeviceName
		{
			get
			{
				if (codecAttribute != null)
					return codecAttribute.DeviceName;
				else if (codec == SupportedCodec.Uncompressed)
					return VideoCodecs.UNCOMPRESSED_VIDEO;
                else if (codec == SupportedCodec.Unsupported && Device != null)
                    return Device.Name;
				else
					return null;
			}
		}

		public string FourCC
		{
			get
			{
                if (codecAttribute != null)
                    return codecAttribute.FourCC;
                else if (codec == SupportedCodec.Uncompressed)
                    return VideoCodecs.UNCOMPRESSED_VIDEO;
                else if (codec == SupportedCodec.Unsupported && Device != null)
                    return Device.Name;
                else
                    return null;
			}
		}

		public DsDevice Device { get; set; }

		public bool IsInstalled
		{
			get
			{
				return 
					Codec == SupportedCodec.Uncompressed || 
					Device != null;
			}
		}		
	}

	internal static class VideoCodecs
	{
		public static string UNCOMPRESSED_VIDEO = "Uncompressed";

		// From DirectShowNet Forums: http://sourceforge.net/p/directshownet/discussion/460697/thread/edb37d2a/
		// ----------------------------------------------------------------------------------------------------------------
		// The ugly truth is that some people who have written codecs have done a lousy job of it, and they barely work. 
		// Some codecs are complex and need special handling. A generic program to work with all codecs is probably impossible.
		// And if it were possible, it would be so complex that it would be a failure as a sample, since no one would be able
		// to read the code and figure out how it worked (which is what samples are for).
		// ------------------------------------------------------------------------------------------------------------------
		// It also appears that if the application is not built as x86 it will not display a whole bunch of codecs on 64bit machines 
		// http://social.msdn.microsoft.com/Forums/en-US/windowsdirectshowdevelopment/thread/d9029891-25e5-4ed1-ab31-9cae7c6c8eae/
		// There is not much we can do here other than making sure the build is always x86
		public static List<SystemCodecEntry> GetSupportedVideoCodecs()
		{
			var supportedCodecs = new List<SystemCodecEntry>();
			
			var allCodecs = new List<DsDevice>();
			allCodecs.AddRange(DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory));

			foreach (SupportedCodec codec in Enum.GetValues(typeof (SupportedCodec)))
			{
                if (codec == SupportedCodec.Unsupported)
                    continue;

				var entry = new SystemCodecEntry { Codec = codec };

				entry.Device = allCodecs.FirstOrDefault(x => x.Name == entry.DeviceName);

				supportedCodecs.Add(entry);
			}

			return supportedCodecs;
		}

		public static SystemCodecEntry GetSupportedVideoCodec(string codecName)
		{
			List<SystemCodecEntry> supportedCodecs = GetSupportedVideoCodecs();

            SystemCodecEntry rv = supportedCodecs.FirstOrDefault(x => x.DeviceName == codecName && x.IsInstalled);

		    DsDevice unsupportedCodec = DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory).FirstOrDefault(x => x.Name == codecName);

            if (unsupportedCodec != null)
                return new SystemCodecEntry()
                {
                    Device = unsupportedCodec
                };

			if (rv == null)
				rv = supportedCodecs.SingleOrDefault(x => x.Codec == SupportedCodec.Uncompressed);

			return rv;
		}
	}
}
