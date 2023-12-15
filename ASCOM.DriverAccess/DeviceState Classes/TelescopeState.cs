using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class TelescopeState
    {
        // Assign the name of this class
        readonly string className = nameof(TelescopeState);

        /// <summary>
        /// Create a new TelescopeState instance
        /// </summary>
        public TelescopeState() { }

        /// <summary>
        /// Create a new TelescopeState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public TelescopeState(IStateValueCollection deviceStateArrayList, object TL)
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
                        case nameof(ITelescopeV4.Altitude):
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

                        case nameof(ITelescopeV4.AtHome):
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

                        case nameof(ITelescopeV4.AtPark):
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

                        case nameof(ITelescopeV4.Azimuth):
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

                        case nameof(ITelescopeV4.Declination):
                            try
                            {
                                Declination = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Declination - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Declination has value: {Declination.HasValue}, Value: {Declination}");
                            break;

                        case nameof(ITelescopeV4.IsPulseGuiding):
                            try
                            {
                                IsPulseGuiding = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"IsPulseGuiding - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"IsPulseGuiding has value: {IsPulseGuiding.HasValue}, Value: {IsPulseGuiding}");
                            break;

                        case nameof(ITelescopeV4.RightAscension):
                            try
                            {
                                RightAscension = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"RightAscension - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"RightAscension has value: {RightAscension.HasValue}, Value: {RightAscension}");
                            break;

                        case nameof(ITelescopeV4.SideOfPier):
                            try
                            {
                                SideOfPier = (PierSide)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"SideOfPier - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"SideOfPier has value: {SideOfPier.HasValue}, Value: {SideOfPier}");
                            break;

                        case nameof(ITelescopeV4.SiderealTime):
                            try
                            {
                                SiderealTime = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"SiderealTime - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"SiderealTime has value: {SiderealTime.HasValue}, Value: {SiderealTime}");
                            break;

                        case nameof(ITelescopeV4.Slewing):
                            try
                            {
                                Slewing = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Slewing - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Slewing has value: {Slewing.HasValue}, Value: {Slewing}");
                            break;

                        case nameof(ITelescopeV4.Tracking):
                            try
                            {
                                Tracking = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Tracking - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Tracking has value: {Tracking.HasValue}, Value: {Tracking}");
                            break;

                        case nameof(ITelescopeV4.UTCDate):
                            try
                            {
                                UTCDate = (DateTime)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"UTCDate - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"UTCDate has value: {UTCDate.HasValue}, Value: {UTCDate}");
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
        public double? Altitude { get; set; } = null;

        /// <summary>
        /// Telescope is at home
        /// </summary>
        public bool? AtHome { get; set; } = null;

        /// <summary>
        /// Telescope is parked
        /// </summary>
        public bool? AtPark { get; set; } = null;

        /// <summary>
        /// Telescope azimuth
        /// </summary>
        public double? Azimuth { get; set; } = null;

        /// <summary>
        /// Telescope declination
        /// </summary>
        public double? Declination { get; set; } = null;

        /// <summary>
        /// Telescope is pulse guiding
        /// </summary>
        public bool? IsPulseGuiding { get; set; } = null;

        /// <summary>
        /// Telescope right ascension
        /// </summary>
        public double? RightAscension { get; set; } = null;

        /// <summary>
        /// Telescope pointing state
        /// </summary>
        public PierSide? SideOfPier { get; set; } = null;

        /// <summary>
        /// Telescope sidereal time
        /// </summary>
        public double? SiderealTime { get; set; } = null;

        /// <summary>
        /// Telescope is slewing
        /// </summary>
        public bool? Slewing { get; set; } = null;

        /// <summary>
        /// Telescope  is tracking
        /// </summary>
        public bool? Tracking { get; set; } = null;

        /// <summary>
        /// Telescope UTC date and time
        /// </summary>
        public DateTime? UTCDate { get; set; } = null;

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

