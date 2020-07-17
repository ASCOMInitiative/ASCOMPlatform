namespace ASCOM.DeviceHub
{
	public class AzimuthConverter : DegreesConverter
	{
		public AzimuthConverter( decimal rawAzimuth )
			: base( rawAzimuth, 0, 360 )
		{}

		public AzimuthConverter ( int[] values )
			: this( values[0], values[1], values[2])
		{}

		public AzimuthConverter( int degrees, int minutes, int seconds )
			: base( degrees, minutes, seconds, 0, 360 )
		{}
	}
}
