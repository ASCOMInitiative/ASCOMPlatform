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
	class MultiBooleanVisibilityConverter : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			Visibility visibility = Visibility.Visible;

			if ( values.Length == 2 )
			{
				bool bv1 = (bool)values[0];
				bool bv2 = (bool)values[1];

				string parm = parameter.ToString();
				string[] parts = parm.Split( '|');

				if ( parts.Length == 2 )
				{
					bool part1 = Boolean.Parse( parts[0] );
					bool part2 = Boolean.Parse( parts[1] );

					if ( bv1 != part1 || bv2 != part2 )
					{
						visibility = Visibility.Collapsed;
					}
				}
			}

			return visibility;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}
