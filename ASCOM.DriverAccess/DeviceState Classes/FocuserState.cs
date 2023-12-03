using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class FocuserState
    {
        // Assign the name of this class
        readonly string className = nameof(FilterWheelState);

        /// <summary>
        /// Create a new FocuserState instance
        /// </summary>
        public FocuserState() { }

        /// <summary>
        /// Create a new FocuserState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public FocuserState(ArrayList deviceStateArrayList, object TL)
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
                        case nameof(IFocuserV4.IsMoving):
                            try
                            {
                                IsMoving = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"IsMoving - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"IsMoving has value: {IsMoving.HasValue}, Value: {IsMoving}");
                            break;

                        case nameof(IFocuserV4.Position):
                            try
                            {
                                Position = (int)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Position - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Position has value: {Position.HasValue}, Value: {Position}");
                            break;

                        case nameof(IFocuserV4.Temperature):
                            try
                            {
                                Temperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Temperature - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Temperature has value: {Position.HasValue}, Value: {Position}");
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
        /// Focuser IsMoving state
        /// </summary>
        public bool? IsMoving { get; set; } = null;

        /// <summary>
        /// Focuser position
        /// </summary>
        public int? Position { get; set; } = null;

        /// <summary>
        /// Focuser temperature
        /// </summary>
        public double? Temperature { get; set; } = null;

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

