using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	[ComVisible( false )]
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			DeviceTypeEnum devType = GetDeviceType();

			Server.CountObject( devType );
		}

		~ReferenceCountedObjectBase()
		{
			DeviceTypeEnum devType = GetDeviceType();

			Server.UncountObject( devType );

			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.

			Server.ExitIf();
		}

		private DeviceTypeEnum GetDeviceType()
		{
			DeviceTypeEnum devType = DeviceTypeEnum.Unknown;

			// We increment the global count of objects.

			if ( this is ITelescopeV3 )
			{
				devType = DeviceTypeEnum.Telescope;
			}
			else if ( this is IDomeV2 )
			{
				devType = DeviceTypeEnum.Dome;
			}
			else if ( this is IFocuserV3 )
			{
				devType = DeviceTypeEnum.Focuser;
			}

			return devType;
		}
	}
}
