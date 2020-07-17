using System;
using System.Globalization;
using System.Windows.Data;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class EquCoordTypeToStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = "";

			EquatorialCoordinateType equType = (EquatorialCoordinateType)value;

			switch ( equType )
			{
				case EquatorialCoordinateType.equOther:
					retval = "Other";

					break;

				case EquatorialCoordinateType.equTopocentric:
					retval = "Topocentric";

					break;

				case EquatorialCoordinateType.equJ2000:
					retval = "J2000";

					break;

				case EquatorialCoordinateType.equJ2050:
					retval = "J2050";

					break;
				case EquatorialCoordinateType.equB1950:
					retval = "B1950";

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
