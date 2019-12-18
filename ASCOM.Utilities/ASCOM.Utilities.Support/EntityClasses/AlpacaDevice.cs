using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ASCOM.Utilities.Alpaca
{
    /// <summary>
    /// Alpaca device entity class
    /// </summary>
    public class AlpacaDevice
    {
        /// <summary>
        /// Default initialiser
        /// </summary>
        public AlpacaDevice() : this(new IPEndPoint(IPAddress.Any, 0), "", "")
        {
        }

        /// <summary>
        /// Initialise IP end point, ALpaca unique ID and Status message
        /// </summary>
        /// <param name="ipEndPoint">Alpaca device IP endpoint</param>
        /// <param name="alpacaUniqueId">ALpaca device unique ID</param>
        /// <param name="statusMessage">Device status message</param>
        public AlpacaDevice(IPEndPoint ipEndPoint, string alpacaUniqueId, string statusMessage)
        {
            IPEndPoint = ipEndPoint;
            HostName = "";
            AlpacaUniqueId = alpacaUniqueId;
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
        /// Alpaca device unique ID
        /// </summary>
        public string AlpacaUniqueId { get; set; }

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
