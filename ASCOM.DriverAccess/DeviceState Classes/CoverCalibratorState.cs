using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class CoverCalibratorState
    {
        // Assign the name of this class
        readonly string className = nameof(CoverCalibratorState);

        /// <summary>
        /// Create a new CoverCalibratorState instance
        /// </summary>
        public CoverCalibratorState() { }

        /// <summary>
        /// Create a new CoverCalibratorState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public CoverCalibratorState(ArrayList deviceStateArrayList, object TL)
        {
            LogMessage(className, $"Received {deviceStateArrayList.Count} items");

            List<IStateValue> deviceState = new List<IStateValue>();

            // Handle null ArrayList
            if (deviceStateArrayList is null) // No ArrayList was supplied so return
            {
                LogMessage(className, $"Supplied device state ArrayList is null, all values will be unknown.");
                return;
            }

            // An ArrayList was supplied so process each supplied value
            foreach (IStateValue stateValue in deviceStateArrayList)
            {
                try
                {
                    LogMessage(className, $"{stateValue.Name} = {stateValue.Value}");
                    deviceState.Add(new StateValue(stateValue.Name, stateValue.Value));

                    switch (stateValue.Name)
                    {
                        case nameof(ICoverCalibratorV2.Brightness):
                            try
                            {
                                Brightness = (int)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"Brightness - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"Brightness has value: {Brightness.HasValue}, Value: {Brightness}");
                            break;

                        case nameof(ICoverCalibratorV2.CalibratorState):
                            try
                            {
                                CalibratorState = (CalibratorStatus)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CalibratorState - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CalibratorState has value: {CalibratorState.HasValue}, Value: {CalibratorState}");
                            break;

                        case nameof(ICoverCalibratorV2.CoverState):
                            try
                            {
                                CoverState = (CoverStatus)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CoverState - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CoverState has value: {CoverState.HasValue}, Value: {CoverState}");
                            break;

                        case nameof(ICoverCalibratorV2.CalibratorReady):
                            try
                            {
                                CalibratorReady = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CalibratorReady - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CalibratorReady has value: {CalibratorReady.HasValue}, Value: {CalibratorReady}");
                            break;

                        case nameof(ICoverCalibratorV2.CoverMoving):
                            try
                            {
                                CoverMoving = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CoverMoving - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CoverMoving has value: {CoverMoving.HasValue}, Value: {CoverMoving}");
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
        /// The device's Brightness
        /// </summary>
        public double? Brightness { get; set; } = null;

        /// <summary>
        /// The device's CalibratorState
        /// </summary>
        public CalibratorStatus? CalibratorState { get; set; } = null;

        /// <summary>
        /// The device's CoverState
        /// </summary>
        public CoverStatus? CoverState { get; set; } = null;

        /// <summary>
        /// The device's CalibratorReady state
        /// </summary>
        public bool? CalibratorReady { get; set; } = null;

        /// <summary>
        /// The device's CoverMoving state
        /// </summary>
        public bool? CoverMoving { get; set; } = null;

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
