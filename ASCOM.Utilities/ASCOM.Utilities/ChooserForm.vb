
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading
Imports ASCOM.Utilities
Imports Newtonsoft.Json.Serialization

Friend Class ChooserForm
    Inherits Form

#Region "Constants"

    ' Debug constants
    Private Const DEBUG_SINGLE_THREADING As Boolean = True

    ' General constants
    Private Const ALERT_MESSAGEBOX_TITLE As String = "ASCOM Chooser"
    Private Const PROPERTIES_TOOLTIP_DISPLAY_TIME As Integer = 5000 ' Time to display the Properties tooltip (milliseconds)
    Private Const FORM_LOAD_WARNING_MESSAGE_DELAY_TIME As Integer = 250 ' Delay time before any warning message is displayed on form load
    Private Const ALPACA_STATUS_BLINK_TIME As Integer = 100 ' Length of time the Alpaca status indicator spends in the on and off state (ms)
    Private Const TOOLTIP_PROPERTIES_TITLE As String = "Driver Setup"
    Private Const TOOLTIP_PROPERTIES_MESSAGE As String = "Check or change driver Properties (configuration)"
    Private Const TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE As String = "You must check driver configuration before first time use, please click the Properties... button." & vbCrLf & "The OK button will remain greyed out until this is done."
    Private Const TOOLTIP_CREATE_ALPACA_DEVICE_TITLE As String = "Alpaca Device Selected"
    Private Const TOOLTIP_CREATE_ALPACA_DEVICE_MESSAGE As String = "Please click this button to create the Alpaca Dynamic driver"
    Private Const TOOLTIP_CREATE_ALPACA_DEVICE_DISPLAYTIME As Integer = 5 ' Number of seconds to display the Create Alpaca Device informational message
    Private Const CHOOSER_LIST_WIDTH_NEW_ALPACA As Integer = 600 ' Width of the Chooser list when new Alpaca devices are present

    ' Chooser persistence constants
    Friend Const CONFIGRATION_SUBKEY As String = "Chooser\Configuration" ' Store configuration in a subkey under the Chooser key
    Private Const ALPACA_ENABLED As String = "Alpaca enabled" : Private Const ALPACA_ENABLED_DEFAULT As Boolean = False
    Friend Const ALPACA_DISCOVERY_PORT As String = "Alpaca discovery port" : Friend Const ALPACA_DISCOVERY_PORT_DEFAULT As Integer = 32227
    Private Const ALPACA_NUMBER_OF_BROADCASTS As String = "Alpaca number of broadcasts" : Private Const ALPACA_NUMBER_OF_BROADCASTS_DEFAULT As Integer = 2
    Private Const ALPACA_TIMEOUT As String = "Alpaca timeout" : Private Const ALPACA_TIMEOUT_DEFAULT As Double = 1.0
    Private Const ALPACA_DNS_RESOLUTION As String = "Alpaca DNS resolution" : Private Const ALPACA_DNS_RESOLUTION_DEFAULT As Boolean = False
    Private Const ALPACA_SHOW_DISCOVERED_DEVICES As String = "Show discovered Alpaca devices" : Private Const ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT As Boolean = False
    Private Const ALPACA_SHOW_DEVICE_DETAILS As String = "Show Alpaca device details" : Private Const ALPACA_SHOW_DEVICE_DETAILS_DEFAULT As Boolean = False
    Private Const ALPACA_CHOOSER_WIDTH As String = "Alpaca Chooser width" : Private Const ALPACA_CHOOSER_WIDTH_DEFAULT As Integer = 0
    Private Const ALPACA_USE_IPV4 As String = "Use IPv4" : Private Const ALPACA_USE_IPV4_DEFAULT As Boolean = True
    Private Const ALPACA_USE_IPV6 As String = "Use IPv6" : Private Const ALPACA_USE_IPV6_DEFAULT As Boolean = False

    ' Alpaca integration constants
    Private Const ALPACA_DYNAMIC_CLIENT_MANAGER_RELATIVE_PATH As String = "ASCOM\Platform 6\Tools\AlpacaDynamicClientManager"
    Private Const ALPACA_DYNAMIC_CLIENT_MANAGER_EXE_NAME As String = "ASCOM.AlpacaDynamicClientManager.exe"
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
    Private WithEvents clientManagerProcess As Process
    Private driverGenerationComplete As Boolean
    Private currentOkButtonEnabledState As Boolean
    Private currentPropertiesButtonEnabledState As Boolean

    ' Component variables
    Private TL As TraceLogger
    Private chooserWarningToolTip As ToolTip
    Private chooserPropertiesToolTip As ToolTip
    Private createAlpacaDeviceToolTip As ToolTip
    Private alpacaStatusToolstripLabel As ToolStripLabel
    Private WithEvents alpacaStatusIndicatorTimer As System.Windows.Forms.Timer
    Private profile As Profile
    Private registryAccess As RegistryAccess

    ' Persistence variables
    Friend AlpacaEnabled As Boolean
    Friend AlpacaDiscoveryPort As Integer
    Friend AlpacaNumberOfBroadcasts As Integer
    Friend AlpacaTimeout As Double
    Friend AlpacaDnsResolution As Boolean
    Friend AlpacaShowDiscoveredDevices As Boolean
    Friend AlpacaShowDeviceDetails As Boolean
    Friend AlpacaChooserIncrementalWidth As Integer
    Friend AlpacaUseIpV4 As Boolean
    Friend AlpacaUseIpV6 As Boolean

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

        'Get access to the profile registry area
        registryAccess = New RegistryAccess(ERR_SOURCE_CHOOSER)

        ReadState() ' Read in the state variables from persisted storage
        ResizeChooser()

        'Create a trace logger
        TL = New TraceLogger("", "ChooserForm")
        TL.IdentifierWidth = 50
        TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT) ' Enable the trace logger if Util trace is enabled

        profile = New Profile()

    End Sub

    Private Sub ChooserForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Try

            ' Initialise form title and message text
            Text = "ASCOM " & deviceTypeValue & " Chooser"
            lblTitle.Text = "Select the type of " & LCase(deviceTypeValue) & " you have, then be " & "sure to click the Properties... button to configure the driver for your " & LCase(deviceTypeValue) & "."

            ' Initialise the Profile component with the supplied device type
            profile.DeviceType = deviceTypeValue

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

            ' Create Alpaca information tooltip 
            createAlpacaDeviceToolTip = New ToolTip()

            ' Configure the Create Alpaca Device tooltip
            'createAlpacaDeviceToolTip.IsBalloon = True
            'createAlpacaDeviceToolTip.ToolTipIcon = ToolTipIcon.Info
            'createAlpacaDeviceToolTip.UseFading = True
            'createAlpacaDeviceToolTip.ToolTipTitle = TOOLTIP_CREATE_ALPACA_DEVICE_TITLE



            'createAlpacaDeviceToolTip.UseAnimation = True
            'createAlpacaDeviceToolTip.UseFading = False
            'createAlpacaDeviceToolTip.ToolTipIcon = ToolTipIcon.Warning
            'createAlpacaDeviceToolTip.AutoPopDelay = 5000
            'createAlpacaDeviceToolTip.InitialDelay = 0
            'createAlpacaDeviceToolTip.IsBalloon = False
            'createAlpacaDeviceToolTip.ReshowDelay = 0
            'createAlpacaDeviceToolTip.OwnerDraw = False
            'createAlpacaDeviceToolTip.ToolTipTitle = TOOLTIP_CREATE_ALPACA_DEVICE_TITLE
            'createAlpacaDeviceToolTip.ShowAlways = True



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
            If Not profile Is Nothing Then
                Try : profile.Dispose() : Catch : End Try
            End If
            If Not registryAccess Is Nothing Then
                Try : registryAccess.Dispose() : Catch : End Try
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub

