﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    public class AltitudeValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = Globals.NO_DATA_MESSAGE;
			double rawAltitude = (double)value;

			if ( !Double.IsNaN( rawAltitude ) )
			{
				AltitudeConverter converter = new AltitudeConverter( (decimal)rawAltitude );
				retval = converter.ToString();
			}

			return retval;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
