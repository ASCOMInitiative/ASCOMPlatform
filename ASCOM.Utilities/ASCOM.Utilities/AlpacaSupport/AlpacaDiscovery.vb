Imports System.Collections.Generic
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Threading
Imports ASCOM.Utilities.Interfaces
Imports Newtonsoft.Json

''' <summary>
''' Enables clients to discover Alpaca devices by sending one or more discovery polls. Returns information on discovered Alpaca devices and the ASCOM devices that are available.
''' </summary>
<Guid("877A70E7-0A70-41EE-829A-8C00CAE2B9F0"),
ComVisible(True),
ClassInterface(ClassInterfaceType.None)>
Public Class AlpacaDiscovery
    Implements IAlpacaDiscovery, IAlpacaDiscoveryExtra, IDisposable

#Region "Variables"

    ' Utility objects
    Private TL As TraceLogger
    Private finder As Finder
    Private discoveryCompleteTimer As System.Threading.Timer

    ' Private variables
    Private alpacaDeviceList As Dictionary(Of IPEndPoint, AlpacaDevice) = New Dictionary(Of IPEndPoint, AlpacaDevice)() ' List of discovered Alpaca devices keyed on IP:Port
    Private disposedValue As Boolean = False ' To detect redundant Dispose() method calls
    Private discoveryTime As Double ' Length of the discovery phase before it times out
    Private tryDnsNameResolution As Boolean ' Flag indicating whether to attempt name resolution on the host IP address
    Private discoveryStartTime As Date ' Time at which the start discovery command was received
    Private discoveryCompleteValue As Boolean ' Discovery completion status
    Private ReadOnly deviceListLockObject As Object = New Object() ' Lock object to synchronise access to the Alpaca device list collection, which is not a thread safe collection

#End Region

#Region "New and IDisposable Support"

    ''' <summary>
    ''' Initialise the Alpaca discovery component
    ''' </summary>
    Public Sub New()
        InitialiseClass() ' Initialise without a trace logger
    End Sub

    ''' <summary>
    ''' Initialiser that takes a trace logger (Can only be used from .NET clients)
    ''' </summary>
    ''' <param name="traceLogger">Trace logger instance to use for activity logging</param>
    Friend Sub New(ByVal traceLogger As TraceLogger)
        TL = traceLogger ' Save the supplied trace logger object
        InitialiseClass() ' Initialise using the trace logger
    End Sub

    Private Sub InitialiseClass()
        Try
            ' Initialise variables
            tryDnsNameResolution = False ' Initialise so that there is no host name resolution by default
            discoveryCompleteValue = True ' Initialise so that discoveries can be run

            ' Initialise utility objects
            discoveryCompleteTimer = New System.Threading.Timer(AddressOf OnDiscoveryCompleteTimer)

            If finder IsNot Nothing Then
                finder.Dispose()
                finder = Nothing
            End If

            ' Get a new broadcast response finder
            finder = New Finder(AddressOf FoundDeviceEventHandler, TL)

            LogMessage("AlpacaDiscoveryInitialise", $"Complete - Running on thread {Thread.CurrentThread.ManagedThreadId}")
        Catch ex As Exception
            LogMessage("AlpacaDiscoveryInitialise", $"Exception{ex}")
        End Try

    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then

            If disposing Then
                ' The trace logger is not disposed here because it is supplied by the client, which is response for disposing of it as appropriate.

                If finder IsNot Nothing Then finder.Dispose()

                If discoveryCompleteTimer IsNot Nothing Then discoveryCompleteTimer.Dispose()
            End If

            disposedValue = True
        End If
    End Sub

    ''' <summary>
    ''' Disposes of the discovery component and cleans up resources
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put clean-up code in Dispose(bool disposing) above.
        Dispose(True)
    End Sub

#End Region

