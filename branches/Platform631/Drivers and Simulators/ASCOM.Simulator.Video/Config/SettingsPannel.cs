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
	public class SettingsPannel : UserControl
	{
		protected ISettingsPagesManager settingsManager;

		public SettingsPannel()
		{ }

		public SettingsPannel(ISettingsPagesManager settingsManager)
		{
			this.settingsManager = settingsManager;
		}

		internal virtual void LoadSettings()
		{ }

		internal virtual void SaveSettings()
		{ }

		internal virtual bool ValidateSettings()
		{
			return true;
		}
	}
}
