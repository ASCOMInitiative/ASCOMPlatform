using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class PauseStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			int pause = (int)value;

			switch ( pause )
			{
				case -1:
					return "Manual";

				case 0:
					return "None";

				default:
					return pause.ToString();
			}
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null )
			{
				return 0;
			}

			string pause = value.ToString().Trim().ToLower();

			if ( "manual" == pause )
			{
				return -1;
			}			

			if ( "none" == pause )
			{
				return 0;
			}

			return Int32.Parse( pause );
		}
	}
}
