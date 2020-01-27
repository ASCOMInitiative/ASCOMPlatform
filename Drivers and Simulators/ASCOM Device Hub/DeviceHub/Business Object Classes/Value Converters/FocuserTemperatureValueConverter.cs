using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class FocuserTemperatureValueConverter : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = "NO DATA";
			double rawTemp = Double.NaN;
			bool convertToF = false;
			double offset = 0.0;

			if ( values[0] != DependencyProperty.UnsetValue )
			{
				rawTemp = (double)values[0];
			}

			if ( values[1] != DependencyProperty.UnsetValue )
			{
				offset = (double)values[1];
			}

			if ( values[2] != null )
			{
				convertToF = (bool)values[2];
			}

			if ( !Double.IsNaN( rawTemp ) )
			{
				rawTemp += offset;

				if ( convertToF )
				{
					rawTemp = rawTemp * 1.8 + 32.0;
				}

				retval = String.Format( "{0:F1}", rawTemp );
			}

			return retval;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
