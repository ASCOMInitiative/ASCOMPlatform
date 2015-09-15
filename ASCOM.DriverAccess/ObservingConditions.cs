using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region ObservingConditions wrapper
    /// <summary>
    /// Provides universal access to ObservingConditions drivers.
    /// Defines the IObservingConditions Interface. This interface provides a limited set of values that are useful
    /// for astronomical purposes for things such as determining if it is safe to open or operate the observing system
    /// and for recording astronomical data or determining refraction corrections.
    /// </summary>
    /// <remarks>It is NOT intended as a general purpose environmental sensor system.
    /// The <see cref="IObservingConditions.Action">Action</see> method and 
    /// <see cref="IObservingConditions.SupportedActions">SupportedActions</see> property 
    /// can be used to add driver specific sensors.
    /// </remarks>
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
        /// Brings up the ASCOM Chooser Dialog to choose an ObservingConditions driver.
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
        /// Gets and sets the time period over which observations will be averaged
        /// </summary>
        /// <value>Time period (hours) over which to average sensor readings</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
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
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>0%= clear sky, 100% = 100% cloud coverage</remarks>
        public double CloudCover
        {
            get { return (double)_memberFactory.CallMember(1, "CloudCover", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory
        /// </summary>
        /// <value>Atmospheric dew point reported in °C.</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>Normally optional but mandatory if <see cref="Humidity"/> is provided</remarks>
        public double DewPoint
        {
            get { return (double)_memberFactory.CallMember(1, "DewPoint", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric humidity at the observatory
        /// </summary>
        /// <value>Atmospheric humidity (%)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>Normally optional but mandatory if <see cref="DewPoint"/> is provided</remarks>
        public double Humidity
        {
            get { return (double)_memberFactory.CallMember(1, "Humidity", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory
        /// </summary>
        /// <value>Atmospheric presure at the observatory (hPa)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>This must be the pressure at the observatory and not the "reduced" pressure at sea level.
        /// Please check whether your pressure sensor delivers local pressure or sea level pressure
        /// and adjust if required to observatory pressure.</remarks>
        public double Pressure
        {
            get { return (double)_memberFactory.CallMember(1, "Pressure", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <value>Rain rate (mm / hour)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.</remarks>
        public double RainRate
        {
            get { return (double)_memberFactory.CallMember(1, "RainRate", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        /// <value>Sky brightness (Lux)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks></remarks>
        public double SkyBrightness
        {
            get { return (double)_memberFactory.CallMember(1, "SkyBrightness", new Type[] { }, new object[] { }); }
        }
        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        /// <value>Sky quality measured in magnitudes per square arc second</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks></remarks>
        public double SkyQuality
        {
            get { return (double)_memberFactory.CallMember(1, "SkyQuality", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Seeing at the observatory as FWHM in arc secs.
        /// </summary>
        /// <value>Seeing reported as star full width half magnitude (arc seconds)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        public double SkySeeing
        {
            get { return (double)_memberFactory.CallMember(1, "SkySeeing", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sky temperature at the observatory
        /// </summary>
        /// <value>Sky temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>This is expected to be returned by an infra ref sensor looking at the sky.
        /// The lower the temperature the more the sky is likely to be clear.</remarks>
        public double SkyTemperature
        {
            get { return (double)_memberFactory.CallMember(1, "SkyTemperature", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Temperature at the observatory
        /// </summary>
        /// <value>Temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>This is expected to be the ambient temperature.</remarks>
        public double Temperature
        {
            get { return (double)_memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Wind direction at the observatory
        /// </summary>
        /// <value>Wind direction (degrees, 0..360.0)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>0..360.0, 360=N, 180=S, 90=E, 270=W. When there is no wind the driver will return a value of 0 for wind direction</remarks>
        public double WindDirection
        {
            get { return (double)_memberFactory.CallMember(1, "WindDirection", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes
        /// </summary>
        /// <value>Wind gust (m/s) Peak 3 second wind speed over the last 2 minutes</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks></remarks>
        public double WindGust
        {
            get { return (double)_memberFactory.CallMember(1, "WindGust", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Wind speed at the observatory
        /// </summary>
        /// <value>Wind speed (m/s)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
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
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <remarks>PropertyName should be one of the sensor properties or an empty string to get the last update of any parameter.
        /// A negative value indicates that no valid value has been received.</remarks>
        public double TimeSinceLastUpdate(string PropertyName)
        {
            return (double)_memberFactory.CallMember(3, "TimeSinceLastUpdate", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property.
        /// </summary>
        /// <param name="PropertyName">Name of the sensor whose description is required</param>
        /// <returns>The description of the specified sensor.</returns>
        /// <exception cref="MethodNotImplementedException">If the sensor is not available.</exception>
        /// <remarks>PropertyName must be the name of one of the sensor properties implemented by this driver.
        /// This must return a valid string even if the driver is not connected so that 
        /// applications can use this to determine what sensors are available.</remarks>
        public string SensorDescription(string PropertyName)
        {
            return (string)_memberFactory.CallMember(3, "SensorDescription", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <summary>
        /// Forces the driver to immediatley query its attached hardware to refresh sensor values
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="MethodNotImplementedException">If this method is not available.</exception>
        public void Refresh()
        {
            _memberFactory.CallMember(3, "Refresh", new Type[] { }, new object[] { });
        }

        #endregion
    }
    #endregion

}
