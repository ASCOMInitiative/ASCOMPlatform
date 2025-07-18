namespace ASCOM.DeviceHub
{
	public class DomeLayoutSettingsChangedMessage
    {
		public DomeLayoutSettingsChangedMessage( DomeLayoutSettings settings )
		{
			Settings = settings;
		}

		public DomeLayoutSettings Settings { get; private set; }
    }
}
