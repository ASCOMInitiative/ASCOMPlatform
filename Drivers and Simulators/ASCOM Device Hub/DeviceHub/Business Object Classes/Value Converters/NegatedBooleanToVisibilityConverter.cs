using System;
using System.Windows;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	class NegatedBooleanToVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bVal = (bool)value;

			Visibility visValue = Visibility.Collapsed;

			if ( parameter != null )
			{
				visValue = (Visibility)parameter;
			}

			return bVal ? visValue : Visibility.Visible;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
