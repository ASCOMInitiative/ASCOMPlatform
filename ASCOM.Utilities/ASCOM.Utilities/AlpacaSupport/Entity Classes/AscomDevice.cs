using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Utilities
{

    /// <summary>
/// Data object describing an ASCOM device that is served by an Alpaca device as returned by the <see cref="AlpacaDiscovery"/> component.
/// </summary>
    [Guid("E768E0BB-D795-4CAE-95D0-9D0173BF57BC")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AscomDevice : IAscomDevice, IAscomDeviceExtra
    {

        /// <summary>
    /// Initialises the class with default values
    /// </summary>
    /// <remarks>COM clients should use this initialiser and set the properties individually because COM only supports parameterless initialisers.</remarks>
        public AscomDevice()
        {
        }

        /// <summary>
    /// Initialise the ASCOM device name, ASCOM device type and ASCOM device unique ID, plus
    /// the Alpaca API device number, unique ID, device IP endpoint, Alpaca unique ID, interface version and status message
    /// </summary>
    /// <param name="ascomDdeviceName">ASCOM device name</param>
    /// <param name="ascomDeviceType">ASCOM device type</param>
    /// <param name="alpacaDeviceNumber">Alpaca API device number</param>
    /// <param name="uniqueId">ASCOM device unique ID</param>
    /// <param name="ipEndPoint">Alpaca device IP endpoint</param>
    /// <param name="hostName">ALapca device host name</param>
    /// <param name="interfaceVersion">Supported Alpaca interface version</param>
    /// <param name="statusMessage">Alpaca device status message</param>
    /// <remarks>This can only be used by .NET clients because COM only supports parameterless initialisers.</remarks>
        internal AscomDevice(string ascomDdeviceName, string ascomDeviceType, int alpacaDeviceNumber, string uniqueId, IPEndPoint ipEndPoint, string hostName, int interfaceVersion, string statusMessage)
        {
            AscomDeviceName = ascomDdeviceName;
            AscomDeviceType = ascomDeviceType;
            AlpacaDeviceNumber = alpacaDeviceNumber;
            UniqueId = uniqueId;
            IPEndPoint = ipEndPoint;
            HostName = hostName;
            InterfaceVersion = interfaceVersion;
            StatusMessage = statusMessage;

            // Populate the IP address based on the supplied IPEndPoint value and address type
            if (ipEndPoint.AddressFamily == AddressFamily.InterNetwork) // IPv4 address
            {
                IpAddress = ipEndPoint.Address.ToString();
            }
            else if (ipEndPoint.AddressFamily == AddressFamily.InterNetworkV6) // IPv6 address so save it in canonical form
            {
                IpAddress = $"[{ipEndPoint.Address}]";
            }
            else
            {
                throw new InvalidValueException($"Unsupported network type {ipEndPoint.AddressFamily} when creating a new ASCOMDevice");
            }

        }

        /// <summary>
    /// ASCOM device name
    /// </summary>
        public string AscomDeviceName { get; set; }

        /// <summary>
    /// ASCOM device type
    /// </summary>
        public string AscomDeviceType { get; set; }

        /// <summary>
    /// Alpaca API device number
    /// </summary>
        public int AlpacaDeviceNumber { get; set; }

        /// <summary>
    /// ASCOM device unique ID
    /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
    /// The ASCOM device's DNS host name, if available, otherwise its IP address. IPv6 addresses will be in canonical form.
    /// </summary>
        public string HostName { get; set; }

        /// <summary>
    /// The ASCOM device's IP address. IPv6 addresses will be in canonical form.
    /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
    /// Supported Alpaca interface version
    /// </summary>
        public int InterfaceVersion { get; set; }

        /// <summary>
    /// Alpaca device status message
    /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
    /// Alpaca device IP endpoint
    /// </summary>
        internal IPEndPoint IPEndPoint { get; set; }
        IPEndPoint IAscomDeviceExtra.IPEndPoint { get => IPEndPoint; set => IPEndPoint = value; }

    }
}