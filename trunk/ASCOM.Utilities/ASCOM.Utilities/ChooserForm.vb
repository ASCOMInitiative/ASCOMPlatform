Option Strict Off
Option Explicit On
Imports System.Collections
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces

Friend Class ChooserForm
    Inherits System.Windows.Forms.Form

    '---------------------------------------------------------------------
    ' 21-Feb-09 pwgs    5.1.0 - Refactored for Utilities
    '---------------------------------------------------------------------

    Private Const ALERT_TITLE As String = "ASCOM Chooser"

    Private m_sDeviceType, m_sResult, m_sStartSel As String
    Private m_Drivers As Generic.SortedList(Of String, String)
    Private WithEvents ToolTipMsg As ToolTip

    Private Sub ChooserForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim ProfileStore As RegistryAccess
        Dim i, iSel As Integer
        Dim sDescription As String = ""
        Dim TraceFileName As String
        Dim Description As String
        '
        ' Enumerate the available ASCOM scope drivers, and
        ' load their descriptions and ProgIDs into the
        ' list box. Key is ProgID, value is friendly name.
        '
        'MsgBox("ChooserformLoad Start")
        Try
            ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
            Try 'Get the list of drivers of this device type
                m_Drivers = ProfileStore.EnumKeys(m_sDeviceType & " Drivers") ' Get Key-Class pairs
            Catch ex1 As Exception
                'Ignore any exceptions from this call e.g. if there are no devices of that type installed
                'Just create an empty list
                m_Drivers = New Generic.SortedList(Of String, String)
            End Try

            'cbDriverSelector = Me.cbDriverSelector ' Handy shortcut
            cbDriverSelector.Items.Clear()
            If m_Drivers.Count = 0 Then
                MsgBox("There are no ASCOM " & m_sDeviceType & " drivers installed.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_TITLE)
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

            '
            ' Now items in list are sorted. Preselect.
            '
            'Find the description corresponding to the set progid
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

            TraceFileName = ProfileStore.GetProfile("", SERIAL_FILE_NAME_VARNAME)
            Select Case TraceFileName
                Case "" 'Trace is disabled
                    MenuUseTraceAutoFilenames.Enabled = True 'Autofilenames are enabled but unchecked
                    MenuUseTraceAutoFilenames.Checked = False
                    MenuUseTraceManualFilename.Enabled = True 'Manual trace filename is enabled but unchecked
                    MenuUseTraceManualFilename.Checked = False
                    MenuSerialTraceEnabled.Checked = False 'The trace enabled flag is unchecked and disabled
                    MenuSerialTraceEnabled.Enabled = False
                Case SERIAL_AUTO_FILENAME 'Tracing is on using an automatic filename
                    MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and checked
                    MenuUseTraceAutoFilenames.Checked = True
                    MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is dis enabled and unchecked
                    MenuUseTraceManualFilename.Checked = False
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
                Case Else 'Tracing using some other fixed filename
                    MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and unchecked
                    MenuUseTraceAutoFilenames.Checked = False
                    MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is disabled enabled and checked
                    MenuUseTraceManualFilename.Checked = True
                    MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                    MenuSerialTraceEnabled.Enabled = True
            End Select

            'Set Profile trace checked state on menu item 
            MenuProfileTraceEnabled.Checked = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT)
            MenuUtilTraceEnabled.Checked = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)
            MenuTransformTraceEnabled.Checked = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT)

            MenuIncludeSerialTraceDebugInformation.Checked = GetBool(SERIAL_TRACE_DEBUG, SERIAL_TRACE_DEBUG_DEFAULT)

            ' Assure window pops up on top of others.
            'Me.BringToFront()

            ProfileStore.Dispose() 'Close down the profile store
            ProfileStore = Nothing
        Catch ex As Exception
            MsgBox("ChooserForm Load " & ex.ToString)
            LogEvent("ChooserForm Load ", ex.ToString, System.Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString)
        End Try
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

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        cb = Me.cbDriverSelector ' Convenient shortcut

        'Find ProgID corresponding to description
        For Each de As Generic.KeyValuePair(Of String, String) In m_Drivers
            If LCase(de.Value.ToString) = LCase(cb.SelectedItem.ToString) Then sProgID = de.Key.ToString
        Next
        Try
            'oDrv = CreateObject(sProgID) ' Rob suggests that Activator.CreateInstance gives better error diagnostics
            ProgIdType = Type.GetTypeFromProgID(sProgID)
            oDrv = Activator.CreateInstance(ProgIdType)

            ' Here we try to see if a device is already connected. If so, alert and just turn on the OK button.
            bConnected = False
            Try
                bConnected = oDrv.Connected
            Catch
                Try : bConnected = oDrv.Link : Catch : End Try
            End Try
            If bConnected Then
                MsgBox("The device is already connected. Just click OK.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_TITLE)
            Else
                Try
                    oDrv.SetupDialog()
                Catch ex As Exception
                    MsgBox("Driver setup method failed: """ & sProgID & """ " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_TITLE)
                    LogEvent("ChooserForm", "Driver setup method failed for driver: """ & sProgID & """", Diagnostics.EventLogEntryType.Error, EventLogErrors.ChooserSetupFailed, ex.ToString)
                End Try
            End If
            ProfileStore.WriteProfile("Chooser", sProgID & " Init", "True") ' Remember it has been initialized
            Me.cmdOK.Enabled = True
        Catch ex As Exception
            MsgBox("Failed to load driver: """ & sProgID & """ " & ex.ToString, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_TITLE)
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
        For Each de As Generic.KeyValuePair(Of String, String) In m_Drivers
            If LCase(de.Value.ToString) = LCase(cb.SelectedItem.ToString) Then m_sResult = de.Key.ToString
        Next

        Me.Hide()
    End Sub

    Private Sub cbDriverSelector_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbDriverSelector.SelectionChangeCommitted
        Dim sProgID As String = "", DriverIsCompatible As String
        Dim buf As String, ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        If Me.cbDriverSelector.SelectedIndex >= 0 Then ' Something selected
            'Find ProgID corresponding to description
            For Each de As Generic.KeyValuePair(Of String, String) In m_Drivers
                If LCase(de.Value.ToString) = LCase(Me.cbDriverSelector.SelectedItem.ToString) Then sProgID = de.Key.ToString
            Next

            DriverIsCompatible = VersionCode.DriverCompatibilityMessage(sProgID, ApplicationBits)
            If DriverIsCompatible <> "" Then 'This is a 32bit driver being accessed by a 64bit application
                Me.cmdProperties.Enabled = False ' So prevent access!
                Me.cmdOK.Enabled = False
                'If we don't have focus then the baloon arrow has a habit of pointing away from the combo box so only display when we do have focus
                'Likewise if we don't create the tooltip new each time it points upwards after the first use.
                If Me.ContainsFocus Then 'The mouse is within the Chooser form so the baloon arrow will point downwards towards the combo box
                    If Not ToolTipMsg Is Nothing Then
                        ToolTipMsg.Hide(cbDriverSelector)
                        ToolTipMsg.Dispose()
                        ToolTipMsg = Nothing
                    End If
                    ToolTipMsg = New ToolTip()
                    ToolTipMsg.ToolTipIcon = ToolTipIcon.Warning 'Set tooltip properties
                    ToolTipMsg.AutoPopDelay = 5000
                    ToolTipMsg.InitialDelay = 0
                    ToolTipMsg.IsBalloon = True
                    ToolTipMsg.ReshowDelay = 100
                    ToolTipMsg.ToolTipTitle = "Incompatible Driver"
                    If DriverIsCompatible.Contains(vbCrLf) Then
                        ToolTipMsg.Show(DriverIsCompatible, cbDriverSelector, 50, -87) 'Position for a two line message
                    Else
                        ToolTipMsg.Show(DriverIsCompatible, cbDriverSelector, 50, -70) 'Position for a one line message
                    End If
                End If
            Else
                If Not ToolTipMsg Is Nothing Then
                    ToolTipMsg.Hide(Me)
                    ToolTipMsg.Dispose()
                    ToolTipMsg = Nothing
                End If

                Me.cmdProperties.Enabled = True ' Turn on Properties
                buf = ProfileStore.GetProfile("Chooser", sProgID & " Init")
                If LCase(buf) = "true" Then
                    Me.cmdOK.Enabled = True ' This device has been initialized
                Else
                    Me.cmdOK.Enabled = False ' Never been initialized
                End If
            End If
            Else ' Nothing has been selected
                Me.cmdProperties.Enabled = False
                Me.cmdOK.Enabled = False
            End If

            ProfileStore.Dispose() 'Clean up profile store
            ProfileStore = Nothing
    End Sub

    Private Sub picASCOM_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picASCOM.Click
        Try
            Process.Start("http://ASCOM-Standards.org/")
        Catch ex As Exception
            MsgBox("Unable to display ASCOM-Standards web site in your browser: " & ex.Message, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), ALERT_TITLE)
        End Try
    End Sub

    Private Sub ChooserForm_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        'Routine to draw horizontal line on the ASCOM Chooser form
        'Fired from the chooserform.paint event

        Dim SolidBrush As New SolidBrush(Color.Black)
        Dim LinePen As Pen
        LinePen = New Pen(SolidBrush, 1)
        e.Graphics.DrawLine(LinePen, 14, 103, Me.Width - 20, 103)
    End Sub

    Private Sub MenuAutoTraceFilenames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUseTraceAutoFilenames.Click
        Dim ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        'Auto filenames currently disabled, so enable them
        MenuUseTraceAutoFilenames.Checked = True 'Enable the auto tracename flag
        MenuUseTraceAutoFilenames.Enabled = False
        MenuUseTraceManualFilename.Checked = False 'Unset the manual file flag
        MenuUseTraceManualFilename.Enabled = False
        MenuSerialTraceEnabled.Enabled = True 'Set the trace enabled flag
        MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
        ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub MenuSerialTraceFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUseTraceManualFilename.Click
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
                MenuUseTraceAutoFilenames.Checked = False
                MenuUseTraceAutoFilenames.Enabled = False
                MenuUseTraceManualFilename.Checked = True
                MenuUseTraceManualFilename.Enabled = False
            Case Else 'Ignore everything else

        End Select
        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub MenuSerialTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSerialTraceEnabled.Click
        Dim ProfileStore As RegistryAccess

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        MenuSerialTraceEnabled.Checked = False 'Uncheck the enabled flag, make it inaccessible and clear the trace file name
        MenuSerialTraceEnabled.Enabled = False
        ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "")

        'Enable the set trace options
        MenuUseTraceManualFilename.Enabled = True
        MenuUseTraceManualFilename.Checked = False
        MenuUseTraceAutoFilenames.Enabled = True
        MenuUseTraceAutoFilenames.Checked = False
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

    Private Sub MenuIncludeSerialTraceDebugInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIncludeSerialTraceDebugInformation.Click
        MenuIncludeSerialTraceDebugInformation.Checked = Not MenuIncludeSerialTraceDebugInformation.Checked 'Invert selection
        SetName(SERIAL_TRACE_DEBUG, MenuIncludeSerialTraceDebugInformation.Checked.ToString)
    End Sub

    'This routine fires the driver compatibility test when the mouse enters the chooser for the first time.
    'This ensures that drivers that are pre-selected but are not compatible display a compatibility message if appropriate
    Private Sub ChooserForm_MouseEnter(ByVal e As Object, ByVal sender As EventArgs) Handles Me.MouseEnter
        Static DoneOnce As Boolean = False 'Static variable to last lifetime of the app
        'Only do this once to avoid excessive resource use
        If Not DoneOnce Then cbDriverSelector_SelectedIndexChanged(cbDriverSelector, New System.EventArgs()) ' Also dims OK
        DoneOnce = True 'Set flag to ensure only done once

    End Sub
End Class