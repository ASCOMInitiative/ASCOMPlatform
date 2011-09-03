Attribute VB_Name = "Common"
'---------------------------------------------------------------------
'   ==========
'   COMMON.BAS
'   ==========
'
' Common constants and functions shared by Telescope, Focuser, and Dome
'
' Implements shared connections for the COMSOFT PC-TCS combined driver.
' This is necessary, because the dome, scope, and focuser commands must
' be issued on the same COM port.
'
' Written:  29-Mar-06   Robert B. Denny
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Nov-06 rbd     Initial edit, stolen from MeadeEx driver
' 30-Aug-06 rbd     4.0.6 - Move ASCOM registration of all interfaces
'                   to app startup.
'---------------------------------------------------------------------
Option Explicit

Public Const SCOPE_ID As String = "PCTCS.Telescope" ' COM port stored under this
Public Const DOME_ID As String = "PCTCS.Dome"
Public Const FOCUSER_ID As String = "PCTCS.Focuser"

Public Const SCOPE_DESC As String = "PC-TCS Telescope"
Public Const DOME_DESC As String = "PC-TCS Dome"
Public Const FOCUSER_DESC As String = "PC-TCS Focuser"


Public Enum DeviceClass
    devTelescope = 0
    devFocuser = 1
    devDome = 2
End Enum

Public g_Util As DriverHelper.Util                  ' Shared instance of Util

Private g_Serial As DriverHelper.Serial             ' Shared serial port
Private g_MNCP As MNCP                              ' Shared MNCP engine
Private g_bTelescopeConnected As Boolean            ' Device state flags
Private g_bFocuserConnected As Boolean
Private g_bDomeConnected As Boolean

'
' Private objects for code in this module
'
Private oProfile As DriverHelper.Profile
'
' ==============
' SERVER STARTUP
' ==============
'
Public Sub Main()
    '
    ' Even double-clicked should register all ASCOM driver interfaces
    '
    Set oProfile = New DriverHelper.Profile
    oProfile.DeviceType = "Dome"                    ' Register Dome interface if needed
    oProfile.Register DOME_ID, DOME_DESC
    oProfile.DeviceType = "Focuser"                 ' Register Focuser interface if needed
    oProfile.Register FOCUSER_ID, FOCUSER_DESC
    oProfile.DeviceType = "Telescope"               ' Must be "Telescope" for later calls
    oProfile.Register SCOPE_ID, SCOPE_DESC

    '
    ' Catch the curious who might try to double-click
    ' this executable.
    '
    If App.StartMode = vbSModeStandalone Then
        Set oProfile = Nothing
        MsgBox "This is an ASCOM driver. It cannot be run stand-alone", _
                (vbOKOnly + vbCritical + vbMsgBoxSetForeground), App.FileDescription
        Exit Sub
    End If
    '
    ' We are serving objects, initialize all shared resources
    '
    g_bTelescopeConnected = False
    g_bFocuserConnected = False
    g_bDomeConnected = False
    Set g_Serial = New DriverHelper.Serial
    Set g_Util = New DriverHelper.Util
    Set g_MNCP = New MNCP
    Set g_MNCP.SerialObject = g_Serial
    
End Sub

Public Sub OpenConnection(Who As DeviceClass)
    Dim buf As String, code As Long, src As String
    
    Select Case Who
        Case devTelescope:
            g_bTelescopeConnected = True
        Case devFocuser:
            g_bFocuserConnected = True
        Case devDome:
            g_bDomeConnected = True
    End Select
    
    If g_Serial.Connected Then Exit Sub                     ' Open port only once
        
    '
    ' (1) Set up the communications link. Default to COM1.
    '     Store COM port under the telescope area
    '
    buf = oProfile.GetValue(SCOPE_ID, "COM Port")
    If buf = "" Then                                        ' Default to COM1
        buf = "1"
        oProfile.WriteValue SCOPE_ID, "COM Port", buf
    End If
    On Error GoTo SERIAL_PORT_FAILED
    g_Serial.port = CLng(buf)                               ' Set port
    g_Serial.Speed = ps9600                                 ' LX200 uses 9600
    g_Serial.ReceiveTimeout = 2                             ' 2 second timeout
    g_Serial.Connected = True                               ' Grab the serial port
    '
    ' (2) Initialize the MNCP protocol.
    '
    On Error GoTo MNCP_DETECT_FAILED
    g_MNCP.Address = 1
    g_MNCP.Retries = 4
    g_MNCP.ResetSlave                                       ' Try to talk to the slave
    '
    ' (3) Determine that there is an ACL scope there.
    '
    C_CommandString "status"                                ' Do a status

    Exit Sub                                                ' No errors -- DONE!

