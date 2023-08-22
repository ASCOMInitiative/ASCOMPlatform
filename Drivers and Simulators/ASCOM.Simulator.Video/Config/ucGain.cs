﻿using System;
using System.Drawing;

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

		private void SetGainRangeVisibility(bool enabled)
		{
			if (enabled)
			{
				lblMinGain.ForeColor = SystemColors.Window;
				lblMaxGain.ForeColor = SystemColors.Window;
				nudMinGain.Enabled = true;
				nudMaxGain.Enabled = true;
				pnlDiscreteGain.Enabled = false;
			}
			else
			{
				lblMinGain.ForeColor = SystemColors.ControlDark;
				lblMaxGain.ForeColor = SystemColors.ControlDark;
				nudMinGain.Enabled = false;
				nudMaxGain.Enabled = false;
				pnlDiscreteGain.Enabled = true;
			}
		}

		private void rbGainRange_CheckedChanged(object sender, EventArgs e)
		{
			SetGainRangeVisibility(rbGainRange.Checked);
		}
	}
}
