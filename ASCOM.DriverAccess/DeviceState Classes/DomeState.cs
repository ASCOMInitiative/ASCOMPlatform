using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class DomeState
    {
        // Assign the name of this class
        readonly string className = nameof(DomeState);

        /// <summary>
        /// Create a new DomeState instance
        /// </summary>
        public DomeState() { }

        /// <summary>
        /// Create a new DomeState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public DomeState(IStateValueCollection deviceStateArrayList, object TL)
        {
            LogMessage(className, $"Received {deviceStateArrayList.Count} items");

            List<IStateValue> deviceState = new List<IStateValue>();

            // Handle null ArrayList
            if (deviceStateArrayList is null) // No ArrayList was supplied so return
            {
                LogMessage(className, $"Supplied device state ArrayList is null, all values will be unknown.");
                return;
            }

            LogMessage(className, $"ArrayList from device contained {deviceStateArrayList.Count} DeviceSate items.");

            // An ArrayList was supplied so process each supplied value
            foreach (IStateValue stateValue in deviceStateArrayList)
            {
                try
                {
                    LogMessage(className, $"{stateValue.Name} = {stateValue.Value}");
                    deviceState.Add(new StateValue(stateValue.Name, stateValue.Value));

                    switch (stateValue.Name)
                    {
                        case nameof(IDomeV3.Altitude):
                            try
                            {
                                Altitude = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Altitude - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Altitude has value: {Altitude.HasValue}, Value: {Altitude}");
                            break;

                        case nameof(IDomeV3.AtHome):
                            try
                            {
                                AtHome = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"AtHome - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"AtHome has value: {AtHome.HasValue}, Value: {AtHome}");
                            break;

                        case nameof(IDomeV3.AtPark):
                            try
                            {
                                AtPark = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"AtPark - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"AtPark has value: {AtPark.HasValue}, Value: {AtPark}");
                            break;

                        case nameof(IDomeV3.Azimuth):
                            try
                            {
                                Azimuth = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Azimuth - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Azimuth has value: {Azimuth.HasValue}, Value: {Azimuth}");
                            break;

                        case nameof(IDomeV3.ShutterStatus):
                            try
                            {
                                ShutterStatus = (ShutterState)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Declination - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"ShutterStatus has value: {ShutterStatus.HasValue}, Value: {ShutterStatus}");
                            break;

                        case nameof(IDomeV3.Slewing):
                            try
                            {
                                Slewing = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Slewing - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Slewing has value: {Slewing.HasValue}, Value: {Slewing.Value}");
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
        /// Dome altitude
        /// </summary>
        public double? Altitude { get; set; } = null;

        /// <summary>
        /// Dome is at home
        /// </summary>
        public bool? AtHome { get; set; } = null;

        /// <summary>
        /// Dome is parked
        /// </summary>
        public bool? AtPark { get; set; } = null;

        /// <summary>
        /// Dome azimuth
        /// </summary>
        public double? Azimuth { get; set; } = null;

        /// <summary>
        /// Dome shutter state
        /// </summary>
        public ShutterState? ShutterStatus { get; set; } = null;

        /// <summary>
        /// Dome is slewing
        /// </summary>
        public bool? Slewing { get; set; } = null;

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