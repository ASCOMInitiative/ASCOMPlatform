namespace ASCOM.DeviceHub
{
	public class DeclinationConverter : DegreesConverter
	{
		public DeclinationConverter( decimal rawDeclination )
			: base( rawDeclination, -90, 90 )
		{}

		public DeclinationConverter( int[] values )
			: base( values[0], values[1], values[2], -90, 90 )
		{}

		public DeclinationConverter( int degrees, int minutes, int seconds )
			: base( degrees, minutes, seconds, -90, 90 )
		{}
	}
}
