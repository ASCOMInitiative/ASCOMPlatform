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
Attribute VB_Description = "ASCOM Dome driver for Pipe and Hub"
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
' 01-Sep-06 jab     Initial edit
' 17-Feb-07 jab     change Name to "Pipe->RealName"
' 03-Jun-07 jab     5.0.0 - Implement new standard IDome interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements IDome

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()
    
    If App.StartMode <> vbSModeStandalone Then _
        DoStartupIf
    
    g_iDomeConnections = g_iDomeConnections + 1
    
    check_object
    
End Sub

Private Sub Class_Terminate()

    ' decrement user count
    If g_iDomeConnections > 0 Then
        g_iDomeConnections = g_iDomeConnections - 1
    
        ' go away if no more users
        If g_iDomeConnections <= 0 Then
            ' If we've never forced a connect, or we're not connected now, then unattach
            If Not g_bDomeManual Or Not g_bDomeConnected Then
                DomeSave
                DomeDelete
                g_setupDlg.UpdateDome False, False
            End If
            
            ' if this was the last thing going, then the whole program is going down
            If g_iConnections <= 0 And g_iFocuserConnections <= 0 And _
                    App.StartMode <> vbSModeStandalone Then
                Unload g_handBox
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Altitude() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "Dome Altitude: "
    End If
    
    On Error GoTo CatchError
    Altitude = g_Dome.Altitude

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(Altitude, False)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get AtHome() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficStart "Dome AtHome: "
    End If
    
    On Error GoTo CatchError
    AtHome = g_Dome.AtHome

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficEnd CStr(AtHome)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Get AtPark() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficStart "Dome AtPark: "
    End If
    
    On Error GoTo CatchError
    AtPark = g_Dome.AtPark

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficEnd CStr(AtPark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Get Azimuth() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "Dome Azimuth: "
    End If

    On Error GoTo CatchError
    Azimuth = g_Dome.Azimuth

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd Format$(Azimuth, "0.0")
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get CanFindHome() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanFindHome: "
    End If

    On Error GoTo CatchError
    CanFindHome = g_Dome.CanFindHome
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanFindHome)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanPark() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanPark: "
    End If

    On Error GoTo CatchError
    CanPark = g_Dome.CanPark
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanPark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetAltitude() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSetAltitude: "
    End If

    On Error GoTo CatchError
    CanSetAltitude = g_Dome.CanSetAltitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetAltitude)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetAzimuth() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSetAzimuth: "
    End If

    On Error GoTo CatchError
    CanSetAzimuth = g_Dome.CanSetAzimuth
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetAzimuth)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetPark() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSetPark: "
    End If

    On Error GoTo CatchError
    CanSetPark = g_Dome.CanSetPark
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetPark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetShutter() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSetShutter: "
    End If
    
    On Error GoTo CatchError
    CanSetShutter = g_Dome.CanSetShutter
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetShutter)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSlave() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSlave: "
    End If
    
    On Error GoTo CatchError
    CanSlave = g_Dome.CanSlave
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSlave)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSyncAzimuth() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome CanSyncAzimuth: "
    End If
    
    On Error GoTo CatchError
    CanSyncAzimuth = g_Dome.CanSyncAzimuth
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSyncAzimuth)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Connected() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Connected: "
    End If
    
    On Error GoTo CatchError
    Connected = g_Dome.Connected
    g_setupDlg.UpdateDome True, Connected
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(Connected)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Connected: -> " & newVal
    End If
    
    On Error GoTo CatchError
    
    out = " (done)"     ' seed
    ' if trying to disconnect and (not last or we're manual), ignor
    If Not newVal And (g_iDomeConnections > 1 Or g_bDomeManual) Then
        If g_iDomeConnections > 1 Then
            out = " (ignored, not last user)"
        ElseIf g_bDomeManual Then
            out = " (ignored, manual connection)"
        End If
    Else
        g_Dome.Connected = newVal
        g_setupDlg.UpdateDome True, newVal
    End If
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd out
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get Description() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Description: "
    End If
    
    On Error GoTo CatchError
    Description = g_Dome.Description
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine Description
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get DriverInfo() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome DriverInfo: "
    End If
    
    On Error GoTo CatchError
    
    ' start with Pipe data
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
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine DriverInfo
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get InterfaceVersion() As Integer

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Dome InterfaceVersion: "
    End If

    On Error GoTo CatchError
    InterfaceVersion = g_Dome.InterfaceVersion
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficLine CStr(InterfaceVersion)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Name() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Name: "
    End If
    
    On Error GoTo CatchError
    Name = App.EXEName & "->" & g_sDomeName
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd Name
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get Slaved() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Slaved: "
    End If
    
    On Error GoTo CatchError
    Slaved = g_Dome.Slaved
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine CStr(Slaved)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let Slaved(ByVal newVal As Boolean)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome Slaved: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Dome.Slaved = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get ShutterStatus() As ShutterState

    Dim out As String
   
    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then _
            g_handBox.TrafficStart "Dome ShutterStatus: "
    End If
    
    On Error GoTo CatchError
    ShutterStatus = g_Dome.ShutterStatus
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then
            out = "unknown"
            Select Case ShutterStatus
                Case shutterOpen:    out = "Open"
                Case shutterClosed:  out = "Close"
                Case shutterOpening: out = "Opening"
                Case shutterClosing: out = "Closing"
                Case shutterError:   out = "Error"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkShutter.Value
    ' does not return

