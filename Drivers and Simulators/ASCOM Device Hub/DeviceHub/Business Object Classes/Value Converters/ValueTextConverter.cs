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
			return String.Format( "{0:F2}", v );
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
