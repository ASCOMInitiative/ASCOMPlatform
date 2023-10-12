using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Script.Serialization;
using ASCOM.Utilities.Interfaces;
//using Newtonsoft.Json;

namespace ASCOM.Utilities
{

    /// <summary>
    /// Enables clients to discover Alpaca devices by sending one or more discovery polls. Returns information on discovered <see cref="AlpacaDevice">Alpaca devices</see> and the <see cref="AscomDevice">ASCOM devices</see> that are available.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The discovery process is asynchronous and is initiated by the <see cref="StartDiscovery(int, int, int, double, bool, bool, bool)"/> method. Clients can then either work synchronously by looping and periodically 
    /// polling the <see cref="DiscoveryComplete"/> property or work asynchronously by handling the <see cref="AlpacaDevicesUpdated"/> and <see cref="DiscoveryCompleted"/> events while doing other work.
    /// </para>
    /// <para>
    /// The <see cref="StartDiscovery(int, int, int, double, bool, bool, bool)"/> method is used to set the character of the discovery e.g. the discovery duration and whether to search for IPv4 and/or IPv6 devices. 
    /// After the specified discovery duration, the <see cref="DiscoveryComplete"/> event fires and the <see cref="DiscoveryCompleted"/> property returns True.
    /// </para>
    /// <para>
    /// Once discovery is complete, .NET clients can retrieve details of discovered Alpaca devices and associated ASCOM interface devices through the <see cref="GetAlpacaDevices"/> and <see cref="GetAscomDevices(string)"/> methods.
    /// COM clients must use the <see cref="GetAlpacaDevicesAsArrayList"/> and <see cref="GetAscomDevicesAsArrayList(string)"/> properties because COM does not support the generic classes used 
    /// in the <see cref="GetAlpacaDevices"/> and <see cref="GetAscomDevices(string)"/> methods. 
    /// </para>
    /// </remarks>
    [Guid("877A70E7-0A70-41EE-829A-8C00CAE2B9F0")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AlpacaDiscovery : IAlpacaDiscovery, IAlpacaDiscoveryExtra, IDisposable
    {

        #region Variables

        // Utility objects
        private TraceLogger TL;
        private Finder finder;
        private System.Threading.Timer discoveryCompleteTimer;

        // Private variables
        private Dictionary<IPEndPoint, AlpacaDevice> alpacaDeviceList = new Dictionary<IPEndPoint, AlpacaDevice>(); // List of discovered Alpaca devices keyed on IP:Port
        private bool disposedValue = false; // To detect redundant Dispose() method calls
        private double discoveryTime; // Length of the discovery phase before it times out
        private bool tryDnsNameResolution; // Flag indicating whether to attempt name resolution on the host IP address
        private DateTime discoveryStartTime; // Time at which the start discovery command was received
        private bool discoveryCompleteValue; // Discovery completion status
        private readonly object deviceListLockObject = new object(); // Lock object to synchronise access to the Alpaca device list collection, which is not a thread safe collection

        #endregion

        #region New and IDisposable Support

        /// <summary>
        /// Initialise the Alpaca discovery component
        /// </summary>
        public AlpacaDiscovery()
        {
            InitialiseClass(); // Initialise without a trace logger
        }

        /// <summary>
        /// Initialiser that takes a trace logger (Can only be used from .NET clients)
        /// </summary>
        /// <param name="traceLogger">Trace logger instance to use for activity logging</param>
        internal AlpacaDiscovery(TraceLogger traceLogger)
        {
            TL = traceLogger; // Save the supplied trace logger object
            InitialiseClass(); // Initialise using the trace logger
        }

        private void InitialiseClass()
        {
            try
            {
                // Initialise variables
                tryDnsNameResolution = false; // Initialise so that there is no host name resolution by default
                discoveryCompleteValue = true; // Initialise so that discoveries can be run

                // Initialise utility objects
                discoveryCompleteTimer = new System.Threading.Timer(OnDiscoveryCompleteTimer);

                if (finder is not null)
                {
                    finder.Dispose();
                    finder = null;
                }

                // Get a new broadcast response finder
                finder = new Finder(FoundDeviceEventHandler, TL);

                LogMessage("AlpacaDiscoveryInitialise", $"Complete - Running on thread {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception ex)
            {
                LogMessage("AlpacaDiscoveryInitialise", $"Exception{ex}");
            }

        }

        /// <summary>
        /// Dispose of the class
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                if (disposing)
                {
                    // The trace logger is not disposed here because it is supplied by the client, which is response for disposing of it as appropriate.

                    if (finder is not null)
                        finder.Dispose();

                    if (discoveryCompleteTimer is not null)
                        discoveryCompleteTimer.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes of the discovery component and cleans up resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put clean-up code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Raised every time information about discovered devices is updated
        /// </summary>
        /// <remarks>This event is only available to .NET clients, there is no equivalent for COM clients.</remarks>
        public event EventHandler AlpacaDevicesUpdated;

        /// <summary>
        /// Raised when the discovery is complete
        /// </summary>
        /// <remarks>This event is only available to .NET clients. COM clients should poll the <see cref="DiscoveryComplete"/> property periodically to determine when discovery is complete.</remarks>
        public event EventHandler DiscoveryCompleted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Flag that indicates when a discovery cycle is complete
        /// </summary>
        /// <returns>True when discovery is complete.</returns>
        /// <remarks>The discovery is considered complete when the time period specified on the <see cref="StartDiscovery(int, int, int, double, bool, bool, bool)"/> method is exceeded.</remarks>
        public bool DiscoveryComplete
        {
            get
            {
                return discoveryCompleteValue;
            }
            private set
            {
                discoveryCompleteValue = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an ArrayList of discovered Alpaca devices for use by COM clients
        /// </summary>
        /// <returns>ArrayList of <see cref="AlpacaDevice"/>classes</returns>
        /// <remarks>This method is for use by COM clients because it is not possible to pass a generic list as used in <see cref="GetAlpacaDevices"/> through a COM interface. 
        /// .NET clients should use <see cref="GetAlpacaDevices()"/> instead of this method.</remarks>
        public ArrayList GetAlpacaDevicesAsArrayList()
        {
            ArrayList alpacaDevicesAsArrayList; // Variable to hold the array-list analogue of the generic list of Alpaca devices

            alpacaDevicesAsArrayList = new ArrayList(); // Create a new array-list

            // populate the array-list with data from the generic list
            foreach (AlpacaDevice alpacaDevice in GetAlpacaDevices())
                alpacaDevicesAsArrayList.Add(alpacaDevice);

            return alpacaDevicesAsArrayList; // Return the Alpaca devices list as an ArrayList
        }

        /// <summary>
        /// Returns an ArrayList of discovered ASCOM devices, of the specified device type, for use by COM clients
        /// </summary>
        /// <param name="deviceType">The device type for which to search e.g. Telescope, Focuser. An empty string will return devices of all types.</param>
        /// <returns>ArrayList of <see cref="AscomDevice"/>classes</returns>
        /// <remarks>
        /// <para>
        /// This method is for use by COM clients because it is not possible to return a generic list, as used in <see cref="GetAscomDevices(String)"/>, through a COM interface. 
        /// .NET clients should use <see cref="GetAscomDevices(String)"/> instead of this method.
        /// </para>
        /// <para>
        /// This method will return every discovered device, regardless of device type, if the supplied "deviceType" parameter is an empty string.
        /// </para>
        /// </remarks>
        public ArrayList GetAscomDevicesAsArrayList(string deviceType)
        {
            return new ArrayList(GetAscomDevices(deviceType)); // Return the ASCOM devices list as an ArrayList
        }

        /// <summary>
        /// Returns a generic List of discovered Alpaca devices.
        /// </summary>
        /// <returns>List of <see cref="AlpacaDevice"/>classes</returns>
        /// <remarks>This method is only available to .NET clients because COM cannot handle generic types. COM clients should use <see cref="GetAlpacaDevicesAsArrayList()"/>.</remarks>
        public List<AlpacaDevice> GetAlpacaDevices()
        {
            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while copying it to the list
                return alpacaDeviceList.Values.ToList(); // Create a copy of the dynamically changing alpacaDeviceList ConcurrentDictionary of discovered devices
        }

        /// <summary>
        /// Returns a generic list of discovered ASCOM devices of the specified device type.
        /// </summary>
        /// <param name="deviceType">The device type for which to search e.g. Telescope, Focuser. An empty string will return devices of all types.</param>
        /// <returns>List of AscomDevice classes</returns>
        /// <remarks>
        /// <para>
        /// This method is only available to .NET clients because COM cannot handle generic types. COM clients should use <see cref="GetAlpacaDevicesAsArrayList()"/>.
        /// </para>
        /// <para>
        /// This method will return every discovered device, regardless of device type, if the supplied "deviceType" parameter is an empty string.
        /// </para>
        /// </remarks>
        public List<AscomDevice> GetAscomDevices(string deviceType)
        {
            var ascomDeviceList = new List<AscomDevice>(); // List of discovered ASCOM devices to support Chooser-like functionality

            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while processing this command
            {

                // Iterate over the discovered Alpaca devices
                foreach (KeyValuePair<IPEndPoint, AlpacaDevice> alpacaDevice in alpacaDeviceList)
                {

                    // Iterate over each Alpaca interface version that the Alpaca device supports
                    foreach (int alpacaDeviceInterfaceVersion in alpacaDevice.Value.SupportedInterfaceVersions)
                    {

                        // Iterate over the ASCOM devices presented by this Alpaca device adding them to the return dictionary
                        foreach (ConfiguredDevice ascomDevice in alpacaDevice.Value.ConfiguredDevices)
                        {

                            // Test whether all devices or only devices of a specific device type are required
                            if (string.IsNullOrEmpty(deviceType)) // Return a full list of every discovered device regardless of device type 
                            {
                                ascomDeviceList.Add(new AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)); // ASCOM device information 
                            }
                            else if ((ascomDevice.DeviceType.ToLowerInvariant() ?? "") == (deviceType.ToLowerInvariant() ?? "")) // Return only devices of the specified type
                            {
                                ascomDeviceList.Add(new AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)); // ASCOM device information 
                            }

                        } // Next Ascom Device
                    } // Next interface version
                } // Next Alpaca device

                // Return the information requested
                return ascomDeviceList; // Return the list of ASCOM devices

            }
        }

        /// <summary>
        /// Start an Alpaca device discovery based on the supplied parameters
        /// </summary>
        /// <param name="numberOfPolls">Number of polls to send in the range 1 to 5</param>
        /// <param name="pollInterval">Interval between each poll in the range 10 to 5000 milliseconds</param>
        /// <param name="discoveryPort">Discovery port on which to send the broadcast (normally 32227) in the range 1025 to 65535</param>
        /// <param name="discoveryDuration">Length of time (seconds) to wait for devices to respond</param>
        /// <param name="resolveDnsName">Attempt to resolve host IP addresses to DNS names</param>
        /// <param name="useIpV4">Search for Alpaca devices that use IPv4 addresses. (One or both of useIpV4 and useIpV6 must be True.)</param>
        /// <param name="useIpV6">Search for Alpaca devices that use IPv6 addresses. (One or both of useIpV4 and useIpV6 must be True.)</param>
        public void StartDiscovery(int numberOfPolls, int pollInterval, int discoveryPort, double discoveryDuration, bool resolveDnsName, bool useIpV4, bool useIpV6)
        {

            // Validate parameters
            if (numberOfPolls < 1 | numberOfPolls > 5)
                throw new InvalidValueException($"StartDiscovery - NumberOfPolls: {numberOfPolls} is not within the valid range of 1::5");
            if (pollInterval < 10 | pollInterval > 60000)
                throw new InvalidValueException($"StartDiscovery - PollInterval: {pollInterval} is not within the valid range of 10::5000");
            if (discoveryPort < 1025 | discoveryPort > 65535)
                throw new InvalidValueException($"StartDiscovery - DiscoveryPort: {discoveryPort} is not within the valid range of 1025::65535");
            if (discoveryDuration < 0.0d)
                throw new InvalidValueException($"StartDiscovery - DiscoverDuration: {discoveryDuration} must be greater than 0.0");
            if (!(useIpV4 | useIpV6))
                throw new InvalidValueException("StartDiscovery: Both the use IPv4 and use IPv6 flags are false. At least one of these must be set True.");
            if (!discoveryCompleteValue)
                throw new InvalidOperationException("Cannot start a new discovery because a previous discovery is still running.");

            // Save supplied parameters for use within the application 
            discoveryTime = discoveryDuration;
            tryDnsNameResolution = resolveDnsName;

            // Prepare for a new search
            LogMessage("StartDiscovery", $"Starting search for Alpaca devices with timeout: {discoveryTime} Broadcast polls: {numberOfPolls} sent every {pollInterval} milliseconds");
            finder.ClearCache();

            // Clear the device list dictionary
            lock (deviceListLockObject) // Make sure that the clear operation is not interrupted by other threads
                alpacaDeviceList.Clear();

            discoveryCompleteTimer.Change(Convert.ToInt32(discoveryTime * 1000d), Timeout.Infinite);
            discoveryCompleteValue = false;
            discoveryStartTime = DateTime.Now; // Save the start time

            // Send the broadcast polls
            for (int i = 1, loopTo = numberOfPolls; i <= loopTo; i++)
            {
                LogMessage("StartDiscovery", $"Sending poll {i}...");
                finder.Search(discoveryPort, useIpV4, useIpV6);
                LogMessage("StartDiscovery", $"Poll {i} sent.");
                if (i < numberOfPolls)
                    Thread.Sleep(pollInterval); // Sleep after sending the poll, except for the last one
            }

            LogMessage("StartDiscovery", "Alpaca device broadcast polls completed, discovery started");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Raise an Alpaca devices updated event
        /// </summary>
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
            discoveryCompleteValue = true; // Flag that the timer out has expired
            bool statusMessagesUpdated = false;

            // Update the status messages of management API calls that didn't connect in time
            lock (deviceListLockObject) // Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
            {
                foreach (KeyValuePair<IPEndPoint, AlpacaDevice> alpacaDevice in alpacaDeviceList)
                {

                    if (ReferenceEquals(alpacaDevice.Value.StatusMessage, Constants.TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE))
                    {
                        alpacaDevice.Value.StatusMessage = Constants.FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE;
                        statusMessagesUpdated = true;
                    }
                }
            }

            if (statusMessagesUpdated)
                RaiseAnAlpacaDevicesChangedEvent(); // Raise a devices changed event if any status messages have been updated
            DiscoveryCompleted?.Invoke(this, EventArgs.Empty); // Raise an event to indicate that discovery is complete
        }

        /// <summary>
        /// Handler for device responses coming from the Finder
        /// </summary>
        /// <param name="responderIPEndPoint">Responder's IP address and port</param>
        /// <param name="alpacaDiscoveryResponse">Class containing the information provided by the device in its response.</param>
        private void FoundDeviceEventHandler(IPEndPoint responderIPEndPoint, AlpacaDiscoveryResponse alpacaDiscoveryResponse)
        {
            try
            {
                LogMessage("FoundDeviceEventHandler", $"FOUND Alpaca device at {responderIPEndPoint.Address}:{responderIPEndPoint.Port}"); // Log reception of the broadcast response

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
                    LogMessage("FoundDeviceEventHandler", $"Creating task to retrieve DNS information for device {responderIPEndPoint}:{responderIPEndPoint.Port}");
                    var dnsResolutionThread = new Thread(ResolveIpAddressToHostName);
                    dnsResolutionThread.IsBackground = true;
                    dnsResolutionThread.Start(responderIPEndPoint);
                }

                // Create a task to query this device's Alpaca management API
                LogMessage("FoundDeviceEventHandler", $"Creating thread to retrieve Alpaca management description for device {responderIPEndPoint}:{responderIPEndPoint.Port}");
                var descriptionThread = new Thread(GetAlpacaDeviceInformation);
                descriptionThread.IsBackground = true;
                descriptionThread.Start(responderIPEndPoint);
            }
            catch (Exception ex)
            {
                LogMessage("FoundDeviceEventHandler", $"AddresssFound Exception: {ex}");
            }
        }

        /// <summary>
        /// Get Alpaca device information from the management API
        /// </summary>
        /// <param name="deviceIpEndPointObject"></param>
        private void GetAlpacaDeviceInformation(object deviceIpEndPointObject)
        {
            IPEndPoint deviceIpEndPoint = deviceIpEndPointObject as IPEndPoint;
            string hostIpAndPort = deviceIpEndPoint.ToString();

            try
            {
                LogMessage("GetAlpacaDeviceInformation", $"DISCOVERY TIMEOUT: {discoveryTime} ({discoveryTime * 1000d})");

                // Wait for API version result and process it
                using (var apiClient = new WebClientWithTimeOut())
                {
                    LogMessage("GetAlpacaDeviceInformation", $"About to get version information from http://{hostIpAndPort}/management/apiversions at IP endpoint {deviceIpEndPoint.Address} {deviceIpEndPoint.AddressFamily}");

                    string apiVersionsJsonResponse = GetRequest($"http://{hostIpAndPort}/management/apiversions", Convert.ToInt32(discoveryTime * 1000d));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {apiVersionsJsonResponse}");

                    var serializer = new JavaScriptSerializer();
                    var apiVersionsResponse = serializer.Deserialize<IntArray1DResponse>(apiVersionsJsonResponse);
                    // Dim apiVersionsResponse As IntArray1DResponse = JsonConvert.DeserializeObject(Of IntArray1DResponse)(apiVersionsJsonResponse)

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].SupportedInterfaceVersions = apiVersionsResponse.Value;
                        alpacaDeviceList[deviceIpEndPoint].StatusMessage = ""; // Clear the status field to indicate that this first call was successful
                    }

                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                // Wait for device description result and process it
                using (var descriptionClient = new WebClientWithTimeOut())
                {
                    string deviceDescriptionJsonResponse = GetRequest($"http://{hostIpAndPort}/management/v1/description", Convert.ToInt32(discoveryTime * 1000d));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {deviceDescriptionJsonResponse}");
                    var serializer = new JavaScriptSerializer();
                    var deviceDescriptionResponse = serializer.Deserialize<AlpacaDescriptionResponse>(deviceDescriptionJsonResponse);

                    // Dim deviceDescriptionResponse As AlpacaDescriptionResponse = JsonConvert.DeserializeObject(Of AlpacaDescriptionResponse)(deviceDescriptionJsonResponse)

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].ServerName = deviceDescriptionResponse.Value.ServerName;
                        alpacaDeviceList[deviceIpEndPoint].Manufacturer = deviceDescriptionResponse.Value.Manufacturer;
                        alpacaDeviceList[deviceIpEndPoint].ManufacturerVersion = deviceDescriptionResponse.Value.ManufacturerVersion;
                        alpacaDeviceList[deviceIpEndPoint].Location = deviceDescriptionResponse.Value.Location;
                    }

                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                // Wait for configured devices result and process it
                using (var configuredDevicesClient = new WebClientWithTimeOut())
                {
                    string configuredDevicesJsonResponse = GetRequest($"http://{hostIpAndPort}/management/v1/configureddevices", Convert.ToInt32(discoveryTime * 1000d));
                    LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {configuredDevicesJsonResponse}");

                    var serializer = new JavaScriptSerializer();
                    var configuredDevicesResponse = serializer.Deserialize<AlpacaConfiguredDevicesResponse>(configuredDevicesJsonResponse);

                    // Dim configuredDevicesResponse As AlpacaConfiguredDevicesResponse = JsonConvert.DeserializeObject(Of AlpacaConfiguredDevicesResponse)(configuredDevicesJsonResponse)

                    lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                    {
                        alpacaDeviceList[deviceIpEndPoint].ConfiguredDevices = configuredDevicesResponse.Value;
                        LogMessage("GetAlpacaDeviceInformation", $"Listing configured devices");
                        foreach (ConfiguredDevice configuredDevce in alpacaDeviceList[deviceIpEndPoint].ConfiguredDevices)
                            LogMessage("GetAlpacaDeviceInformation", $"Found configured device: {configuredDevce.DeviceName} {configuredDevce.DeviceType} {configuredDevce.UniqueID}");
                        LogMessage("GetAlpacaDeviceInformation", $"Completed list of configured devices");
                    }

                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }

                LogMessage("GetAlpacaDeviceInformation", $"COMPLETED API tasks for {hostIpAndPort}");
            }
            catch (Exception ex)
            {
                // Something went wrong so log the issue and sent a message to the user
                LogMessage("GetAlpacaDeviceInformation", $"GetAlpacaDescriptions exception: {ex}");

                lock (deviceListLockObject) // Make sure that only one thread can update the device list dictionary at a time
                {
                    alpacaDeviceList[deviceIpEndPoint].StatusMessage = ex.Message;
                    RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                }
            }
        }

        /// <summary>
        /// Resolve a host IP address to a host name
        /// </summary>
        /// <remarks>This first makes a DNS query and uses the result if found. If not found it then tries a Microsoft DNS call which also searches the local hosts and makes a NetBios query.
        /// If this returns an answer it is use. Otherwise the IP address is returned as the host name</remarks>
        private void ResolveIpAddressToHostName(object deviceIpEndPointObject)
        {
            IPEndPoint deviceIpEndPoint = deviceIpEndPointObject as IPEndPoint; // Get the supplied device endpoint as an IPEndPoint

            // test whether the cast was successful
            if (deviceIpEndPoint is not null) // The cast was successful so we can try to search for the host name
            {
                var dnsResponse = new DnsResponse(); // Create a new DnsResponse to hold and return the 

                // Calculate the remaining time before this discovery needs to finish and only undertake DNS resolution if sufficient time remains
                var timeOutTime = TimeSpan.FromSeconds(discoveryTime).Subtract(DateTime.Now - discoveryStartTime).Subtract(TimeSpan.FromSeconds(0.2d));

                if (timeOutTime.TotalSeconds > Constants.MINIMUM_TIME_REMAINING_TO_UNDERTAKE_DNS_RESOLUTION) // We have more than the configured time left so we will attempt a reverse DNS name resolution
                {
                    LogMessage("ResolveIpAddressToHostName", $"Resolving IP address: {deviceIpEndPoint.Address}, Timeout: {timeOutTime}");
                    Dns.BeginGetHostEntry(deviceIpEndPoint.Address.ToString(), new AsyncCallback(GetHostEntryCallback), dnsResponse);

                    // Wait here until the resolve completes and the callback calls .Set()
                    bool dnsWasResolved = dnsResponse.CallComplete.WaitOne(timeOutTime); // Wait for the remaining discovery time

                    // Execution continues here after either a DNS response is found or the request times out
                    if (dnsWasResolved) // A response was received rather than timing out
                    {
                        LogMessage("ResolveIpAddressToHostName", $"{deviceIpEndPoint} has host name: {dnsResponse.HostName} IP address count: {dnsResponse.AddressList.Length} Alias count: {dnsResponse.Aliases.Length}");

                        foreach (IPAddress address in dnsResponse.AddressList)
                            LogMessage("ResolveIpAddressToHostName", $"  Received {address.AddressFamily} address: {address}");

                        foreach (string hostAlias in dnsResponse.Aliases)
                            LogMessage("ResolveIpAddressToHostName", $"  Received alias: {hostAlias}");

                        if (dnsResponse.AddressList.Length > 0) // We got a reply that contains host addresses so there may be a valid host name
                        {

                            lock (deviceListLockObject)
                            {
                                if (!string.IsNullOrEmpty(dnsResponse.HostName))
                                    alpacaDeviceList[deviceIpEndPoint].HostName = dnsResponse.HostName;
                            }

                            RaiseAnAlpacaDevicesChangedEvent(); // Device list was changed so set the changed flag
                        }
                        else
                        {
                            LogMessage("ResolveIpAddressToHostName", $"***** DNS responded with a name ({dnsResponse.HostName}) but this has no associated IP addresses and is probably a NETBIOS name *****");
                        }

                        foreach (IPAddress address in dnsResponse.AddressList)
                            LogMessage("ResolveIpAddressToHostName", $"Address: {address}");

                        foreach (string alias in dnsResponse.Aliases)
                            LogMessage("ResolveIpAddressToHostName", $"Alias: {alias}");
                    }
                    else // DNS did not respond in time
                    {
                        LogMessage("ResolveIpAddressToHostName", $"***** DNS did not respond within timeout - unable to resolve IP address to host name *****");
                    }
                }
                else // There was insufficient time to query DNS
                {
                    LogMessage("ResolveIpAddressToHostName", $"***** Insufficient time remains ({timeOutTime.TotalSeconds} seconds) to conduct a DNS query, ignoring request *****");
                }
            }
            else // The IPEndPoint cast was not successful so we cannot carry out a DNS name search because we don't have the device's IP address
            {
                LogMessage("ResolveIpAddressToHostName", $"DNS resolution could not be undertaken - It was not possible to cast the supplied IPEndPoint object to an IPEndPoint type: {deviceIpEndPoint}.");
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
                LogMessage("GetHostEntryCallback", $"Exception: {ex}"); // Log exceptions but don't throw them
            }
        }

        /// <summary>
        /// Log a message to the screen, adding the current managed thread ID
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="message"></param>
        private void LogMessage(string methodName, string message)
        {
            string indentSpaces;

            // Create the required number of space characters for indented logging based on the managed thread number
            indentSpaces = new string(' ', Thread.CurrentThread.ManagedThreadId * Constants.NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES);

            // Log the message so long as the trace logger is not null
            TL?.LogMessageCrLf($"AlpacaDiscovery - {methodName}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} {message}");
        }

        /// <summary>
        /// Call a device URL and return the response as a string, timing out after a specified time
        /// </summary>
        /// <param name="deviceUrl">Device's URL to call</param>
        /// <param name="timeOut">Length of time to wait for a response</param>
        /// <returns>Device response as a string</returns>
        private string GetRequest(string deviceUrl, int timeOut)
        {
            WebClientWithTimeOut webClient;
            string returnString;

            webClient = new WebClientWithTimeOut();
            webClient.Timeout = timeOut;

            // Get the string response from the device
            returnString = webClient.DownloadString(deviceUrl);

            return returnString;
        }

        #endregion

    }
}