using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    internal class UnitsBooleanToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null)
                return string.Empty;

            if ((values.Length == 0) | (values.Length == 1))
                return string.Empty;

            if (values[0] is null)
                return string.Empty;

            if (values[1] is null)
                return string.Empty;

            if (values[2] is null)
                return string.Empty;

            bool useNasaUnits = (bool)values[0];
            string nasaUnits = values[1].ToString();
            string ascomUnits = values[2].ToString();

            return (useNasaUnits ? nasaUnits : ascomUnits);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
