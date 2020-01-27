using System;

using ASCOM.DeviceHub;

namespace Unit_Tests
{
	public class MockAppSettingsService : IAppSettingsService
	{
		public double MockTemperatureOffset { get; set; }

		public ApplicationSettings LoadSettings()
		{
			throw new NotImplementedException();
		}

		public void SaveSettings( ApplicationSettings settings )
		{
			MockTemperatureOffset = Globals.FocuserTemperatureOffset;
		}
	}
}
