using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
    public interface IDomeService : IDeviceService, IDomeV3
    {
		void CreateDevice( string id );
	}
}
