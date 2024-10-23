using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    /// <summary>
    /// Provides universal access to ObservingConditions drivers.
    /// Defines the IObservingConditions Interface. This interface provides a limited set of values that are useful
    /// for astronomical purposes for things such as determining if it is safe to open or operate the observing system,
    /// for recording astronomical data or determining refraction corrections.
    /// </summary>
    /// <remarks>It is NOT intended as a general purpose environmental sensor system.
    /// The <see cref="IObservingConditionsV2.Action">Action</see> method and 
    /// <see cref="IObservingConditionsV2.SupportedActions">SupportedActions</see> property 
    /// can be used to add driver-specific sensors.
    /// </remarks>
    public class ObservingConditions : AscomDriver, IObservingConditions, IObservingConditionsV2
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

        /// <summary>
        /// ObservingConditions device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public ObservingConditionsState ObservingConditionsState
        {
            get
            {
                // Create a state object to return.
                ObservingConditionsState observingConditionsState = new ObservingConditionsState(DeviceState, TL);
                TL.LogMessage(nameof(ObservingConditionsState), $"Returning: " +
                    $"Cloud cover: '{observingConditionsState.CloudCover}', " +
                    $"Dew point: '{observingConditionsState.DewPoint}', " +
                    $"Humidity: '{observingConditionsState.Humidity}', " +
                    $"Pressure: '{observingConditionsState.Pressure}', " +
                    $"Rain rate: '{observingConditionsState.RainRate}', " +
                    $"Sky brightness: '{observingConditionsState.SkyBrightness}', " +
                    $"Sky quality: '{observingConditionsState.SkyQuality}', " +
                    $"Sky temperature'{observingConditionsState.SkyTemperature}', " +
                    $"Star FWHM: '{observingConditionsState.StarFWHM}', " +
                    $"Temperature: '{observingConditionsState.Temperature}', " +
                    $"Wind direction: '{observingConditionsState.WindDirection}', " +
                    $"Wind gust: '{observingConditionsState.WindGust}', " +
                    $"Wind speed: '{observingConditionsState.WindSpeed}', " +
                    $"Time stamp: '{observingConditionsState.TimeStamp}'");

                // Return the device specific state class
                return observingConditionsState;
            }
        }

        #endregion

        #region ObservingConditions Properties

        /// <inheritdoc/>
        public double AveragePeriod
        {
            get { return (double)_memberFactory.CallMember(1, "AveragePeriod", new Type[] { }, new object[] { }); }
            set { _memberFactory.CallMember(2, "AveragePeriod", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double CloudCover
        {
            get { return (double)_memberFactory.CallMember(1, "CloudCover", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double DewPoint
        {
            get { return (double)_memberFactory.CallMember(1, "DewPoint", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double Humidity
        {
            get { return (double)_memberFactory.CallMember(1, "Humidity", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double Pressure
        {
            get { return (double)_memberFactory.CallMember(1, "Pressure", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double RainRate
        {
            get { return (double)_memberFactory.CallMember(1, "RainRate", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double SkyBrightness
        {
            get { return (double)_memberFactory.CallMember(1, "SkyBrightness", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double SkyQuality
        {
            get { return (double)_memberFactory.CallMember(1, "SkyQuality", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double StarFWHM
        {
            get { return (double)_memberFactory.CallMember(1, "StarFWHM", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double SkyTemperature
        {
            get { return (double)_memberFactory.CallMember(1, "SkyTemperature", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double Temperature
        {
            get { return (double)_memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double WindDirection
        {
            get { return (double)_memberFactory.CallMember(1, "WindDirection", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double WindGust
        {
            get { return (double)_memberFactory.CallMember(1, "WindGust", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public double WindSpeed
        {
            get { return (double)_memberFactory.CallMember(1, "WindSpeed", new Type[] { }, new object[] { }); }
        }

        #endregion

        #region ObservingConditions methods

        /// <inheritdoc/>
        public double TimeSinceLastUpdate(string PropertyName)
        {
            return (double)_memberFactory.CallMember(3, "TimeSinceLastUpdate", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <inheritdoc/>
        public string SensorDescription(string PropertyName)
        {
            return (string)_memberFactory.CallMember(3, "SensorDescription", new Type[] { typeof(string) }, new object[] { PropertyName });
        }

        /// <inheritdoc/>
        public void Refresh()
        {
            _memberFactory.CallMember(3, "Refresh", new Type[] { }, new object[] { });
        }

        #endregion
    }
}
