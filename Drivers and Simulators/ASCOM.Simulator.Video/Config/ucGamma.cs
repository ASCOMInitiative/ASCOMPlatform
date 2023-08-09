using System;

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
