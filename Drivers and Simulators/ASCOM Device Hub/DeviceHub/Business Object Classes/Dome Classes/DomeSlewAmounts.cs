using System.Collections.Generic;

namespace ASCOM.DeviceHub
{
	public class DomeSlewAmounts : List<JogAmount>
	{
		public DomeSlewAmounts()
		{
			Add( new JogAmount( "1°", 1.0 ) );
			Add( new JogAmount( "4°", 4.0 ) );
			Add( new JogAmount( "10°", 10.0 ) );
			Add( new JogAmount( "40°", 40.0 ) );
			Add( new JogAmount( "100°", 100.0 ) );
			Add( new JogAmount( "180°", 180.0 ) );
		}
	}
}
