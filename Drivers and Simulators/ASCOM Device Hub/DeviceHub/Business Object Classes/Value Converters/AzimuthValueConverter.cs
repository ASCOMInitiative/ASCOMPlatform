using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class AzimuthValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = "NO DATA";
			double rawAzimuth = (double)value;

			if ( !Double.IsNaN( rawAzimuth ) )
			{
				AzimuthConverter converter = new AzimuthConverter( (decimal)rawAzimuth );
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
