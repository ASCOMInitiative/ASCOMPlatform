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
	public partial class ucGamma : SettingsPannel
	{
		public ucGamma()
		{
			InitializeComponent();
		}

		internal override void LoadSettings()
		{
			tbxSupportedGammas.Lines = Properties.Settings.Default.Gammas.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries);
		}

		internal override void SaveSettings()
		{
			Properties.Settings.Default.Gammas = string.Join("#;", tbxSupportedGammas.Lines);
		}
	}
}
