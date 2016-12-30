using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.OpenWeatherMap
{
    class OpenWeatherMap
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.OpenWeatherMap.ObservingConditions";

        internal static string CityName { get; set; }

        internal static double SiteLongitude { get; set; }

        internal static double SiteLatitude { get; set; }

        internal static double SiteElevation { get; set; }

        internal static LocationType locationType = LocationType.CityName;

        internal static string apiUrl = "http://api.openweathermap.org/data/2.5/weather";

        internal static string apiKey;

        private static WeatherData weatherData;

        private static Util util = new Util();

        private static DateTime lastUpdate = DateTime.MinValue;
        private static bool isConnected;

        internal static bool UpdateWeather()
        {
            try
            {
                var reply = GetResponseStr();
                Log.LogMessage("UpdateWeather", "reply {0}", reply);
                // this kludge handles the problem of the rain rate field name being "1h"
                reply = reply.Replace("{\"1h\":", "{\"rate1h\":");
                reply = reply.Replace("{\"3h\":", "{\"rate3h\":");
                var jss = new JavaScriptSerializer();
                weatherData = (WeatherData)jss.Deserialize<WeatherData>(reply);
                if (weatherData.cod == 404)
                    return false;
                var lu = FromUnix(weatherData.dt);
                var sr = FromUnix(weatherData.sys.sunrise.Value);
                var ss = FromUnix(weatherData.sys.sunset.Value);
                Log.LogMessage("UpdateWeather", "update {0}, sunrise {1}, sunset {2}", lu, sr, ss);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogMessage("UpdateWeather", "error {0}", ex);
                return false;
            }
        }

        private static DateTime FromUnix(int dt)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(dt);
        }

        private static string BuildUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(apiUrl);
            switch (locationType)
            {
                case LocationType.CityName:
                    sb.AppendFormat("?q={0}", CityName);
                    break;
                case LocationType.LatLong:
                    sb.AppendFormat("?lat={0}&lon={1}", SiteLatitude, SiteLongitude);
                    break;
                default:
                    break;
            }
            sb.AppendFormat("&APPID={0}", apiKey);
            Log.LogMessage("BuildUrl", "uri {0}", sb);
            return sb.ToString();
        }

        private static string GetResponseStr()
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                var uri = BuildUrl();
                var str = wc.DownloadString(uri);
                return str;
            }
        }

        private static void GetData(string message)
        {
            try
            {
                if (weatherData == null || (DateTime.Now - lastUpdate).TotalMinutes > 10)
                {
                    UpdateWeather();
                    lastUpdate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Log.LogMessage("GetData", "GetData({0}), error {1}", message, ex);
                throw new DriverException(message, ex);
            }
        }

        internal static double CloudCover
        {
            get
            {
                GetData("CloudCover");
                if (weatherData.clouds.all.HasValue)
                    return weatherData.clouds.all.Value;
                throw new PropertyNotImplementedException("CloudCover");
            }
        }

        internal static double DewPoint
        {
            get
            {
                try
                {
                    GetData("DewPoint");
                    if (weatherData.main.humidity.HasValue && weatherData.main.temp.HasValue)
                    {
                        var rh = weatherData.main.humidity.Value;
                        var dc = weatherData.main.temp.Value - 273.15;
                        var dp = util.Humidity2DewPoint(rh, dc);
                        Log.LogMessage("DewPoint", "Rh {0}, Temp {1}, dp {2}", rh, dc, dp);
                        return Math.Round(dp, 2);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogMessage("DewPoint", "error {0}", ex);
                    throw;
                }
                throw new PropertyNotImplementedException("DewPoint");
            }
        }
        
        internal static double Humidity
        {
            get
            {
                try
                {
                    GetData("Humidity");
                    if (weatherData.main.humidity.HasValue)
                        return weatherData.main.humidity.Value;
                }
                catch (Exception ex)
                {
                    Log.LogMessage("Humidity", "error {0}", ex);
                    throw;
                }
                throw new PropertyNotImplementedException("Humidity");
            }
        }
        
        internal static double Pressure
        {
            get
            {
                try
                {
                    GetData("Pressure");
                    // try to get sea level pressure
                    if (weatherData.main.sea_level.HasValue)
                    {
                        // convert to site pressure
                        var qnh = weatherData.main.sea_level.Value;
                        var qfe = util.ConvertPressure(qnh, 0, SiteElevation);
                        Log.LogMessage("Pressure", "qnh {0}, elevation {1}, qfe {2}", qnh, SiteElevation, qfe);
                        return Math.Round(qfe, 1);
                    }
                    if (weatherData.main.grnd_level.HasValue)
                    {
                        var qfe = weatherData.main.grnd_level.Value;
                        Log.LogMessage("Pressure", "ground level {0}", qfe);
                        return Math.Round(qfe, 1);
                    }
                    if (weatherData.main.pressure.HasValue)
                    {
                        // assume QNH
                        var qnh = weatherData.main.pressure.Value;
                        var qfe = util.ConvertPressure(qnh, 0, SiteElevation);
                        Log.LogMessage("Pressure", "pressure {0}, elevation {1}, qfe {2}", qnh, SiteElevation, qfe);
                        return Math.Round(qfe, 1);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogMessage("Pressure", "error {0}", ex);
                    throw;
                }
                throw new PropertyNotImplementedException("Pressure");
            }
        }
        
        internal static double RainRate
        {
            get
            {
                GetData("RainRate");
                try 
                {
                    var rr = 0.0;
                    if (weatherData.rain.rate1h.HasValue)
                        rr = weatherData.rain.rate1h.Value;
                    else if (weatherData.rain.rate3h.HasValue)
                        rr = weatherData.rain.rate3h.Value;
                    else if (weatherData.snow.rate1h.HasValue)
                        rr = weatherData.snow.rate1h.Value;
                    else if (weatherData.snow.rate3h.HasValue)
                        rr = weatherData.snow.rate3h.Value;
                    return rr; 
                }
                catch
                {
                    if (weatherData.weather[0].main.ToLowerInvariant().Contains("rain") ||
                        weatherData.weather[0].main.ToLowerInvariant().Contains("snow"))
                        return 1;
                    return 0;
                }
            }
        }

        internal static double Temperature
        {
            get
            {
                try
                {
                    GetData("Temperature");
                    if (weatherData.main.temp.HasValue)
                    {
                        return Math.Round(weatherData.main.temp.Value - 273.15, 1);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogMessage("Temperature", "error {0}", ex);
                    throw;
                }
                throw new PropertyNotImplementedException("");
            }
        }

        internal static double WindDirection
        {
            get
            {
                GetData("WindDirection");
                if (weatherData.wind.deg.HasValue)
                    return Math.Round(weatherData.wind.deg.Value);
                if (weatherData.wind.speed.HasValue)
                    return 0;
                throw new PropertyNotImplementedException("WindDirection");
            }
        }

        internal static double WindSpeed
        {
            get
            {
                GetData("WindSpeed");
                if (weatherData.wind.speed.HasValue)
                    return Math.Round(weatherData.wind.speed.Value, 1);
                throw new PropertyNotImplementedException("WindSpeed");
            }
        }

        internal static double TimeSinceLastUpdate
        {
            get
            {
                DateTime lu = lastUpdate;
                try
                {
                    GetData("TimeSinceLastUpdate");
                    lu = FromUnix(weatherData.dt);
                }
                catch {}
                return (DateTime.UtcNow - lu).TotalSeconds;
            }
        }

        internal static string Description
        {
            get
            {
                try
                {
                    if (weatherData == null)
                        return "";
                    return string.Format("Name {0}, Latitude {1}, Longitude {2}", weatherData.name, weatherData.coord.lat, weatherData.coord.lon);
                }
                catch (Exception ex)
                {
                    Log.LogMessage("Description", "error {0}", ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile profile = new Profile())
            {
                profile.DeviceType = "ObservingConditions";
                Log.ReadProfile(profile);
                locationType = (LocationType)Enum.Parse(typeof(LocationType), profile.GetValue(driverID, "LocationType", string.Empty, LocationType.CityName.ToString()));
                CityName = profile.GetValue(driverID, "CityName", string.Empty, CityName);
                SiteElevation = double.Parse(profile.GetValue(driverID, "SiteElevation", string.Empty, "0"), CultureInfo.InvariantCulture);
                SiteLatitude = double.Parse(profile.GetValue(driverID, "SiteLatitude", string.Empty, "0"), CultureInfo.InvariantCulture);
                SiteLongitude = double.Parse(profile.GetValue(driverID, "SiteLongitude", string.Empty, "0"), CultureInfo.InvariantCulture);
                apiKey = profile.GetValue(driverID, "ApiKey");
                apiUrl = profile.GetValue(driverID, "ApiUrl", string.Empty, apiUrl);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                Log.WriteProfile(driverProfile);
                driverProfile.WriteValue(driverID, "LocationType", locationType.ToString());
                driverProfile.WriteValue(driverID, "CityName", CityName);
                driverProfile.WriteValue(driverID, "SiteElevation", SiteElevation.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverID, "SiteLatitude", SiteLatitude.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverID, "SiteLongitude", SiteLongitude.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverID, "ApiKey", apiKey);
                driverProfile.WriteValue(driverID, "ApiUrl", apiUrl);
            }
        }

        public static bool Connected
        {
            get 
            {
                Log.LogMessage("Connected", "Get {0}", isConnected);
                return isConnected;
            }
            set
            {
                Log.LogMessage("Connected", "Set {0}", value);
                if (value == isConnected)
                    return;

                // check for valid data
                if (string.IsNullOrEmpty(apiKey))
                    throw new DriverException("Cannot connect, no API key has been defined.");
                switch (locationType)
	            {
		            case LocationType.CityName:
                        if (string.IsNullOrEmpty(CityName))
                            throw new DriverException("City name is selected but no city name has been defined");
                        break;
                    case LocationType.LatLong:
                        if (SiteLatitude == 0 && SiteLongitude == 0)
                            throw new DriverException("Position has been selected but no position has been defined");
                        break;
                    default:
                        throw new DriverException("Unspecified Location type is defined");
	            }
                isConnected = value;
                lastUpdate = DateTime.MinValue;
            }
        }
    }

    enum LocationType
    {
        /// <summary>
        /// api.openweathermap.org/data/2.5/weather?q={city name}
        /// or
        /// api.openweathermap.org/data/2.5/weather?q={city name},{country code}
        /// </summary>
        CityName,
        /// <summary>
        /// api.openweathermap.org/data/2.5/weather?id={city id}
        /// </summary>
        CityId,
        /// <summary>
        /// api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}
        /// </summary>
        LatLong,
        /// <summary>
        /// api.openweathermap.org/data/2.5/weather?zip={zip code},{country code} 
        /// </summary>
        ZipCode
    }
}
