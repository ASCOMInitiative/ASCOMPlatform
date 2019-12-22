using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace ASCOM.Utilities.Support
{
    /// <summary>
    /// Alpaca discovery component
    /// Note: Forced the use of System.Net.Http.HttpClientHandler instead of System.Net.Http.SocketsHttpHandler because HttpClient timeout doesn't work correctly in the latter
    /// </summary>
    public class AlpacaDiscovery : IDisposable
    {
        // Utility objects
        private ITraceLoggerUtility TL;
        private Finder finder;
        private Timer discoveryCompleteTimer;

        // Private variables
        private Dictionary<IPEndPoint, AlpacaDevice> alpacaDeviceList = new Dictionary<IPEndPoint, AlpacaDevice>(); // List of discovered Alpaca devices keyed on IP:Port
        private bool disposedValue = false; // To detect redundant Dispose() method calls
        private double discoveryTime; // Length of the discovery phase before it times out
        private bool tryDnsNameResolution; // Flag indicating whether to attempt name resolution on the host IP address
        private DateTime discoveryStartTime; // Time at which the start discovery command was received
        private readonly object deviceListLockObject = new object(); // Lock object to synchronise access to the Alpaca device list collection, which is not a thread safe collection

        #region New and IDisposable Support

        /// <summary>
        /// Initialiser that takes a trace logger
        /// </summary>
        /// <param name="tl">Trace logger instance to use for activity logging</param>
        public AlpacaDiscovery(ITraceLoggerUtility tl)
        {
            try
            {
                // Save the supplied trace logger object
                TL = tl;

                // Initialise variables
                tryDnsNameResolution = false; // Initialise so that there is no host name resolution by default
                DiscoveryComplete = true; // Initialise so that discoveries can be run

                // Initialise utility objects
                discoveryCompleteTimer = new System.Threading.Timer(OnDiscoveryCompleteTimer);

                if (finder != null)
                {
                    finder.Dispose();
                    finder = null;
                }
                finder = new Finder(BroadcastResponseEventHandler, TL); // Get a new broadcast response finder
                LogMessage("AlpacaDiscoveryInitialise", $"Complete - Running on thread {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception ex)
            {
                LogMessage("AlpacaDiscoveryInitialise", $"Exception{ex.ToString()}");
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (finder != null) finder.Dispose();
                    if (discoveryCompleteTimer != null) discoveryCompleteTimer.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put clean-up code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

        #region Public events and methods

        /// <summary>
        /// Raised every time information about discovered devices is updated
        /// </summary>
        public event EventHandler AlpacaDevicesUpdated;

        /// <summary>
        /// Raised when the discovery is complete
        /// </summary>
        public event EventHandler DiscoveryCompleted;

        /// <summary>
        /// Returns a list of discovered Alpaca devices
        /// </summary>
        /// <returns>List of AlpacaDevice classes</returns>
        public List<AlpacaDevice> GetAlpacaDevices()
        {
            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while copying it to the list
            {
                return alpacaDeviceList.Values.ToList(); // Create a copy of the dynamically changing alpacaDeviceList ConcurrentDictionary of discovered devices
            }
        }

        /// <summary>
        /// Returns a list of discovered ASCOM devices of the specified device type for Chooser-like functionality
        /// </summary>
        /// <returns>List of AscomDevice classes</returns>
        public List<AscomDevice> GetAscomDevices(string deviceType)
        {
            return GetAscomDevices().Where(ascomDevice => ascomDevice.AscomDeviceType.ToLowerInvariant() == deviceType.ToLowerInvariant()).ToList();
        }

        /// <summary>
        /// Returns a list of all discovered ASCOM devices for Chooser-like functionality
        /// </summary>
        /// <returns>List of AscomDevice classes</returns>
        public List<AscomDevice> GetAscomDevices()
        {
            List<AscomDevice> ascomDeviceList = new List<AscomDevice>(); // List of discovered ASCOM devices to support Chooser-like functionality

            // Iterate over the discovered Alpaca devices
            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while copying it to the list
            {
                foreach (KeyValuePair<IPEndPoint, AlpacaDevice> alpacaDevice in alpacaDeviceList)
                {
                    // Iterate over each Alpaca interface version that the Alpaca device supports
                    foreach (int alpacaDeviceInterfaceVersion in alpacaDevice.Value.SupportedInterfaceVersions)
                    {
                        // Iterate over the ASCOM devices presented by this Alpaca device adding them to the return dictionary
                        foreach (ConfiguredDevice ascomDevice in alpacaDevice.Value.ConfiguredDevices)
                        {
                            ascomDeviceList.Add(new AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, // ASCOM device information 
                                                   alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)); // Alpaca device information
                        }
                    }
                }
            }
            return ascomDeviceList; // Return the list of ASCOM devices
        }

        /// <summary>
        /// Returns discovery completion status
        /// </summary>
        public bool DiscoveryComplete { get; private set; }

        /// <summary>
        /// Start an Alpaca device discovery
        /// </summary>
        /// <param name="numberOfPolls">Number of polls to send in the range 1 to 5</param>
        /// <param name="pollInterval">Interval between each poll in the range 10 to 5000 milliseconds</param>
        /// <param name="discoveryPort">Discovery port on which to send the broadcast (normally 32227)</param>
        /// <param name="discoveryDuration">Length of time to wait for devices to respond</param>
        /// <param name="resolveDnsName">Attempt to resolve host IP addresses to DNS names</param>
        public void StartDiscovery(int numberOfPolls, int pollInterval, int discoveryPort, double discoveryDuration, bool resolveDnsName)
        {
            // Validate parameters
            if ((numberOfPolls < 1) || (numberOfPolls > 5)) throw new ArgumentException($"StartDiscovery - NumberOfPolls: {numberOfPolls} is not within the valid range of 1::5");
            if ((pollInterval < 10) || (pollInterval > 5000)) throw new ArgumentException($"StartDiscovery - PollInterval: {numberOfPolls} is not within the valid range of 10::5000");
            if (!DiscoveryComplete) throw new InvalidOperationException("Cannot start a new discovery because a previous discovery is still running.");

            // Save supplied parameters for use within the application 
            discoveryTime = discoveryDuration;
            tryDnsNameResolution = resolveDnsName;

            // Prepare for a new search
            LogMessage("StartDiscovery", $"Starting search for Alpaca devices with timeout: {discoveryTime} Broadcast polls: {numberOfPolls} sent every {pollInterval} milliseconds");

            finder.ClearCache();

            // Clear the device list dictionary
            lock (deviceListLockObject) // Make sure that the clear operation is not interrupted by other threads
            {
                alpacaDeviceList.Clear();
            }

            discoveryCompleteTimer.Change(Convert.ToInt32(discoveryTime * 1000), Timeout.Infinite);
            DiscoveryComplete = false;
            discoveryStartTime = DateTime.Now; // Save the start time

            // Send the broadcast polls
            for (int i = 1; i <= numberOfPolls; i++)
            {
                LogMessage("StartDiscovery", $"Sending poll {i}...");
                finder.Search(discoveryPort);
                LogMessage("StartDiscovery", $"Poll {i} sent.");
                if (i < numberOfPolls) Thread.Sleep(pollInterval); // Sleep after sending the poll, except for the last one
            }

            LogMessage("StartDiscovery", "Alpaca device broadcast polls completed, discovery started");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Raise an Alpaca devices updated event
        /// </summary>
        /// <param name="e"></param>
        private void RaiseAnAlpacaDevicesChangedEvent()
        {
            AlpacaDevicesUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Discovery timer event handler - called when the allocated discovery period has ended
        /// </summary>
        /// <param name="state">Timer state</param>
        private void OnDiscoveryCompleteTimer(object state)
        {
            LogMessage("OnTimeOutTimerFired", $"Firing discovery complete event");

            DiscoveryComplete = true; // Flag that the timer out has expired

            bool statusMessagesUpdated = false;

            // Update the status messages of management API calls that didn't connect in time
            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
            {
                foreach (KeyValuePair<IPEndPoint, AlpacaDevice> alpacaDevice in alpacaDeviceList)
                {
                    if (alpacaDevice.Value.StatusMessage == Constants.TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE)
                    {
                        alpacaDevice.Value.StatusMessage = Constants.FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE;
                        statusMessagesUpdated = true;
                    }
                }
            }
            if (statusMessagesUpdated) RaiseAnAlpacaDevicesChangedEvent(); // Raise a devices changed event if any status messages have been updated

            DiscoveryCompleted?.Invoke(this, EventArgs.Empty); // Raise an event to indicate that discovery is complete
        }

        /// <summary>
        /// Handler for device responses coming from the Finder
        /// </summary>
        /// <param name="responderIPEndPoint">Responder's IP address and port</param>
        /// <param name="alpacaDiscoveryResponse">Class containing the information provided by the device in its response.</param>
        private void BroadcastResponseEventHandler(IPEndPoint responderIPEndPoint, AlpacaDiscoveryResponse alpacaDiscoveryResponse)
        {
            try
            {
                LogMessage("BroadcastResponseEventHandler", $"FOUND Alpaca device at {responderIPEndPoint.Address}:{responderIPEndPoint.Port}"); // Log reception of the broadcast response

                // Add the new device or ignore this duplicate if it already exists
                lock (deviceListLockObject) // Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
                {
                    if (!alpacaDeviceList.ContainsKey(responderIPEndPoint))
                    {
                        alpacaDeviceList.Add(responderIPEndPoint, new AlpacaDevice(responderIPEndPoint, Constants.TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE));
                        RaiseAnAlpacaDevicesChangedEvent(); // Device was added so set the changed flag
                    }
                }

                // Create a task to query this device's DNS name, if configured to do so
                if (tryDnsNameResolution)
                {
                    LogMessage("BroadcastResponseEventHandler", $"Creating task to retrieve DNS information for device {responderIPEndPoint.ToString()}:{responderIPEndPoint.Port}");
                    Thread dnsResolutionThread = new Thread(ResolveIpAddressToHostName);
                    dnsResolutionThread.IsBackground = true;
                    dnsResolutionThread.Start(responderIPEndPoint);

                }

                // Create a task to query this device's Alpaca management API
                LogMessage("BroadcastResponseEventHandler", $"Creating thread to retrieve Alpaca management description for device {responderIPEndPoint.ToString()}:{responderIPEndPoint.Port}");

                Thread descriptionThread = new Thread(GetAlpacaDeviceInformation);
                descriptionThread.IsBackground = true;
                descriptionThread.Start(responderIPEndPoint);

            }
            catch (Exception ex)
            {
                LogMessage("BroadcastResponseEventHandler", $"AddresssFound Exception: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Get Alpaca device information from the management API
        /// </summary>
        /// <param name="deviceIpEndPoint"></param>
        private void GetAlpacaDeviceInformation(object deviceIpEndPointObject)
        {
            IPEndPoint deviceIpEndPoint = deviceIpEndPointObject as IPEndPoint;
            string hostIpAndPort = deviceIpEndPoint.ToString();

            try
            {
                // Create a combined host and port string

                // Wait for API version result and process it
                using (WebClient apiClient = new WebClient())
                {
                    string apiVersionsJsonResponse = GetRequest($"http://{hostIpAndPort}/management/apiversions", Convert.ToInt32(discoveryTime * 1000));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {apiVersionsJsonResponse}");
                    IntArray1DResponse apiVersionsResponse = JsonConvert.DeserializeObject<IntArray1DResponse>(apiVersionsJsonResponse);

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].SupportedInterfaceVersions = apiVersionsResponse.Value;
                        alpacaDeviceList[deviceIpEndPoint].StatusMessage = ""; // Clear the status field to indicate that this first call was successful
                    }
                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                // Wait for device description result and process it
                using (WebClient descriptionClient = new WebClient())
                {
                    string deviceDescriptionJsonResponse = GetRequest($"http://{hostIpAndPort}/management/v1/description", Convert.ToInt32(discoveryTime * 1000));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {deviceDescriptionJsonResponse}");
                    AlpacaDescriptionResponse deviceDescriptionResponse = JsonConvert.DeserializeObject<AlpacaDescriptionResponse>(deviceDescriptionJsonResponse);

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].AlpacaDeviceDescription = deviceDescriptionResponse.Value;
                    }
                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                // Wait for configured devices result and process it
                using (WebClient configuredDevicesClient = new WebClient())
                {
                    string configuredDevicesJsonResponse = GetRequest($"http://{hostIpAndPort}/management/v1/configureddevices", Convert.ToInt32(discoveryTime * 1000));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {configuredDevicesJsonResponse}");
                    AlpacaConfiguredDevicesResponse configuredDevicesResponse = JsonConvert.DeserializeObject<AlpacaConfiguredDevicesResponse>(configuredDevicesJsonResponse);

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].ConfiguredDevices = configuredDevicesResponse.Value;
                    }
                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                LogMessage("GetAlpacaDeviceInformation", $"COMPLETED API tasks for {hostIpAndPort}");
            }

            catch (Exception ex)
            {
                // Something went wrong so log the issue and sent a message to the user
                LogMessage("GetAlpacaDeviceInformation", $"GetAlpacaDescriptions exception: \r\n{ex.ToString()}");
                lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                {
                    alpacaDeviceList[deviceIpEndPoint].StatusMessage = ex.Message; RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }
            }
        }

        /// <summary>
        /// Resolve a host IP address to a host name
        /// </summary>
        /// <param name="hostIp"></param>
        /// <param name="HostPort"></param>
        /// <returns></returns>
        /// <remarks>This first makes a DNS query and uses the result if found. If not found it then tries a Microsoft DNS call which also searches the local hosts and makes a netbios query.
        /// If this returns an answer it is use. Otherwise the IP address is returned as the host name</remarks>
        private void ResolveIpAddressToHostName(object deviceIpEndPointObject)
        {
            IPEndPoint deviceIpEndPoint = deviceIpEndPointObject as IPEndPoint; // Get the supplied device endpoint as an IPEndPoint

            DnsResponse dnsResponse = new DnsResponse(); // Create a new DnsResponse to hold and return the 

            TimeSpan timeOutTime = TimeSpan.FromSeconds(discoveryTime).Subtract(DateTime.Now - discoveryStartTime).Subtract(TimeSpan.FromSeconds(0.2));

            LogMessage("ResolveIpAddressToHostName", $"Resolving IP address: {deviceIpEndPoint.Address.ToString()}, Timeout: {timeOutTime}");

            Dns.BeginGetHostEntry(deviceIpEndPoint.Address.ToString(), new AsyncCallback(GetHostEntryCallback), dnsResponse);

            // Wait here until the resolve completes and the callback calls .Set()
            bool dnsWasResolved = dnsResponse.CallComplete.WaitOne(timeOutTime); // Wait for the remaining discovery time less a small amount

            if (dnsWasResolved) // A response was received rather than timing out
            {
                LogMessage("ResolveIpAddressToHostName", $"{deviceIpEndPoint.ToString()} has host name: {dnsResponse.HostName} IP address count: {dnsResponse.AddressList.Length} Alias count: { dnsResponse.Aliases.Length}");

                if (dnsResponse.AddressList.Length > 0)
                {
                    lock (deviceListLockObject)
                    {
                        alpacaDeviceList[deviceIpEndPoint].HostName = dnsResponse.HostName;
                    }
                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }
                else
                {
                    LogMessage("ResolveIpAddressToHostName", $"***** DNS responded with a name ({dnsResponse.HostName}) but this has no associated IP addresses and is probably a NETBIOS name *****");
                }

                foreach (IPAddress address in dnsResponse.AddressList)
                {
                    LogMessage("ResolveIpAddressToHostName", $"Address: {address}");
                }
                foreach (string alias in dnsResponse.Aliases)
                {
                    LogMessage("ResolveIpAddressToHostName", $"Alias: {alias}");
                }

            }
            else
            {
                LogMessage("ResolveIpAddressToHostName", $"***** DNS did not respond within timeout - unable to resolve IP address to host name *****");
            }

        }

        /// <summary>
        /// Record the IPs in the state object for later use.
        /// </summary>
        private void GetHostEntryCallback(IAsyncResult ar)
        {
            try
            {
                DnsResponse dnsResponse = (DnsResponse)ar.AsyncState; // Turn the state object into the DnsResponse type
                dnsResponse.IpHostEntry = Dns.EndGetHostEntry(ar); // Save the returned IpHostEntry and populate other fields based on its parameters
                dnsResponse.CallComplete.Set(); // Set the wait handle so that the caller knows that the asynchronous call has completed and that the response has been updated
            }
            catch (Exception ex)
            {
                LogMessage("GetHostEntryCallback", $"Exception: {ex.ToString()}"); // Log exceptions but don't throw them
            }
        }

        /// <summary>
        /// Log a message to the screen, adding the current managed thread ID
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="message"></param>
        private void LogMessage(string methodName, string message)
        {
            string indentSpaces = new String(' ', Thread.CurrentThread.ManagedThreadId * Constants.NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES);
            TL?.LogMessageCrLf($"AlpacaDiscovery - {methodName}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} {message}");
        }

        private string GetRequest(string aURL, int timeOut)
        {
            using var webClient = new WebClient();
            webClient.Timeout = timeOut;

            string s = webClient.DownloadString(aURL);
            return s;
        }
        #endregion
    }
}