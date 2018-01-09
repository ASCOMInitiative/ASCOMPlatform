VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Dome"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ACL Dome Driver"
'================================================================================
'   ----------
'    Dome.cls
'   ----------
'
' Implementation of the ASCOM dome driver Dome class for ACL domes
'
' Written:  13-Jan-04   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 13-Jan-04 jab     1.0.0 - initial version
' 23-Nov-04 rbd     4.0.1 - Version for Platform 4
' 13-Jul-05 rbd     4.0.3 - Add command/error tracing, round telescopeaz
'                   to whole degrees for Meridian controller.
' 31-Aug-05 rbd     4.0.4 - Add timer to poll for dome azimuth at 1 min
'                   intervals if otherwise not polled.
' 09-Jan-06 rbd     4.0.5 - Fix "rounding" of telescopeaz, it should never be
'                   360, so if 359.5 it will be 359. Use VB equivalent of floor()
'                   which is Fix(), instead of CInt() which rounds.
' 10-Jan-05 rbd     4.0.6 - Go back to rounding, and catch the 360 explicitly,
'                   converting it to 0. This avoids a 0.5 deg systematic error.
' 07-Feb-05 rbd     4.0.7 - More shot in the dark for "range" error. Per Tom
'                   Melsheimer, try sending a "float", so now I send the az
'                   to 1 decimal precision. I also added a 500ms wait between
'                   sending the targt coordinates and the 'goto' or 'sync' cmd.
'                   Comment out registry write in GetStatus(). Yeek!
' 15-Mar-06 rbd     4.0.8 - Now we have a "no error" ("error 0") response to
'                   Status. Ignore it and retry at 1 sec intervals till we get
'                   a good response. Melsheimer punted on this one.
' 22-Mar-06 rbd     4.0.9 - OOPS! Exit from "no error" retry loop!
' 04-Jan-08 rbd     5.0.1 - Version change for Platform 5
' 08-Oct-08 rbd     5.0.2 - Add MNCP channel control, implement ASCOM 5
'                   early binding interface.
'================================================================================

Option Explicit

Implements IDome

'----------------------
' Enumerated Constants
'----------------------

' Public ASCOM shutter states
Public Enum ShutterState
    shutterOpen = 0
    shutterClosed = 1
    shutterOpening = 2
    shutterClosing = 3
    shutterError = 4
End Enum

' ACL Dome Status Bits (managed as a Long)
Private Enum StatusBits
    stReserved = 1
    stSlewInProgress = 2
    stHomeInProgress = 4
    stLastHomeFailed = 8
    stShutterOpen = 16
    stShutterInProgress = 32
    stAzFailure = 64
    stShutterFailure = 128
    stDomeReady = 256           ' dome has been homed since power up
    stSimulated = 4096          ' set when ACL HW is being simulated
End Enum

' ASCOM identifiers
Private Const ID As String = "ACLDome.Dome"
Private Const ASCOM_NAME As String = "ACL Based Dome"

' primary objects
Private m_Serial As DriverHelper.Serial
Private m_MNCP As MNCP
Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile
Private WithEvents m_Timer As DriverHelper.Timer
Attribute m_Timer.VB_VarHelpID = -1
Private m_FSO As Scripting.FileSystemObject
Private m_TraceFile As Scripting.TextStream

' dome capabilities (set during connect)
Private m_dFWVersion As Double
Private m_bNoShutterBits As Boolean
Private m_bNoReadyBit As Boolean
Private m_bCanFindHome As Boolean
Private m_bCanPark As Boolean
Private m_bCanSetAltitude As Boolean
Private m_bCanSetAzimuth As Boolean
Private m_bCanSetPark As Boolean
Private m_bCanSetShutter As Boolean
Private m_bCanSyncAzimuth As Boolean

' state variables
Private m_bConnected As Boolean              ' as per "connected" property
Private m_bDomeReady As Boolean              ' dome has been homed (az valid)
Private m_eShutterState As ShutterState      ' shutter status
Private m_bAtHome As Boolean                 ' Home state
Private m_bAtPark As Boolean                 ' Park state
Private m_bDebug As Boolean                  ' Whether to write breadcrumbs
Private m_bTrace As Boolean                  ' Whether to write commands to trace file

' simulation stuff
Private m_bSimulated As Boolean
Private m_lStatus As Long                    ' dome status word as per ACL dome doc
Private m_AzType As Long                     ' 0 done, 1 normal, 2 park, 3 home
Private m_dDomeAlt As Double                 ' Current Alt for Dome
Private m_dDomeAz As Double                  ' Current Az for Dome
Private m_dTargetAz As Double                ' Target Az for Dome
Private m_dSetPark As Double                 ' Park position
Private m_dSetHome As Double                 ' Home position
Private m_iSCount As Integer                 ' shutter move simulation
Private m_iAzCount As Integer                ' az move simulation

'================================================================================
' Class_Initialize()
'
' Initialize the class.
'================================================================================

