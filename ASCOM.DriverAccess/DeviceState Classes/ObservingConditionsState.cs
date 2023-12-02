using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class ObservingConditionsState
    {
        // Assign the name of this class
        readonly string className = nameof(ObservingConditionsState);

        /// <summary>
        /// Create a new ObservingConditionsState instance
        /// </summary>
        public ObservingConditionsState() { }

        /// <summary>
        /// Create a new ObservingConditionsState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public ObservingConditionsState(IEnumerable deviceStateArrayList, object TL)
        {
            //LogMessage(className, $"Received {deviceStateArrayList.Count} items");

            List<IStateValue> deviceState = new List<IStateValue>();

            // Handle null ArrayList
            if (deviceStateArrayList is null) // No ArrayList was supplied so return
            {
                LogMessage(className, $"Supplied device state ArrayList is null, all values will be unknown.");
                return;
            }

            //LogMessage(className, $"ArrayList from device contained {deviceStateArrayList.Count} DeviceSate items.");

            // An ArrayList was supplied so process each supplied value
            foreach (IStateValue stateValue in deviceStateArrayList)
            {
                try
                {
                    LogMessage(className, $"{stateValue.Name} = {stateValue.Value}");
                    deviceState.Add(new StateValue(stateValue.Name, stateValue.Value));

                    switch (stateValue.Name)
                    {
                        case nameof(IObservingConditionsV2.CloudCover):
                            try
                            {
                                CloudCover = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CloudCover - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CloudCover has value: {CloudCover.HasValue}, Value: {CloudCover}");
                            break;

                        case nameof(IObservingConditionsV2.DewPoint):
                            try
                            {
                                DewPoint = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"DewPoint - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"DewPoint has value: {DewPoint.HasValue} , Value:  {DewPoint}");
                            break;

                        case nameof(IObservingConditionsV2.Humidity):
                            try
                            {
                                Humidity = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Humidity - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Humidity has value: {Humidity.HasValue} , Value:  {Humidity}");
                            break;

                        case nameof(IObservingConditionsV2.Pressure):
                            try
                            {
                                Pressure = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Pressure - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Pressure has value: {Pressure.HasValue} , Value:  {Pressure}");
                            break;

                        case nameof(IObservingConditionsV2.RainRate):
                            try
                            {
                                RainRate = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"RainRate - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"RainRate has value: {RainRate.HasValue}, Value: {RainRate}");
                            break;

                        case nameof(IObservingConditionsV2.SkyBrightness):
                            try
                            {
                                SkyBrightness = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"SkyBrightness - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"SkyBrightness has value: {SkyBrightness.HasValue}, Value: {SkyBrightness}");
                            break;

                        case nameof(IObservingConditionsV2.SkyQuality):
                            try
                            {
                                SkyQuality = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"SkyQuality - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"SkyQuality has value: {SkyQuality.HasValue}, Value: {SkyQuality}");
                            break;

                        case nameof(IObservingConditionsV2.SkyTemperature):
                            try
                            {
                                SkyTemperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"SkyTemperature - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"SkyTemperature has value: {SkyTemperature.HasValue}, Value: {SkyTemperature}");
                            break;

                        case nameof(IObservingConditionsV2.StarFWHM):
                            try
                            {
                                StarFWHM = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"StarFWHM - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"StarFWHM has value: {StarFWHM.HasValue}, Value: {StarFWHM}");
                            break;

                        case nameof(IObservingConditionsV2.Temperature):
                            try
                            {
                                Temperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Temperature - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Temperature has value: {Temperature.HasValue}, Value: {Temperature}");
                            break;

                        case nameof(IObservingConditionsV2.WindDirection):
                            try
                            {
                                WindDirection = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"WindDirection - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"WindDirection has value: {WindDirection.HasValue}, Value: {WindDirection}");
                            break;

                        case nameof(IObservingConditionsV2.WindGust):
                            try
                            {
                                WindGust = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"WindGust - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"WindGust has value: {WindGust.HasValue}, Value: {WindGust}");
                            break;

                        case nameof(IObservingConditionsV2.WindSpeed):
                            try
                            {
                                WindSpeed = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"WindSpeed - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"WindSpeed has value: {WindSpeed.HasValue}, Value: {WindSpeed}");
                            break;

                        case "TimeStamp":
                            try
                            {
                                TimeStamp = (DateTime)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"TimeStamp - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"TimeStamp has value: {TimeStamp.HasValue}, Value: {TimeStamp}");
                            break;

                        default:
                            LogMessage(className, $"Ignoring {stateValue.Name}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(className, $"Exception: {ex.Message}.\r\n{ex}");
                }
            }
        }

        /// <summary>
        /// Telescope altitude
        /// </summary>
        public double? CloudCover { get; set; } = null;

        /// <summary>
        /// Telescope is at home
        /// </summary>
        public double? DewPoint { get; set; } = null;

        /// <summary>
        /// Telescope is parked
        /// </summary>
        public double? Humidity { get; set; } = null;

        /// <summary>
        /// Telescope azimuth
        /// </summary>
        public double? Pressure { get; set; } = null;

        /// <summary>
        /// Telescope declination
        /// </summary>
        public double? RainRate { get; set; } = null;

        /// <summary>
        /// Telescope is pulse guiding
        /// </summary>
        public double? SkyBrightness { get; set; } = null;

        /// <summary>
        /// Telescope right ascension
        /// </summary>
        public double? SkyQuality { get; set; } = null;

        /// <summary>
        /// Telescope pointing state
        /// </summary>
        public double? SkyTemperature { get; set; } = null;

        /// <summary>
        /// Telescope sidereal time
        /// </summary>
        public double? StarFWHM { get; set; } = null;

        /// <summary>
        /// Telescope is slewing
        /// </summary>
        public double? Temperature { get; set; } = null;

        /// <summary>
        /// Telescope  is tracking
        /// </summary>
        public double? WindDirection { get; set; } = null;

        /// <summary>
        /// Telescope UTC date and time
        /// </summary>
        public double? WindGust { get; set; } = null;

        /// <summary>
        /// Telescope UTC date and time
        /// </summary>
        public double? WindSpeed { get; set; } = null;
        /// <summary>
        /// The time at which the state was recorded
        /// </summary>
        public DateTime? TimeStamp { get; set; } = null;
        #region Private methods

        private void LogMessage(string method, string name)
        {

        }

        #endregion

    }
}

