using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    internal class EditLockTextBooleanValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool bval = (bool)value;

			return bval ? "Lock" : "Edit";
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
