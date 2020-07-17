using System.Collections.Generic;

namespace ASCOM.DeviceHub
{
	public class DomeSlewAmounts : List<SlewAmount>
	{
		public DomeSlewAmounts()
		{
			Add( new SlewAmount( "1°", 1.0 ) );
			Add( new SlewAmount( "4°", 4.0 ) );
			Add( new SlewAmount( "10°", 10.0 ) );
			Add( new SlewAmount( "40°", 40.0 ) );
			Add( new SlewAmount( "100°", 100.0 ) );
			Add( new SlewAmount( "180°", 180.0 ) );
		}
	}
}
