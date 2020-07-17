namespace ASCOM.DeviceHub
{
	public class RightAscensionConverter : HoursConverter
	{
		public RightAscensionConverter( decimal rawRA )
		  : base( rawRA, 0, 24 )
		{}

		public RightAscensionConverter( int[] values )
			: base( values[0], values[1], values[2], 0, 24 )
		{}

		public RightAscensionConverter( int hours, int minutes, int seconds )
			: base( hours, minutes, seconds, 0, 24 )
		{}
	}
}
