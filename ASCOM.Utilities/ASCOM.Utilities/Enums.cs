using System;

namespace ASCOM.Utilities
{
    #region Enums
    /// <summary>
    /// List of available conversion units for use in the Util.ConvertUnits method
    /// </summary>
    public enum Units : int
    {
        // Speed
        metresPerSecond = 0,
        milesPerHour = 1,
        knots = 2,
        // Temperature
        degreesCelsius = 10,
        [Obsolete("Units.degreesFarenheit is an incorrect spelling and has been deprecated, please use Units.degreesFahrenheit instead.", true)]
        degreesFarenheit = 11,
        degreesFahrenheit = 11,
        degreesKelvin = 12,
        // Pressure
        hPa = 20,
        mBar = 21,
        mmHg = 22,
        inHg = 23,
        // RainRate
        mmPerHour = 30,
        inPerHour = 31
    }
}
#endregion
