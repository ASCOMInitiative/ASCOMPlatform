VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Telescope"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Telescope driver for Pipe and Hub"
' -----------------------------------------------------------------------------'
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the Pipe Telescope class
'
' Written: Jon Brewster
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 01-Sep-06 jab     Initial edit
' 17-Sep-06 jab     Added logging of special CommandBlind messages
' 17-Feb-07 jab     change Name to "Pipe->RealName"
' 03-Jun-07 jab     5.0.0 - Implement new standard ITelescope interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements ITelescope

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    If App.StartMode <> vbSModeStandalone Then _
        DoStartupIf
    
    g_iConnections = g_iConnections + 1
    
    check_object
    
End Sub

Private Sub Class_Terminate()

    ' decrement user count
    If g_iConnections > 0 Then
        g_iConnections = g_iConnections - 1
    
        ' go away if no more users
        If g_iConnections <= 0 Then
            ' If we've never forced a connect, or we're not connected now, then unattach
            If Not g_bManual Or Not g_bConnected Then
                ScopeSave
                ScopeDelete
                g_setupDlg.UpdateScope False, False
            End If
            
            ' if this was the last thing going, then the whole program is going down
            If g_iDomeConnections <= 0 And g_iFocuserConnections <= 0 And _
                    App.StartMode <> vbSModeStandalone Then
                Unload g_handBox
            End If
        End If
    End If

End Sub

' ----------
' Properties
' ----------

Public Property Get AlignmentMode() As AlignmentModes

    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "AlignmentMode: "
    End If
    
    On Error GoTo CatchError
    AlignmentMode = g_Scope.AlignmentMode
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then
            out = "unknown"
            Select Case AlignmentMode
                Case algAltAz:       out = "Alt-Azimuth"
                Case algPolar:       out = "Equatorial"
                Case algGermanPolar: out = "German Equatorial"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Altitude() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "Altitude: "
    End If
    
    On Error GoTo CatchError
    
    Altitude = g_Scope.Altitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(Altitude, False)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get ApertureArea() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "ApertureArea: "
    End If
    
    On Error GoTo CatchError
    ApertureArea = g_Scope.ApertureArea
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(ApertureArea)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get ApertureDiameter() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "ApertureDiameter: "
    End If
    
    On Error GoTo CatchError
    ApertureDiameter = g_Scope.ApertureDiameter
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(ApertureDiameter)
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get AtHome() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficStart "AtHome: "
    End If
    
    On Error GoTo CatchError
    AtHome = g_Scope.AtHome

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
            g_handBox.TrafficStart "AtPark: "
    End If
    
    On Error GoTo CatchError
    AtPark = g_Scope.AtPark

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
            g_handBox.TrafficStart "Azimuth: "
    End If
    
    On Error GoTo CatchError
    Azimuth = g_Scope.Azimuth
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(Azimuth, False)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get CanFindHome() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanFindHome: "
    End If
    
    On Error GoTo CatchError
    CanFindHome = g_Scope.CanFindHome
    
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
            g_handBox.TrafficStart "CanPark: "
    End If

    On Error GoTo CatchError
    CanPark = g_Scope.CanPark
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanPark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanPulseGuide() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanPulseGuide: "
    End If

    On Error GoTo CatchError
    CanPulseGuide = g_Scope.CanPulseGuide
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanPulseGuide)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetDeclinationRate() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetDeclinationRate: "
    End If
    
    On Error GoTo CatchError
    CanSetDeclinationRate = g_Scope.CanSetDeclinationRate

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetDeclinationRate)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetGuideRates() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetGuideRates: "
    End If
    
    On Error GoTo CatchError
    CanSetGuideRates = g_Scope.CanSetGuideRates

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetGuideRates)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetPark() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetPark: "
    End If

    On Error GoTo CatchError
    CanSetPark = g_Scope.CanSetPark

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetPark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetPierSide() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetPierSide: "
    End If

    On Error GoTo CatchError
    CanSetPierSide = g_Scope.CanSetPierSide
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetPierSide)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetRightAscensionRate() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetRightAscensionRate: "
    End If
    
    On Error GoTo CatchError
    CanSetRightAscensionRate = g_Scope.CanSetRightAscensionRate

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetRightAscensionRate)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSetTracking() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSetTracking: "
    End If

    On Error GoTo CatchError
    CanSetTracking = g_Scope.CanSetTracking
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSetTracking)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSlew() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSlew: "
    End If
 
    On Error GoTo CatchError
    CanSlew = g_Scope.CanSlew
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSlew)
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSlewAltAz() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSlewAltAz: "
    End If
 
    On Error GoTo CatchError
    CanSlewAltAz = g_Scope.CanSlewAltAz
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSlewAltAz)
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSlewAltAzAsync() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSlewAltAzAsync: "
    End If
 
    On Error GoTo CatchError
    CanSlewAltAzAsync = g_Scope.CanSlewAltAzAsync
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSlewAltAzAsync)
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSlewAsync() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSlewAsync: "
    End If

    On Error GoTo CatchError
    CanSlewAsync = g_Scope.CanSlewAsync
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSlewAsync)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSync() As Boolean
        
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSync: "
    End If

    On Error GoTo CatchError
    CanSync = g_Scope.CanSync
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSync)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanSyncAltAz() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanSyncAltAz: "
    End If

    On Error GoTo CatchError
    CanSyncAltAz = g_Scope.CanSyncAltAz
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanSyncAltAz)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get CanUnpark() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "CanUnpark: "
    End If

    On Error GoTo CatchError
    CanUnpark = g_Scope.CanUnpark
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanUnpark)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Connected() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Connected: "
    End If
    
    On Error GoTo CatchError
    Connected = g_Scope.Connected
    g_setupDlg.UpdateScope True, Connected
    
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
            g_handBox.TrafficStart "Connected: -> " & newVal
    End If
    
    On Error GoTo CatchError
    
    out = " (done)"     ' seed
    ' if trying to disconnect and (not last or we're manual), ignor
    If Not newVal And (g_iConnections > 1 Or g_bManual) Then
        If g_iConnections > 1 Then
            out = " (ignored, not last user)"
        ElseIf g_bManual Then
            out = " (ignored, manual connection)"
        End If
    Else
        g_Scope.Connected = newVal
        g_setupDlg.UpdateScope True, newVal
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

