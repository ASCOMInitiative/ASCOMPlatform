namespace ASCOM.DeviceHub
{
	public class DeviceDisconnectedMessage
	{
		public DeviceDisconnectedMessage(DeviceTypeEnum deviceType )
		{
			DeviceType = deviceType;
		}

		public DeviceTypeEnum DeviceType { get; private set; }
	}
}
