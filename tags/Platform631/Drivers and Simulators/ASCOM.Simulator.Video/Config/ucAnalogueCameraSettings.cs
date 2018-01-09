using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Simulator.Properties;

namespace ASCOM.Simulator.Config
{
	public partial class ucAnalogueCameraSettings : SettingsPannel
	{
		public ucAnalogueCameraSettings()
		{
			InitializeComponent();
		}

		internal override void LoadSettings()
		{
			tbxVideoCaptureDeviceName.Text = Properties.Settings.Default.VideoCaptureDeviceName;

			rbVideoFrameRatePAL.Checked = Properties.Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.PAL;
			rbVideoFrameRateNTSC.Checked = Properties.Settings.Default.CameraFrameRate == AnalogueCameraFrameRate.NTSC;
		}

		internal override void SaveSettings()
		{
			Properties.Settings.Default.VideoCaptureDeviceName = tbxVideoCaptureDeviceName.Text;

			Properties.Settings.Default.CameraFrameRate = rbVideoFrameRatePAL.Checked
				? AnalogueCameraFrameRate.PAL
				: AnalogueCameraFrameRate.NTSC;
		}

		internal void SetVisibility(bool enabled)
		{
			if (enabled)
			{
				gpxFrameRate.ForeColor = SystemColors.Window;
				rbVideoFrameRatePAL.ForeColor = SystemColors.Window;
				rbVideoFrameRateNTSC.ForeColor = SystemColors.Window;
				rbVideoFrameRatePAL.Enabled = true;
				rbVideoFrameRateNTSC.Enabled = true;
				lblDeviceName.ForeColor = SystemColors.Window;
				tbxVideoCaptureDeviceName.Enabled = true;
			}
			else
			{
				gpxFrameRate.ForeColor = SystemColors.ControlDark;
				rbVideoFrameRatePAL.ForeColor = SystemColors.ControlDark;
				rbVideoFrameRateNTSC.ForeColor = SystemColors.ControlDark;
				rbVideoFrameRatePAL.Enabled = false;
				rbVideoFrameRateNTSC.Enabled = false;
				lblDeviceName.ForeColor = SystemColors.ControlDark;
				tbxVideoCaptureDeviceName.Enabled = false;
			}
		}
	}
}
