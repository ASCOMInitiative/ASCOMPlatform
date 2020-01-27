using System;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
	public class FocuserPositionValueConverter : IValueConverter
	{
		private const string NoData = "NO DATA";

		#region IValueConverter Members

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			int rawInt = (int)value;

			if ( rawInt == Int32.MinValue )
			{
				return NoData;
			}

			return rawInt.ToString();
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ( String.Compare( value.ToString(), NoData ) == 0 )
			{
				return Int32.MinValue;
			}

			return Int32.Parse( value.ToString() );
		}

		#endregion
	}
}

