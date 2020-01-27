using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class BooleanBooleanValueConverter: IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bValue = (bool)value;
			bool bParm = Boolean.Parse( parameter.ToString() );

			return bValue == bParm;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bValue = (bool)value;
			bool bParm = Boolean.Parse( parameter.ToString() );

			return bValue == bParm;
		}

		#endregion
	}
}