#End Region

#Region "Public methods"

    Public WriteOnly Property DeviceType() As String
        Set(ByVal Value As String)

            ' Clean up the supplied device type to consistent values
            Select Case Value.ToLowerInvariant()
                Case "camera"
                    deviceTypeValue = "Camera"
                Case "covercalibrator"
                    deviceTypeValue = "CoverCalibrator"
                Case "dome"
                    deviceTypeValue = "Dome"
                Case "filterwheel"
                    deviceTypeValue = "FilterWheel"
                Case "focuser"
                    deviceTypeValue = "Focuser"
                Case "observingconditions"
                    deviceTypeValue = "ObservingConditions"
                Case "rotator"
                    deviceTypeValue = "Rotator"
                Case "safetymonitor"
                    deviceTypeValue = "SafetyMonitor"
                Case "switch"
                    deviceTypeValue = "Switch"
                Case "telescope"
                    deviceTypeValue = "Telescope"
                Case "video"
                    deviceTypeValue = "Video"
                Case Else ' If not recognised just use as supplied for backward compatibility
                    deviceTypeValue = Value
            End Select

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

#Region "Form, button, control and timer event handlers"

    Private Sub comboProduct_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) 'Handles CmbDriverSelector.DrawItem
        Dim brush As Brush
        Dim colour As Color
        Dim combo As ComboBox
        Dim text As String = ""

        Try
            e.DrawBackground()
            combo = CType(sender, ComboBox)

            brush = Brushes.White
            colour = Color.White

            If combo.SelectedIndex >= 0 Then
                Dim chooseritem As Generic.KeyValuePair(Of ChooserItem, String) = CType(combo.Items(e.Index), Generic.KeyValuePair(Of ChooserItem, String))

                TL.LogMessage("comboProduct_DrawItem", $"IsComDriver: {chooseritem.Key.IsComDriver} {chooseritem.Key.Name} {chooseritem.Value}")
                text = chooseritem.Value
                If chooseritem.Key.IsComDriver Then
                    If chooseritem.Key.ProgID.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant()) Then
                        brush = Brushes.Red
                        colour = Color.Red
                    Else
                        brush = Brushes.LightPink
                        colour = Color.LightPink

                    End If
                Else
                    brush = Brushes.LightGreen
                    colour = Color.LightGreen
                End If
            End If



            e.Graphics.DrawRectangle(New Pen(Color.Black), e.Bounds)
            e.Graphics.FillRectangle(brush, e.Bounds)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            e.Graphics.DrawString(text, combo.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y)

        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Sub ChooserFormMoveEventHandler(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        If currentWarningMesage <> "" Then WarningToolTipShow(currentWarningTitle, currentWarningMesage)
        DisplayAlpacaDeviceToolTip()
    End Sub

    Private Sub AlpacaStatusIndicatorTimerEventHandler(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles alpacaStatusIndicatorTimer.Tick
        If AlpacaStatus.BackColor = Color.Orange Then
            AlpacaStatus.BackColor = Color.DimGray
        Else
            AlpacaStatus.BackColor = Color.Orange
        End If

    End Sub

    ''' <summary>
    ''' Click in Properties... button. Loads the currently selected driver and activate its setup dialogue.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub cmdProperties_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnProperties.Click
        Dim oDrv As Object = Nothing ' The driver
        Dim bConnected As Boolean
        Dim sProgID As String
        Dim ProgIdType As Type
        Dim UseCreateObject As Boolean = False

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

            registryAccess.WriteProfile("Chooser", sProgID & " Init", "True") ' Remember it has been initialized
            EnableOkButton(True)
            WarningTooltipClear()
        Catch ex As Exception
            MsgBox("Failed to load driver: """ & sProgID & """ " & ex.ToString, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            LogEvent("ChooserForm", "Failed to load driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserDriverFailed, ex.ToString)
        End Try

        'Clean up and release resources
        Try : oDrv.Dispose() : Catch ex As Exception : End Try
        Try : Marshal.ReleaseComObject(oDrv) : Catch ex As Exception : End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnCancel.Click
        selectedProgIdValue = ""
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BtnOK.Click
        Dim newProgId As String
        Dim userResponse As DialogResult

        TL.LogMessage("OK Click", $"Combo box selected index = {CmbDriverSelector.SelectedIndex}")

        If selectedChooserItem.IsComDriver Then ' User has selected an existing COM driver so return its ProgID
            selectedProgIdValue = selectedChooserItem.ProgID

            TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'")

            ' Close the UI because the COM driver is selected
            Me.Hide()
        Else ' User has selected a new Alpaca device so we need to create a new COM driver for this

            ' SHow the admin request dialogue if it has not been suppressed by the user
            If Not GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT) Then ' The admin request coming dialogue has not been suppressed so show the dialogue
                Using checkedMessageBox As New CheckedMessageBox()
                    userResponse = checkedMessageBox.ShowDialog()
                End Using
            Else ' The admin request coming dialogue has been suppressed so flag the user response as OK
                userResponse = DialogResult.OK
            End If

            ' Test whether the user clicked the OK button or pressed the "x" cancel icon in the top right of the form
            If userResponse = DialogResult.OK Then ' User pressed the OK button

                Try
                    ' Create a new Alpaca driver of the current ASCOM device type
                    newProgId = CreateNewAlpacaDriver(selectedChooserItem.Name)

                    ' Configure the IP address, port number and Alpaca device number in the newly registered driver
                    profile.DeviceType = deviceTypeValue
                    profile.WriteValue(newProgId, PROFILE_VALUE_NAME_IP_ADDRESS, selectedChooserItem.HostName)
                    profile.WriteValue(newProgId, PROFILE_VALUE_NAME_PORT_NUMBER, selectedChooserItem.Port.ToString())
                    profile.WriteValue(newProgId, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER, selectedChooserItem.DeviceNumber.ToString())
                    profile.WriteValue(newProgId, PROFILE_VALUE_NAME_UNIQUEID, selectedChooserItem.DeviceUniqueID.ToString())

                    ' Flag the driver as being already configured so that it can be used immediately
                    registryAccess.WriteProfile("Chooser", $"{newProgId} Init", "True")

                    ' Select the new driver in the Chooser combo box list
                    selectedProgIdValue = newProgId
                    InitialiseComboBox()

                    TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'")
                Catch ex As Win32Exception When (ex.ErrorCode = &H80004005)
                    TL.LogMessage("OK Click", $"Driver creation cancelled: {ex.Message}")
                    MessageBox.Show($"Driver creation cancelled: {ex.Message}")
                Catch ex As Exception
                    MessageBox.Show($"{ex.ToString()}")
                End Try
            End If

            ' Don't exit the Chooser but instead return to the UI so that the user can see that a new driver has been created and selected
        End If
    End Sub

    ''' <summary>
    ''' Driver generation completion event handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DriverGeneration_Complete(ByVal sender As Object, ByVal e As System.EventArgs) Handles clientManagerProcess.Exited
        driverGenerationComplete = True ' Flag that driver generation is complete
    End Sub

    Private Sub cbDriverSelector_SelectionChangeCommitted(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmbDriverSelector.SelectionChangeCommitted
        If CmbDriverSelector.SelectedIndex >= 0 Then

            ' Save the newly selected chooser item
            selectedChooserItem = CType(CmbDriverSelector.SelectedItem, Generic.KeyValuePair(Of ChooserItem, String)).Key
            selectedProgIdValue = selectedChooserItem.ProgID

            ' Validate the driver if it is a COM driver
            If selectedChooserItem.IsComDriver Then ' This is a COM driver
                TL.LogMessage("SelectedIndexChanged", $"New COM driver selected. ProgID: {selectedChooserItem.ProgID} {selectedChooserItem.Name}")
                ValidateDriver(selectedChooserItem.ProgID)
            Else ' This is a new Alpaca driver
                TL.LogMessage("SelectedIndexChanged", $"New Alpaca driver selected : {selectedChooserItem.Name}")
                EnablePropertiesButton(False) ' Disable the Properties button because there is not yet a COM driver to configure
                WarningTooltipClear()
                EnableOkButton(True)
                DisplayAlpacaDeviceToolTip()
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
        Dim TraceFileName As String


        TraceFileName = registryAccess.GetProfile("", SERIAL_FILE_NAME_VARNAME)
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

    End Sub

    Private Sub MenuAutoTraceFilenames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Auto filenames currently disabled, so enable them
        MenuSerialTraceEnabled.Enabled = True 'Set the trace enabled flag
        MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
        registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
    End Sub

    Private Sub MenuSerialTraceFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim RetVal As System.Windows.Forms.DialogResult

        RetVal = SerialTraceFileName.ShowDialog()
        Select Case RetVal
            Case Windows.Forms.DialogResult.OK
                'Save the result
                registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName)
                'Check and enable the serial trace enabled flag
                MenuSerialTraceEnabled.Enabled = True
                MenuSerialTraceEnabled.Checked = True
                'Enable manual serial trace file flag
            Case Else 'Ignore everything else

        End Select
    End Sub

    Private Sub MenuSerialTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSerialTraceEnabled.Click

        If MenuSerialTraceEnabled.Checked Then ' Auto serial trace is on so turn it off
            MenuSerialTraceEnabled.Checked = False
            registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "")
        Else ' Auto serial trace is off so turn it on
            MenuSerialTraceEnabled.Checked = True
            registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        End If
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

    Private Sub MnuManageAlpacaDevices_Click(sender As Object, e As EventArgs) Handles MnuManageAlpacaDevices.Click
        Dim deviceWasRegistered As Boolean

        ' Get the current registration state for the selected ProgID
        deviceWasRegistered = profile.IsRegistered(selectedProgIdValue)

        TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} of type {profile.DeviceType} is registered: {deviceWasRegistered}")

        ' Run the client manager in manage mode
        RunDynamicClientManager("ManageDevices")

        'Test whether the selected ProgID has just been deleted and if so unselect the ProgID
        If deviceWasRegistered Then
            ' Unselect the ProgID if it has just been deleted
            If Not profile.IsRegistered(selectedProgIdValue) Then
                selectedChooserItem = Nothing
                TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} was registered but has been deleted")
            Else
                TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} is still registered - no action")
            End If
        Else
            TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} was NOT registered - no action")
        End If

        ' Refresh the driver list after any changes made by the management tool
        InitialiseComboBox()

    End Sub

    ''' <summary>
    ''' Find the lowest numbered unused ProgID in the series {progIdBase}{N}.{deviceType} where N Is an integer starting at 1
    ''' </summary>
    ''' <param name="progIdBase">ProgID base string</param>
    ''' <param name="deviceType">ASCOM device type</param>
    ''' <returns></returns>
    Private Function CreateNewAlpacaDriver(deviceDescription As String) As String
        Dim newProgId As String
        Dim deviceNumber As Integer
        Dim typeFromProgId As Type

        ' Initialise to a starting value
        deviceNumber = 0

        ' Try successive ProgIDs until one is found that is not COM registered
        Do
            deviceNumber += 1 ' Increment the device number
            newProgId = $"{DRIVER_PROGID_BASE}{deviceNumber}.{deviceTypeValue}" ' Create the new ProgID to be tested
            typeFromProgId = Type.GetTypeFromProgID(newProgId) ' Try to get the type with the new ProgID
            TL.LogMessage("CreateAlpacaClient", $"Testing ProgID: {newProgId} Type name: {typeFromProgId?.Name}")
        Loop While (Not (typeFromProgId Is Nothing)) ' Loop until the returned type is null indicating that this type is not COM registered

        TL.LogMessage("CreateAlpacaClient", $"Creating new ProgID: {newProgId}")

        ' Create the new Alpaca Client appending the device description if required 
        If (String.IsNullOrEmpty(deviceDescription)) Then
            RunDynamicClientManager($"\CreateNamedClient {deviceTypeValue} {deviceNumber} {newProgId}")
        Else
            RunDynamicClientManager($"\CreateAlpacaClient {deviceTypeValue} {deviceNumber} {newProgId} ""{deviceDescription}""")
        End If

        Return newProgId ' Return the new ProgID
    End Function

    Private Sub MnuCreateAlpacaDriver_Click(sender As Object, e As EventArgs) Handles MnuCreateAlpacaDriver.Click
        Dim newProgId As String
        Dim userResponse As MsgBoxResult

        ' Create a new Alpaca driver of the current ASCOM device type
        newProgId = CreateNewAlpacaDriver("")

        ' Select the new driver in the Chooser combo box list
        selectedProgIdValue = newProgId
        InitialiseComboBox()

        TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'")

    End Sub

#End Region

#Region "State Persistence"

    Private Overloads Sub ReadState()
        ReadState("Telescope")
    End Sub

    Private Overloads Sub ReadState(DeviceType As String)
        Try
            TL?.LogMessageCrLf("ChooserReadState", $"Reading state for device type: {DeviceType}. Configuration key: {CONFIGRATION_SUBKEY}, Alpaca enabled: {$"{DeviceType} {ALPACA_ENABLED}"}, ALapca default: {ALPACA_ENABLED_DEFAULT}")

            ' The enabled state is per device type
            AlpacaEnabled = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", ALPACA_ENABLED_DEFAULT.ToString()), CultureInfo.InvariantCulture)

            ' These values are for all Alpaca devices
            AlpacaDiscoveryPort = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, ALPACA_DISCOVERY_PORT_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaNumberOfBroadcasts = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, ALPACA_NUMBER_OF_BROADCASTS_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaTimeout = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, ALPACA_TIMEOUT_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaDnsResolution = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, ALPACA_DNS_RESOLUTION_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaShowDeviceDetails = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, ALPACA_SHOW_DEVICE_DETAILS_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaShowDiscoveredDevices = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaChooserIncrementalWidth = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, ALPACA_CHOOSER_WIDTH_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaUseIpV4 = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV4, ALPACA_USE_IPV4_DEFAULT.ToString()), CultureInfo.InvariantCulture)
            AlpacaUseIpV6 = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV6, ALPACA_USE_IPV6_DEFAULT.ToString()), CultureInfo.InvariantCulture)
        Catch ex As Exception
            MsgBox("Chooser Read State " & ex.ToString)
            LogEvent("Chooser Read State ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
            TL?.LogMessageCrLf("ChooserReadState", ex.ToString())
        End Try
    End Sub

    Private Sub WriteState(DeviceType As String)

        Try

            ' Save the enabled state per "device type" 
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", AlpacaEnabled.ToString(CultureInfo.InvariantCulture))

            ' Save other states for all Alpaca devices 
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, AlpacaDiscoveryPort.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, AlpacaNumberOfBroadcasts.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, AlpacaTimeout.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, AlpacaDnsResolution.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, AlpacaShowDeviceDetails.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, AlpacaShowDiscoveredDevices.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, AlpacaChooserIncrementalWidth.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV4, AlpacaUseIpV4.ToString(CultureInfo.InvariantCulture))
            registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV6, AlpacaUseIpV6.ToString(CultureInfo.InvariantCulture))

        Catch ex As Exception
            MsgBox("Chooser Write State " & ex.ToString)
            LogEvent("Chooser Write State ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
            TL?.LogMessageCrLf("ChooserWriteState", ex.ToString())
        End Try

    End Sub

#End Region

#Region "Support code"

    ''' <summary>
    ''' Run the Alpaca dynamic client manager application with the supplied parameters
    ''' </summary>
    ''' <param name="parameterString">Parameter string to pass to the application</param>
    Private Sub RunDynamicClientManager(parameterString As String)
        Dim clientManagerWorkingDirectory, clientManagerExeFile As String
        Dim clientManagerProcessStartInfo As ProcessStartInfo

        ' Construct path to the executable that will dynamically create a new Alpaca COM client
        clientManagerWorkingDirectory = $"{Get32BitProgramFilesPath()}\{ALPACA_DYNAMIC_CLIENT_MANAGER_RELATIVE_PATH}"
        clientManagerExeFile = $"{clientManagerWorkingDirectory}\{ALPACA_DYNAMIC_CLIENT_MANAGER_EXE_NAME}"

        TL.LogMessage("RunDynamicClientManager", $"Generator parameters: '{parameterString}'")
        TL.LogMessage("RunDynamicClientManager", $"Managing drivers using the {clientManagerExeFile} executable in working directory {clientManagerWorkingDirectory}")

        If Not File.Exists(clientManagerExeFile) Then
            MsgBox("The client generator executable can not be found, please repair the ASCOM Platform.", MsgBoxStyle.Critical, "Alpaca Client Generator Not Found")
            TL.LogMessage("RunDynamicClientManager", $"ERROR - Unable to find the client generator executable at {clientManagerExeFile}, cannot create a new Alpaca client.")
            selectedProgIdValue = ""
            Return
        End If

        ' Set the process run time environment and parameters
        clientManagerProcessStartInfo = New ProcessStartInfo(clientManagerExeFile, parameterString) ' Run the executable with no parameters in order to show the management GUI
        clientManagerProcessStartInfo.WorkingDirectory = clientManagerWorkingDirectory

        ' Create the management process
        clientManagerProcess = New Process()
        clientManagerProcess.StartInfo = clientManagerProcessStartInfo
        clientManagerProcess.EnableRaisingEvents = True

        ' Initialise the process complete flag to false
        driverGenerationComplete = False

        ' Run the process
        TL.LogMessage("RunDynamicClientManager", $"Starting driver management process")
        clientManagerProcess.Start()

        ' Wait for the process to complete at which point the process complete event will fire and driverGenerationComplete will be set true
        Do
            Thread.Sleep(10)
            Application.DoEvents()
        Loop While Not driverGenerationComplete

        TL.LogMessage("RunDynamicClientManager", $"Completed driver management process")

        clientManagerProcess.Dispose()

    End Sub

    ''' <summary>
    ''' Get the 32bit ProgramFiles path on both 32bit and 64bit systems
    ''' </summary>
    ''' <returns></returns>
    Private Function Get32BitProgramFilesPath() As String
        ' Try to get the 64bit path
        Dim returnValue As String = Environment.GetEnvironmentVariable("ProgramFiles(x86)")

        ' If no path is returned get the 32bit path
        If String.IsNullOrEmpty(returnValue) Then
            returnValue = Environment.GetEnvironmentVariable("ProgramFiles")
        End If

        TL.LogMessage("Get32BitProgramFilesPath", $"Returned path: {returnValue}")
        Return returnValue
    End Function

    Private Sub InitialiseComboBox()

        TL.LogMessage("InitialiseComboBox", $"Arrived at InitialiseComboBox - Running On thread: {Thread.CurrentThread.ManagedThreadId}.")

        If DEBUG_SINGLE_THREADING Then
            TL.LogMessage("InitialiseComboBox", $"Starting single threaded discovery...")
            DiscoverAlpacaDevicesAndPopulateDriverComboBox()
            TL.LogMessage("InitialiseComboBox", $"Completed single threaded discovery")
        Else ' Normal multi-threading behaviour
            TL.LogMessage("InitialiseComboBox", $"Creating discovery thread...")
            Dim discoveryThread As Thread = New Thread(AddressOf DiscoverAlpacaDevicesAndPopulateDriverComboBox)
            TL.LogMessage("InitialiseComboBox", $"Successfully created discovery thread, about to start discovery thread...")
            discoveryThread.Start()
            TL.LogMessage("InitialiseComboBox", $"Discovery thread started OK")
        End If

        TL.LogMessage("InitialiseComboBox", $"Exiting InitialiseComboBox on thread: {Thread.CurrentThread.ManagedThreadId}.")
    End Sub

    Private Sub DiscoverAlpacaDevicesAndPopulateDriverComboBox()
        Try

            TL.LogMessage("DiscoverAlpacaDevices", $"Running On thread: {Thread.CurrentThread.ManagedThreadId}.")

            chooserList = New SortedList(Of ChooserItem, String)

            ' Enumerate the available drivers, and load their descriptions and ProgIDs into the driversList generic sorted list collection. Key is ProgID, value is friendly name.
            Try
                ' Get Key-Class pairs in the subkey "{DeviceType} Drivers" e.g. "Telescope Drivers"
                Dim driverList As SortedList(Of String, String) = registryAccess.EnumKeys(deviceTypeValue & " Drivers")
                TL.LogMessage("DiscoverAlpacaDevices", $"Returned {driverList.Count} COM drivers")

                For Each driver As KeyValuePair(Of String, String) In driverList
                    Dim driverProgId, driverName As String

                    driverProgId = driver.Key
                    driverName = driver.Value

                    TL.LogMessage("PopulateDriverComboBox", $"Found ProgID: {driverProgId} , Description: '{driverName}'")

                    If String.IsNullOrEmpty(driverName) Then ' Description Is missing
                        TL.LogMessage("PopulateDriverComboBox", $"  ***** Description missing for ProgID: {driverProgId}")
                    End If

                    ' Annotate the device description as configured
                    If (driverProgId.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant())) Then ' This is a COM driver for an Alpaca device
                        If AlpacaShowDeviceDetails Then ' Get device details from the Profile and display these
                            driverName = $"{driverName}    ({driverProgId} ==> {profile.GetValue(driverProgId, PROFILE_VALUE_NAME_IP_ADDRESS, Nothing)}:" +
                                         $"{profile.GetValue(driverProgId, PROFILE_VALUE_NAME_PORT_NUMBER, Nothing)}/api/v1/{deviceTypeValue}/{profile.GetValue(driverProgId, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER, Nothing)}" +
                                         $") - {profile.GetValue(driverProgId, PROFILE_VALUE_NAME_UNIQUEID)}" ' Annotate as Alpaca Dynamic driver to differentiate from other COM drivers
                        Else ' Just annotate as an Alpaca device
                            driverName = $"{driverName}    (Alpaca)" ' Annotate as an Alpaca device
                        End If
                    Else ' This is not an Alpaca COM driver
                        If AlpacaShowDeviceDetails Then ' Get device details from the Profile and display these
                            driverName = $"{driverName}    ({driverProgId})" ' Annotate with ProgID
                        Else
                            ' No action just use the driver description as is
                        End If
                    End If

                    chooserList.Add(New ChooserItem(driverProgId, driverName), driverName)
                Next

            Catch ex1 As Exception
                TL.LogMessageCrLf("DiscoverAlpacaDevices", "Exception: " & ex1.ToString)
                'Ignore any exceptions from this call e.g. if there are no devices of that type installed just create an empty list
            End Try

            TL.LogMessage("DiscoverAlpacaDevices", $"Completed COM driver enumeration")

            If (AlpacaEnabled) Then
                alpacaDevices = New List(Of AscomDevice) ' Initialise to a clear list with no Alpaca devices

                ' Render the user interface unresponsive while discovery is underway, except for the Cancel button.
                SetStateAlpacaDiscovering()

                ' Initiate discovery and wait for it to complete
                Using discovery As AlpacaDiscovery = New AlpacaDiscovery(TL)
                    TL.LogMessage("DiscoverAlpacaDevices", $"AlpacaDiscovery created")
                    discovery.StartDiscovery(AlpacaNumberOfBroadcasts, 200, AlpacaDiscoveryPort, AlpacaTimeout, AlpacaDnsResolution, AlpacaUseIpV4, AlpacaUseIpV6)
                    TL.LogMessage("DiscoverAlpacaDevices", $"AlpacaDiscovery started")

                    ' Keep the UI alive while the discovery is running
                    Do
                        Threading.Thread.Sleep(10)
                        Application.DoEvents()
                    Loop Until discovery.DiscoveryComplete
                    TL.LogMessage("DiscoverAlpacaDevices", $"Discovery phase has finished")

                    TL.LogMessage("DiscoverAlpacaDevices", $"Discovered {discovery.GetAscomDevices("").Count} devices")

                    ' List discovered devices to the log
                    For Each ascomDevice As AscomDevice In discovery.GetAscomDevices("")
                        TL.LogMessage("DiscoverAlpacaDevices", $"FOUND {ascomDevice.AscomDeviceType} {ascomDevice.AscomDeviceName} {ascomDevice.IPEndPoint.ToString()}")
                    Next

                    TL.LogMessage("DiscoverAlpacaDevices", $"Discovered {discovery.GetAscomDevices(deviceTypeValue).Count} {deviceTypeValue} devices")

                    ' Get discovered devices of the requested ASCOM device type
                    alpacaDevices = discovery.GetAscomDevices(deviceTypeValue)
                End Using

                ' Add any Alpaca devices to the list
                For Each device As AscomDevice In alpacaDevices
                    TL.LogMessage("DiscoverAlpacaDevices", $"Discovered Alpaca device: {device.AscomDeviceType} {device.AscomDeviceName} {device.UniqueId} at  http://{device.HostName}:{device.IPEndPoint.Port.ToString()}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber}")

                    Dim displayHostName As String = CType(IIf(device.HostName = device.IPEndPoint.Address.ToString(), device.IPEndPoint.Address.ToString(), $"{device.HostName} ({device.IPEndPoint.Address.ToString()})"), String)
                    Dim displayName As String

                    Dim deviceUniqueId, deviceHostName As String
                    Dim deviceIPPort, deviceNumber As Integer

                    ' Get a list of dynamic drivers already configured on the system
                    Dim foundDriver As Boolean = False

                    For Each arrayListDevice As KeyValuePair In profile.RegisteredDevices(deviceTypeValue) ' Iterate over a list of all devices of the current device type
                        If arrayListDevice.Key.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant()) Then 'This is a dynamic Alpaca COM driver

                            ' Get and validate the device values to compare with the discovered device
                            Try
                                deviceUniqueId = profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_UNIQUEID)
                            Catch ex As Exception
                                MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device unique ID. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Continue For ' Don't process this driver further, move on to the next driver
                            End Try

                            Try
                                deviceHostName = profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_IP_ADDRESS)
                                If String.IsNullOrEmpty(deviceHostName) Then
                                    MessageBox.Show($"{arrayListDevice.Key} - The device IP address is blank. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Continue For ' Don't process this driver further, move on to the next driver
                                End If
                            Catch ex As Exception
                                MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device IP address. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Continue For ' Don't process this driver further, move on to the next driver
                            End Try

                            Try
                                deviceIPPort = Convert.ToInt32(profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_PORT_NUMBER))
                            Catch ex As Exception
                                MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device IP Port. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Continue For ' Don't process this driver further, move on to the next driver
                            End Try

                            Try
                                deviceNumber = Convert.ToInt32(profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER))
                            Catch ex As Exception
                                MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device number. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Continue For ' Don't process this driver further, move on to the next driver
                            End Try

                            TL.LogMessage("DiscoverAlpacaDevices", $"Found existing COM dynamic driver for device {deviceUniqueId} at http://{deviceHostName}:{deviceIPPort}/api/v1/{deviceTypeValue}/{deviceNumber}")
                            TL.LogMessage("DiscoverAlpacaDevices", $"{device.UniqueId} {deviceUniqueId} {device.UniqueId = deviceUniqueId} {device.HostName = deviceHostName} {device.IPEndPoint.Port = deviceIPPort} {device.AlpacaDeviceNumber = deviceNumber}")

                            If (device.UniqueId = deviceUniqueId) And (device.HostName = deviceHostName) And (device.IPEndPoint.Port = deviceIPPort) And (device.AlpacaDeviceNumber = deviceNumber) Then
                                foundDriver = True
                                TL.LogMessage("DiscoverAlpacaDevices", $"    Found existing COM driver match!")
                            End If
                        End If
                    Next

                    If foundDriver Then
                        TL.LogMessage("DiscoverAlpacaDevices", $"Found driver match for {device.AscomDeviceName}")
                        If AlpacaShowDiscoveredDevices Then
                            TL.LogMessage("DiscoverAlpacaDevices", $"Showing KNOWN ALPACA DEVICE entry for {device.AscomDeviceName}")
                            displayName = $"* KNOWN ALPACA DEVICE   {device.AscomDeviceName}   {displayHostName}:{ device.IPEndPoint.Port.ToString()}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber} - {device.UniqueId}"
                            chooserList.Add(New ChooserItem(device.UniqueId, device.AlpacaDeviceNumber, device.HostName, device.IPEndPoint.Port, device.AscomDeviceName), displayName)
                        Else
                            TL.LogMessage("DiscoverAlpacaDevices", $"This device MATCHES an existing COM driver so NOT adding it to the Combo box list")
                        End If

                    Else
                        TL.LogMessage("DiscoverAlpacaDevices", $"This device does NOT match an existing COM driver so ADDING it to the Combo box list")
                        displayName = $"* NEW ALPACA DEVICE   {device.AscomDeviceName}   {displayHostName}:{ device.IPEndPoint.Port.ToString()}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber} - {device.UniqueId}"
                        chooserList.Add(New ChooserItem(device.UniqueId, device.AlpacaDeviceNumber, device.HostName, device.IPEndPoint.Port, device.AscomDeviceName), displayName)
                    End If

                Next
            End If

            ' List the ChooserList contents
            TL.LogMessage("DiscoverAlpacaDevices", $"Start of Chooser List")
            For Each item As System.Collections.Generic.KeyValuePair(Of ChooserItem, String) In chooserList
                TL.LogMessage("DiscoverAlpacaDevices", $"List includes device {item.Value}")
            Next
            TL.LogMessage("DiscoverAlpacaDevices", $"End of Chooser List")

            ' Populate the device list combo box with COM and Alpaca devices.
            ' This Is implemented as an independent method because it interacts with UI controls And will self invoke if required
            PopulateDriverComboBox()

        Catch ex As Exception
            TL.LogMessageCrLf("DiscoverAlpacaDevices", ex.ToString())
        Finally
            ' Restore a usable user interface
            If AlpacaEnabled Then
                If alpacaDevices.Count > 0 Then
                    SetStateAlpacaDiscoveryCompleteFoundDevices()
                Else
                    SetStateAlpacaDiscoveryCompleteNoDevices()
                End If
            Else
                SetStateNoAlpaca()
            End If
            DisplayAlpacaDeviceToolTip()
        End Try
    End Sub

    Private Sub PopulateDriverComboBox()
        ' Only proceed if there are drivers or Alpaca devices to display
        If (chooserList.Count = 0) And (alpacaDevices.Count = 0) Then ' No drivers to add to the combo box 
            MsgBox("There are no ASCOM " & deviceTypeValue & " drivers installed.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            Return
        End If

        If CmbDriverSelector.InvokeRequired Then ' We are not running on the UI thread
            TL.LogMessage("PopulateDriverComboBox", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(PopulateDriverComboBoxDelegate)
        Else ' We are running on the UI thread
            Try
                TL.LogMessage("PopulateDriverComboBox", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

                ' Initialise the selected chooser item
                'selectedChooserItem = Nothing

                ' Set the combo box data source to the chooserList list of items to display
                CmbDriverSelector.SelectedIndex = -1
                CmbDriverSelector.DataSource = New BindingSource(chooserList, Nothing)
                CmbDriverSelector.DisplayMember = "Value"
                CmbDriverSelector.ValueMember = "Key"

                CmbDriverSelector.DropDownWidth = DropDownWidth(CmbDriverSelector) ' AutoSize the combo box width

                ' If a ProgID has been provided, test whether it matches a ProgID in the driver list
                If selectedProgIdValue <> "" Then ' A progID was provided

                    ' Select the current device in the list
                    For Each driver As Generic.KeyValuePair(Of ChooserItem, String) In CmbDriverSelector.Items
                        TL.LogMessage("PopulateDriverComboBox", $"Searching for ProgID: {selectedProgIdValue}, found ProgID: {driver.Key.ProgID}")
                        If driver.Key.ProgID.ToLowerInvariant() = selectedProgIdValue.ToLowerInvariant() Then
                            TL.LogMessage("PopulateDriverComboBox", $"*** Found ProgID: {selectedProgIdValue}")
                            CmbDriverSelector.SelectedItem = driver
                            selectedChooserItem = driver.Key
                            EnableOkButton(True) ' Enable the OK button
                        End If
                    Next
                End If

                If selectedChooserItem Is Nothing Then ' The requested driver was not found so display a blank Chooser item
                    TL.LogMessage("PopulateDriverComboBox", $"Selected ProgID {selectedProgIdValue} WAS NOT found, displaying a blank combo list item")

                    CmbDriverSelector.ResetText()
                    CmbDriverSelector.SelectedIndex = -1

                    EnablePropertiesButton(False)
                    EnableOkButton(False)
                Else
                    TL.LogMessage("PopulateDriverComboBox", $"Selected ProgID {selectedProgIdValue} WAS found. Device is: {selectedChooserItem.Name}, Is COM driver: {selectedChooserItem.IsComDriver}")

                    ' Validate the selected driver if it is a COM driver
                    If selectedChooserItem.IsComDriver Then ' This is a COM driver so validate that it is functional
                        ValidateDriver(selectedChooserItem.ProgID)
                    Else ' This is a new Alpaca driver
                        WarningTooltipClear()
                        EnablePropertiesButton(False) ' Disable the Properties button because there is not yet a COM driver to configure
                        EnableOkButton(True)

                    End If
                End If

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
        TL.LogMessage("DropDownWidth", $"Combo box: {comboBox.Name} Number of items: {comboBox.Items.Count} ")

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
            BtnProperties.Enabled = currentPropertiesButtonEnabledState
            BtnOK.Enabled = currentOkButtonEnabledState
            AlpacaStatus.Visible = False
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub SetStateAlpacaDiscovering()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateAlpacaDiscovering", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(SetStateAlpacaDiscoveringDelegate)
        Else
            TL.LogMessage("SetStateAlpacaDiscovering", $"Running on thread {Thread.CurrentThread.ManagedThreadId} OK button enabled state: {currentOkButtonEnabledState}")
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
            BtnProperties.Enabled = currentPropertiesButtonEnabledState
            BtnOK.Enabled = currentOkButtonEnabledState
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Lime
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub SetStateAlpacaDiscoveryCompleteNoDevices()
        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("SetStateAlpacaDiscoveryCompleteNoDevices", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
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
            BtnProperties.Enabled = currentPropertiesButtonEnabledState
            BtnOK.Enabled = currentOkButtonEnabledState
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Red
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub ValidateDriver(progId As String)
        Dim deviceInitialised As String

        If Not String.IsNullOrEmpty(progId) Then

            If Not (progId = "") Then ' Something selected

                WarningTooltipClear() 'Hide any previous message

                TL.LogMessage("ValidateDriver", "ProgID:" & progId & ", Bitness: " & ApplicationBits.ToString)
                driverIsCompatible = VersionCode.DriverCompatibilityMessage(progId, ApplicationBits, CType(TL, TraceLogger)) 'Get compatibility warning message, if any

                If driverIsCompatible <> "" Then 'This is an incompatible driver so we need to prevent access
                    EnablePropertiesButton(False)
                    EnableOkButton(False)
                    TL.LogMessage("ValidateDriver", "Showing incompatible driver message")
                    WarningToolTipShow("Incompatible Driver (" & progId & ")", driverIsCompatible)
                Else ' This is a compatible driver
                    EnablePropertiesButton(True) ' Turn on Properties
                    deviceInitialised = registryAccess.GetProfile("Chooser", progId & " Init")
                    If LCase(deviceInitialised) = "true" Then ' This device has been initialized
                        EnableOkButton(True)
                        currentWarningMesage = ""
                        TL.LogMessage("ValidateDriver", "Driver is compatible and configured so no message")
                    Else ' This device has not been initialised
                        selectedProgIdValue = ""
                        EnableOkButton(False) ' Ensure OK is disabled
                        TL.LogMessage("ValidateDriver", "Showing first time configuration required message")
                        WarningToolTipShow(TOOLTIP_PROPERTIES_TITLE, TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE)
                    End If
                End If
            Else ' Nothing has been selected
                TL.LogMessage("ValidateDriver", "Nothing has been selected")
                selectedProgIdValue = ""
                EnablePropertiesButton(False)
                EnableOkButton(False) ' Ensure OK is disabled
            End If
        End If

    End Sub

    Private Sub WarningToolTipShow(Title As String, Message As String)
        Const MESSAGE_X_POSITION As Integer = 120 ' Was 18

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
            chooserWarningToolTip.Show(Message, Me, MESSAGE_X_POSITION, 24) 'Display at position for a two line message
        Else
            chooserWarningToolTip.Show(Message, Me, MESSAGE_X_POSITION, 50) 'Display at position for a one line message
        End If
    End Sub

    Private Delegate Sub NoParameterDelegate()
    Private displayCreateAlpacDeviceTooltip As NoParameterDelegate = New NoParameterDelegate(AddressOf DisplayAlpacaDeviceToolTip)

    Private Sub DisplayAlpacaDeviceToolTip()
        Dim selectedItem As Generic.KeyValuePair(Of ChooserItem, String)

        ' Only consider displaying the tooltip if it has been instantiated
        If Not createAlpacaDeviceToolTip Is Nothing Then

            ' Only display the tooltip if Alpaca discovery is enabled and the Alpaca dialogues have NOT been suppressed
            If AlpacaEnabled And Not GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT) Then

                ' The tooltip code must be executed by the UI thread so invoke this if required
                If BtnOK.InvokeRequired Then
                    TL.LogMessage("DisplayAlpacaDeviceToolTip", $"Invoke required on thread {Thread.CurrentThread.ManagedThreadId}")
                    BtnOK.Invoke(displayCreateAlpacDeviceTooltip)
                Else
                    ' Only display the tooltip if a device has been selected
                    If Not CmbDriverSelector.SelectedItem Is Nothing Then
                        selectedItem = CType(CmbDriverSelector.SelectedItem, Generic.KeyValuePair(Of ChooserItem, String))

                        ' Only display the tooltip if the an Alpaca driver has been selected
                        If Not selectedItem.Key.IsComDriver Then

                            createAlpacaDeviceToolTip.RemoveAll()

                            createAlpacaDeviceToolTip.UseAnimation = True
                            createAlpacaDeviceToolTip.UseFading = False
                            createAlpacaDeviceToolTip.ToolTipIcon = ToolTipIcon.Info
                            createAlpacaDeviceToolTip.AutoPopDelay = 5000
                            createAlpacaDeviceToolTip.InitialDelay = 0
                            createAlpacaDeviceToolTip.IsBalloon = True
                            createAlpacaDeviceToolTip.ReshowDelay = 0
                            createAlpacaDeviceToolTip.OwnerDraw = False
                            createAlpacaDeviceToolTip.ToolTipTitle = TOOLTIP_CREATE_ALPACA_DEVICE_TITLE

                            'createAlpacaDeviceToolTip.Show(TOOLTIP_CREATE_ALPACA_DEVICE_MESSAGE, Me, 295, 85, TOOLTIP_CREATE_ALPACA_DEVICE_DISPLAYTIME * 1000) 'Display at position for a two line message
                            createAlpacaDeviceToolTip.Show(TOOLTIP_CREATE_ALPACA_DEVICE_MESSAGE, BtnOK, 45, -60, TOOLTIP_CREATE_ALPACA_DEVICE_DISPLAYTIME * 1000) 'Display at position for a two line message
                            TL.LogMessage("DisplayAlpacaDeviceToolTip", $"Set tooltip on thread {Thread.CurrentThread.ManagedThreadId}")
                        End If
                    End If

                End If
            End If
        End If
    End Sub

    Private Sub WarningTooltipClear()
        chooserWarningToolTip.RemoveAll()
        currentWarningTitle = ""
        currentWarningMesage = ""
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

    ''' <summary>
    ''' Set the enabled state of the OK button and record this as the current state
    ''' </summary>
    ''' <param name="state"></param>
    Private Sub EnableOkButton(state As Boolean)
        BtnOK.Enabled = state
        currentOkButtonEnabledState = state
    End Sub

    ''' <summary>
    ''' Set the enabled state of the Properties button and record this as the current state
    ''' </summary>
    ''' <param name="state"></param>
    Private Sub EnablePropertiesButton(state As Boolean)
        BtnProperties.Enabled = state
        currentPropertiesButtonEnabledState = state
    End Sub

#End Region

End Class