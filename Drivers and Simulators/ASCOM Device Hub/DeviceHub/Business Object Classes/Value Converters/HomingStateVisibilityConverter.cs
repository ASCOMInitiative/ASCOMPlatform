using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class HomingStateVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			HomingStateEnum state = (HomingStateEnum)value;
			HomingStateEnum visibleState = (HomingStateEnum)parameter;

			return ( state == visibleState ) ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
