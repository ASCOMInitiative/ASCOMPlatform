// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;

static class ObservingConditionsHardware
{
    #region IObservingConditions Implementation

    // Time and wind speed values
    private static Dictionary<DateTime, double> winds = new Dictionary<DateTime, double>();

    /// <summary>
    /// Gets and sets the time period over which observations wil be averaged
    /// </summary>
    internal static double AveragePeriod
    {
        get
        {
            LogMessage("AveragePeriod", "get - 0");
            return 0;
        }
        set
        {
            LogMessage("AveragePeriod Set", value.ToString());
            if (value != 0)
                throw new InvalidValueException("AveragePeriod", value.ToString(), "0 only");
        }
    }

    /// <summary>
    /// Amount of sky obscured by cloud
    /// </summary>
    internal static double CloudCover
    {
        get
        {
            LogMessage("CloudCover", "get - not implemented");
            throw new PropertyNotImplementedException("CloudCover", false);
        }
    }

    /// <summary>
    /// Atmospheric dew point at the observatory in deg C
    /// </summary>
    internal static double DewPoint
    {
        get
        {
            LogMessage("DewPoint", "get - not implemented");
            throw new PropertyNotImplementedException("DewPoint", false);
        }
    }

    /// <summary>
    /// Atmospheric relative humidity at the observatory in percent
    /// </summary>
    internal static double Humidity
    {
        get
        {
            LogMessage("Humidity", "get - not implemented");
            throw new PropertyNotImplementedException("Humidity", false);
        }
    }

    /// <summary>
    /// Atmospheric pressure at the observatory in hectoPascals (mB)
    /// </summary>
    internal static double Pressure
    {
        get
        {
            LogMessage("Pressure", "get - not implemented");
            throw new PropertyNotImplementedException("Pressure", false);
        }
    }

    /// <summary>
    /// Rain rate at the observatory
    /// </summary>
    internal static double RainRate
    {
        get
        {
            LogMessage("RainRate", "get - not implemented");
            throw new PropertyNotImplementedException("RainRate", false);
        }
    }

    /// <summary>
    /// Forces the driver to immediately query its attached hardware to refresh sensor
    /// values
    /// </summary>
    internal static void Refresh()
    {
        throw new MethodNotImplementedException();
    }

    /// <summary>
    /// Provides a description of the sensor providing the requested property
    /// </summary>
    /// <param name="propertyName">Name of the property whose sensor description is required</param>
    /// <returns>The sensor description string</returns>
    internal static string SensorDescription(string propertyName)
    {
        switch (propertyName.Trim().ToLowerInvariant())
        {
            case "averageperiod":
                return "Average period in hours, immediate values are only available";
            case "cloudcover":
            case "dewpoint":
            case "humidity":
            case "pressure":
            case "rainrate":
            case "skybrightness":
            case "skyquality":
            case "skytemperature":
            case "starfwhm":
            case "temperature":
            case "winddirection":
            case "windgust":
            case "windspeed":
                // Throw an exception on the properties that are not implemented
                LogMessage("SensorDescription", $"Property {propertyName} is not implemented");
                throw new MethodNotImplementedException($"SensorDescription - Property {propertyName} is not implemented");
            default:
                LogMessage("SensorDescription", $"Invalid sensor name: {propertyName}");
                throw new InvalidValueException($"SensorDescription - Invalid property name: {propertyName}");
        }
    }

    /// <summary>
    /// Sky brightness at the observatory
    /// </summary>
    internal static double SkyBrightness
    {
        get
        {
            LogMessage("SkyBrightness", "get - not implemented");
            throw new PropertyNotImplementedException("SkyBrightness", false);
        }
    }

    /// <summary>
    /// Sky quality at the observatory
    /// </summary>
    internal static double SkyQuality
    {
        get
        {
            LogMessage("SkyQuality", "get - not implemented");
            throw new PropertyNotImplementedException("SkyQuality", false);
        }
    }

