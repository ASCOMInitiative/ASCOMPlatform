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
Attribute VB_Description = "ASCOM Dome driver for POTH"
' -----------------------------------------------------------------------------'
'   ========
'   DOME.CLS
'   ========
'
' Implementation of the ASCOM Dome Class
'
' Written: Jon Brewster
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
' ??-??-??  jab     sew into POTH
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 10-Jan-06 dpp     Focuser implementation
' 07-Sep-06 jab     Fixed object deletion code to be more robust
' 17-Feb-07 jab     change Name to "POTH->RealName"
' 03-Jun-07 jab     5.0.1 - Implement new standard IDome interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements IDome

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    If App.StartMode <> vbSModeStandalone Then
        DoStartupIf
        g_iDomeConnections = g_iDomeConnections + 1
    End If
    
    DomeCreate (g_sDomeID)
      
End Sub

Private Sub Class_Terminate()
    
    ' if fired up to be a server, then see if this is the last user
    If App.StartMode <> vbSModeStandalone Then
    
        ' decrement user count
        If g_iDomeConnections > 0 Then
            g_iDomeConnections = g_iDomeConnections - 1
            
            ' go away if no more users
            If g_iDomeConnections <= 0 Then
                If g_iConnections <= 0 And g_iFocuserConnections <= 0 Then
                    Unload g_handBox
                Else
                    DomeSave
                    DomeDelete
                End If
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Altitude() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Dome Altitude: "
    End If

'    If Not g_bCanSetAltitude Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Property Altitude" & MSG_NOT_IMPLEMENTED
    
    check_connected
    
'    If g_eShutterState = shutterError Then _
'        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
'            "Property Altitude: " & MSG_SHUTTER_ERROR
'
'    If g_eShutterState <> shutterOpen Then _
'        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
'            "Property Altitude: " & MSG_SHUTTER_NOT_OPEN
    
    Altitude = g_Dome.Altitude

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(Altitude, False)
    End If
    
End Property

Public Property Get AtHome() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficStart "Dome AtHome: "
    End If
    
    check_connected
    AtHome = g_Dome.AtHome

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficEnd CStr(AtHome)
    End If
    
End Property

Public Property Get AtPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficStart "Dome AtPark: "
    End If
    
    check_connected
    AtPark = g_Dome.AtPark

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficEnd CStr(AtPark)
    End If
    
End Property

Public Property Get Azimuth() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Dome Azimuth: "
    End If

'    If Not g_bCanSetAzimuth Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Property Azimuth" & MSG_NOT_IMPLEMENTED

    check_connected
    Azimuth = g_Dome.Azimuth

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(Azimuth, False)
    End If
    
End Property

Public Property Get CanFindHome() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanFindHome: " & g_bDomeFindHome
    End If

    check_connected
    CanFindHome = g_bDomeFindHome
    
End Property

Public Property Get CanPark() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanPark: " & g_bDomePark
    End If

    check_connected
    CanPark = g_bDomePark
    
End Property

Public Property Get CanSetAltitude() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSetAltitude: " & g_bDomeSetAltitude
    End If

    check_connected
    CanSetAltitude = g_bDomeSetAltitude
    
End Property

Public Property Get CanSetAzimuth() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSetAzimuth: " & g_bDomeSetAzimuth
    End If

    check_connected
    CanSetAzimuth = g_bDomeSetAzimuth
        
End Property

Public Property Get CanSetPark() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSetPark: " & g_bDomeSetPark
    End If

    check_connected
    CanSetPark = g_bDomeSetPark
    
End Property

Public Property Get CanSetShutter() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSetShutter: " & g_bDomeSetShutter
    End If
    
    check_connected
    CanSetShutter = g_bDomeSetShutter

End Property

Public Property Get CanSlave() As Boolean

    CanSlave = True
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSlave: " & CanSlave
    End If
    
End Property

Public Property Get CanSyncAzimuth() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome CanSyncAzimuth: " & g_bDomeSyncAzimuth
    End If
    
    check_connected
    CanSyncAzimuth = g_bDomeSyncAzimuth
    
End Property

