using System;
using System.Runtime.InteropServices;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Utilities
{

    /// <summary>
/// Data object describing a single ASCOM device as returned within the <see cref="AlpacaDevice"/> class.
/// </summary>
    [Guid("AAB2619C-69FC-4717-8F01-10228D59E99E")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ConfiguredDevice : IConfiguredDevice
    {

        // State variables
        private string deviceNameValue, deviceTypeValue, uniqueIDValue;

        /// <summary>
    /// Initialises the class with default values
    /// </summary>
        public ConfiguredDevice()
        {
            // Initialise all strings to empty values
            FixNullStrings();
        }

        /// <summary>
    /// Initialise the device name, device type, device number and ASCOM device unique ID
    /// </summary>
    /// <param name="deviceName">ASCOM device name</param>
    /// <param name="deviceType">ASCOM device type</param>
    /// <param name="deviceNumber">Alpaca API device number</param>
    /// <param name="uniqueID">ASCOM device unique ID</param>
        internal ConfiguredDevice(string deviceName, string deviceType, int deviceNumber, string uniqueID)
        {
            deviceNameValue = deviceName;
            deviceTypeValue = deviceType;
            DeviceNumber = deviceNumber;
            uniqueIDValue = uniqueID;

            // Ensure that none of the strings are null
            FixNullStrings();
        }

        /// <summary>
    /// ASCOM device name
    /// </summary>
        public string DeviceName
        {
            get
            {
                return deviceNameValue;
            }
            set
            {
                deviceNameValue = value;
                // Protect against null values being assigned
                if (string.IsNullOrEmpty(deviceNameValue))
                    deviceNameValue = "";
            }
        }

        /// <summary>
    /// ASCOM device type
    /// </summary>
        public string DeviceType
        {
            get
            {
                return deviceTypeValue;
            }
            set
            {
                deviceTypeValue = value;
                // Protect against null values being assigned
                if (string.IsNullOrEmpty(deviceTypeValue))
                    deviceTypeValue = "";
            }
        }

        /// <summary>
    /// Device number used to access the device through the Alpaca API
    /// </summary>
        public int DeviceNumber { get; set; }

        /// <summary>
    /// ASCOM device unique ID
    /// </summary>
        public string UniqueID
        {
            get
            {
                return uniqueIDValue;
            }
            set
            {
                uniqueIDValue = value;
                // Protect against null values being assigned
                if (string.IsNullOrEmpty(uniqueIDValue))
                    uniqueIDValue = "";
            }
        }

        /// <summary>
    /// Make sure that all properties return a string i.e. that they do not return "null" or an unhelpful empty string
    /// </summary>
        private void FixNullStrings()
        {
            if (string.IsNullOrEmpty(deviceNameValue))
                deviceNameValue = "";
            if (string.IsNullOrEmpty(deviceTypeValue))
                deviceTypeValue = "";
            if (string.IsNullOrEmpty(uniqueIDValue))
                uniqueIDValue = "";
        }
    }
}