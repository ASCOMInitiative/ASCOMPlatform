using System;

using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
	public class TelescopeSettings
	{
		private const string _telescopeIDProfileName = "Telescope ID";
		private const string _telescopeIDDefault = "ASCOM.Simulator.Telescope";
		private const string _traceStateProfileName = "Trace Level";
		private const string _traceStateDefault = "false";

		private static string DriverID => Globals.DevHubTelescopeID;

		public static TelescopeSettings FromProfile()
		{
			string telescopeID;
			bool loggerEnabled;

			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Telescope";
				telescopeID = profile.GetValue( DriverID, _telescopeIDProfileName, string.Empty, _telescopeIDDefault );
				loggerEnabled = Convert.ToBoolean( profile.GetValue( DriverID, _traceStateProfileName, string.Empty, _traceStateDefault ) );
			}

			TelescopeSettings settings = new TelescopeSettings
			{
				TelescopeID = telescopeID,
				IsLoggingEnabled = loggerEnabled
			};

			return settings;
		}	

		public TelescopeSettings()
		{ }

		public string TelescopeID { get; set; }
		public bool IsLoggingEnabled { get; set; }

		public void ToProfile()
		{
			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Telescope";
				profile.WriteValue( DriverID, _telescopeIDProfileName, TelescopeID );
				profile.WriteValue( DriverID, _traceStateProfileName, IsLoggingEnabled.ToString() );
			}
		}
	}
}
