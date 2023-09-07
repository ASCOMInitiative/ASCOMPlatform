using System;

namespace ASCOM.Utilities
{
    /// <summary>
    /// List of available conversion units for use in the Util.ConvertUnits method
    /// </summary>
    public enum Units : int
    {
        /// <summary>
        /// Speed (metres per second)
        /// </summary>
        metresPerSecond = 0,

        /// <summary>
        /// Sped (miles per hour)
        /// </summary>
        milesPerHour = 1,

        /// <summary>
        /// Speed (knots)
        /// </summary>
        knots = 2,

        /// <summary>
        /// Temperature (Celsius)
        /// </summary>
        degreesCelsius = 10,

        /// <summary>
        /// Deprecated Temperature (Fahrenheit) due to incorrect spelling
        /// </summary>
        [Obsolete("Units.degreesFarenheit is an incorrect spelling and has been deprecated, please use Units.degreesFahrenheit instead.", true)]
        degreesFarenheit = 11,

        /// <summary>
        /// Temperature (Fahrenheit) 
        /// </summary>
        degreesFahrenheit = 11,

        /// <summary>
        /// Temperature (kelvin)
        /// </summary>
        degreesKelvin = 12,

        /// <summary>
        /// Pressure (hectoPascals)
        /// </summary>
        hPa = 20,

        /// <summary>
        /// Pressure (millibar)
        /// </summary>
        mBar = 21,

        /// <summary>
        /// Pressure (mm of mercury)
        /// </summary>
        mmHg = 22,

        /// <summary>
        /// Pressure (inches of mercury)
        /// </summary>
        inHg = 23,

        /// <summary>
        /// Rain rate (mm per hour)
        /// </summary>
        mmPerHour = 30,

        /// <summary>
        /// Rain rate (inches per hour)
        /// </summary>
        inPerHour = 31
    }
}
