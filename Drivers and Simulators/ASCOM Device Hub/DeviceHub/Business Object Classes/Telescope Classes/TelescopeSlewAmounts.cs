using System.Collections.Generic;

namespace ASCOM.DeviceHub
{
	public class TelescopeSlewAmounts : List<SlewAmount>
    {
		public TelescopeSlewAmounts()
		{
			Add( new SlewAmount( "1'", 1/60.0 ) );
			Add( new SlewAmount( "4'", 4/60.0 ) );
			Add( new SlewAmount( "15'", 15/60.0 ) );
			Add( new SlewAmount( "1°", 1.0 ) );
			Add( new SlewAmount( "4°", 4.0 ) );
			Add( new SlewAmount( "10°", 10.0 ) );
			Add( new SlewAmount( "40°", 40.0 ) );
		}
	}
}
