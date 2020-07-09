using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class DeviceIDConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string deviceType = parameter.ToString();
			string id = ( value != null ) ? value.ToString() : "";

			return ( String.IsNullOrEmpty( id ) ) ? String.Format("(No {0})", deviceType) : id;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