Private Sub Class_Initialize()
    Dim buf As String
    
    '-------------------------
    ' get the objects we need
    '-------------------------
    
    ' comunication technology
    Set m_Serial = New DriverHelper.Serial
    Set m_MNCP = New MNCP
    Set m_MNCP.SerialObject = m_Serial
    
    ' ASCOM facilities
    Set m_Util = New DriverHelper.Util
    Set m_Profile = New DriverHelper.Profile
    Set m_Timer = New DriverHelper.Timer
    m_Timer.Enabled = False
    m_Timer.Interval = 60000
    m_Profile.DeviceType = "Dome"
    m_Profile.Register ID, ASCOM_NAME             ' Self-register, if needed
        
    '-------------------------
    ' set up global variables
    '-------------------------
    
    ' initialize back door to Sync support (make the entry so its
    ' easier to edit)
    buf = m_Profile.GetValue(ID, "CanSyncAzimuth")
    If buf = "" Then _
        m_Profile.WriteValue ID, "CanSyncAzimuth", "False"
    '
    ' Initialize debugging/tracing flags
    '
    buf = m_Profile.GetValue(ID, "Debug")
    If buf = "" Then
        m_Profile.WriteValue ID, "Debug", "False"
    Else                                            ' Need this now instead of at Connect
        On Error Resume Next
        m_bDebug = False                            ' seed value
        m_bDebug = CBool(buf)
        On Error GoTo 0
    End If

    ' Trace is read at connect time like most others
    buf = m_Profile.GetValue(ID, "Trace")
    If buf = "" Then _
        m_Profile.WriteValue ID, "Trace", "False"
        
    m_bConnected = False

    '----------------------------------------------
    ' set up simualation in case we are simulating
    '----------------------------------------------
    
    ' initialize back doors to simulation support (make the entries so its
    ' easier to edit)
    buf = m_Profile.GetValue(ID, "Simulation")
    If buf = "" Then _
        m_Profile.WriteValue ID, "Simulation", "False"
        
    buf = m_Profile.GetValue(ID, "SimFWVersion")
    If buf = "" Then _
        m_Profile.WriteValue ID, "SimFWVersion", "0.1"
        
    m_lStatus = stSimulated
    m_AzType = 0
    m_dDomeAlt = 0#
    m_dDomeAz = 180#
    m_dTargetAz = 0#
    m_dSetPark = 180#
    m_dSetHome = 0#
    m_iSCount = 0
    m_iAzCount = 0
    
    ' initialize debug
    If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "Initialized"
    
End Sub

'================================================================================
' Class_Terminate()
'
' Destroy the instance.
'================================================================================

Private Sub Class_Terminate()
    
    On Error Resume Next
    If m_bConnected Then _
        Me.Connected = False                    ' Force disconnect
    On Error GoTo 0
    
End Sub

'=======================
' ASCOM Dome Properties
'=======================

'================================================================================
' Altitude
'
' Not implemented. (Although simulation works).
'================================================================================

Public Property Get Altitude() As Double

    CheckConnected False
    
    ' see if we support this at all
    If Not m_bCanSetAltitude Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Altitude" & MSG_NOT_IMPLEMENTED
    
'    ' can't return what hasn't been set ??? or fetch it
'    If m_dDomeAlt = INVALID_COORDINATE Then _
'        Err.Raise SCODE_PROP_NOT_SET, ERR_SOURCE, _
'            "Property Altitude: " & MSG_PROP_NOT_SET
    
    m_eShutterState = Me.ShutterStatus
    
    If m_eShutterState = shutterError Then _
        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
            "Property Altitude: " & MSG_SHUTTER_ERROR
    
    If m_eShutterState <> shutterOpen Then _
        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
            "Property Altitude: " & MSG_SHUTTER_NOT_OPEN
    
    Altitude = m_dDomeAlt
    
End Property

'================================================================================
' AtHome
'
' Fully implemented, but state is tracked by knowing what we send to the dome.
' The dome handset could mess us up (no HW check possible).
'================================================================================

Public Property Get AtHome() As Boolean
    
    CheckConnected False
    
    ' if the dome hasn't been ready, go get a fresh look
    If Not m_bDomeReady Then
        If (GetStatus And stDomeReady) <> 0 Then
        
            ' we see a ready indication, perhaps a FW upgrade
            If m_bNoReadyBit Then
                m_bNoReadyBit = False       ' FW upgrade
                m_Profile.WriteValue ID, "NoReadyBit", "False"
            End If
            
            ' set the new state
            m_bDomeReady = True
            m_bAtPark = False
        End If
    End If
        
    AtHome = m_bAtHome
    
End Property

'================================================================================
' AtPark
'
' Fully implemented, but state is tracked by knowing what we send to the dome.
' The dome handset could mess us up (no HW check possible).
'================================================================================

Public Property Get AtPark() As Boolean
    
    CheckConnected False
    
    ' if the dome hasn't been ready, go get a fresh look
    If Not m_bDomeReady Then
        If (GetStatus And stDomeReady) <> 0 Then
        
            ' we see a ready indication, perhaps a FW upgrade
            If m_bNoReadyBit Then
                m_bNoReadyBit = False       ' FW upgrade
                m_Profile.WriteValue ID, "NoReadyBit", "False"
            End If
            
            ' set the new state
            m_bDomeReady = True
            m_bAtPark = False
        End If
    End If
        
    AtPark = m_bAtPark

End Property

'================================================================================
' Azimuth
'
' Fully implemented.
'================================================================================

Public Property Get Azimuth() As Double

    CheckConnected True
                    
    If Not m_bCanSetAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Azimuth" & MSG_NOT_IMPLEMENTED

    Azimuth = m_Util.DMSToDegrees(Me.CommandString("get az"))
    
End Property

'================================================================================
' CanFindHome
'================================================================================

Public Property Get CanFindHome() As Boolean

    CheckConnected False
    
    CanFindHome = m_bCanFindHome
    
End Property

'================================================================================
' CanPark
'================================================================================

Public Property Get CanPark() As Boolean

    CheckConnected False
    
    CanPark = m_bCanPark
        
End Property

'================================================================================
' CanSetAltitude
'================================================================================

Public Property Get CanSetAltitude() As Boolean

    CheckConnected False
    
    CanSetAltitude = m_bCanSetAltitude
     
End Property

'================================================================================
' CanSetAzimuth
'================================================================================

Public Property Get CanSetAzimuth() As Boolean

    CheckConnected False
    
    CanSetAzimuth = m_bCanSetAzimuth
    
End Property

'================================================================================
' CanSetPark
'================================================================================

