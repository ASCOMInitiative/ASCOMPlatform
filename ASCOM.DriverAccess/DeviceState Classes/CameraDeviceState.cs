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
    public class CameraDeviceState
    {
        // Assign the name of this class
        string className = nameof(CameraDeviceState);

        /// <summary>
        /// Create a new CameraDeviceState instance
        /// </summary>
        public CameraDeviceState() { }

        /// <summary>
        /// Create a new CameraDeviceState instance from the device's DeviceState response.
        /// </summary>
        /// <param name="deviceStateArrayList">The device's DeviceState response.</param>
        /// <param name="TL">Debug TraceLogger instance.</param>
        public CameraDeviceState(ArrayList deviceStateArrayList, TraceLogger TL)
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
                        case nameof(ICameraV4.CameraState):
                            try
                            {
                                CameraState = (CameraStates)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CameraState - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CameraState has value: {CameraState.HasValue}, Value: {CameraState}");
                            break;

                        case nameof(ICameraV4.CCDTemperature):
                            try
                            {
                                CCDTemperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CCDTemperature - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CCDTemperature has value: {CCDTemperature.HasValue}, Value: {CCDTemperature}");
                            break;

                        case nameof(ICameraV4.CoolerPower): 
                            try
                            {
                                CoolerPower = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"CoolerPower - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"CoolerPower has value: {CoolerPower.HasValue}, Value: {CoolerPower}");
                            break;

                        case nameof(ICameraV4.HeatSinkTemperature):
                            try
                            {
                                HeatSinkTemperature = (double)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"HeatSinkTemperature - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"HeatSinkTemperature has value: {HeatSinkTemperature.HasValue}, Value: {HeatSinkTemperature}");
                            break;

                        case nameof(ICameraV4.ImageReady):
                            try
                            {
                                ImageReady = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"ImageReady - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"ImageReady has value: {ImageReady.HasValue}, Value: {ImageReady}");
                            break;

                        case nameof(ICameraV4.IsPulseGuiding):
                            try
                            {
                                IsPulseGuiding = (bool)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"IsPulseGuiding - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"IsPulseGuiding has value: {IsPulseGuiding.HasValue}, Value: {IsPulseGuiding}");
                            break;

                        case nameof(ICameraV4.PercentCompleted):
                            try
                            {
                                PercentCompleted = (short)stateValue.Value;
                            }
                            catch (Exception ex)
                            {
                                TL?.LogMessage(className, $"PercentCompleted - Ignoring exception: {ex.Message}");
                            }
                            TL?.LogMessage(className, $"PercentCompleted has value: {PercentCompleted.HasValue}, Value: {PercentCompleted}");
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
    }
}
