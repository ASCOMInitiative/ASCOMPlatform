namespace ASCOM.DeviceHub
{
	public class NormalizedValueBase
	{
		internal double NormalizeValue( double dblValue, double minValue, double maxValue )
		{
			// Zero is the implied min value.

			double normalized = dblValue - minValue;

			while ( normalized < 0.0 )
			{
				normalized += maxValue - minValue;
			}

			while ( normalized >= maxValue - minValue )
			{
				normalized -= maxValue;
			}

			return normalized + minValue;
		}
	}
}
