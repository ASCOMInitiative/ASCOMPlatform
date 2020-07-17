namespace ASCOM.DeviceHub
{
	public interface IAppSettingsService
	{
		ApplicationSettings LoadSettings();
		void SaveSettings( ApplicationSettings settings );
	}
}
