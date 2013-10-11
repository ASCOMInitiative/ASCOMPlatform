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
	}
}
