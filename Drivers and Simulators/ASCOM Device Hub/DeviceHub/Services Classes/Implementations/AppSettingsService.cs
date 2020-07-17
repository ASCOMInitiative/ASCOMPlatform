namespace ASCOM.DeviceHub
{
	public class AppSettingsService : IAppSettingsService
	{
		public static IAppSettingsService Instance { get; private set; }

		static AppSettingsService()
		{
			Instance = new AppSettingsService();
		}

		public ApplicationSettings LoadSettings()
		{
			ApplicationSettings settings = ApplicationSettings.FromXmlFile();

			return settings;
		}

		public void SaveSettings( ApplicationSettings settings )
		{
			settings.ToXmlFile();
		}
	}
}