Public Property Get Connected() As Boolean

    Connected = g_bDomeConnected
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome Connected: " & Connected
    End If
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    
    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome Connected: " & _
                g_bDomeConnected & " -> " & newVal
    End If
    
    out = " (done)"
    
    If g_bDomeConnected = newVal Then
        If Not g_show Is Nothing Then
            If g_show.chkOther.Value = 1 Then _
                g_show.TrafficEnd " no change"
        End If
        Exit Property
    End If
    
    ' only with a state change does anything happen
    If g_bDomeConnected <> newVal Then
        If newVal Then
            g_setupDlg.ConnectDome False
            g_setupDlg.UpdateDomeGlobals
            If g_bDomeConnected Then
                out = " (connected)"
            Else
                out = " (error)"
            End If
        Else
            If App.StartMode = vbSModeStandalone Then
                out = " (ignored, not auto launch)"
            Else
                If g_iDomeConnections <= 1 Then
                    g_setupDlg.ConnectDome False
                    g_setupDlg.UpdateDomeGlobals
                Else
                    out = " (ignored, not last user)"
                End If
            End If
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get Description() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome Description"
    End If
    
    check_connected
    Description = g_Dome.Description
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get DriverInfo() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome DriverInfo"
    End If
    
    ' start with POTH data
    DriverInfo = App.EXEName & "->" & g_sDomeName & vbCrLf & _
        App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
    If App.CompanyName <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
    If App.LegalCopyright <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
    
    ' tack on connected driver info
    If g_bDomeConnected Then
        DriverInfo = DriverInfo & vbCrLf & vbCrLf & _
            "Connected to a dome driver with the following information:" & vbCrLf & _
            g_Dome.DriverInfo
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
        
End Property

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 1
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Dome InterfaceVersion: " & InterfaceVersion
    End If
    
End Property

Public Property Get Name() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome Name: "
    End If
    
    Name = App.EXEName & "->" & g_sDomeName
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd Name
    End If
    
End Property

Public Property Get Slaved() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome Slaved: "
    End If
    
    check_connected
    
    ' if dome driver supports slaving ... ???
    
    Slaved = g_bSlaved
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(Slaved)
    End If
    
End Property

Public Property Let Slaved(ByVal newVal As Boolean)
    
    check_connected
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome Slaved: " & g_bSlaved & " -> " & newVal
    End If
    
    If newVal <> g_bSlaved Then
        g_bSlaveSlew = False
        If newVal Then
            ' check scope connection
            If Not g_bConnected Then
                Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, _
                    MSG_NOT_CONNECTED
            End If
            
            g_bSlaved = True
        Else
            g_bSlaved = False
        End If
        
        g_handBox.Slave
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get ShutterStatus() As ShutterState

    Dim out As String
   
    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "Dome ShutterStatus: "
    End If
    
'    If Not g_bCanSetShutter Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Property ShutterStatus" & MSG_NOT_IMPLEMENTED
    
    check_connected
    ShutterStatus = g_Dome.ShutterStatus
    
    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then
            out = "unknown"
            Select Case ShutterStatus
                Case shutterOpen:    out = "Open"
                Case shutterClosed:  out = "Close"
                Case shutterOpening: out = "Opening"
                Case shutterClosing: out = "Closing"
                Case shutterError:   out = "Error"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Get Slewing() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "Dome Slewing: "
    End If
    
    check_connected
    
    ' slewing is true if either Alt or Az are in motion, but shutter
    ' opening and closing do not count as slewing
    
    Slewing = g_Dome.Slewing
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(Slewing, "True ", "False ")
    End If
 
End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Dome AbortSlew"
    End If
    
    check_connected
    DomeAbortSlew
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub CloseShutter()

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "Dome CloseShutter"
    End If
    
'    If Not g_bDomeSetShutter Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Method CloseShutter" & MSG_NOT_IMPLEMENTED
                
    check_connected
    g_Dome.CloseShutter
    
    ' slew code in Startup.bas journals end of close
End Sub

Public Sub CommandBlind(ByVal Command As String)
    
    ' Check for POTH specific commands
    If InStr(1, Command, "POTH Command:", vbTextCompare) = 1 Then
        
        ' unknown command
        If Not g_show Is Nothing Then
            g_show.TrafficLine vbCrLf & "Unknown POTH Command: " & _
                Command & vbCrLf
        End If
        
        Exit Sub
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome CommandBlind"
    End If
    
    check_connected
    g_Dome.CommandBlind Command

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Function CommandBool(ByVal Command As String) As Boolean

    ' Check for POTH specific commands
    If InStr(1, Command, "POTH Command:", vbTextCompare) = 1 Then
        
        ' unknown command
        If Not g_show Is Nothing Then
            g_show.TrafficLine vbCrLf & "Unknown POTH Command: " & _
                Command & vbCrLf
        End If
        
        Exit Function
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome CommandBool: " & Command
    End If
    
    check_connected
    CommandBool = g_Dome.CommandBool(Command)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd "(" & CStr(CommandBool) & ")"
    End If
    
