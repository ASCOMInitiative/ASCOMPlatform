Attribute VB_Name = "Common"
'---------------------------------------------------------------------
'   ==========
'   COMMON.BAS
'   ==========
'
' Common constants and functions shared by Telescope and Focuser
'
' Implements shared connections for the Meade Telescope/Focuser driver.
' This is necessary, because the focuser and telescope commands must
' be issued on the same COM port.
'
' Written:  February 2nd, 2002   Chris Creery <ccreery@cyanogen.com>
'
' Edits:
'
' When          Who What
' ----------    --- --------------------------------------------------
' 02-Feb-2002   CC  1.5.0   Initial edit
' 07-May-2002   rbd 1.8.3   Name changes to g_xxx
' 18-May-2002   rbd 1.8.4   Change to out-of-proc server to support
'                           ownership of Telescope and Focuser by
'                           different processes. Add logic to prevent
'                           creating multiple instances of interfaces.
' 18-May-2002   rbd 1.8.4   Move ID's and DESC here, they are common.
'                           Rename module to Common. Add Who parameter
'                           to OpenConnection() and CloseConnection()
'                           to avoid the need for setting global after
'                           calling OpenConnection() and before
'                           calling CloseConnection(). Move common GS
'                           scope test to OpenConnection(). Move
'                           common port setting from registry here.
' 18-Jul-02     rbd 1.8.6   Two drivers share same soruces, one the
'                           is the DLL version, the other is the EXE.
' 30-Sep-2002   rbd 2.1.0   Fix error handling in OpenConnection()
' 04-Jan-2003   rbd 2.1.2   Reduce initial timeout from 5 to 2 sec,
'                           set 2 sec if exit from OpenConnection()
'                           for "already open".
' 03-Jan-2004   rbd 4.1.0   Move scope detection to Telescope, new
'                           LX200GPS startup will reboot a parked
'                           scope.
' 11-Oct-2005   rbd 4.1.7   New routine ShowHelp() added here.
' 27-Dec-2005   rbd 4.1.9   Make receive timeout constant, increase to
'                           5 sec for Ray Gralak's PEMPro.
' 30-Sep-2006   jab 4.1.16  GPS split out to new driver
' 20-Jan-2007   jab 4.1.27  rename description/chooser value
' 12-Jun-2007   jab 4.1.30  move old setup data over to new driver
'---------------------------------------------------------------------

Option Explicit

Public Const SWP_NOSIZE            As Long = &H1
Public Const SWP_NOMOVE            As Long = &H2
Public Const SWP_NOZORDER          As Long = &H4
Public Const SWP_NOREDRAW          As Long = &H8
Public Const SWP_NOACTIVATE        As Long = &H10
Public Const SWP_FRAMECHANGED      As Long = &H20
Public Const SWP_SHOWWINDOW        As Long = &H40
Public Const SWP_HIDEWINDOW        As Long = &H80
Public Const SWP_NOCOPYBITS        As Long = &H100
Public Const SWP_NOOWNERZORDER     As Long = &H200
Public Const SWP_NOSENDCHANGING    As Long = &H400

Public Const SWP_DRAWFRAME         As Long = SWP_FRAMECHANGED
Public Const SWP_NOREPOSITION      As Long = SWP_NOOWNERZORDER

Public Const HWND_TOP              As Long = 0
Public Const HWND_BOTTOM           As Long = 1
Public Const HWND_TOPMOST          As Long = -1
Public Const HWND_NOTOPMOST        As Long = -2

Public Const SW_SHOWNORMAL         As Long = 1

Public Declare Function SetWindowPos Lib "user32.dll" ( _
                ByVal hWnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal y As Long, _
                ByVal cx As Long, _
                ByVal cy As Long, _
                ByVal uFLags As Long) As Long

Public Declare Function ShellExecute Lib "shell32" Alias "ShellExecuteA" ( _
                ByVal hWnd As Long, _
                ByVal lpOperation As String, _
                ByVal lpFile As String, _
                ByVal lpParameters As String, _
                ByVal lpDirectory As String, _
                ByVal nShowCmd As Long) As Long

Public Const DESC_OLD As String = "Meade Telescope and Focuser"
Public Const DESC_NEW As String = "Meade Classic and Autostar I"
Public Const SCOPE_ID As String = "MeadeEx.Telescope"
Public Const FOCUSER_ID As String = "MeadeEx.Focuser"
Public Const SCOPEDLL_ID As String = "Meade.Telescope"
Public Const FOCUSERDLL_ID As String = "Meade.Focuser"

Public Const RECEIVE_TIMEOUT As Integer = 5     ' Needed for Ray Gralak's PEM Pro

Public Enum DeviceClass
    devTelescope = 0
    devFocuser = 1
End Enum

