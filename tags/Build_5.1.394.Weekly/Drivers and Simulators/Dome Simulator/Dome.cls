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
Attribute VB_Description = "ASCOM Dome Driver for Dome Simulator"
' ============
'   DOME.CLS
' ============
'
' Implementation of the ASCOM Dome Class
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 15-Jul-03 rbd     1.0.3 Add ByVal to Slew/Sync parameters, spell out.
'                   Add (unimplemented) Commandxxx() per spec. Fix
'                   shutter error message prefix in SlewToAltitude().
' 02-Sep-03 rbd     3.0.1 - Version change for Platform 3.0
' 03-Sep-03 jab     Additional checks for home/park positions
' 31-Jan-04 jab     Treat home/park as state, not position
' 22-May-04 jab     opening/closing shutter now count as slewing
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 06-Dec-04 rbd     4.0.2 - Slewing True while opening/closing only if
'                   non-standard switch is set. Move slewing logic
'                   into HW layer.
' 12-Apr-07 rbd     5.0.1 - New startup refactoring.
' 21-May-07 rbd     5.0.2 - Implement new master interface
' 01-Jun-07 rbd     5.0.3 - Oops, many traffic printouts recursed
'                   infinitely, due to missing name changes.
' 03-Jun-07 jab     5.0.4 converted back to public as was, and IDome
'                   calls the old public code (fixed many issues)
' 03-Jun-08 pwgs    5.0.7 chganged valid azimuth range from -360 to +360 to 0 to 360
' 18-Nov-08 rbd     5.0.8 version change only for Platform Update installer
'---------------------------------------------------------------------

Option Explicit
Implements IDome

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()
    
    If App.StartMode <> vbSModeStandalone Then                    ' If loaded via ActiveX
        DoStartupIf
        g_iConnections = g_iConnections + 1
    End If
      
End Sub

Private Sub Class_Terminate()
    
    If App.StartMode <> vbSModeStandalone Then
        If g_iConnections > 1 Then
            g_iConnections = g_iConnections - 1
        Else
            Unload g_handBox
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Altitude() As Double

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "Altitude: "
    End If

    If Not g_bCanSetAltitude Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Altitude" & MSG_NOT_IMPLEMENTED
    
    check_connected
    
    If g_eShutterState = shutterError Then _
        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
            "Property Altitude: " & MSG_SHUTTER_ERROR
    
    If g_eShutterState <> shutterOpen Then _
        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
            "Property Altitude: " & MSG_SHUTTER_NOT_OPEN
    
    Altitude = g_dDomeAlt

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficEnd Format$(Altitude, "0.0")
    End If
    
End Property

Public Property Get AtHome() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "AtHome: "
    End If
    
    check_connected
    AtHome = g_bAtHome

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(AtHome)
    End If
    
End Property

Public Property Get AtPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "AtPark: "
    End If
    
    check_connected
    AtPark = g_bAtPark

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(AtPark)
    End If
    
End Property

Public Property Get Azimuth() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Azimuth: "
    End If
    
    If Not g_bCanSetAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Azimuth" & MSG_NOT_IMPLEMENTED

    check_connected
    Azimuth = g_dDomeAz

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd Format$(Azimuth, "0.0")
    End If
    
End Property

Public Property Get CanFindHome() As Boolean

    CanFindHome = g_bCanFindHome

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanFindHome: " & CanFindHome
    End If
    
End Property

Public Property Get CanPark() As Boolean

    CanPark = g_bCanPark
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanPark: " & CanPark
    End If
    
End Property

Public Property Get CanSetAltitude() As Boolean

    CanSetAltitude = g_bCanSetAltitude
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSetAltitude: " & CanSetAltitude
    End If
    
End Property

Public Property Get CanSetAzimuth() As Boolean

    CanSetAzimuth = g_bCanSetAzimuth
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSetAzimuth: " & CanSetAzimuth
    End If
    
End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = g_bCanSetPark
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSetPark: " & CanSetPark
    End If
    
End Property

Public Property Get CanSetShutter() As Boolean

    CanSetShutter = g_bCanSetShutter

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSetShutter: " & CanSetShutter
    End If
    
End Property

Public Property Get CanSlave() As Boolean

    CanSlave = False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSlave: " & CanSlave
    End If
    
End Property

Public Property Get CanSyncAzimuth() As Boolean

    CanSyncAzimuth = g_bCanSyncAzimuth
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "CanSyncAzimuth: " & CanSyncAzimuth
    End If
    
End Property

