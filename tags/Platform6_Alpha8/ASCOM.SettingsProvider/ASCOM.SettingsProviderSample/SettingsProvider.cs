using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM;
using System.Configuration;

namespace ASCOM.SettingsProviderSample.Properties
	{
	/// <summary>
	/// The purpose of this partial class is merely to decorate the designer-generated
	/// Settings class with the appropriate attributes. It should not contain any executable code.
	/// </summary>
	[SettingsProvider(typeof(ASCOM.SettingsProvider))]
	[DeviceId("ASCOM.SettingsProvider.Sample")]			// This is obviously a fake DeviceID.
	partial class Settings
		{
		}
	}
