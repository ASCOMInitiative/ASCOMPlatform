namespace ASCOM.DeviceHub
{
	public class HourAngleConverter : HoursConverter
	{
		public HourAngleConverter( decimal rawRA )
			: base( rawRA, -12, 12 )
		{ }

		public HourAngleConverter( int[] values )
			: base( values[0], values[1], values[2], -12, 12 )
		{ }

		public HourAngleConverter( int hours, int minutes, int seconds )
			: base( hours, minutes, seconds, -12, 12 )
		{ }
	}
}
