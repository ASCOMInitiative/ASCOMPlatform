using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class ValueAngleConverter : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter,
					  System.Globalization.CultureInfo culture )
		{
			double value = (double)values[0];
			double minimum = (double)values[1];
			double maximum = (double)values[2];

			return GetAngle( value, minimum, maximum );
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter,
			  System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		private double GetAngle( double value, double minimum, double maximum )
		{
			double current = ( value / ( maximum - minimum ) ) * 360.0;

			if ( Math.Abs( current - 360.0 ) < 1E-05 )
			{
				current = 0.0;
			}

			return current;
		}
	}
}
