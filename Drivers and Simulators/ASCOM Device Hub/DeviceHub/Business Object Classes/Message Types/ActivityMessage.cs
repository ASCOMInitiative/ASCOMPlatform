namespace ASCOM.DeviceHub
{
	public class ActivityMessage
    {
        public ActivityMessage( DeviceTypeEnum deviceType, ActivityMessageTypes messageType, string messageText )
        {
			DeviceType = deviceType;
            MessageType = messageType;
            MessageText = messageText;
        }

		public DeviceTypeEnum DeviceType { get; private set; }
		public ActivityMessageTypes MessageType { get; private set; }
		public string MessageText { get; private set; }
	}
}
