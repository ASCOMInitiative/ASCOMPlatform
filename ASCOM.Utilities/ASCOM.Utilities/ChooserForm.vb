Option Strict Off
Option Explicit On

Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Threading
Imports ASCOM.Utilities
Imports ASCOM.Utilities.Alpaca
Imports ASCOM.Utilities.Support

Friend Class ChooserForm
    Inherits System.Windows.Forms.Form

#Region "Constants"

    Private Const ALERT_MESSAGEBOX_TITLE As String = "ASCOM Chooser"
    Private Const PROPERTIES_TOOLTIP_DISPLAY_TIME As Integer = 5000 ' Time to display the Properties tooltip (milliseconds)
    Private Const FORM_LOAD_WARNING_MESSAGE_DELAY_TIME As Integer = 250 ' Delay time before any warning message is displayed on form load
    Private Const ALPACA_STATUS_BLINK_TIME As Integer = 100 ' Length of time the Alpaca status indicator spends in the on and off state (ms)
    Private Const TOOLTIP_PROPERTIES_TITLE As String = "Driver Setup"
    Private Const TOOLTIP_PROPERTIES_MESSAGE As String = "Check or change driver Properties (configuration)"
    Private Const TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE As String = "You must check driver configuration before first time use, please click the Properties... button." & vbCrLf & "The OK button will remain greyed out until this is done."
    Private Const CHOOSER_LIST_WIDTH_NEW_ALPACA As Integer = 600 ' Width of the Chooser list when new Alpaca devices are present


    ' Persistence constants
    Private Const CONFIGRATION_SUBKEY As String = "Chooser\Configuration" ' Store configuration in a subkey under the Chooser key
    Private Const ALPACA_ENABLED As String = "Alpaca enabled" : Private Const ALPACA_ENABLED_DEFAULT As Boolean = False
    Private Const ALPACA_DISCOVERY_PORT As String = "Alpaca discovery port" : Private Const ALPACA_DISCOVERY_PORT_DEFAULT As Integer = 32227
    Private Const ALPACA_NUMBER_OF_BROADCASTS As String = "Alpaca number of broadcasts" : Private Const ALPACA_NUMBER_OF_BROADCASTS_DEFAULT As Integer = 2
    Private Const ALPACA_TIMEOUT As String = "Alpaca timeout" : Private Const ALPACA_TIMEOUT_DEFAULT As Double = 2.0
    Private Const ALPACA_DNS_RESOLUTION As String = "Alpaca DNS resolution" : Private Const ALPACA_DNS_RESOLUTION_DEFAULT As Boolean = False

#End Region

#Region "Variables"

    ' Chooser variables
    Private deviceTypeValue, initiallySelectedProgIdValue, selectedProgIdValue As String
    Private driversList As Generic.SortedList(Of String, String)
    Private driverIsCompatible As String = ""
    Private currentWarningTitle, currentWarningMesage As String
    Private ascomDevices As Generic.List(Of AscomDevice) = New Generic.List(Of AscomDevice)()

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

#End Region

#Region "Form load, close, paint and dispose event handlers"

    Public Sub New()
        MyBase.New()
        InitializeComponent()

        'Create a trace logger
        TL = New TraceLogger("", "ChooserForm")
        TL.IdentifierWidth = 50
        TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT) ' Enable the trace logger if Util trace is enabled

    End Sub

    Private Sub ChooserForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Try

            ' Initialise form title and message text
            Text = "ASCOM " & deviceTypeValue & " Chooser"
            lblTitle.Text = "Select the type of " & LCase(deviceTypeValue) & " you have, then be " & "sure to click the Properties... button to configure the driver for your " & LCase(deviceTypeValue) & "."
            selectedProgIdValue = ""

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

            If AlpacaEnabled Then
                Dim discoveryThread As Thread = New Thread(New Threading.ThreadStart(AddressOf StartAlpacaDiscovery))
                'StartAlpacaDiscovery()
                discoveryThread.Start()
            Else
                PopulateDriverComboBox()
                SetStateNoAlpaca()

                ' Set up a one-off timer in order to force display of the warning message if the pre-selected driver is not compatible
                initialMessageTimer = New System.Windows.Forms.Timer
                initialMessageTimer.Interval = FORM_LOAD_WARNING_MESSAGE_DELAY_TIME ' Set it to fire after 250ms
                initialMessageTimer.Start() ' Kick off the timer
            End If

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
        End If
        MyBase.Dispose(Disposing)
    End Sub

