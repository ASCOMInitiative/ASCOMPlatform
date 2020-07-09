namespace ASCOM.DeviceHub
{
	public class DomeParametersUpdatedMessage
	{
		public DomeParametersUpdatedMessage( DomeParameters parameters )
		{
			Parameters = parameters;
		}

		public DomeParameters Parameters { get; private set; }
	}
}
