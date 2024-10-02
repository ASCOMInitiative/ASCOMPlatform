using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    public class DoubleEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Globals.NO_DATA_MESSAGE;

            double d = (double)value;
            if (double.IsNaN(d))
                return string.Empty;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == Globals.NO_DATA_MESSAGE)
                return Double.NaN;

            return System.Convert.ToDouble(value);

        }
    }
}
