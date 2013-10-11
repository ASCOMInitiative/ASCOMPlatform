using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator.Config
{
	public partial class ucIntegratingCameraSettings : SettingsPannel
	{
		public ucIntegratingCameraSettings()
		{
			InitializeComponent();
		}

		internal override void LoadSettings()
		{
			nudMinExposureSec.Value = (decimal)Properties.Settings.Default.ExposureMin;
			nudMaxExposureSec.Value = (decimal)Properties.Settings.Default.ExposureMax;
			tbxSupportedExposures.Lines = Properties.Settings.Default.SupportedExposuresList.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries);
		}

		internal override void SaveSettings()
		{
			Properties.Settings.Default.SupportedExposuresList = string.Join("#;", tbxSupportedExposures.Lines);
			Properties.Settings.Default.ExposureMin = (double)nudMinExposureSec.Value;
			Properties.Settings.Default.ExposureMax = (double)nudMaxExposureSec.Value;
		}
	}
}
