using System;
using System.Globalization;
using System.Windows.Data;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class AlignmentModeToStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = "";

			AlignmentModes mode = (AlignmentModes)value;

			switch ( mode )
			{
				case AlignmentModes.algAltAz:
					retval = "Altitude-Azimuth";

					break;

				case AlignmentModes.algGermanPolar:
					retval = "German Equatorial";

					break;

				case AlignmentModes.algPolar:
					retval = "Equatorial";

					break;
			}

			return retval;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
