namespace ASCOM.DeviceHub
{
	public class DomeSlavedChangedMessage
	{
		public DomeSlavedChangedMessage( bool state )
		{
			State = state;
		}

		public bool State { get; private set; }
	}
}
