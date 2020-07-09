using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
    public interface IDomeService : IDeviceService, IDomeV2
    {
		void CreateDevice( string id );
	}
}
