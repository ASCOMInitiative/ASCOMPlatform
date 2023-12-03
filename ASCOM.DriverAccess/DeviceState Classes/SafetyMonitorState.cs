using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class SafetyMonitorState
    {
        readonly Type traceLoggerType;
        readonly object TL;

        // Assign the name of this class
        readonly string className = nameof(SafetyMonitorState);

        /// <summary>
        /// Create a new FocuserState instance
        /// </summary>
        public SafetyMonitorState() { }

        /// <summary>
        /// Create a new FocuserState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public SafetyMonitorState(ArrayList deviceStateArrayList, object TL) // 
        {
            // SAve the TraceLogger and it's type
            this.TL = TL;
            traceLoggerType = TL.GetType();

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
                        case nameof(ISafetyMonitorV3.IsSafe):
                            try
                            {
                                IsSafe = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"IsSafe - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"IsSafe has value: {IsSafe.HasValue}, Value: {IsSafe}");
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
        public bool? IsSafe { get; set; } = null;

        /// <summary>
        /// The time at which the state was recorded
        /// </summary>
        public DateTime? TimeStamp { get; set; } = null;
        #region Private methods

        private void LogMessage(string method, string message)
        {
            // Create a parameter object array containing the two parameters
            object[] parms = new object[] { method, message };

            try
            {
            // Invoke the LogMessage method.
            object result = traceLoggerType.InvokeMember("LogMessage",
                                                           System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod,
                                                           null,
                                                           TL,
                                                           parms,
                                                           System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                // Remove any TargetInvocationException wrapper and throw the real exception
                throw e.InnerException;
            }
        }

        #endregion
    }
}

