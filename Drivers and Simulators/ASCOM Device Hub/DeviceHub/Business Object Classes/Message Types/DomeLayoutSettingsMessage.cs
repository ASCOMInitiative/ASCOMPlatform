namespace ASCOM.DeviceHub
{
	public class DomeLayoutSettingsMessage
    {
		public DomeLayoutSettingsMessage( DomeLayoutSettings settings )
		{
			Settings = settings;
		}

		public DomeLayoutSettings Settings { get; private set; }
    }
}
