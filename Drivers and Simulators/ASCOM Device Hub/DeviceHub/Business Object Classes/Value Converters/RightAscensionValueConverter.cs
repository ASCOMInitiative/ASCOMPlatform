using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{ 
	public class RightAscensionValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			string retval = Globals.NO_DATA_MESSAGE;
			double rawRA = (double)value;

			if ( !Double.IsNaN( rawRA ) )
			{
				RightAscensionConverter converter = new RightAscensionConverter( (decimal)rawRA );
				retval = converter.ToString();
			}

			return retval;
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ((string)value == Globals.NO_DATA_MESSAGE)
				return Double.NaN;

			return System.Convert.ToDouble(value);
		}

		#endregion
	}
}
