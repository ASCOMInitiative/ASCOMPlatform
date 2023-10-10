using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Device type name functions
    /// </summary>
    public static class Devices
    {
        /// <summary>
        /// Returns a list of valid ASCOM device type names
        /// </summary>
        /// <returns>String list of ASCOM device types.</returns>
        public static List<string> DeviceTypeNames()
        {
            return Enum.GetNames(typeof(AscomDeviceTypes)).ToList();
        }

        /// <summary>
        /// Confirms that the supplied device type is a valid ASCOM device type.
        /// </summary>
        /// <param name="deviceType">Device type name to assess.</param>
        /// <returns>Returns true if the supplied type is a valid ASCOM device type, otherwise returns false.</returns>
        public static bool IsValidDeviceType(AscomDeviceTypes deviceType)
        {
            return Enum.IsDefined(typeof(AscomDeviceTypes), deviceType);
        }

        /// <summary>
        /// Confirms that the supplied device type string name is valid
        /// </summary>
        /// <param name="deviceTypeName">Device type name as a string</param>
        /// <returns>True if the supplied name is a valid ASCOM device type, otherwise returns false.</returns>
        public static bool IsValidDeviceTypeName(string deviceTypeName)
        {
            try
            {
                deviceTypeName.ToDeviceType();
                return true;
            }
            catch 
            {
                return false;
            }
        }

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
        [ComVisible(false)]
        public static bool HasConnectAndDeviceState(AscomDeviceTypes deviceType, short driverInterfaceVersion)
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
        [ComVisible(false)]
        public static bool HasConnectAndDeviceState(AscomDeviceTypes deviceType, int driverInterfaceVersion)
        {
            // Switch on the type of this DriverAccess object
            switch (deviceType)
            {
                // True if interface version is greater than 3
                case AscomDeviceTypes.Camera:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case AscomDeviceTypes.CoverCalibrator:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 2
                case AscomDeviceTypes.Dome:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case AscomDeviceTypes.FilterWheel:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case AscomDeviceTypes.Focuser:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case AscomDeviceTypes.ObservingConditions:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 3
                case AscomDeviceTypes.Rotator:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case AscomDeviceTypes.SafetyMonitor:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case AscomDeviceTypes.Switch:
                    if (driverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case AscomDeviceTypes.Telescope:
                    if (driverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case AscomDeviceTypes.Video:
                    if (driverInterfaceVersion > 1)
                        return true;
                    break;

                default:
                    throw new InvalidOperationException($"DeviceCapabillities.HasConnectAndDeviceState - Unsupported device type: {deviceType}. The Platform code needs to be updated to add support.");
            }

            // Device has a Platform 6 or earlier interface
            return false;
        }

        #region Extension methods

        /// <summary>
        /// Extension method that converts a string device name to a <see cref="AscomDeviceTypes"/> enum value.
        /// </summary>
        /// <param name="device">Device type</param>
        /// <returns>DeviceTypes enum value corresponding to the given string device name</returns>
        /// <exception cref="InvalidValueException">If the supplied device type is not valid.</exception>
        public static AscomDeviceTypes ToDeviceType(this string device)
        {
            // Try to parse the supplied string device type name
            try
            {
                return (AscomDeviceTypes)Enum.Parse(typeof(AscomDeviceTypes), device); // Parsed successfully so return the enum value
            }
            catch (Exception ex)// Bad value to return an exception
            {
                // Failed to parse so return an exception and include the parser exception
                throw new InvalidOperationException($"Devices.ToDeviceType - Device type: {device} is not an ASCOM device type. Parser failure message: {ex.Message}.", ex);
            }
        }

        /// <summary>
        /// Extension method that converts a <see cref="AscomDeviceTypes"/> enum value to a string
        /// </summary>
        /// <param name="deviceType">Device type</param>
        /// <returns>String device type name corresponding to the given DeviceTypes enum value</returns>
        /// <exception cref="InvalidValueException">If the supplied device type is not valid.</exception>
        public static string ToDeviceString(this AscomDeviceTypes deviceType)
        {
            string deviceTypeString = Enum.GetName(typeof(AscomDeviceTypes), deviceType);

            // Validate the supplied DeviceTypes enum value
            if (deviceTypeString != null) return deviceTypeString; // OK

            // Bad value to return an exception
            throw new InvalidOperationException($"Devices.ToDeviceString - Supplied DeviceType enum value {(int)deviceType} is not a valid member of the DeviceTypes enum.");
        }

        #endregion

    }
}
