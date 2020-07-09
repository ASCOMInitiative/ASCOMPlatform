using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class DeclinationValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			string retval = "NO DATA";
			double rawDec = (double)value;

			if ( !Double.IsNaN( rawDec ) )
			{
				DeclinationConverter converter = new DeclinationConverter( (decimal)rawDec );
				retval = converter.ToString();
			}

			return retval;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
