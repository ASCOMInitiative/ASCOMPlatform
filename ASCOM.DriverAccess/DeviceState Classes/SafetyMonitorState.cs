using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class SafetyMonitorState
    {
        // Assign the name of this class
        string className = nameof(SafetyMonitorState);

        /// <summary>
        /// Create a new FocuserState instance
        /// </summary>
        public SafetyMonitorState() { }

        /// <summary>
        /// Create a new FocuserState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance.</param>
        public SafetyMonitorState(ArrayList deviceStateArrayList, TraceLogger TL)
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
                        case nameof(ISafetyMonitorV3.IsSafe):
                            try
                            {
                                IsSafe = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(className, $"IsSafe - Ignoring exception: {ex.Message}");
                            }
                            TL.LogMessage(className, $"IsSafe has value: {IsSafe.HasValue}, Value: {IsSafe}");
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
        /// Focuser IsMoving state
        /// </summary>
        public bool? IsSafe { get; set; } = null;

        /// <summary>
        /// The time at which the state was recorded
        /// </summary>
        public DateTime? TimeStamp { get; set; } = null;
    }
}

