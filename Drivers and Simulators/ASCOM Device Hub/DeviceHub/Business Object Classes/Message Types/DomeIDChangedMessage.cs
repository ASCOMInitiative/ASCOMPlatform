namespace ASCOM.DeviceHub
{
	public class DomeIDChangedMessage
    {
		public DomeIDChangedMessage( string id )
		{
			ID = id;
		}

		public string ID { get; private set; }
	}
}