Public Property Get Declination() As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "Declination: "
    End If
    
    On Error GoTo CatchError
    Declination = g_Scope.Declination
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(Declination, True)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get DeclinationRate() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "DeclinationRate: "
    End If
    
    On Error GoTo CatchError
    DeclinationRate = g_Scope.DeclinationRate
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(DeclinationRate)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let DeclinationRate(ByVal newVal As Double)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "DeclinationRate: -> " & newVal
    End If

    On Error GoTo CatchError
    g_Scope.DeclinationRate = newVal
         
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get Description() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Description: "
    End If
    
    On Error GoTo CatchError
    Description = g_Scope.Description
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine Description
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get DoesRefraction() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "DoesRefraction: "
    End If
    
    On Error GoTo CatchError
    DoesRefraction = g_Scope.DoesRefraction
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(DoesRefraction)
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Let DoesRefraction(ByVal newVal As Boolean)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "DoesRefraction: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Scope.DoesRefraction = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get DriverInfo() As String
    Dim tmpInfo As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "DriverInfo: "
    End If
    
    On Error GoTo CatchError
    
    ' start with Pipe data
    DriverInfo = App.EXEName & "->" & g_sScopeName & vbCrLf & _
        App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
    If App.CompanyName <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
    If App.LegalCopyright <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
    
    ' attempt to get down stream driver information
    On Error Resume Next
    tmpInfo = ""
    tmpInfo = g_Scope.DriverInfo
    If tmpInfo <> "" Then
        DriverInfo = DriverInfo & vbCrLf & vbCrLf & _
            "Connected to a scope driver with the following information:" & vbCrLf & _
            tmpInfo
    End If
    On Error GoTo CatchError
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine DriverInfo
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get DriverVersion() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "DriverVersion: "
    End If
    
    On Error GoTo CatchError
    DriverVersion = g_Scope.DriverVersion
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficLine DriverVersion
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get EquatorialSystem() As EquatorialCoordinateType

    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "EquatorialSystem: "
    End If
    
    On Error GoTo CatchError
    EquatorialSystem = g_Scope.EquatorialSystem
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then
            out = "unknown"
            Select Case EquatorialSystem
                Case equLocalTopocentric:   out = "LocalTopocentric"
                Case equB1950:              out = "B1950"
                Case equJ2000:              out = "J2000"
                Case equJ2050:              out = "J2050"
                Case equOther:              out = "Other"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get FocalLength() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "FocalLength: "
    End If
    
    On Error GoTo CatchError
    FocalLength = g_Scope.FocalLength
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(FocalLength)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get GuideRateDeclination() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "GuideRateDeclination: "
    End If

    On Error GoTo CatchError
    GuideRateDeclination = g_Scope.GuideRateDeclination

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(GuideRateDeclination)
    End If

    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let GuideRateDeclination(ByVal newVal As Double)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "GuideRateDeclination: -> " & newVal
    End If

    On Error GoTo CatchError
    g_Scope.GuideRateDeclination = newVal
         
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get GuideRateRightAscension() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "GuideRateRightAscension: "
    End If

    On Error GoTo CatchError
    GuideRateRightAscension = g_Scope.GuideRateRightAscension

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(GuideRateRightAscension)
    End If

    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let GuideRateRightAscension(ByVal newVal As Double)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "GuideRateRightAscension: -> " & newVal
    End If

    On Error GoTo CatchError
    g_Scope.GuideRateRightAscension = newVal
         
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get InterfaceVersion() As Integer

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "InterfaceVersion: "
    End If

    On Error GoTo CatchError
    InterfaceVersion = g_Scope.InterfaceVersion
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(InterfaceVersion)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get IsPulseGuiding() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar "IsPulseGuiding: "
    End If

    On Error GoTo CatchError
    IsPulseGuiding = g_Scope.IsPulseGuiding
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar CStr(IsPulseGuiding)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Get Name() As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Name: "
    End If
  
    On Error GoTo CatchError
    g_sScopeName = Trim(g_Scope.Name)
    Name = App.EXEName & "->" & g_sScopeName
        
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd Name
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get RightAscension() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "RightAscension: "
    End If
    
    On Error GoTo CatchError
    RightAscension = g_Scope.RightAscension
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(RightAscension, False)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get RightAscensionRate() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "RightAscensionRate: "
    End If
    
    On Error GoTo CatchError
    RightAscensionRate = g_Scope.RightAscensionRate
      
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(RightAscensionRate)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "RightAscensionRate: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Scope.RightAscensionRate = newVal

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get SideOfPier() As PierSide
    
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficStart "SideOfPier: "
    End If
    
    On Error GoTo CatchError
    SideOfPier = g_Scope.SideOfPier
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then
            out = "unknown"
            Select Case SideOfPier
                Case pierUnknown:    out = "Unknown"
                Case pierEast:       out = "East"
                Case pierWest:       out = "West"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Let SideOfPier(ByVal newVal As PierSide)
    
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case newVal
                Case pierUnknown:    out = "Unknown"
                Case pierEast:       out = "East"
                Case pierWest:       out = "West"
            End Select
            g_handBox.TrafficStart "SideOfPier: -> " & out
        End If
    End If
    
    On Error GoTo CatchError
    g_Scope.SideOfPier newVal
            
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Property

