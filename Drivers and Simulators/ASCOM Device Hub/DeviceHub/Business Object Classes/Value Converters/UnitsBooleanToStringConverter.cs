using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCOM.DeviceHub
{
    internal class UnitsBooleanToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            const string DEFAULT_VALUE = "";

            // Make sure that a string value is always returned
            try
            {
                if (values is null)
                    return DEFAULT_VALUE;

                if ((values.Length == 0) | (values.Length == 1))
                    return DEFAULT_VALUE;

                if (values[0] is null)
                    return DEFAULT_VALUE;

                if (values[1] is null)
                    return DEFAULT_VALUE;

                if (values[2] is null)
                    return DEFAULT_VALUE;

                bool useNasaUnits = (bool)values[0];
                string nasaUnits = values[1].ToString();
                string ascomUnits = values[2].ToString();

                return (useNasaUnits ? nasaUnits : ascomUnits);
            }
            catch (Exception)
            {
                return DEFAULT_VALUE;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
