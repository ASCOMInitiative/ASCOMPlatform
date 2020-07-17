using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ASCOM.DeviceHub
{
	public class IntegerForegroundConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			int iIndex = Int32.Parse( value.ToString() );
			int iParm = Int32.Parse( parameter.ToString() );

			SolidColorBrush retval = GetResourceBrush( iIndex == iParm );

			return retval;
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
