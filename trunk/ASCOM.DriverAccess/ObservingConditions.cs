using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region ObservingConditions wrapper
    /// <summary>
    /// Provides universal access to ObservingConditions drivers
    /// </summary>
    public class ObservingConditions : AscomDriver, IObservingConditions
    {
        #region ObservingConditions constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates an ObservingConditions object with the given Prog ID
        /// </summary>
        /// <param name="observingConditionsId">ProgID of the device to be accessed.</param>
        public ObservingConditions(string observingConditionsId)
            : base(observingConditionsId)
        {
            _memberFactory = MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a ObservingConditions
        /// </summary>
        /// <param name="observingConditionsId">ObservingConditions Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen ObservingConditions or null for none</returns>
        public static string Choose(string observingConditionsId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "ObservingConditions";
                return chooser.Choose(observingConditionsId);
            }
        }

        #endregion

        #region ObservingConditions Properties

        /// <summary>
        /// Gets and sets the time period over which observations wil be averaged
        /// </summary>
        /// <value>Time period (hours) over which to average sensor readings</value>
        /// <remarks>
        /// Time period (hours) over which the property values will be averaged 0.0 = current value, 0.5= average for the last 30 minutes, 1.0 = average for the last hour
        /// </remarks>
        public double AveragePeriod
        {
            get { return (double)_memberFactory.CallMember(1, "AveragePeriod", new Type[] { }, new object[] { }); }
            set { _memberFactory.CallMember(2, "AveragePeriod", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        /// <value>percentage of the sky covered by cloud</value>
        /// <remarks>0%= clear sky, 100% = 100% cloud coverage</remarks>
        public double CloudCover
        {
            get { return (double)_memberFactory.CallMember(1, "CloudCover", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory
        /// </summary>
        /// <value>Atmospheric dew point reported in °C.</value>
        /// <remarks>Normally optional but mandatory if <see cref="Humidity"/> is provided</remarks>
        public double DewPoint
        {
            get { return (double)_memberFactory.CallMember(1, "DewPoint", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric humidity at the observatory
        /// </summary>
        /// <value>Atmospheric humidity (%)</value>
        /// <remarks>Normally optional but mandatory if <see cref="DewPoint"/> is provided</remarks>
        public double Humidity
        {
            get { return (double)_memberFactory.CallMember(1, "Humidity", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory
        /// </summary>
        /// <value>Atmospheric presure at the observatory(hPa)</value>
        /// <remarks>This must be the pressure at the observatory and not the "redued" pressure at sea level. Please check what your pressure sensor delivers and adjust to observatory altitude if required.</remarks>
        public double Pressure
        {
            get { return (double)_memberFactory.CallMember(1, "Pressure", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <value>Rain rate (mm / hour)</value>
        /// <remarks>This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.</remarks>
        public double RainRate
        {
            get { return (double)_memberFactory.CallMember(1, "RainRate", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        /// <value>Sky quality measured in magnitudes per square arc second</value>
        /// <remarks></remarks>
        public double SkyQuality
        {
            get { return (double)_memberFactory.CallMember(1, "SkyQuality", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Seeing at the observatory
        /// </summary>
        /// <value>Seeing reported as star full width half magnitude (arc seconds) </value>
        public double SkySeeing
        {
            get { return (double)_memberFactory.CallMember(1, "SkySeeing", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sky temperature at the observatory
        /// </summary>
        /// <value>Sky temperature in °C</value>
        /// <remarks></remarks>
        public double SkyTemperature
        {
            get { return (double)_memberFactory.CallMember(1, "SkyTemperature", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Temperature at the observatory
        /// </summary>
        /// <value>Temperature in °C</value>
        /// <remarks></remarks>
        public double Temperature
        {
            get { return (double)_memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Wind direction at the observatory
        /// </summary>
        /// <value>Wind direction (degrees, 0..360.0)</value>
        /// <remarks>0..360.0, 360=N, 180=S, 90=E, 270=W. When there is no wind the driver will return a value of 0 for wind direction</remarks>
        public double WindDirection
        {
            get { return (double)_memberFactory.CallMember(1, "WindDirection", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes
        /// </summary>
        /// <value>Wind gust (m/s) Peak 3 second wind speed over the last 2 minutes</value>
        /// <remarks></remarks>
        public double WindGust
        {
            get { return (double)_memberFactory.CallMember(1, "WindGust", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Wind speed at the observatory
        /// </summary>
        /// <value>Wind speed (m/s)</value>
        /// <remarks></remarks>
        public double WindSpeed
        {
            get { return (double)_memberFactory.CallMember(1, "WindSpeed", new Type[] { }, new object[] { }); }
        }

        #endregion

        #region ObservingConditions methods

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="PropertyName">Name of the property whose time since last update is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        /// <remarks>PropertyName should be one of the sensor properties or empty string to get the last update of any parameter. A negative value indicates no valid value ever received.</remarks>
        public double TimeSinceLastUpdate(string PropertyName)
        {
            return (double)_memberFactory.CallMember(3, "TimeSinceLastUpdate", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="PropertyName">Name of the property whose sensor description is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        /// <remarks>PropertyName should be one of the sensor properties or empty string to get the last update of any parameter. A negative value indicates no valid value ever received.</remarks>
        public string SensorDescription(string PropertyName)
        {
            return (string)_memberFactory.CallMember(3, "SensorDescription", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <summary>
        /// Forces the driver to immediatley query its atatched hardware to refersh sensor values
        /// </summary>
        public void Refresh()
        {
            _memberFactory.CallMember(3, "Refresh", new Type[] { }, new object[] { });
        }

        #endregion
    }
    #endregion

}
