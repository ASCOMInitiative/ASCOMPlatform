using System;

namespace ASCOM.Utilities.Support
{
    /// <summary>
    /// Configured device entity class
    /// </summary>
    public class ConfiguredDevice
    {
        /// <summary>
        /// Default initialiser
        /// </summary>
        public ConfiguredDevice() { }

        /// <summary>
        /// Initialise the device name, device type, device number and ASCOM device unique ID
        /// </summary>
        /// <param name="deviceName">ASCOM device name</param>
        /// <param name="deviceType">ASCOM device type</param>
        /// <param name="deviceNumber">Alpaca API device number</param>
        /// <param name="uniqueID">ASCOM device unique ID</param>
        public ConfiguredDevice(string deviceName,string deviceType,int deviceNumber,string uniqueID)
        {
            DeviceName = deviceName;
            DeviceType = deviceType;
            DeviceNumber = deviceNumber;
            UniqueID = uniqueID;
        }

        /// <summary>
        /// ASCOM device name
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// ASCOM device type
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// Alpaca API device number
        /// </summary>
        public int DeviceNumber { get; set; }

        /// <summary>
        /// ASCOM device unique ID
        /// </summary>
        public string UniqueID { get; set; }
    }
}