MNCP_DETECT_FAILED:
    CloseConnection Who
    Err.Raise SCODE_NO_PCTCS, _
                ERR_SOURCE, _
                MSG_NO_PCTCS
    Exit Sub
    
SERIAL_PORT_FAILED:
    g_bTelescopeConnected = False
    g_bFocuserConnected = False
    g_bDomeConnected = False
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
        Case devFocuser:
            g_bFocuserConnected = False
        Case devDome:
            g_bDomeConnected = False
    End Select
    
    If g_Serial.Connected Then                              ' If we ever connected
        g_Serial.ClearBuffers                               ' Always clear serial buffers
        If (Not g_bTelescopeConnected) And _
                (Not g_bFocuserConnected) And _
                (Not g_bDomeConnected) Then
            g_Serial.Connected = False                      ' Release if no longer used
        End If
    End If
    
End Sub

Public Function IsConnected(Who As DeviceClass)

    Select Case Who
        Case devTelescope: IsConnected = g_bTelescopeConnected
        Case devFocuser: IsConnected = g_bFocuserConnected
        Case devDome: IsConnected = g_bDomeConnected
    End Select

End Function

Public Property Get SerialConnected() As Boolean             ' Check before opening Setup Dialog

    SerialConnected = g_Serial.Connected
    
End Property

Public Sub C_CommandBlind(ByVal Command As String)

    C_CommandString Command                                 ' Just toss any results
    
End Sub

Public Function C_CommandBool(ByVal Command As String) As Boolean
    Dim c As Long
    Dim d As String, s As String
    
    On Error GoTo CMDBOOL_ERR                               ' This is sort of cheesy
    
    C_CommandString Command                                 ' Only goto errors are non-fatal
    C_CommandBool = True
    Exit Function
    
CMDBOOL_ERR:
    c = Err.Number
    d = Err.Description
    s = Err.Source
    Resume CMDBOOL_RSUM
    
CMDBOOL_RSUM:
    On Error GoTo 0
    C_CommandBool = False
    If c <> SCODE_ACLERR Then
        Err.Raise c, s, d
    ElseIf LCase$(Left$(d, 4)) <> "goto" Then
        Err.Raise c, s, d
    Else
        C_CommandBool = True
    End If
    
End Function

Public Function C_CommandString(ByVal Command As String) As String
    Dim buf As String
    
    g_MNCP.ToSlave Command                                  ' No NL at end...
    buf = g_MNCP.FromSlave()
    If LCase$(Left$(buf, 5)) = "error" Then                 ' Returned an error message
        Select Case CInt(Trim$(Mid$(buf, 6)))               ' Numeric error code
            ' ACL core errors
            Case 0:     buf = "No error"
            Case 1:     buf = "Undefined error"
            Case 2:     buf = "Syntax error"
            Case 10:    buf = "Type Mismatch"
            Case 12:    buf = "Value is read-only"
            Case 13:    buf = "Unsupported command"
            Case 14:    buf = "Unsupported identifier"
            Case 15:    buf = "Command inactive"
            ' ACL Telescope errors
            Case 100:   buf = "Goto - illegal parameter(s)"
            Case 101:   buf = "Goto - object below horizon"
            Case 102:   buf = "Goto - object outside limits"
            ' Others are reserved, should never be seen (ha ha ha)
            Case Else:  buf = "Reserved error code, you shouldn't see this!"
        End Select
        Err.Raise SCODE_ACLERR, ERR_SOURCE, _
                "Low level ACL error from telescope: """ & buf & """"
    End If
    If LCase$(Left$(buf, 2)) <> "ok" Then _
        Err.Raise SCODE_NOT_ACL, ERR_SOURCE, MSG_NOT_ACL    ' If not "ok" then not ACL!
    '
    ' OK was received, the remainder is the returned string
    '
    C_CommandString = Trim$(Mid$(buf, 3))                   ' Remove "ok " and trim
    
End Function


