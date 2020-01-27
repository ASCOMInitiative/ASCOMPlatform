using System;
using System.Windows;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class BooleanVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool boolValue = (bool)value;

			if ( boolValue )
			{
				return Visibility.Visible;
			}

			Visibility visValue = Visibility.Collapsed;

			if ( parameter != null )
			{
				visValue = (Visibility)parameter;
			}

			return visValue;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			Visibility visValue = (Visibility)value;

			return ( visValue == Visibility.Visible );
		}
	}
}