    /// <summary>
    /// Seeing at the observatory
    /// </summary>
    internal static double StarFWHM
    {
        get
        {
            LogMessage("StarFWHM", "get - not implemented");
            throw new PropertyNotImplementedException("StarFWHM", false);
        }
    }

    /// <summary>
    /// Sky temperature at the observatory in deg C
    /// </summary>
    internal static double SkyTemperature
    {
        get
        {
            LogMessage("SkyTemperature", "get - not implemented");
            throw new PropertyNotImplementedException("SkyTemperature", false);
        }
    }

    /// <summary>
    /// Temperature at the observatory in deg C
    /// </summary>
    internal static double Temperature
    {
        get
        {
            LogMessage("Temperature", "get - not implemented");
            throw new PropertyNotImplementedException("Temperature", false);
        }
    }

    /// <summary>
    /// Provides the time since the sensor value was last updated
    /// </summary>
    /// <param name="propertyName">Name of the property whose time since last update Is required</param>
    /// <returns>Time in seconds since the last sensor update for this property</returns>
    internal static double TimeSinceLastUpdate(string propertyName)
    {
        // Test for an empty property name, if found, return the time since the most recent update to any sensor
        if (!string.IsNullOrEmpty(propertyName))
        {
            switch (propertyName.Trim().ToLowerInvariant())
            {
                // Return the time for properties that are implemented, otherwise fall through to the MethodNotImplementedException
                case "averageperiod":
                case "cloudcover":
                case "dewpoint":
                case "humidity":
                case "pressure":
                case "rainrate":
                case "skybrightness":
                case "skyquality":
                case "skytemperature":
                case "starfwhm":
                case "temperature":
                case "winddirection":
                case "windgust":
                case "windspeed":
                    // Throw an exception on the properties that are not implemented
                    LogMessage("TimeSinceLastUpdate", $"Property {propertyName} is not implemented");
                    throw new MethodNotImplementedException($"TimeSinceLastUpdate - Property {propertyName} is not implemented");
                default:
                    LogMessage("TimeSinceLastUpdate", $"Invalid sensor name: {propertyName}");
                    throw new InvalidValueException($"TimeSinceLastUpdate - Invalid property name: {propertyName}");
            }
        }

        // Return the time since the most recent update to any sensor
        LogMessage("TimeSinceLastUpdate", $"The time since the most recent sensor update is not implemented");
        throw new MethodNotImplementedException("TimeSinceLastUpdate(" + propertyName + ")");
    }

    /// <summary>
    /// Wind direction at the observatory in degrees
    /// </summary>
    internal static double WindDirection
    {
        get
        {
            LogMessage("WindDirection", "get - not implemented");
            throw new PropertyNotImplementedException("WindDirection", false);
        }
    }

    /// <summary>
    /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
    /// </summary>
    internal static double WindGust
    {
        get
        {
            LogMessage("WindGust", "get - not implemented");
            throw new PropertyNotImplementedException("WindGust", false);
        }
    }

    /// <summary>
    /// Wind speed at the observatory in m/s
    /// </summary>
    internal static double WindSpeed
    {
        get
        {
            LogMessage("WindSpeed", "get - not implemented");
            throw new PropertyNotImplementedException("WindSpeed", false);
        }
    }

    #endregion

    #region Private methods

    #region Calculate the gust strength as the largest wind recorded over the last two minutes


    private static void UpdateGusts(double speed)
    {
        Dictionary<DateTime, double> newWinds = new Dictionary<DateTime, double>();
        var last = DateTime.Now - TimeSpan.FromMinutes(2);
        winds.Add(DateTime.Now, speed);
        var gust = 0.0;
        foreach (var item in winds)
        {
            if (item.Key > last)
            {
                newWinds.Add(item.Key, item.Value);
                if (item.Value > gust)
                    gust = item.Value;
            }
        }
        winds = newWinds;
    }

    #endregion

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }
}