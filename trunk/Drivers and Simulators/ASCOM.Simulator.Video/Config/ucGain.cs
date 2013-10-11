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
	public partial class ucGain : SettingsPannel
	{
		public ucGain()
		{
			InitializeComponent();
		}

		internal override void LoadSettings()
		{
			rbGainRange.Checked = Properties.Settings.Default.SupportsGainRange;
			rbDiscreteGain.Checked = !Properties.Settings.Default.SupportsGainRange;
			nudMinGain.Value = Properties.Settings.Default.GainMin;
			nudMaxGain.Value = Properties.Settings.Default.GainMax;
			tbxSupportedGains.Lines = Properties.Settings.Default.Gains.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries);
		}

		internal override void SaveSettings()
		{
			Properties.Settings.Default.SupportsGainRange = rbGainRange.Checked;
			Properties.Settings.Default.Gains = string.Join("#;", tbxSupportedGains.Lines);
			if (Properties.Settings.Default.SupportsGainRange)
			{
				Properties.Settings.Default.GainMin = (short)nudMinGain.Value;
				Properties.Settings.Default.GainMax = (short)nudMaxGain.Value;
			}
		}
	}
}
