namespace ASCOM.DynamicRemoteClients
{
    sealed class DynamicDriverRegistration
    {

        /// <summary>
        /// Device ProgID
        /// </summary>
        public string ProgId { get; set; }

        /// <summary>
        /// Dynamic driver number used in the ProgID e.g. ASCOM.AlpacaDynamic{Number}.Telescope
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// ASCOM device type e.g. telescope or Focuser
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// Device IP address or host name
        /// </summary>
        public string IPAdrress { get; set; }

        /// <summary>
        /// IP port on which the Alpaca device can be accessed
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// Alpaca device number used to access the device e.g. /api/v1/Telescope/{RemoteDeviceNumber}/...
        /// </summary>
        public int RemoteDeviceNumber { get; set; }

        /// <summary>
        /// Alpaca device's unique ID
        /// </summary>
        public string UniqueID { get; set; }

        /// <summary>
        /// Alpaca device's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alpaca device's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Flag indicating whether the device is installed correctly. i.e. the driver DLL exists and the driver is both COM registered and ASCOM registered.
        /// </summary>
        public InstallationState InstallState { get; set; }

        /// <summary>
        /// The ToString method is used by the checked list box control to provide the display description 
        /// </summary>
        /// <returns>Alpaca device description</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
