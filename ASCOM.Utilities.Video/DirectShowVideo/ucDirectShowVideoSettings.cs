using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities.Video.DirectShowVideo;
using ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo
{
	public partial class ucDirectShowVideoSettings : UserControl
	{
		IBaseFilter theDevice = null;
		IBaseFilter theCompressor = null;

		private DirectShowVideoSettings settings;
		private CrossbarHelper crossbarHelper;

		public ucDirectShowVideoSettings()
		{
			InitializeComponent();
		}

		public void Initialize(DirectShowVideoSettings settings)
		{
			this.settings = settings;
	
			crossbarHelper = new CrossbarHelper(settings);

			crossbarHelper.UpdateNoCrossbarSettings(cbxCrossbarInput);

			LoadSettingsInternal();
		}

		private void LoadSettingsInternal()
		{
			cbxCaptureDevices.Items.Clear();
			foreach (DsDevice ds in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
			{
				cbxCaptureDevices.Items.Add(ds.Name);
			}

			List<SystemCodecEntry> systemCodecs = VideoCodecs.GetSupportedVideoCodecs();
			foreach (SystemCodecEntry codec in systemCodecs)
			{
				RadioButton rbCodec = gbxCompression
					.Controls
					.Cast<Control>()
					.SingleOrDefault(x => x is RadioButton && string.Equals(x.Text, codec.DeviceName.ToString())) as RadioButton;

				if (rbCodec != null)
				{
					rbCodec.Enabled = codec.DeviceName != null && codec.IsInstalled;
					rbCodec.Tag = codec;
					rbCodec.Checked = codec.DeviceName == settings.PreferredCompressorDevice;					
				}
			}

			cbxOtherCodecs.Items.Clear();
			foreach (DsDevice ds in DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory).Where(x => systemCodecs.All(y => y.DeviceName != x.Name)))
			{
				cbxOtherCodecs.Items.Add(ds.Name);
				if (ds.Name == settings.PreferredCompressorDevice)
				{
					cbxOtherCodecs.SelectedItem = ds.Name;
					rbCompressionUnsupported.Checked = true;
				}
			}

			if (cbxCaptureDevices.Items.Count > 0)
			{
				if (cbxCaptureDevices.Items.Contains(settings.PreferredCaptureDevice))
					cbxCaptureDevices.SelectedIndex = cbxCaptureDevices.Items.IndexOf(settings.PreferredCaptureDevice);
				else
					cbxCaptureDevices.SelectedIndex = 0;
			}

			cbxSensorType.Items.Clear();
			cbxSensorType.Items.Add(VideoFrameLayout.Monochrome);
			cbxSensorType.Items.Add(VideoFrameLayout.Color);
			//cbxSensorType.Items.Add(VideoFrameLayout.BayerRGGB);
			cbxSensorType.SelectedItem = settings.SimulatedImageLayout;

			cbxMonochromePixelsFrom.Items.Clear();
			cbxMonochromePixelsFrom.Items.Add(LumaConversionMode.R);
			cbxMonochromePixelsFrom.Items.Add(LumaConversionMode.G);
			cbxMonochromePixelsFrom.Items.Add(LumaConversionMode.B);
			cbxMonochromePixelsFrom.Items.Add(LumaConversionMode.GrayScale);
			cbxMonochromePixelsFrom.SelectedItem = settings.LumaConversionMode;
		}

		private bool SaveSettingsInternal()
		{
			if (cbxCaptureDevices.SelectedIndex == -1)
				settings.PreferredCaptureDevice = string.Empty;
			else
				settings.PreferredCaptureDevice = (string)cbxCaptureDevices.SelectedItem;

			if (rbCompressionUnsupported.Checked && cbxOtherCodecs.SelectedIndex != -1)
				settings.PreferredCompressorDevice = (string)cbxOtherCodecs.SelectedItem;
			else
			{
				RadioButton rbCodec = gbxCompression
									.Controls
									.Cast<Control>()
									.SingleOrDefault(x => x is RadioButton && ((RadioButton)x).Checked) as RadioButton;

				if (rbCodec != null && rbCodec.Tag is SystemCodecEntry)
					settings.PreferredCompressorDevice = ((SystemCodecEntry)rbCodec.Tag).DeviceName;
				else
					settings.PreferredCompressorDevice = VideoCodecs.UNCOMPRESSED_VIDEO;
			}

			settings.SimulatedImageLayout = (VideoFrameLayout)cbxSensorType.SelectedItem;
			settings.LumaConversionMode = (LumaConversionMode)cbxMonochromePixelsFrom.SelectedItem;

			return true;
		}

		public DialogResult SaveSettings()
		{
			if (rbCompressionUnsupported.Checked && cbxOtherCodecs.SelectedIndex == -1)
			{
				MessageBox.Show("Please select a video codec to use.", "ASCOM Video Capture");
				cbxOtherCodecs.Focus();
				return DialogResult.Cancel;
			}

			if (SaveSettingsInternal())
			{
				settings.Save();
				return DialogResult.OK;
			}

			return DialogResult.None;
		}

		/// <summary>
		/// Enumerates all filters of the selected category and returns the IBaseFilter for the 
		/// filter described in friendlyname
		/// </summary>
		/// <param name="category">Category of the filter</param>
		/// <param name="friendlyname">Friendly name of the filter</param>
		/// <returns>IBaseFilter for the device</returns>
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

		private void btnInputPros_Click(object sender, EventArgs e)
		{
			DirectShowCapture.DisplayPropertyPage(theDevice, this.Handle);
		}

		private void cbxCaptureDevices_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Release COM objects
			if (theDevice != null)
			{
				Marshal.ReleaseComObject(theDevice);
				theDevice = null;
			}

			if (cbxCaptureDevices.SelectedIndex != -1)
			{
				//Create the filter for the selected video input device
				string deviceName = cbxCaptureDevices.SelectedItem.ToString();
				theDevice = CreateFilter(FilterCategory.VideoInputDevice, deviceName);

				if (!string.IsNullOrEmpty(deviceName))
				{
					Cursor = Cursors.WaitCursor;
					Update();

					cbxCrossbarInput.Items.Clear();
					cbxCrossbarInput.SelectedIndexChanged -= new EventHandler(cbxCrossbarInput_SelectedIndexChanged);
					try
					{
						crossbarHelper.LoadCrossbarSources(deviceName, cbxCrossbarInput);
						// TODO: Save the settings
					}
					finally
					{
						cbxCrossbarInput.SelectedIndexChanged += new EventHandler(cbxCrossbarInput_SelectedIndexChanged);
						Cursor = Cursors.Default;
					}

					cbxVideoFormats.Items.Clear();
					cbxVideoFormats.SelectedIndexChanged -= new EventHandler(cbxVideoFormats_SelectedIndexChanged);
					try
					{
						VideoFormatHelper.LoadSupportedVideoFormats(deviceName, cbxVideoFormats);
					}
					finally
					{
						cbxVideoFormats.SelectedIndexChanged += new EventHandler(cbxVideoFormats_SelectedIndexChanged);
						Cursor = Cursors.Default;
					}

					VideoFormatHelper.SupportedVideoFormat selectedVideoFormat = null;
					foreach (VideoFormatHelper.SupportedVideoFormat format in cbxVideoFormats.Items)
					{
						if (settings.SelectedVideoFormat == format.AsSerialized())
						{
							selectedVideoFormat = format;
							break;
						}
					}

					if (selectedVideoFormat != null)
						cbxVideoFormats.SelectedItem = selectedVideoFormat;
					else
						cbxVideoFormats.SelectedIndex = 0;
				}
			}
		}

		private void SelectedCodecChanged(object sender, EventArgs e)
		{
			RadioButton rbSender = sender as RadioButton;
			if (rbSender != null && rbSender.Checked)
			{
				SystemCodecEntry selectedCodec = ((SystemCodecEntry)(sender as RadioButton).Tag);
				if (selectedCodec != null)
				{
					theCompressor = selectedCodec.Device != null
						? CreateFilter(FilterCategory.VideoCompressorCategory, selectedCodec.Device.Name)
						: null;
				}
			}
		}

		private void btnCompressorProps_Click(object sender, EventArgs e)
		{
			if (theCompressor != null)
				DirectShowCapture.DisplayPropertyPage(theCompressor, this.Handle);
		}

		private void cbSensorType_SelectedIndexChanged(object sender, EventArgs e)
		{
			pnlBWPixels.Visible = cbxSensorType.SelectedIndex == 0;
		}

		private void cbxOtherCodecs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbxOtherCodecs.SelectedIndex != -1)
			{
				theCompressor = CreateFilter(FilterCategory.VideoCompressorCategory, (string)cbxOtherCodecs.SelectedItem);

				rbCompressionUncompressed.Checked = false;
				rbCompressionDV.Checked = false;
				rbCompressionXviD.Checked = false;
				rbCompressionHuffyuv211.Checked = false;
				rbCompressionUnsupported.Checked = true;
			}
		}

		private void cbxCrossbarInput_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void rbCompressionUnsupported_CheckedChanged(object sender, EventArgs e)
		{
			cbxOtherCodecs.Enabled = rbCompressionUnsupported.Checked;
		}

		private void ucDirectShowVideoSettings_Load(object sender, EventArgs e)
		{
			btnInputPros.Enabled = true;
			btnCompressorProps.Enabled = true;
		}

		private void cbxVideoFormats_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedFormat = (VideoFormatHelper.SupportedVideoFormat)cbxVideoFormats.SelectedItem;
			settings.SelectedVideoFormat = selectedFormat.AsSerialized();
		}

	}
}