Public Property Get Connected() As Boolean

    Connected = g_bConnected
    
    If Not g_show Is Nothing Then
        If g_show.chkConnected.Value = 1 Then _
            g_show.TrafficLine "Connected: " & Connected
    End If
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    
    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkConnected.Value = 1 Then _
            g_show.TrafficStart "Connected: " & g_bConnected & " -> " & newVal
    End If
    
    out = " (done)"
    
    ' only with a state change does anything happen
    If g_bConnected <> newVal Then
        If newVal Then
            g_bConnected = True
        Else
            If App.StartMode = vbSModeStandalone Then
                out = " (ignored, not auto launch)"
            Else
                If g_iConnections < 2 Then
                    g_bConnected = False
                Else
                    out = " (ignored, not last user)"
                End If
            End If
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkConnected.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get Description() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Description"
    End If
    
    Description = INSTRUMENT_DESCRIPTION
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get DriverInfo() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DriverInfo"
    End If
    
    '
    ' Use the Project/Properties sheet, Make tab, to set these
    ' items. That way they will show in the Version tab of the
    ' Explorer property sheet, and the exact same data will
    ' show in Dome.DriverInfo.
    '
    DriverInfo = App.FileDescription & " " & _
                App.Major & "." & App.Minor & "." & App.Revision
    If App.CompanyName <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
    If App.LegalCopyright <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
        
End Property

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 1
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "InterfaceVersion: " & InterfaceVersion
    End If
    
End Property

Public Property Get Name() As String

    Name = INSTRUMENT_NAME
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Name: " & Name
    End If
    
End Property

Public Property Get Slaved() As Boolean

    Slaved = False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Slaved: " & Slaved
    End If
    
End Property

Public Property Let Slaved(ByVal newVal As Boolean)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Slaved: " & False & " -> " & newVal
    End If
        
    If newVal Then
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Slaving Dome" & MSG_NOT_IMPLEMENTED
    End If
        
End Property

Public Property Get ShutterStatus() As ShutterState

    Dim out As String
   
    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "ShutterStatus: "
    End If
    
    If Not g_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property ShutterStatus" & MSG_NOT_IMPLEMENTED
    
    check_connected
    ShutterStatus = g_eShutterState
    
    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then
            out = "unknown"
            Select Case ShutterStatus
                Case shutterOpen:    out = "Open"
                Case shutterClosed:   out = "Close"
                Case shutterOpening: out = "Opening"
                Case shutterClosing: out = "Closing"
                Case shutterError:   out = "Error"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Get Slewing() As Boolean

    check_connected
    
    Slewing = HW_Slewing
        
    If Not g_show Is Nothing Then
        If g_show.chkSlewing.Value = 1 Then _
            g_show.TrafficChar _
                IIf(Slewing, "Slewing: True", "Slewing: False")
    End If
 
End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "AbortSlew"
    End If
    
    check_connected
    HW_Halt
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub CloseShutter()

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "CloseShutter"
    End If
    
    If Not g_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CloseShutter" & MSG_NOT_IMPLEMENTED
                
    check_connected
    HW_CloseShutter
    
    ' slew code in Startup.bas journals end of close
End Sub

Public Sub FindHome()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "FindHome"
    End If
    
    If Not g_bCanFindHome Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method FindHome" & MSG_NOT_IMPLEMENTED
            
    check_connected
    If Not g_bAtHome Then _
        HW_FindHome
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub OpenShutter()

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "OpenShutter"
    End If
    
    If Not g_bCanSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method OpenShutter" & MSG_NOT_IMPLEMENTED
            
    check_connected
                
    If g_eShutterState = shutterError Then _
        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
            "Method OpenShutter: " & MSG_SHUTTER_ERROR
            
    HW_OpenShutter
        
    ' slew code in Startup.bas journals end of open
End Sub

