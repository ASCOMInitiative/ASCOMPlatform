using System;

namespace ASCOM.DeviceHub
{
    internal static class Extensions
    {

        #region Internal methods

        /// <summary>
        /// Convert a double to sexagesimal with three decimal points for seconds
        /// </summary>
        /// <param name="value">Double to convert to sexagesimal</param>
        /// <returns>Sexagesimal string of the input value</returns>
        internal static string ToHMS(this double value)
        {
            return DoubleToSexagesimalSeconds(value, ":", ":", "", 1);
        }

        /// <summary>
        /// Convert a double to sexagesimal with two decimal points for seconds
        /// </summary>
        /// <param name="value">Double to convert to sexagesimal</param>
        /// <returns>Sexagesimal string of the input value</returns>
        internal static string ToDMS(this double value)
        {
            return DoubleToSexagesimalSeconds(value, ":", ":", "", 2);
        }

        #endregion

        #region Private methods

        private static string DoubleToSexagesimalSeconds(double value, string degDelim, string minDelim, string secDelim, int secDecimalDigits)
        {
            string wholeUnits, wholeMinutes, seconds, secondsFormatString;
            bool inputIsNegative;

            try
            {
                if (Double.IsNaN(value) || Double.IsInfinity(value))
                    return value.ToString();

                // Convert the input value to a positive number if required and store the sign
                if (value < 0.0)
                {
                    value = -value;
                    inputIsNegative = true;
                }
                else
                    inputIsNegative = false;

                // Extract the number of whole units and save the remainder
                wholeUnits = Math.Floor(value).ToString().PadLeft(2, '0');
                double remainderInMinutes = (value - Convert.ToDouble(wholeUnits)) * 60.0; // Remainder in minutes

                // Extract the number of whole minutes and save the remainder
                wholeMinutes = Math.Floor(remainderInMinutes).ToString().PadLeft(2, '0');// Integral minutes
                double remainderInSeconds = (remainderInMinutes - System.Convert.ToDouble(wholeMinutes)) * 60.0; // Remainder in seconds

                if (secDecimalDigits == 0) secondsFormatString = "00"; // No decimal point or decimal digits
                else secondsFormatString = "00." + new String('0', secDecimalDigits); // Format$ string of form 00.0000

                seconds = remainderInSeconds.ToString(secondsFormatString); // Format seconds with requested number of decimal digits

                // Check to see whether rounding has pushed the number of whole seconds or minutes to 60
                if (seconds.Substring(0, 2) == "60") // The rounded seconds value is 60 so we need to add one to the minutes count and make the seconds 0
                {
                    seconds = 0.0.ToString(secondsFormatString); // Seconds are 0.0 formatted as required
                    wholeMinutes = (Convert.ToInt32(wholeMinutes) + 1).ToString("00"); // Add one to the to the minutes count
                    if (wholeMinutes == "60")// The minutes value is 60 so we need to add one to the units count and make the minutes 0
                    {
                        wholeMinutes = "00"; // Minutes are 0.0
                        wholeUnits = (Convert.ToInt32(wholeUnits) + 1).ToString("00"); // Add one to the units count
                    }
                }

                // Create the full formatted string from the units, minute and seconds parts and add a leading negative sign if required
                string returnValue = wholeUnits + degDelim + wholeMinutes + minDelim + seconds + secDelim;
                if (inputIsNegative) returnValue = $"-{returnValue}";

                return returnValue;

            }
            catch (Exception)
            {
                return value.ToString();
            }
        }

        #endregion

    }
}
