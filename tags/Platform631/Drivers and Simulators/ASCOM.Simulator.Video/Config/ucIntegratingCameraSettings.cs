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

		internal void SetVisibility(bool enabled)
		{
			if (enabled)
			{
				lblSuppExps.ForeColor = SystemColors.Window;
				lblMinExp.ForeColor = SystemColors.Window;
				lblMaxExp.ForeColor = SystemColors.Window;
				nudMinExposureSec.Enabled = true;
				nudMaxExposureSec.Enabled = true;
				tbxSupportedExposures.Enabled = true;
			}
			else
			{
				lblSuppExps.ForeColor = SystemColors.ControlDark;
				lblMinExp.ForeColor = SystemColors.ControlDark;
				lblMaxExp.ForeColor = SystemColors.ControlDark;
				nudMinExposureSec.Enabled = false;
				nudMaxExposureSec.Enabled = false;
				tbxSupportedExposures.Enabled = false;
			}
		}
	}
}
