using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class RotatorState
    {
        // Assign the name of this class
        readonly string className = nameof(RotatorState);

        /// <summary>
        /// Create a new RotatorState instance
        /// </summary>
        public RotatorState() { }

        /// <summary>
        /// Create a new RotatorState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public RotatorState(IEnumerable deviceStateArrayList, object TL)
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
                        case nameof(IRotatorV4.IsMoving):
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

                        case nameof(IRotatorV4.MechanicalPosition):
                            try
                            {
                                MechanicalPosition = (float)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"MechanicalPosition - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"MechanicalPosition has value: {MechanicalPosition.HasValue}, Value: {MechanicalPosition}");
                            break;

                        case nameof(IRotatorV4.Position):
                            try
                            {
                                Position = (float)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Position - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Position has value: {Position.HasValue}, Value: {Position}");
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
        /// Rotator is moving state
        /// </summary>
        public bool? IsMoving{ get; set; } = null;

        /// <summary>
        /// Rotator position
        /// </summary>
        public float? Position { get; set; } = null;

        /// <summary>
        /// Rotator mechanical position
        /// </summary>
        public float? MechanicalPosition { get; set; } = null;

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