Public g_SharedSerial As DriverHelper.Serial    ' Shared serial port
Public g_bTelescopeConnected As Boolean         ' Device state flags
Public g_bFocuserConnected As Boolean
Public g_bTelescopeCreated As Boolean           ' Multiple instance interlocks
Public g_bFocuserCreated As Boolean
Public g_Scope As Telescope
'
' Private objects for code in this module
'
Private oProfile As DriverHelper.Profile
Private oUtil As DriverHelper.Util
'
' ==============
' SERVER STARTUP
' ==============
'
Public Sub Main()

    ' get access to persisted data
    Set oProfile = New DriverHelper.Profile
    
    oProfile.DeviceType = "Focuser"               ' and a Focuser driver
    oProfile.Register FOCUSER_ID, DESC_NEW        ' Self reg (skips if already reg)
    
    oProfile.DeviceType = "Telescope"             ' We're a Telescope driver
    oProfile.Register SCOPE_ID, DESC_NEW          ' Self reg (skips if already reg)
    ' leave oProfile set to Telescope
    
    If App.StartMode = vbSModeStandalone Then
        ' check if all we're doing is registering with ASCOM
        If InStr(Command$, "-r") >= 1 Then
            ' old driver data ought to be moved over
            If (oProfile.GetValue(SCOPE_ID, "") = DESC_OLD) Then _
                CopySetup
                
            ' get rid of the old dll registries
            On Error Resume Next
            oProfile.DeviceType = "Telescope"
            oProfile.Unregister SCOPEDLL_ID
        
            oProfile.DeviceType = "Focuser"
            oProfile.Unregister FOCUSERDLL_ID
            On Error GoTo 0
            Exit Sub
        End If
    
        ' check if all we're doing is unregistering with ASCOM
        If InStr(Command$, "-ur") >= 1 Then
        
            On Error Resume Next
            oProfile.DeviceType = "Telescope"
            oProfile.Unregister SCOPE_ID
        
            oProfile.DeviceType = "Focuser"
            oProfile.Unregister FOCUSER_ID
            On Error GoTo 0
            Exit Sub
        End If
        
        ' Catch the curious who might try to run the driver
        MsgBox "This is an ASCOM driver. It cannot be run stand-alone", _
                (vbOKOnly + vbCritical + vbMsgBoxSetForeground), App.FileDescription
        Exit Sub
    End If
    
    '
    ' We are serving objects, initialize all shared resources
    '
    g_bTelescopeConnected = False
    g_bFocuserConnected = False
    g_bTelescopeCreated = False
    g_bFocuserCreated = False
    
    Set g_SharedSerial = New DriverHelper.Serial
    Set oUtil = New DriverHelper.Util
    oProfile.DeviceType = "Telescope"
    
End Sub

Public Sub OpenConnection(Who As DeviceClass)
    Dim buf As String, code As Long, src As String
    
    Select Case Who
        Case devTelescope:
            g_bTelescopeConnected = True
        Case devFocuser:
            g_bFocuserConnected = True
    End Select
    
    If g_SharedSerial.Connected Then                        ' Open port only once
        g_SharedSerial.ReceiveTimeout = RECEIVE_TIMEOUT     ' Caller expects 5 second timeout
        Exit Sub
    End If

    buf = oProfile.GetValue(SCOPE_ID, "COM Port")
    If buf = "" Then                                        ' Default to COM1
        buf = "1"
        oProfile.WriteValue SCOPE_ID, "COM Port", buf
    End If
    On Error GoTo SERIAL_PORT_FAILED
    g_SharedSerial.Port = CLng(buf)                         ' Set port
    g_SharedSerial.Speed = ps9600                           ' LX200 uses 9600
    g_SharedSerial.ReceiveTimeout = RECEIVE_TIMEOUT         ' 5 second timeout
    g_SharedSerial.Connected = True                         ' Grab the serial port

    ' even though the following is claimed for the GPS, its wasn't conditional, so leave it in - jab
    g_SharedSerial.Transmit "#"                             ' Kick the LX200 GPS in the butt (ESSENTIAL!)
    oUtil.WaitForMilliseconds 500
    g_SharedSerial.ClearBuffers                             ' Clear remaining junk in buffer
    
    Exit Sub


SERIAL_PORT_FAILED:
    g_bTelescopeConnected = False
    g_bFocuserConnected = False
    buf = Err.Description
    code = Err.Number
    src = Err.Source
    Resume SER_FAIL_FIN
    
SER_FAIL_FIN:
    On Error GoTo 0                                         ' Resignal to callers & client
    Err.Raise code, src, buf
    
End Sub

Public Sub CloseConnection(Who As DeviceClass)
    
    Select Case Who
        Case devTelescope:
            g_bTelescopeConnected = False
            Set g_Scope = Nothing
        Case devFocuser:
            g_bFocuserConnected = False
    End Select
    
    If Not (g_SharedSerial Is Nothing) Then                 ' If we ever connected
        g_SharedSerial.ClearBuffers                         ' Always clear serial buffers
        If (Not g_bTelescopeConnected) And (Not g_bFocuserConnected) Then
            g_SharedSerial.Connected = False                ' Release if no longer used
        End If
    End If
    
End Sub

Public Function IsConnected(Who As DeviceClass)
    Select Case Who
        Case devTelescope: IsConnected = g_bTelescopeConnected
        Case devFocuser: IsConnected = g_bFocuserConnected
    End Select

