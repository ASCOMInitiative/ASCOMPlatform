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
		private const string _fastUpdateProfileName = "Fast Update Period";
		private static readonly string _fastUpdateDefault = Globals.SCOPE_FAST_UPDATE_MAX.ToString();

		private static string DriverID => Globals.DevHubTelescopeID;

		public static TelescopeSettings FromProfile()
		{
			string telescopeID;
			bool loggerEnabled;
			double fastUpdatePeriod;

			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Telescope";
				telescopeID = profile.GetValue( DriverID, _telescopeIDProfileName, String.Empty, _telescopeIDDefault );
				loggerEnabled = Convert.ToBoolean( profile.GetValue( DriverID, _traceStateProfileName, String.Empty, _traceStateDefault ) );
				fastUpdatePeriod = Convert.ToDouble( profile.GetValue( DriverID, _fastUpdateProfileName, String.Empty, _fastUpdateDefault ) );
			}

			// Prevent the user from circumventing the valid fast update period range by setting the profile directly.

			fastUpdatePeriod = Math.Max( Globals.SCOPE_FAST_UPDATE_MIN, Math.Min( fastUpdatePeriod, Globals.SCOPE_FAST_UPDATE_MAX) );

			TelescopeSettings settings = new TelescopeSettings
			{
				TelescopeID = telescopeID,
				IsLoggingEnabled = loggerEnabled,
				FastUpdatePeriod = fastUpdatePeriod
			};

			return settings;
		}	

		public TelescopeSettings()
		{ }

		public string TelescopeID { get; set; }
		public bool IsLoggingEnabled { get; set; }
		public double FastUpdatePeriod { get; set; }

		public void ToProfile()
		{
			using ( Profile profile = new Profile() )
			{
				profile.DeviceType = "Telescope";
				profile.WriteValue( DriverID, _telescopeIDProfileName, TelescopeID );
				profile.WriteValue( DriverID, _traceStateProfileName, IsLoggingEnabled.ToString() );
				profile.WriteValue( DriverID, _fastUpdateProfileName, FastUpdatePeriod.ToString() );
			}
		}
	}
}
