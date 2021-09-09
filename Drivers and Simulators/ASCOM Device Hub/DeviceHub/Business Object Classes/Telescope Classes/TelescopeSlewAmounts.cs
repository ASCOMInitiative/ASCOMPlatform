using System.Collections.Generic;

namespace ASCOM.DeviceHub
{
	public class TelescopeSlewAmounts : List<JogAmount>
    {
		public TelescopeSlewAmounts()
		{
			Add( new JogAmount( "1'", 1/60.0 ) );
			Add( new JogAmount( "4'", 4/60.0 ) );
			Add( new JogAmount( "15'", 15/60.0 ) );
			Add( new JogAmount( "1°", 1.0 ) );
			Add( new JogAmount( "4°", 4.0 ) );
			Add( new JogAmount( "10°", 10.0 ) );
			Add( new JogAmount( "40°", 40.0 ) );
		}
	}
}
