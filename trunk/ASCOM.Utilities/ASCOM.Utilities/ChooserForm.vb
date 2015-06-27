Option Strict Off
Option Explicit On
Imports System.Collections
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces
Imports System.Drawing
Imports System.Drawing.Drawing2D

Friend Class ChooserForm
    Inherits System.Windows.Forms.Form

    Private Const ALERT_MESSAGEBOX_TITLE As String = "ASCOM Chooser"
    Private Const PROPERTIES_TOOLTIP_DISPLAY_TIME As Integer = 5000 ' Time to display the Properties tooltip (milliseconds)
    Private Const FORM_LOAD_WARNING_MESSAGE_DELAY_TIME As Integer = 250 ' Delay time before any warning message is displayed on form load
    Private Const TOOLTIP_PROPERTIES_TITLE As String = "Driver Setup"
    Private Const TOOLTIP_PROPERTIES_MESSAGE As String = "Check or change driver Properties (configuration)"
    Private Const TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE As String = "You must check driver configuration before first time use, please click the Properties... button." & vbCrLf & "The OK button will remain greyed out until this is done."

    Private m_sDeviceType, m_sResult, m_sStartSel, sProgID As String
    Private m_Drivers As Generic.SortedList(Of String, String)
    Private ChooserWarningToolTip As ToolTip
    Private ChooserPropertiesToolTip As ToolTip
    Private DriverIsCompatible As String = ""
    Dim CurrentWarningTitle, CurrentWarningMesage As String
    Private TL As TraceLogger
    Private WithEvents InitialMessageTimer As System.Windows.Forms.Timer

    Private Sub ChooserForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'Clean up the trace logger
        TL.Enabled = False
        TL.Dispose()
    End Sub

    Private Sub ChooserForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim ProfileStore As RegistryAccess
        Dim i, iSel As Integer
        Dim sDescription As String = ""
        Dim Description As String

        Try
            'Create the trace logger
            TL = New TraceLogger("", "ChooserForm")
            TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)

            'Configure the tooltip warning for 32/64bit driver compatibility messages
            ChooserWarningToolTip = New ToolTip()

            ' Configure the Properties button tooltip
            ChooserPropertiesToolTip = New ToolTip()
            ChooserPropertiesToolTip.IsBalloon = True
            ChooserPropertiesToolTip.ToolTipIcon = ToolTipIcon.Info
            ChooserPropertiesToolTip.UseFading = True
            ChooserPropertiesToolTip.ToolTipTitle = TOOLTIP_PROPERTIES_TITLE
            ChooserPropertiesToolTip.SetToolTip(cmdProperties, TOOLTIP_PROPERTIES_MESSAGE)

            ' Enumerate the available ASCOM scope drivers, and
            ' load their descriptions and ProgIDs into the
            ' list box. Key is ProgID, value is friendly name.
            ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
            Try 'Get the list of drivers of this device type
                m_Drivers = ProfileStore.EnumKeys(m_sDeviceType & " Drivers") ' Get Key-Class pairs
                'Now list the drivers 
                For Each Driver As Generic.KeyValuePair(Of String, String) In m_Drivers
                    TL.LogMessage("ChooserForm Load", "Found ProgID: " & Driver.Key.ToString & ", Description: @" & Driver.Value.ToString & "@")
                    If Driver.Value = "" Then
                        TL.LogMessage("ChooserForm Load", "  ***** Description missing for ProgID: " & Driver.Key.ToString)
                    End If
                Next
            Catch ex1 As Exception
                TL.LogMessageCrLf("ChooserForm Load", "Exception: " & ex1.ToString)
                'Ignore any exceptions from this call e.g. if there are no devices of that type installed just create an empty list
                m_Drivers = New Generic.SortedList(Of String, String)
            End Try

            cbDriverSelector.Items.Clear()
            If m_Drivers.Count = 0 Then
                MsgBox("There are no ASCOM " & m_sDeviceType & " drivers installed.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            Else
                For Each de As Generic.KeyValuePair(Of String, String) In m_Drivers
                    Description = de.Value ' Set the device description
                    If de.Value = "" Then Description = de.Key 'Deal with the possibility that it is an empty string, i.e. the driver author has forgotton to set it!
                    cbDriverSelector.Items.Add(Description) ' Add items & allow to sort
                Next
            End If

            Me.Text = "ASCOM " & m_sDeviceType & " Chooser"
            Me.lblTitle.Text = "Select the type of " & LCase(m_sDeviceType) & " you have, then be " & "sure to click the Properties... button to configure the driver for your " & LCase(m_sDeviceType) & "."
            m_sResult = ""

            ' Now items in list are sorted. Preselect and find the description corresponding to the set progid
            For Each de As Generic.KeyValuePair(Of String, String) In m_Drivers
                If LCase(m_sStartSel) = LCase(de.Key.ToString) Then sDescription = de.Value.ToString
            Next

            iSel = -1
            i = -1
            For Each Desc As String In cbDriverSelector.Items
                i += 1
                If LCase(sDescription) = LCase(Desc) Then iSel = i
            Next

            If iSel >= 0 Then ' Start selection?
                cbDriverSelector.SelectedIndex = iSel ' Jump list to that
                Me.cmdOK.Enabled = True ' Allow OK, probably already conf'd
            Else
                cbDriverSelector.SelectedIndex = -1
            End If
            cbDriverSelector_SelectedIndexChanged(cbDriverSelector, New System.EventArgs()) ' Also dims OK

            ProfileStore.Dispose() 'Close down the profile store
            ProfileStore = Nothing

            RefreshTraceMenu() ' Refresh the trace menu

            ' Set up a one-off timer in order to force display of the warning message if the pre-selected driver is not compatible
            InitialMessageTimer = New System.Windows.Forms.Timer
            InitialMessageTimer.Interval = FORM_LOAD_WARNING_MESSAGE_DELAY_TIME ' Set it to fire after 250ms
            InitialMessageTimer.Start() ' Kick off the timer

        Catch ex As Exception
            MsgBox("ChooserForm Load " & ex.ToString)
            LogEvent("ChooserForm Load ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
        End Try
    End Sub

    Private Sub TimerEventProcessor(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles InitialMessageTimer.Tick
        ' This event kicks off once triggered by form load in order to force display of the warning message for a driver that is pre-selected by the user.
        InitialMessageTimer.Stop() ' Disable the timer to prevent future events from firing
        InitialMessageTimer.Enabled = False
        TL.LogMessageCrLf("ChooserForm Timer", "Displaying warning message, if there is one")
        cbDriverSelector_SelectedIndexChanged(cbDriverSelector, New System.EventArgs()) ' Forces display of the  warning tooltip because it does not show up when displayed during FORM load
    End Sub

    Private Sub RefreshTraceMenu()
        Dim TraceFileName As String ', ProfileStore As RegistryAccess

        Using ProfileStore As New RegistryAccess

            TraceFileName = ProfileStore.GetProfile("", SERIAL_FILE_NAME_VARNAME)
            Select Case TraceFileName
                Case "" 'Trace is disabled
                    'MenuUseTraceAutoFilenames.Enabled = True 'Autofilenames are enabled but unchecked
                    'MenuUseTraceAutoFilenames.Checked = False
                    'MenuUseTraceManualFilename.Enabled = True 'Manual trace filename is enabled but unchecked
                    'MenuUseTraceManualFilename.Checked = False
                    MenuSerialTraceEnabled.Checked = False 'The trace enabled flag is unchecked and disabled
                    MenuSerialTraceEnabled.Enabled = True
                Case SERIAL_AUTO_FILENAME 'Tracing is on using an automatic filename
                    'MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and checked
                    'MenuUseTraceAutoFilenames.Checked = True
                    'MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is dis enabled and unchecked
                    'MenuUseTraceManualFilename.Checked = False
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
                Case Else 'Tracing using some other fixed filename
                    'MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and unchecked
                    'MenuUseTraceAutoFilenames.Checked = False
                    'MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is disabled enabled and checked
                    'MenuUseTraceManualFilename.Checked = True
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
            End Select

            'Set Profile trace checked state on menu item 
            MenuProfileTraceEnabled.Checked = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT)
            MenuUtilTraceEnabled.Checked = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)
            MenuTransformTraceEnabled.Checked = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT)
            'MenuIncludeSerialTraceDebugInformation.Checked = GetBool(SERIAL_TRACE_DEBUG, SERIAL_TRACE_DEBUG_DEFAULT)
            MenuSimulatorTraceEnabled.Checked = GetBool(SIMULATOR_TRACE, SIMULATOR_TRACE_DEFAULT)
            MenuDriverAccessTraceEnabled.Checked = GetBool(DRIVERACCESS_TRACE, DRIVERACCESS_TRACE_DEFAULT)
            MenuAstroUtilsTraceEnabled.Checked = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT)
            MenuNovasTraceEnabled.Checked = GetBool(NOVAS_TRACE, NOVAS_TRACE_DEFAULT)
        End Using
    End Sub

    Public WriteOnly Property DeviceType() As String
        Set(ByVal Value As String)
            m_sDeviceType = Value
        End Set
    End Property

    Public WriteOnly Property StartSel() As String
        Set(ByVal Value As String)
            m_sStartSel = Value
        End Set
    End Property
    '
    ' Isolate this form from the rest of the component
    ' Return of "" indicates error or Cancel clicked
    '
    Public ReadOnly Property Result() As String
        Get
            Result = m_sResult
        End Get
    End Property
    '
    ' Click in Properties... button. Load the currently selected
    ' driver and activate its setup dialog.
    '
    Private Sub cmdProperties_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdProperties.Click
        Dim ProfileStore As RegistryAccess
        Dim oDrv As Object = Nothing ' The driver
        Dim cb As System.Windows.Forms.ComboBox
        Dim bConnected As Boolean
        Dim sProgID As String = ""
        Dim ProgIdType As Type
        Dim UseCreateObject As Boolean = False

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        cb = Me.cbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        For Each Driver As Generic.KeyValuePair(Of String, String) In m_Drivers
            If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                If LCase(Driver.Key.ToString) = LCase(Me.cbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
            Else 'Description is present
                If LCase(Driver.Value.ToString) = LCase(Me.cbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
            End If
        Next
        TL.LogMessage("PropertiesClick", "ProgID:" & sProgID)
        Try
            ' Mechanic to revert to Platform 5 behaviour in the event that Activator.CreateInstance has unforseen consequqnces
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
                    oDrv.SetupDialog()
                Catch ex As Exception
                    MsgBox("Driver setup method failed: """ & sProgID & """ " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
                    LogEvent("ChooserForm", "Driver setup method failed for driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserSetupFailed, ex.ToString)
                End Try
            End If

            ProfileStore.WriteProfile("Chooser", sProgID & " Init", "True") ' Remember it has been initialized
            Me.cmdOK.Enabled = True
            WarningTooltipClear()
        Catch ex As Exception
            MsgBox("Failed to load driver: """ & sProgID & """ " & ex.ToString, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
            LogEvent("ChooserForm", "Failed to load driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserDriverFailed, ex.ToString)
        End Try

        'Clean up and release resources
        Try : oDrv.Dispose() : Catch ex As Exception : End Try
        Try : Marshal.ReleaseComObject(oDrv) : Catch ex As Exception : End Try
        oDrv = Nothing

        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        m_sResult = ""
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Dim cb As System.Windows.Forms.ComboBox

        cb = Me.cbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        For Each Driver As Generic.KeyValuePair(Of String, String) In m_Drivers
            TL.LogMessage("OK Click", "Processing ProgID: " & Driver.Key.ToString & ", Description: @" & Driver.Value.ToString & "@")
            If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                If LCase(Driver.Key.ToString) = LCase(cb.SelectedItem.ToString) Then
                    sProgID = Driver.Key.ToString
                    TL.LogMessage("OK Click", "  Description is missing... selecting ProgID: " & sProgID)
                End If
            Else
                If LCase(Driver.Value.ToString) = LCase(cb.SelectedItem.ToString) Then
                    m_sResult = Driver.Key.ToString
                    TL.LogMessage("OK Click", "  Description is present... selecting ProgID: " & sProgID)
                End If
            End If
        Next
        TL.LogMessage("OK Click", "Returning ProgID: " & sProgID)
        Me.Hide()
    End Sub

    Private Sub cbDriverSelector_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbDriverSelector.SelectionChangeCommitted
        Dim buf As String, ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        If Me.cbDriverSelector.SelectedIndex >= 0 Then ' Something selected

            WarningTooltipClear() 'Hide any previous message

            'Find ProgID corresponding to description
            For Each Driver As Generic.KeyValuePair(Of String, String) In m_Drivers
                If Driver.Value = "" Then 'Deal with the possibility that the description is missing, in which case use the ProgID as the identifier
                    If LCase(Driver.Key.ToString) = LCase(Me.cbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
                Else 'Description is present
                    If LCase(Driver.Value.ToString) = LCase(Me.cbDriverSelector.SelectedItem.ToString) Then sProgID = Driver.Key.ToString
                End If
            Next
            TL.LogMessage("DriverSelected", "ProgID:" & sProgID & ", Bitness: " & ApplicationBits.ToString)
            DriverIsCompatible = VersionCode.DriverCompatibilityMessage(sProgID, ApplicationBits, TL) 'Get compatibility warning message, if any

            If DriverIsCompatible <> "" Then 'This is an incompatible driver
                Me.cmdProperties.Enabled = False ' So prevent access!
                Me.cmdOK.Enabled = False
                TL.LogMessage("DriverSelected", "Showing incompatible driver message")
                WarningToolTipShow("Incompatible Driver (" & sProgID & ")", DriverIsCompatible)
            Else
                Me.cmdProperties.Enabled = True ' Turn on Properties
                buf = ProfileStore.GetProfile("Chooser", sProgID & " Init")
                If LCase(buf) = "true" Then
                    Me.cmdOK.Enabled = True ' This device has been initialized
                    CurrentWarningMesage = ""
                    TL.LogMessage("DriverSelected", "Driver is compatible and configured so no message")
                Else
                    Me.cmdOK.Enabled = False ' Ensure OK is enabled
                    TL.LogMessage("DriverSelected", "Showing first time configuration required message")
                    WarningToolTipShow(TOOLTIP_PROPERTIES_TITLE, TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE)
                End If
            End If
        Else ' Nothing has been selected
            TL.LogMessage("DriverSelected", "Nothing has been selected")
            Me.cmdProperties.Enabled = False
            Me.cmdOK.Enabled = False
        End If

        ProfileStore.Dispose() 'Clean up profile store
        ProfileStore = Nothing
    End Sub

    Private Sub WarningToolTipShow(Title As String, Message As String)
        WarningTooltipClear()
        ChooserWarningToolTip.UseAnimation = True
        ChooserWarningToolTip.UseFading = False
        ChooserWarningToolTip.ToolTipIcon = ToolTipIcon.Warning
        ChooserWarningToolTip.AutoPopDelay = 5000
        ChooserWarningToolTip.InitialDelay = 0
        ChooserWarningToolTip.IsBalloon = False
        ChooserWarningToolTip.ReshowDelay = 0
        ChooserWarningToolTip.OwnerDraw = False
        ChooserWarningToolTip.ToolTipTitle = Title
        CurrentWarningTitle = Title
        CurrentWarningMesage = Message

        If Message.Contains(vbCrLf) Then
            ChooserWarningToolTip.Show(Message, Me, 18, 24) 'Display at position for a two line message
        Else
            ChooserWarningToolTip.Show(Message, Me, 18, 50) 'Display at position for a one line message
        End If
    End Sub

    Private Sub WarningTooltipClear()
        ChooserWarningToolTip.RemoveAll()
        CurrentWarningTitle = ""
        CurrentWarningMesage = ""
    End Sub

    Private Sub ChooserForm_ShowMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        If CurrentWarningMesage <> "" Then WarningToolTipShow(CurrentWarningTitle, CurrentWarningMesage)
    End Sub

    Private Sub ChooserForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim SolidBrush As New SolidBrush(Color.Black), LinePen As Pen

        'Routine to draw horizontal line on the ASCOM Chooser form
        LinePen = New Pen(SolidBrush, 1)
        e.Graphics.DrawLine(LinePen, 14, 103, Me.Width - 20, 103)
    End Sub

    Private Sub picASCOM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picASCOM.Click
        Try
            Process.Start("http://ASCOM-Standards.org/")
        Catch ex As Exception
            MsgBox("Unable to display ASCOM-Standards web site in your browser: " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_MESSAGEBOX_TITLE)
        End Try
    End Sub

    Private Sub MenuAutoTraceFilenames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        'Auto filenames currently disabled, so enable them
        'MenuUseTraceAutoFilenames.Checked = True 'Enable the auto tracename flag
        'MenuUseTraceAutoFilenames.Enabled = False
        'MenuUseTraceManualFilename.Checked = False 'Unset the manual file flag
        'MenuUseTraceManualFilename.Enabled = False
        MenuSerialTraceEnabled.Enabled = True 'Set the trace enabled flag
        MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
        ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub MenuSerialTraceFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ProfileStore As RegistryAccess
        Dim RetVal As System.Windows.Forms.DialogResult

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        RetVal = SerialTraceFileName.ShowDialog()
        Select Case RetVal
            Case Windows.Forms.DialogResult.OK
                'Save the reault
                ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName)
                'Check and enable the serial trace enabled flag
                MenuSerialTraceEnabled.Enabled = True
                MenuSerialTraceEnabled.Checked = True
                'Enable maual serial trace file flag
                'MenuUseTraceAutoFilenames.Checked = False
                'MenuUseTraceAutoFilenames.Enabled = False
                'MenuUseTraceManualFilename.Checked = True
                'MenuUseTraceManualFilename.Enabled = False
            Case Else 'Ignore everything else

        End Select
        ProfileStore.Dispose()
        ProfileStore = Nothing
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
        ProfileStore = Nothing
    End Sub

    Private Sub MenuProfileTraceEnabled_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuProfileTraceEnabled.Click
        MenuProfileTraceEnabled.Checked = Not MenuProfileTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_XMLACCESS, MenuProfileTraceEnabled.Checked.ToString)
        SetName(TRACE_PROFILE, MenuProfileTraceEnabled.Checked.ToString)
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

    Private Sub MenuTrace_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTrace.DropDownOpening
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
End Class