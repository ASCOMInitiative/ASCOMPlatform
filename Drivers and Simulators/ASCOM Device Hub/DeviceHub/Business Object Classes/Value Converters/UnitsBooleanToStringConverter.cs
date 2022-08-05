using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	internal class UnitsBooleanToStringConverter : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			bool useNasaUnits = (bool)values[0];
			string nasaUnits = values[1].ToString();
			string ascomUnits = values[2].ToString();

			return ( useNasaUnits ? nasaUnits : ascomUnits );
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
