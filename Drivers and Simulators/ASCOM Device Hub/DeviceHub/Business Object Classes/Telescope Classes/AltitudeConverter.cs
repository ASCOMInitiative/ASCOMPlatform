namespace ASCOM.DeviceHub
{
	public class AltitudeConverter : DegreesConverter
	{
		public AltitudeConverter( decimal rawAltitude )
			: base( rawAltitude, -90, 90 )
		{}

		public AltitudeConverter( int[] values )
			: base( values[0], values[1], values[2], -90, 90 )
		{}

		public AltitudeConverter( int degrees, int minutes, int seconds )
			: base( degrees, minutes, seconds, -90, 90 )
		{}

	}
}
