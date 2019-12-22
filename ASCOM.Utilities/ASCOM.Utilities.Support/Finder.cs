// (c) 2019 Daniel Van Noord
// This code is licensed under MIT license (see License.txt for details)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ASCOM.Utilities.Support;
using Newtonsoft.Json;

//This namespace dual targets NetStandard2.0 and Net35, thus no async await
namespace ASCOM.Utilities.Support
{
    public class Finder : IDisposable
    {
        private ITraceLoggerUtility TL;

        private readonly Action<IPEndPoint, AlpacaDiscoveryResponse> callbackFunctionDelegate;
        private readonly UdpClient udpClient;

        /// <summary>
        /// A cache of all endpoints found by the server
        /// </summary>
        public List<IPEndPoint> CachedEndpoints
        {
            get;
        } = new List<IPEndPoint>();

        #region Initialisation and Dispose
        /// <summary>
        /// Creates a Alpaca Finder object that sends out a search request for Alpaca devices
        /// The results will be sent to the callback and stored in the cache
        /// Calling search and concatenating the results reduces the chance that a UDP packet is lost
        /// This may require firewall access
        /// </summary>
        /// <param name="callback">A callback function to receive the endpoint result</param>
        internal Finder(Action<IPEndPoint, AlpacaDiscoveryResponse> callback, ITraceLoggerUtility traceLogger)
        {
            TL = traceLogger; // Save the trace logger object
            LogMessage("Finder", "Starting Initialisation...");
            callbackFunctionDelegate = callback;

            udpClient = new UdpClient();

            udpClient.EnableBroadcast = true;
            udpClient.MulticastLoopback = false;

            //0 tells OS to give us a free ethereal port
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));

            udpClient.BeginReceive(FinderDiscoveryCallback, udpClient);

            LogMessage("Finder", "Initialised");
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (udpClient != null) try { udpClient.Close(); } catch { }
                    //try { udpClient.Dispose(); } catch { }
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
        public void Search(int discoveryport)
        {
            SendDiscoveryMessage(Encoding.ASCII.GetBytes(Constants.DISCOVERY_MESSAGE), discoveryport);
        }

        /// <summary>
        /// Clears the cached IP Endpoints in CachedEndpoints
        /// </summary>
        public void ClearCache()
        {
            CachedEndpoints.Clear();
        }

        #endregion

        /*
         * On my test systems I discovered that some computer network adapters / networking gear will not forward 255.255.255.255 broadcasts. 
         * This binds to each network adapter on the computer, determines if it will work and then
         * Sends an IP and Subnet correct broadcast for each address combination on that adapter
         * This may result in some addresses being duplicated. For example if there are multiple addresses assigned to the same
         * Server this will find them all.
         * Also NCAP style loop backs may duplicate IP address running on local host
         */
        private void SendDiscoveryMessage(byte[] message, int discoveryPort)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                //Do not try and use non-operational adapters
                if (adapter.OperationalStatus != OperationalStatus.Up)
                    continue;
                // Currently this only works for IPv4, skip any adapters that do not support it.
                if (!adapter.Supports(NetworkInterfaceComponent.IPv4))
                    continue;
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                if (adapterProperties == null)
                    continue;
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                if (uniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        // Currently this only works for IPv4.
                        if (uni.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;

                        // Local host addresses (127.*.*.*) may have a null mask in Net Framework. We do want to search these. The correct mask is 255.0.0.0.
                        udpClient.Send(message,
                                       message.Length,
                                       new IPEndPoint(GetBroadcastAddress(uni.Address, uni.IPv4Mask ?? IPAddress.Parse("255.0.0.0")), discoveryPort)
                                       );
                        LogMessage("SendDiscoveryMessage", $"Sent broadcast to: {uni.Address.ToString()}");
                    }
                }
            }
        }

        private void FinderDiscoveryCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient udpClient = (UdpClient)ar.AsyncState;

                IPEndPoint alpacaBroadcastResponseEndPoint = new IPEndPoint(IPAddress.Any, Constants.DEFAULT_DISCOVERY_PORT);

                // Obtain the UDP message body and convert it to a string, with remote IP address attached as well
                string ReceiveString = Encoding.ASCII.GetString(udpClient.EndReceive(ar, ref alpacaBroadcastResponseEndPoint));
                LogMessage($"FinderDiscoveryCallback", $"Received {ReceiveString} from Alpaca device at {alpacaBroadcastResponseEndPoint.Address.ToString()}");

                // Configure the UdpClient class to accept more messages, if they arrive
                udpClient.BeginReceive(FinderDiscoveryCallback, udpClient);

                //Only process Alpaca device responses
                if (ReceiveString.ToLowerInvariant().Contains(Constants.DISCOVERY_RESPONSE_STRING))
                {
                    // Extract the discovery response parameters from the device's JSON response
                    AlpacaDiscoveryResponse discoveryResponse = JsonConvert.DeserializeObject<AlpacaDiscoveryResponse>(ReceiveString);

                    IPEndPoint alpacaApiEndpoint = new IPEndPoint(alpacaBroadcastResponseEndPoint.Address, discoveryResponse.AlpacaPort); // Create 

                    if (!CachedEndpoints.Contains(alpacaApiEndpoint))
                    {
                        CachedEndpoints.Add(alpacaApiEndpoint);
                        LogMessage("FinderDiscoveryCallback", $"Received new Alpaca API endpoint: {alpacaApiEndpoint.ToString()} from broadcast endpoint: {alpacaBroadcastResponseEndPoint.ToString()}");
                        callbackFunctionDelegate?.Invoke(alpacaApiEndpoint, discoveryResponse); // Moved inside the loop so that the callback is only called once per IP address
                    }
                    else
                    {
                        LogMessage("FinderDiscoveryCallback", $"Ignoring duplicate Alpaca API endpoint: {alpacaApiEndpoint.ToString()} from broadcast endpoint: {alpacaBroadcastResponseEndPoint.ToString()}");
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                LogMessage("FinderDiscoveryCallback", $"ObjectDisposedException: " + ex.ToString());
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
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        private void LogMessage(string method, string message)
        {
            if (!(TL == null))
            {
                string indentSpaces = new String(' ', Thread.CurrentThread.ManagedThreadId * Constants.NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES);
                TL.LogMessage($"Finder - {method}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} - {message}");
            }
        }
    }
}