namespace ASCOM.DeviceHub
{
    internal class SignalWaitMessage
	{
		public SignalWaitMessage( bool wait )
		{
			Wait = wait;
		}

		public bool Wait { get; private set; }
	}
}
