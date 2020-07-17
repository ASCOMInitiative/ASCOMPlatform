using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ASCOM.DeviceHub
{
	class BooleanForegroundConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool boolVal = (bool)value;
			bool boolParm = (bool)parameter;

			SolidColorBrush brush = GetResourceBrush( boolVal == boolParm );

			return brush;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}

		private SolidColorBrush GetResourceBrush( bool selected )
		{
			string name = selected ? "SelectedRotaryValueForeground" : "RotarySliderForeground";

			SolidColorBrush brush = (SolidColorBrush)Application.Current.Resources[name];

			return brush;
		}
	}
}
