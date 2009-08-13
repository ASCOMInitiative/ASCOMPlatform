using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ASCOM;

namespace ASCOM.Controls.Properties
	{
	[SettingsProvider(typeof(ASCOM.SettingsProvider))]
	[ASCOM.DeviceId("ASCOM.Controls")]
	internal sealed partial class Settings
		{
		}
	}
