using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

using ASCOM.Utilities;
using ASCOM.DeviceInterface;

using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Numerics;
using ASCOM.Common.Alpaca;
using ASCOM.Alpaca.Clients;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Policy;
using System.Web;
using ASCOM.Common.Com;

namespace ASCOM.DynamicRemoteClients
{
    public static class DynamicClientDriver
    {
        #region Private variables and constants

        // Private constants
        private const int DYNAMIC_DRIVER_ERROR_NUMBER = 4095; // Alpaca error number that will be returned when a required JSON "Value" element is either absent from the response or is set to "null"

        //Private variables
        private static TraceLoggerPlus TLLocalServer;

        private static uint uniqueTransactionNumber = 0; // Unique number that increments on each call to TransactionNumber

        // Lock objects
        private readonly static object connectLockObject = new object();
        private readonly static object transactionCountlockObject = new object();

        private static ConcurrentDictionary<long, bool> connectStates;

        private struct AvailableInterface
        {
            public string HostName;
            public int Port;
            public BigInteger IpAddress;
            public BigInteger AddressDistance;
        }

        #endregion

        #region Initialiser

        /// <summary>
        /// Static initialiser to set up the objects we need at run time
        /// </summary>
        static DynamicClientDriver()
        {
            try
            {
                TLLocalServer = new TraceLoggerPlus("", "DynamicClientLocalServer")
                {
                    Enabled = false
                }; // Trace state is set in ReadProfile, immediately after being read from the Profile
                TLLocalServer.LogMessage("DynamicClientDriver", $"Initialising - Version: { Assembly.GetEntryAssembly().GetName().Version}");

                connectStates = new ConcurrentDictionary<long, bool>();

                TLLocalServer.LogMessage("DynamicClientDriver", "Initialisation complete.");
            }
            catch (Exception ex)
            {
                TLLocalServer.LogMessageCrLf("DynamicClientDriver", ex.ToString());
                MessageBox.Show(ex.ToString(), "Error initialising the DynamicClientDriver base class", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Utility code

        /// <summary>
        /// Returns a unique client number to the calling instance in the range 1::65536
        /// </summary>
        public static uint GetUniqueClientNumber()
        {
            uint randomvalue;

            using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
            {
                byte[] rno = new byte[5]; // Create a four byte array
                rg.GetBytes(rno); // Fill the array with four random bytes
                rno[2] = 0; // Zero the higher two bytes to limit the resulting integer to the range 0::65535
                rno[3] = 0;
                rno[4] = 0;
                randomvalue = BitConverter.ToUInt32(rno, 0) + 1; // Convert the bytes to an integer in the range 0::65535 and add 1 to get an integer in the range 1::65536
            }

            return randomvalue;
        }

        /// <summary>
        /// Returns a unique client number to the calling instance
        /// </summary>
        public static uint TransactionNumber()
        {
            lock (transactionCountlockObject)
            {
                uniqueTransactionNumber += 1;
            }
            return uniqueTransactionNumber;
        }

        /// <summary>
        /// Tests whether the hub is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the client making the call</param>
        /// <returns>Boolean true if the hub is already connected</returns>
        public static bool IsHardwareConnected(TraceLoggerPlus TL)
        {
            if (TL.DebugTraceState) TL.LogMessage("IsHardwareConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
            return connectStates.Count > 0;
        }

        /// <summary>
        /// Test whether a particular client is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the calling client</param>
        /// <returns></returns>
        public static bool IsClientConnected(uint clientNumber, TraceLoggerPlus TL)
        {
            foreach (KeyValuePair<long, bool> kvp in connectStates)
            {
                TL.LogMessage(clientNumber, "IsClientConnected", string.Format("This device ClientID is in the ConnectedStates list: {0}, Value: {1}", kvp.Key, kvp.Value));
            }

            TL.LogMessage(clientNumber, "IsClientConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + connectStates.ContainsKey(clientNumber).ToString());

            return connectStates.ContainsKey(clientNumber);
        }

        /// <summary>
        /// Returns the number of connected clients
        /// </summary>
        public static uint ConnectionCount(TraceLoggerPlus TL)
        {
            TL.LogMessage("ConnectionCount", connectStates.Count.ToString());
            return (uint)connectStates.Count;
        }

        /// <summary>
        /// Return name of current method
        /// </summary>
        /// <returns>Name of current method</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Set the REST client communications timeout for the next transaction
        /// </summary>
        /// <param name="client">REST client to use</param>
        /// <param name="deviceResponseTimeout">Timeout to be set</param>
        public static void SetClientTimeout(RestClient client, int deviceResponseTimeout)
        {
            client.Timeout = deviceResponseTimeout * 1000;
            client.ReadWriteTimeout = deviceResponseTimeout * 1000;
        }

        /// <summary>
        /// Create and configure a REST client to communicate with the Alpaca device
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ipAddressString"></param>
        /// <param name="portNumber"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="serviceType"></param>
        /// <param name="TL"></param>
        /// <param name="clientNumber"></param>
        /// <param name="deviceType"></param>
        /// <param name="deviceResponseTimeout"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="uniqueId"></param>
        /// <remarks>This method will attempt to re-discover the Alpaca device if it is not possible to establish a TCP connection with the device at the specified address and port.</remarks>
        public static void ConnectToRemoteDevice(ref RestClient client, string ipAddressString, decimal portNumber, int connectionTimeout, string serviceType, TraceLoggerPlus TL,
                                                 uint clientNumber, string driverProgId, string deviceType, int deviceResponseTimeout, string userName, string password, string uniqueId, bool enableRediscovery, bool ipV4Enabled, bool ipV6Enabled, int discoveryPort)
        {
            List<AvailableInterface> availableInterfaces = new List<AvailableInterface>();

            string clientHostAddress = $"{serviceType}://{ipAddressString}:{portNumber}";
            TL.LogMessage(clientNumber, deviceType, $"Connecting to device: {ipAddressString}:{portNumber}, Unique ID: {uniqueId} through URL: {clientHostAddress}");

            // Test whether automatic Alpaca device rediscovery is enabled for this device
            if (enableRediscovery) // Automatic rediscovery is enabled
            {
                TL.LogMessage(clientNumber, deviceType, $"Testing whether client at address {clientHostAddress} can be contacted.");

                // Test whether there is a device at the configured IP address and port by trying to open a TCP connection to it
                if (!ClientIsUp(ipAddressString, portNumber, connectionTimeout, TL, clientNumber)) // It was not possible to establish TCP communication with a device at the IP address provided
                {
                    // Attempt to "re-discover" the device and use it's new address and / or port
                    TL.LogMessage(clientNumber, deviceType, $"The device at the configured IP address and port {ipAddressString} cannot be contacted, attempting to re-discover it");

                    // Create an AlapcaDiscovery component to conduct the search
                    using (AlpacaDiscovery alpacaDiscovery = new AlpacaDiscovery())
                    {
                        // Start a discovery using two polls, 100ms apart, timing out after 2 seconds, don't attempt to resolve the IP address to a DNS name use the discovery port and IP settings of this device
                        alpacaDiscovery.StartDiscovery(2, 100, discoveryPort, 2.0, false, ipV4Enabled, ipV6Enabled);

                        // Wait for the discovery cycle to complete, making sure that the UI remains responsive
                        do
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                        } while (!alpacaDiscovery.DiscoveryComplete);

                        // Get a list of the discovered Alpaca devices
                        List<AlpacaDevice> discoveredDevices = alpacaDiscovery.GetAlpacaDevices();

                        // Iterate over these to find which ASCOM devices are served by them
                        foreach (AlpacaDevice alpacaDevice in discoveredDevices)
                        {
                            TL.LogMessage(clientNumber, deviceType, $"Found Alpaca device {alpacaDevice.HostName}:{alpacaDevice.Port} - {alpacaDevice.ServerName}");

                            // Iterate over the devices served by the Alpaca device
                            foreach (ConfiguredDevice ascomDevice in alpacaDevice.ConfiguredDevices)
                            {
                                TL.LogMessage(clientNumber, deviceType, $"Found ASCOM device {ascomDevice.DeviceName}:{ascomDevice.DeviceType} - {ascomDevice.UniqueID} at {alpacaDevice.HostName}:{alpacaDevice.Port}");

                                // Test whether the found ASCOM device has the same unique ID as the device for which we are looking
                                if (ascomDevice.UniqueID.ToLowerInvariant() == uniqueId.ToLowerInvariant()) // We have a match so we can use this address and port instead of the configured values that no longer work
                                {
                                    TL.LogMessage(clientNumber, deviceType, $"  *** Found REQUIRED ASCOM device ***");

                                    // Get the IP address as a big endian byte array
                                    byte[] addressBytes = IPAddress.Parse(alpacaDevice.HostName).GetAddressBytes();

                                    // Create an array large enough to hold an IPv6 address (16 bytes) plus one extra byte at the high end that will always be 0.
                                    // This ensures that the IPv6 address will not be interpreted as a negative number if its top bit is set
                                    byte[] hostBytes = new byte[17];

                                    // Re-order the network address byte array to little endian as used in Windows
                                    Array.Copy(addressBytes.Reverse().ToArray<byte>(), hostBytes, addressBytes.Length);

                                    // Create a big integer from the little endian byte array
                                    BigInteger bigIntegerAddress = new BigInteger(hostBytes);

                                    // Create a new structure to hold the interface information and add it to the list of interfaces
                                    AvailableInterface availableInterface = new AvailableInterface();
                                    availableInterface.HostName = alpacaDevice.HostName;
                                    availableInterface.Port = alpacaDevice.Port;
                                    availableInterface.IpAddress = bigIntegerAddress;
                                    availableInterfaces.Add(availableInterface);

                                }
                            }
                            TL.BlankLine();
                        }

                    }

                    // Search the discovered interfaces for the one whose network address is closest to the original address
                    // This will ensure that we pick an address on the original subnet if this is available.
                    switch (availableInterfaces.Count)
                    {
                        case 0:
                            TL.LogMessage(clientNumber, deviceType, $"No ASCOM device was discovered that had a UniqueD of {uniqueId}");
                            TL.BlankLine();
                            break;

                        case 1:
                            // Update the client host address with the newly discovered address and port
                            clientHostAddress = $"{serviceType}://{availableInterfaces[0].HostName}:{availableInterfaces[0].Port}";
                            TL.LogMessage(clientNumber, deviceType, $"One ASCOM device was discovered that had a UniqueD of {uniqueId}. Now using URL: {clientHostAddress}");

                            // Write the new value to the driver's Profile so it is found immediately in future
                            using (Profile profile = new Profile())
                            {
                                profile.DeviceType = deviceType;
                                profile.WriteValue(driverProgId, SharedConstants.IPADDRESS_PROFILENAME, availableInterfaces[0].HostName);
                                profile.WriteValue(driverProgId, SharedConstants.PORTNUMBER_PROFILENAME, availableInterfaces[0].Port.ToString());
                                TL.LogMessage(clientNumber, deviceType, $"Written new values {availableInterfaces[0].HostName} and {availableInterfaces[0].Port} to profile {driverProgId}");
                            }

                            TL.BlankLine();
                            break;

                        default:
                            TL.LogMessage(clientNumber, deviceType, $"{availableInterfaces.Count} ASCOM devices were discovered that had a UniqueD of {uniqueId}.");

                            // Get the original IP address as a big endian byte array
                            byte[] addressBytes = new byte[0]; // Create a zero length array in case its not possible to parse the IP address string (it may be a host name or may just be corrupted)

                            try
                            {
                                addressBytes = IPAddress.Parse(ipAddressString).GetAddressBytes();
                            }
                            catch { }

                            // Create an array large enough to hold an IPv6 address (16 bytes) plus one extra byte at the high end that will always be 0.
                            // This ensures that the IPv6 address will not be interpreted as a negative number if its top bit is set
                            byte[] hostBytes = new byte[17];

                            // Re-order the network address byte array to little endian as used in Windows
                            Array.Copy(addressBytes.Reverse().ToArray<byte>(), hostBytes, addressBytes.Length);

                            // Create a big integer from the little endian byte array
                            BigInteger currentIpAddress = new BigInteger(hostBytes);

                            // Iterate over the discovered interfaces to find the one that is closest to the original IP address
                            for (int i = 0; i < availableInterfaces.Count; i++)
                            {
                                AvailableInterface ai = new AvailableInterface();
                                ai.IpAddress = availableInterfaces[i].IpAddress;
                                ai.Port = availableInterfaces[i].Port;
                                ai.HostName = availableInterfaces[i].HostName;
                                ai.AddressDistance = BigInteger.Abs(BigInteger.Subtract(currentIpAddress, ai.IpAddress));
                                availableInterfaces[i] = ai;
                            }

                            // Initialise a big integer variable with an impossibly large address to ensure that the first iterated value will be used
                            // The following number requires a leading zero to ensure that it is not interpreted as a negative number because its most significant bit is set
                            // Hex number character count                    1234567890123456789012345678901234 = 34 hex characters = 17 bytes = a leading 0 byte plus 16 bytes of value 255
                            BigInteger largestDifference = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            TL.LogMessage(clientNumber, deviceType, $"Initialised largest value: {largestDifference} = {largestDifference:X34}");

                            // Now iterate over the values and pick the entry with the smallest difference in IP address
                            foreach (AvailableInterface availableInterface in availableInterfaces)
                            {
                                if (availableInterface.AddressDistance < largestDifference)
                                {
                                    largestDifference = availableInterface.AddressDistance;
                                    clientHostAddress = $"{serviceType}://{availableInterface.HostName}:{availableInterface.Port}";

                                    TL.LogMessage(clientNumber, deviceType, $"New lowest address difference found: {availableInterface.AddressDistance} ({availableInterface.AddressDistance:X32}) for UniqueD {uniqueId}. Now using URL: {clientHostAddress}");

                                    // Write the new value to the driver's Profile so it is found immediately in future
                                    using (Profile profile = new Profile())
                                    {
                                        profile.DeviceType = deviceType;
                                        profile.WriteValue(driverProgId, SharedConstants.IPADDRESS_PROFILENAME, availableInterface.HostName);
                                        profile.WriteValue(driverProgId, SharedConstants.PORTNUMBER_PROFILENAME, availableInterface.Port.ToString());
                                        TL.LogMessage(clientNumber, deviceType, $"Written new values {availableInterface.HostName} and {availableInterface.Port} to profile {driverProgId}");
                                    }
                                }
                            }


                            TL.BlankLine();
                            break;
                    }
                }
            }

            // Remove any old client, if present
            if (client != null)
            {
                client.ClearHandlers();
            }

            // Create a new client pointing at the alpaca device
            client = new RestClient(clientHostAddress)
            {
                PreAuthenticate = true
            };

            // Add a basic authenticator if the user name is not null or white space
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // Deal with null passwords
                if (string.IsNullOrEmpty(password)) // Handle the special case of an empty string password
                {
                    // Add an HTTP basic authenticator configured with the user name and empty password to the client
                    TL.LogMessage(clientNumber, deviceType, "Creating Authenticator with an empty password.");
                    client.Authenticator = new HttpBasicAuthenticator(userName.Unencrypt(TL), ""); // Need to decrypt the user name so the correct value is sent to the remote device
                }
                else // Handle the normal case of a non-empty string password
                {
                    // Add an HTTP basic authenticator configured with the user name and password to the client
                    TL.LogMessage(clientNumber, deviceType, "Creating Authenticator with a non-empty password.");
                    client.Authenticator = new HttpBasicAuthenticator(userName.Unencrypt(TL), password.Unencrypt(TL)); // Need to decrypt the user name and password so the correct values are sent to the remote device
                }
            }

            // Set the client timeout
            SetClientTimeout(client, deviceResponseTimeout);
        }

        /// <summary>
        /// test whether there is a device at the specified IP address and port by opening a TCP connection to it
        /// </summary>
        /// <param name="ipAddressString">IP address of the device</param>
        /// <param name="portNumber">IP port number on the device</param>
        /// <param name="connectionTimeout">Time to wait before timing out</param>
        /// <param name="TL">Trace logger in which to report progress</param>
        /// <param name="clientNumber">The client's number</param>
        /// <returns></returns>
        private static bool ClientIsUp(string ipAddressString, decimal portNumber, int connectionTimeout, TraceLoggerPlus TL, uint clientNumber)
        {
            TcpClient tcpClient = null;

            bool returnValue = false; // Assume a bad outcome in case there is an exception 

            try
            {
                // Create a TcpClient 
                if (IPAddress.TryParse(ipAddressString, out IPAddress ipAddress))
                {
                    // Create an IPv4 or IPv6 TCP client as required
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork) tcpClient = new TcpClient(AddressFamily.InterNetwork); // Test IPv4 addresses
                    else tcpClient = new TcpClient(AddressFamily.InterNetworkV6);
                    TL.LogMessage(clientNumber, "ClientIsUp", $"Created an {ipAddress.AddressFamily} TCP client");
                }
                else
                {
                    tcpClient = new TcpClient(); // Create a generic TcpClient that can work with host names
                }

                // Create a task that will return True if a connection to the device can be established or False if the connection is rejected or not possible
                Task<bool> connectionTask = tcpClient.ConnectAsync(ipAddressString, (int)portNumber).ContinueWith(task => { return !task.IsFaulted; }, TaskContinuationOptions.ExecuteSynchronously);
                TL.LogMessage(clientNumber, "ClientIsUp", $"Created connection task");

                // Create a task that will time out after the specified time and return a value of False
                Task<bool> timeoutTask = Task.Delay(connectionTimeout * 1000).ContinueWith<bool>(task => false, TaskContinuationOptions.ExecuteSynchronously);
                TL.LogMessage(clientNumber, "ClientIsUp", $"Created timeout task");

                // Create a task that will wait until either of the two preceding tasks completes
                Task<bool> resultTask = Task.WhenAny(connectionTask, timeoutTask).Unwrap();
                TL.LogMessage(clientNumber, "ClientIsUp", $"Waiting for a task to complete");

                // Wait for one of the tasks to complete
                resultTask.Wait();
                TL.LogMessage(clientNumber, "ClientIsUp", $"A task has completed");

                // Test whether or not we connected OK within the timeout period
                if (resultTask.Result) // We did connect OK within the timeout period
                {
                    TL.LogMessage(clientNumber, "ClientIsUp", $"Contacted client OK!");
                    tcpClient.Close();
                    returnValue = true;
                }
                else // We did not connect successfully within the timeout period
                {
                    TL.LogMessage(clientNumber, "ClientIsUp", $"Unable to contact client....");
                    returnValue = false;
                }
            }

            catch (Exception ex)
            {
                TL.LogMessageCrLf(clientNumber, "ClientIsUp", $"Exception: {ex}");

            }
            finally
            {
                tcpClient.Dispose();
            }
            return returnValue;
        }

        #endregion

        #region Profile management
        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        public static void ReadProfile(uint clientNumber, TraceLoggerPlus TL, string deviceType, string driverProgID,
                                       ref bool traceState,
                                       ref bool debugTraceState,
                                       ref string ipAddressString,
                                       ref decimal portNumber,
                                       ref decimal remoteDeviceNumber,
                                       ref string serviceType,
                                       ref int establishConnectionTimeout,
                                       ref int standardDeviceResponseTimeout,
                                       ref int longDeviceResponseTimeout,
                                       ref string userName,
                                       ref string password,
                                       ref bool manageConnectLocally,
                                       ref ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType,
                                       ref ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression,
                                       ref string uniqueId,
                                       ref bool enableRediscovery,
                                       ref bool ipV4Enabled,
                                       ref bool ipV6Enabled,
                                       ref int discoveryPort
                                       )
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = deviceType;

                // Initialise the logging trace state from the Profile
                traceState = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.TRACE_LEVEL_PROFILENAME, string.Empty, SharedConstants.CLIENT_TRACE_LEVEL_DEFAULT);
                TL.Enabled = traceState; // Set the logging state immediately after this has been retrieved from Profile

                // Initialise other variables from the Profile
                debugTraceState = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.DEBUG_TRACE_PROFILENAME, string.Empty, SharedConstants.DEBUG_TRACE_DEFAULT);
                ipAddressString = driverProfile.GetValue(driverProgID, SharedConstants.IPADDRESS_PROFILENAME, string.Empty, SharedConstants.IPADDRESS_DEFAULT);
                portNumber = GetDecimalValue(TL, driverProfile, driverProgID, SharedConstants.PORTNUMBER_PROFILENAME, string.Empty, SharedConstants.PORTNUMBER_DEFAULT);
                remoteDeviceNumber = GetDecimalValue(TL, driverProfile, driverProgID, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, string.Empty, SharedConstants.REMOTE_DEVICE_NUMBER_DEFAULT);
                serviceType = driverProfile.GetValue(driverProgID, SharedConstants.SERVICE_TYPE_PROFILENAME, string.Empty, SharedConstants.SERVICE_TYPE_DEFAULT);
                establishConnectionTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_DEFAULT);
                standardDeviceResponseTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.STANDARD_SERVER_RESPONSE_TIMEOUT_DEFAULT);
                longDeviceResponseTimeout = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, string.Empty, SharedConstants.LONG_SERVER_RESPONSE_TIMEOUT_DEFAULT);
                userName = driverProfile.GetValue(driverProgID, SharedConstants.USERNAME_PROFILENAME, string.Empty, SharedConstants.USERNAME_DEFAULT);
                password = driverProfile.GetValue(driverProgID, SharedConstants.PASSWORD_PROFILENAME, string.Empty, SharedConstants.PASSWORD_DEFAULT);
                manageConnectLocally = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, string.Empty, SharedConstants.MANAGE_CONNECT_LOCALLY_DEFAULT);
                imageArrayTransferType = (ASCOM.Common.Alpaca.ImageArrayTransferType)GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, string.Empty, (int)SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT);
                imageArrayCompression = (ASCOM.Common.Alpaca.ImageArrayCompression)GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, string.Empty, (int)SharedConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT);
                uniqueId = driverProfile.GetValue(driverProgID, SharedConstants.UNIQUEID_PROFILENAME, string.Empty, SharedConstants.UNIQUEID_DEFAULT);
                enableRediscovery = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.ENABLE_REDISCOVERY_PROFILENAME, string.Empty, SharedConstants.ENABLE_REDISCOVERY_DEFAULT);
                ipV4Enabled = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.ENABLE_IPV4_DISCOVERY_PROFILENAME, string.Empty, SharedConstants.ENABLE_IPV4_DISCOVERY_DEFAULT);
                ipV6Enabled = GetBooleanValue(TL, driverProfile, driverProgID, SharedConstants.ENABLE_IPV6_DISCOVERY_PROFILENAME, string.Empty, SharedConstants.ENABLE_IPV6_DISCOVERY_DEFAULT);
                discoveryPort = GetInt32Value(TL, driverProfile, driverProgID, SharedConstants.DISCOVERY_PORT_PROFILENAME, string.Empty, SharedConstants.DISCOVERY_PORT_DEFAULT);

                TL.DebugTraceState = debugTraceState; // Save the debug state for use when needed wherever the trace logger is used

                TL.LogMessage(clientNumber, "ReadProfile", string.Format("New values: Device IP: {0} {1}, Remote device number: {2}", ipAddressString, portNumber.ToString(), remoteDeviceNumber.ToString()));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public static void WriteProfile(uint clientNumber, TraceLoggerPlus TL, string deviceType, string driverProgID,
                                        bool traceState,
                                        bool debugTraceState,
                                        string ipAddressString,
                                        decimal portNumber,
                                        decimal remoteDeviceNumber,
                                        string serviceType,
                                        int establishConnectionTimeout,
                                        int standardDeviceResponseTimeout,
                                        int longDeviceResponseTimeout,
                                        string userName,
                                        string password,
                                        bool manageConnectLocally,
                                        ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType,
                                        ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression,
                                        string uniqueId,
                                        bool enableRediscovery,
                                        bool ipV4Enabled,
                                        bool ipV6Enabled,
                                        int discoveryPort
                                        )
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = deviceType;

                // Save the variable state to the Profile
                driverProfile.WriteValue(driverProgID, SharedConstants.TRACE_LEVEL_PROFILENAME, traceState.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.DEBUG_TRACE_PROFILENAME, debugTraceState.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.IPADDRESS_PROFILENAME, ipAddressString.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.PORTNUMBER_PROFILENAME, portNumber.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, remoteDeviceNumber.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.SERVICE_TYPE_PROFILENAME, serviceType.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.ESTABLISH_CONNECTION_TIMEOUT_PROFILENAME, establishConnectionTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.STANDARD_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, standardDeviceResponseTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.LONG_DEVICE_RESPONSE_TIMEOUT_PROFILENAME, longDeviceResponseTimeout.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.USERNAME_PROFILENAME, userName.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.PASSWORD_PROFILENAME, password.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.MANAGE_CONNECT_LOCALLY_PROFILENAME, manageConnectLocally.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_PROFILENAME, ((int)imageArrayTransferType).ToString());
                driverProfile.WriteValue(driverProgID, SharedConstants.IMAGE_ARRAY_COMPRESSION_PROFILENAME, ((int)imageArrayCompression).ToString());
                driverProfile.WriteValue(driverProgID, SharedConstants.UNIQUEID_PROFILENAME, uniqueId.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.ENABLE_REDISCOVERY_PROFILENAME, enableRediscovery.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.ENABLE_IPV4_DISCOVERY_PROFILENAME, ipV4Enabled.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.ENABLE_IPV6_DISCOVERY_PROFILENAME, ipV6Enabled.ToString(CultureInfo.InvariantCulture));
                driverProfile.WriteValue(driverProgID, SharedConstants.DISCOVERY_PORT_PROFILENAME, discoveryPort.ToString(CultureInfo.InvariantCulture));

                TL.DebugTraceState = debugTraceState; // Save the new debug state for use when needed wherever the trace logger is used

                TL.LogMessage(clientNumber, "WriteProfile", string.Format("New values: Device IP: {0} {1}, Remote device number: {2}", ipAddressString, portNumber.ToString(), remoteDeviceNumber.ToString()));
            }
        }

        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static bool GetBooleanValue(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, bool defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetBooleanValue", $"{valueName} = '{profileValue}'");

            bool convertedOk = Boolean.TryParse(profileValue, out bool returnValue); // Try to convert the profile value to a boolean
            if (!convertedOk) // If unable to convert to a boolean, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetBooleanValue", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;
        }

        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static int GetInt32Value(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, int defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetInt32Value", $"{valueName} = '{profileValue}'");

            bool convertedOk = Int32.TryParse(profileValue, out int returnValue); // Try to convert the profile value to an int
            if (!convertedOk) // If unable to convert to a int, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetInt32Value", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;

        }
        /// <summary>
        /// Read a profile value and return its value handling uninitialised and invalid values by returning a default value instead
        /// </summary>
        /// <param name="driverProfile">Profile object from which to read values</param>
        /// <param name="driverProgId">Driver's ProgID</param>
        /// <param name="valueName">Name of the value to retrieve</param>
        /// <param name="subKeyName">Subkey in which the value is located</param>
        /// <param name="defaultValue">Default value to return if the value doesn't exist or is invalid</param>
        /// <returns></returns>
        private static decimal GetDecimalValue(TraceLoggerPlus TL, Profile driverProfile, string driverProgId, string valueName, string subKeyName, decimal defaultValue)
        {
            string profileValue = driverProfile.GetValue(driverProgId, valueName, subKeyName, defaultValue.ToString()); // Return a profile value substituting a default value if not already set
            TL.LogMessage("GetDecimalValue", $"{valueName} = '{profileValue}'");

            bool convertedOk = Decimal.TryParse(profileValue, out decimal returnValue); // Try to convert the profile value to a decimal
            if (!convertedOk) // If unable to convert to a decimal, return the default value and write this back to the Profile
            {
                TL.LogMessage("GetDecimalValue", $"Correcting invalid value: '{profileValue}' to '{defaultValue}'");
                returnValue = defaultValue;
                driverProfile.WriteValue(driverProgId, valueName, defaultValue.ToString(CultureInfo.InvariantCulture), subKeyName);
            }
            return returnValue;
        }
        #endregion

        #region Remote access methods

        public static void CallMethodWithNoParameters(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        /// <summary>
        /// Overload used by methods other than ImageArray and ImageArrayVariant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="memberType"></param>
        /// <returns></returns>
        public static T GetValue<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, MemberTypes memberType)
        {
            return GetValue<T>(clientNumber, client, URIBase, TL, method, AlpacaConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT, AlpacaConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT, memberType); // Set an arbitrary value for ImageArrayTransferType
        }

        /// <summary>
        /// Overload for use by the ImageArray and ImageArrayVariant methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="imageArrayTransferType"></param>
        /// <param name="imageArrayCompression"></param>
        /// <param name="memberType"></param>
        /// <returns></returns>
        public static T GetValue<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType, ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            return SendToRemoteDevice<T>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, imageArrayTransferType, imageArrayCompression, memberType);
        }

        public static void SetBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, bool parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetInt(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, int parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetShort(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, double parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { method, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetDoubleWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, double parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.VALUE_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetBoolWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, bool parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.STATE_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static void SetStringWithShortParameter(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short index, string parmeterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, index.ToString(CultureInfo.InvariantCulture) },
                { AlpacaConstants.NAME_PARAMETER_NAME, parmeterValue.ToString(CultureInfo.InvariantCulture) }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, method, Parameters, Method.PUT, memberType);
        }

        public static string GetStringIndexedString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, string parameterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.SENSORNAME_PARAMETER_NAME, parameterValue }
            };
            return SendToRemoteDevice<string>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, memberType);
        }

        public static double GetStringIndexedDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, string parameterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.SENSORNAME_PARAMETER_NAME, parameterValue }
            };
            return SendToRemoteDevice<double>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, memberType);
        }

        public static double GetShortIndexedDouble(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDevice<double>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, memberType);
        }

        public static bool GetShortIndexedBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDevice<bool>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, memberType);
        }

        public static string GetShortIndexedString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, short parameterValue, MemberTypes memberType)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ID_PARAMETER_NAME, parameterValue.ToString(CultureInfo.InvariantCulture) }
            };
            return SendToRemoteDevice<string>(clientNumber, client, URIBase, TL, method, Parameters, Method.GET, memberType);
        }

        /// <summary>
        /// Send a command to the remote device, retrying a given number of times if a socket exception is received
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="URIBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="Parameters"></param>
        /// <param name="HttpMethod"></param>
        /// <returns></returns>
        public static T SendToRemoteDevice<T>(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string method, Dictionary<string, string> Parameters, Method HttpMethod, MemberTypes memberType)
        {
            return SendToRemoteDevice<T>(clientNumber, client, URIBase, TL, method, Parameters, HttpMethod, AlpacaConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT, AlpacaConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT, memberType);
        }

        /// <summary>
        /// Send a command to the remote device, retrying a given number of times if a socket exception is received, specifying an image array transfer type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNumber"></param>
        /// <param name="client"></param>
        /// <param name="uriBase"></param>
        /// <param name="TL"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="httpMethod"></param>
        /// <param name="imageArrayTransferType"></param>
        /// <returns></returns>
        public static T SendToRemoteDevice<T>(uint clientNumber, RestClient client, string uriBase, TraceLoggerPlus TL, string method, Dictionary<string, string> parameters, Method httpMethod, ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType, ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression, MemberTypes memberType)
        {
            int retryCounter = 0; // Initialise the socket error retry counter
            Stopwatch sw = new Stopwatch(); // Stopwatch to time activities
            long lastTime = 0; // Holder for the accumulated elapsed time, used when reporting intermediate step timings
            Array remoteArray = null;

            sw.Start();

            do // Socket communications error retry loop
            {
                try
                {
                    const string LOG_FORMAT_STRING = "Client Txn ID: {0}, Server Txn ID: {1}, Value: {2}";

                    RestResponseBase restResponseBase = null; // This has to be the base class of the data type classes in order for exception and error responses to be handled generically
                    RestRequest request = new RestRequest((uriBase + method).ToLowerInvariant(), httpMethod);
                    {
                        request.RequestFormat = DataFormat.Json;
                    };

                    // Set the default JSON compression behaviour to None
                    client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.None); // Prevent any decompression

                    // Apply appropriate headers to control image array transfer
                    if (typeof(T) == typeof(Array))
                    {
                        switch (imageArrayCompression)
                        {
                            case ImageArrayCompression.None:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.None); // Prevent any decompression
                                break;
                            case ImageArrayCompression.Deflate:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.Deflate); // Allow only Deflate decompression
                                break;
                            case ImageArrayCompression.GZip:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.GZip); // Allow only GZip decompression
                                break;
                            case ImageArrayCompression.GZipOrDeflate:
                                client.ConfigureWebRequest(wr => wr.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip); // Allow both Deflate and GZip decompression
                                break;
                            default:
                                throw new InvalidValueException($"Invalid image array compression type: {imageArrayCompression} - Correct this in the Dynamic Client setup dialogue.");
                        }

                        switch (imageArrayTransferType)
                        {
                            case ImageArrayTransferType.JSON:
                                // No extra action because "accepts = application/json" will be applied automatically by the client
                                break;

                            case ImageArrayTransferType.Base64HandOff:
                                request.AddHeader(AlpacaConstants.BASE64_HANDOFF_HEADER, AlpacaConstants.BASE64_HANDOFF_SUPPORTED);
                                break;

                            case ImageArrayTransferType.ImageBytes:
                                request.AddHeader(AlpacaConstants.ACCEPT_HEADER_NAME, AlpacaConstants.IMAGE_BYTES_ACCEPT_HEADER);
                                break;

                            case ImageArrayTransferType.BestAvailable:
                                request.AddHeader(AlpacaConstants.BASE64_HANDOFF_HEADER, AlpacaConstants.BASE64_HANDOFF_SUPPORTED);
                                request.AddHeader(AlpacaConstants.ACCEPT_HEADER_NAME, AlpacaConstants.IMAGE_BYTES_ACCEPT_HEADER);
                                break;

                            default:
                                throw new InvalidValueException($"Invalid image array transfer type: {imageArrayTransferType} - Correct this in the Dynamic Client setup dialogue.");
                        }
                    }

                    // Add the transaction number and client ID parameters
                    uint transaction = TransactionNumber();
                    request.AddParameter(AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME, transaction.ToString());
                    request.AddParameter(AlpacaConstants.CLIENTID_PARAMETER_NAME, clientNumber.ToString());

                    // Add any supplied parameters to the request
                    foreach (KeyValuePair<string, string> parameter in parameters)
                    {
                        request.AddParameter(parameter.Key, parameter.Value);
                    }

                    // Call the remote device and get the response
                    lastTime = sw.ElapsedMilliseconds;
                    if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, "Client Txn ID: " + transaction.ToString() + ", Sending command to remote device");
                    if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, $"Client base URL: '{client.BaseUrl}', URI base: '{uriBase}', Method: {method}.");

                    IRestResponse deviceJsonResponse;

                    // Use the more efficient .NET HttpClient to get the large image array as a byte[] for the ImageBytes mechanic
                    if ((typeof(T) == typeof(Array)) & ((imageArrayTransferType == ImageArrayTransferType.ImageBytes) | ((imageArrayTransferType == ImageArrayTransferType.BestAvailable))))
                    {
                        deviceJsonResponse = GetResponse($"{client.BaseUrl}{uriBase}{method}".ToLowerInvariant(), AlpacaConstants.IMAGE_BYTES_ACCEPT_HEADER, clientNumber, transaction, TL); ;
                    }
                    else // Use the RestSharp client for everything else
                    {
                        deviceJsonResponse = client.Execute(request);
                    }
                    if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, $"Returned data content type: '{deviceJsonResponse.ContentType}'");

                    long timeDeviceResponse = sw.ElapsedMilliseconds - lastTime;

                    // Log the device's response
                    if (deviceJsonResponse.ContentType.ToLowerInvariant().Contains(AlpacaConstants.IMAGE_BYTES_MIME_TYPE)) // Image bytes response
                    {
                        TL.LogMessage(clientNumber, method, $"Response Status: '{deviceJsonResponse.StatusDescription}', Content type: {deviceJsonResponse.ContentType}, Content encoding: {deviceJsonResponse.ContentEncoding}, Content length: {deviceJsonResponse.ContentLength}, Protocol version: {deviceJsonResponse.ProtocolVersion}");
                    }
                    else // JSON response
                    {
                        string responseContent;
                        if (deviceJsonResponse.Content.Length > 1000) responseContent = deviceJsonResponse.Content.Substring(0, 1000);
                        else responseContent = deviceJsonResponse.Content;
                        TL.LogMessage(clientNumber, method, $"Response Status: '{deviceJsonResponse.StatusDescription}', Response: {responseContent}");
                    }

                    // Assess success at the communications level and handle accordingly 
                    if ((deviceJsonResponse.ResponseStatus == ResponseStatus.Completed) & (deviceJsonResponse.StatusCode == System.Net.HttpStatusCode.OK))
                    {
                        // GENERAL MULTI-DEVICE TYPES
                        if (typeof(T) == typeof(bool))
                        {
                            BoolResponse boolResponse = JsonConvert.DeserializeObject<BoolResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, boolResponse.ClientTransactionID, boolResponse.ServerTransactionID, boolResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, boolResponse)) return (T)((object)boolResponse.Value);
                            restResponseBase = (RestResponseBase)boolResponse;
                        }
                        if (typeof(T) == typeof(float))
                        {
                            // Handle float as double over the web, remembering to convert the returned value to float
                            DoubleResponse doubleResponse = JsonConvert.DeserializeObject<DoubleResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleResponse.ClientTransactionID, doubleResponse.ServerTransactionID, doubleResponse.Value.ToString()));
                            float floatValue = (float)doubleResponse.Value;
                            if (CallWasSuccessful(TL, doubleResponse)) return (T)((object)floatValue);
                            restResponseBase = (RestResponseBase)doubleResponse;
                        }
                        if (typeof(T) == typeof(double))
                        {
                            DoubleResponse doubleResponse = JsonConvert.DeserializeObject<DoubleResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleResponse.ClientTransactionID, doubleResponse.ServerTransactionID, doubleResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, doubleResponse)) return (T)((object)doubleResponse.Value);
                            restResponseBase = (RestResponseBase)doubleResponse;
                        }
                        if (typeof(T) == typeof(string))
                        {
                            TL.LogMessage(clientNumber, method, "About to de-serialise StringResponse");
                            StringResponse stringResponse = JsonConvert.DeserializeObject<StringResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringResponse.ClientTransactionID, stringResponse.ServerTransactionID, (stringResponse.Value is null) ? "NO VALUE OR NULL VALUE RETURNED" : (stringResponse.Value.Length < 1000) ? stringResponse.Value : stringResponse.Value.Substring(0, 1000)));
                            if (CallWasSuccessful(TL, stringResponse)) return (T)((object)stringResponse.Value);
                            restResponseBase = (RestResponseBase)stringResponse;
                        }
                        if (typeof(T) == typeof(string[]))
                        {
                            StringArrayResponse stringArrayResponse = JsonConvert.DeserializeObject<StringArrayResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringArrayResponse.ClientTransactionID, stringArrayResponse.ServerTransactionID, (stringArrayResponse.Value is null) ? "NO VALUE OR NULL VALUE RETURNED" : stringArrayResponse.Value.Count().ToString()));
                            if (CallWasSuccessful(TL, stringArrayResponse)) return (T)((object)stringArrayResponse.Value);
                            restResponseBase = (RestResponseBase)stringArrayResponse;
                        }
                        if (typeof(T) == typeof(short))
                        {
                            ShortResponse shortResponse = JsonConvert.DeserializeObject<ShortResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortResponse.ClientTransactionID, shortResponse.ServerTransactionID, shortResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, shortResponse)) return (T)((object)shortResponse.Value);
                            restResponseBase = (RestResponseBase)shortResponse;
                        }
                        if (typeof(T) == typeof(int))
                        {
                            IntResponse intResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intResponse.ClientTransactionID, intResponse.ServerTransactionID, intResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, intResponse)) return (T)((object)intResponse.Value);
                            restResponseBase = (RestResponseBase)intResponse;
                        }
                        if (typeof(T) == typeof(int[]))
                        {
                            IntArray1DResponse intArrayResponse = JsonConvert.DeserializeObject<IntArray1DResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArrayResponse.ClientTransactionID, intArrayResponse.ServerTransactionID, (intArrayResponse.Value is null) ? "NO VALUE OR NULL VALUE RETURNED" : intArrayResponse.Value.Count().ToString()));
                            if (CallWasSuccessful(TL, intArrayResponse)) return (T)((object)intArrayResponse.Value);
                            restResponseBase = (RestResponseBase)intArrayResponse;
                        }
                        if (typeof(T) == typeof(DateTime))
                        {
                            DateTimeResponse dateTimeResponse = JsonConvert.DeserializeObject<DateTimeResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, dateTimeResponse.ClientTransactionID, dateTimeResponse.ServerTransactionID, dateTimeResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, dateTimeResponse)) return (T)((object)dateTimeResponse.Value);
                            restResponseBase = (RestResponseBase)dateTimeResponse;
                        }
                        if (typeof(T) == typeof(List<string>)) // Used for ArrayLists of string
                        {
                            StringListResponse stringListResponse = JsonConvert.DeserializeObject<StringListResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, stringListResponse.ClientTransactionID, stringListResponse.ServerTransactionID, (stringListResponse.Value is null) ? "NO VALUE OR NULL VALUE RETURNED" : stringListResponse.Value.Count.ToString()));
                            if (CallWasSuccessful(TL, stringListResponse)) return (T)((object)stringListResponse.Value);
                            restResponseBase = (RestResponseBase)stringListResponse;
                        }
                        if (typeof(T) == typeof(NoReturnValue)) // Used for Methods that have no response and Property Set members
                        {
                            MethodResponse deviceResponse = JsonConvert.DeserializeObject<MethodResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, deviceResponse.ClientTransactionID.ToString(), deviceResponse.ServerTransactionID.ToString(), "No response"));
                            if (CallWasSuccessful(TL, deviceResponse)) return (T)((object)new NoReturnValue());
                            restResponseBase = (RestResponseBase)deviceResponse;
                        }

                        // DEVICE SPECIFIC TYPES
                        if (typeof(T) == typeof(PierSide))
                        {
                            IntResponse pierSideResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, pierSideResponse.ClientTransactionID, pierSideResponse.ServerTransactionID, pierSideResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, pierSideResponse)) return (T)((object)pierSideResponse.Value);
                            restResponseBase = (RestResponseBase)pierSideResponse;
                        }
                        if (typeof(T) == typeof(ITrackingRates))
                        {
                            TrackingRatesResponse trackingRatesResponse = JsonConvert.DeserializeObject<TrackingRatesResponse>(deviceJsonResponse.Content);
                            if (trackingRatesResponse.Value != null) // A TrackingRates object was returned so process the response normally
                            {
                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, trackingRatesResponse.ClientTransactionID, trackingRatesResponse.ServerTransactionID, trackingRatesResponse.Value.Count));
                                List<DriveRates> rates = new List<DriveRates>();
                                DriveRates[] ratesArray = new DriveRates[trackingRatesResponse.Value.Count];
                                int i = 0;
                                foreach (DriveRates rate in trackingRatesResponse.Value)
                                {
                                    TL.LogMessage(clientNumber, method, string.Format("Rate: {0}", rate.ToString()));
                                    ratesArray[i] = rate;
                                    i++;
                                }

                                TrackingRates trackingRates = new TrackingRates();
                                trackingRates.SetRates(ratesArray);
                                if (CallWasSuccessful(TL, trackingRatesResponse))
                                {
                                    TL.LogMessage(clientNumber, method, $"Returning {trackingRates.Count} tracking rates to the client - now measured from trackingRates");
                                    return (T)((object)trackingRates);
                                }
                            }
                            else // No TrackingRates object was returned so handle this as an error
                            {
                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, trackingRatesResponse.ClientTransactionID, trackingRatesResponse.ServerTransactionID, "NO VALUE OR NULL VALUE RETURNED"));

                                // Now force an error return
                                trackingRatesResponse = new TrackingRatesResponse();
                                trackingRatesResponse.ErrorNumber = DYNAMIC_DRIVER_ERROR_NUMBER;
                                trackingRatesResponse.ErrorMessage = "Dynamic driver generated error: the Alpaca device returned no value or a null value for TrackingRates";
                            }
                            restResponseBase = (RestResponseBase)trackingRatesResponse;
                        }
                        if (typeof(T) == typeof(EquatorialCoordinateType))
                        {
                            IntResponse equatorialCoordinateResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, equatorialCoordinateResponse.ClientTransactionID, equatorialCoordinateResponse.ServerTransactionID, equatorialCoordinateResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, equatorialCoordinateResponse)) return (T)((object)equatorialCoordinateResponse.Value);
                            restResponseBase = (RestResponseBase)equatorialCoordinateResponse;
                        }
                        if (typeof(T) == typeof(AlignmentModes))
                        {
                            IntResponse alignmentModesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, alignmentModesResponse.ClientTransactionID, alignmentModesResponse.ServerTransactionID, alignmentModesResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, alignmentModesResponse)) return (T)((object)alignmentModesResponse.Value);
                            restResponseBase = (RestResponseBase)alignmentModesResponse;
                        }
                        if (typeof(T) == typeof(DriveRates))
                        {
                            IntResponse driveRatesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, driveRatesResponse.ClientTransactionID, driveRatesResponse.ServerTransactionID, driveRatesResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, driveRatesResponse)) return (T)((object)driveRatesResponse.Value);
                            restResponseBase = (RestResponseBase)driveRatesResponse;
                        }
                        if (typeof(T) == typeof(SensorType))
                        {
                            IntResponse sensorTypeResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, sensorTypeResponse.ClientTransactionID, sensorTypeResponse.ServerTransactionID, sensorTypeResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, sensorTypeResponse)) return (T)((object)sensorTypeResponse.Value);
                            restResponseBase = (RestResponseBase)sensorTypeResponse;
                        }
                        if (typeof(T) == typeof(CameraStates))
                        {
                            IntResponse cameraStatesResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, cameraStatesResponse.ClientTransactionID, cameraStatesResponse.ServerTransactionID, cameraStatesResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, cameraStatesResponse)) return (T)((object)cameraStatesResponse.Value);
                            restResponseBase = (RestResponseBase)cameraStatesResponse;
                        }
                        if (typeof(T) == typeof(ShutterState))
                        {
                            IntResponse domeShutterStateResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, domeShutterStateResponse.ClientTransactionID, domeShutterStateResponse.ServerTransactionID, domeShutterStateResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, domeShutterStateResponse)) return (T)((object)domeShutterStateResponse.Value);
                            restResponseBase = (RestResponseBase)domeShutterStateResponse;
                        }
                        if (typeof(T) == typeof(CoverStatus))
                        {
                            IntResponse coverStatusResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, coverStatusResponse.ClientTransactionID, coverStatusResponse.ServerTransactionID, coverStatusResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, coverStatusResponse)) return (T)((object)coverStatusResponse.Value);
                            restResponseBase = (RestResponseBase)coverStatusResponse;
                        }
                        if (typeof(T) == typeof(CalibratorStatus))
                        {
                            IntResponse calibratorStatusResponse = JsonConvert.DeserializeObject<IntResponse>(deviceJsonResponse.Content);
                            TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, calibratorStatusResponse.ClientTransactionID, calibratorStatusResponse.ServerTransactionID, calibratorStatusResponse.Value.ToString()));
                            if (CallWasSuccessful(TL, calibratorStatusResponse)) return (T)((object)calibratorStatusResponse.Value);
                            restResponseBase = (RestResponseBase)calibratorStatusResponse;
                        }
                        if (typeof(T) == typeof(IAxisRates))
                        {
                            AxisRatesResponse axisRatesResponse = JsonConvert.DeserializeObject<AxisRatesResponse>(deviceJsonResponse.Content);
                            if (axisRatesResponse.Value != null) // A AxisRates object was returned so process the response normally
                            {
                                AxisRates axisRates = new AxisRates((TelescopeAxes)(Convert.ToInt32(parameters[AlpacaConstants.AXIS_PARAMETER_NAME])));
                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, axisRatesResponse.ClientTransactionID.ToString(), axisRatesResponse.ServerTransactionID.ToString(), axisRatesResponse.Value.Count.ToString()));
                                foreach (RateResponse rr in axisRatesResponse.Value)
                                {
                                    axisRates.Add(rr.Minimum, rr.Maximum, TL);
                                    TL.LogMessage(clientNumber, method, string.Format("Found rate: {0} - {1}", rr.Minimum, rr.Maximum));
                                }

                                if (CallWasSuccessful(TL, axisRatesResponse)) return (T)((object)axisRates);
                            }
                            else // No AxisRates object was returned so handle this as an error
                            {
                                TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, axisRatesResponse.ClientTransactionID, axisRatesResponse.ServerTransactionID, "NO VALUE OR NULL VALUE RETURNED"));

                                // Now force an error return
                                axisRatesResponse = new AxisRatesResponse();
                                axisRatesResponse.ErrorNumber = DYNAMIC_DRIVER_ERROR_NUMBER;
                                axisRatesResponse.ErrorMessage = "Dynamic driver generated error: the Alpaca device returned no value or a null value for AxisRates";
                            }

                            restResponseBase = (RestResponseBase)axisRatesResponse;
                        }
                        if (typeof(T) == typeof(Array)) // Used for Camera.ImageArray and Camera.ImageArrayVariant
                        {
                            // Include some debug logging
                            if (TL.DebugTraceState)
                            {
                                foreach (var header in deviceJsonResponse.Headers)
                                {
                                    TL.LogMessage(clientNumber, method, $"Response header {header.Name} = {header.Value}");
                                }
                            }

                            // Handle the ImageBytes image transfer mechanic
                            if (deviceJsonResponse.ContentType.ToLowerInvariant().Contains(AlpacaConstants.IMAGE_BYTES_MIME_TYPE)) // Image bytes have been returned so the server supports raw array data transfer
                            {
                                Stopwatch swOverall = new Stopwatch();

                                // Get the byte array from the task
                                byte[] imageBytes = deviceJsonResponse.RawBytes;

                                sw.Stop();
                                TL.LogMessage(clientNumber, "ImageBytes", $"Downloaded {imageBytes.Length} bytes in {sw.ElapsedMilliseconds}ms.");

                                TL.LogMessage(clientNumber, "ImageBytes", $"Metadata bytes: " +
                                    $"[ {imageBytes[0]:X2} {imageBytes[1]:X2} {imageBytes[2]:X2} {imageBytes[3]:X2} ] [ {imageBytes[4]:X2} {imageBytes[5]:X2} {imageBytes[6]:X2} {imageBytes[7]:X2} ] " +
                                    $"[ {imageBytes[8]:X2} {imageBytes[9]:X2} {imageBytes[10]:X2} {imageBytes[11]:X2} ] [ {imageBytes[12]:X2} {imageBytes[13]:X2} {imageBytes[14]:X2} {imageBytes[15]:X2} ] " +
                                    $"[ {imageBytes[16]:X2} {imageBytes[17]:X2} {imageBytes[18]:X2} {imageBytes[19]:X2} ] [ {imageBytes[20]:X2} {imageBytes[21]:X2} {imageBytes[22]:X2} {imageBytes[23]:X2} ] " +
                                    $"[ {imageBytes[24]:X2} {imageBytes[25]:X2} {imageBytes[26]:X2} {imageBytes[27]:X2} ] [ {imageBytes[28]:X2} {imageBytes[29]:X2} {imageBytes[30]:X2} {imageBytes[31]:X2} ] " +
                                    $"[ {imageBytes[32]:X2} {imageBytes[33]:X2} {imageBytes[34]:X2} {imageBytes[35]:X2} ] [ {imageBytes[36]:X2} {imageBytes[37]:X2} {imageBytes[38]:X2} {imageBytes[39]:X2} ] " +
                                    $"[ {imageBytes[40]:X2} {imageBytes[41]:X2} {imageBytes[42]:X2} {imageBytes[43]:X2} ]");

                                int metadataVersion = imageBytes.GetMetadataVersion();
                                TL.LogMessage(clientNumber, "ImageBytes", $"Metadata version: {metadataVersion}");

                                AlpacaErrors errorNumber;
                                switch (metadataVersion)
                                {
                                    case 1:
                                        ArrayMetadataV1 metadataV1 = imageBytes.GetMetadataV1();
                                        TL.LogMessage(clientNumber, "ImageArrayBytes", $"Received array: Metadata version: {metadataV1.MetadataVersion}, " +
                                            $"Error number: {metadataV1.ErrorNumber}, " +
                                            $"Client transaction ID: {metadataV1.ClientTransactionID}, " +
                                            $"Server transaction ID: {metadataV1.ServerTransactionID}, " +
                                            $"Image element type: {metadataV1.ImageElementType}, " +
                                            $"Transmission element type: {metadataV1.TransmissionElementType}, " +
                                            $"Array rank: {metadataV1.Rank}, " +
                                            $"Dimension 1: {metadataV1.Dimension1}, " +
                                            $"Dimension 2: {metadataV1.Dimension2}, " +
                                            $"Dimension 3: {metadataV1.Dimension3}.");

                                        errorNumber = metadataV1.ErrorNumber;
                                        break;

                                    default:
                                        throw new InvalidValueException($"ImageArray - ImageArrayBytes - Received an unsupported metadata version number: {metadataVersion} from the Alpaca device.");
                                }

                                // Convert the byte[] back to an image array
                                sw.Restart();
                                Array returnArray = imageBytes.ToImageArray();
                                TL.LogMessage(clientNumber, "ImageBytes", $"Converted byte[] to image array in {sw.ElapsedMilliseconds}ms. Overall time: {swOverall.ElapsedMilliseconds}ms.");
                                TL.LogMessage(clientNumber, "", $"");

                                return (T)(Object)returnArray;

                                // No need for error handling here because any error will have been returned as a JSON response rather than as this ImageBytes response.
                            }

                            // Handle the base64 hand-off image transfer mechanic
                            else if (deviceJsonResponse.Headers.Any(t => t.Name == AlpacaConstants.BASE64_HANDOFF_HEADER)) // Base64 format header is present so the server supports base64 serialised transfer
                            {
                                // De-serialise the JSON image array hand-off response 
                                sw.Restart(); // Clear and start the stopwatch
                                Base64ArrayHandOffResponse base64HandOffresponse = JsonConvert.DeserializeObject<Base64ArrayHandOffResponse>(deviceJsonResponse.Content);
                                if (CallWasSuccessful(TL, base64HandOffresponse))
                                {
                                    ImageArrayElementTypes arrayType = (ImageArrayElementTypes)base64HandOffresponse.Type; // Extract the array type from the JSON response

                                    TL.LogMessage(clientNumber, method, $"Base64 - Extracted array information in {sw.ElapsedMilliseconds}ms. Array Type: {arrayType}, Rank: {base64HandOffresponse.Rank}, Dimension 0 length: {base64HandOffresponse.Dimension0Length}, Dimension 1 length: {base64HandOffresponse.Dimension1Length}, Dimension 2 length: {base64HandOffresponse.Dimension2Length}");
                                    sw.Restart();

                                    TL.LogMessage(clientNumber, method, $"Base64 - Downloading base64 serialised image");

                                    // Construct an HTTP request to get the base 64 encoded image
                                    string base64Uri = (client.BaseUrl + uriBase.TrimStart('/') + method.ToLowerInvariant() + AlpacaConstants.BASE64_HANDOFF_FILE_DOWNLOAD_URI_EXTENSION).ToLowerInvariant(); // Create the download URI from the REST client elements
                                    if (TL.DebugTraceState) TL.LogMessage(clientNumber, method, $"Base64 URI: {base64Uri}");

                                    // Create a variable to hold the returned base 64 string
                                    string base64ArrayString = "";

                                    // Create a handler to indicate the compression levels supported by this client
                                    using (HttpClientHandler imageDownloadHandler = new HttpClientHandler())
                                    {
                                        switch (imageArrayCompression)
                                        {
                                            case ASCOM.Common.Alpaca.ImageArrayCompression.None:
                                                imageDownloadHandler.AutomaticDecompression = DecompressionMethods.None;
                                                break;
                                            case ASCOM.Common.Alpaca.ImageArrayCompression.Deflate:
                                                imageDownloadHandler.AutomaticDecompression = DecompressionMethods.Deflate;
                                                break;
                                            case ASCOM.Common.Alpaca.ImageArrayCompression.GZip:
                                                imageDownloadHandler.AutomaticDecompression = DecompressionMethods.GZip;
                                                break;
                                            case ASCOM.Common.Alpaca.ImageArrayCompression.GZipOrDeflate:
                                                imageDownloadHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip; // Allow both Deflate and GZip decompression
                                                break;
                                            default:
                                                throw new InvalidValueException($"Unknown ImageArrayCompression value: {imageArrayCompression} - Can't proceed further!");
                                        }

                                        // Create an HTTP client  to download the base64 string
                                        using (HttpClient httpClient = new HttpClient(imageDownloadHandler))
                                        {
                                            // Get the async stream from the HTTPClient
                                            Stream base64ArrayStream = httpClient.GetStreamAsync(base64Uri).Result;
                                            TL.LogMessage(clientNumber, method, $"Downloaded base64 stream obtained in {sw.ElapsedMilliseconds}ms"); sw.Restart();

                                            // Read the stream contents into the string variable ready for further processing
                                            using (StreamReader sr = new StreamReader(base64ArrayStream, System.Text.Encoding.ASCII, false))
                                            {
                                                base64ArrayString = sr.ReadToEnd();
                                            }
                                        }
                                    }

                                    TL.LogMessage(clientNumber, method, $"Read base64 string from stream ({base64ArrayString.Length} bytes) in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    try { TL.LogMessage(clientNumber, method, $"Base64 string start: {base64ArrayString.Substring(0, 300)}"); } catch { }
                                    try { TL.LogMessage(clientNumber, method, $"Base64 string end: {base64ArrayString.Substring(60000000, 300)}"); } catch { }

                                    // Convert the array from base64 encoding to a byte array
                                    byte[] base64ArrayByteArray = Convert.FromBase64String(base64ArrayString);
                                    TL.LogMessage(clientNumber, method, $"Converted base64 string of length {base64ArrayString.Length} to byte array of length {base64ArrayByteArray.Length} in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    string byteLine = "";
                                    try
                                    {
                                        for (int i = 0; i < 300; i++)
                                        {
                                            byteLine += base64ArrayByteArray[i].ToString("X2") + " ";
                                        }
                                        TL.LogMessage(clientNumber, method, $"Converted base64 bytes: {byteLine}");
                                    }
                                    catch { }

                                    // Now create and populate an appropriate array to return to the client that mirrors the array type returned by the device
                                    switch (arrayType) // Handle the different array return types
                                    {
                                        case ImageArrayElementTypes.Int32:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new int[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new int[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }

                                            // Copy the array bytes to the response array that will return to the client
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length);
                                            break;

                                        case ImageArrayElementTypes.Int16:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new short[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new short[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length); // Copy the array bytes to the response array that will return to the client
                                            break;

                                        case ImageArrayElementTypes.Double:
                                            switch (base64HandOffresponse.Rank)
                                            {
                                                case 2:
                                                    remoteArray = new double[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length];
                                                    break;

                                                case 3:
                                                    remoteArray = new double[base64HandOffresponse.Dimension0Length, base64HandOffresponse.Dimension1Length, base64HandOffresponse.Dimension2Length];
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + base64HandOffresponse.Rank + " are not supported.");
                                            }
                                            Buffer.BlockCopy(base64ArrayByteArray, 0, remoteArray, 0, base64ArrayByteArray.Length); // Copy the array bytes to the response array that will return to the client
                                            break;

                                        default:
                                            throw new InvalidOperationException($"SendToRemoteDevice Base64HandOff - Image array element type {arrayType} is not supported. The device returned this value: {base64HandOffresponse.Type}");
                                    }

                                    if (TL.DebugTraceState)
                                    {
                                        TL.LogMessage(clientNumber, method, $"Created and copied the array in {sw.ElapsedMilliseconds}ms"); sw.Restart();
                                    }
                                    TL.LogMessage(clientNumber, "", $"");

                                    return (T)(object)remoteArray;
                                }
                                restResponseBase = (RestResponseBase)base64HandOffresponse;
                            }

                            // Handle a conventional JSON response with integer array elements individually serialised
                            else
                            {
                                sw.Restart(); // Clear and start the stopwatch
                                ImageArrayResponseBase responseBase = JsonConvert.DeserializeObject<ImageArrayResponseBase>(deviceJsonResponse.Content);
                                if (CallWasSuccessful(TL, responseBase))
                                {
                                    ImageArrayElementTypes arrayType = (ImageArrayElementTypes)responseBase.Type;
                                    int arrayRank = responseBase.Rank;

                                    // Include some debug logging
                                    if (TL.DebugTraceState)
                                    {
                                        TL.LogMessage(clientNumber, method, $"Extracted array type and rank by JsonConvert.DeserializeObject in {sw.ElapsedMilliseconds}ms. Type: {arrayType}, Rank: {arrayRank}, Response values - Type: {responseBase.Type}, Rank: {responseBase.Rank}");
                                    }

                                    sw.Restart(); // Clear and start the stopwatch
                                    switch (arrayType) // Handle the different return types that may come from ImageArrayVariant
                                    {
                                        case ImageArrayElementTypes.Int32:
                                            switch (arrayRank)
                                            {
                                                case 2:
                                                    IntArray2DResponse intArray2DResponse = JsonConvert.DeserializeObject<IntArray2DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArray2DResponse.ClientTransactionID, intArray2DResponse.ServerTransactionID, intArray2DResponse.Rank.ToString())); //, intArray2DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)intArray2DResponse.Type}, Rank: {intArray2DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, intArray2DResponse)) return (T)((object)intArray2DResponse.Value);
                                                    restResponseBase = (RestResponseBase)intArray2DResponse;
                                                    break;

                                                case 3:
                                                    IntArray3DResponse intArray3DResponse = JsonConvert.DeserializeObject<IntArray3DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, intArray3DResponse.ClientTransactionID, intArray3DResponse.ServerTransactionID, intArray3DResponse.Rank.ToString())); //, intArray3DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)intArray3DResponse.Type}, Rank: {intArray3DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, intArray3DResponse)) return (T)((object)intArray3DResponse.Value);
                                                    restResponseBase = (RestResponseBase)intArray3DResponse;
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                            }
                                            break;

                                        case ImageArrayElementTypes.Int16:
                                            switch (arrayRank)
                                            {
                                                case 2:
                                                    ShortArray2DResponse shortArray2DResponse = JsonConvert.DeserializeObject<ShortArray2DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortArray2DResponse.ClientTransactionID, shortArray2DResponse.ServerTransactionID, shortArray2DResponse.Rank.ToString())); //, shortArray2DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)shortArray2DResponse.Type}, Rank: {shortArray2DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, shortArray2DResponse)) return (T)((object)shortArray2DResponse.Value);
                                                    restResponseBase = (RestResponseBase)shortArray2DResponse;
                                                    break;

                                                case 3:
                                                    ShortArray3DResponse shortArray3DResponse = JsonConvert.DeserializeObject<ShortArray3DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, shortArray3DResponse.ClientTransactionID, shortArray3DResponse.ServerTransactionID, shortArray3DResponse.Rank.ToString())); //, shortArray3DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)shortArray3DResponse.Type}, Rank: {shortArray3DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, shortArray3DResponse)) return (T)((object)shortArray3DResponse.Value);
                                                    restResponseBase = (RestResponseBase)shortArray3DResponse;
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                            }
                                            break;

                                        case ImageArrayElementTypes.Double:
                                            switch (arrayRank)
                                            {
                                                case 2:
                                                    DoubleArray2DResponse doubleArray2DResponse = JsonConvert.DeserializeObject<DoubleArray2DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleArray2DResponse.ClientTransactionID, doubleArray2DResponse.ServerTransactionID, doubleArray2DResponse.Rank.ToString())); //, doubleArray2DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)doubleArray2DResponse.Type}, Rank: {doubleArray2DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, doubleArray2DResponse)) return (T)((object)doubleArray2DResponse.Value);
                                                    restResponseBase = (RestResponseBase)doubleArray2DResponse;
                                                    break;

                                                case 3:
                                                    DoubleArray3DResponse doubleArray3DResponse = JsonConvert.DeserializeObject<DoubleArray3DResponse>(deviceJsonResponse.Content);
                                                    TL.LogMessage(clientNumber, method, string.Format(LOG_FORMAT_STRING, doubleArray3DResponse.ClientTransactionID, doubleArray3DResponse.ServerTransactionID, doubleArray3DResponse.Rank.ToString())); //, doubleArray3DResponse.Method));
                                                    TL.LogMessage(clientNumber, method, $"Array was deserialised in {sw.ElapsedMilliseconds} ms, Type: {(ImageArrayElementTypes)doubleArray3DResponse.Type}, Rank: {doubleArray3DResponse.Rank}");
                                                    if (CallWasSuccessful(TL, doubleArray3DResponse)) return (T)((object)doubleArray3DResponse.Value);
                                                    restResponseBase = (RestResponseBase)doubleArray3DResponse;
                                                    break;

                                                default:
                                                    throw new InvalidOperationException("Arrays of Rank " + arrayRank + " are not supported.");
                                            }
                                            break;

                                        default:
                                            throw new InvalidOperationException($"SendToRemoteDevice JSON - Image array element type {arrayType} is not supported. The device returned this value: {responseBase.Type}");
                                    }
                                }
                                restResponseBase = (RestResponseBase)responseBase;
                            } // remote device has used JSON encoding
                        }

                        // HANDLE COM EXCEPTIONS THROWN BY WINDOWS BASED DRIVERS RUNNING IN THE REMOTE DEVICE
                        if (restResponseBase.DriverException != null)
                        {
                            TL.LogMessageCrLf(clientNumber, method, string.Format("Exception Message: \"{0}\", Exception Number: 0x{1}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber.ToString("X8")));
                            throw restResponseBase.DriverException;
                        }

                        // HANDLE ERRORS REPORTED BY ALPACA DEVICES THAT USE THE ERROR NUMBER AND ERROR MESSAGE FIELDS
                        if ((restResponseBase.ErrorMessage != "") || (restResponseBase.ErrorNumber != 0))
                        {
                            TL.LogMessageCrLf(clientNumber, method, string.Format("Received an Alpaca error - ErrorMessage: \"{0}\", ErrorNumber: 0x{1}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber.ToString("X8")));

                            // Handle ASCOM Alpaca reserved error numbers between 0x400 and 0xFFF by translating these to the COM HResult error number range: 0x80040400 to 0x80040FFF and throwing the translated value as an exception
                            if ((restResponseBase.ErrorNumber >= (int)AlpacaErrors.AlpacaErrorCodeBase) & (restResponseBase.ErrorNumber <= (int)AlpacaErrors.AlpacaErrorCodeMax)) // This error is within the ASCOM Alpaca reserved error number range
                            {
                                // Calculate the equivalent COM HResult error number from the supplied Alpaca error number so that comparison can be made with the original ASCOM COM exception HResult numbers that Windows clients expect in their exceptions
                                int ascomCOMErrorNumber = restResponseBase.ErrorNumber + (int)ComErrorCodes.ComErrorNumberOffset;
                                TL.LogMessageCrLf(clientNumber, method, string.Format("Received Alpaca error code: {0} (0x{0:X4}), the equivalent COM error HResult error code is {1} (0x{1:X8})", restResponseBase.ErrorNumber, ascomCOMErrorNumber));

                                // Now check whether the COM HResult matches any of the built-in ASCOM exception types. If so, we throw that exception type otherwise we throw a generic DriverException
                                if (ascomCOMErrorNumber == ErrorCodes.ActionNotImplementedException) // Handle ActionNotImplementedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca action not implemented error, throwing ActionNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ActionNotImplementedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidOperationException) // Handle InvalidOperationException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid operation error, throwing InvalidOperationException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new InvalidOperationException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidValue) // Handle InvalidValueException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid value error, throwing InvalidValueException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new InvalidValueException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidWhileParked) // Handle ParkedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca invalid while parked error, throwing ParkedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ParkedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.InvalidWhileSlaved) // Handle SlavedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca invalid while slaved error, throwing SlavedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new SlavedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.NotConnected) // Handle NotConnectedException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca not connected error, throwing NotConnectedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new NotConnectedException(restResponseBase.ErrorMessage);
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.NotImplemented) // Handle PropertyNotImplementedException and MethodNotImplementedException (both have the same error code)
                                {
                                    // Throw the relevant exception depending on whether this is a property or a method
                                    if (memberType == MemberTypes.Property) // Calling member is a property so throw a PropertyNotImplementedException
                                    {
                                        TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca property not implemented error, throwing PropertyNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                        throw new PropertyNotImplementedException(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(method), httpMethod == Method.PUT, restResponseBase.ErrorMessage);
                                    }
                                    else // Calling member is a method so throw a MethodNotImplementedException
                                    {
                                        TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca method not implemented error, throwing MethodNotImplementedException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                        throw new MethodNotImplementedException(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(method), restResponseBase.ErrorMessage);
                                    }
                                }
                                else if (ascomCOMErrorNumber == ErrorCodes.ValueNotSet) // Handle ValueNotSetException
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format(" Alpaca value not set error, throwing ValueNotSetException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new ValueNotSetException(restResponseBase.ErrorMessage);
                                }
                                else // The exception is inside the ASCOM Alpaca reserved range but is not one of those with their own specific exception types above, so wrap it in a DriverException and throw this to the client
                                {
                                    TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca undefined ASCOM error, throwing DriverException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, ascomCOMErrorNumber));
                                    throw new DriverException(restResponseBase.ErrorMessage, ascomCOMErrorNumber);
                                }
                            }
                            else // An exception has been thrown with an error number outside the ASCOM Alpaca reserved range, so wrap it in a DriverException and throw this to the client.
                            {
                                TL.LogMessageCrLf(clientNumber, method, string.Format("Alpaca error outside ASCOM reserved range, throwing DriverException - ErrorMessage: \"{0}\", ErrorNumber: 0x{1:X8}", restResponseBase.ErrorMessage, restResponseBase.ErrorNumber));
                                throw new DriverException(restResponseBase.ErrorMessage, restResponseBase.ErrorNumber);
                            }
                        }

                        // Internal error if an unsupported type is requested - should only occur during development and not in production operation!
                        throw new InvalidOperationException("Type " + typeof(T).ToString() + " is not supported. You should never see this message, if you do, please report it on the ASCOM Talk Forum!");
                    }
                    else
                    {
                        if (deviceJsonResponse.ErrorException != null)
                        {
                            TL.LogMessageCrLf(clientNumber, method, "RestClient exception: " + deviceJsonResponse.ErrorMessage + "\r\n " + deviceJsonResponse.ErrorException.ToString());
                            throw new DriverException(string.Format("Communications exception: {0} - {1}", deviceJsonResponse.ErrorMessage, deviceJsonResponse.ResponseStatus), deviceJsonResponse.ErrorException);
                        }
                        else
                        {
                            TL.LogMessage(clientNumber, method + " Error", string.Format("RestRequest response status: {0}, HTTP response code: {1}, HTTP response description: {2}", deviceJsonResponse.ResponseStatus.ToString(), deviceJsonResponse.StatusCode, deviceJsonResponse.StatusDescription));
                            throw new DriverException(string.Format("Error calling method: {0}, HTTP Completion Status: {1}, Error Message:\r\n{2}", method, deviceJsonResponse.ResponseStatus, deviceJsonResponse.Content));
                        }
                    }
                }
                catch (Exception ex) // Process unexpected exceptions
                {
                    if (ex is System.Net.WebException) // Received a WebException, this could indicate that the remote device actively refused the connection so test for this and retry if appropriate
                    {
                        if (ex.InnerException != null) // First make sure the is an inner exception
                        {
                            if (ex.InnerException is System.Net.Sockets.SocketException) // There is an inner exception and it is a SocketException so apply the retry logic
                            {
                                retryCounter += 1; // Increment the retry counter
                                if (retryCounter <= AlpacaConstants.SOCKET_ERROR_MAXIMUM_RETRIES) // The retry count is less than or equal to the maximum allowed so retry the command
                                {
                                    TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                    if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "SocketException: " + ex.ToString());

                                    // Log that we are retrying the command and wait a short time in the hope that the transient condition clears
                                    TL.LogMessage(clientNumber, method, string.Format("Socket exception, retrying command - retry-count {0}/{1}", retryCounter, AlpacaConstants.SOCKET_ERROR_MAXIMUM_RETRIES));
                                    Thread.Sleep(AlpacaConstants.SOCKET_ERROR_RETRY_DELAY_TIME);
                                }
                                else // The retry count exceeds the maximum allowed so throw the exception to the client
                                {
                                    TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                    if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "SocketException: " + ex.ToString());
                                    throw;
                                }

                            }
                            else  // There is an inner exception but it is not a SocketException so log it and throw it  to the client
                            {
                                TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                                if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "WebException: " + ex.ToString());
                                throw;
                            }

                        }
                    }
                    else // Some other type of exception that isn't System.Net.WebException so log it and throw it to the client
                    {
                        TL.LogMessageCrLf(clientNumber, method, typeof(T).Name + " " + ex.Message);
                        if (TL.DebugTraceState) TL.LogMessageCrLf(clientNumber, method, "Exception: " + ex.ToString());
                        throw;
                    }
                }
            } while (true); // Execution will only reach here if a communications retry is required, all other conditions are handled by return statements or by throwing exceptions

            // Execution will never reach this point
        }

        /// <summary>
        /// Test whether an error occurred in the driver
        /// </summary>
        /// <param name="response">The driver's response </param>
        /// <returns>True if the call was successful otherwise returns false.</returns>
        private static bool CallWasSuccessful(TraceLogger TL, RestResponseBase response)
        {
            if (response is null)
            {
                TL.LogMessage("CallWasNotSuccessful", "No response from device - Returning False");
                TL.BlankLine();
                return false; // No response so return false
            }

            TL.LogMessage("CallWasSuccessful", string.Format("DriverException == null: {0}, ErrorMessage: {1}, ErrorNumber: 0x{2}", response.DriverException == null, response.ErrorMessage, response.ErrorNumber.ToString("X8")));
            if (response.DriverException != null) TL.LogMessage("CallWasSuccessfulEx", response.DriverException.ToString());
            if ((response.DriverException == null) & (response.ErrorMessage == "") & (response.ErrorNumber == 0))
            {
                TL.LogMessage("CallWasSuccessful", "Returning True");
                TL.BlankLine();
                return true; // All was OK so return true
            }
            else
            {
                TL.LogMessage("CallWasNotSuccessful", "Returning False");
                TL.BlankLine();
                return false; // Some sort of issue so return false
            }
        }

        #endregion

        #region ASCOM Common members

        public static string Action(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string actionName, string actionParameters)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.ACTION_COMMAND_PARAMETER_NAME, actionName },
                { AlpacaConstants.ACTION_PARAMETERS_PARAMETER_NAME, actionParameters }
            };
            string remoteString = SendToRemoteDevice<string>(clientNumber, client, URIBase, TL, "Action", Parameters, Method.PUT, MemberTypes.Method);

            TL.LogMessage(clientNumber, "Action", $"Response: {(remoteString.Length < 1000 ? remoteString : remoteString.Substring(0, 1000))}");
            return remoteString;
        }

        public static void CommandBlind(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.COMMAND_PARAMETER_NAME, command },
                { AlpacaConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            SendToRemoteDevice<NoReturnValue>(clientNumber, client, URIBase, TL, "CommandBlind", Parameters, Method.PUT, MemberTypes.Method);
            TL.LogMessage(clientNumber, "CommandBlind", "Completed OK");
        }

        public static bool CommandBool(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.COMMAND_PARAMETER_NAME, command },
                { AlpacaConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            bool remoteBool = SendToRemoteDevice<bool>(clientNumber, client, URIBase, TL, "CommandBool", Parameters, Method.PUT, MemberTypes.Method);

            TL.LogMessage(clientNumber, "CommandBool", remoteBool.ToString());
            return remoteBool;
        }

        public static string CommandString(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, string command, bool raw)
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { AlpacaConstants.COMMAND_PARAMETER_NAME, command },
                { AlpacaConstants.RAW_PARAMETER_NAME, raw.ToString() }
            };
            string remoteString = SendToRemoteDevice<string>(clientNumber, client, URIBase, TL, "CommandString", Parameters, Method.PUT, MemberTypes.Method);

            TL.LogMessage(clientNumber, "CommandString", remoteString);
            return remoteString;
        }

        public static void Connect(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            if (TL.DebugTraceState) TL.LogMessage(clientNumber, "Connect", "Acquiring connection lock");
            lock (connectLockObject) // Ensure that only one connection attempt can happen at a time
            {
                TL.LogMessage(clientNumber, "Connect", "Has connection lock");
                if (IsClientConnected(clientNumber, TL)) // If we are already connected then just log this 
                {
                    TL.LogMessage(clientNumber, "Connect", "Already connected, just incrementing connection count.");
                }
                else // We are not connected so connect now
                {
                    try
                    {
                        TL.LogMessage(clientNumber, "Connect", "This is the first connection so set Connected to True");
                        SetBool(clientNumber, client, URIBase, TL, "Connected", true, MemberTypes.Property);
                        bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true);
                        TL.LogMessage(clientNumber, "Connect", "Successfully connected, AlreadyConnected: " + (!notAlreadyPresent).ToString() + ", number of connections: " + connectStates.Count);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf(clientNumber, "Connect", "Exception: " + ex.ToString());
                        throw;
                    }
                }
            }
        }

        public static string Description(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            return GetValue<string>(clientNumber, client, URIBase, TL, "Description", MemberTypes.Property);
        }

        public static void Disconnect(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {

            if (IsClientConnected(clientNumber, TL)) // If we are already connected then disconnect, otherwise ignore disconnect 
            {
                TL.LogMessage(clientNumber, "Disconnect", "We are connected, setting Connected to False on remote driver");
                SetBool(clientNumber, client, URIBase, TL, "Connected", false, MemberTypes.Property);
                bool successfullyRemoved = connectStates.TryRemove(clientNumber, out bool lastValue);
                TL.LogMessage("Disconnect", $"Set Connected to: False, Successfully removed: {successfullyRemoved}, previous value: {lastValue}");
            }
            else
            {
                TL.LogMessage("Disconnect", "Already disconnected, not sending command to driver");
            }
        }

        public static string DriverInfo(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            return GetValue<string>(clientNumber, client, URIBase, TL, "DriverInfo", MemberTypes.Property);
        }

        public static string DriverVersion(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            string remoteString = GetValue<string>(clientNumber, client, URIBase, TL, "DriverVersion", MemberTypes.Property);
            TL.LogMessage(clientNumber, "DriverVersion", remoteString);
            return remoteString;
        }

        public static short InterfaceVersion(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            short interfaceVersion = GetValue<short>(clientNumber, client, URIBase, TL, "InterfaceVersion", MemberTypes.Property);
            TL.LogMessage(clientNumber, "InterfaceVersion", interfaceVersion.ToString());
            return interfaceVersion;
        }

        public static ArrayList SupportedActions(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL)
        {
            List<string> supportedActions = GetValue<List<string>>(clientNumber, client, URIBase, TL, "SupportedActions", MemberTypes.Property);
            TL.LogMessage(clientNumber, "SupportedActions", string.Format("Returning {0} actions", supportedActions.Count));

            ArrayList returnValues = new ArrayList();
            foreach (string action in supportedActions)
            {
                returnValues.Add(action);
                TL.LogMessage(clientNumber, "SupportedActions", string.Format("Returning action: {0}", action));
            }

            return returnValues;
        }

        #endregion

        #region Complex Camera Properties

        public static int GetIntParameter(string parameterName, string response, TraceLoggerPlus TL)
        {
            const string COMMA = ",";

            string formattedParameterName = $"\"{parameterName}\":";

            int parameterTextStart = response.IndexOf(formattedParameterName, StringComparison.Ordinal);
            int parameterStart = parameterTextStart + formattedParameterName.Length;
            int parameterEnd = response.IndexOf(COMMA, parameterStart, StringComparison.Ordinal);
            string parameterString = response.Substring(parameterStart, parameterEnd - parameterStart);

            bool success = int.TryParse(parameterString, out int parameterValue);

            TL.LogMessage("GetIntParameter", $"ParameterTextStart: {parameterTextStart}, ParameterStart: {parameterStart}, PArameterEnd: {parameterEnd}, ParameterString: {parameterString}, ParameterValue: {parameterValue}, Success: {success}");

            return parameterValue;
        }

        public static object ImageArrayVariant(uint clientNumber, RestClient client, string URIBase, TraceLoggerPlus TL, ASCOM.Common.Alpaca.ImageArrayTransferType imageArrayTransferType, ASCOM.Common.Alpaca.ImageArrayCompression imageArrayCompression)
        {
            Array returnArray;
            object[,] objectArray2D;
            object[,,] objectArray3D;
            Stopwatch sw = new Stopwatch();

            returnArray = GetValue<Array>(clientNumber, client, URIBase, TL, "ImageArrayVariant", imageArrayTransferType, imageArrayCompression, MemberTypes.Property);

            try
            {
                string variantType = returnArray.GetType().Name;
                string elementType;
                switch (returnArray.Rank) // Process 2D and 3D variant arrays, all other types are unsupported
                {
                    case 2: // 2D Array
                        elementType = returnArray.GetValue(0, 0).GetType().Name;
                        break;
                    case 3: // 3D array
                        elementType = returnArray.GetValue(0, 0, 0).GetType().Name;
                        break;
                    default:
                        throw new InvalidValueException("ReturnImageArray: Received an unsupported return array rank: " + returnArray.Rank);
                }

                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Received {variantType} array of Rank {returnArray.Rank} with Length {returnArray.Length} and element type {elementType}");

                // convert to variant

                switch (returnArray.Rank)
                {
                    case 2:
                        objectArray2D = new object[returnArray.GetLength(0), returnArray.GetLength(1)];
                        switch (variantType)
                        {
                            case "Byte[,]":
                                Byte[,] byte2DArray = (Byte[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = byte2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Byte[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Int16[,]":
                                Int16[,] short2DArray = (Int16[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = short2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int16[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "UInt16[,]":
                                UInt16[,] uint16Array2D = (UInt16[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = uint16Array2D[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt16[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Int32[,]":
                                Int32[,] int2DArray = (Int32[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = int2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int32[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "UInt32[,]":
                                UInt32[,] uint2DArray = (UInt32[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = uint2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt32[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Int64[,]":
                                Int64[,] int64Array2D = (Int64[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = int64Array2D[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int64[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "UInt64[,]":
                                UInt64[,] uint64Array2D = (UInt64[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = uint64Array2D[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt64[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Single[,]":
                                Single[,] single2DArray = (Single[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = single2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Single[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Double[,]":
                                Double[,] double2DArray = (Double[,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        objectArray2D[i, j] = double2DArray[i, j];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Double[,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray2D;

                            case "Object[,]":
                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Received an Object[,] array, returning it directly to the client without further processing.");
                                return returnArray;

                            default:
                                throw new InvalidValueException("DynamicRemoteClient Driver Camera.ImageArrayVariant: Unsupported return array rank from DynamicClientDriver.GetValue<Array>: " + returnArray.Rank);
                        }
                    case 3:
                        objectArray3D = new object[returnArray.GetLength(0), returnArray.GetLength(1), returnArray.GetLength(2)];
                        switch (variantType)
                        {
                            case "Byte[,,]":
                                Byte[,,] byte3DArray = (Byte[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                            objectArray3D[i, j, k] = byte3DArray[i, j, k];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Byte[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Int16[,,]":
                                Int16[,,] short3DArray = (Int16[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                            objectArray3D[i, j, k] = short3DArray[i, j, k];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int16[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "UInt16[,,]":
                                UInt16[,,] uint16Array3D = (UInt16[,,])returnArray;
 
                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                            objectArray3D[i, j, k] = uint16Array3D[i, j, k];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt16[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Int32[,,]":
                                Int32[,,] int3DArray = (Int32[,,])returnArray;
   
                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                        {
                                            objectArray3D[i, j, k] = int3DArray[i, j, k];
                                        }
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int32[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "UInt32[,,]":
                                UInt32[,,] uint32Array3D = (UInt32[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                        {
                                            objectArray3D[i, j, k] = uint32Array3D[i, j, k];
                                        }
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt32[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Int64[,,]":
                                Int64[,,] int64Array3D = (Int64[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                        {
                                            objectArray3D[i, j, k] = int64Array3D[i, j, k];
                                        }
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Int64[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "UInt64[,,]":
                                UInt64[,,] uint64Array3D = (UInt64[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                        {
                                            objectArray3D[i, j, k] = uint64Array3D[i, j, k];
                                        }
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying UInt64[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Single[,,]":
                                Single[,,] single3DDArray = (Single[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                            objectArray3D[i, j, k] = single3DDArray[i, j, k];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Single[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Double[,,]":
                                Double[,,] double3DDArray = (Double[,,])returnArray;

                                sw.Restart();
                                Parallel.For(0, returnArray.GetLength(0), (i) =>
                                {
                                    for (int j = 0; j < returnArray.GetLength(1); j++)
                                    {
                                        for (int k = 0; k < returnArray.GetLength(2); k++)
                                            objectArray3D[i, j, k] = double3DDArray[i, j, k];
                                    }
                                });

                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Finished copying Double[,,] array in {sw.ElapsedMilliseconds}ms.");
                                return objectArray3D;

                            case "Object[,,]":
                                TL.LogMessage(clientNumber, "ImageArrayVariant", $"Received an Object[,,] array, returning it directly to the client without further processing.");
                                return returnArray;

                            default:
                                throw new InvalidValueException("DynamicRemoteClient Driver Camera.ImageArrayVariant: Unsupported return array rank from DynamicClientDriver.GetValue<Array>: " + returnArray.Rank);
                        }

                    default:
                        throw new InvalidValueException("DynamicRemoteClient Driver Camera.ImageArrayVariant: Unsupported return array rank from DynamicClientDriver.GetValue<Array>: " + returnArray.Rank);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(clientNumber, "ImageArrayVariant", $"Exception: \r\n{ex}");
                throw;
            }
        }

        #endregion

        #region Support code

        /// <summary>
        /// Use an HttpClient to retrieve the image array byte data
        /// </summary>
        /// <param name="url">URL from which to retireve data</param>
        /// <param name="acceptString">The Acdept string og mim types that we are prepared to accept.</param>
        /// <param name="TL">TraceLogger for logging purposes.</param>
        /// <returns>A populated RestSharp RestResponse</returns>
        /// <remarks>This approach is used because of inexplicable delays that occured when using the RestSharp client to retieve large binary byte arrays.</remarks>
        private static IRestResponse GetResponse(string url, string acceptString, uint clientId, uint clientTransactionId, TraceLogger TL)
        {
            HttpClient wClient = new HttpClient();
            IEnumerable<string> contentTypeValues;
            string contentType = string.Empty;
            Stopwatch sw = new Stopwatch();
            Stopwatch swOverall = new Stopwatch();

            //Add an ACCEPT header
            wClient.DefaultRequestHeaders.Accept.ParseAdd(acceptString);

            // Add the ClientID and ClientTransactionID parameters
            UriBuilder builder = new UriBuilder(url)
            {
                Query = $"{AlpacaConstants.CLIENTID_PARAMETER_NAME}={clientId}&{AlpacaConstants.CLIENTTRANSACTION_PARAMETER_NAME}={clientTransactionId}"
            };

            sw.Start();
            swOverall.Start();

            // Get the data from the Alpaca device
            // using (HttpResponseMessage response = wClient.GetAsync(url, HttpCompletionOption.ResponseContentRead).Result)
            TL.LogMessage("GetResponse", $"Base address: '{wClient.BaseAddress}', relative URI: '{builder.Uri}'.");

            using (HttpResponseMessage response = wClient.GetAsync(builder.Uri, HttpCompletionOption.ResponseContentRead).Result)
            {
                TL.LogMessage("GetResponse", $"GetAsync time: {sw.ElapsedMilliseconds}ms, Overall time: {swOverall.ElapsedMilliseconds}ms.");

                // Get the response CONTENT headers (different to response TRANSPORT headers, hard won knowledge!)
                sw.Restart();
                HttpContentHeaders headers = response.Content.Headers;

                if (headers is null) throw new InvalidValueException("The device did not return any headers. Expected a Content-Type header with a value of 'application/json' or 'text/json' or 'application/imagebytes'.");

                // Extract the content type from tyhe headers
                if (headers.TryGetValues(AlpacaConstants.CONTENT_TYPE_HEADER_NAME, out contentTypeValues))
                {
                    contentType = contentTypeValues.First().ToLowerInvariant();
                }

                // Get the returned data as a byte[] (could be JSON or ImageBytes image data)
                sw.Restart();
                byte[] rawbytes = response.Content.ReadAsByteArrayAsync().Result;
                TL.LogMessage("GetResponse", $"ReadAsByteArrayAsync time: {sw.ElapsedMilliseconds}ms, Overall time: {swOverall.ElapsedMilliseconds}ms.");

                // If the content type is JSON - Populate the Content property with a string converted from the byte[] 
                if ((contentType.Contains(AlpacaConstants.APPLICATION_JSON_MIME_TYPE)) | (contentType.Contains(AlpacaConstants.TEXT_JSON_MIME_TYPE)))
                {
                    sw.Restart();
                    RestResponse restResponse = new RestResponse()
                    {
                        Content = Encoding.UTF8.GetString(rawbytes),
                        ContentType = contentType,
                        ResponseStatus = ResponseStatus.Completed,
                        StatusCode = response.StatusCode,
                        StatusDescription = response.ReasonPhrase
                    };
                    TL.LogMessage("GetResponse", $"GetStringAsync time: {sw.ElapsedMilliseconds}ms, Overall time: {swOverall.ElapsedMilliseconds}ms.");

                    return restResponse;
                }

                // If the content type is ImageBytes - ASsign the byte[] to the rawBytes property 
                else if (contentType.ToLowerInvariant().Contains(AlpacaConstants.IMAGE_BYTES_MIME_TYPE))
                {
                    sw.Restart();
                    RestResponse restResponse = new RestResponse
                    {
                        RawBytes = rawbytes,
                        ContentType = contentType,
                        ResponseStatus = ResponseStatus.Completed,
                        StatusCode = response.StatusCode,
                        StatusDescription = response.ReasonPhrase
                    };
                    TL.LogMessage("GetResponse", $"GetByteArrayAsync time: {sw.ElapsedMilliseconds}ms, Overall time: {swOverall.ElapsedMilliseconds}ms.");

                    return restResponse;
                }

                // Otherwise we didn't receive a content type header or received an unsupported content type, so throw an exception to indicate the problem.
                else
                {
                    TL.LogMessage("GetResponse", $"Did not find expected content type of 'application.json' or 'application/imagebytes'. Found: {contentType}");
                    throw new InvalidValueException($"The device did not return a content type or returned an unsupported content type: '{contentType}'");
                }
            }
        }
        #endregion

    }
}
