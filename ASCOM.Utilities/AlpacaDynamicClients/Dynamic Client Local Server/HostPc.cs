using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ASCOM.DynamicClients
{
    /// <summary>
    /// Class that presents all usable IPv4 and IPv6 addresses on the host
    /// </summary>
    internal class HostPc
    {

        /// <summary>
        /// Class initialiser
        /// </summary>
        public HostPc()
        {
        }

        /// <summary>
        /// Returns the hosts IPv4 addresses
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> IpV4Addresses
        {
            get
            {
                return GetIpAddresses(AddressFamily.InterNetwork);
            }
        }
        /// <summary>
        /// Returns the hosts IPv6 addresses
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> IpV6Addresses
        {
            get
            {
                return GetIpAddresses(AddressFamily.InterNetworkV6);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        private static List<IPAddress> GetIpAddresses(AddressFamily addressFamily)
        {
            // Initialise the IPv4 and IPv6 address lists
            List<IPAddress> ipAddresses = new List<IPAddress>();

            // Get an array of all network interfaces on this host
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            // Iterate over each network adapter looking for usable IP addresses
            foreach (NetworkInterface adapter in adapters)
            {
                // Only test operational adapters
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    // Get the adapter's properties
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                    // If the adapter has properties get the collection of unicast addresses
                    if (adapterProperties != null)
                    {
                        // Get the collection of unicast addresses
                        UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;

                        // If there are some unicast IP addresses get these
                        if (uniCast.Count > 0)
                        {
                            // Iterate over the unicast addresses 
                            foreach (UnicastIPAddressInformation uni in uniCast)
                            {
                                // Save IPv4 addresses to the IPv4 list
                                if (uni.Address.AddressFamily == addressFamily)
                                {
                                    ipAddresses.Add(uni.Address);
                                }
                            }
                        }
                    }
                }
            }

            // Sort the addresses into ascending text order
            ipAddresses.Sort(CompareIPaddresses);

            return ipAddresses;
        }

        /// <summary>
        /// COmpare two IPAdress values based on their text representations
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int CompareIPaddresses(IPAddress x, IPAddress y)
        {
            if (x == null)
            {
                if (y == null) // If x is null and y is null, they're equal. 
                {
                    return 0;
                }
                else // If x is null and y is not null, y is greater. 
                {
                    return -1;
                }
            }
            else // If x is not null...
            {
                if (y == null) // ...and y is null, x is greater.
                {
                    return 1;
                }
                else // ...and y is not null, compare the lengths of the two strings.
                {
                    return x.ToString().CompareTo(y.ToString());
                }
            }
        }
    }
}
