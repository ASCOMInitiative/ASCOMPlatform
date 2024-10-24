﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    class BooleanOnOffConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool state = (bool)value;

			return ( state ) ? "On" : "Off";
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