Public Property Get CanSetPark() As Boolean
    
    CanSetPark = m_bCanSetPark
        
End Property

'================================================================================
' CanSetShutter
'================================================================================

Public Property Get CanSetShutter() As Boolean

    CheckConnected False
    
    CanSetShutter = m_bCanSetShutter
    
End Property

'================================================================================
' CanSlave
'================================================================================

Public Property Get CanSlave() As Boolean

    CanSlave = False
    
End Property

'================================================================================
' CanSyncAzimuth
'================================================================================

Public Property Get CanSyncAzimuth() As Boolean

    CheckConnected False
    
    CanSyncAzimuth = m_bCanSyncAzimuth
        
End Property

'================================================================================
' Connected
'
' Fully implemented.  At connect time the following steps occur:
'   - need for HW simulation is determined.  This allows for debug without HW
'   - communications (including MNCP) are set up
'   - dome existence and capabilities are determined
'   - dome status is determined and updated (home / park, and shutter)
'================================================================================

Public Property Get Connected() As Boolean

    Connected = m_bConnected
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim buf As String, port As Integer, chan As Integer
    Dim Position As Integer
    Dim Status As Long
    Dim Ecode As Long
    Dim Esrc As String
    Dim Emsg As String
    
    If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "in Connected method: " & newVal
            
    ' only with a state change does anything happen
    If m_bConnected = newVal Then _
        Exit Property
    
    If newVal Then
        
        '------------
        ' Connecting
        '------------
            
        m_bTrace = False            ' seed value
        m_bTrace = CBool(m_Profile.GetValue(ID, "Trace"))
        On Error GoTo fail
        
        Set m_FSO = Nothing         ' [sentinel]
        If m_bTrace Then
            Set m_FSO = New Scripting.FileSystemObject
            Set m_TraceFile = m_FSO.CreateTextFile("C:\ACLTrace.txt", True)
        End If
        
        ' check for simulation
        buf = m_Profile.GetValue(ID, "Simulation")
        m_bSimulated = IIf(buf = "True", True, False)
                        
        ' set up the communications link. Default to COM1.
        buf = m_Profile.GetValue(ID, "COM Port")
        If buf = "" Then                                ' Default to COM1
            buf = "1"
            m_Profile.WriteValue ID, "COM Port", buf
        End If
        port = CInt(buf)
        
        buf = m_Profile.GetValue(ID, "MNCP Channel")
        If buf = "" Then                                ' Default to COM1
            buf = "1"
            m_Profile.WriteValue ID, "MNCP Channel", buf
        End If
        chan = CInt(buf)
        
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "COM port: " & CInt(buf)
        If m_bTrace Then
            m_TraceFile.WriteLine "ACL Trace at " & CStr(Now())
            m_TraceFile.WriteLine "COM port: " & CInt(buf)
        End If
        
        On Error GoTo fail
        m_Serial.port = port                            ' Set port
        m_Serial.Speed = ps9600                         ' ACL runs 9600
        m_Serial.ReceiveTimeout = 5                     ' 5 second timeout
        m_Serial.Connected = True                       ' Grab the serial port
        m_Serial.ClearBuffers                           ' Clear serial buffers
        
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "COM ready"
        If m_bTrace Then m_TraceFile.WriteLine "COM ready"
        
        ' initialize the MNCP protocol.
        m_MNCP.Address = chan                           ' Selected channel
        m_MNCP.Retries = 4
        If Not m_bSimulated Then _
            m_MNCP.ResetSlave               ' Try to talk to the slave
        m_bConnected = True                 ' needed for following code to work
        
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "MNCP ready"
        If m_bTrace Then m_TraceFile.WriteLine "MNCP ready"
        
        ' determine if there is an ACL dome there.  (This will
        ' throw all kinds of errors if things are not just so)
        Status = GetStatus()
        
        ' identify capabilities
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "calling device"
        buf = Me.CommandString("device")
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "back from device"
        
        ' figure out version formats ??? fix up simulator too
        ' use mid, InStr, and/or indexofany
        If m_bSimulated Then
            Position = InStr(1, buf, "Version")
            m_dFWVersion = val(Mid(buf, Position + 9))
        Else
            m_dFWVersion = 0# ' untill we learn how to parse real
        End If
        m_Profile.WriteValue ID, "FWVersion", Format(m_dFWVersion, "0.0")
                
        On Error Resume Next
        m_bCanFindHome = True
        m_bCanPark = True
        m_bCanSetAltitude = False
        m_bCanSetAzimuth = True
        m_bCanSetPark = False
        
        m_bCanSyncAzimuth = False   ' seed value
        m_bCanSyncAzimuth = CBool(m_Profile.GetValue(ID, "CanSyncAzimuth"))
        
        m_bCanSetShutter = True     ' seed value
        m_bCanSetShutter = CBool(m_Profile.GetValue(ID, "CanSetShutter"))
                    
        ' figure out home and park conditions
        buf = m_Profile.GetValue(ID, "NoReadyBit")
        m_bNoReadyBit = IIf(buf = "False", False, True)
        
        If (Status And stDomeReady) <> 0 Then
            m_bNoReadyBit = False
            m_Profile.WriteValue ID, "NoReadyBit", "False"
            m_bDomeReady = True
            m_bAtPark = False
            m_bAtHome = False
        Else
            m_bDomeReady = False
            m_bAtPark = True
            m_bAtHome = False
            
            buf = m_Profile.GetValue(ID, "AutoUnpark")
            If buf <> "0" Then
                If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "calling FindHome"
                Me.FindHome             ' auto unpark
                If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "back from FindHome"
            Else
                If m_bNoReadyBit Then
                    m_bDomeReady = True
                    m_bAtPark = False
                End If
            End If
        End If
        
        ' get shutter state
        buf = m_Profile.GetValue(ID, "NoShutterBits")
        m_bNoShutterBits = IIf(buf = "False", False, True)
        m_eShutterState = shutterError      ' seed in case we can't tell
        If m_bCanSetShutter = True Then
            If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "calling ShutterStatus"
            m_eShutterState = Me.ShutterStatus
            If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "back from ShutterStatus"
        End If
        
        ' Start the sanity poll timer
        m_Timer.Enabled = True
        
        If m_bDebug Then m_Profile.WriteValue ID, "Breadcrumb", "done connecting"
        On Error GoTo 0
    Else
    
        '---------------
        ' disconnecting
        '---------------
        m_Timer.Enabled = False
        m_bConnected = False
        On Error Resume Next                            ' Best efforts...
        m_Serial.ClearBuffers                           ' Clear serial buffers
        m_Serial.Connected = False                      ' Release COM port
        If m_bTrace Then m_TraceFile.Close              ' Close the trace file
        On Error GoTo 0
        
    End If

    Exit Property

