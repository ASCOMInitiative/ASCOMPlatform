// (c) 2019 Daniel Van Noord
// This code is licensed under MIT license (see License.txt for details)

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
//using Newtonsoft.Json;

namespace ASCOM.Utilities
{


    // This namespace dual targets NetStandard2.0 and Net35, thus no async await
    internal class Finder : IDisposable
    {

        private TraceLogger TL;
        private readonly Action<IPEndPoint, AlpacaDiscoveryResponse> callbackFunctionDelegate;

        private Dictionary<IPAddress, UdpClient> ipV4DiscoveryClients = new(); // Collection Of IP v4 clients For the various link local And localhost network

        private Dictionary<IPAddress, UdpClient> ipV6Discoveryclients = new(); // Collection Of IP v6 clients For the various link local And localhost network

        /// <summary>
        /// A cache of all endpoints found by the server
        /// </summary>
        public List<IPEndPoint> CachedEndpoints { get; private set; } = new List<IPEndPoint>();

        #region Initialisation and Dispose
        /// <summary>
        /// Creates a Alpaca Finder object that sends out a search request for Alpaca devices
        /// The results will be sent to the callback and stored in the cache
        /// Calling search and concatenating the results reduces the chance that a UDP packet is lost
        /// This may require firewall access
        /// </summary>
        /// <param name="callback">A callback function to receive the endpoint result</param>
        /// <param name="traceLogger">Trace logger for debugging</param>
        internal Finder(Action<IPEndPoint, AlpacaDiscoveryResponse> callback, TraceLogger traceLogger)
        {
            TL = traceLogger; // Save the trace logger object
            LogMessage("Finder", "Starting Initialisation...");
            callbackFunctionDelegate = callback;
            LogMessage("Finder", "Initialised");
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                if (disposing)
                {

                    // Dispose IPv4
                    foreach (KeyValuePair<IPAddress, UdpClient> dev in ipV4DiscoveryClients)
                    {
                        try
                        {
                            dev.Value.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    ipV4DiscoveryClients.Clear();

                    foreach (KeyValuePair<IPAddress, UdpClient> dev in ipV6Discoveryclients)
                    {
                        try
                        {
                            dev.Value.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    ipV6Discoveryclients.Clear();

                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put clean-up code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Resends the search request
        /// </summary>
        public void Search(int discoveryport, bool ipV4Enabled, bool ipV6Enabled)
        {
            if (ipV4Enabled)
                SendDiscoveryMessageIpV4(discoveryport);
            if (ipV6Enabled)
                SendDiscoveryMessageIpV6(discoveryport);
        }

        /// <summary>
        /// Clears the cached IP Endpoints in CachedEndpoints
        /// </summary>
        public void ClearCache()
        {
            CachedEndpoints.Clear();
        }

        #endregion

        #region Private methods
        // Private Sub SendDiscoveryMessageIpV4(ByVal discoveryPort As Integer)
        // Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        // LogMessage("SendDiscoveryMessageIpV4", $"Sending IPv$ discovery broadcasts")

        // For Each adapter As NetworkInterface In adapters
        // 'Do not try and use non-operational adapters
        // If adapter.OperationalStatus <> OperationalStatus.Up Then Continue For

        // If adapter.Supports(NetworkInterfaceComponent.IPv4) Then
        // Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
        // If adapterProperties Is Nothing Then Continue For
        // Dim uniCast As UnicastIPAddressInformationCollection = adapterProperties.UnicastAddresses

        // If uniCast.Count > 0 Then

        // For Each uni As UnicastIPAddressInformation In uniCast
        // If uni.Address.AddressFamily <> AddressFamily.InterNetwork Then Continue For

        // ' Local host addresses (127.*.*.*) may have a null mask in Net Framework. We do want to search these. The correct mask is 255.0.0.0.
        // udpClient.Send(Encoding.ASCII.GetBytes(DISCOVERY_MESSAGE), Encoding.ASCII.GetBytes(DISCOVERY_MESSAGE).Length, New IPEndPoint(GetBroadcastAddress(uni.Address, If(uni.IPv4Mask, IPAddress.Parse("255.0.0.0"))), discoveryPort))
        // LogMessage("SendDiscoveryMessageIpV4", $"Sent broadcast to: {uni.Address}")
        // Next
        // End If
        // End If
        // Next
        // End Sub

        private void SendDiscoveryMessageIpV4(int discoveryPort)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            LogMessage("SearchIPv4", $"Sending IPv4 discovery broadcasts");

            foreach (NetworkInterface adapter in adapters)
            {

                try
                {
                    if (adapter.OperationalStatus != OperationalStatus.Up)
                        continue;

                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        var adapterProperties = adapter.GetIPProperties();
                        if (adapterProperties is null)
                            continue;
                        var uniCast = adapterProperties.UnicastAddresses;

                        if (uniCast.Count > 0)
                        {

                            foreach (UnicastIPAddressInformation uni in uniCast)
                            {

                                try
                                {
                                    if (uni.Address.AddressFamily != AddressFamily.InterNetwork)
                                        continue;

                                    // Protect against IPV4Mak being null, which it can occasionally be
                                    if (uni.IPv4Mask is not null)
                                    {
                                        if (uni.IPv4Mask.Equals(IPAddress.Parse("255.255.255.255")))
                                        {
                                            continue;
                                        }
                                    }

                                    if (!ipV4DiscoveryClients.ContainsKey(uni.Address))
                                    {
                                        ipV4DiscoveryClients.Add(uni.Address, NewIPv4Client(uni.Address));
                                    }

                                    if (!ipV4DiscoveryClients[uni.Address].Client.IsBound)
                                    {
                                        ipV4DiscoveryClients.Remove(uni.Address);
                                        continue;
                                    }

                                    ipV4DiscoveryClients[uni.Address].Send(Encoding.ASCII.GetBytes(Constants.DISCOVERY_MESSAGE), Encoding.ASCII.GetBytes(Constants.DISCOVERY_MESSAGE).Length, new IPEndPoint(GetBroadcastAddress(uni.Address, uni.IPv4Mask ?? IPAddress.Parse("255.0.0.0")), discoveryPort));
                                    LogMessage("SearchIPv4", $"Sent broadcast to: {uni.Address}");
                                }
                                catch (Exception ex)
                                {
                                    LogMessage("SearchIPv4", ex.ToString());
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogMessage("SearchIPv4", ex.ToString());
                }
            }
        }

        private UdpClient NewIPv4Client(IPAddress host)
        {
            var client = new UdpClient()
            {
                EnableBroadcast = true,
                MulticastLoopback = false
            };

            int SIO_UDP_CONNRESET = -1744830452;
            client.Client.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);

            client.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
            client.BeginReceive(new AsyncCallback(FinderDiscoveryCallback), client);
            return client;
        }

        /// <summary>
        /// Send out discovery message on the IPv6 multicast group
        /// This dual targets NetStandard 2.0 and NetFX 3.5 so no Async Await
        /// </summary>
        private void SendDiscoveryMessageIpV6(int discoveryPort)
        {
            LogMessage("SearchIPv6", $"Sending IPv6 discovery broadcasts");

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {

                try
                {
                    LogMessage("SearchIPv6", $"Found network interface {adapter.Description}, Interface type: {adapter.NetworkInterfaceType} - supports multicast: {adapter.SupportsMulticast}");
                    if (adapter.OperationalStatus != OperationalStatus.Up)
                        continue;
                    LogMessage("SearchIPv6", $"Interface is up");

                    if (adapter.Supports(NetworkInterfaceComponent.IPv6) && adapter.SupportsMulticast)
                    {
                        LogMessage("SearchIPv6", $"Interface supports IPv6");
                        var adapterProperties = adapter.GetIPProperties();
                        if (adapterProperties is null)
                            continue;
                        var uniCast = adapterProperties.UnicastAddresses;
                        LogMessage("SearchIPv6", $"Adapter does have properties. Number of unicast addresses: {uniCast.Count}");

                        if (uniCast.Count > 0)
                        {

                            foreach (UnicastIPAddressInformation uni in uniCast)
                            {

                                try
                                {
                                    if (uni.Address.AddressFamily != AddressFamily.InterNetworkV6)
                                        continue;
                                    LogMessage("SearchIPv6", $"Interface {uni.Address} supports IPv6 - IsLinkLocal: {uni.Address.IsIPv6LinkLocal}, LocalHost: {uni.Address.Equals(IPAddress.Parse("::1"))}");
                                    if (!uni.Address.IsIPv6LinkLocal && !IPAddress.IsLoopback(uni.Address))
                                        continue;

                                    try
                                    {

                                        if (!ipV6Discoveryclients.ContainsKey(uni.Address))
                                        {
                                            ipV6Discoveryclients.Add(uni.Address, NewIPv6Client(uni.Address, 0));
                                        }

                                        ipV6Discoveryclients[uni.Address].Send(Encoding.ASCII.GetBytes(Constants.DISCOVERY_MESSAGE), Encoding.ASCII.GetBytes(Constants.DISCOVERY_MESSAGE).Length, new IPEndPoint(IPAddress.Parse(Constants.ALPACA_DISCOVERY_IPV6_MULTICAST_ADDRESS), discoveryPort));
                                        LogMessage("SearchIPv6", $"Sent multicast IPv6 discovery packet");
                                    }
                                    catch (SocketException)
                                    {
                                    }
                                }

                                catch (Exception ex)
                                {
                                    LogMessage("SearchIPv6", $"Exception sending IPv6 discovery packet: {ex}");
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogMessage("SearchIPv6", $"Exception: {ex}");
                }
            }
        }

        private UdpClient NewIPv6Client(IPAddress host, int port)
        {
            var client = new UdpClient(AddressFamily.InterNetworkV6);
            client.Client.Bind(new IPEndPoint(host, port));
            client.BeginReceive(new AsyncCallback(FinderDiscoveryCallback), client);
            return client;
        }















        /// <summary>
        /// This callback is shared between IPv4 and IPv6
        /// </summary>
        /// <param name="ar"></param>
        private void FinderDiscoveryCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient udpClient = (UdpClient)ar.AsyncState;
                var alpacaBroadcastResponseEndPoint = new IPEndPoint(IPAddress.Any, Constants.DEFAULT_DISCOVERY_PORT);

                // Obtain the UDP message body and convert it to a string, with remote IP address attached as well
                string ReceiveString = Encoding.ASCII.GetString(udpClient.EndReceive(ar, ref alpacaBroadcastResponseEndPoint));
                LogMessage($"FinderDiscoveryCallback", $"Received {ReceiveString} from Alpaca device at {alpacaBroadcastResponseEndPoint.Address}");

                // Configure the UdpClient class to accept more messages, if they arrive
                udpClient.BeginReceive(new AsyncCallback(FinderDiscoveryCallback), udpClient);

                // Only process Alpaca device responses
                if (ReceiveString.ToLowerInvariant().Contains(Constants.DISCOVERY_RESPONSE_STRING))
                {
                    // Extract the discovery response parameters from the device's JSON response
                    var serializer = new JavaScriptSerializer();
                    var discoveryResponse = serializer.Deserialize<AlpacaDiscoveryResponse>(ReceiveString);

                    // Dim discoveryResponse As AlpacaDiscoveryResponse = JsonConvert.DeserializeObject(Of AlpacaDiscoveryResponse)(ReceiveString)
                    var alpacaApiEndpoint = new IPEndPoint(alpacaBroadcastResponseEndPoint.Address, discoveryResponse.AlpacaPort); // Create 
                    if (!CachedEndpoints.Contains(alpacaApiEndpoint))
                    {
                        CachedEndpoints.Add(alpacaApiEndpoint);
                        LogMessage("FinderDiscoveryCallback", $"Received new Alpaca API endpoint: {alpacaApiEndpoint} from broadcast endpoint: {alpacaBroadcastResponseEndPoint}");
                        callbackFunctionDelegate?.Invoke(alpacaApiEndpoint, discoveryResponse); // Moved inside the loop so that the callback is only called once per IP address
                    }
                    else
                    {
                        LogMessage("FinderDiscoveryCallback", $"Ignoring duplicate Alpaca API endpoint: {alpacaApiEndpoint} from broadcast endpoint: {alpacaBroadcastResponseEndPoint}");
                    }
                }
            }

            // Ignore these, they can occur after the Finder is disposed
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                LogMessage("FinderDiscoveryCallback", $"Exception: " + ex.ToString());
            }
        }


        // This turns the unicast address and the subnet into the broadcast address for that range
        // http://blogs.msdn.com/b/knom/archive/2008/12/31/ip-address-calculations-with-c-subnetmasks-networks.aspx
        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            byte[] broadcastAddress = new byte[ipAdressBytes.Length];

            for (int i = 0, loopTo = broadcastAddress.Length - 1; i <= loopTo; i++)
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | subnetMaskBytes[i] ^ 255);

            return new IPAddress(broadcastAddress);
        }

        private void LogMessage(string method, string message)
        {
            if (TL is not null)
            {
                string indentSpaces = new(' ', Thread.CurrentThread.ManagedThreadId * Constants.NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES);
                TL.LogMessage($"Finder - {method}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} - {message}");
            }
        }

        #endregion
    }
}