using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class DeviceConnectedValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool isConnected = (bool)value;
			string device = parameter.ToString();


			return String.Format( "{0} {1}", isConnected ? "Disconnect" : "Connect", device );
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
