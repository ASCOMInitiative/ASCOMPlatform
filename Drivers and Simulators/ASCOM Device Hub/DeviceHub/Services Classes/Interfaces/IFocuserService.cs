using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface IFocuserService : IDeviceService, IFocuserV3
	{
		void CreateDevice( string id );
	}
}
