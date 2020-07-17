using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class MoveEnumToBooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			ScopeMoveEnum moveType = (ScopeMoveEnum)value;
			ScopeMoveEnum paramTrue = (ScopeMoveEnum)parameter;

			return ( moveType == paramTrue );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
