using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface ITelescopeService : IDeviceService, ITelescopeV4
    {
		void CreateDevice( string id );
    }
}
