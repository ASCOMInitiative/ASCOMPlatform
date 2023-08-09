using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DeviceInterface.DeviceState
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class DomeState
    {
        // Assign the name of this class
        string className = nameof(DomeState);

        /// <summary>
        /// Create a new DomeState instance
        /// </summary>
        public DomeState() { }

        /// <summary>
        /// Create a new DomeState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance.</param>
        public DomeState(ArrayList deviceStateArrayList, TraceLogger TL)
        {
            TL?.LogMessage(className, $"Received {deviceStateArrayList.Count} items");

            List<IStateValue> deviceState = new List<IStateValue>();

            // Handle null ArrayList
            if (deviceStateArrayList is null) // No ArrayList was supplied so return
            {
                TL?.LogMessage(className, $"Supplied device state ArrayList is null, all values will be unknown.");
                return;
            }

            TL?.LogMessage(className, $"ArrayList from device contained {deviceStateArrayList.Count} DeviceSate items.");

            // An ArrayList was supplied so process each supplied value
            foreach (IStateValue stateValue in deviceStateArrayList)
            {
                try
                {
                    TL?.LogMessage(className, $"{stateValue.Name} = {stateValue.Value}");
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
                                TL.LogMessage(className, $"Altitude - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"Altitude has value: {Altitude.HasValue}, Value: {Altitude}");
                            break;

                        case nameof(IDomeV3.AtHome):
                            try
                            {
                                AtHome = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"AtHome - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"AtHome has value: {AtHome.HasValue}, Value: {AtHome}");
                            break;

                        case nameof(IDomeV3.AtPark):
                            try
                            {
                                AtPark = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"AtPark - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"AtPark has value: {AtPark.HasValue}, Value: {AtPark}");
                            break;

                        case nameof(IDomeV3.Azimuth):
                            try
                            {
                                Azimuth = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"Azimuth - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"Azimuth has value: {Azimuth.HasValue}, Value: {Azimuth}");
                            break;

                        case nameof(IDomeV3.ShutterStatus):
                            try
                            {
                                ShutterStatus = (ShutterState)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"Declination - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"ShutterStatus has value: {ShutterStatus.HasValue}, Value: {ShutterStatus}");
                            break;

                        case nameof(IDomeV3.Slewing):
                            try
                            {
                                Slewing = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"Slewing - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"Slewing has value: {Slewing.HasValue}, Value: {Slewing.Value}");
                            break;


                        case "TimeStamp":
                            try
                            {
                                TimeStamp = (DateTime)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"TimeStamp - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"TimeStamp has value: {TimeStamp.HasValue}, Value: {TimeStamp}");
                            break;

                        default:
                            TL?.LogMessage(className, $"Ignoring {stateValue.Name}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TL?.LogMessageCrLf(className, $"Exception: {ex.Message}.\r\n{ex}");
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
    }
}