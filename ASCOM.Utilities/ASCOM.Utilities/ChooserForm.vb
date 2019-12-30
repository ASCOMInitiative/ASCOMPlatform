
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading
Imports ASCOM.Utilities.Support

Friend Class ChooserForm
    Inherits Form

#Region "Constants"

    Private Const ALERT_MESSAGEBOX_TITLE As String = "ASCOM Chooser"
    Private Const PROPERTIES_TOOLTIP_DISPLAY_TIME As Integer = 5000 ' Time to display the Properties tooltip (milliseconds)
    Private Const FORM_LOAD_WARNING_MESSAGE_DELAY_TIME As Integer = 250 ' Delay time before any warning message is displayed on form load
    Private Const ALPACA_STATUS_BLINK_TIME As Integer = 100 ' Length of time the Alpaca status indicator spends in the on and off state (ms)
    Private Const TOOLTIP_PROPERTIES_TITLE As String = "Driver Setup"
    Private Const TOOLTIP_PROPERTIES_MESSAGE As String = "Check or change driver Properties (configuration)"
    Private Const TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE As String = "You must check driver configuration before first time use, please click the Properties... button." & vbCrLf & "The OK button will remain greyed out until this is done."
    Private Const CHOOSER_LIST_WIDTH_NEW_ALPACA As Integer = 600 ' Width of the Chooser list when new Alpaca devices are present
    Private Const ALPACA_PROGID_PREFIX As String = "ASCOM.AlpacaDynamic" ' Prefix applied to all COM drivers created to front Alpaca devices

    ' Persistence constants
    Private Const CONFIGRATION_SUBKEY As String = "Chooser\Configuration" ' Store configuration in a subkey under the Chooser key
    Private Const ALPACA_ENABLED As String = "Alpaca enabled" : Private Const ALPACA_ENABLED_DEFAULT As Boolean = False
    Private Const ALPACA_DISCOVERY_PORT As String = "Alpaca discovery port" : Private Const ALPACA_DISCOVERY_PORT_DEFAULT As Integer = 32227
    Private Const ALPACA_NUMBER_OF_BROADCASTS As String = "Alpaca number of broadcasts" : Private Const ALPACA_NUMBER_OF_BROADCASTS_DEFAULT As Integer = 2
    Private Const ALPACA_TIMEOUT As String = "Alpaca timeout" : Private Const ALPACA_TIMEOUT_DEFAULT As Double = 2.0
    Private Const ALPACA_DNS_RESOLUTION As String = "Alpaca DNS resolution" : Private Const ALPACA_DNS_RESOLUTION_DEFAULT As Boolean = False
    Private Const ALPACA_SHOW_DISCOVERED_DEVICES As String = "Show discovered Alpaca devices" : Private Const ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT As Boolean = False
    Private Const ALPACA_SHOW_DEVICE_DETAILS As String = "Show Alpaca device details" : Private Const ALPACA_SHOW_DEVICE_DETAILS_DEFAULT As Boolean = False
    Private Const ALPACA_CHOOSER_WIDTH As String = "Alpaca Chooser width" : Private Const ALPACA_CHOOSER_WIDTH_DEFAULT As Integer = 0

    ' Alpaca integration constants
    Private Const ALPACA_DYNAMIC_CLIENT_GENERATOR_RELATIVE_PATH As String = "ASCOM\Platform 6\Tools\CreateAlpacaClient"
    Private Const ALPACA_DYNAMIC_CLIENT_EXE_NAME As String = "ASCOM.CreateAlpacaClient.exe"
    Private Const DRIVER_PROGID_BASE As String = "ASCOM.AlpacaDynamic"

    ' Alpaca driver Profile store value names
    Private Const PROFILE_VALUE_NAME_UNIQUEID As String = "UniqueID" ' Prefix applied to all COM drivers created to front Alpaca devices
    Private Const PROFILE_VALUE_NAME_IP_ADDRESS As String = "IP Address"
    Private Const PROFILE_VALUE_NAME_PORT_NUMBER As String = "Port Number"
    Private Const PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER As String = "Remote Device Number"

#End Region

#Region "Variables"

    ' Chooser variables
    Private deviceTypeValue, selectedProgIdValue As String
    Private chooserList As Generic.SortedList(Of ChooserItem, String)
    Private driverIsCompatible As String = ""
    Private currentWarningTitle, currentWarningMesage As String
    Private alpacaDevices As Generic.List(Of AscomDevice) = New Generic.List(Of AscomDevice)()
    Private selectedChooserItem As ChooserItem
    Private WithEvents generatorProcess As Process
    Private driverGenerationComplete As Boolean

    ' Component variables
    Private TL As ITraceLoggerUtility
    Private chooserWarningToolTip As ToolTip
    Private chooserPropertiesToolTip As ToolTip
    Private alpacaStatusToolstripLabel As ToolStripLabel
    Private WithEvents initialMessageTimer As System.Windows.Forms.Timer
    Private WithEvents alpacaStatusIndicatorTimer As System.Windows.Forms.Timer

    ' Persistence variables
    Friend AlpacaEnabled As Boolean
    Friend AlpacaDiscoveryPort As Integer
    Friend AlpacaNumberOfBroadcasts As Integer
    Friend AlpacaTimeout As Double
    Friend AlpacaDnsResolution As Boolean
    Friend AlpacaShowDiscoveredDevices As Boolean
    Friend AlpacaShowDeviceDetails As Boolean
    Friend AlpacaChooserIncrementalWidth As Integer

    ' Delegates
    Private PopulateDriverComboBoxDelegate As MethodInvoker = AddressOf PopulateDriverComboBox ' Device list combo box delegate
    Private SetStateNoAlpacaDelegate As MethodInvoker = AddressOf SetStateNoAlpaca
    Private SetStateAlpacaDiscoveringDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscovering
    Private SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscoveryCompleteFoundDevices
    Private SetStateAlpacaDiscoveryCompleteNoDevicesDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscoveryCompleteNoDevices

    ' Chooser form control positions
    Private OriginalForm1Width As Integer
    Private OriginalBtnCancelPosition As Point
    Private OriginalBtnOKPosition As Point
    Private OriginalBtnPropertiesPosition As Point
    Private OriginalCmbDriverSelectorWidth As Integer
    Private OriginalLblAlpacaDiscoveryPosition As Integer
    Private OriginalAlpacaStatusPosition As Integer
    Private OriginalDividerLineWidth As Integer

#End Region

