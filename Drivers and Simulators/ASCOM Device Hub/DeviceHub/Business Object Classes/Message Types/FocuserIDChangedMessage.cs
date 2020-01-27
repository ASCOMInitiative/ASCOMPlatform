namespace ASCOM.DeviceHub
{
	public class FocuserIDChangedMessage
	{
		public FocuserIDChangedMessage( string id)
		{
			ID = id;
		}

		public string ID { get; private set; }
	}
}