Public Property Get SiderealTime() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficStart "SiderealTime: "
    End If
    
    On Error GoTo CatchError
    SiderealTime = g_Scope.SiderealTime
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(SiderealTime, False)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkTime.Value
    ' does not return

End Property

Public Property Get SiteElevation() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteElevation: "
    End If
    
    On Error GoTo CatchError
    SiteElevation = g_Scope.SiteElevation
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(SiteElevation)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let SiteElevation(ByVal newVal As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteElevation: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Scope.SiteElevation = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get SiteLatitude() As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteLatitude: "
    End If
    
    On Error GoTo CatchError
    SiteLatitude = g_Scope.SiteLatitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(SiteLatitude, True)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let SiteLatitude(ByVal newVal As Double)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteLatitude: -> " & FmtSexa(newVal, True)
    End If
    
    On Error GoTo CatchError
    g_Scope.SiteLatitude = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get SiteLongitude() As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteLongitude: "
    End If
    
    On Error GoTo CatchError
    SiteLongitude = g_Scope.SiteLongitude

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(SiteLongitude, False)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let SiteLongitude(ByVal newVal As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SiteLongitude: -> " & FmtSexa(newVal, False)
    End If
    
    On Error GoTo CatchError
    g_Scope.SiteLongitude = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If

    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get Slewing() As Boolean
        
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar "Slewing:"
    End If
    
    On Error GoTo CatchError
    Slewing = g_Scope.Slewing
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar IIf(Slewing, "True", "False")
    End If
        
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Get SlewSettleTime() As Integer

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SlewSettleTime: "
    End If
    
    On Error GoTo CatchError
    SlewSettleTime = g_Scope.SlewSettleTime
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(SlewSettleTime)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let SlewSettleTime(ByVal newVal As Integer)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "SlewSettleTime: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Scope.SlewSettleTime = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get TargetDeclination() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "TargetDeclination: "
    End If
    
    On Error GoTo CatchError
    TargetDeclination = g_Scope.TargetDeclination
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(TargetDeclination, True)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "TargetDeclination: " & _
                " -> " & FmtSexa(newVal, True)
    End If
    
    On Error GoTo CatchError
    g_Scope.TargetDeclination = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Property

Public Property Get TargetRightAscension() As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "TargetRightAscension: "
    End If
    
    On Error GoTo CatchError
    TargetRightAscension = g_Scope.TargetRightAscension
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd FmtSexa(TargetRightAscension, False)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "TargetRightAscension: " & _
                " -> " & FmtSexa(newVal, False)
    End If
    
    On Error GoTo CatchError
    g_Scope.TargetRightAscension = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Property

Public Property Get Tracking() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar "Tracking:"
    End If
    
    On Error GoTo CatchError
    Tracking = g_Scope.Tracking
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar CStr(Tracking)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Let Tracking(ByVal newVal As Boolean)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Tracking: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Scope.Tracking = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get TrackingRate() As DriveRates

    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "TrackingRate: "
    End If
    
    On Error GoTo CatchError
    TrackingRate = g_Scope.TrackingRate

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then
            out = "unknown"
            Select Case TrackingRate
                Case driveSidereal:     out = "Sidereal"
                Case driveLunar:        out = "Lunar"
                Case driveSolar:        out = "Solar"
                Case driveKing:         out = "King"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let TrackingRate(ByVal newVal As DriveRates)

    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then
            out = "unknown"
            Select Case newVal
                Case driveSidereal:     out = "Sidereal"
                Case driveLunar:        out = "Lunar"
                Case driveSolar:        out = "Solar"
                Case driveKing:         out = "King"
            End Select
            g_handBox.TrafficStart "TrackingRate: -> " & out
        End If
    End If
    
    On Error GoTo CatchError
    g_Scope.TrackingRate = newVal
        
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get TrackingRates() As Object

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "TrackingRates: "
    End If

    On Error GoTo CatchError
    Set TrackingRates = g_Scope.TrackingRates
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get UTCDate() As Date

    Dim CurDate As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficStart "UTCDate: "
    End If
    
    On Error GoTo CatchError
    UTCDate = g_Scope.UTCDate

    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficEnd CStr(UTCDate)
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkTime.Value
    ' does not return

End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Dim CurDate As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficStart "UTCDate: " & " -> " & CStr(newVal)
    End If
    
    On Error GoTo CatchError
    g_Scope.UTCDate = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkTime.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
            
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkTime.Value
    ' does not return

End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "AbortSlew:"
    End If
    
    On Error GoTo CatchError
    g_Scope.AbortSlew
     
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Function AxisRates(ByVal Axis As TelescopeAxes) As Object

    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_handBox.TrafficStart "AxisRates " & out & ":"
        End If
    End If

    On Error GoTo CatchError
    Set AxisRates = g_Scope.AxisRates(Axis)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If

    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Function

Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_handBox.TrafficStart "CanMoveAxis " & out & ": "
        End If
    End If

    On Error GoTo CatchError
    CanMoveAxis = g_Scope.CanMoveAxis(Axis)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CanMoveAxis)
    End If

    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Function

Public Sub CommandBlind(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False)
    
    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " And App.Title = "Pipe" Then
    
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
            g_handBox.TrafficStart "CommandBlind (" & Command & ") Raw = " & Raw & ": "
    End If
    
    On Error GoTo CatchError
    g_Scope.CommandBlind Command, Raw

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Sub

Public Function CommandBool(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As Boolean
    
    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " Then
    
        ' unknown command
        g_handBox.TrafficLine vbCrLf & "Unknown Pipe Command: " & _
            Command & vbCrLf
        
        Exit Function
    End If

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "CommandBool (" & Command & ") Raw = " & Raw & ": "
    End If
    
    On Error GoTo CatchError
    CommandBool = g_Scope.CommandBool(Command, Raw)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(CommandBool)
    End If
    
    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Function

Public Function CommandString(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As String
    
    ' check for a possible Pipe specific command
    If Left(Command, 5) = "Pipe " Then
        
        ' unknown command
        g_handBox.TrafficLine vbCrLf & "Unknown Pipe Command: " & _
            Command & vbCrLf
        
        Exit Function
    End If
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "CommandString (" & Command & ") Raw = " & Raw & ": "
    End If
    
    On Error GoTo CatchError
    CommandString = g_Scope.CommandString(Command, Raw)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CommandString
    End If
    
    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Function

Public Function DestinationSideOfPier(ByVal RightAscension As Double, _
        ByVal Declination As Double) As PierSide

    Dim out As String

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "DestinationSideOfPier" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ": "
    End If
    
    On Error GoTo CatchError
    DestinationSideOfPier = g_Scope.DestinationSideOfPier(RightAscension, Declination)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then
            out = "unknown"
            Select Case DestinationSideOfPier
                Case pierUnknown:  out = "Unknown"
                Case pierEast:     out = "East"
                Case pierWest:     out = "West"
            End Select
            g_handBox.TrafficEnd out
        End If
    End If
    
    Exit Function
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Function

Public Sub FindHome()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "FindHome:"
    End If
    
    On Error GoTo CatchError
    g_Scope.FindHome

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_handBox.TrafficStart "MoveAxis " & out & ", " & Rate & ":"
        End If
    End If

    On Error GoTo CatchError
    g_Scope.MoveAxis Axis, Rate

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If

    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub Park()
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Park:"
    End If
    
    On Error GoTo CatchError
    g_Scope.Park
  
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)

    Dim out As String
 
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case Direction
                Case guideNorth:    out = "North"
                Case guideSouth:    out = "South"
                Case guideEast:     out = "East"
                Case guideWest:     out = "West"
            End Select
            g_handBox.TrafficStart "PulseGuide: " & out & ", " & Duration & ":"
        End If
    End If
    
    On Error GoTo CatchError
    g_Scope.PulseGuide Direction, Duration
    
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
            g_handBox.TrafficStart "SetPark:"
    End If
    
    On Error GoTo CatchError
    g_Scope.SetPark
    
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
            g_handBox.TrafficStart "SetupDialog:"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SlewToAltAz(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    
    Dim RA As Double, Dec As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SlewToAltAz:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
        
    On Error GoTo CatchError
    g_Scope.SlewToAltAz Azimuth, Altitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (slew complete)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    Dim RA As Double, Dec As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SlewToAltAzAsync:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
    
    On Error GoTo CatchError
    g_Scope.SlewToAltAzAsync Azimuth, Altitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
 
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SlewToCoordinates:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
        
    On Error GoTo CatchError
    g_Scope.SlewToCoordinates RightAscension, Declination
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (slew complete)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine "SlewToCoordinatesAsync:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
    
    On Error GoTo CatchError
    g_Scope.SlewToCoordinatesAsync RightAscension, Declination
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToTarget()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SlewToTarget:"
    End If
    
    On Error GoTo CatchError
    g_Scope.SlewToTarget
                
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine " (done)"
    End If

    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SlewToTargetAsync()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SlewToTargetAsync:"
    End If
    
    On Error GoTo CatchError
    g_Scope.SlewToTargetAsync
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
     
    Dim RA As Double, Dec As Double

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine "SyncToAltAz:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
    
    On Error GoTo CatchError
    g_Scope.SyncToAltAz Azimuth, Altitude
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
     
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficLine "SyncToCoordinates:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
    
    On Error GoTo CatchError
    g_Scope.SyncToCoordinates RightAscension, Declination
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SyncToTarget()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "SyncToTarget:"
    End If
    
    On Error GoTo CatchError
    g_Scope.SyncToTarget
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub Unpark()
    Dim m_Util As DriverHelper.Util
    Set m_Util = New DriverHelper.Util
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "UnPark:"
    End If
    
    On Error GoTo CatchError
    g_Scope.Unpark
    
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

    If (g_Scope Is Nothing) Then
        ScopeCreate (g_sScopeID)    ' it will throw an error for failure
        g_setupDlg.UpdateScope False, g_bConnected
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

' ============================
' Implementation of ITelescope
' ============================

Private Sub ITelescope_AbortSlew()
    AbortSlew
End Sub

Private Property Get ITelescope_AlignmentMode() As AlignmentModes
    ITelescope_AlignmentMode = AlignmentMode
End Property

Private Property Get ITelescope_Altitude() As Double
    ITelescope_Altitude = Altitude
End Property

Private Property Get ITelescope_ApertureArea() As Double
    ITelescope_ApertureArea = ApertureArea
End Property

Private Property Get ITelescope_ApertureDiameter() As Double
    ITelescope_ApertureDiameter = ApertureDiameter
End Property

Private Property Get ITelescope_AtHome() As Boolean
    ITelescope_AtHome = AtHome
End Property

Private Property Get ITelescope_AtPark() As Boolean
    ITelescope_AtPark = AtPark
End Property

Private Function ITelescope_AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates
    '
    ' Note that this more or less "casts" our internal AxisRates
    ' object's interface to AxisRates.
    '
    Set ITelescope_AxisRates = AxisRates(Axis)
End Function

Private Property Get ITelescope_Azimuth() As Double
    ITelescope_Azimuth = Azimuth
End Property

Private Property Get ITelescope_CanFindHome() As Boolean
    ITelescope_CanFindHome = CanFindHome
End Property

Private Function ITelescope_CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    ITelescope_CanMoveAxis = CanMoveAxis(Axis)
End Function

Private Property Get ITelescope_CanPark() As Boolean
    ITelescope_CanPark = CanPark
End Property

Private Property Get ITelescope_CanPulseGuide() As Boolean
    ITelescope_CanPulseGuide = CanPulseGuide
End Property

Private Property Get ITelescope_CanSetDeclinationRate() As Boolean
    ITelescope_CanSetDeclinationRate = CanSetDeclinationRate
End Property

Private Property Get ITelescope_CanSetGuideRates() As Boolean
    ITelescope_CanSetGuideRates = CanSetGuideRates
End Property

Private Property Get ITelescope_CanSetPark() As Boolean
    ITelescope_CanSetPark = CanSetPark
End Property

Private Property Get ITelescope_CanSetPierSide() As Boolean
    ITelescope_CanSetPierSide = CanSetPierSide
End Property

Private Property Get ITelescope_CanSetRightAscensionRate() As Boolean
    ITelescope_CanSetRightAscensionRate = CanSetRightAscensionRate
End Property

Private Property Get ITelescope_CanSetTracking() As Boolean
    ITelescope_CanSetTracking = CanSetTracking
End Property

Private Property Get ITelescope_CanSlew() As Boolean
    ITelescope_CanSlew = CanSlew
End Property

Private Property Get ITelescope_CanSlewAltAz() As Boolean
    ITelescope_CanSlewAltAz = CanSlewAltAz
End Property

Private Property Get ITelescope_CanSlewAltAzAsync() As Boolean
    ITelescope_CanSlewAltAzAsync = CanSlewAltAzAsync
End Property

Private Property Get ITelescope_CanSlewAsync() As Boolean
    ITelescope_CanSlewAsync = CanSlewAsync
End Property

Private Property Get ITelescope_CanSync() As Boolean
    ITelescope_CanSync = CanSync
End Property

Private Property Get ITelescope_CanSyncAltAz() As Boolean
    ITelescope_CanSyncAltAz = CanSyncAltAz
End Property

Private Property Get ITelescope_CanUnpark() As Boolean
    ITelescope_CanUnpark = CanUnpark
End Property

Private Sub ITelescope_CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)
    CommandBlind Command, Raw
End Sub

Private Function ITelescope_CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean
    ITelescope_CommandBool = CommandBool(Command, Raw)
End Function

Private Function ITelescope_CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String
    ITelescope_CommandString = CommandString(Command, Raw)
End Function

Private Property Let ITelescope_Connected(ByVal RHS As Boolean)
    Connected = RHS
End Property

Private Property Get ITelescope_Connected() As Boolean
    ITelescope_Connected = Connected
End Property

Private Property Get ITelescope_Declination() As Double
    ITelescope_Declination = Declination
End Property

Private Property Let ITelescope_DeclinationRate(ByVal RHS As Double)
    DeclinationRate = RHS
End Property

Private Property Get ITelescope_DeclinationRate() As Double
    ITelescope_DeclinationRate = DeclinationRate
End Property

Private Property Get ITelescope_Description() As String
    ITelescope_Description = Description
End Property

Private Function ITelescope_DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide
    ITelescope_DestinationSideOfPier = DestinationSideOfPier(RightAscension, Declination)
End Function

Private Property Let ITelescope_DoesRefraction(ByVal RHS As Boolean)
    DoesRefraction = RHS
End Property

Private Property Get ITelescope_DoesRefraction() As Boolean
    ITelescope_DoesRefraction = DoesRefraction
End Property

Private Property Get ITelescope_DriverInfo() As String
    ITelescope_DriverInfo = DriverInfo
End Property

Private Property Get ITelescope_DriverVersion() As String
    ITelescope_DriverVersion = DriverVersion
End Property

Private Property Get ITelescope_EquatorialSystem() As EquatorialCoordinateType
    ITelescope_EquatorialSystem = EquatorialSystem
End Property

Private Sub ITelescope_FindHome()
    FindHome
End Sub

Private Property Get ITelescope_FocalLength() As Double
    ITelescope_FocalLength = FocalLength
End Property

Private Property Let ITelescope_GuideRateDeclination(ByVal RHS As Double)
    GuideRateDeclination = RHS
End Property

Private Property Get ITelescope_GuideRateDeclination() As Double
    ITelescope_GuideRateDeclination = GuideRateDeclination
End Property

Private Property Let ITelescope_GuideRateRightAscension(ByVal RHS As Double)
    GuideRateRightAscension = RHS
End Property

Private Property Get ITelescope_GuideRateRightAscension() As Double
    ITelescope_GuideRateRightAscension = GuideRateRightAscension
End Property

Private Property Get ITelescope_InterfaceVersion() As Integer
    ITelescope_InterfaceVersion = InterfaceVersion
End Property

Private Property Get ITelescope_IsPulseGuiding() As Boolean
    ITelescope_IsPulseGuiding = IsPulseGuiding
End Property

Private Sub ITelescope_MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    MoveAxis Axis, Rate
End Sub

Private Property Get ITelescope_Name() As String
    ITelescope_Name = "I_" & Name
End Property

Private Sub ITelescope_Park()
    Park
End Sub

Private Sub ITelescope_PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)
    PulseGuide Direction, Duration
End Sub

Private Property Get ITelescope_RightAscension() As Double
    ITelescope_RightAscension = RightAscension
End Property

Private Property Let ITelescope_RightAscensionRate(ByVal RHS As Double)
    RightAscensionRate = RHS
End Property

Private Property Get ITelescope_RightAscensionRate() As Double
    ITelescope_RightAscensionRate = RightAscensionRate
End Property

Private Sub ITelescope_SetPark()
    SetPark
End Sub

Private Sub ITelescope_SetupDialog()
    SetupDialog
End Sub

Private Property Let ITelescope_SideOfPier(ByVal RHS As PierSide)
    SideOfPier = RHS
End Property

Private Property Get ITelescope_SideOfPier() As PierSide
    ITelescope_SideOfPier = SideOfPier
End Property

Private Property Get ITelescope_SiderealTime() As Double
    ITelescope_SiderealTime = SiderealTime
End Property

Private Property Let ITelescope_SiteElevation(ByVal RHS As Double)
    SiteElevation = RHS
End Property

Private Property Get ITelescope_SiteElevation() As Double
    ITelescope_SiteElevation = SiteElevation
End Property

Private Property Let ITelescope_SiteLatitude(ByVal RHS As Double)
    SiteLatitude = RHS
End Property

Private Property Get ITelescope_SiteLatitude() As Double
    ITelescope_SiteLatitude = SiteLatitude
End Property

Private Property Let ITelescope_SiteLongitude(ByVal RHS As Double)
    SiteLongitude = RHS
End Property

Private Property Get ITelescope_SiteLongitude() As Double
    ITelescope_SiteLongitude = SiteLongitude
End Property

Private Property Get ITelescope_Slewing() As Boolean
    ITelescope_Slewing = Slewing
End Property

Private Property Let ITelescope_SlewSettleTime(ByVal RHS As Integer)
    SlewSettleTime = RHS
End Property

Private Property Get ITelescope_SlewSettleTime() As Integer
    ITelescope_SlewSettleTime = SlewSettleTime
End Property

Private Sub ITelescope_SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    SlewToAltAz Azimuth, Altitude
End Sub

Private Sub ITelescope_SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double)
    SlewToAltAzAsync Azimuth, Altitude
