using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DeviceInterface.DeviceState
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
        /// <param name="TL">Debug TraceLogger instance.</param>
        public CoverCalibratorState(ArrayList deviceStateArrayList, TraceLogger TL)
        {
            TL?.LogMessage(className, $"Received {deviceStateArrayList.Count} items");

            List<IStateValue> deviceState = new List<IStateValue>();

            // Handle null ArrayList
            if (deviceStateArrayList is null) // No ArrayList was supplied so return
            {
                TL?.LogMessage(className, $"Supplied device state ArrayList is null, all values will be unknown.");
                return;
            }

            // An ArrayList was supplied so process each supplied value
            foreach (IStateValue stateValue in deviceStateArrayList)
            {
                try
                {
                    TL?.LogMessage(className, $"{stateValue.Name} = {stateValue.Value}");
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
                                TL?.LogMessage(className, $"Brightness - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"Brightness has value: {Brightness.HasValue}, Value: {Brightness}");
                            break;

                        case nameof(ICoverCalibratorV2.CalibratorState):
                            try
                            {
                                CalibratorState = (CalibratorStatus)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CalibratorState - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CalibratorState has value: {CalibratorState.HasValue}, Value: {CalibratorState}");
                            break;

                        case nameof(ICoverCalibratorV2.CoverState):
                            try
                            {
                                CoverState = (CoverStatus)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CoverState - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CoverState has value: {CoverState.HasValue}, Value: {CoverState}");
                            break;

                        case nameof(ICoverCalibratorV2.CalibratorReady):
                            try
                            {
                                CalibratorReady = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CalibratorReady - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CalibratorReady has value: {CalibratorReady.HasValue}, Value: {CalibratorReady}");
                            break;

                        case nameof(ICoverCalibratorV2.CoverMoving):
                            try
                            {
                                CoverMoving = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CoverMoving - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CoverMoving has value: {CoverMoving.HasValue}, Value: {CoverMoving}");
                            break;

                        case "TimeStamp":
                            try
                            {
                                TimeStamp = (DateTime)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"TimeStamp - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"TimeStamp has value: {TimeStamp.HasValue}, Value: {TimeStamp}");
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

    }
}