Public Sub Park()
 
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Park"
    End If
    
    If Not g_bCanPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Park" & MSG_NOT_IMPLEMENTED
    
    check_connected
    If Not g_bAtPark Then _
        HW_Park
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SetPark()

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetPark: " & Format$(g_dDomeAz, "0.0")
    End If
    
    If Not g_bCanSetPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Park" & MSG_NOT_IMPLEMENTED
            
    check_connected
    g_dSetPark = g_dDomeAz
    
    If Not g_bStandardAtPark Then               ' Non-standard, position
        g_bAtPark = True
        g_handBox.RefreshLEDs
    End If
    
    g_handBox.LabelButtons
        
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetupDialog"
    End If
    
    DoSetup
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SlewToAltitude(ByVal Altitude As Double)

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficLine "SlewToAltitude:" & Format$(Altitude, "0.0")
    End If
    
    If Not g_bCanSetAltitude Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltitude" & MSG_NOT_IMPLEMENTED
    
    check_connected
    
    If g_eShutterState = shutterError Then _
        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
            "Method SlewToAltitude " & MSG_SHUTTER_ERROR
            
    If g_eShutterState <> shutterOpen Then _
        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
            "Method SlewToAltitude " & MSG_SHUTTER_NOT_OPEN
            
    If Altitude < g_dMinAlt Or Altitude > g_dMaxAlt Then _
         Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Altitude " & MSG_VAL_OUTOFRANGE
            
    HW_MoveShutter Altitude
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SlewToAzimuth(ByVal Azimuth As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    If Not g_bCanSetAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAzimuth" & MSG_NOT_IMPLEMENTED
    
    check_connected
    check_Az Azimuth
    HW_Move Azimuth
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SyncToAzimuth(ByVal Azimuth As Double)
      
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SyncToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    If Not g_bCanSyncAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToAzimuth" & MSG_NOT_IMPLEMENTED
    
    check_connected
    check_Az Azimuth
    HW_Sync Azimuth
        
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
 
End Sub

Public Sub CommandBlind(ByVal Command As String)

        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBlind" & MSG_NOT_IMPLEMENTED
            
End Sub

Public Function CommandBool(ByVal Command As String) As Boolean

        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBool" & MSG_NOT_IMPLEMENTED
            
End Function

Public Function CommandString(ByVal Command As String) As String

        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandString" & MSG_NOT_IMPLEMENTED
            
End Function

' ================
' Private Routines
' ================

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the dome is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If Not g_bConnected Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
        
End Sub

'---------------------------------------------------------------------
'
' check_Az() - Raise an error if Az is too weird
'
'---------------------------------------------------------------------
Private Sub check_Az(Az As Double)

    If Az = INVALID_COORDINATE Then _
        Err.Raise SCODE_NO_TARGET_COORDS, ERR_SOURCE, _
            "Azimuth " & MSG_NO_TARGET_COORDS
     
    'pwgs changed next line from If Az > 360 Or Az < -360 Then
    If Az >= 360# Or Az < 0# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Azimuth " & MSG_VAL_OUTOFRANGE
            
End Sub

' =======================
' Implementation of IDome
' =======================

Private Sub IDome_AbortSlew()
    AbortSlew
End Sub

Private Property Get IDome_Altitude() As Double
    IDome_Altitude = Altitude
End Property

Private Property Get IDome_AtHome() As Boolean
    IDome_AtHome = AtHome()
End Property

Private Property Get IDome_AtPark() As Boolean
    IDome_AtPark = AtPark
End Property

Private Property Get IDome_Azimuth() As Double
    IDome_Azimuth = Azimuth
End Property

Private Property Get IDome_CanFindHome() As Boolean
    IDome_CanFindHome = CanFindHome
End Property

Private Property Get IDome_CanPark() As Boolean
    IDome_CanPark = CanPark
End Property

Private Property Get IDome_CanSetAltitude() As Boolean
    IDome_CanSetAltitude = CanSetAltitude
End Property

Private Property Get IDome_CanSetAzimuth() As Boolean
    IDome_CanSetAzimuth = CanSetAzimuth
End Property

Private Property Get IDome_CanSetPark() As Boolean
    IDome_CanSetPark = CanSetPark
End Property

Private Property Get IDome_CanSetShutter() As Boolean
    IDome_CanSetShutter = CanSetShutter
End Property

Private Property Get IDome_CanSlave() As Boolean
    IDome_CanSlave = CanSlave
End Property

Private Property Get IDome_CanSyncAzimuth() As Boolean
    IDome_CanSyncAzimuth = CanSyncAzimuth
End Property

Private Sub IDome_CloseShutter()
    CloseShutter
End Sub

Private Sub IDome_CommandBlind(ByVal Command As String)
    CommandBlind (Command)
End Sub

Private Function IDome_CommandBool(ByVal Command As String) As Boolean
    IDome_CommandBool = CommandBool(Command)
End Function

Private Function IDome_CommandString(ByVal Command As String) As String
    IDome_CommandString = CommandString(Command)
End Function

Private Property Let IDome_Connected(ByVal RHS As Boolean)
    Connected = RHS
End Property

Private Property Get IDome_Connected() As Boolean
    IDome_Connected = Connected
End Property

Private Property Get IDome_Description() As String
    IDome_Description = Description
End Property

Private Property Get IDome_DriverInfo() As String
    IDome_DriverInfo = DriverInfo
End Property

Private Sub IDome_FindHome()
    FindHome
End Sub

Private Property Get IDome_InterfaceVersion() As Integer
    IDome_InterfaceVersion = InterfaceVersion
End Property

Private Property Get IDome_Name() As String
    IDome_Name = "I_" & Name
End Property

Private Sub IDome_OpenShutter()
    OpenShutter
End Sub

Private Sub IDome_Park()
    Park
End Sub

Private Sub IDome_SetPark()
    SetPark
End Sub

Private Sub IDome_SetupDialog()
    SetupDialog
End Sub

Private Property Get IDome_ShutterStatus() As AscomInterfacesLib.ShutterState
    IDome_ShutterStatus = ShutterStatus
End Property

Private Property Let IDome_Slaved(ByVal RHS As Boolean)
    Slaved = RHS
End Property

Private Property Get IDome_Slaved() As Boolean
    IDome_Slaved = Slaved
End Property

Private Property Get IDome_Slewing() As Boolean
    IDome_Slewing = Slewing
End Property

Private Sub IDome_SlewToAltitude(ByVal Altitude As Double)
    SlewToAltitude Altitude
End Sub

Private Sub IDome_SlewToAzimuth(ByVal Azimuth As Double)
    SlewToAzimuth Azimuth
End Sub

Private Sub IDome_SyncToAzimuth(ByVal Azimuth As Double)
    SyncToAzimuth Azimuth
End Sub
