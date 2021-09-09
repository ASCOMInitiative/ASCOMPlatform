using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	class MultiBindingTrueIfNotParking : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			bool retval = true;

			// During initialization disable the button.

			if ( values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue )
			{
				retval = false;
			}

			if ( retval )
			{ 
				bool bVal0 = (bool)values[0];
				ParkingStateEnum parkingState = (ParkingStateEnum)values[1];

				// Only return true if first value is true and the second indicates that we are not parked or parking.

				if ( !bVal0 )
				{
					retval = false;
				}

				if ( retval && parkingState == ParkingStateEnum.ParkInProgress || parkingState == ParkingStateEnum.IsAtPark )
				{
					retval = false;
				}
			}

			return retval;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