'-----------------------------------------------------------
' Connect failure code.  Clean up befor throwing the error.
'-----------------------------------------------------------

fail:
    Ecode = Err.Number            ' remember the error
    Esrc = Err.Source
    Emsg = Err.Description
    Resume FailResume             ' clean up from resume state
    
FailResume:
    m_bConnected = False          ' just to be sure
    
    On Error Resume Next
    m_Serial.ClearBuffers         ' Clear serial buffers
    m_Serial.Connected = False    ' Release COM port
    On Error GoTo 0
    
    Err.Raise Ecode, Esrc, Emsg   ' now, throw the error
    
End Property

'================================================================================
' Description
'
' Fully implemented.
'================================================================================

Public Property Get Description() As String

    Description = ASCOM_NAME
    
End Property

'================================================================================
' DriverInfo
'
' Fully implemented, including HW returned strings.
'================================================================================

Public Property Get DriverInfo() As String
    Dim Hardware As String

    ' Use the Project/Properties sheet, Make tab, to set these
    ' items. That way they will show in the Version tab of the
    ' Explorer property sheet, and the exact same data will
    ' show in Dome.DriverInfo.

    DriverInfo = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
        
'    other stuff that could be added ...
'    If App.CompanyName <> "" Then _
'        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
'    If App.LegalCopyright <> "" Then _
'        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright

    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
        
    If m_bConnected Then
        Hardware = ""
        On Error Resume Next
        Hardware = Me.CommandString("device")
        On Error GoTo 0
        
        DriverInfo = DriverInfo & vbCrLf & Hardware
    End If
        
End Property

'================================================================================
' InterfaceVersion
'
' Fully implemented as version 1.
'================================================================================

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 1
        
End Property

'================================================================================
' Name
'
' Fully implemented.
'================================================================================

Public Property Get Name() As String
    
    Name = ASCOM_NAME

End Property

'================================================================================
' ShutterStatus
'
' Fully implemented on newest firmware, including transition and error states.
' Older firmware had no shutter status bits, so no transition possible.
' Initial state will be error until first open or close.
'================================================================================

Public Property Get ShutterStatus() As ShutterState
    Dim Status As Long
    
    CheckConnected False
    
    If Not m_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property ShutterStatus" & MSG_NOT_IMPLEMENTED
 
    Status = GetStatus()
    
    ' check for error state first
    If (Status And stShutterFailure) <> 0 Then
        m_eShutterState = shutterError
    
    ' check for motion and get direction from remembered state
    ElseIf (Status And stShutterInProgress) <> 0 Then
            
        ' discovered motion from handset
        If m_eShutterState = shutterOpen Then _
            m_eShutterState = shutterClosing
        If m_eShutterState = shutterClosed Then _
            m_eShutterState = shutterOpening
        
    ' lastly, check for open / close
    ElseIf (Status And stShutterOpen) <> 0 Then
    
        ' adaptive learning about existence of shutter feedback
        If m_bNoShutterBits Then
            m_bNoShutterBits = False
            m_Profile.WriteValue ID, "NoShutterBits", "False"
        End If
        
        ' it's open for sure
        m_eShutterState = shutterOpen
    Else
    
        ' if no shutter feedback, then use remembered motion to set state
        If m_bNoShutterBits Then
            If m_eShutterState = shutterClosing Then _
                m_eShutterState = shutterClosed
            If m_eShutterState = shutterOpening Then _
                m_eShutterState = shutterOpen
        Else
        
            ' it's closed for sure
            m_eShutterState = shutterClosed
        End If
    End If
    
    ShutterStatus = m_eShutterState
            
End Property

'================================================================================
' Slaved
'
' Setting to "True" is not supported.  Use high level SW.
'================================================================================

Public Property Get Slaved() As Boolean

    Slaved = False
        
End Property

Public Property Let Slaved(ByVal newVal As Boolean)
    
    ' only error out if we're trying turn slaving on
    If newVal Then
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Slaving Dome" & MSG_NOT_IMPLEMENTED
    End If
        
End Property

'================================================================================
' Slewing
'
' Fully implemented.
'================================================================================

Public Property Get Slewing() As Boolean
    Dim Status As Long

    CheckConnected False
    
    Status = GetStatus()
    
    ' check for trouble
    If (Status And stAzFailure) <> 0 Then _
        Err.Raise SCODE_SLEW_FAIL, ERR_SOURCE, _
            "Checking slew: " & MSG_SLEW_FAIL
    
    ' slewing is true if either Alt or Az are in motion, but shutter
    ' opening and closing do not count as slewing as per ASCOM spec
    
    ' if we add altitude support, how do we detect alt moves ???
    
    If (Status And (stSlewInProgress Or stHomeInProgress)) <> 0 Then
        Slewing = True
        m_bAtHome = False
        m_bAtPark = False
    Else
        Slewing = False
    End If

