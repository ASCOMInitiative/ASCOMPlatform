using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ASCOM.Utilities.Support
{
    /// <summary>
    /// Alpaca device entity class
    /// </summary>
    public class AlpacaDevice
    {
        /// <summary>
        /// Default initialiser
        /// </summary>
        public AlpacaDevice() : this(new IPEndPoint(IPAddress.Any, 0), "")
        {
        }

        /// <summary>
        /// Initialise IP end point, Alpaca unique ID and Status message
        /// </summary>
        /// <param name="ipEndPoint">Alpaca device IP endpoint</param>
        /// <param name="alpacaUniqueId">Alpaca device unique ID</param>
        /// <param name="statusMessage">Device status message</param>
        public AlpacaDevice(IPEndPoint ipEndPoint, string statusMessage)
        {
            IPEndPoint = ipEndPoint;
            HostName = IPEndPoint.Address.ToString(); // Initialise the host name to the IP address in case there is no DNS name resolution or in case this fails
            AlpacaDeviceDescription = new AlpacaDeviceDescription();
            ConfiguredDevices = new List<ConfiguredDevice>();
            SupportedInterfaceVersions = new int[] { };
            StatusMessage = statusMessage;
        }

        /// <summary>
        /// Alpaca device IP endpoint
        /// </summary>
        public IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// Alpaca device host name
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Alpaca device description
        /// </summary>
        public AlpacaDeviceDescription AlpacaDeviceDescription { get; set; }

        /// <summary>
        /// List of ASCOM devices available on this Alpaca device
        /// </summary>
        public List<ConfiguredDevice> ConfiguredDevices { get; set; }

        /// <summary>
        /// Array of supported Alpaca interface version numbers
        /// </summary>
        public int[] SupportedInterfaceVersions { get; set; }

        /// <summary>
        /// Alpaca device status message
        /// </summary>
        /// <remarks>This should be an empty string in normal operation when there are no issues but should be changed to an error message when an issue occurs.</remarks>
        public string StatusMessage { get; set; } = "";
    }
}
