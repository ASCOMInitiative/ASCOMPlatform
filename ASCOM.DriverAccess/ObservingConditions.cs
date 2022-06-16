using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region ObservingConditions wrapper
    /// <summary>
    /// Provides universal access to ObservingConditions drivers.
    /// Defines the IObservingConditions Interface. This interface provides a limited set of values that are useful
    /// for astronomical purposes for things such as determining if it is safe to open or operate the observing system,
    /// for recording astronomical data or determining refraction corrections.
    /// </summary>
    /// <remarks>It is NOT intended as a general purpose environmental sensor system.
    /// The <see cref="IObservingConditions.Action">Action</see> method and 
    /// <see cref="IObservingConditions.SupportedActions">SupportedActions</see> property 
    /// can be used to add driver-specific sensors.
    /// </remarks>
    public class ObservingConditions : AscomDriver, IObservingConditions
    {
        private readonly MemberFactory _memberFactory;

        #region ObservingConditions constructors

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
		/// Brings up the ASCOM Chooser Dialogue to choose an ObservingConditions driver.
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
		/// <exception cref="ASCOM.InvalidValueException">If the value set is not available for this driver. All drivers must accept 0.0 to specify that
		/// an instantaneous value is available.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Mandatory property, must be implemented, can NOT throw a PropertyNotImplementedException</b></p>
		/// <para>This property should return the time period (hours) over which sensor readings will be averaged. If your driver is delivering instantaneous sensor readings this property should return a value of 0.0.</para>
		/// <para>Please resist the temptation to throw exceptions when clients query sensor properties when insufficient time has passed to get a true average reading. 
		/// A best estimate of the average sensor value should be returned in these situations. </para> 
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// This property should return a value between 0.0 and 100.0 where 0.0 = clear sky and 100.0 = 100% cloud coverage
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="Humidity"/> property also throws a PropertyNotImplementedException.</b></p>
		/// <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="Humidity"/> property is implemented.</b></p>
		/// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
		/// to convert these units to and from degrees Fahrenheit.</para>
		/// <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for 
		/// one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity"/> and 
		/// <see cref="Util.Humidity2DewPoint"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
		/// <para>This property should return a value between 0.0 and 100.0 where 0.0 = 0% relative humidity and 100.0 = 100% relative humidity.</para>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property also throws a PropertyNotImplementedException.</b></p>
		/// <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property is implemented.</b></p>
		/// <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for 
		/// one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity(Double, Double)"/> and 
		/// <see cref="Util.Humidity2DewPoint(Double, Double)"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
		/// </remarks>   
		public double Humidity
        {
            get { return (double)_memberFactory.CallMember(1, "Humidity", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Atmospheric pressure at the observatory
		/// </summary>
		/// <value>Atmospheric pressure at the observatory (hPa)</value>
		/// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// <para>The units of this property are hectoPascals. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
		/// to convert these units to and from milliBar, mm of mercury and inches of mercury.</para>
		/// <para>This must be the pressure at the observatory altitude and not the adjusted pressure at sea level.
		/// Please check whether your pressure sensor delivers local observatory pressure or sea level pressure and, if it returns sea level pressure, 
		/// adjust this to actual pressure at the observatory's altitude before returning a value to the client.
		/// The <see cref="Util.ConvertPressure"/> method can be used to effect this adjustment.
		/// </para>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// <para>The units of this property are millimetres per hour. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
		/// to convert these units to and from inches per hour.</para>
		/// <para>This property can be interpreted as 0.0 = Dry any positive non-zero value = wet.</para>
		/// <para>Rainfall intensity is classified according to the rate of precipitation:</para>
		/// <list type="bullet">
		/// <item><description>Light rain — when the precipitation rate is &lt; 2.5 mm (0.098 in) per hour</description></item>
		/// <item><description>Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) - 7.6 mm (0.30 in) or 10 mm (0.39 in) per hour</description></item>
		/// <item><description>Heavy rain — when the precipitation rate is &gt; 7.6 mm (0.30 in) per hour, or between 10 mm (0.39 in) and 50 mm (2.0 in) per hour</description></item>
		/// <item><description>Violent rain — when the precipitation rate is &gt; 50 mm (2.0 in) per hour</description></item>
		/// </list>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// This property returns the sky brightness measured in Lux.
		/// <para>Luminance Examples in Lux</para>
		/// <list type="table">
		/// <listheader>
		/// <term>Illuminance</term><term>Surfaces illuminated by:</term>
		/// </listheader>
		/// <item><description>0.0001 lux</description><description>Moonless, overcast night sky (starlight)</description></item>
		/// <item><description>0.002 lux</description><description>Moonless clear night sky with air glow</description></item>
		/// <item><description>0.27–1.0 lux</description><description>Full moon on a clear night</description></item>
		/// <item><description>3.4 lux</description><description>Dark limit of civil twilight under a clear sky</description></item>
		/// <item><description>50 lux</description><description>Family living room lights (Australia, 1998)</description></item>
		/// <item><description>80 lux</description><description>Office building hallway/toilet lighting</description></item>
		/// <item><description>100 lux</description><description>Very dark overcast day</description></item>
		/// <item><description>320–500 lux</description><description>Office lighting</description></item>
		/// <item><description>400 lux</description><description>Sunrise or sunset on a clear day.</description></item>
		/// <item><description>1000 lux</description><description>Overcast day; typical TV studio lighting</description></item>
		/// <item><description>10000–25000 lux</description><description>Full daylight (not direct sun)</description></item>
		/// <item><description>32000–100000 lux</description><description>Direct sunlight</description></item>
		/// </list>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// </remarks>
		public double SkyQuality
        {
            get { return (double)_memberFactory.CallMember(1, "SkyQuality", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Seeing at the observatory measured as the average star full width half maximum (FWHM in arc secs) within a star field.
		/// </summary>
		/// <value>Seeing reported as star full width half maximum (arc seconds)</value>
		/// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// </remarks>
		public double StarFWHM
        {
            get { return (double)_memberFactory.CallMember(1, "StarFWHM", new Type[] { }, new object[] { }); }
        }

		/// <summary>
		/// Sky temperature at the observatory
		/// </summary>
		/// <value>Sky temperature in °C</value>
		/// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
		/// to convert these units to and from degrees Fahrenheit.</para>
		/// <para>This is expected to be returned by an infra-red sensor looking at the sky. The lower the temperature the more the sky is likely to be clear.</para>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
		/// to convert these units to and from degrees Fahrenheit.</para>
		/// <para>This is expected to be the ambient temperature at the observatory.</para>
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// The returned value must be between 0.0 and 360.0, interpreted according to the meteorological standard, where a special value of 0.0 is returned when the wind speed is 0.0. 
		/// Wind direction is measured clockwise from north, through east, where East=90.0, South=180.0, West=270.0 and North=360.0.
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
		/// to convert these units to and from miles per hour or knots.
		/// </remarks>
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
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
		/// The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
		/// to convert these units to and from miles per hour or knots.
		/// </remarks>
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
		/// <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
		/// <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Must NOT throw a MethodNotImplementedException when the specified sensor is implemented but must throw a MethodNotImplementedException when the specified sensor is not implemented.</b></p>
		///<para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditions"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
		/// <para>Return a negative value to indicate that no valid value has ever been received from the hardware.</para>
		/// <para>If an empty string is supplied as the PropertyName, the driver must return the time since the most recent update of any sensor. A MethodNotImplementedException must not be thrown in this circumstance.</para>
		/// </remarks>
		public double TimeSinceLastUpdate(string PropertyName)
        {
            return (double)_memberFactory.CallMember(3, "TimeSinceLastUpdate", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

		/// <summary>
		/// Provides a description of the sensor providing the requested property.
		/// </summary>
		/// <param name="PropertyName">Name of the sensor whose description is required</param>
		/// <returns>The description of the specified sensor.</returns>
		/// <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
		/// <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Must NOT throw a MethodNotImplementedException when the specified sensor is implemented 
		/// but must throw a MethodNotImplementedException when the specified sensor is not implemented.</b></p>
		/// <para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditions"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
		/// <para>If the sensor is implemented, this must return a valid string, even if the driver is not connected, so that applications can use this to determine what sensors are available.</para>
		/// <para>If the sensor is not implemented, this must throw a MethodNotImplementedException.</para>
		/// </remarks>
		public string SensorDescription(string PropertyName)
        {
            return (string)_memberFactory.CallMember(3, "SensorDescription", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

		/// <summary>
		/// Forces the driver to immediately query its attached hardware to refresh sensor values
		/// </summary>
		/// <exception cref="MethodNotImplementedException">If this method is not available.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Optional method, can throw a MethodNotImplementedException</b></p>
		/// </remarks>
		public void Refresh()
        {
            _memberFactory.CallMember(3, "Refresh", new Type[] { }, new object[] { });
        }

        #endregion
    }

    #endregion
}