End Property

Public Property Get Slewing() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar "Dome Slewing:"
    End If
    
    On Error GoTo CatchError
    Slewing = g_Dome.Slewing
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar IIf(Slewing, "True", "False")
    End If
 
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Dome AbortSlew"
    End If
    
    On Error GoTo CatchError
    g_Dome.AbortSlew
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub CloseShutter()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then _
            g_handBox.TrafficStart "Dome CloseShutter"
    End If
                
    On Error GoTo CatchError
    g_Dome.CloseShutter
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkShutter.Value
    ' does not return

End Sub

Public Sub CommandBlind(ByVal Command As String)

    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " Then
        
        ' Check for logging messages
        If InStr(1, Command, "Pipe Log Message", vbTextCompare) = 1 Then
            g_handBox.TrafficLine vbCrLf & Command & vbCrLf
        
        ' Check for visible mode
        ElseIf InStr(1, Command, "Pipe Visible", vbTextCompare) = 1 Then
            g_handBox.WindowState = vbNormal
        
        ' Check for "log all" mode
        ElseIf InStr(1, Command, "Pipe Log All", vbTextCompare) = 1 Then
            g_handBox.cmdSetAll_Click
        
        ' Check for clear command
        ElseIf InStr(1, Command, "Pipe Clear", vbTextCompare) = 1 Then
            g_handBox.cmdClear_Click
        
        ' clear manual flag so connection can be dropped
        ElseIf InStr(1, Command, "Pipe Manual Clear", vbTextCompare) = 1 Then
            g_bManual = False
            
        ' unknown command
        Else
            g_handBox.TrafficLine vbCrLf & "Unknown Pipe Command: " & _
                Command & vbCrLf
        End If
        
        Exit Sub
    End If
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome CommandBlind: " & Command
    End If
    
    On Error GoTo CatchError
    g_Dome.CommandBlind Command

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Sub

Public Function CommandBool(ByVal Command As String) As Boolean
    
    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " Then
    
        ' unknown command
        g_handBox.TrafficLine vbCrLf & "Unknown Pipe Command: " & _
            Command & vbCrLf
        
        Exit Function
    End If

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome CommandBool: " & Command
    End If
    
    On Error GoTo CatchError
    CommandBool = g_Dome.CommandBool(Command)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd "(" & CStr(CommandBool) & ")"
    End If
    
    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Function

Public Function CommandString(ByVal Command As String) As String
    
    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " Then
    
        ' unknown command
        g_handBox.TrafficLine vbCrLf & "Unknown Pipe Command: " & _
            Command & vbCrLf
        
        Exit Function
    End If

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome CommandString: " & Command
    End If
    
    On Error GoTo CatchError
    CommandString = g_Dome.CommandString(Command)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd "(" & CommandString & ")"
    End If
    
    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Function

Public Sub FindHome()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Dome FindHome"
    End If
    
    On Error GoTo CatchError
    g_Dome.FindHome

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub OpenShutter()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then _
            g_handBox.TrafficStart "Dome OpenShutter"
    End If
    
    On Error GoTo CatchError
    g_Dome.OpenShutter

    If Not g_handBox Is Nothing Then
        If g_handBox.chkShutter.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkShutter.Value
    ' does not return

End Sub

Public Sub Park()
 
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Dome Park"
    End If
    
    On Error GoTo CatchError
    g_Dome.Park

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SetPark()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome SetPark "
    End If
            
    On Error GoTo CatchError
    g_Dome.SetPark
        
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Sub

Public Sub SetupDialog()
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Dome SetupDialog:"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SlewToAltitude(ByVal Altitude As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine "Dome SlewToAltitude:" & Format$(Altitude, "0.0")
    End If
    
    On Error GoTo CatchError
    g_Dome.SlewToAltitude Altitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToAzimuth(ByVal Azimuth As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine "Dome SlewToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    On Error GoTo CatchError
    g_Dome.SlewToAzimuth Azimuth
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SyncToAzimuth(ByVal Azimuth As Double)
      
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Dome SyncToAzimuth: " & Format$(Azimuth, "0.0")
    End If
    
    On Error GoTo CatchError
    g_Dome.SyncToAzimuth Azimuth
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
 
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

' ================
' Private Routines
' ================

'---------------------------------------------------------------------
'
' check_object() - Raise an error if the scope is not defined/loaded
'
'---------------------------------------------------------------------

Private Sub check_object()

    If (g_Dome Is Nothing) Then
        DomeCreate (g_sDomeID)    ' it will throw an error for failure
        g_setupDlg.UpdateDome False, g_bDomeConnected
    End If
       
End Sub

'---------------------------------------------------------------------
'
' report_error() - Log errors, then raise them
'
'---------------------------------------------------------------------

Private Sub report_error(tNum As Long, tSrc As String, tMsg As String, logIt As Integer)
    
    ' log errors if the user wants them
    If logIt = 1 Then _
        g_handBox.TrafficEnd " (Error) - " & _
            Hex(tNum) & " - " & tSrc & " - " & tMsg
    
    Err.Raise tNum, tSrc, tMsg
    
    ' We're gone now, we should never get here...
    g_handBox.TrafficLine "Internal Pipe error: shouldn't be at end of error report."
        
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
