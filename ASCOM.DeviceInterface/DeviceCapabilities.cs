using System;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Methods showing whether specific features are available in a given device type and interface version.
    /// </summary>
    public static class DeviceCapabilities
    {
        /// <summary>
        /// Returns <see langword="true"/> if the device has a Platform 7 or later interface that supports asynchronous Switch methods
        /// </summary>
        /// <param name="driverInterfaceVersion">Interface version of this driver (Int16)</param>
        /// <returns>True if the switch supports asynchronous operations.</returns>
        /// <exception cref="InvalidOperationException">The supplied interface version is 0 or less.</exception>
        public static bool HasAsyncSwitch(int driverInterfaceVersion)
        {
            // Validate parameter
            if (driverInterfaceVersion < 1)
                throw new InvalidOperationException($"ASCOMLibrary.DeviceCapabilities.HasAsyncSwitch - Supplied interface version is 0 or negative: {driverInterfaceVersion}");

            return driverInterfaceVersion >= 3;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the device has a Platform 7 or later interface that supports Connect / Disconnect and DeviceState
        /// </summary>
        /// <returns>True if the feature is supported</returns>
        /// <param name="deviceType">Device type.</param>
        /// <param name="driverInterfaceVersion">Interface version of this driver (Int16)</param>
        public static bool HasConnectAndDeviceState(DeviceType deviceType, short driverInterfaceVersion)
        {
            return HasConnectAndDeviceState(deviceType, Convert.ToInt32(driverInterfaceVersion));
        }

        /// <summary>
        /// Returns <see langword="true"/> if the device has a Platform 7 or later interface that supports Connect / Disconnect and DeviceState
        /// </summary>
        /// <param name="deviceType">Device type.</param>
        /// <param name="driverInterfaceVersion">Interface version of this driver (Int16)</param>
        /// <returns>True if the feature is supported</returns>
        /// <exception cref="InvalidValueException">If the device type is not one of the standard ASCOM device types.</exception>
        public static bool HasConnectAndDeviceState(DeviceType deviceType, int driverInterfaceVersion)
        {
            // Switch on the type of this DriverAccess object
            switch (deviceType)
            {
                // True if interface version is greater than 3
                case DeviceType.Camera:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case DeviceType.CoverCalibrator:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 2
                case DeviceType.Dome:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case DeviceType.FilterWheel:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case DeviceType.Focuser:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case DeviceType.ObservingConditions:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 3
                case DeviceType.Rotator:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case DeviceType.SafetyMonitor:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case DeviceType.Switch:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case DeviceType.Telescope:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case DeviceType.Video:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                default:
                    throw new InvalidOperationException($"DeviceCapabillities.HasConnectAndDeviceState - Unsupported device type: {deviceType}. The Platform code needs to be updated to add support.");
            }

            // Device has a Platform 6 or earlier interface
            return false;
        }
    }
}
