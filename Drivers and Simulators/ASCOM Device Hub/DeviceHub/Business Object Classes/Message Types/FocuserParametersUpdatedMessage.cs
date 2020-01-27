namespace ASCOM.DeviceHub
{
	public class FocuserParametersUpdatedMessage
	{
		public FocuserParametersUpdatedMessage( FocuserParameters parameters )
		{
			Parameters = parameters;
		}

		public FocuserParameters Parameters { get; private set; }
	}
}
