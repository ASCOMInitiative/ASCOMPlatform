Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Threading
Imports Newtonsoft.Json

''' <summary>
''' Alpaca discovery component
''' Note: Forced the use of System.Net.Http.HttpClientHandler instead of System.Net.Http.SocketsHttpHandler because HttpClient timeout doesn't work correctly in the latter
''' </summary>
Friend Class AlpacaDiscovery
    Implements IDisposable


    ''' <summary>
    ''' Returns discovery completion status
    ''' </summary>
    Private _DiscoveryComplete As Boolean
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
    Private ReadOnly deviceListLockObject As Object = New Object() ' Lock object to synchronise access to the Alpaca device list collection, which is not a thread safe collection

#Region "New and IDisposable Support"

    ''' <summary>
    ''' Initialiser that takes a trace logger
    ''' </summary>
    ''' <paramname="tl">Trace logger instance to use for activity logging</param>
    Friend Sub New(ByVal tl As TraceLogger)
        Try
            ' Save the supplied trace logger object
            Me.TL = tl

            ' Initialise variables
            tryDnsNameResolution = False ' Initialise so that there is no host name resolution by default
            DiscoveryComplete = True ' Initialise so that discoveries can be run

            ' Initialise utility objects
            discoveryCompleteTimer = New System.Threading.Timer(AddressOf OnDiscoveryCompleteTimer)

            If finder IsNot Nothing Then
                finder.Dispose()
                finder = Nothing
            End If

            finder = New Finder(AddressOf BroadcastResponseEventHandler, Me.TL) ' Get a new broadcast response finder
            LogMessage("AlpacaDiscoveryInitialise", $"Complete - Running on thread {Thread.CurrentThread.ManagedThreadId}")
        Catch ex As Exception
            LogMessage("AlpacaDiscoveryInitialise", $"Exception{ex.ToString()}")
        End Try
    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then

            If disposing Then
                If finder IsNot Nothing Then finder.Dispose()
                If discoveryCompleteTimer IsNot Nothing Then discoveryCompleteTimer.Dispose()
            End If

            disposedValue = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put clean-up code in Dispose(bool disposing) above.
        Dispose(True)
    End Sub


#End Region

#Region "Public events and methods"

    ''' <summary>
    ''' Raised every time information about discovered devices is updated
    ''' </summary>
    Public Event AlpacaDevicesUpdated As EventHandler

    ''' <summary>
    ''' Raised when the discovery is complete
    ''' </summary>
    Public Event DiscoveryCompleted As EventHandler


    ''' <summary>
    ''' Returns a list of discovered Alpaca devices
    ''' </summary>
    ''' <returns>List of AlpacaDevice classes</returns>
    Public Function GetAlpacaDevices() As List(Of AlpacaDevice)
        SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while copying it to the list
            Return alpacaDeviceList.Values.ToList() ' Create a copy of the dynamically changing alpacaDeviceList ConcurrentDictionary of discovered devices
        End SyncLock
    End Function


    ''' <summary>
    ''' Returns a list of discovered ASCOM devices of the specified device type for Chooser-like functionality
    ''' </summary>
    ''' <returns>List of AscomDevice classes</returns>
    Public Function GetAscomDevices(ByVal deviceType As String) As List(Of AscomDevice)
        Return GetAscomDevices().Where(Function(ascomDevice) ascomDevice.AscomDeviceType.ToLowerInvariant() = deviceType.ToLowerInvariant()).ToList()
    End Function


    ''' <summary>
    ''' Returns a list of all discovered ASCOM devices for Chooser-like functionality
    ''' </summary>
    ''' <returns>List of AscomDevice classes</returns>
    Public Function GetAscomDevices() As List(Of AscomDevice)
        Dim ascomDeviceList As List(Of AscomDevice) = New List(Of AscomDevice)() ' List of discovered ASCOM devices to support Chooser-like functionality

        ' Iterate over the discovered Alpaca devices
        SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while copying it to the list
            For Each alpacaDevice As KeyValuePair(Of IPEndPoint, AlpacaDevice) In alpacaDeviceList

                ' Iterate over each Alpaca interface version that the Alpaca device supports
                For Each alpacaDeviceInterfaceVersion As Integer In alpacaDevice.Value.SupportedInterfaceVersions

                    ' Iterate over the ASCOM devices presented by this Alpaca device adding them to the return dictionary
                    For Each ascomDevice As ConfiguredDevice In alpacaDevice.Value.ConfiguredDevices
                        ascomDeviceList.Add(New AscomDevice(ascomDevice.DeviceName, ascomDevice.DeviceType, ascomDevice.DeviceNumber, ascomDevice.UniqueID, alpacaDevice.Value.IPEndPoint, alpacaDevice.Value.HostName, alpacaDeviceInterfaceVersion, alpacaDevice.Value.StatusMessage)) ' ASCOM device information 
                        ' Alpaca device information
                    Next
                Next
            Next
        End SyncLock

        Return ascomDeviceList ' Return the list of ASCOM devices
    End Function

    Public Property DiscoveryComplete As Boolean
        Get
            Return _DiscoveryComplete
        End Get
        Private Set(ByVal value As Boolean)
            _DiscoveryComplete = value
        End Set
    End Property


    ''' <summary>
    ''' Start an Alpaca device discovery
    ''' </summary>
    ''' <paramname="numberOfPolls">Number of polls to send in the range 1 to 5</param>
    ''' <paramname="pollInterval">Interval between each poll in the range 10 to 5000 milliseconds</param>
    ''' <paramname="discoveryPort">Discovery port on which to send the broadcast (normally 32227)</param>
    ''' <paramname="discoveryDuration">Length of time to wait for devices to respond</param>
    ''' <paramname="resolveDnsName">Attempt to resolve host IP addresses to DNS names</param>
    Public Sub StartDiscovery(ByVal numberOfPolls As Integer, ByVal pollInterval As Integer, ByVal discoveryPort As Integer, ByVal discoveryDuration As Double, ByVal resolveDnsName As Boolean)
        ' Validate parameters
        If numberOfPolls < 1 OrElse numberOfPolls > 5 Then Throw New ArgumentException($"StartDiscovery - NumberOfPolls: {numberOfPolls} is not within the valid range of 1::5")
        If pollInterval < 10 OrElse pollInterval > 5000 Then Throw New ArgumentException($"StartDiscovery - PollInterval: {numberOfPolls} is not within the valid range of 10::5000")
        If Not DiscoveryComplete Then Throw New InvalidOperationException("Cannot start a new discovery because a previous discovery is still running.")

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
        DiscoveryComplete = False
        discoveryStartTime = Date.Now ' Save the start time

        ' Send the broadcast polls
        For i As Integer = 1 To numberOfPolls
            LogMessage("StartDiscovery", $"Sending poll {i}...")
            finder.Search(discoveryPort)
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
    ''' <paramname="e"></param>
    Private Sub RaiseAnAlpacaDevicesChangedEvent()
        RaiseEvent AlpacaDevicesUpdated(Me, EventArgs.Empty)
    End Sub


    ''' <summary>
    ''' Discovery timer event handler - called when the allocated discovery period has ended
    ''' </summary>
    ''' <paramname="state">Timer state</param>
    Private Sub OnDiscoveryCompleteTimer(ByVal state As Object)
        LogMessage("OnTimeOutTimerFired", $"Firing discovery complete event")
        DiscoveryComplete = True ' Flag that the timer out has expired
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
    ''' <paramname="responderIPEndPoint">Responder's IP address and port</param>
    ''' <paramname="alpacaDiscoveryResponse">Class containing the information provided by the device in its response.</param>
    Private Sub BroadcastResponseEventHandler(ByVal responderIPEndPoint As IPEndPoint, ByVal alpacaDiscoveryResponse As AlpacaDiscoveryResponse)
        Try
            LogMessage("BroadcastResponseEventHandler", $"FOUND Alpaca device at {responderIPEndPoint.Address}:{responderIPEndPoint.Port}") ' Log reception of the broadcast response

            ' Add the new device or ignore this duplicate if it already exists
            SyncLock deviceListLockObject ' Make sure that the device list dictionary can't change while being read and that only one thread can update it at a time
                If Not alpacaDeviceList.ContainsKey(responderIPEndPoint) Then
                    alpacaDeviceList.Add(responderIPEndPoint, New AlpacaDevice(responderIPEndPoint, TRYING_TO_CONTACT_MANAGEMENT_API_MESSAGE))
                    RaiseAnAlpacaDevicesChangedEvent() ' Device was added so set the changed flag
                End If
            End SyncLock


            ' Create a task to query this device's DNS name, if configured to do so
            If tryDnsNameResolution Then
                LogMessage("BroadcastResponseEventHandler", $"Creating task to retrieve DNS information for device {responderIPEndPoint.ToString()}:{responderIPEndPoint.Port}")
                Dim dnsResolutionThread As Thread = New Thread(AddressOf ResolveIpAddressToHostName)
                dnsResolutionThread.IsBackground = True
                dnsResolutionThread.Start(responderIPEndPoint)
            End If


            ' Create a task to query this device's Alpaca management API
            LogMessage("BroadcastResponseEventHandler", $"Creating thread to retrieve Alpaca management description for device {responderIPEndPoint.ToString()}:{responderIPEndPoint.Port}")
            Dim descriptionThread As Thread = New Thread(AddressOf GetAlpacaDeviceInformation)
            descriptionThread.IsBackground = True
            descriptionThread.Start(responderIPEndPoint)
        Catch ex As Exception
            LogMessage("BroadcastResponseEventHandler", $"AddresssFound Exception: {ex.ToString()}")
        End Try
    End Sub


    ''' <summary>
    ''' Get Alpaca device information from the management API
    ''' </summary>
    ''' <paramname="deviceIpEndPoint"></param>
    Private Sub GetAlpacaDeviceInformation(ByVal deviceIpEndPointObject As Object)
        Dim deviceIpEndPoint As IPEndPoint = TryCast(deviceIpEndPointObject, IPEndPoint)
        Dim hostIpAndPort As String = deviceIpEndPoint.ToString()

        Try
            LogMessage("GetAlpacaDeviceInformation", $"dISCOVER TIMEOUT: {discoveryTime} ({discoveryTime * 1000})")


            ' Wait for API version result and process it
            Using apiClient As WebClientWithTimeOut = New WebClientWithTimeOut()
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
                    alpacaDeviceList(deviceIpEndPoint).AlpacaDeviceDescription = deviceDescriptionResponse.Value
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
                End SyncLock

                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End Using

            LogMessage("GetAlpacaDeviceInformation", $"COMPLETED API tasks for {hostIpAndPort}")
        Catch ex As Exception
            ' Something went wrong so log the issue and sent a message to the user
            LogMessage("GetAlpacaDeviceInformation", $"GetAlpacaDescriptions exception: 
{ex.ToString()}")

            SyncLock deviceListLockObject ' Make sure that only one thread can update the device list dictionary at a time
                alpacaDeviceList(deviceIpEndPoint).StatusMessage = ex.Message
                RaiseAnAlpacaDevicesChangedEvent() ' Device list was changed so set the changed flag
            End SyncLock
        End Try
    End Sub


    ''' <summary>
    ''' Resolve a host IP address to a host name
    ''' </summary>
    ''' <paramname="hostIp"></param>
    ''' <paramname="HostPort"></param>
    ''' <returns></returns>
    ''' <remarks>This first makes a DNS query and uses the result if found. If not found it then tries a Microsoft DNS call which also searches the local hosts and makes a netbios query.
    ''' If this returns an answer it is use. Otherwise the IP address is returned as the host name</remarks>
    Private Sub ResolveIpAddressToHostName(ByVal deviceIpEndPointObject As Object)
        Dim deviceIpEndPoint As IPEndPoint = TryCast(deviceIpEndPointObject, IPEndPoint) ' Get the supplied device endpoint as an IPEndPoint
        Dim dnsResponse As DnsResponse = New DnsResponse() ' Create a new DnsResponse to hold and return the 

        ' Calculate the remaining time before this discovery needs to finish and only undertake DNS resolution if sufficient time remains
        Dim timeOutTime As TimeSpan = TimeSpan.FromSeconds(discoveryTime).Subtract(Date.Now - discoveryStartTime).Subtract(TimeSpan.FromSeconds(0.2))

        If timeOutTime.TotalSeconds > 0.2 Then
            LogMessage("ResolveIpAddressToHostName", $"Resolving IP address: {deviceIpEndPoint.Address.ToString()}, Timeout: {timeOutTime}")
            Dns.BeginGetHostEntry(deviceIpEndPoint.Address.ToString(), New AsyncCallback(AddressOf GetHostEntryCallback), dnsResponse)

            ' Wait here until the resolve completes and the callback calls .Set()
            Dim dnsWasResolved As Boolean = dnsResponse.CallComplete.WaitOne(timeOutTime) ' Wait for the remaining discovery time less a small amount
            If dnsWasResolved Then ' A response was received rather than timing out
                LogMessage("ResolveIpAddressToHostName", $"{deviceIpEndPoint.ToString()} has host name: {dnsResponse.HostName} IP address count: {dnsResponse.AddressList.Length} Alias count: {dnsResponse.Aliases.Length}")

                If dnsResponse.AddressList.Length > 0 Then

                    SyncLock deviceListLockObject
                        alpacaDeviceList(deviceIpEndPoint).HostName = dnsResponse.HostName
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
            Else
                LogMessage("ResolveIpAddressToHostName", $"***** DNS did not respond within timeout - unable to resolve IP address to host name *****")
            End If
        Else
            LogMessage("ResolveIpAddressToHostName", $"***** Insufficient time remains ({timeOutTime.TotalSeconds} seconds) to conduct a DNS query, ignoring request *****")
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
            LogMessage("GetHostEntryCallback", $"Exception: {ex.ToString()}") ' Log exceptions but don't throw them
        End Try
    End Sub


    ''' <summary>
    ''' Log a message to the screen, adding the current managed thread ID
    ''' </summary>
    ''' <paramname="methodName"></param>
    ''' <paramname="message"></param>
    Private Sub LogMessage(ByVal methodName As String, ByVal message As String)
        Dim indentSpaces As String = New [String](" "c, Thread.CurrentThread.ManagedThreadId * NUMBER_OF_THREAD_MESSAGE_INDENT_SPACES)
        TL?.LogMessageCrLf($"AlpacaDiscovery - {methodName}", $"{indentSpaces}{Thread.CurrentThread.ManagedThreadId} {message}")
    End Sub

    Private Function GetRequest(ByVal aURL As String, ByVal timeOut As Integer) As String
        Dim webClient As WebClientWithTimeOut = New WebClientWithTimeOut()
        webClient.Timeout = timeOut
        Dim s As String = CType(webClient.DownloadString(aURL), String)
        Return s
    End Function
#End Region
End Class