#End Region

#Region "Public methods"

    ' Isolate this form from the rest of the component
    ' Return of "" indicates error or Cancel clicked
    Public ReadOnly Property SelectedProgId() As String
        Get
            Return selectedProgIdValue
        End Get
    End Property

    Public WriteOnly Property DeviceType() As String
        Set(ByVal Value As String)
            deviceTypeValue = Value
            TL.LogMessage("DeviceType Set", deviceTypeValue)
            ReadState(deviceTypeValue)
        End Set
    End Property

    Public WriteOnly Property InitiallySelectedProgId() As String
        Set(ByVal Value As String)
            initiallySelectedProgIdValue = Value
            TL.LogMessage("InitiallySelectedProgId Set", initiallySelectedProgIdValue)
        End Set
    End Property

#End Region

#Region "Form and timer event handlers"

    Private Sub ChooserFormMoveEventHandler(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        If currentWarningMesage <> "" Then WarningToolTipShow(currentWarningTitle, currentWarningMesage)
    End Sub

    Private Sub ChooserForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim SolidBrush As New SolidBrush(Color.Black), LinePen As Pen

        'Routine to draw horizontal line on the ASCOM Chooser form
        LinePen = New Pen(SolidBrush, 1)
        e.Graphics.DrawLine(LinePen, 14, 103, Me.Width - 20, 103)
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
        Dim sProgID As String = ""
        Dim ProgIdType As Type
        Dim UseCreateObject As Boolean = False

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        cb = CmbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        For Each Driver As Generic.KeyValuePair(Of String, String) In driversList
            If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                If LCase(Driver.Key.ToString) = LCase(CmbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
            Else 'Description is present
                If LCase(Driver.Value.ToString) = LCase(CmbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
            End If
        Next
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
                bConnected = oDrv.Connected
            Catch
                Try : bConnected = oDrv.Link : Catch : End Try
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
        Dim cb As System.Windows.Forms.ComboBox

        cb = CmbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        For Each Driver As Generic.KeyValuePair(Of String, String) In driversList
            TL.LogMessage("OK Click", "Processing ProgID: " & Driver.Key.ToString & ", Description: @" & Driver.Value.ToString & "@")
            If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                If LCase(Driver.Key.ToString) = LCase(cb.SelectedItem.ToString) Then
                    selectedProgIdValue = Driver.Key.ToString
                    TL.LogMessage("OK Click", "  Description is missing... selecting ProgID: " & selectedProgIdValue)
                End If
            Else
                If LCase(Driver.Value.ToString) = LCase(cb.SelectedItem.ToString) Then
                    selectedProgIdValue = Driver.Key.ToString
                    TL.LogMessage("OK Click", "  Description is present... selecting ProgID: " & selectedProgIdValue)
                End If
            End If
        Next
        TL.LogMessage("OK Click", "Returning ProgID: " & selectedProgIdValue)
        Me.Hide()
    End Sub

    Private Sub cbDriverSelector_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmbDriverSelector.SelectionChangeCommitted
        ValidateDriver()
    End Sub

    Private Sub picASCOM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picASCOM.Click
        Try
            Process.Start("http://ASCOM-Standards.org/")
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
        StartAlpacaDiscovery()
    End Sub

    Private Sub MnuDisableDiscovery_Click(sender As Object, e As EventArgs) Handles MnuDisableDiscovery.Click
        AlpacaEnabled = False
        WriteState(deviceTypeValue)
        SetStateNoAlpaca()
    End Sub

    Private Sub MnuDiscoverNow_Click(sender As Object, e As EventArgs) Handles MnuDiscoverNow.Click
        TL.LogMessage("Menu", "DiscoverNow Clicked!")
        StartAlpacaDiscovery()
        TL.LogMessage("Menu", "DiscoverNow Completed!")
    End Sub

    Private Sub MnuConfigureDiscovery_Click(sender As Object, e As EventArgs) Handles MnuConfigureDiscovery.Click
        Dim alpacaConfigurationForm As ChooserAlpacaConfigurationForm
        Dim outcome As DialogResult

        TL.LogMessage("ConfigureDiscovery", $"About to create Alpaca configuration form")
        alpacaConfigurationForm = New ChooserAlpacaConfigurationForm(Me) ' Create a new configuration form
        alpacaConfigurationForm.ShowDialog() ' Display the form as a modal dialogue box
        TL.LogMessage("ConfigureDiscovery", $"Exited Alpaca configuration form. Result: {alpacaConfigurationForm.DialogResult.ToString()}")

        If alpacaConfigurationForm.DialogResult = DialogResult.OK Then ' If the user clicked OK then persist the new state
            TL.LogMessage("ConfigureDiscovery", $"Persisting new configuration for {deviceTypeValue}")
            WriteState(deviceTypeValue)
        End If

        alpacaConfigurationForm.Dispose() ' Dispose of the configuration form

    End Sub

#End Region

#Region "State Persistence"

    Private Sub ReadState(DeviceType As String)
        Dim registry As RegistryAccess

        Try
            TL?.LogMessageCrLf("ChooserReadState", $"Reading state for device type: {DeviceType}. Configuration key: {CONFIGRATION_SUBKEY}, Alpaca enabled: {$"{DeviceType} {ALPACA_ENABLED}"}, ALapca default: {ALPACA_ENABLED_DEFAULT}")
            registry = New RegistryAccess

            ' The enabled state is per device type
            AlpacaEnabled = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", ALPACA_ENABLED_DEFAULT), CultureInfo.InvariantCulture)

            ' These values are for all Alpaca devices
            AlpacaDiscoveryPort = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, ALPACA_DISCOVERY_PORT_DEFAULT), CultureInfo.InvariantCulture)
            AlpacaNumberOfBroadcasts = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, ALPACA_NUMBER_OF_BROADCASTS_DEFAULT), CultureInfo.InvariantCulture)
            AlpacaTimeout = Convert.ToInt32(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, ALPACA_TIMEOUT_DEFAULT), CultureInfo.InvariantCulture)
            AlpacaDnsResolution = Convert.ToBoolean(registry.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, ALPACA_DNS_RESOLUTION_DEFAULT), CultureInfo.InvariantCulture)

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

    Private Sub StartAlpacaDiscovery()

        Try

            TL.LogMessage("StartAlpacaDiscovery", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

            ' Render the user interface unresponsive while discovery is underway, except for the Cancel button.
            SetStateAlpacaDiscovering()

            Dim discovery As AlpacaDiscovery
            discovery = New AlpacaDiscovery(TL)
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery created")
            discovery.StartDiscovery(AlpacaNumberOfBroadcasts, 200, AlpacaDiscoveryPort, AlpacaTimeout, False)
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery started")

            Do

                Threading.Thread.Sleep(10)
                Application.DoEvents()

            Loop Until discovery.DiscoveryComplete
            TL.LogMessage("StartAlpacaDiscovery", $"AlpacaDiscovery Completed")

            For Each ascomDevice As AscomDevice In discovery.GetAscomDevices()
                TL.LogMessage("StartAlpacaDiscovery", $"FOUND {ascomDevice.AscomDeviceType} {ascomDevice.AscomDeviceName} {ascomDevice.IPEndPoint.ToString()}")
            Next
            ascomDevices = discovery.GetAscomDevices(deviceTypeValue)

            PopulateDriverComboBox()

            discovery.Dispose()
        Catch ex As Exception
            TL.LogMessageCrLf("StartAlpacaDiscovery", ex.ToString())
        Finally
            ' Restore a usable user interface
            If ascomDevices.Count > 0 Then
                SetStateAlpacaDiscoveryCompleteFoundDevices()
            Else
                SetStateAlpacaDiscoveryCompleteNoDevices()
            End If

        End Try
    End Sub

    Private PopulateDriverComboBoxDelegate As MethodInvoker = AddressOf PopulateDriverComboBox

    Private Sub PopulateDriverComboBox()
        Dim profileStore As RegistryAccess
        Dim i, iSel As Integer
        Dim description As String

        If CmbDriverSelector.InvokeRequired Then
            TL.LogMessage("PopulateDriverComboBox", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}")
            CmbDriverSelector.Invoke(PopulateDriverComboBoxDelegate)
        Else
            Try
                TL.LogMessage("PopulateDriverComboBox", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

                profileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

                ' Enumerate the available drivers, and load their descriptions and ProgIDs into the list box. Key is ProgID, value is friendly name.
                Try 'Get the list of drivers of this device type
                    driversList = profileStore.EnumKeys(deviceTypeValue & " Drivers") ' Get Key-Class pairs
                    'Now list the drivers 
                    For Each Driver As Generic.KeyValuePair(Of String, String) In driversList
                        TL.LogMessage("PopulateDriverComboBox", "Found ProgID: " & Driver.Key.ToString & ", Description: @" & Driver.Value.ToString & "@")
                        If Driver.Value = "" Then
                            TL.LogMessage("PopulateDriverComboBox", "  ***** Description missing for ProgID: " & Driver.Key.ToString)
                        End If
                    Next
                Catch ex1 As Exception
                    TL.LogMessageCrLf("PopulateDriverComboBox", "Exception: " & ex1.ToString)
                    'Ignore any exceptions from this call e.g. if there are no devices of that type installed just create an empty list
                    driversList = New Generic.SortedList(Of String, String)
                End Try

                TL.LogMessage("PopulateDriverComboBox", $"Completed driver enumeration")
                TL.LogMessage("PopulateDriverComboBox", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}")

                ' Populate the driver selection combo box with driver friendly names
                CmbDriverSelector.Items.Clear()
                If (driversList.Count = 0) And (ascomDevices.Count = 0) Then
                    MsgBox("There are no ASCOM " & deviceTypeValue & " drivers installed.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
                Else
                    TL.LogMessage("PopulateDriverComboBox", $"Adding ASCOM COM devices")
                    For Each driver As Generic.KeyValuePair(Of String, String) In driversList
                        TL.LogMessage("PopulateDriverComboBox", $"Adding ASCOM device {driver.Key} {driver.Value}")
                        description = driver.Value ' Set the device description
                        If driver.Value = "" Then description = driver.Key 'Deal with the possibility that it is an empty string, i.e. the driver author has forgotten to set it!
                        CmbDriverSelector.Items.Add(description) ' Add items & allow to sort
                    Next

                    TL.LogMessage("PopulateDriverComboBox", $"Adding Alpaca devices")

                    For Each device As AscomDevice In ascomDevices
                        TL.LogMessage("PopulateDriverComboBox", $"Adding Alpaca device {device.AscomDeviceType} {device.AscomDeviceName}")

                        CmbDriverSelector.Items.Add($"ALPACA - NEW DEVICE   {device.AscomDeviceName}   ({device.IPEndPoint.ToString()} - {deviceTypeValue}/{device.AlpacaDeviceNumber})")
                    Next
                End If

                ' Find the description corresponding to the set ProgID
                For Each driver As Generic.KeyValuePair(Of String, String) In driversList
                    If LCase(initiallySelectedProgIdValue) = LCase(driver.Key.ToString) Then description = driver.Value.ToString
                Next

                ' Find the index of the combo box item that matches the description derived from the ProgID
                iSel = -1
                i = -1
                For Each Desc As String In CmbDriverSelector.Items
                    i += 1
                    If LCase(description) = LCase(Desc) Then iSel = i
                Next

                If iSel >= 0 Then ' If a match was found select the item
                    CmbDriverSelector.SelectedIndex = iSel ' Jump list to that
                    BtnOK.Enabled = True ' Enable the OK button
                Else ' Match not found so don't pre-select an item
                    CmbDriverSelector.SelectedIndex = -1
                End If

                ' Check that the selected driver is valid
                ValidateDriver()

                profileStore.Dispose() 'Close down the profile store
            Catch ex As Exception
                TL.LogMessageCrLf("PopulateDriverComboBox Top", "Exception: " & ex.ToString)

            End Try
        End If
    End Sub

    Public SetStateNoAlpacaDelegate As MethodInvoker = AddressOf SetStateNoAlpaca
    Public SetStateAlpacaDiscoveringDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscovering
    Public SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscoveryCompleteFoundDevices
    Public SetStateAlpacaDiscoveryCompleteNoDevicesDelegate As MethodInvoker = AddressOf SetStateAlpacaDiscoveryCompleteNoDevices

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
            MnuConfigureDiscovery.Enabled = True
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
            MnuConfigureDiscovery.Enabled = False
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
            MnuConfigureDiscovery.Enabled = True
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
            MnuConfigureDiscovery.Enabled = True
            BtnProperties.Enabled = True
            BtnOK.Enabled = True
            AlpacaStatus.Visible = True
            AlpacaStatus.BackColor = Color.Red
            alpacaStatusIndicatorTimer.Stop()
        End If
    End Sub

    Private Sub ValidateDriver()
        Dim deviceInitialised As String, ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        If CmbDriverSelector.SelectedIndex >= 0 Then ' Something selected

            WarningTooltipClear() 'Hide any previous message

            'Find ProgID corresponding to description
            For Each Driver As Generic.KeyValuePair(Of String, String) In driversList
                If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                    If LCase(Driver.Key.ToString) = LCase(CmbDriverSelector.SelectedItem.ToString) Then selectedProgIdValue = Driver.Key.ToString
                Else 'Description is present
                    If LCase(Driver.Value.ToString) = LCase(CmbDriverSelector.SelectedItem.ToString) Then selectedProgIdValue = Driver.Key.ToString
                End If
            Next
            TL.LogMessage("DriverSelected", "ProgID:" & selectedProgIdValue & ", Bitness: " & ApplicationBits.ToString)
            driverIsCompatible = VersionCode.DriverCompatibilityMessage(selectedProgIdValue, ApplicationBits, TL) 'Get compatibility warning message, if any

            If driverIsCompatible <> "" Then 'This is an incompatible driver
                BtnProperties.Enabled = False ' So prevent access!
                BtnOK.Enabled = False
                TL.LogMessage("DriverSelected", "Showing incompatible driver message")
                WarningToolTipShow("Incompatible Driver (" & selectedProgIdValue & ")", driverIsCompatible)
            Else
                BtnProperties.Enabled = True ' Turn on Properties
                deviceInitialised = ProfileStore.GetProfile("Chooser", selectedProgIdValue & " Init")
                If LCase(deviceInitialised) = "true" Then
                    BtnOK.Enabled = True ' This device has been initialized
                    currentWarningMesage = ""
                    TL.LogMessage("DriverSelected", "Driver is compatible and configured so no message")
                Else
                    BtnOK.Enabled = False ' Ensure OK is enabled
                    TL.LogMessage("DriverSelected", "Showing first time configuration required message")
                    WarningToolTipShow(TOOLTIP_PROPERTIES_TITLE, TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE)
                End If
            End If
        Else ' Nothing has been selected
            TL.LogMessage("DriverSelected", "Nothing has been selected")
            BtnProperties.Enabled = False
            BtnOK.Enabled = False
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