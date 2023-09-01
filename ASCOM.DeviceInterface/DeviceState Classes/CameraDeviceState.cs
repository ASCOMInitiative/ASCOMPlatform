using System;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.DeviceInterface.DeviceState
{
    /// <summary>
    /// Class that presents the device's operation state as a set of nullable properties
    /// </summary>
    public class CameraDeviceState
    {
        // Assign the name of this class
        readonly string className = nameof(CameraDeviceState);

        /// <summary>
        /// Create a new CameraDeviceState instance
        /// </summary>
        public CameraDeviceState() { }

        /// <summary>
        /// Create a new CameraDeviceState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance. The type of this parameter is Object - see remarks.</param>
        /// <remarks>This class supports .NET Framework 3.5, 4.x and .NET Standard 2.0. In order to avoid use of dynamic and inclusion of projects or packages that define the TraceLogger
        /// component, the TL parameter is typed as an object and a reflection method is used to call the LogMessage member.</remarks>
        public CameraDeviceState(ArrayList deviceStateArrayList, object TL)
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
                        case nameof(ICameraV4.CameraState):
                            try
                            {
                                CameraState = (CameraStates)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CameraState - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CameraState has value: {CameraState.HasValue}, Value: {CameraState}");
                            break;

                        case nameof(ICameraV4.CCDTemperature):
                            try
                            {
                                CCDTemperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CCDTemperature - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CCDTemperature has value: {CCDTemperature.HasValue}, Value: {CCDTemperature}");
                            break;

                        case nameof(ICameraV4.CoolerPower): 
                            try
                            {
                                CoolerPower = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"CoolerPower - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"CoolerPower has value: {CoolerPower.HasValue}, Value: {CoolerPower}");
                            break;

                        case nameof(ICameraV4.HeatSinkTemperature):
                            try
                            {
                                HeatSinkTemperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"HeatSinkTemperature - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"HeatSinkTemperature has value: {HeatSinkTemperature.HasValue}, Value: {HeatSinkTemperature}");
                            break;

                        case nameof(ICameraV4.ImageReady):
                            try
                            {
                                ImageReady = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"ImageReady - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"ImageReady has value: {ImageReady.HasValue}, Value: {ImageReady}");
                            break;

                        case nameof(ICameraV4.IsPulseGuiding):
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

                        case nameof(ICameraV4.PercentCompleted):
                            try
                            {
                                PercentCompleted = (short)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                LogMessage(className, $"PercentCompleted - Ignoring exception: {ex.Message}");
                            }
                            LogMessage(className, $"PercentCompleted has value: {PercentCompleted.HasValue}, Value: {PercentCompleted}");
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
        /// The device's CameraState
        /// </summary>
        public CameraStates? CameraState { get; set; } = null;

        /// <summary>
        /// The device's CCDTemperature
        /// </summary>
        public double? CCDTemperature { get; set; } = null;

        /// <summary>
        /// The device's CoolerPower
        /// </summary>
        public double? CoolerPower { get; set; } = null;

        /// <summary>
        /// The device's HeatSinkTemperature
        /// </summary>
        public double? HeatSinkTemperature { get; set; } = null;

        /// <summary>
        /// The device's ImageReady property
        /// </summary>
        public bool? ImageReady { get; set; } = null;

        /// <summary>
        /// The device's IsPulseGuiding property
        /// </summary>
        public bool? IsPulseGuiding { get; set; } = null;

        /// <summary>
        /// The device's PercentCompleted property
        /// </summary>
        public double? PercentCompleted { get; set; } = null;

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