End Property

'====================
' ASCOM Dome Methods
'====================

'================================================================================
' AbortSlew()
'
' Fully implemented.  AtPark and AtHome are cleared (no HW check possible).
'================================================================================

Public Sub AbortSlew()

    CheckConnected False
    
    Me.CommandBlind "stop"                                 ' Halt the slew
    m_bAtHome = False
    m_bAtPark = False
        
End Sub

'================================================================================
' CloseShutter()
'
' Fully implemented as asynchronous.
'================================================================================

Public Sub CloseShutter()
    
    CheckConnected False
    
    If Not m_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CloseShutter" & MSG_NOT_IMPLEMENTED
    
    m_eShutterState = shutterClosing
    Me.CommandBlind "close"
    
End Sub

'================================================================================
' CommandBlind()
'
' Fully implemented.  Unprotected by connect check.
'================================================================================

Public Sub CommandBlind(ByVal Command As String)
        
    Me.CommandString Command            ' Just toss any results
    
End Sub

'================================================================================
' CommandBool()
'
' Fully implemented.  Unprotected by connect check.
'================================================================================

Public Function CommandBool(ByVal Command As String) As Boolean
    Dim c As Long
    Dim d As String, s As String
        
    On Error GoTo CMDBOOL_ERR           ' This is sort of cheesy
    
    Me.CommandString Command            ' Only goto errors are non-fatal
    CommandBool = True
    Exit Function
    
CMDBOOL_ERR:
    c = Err.Number
    d = Err.Description
    s = Err.Source
    Resume CMDBOOL_RSUM
    
CMDBOOL_RSUM:
    On Error GoTo 0
    CommandBool = False
    If c <> SCODE_ACLERR Then
        Err.Raise c, s, d
    ElseIf LCase$(Left$(d, 4)) <> "goto" Then
        Err.Raise c, s, d
    Else
        CommandBool = True
    End If
    
End Function

'================================================================================
' CommandString()
'
' Fully implemented.  Unprotected by connect check.
'================================================================================

