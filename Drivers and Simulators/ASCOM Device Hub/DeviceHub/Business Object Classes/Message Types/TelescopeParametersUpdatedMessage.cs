namespace ASCOM.DeviceHub
{
	public class TelescopeParametersUpdatedMessage
	{
		public TelescopeParametersUpdatedMessage( TelescopeParameters parameters )
		{
			Parameters = parameters;
		}

		public TelescopeParameters Parameters { get; private set; }
	}
}
