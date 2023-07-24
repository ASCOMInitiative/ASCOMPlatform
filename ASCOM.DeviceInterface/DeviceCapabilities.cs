using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// 
    /// </summary>
    public static class DeviceCapabilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="DriverInterfaceVersion"></param>
        /// <returns></returns>
        /// <exception cref="InvalidValueException"></exception>
        public static bool HasConnectAndDeviceState(string deviceType, short DriverInterfaceVersion)
        {
            // Switch on the type of this DriverAccess object
            switch (deviceType.ToUpperInvariant())
            {
                // True if interface version is greater than 3
                case "Camera":
                    if (DriverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case "CoverCalibrator":
                    if (DriverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 2
                case "Dome":
                    if (DriverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case "FilterWheel":
                    if (DriverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case "Focuser":
                    if (DriverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case "ObservingConditions":
                    if (DriverInterfaceVersion > 1)
                        return true;
                    break;

                // True if interface version is greater than 3
                case "Rotator":
                    if (DriverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case "SafetyMonitor":
                    if (DriverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 2
                case "Switch":
                    if (DriverInterfaceVersion > 2)
                        return true;
                    break;

                // True if interface version is greater than 3
                case "Telescope":
                    if (DriverInterfaceVersion > 3)
                        return true;
                    break;

                // True if interface version is greater than 1
                case "Video":
                    if (DriverInterfaceVersion > 1)
                        return true;
                    break;

                default:
                    throw new InvalidValueException($"DeviceCapabillities.HasConnectAndDeviceState - Unsupported device type: {deviceType}. Please update the Library code to add support.");
            }

            // Device has a Platform 6 or earlier interface
            return false;
        }

    }
}
