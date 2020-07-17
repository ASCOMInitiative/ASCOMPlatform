using System;

namespace ASCOM.DeviceHub
{
	public class DegreesConverter
	{
		protected DegreesConverter( decimal rawDegrees, int minDegrees, int maxDegrees )
		{
			if ( minDegrees >= maxDegrees )
			{
				throw new ASCOM.InvalidValueException( "The minDegrees must be less than maxDegrees" );
			}

			if ( rawDegrees < minDegrees || rawDegrees > maxDegrees )
			{
				string msg = String.Format( "Degree value must be greater than or equal to {0} and less than or equal to {1}.", minDegrees, maxDegrees );

				throw new ASCOM.InvalidValueException( msg );
			}

			_minDegrees = minDegrees;
			_maxDegrees = maxDegrees;

			_value = rawDegrees;
			DegreesToDms( rawDegrees, ref _degrees, ref _minutes, ref _seconds );
		}

		protected DegreesConverter( int degrees, int minutes, int seconds, decimal minDegrees, decimal maxDegrees )
		{
			if ( minDegrees >= maxDegrees )
			{
				throw new ArgumentOutOfRangeException( "The minDegrees must be less than maxDegrees" );
			}

			if ( degrees < minDegrees || degrees > maxDegrees )
			{
				string msg = String.Format( "Degrees must be greater than or equal to {0} and less than or equal to {1}.", minDegrees, maxDegrees );

				throw new ASCOM.InvalidValueException( msg );
			}

			if ( ( degrees == minDegrees || degrees == maxDegrees ) 
				&& ( minutes != 0 || seconds != 0.0m ) )
			{
				throw new ASCOM.InvalidValueException( "Minutes and seconds must be 0 when degrees is at the minimum or the maximum." );
			}

			if ( minutes < 0 || minutes > 59 )
			{
				throw new ASCOM.InvalidValueException( "Minutes must be greater than or equal to 0 and less than 60." );
			}

			if ( seconds < 0 || seconds >= 60 )
			{
				throw new ASCOM.InvalidValueException( "Seconds must be greater than or equal to 0 and less than 60." );
			}

			_minDegrees = minDegrees;
			_maxDegrees = maxDegrees;
			_degrees = degrees;
			_minutes = minutes;
			_seconds = seconds;

			_value = DmsToDec( degrees, minutes, seconds );
		}

		private decimal _minDegrees;
		private decimal _maxDegrees;

		private decimal _value;

		public decimal Value
		{
			get { return _value; }
		}

		private int _degrees;
		public int Degrees { get => _degrees; }

		private int _minutes;
		public int Minutes { get => _minutes; }

		private int _seconds;
		public int Seconds { get => _seconds; }

		public int[] Values	{ get => new int[] { _degrees, _minutes, _seconds }; }

		public override string ToString()
		{
			char signChar = ( _degrees < 0.0 ) ? '-' : ' ';
			decimal degrees = Math.Abs( _degrees );

			return String.Format( "{0}{1:000}° {2:00}' {3:00}\"", signChar, degrees, _minutes, _seconds );
		}

		private decimal DmsToDec( int degrees, int minutes, int seconds )
		{
			decimal sign = ( degrees < 0.0m ) ? -1.0m : 1.0m;
			degrees = Math.Abs( degrees );
			decimal retval = (decimal)minutes + seconds / 60.0m;
			retval = (decimal)degrees + retval / 60.0m;
			retval *= sign;

			return retval;
		}

		private void DegreesToDms( decimal degreeValue, ref int degrees, ref int minutes, ref int seconds )
		{
			decimal sign;
			decimal tempDegrees;
			decimal tempMinutes;
			decimal tempSeconds;

			sign = ( degreeValue < 0.0m ) ? -1.0m : 1.0m;
			decimal temp = Math.Abs( degreeValue );
			tempDegrees = Math.Truncate( temp );

			temp = ( temp - tempDegrees ) * 60.0m;
			tempMinutes = Math.Truncate( temp );
			tempSeconds = ( temp - tempMinutes ) * 60.0m;
			tempSeconds = Math.Round( tempSeconds );

			// This is all to keep the seconds from being rounded up to 60.0 when displayed.

			if ( tempSeconds >= 59.95m)
			{
				tempSeconds = 0.0m;
				++tempMinutes;

				if ( tempMinutes == 60 )
				{
					tempMinutes = 0;
					++tempDegrees;
				}
			}

			degrees = (int)(tempDegrees * sign);
			minutes = (int)tempMinutes;
			seconds = (int)tempSeconds;
		}
	}
}
