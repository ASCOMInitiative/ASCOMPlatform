using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class MoveValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			int moveAmount = (int)value;

			return String.Format( "{0} step{1}/move", moveAmount, moveAmount == 1 ? "" : "s" );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
