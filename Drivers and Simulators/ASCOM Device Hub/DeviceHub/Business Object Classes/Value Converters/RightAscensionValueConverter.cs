using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{ 
	public class RightAscensionValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			string retval = "NO DATA";
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
			throw new NotImplementedException();
		}

		#endregion
	}
}
