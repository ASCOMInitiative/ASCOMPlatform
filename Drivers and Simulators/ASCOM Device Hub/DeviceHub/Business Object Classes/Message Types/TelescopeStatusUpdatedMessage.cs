namespace ASCOM.DeviceHub
{
	public class TelescopeStatusUpdatedMessage
    {
		public TelescopeStatusUpdatedMessage( DevHubTelescopeStatus status )
		{
			Status = status;
		}

		public DevHubTelescopeStatus Status { get; private set; }
    }
}
