namespace ASCOM.DeviceHub
{
	public class FocuserStatusUpdatedMessage
	{
		public FocuserStatusUpdatedMessage( DevHubFocuserStatus status )
		{
			Status = status;
		}

		public DevHubFocuserStatus Status { get; private set; }
	}
}
