using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class NullableBooleanValueConverter : IValueConverter
    {
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool? input = (bool?)value;
			bool param = bool.Parse( parameter.ToString() );

			if ( !input.HasValue )
			{
				return false;
			}
			else
			{
				return !( input.Value ^ param );
			}
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool input = (bool)value;
			bool param = bool.Parse( parameter.ToString() );

			return !( input ^ param );
		}
	}
}
