using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class TrackingRateItem
    {
		public TrackingRateItem( string name, DriveRates rate)
		{
			Name = name;
			Rate = rate;
		}

		public string Name { get; private set; }
		public DriveRates Rate { get; private set; }
    }
}
