using System;
using System.Globalization;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
	public class FocuserSettings
	{
		private const string _focuserIDProfileName = "Focuser ID";
		private const string _focuserIDDefault = "ASCOM.Simulator.Focuser";
		private const string _temperatureOffsetProfileName = "Temperature Offset";
		private const string _temperatureOffsetDefault = "0.0";
		private const string _traceStateProfileName = "Trace Level";
		private const string _traceStateDefault = "false";

		private static string DriverID => Globals.DevHubFocuserID;

		public static FocuserSettings FromProfile()
		{
			string focuserID;
			double temperatureOffset;
			bool loggerEnabled;

			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Focuser";
				focuserID = profile.GetValue( DriverID, _focuserIDProfileName, String.Empty, _focuserIDDefault );
				temperatureOffset = Convert.ToDouble( profile.GetValue( DriverID, _temperatureOffsetProfileName, String.Empty, _temperatureOffsetDefault ), CultureInfo.InvariantCulture );

				loggerEnabled = Convert.ToBoolean( profile.GetValue( DriverID, _traceStateProfileName, String.Empty, _traceStateDefault ) );
			}

			FocuserSettings settings = new FocuserSettings
			{
				FocuserID = focuserID,
				TemperatureOffset = temperatureOffset,
				IsLoggingEnabled = loggerEnabled
			};

			return settings;
		}

		public FocuserSettings()
		{ }

		public string FocuserID { get; set; }
		public double TemperatureOffset { get; set; }
		public bool IsLoggingEnabled { get; set; }

		public void ToProfile()
		{
			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Focuser";
				profile.WriteValue( DriverID, _focuserIDProfileName, FocuserID );
				profile.WriteValue( DriverID, _temperatureOffsetProfileName, TemperatureOffset.ToString(CultureInfo.InvariantCulture) );
				profile.WriteValue( DriverID, _traceStateProfileName, IsLoggingEnabled.ToString() );
			}
		}
	}
}