#Region "Form load, close, paint and dispose event handlers"

    Public Sub New()
        MyBase.New()
        InitializeComponent()

        ' Record initial control positions
        OriginalForm1Width = Me.Width
        OriginalBtnCancelPosition = BtnCancel.Location
        OriginalBtnOKPosition = BtnOK.Location
        OriginalBtnPropertiesPosition = BtnProperties.Location
        OriginalCmbDriverSelectorWidth = CmbDriverSelector.Width
        OriginalLblAlpacaDiscoveryPosition = LblAlpacaDiscovery.Left
        OriginalAlpacaStatusPosition = AlpacaStatus.Left
        OriginalDividerLineWidth = DividerLine.Width

        ReadState() ' Read in the state variables from persisted storage
        ResizeChooser()

        'Create a trace logger
        TL = New TraceLogger("", "ChooserForm")
        TL.IdentifierWidth = 50
        TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT) ' Enable the trace logger if Util trace is enabled

    End Sub

    Private Sub ResizeChooser()
        ' Position controls if the Chooser has an increased width
        Me.Width = OriginalForm1Width + AlpacaChooserIncrementalWidth
        BtnCancel.Location = New Point(OriginalBtnCancelPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnCancelPosition.Y)
        BtnOK.Location = New Point(OriginalBtnOKPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnOKPosition.Y)
        BtnProperties.Location = New Point(OriginalBtnPropertiesPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnPropertiesPosition.Y)
        CmbDriverSelector.Width = OriginalCmbDriverSelectorWidth + AlpacaChooserIncrementalWidth
        LblAlpacaDiscovery.Left = OriginalLblAlpacaDiscoveryPosition + AlpacaChooserIncrementalWidth
        AlpacaStatus.Left = OriginalAlpacaStatusPosition + AlpacaChooserIncrementalWidth
        DividerLine.Width = OriginalDividerLineWidth + AlpacaChooserIncrementalWidth

    End Sub

    Private Sub ChooserForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Try

            ' Initialise form title and message text
            Text = "ASCOM " & deviceTypeValue & " Chooser"
            lblTitle.Text = "Select the type of " & LCase(deviceTypeValue) & " you have, then be " & "sure to click the Properties... button to configure the driver for your " & LCase(deviceTypeValue) & "."
            'selectedProgIdValue = ""

            'Initialise the tooltip warning for 32/64bit driver compatibility messages
            chooserWarningToolTip = New ToolTip()

            CmbDriverSelector.DropDownWidth = CHOOSER_LIST_WIDTH_NEW_ALPACA

            ' Configure the Properties button tooltip
            chooserPropertiesToolTip = New ToolTip()
            chooserPropertiesToolTip.IsBalloon = True
            chooserPropertiesToolTip.ToolTipIcon = ToolTipIcon.Info
            chooserPropertiesToolTip.UseFading = True
            chooserPropertiesToolTip.ToolTipTitle = TOOLTIP_PROPERTIES_TITLE
            chooserPropertiesToolTip.SetToolTip(BtnProperties, TOOLTIP_PROPERTIES_MESSAGE)

            ' Set a custom rendered for the tool strip so that colours and appearance can be controlled better
            ChooserMenu.Renderer = New ChooserCustomToolStripRenderer()

            ' Create a tool strip label whose background colour can  be changed and add it at the top of the Alpaca menu
            alpacaStatusToolstripLabel = New ToolStripLabel("Discovery status unknown")
            MnuAlpaca.DropDownItems.Insert(0, alpacaStatusToolstripLabel)

            RefreshTraceMenu() ' Refresh the trace menu

            ' Set up the Alpaca status blink timer but make sure its not running
            alpacaStatusIndicatorTimer = New System.Windows.Forms.Timer
            alpacaStatusIndicatorTimer.Interval = ALPACA_STATUS_BLINK_TIME ' Set it to fire after 250ms
            alpacaStatusIndicatorTimer.Stop()

            TL.LogMessage("ChooserForm_Load", $"UI thread: {Thread.CurrentThread.ManagedThreadId}")

            InitialiseComboBox() '' Kick off a discover and populate the combo box or just populate the combo box if no discovery is required

        Catch ex As Exception
            MsgBox("ChooserForm Load " & ex.ToString)
            LogEvent("ChooserForm Load ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
        End Try
    End Sub

    Private Sub InitialiseComboBox()
        alpacaDevices = New List(Of AscomDevice) ' Initialise to a clear list with no Alpaca devices

        If AlpacaEnabled Then
            Dim discoveryThread As Thread = New Thread(AddressOf DiscoverAlpacaDevicesAndPopulateDriverComboBox)
            discoveryThread.Start()
        Else
            PopulateDriverComboBox()
            SetStateNoAlpaca()

            ' Set up a one-off timer in order to force display of the warning message if the pre-selected driver is not compatible
            initialMessageTimer = New System.Windows.Forms.Timer
            initialMessageTimer.Interval = FORM_LOAD_WARNING_MESSAGE_DELAY_TIME ' Set it to fire after 250ms
            initialMessageTimer.Start() ' Kick off the timer
        End If

    End Sub


    Private Sub ChooserForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'Clean up the trace logger
        TL.Enabled = False
    End Sub

    ''' <summary>
    ''' Dispose of disposable components
    ''' </summary>
    ''' <param name="Disposing"></param>
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
            If Not TL Is Nothing Then
                Try : TL.Dispose() : Catch : End Try
            End If
            If Not chooserWarningToolTip Is Nothing Then
                Try : chooserWarningToolTip.Dispose() : Catch : End Try
            End If
            If Not chooserPropertiesToolTip Is Nothing Then
                Try : chooserPropertiesToolTip.Dispose() : Catch : End Try
            End If
            If Not alpacaStatusToolstripLabel Is Nothing Then
                Try : alpacaStatusToolstripLabel.Dispose() : Catch : End Try
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub

#End Region

#Region "Public methods"

    Public WriteOnly Property DeviceType() As String
        Set(ByVal Value As String)
            deviceTypeValue = Value
            TL.LogMessage("DeviceType Set", deviceTypeValue)
            ReadState(deviceTypeValue)
        End Set
    End Property

    Public Property SelectedProgId() As String
        Get
            Return selectedProgIdValue
        End Get
        Set(ByVal Value As String)
            selectedProgIdValue = Value
            TL.LogMessage("InitiallySelectedProgId Set", selectedProgIdValue)
        End Set
    End Property

#End Region

#Region "Form and timer event handlers"

    Private Sub ChooserFormMoveEventHandler(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        If currentWarningMesage <> "" Then WarningToolTipShow(currentWarningTitle, currentWarningMesage)
    End Sub

    Private Sub FormLoadTimerEventHandler(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles initialMessageTimer.Tick
        ' This event kicks off once triggered by form load in order to force display of the warning message for a driver that is pre-selected by the user.
        initialMessageTimer.Stop() ' Disable the timer to prevent future events from firing
        initialMessageTimer.Enabled = False
        TL.LogMessageCrLf("ChooserForm Timer", "Displaying warning message, if there is one")
        cbDriverSelector_SelectedIndexChanged(CmbDriverSelector, New System.EventArgs()) ' Force display of the  warning tooltip because it does not show up when displayed during FORM load
    End Sub

    Private Sub AlpacaStatusIndicatorTimerEventHandler(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles alpacaStatusIndicatorTimer.Tick
        If AlpacaStatus.BackColor = Color.Orange Then
            AlpacaStatus.BackColor = Color.DimGray
        Else
            AlpacaStatus.BackColor = Color.Orange
        End If

    End Sub
#End Region

#Region "UI event handlers"

    ' Click in Properties... button. Loads the currently selected driver and activate its setup dialogue.
    Private Sub cmdProperties_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnProperties.Click
        Dim ProfileStore As RegistryAccess
        Dim oDrv As Object = Nothing ' The driver
        Dim cb As System.Windows.Forms.ComboBox
        Dim bConnected As Boolean
        Dim sProgID As String
        Dim ProgIdType As Type
        Dim UseCreateObject As Boolean = False

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        cb = CmbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        sProgID = CType(CmbDriverSelector.SelectedItem, Generic.KeyValuePair(Of ChooserItem, String)).Key.ProgID

        TL.LogMessage("PropertiesClick", "ProgID:" & sProgID)
        Try
            ' Mechanic to revert to Platform 5 behaviour in the event that Activator.CreateInstance has unforeseen consequences
            Try : UseCreateObject = RegistryCommonCode.GetBool(CHOOSER_USE_CREATEOBJECT, CHOOSER_USE_CREATEOBJECT_DEFAULT) : Catch : End Try

            If UseCreateObject Then ' Platform 5 behaviour
                LogEvent("ChooserForm", "Using CreateObject for driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Information, EventLogErrors.ChooserSetupFailed, "")
                oDrv = CreateObject(sProgID) ' Rob suggests that Activator.CreateInstance gives better error diagnostics
            Else ' New Platform 6 behaviour
                ProgIdType = Type.GetTypeFromProgID(sProgID)
                oDrv = Activator.CreateInstance(ProgIdType)
            End If

            ' Here we try to see if a device is already connected. If so, alert and just turn on the OK button.
            bConnected = False
            Try
                bConnected = CBool(oDrv.Connected)
            Catch
                Try : bConnected = CBool(oDrv.Link) : Catch : End Try
            End Try

            If bConnected Then
                MsgBox("The device is already connected. Just click OK.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            Else
                Try
                    WarningTooltipClear() ' Clear warning tool tip before entering setup so that the dialogue doesn't interfere with or obscure the setup dialogue.
                    oDrv.SetupDialog()
                Catch ex As Exception
                    MsgBox("Driver setup method failed: """ & sProgID & """ " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
                    LogEvent("ChooserForm", "Driver setup method failed for driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserSetupFailed, ex.ToString)
                End Try
            End If

            ProfileStore.WriteProfile("Chooser", sProgID & " Init", "True") ' Remember it has been initialized
            BtnOK.Enabled = True
            WarningTooltipClear()
        Catch ex As Exception
            MsgBox("Failed to load driver: """ & sProgID & """ " & ex.ToString, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            LogEvent("ChooserForm", "Failed to load driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserDriverFailed, ex.ToString)
        End Try

        'Clean up and release resources
        Try : oDrv.Dispose() : Catch ex As Exception : End Try
        Try : Marshal.ReleaseComObject(oDrv) : Catch ex As Exception : End Try

        ProfileStore.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnCancel.Click
        selectedProgIdValue = ""
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnOK.Click
        Dim generatorWorkingDirectory, generatorExeFile, newProgId As String
        Dim generatorProcessStartInfo As ProcessStartInfo

        If selectedChooserItem.IsComDriver Then ' USer has selected an existing COM driver so return its ProgID
            selectedProgIdValue = selectedChooserItem.ProgID

            TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'")

            Me.Hide()
        Else ' USer has selected a new Alpaca device so we need to create a new COM driver for this
            MsgBox("After pressing OK, you will be asked for Administrator approval to register this new Alpaca device.", MsgBoxStyle.OkCancel, "New Alpaca Device Selected")

            ' Construct path to the executable that will dynamically create a new ALpaca COM client
            generatorWorkingDirectory = $"{Environment.GetEnvironmentVariable("ProgramFiles(x86)")}\{ALPACA_DYNAMIC_CLIENT_GENERATOR_RELATIVE_PATH}"
            generatorExeFile = $"{generatorWorkingDirectory}\{ALPACA_DYNAMIC_CLIENT_EXE_NAME}"
            TL.LogMessage("OK Click", $"Creating driver for ProgID: {selectedProgIdValue} using the {generatorExeFile} executable in working directory {generatorWorkingDirectory}")

            If Not File.Exists(generatorExeFile) Then
                MsgBox("The client generator executable can not be found, please repair the ASCOM Platform.", MsgBoxStyle.Critical, "ALpaca Client Generator Not Found")
                TL.LogMessage("OK Click", $"ERROR - Unable to find the client generator executable at {generatorExeFile}, cannot create a new Alpaca client.")
                selectedProgIdValue = ""
            End If

            Dim deviceNumber As Integer = 0
            Dim typeFromProgId As Type

            ' Find an unused number in the ProgID series ASCOM.Alpaca{N}.{DeviceType} where N Is an integer starting at 1
            Do

                deviceNumber += 1
                newProgId = $"{DRIVER_PROGID_BASE}{deviceNumber}.{deviceTypeValue}"
                typeFromProgId = Type.GetTypeFromProgID(newProgId)

            Loop While (Not (typeFromProgId Is Nothing))

            TL.LogMessage("CreateAlpacaClient", $"Creating new ProgID: {newProgId}")

            generatorProcessStartInfo = New ProcessStartInfo(generatorExeFile, $"\CreateAlpacaClient {deviceTypeValue} {deviceNumber} {newProgId} ""{selectedChooserItem.Name}""")
            generatorProcessStartInfo.WorkingDirectory = generatorWorkingDirectory


            generatorProcess = New Process()
            generatorProcess.StartInfo = generatorProcessStartInfo
            generatorProcess.EnableRaisingEvents = True

            driverGenerationComplete = False

            TL.LogMessage("CreateAlpacaClient", $"Starting driver generation process")
            'generatorProcess.WaitForExit(10000) ' Wait for up to 5 seconds for the process to complete
            generatorProcess.Start()

            Do
                Thread.Sleep(10)
                Application.DoEvents()
            Loop While Not driverGenerationComplete

            TL.LogMessage("CreateAlpacaClient", $"Completed driver generation process")

            generatorProcess.Dispose()

            ' Configure the IP address, port number and Alpaca device number in the newly registered driver
            Dim p As Profile = New Profile()
            p.DeviceType = $"{deviceTypeValue.Substring(0, 1).ToUpperInvariant()}{deviceTypeValue.Substring(1).ToLowerInvariant()}"
            p.WriteValue(newProgId, PROFILE_VALUE_NAME_IP_ADDRESS, selectedChooserItem.HostName)
            p.WriteValue(newProgId, PROFILE_VALUE_NAME_PORT_NUMBER, selectedChooserItem.Port.ToString())
            p.WriteValue(newProgId, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER, selectedChooserItem.DeviceNumber.ToString())
            p.WriteValue(newProgId, PROFILE_VALUE_NAME_UNIQUEID, selectedChooserItem.DeviceUniqueID.ToString())
            p.Dispose()

            Dim r As New RegistryAccess

            r.WriteProfile("Chooser", $"{newProgId} Init", "True")
            r.Dispose()
            selectedProgIdValue = newProgId
            PopulateDriverComboBox()
        End If

        TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'")

        'Me.Hide()
    End Sub

    ''' <summary>
    ''' Driver generation completion event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DriverGeneration_Complete(ByVal sender As Object, ByVal e As System.EventArgs) Handles generatorProcess.Exited
        driverGenerationComplete = True ' Flag that driver generation is complete
    End Sub

    Private Sub cbDriverSelector_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmbDriverSelector.SelectionChangeCommitted
        If CmbDriverSelector.SelectedIndex >= 0 Then

            ' Save the newly selected chooser item
            selectedChooserItem = CType(CmbDriverSelector.SelectedItem, Generic.KeyValuePair(Of ChooserItem, String)).Key

            ' Validate the driver if it is a COM driver
            If selectedChooserItem.IsComDriver Then ' This is a COM driver
                TL.LogMessage("SelectedIndexChanged", $"New COM driver selected. ProgID: {selectedChooserItem.ProgID} {selectedChooserItem.Name}")
                ValidateDriver(selectedChooserItem.ProgID)
            Else ' This is a new Alpaca driver
                TL.LogMessage("SelectedIndexChanged", $"New Alpaca driver selected : {selectedChooserItem.Name}")
                BtnProperties.Enabled = False ' Disable the Properties button because there is not yet a COM driver to configure
            End If

        Else ' Selected index is negative
            TL.LogMessage("SelectedIndexChanged", $"Ignoring index changed event because no item is selected: {CmbDriverSelector.SelectedIndex}")
        End If
    End Sub

    Private Sub picASCOM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picASCOM.Click
        Try
            Process.Start("https://ASCOM-Standards.org/")
        Catch ex As Exception
            MsgBox("Unable to display ASCOM-Standards web site in your browser: " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
        End Try
    End Sub

#End Region

#Region "Menu code and event handlers"

    Private Sub RefreshTraceMenu()
        Dim TraceFileName As String ', ProfileStore As RegistryAccess

        Using ProfileStore As New RegistryAccess

            TraceFileName = ProfileStore.GetProfile("", SERIAL_FILE_NAME_VARNAME)
            Select Case TraceFileName
                Case "" 'Trace is disabled
                    MenuSerialTraceEnabled.Checked = False 'The trace enabled flag is unchecked and disabled
                    MenuSerialTraceEnabled.Enabled = True
                Case SERIAL_AUTO_FILENAME 'Tracing is on using an automatic filename
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
                Case Else 'Tracing using some other fixed filename
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
            End Select

            'Set Profile trace checked state on menu item 
            MenuProfileTraceEnabled.Checked = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT)
            MenuRegistryTraceEnabled.Checked = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT)
            MenuUtilTraceEnabled.Checked = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)
            MenuTransformTraceEnabled.Checked = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT)
            MenuSimulatorTraceEnabled.Checked = GetBool(SIMULATOR_TRACE, SIMULATOR_TRACE_DEFAULT)
            MenuDriverAccessTraceEnabled.Checked = GetBool(DRIVERACCESS_TRACE, DRIVERACCESS_TRACE_DEFAULT)
            MenuAstroUtilsTraceEnabled.Checked = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT)
            MenuNovasTraceEnabled.Checked = GetBool(NOVAS_TRACE, NOVAS_TRACE_DEFAULT)
            MenuCacheTraceEnabled.Checked = GetBool(TRACE_CACHE, TRACE_CACHE_DEFAULT)
            MenuEarthRotationDataFormTraceEnabled.Checked = GetBool(TRACE_EARTHROTATION_DATA_FORM, TRACE_EARTHROTATION_DATA_FORM_DEFAULT)

        End Using
    End Sub

    Private Sub MenuAutoTraceFilenames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        'Auto filenames currently disabled, so enable them
        MenuSerialTraceEnabled.Enabled = True 'Set the trace enabled flag
        MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
        ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        ProfileStore.Dispose()
    End Sub

    Private Sub MenuSerialTraceFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ProfileStore As RegistryAccess
        Dim RetVal As System.Windows.Forms.DialogResult

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        RetVal = SerialTraceFileName.ShowDialog()
        Select Case RetVal
            Case Windows.Forms.DialogResult.OK
                'Save the result
                ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName)
                'Check and enable the serial trace enabled flag
                MenuSerialTraceEnabled.Enabled = True
                MenuSerialTraceEnabled.Checked = True
                'Enable manual serial trace file flag
            Case Else 'Ignore everything else

        End Select
        ProfileStore.Dispose()
    End Sub

    Private Sub MenuSerialTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSerialTraceEnabled.Click
        Dim ProfileStore As RegistryAccess

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        If MenuSerialTraceEnabled.Checked Then ' Auto serial trace is on so turn it off
            MenuSerialTraceEnabled.Checked = False
            ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "")
        Else ' Auto serial trace is off so turn it on
            MenuSerialTraceEnabled.Checked = True
            ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        End If
        ProfileStore.Dispose()
    End Sub

    Private Sub MenuProfileTraceEnabled_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuProfileTraceEnabled.Click
        MenuProfileTraceEnabled.Checked = Not MenuProfileTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_PROFILE, MenuProfileTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuRegistryTraceEnabled_Click(sender As Object, e As EventArgs) Handles MenuRegistryTraceEnabled.Click
        MenuRegistryTraceEnabled.Checked = Not MenuRegistryTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_XMLACCESS, MenuRegistryTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuUtilTraceEnabled_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUtilTraceEnabled.Click
        MenuUtilTraceEnabled.Checked = Not MenuUtilTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_UTIL, MenuUtilTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuTransformTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransformTraceEnabled.Click
        MenuTransformTraceEnabled.Checked = Not MenuTransformTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_TRANSFORM, MenuTransformTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuIncludeSerialTraceDebugInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'MenuIncludeSerialTraceDebugInformation.Checked = Not MenuIncludeSerialTraceDebugInformation.Checked 'Invert selection
        'SetName(SERIAL_TRACE_DEBUG, MenuIncludeSerialTraceDebugInformation.Checked.ToString)
    End Sub

    Private Sub MenuSimulatorTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSimulatorTraceEnabled.Click
        MenuSimulatorTraceEnabled.Checked = Not MenuSimulatorTraceEnabled.Checked 'Invert selection
        SetName(SIMULATOR_TRACE, MenuSimulatorTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuCacheTraceEnabled_Click(sender As Object, e As EventArgs) Handles MenuCacheTraceEnabled.Click
        MenuCacheTraceEnabled.Checked = Not MenuCacheTraceEnabled.Checked 'Invert selection
        SetName(TRACE_CACHE, MenuCacheTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuEarthRotationDataTraceEnabled_Click(sender As Object, e As EventArgs) Handles MenuEarthRotationDataFormTraceEnabled.Click
        MenuEarthRotationDataFormTraceEnabled.Checked = Not MenuEarthRotationDataFormTraceEnabled.Checked 'Invert selection
        SetName(TRACE_EARTHROTATION_DATA_FORM, MenuEarthRotationDataFormTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuTrace_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MnuTrace.DropDownOpening
        RefreshTraceMenu()
    End Sub

    Private Sub MenuDriverAccessTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDriverAccessTraceEnabled.Click
        MenuDriverAccessTraceEnabled.Checked = Not MenuDriverAccessTraceEnabled.Checked 'Invert selection
        SetName(DRIVERACCESS_TRACE, MenuDriverAccessTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuAstroUtilsTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAstroUtilsTraceEnabled.Click
        MenuAstroUtilsTraceEnabled.Checked = Not MenuAstroUtilsTraceEnabled.Checked 'Invert selection
        SetName(ASTROUTILS_TRACE, MenuAstroUtilsTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuNovasTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNovasTraceEnabled.Click
        MenuNovasTraceEnabled.Checked = Not MenuNovasTraceEnabled.Checked 'Invert selection
        SetName(NOVAS_TRACE, MenuNovasTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MnuEnableDiscovery_Click(sender As Object, e As EventArgs) Handles MnuEnableDiscovery.Click
        AlpacaEnabled = True
        WriteState(deviceTypeValue)
        InitialiseComboBox()
    End Sub

    Private Sub MnuDisableDiscovery_Click(sender As Object, e As EventArgs) Handles MnuDisableDiscovery.Click
        AlpacaEnabled = False
        WriteState(deviceTypeValue)
        InitialiseComboBox()
        SetStateNoAlpaca()
    End Sub

    Private Sub MnuDiscoverNow_Click(sender As Object, e As EventArgs) Handles MnuDiscoverNow.Click
        InitialiseComboBox()
    End Sub

    Private Sub MnuConfigureDiscovery_Click(sender As Object, e As EventArgs) Handles MnuConfigureChooser.Click
        Dim alpacaConfigurationForm As ChooserAlpacaConfigurationForm
        Dim outcome As DialogResult

        TL.LogMessage("ConfigureDiscovery", $"About to create Alpaca configuration form")
        alpacaConfigurationForm = New ChooserAlpacaConfigurationForm(Me) ' Create a new configuration form
        alpacaConfigurationForm.ShowDialog() ' Display the form as a modal dialogue box
        TL.LogMessage("ConfigureDiscovery", $"Exited Alpaca configuration form. Result: {alpacaConfigurationForm.DialogResult.ToString()}")

        If alpacaConfigurationForm.DialogResult = DialogResult.OK Then ' If the user clicked OK then persist the new state
            TL.LogMessage("ConfigureDiscovery", $"Persisting new configuration for {deviceTypeValue}")
            WriteState(deviceTypeValue)

            ResizeChooser() ' Resize the chooser to reflect any configuration change

            InitialiseComboBox() '' Kick off a discover and populate the combo box or just populate the combo box if no discovery is required

        End If

        alpacaConfigurationForm.Dispose() ' Dispose of the configuration form

    End Sub

#End Region

#Region "State Persistence"

    Private Overloads Sub ReadState()
        ReadState("Telescope")
    End Sub


    Private Overloads Sub ReadState(DeviceType As String)
        Dim registry As RegistryAccess

        Try
            TL?.LogMessageCrLf("ChooserReadState", $"Reading state for device type: {DeviceType}. Configuration key: {CONFIGRATION_SUBKEY}, Alpaca enabled: {$"{DeviceType} {ALPACA_ENABLED}"}, ALapca default: {ALPACA_ENABLED_DEFAULT}")
            registry = New RegistryAccess

            ' The enabled state is per device type
            AlpacaEnabled = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", ALPACA_ENABLED_DEFAULT.ToString()), CultureInfo.InvariantCulture)

            ' These values are for all Alpaca devices
            AlpacaDiscoveryPort = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, ALPACA_DISCOVERY_PORT_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaNumberOfBroadcasts = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, ALPACA_NUMBER_OF_BROADCASTS_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaTimeout = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, ALPACA_TIMEOUT_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaDnsResolution = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, ALPACA_DNS_RESOLUTION_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaShowDeviceDetails = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, ALPACA_SHOW_DEVICE_DETAILS_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaShowDiscoveredDevices = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaChooserIncrementalWidth = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, ALPACA_CHOOSER_WIDTH_DEFAULT.ToString()), CultureInfo.InvariantCulture)

        Catch ex As Exception
            MsgBox("Chooser Read State " & ex.ToString)
            LogEvent("Chooser Read State ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
            TL?.LogMessageCrLf("ChooserReadState", ex.ToString())
        Finally
            registry.Dispose()
        End Try
    End Sub

    Private Sub WriteState(DeviceType As String)
        Dim registry As RegistryAccess

        Try
            registry = New RegistryAccess

            ' Save the enabled state per "device type" 
            registry.WriteProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", AlpacaEnabled.ToString(CultureInfo.InvariantCulture))

            ' Save other states for all Alpaca devices 
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, AlpacaDiscoveryPort.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, AlpacaNumberOfBroadcasts.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, AlpacaTimeout.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, AlpacaDnsResolution.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, AlpacaShowDeviceDetails.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, AlpacaShowDiscoveredDevices.ToString(CultureInfo.InvariantCulture))
            registry.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, AlpacaChooserIncrementalWidth.ToString(CultureInfo.InvariantCulture))

        Catch ex As Exception
            MsgBox("Chooser Write State " & ex.ToString)
            LogEvent("Chooser Write State ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
            TL?.LogMessageCrLf("ChooserWriteState", ex.ToString())
        Finally
            registry.Dispose()
        End Try

    End Sub

#End Region

#Region "Support code"

    Private Sub DiscoverAlpacaDevicesAndPopulateDriverComboBox()

        Try

            TL.LogMessage("StartAlpacaDiscovery", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

            ' Render the user interface unresponsive while discovery is underway, except for the Cancel button.
            SetStateAlpacaDiscovering()

            Dim discovery As AlpacaDiscovery
            discovery = New AlpacaDiscovery(TL)
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery created")
            discovery.StartDiscovery(AlpacaNumberOfBroadcasts, 200, AlpacaDiscoveryPort, AlpacaTimeout, AlpacaDnsResolution)
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery started")

            ' Keep the UI alive while the discovery is running
            Do

                Threading.Thread.Sleep(10)
                Application.DoEvents()

            Loop Until discovery.DiscoveryComplete
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery Completed")

            ' List discovered devices to the log
            For Each ascomDevice As AscomDevice In discovery.GetAscomDevices()
                TL.LogMessage("StartAlpacaDiscovery", $"FOUND {ascomDevice.AscomDeviceType} {ascomDevice.AscomDeviceName} {ascomDevice.IPEndPoint.ToString()}")
            Next

            ' Get discovered devices of the requested ASCOM device type
            alpacaDevices = discovery.GetAscomDevices(deviceTypeValue)

            ' Populate the device list combo box with COM and Alpaca devices
            PopulateDriverComboBox()

            discovery.Dispose()
        Catch ex As Exception
            TL.LogMessageCrLf("StartAlpacaDiscovery", ex.ToString())
        Finally
            ' Restore a usable user interface
            If alpacaDevices.Count > 0 Then
                SetStateAlpacaDiscoveryCompleteFoundDevices()
            Else
                SetStateAlpacaDiscoveryCompleteNoDevices()
            End If

        End Try
    End Sub

    Private Sub PopulateDriverComboBox()
        Dim profileStore As RegistryAccess
        Dim i As Integer
        Dim description As String
        chooserList = New SortedList(Of ChooserItem, String)

        If CmbDriverSelector.InvokeRequired Then ' We are not running on the Ui thread
            TL.LogMessage("PopulateDriverComboBox", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(PopulateDriverComboBoxDelegate)
        Else ' We are running on the UI thread
            Try
                TL.LogMessage("PopulateDriverComboBox", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

                profileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

                ' Enumerate the available drivers, and load their descriptions and ProgIDs into the driversList generic sorted list collection. Key is ProgID, value is friendly name.
                Try
                    ' Get Key-Class pairs in the subkey "{DeviceType} Drivers" e.g. "Telescope Drivers"
                    Dim driverList As SortedList(Of String, String) = profileStore.EnumKeys(deviceTypeValue & " Drivers")
                    TL.LogMessage("PopulateDriverComboBox", $"Returned {driverList.Count} COM drivers")

                    For Each driver As KeyValuePair(Of String, String) In driverList
                        Dim driverProgId, driverName As String

                        driverProgId = driver.Key
                        driverName = driver.Value

                        TL.LogMessage("PopulateDriverComboBox", $"Found ProgID: {driverProgId} , Description: @{driverName}@")

                        If String.IsNullOrEmpty(driverName) Then ' Description Is missing
                            TL.LogMessage("PopulateDriverComboBox", $"  ***** Description missing for ProgID: {driverProgId}")
                        End If

                        ' If Alpaca discovery is enabled, annotate these entries as being either real COM or Alpaca devices
                        If AlpacaEnabled Then
                            If (driverProgId.ToLowerInvariant().StartsWith(ALPACA_PROGID_PREFIX.ToLowerInvariant())) Then ' This is a COM driver for an Alpaca device
                                If AlpacaShowDeviceDetails Then ' Get device details from the Profile and display these

                                    Dim p As New Profile
                                    Dim r As New RegistryAccess
                                    p.DeviceType = deviceTypeValue
                                    Dim ascomProfile As ASCOMProfile = p.GetProfile(driverProgId)
                                    driverName = $"{driverName}    ({driverProgId} ==> {ascomProfile.GetValue(PROFILE_VALUE_NAME_IP_ADDRESS)}:{ascomProfile.GetValue(PROFILE_VALUE_NAME_PORT_NUMBER)}/api/v1/{deviceTypeValue}/{ascomProfile.GetValue(PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER)})" ' Annotate as COM to differentiate from Alpaca drivers
                                    p.Dispose()
                                Else ' Just annotate as an Alpaca device
                                    driverName = $"{driverName}    (Alpaca)" ' Annotate as an Alpaca device
                                End If
                            End If
                        Else ' This is not an ALpaca device so annotate it accordingly
                            If AlpacaShowDeviceDetails Then ' Get device details from the Profile and display these
                                driverName = $"{driverName}    ({driverProgId})" ' Annotate with ProgID
                            Else
                                ' No action just use the driver description as is
                            End If

                        End If
                        Dim c As ChooserItem
                        c = New ChooserItem(driverProgId, driverName)
                        chooserList.Add(c, driverName)
                    Next

                Catch ex1 As Exception
                    TL.LogMessageCrLf("PopulateDriverComboBox", "Exception: " & ex1.ToString)
                    'Ignore any exceptions from this call e.g. if there are no devices of that type installed just create an empty list
                End Try
                TL.LogMessage("PopulateDriverComboBox", $"Completed driver enumeration")

                ' Populate the driver selection combo box with driver friendly names
                If (chooserList.Count = 0) And (alpacaDevices.Count = 0) Then ' No drivers to add to the combo box 
                    MsgBox("There are no ASCOM " & deviceTypeValue & " drivers installed.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
                Else ' There are some drivers in or to add to the combo box

                    ' Add any Alpaca devices to the list
                    For Each device As AscomDevice In alpacaDevices
                        TL.LogMessage("PopulateDriverComboBox", $"Discovered Alpaca device: {device.AscomDeviceType} {device.AscomDeviceName} {device.AscomDeviceUniqueId} at  http://{device.HostName}:{device.IPEndPoint.Port.ToString()}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber}")

                        Dim displayHostName As String = CType(IIf(device.HostName = device.IPEndPoint.Address.ToString(), device.IPEndPoint.Address.ToString(), $"{device.HostName} ({device.IPEndPoint.Address.ToString()})"), String)
                        Dim displayName As String = $"* NEW ALPACA DEVICE   {device.AscomDeviceName}   {displayHostName} : { device.IPEndPoint.Port.ToString()} - {deviceTypeValue}/{device.AlpacaDeviceNumber} - {device.AscomDeviceUniqueId}"

                        ' Get a list of dynamic drivers already configured on the system

                        Dim p As New Profile
                        p.DeviceType = deviceTypeValue

                        Dim devicesArrayList As ArrayList = p.RegisteredDevices(deviceTypeValue) ' Get a list of all devices of the current device type

                        Dim foundDriver As Boolean = False

                        For Each arrayListDevice As KeyValuePair In devicesArrayList
                            If arrayListDevice.Key.ToLowerInvariant().StartsWith(ALPACA_PROGID_PREFIX.ToLowerInvariant()) Then 'This is a dynamic Alpaca COM driver

                                ' Get the device values to compare with the discovered device
                                Dim deviceUniqueId As String = p.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_UNIQUEID)
                                Dim deviceHostName As String = p.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_IP_ADDRESS)
                                Dim deviceIPPort As Integer = Convert.ToInt32(p.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_PORT_NUMBER))
                                Dim deviceNumber As Integer = Convert.ToInt32(p.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER))

                                TL.LogMessage("PopulateDriverComboBox", $"Found existing COM dynamic driver for device {deviceUniqueId} at http://{deviceHostName}:{deviceIPPort}/api/v1/{deviceTypeValue}/{deviceNumber}")
                                TL.LogMessage("PopulateDriverComboBox", $"{device.AscomDeviceUniqueId} {deviceUniqueId} {device.AscomDeviceUniqueId = deviceUniqueId} {device.HostName = deviceHostName} {device.IPEndPoint.Port = deviceIPPort} {device.AlpacaDeviceNumber = deviceNumber}")

                                If (device.AscomDeviceUniqueId = deviceUniqueId) And (device.HostName = deviceHostName) And (device.IPEndPoint.Port = deviceIPPort) And (device.AlpacaDeviceNumber = deviceNumber) Then
                                    foundDriver = True
                                    TL.LogMessage("PopulateDriverComboBox", $"    Found existing COM driver match!")
                                End If
                            End If
                        Next

                        If foundDriver Then
                            TL.LogMessage("PopulateDriverComboBox", $"This device MATCHES an existing COM driver so NOT adding it to the Combo box list")
                        Else
                            TL.LogMessage("PopulateDriverComboBox", $"This device does NOT match an existing COM driver so ADDING it to the Combo box list")
                            chooserList.Add(New ChooserItem(device.AscomDeviceUniqueId, device.AlpacaDeviceNumber, device.HostName, device.IPEndPoint.Port, device.AscomDeviceName), displayName)
                        End If



                        p.Dispose()
                    Next
                End If
                CmbDriverSelector.DataSource = New BindingSource(chooserList, Nothing)
                CmbDriverSelector.ValueMember = "Key"
                CmbDriverSelector.DisplayMember = "Value"

                CmbDriverSelector.DropDownWidth = DropDownWidth(CmbDriverSelector) ' AutoSize the combo box width
                CmbDriverSelector.SelectedIndex = -1

                For Each driver As Generic.KeyValuePair(Of ChooserItem, String) In CmbDriverSelector.Items
                    TL.LogMessage("PopulateDriverComboBox", $"Searching for ProgID: {selectedProgIdValue}, found ProgID: {driver.Key.ProgID}")
                    If driver.Key.ProgID = selectedProgIdValue Then
                        TL.LogMessage("PopulateDriverComboBox", $"*** Found ProgID: {selectedProgIdValue}")
                        CmbDriverSelector.SelectedItem = driver
                        selectedChooserItem = driver.Key
                        BtnOK.Enabled = True ' Enable the OK button
                    End If
                Next

                TL.LogMessage("PopulateDriverComboBox", $"Selected ProgID is: {selectedProgIdValue}")

                ' Validate the selected driver if it is a COM driver
                If selectedChooserItem.IsComDriver Then ' This is a COM driver
                    ValidateDriver(selectedChooserItem.ProgID)
                Else ' This is a new Alpaca driver
                    BtnProperties.Enabled = False ' Disable the Properties button because there is not yet a COM driver to configure
                End If

                profileStore.Dispose() 'Close down the profile store
            Catch ex As Exception
                TL.LogMessageCrLf("PopulateDriverComboBox Top", "Exception: " & ex.ToString)

            End Try
        End If
    End Sub

    ''' <summary>
    ''' Return the maximum width of a combo box's drop-down items
    ''' </summary>
    ''' <param name="comboBox">Combo box to inspect</param>
    ''' <returns>Maximum width of supplied combo box drop-down items</returns>
    Private Function DropDownWidth(ByVal comboBox As ComboBox) As Integer
        Dim maxWidth As Integer
        Dim temp As Integer
        Dim label1 As Label = New Label()

        maxWidth = comboBox.Width ' Ensure that the minimum width is the width of the combo box

        For Each obj As Generic.KeyValuePair(Of ChooserItem, String) In comboBox.Items
            label1.Text = obj.Value
            temp = label1.PreferredWidth

            If temp > maxWidth Then
                maxWidth = temp
            End If
        Next

        label1.Dispose()

        Return maxWidth
    End Function

    Private Sub SetStateNoAlpaca()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateNoAlpaca", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(SetStateNoAlpacaDelegate)
        Else
            TL.LogMessage("SetStateNoAlpaca", $"Running on thread {Thread.CurrentThread.ManagedThreadId}")

            LblAlpacaDiscovery.Visible = False
            CmbDriverSelector.Enabled = True
            alpacaStatusToolstripLabel.Text = "Discovery Disabled"
            alpacaStatusToolstripLabel.BackColor = Color.Salmon
            MnuDiscoverNow.Enabled = False
            MnuEnableDiscovery.Enabled = True
            MnuDisableDiscovery.Enabled = False
            MnuConfigureChooser.Enabled = True
            BtnProperties.Enabled = True
            BtnOK.Enabled = True
            AlpacaStatus.Visible = False
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub SetStateAlpacaDiscovering()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateAlpacaDiscovering", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(SetStateAlpacaDiscoveringDelegate)
        Else

            TL.LogMessage("SetStateAlpacaDiscovering", $"Running on thread {Thread.CurrentThread.ManagedThreadId}")
            LblAlpacaDiscovery.Visible = True
            CmbDriverSelector.Enabled = False
            alpacaStatusToolstripLabel.Text = "Discovery Enabled"
            alpacaStatusToolstripLabel.BackColor = Color.LightGreen
            MnuDiscoverNow.Enabled = False
            MnuEnableDiscovery.Enabled = False
            MnuDisableDiscovery.Enabled = False
            MnuConfigureChooser.Enabled = False
            BtnProperties.Enabled = False
            BtnOK.Enabled = False
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Orange
            alpacaStatusIndicatorTimer.Start()
        End If
    End Sub

    Private Sub SetStateAlpacaDiscoveryCompleteFoundDevices()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateAlpacaDiscoveryCompleteFoundDevices", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate)
        Else
            TL.LogMessage("SetStateAlpacaDiscoveryCompleteFoundDevices", $"Running on thread {Thread.CurrentThread.ManagedThreadId}")
            LblAlpacaDiscovery.Visible = True
            alpacaStatusToolstripLabel.Text = "Discovery Enabled"
            alpacaStatusToolstripLabel.BackColor = Color.LightGreen
            CmbDriverSelector.Enabled = True
            MnuDiscoverNow.Enabled = True
            MnuEnableDiscovery.Enabled = False
            MnuDisableDiscovery.Enabled = True
            MnuConfigureChooser.Enabled = True
            BtnProperties.Enabled = True
            BtnOK.Enabled = True
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Lime
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub SetStateAlpacaDiscoveryCompleteNoDevices()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateAlpacaDiscoveryCompleteFoundDevices", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(SetStateAlpacaDiscoveryCompleteNoDevicesDelegate)
        Else
            TL.LogMessage("SetStateAlpacaDiscoveryCompleteNoDevices", $"Running on thread {Thread.CurrentThread.ManagedThreadId}")
            LblAlpacaDiscovery.Visible = True
            alpacaStatusToolstripLabel.Text = "Discovery Enabled"
            alpacaStatusToolstripLabel.BackColor = Color.LightGreen
            CmbDriverSelector.Enabled = True
            MnuDiscoverNow.Enabled = True
            MnuEnableDiscovery.Enabled = False
            MnuDisableDiscovery.Enabled = True
            MnuConfigureChooser.Enabled = True
            BtnProperties.Enabled = True
            BtnOK.Enabled = True
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Red
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub ValidateDriver(identifier As String)
        Dim deviceInitialised As String, ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        If Not String.IsNullOrEmpty(identifier) Then

            If Not (identifier = "") Then ' Something selected

                WarningTooltipClear() 'Hide any previous message

                TL.LogMessage("ValidateDriver", "ProgID:" & identifier & ", Bitness: " & ApplicationBits.ToString)
                driverIsCompatible = VersionCode.DriverCompatibilityMessage(identifier, ApplicationBits, CType(TL, TraceLogger)) 'Get compatibility warning message, if any

                If driverIsCompatible <> "" Then 'This is an incompatible driver
                    BtnProperties.Enabled = False ' So prevent access!
                    BtnOK.Enabled = False
                    TL.LogMessage("ValidateDriver", "Showing incompatible driver message")
                    WarningToolTipShow("Incompatible Driver (" & identifier & ")", driverIsCompatible)
                Else ' Driver is compatible
                    BtnProperties.Enabled = True ' Turn on Properties
                    deviceInitialised = ProfileStore.GetProfile("Chooser", identifier & " Init")
                    If LCase(deviceInitialised) = "true" Then
                        BtnOK.Enabled = True ' This device has been initialized
                        currentWarningMesage = ""
                        TL.LogMessage("ValidateDriver", "Driver is compatible and configured so no message")
                    Else
                        selectedProgIdValue = ""
                        BtnOK.Enabled = False ' Ensure OK is enabled
                        TL.LogMessage("ValidateDriver", "Showing first time configuration required message")
                        WarningToolTipShow(TOOLTIP_PROPERTIES_TITLE, TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE)
                    End If
                End If
            Else ' Nothing has been selected
                TL.LogMessage("ValidateDriver", "Nothing has been selected")
                selectedProgIdValue = ""
                BtnProperties.Enabled = False
                BtnOK.Enabled = False
            End If
        End If
        ProfileStore.Dispose() 'Clean up profile store

    End Sub

    Private Sub WarningToolTipShow(Title As String, Message As String)
        WarningTooltipClear()
        chooserWarningToolTip.UseAnimation = True
        chooserWarningToolTip.UseFading = False
        chooserWarningToolTip.ToolTipIcon = ToolTipIcon.Warning
        chooserWarningToolTip.AutoPopDelay = 5000
        chooserWarningToolTip.InitialDelay = 0
        chooserWarningToolTip.IsBalloon = False
        chooserWarningToolTip.ReshowDelay = 0
        chooserWarningToolTip.OwnerDraw = False
        chooserWarningToolTip.ToolTipTitle = Title
        currentWarningTitle = Title
        currentWarningMesage = Message

        If Message.Contains(vbCrLf) Then
            chooserWarningToolTip.Show(Message, Me, 18, 24) 'Display at position for a two line message
        Else
            chooserWarningToolTip.Show(Message, Me, 18, 50) 'Display at position for a one line message
        End If
    End Sub

    Private Sub WarningTooltipClear()
        chooserWarningToolTip.RemoveAll()
        currentWarningTitle = ""
        currentWarningMesage = ""
    End Sub

#End Region

End Class