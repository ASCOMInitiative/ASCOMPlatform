using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public interface ITelescopeService : IDeviceService, ITelescopeV3
    {
		void CreateDevice( string id );
    }
}