End Function

Public Function CommandString(ByVal Command As String) As String
    
    ' Check for POTH specific commands
    If InStr(1, Command, "POTH Command:", vbTextCompare) = 1 Then
        
        ' unknown command
        If Not g_show Is Nothing Then
            g_show.TrafficLine vbCrLf & "Unknown POTH Command: " & _
                Command & vbCrLf
        End If
        
        Exit Function
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome CommandString: " & Command
    End If
    
    check_connected
    CommandString = g_Dome.CommandString(Command)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd "(" & CommandString & ")"
    End If
    
End Function

Public Sub FindHome()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Dome FindHome"
    End If
    
    If Not g_bDomeFindHome Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method FindHome" & MSG_NOT_IMPLEMENTED
            
    check_connected
    
    DomeFindHome
    
End Sub

Public Sub OpenShutter()

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficStart "Dome OpenShutter"
    End If
    
    If Not g_bDomeSetShutter Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method OpenShutter" & MSG_NOT_IMPLEMENTED
            
    check_connected
                
'    If g_eShutterState = shutterError Then _
'        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
'            "Method OpenShutter: " & MSG_SHUTTER_ERROR
            
    g_Dome.OpenShutter
        
    ' slew code in Startup.bas journals end of open
End Sub

Public Sub Park()
 
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Dome Park"
    End If
    
    If Not g_bDomePark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Park" & MSG_NOT_IMPLEMENTED
    
    check_connected

'    If g_bSlaved Then _
'        Err.Raise SCODE_SLEW_WHILE_SLAVED, ERR_SOURCE, _
'            "Method Park: " & MSG_SLEW_WHILE_SLAVED
            
    DomePark
    
End Sub

Public Sub SetPark()

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome SetPark "
    End If
    
    If Not g_bDomeSetPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SetPark" & MSG_NOT_IMPLEMENTED
            
    check_connected
    g_Dome.SetPark
        
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Dome SetupDialog"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SlewToAltitude(ByVal Altitude As Double)

    If Not g_show Is Nothing Then
        If g_show.chkShutter.Value = 1 Then _
            g_show.TrafficLine "Dome SlewToAltitude:" & Format$(Altitude, "0.0")
    End If
    
    If Not g_bDomeSetAltitude Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltitude" & MSG_NOT_IMPLEMENTED
    
    check_connected
    
'    If g_eShutterState = shutterError Then _
'        Err.Raise SCODE_SHUTTER_ERROR, ERR_SOURCE, _
'            "Method SlewToAltitude " & MSG_SHUTTER_ERROR
'
'    If g_eShutterState <> shutterOpen Then _
'        Err.Raise SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
'            "Method SlewToAltitude " & MSG_SHUTTER_NOT_OPEN
    
    If g_bSlaved Then
        g_bSlaved = False
        g_handBox.Slave
    End If
    
    g_Dome.SlewToAltitude Altitude
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SlewToAzimuth(ByVal Azimuth As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "Dome SlewToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    If Not g_bDomeSetAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAzimuth" & MSG_NOT_IMPLEMENTED
    
    check_connected
    check_Az Azimuth
    
    If g_bSlaved Then
        g_bSlaved = False
        g_handBox.Slave
    End If
    
    g_Dome.SlewToAzimuth Azimuth
    
    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SyncToAzimuth(ByVal Azimuth As Double)
      
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "Dome SyncToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    If Not g_bDomeSyncAzimuth Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToAzimuth" & MSG_NOT_IMPLEMENTED
    
    check_connected
    check_Az Azimuth
        
    g_Dome.SyncToAzimuth Azimuth
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
 
End Sub

' ----------------
' Private Routines
' ----------------

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the dome is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If (Not g_bDomeConnected) Or (g_Dome Is Nothing) Then _
        Err.Raise SCODE_DOME_NOT_CONNECTED, ERR_SOURCE, _
            MSG_DOME_NOT_CONNECTED
        
End Sub

'---------------------------------------------------------------------
'
' check_Az() - Raise an error if Az is too weird
'
'---------------------------------------------------------------------
Private Sub check_Az(Az As Double)

    If Az > 360 Or Az < -360 Then _
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