#Region "Public Events"

    ''' <summary>
    ''' Raised every time information about discovered devices is updated
    ''' </summary>
    ''' <remarks>This event is only available to .NET clients, there is no equivalent for COM clients.</remarks>
    Public Event AlpacaDevicesUpdated As EventHandler Implements IAlpacaDiscoveryExtra.AlpacaDevicesUpdated

    ''' <summary>
    ''' Raised when the discovery is complete
    ''' </summary>
    ''' <remarks>This event is only available to .NET clients. COM clients should poll the <see cref="DiscoveryComplete"/> property periodically to determine when discovery is complete.</remarks>
    Public Event DiscoveryCompleted As EventHandler Implements IAlpacaDiscoveryExtra.DiscoveryCompleted

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Flag that indicates when a discovery cycle is complete
    ''' </summary>
    ''' <returns>True when discovery is complete.</returns>
    ''' <remarks>The discovery is considered complete when the time period specified on the <see cref="StartDiscovery(Integer, Integer, Integer, Double, Boolean, Boolean, Boolean)"/> method is exceeded.</remarks>
    Public Property DiscoveryComplete As Boolean Implements IAlpacaDiscovery.DiscoveryComplete
        Get
            Return discoveryCompleteValue
        End Get
        Private Set(ByVal value As Boolean)
            discoveryCompleteValue = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Returns an ArrayList of discovered Alpaca devices for use by COM clients
    ''' </summary>
    ''' <returns>ArrayList of <see cref="AlpacaDevice"/>classes</returns>
    ''' <remarks>This method is for use by COM clients because it is not possible to pass a generic list as used in <see cref="GetAlpacaDevices"/> through a COM interface. 
    ''' .NET clients should use <see cref="GetAlpacaDevices()"/> instead of this method.</remarks>
    Public Function GetAlpacaDevicesAsArrayList() As ArrayList Implements IAlpacaDiscovery.GetAlpacaDevicesAsArrayList
        Dim alpacaDevicesAsArrayList As ArrayList ' Variable to hold the arraylist analogue of the generic list of Alpaca devices

        alpacaDevicesAsArrayList = New ArrayList ' Create a new array-list

        ' populate the array-list with data from the generic list
        For Each alpacaDevice As AlpacaDevice In GetAlpacaDevices()
            alpacaDevicesAsArrayList.Add(alpacaDevice)
        Next

        Return alpacaDevicesAsArrayList ' Return the Alpaca devices list as an ArrayList
    End Function

    ''' <summary>
    ''' Returns an ArrayList of discovered ASCOM devices, of the specified device type, for use by COM clients
    ''' </summary>
    ''' <param name="deviceType">The device type for which to search e.g. Telescope, Focuser. An empty string will return devices of all types.</param>
    ''' <returns>ArrayList of <see cref="AscomDevice"/>classes</returns>
    ''' <remarks>
    ''' <para>
    ''' This method is for use by COM clients because it is not possible to return a generic list, as used in <see cref="GetAscomDevices(String)"/>, through a COM interface. 
    ''' .NET clients should use <see cref="GetAscomDevices(String)"/> instead of this method.
    ''' </para>
    ''' <para>
    ''' This method will return every discovered device, regardless of device type, if the supplied "deviceType" parameter is an empty string.
    ''' </para>
    ''' </remarks>
    Public Function GetAscomDevicesAsArrayList(ByVal deviceType As String) As ArrayList Implements IAlpacaDiscovery.GetAscomDevicesAsArrayList
        Return New ArrayList(GetAscomDevices(deviceType)) ' Return the ASCOM devices list as an ArrayList
    End Function

    ''' <summary>
    ''' Returns a generic List of discovered Alpaca devices.
    ''' </summary>
    ''' <returns>List of <see cref="AlpacaDevice"/>classes</returns>
    ''' <remarks>This method is only available to .NET clients because COM cannot handle generic types. COM clients should use <see cref="GetAlpacaDevicesAsArrayList()"/>.</remarks>
    Public Function GetAlpacaDevices() As List(Of AlpacaDevice) Implements IAlpacaDiscoveryExtra.GetAlpacaDevices
        SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while copying it to the list
            Return alpacaDeviceList.Values.ToList() ' Create a copy of the dynamically changing alpacaDeviceList ConcurrentDictionary of discovered devices
        End SyncLock
    End Function

    ''' <summary>
    ''' Returns a generic list of discovered ASCOM devices of the specified device type.
    ''' </summary>
    ''' <param name="deviceType">The device type for which to search e.g. Telescope, Focuser. An empty string will return devices of all types.</param>
    ''' <returns>List of AscomDevice classes</returns>
    ''' <remarks>
    ''' <para>
    ''' This method is only available to .NET clients because COM cannot handle generic types. COM clients should use <see cref="GetAlpacaDevicesAsArrayList()"/>.
    ''' </para>
    ''' <para>
    ''' This method will return every discovered device, regardless of device type, if the supplied "deviceType" parameter is an empty string.
    ''' </para>
    ''' </remarks>
    Public Function GetAscomDevices(ByVal deviceType As String) As List(Of AscomDevice) Implements IAlpacaDiscoveryExtra.GetAscomDevices
        Dim ascomDeviceList As List(Of AscomDevice) = New List(Of AscomDevice)() ' List of discovered ASCOM devices to support Chooser-like functionality

        SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while processing this command

            ' Iterate over the discovered Alpaca devices
            For Each alpacaDevice As KeyValuePair(Of IPEndPoint, AlpacaDevice) In alpacaDeviceList

                ' Iterate over each Alpaca interface version that the Alpaca device supports
                For Each alpacaDeviceInterfaceVersion As Integer In alpacaDevice.Value.SupportedInterfaceVersions

                    ' Iterate over the ASCOM devices presented by this Alpaca device adding them to the return dictionary
                    For Each ascomDevice As ConfiguredDevice In alpacaDevice.Value.ConfiguredDevices

                        ' Test whether all devices or only devices of a specific device type are required
                        If String.IsNullOrEmpty(deviceType) Then ' Return a full list of every discovered device regardless of device type 
                            ascomDeviceList.Add(New AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)) ' ASCOM device information 
                        Else ' Return only devices of the specified type
                            If ascomDevice.DeviceType.ToLowerInvariant() = deviceType.ToLowerInvariant() Then
                                ascomDeviceList.Add(New AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)) ' ASCOM device information 
                            End If
                        End If

                    Next ' Next Ascom Device
                Next ' Next interface version
            Next ' Next Alpaca device

            ' Return the information requested
            Return ascomDeviceList ' Return the list of ASCOM devices

        End SyncLock
    End Function

    ''' <summary>
    ''' Start an Alpaca device discovery based on the supplied parameters
    ''' </summary>
    ''' <param name="numberOfPolls">Number of polls to send in the range 1 to 5</param>
    ''' <param name="pollInterval">Interval between each poll in the range 10 to 5000 milliseconds</param>
    ''' <param name="discoveryPort">Discovery port on which to send the broadcast (normally 32227) in the range 1025 to 65535</param>
    ''' <param name="discoveryDuration">Length of time (seconds) to wait for devices to respond</param>
    ''' <param name="resolveDnsName">Attempt to resolve host IP addresses to DNS names</param>
    ''' <param name="useIpV4">Search for Alpaca devices that use IPv4 addresses. (One or both of useIpV4 and useIpV6 must be True.)</param>
    ''' <param name="useIpV6">Search for Alpaca devices that use IPv6 addresses. (One or both of useIpV4 and useIpV6 must be True.)</param>
    Public Sub StartDiscovery(ByVal numberOfPolls As Integer,
                              ByVal pollInterval As Integer,
                              ByVal discoveryPort As Integer,
                              ByVal discoveryDuration As Double,
                              ByVal resolveDnsName As Boolean,
                              ByVal useIpV4 As Boolean,
                              ByVal useIpV6 As Boolean) Implements IAlpacaDiscovery.StartDiscovery

        ' Validate parameters
        If (numberOfPolls < 1) Or (numberOfPolls > 5) Then Throw New InvalidValueException($"StartDiscovery - NumberOfPolls: {numberOfPolls} is not within the valid range of 1::5")
        If (pollInterval < 10) Or (pollInterval > 60000) Then Throw New InvalidValueException($"StartDiscovery - PollInterval: {pollInterval} is not within the valid range of 10::5000")
        If (discoveryPort < 1025) Or (discoveryPort > 65535) Then Throw New InvalidValueException($"StartDiscovery - DiscoveryPort: {discoveryPort} is not within the valid range of 1025::65535")
        If discoveryDuration < 0.0 Then Throw New InvalidValueException($"StartDiscovery - DiscoverDuration: {discoveryDuration} must be greater than 0.0")
        If Not (useIpV4 Or useIpV6) Then Throw New InvalidValueException("StartDiscovery: Both the use IPv4 and use IPv6 flags are false. At least one of these must be set True.")
        If Not discoveryCompleteValue Then Throw New InvalidOperationException("Cannot start a new discovery because a previous discovery is still running.")

        ' Save supplied parameters for use within the application 
        discoveryTime = discoveryDuration
        tryDnsNameResolution = resolveDnsName

        ' Prepare for a new search
        LogMessage("StartDiscovery", $"Starting search for Alpaca devices with timeout: {discoveryTime} Broadcast polls: {numberOfPolls} sent every {pollInterval} milliseconds")
        finder.ClearCache()

        ' Clear the device list dictionary
        SyncLock deviceListLockObject ' Make sure that the clear operation is not interrupted by other threads
            alpacaDeviceList.Clear()
        End SyncLock

        discoveryCompleteTimer.Change(Convert.ToInt32(discoveryTime * 1000), Timeout.Infinite)
        discoveryCompleteValue = False
        discoveryStartTime = Date.Now ' Save the start time

        ' Send the broadcast polls
        For i As Integer = 1 To numberOfPolls
            LogMessage("StartDiscovery", $"Sending poll {i}...")
            finder.Search(discoveryPort, useIpV4, useIpV6)
            LogMessage("StartDiscovery", $"Poll {i} sent.")
            If i < numberOfPolls Then Thread.Sleep(pollInterval) ' Sleep after sending the poll, except for the last one
        Next

        LogMessage("StartDiscovery", "Alpaca device broadcast polls completed, discovery started")
    End Sub

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Raise an Alpaca devices updated event
    ''' </summary>
    Private Sub RaiseAnAlpacaDevicesChangedEvent()
        RaiseEvent AlpacaDevicesUpdated(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Discovery timer event handler - called when the allocated discovery period has ended
    ''' </summary>
    ''' <param name="state">Timer state</param>
    Private Sub OnDiscoveryCompleteTimer(ByVal state As Object)
        LogMessage("OnTimeOutTimerFired", $"Firing discovery complete event")
        discoveryCompleteValue = True ' Flag that the timer out has expired
        Dim statusMessagesUpdated As Boolean = False

        ' Update the status messages of management API calls that didn't connect in time
        SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
            For Each alpacaDevice As KeyValuePair(Of IPEndPoint, AlpacaDevice) In alpacaDeviceList

                If alpacaDevice.Value.StatusMessage Is TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE Then
                    alpacaDevice.Value.StatusMessage = FAILED_TO_CONTACT_MANAGEMENT_API_MESSAGE
                    statusMessagesUpdated = True
                End If
            Next
        End SyncLock

        If statusMessagesUpdated Then RaiseAnAlpacaDevicesChangedEvent() ' Raise a devices changed event if any status messages have been updated
        RaiseEvent DiscoveryCompleted(Me, EventArgs.Empty) ' Raise an event to indicate that discovery is complete
    End Sub

    ''' <summary>
    ''' Handler for device responses coming from the Finder
    ''' </summary>
    ''' <param name="responderIPEndPoint">Responder's IP address and port</param>
    ''' <param name="alpacaDiscoveryResponse">Class containing the information provided by the device in its response.</param>
    Private Sub FoundDeviceEventHandler(ByVal responderIPEndPoint As IPEndPoint, ByVal alpacaDiscoveryResponse As AlpacaDiscoveryResponse)
        Try
            LogMessage("FoundDeviceEventHandler", $"FOUND Alpaca device at {responderIPEndPoint.Address}:{responderIPEndPoint.Port}") ' Log reception of the broadcast response

            ' Add the new device or ignore this duplicate if it already exists
            SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
                If Not alpacaDeviceList.ContainsKey(responderIPEndPoint) Then
                    alpacaDeviceList.Add(responderIPEndPoint, New AlpacaDevice(responderIPEndPoint, TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE))
                    RaiseAnAlpacaDevicesChangedEvent() ' Device was added so set the changed flag
                End If
            End SyncLock

            ' Create a task to query this device's DNS name, if configured to do so
            If tryDnsNameResolution Then
                LogMessage("FoundDeviceEventHandler", $"Creating task to retrieve DNS information for device {responderIPEndPoint}:{responderIPEndPoint.Port}")
                Dim dnsResolutionThread As Thread = New Thread(AddressOf ResolveIpAddressToHostName)
                dnsResolutionThread.IsBackground = True
                dnsResolutionThread.Start(responderIPEndPoint)
            End If

            ' Create a task to query this device's Alpaca management API
            LogMessage("FoundDeviceEventHandler", $"Creating thread to retrieve Alpaca management description for device {responderIPEndPoint}:{responderIPEndPoint.Port}")
            Dim descriptionThread As Thread = New Thread(AddressOf GetAlpacaDeviceInformation)
            descriptionThread.IsBackground = True
            descriptionThread.Start(responderIPEndPoint)
        Catch ex As Exception
            LogMessage("FoundDeviceEventHandler", $"AddresssFound Exception: {ex}")
        End Try
    End Sub

    ''' <summary>
    ''' Get Alpaca device information from the management API
    ''' </summary>
    ''' <param name="deviceIpEndPointObject"></param>
    Private Sub GetAlpacaDeviceInformation(ByVal deviceIpEndPointObject As Object)
        Dim deviceIpEndPoint As IPEndPoint = TryCast(deviceIpEndPointObject, IPEndPoint)
        Dim hostIpAndPort As String = deviceIpEndPoint.ToString()

        Try
            LogMessage("GetAlpacaDeviceInformation", $"DISCOVERY TIMEOUT: {discoveryTime} ({discoveryTime * 1000})")

            ' Wait for API version result and process it
            Using apiClient As WebClientWithTimeOut = New WebClientWithTimeOut()
                LogMessage("GetAlpacaDeviceInformation", $"About to get version information from http://{hostIpAndPort}/management/apiversions at IP endpoint {deviceIpEndPoint.Address} {deviceIpEndPoint.AddressFamily}")

                Dim apiVersionsJsonResponse As String = GetRequest($"http://{hostIpAndPort}/management/apiversions", Convert.ToInt32(discoveryTime * 1000))
                LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {apiVersionsJsonResponse}")
                Dim apiVersionsResponse As IntArray1DResponse = JsonConvert.DeserializeObject(Of IntArray1DResponse)(apiVersionsJsonResponse)

                SyncLock deviceListLockObject ' Make sure that only one thread can update the device list dictionary at a time
                    alpacaDeviceList(deviceIpEndPoint).SupportedInterfaceVersions = apiVersionsResponse.Value
                    alpacaDeviceList(deviceIpEndPoint).StatusMessage = "" ' Clear the status field to indicate that this first call was successful
                End SyncLock

                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End Using

            ' Wait for device description result and process it
            Using descriptionClient As WebClientWithTimeOut = New WebClientWithTimeOut()
                Dim deviceDescriptionJsonResponse As String = GetRequest($"http://{hostIpAndPort}/management/v1/description", Convert.ToInt32(discoveryTime * 1000))
                LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {deviceDescriptionJsonResponse}")
                Dim deviceDescriptionResponse As AlpacaDescriptionResponse = JsonConvert.DeserializeObject(Of AlpacaDescriptionResponse)(deviceDescriptionJsonResponse)

                SyncLock deviceListLockObject ' Make sure that only one thread can update the device list dictionary at a time
                    alpacaDeviceList(deviceIpEndPoint).ServerName = deviceDescriptionResponse.Value.ServerName
                    alpacaDeviceList(deviceIpEndPoint).Manufacturer = deviceDescriptionResponse.Value.Manufacturer
                    alpacaDeviceList(deviceIpEndPoint).ManufacturerVersion = deviceDescriptionResponse.Value.ManufacturerVersion
                    alpacaDeviceList(deviceIpEndPoint).Location = deviceDescriptionResponse.Value.Location
                End SyncLock

                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End Using

            ' Wait for configured devices result and process it
            Using configuredDevicesClient As WebClientWithTimeOut = New WebClientWithTimeOut()
                Dim configuredDevicesJsonResponse As String = GetRequest($"http://{hostIpAndPort}/management/v1/configureddevices", Convert.ToInt32(discoveryTime * 1000))
                LogMessage("GetAlpacaDeviceInformation", $"Received JSON response from {hostIpAndPort}: {configuredDevicesJsonResponse}")
                Dim configuredDevicesResponse As AlpacaConfiguredDevicesResponse = JsonConvert.DeserializeObject(Of AlpacaConfiguredDevicesResponse)(configuredDevicesJsonResponse)

                SyncLock deviceListLockObject ' Make sure that only one thread can update the device list dictionary at a time
                    alpacaDeviceList(deviceIpEndPoint).ConfiguredDevices = configuredDevicesResponse.Value
                    LogMessage("GetAlpacaDeviceInformation", $"Listing configured devices")
                    For Each configuredDevce As ConfiguredDevice In alpacaDeviceList(deviceIpEndPoint).ConfiguredDevices
                        LogMessage("GetAlpacaDeviceInformation", $"Found configured device: {configuredDevce.DeviceName} {configuredDevce.DeviceType} {configuredDevce.UniqueID}")
                    Next
                    LogMessage("GetAlpacaDeviceInformation", $"Completed list of configured devices")
                End SyncLock

                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End Using

            LogMessage("GetAlpacaDeviceInformation", $"COMPLETED API tasks for {hostIpAndPort}")
        Catch ex As Exception
            ' Something went wrong so log the issue and sent a message to the user
            LogMessage("GetAlpacaDeviceInformation", $"GetAlpacaDescriptions exception: {ex}")

            SyncLock deviceListLockObject ' Make sure that only one thread can update the device list dictionary at a time
                alpacaDeviceList(deviceIpEndPoint).StatusMessage = ex.Message
                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End SyncLock
        End Try
    End Sub

    ''' <summary>
    ''' Resolve a host IP address to a host name
    ''' </summary>
    ''' <remarks>This first makes a DNS query and uses the result if found. If not found it then tries a Microsoft DNS call which also searches the local hosts and makes a netbios query.
    ''' If this returns an answer it is use. Otherwise the IP address is returned as the host name</remarks>
    Private Sub ResolveIpAddressToHostName(ByVal deviceIpEndPointObject As Object)
        Dim deviceIpEndPoint As IPEndPoint = TryCast(deviceIpEndPointObject, IPEndPoint) ' Get the supplied device endpoint as an IPEndPoint

        ' test whether the cast was successful
        If Not deviceIpEndPoint Is Nothing Then ' The cast was successful so we can try to search for the host name
            Dim dnsResponse As DnsResponse = New DnsResponse() ' Create a new DnsResponse to hold and return the 

            ' Calculate the remaining time before this discovery needs to finish and only undertake DNS resolution if sufficient time remains
            Dim timeOutTime As TimeSpan = TimeSpan.FromSeconds(discoveryTime).Subtract(Date.Now - discoveryStartTime).Subtract(TimeSpan.FromSeconds(0.2))

            If timeOutTime.TotalSeconds > Constants.MINIMUM_TIME_REMAINING_TO_UNDERTAKE_DNS_RESOLUTION Then ' We have more than the configured time left so we will attempt a reverse DNS name resolution
                LogMessage("ResolveIpAddressToHostName", $"Resolving IP address: {deviceIpEndPoint.Address}, Timeout: {timeOutTime}")
                Dns.BeginGetHostEntry(deviceIpEndPoint.Address.ToString(), New AsyncCallback(AddressOf GetHostEntryCallback), dnsResponse)

                ' Wait here until the resolve completes and the callback calls .Set()
                Dim dnsWasResolved As Boolean = dnsResponse.CallComplete.WaitOne(timeOutTime) ' Wait for the remaining discovery time

                ' Execution continues here after either a DNS response is found or the request times out
                If dnsWasResolved Then ' A response was received rather than timing out
                    LogMessage("ResolveIpAddressToHostName", $"{deviceIpEndPoint} has host name: {dnsResponse.HostName} IP address count: {dnsResponse.AddressList.Length} Alias count: {dnsResponse.Aliases.Length}")

                    For Each address As IPAddress In dnsResponse.AddressList
                        LogMessage("ResolveIpAddressToHostName", $"  Received {address.AddressFamily} address: {address}")
                    Next

                    For Each hostAlias As String In dnsResponse.Aliases
                        LogMessage("ResolveIpAddressToHostName", $"  Received alias: {hostAlias}")
                    Next

                    If dnsResponse.AddressList.Length > 0 Then ' We got a reply that contains host addresses so there may be a valid host name

                        SyncLock deviceListLockObject
                            If Not String.IsNullOrEmpty(dnsResponse.HostName) Then alpacaDeviceList(deviceIpEndPoint).HostName = dnsResponse.HostName
                        End SyncLock

                        RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
                    Else
                        LogMessage("ResolveIpAddressToHostName", $"***** DNS responded with a name ({dnsResponse.HostName}) but this has no associated IP addresses and is probably a NETBIOS name *****")
                    End If

                    For Each address As IPAddress In dnsResponse.AddressList
                        LogMessage("ResolveIpAddressToHostName", $"Address: {address}")
                    Next

                    For Each [alias] As String In dnsResponse.Aliases
                        LogMessage("ResolveIpAddressToHostName", $"Alias: {[alias]}")
                    Next
                Else ' DNS did not respond in time
                    LogMessage("ResolveIpAddressToHostName", $"***** DNS did not respond within timeout - unable to resolve IP address to host name *****")
                End If
            Else ' There was insufficient time to query DNS
                LogMessage("ResolveIpAddressToHostName", $"***** Insufficient time remains ({timeOutTime.TotalSeconds} seconds) to conduct a DNS query, ignoring request *****")
            End If
        Else ' The IPEndPoint cast was not successful so we cannot carry out a DNS name search because we don't have the device's IP address
            LogMessage("ResolveIpAddressToHostName", $"DNS resolution could not be undertaken - It was not possible to cast the supplied IPEndPoint object to an IPEndPoint type: {deviceIpEndPoint}.")
        End If
    End Sub

    ''' <summary>
    ''' Record the IPs in the state object for later use.
    ''' </summary>
    Private Sub GetHostEntryCallback(ByVal ar As IAsyncResult)
        Try
            Dim dnsResponse As DnsResponse = CType(ar.AsyncState, DnsResponse) ' Turn the state object into the DnsResponse type
            dnsResponse.IpHostEntry = Dns.EndGetHostEntry(ar) ' Save the returned IpHostEntry and populate other fields based on its parameters
            dnsResponse.CallComplete.Set() ' Set the wait handle so that the caller knows that the asynchronous call has completed and that the response has been updated
        Catch ex As Exception
            LogMessage("GetHostEntryCallback", $"Exception: {ex}") ' Log exceptions but don't throw them
        End Try
    End Sub

    ''' <summary>
    ''' Log a message to the screen, adding the current managed thread ID
    ''' </summary>
    ''' <param name="methodName"></param>
    ''' <param name="message"></param>
    Private Sub LogMessage(ByVal methodName As String, ByVal message As String)
        Dim indentSpaces As String

        ' Create the required number of space characters for indented logging based on the managed thread number
        indentSpaces = New [String](" "c, Thread.CurrentThread.ManagedThreadId * NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES)

        ' Log the message so long as the trace logger is not null
        TL?.LogMessageCrLf($"AlpacaDiscovery - {methodName}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} {message}")
    End Sub

    ''' <summary>
    ''' Call a device URL and return the response as a string, timing out after a specified time
    ''' </summary>
    ''' <param name="deviceUrl">Device's URL to call</param>
    ''' <param name="timeOut">Length of time to wait for a response</param>
    ''' <returns>Device response as a string</returns>
    Private Function GetRequest(ByVal deviceUrl As String, ByVal timeOut As Integer) As String
        Dim webClient As WebClientWithTimeOut
        Dim returnString As String

        webClient = New WebClientWithTimeOut()
        webClient.Timeout = timeOut

        ' Get the string response from the device
        returnString = CType(webClient.DownloadString(deviceUrl), String)

        Return returnString
    End Function

#End Region

End Class
