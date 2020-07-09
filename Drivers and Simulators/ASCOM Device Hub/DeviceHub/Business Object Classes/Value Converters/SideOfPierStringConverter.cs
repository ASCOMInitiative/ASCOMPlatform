using System;
using System.Globalization;
using System.Windows.Data;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class SideOfPierStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string retval = null;

			PierSide sop = (PierSide)value;

			switch (sop)
			{
				case PierSide.pierEast:
					retval = "East";
					break;

				case PierSide.pierWest:
					retval = "West";
					break;

				case PierSide.pierUnknown:
					retval = "Unknown";
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
