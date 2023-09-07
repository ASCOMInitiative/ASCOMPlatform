using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Utilities
{

    /// <summary>
/// Data object describing an Alpaca device that supports discovery as returned by the <see cref="AlpacaDiscovery"/> component.
/// </summary>
    [Guid("D572145F-E4CF-4A9E-B2AE-A0D32604E20C")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AlpacaDevice : IAlpacaDevice, IAlpacaDeviceExtra
    {

        private List<ConfiguredDevice> configuredDevicesValue;
        private ArrayList configuredDevicesAsArrayListValue;

        private string hostNameValue, ipAddressValue, serverNameValue, manufacturerValue, manufacturerVersionValue, locationValue;

        /// <summary>
    /// Initialises the class with default values
    /// </summary>
        public AlpacaDevice() : this(new IPEndPoint(0L, 0), "")
        {
        }

        /// <summary>
    /// Initialise IP end point, Alpaca unique ID and Status message - Can only be used from .NET clients
    /// </summary>
    /// <param name="ipEndPoint">Alpaca device IP endpoint</param>
    /// <param name="statusMessage">Device status message</param>
        internal AlpacaDevice(IPEndPoint ipEndPoint, string statusMessage)
        {
            // Initialise internal storage variables
            configuredDevicesValue = new List<ConfiguredDevice>();
            configuredDevicesAsArrayListValue = new ArrayList();
            SupportedInterfaceVersions = new int[] { };

            // Initialise string variables to ensure that no null values are present
            hostNameValue = "";
            ipAddressValue = "";
            serverNameValue = "";
            manufacturerValue = "";
            manufacturerVersionValue = "";
            locationValue = "";

            IPEndPoint = ipEndPoint; // Set the IPEndpoint to the supplied value

            // Initialise the Host name to the IP address using the normal ToString version if IPv4 or the canonical form if IPv6
            if (ipEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
            {
                IpAddress = $"[{ipEndPoint.Address}]";  // Set the IPv6 canonical IP host address
                HostName = IpAddress; // Initialise the host name to the IPv6 address in case there is no DNS name resolution or in case this fails
            }
            else
            {
                IpAddress = ipEndPoint.Address.ToString();  // Set the IPv4 IP host address
                HostName = IpAddress;
            } // Initialise the host name to the IPv4 address in case there is no DNS name resolution or in case this fails

            Port = ipEndPoint.Port; // Set the port number to the port set in the IPEndPoint

            StatusMessage = statusMessage; // Set the status message to the supplied value

        }

        #region Public Properties

        /// <summary>
    /// The Alpaca device's DNS host name, if available, otherwise its IP address. IPv6 addresses will be in canonical form.
    /// </summary>
        public string HostName
        {
            get
            {
                return hostNameValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    hostNameValue = "";
                }
                else
                {
                    hostNameValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// The Alpaca device's IP address. IPv6 addresses will be in canonical form.
    /// </summary>
        public string IpAddress
        {
            get
            {
                return ipAddressValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    ipAddressValue = "";
                }
                else
                {
                    ipAddressValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// Alpaca device's IP port number
    /// </summary>
        public int Port { get; set; }

        /// <summary>
    /// List of ASCOM devices available on this Alpaca device as an ArrayList for COM clients
    /// </summary>
    /// <remarks>
    /// This method is primarily to support COM clients because COM does not support generic lists. .NET clients should use the <see cref="ConfiguredDevices"/> property instead.
    /// </remarks>
        public ArrayList ConfiguredDevicesAsArrayList
        {
            get
            {
                return configuredDevicesAsArrayListValue; // Return the array-list of devices that was populated by the set ConfiguredDevices method
            }
        }

        /// <summary>
    /// Array of supported Alpaca interface version numbers
    /// </summary>
        public int[] SupportedInterfaceVersions { get; set; }

        /// <summary>
    /// The Alpaca device's configured name
    /// </summary>
    /// <returns></returns>
        public string ServerName
        {
            get
            {
                return serverNameValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    serverNameValue = "";
                }
                else
                {
                    serverNameValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// The device manufacturer's name
    /// </summary>
    /// <returns></returns>
        public string Manufacturer
        {
            get
            {
                return manufacturerValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    manufacturerValue = "";
                }
                else
                {
                    manufacturerValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// The device's version as set by the manufacturer
    /// </summary>
    /// <returns></returns>
        public string ManufacturerVersion
        {
            get
            {
                return manufacturerVersionValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    manufacturerVersionValue = "";
                }
                else
                {
                    manufacturerVersionValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// The Alpaca device's configured location
    /// </summary>
    /// <returns></returns>
        public string Location
        {
            get
            {
                return locationValue;

            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Make sure a null value is converted to an empty string
                {
                    locationValue = "";
                }
                else
                {
                    locationValue = value;
                } // Save the supplied value
            }
        }

        /// <summary>
    /// List of ASCOM devices available on this Alpaca device
    /// </summary>
    /// <remarks>
    /// This method can only be used by .NET clients. COM clients should use the <see cref="ConfiguredDevicesAsArrayList"/> property.
    /// </remarks>
        public List<ConfiguredDevice> ConfiguredDevices
        {
            get
            {
                return configuredDevicesValue; // Return the list of configured devices
            }

            set
            {
                // Save the supplied list of configured devices 
                configuredDevicesValue = value;

                // Populate the read-only arraylist with the same data
                configuredDevicesAsArrayListValue.Clear();
                foreach (ConfiguredDevice configuredDevice in configuredDevicesValue)
                    configuredDevicesAsArrayListValue.Add(configuredDevice);
            }
        }
        #endregion

        #region Internal Properties

        /// <summary>
    /// Alpaca device's status message
    /// </summary>
    /// <remarks>This should be an empty string in normal operation when there are no issues but should be changed to an error message when an issue occurs.</remarks>
        internal string StatusMessage { get; set; } = "";

        /// <summary>
    /// Alpaca device's IP endpoint
    /// </summary>
        internal IPEndPoint IPEndPoint { get; set; }

        #endregion

    }
}