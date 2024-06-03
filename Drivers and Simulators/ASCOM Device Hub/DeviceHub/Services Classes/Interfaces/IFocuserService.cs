using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface IFocuserService : IDeviceService, IFocuserV4
	{
		void CreateDevice( string id );
	}
}
