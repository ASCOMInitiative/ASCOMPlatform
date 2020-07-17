namespace ASCOM.DeviceHub
{
	public class TelescopeIDChangedMessage
    {
		public TelescopeIDChangedMessage( string id )
		{
			ID = id;
		}

		public string ID { get; private set; }
	}
}
