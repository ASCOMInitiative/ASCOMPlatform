namespace ASCOM.DeviceHub
{
	public class DomeStatusUpdatedMessage
	{
		public DomeStatusUpdatedMessage( DevHubDomeStatus status )
		{
			Status = status;
		}

		public DevHubDomeStatus Status { get; private set; }
    }
}
