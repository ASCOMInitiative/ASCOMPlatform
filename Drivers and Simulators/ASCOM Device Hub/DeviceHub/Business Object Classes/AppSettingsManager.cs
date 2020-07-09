namespace ASCOM.DeviceHub
{
	public class AppSettingsManager
	{ 
		static AppSettingsManager()
		{}

		public static void CreateInitialAppSettings()
		{
			IAppSettingsService svc = ServiceContainer.Instance.GetService<IAppSettingsService>();
			ApplicationSettings settings = svc.LoadSettings();

			// Don't initialize the profile if it already exists.

			if ( settings.RegistryVersion == 0.0 )
			{
				settings.MainWindowLocation = new System.Windows.Point( 100.0, 100.0 );
				settings.ActivityWindowLocation = new System.Windows.Point( 100.0, 100.0 );
				settings.ActivityWindowSize = new System.Windows.Point( 480.0, 540.0 );
				settings.RegistryVersion = Globals.RegistryVersion;

				svc.SaveSettings( settings );
			}
		}

		public static void LoadAppSettings()
		{
			ApplicationSettings settings = ServiceContainer.Instance.GetService<IAppSettingsService>().LoadSettings();

			Globals.RegistryVersion = settings.RegistryVersion;
			Globals.ActivityWindowLeft = settings.ActivityWindowLocation.X;
			Globals.ActivityWindowTop = settings.ActivityWindowLocation.Y;
			Globals.ActivityWindowWidth = settings.ActivityWindowSize.X;
			Globals.ActivityWindowHeight = settings.ActivityWindowSize.Y;

			Globals.SuppressTrayBubble = settings.SuppressTrayBubble;
			Globals.UseCustomTheme = settings.UseCustomTheme;
			Globals.UseExpandedScreenLayout = settings.UseExpandedScreenLayout;
		}

		public static void LoadMainWindowSettings()
		{
			// We need to set the main window position before the window is shown.

			ApplicationSettings settings = ServiceContainer.Instance.GetService<IAppSettingsService>().LoadSettings();

			Globals.MainWindowLeft = settings.MainWindowLocation.X;
			Globals.MainWindowTop = settings.MainWindowLocation.Y;
			Globals.UseCustomTheme = settings.UseCustomTheme;
			Globals.UseExpandedScreenLayout = settings.UseExpandedScreenLayout;
		}

		public static void SaveAppSettings()
		{
			ApplicationSettings settings = new ApplicationSettings
			{
				MainWindowLocation = new System.Windows.Point( Globals.MainWindowLeft, Globals.MainWindowTop ),
				ActivityWindowLocation = new System.Windows.Point( Globals.ActivityWindowLeft, Globals.ActivityWindowTop ),
				ActivityWindowSize = new System.Windows.Point( Globals.ActivityWindowWidth, Globals.ActivityWindowHeight ),
				RegistryVersion = Globals.RegistryVersion,
				SuppressTrayBubble = Globals.SuppressTrayBubble,
				UseCustomTheme = Globals.UseCustomTheme,
				UseExpandedScreenLayout = Globals.UseExpandedScreenLayout
			};

			ServiceContainer.Instance.GetService<IAppSettingsService>().SaveSettings( settings );
		}
	}
}
