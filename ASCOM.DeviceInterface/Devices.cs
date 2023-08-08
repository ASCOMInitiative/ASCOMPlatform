using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DeviceInterface
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
            return Enum.GetNames(typeof(DeviceType)).ToList();
        }

        /// <summary>
        /// Confirms that the supplied device type is a valid ASCOM device type.
        /// </summary>
        /// <param name="deviceType">Device type name to assess.</param>
        /// <returns>Returns true if the supplied type is a valid ASCOM device type, otherwise returns false.</returns>
        public static bool IsValidDeviceType(DeviceType deviceType)
        {
            return Enum.IsDefined(typeof(DeviceType), deviceType);
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
            catch (InvalidValueException)
            {
                return false;
            }
        }

        /// <summary>
        /// Extension method that converts a string device name to a <see cref="DeviceType"/> enum value.
        /// </summary>
        /// <param name="device">Device type</param>
        /// <returns>DeviceTypes enum value corresponding to the given string device name</returns>
        /// <exception cref="InvalidValueException">If the supplied device type is not valid.</exception>
        public static DeviceType ToDeviceType(this string device)
        {
            // Try to parse the supplied string device type name
            try
            {
                return (DeviceType)Enum.Parse(typeof(DeviceType), device); // Parsed successfully so return the enum value
            }
            catch (Exception ex)// Bad value to return an exception
            {
                // Failed to parse so return an excpeiton and include the parser exception
                throw new InvalidValueException($"Devices.ToDeviceType - Device type: {device} is not an ASCOM device type. Parser failure message: {ex.Message}.", ex);
            }
        }

        /// <summary>
        /// Extension method that converts a <see cref="DeviceType"/> enum value to a string
        /// </summary>
        /// <param name="deviceType">Device type</param>
        /// <returns>String device type name corresponding to the given DeviceTypes enum value</returns>
        /// <exception cref="InvalidValueException">If the supplied device type is not valid.</exception>
        public static string ToDeviceString(this DeviceType deviceType)
        {
            string deviceTypeString = Enum.GetName(typeof(DeviceType), deviceType);

            // Validate the supplied DeviceTypes enum value
            if (deviceTypeString != null) return deviceTypeString; // OK

            // Bad value to return an exception
            throw new InvalidValueException($"Devices.ToDeviceString - Supplied DeviceType enum value {(int)deviceType} is not a valid member of the DeviceTypes enum.");
        }
    }
}
