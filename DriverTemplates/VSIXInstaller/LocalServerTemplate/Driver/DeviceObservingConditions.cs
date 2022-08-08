// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;

class DeviceObservingConditions
{
    #region IObservingConditions Implementation

    /// <summary>
    /// Gets and sets the time period over which observations wil be averaged
    /// </summary>
    public double AveragePeriod
    {
        get
        {
            double averageperiod = ObservingConditionsHardware.AveragePeriod;
            LogMessage("AveragePeriod Get", averageperiod.ToString());
            return averageperiod;
        }
        set
        {
            LogMessage("AveragePeriod Set", value.ToString());
            ObservingConditionsHardware.AveragePeriod = value;        }
    }

    /// <summary>
    /// Amount of sky obscured by cloud
    /// </summary>
    public double CloudCover
    {
        get
        {
            double cloudCover = ObservingConditionsHardware.CloudCover;
            LogMessage("CloudCover Get", cloudCover.ToString());
            return cloudCover;
        }
    }

    /// <summary>
    /// Atmospheric dew point at the observatory in deg C
    /// </summary>
    public double DewPoint
    {
        get
        {
            double dewPoint = ObservingConditionsHardware.DewPoint;
            LogMessage("DewPoint Get", dewPoint.ToString());
            return dewPoint;
        }
    }

    /// <summary>
    /// Atmospheric relative humidity at the observatory in percent
    /// </summary>
    public double Humidity
    {
        get
        {
            double humidity = ObservingConditionsHardware.Humidity;
            LogMessage("Humidity Get", humidity.ToString());
            return humidity;
        }
    }

    /// <summary>
    /// Atmospheric pressure at the observatory in hectoPascals (mB)
    /// </summary>
    public double Pressure
    {
        get
        {
            double period = ObservingConditionsHardware.Pressure;
            LogMessage("Pressure Get", period.ToString());
            return period;
        }
    }

    /// <summary>
    /// Rain rate at the observatory
    /// </summary>
    public double RainRate
    {
        get
        {
            double rainRate = ObservingConditionsHardware.RainRate;
            LogMessage("RainRate Get", rainRate.ToString());
            return rainRate;
        }
    }

    /// <summary>
    /// Forces the driver to immediately query its attached hardware to refresh sensor
    /// values
    /// </summary>
    public void Refresh()
    {
        LogMessage("Refresh", $"Calling method.");
        ObservingConditionsHardware.Refresh();
        LogMessage("Refresh", $"Completed.");
    }

    /// <summary>
    /// Provides a description of the sensor providing the requested property
    /// </summary>
    /// <param name="propertyName">Name of the property whose sensor description is required</param>
    /// <returns>The sensor description string</returns>
    public string SensorDescription(string propertyName)
    {
        LogMessage("SensorDescription", $"Calling method.");
        string sensorDescription=ObservingConditionsHardware.SensorDescription(propertyName);
        LogMessage("SensorDescription", $"{sensorDescription}");
        return sensorDescription;
    }

    /// <summary>
    /// Sky brightness at the observatory
    /// </summary>
    public double SkyBrightness
    {
        get
        {
            double skyBrightness = ObservingConditionsHardware.SkyBrightness;
            LogMessage("SkyBrightness Get", skyBrightness.ToString());
            return skyBrightness;
        }
    }

    /// <summary>
    /// Sky quality at the observatory
    /// </summary>
    public double SkyQuality
    {
        get
        {
            double skyQuality = ObservingConditionsHardware.SkyQuality;
            LogMessage("SkyQuality Get", skyQuality.ToString());
            return skyQuality;
        }
    }

    /// <summary>
    /// Seeing at the observatory
    /// </summary>
    public double StarFWHM
    {
        get
        {
            double starFwhm = ObservingConditionsHardware.StarFWHM;
            LogMessage("StarFWHM Get", starFwhm.ToString());
            return starFwhm;
        }
    }

    /// <summary>
    /// Sky temperature at the observatory in deg C
    /// </summary>
    public double SkyTemperature
    {
        get
        {
            double skyTemperature = ObservingConditionsHardware.SkyTemperature;
            LogMessage("SkyTemperature Get", skyTemperature.ToString());
            return skyTemperature;
        }
    }

    /// <summary>
    /// Temperature at the observatory in deg C
    /// </summary>
    public double Temperature
    {
        get
        {
            double temperature = ObservingConditionsHardware.Temperature;
            LogMessage("Temperature Get", temperature.ToString());
            return temperature;
        }
    }

    /// <summary>
    /// Provides the time since the sensor value was last updated
    /// </summary>
    /// <param name="propertyName">Name of the property whose time since last update Is required</param>
    /// <returns>Time in seconds since the last sensor update for this property</returns>
    public double TimeSinceLastUpdate(string propertyName)
    {
        LogMessage("TimeSinceLastUpdate", $"Calling method.");
        ObservingConditionsHardware.TimeSinceLastUpdate(propertyName);
        double timeSincelastUpdate = ObservingConditionsHardware.TimeSinceLastUpdate(propertyName);
        LogMessage("TimeSinceLastUpdate", $"{timeSincelastUpdate}");
        return timeSincelastUpdate;
    }

    /// <summary>
    /// Wind direction at the observatory in degrees
    /// </summary>
    public double WindDirection
    {
        get
        {
            double windDirection = ObservingConditionsHardware.WindDirection;
            LogMessage("WindDirection Get", windDirection.ToString());
            return windDirection;
        }
    }

    /// <summary>
    /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
    /// </summary>
    public double WindGust
    {
        get
        {
            double windGust = ObservingConditionsHardware.WindGust;
            LogMessage("WindGust Get", windGust.ToString());
            return windGust;
        }
    }

    /// <summary>
    /// Wind speed at the observatory in m/s
    /// </summary>
    public double WindSpeed
    {
        get
        {
            double windSpeed = ObservingConditionsHardware.WindSpeed;
            LogMessage("WindSpeed Get", windSpeed.ToString());
            return windSpeed;
        }
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }
}