using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class NegatedBooleanValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bval = (bool)value;

			return !bval;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bval = (bool)value;

			return !bval;
		}
	}
}