End Function

Public Sub ShowHelp()
    Dim z As Long

    z = ShellExecute(0, "Open", App.Path & "/MeadeHelp.htm", 0, 0, SW_SHOWNORMAL)
    If (z > 0) And (z <= 32) Then
        MsgBox _
            "It doesn't appear that you have a web browser installed " & _
            "on your system.", (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ERR_SOURCE
        Exit Sub
    End If

End Sub

' rename the chooser / description by reading all old state, unregitering from ASCOM,
' reregistering with new description, and writting all data back

Private Sub CopySetup()
    Dim SID As String
    Dim FID As String
    Dim buf(20) As String
    Dim dllam As String
    Dim exam As String
    
    On Error Resume Next
    oProfile.DeviceType = "Telescope"
    
    ' figure out if the dll or exe has been the main driver
    ' only use dll data if its been used and the exe hasn't
    dllam = oProfile.GetValue(SCOPEDLL_ID, "AlignMode")
    exam = oProfile.GetValue(SCOPE_ID, "AlignMode")
    If exam = "" Then
        If dllam = "" Then
            ' nothing to copy
            Exit Sub
        Else
            SID = SCOPEDLL_ID
            FID = FOCUSERDLL_ID
        End If
    Else
        SID = SCOPE_ID
        FID = FOCUSER_ID
    End If
    
    ' read the Scope items
    buf(0) = oProfile.GetValue(SID, "AlignMode")
    buf(1) = oProfile.GetValue(SID, "Aperture")
    buf(2) = oProfile.GetValue(SID, "ApertureArea")
    buf(3) = oProfile.GetValue(SID, "AutoSetTime")
    buf(4) = oProfile.GetValue(SID, "AutoUnpark")
    buf(5) = oProfile.GetValue(SID, "Beep")
    buf(6) = oProfile.GetValue(SID, "COM Port")
    buf(7) = oProfile.GetValue(SID, "FocalLength")
    buf(8) = oProfile.GetValue(SID, "GEM")
    buf(9) = oProfile.GetValue(SID, "LX200 16 Inch")
    buf(10) = oProfile.GetValue(SID, "MaxSlew")
    buf(11) = oProfile.GetValue(SID, "ParkAlt")
    buf(12) = oProfile.GetValue(SID, "ParkAz")
    buf(13) = oProfile.GetValue(SID, "SiteElevation")
    buf(14) = oProfile.GetValue(SID, "SyncDelay")
    
    ' read the Focuser items
    oProfile.DeviceType = "Focuser"
    buf(15) = oProfile.GetValue(FID, "Backlash Steps")
    buf(16) = oProfile.GetValue(FID, "Dynamic Braking")
    buf(17) = oProfile.GetValue(FID, "Final Direction")
    buf(18) = oProfile.GetValue(FID, "Flip Dir")
    buf(19) = oProfile.GetValue(FID, "Max Increment")
    
    ' get rid of the old registries, remake with new description
    oProfile.Unregister FOCUSER_ID
    oProfile.Register FOCUSER_ID, DESC_NEW
    
    oProfile.DeviceType = "Telescope"
    oProfile.Unregister SCOPE_ID
    oProfile.Register SCOPE_ID, DESC_NEW

    ' write the old Scope items
    oProfile.WriteValue SCOPE_ID, "AlignMode", buf(0)
    oProfile.WriteValue SCOPE_ID, "Aperture", buf(1)
    oProfile.WriteValue SCOPE_ID, "ApertureArea", buf(2)
    oProfile.WriteValue SCOPE_ID, "AutoSetTime", buf(3)
    oProfile.WriteValue SCOPE_ID, "AutoUnpark", buf(4)
    oProfile.WriteValue SCOPE_ID, "Beep", buf(5)
    oProfile.WriteValue SCOPE_ID, "COM Port", buf(6)
    oProfile.WriteValue SCOPE_ID, "FocalLength", buf(7)
    oProfile.WriteValue SCOPE_ID, "GEM", buf(8)
    oProfile.WriteValue SCOPE_ID, "LX200 16 Inch", buf(9)
    oProfile.WriteValue SCOPE_ID, "MaxSlew", buf(10)
    oProfile.WriteValue SCOPE_ID, "ParkAlt", buf(11)
    oProfile.WriteValue SCOPE_ID, "ParkAz", buf(12)
    oProfile.WriteValue SCOPE_ID, "SiteElevation", buf(13)
    oProfile.WriteValue SCOPE_ID, "SyncDelay", buf(14)
    
    ' write the old Focuser items
    oProfile.DeviceType = "Focuser"
    oProfile.WriteValue FOCUSER_ID, "Backlash Steps", buf(15)
    oProfile.WriteValue FOCUSER_ID, "Dynamic Braking", buf(16)
    oProfile.WriteValue FOCUSER_ID, "Final Direction", buf(17)
    oProfile.WriteValue FOCUSER_ID, "Flip Dir", buf(18)
    oProfile.WriteValue FOCUSER_ID, "Max Increment", buf(19)
    
    On Error GoTo 0
        
End Sub


