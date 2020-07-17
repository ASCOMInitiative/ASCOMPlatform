using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class HourAngleValueConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = "NO DATA";
			double rawValue = (double)value;

			if ( !Double.IsNaN( rawValue ) )
			{
				HourAngleConverter converter = new HourAngleConverter( (decimal)rawValue );
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
