using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class ValueTextConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
				  System.Globalization.CultureInfo culture )
		{
			double v = (double)value;

			return $"{v:F2}";
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
