using System;

namespace ASCOM.DeviceHub
{
	public class HoursConverter
	{
		public HoursConverter( decimal rawHours, int minHours, int maxHours )
		{
			if ( minHours > maxHours )
			{
				throw new ASCOM.InvalidValueException( "The minHours must be less than the maxHours" );
			}

			if ( rawHours < minHours || rawHours >= maxHours )
			{
				string msg = String.Format( "Hour value must be greater than or equal to {0} and less than  {1}.", minHours, maxHours );

				throw new ASCOM.InvalidValueException( msg );
			}

			_minHours = minHours;
			_maxHours = maxHours;

			_value = rawHours;
			HoursToHms( rawHours, ref _hours, ref _minutes, ref _seconds );
		}

		public HoursConverter( int hours, int minutes, int seconds, int minHours, int maxHours )
		{
			if ( minHours > maxHours )
			{
				throw new ASCOM.InvalidValueException( "The minHours must be less than the maxHours" );
			}

			if ( hours < minHours || hours > maxHours )
			{
				string msg = String.Format( "Hour value must be greater than or equal to {0} and less than or equal to {1}.", minHours, maxHours );

				throw new ASCOM.InvalidValueException( msg );
			}

			if ( ( hours == minHours || hours == maxHours )
				&& (minutes != 0 || seconds != 0.0m ) )
			{
				throw new ASCOM.InvalidValueException( "Minutes and seconds must be 0 when hours is at the minimum or the maximum." );
			}

			if ( minutes < 0 || minutes > 59 )
			{
				throw new ASCOM.InvalidValueException( "Minutes must be greater than or equal to 0 and less than 60." );
			}

			if ( seconds < 0 || seconds >= 60 )
			{
				throw new ASCOM.InvalidValueException( "Seconds must be greater than or equal to 0 and less than 60." );
			}

			_minHours = minHours;
			_maxHours = maxHours;
			_hours = hours;
			_minutes = minutes;
			_seconds = seconds;

			_value = HmsToHours( hours, minutes, seconds );
		}

		private decimal _minHours;
		private decimal _maxHours;

		private decimal _value;
		public decimal Value { get => _value; }

		private int _hours;
		public int Hours { get => _hours; }

		private int _minutes;
		public int Minutes { get => _minutes; }

		private int _seconds;
		public int Seconds { get => _seconds; }

		public int[] Values { get => new int[] { _hours, _minutes, _seconds }; }

		public override string ToString()
		{
			decimal hours = Math.Abs( _hours );

			return String.Format( "{0}{1:00}:{2:00}:{3:00}"
				, ( _hours < 0.0 ) ? "-" : "", hours, _minutes, _seconds );
		}

		private decimal HmsToHours( int hours, int minutes, int seconds )
		{
			decimal sign = ( hours < 0.0m ) ? -1.0m : 1.0m;
			hours = Math.Abs( hours );
			decimal retval = (decimal)minutes + seconds / 60.0m;
			retval = (decimal)hours + retval / 60.0m;
			retval *= sign;

			return retval;
		}

		private void HoursToHms( decimal hoursValue, ref int hours, ref int minutes, ref int seconds )
		{
			decimal sign;
			decimal tempHours;
			decimal tempMinutes;
			decimal tempSeconds;

			sign = ( hoursValue < 0.0m ) ? -1.0m : 1.0m;
			decimal temp = Math.Abs( hoursValue );
			tempHours = Math.Truncate( temp );

			temp = ( temp - tempHours ) * 60.0m;
			tempMinutes = Math.Truncate( temp );
			tempSeconds = ( temp - tempMinutes ) * 60.0m;
			tempSeconds = Math.Round( tempSeconds );

			// This is all to keep the seconds from being rounded up to 60.0 when displayed.

			if ( tempSeconds >= 59.95m )
			{
				tempSeconds = 0.0m;
				++tempMinutes;

				if ( tempMinutes == 60 )
				{
					tempMinutes = 0;
					++tempHours;
				}
			}

			hours = (int)( tempHours * sign );
			minutes = (int)tempMinutes;
			seconds = (int)tempSeconds;
		}
	}
}
