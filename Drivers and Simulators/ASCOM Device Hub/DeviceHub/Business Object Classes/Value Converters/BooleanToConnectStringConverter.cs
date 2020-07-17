using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class BooleanToConnectStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool isConnected = (bool)value;

			return ( isConnected ) ? "Connected" : "Not Connected";
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
