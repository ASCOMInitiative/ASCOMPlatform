using System;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	public static class Globals
	{
		static Globals()
		{
			RegistryVersion = 6.0;
		}

		public const string DevHubTelescopeID = "ASCOM.DeviceHub.Telescope";
		public const string DevHubDomeID = "ASCOM.DeviceHub.Dome";
		public const string DevHubFocuserID = "ASCOM.DeviceHub.Focuser";

		public const double DegRad = Math.PI / 180.0;
		public const double UTC_SECS_PER_SIDEREAL_SEC = 0.9972695677;

		public const double DEG_TO_RAD = ( 2.0 * Math.PI ) / 360.0;		// 0.017453 radians per degree
		public const double RAD_TO_DEG = 1.0 / DEG_TO_RAD;				// 57.29578 degrees per radian
		public const double HRS_TO_RAD = ( 2.0 * Math.PI ) / 24.0;		// 0.26180 radians per hour
		public const double RAD_TO_HRS = 1.0 / HRS_TO_RAD;				// 3.81972 hours per radian
		public const double HRS_TO_DEG = 15.0;							// 15 degrees per hour

		public static double RegistryVersion { get; set; }

		public static DeviceTypeEnum ActiveDevice { get; set; }

		public static double MainWindowTop { get; set; }
		public static double MainWindowLeft { get; set; }

		public static double ActivityWindowTop { get; set; }
		public static double ActivityWindowLeft { get; set; }
		public static double ActivityWindowHeight { get; set; }
		public static double ActivityWindowWidth { get; set; }

		public static DomeLayoutSettings DomeLayout { get; set; }

		public static bool IsDomeSlaved { get; set; }

		public static bool SuppressTrayBubble { get; set; }
		public static bool UseCustomTheme { get; set; }
		public static double FocuserTemperatureOffset { get; set; }
		public static bool UsePOTHDomeSlaveCalculation { get; set; }
		public static bool UseExpandedScreenLayout { get; set; }
		public static double DomeAzimuthAdjustment { get; set; }

		public static TaskScheduler UISyncContext { get; set; }
	}
}
