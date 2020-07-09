namespace ASCOM.DeviceHub
{
	public interface IDeviceService
	{
		bool DeviceCreated { get; }
		bool DeviceAvailable { get; }
		bool Initialized { get; }
	}
}