End Sub

Private Sub ITelescope_SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
    SlewToCoordinates RightAscension, Declination
End Sub

Private Sub ITelescope_SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double)
    SlewToCoordinatesAsync RightAscension, Declination
End Sub

Private Sub ITelescope_SlewToTarget()
    SlewToTarget
End Sub

Private Sub ITelescope_SlewToTargetAsync()
    SlewToTargetAsync
End Sub

Private Sub ITelescope_SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    SyncToAltAz Azimuth, Altitude
End Sub

Private Sub ITelescope_SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
    SyncToCoordinates RightAscension, Declination
End Sub

Private Sub ITelescope_SyncToTarget()
    SyncToTarget
End Sub

Private Property Let ITelescope_TargetDeclination(ByVal RHS As Double)
    TargetDeclination = RHS
End Property

Private Property Get ITelescope_TargetDeclination() As Double
    ITelescope_TargetDeclination = TargetDeclination
End Property

Private Property Let ITelescope_TargetRightAscension(ByVal RHS As Double)
    TargetRightAscension = RHS
End Property

Private Property Get ITelescope_TargetRightAscension() As Double
    ITelescope_TargetRightAscension = TargetRightAscension
End Property

Private Property Let ITelescope_Tracking(ByVal RHS As Boolean)
    Tracking = RHS
End Property

Private Property Get ITelescope_Tracking() As Boolean
    ITelescope_Tracking = Tracking
End Property

Private Property Let ITelescope_TrackingRate(ByVal RHS As DriveRates)
    TrackingRate = RHS
End Property

Private Property Get ITelescope_TrackingRate() As DriveRates
    ITelescope_TrackingRate = TrackingRate
End Property

Private Property Get ITelescope_TrackingRates() As ITrackingRates
    '
    ' Note that this more or less "casts" our internal TrackingRates
    ' object's interface to ITrackingRates.
    '
    Set ITelescope_TrackingRates = TrackingRates
End Property

Private Sub ITelescope_Unpark()
    Unpark
End Sub

Private Property Let ITelescope_UTCDate(ByVal RHS As Date)
    UTCDate = RHS
End Property

Private Property Get ITelescope_UTCDate() As Date
    ITelescope_UTCDate = UTCDate
End Property