Public Function CommandString(ByVal Command As String) As String
    Dim buf As String, i As Integer, rc As Integer
        
    If m_bTrace Then m_TraceFile.WriteLine "-> " & Command
    
    '-----------------------------------------------
    ' simulation code for testing driver with no HW
    '-----------------------------------------------
    If m_bSimulated Then
        CommandString = ""                  ' assume no return as guess
        
        If Command = "status" Then          ' process status request
        
            ' simulate shutter motion
            If m_iSCount > 0 Then
                m_iSCount = m_iSCount - 1
                If m_iSCount = 0 Then
                    If m_eShutterState = shutterOpening Then
                        m_lStatus = m_lStatus Or stShutterOpen
                    Else
                        m_lStatus = m_lStatus And _
                            (Not (stShutterOpen Or stShutterFailure))
                    End If
                    m_lStatus = m_lStatus And (Not stShutterInProgress)
                End If
            End If
            
            ' simulate az motion
            If m_iAzCount > 0 Then
                m_iAzCount = m_iAzCount - 1
                m_dDomeAz = m_dDomeAz + 1     ' random movement, but it is visible
                If m_iAzCount = 0 Then
                    m_dDomeAz = m_dTargetAz   ' end up in the right place
                    m_lStatus = m_lStatus And (Not _
                        (stSlewInProgress Or stHomeInProgress))
                End If
            End If
            
            CommandString = Hex(m_lStatus)
        
        ElseIf Command = "device" Then
            buf = m_Profile.GetValue(ID, "SimFWVersion")
            CommandString = "ACL Dome Hardware Simulator Version: " & buf
            
        ElseIf Command = "stop" Then
            If m_iSCount > 0 Then
                m_lStatus = m_lStatus Or stShutterFailure
            End If
            If m_iAzCount > 0 Then
                If m_lStatus And stHomeInProgress Then
                    m_lStatus = m_lStatus Or stLastHomeFailed
                Else
                    m_lStatus = m_lStatus Or stAzFailure
                End If
            End If
            m_lStatus = m_lStatus And (Not _
                (stShutterInProgress Or stHomeInProgress Or stSlewInProgress))
            m_iSCount = 0
            m_iAzCount = 0
            
        ElseIf Command = "close" Then
            m_lStatus = m_lStatus And (Not stShutterOpen)
            If m_dFWVersion >= 1# Then
                m_lStatus = m_lStatus Or stShutterInProgress
                m_iSCount = 10              ' start motion
            End If
            
        ElseIf Command = "open" Then
            If m_dFWVersion >= 1# Then
                m_lStatus = m_lStatus Or stShutterInProgress
                m_iSCount = 10              ' start motion
            End If
            
        ElseIf Command = "home setup" Then
            m_dSetPark = m_dDomeAz      ' pickle the current position as park
            
        ElseIf Command = "home park" Then
            m_lStatus = m_lStatus And (Not stHomeInProgress)
            m_lStatus = m_lStatus Or stSlewInProgress
            m_AzType = 2
            m_dTargetAz = m_dSetPark    ' set park az as destination
            m_iAzCount = 10             ' start motion
            
        ElseIf Command = "home find" Then
            m_lStatus = m_lStatus And _
                (Not (stLastHomeFailed Or stAzFailure))
            If m_lStatus And stDomeReady Then    ' if dome already woke up...
                m_lStatus = m_lStatus Or stSlewInProgress Or stHomeInProgress
                m_AzType = 3
                m_dTargetAz = m_dSetHome    ' set home az as desination
                m_iAzCount = 10             ' start motion
            Else                        ' dome not woke up yet
                m_lStatus = m_lStatus And (Not (stSlewInProgress Or stHomeInProgress))
                m_iAzCount = 0
                m_dDomeAz = m_dSetHome  ' find home switch
                If m_dFWVersion >= 1# Then _
                    m_lStatus = m_lStatus Or stDomeReady
            End If
            
        ElseIf Command = "goto" Then
            m_lStatus = m_lStatus And (Not stHomeInProgress)
            m_lStatus = m_lStatus Or stSlewInProgress
            m_AzType = 1
            m_iAzCount = 10             ' start motion
            
        ElseIf Command = "sync" Then
            m_dDomeAz = m_dTargetAz     ' we're synced
            
        ElseIf Command = "get az" Then
            CommandString = m_Util.DegreesToDMS(m_dDomeAz)
            
        ElseIf Left$(Command, 16) = "set telescopeaz=" Then
            m_dTargetAz = val(Mid$(Command, 17))
        End If
        
        ' ??? generate error codes when any get defined
        Exit Function
    End If
    
    '---------------------------
    ' real command and response
    '---------------------------
    
    '
    ' This is horrible: If we get "error 0" ignore it and retry
    ' after 1 second. (4.0.8, Melsheimer bug)
    '
    rc = 0                                                      ' Retry count (error 0 looping)
    Do While True
        m_MNCP.ToSlave Command                                  ' No NL at end...
        buf = m_MNCP.FromSlave()
        
        If m_bTrace Then m_TraceFile.WriteLine "<- " & buf
    
        ' check for error
        If LCase$(Left$(buf, 5)) = "error" Then                 ' Returned an error message
            i = CInt(Trim$(Mid$(buf, 6)))                       ' Numeric error code
            If i <> 0 Then                                      ' Treat 0 specially (@#$%)
                Select Case i
                    ' ACL core errors
                    ''Case 0:     buf = "No error"
                    Case 1:     buf = "Undefined error"
                    Case 2:     buf = "Syntax error"
                    Case 10:    buf = "Type Mismatch"
                    Case 11:    buf = "Range"                   ' added by jab
                    Case 12:    buf = "Value is read-only"
                    Case 13:    buf = "Unsupported command"
                    Case 14:    buf = "Unsupported identifier"
                    Case 15:    buf = "Command inactive"
                    
        '            ' ACL Telescope errors (what are the dome errors ?)
        '            ' none yet
        '            Case 100:   buf = "Goto - illegal parameter(s)"
        '            Case 101:   buf = "Goto - object below horizon"
        '            Case 102:   buf = "Goto - object outside limits"
                    
                    ' Others are reserved, should never be seen (ha ha ha)
                    Case Else:  buf = "Reserved error code, you shouldn't see this!"
                End Select
                If m_bTrace Then m_TraceFile.WriteLine "  (" & buf & ")"
                
                Err.Raise SCODE_ACLERR, ERR_SOURCE, _
                        "Low level ACL error from dome: """ & buf & """"
            Else                                                ' Treat error 0 special (ignore, delay, loop)
                If rc >= 10 Then
                    Err.Raise SCODE_ACLERR, ERR_SOURCE, _
                            Command & " returned 'error 0' 10 times in a row!"
                Else
                    m_Util.WaitForMilliseconds 1000             ' Wait a second before retrying
                    rc = rc + 1
                End If
            End If
        ElseIf LCase$(Left$(buf, 2)) <> "ok" Then _
            Err.Raise SCODE_NOT_ACL, ERR_SOURCE, MSG_NOT_ACL
        Else
            Exit Do                                             ' Got an OK! *EXIT LOOP*
        End If
    Loop
    
    ' return the response
    CommandString = Trim$(Mid$(buf, 3))             ' Remove "ok " and trim
    
End Function

'================================================================================
' FindHome()
'
' Fully implemented as synchronous.
'================================================================================

Public Sub FindHome()

    CheckConnected False
    
    If Not m_bCanFindHome Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Methode FindHome()" & MSG_NOT_IMPLEMENTED
            
    m_bAtHome = False
    m_bAtPark = False
    
    Me.CommandBlind "home find"
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
    Wend
    
    If (GetStatus And stDomeReady) <> 0 Then
        m_bNoReadyBit = False
        m_Profile.WriteValue ID, "NoReadyBit", "False"
        m_bDomeReady = True
    Else
        If m_bNoReadyBit Then _
            m_bDomeReady = True
    End If
            
    m_bAtHome = True
    
End Sub

'================================================================================
' OpenShutter()
'
' Fully implemented as asynchronous.
'================================================================================

Public Sub OpenShutter()
    Dim Status As Long
    Dim Count As Integer

    CheckConnected False
    
    If Not m_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method OpenShutter" & MSG_NOT_IMPLEMENTED
                    
    m_eShutterState = shutterOpening
    
    Me.CommandBlind "open"
            
End Sub

'================================================================================
' Park()
'
' Fully implemented as synchronous.
'================================================================================

Public Sub Park()

    CheckConnected True
    
    If Not m_bCanPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Methode Park()" & MSG_NOT_IMPLEMENTED
    
    m_bAtHome = False
    m_bAtPark = False
    
    Me.CommandBlind "home park"
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
    Wend
    
    m_bAtPark = True    ' check for error state
    
End Sub

'================================================================================
' SetPark()
'
' Not implemented.  Deemed undesirable by Meridian.
'================================================================================

Public Sub SetPark()

    CheckConnected True
    
    If Not m_bCanSetPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Methode SetPark()" & MSG_NOT_IMPLEMENTED
            
    ' this code should work, but is untested
    Me.CommandBlind "home setup"
    m_bAtPark = True

End Sub

'================================================================================
' SetupDialog()
'
' Fully implemented.  Valid even while connected.
'================================================================================

Public Sub SetupDialog()

    frmSetup.m_DriverID = ID                ' Form calls Profile object methods
    Set frmSetup.m_Profile = m_Profile
    
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

'================================================================================
' SlewToAltitude()
'
' Not implemented. (Although simulation works).
'================================================================================

Public Sub SlewToAltitude(ByVal Altitude As Double)

    CheckConnected False
    
    If Not m_bCanSetAltitude Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltitude" & MSG_NOT_IMPLEMENTED
            
    m_eShutterState = Me.ShutterStatus
    
    If m_eShutterState = shutterError Then _
        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
            "Method SlewToAltitude " & MSG_SHUTTER_ERROR
            
    If m_eShutterState <> shutterOpen Then _
        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
            "Method SlewToAltitude " & MSG_SHUTTER_NOT_OPEN
            
' should there be a range check ???
'    If Altitude < g_dMinAlt Or Altitude > g_dMaxAlt Then _
'         Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
'            "Altitude " & MSG_VAL_OUTOFRANGE
            
    m_dDomeAlt = Altitude
    
End Sub

'================================================================================
' SlewToAzimuth()
'
' Fully implemented as asynchronous.
'================================================================================

Public Sub SlewToAzimuth(ByVal Azimuth As Double)
'    Dim iAz As Integer
    
    CheckConnected True
    
    If Not m_bCanSetAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAzimuth" & MSG_NOT_IMPLEMENTED
    
    Azimuth = RangeAz(Azimuth)

    m_bAtHome = False
    m_bAtPark = False
    
    If Me.Slewing Then                                  ' If slewing
        Me.AbortSlew                                    ' Stop any slew in progress!
        While Me.Slewing                                ' Complete slew cycle as necessary
            m_Util.WaitForMilliseconds 1000
        Wend
' why was the wait in the original scope code ??? (email bob)
'        Me.TargetRightAscension = m_dObjectRA
'        Me.TargetDeclination = m_dObjectDec
'        m_Util.WaitForMilliseconds 500
    End If
        
'(old)    Me.CommandBlind "set telescopeaz=" & m_Util.DegreesToDMS(Azimuth, "d", "m", "s")
'(07-Feb-06) The following was used
'    iAz = CInt(Azimuth)
'    If iAz >= 360 Or iAz < 0 Then iAz = 0               ' Safety range
'    Me.CommandBlind "set telescopeaz=" & iAz & "d"
    If Azimuth > 359.9 Then Azimuth = 359.9             ' Catch rounding to 360.0 in Format() below
    Me.CommandBlind "set telescopeaz=" & Format$(Azimuth, "0.0") & "d"  ' A shot in the dark
    m_Util.WaitForMilliseconds 500                                      ' Same here
'
    Me.CommandBlind "goto"                              ' Start the slew
    
End Sub

'================================================================================
' SyncToAzimuth()
'
' Optionally implemented.  Syncing is supported, but only if the CanSyncAzimuth
' registry entry is set to "True".  This is to improve supportability.  It
' is believed that "sync"ing the dome is a major cause of customer error.
' When sync is enabled, AtPark and AtHome are cleared (no HW check possible).
'================================================================================

Public Sub SyncToAzimuth(ByVal Azimuth As Double)
'    Dim iAz As Integer
    
    CheckConnected True
      
    If Not m_bCanSyncAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToAzimuth" & MSG_NOT_IMPLEMENTED
    
    Azimuth = RangeAz(Azimuth)
    
    m_bAtHome = False
    m_bAtPark = False
    
'(old)    Me.CommandBlind "set telescopeaz=" & m_Util.DegreesToDMS(Azimuth, "d", "m", "s")
'(07-Feb-06) The following was used
'    iAz = CInt(Azimuth)
'    If iAz >= 360 Or iAz < 0 Then iAz = 0               ' Safety range
'    Me.CommandBlind "set telescopeaz=" & iAz & "d"
    If Azimuth > 359.9 Then Azimuth = 359.9             ' Catch rounding to 360.0 in Format() below
    Me.CommandBlind "set telescopeaz=" & Format$(Azimuth, "0.0") & "d"  ' A shot in the dark
    m_Util.WaitForMilliseconds 500                                      ' Same here
'
    Me.CommandBlind "sync"
    
End Sub

' ====================
' IDome Implementation
' ====================

Private Sub IDome_AbortSlew()
    Call Me.AbortSlew
End Sub

Private Property Get IDome_Altitude() As Double
    IDome_Altitude = Me.Altitude
End Property

Private Property Get IDome_AtHome() As Boolean
    IDome_AtHome = Me.AtHome
End Property

Private Property Get IDome_AtPark() As Boolean
    IDome_AtPark = Me.AtPark
End Property

Private Property Get IDome_Azimuth() As Double
    IDome_Azimuth = Me.Azimuth
End Property

Private Property Get IDome_CanFindHome() As Boolean
    IDome_CanFindHome = Me.CanFindHome
End Property

Private Property Get IDome_CanPark() As Boolean
    IDome_CanPark = Me.CanPark
End Property

Private Property Get IDome_CanSetAltitude() As Boolean
    IDome_CanSetAltitude = Me.CanSetAltitude
End Property

Private Property Get IDome_CanSetAzimuth() As Boolean
    IDome_CanSetAzimuth = Me.CanSetAzimuth
End Property

Private Property Get IDome_CanSetPark() As Boolean
    IDome_CanSetPark = Me.CanSetPark
End Property

Private Property Get IDome_CanSetShutter() As Boolean
    IDome_CanSetShutter = Me.CanSetShutter
End Property

Private Property Get IDome_CanSlave() As Boolean
    IDome_CanSlave = Me.CanSlave
End Property

Private Property Get IDome_CanSyncAzimuth() As Boolean
    IDome_CanSyncAzimuth = Me.CanSyncAzimuth
End Property

Private Sub IDome_CloseShutter()
    Call Me.CloseShutter
End Sub

Private Sub IDome_CommandBlind(ByVal Command As String)
    Call Me.CommandBlind(Command)
End Sub

Private Function IDome_CommandBool(ByVal Command As String) As Boolean
   IDome_CommandBool = Me.CommandBool(Command)
End Function

Private Function IDome_CommandString(ByVal Command As String) As String
    IDome_CommandString = Me.CommandString(Command)
End Function

Private Property Let IDome_Connected(ByVal State As Boolean)
    Me.Connected = State
End Property

Private Property Get IDome_Connected() As Boolean
    IDome_Connected = Me.Connected
End Property

Private Property Get IDome_Description() As String
    IDome_Description = Me.Description
End Property

Private Property Get IDome_DriverInfo() As String
    IDome_DriverInfo = Me.DriverInfo
End Property

Private Sub IDome_FindHome()
    Call Me.FindHome
End Sub

Private Property Get IDome_InterfaceVersion() As Integer
    IDome_InterfaceVersion = Me.InterfaceVersion()
End Property

Private Property Get IDome_Name() As String
    IDome_Name = Me.Name
End Property

Private Sub IDome_OpenShutter()
    Call Me.OpenShutter
End Sub

Private Sub IDome_Park()
    Call Me.Park
End Sub

Private Sub IDome_SetPark()
    Call Me.SetPark
End Sub

Private Sub IDome_SetupDialog()
    Call Me.SetupDialog
End Sub

Private Property Get IDome_ShutterStatus() As AscomInterfacesLib.ShutterState
    Select Case Me.ShutterStatus
        Case shutterOpen: IDome_ShutterStatus = AscomInterfacesLib.ShutterState.shutterOpen
        Case shutterClosed: IDome_ShutterStatus = AscomInterfacesLib.ShutterState.shutterClosed
        Case shutterOpening: IDome_ShutterStatus = AscomInterfacesLib.ShutterState.shutterOpening
        Case shutterClosing: IDome_ShutterStatus = AscomInterfacesLib.ShutterState.shutterClosing
        Case shutterError: IDome_ShutterStatus = AscomInterfacesLib.ShutterState.shutterError
    End Select
End Property

Private Property Let IDome_Slaved(ByVal State As Boolean)
    Me.Slaved = State
End Property

Private Property Get IDome_Slaved() As Boolean
    IDome_Slaved = Me.Slaved
End Property

Private Property Get IDome_Slewing() As Boolean
    IDome_Slewing = Me.Slewing
End Property

Private Sub IDome_SlewToAltitude(ByVal Altitude As Double)
    Call Me.SlewToAltitude(Altitude)
End Sub

Private Sub IDome_SlewToAzimuth(ByVal Azimuth As Double)
    Call Me.SlewToAzimuth(Azimuth)
End Sub

Private Sub IDome_SyncToAzimuth(ByVal Azimuth As Double)
    Call Me.SyncToAzimuth(Azimuth)
End Sub


'==========================
' Private Helper Functions
'==========================

'================================================================================
' CheckConnected()
'
' Raise an error if the dome is not connected, and optionally check for
' dome ready.  The HW check of dome ready is only made if the cashed version
' has never been set.  Once its set, we don't bother going back to the HW.
'================================================================================

Private Sub CheckConnected(CReady As Boolean)

    If Not m_bConnected Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    
    ' check dome ready
    If CReady Then
        If Not m_bDomeReady Then
            If (GetStatus And stDomeReady) <> 0 Then
                If m_bNoReadyBit Then
                    m_bNoReadyBit = False   ' yahoo, bits are working
                    m_Profile.WriteValue ID, "NoReadyBit", "False"
                End If
                m_bAtPark = False           ' can't be parked
                m_bDomeReady = True         ' it actually is ready
            Else
                ' seems not to be ready
                If m_bNoReadyBit Then
                    m_bDomeReady = True     ' assume it's ready
                Else
                    ' we know the ready bit works, so we're really not ready
                    Err.Raise SCODE_DOME_NOT_READY, ERR_SOURCE, MSG_DOME_NOT_READY
                End If
            End If
        End If
    End If
                    
End Sub

'================================================================================
' GetStatus()
'
' Return the dome status bits.  Converted to long from the dome hex string.
'================================================================================

Private Function GetStatus() As Long
    
    GetStatus = CLng("&H" & CommandString("status"))
        
    ' comment out after debug sessions stabilize ???
    ' m_Profile.WriteValue ID, "LastStatus", Hex(GetStatus)
        
End Function

'================================================================================
' RangeAz()
'
' Ensure an Azimuth is ranged from 0 >= Az > 360
'================================================================================

Private Function RangeAz(ByVal Az As Double) As Double

    While Az >= 360#
        Az = Az - 360#
    Wend
    While Az < 0#
        Az = Az + 360#
    Wend
             
    RangeAz = Az
    
End Function

'================================================================================
' Hex()
'
' Convert a long into a hex string.
'================================================================================

Private Function Hex(val As Long) As String
    Dim nib As Integer
    
    nib = (val \ 4096) And &HF
    If nib > 9 Then _
       nib = nib + 7
    Hex = Chr(nib + Asc("0"))
    
    nib = (val \ 256) And &HF
    If nib > 9 Then _
       nib = nib + 7
    Hex = Hex & Chr(nib + Asc("0"))
        
    nib = (val \ 16) And &HF
    If nib > 9 Then _
       nib = nib + 7
    Hex = Hex & Chr(nib + Asc("0"))
    
    nib = val And &HF
    If nib > 9 Then _
       nib = nib + 7
    Hex = Hex & Chr(nib + Asc("0"))
    
End Function

'====================
' Timer event handler
'====================
'
' Just asks the controller for the dome azimuth. THis gets fired
' once per minute.
'
Private Sub m_Timer_Tick()

    Me.CommandString "get az"
    
End Sub


