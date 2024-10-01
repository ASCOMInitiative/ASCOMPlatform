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
			string retval = Globals.NO_DATA_MESSAGE;
			double rawTemp = 0.0;
			bool convertToF = true;
			double offset = 0.0;

			if ( values[0] != DependencyProperty.UnsetValue )
			{
				rawTemp = (double)values[0];
			}

			if ( values[1] != DependencyProperty.UnsetValue )
			{
				offset = (double)values[1];
			}

			if ( values[2] != DependencyProperty.UnsetValue )
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

				retval = $"{rawTemp:F1}";
			}

			return retval;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
