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
Attribute VB_Description = "ASCOM Telescope driver for POTH"
' -----------------------------------------------------------------------------'
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the POTH Telescope class
'
' Written: Jon Brewster
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Mar-03 jab     Initial edit
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 16-Oct-03 jab     Detuned to allow V2 to stabilize
' 10-Jan-06 dpp     Focuser implementation
' 10-Sep-06 jab     setting UTCDate now prints correct to traffic window
' 17-Feb-07 jab     change Name to "POTH->RealName"
' 03-Jun-07 jab     5.0.1 - Implement new standard ITelescope interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements ITelescope

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()
    
    If App.StartMode <> vbSModeStandalone Then
        DoStartupIf
        g_iConnections = g_iConnections + 1
    End If

    ScopeCreate (g_sScopeID)
    
End Sub

Private Sub Class_Terminate()
    
    ' if fired up to be a server, then see if this is the last user
    If App.StartMode <> vbSModeStandalone Then
    
        ' decrement user count
        If g_iConnections > 0 Then
            g_iConnections = g_iConnections - 1
        
            ' go away if no more users
            If g_iConnections <= 0 Then
                If g_iDomeConnections <= 0 And g_iFocuserConnections <= 0 Then
                    Unload g_handBox
                Else
                    ScopeSave
                    ScopeDelete
                End If
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get AlignmentMode() As AlignmentModes

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "AlignmentMode: "
    End If
    
    check_connected False, g_bTracking

    If g_eAlignMode = ALG_UNKNOWN Or Not g_bCanAlignMode Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property AlignmentMode" & MSG_NOT_IMPLEMENTED
    
    AlignmentMode = g_eAlignMode
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then
            out = "unknown"
            Select Case AlignmentMode
                Case algAltAz:       out = "Alt-Azimuth"
                Case algPolar:       out = "Equatorial"
                Case algGermanPolar: out = "German Equatorial"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Get Altitude() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Altitude: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSlewing Or g_lPulseGuideTix > 0 Then
        If (Not g_bQuiet) Or g_lPulseGuideTix > 0 Then
            If g_bCanAltAz Then
                g_dAltitude = g_Scope.Altitude
            Else
                g_dRightAscension = g_Scope.RightAscension
                g_dDeclination = g_Scope.Declination
                calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude
            End If
        End If
    Else
        If g_bQuiet Or Not g_bCanAltAz Then
            If g_bTracking Then
                calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude
            End If
        Else
            g_dAltitude = g_Scope.Altitude
        End If
    End If
    
    If g_dAltitude < -90 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Altitude" & MSG_NOT_IMPLEMENTED
            
    Altitude = g_dAltitude
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(Altitude, False)
    End If
    
End Property

Public Property Get ApertureArea() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "ApertureArea: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dApertureArea = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property ApertureArea" & MSG_NOT_IMPLEMENTED
                
    If g_dApertureArea <= 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property ApertureArea " & MSG_VAL_OUTOFRANGE
            
    ApertureArea = g_dApertureArea
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(ApertureArea)
    End If
            
End Property

Public Property Get ApertureDiameter() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "ApertureDiameter: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dAperture = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property ApertureDiameter" & MSG_NOT_IMPLEMENTED
                
    If g_dAperture <= 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property ApertureDiameter " & MSG_VAL_OUTOFRANGE
            
    ApertureDiameter = g_dAperture
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(ApertureDiameter)
    End If
        
End Property

Public Property Get AtHome() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficStart "AtHome: "
    End If
    
    check_connected False, g_bTracking
    
    ScopeAtHome g_bAtHome, False
    AtHome = g_bAtHome

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficEnd CStr(AtHome)
    End If
    
End Property

Public Property Get AtPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficStart "AtPark: "
    End If
    
    check_connected False, g_bTracking
    
    ScopeAtPark g_bAtPark, False
    AtPark = g_bAtPark

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficEnd CStr(AtPark)
    End If
    
End Property

Public Property Get Azimuth() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Azimuth: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSlewing Or g_lPulseGuideTix > 0 Then
        If (Not g_bQuiet) Or g_lPulseGuideTix > 0 Then
            If g_bCanAltAz Then
                g_dAzimuth = g_Scope.Azimuth
            Else
                g_dRightAscension = g_Scope.RightAscension
                g_dDeclination = g_Scope.Declination
                calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude
            End If
        End If
    Else
        If g_bQuiet Or Not g_bCanAltAz Then
            If g_bTracking Then _
                calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude
        Else
            g_dAzimuth = g_Scope.Azimuth
        End If
    End If
    
    If g_dAzimuth < -360 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Azimuth" & MSG_NOT_IMPLEMENTED
            
    Azimuth = g_dAzimuth
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(Azimuth, False)
    End If
    
End Property

Public Property Get CanFindHome() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanFindHome: " & g_bCanFindHome
    End If
    
    check_connected False, g_bTracking
    CanFindHome = g_bCanFindHome
    
End Property

Public Property Get CanPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanPark: " & g_bCanPark
    End If

    check_connected False, g_bTracking
    CanPark = g_bCanPark
    
End Property

Public Property Get CanPulseGuide() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanPulseGuide: " & g_bCanPulseGuide
    End If

    check_connected False, g_bTracking
    CanPulseGuide = g_bCanPulseGuide
    
End Property

Public Property Get CanSetDeclinationRate() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetDeclinationRate: " & g_bCanSetDeclinationRate
    End If
    
    check_connected False, g_bTracking
    CanSetDeclinationRate = g_bCanSetDeclinationRate

End Property

Public Property Get CanSetGuideRates() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetGuideRates: " & g_bCanSetGuideRates
    End If
    
    check_connected False, g_bTracking
    CanSetGuideRates = g_bCanSetGuideRates

End Property

Public Property Get CanSetPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetPark: " & g_bCanSetPark
    End If

    check_connected False, g_bTracking
    CanSetPark = g_bCanSetPark

End Property

Public Property Get CanSetPierSide() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetPierSide: " & g_bCanSetPierSide
    End If

    check_connected False, g_bTracking
    CanSetPierSide = g_bCanSetPierSide
    
End Property

Public Property Get CanSetRightAscensionRate() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetRightAscensionRate: " & g_bCanSetRightAscensionRate
    End If
    
    check_connected False, g_bTracking
    CanSetRightAscensionRate = g_bCanSetRightAscensionRate

End Property

Public Property Get CanSetTracking() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetTracking: " & g_bCanSetTracking
    End If

    check_connected False, g_bTracking
    CanSetTracking = g_bCanSetTracking
    
End Property

Public Property Get CanSlew() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlew: " & (g_bCanSlew Or g_bCanSlewAsync)
    End If
 
    check_connected False, g_bTracking
    CanSlew = g_bCanSlew Or g_bCanSlewAsync
        
End Property

Public Property Get CanSlewAltAz() As Boolean

    Dim Can As Boolean
    
    Can = g_bCanSlewAltAz Or g_bCanSlewAltAzAsync
        
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAltAz: " & Can
    End If
 
    check_connected False, g_bTracking
    CanSlewAltAz = Can
        
End Property

Public Property Get CanSlewAltAzAsync() As Boolean
  
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAltAzAsync: " & g_bCanSlewAltAzAsync
    End If
 
    check_connected False, g_bTracking
    CanSlewAltAzAsync = g_bCanSlewAltAzAsync
        
End Property

Public Property Get CanSlewAsync() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAsync: " & g_bCanSlewAsync
    End If

    check_connected False, g_bTracking
    CanSlewAsync = g_bCanSlewAsync
    
End Property

Public Property Get CanSync() As Boolean

    Dim Can As Boolean
    
    Can = g_bCanSync
        
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSync: " & Can
    End If

    check_connected False, g_bTracking
    CanSync = Can
    
End Property

Public Property Get CanSyncAltAz() As Boolean

    Dim Can As Boolean
    
    Can = g_bCanSync Or g_bCanSyncAltAz
        
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSyncAltAz: " & Can
    End If

    check_connected False, g_bTracking
    CanSyncAltAz = Can
    
End Property

Public Property Get CanUnpark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanUnpark: " & g_bCanUnpark
    End If

    check_connected False, g_bTracking
    CanUnpark = g_bCanUnpark
    
End Property

Public Property Get Connected() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Connected: " & g_bConnected
    End If
    
    Connected = g_bConnected
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    
    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Connected: " & g_bConnected & " -> " & newVal
    End If
    
    out = " (done)"
    
    If g_bConnected = newVal Then
        If Not g_show Is Nothing Then
            If g_show.chkOther.Value = 1 Then _
                g_show.TrafficEnd " (no change)"
        End If
        Exit Property
    End If
    
    ' only with a state change does anything happen
    If g_bConnected <> newVal Then
        If newVal Then
            g_setupDlg.ConnectScope False
            g_setupDlg.UpdateGlobals
            If g_bConnected Then
                out = " (connected)"
            Else
                out = " (error)"
            End If
        Else
            If App.StartMode = vbSModeStandalone Then
                out = " (ignored, not auto launch)"
            Else
                If g_iConnections <= 1 Then
                    g_setupDlg.ConnectScope False
                    g_setupDlg.UpdateGlobals
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

Public Property Get Declination() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Declination: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSlewing Or g_lPulseGuideTix > 0 Then
        If (Not g_bQuiet) Or g_lPulseGuideTix > 0 Then
            If g_bCanEqu Then
                g_dDeclination = g_Scope.Declination
            Else
                g_dAzimuth = g_Scope.Azimuth
                g_dAltitude = g_Scope.Altitude
                calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination
            End If
        End If
    Else
        If g_bQuiet Or Not g_bCanEqu Then
            If Not g_bTracking Then _
                calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination
        Else
            g_dDeclination = g_Scope.Declination
        End If
    End If
    
    If g_dDeclination < -90 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Declination" & MSG_NOT_IMPLEMENTED
    
    Declination = g_dDeclination
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(Declination, True)
    End If
    
End Property

Public Property Get DeclinationRate() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DeclinationRate: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then
        DeclinationRate = 0#
    Else
        DeclinationRate = g_Scope.DeclinationRate
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(DeclinationRate)
    End If
    
End Property

Public Property Let DeclinationRate(ByVal newVal As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DeclinationRate: " & " -> " & newVal
    End If

    check_connected False, g_bTracking
    
    If Not g_bCanSetDeclinationRate Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
            
    g_Scope.DeclinationRate = newVal
         
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get Description() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Description: "
    End If
    
    check_connected False, g_bTracking
    Description = g_Scope.Description
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get DoesRefraction() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "DoesRefraction: "
    End If
    
    check_connected False, g_bTracking
    
    If g_eDoesRefraction = refUnknown Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DoesRefraction" & MSG_NOT_IMPLEMENTED
             
    DoesRefraction = IIf(g_eDoesRefraction = refYes, True, False)
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd IIf(DoesRefraction, "yes", "no")
    End If
        
End Property

Public Property Let DoesRefraction(ByVal newVal As Boolean)

    check_connected False, g_bTracking
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "DoesRefraction: " & " -> " & newVal
    End If
    
    If g_bSimple Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DoesRefraction" & MSG_NOT_IMPLEMENTED
            
    g_Scope.DoesRefraction = newVal                         ' send command
    g_eDoesRefraction = IIf(newVal, refYes, refNo)          ' update global
    ' it happens that the enumeration matches the index
    g_setupDlg.cbRefraction.ListIndex = g_eDoesRefraction   ' update gui
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
        
End Property

Public Property Get DriverInfo() As String
    Dim tmpInfo

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DriverInfo: "
    End If
    
    ' start with POTH data
    DriverInfo = IIf(g_bSimple, "Simple", "") & App.EXEName & "->" & g_sScopeName & vbCrLf & _
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
    On Error GoTo 0
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get DriverVersion() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DriverVersion: "
    End If
    
    DriverVersion = App.Major & "." & App.Minor
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd DriverVersion
    End If
    
End Property

Public Property Get EquatorialSystem() As EquatorialCoordinateType

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "EquatorialSystem: "
    End If
    
    check_connected False, g_bTracking
        
    EquatorialSystem = g_eEquSystem
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then
            out = "unknown"
            Select Case EquatorialSystem
                Case equLocalTopocentric:   out = "LocalTopocentric"
                Case equB1950:              out = "B1950"
                Case equJ2000:              out = "J2000"
                Case equJ2050:              out = "J2050"
                Case equOther:              out = "Other"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Get FocalLength() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "FocalLength: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dFocalLength = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property FocalLength" & MSG_NOT_IMPLEMENTED
                
    If g_dFocalLength <= 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property FocalLength " & MSG_VAL_OUTOFRANGE
    
    FocalLength = g_dFocalLength
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(FocalLength)
    End If
    
End Property

Public Property Get GuideRateDeclination() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateDeclination: "
    End If

    check_connected False, g_bTracking

    If (Not g_bCanSetGuideRates) Or (g_iVersion < 2) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateDeclination" & MSG_NOT_IMPLEMENTED

    GuideRateDeclination = g_Scope.GuideRateDeclination

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(GuideRateDeclination)
    End If

End Property

Public Property Let GuideRateDeclination(ByVal newVal As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateDeclination: " & " -> " & newVal
    End If

    check_connected False, g_bTracking
    
    If (Not g_bCanSetGuideRates) Or (g_iVersion < 2) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateDeclination" & MSG_NOT_IMPLEMENTED
            
    g_Scope.GuideRateDeclination = newVal
         
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get GuideRateRightAscension() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateRightAscension: "
    End If

    check_connected False, g_bTracking

    If (Not g_bCanSetGuideRates) Or (g_iVersion < 2) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateRightAscension" & MSG_NOT_IMPLEMENTED

    GuideRateRightAscension = g_Scope.GuideRateRightAscension

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(GuideRateRightAscension)
    End If

End Property

Public Property Let GuideRateRightAscension(ByVal newVal As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateRightAscension: " & " -> " & newVal
    End If

    check_connected False, g_bTracking
    
    If (Not g_bCanSetGuideRates) Or (g_iVersion < 2) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateRightAscension" & MSG_NOT_IMPLEMENTED
            
    g_Scope.GuideRateRightAscension = newVal
         
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 2
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "InterfaceVersion: " & InterfaceVersion
    End If
    
End Property

Public Property Get IsPulseGuiding() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "IsPulseGuiding: "
    End If
    
    check_connected False, g_bTracking
    
    If Not g_bCanPulseGuide Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property IsPulseGuiding" & MSG_NOT_IMPLEMENTED
            
    IsPulseGuiding = g_Scope.IsPulseGuiding
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(IsPulseGuiding, "True ", "False ")
    End If
    
End Property

Public Property Get Name() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Name: "
    End If
  
    Name = IIf(g_bSimple, "Simple", "") & App.EXEName & "->" & g_sScopeName
        
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd Name
    End If
    
End Property

Public Property Get RightAscension() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "RightAscension: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSlewing Or g_lPulseGuideTix > 0 Then
        If (Not g_bQuiet) Or g_lPulseGuideTix > 0 Then
            If g_bCanEqu Then
                g_dRightAscension = g_Scope.RightAscension
            Else
                g_dAzimuth = g_Scope.Azimuth
                g_dAltitude = g_Scope.Altitude
                calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination
            End If
        End If
    Else
        If g_bQuiet Or Not g_bCanEqu Then
            If Not g_bTracking Then _
                calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination
        Else
            g_dRightAscension = g_Scope.RightAscension
        End If
    End If
    
    If g_dRightAscension < -24 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property RightAscension" & MSG_NOT_IMPLEMENTED
            
    RightAscension = g_dRightAscension
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(RightAscension, False)
    End If
            
End Property

Public Property Get RightAscensionRate() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "RightAscensionRate: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then
        RightAscensionRate = 0#
    Else
        RightAscensionRate = g_Scope.RightAscensionRate
    End If
      
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(RightAscensionRate)
    End If
    
End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "RightAscensionRate: " & " -> " & newVal
    End If
    
    check_connected False, g_bTracking
    
    If Not g_bCanSetRightAscensionRate Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED
            
    g_Scope.RightAscensionRate = newVal

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get SideOfPier() As PierSide
    
    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficStart "SideOfPier: "
    End If
    
    check_connected False, g_bTracking
    
    If g_eAlignMode <> algGermanPolar Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SideOfPier" & MSG_NOT_IMPLEMENTED
            
    ScopeSOP False
    
    If g_SOP = pierUnknown Then _
        Err.Raise SCODE_AMBIGUOUS_SOP, ERR_SOURCE, _
            "Property SideOfPier" & MSG_AMBIGUOUS_SOP
    
    SideOfPier = g_SOP
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then
            out = "unknown"
            Select Case SideOfPier
                Case pierUnknown:    out = "Unknown"
                Case pierEast:       out = "East"
                Case pierWest:       out = "West"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Let SideOfPier(ByVal newVal As PierSide)
    
    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case newVal
                Case pierUnknown:    out = "Unknown"
                Case pierEast:       out = "East"
                Case pierWest:       out = "West"
            End Select
            g_show.TrafficStart "SideOfPier: -> " & out
        End If
    End If
    
    check_connected True, g_bTracking
    
    ' see if its supported
    If (Not g_bCanSetPierSide) Or (g_eAlignMode <> algGermanPolar) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SideOfPier" & MSG_NOT_IMPLEMENTED
                
    ' check incomming parameter
    If newVal <> pierEast And newVal <> pierWest Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SideOfPier" & MSG_VAL_OUTOFRANGE
            
    If newVal <> g_SOP Then
        ScopeSetSOP newVal
        Exit Property
        ' slew code in Startup.bas journals end of slew
    End If
            
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get SiderealTime() As Double

    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficStart "SiderealTime: "
    End If
    
    check_connected False, g_bTracking
    
    SiderealTime = ScopeST()
    
    If SiderealTime < 0 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiderealTime" & MSG_NOT_IMPLEMENTED
    
    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(SiderealTime, False)
    End If
            
End Property

Public Property Get SiteElevation() As Double

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteElevation: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dElevation = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteElevation" & MSG_NOT_IMPLEMENTED
                
    If g_dElevation < -300 Or g_dElevation > 10000 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteElevation " & MSG_VAL_OUTOFRANGE

    SiteElevation = g_dElevation
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(SiteElevation)
    End If
            
End Property

Public Property Let SiteElevation(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteElevation: " & g_dElevation & " -> " & newVal
    End If
    
    check_connected False, g_bTracking
    
    If newVal < -300 Or newVal > 10000 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteElevation " & MSG_VAL_OUTOFRANGE
                
    If g_bCanElevation Then _
        g_Scope.SiteElevation = newVal
    
    g_dElevation = newVal
    g_setupDlg.Elevation = newVal
    ScopeCoords g_bQuiet, False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get SiteLatitude() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteLatitude: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dLatitude = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteLatitude" & MSG_NOT_IMPLEMENTED
                
    If g_dLatitude < -90 Or g_dLatitude > 90 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLatitude " & MSG_VAL_OUTOFRANGE

    SiteLatitude = g_dLatitude
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(SiteLatitude, True)
    End If
            
End Property

Public Property Let SiteLatitude(ByVal newVal As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteLatitude: " & _
                FmtSexa(g_dLatitude, True) & " -> " & FmtSexa(newVal, True)
    End If
    
    check_connected False, g_bTracking
    
    If newVal < -90 Or newVal > 90 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLatitude " & MSG_VAL_OUTOFRANGE
                
                
    If g_bCanLatLong Then _
        g_Scope.SiteLatitude = newVal
    
    g_dLatitude = newVal
    g_setupDlg.Latitude = newVal
    g_handBox.SouthernHemisphere = (g_dLatitude < 0)
    g_handBox.Quiet
    ScopeCoords g_bQuiet, False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
            
End Property

Public Property Get SiteLongitude() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteLongitude: "
    End If
    
    check_connected False, g_bTracking
    
    If g_dLongitude = EMPTY_PARAMETER Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteLongitude" & MSG_NOT_IMPLEMENTED
                
    If g_dLongitude < -180 Or g_dLongitude > 180 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLongitude " & MSG_VAL_OUTOFRANGE

    SiteLongitude = g_dLongitude

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(SiteLongitude, False)
    End If
            
End Property

Public Property Let SiteLongitude(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteLongitude: " & _
                FmtSexa(g_dLongitude, False) & " -> " & FmtSexa(newVal, False)
    End If
    
    check_connected False, g_bTracking
    
    If newVal < -180 Or newVal > 180 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLongitude " & MSG_VAL_OUTOFRANGE
                
    If g_bCanLatLong Then _
        g_Scope.SiteLongitude = newVal
        
    g_dLongitude = newVal
    g_setupDlg.Longitude = newVal
    g_handBox.Quiet
    ScopeCoords g_bQuiet, False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If

End Property

Public Property Get Slewing() As Boolean
        
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "Slewing: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bDomeConnected Then
        Slewing = g_bSlewing Or g_Dome.Slewing
    Else
        Slewing = g_bSlewing
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(Slewing, "True ", "False ")
    End If
        
End Property

Public Property Get SlewSettleTime() As Integer

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SlewSettleTime: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SlewSettleTime" & MSG_NOT_IMPLEMENTED
            
    SlewSettleTime = g_Scope.SlewSettleTime
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(SlewSettleTime)
    End If
    
End Property

Public Property Let SlewSettleTime(ByVal newVal As Integer)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SlewSettleTime: " & _
                g_lSlewSettleTime & " -> " & newVal
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SlewSettleTime" & MSG_NOT_IMPLEMENTED
            
    If newVal < 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SlewSettleTime " & MSG_VAL_OUTOFRANGE
    
    g_Scope.SlewSettleTime = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TargetDeclination() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "TargetDeclination: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then
        TargetDeclination = g_dTargetDec
    Else
        TargetDeclination = g_Scope.TargetDeclination
    End If
    
    If (TargetDeclination < -90) Or (TargetDeclination > 90) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetDeclination " & MSG_VAL_OUTOFRANGE
        
    g_dTargetDec = TargetDeclination
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(TargetDeclination, True)
    End If
    
End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "TargetDeclination: " & _
                " -> " & FmtSexa(newVal, True)
    End If
    
    check_connected False, g_bTracking
    
    If (newVal < -90) Or (newVal > 90) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetDeclination " & MSG_VAL_OUTOFRANGE
            
    If Not g_bSimple Then _
        g_Scope.TargetDeclination = newVal
        
    g_dTargetDec = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TargetRightAscension() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "TargetRightAscension: "
    End If
    
    check_connected False, g_bTracking
    
    If g_bSimple Then
        TargetRightAscension = g_dTargetRA
    Else
        TargetRightAscension = g_Scope.TargetRightAscension
    End If
    
    If (TargetRightAscension < 0) Or (TargetRightAscension > 24) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_VAL_OUTOFRANGE

    g_dTargetRA = TargetRightAscension
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(TargetRightAscension, False)
    End If
    
End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "TargetRightAscension: " & _
                " -> " & FmtSexa(newVal, False)
    End If
    
    check_connected False, g_bTracking
    
    If (newVal < 0) Or (newVal > 24) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_VAL_OUTOFRANGE
    
    If Not g_bSimple Then _
        g_Scope.TargetRightAscension = newVal
        
    g_dTargetRA = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get Tracking() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "Tracking: "
    End If
    
    check_connected False, g_bTracking
    
    ' only call out to the scope driver if not quiet
    If Not g_bQuiet Then _
        ScopeTracking trackRead
    
    Tracking = g_bTracking
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar CStr(g_bTracking) & " "
    End If
    
End Property

Public Property Let Tracking(ByVal newVal As Boolean)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Tracking: " & " -> " & newVal
    End If
    
    check_connected newVal, g_bTracking
    
    If Not g_bCanSetTracking Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Control of Tracking" & MSG_NOT_IMPLEMENTED

    If newVal And g_bAtPark Then _
        Err.Raise SCODE_SLEW_WHILE_PARKED, ERR_SOURCE, _
            "Property Tracking " & MSG_SLEW_WHILE_PARKED
            
    g_Scope.Tracking = newVal
    ScopeTracking trackRead
    ScopeAtPark g_bAtPark, False
    ScopeAtHome g_bAtHome, False
    ScopeCoords g_bQuiet, False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TrackingRate() As DriveRates

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "TrackingRate: "
    End If
    
    check_connected False, g_bTracking

    TrackingRate = driveSidereal
    
    ' just report Sidereal rather than error out
    On Error Resume Next
    If Not g_bSimple Then _
        TrackingRate = g_Scope.TrackingRate
    On Error GoTo 0

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then
            out = "unknown"
            Select Case TrackingRate
                Case driveSidereal:     out = "Sidereal"
                Case driveLunar:        out = "Lunar"
                Case driveSolar:        out = "Solar"
                Case driveKing:         out = "King"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Property

Public Property Let TrackingRate(ByVal newVal As DriveRates)

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then
            out = "unknown"
            Select Case newVal
                Case driveSidereal:     out = "Sidereal"
                Case driveLunar:        out = "Lunar"
                Case driveSolar:        out = "Solar"
                Case driveKing:         out = "King"
            End Select
            g_show.TrafficStart "TrackingRate: -> " & out
        End If
    End If
    
    check_connected False, g_bTracking
    
    If newVal = driveSidereal Then
        If g_iVersion >= 2 Then
            ' setting is optional for V2, but Sidereal is required, so pass it on
            ' but assume success
            On Error Resume Next
            g_Scope.TrackingRate = newVal
            On Error GoTo 0
        End If
    Else
        If g_iVersion >= 2 Then
            g_Scope.TrackingRate = newVal
        Else
            Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Property TrackingRate" & MSG_VAL_OUTOFRANGE
        End If
    End If
        
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TrackingRates() As Object

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "TrackingRates: "
    End If

    check_connected False, g_bTracking
    
    If g_iVersion < 2 Then
        Set TrackingRates = g_TrackingRatesSimple
    Else
        Set TrackingRates = g_Scope.TrackingRates
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get UTCDate() As Date

    Dim CurDate As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficStart "UTCDate: "
    End If
    
    check_connected False, g_bTracking
    
    If (Not g_bCanDateTime) Or g_bQuiet Then
        CurDate = CDbl(Now()) + (CDbl(utc_offs()) / 86400#)
        UTCDate = CDate(CurDate)
    Else
        UTCDate = g_Scope.UTCDate
    End If

    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficEnd CStr(UTCDate)
    End If
            
End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Dim CurDate As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficStart "UTCDate: " & " -> " & CStr(newVal)
    End If
    
    check_connected False, g_bTracking

    ' simulate setting date/time if difference is less than 2 seconds
    If Not g_bCanDateTime Then
        CurDate = (CDbl(Now()) * 86400#) + CDbl(utc_offs())
        If Abs((CDbl(newVal) * 86400#) - CurDate) > 2 Then _
            Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED
    Else
        g_Scope.UTCDate = newVal
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
            
End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "AbortSlew:"
    End If
    
    check_connected True, g_bTracking
    ScopeAbortSlew
    ScopeCoords g_bQuiet, True
     
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Function AxisRates(ByVal Axis As TelescopeAxes) As Object

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_show.TrafficStart "AxisRates " & out & ":"
        End If
    End If

    check_connected False, g_bTracking

    If g_iVersion < 2 Then
        Set AxisRates = g_AxisRatesEmpty
    Else
        Set AxisRates = g_Scope.AxisRates(Axis)
    End If

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If

End Function

Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    
    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_show.TrafficStart "CanMoveAxis " & out & ": "
        End If
    End If

    check_connected False, g_bTracking
    
    If g_iVersion < 2 Then
        CanMoveAxis = False
    Else
        CanMoveAxis = g_Scope.CanMoveAxis(Axis)
    End If

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd CStr(CanMoveAxis)
    End If

End Function

Public Sub CommandBlind(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False)

    ' Check for POTH specific commands
    If InStr(1, Command, "POTH Command:", vbTextCompare) = 1 Then
    
        If InStr(1, Command, "MeridianDelayWest", vbTextCompare) = 15 Then
            g_dMeridianDelay = CDbl(Mid(Command, 33))
            g_setupDlg.txtMeridianDelay.Text = Str(g_dMeridianDelay)
            
        ElseIf InStr(1, Command, "MeridianDelayEast", vbTextCompare) = 15 Then
            g_dMeridianDelayEast = CDbl(Mid(Command, 33))
            g_setupDlg.txtMeridianDelayEast.Text = Str(g_dMeridianDelayEast)
            
        ElseIf InStr(1, Command, "Quiet", vbTextCompare) = 15 Then
            g_bQuiet = CBool(Mid(Command, 21))
            g_handBox.CheckEnable
            
        ElseIf Not g_show Is Nothing Then
            g_show.TrafficLine vbCrLf & "Unknown POTH Command: " & _
                Command & vbCrLf
        End If
        
        Exit Sub
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "CommandBlind " & Command & " " & Raw & ": "
    End If
    
    check_connected False, g_bTracking
    
    ' Only support Raw for V2 and above
    If g_iVersion < 2 And Raw Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method CommandBlind" & MSG_VAL_OUTOFRANGE
            
    g_Scope.CommandBlind Command, Raw

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Function CommandBool(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As Boolean

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
            g_show.TrafficStart "CommandBool " & Command & " " & Raw & ": "
    End If
    
    check_connected False, g_bTracking
    
    ' Only support Raw for V2 and above
    If g_iVersion < 2 And Raw Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method CommandBool" & MSG_VAL_OUTOFRANGE
            
    CommandBool = g_Scope.CommandBool(Command, Raw)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(CommandBool)
    End If
    
End Function

'' hack for peter ??? (pre V2)
'Public Function CommandNative(ByVal Command As String) As String
'
'    If Not g_show Is Nothing Then
'        If g_show.chkOther.Value = 1 Then _
'            g_show.TrafficStart "CommandNative: " & Command
'    End If
'
'    check_connected False, g_bTracking
'    CommandNative = g_Scope.CommandNative(Command)
'
'    If Not g_show Is Nothing Then
'        If g_show.chkOther.Value = 1 Then _
'            g_show.TrafficEnd "(" & CommandNative & ")"
'    End If
'
'End Function

Public Function CommandString(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As String

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
            g_show.TrafficStart "CommandString " & Command & " " & Raw & ":"
    End If
    
    check_connected False, g_bTracking
    
    ' Only support Raw for V2 and above
    If g_iVersion < 2 And Raw Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method CommandString" & MSG_VAL_OUTOFRANGE
            
    CommandString = g_Scope.CommandString(Command, Raw)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CommandString
    End If
    
End Function

Public Function DestinationSideOfPier(ByVal RightAscension As Double, _
        ByVal Declination As Double) As PierSide

    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DestinationSideOfPier" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ": "
    End If
    
    check_connected False, g_bTracking
    
    ' see if its supported
    If g_eAlignMode <> algGermanPolar Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Function DestinationSideOfPier" & MSG_NOT_IMPLEMENTED
            
    DestinationSideOfPier = ScopeDSOP(RightAscension, Declination)
    
    If DestinationSideOfPier = pierUnknown Then _
        Err.Raise SCODE_AMBIGUOUS_SOP, ERR_SOURCE, _
            "Function DestinationSideOfPier" & MSG_AMBIGUOUS_SOP
            
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then
            out = "unknown"
            Select Case DestinationSideOfPier
                Case pierUnknown:  out = "Unknown"
                Case pierEast:     out = "East"
                Case pierWest:     out = "West"
            End Select
            g_show.TrafficEnd out
        End If
    End If
    
End Function

Public Sub FindHome()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "FindHome:"
    End If
    
    check_connected True, g_bTracking
    
    If Not g_bCanFindHome Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method FindHome()" & MSG_NOT_IMPLEMENTED
       
    ScopeHome

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    
    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case Axis
                Case axisPrimary:       out = "Primary"
                Case axisSecondary:     out = "Secondary"
                Case axisTertiary:      out = "Tertiary"
            End Select
            g_show.TrafficStart "MoveAxis " & out & ", " & Rate & ":"
        End If
    End If

    check_connected True, g_bTracking

    If g_iVersion < 2 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method MoveAxis" & MSG_NOT_IMPLEMENTED
            
    g_Scope.MoveAxis Axis, Rate

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If

End Sub

Public Sub Park()
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Park:"
    End If
    
    check_connected False, g_bTracking
        
    If Not g_bCanPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Park()" & MSG_NOT_IMPLEMENTED
    
    ScopePark
  
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)

    Dim out As String
    Dim tmpTix As Long
 
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then
            out = "unknown"
            Select Case Direction
                Case guideNorth:    out = "North"
                Case guideSouth:    out = "South"
                Case guideEast:     out = "East"
                Case guideWest:     out = "West"
            End Select
            g_show.TrafficStart "PulseGuide: " & out & ", " & Duration & ":"
        End If
    End If
    
    check_connected True, g_bTracking

    If Not g_bCanPulseGuide Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method PulseGuide()" & MSG_NOT_IMPLEMENTED
        
    ' set the tix count for the end of the guide
    ' but check that other axis not moving longer duration
    tmpTix = GetTickCount() + Duration
    If tmpTix > g_lPulseGuideTix Then _
        g_lPulseGuideTix = tmpTix
    
    g_Scope.PulseGuide Direction, Duration
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetPark()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetPark:"
    End If
    
    check_connected False, g_bTracking
    
    If Not g_bCanSetPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SetPark()" & MSG_NOT_IMPLEMENTED

    g_Scope.SetPark
    ScopeTracking trackRead
    ScopeAtHome False, False
    ScopeAtPark True, False
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetupDialog:"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SlewToAltAz(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    
    Dim RA As Double, Dec As Double

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToAltAz:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
        
    check_connected True, False
    
    If g_bSimple Or _
            ((Not g_bCanSlew) And (Not g_bCanSlewAsync) And _
            (Not g_bCanSlewAltAz) And (Not g_bCanSlewAltAzAsync)) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltAz()" & MSG_NOT_IMPLEMENTED
    
    ' RA and Dec will be needed for dome at least...
    calc_radec Azimuth, Altitude, RA, Dec
        
    ' try to do async so that halt will work
    If g_bCanSlewAltAzAsync Then
    
        ScopePreSlew RA, Dec, True, True, True
        g_Scope.SlewToAltAzAsync Azimuth, Altitude
        ScopePostSlew True, True
        
        While g_bSlewing
            DoEvents
        Wend
        ' slew code in Startup.bas journals end of slew
        
    Else
        
        If g_bCanSlewAltAz Then
                            
            ScopePreSlew RA, Dec, True, True, False
            g_Scope.SlewToAltAz Azimuth, Altitude
            ScopePostSlew True, False
            
            If Not g_show Is Nothing Then
                If g_show.chkSlew.Value = 1 Then _
                    g_show.TrafficLine " (slew complete)"
            End If
            
        Else
            ' only fall back to RA/Dec based slews if required
            ' but if tracking is off, these may fail,
            ' even though we set the CAN flag ??? same for other AltAz stuff
            
            If g_bCanSlewAsync Then
            
                ScopePreSlew RA, Dec, True, True, True
                g_Scope.SlewToCoordinatesAsync RA, Dec
                ScopePostSlew True, True
                
                While g_bSlewing
                    DoEvents
                Wend
                ' slew code in Startup.bas journals end of slew
                
            Else
                ScopePreSlew RA, Dec, True, True, False
                g_Scope.SlewToCoordinates RA, Dec
                ScopePostSlew True, False
            End If
        
        End If
    End If
    
End Sub

Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    Dim RA As Double, Dec As Double

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToAltAzAsync:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
    
    check_connected True, False
    
    If g_bSimple Or _
            ((Not g_bCanSlewAsync) And (Not g_bCanSlewAltAzAsync)) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltAzAsync()" & MSG_NOT_IMPLEMENTED
    
    ' RA and Dec will be needed for dome slaving at least...
    calc_radec Azimuth, Altitude, RA, Dec
    
    ScopePreSlew RA, Dec, True, True, True
    
    If g_bCanSlewAltAzAsync Then
        g_Scope.SlewToAltAzAsync Azimuth, Altitude
    Else
        g_Scope.SlewToCoordinatesAsync RA, Dec
    End If
    
    ScopePostSlew True, True
    
    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
 
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToCoordinates:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
        
    check_connected True, True
    
    If (Not g_bCanSlew) And (Not g_bCanSlewAsync) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToCoordinates()" & MSG_NOT_IMPLEMENTED
                
    ' try to do async so that halt will work
    If g_bCanSlewAsync Then
    
        ScopePreSlew RightAscension, Declination, True, True, True
        g_Scope.SlewToCoordinatesAsync RightAscension, Declination
        ScopePostSlew True, True
        
        While g_bSlewing
            DoEvents
        Wend
        ' slew code in Startup.bas journals end of slew
    Else
        
        ' ??? catch error, clear slew, pass error up
        ' OR count slew checks...
                
        ScopePreSlew RightAscension, Declination, True, True, False
        g_Scope.SlewToCoordinates RightAscension, Declination
        ScopePostSlew True, False
        
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Value = 1 Then _
                g_show.TrafficLine " (slew complete)"
        End If
        
    End If
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToCoordinatesAsync:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
    
    check_connected True, True
    
    If Not g_bCanSlewAsync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToCoordinatesAsync()" & MSG_NOT_IMPLEMENTED
    
    ScopePreSlew RightAscension, Declination, True, True, True
    g_Scope.SlewToCoordinatesAsync RightAscension, Declination
    ScopePostSlew True, True
    
    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToTarget()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "SlewToTarget:"
    End If
    
    check_connected True, True
    
    If (Not g_bCanSlew) And (Not g_bCanSlewAsync) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToTarget()" & MSG_NOT_IMPLEMENTED
                
    ' try to do async so that halt will work
    If g_bCanSlewAsync Then
    
        ScopePreSlew g_dTargetRA, g_dTargetDec, False, True, True
        g_Scope.SlewToTargetAsync
        ScopePostSlew True, True
        
        While g_bSlewing
            DoEvents
        Wend
        ' slew code in Startup.bas journals end of slew
    Else
        
        ScopePreSlew g_dTargetRA, g_dTargetDec, False, True, False
        g_Scope.SlewToTarget
        ScopePostSlew True, False
        
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Value = 1 Then _
                g_show.TrafficLine " (slew complete)"
        End If
    End If

End Sub

Public Sub SlewToTargetAsync()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "SlewToTargetAsync:"
    End If
    
    check_connected True, True
    
    If Not g_bCanSlewAsync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToTargetAsync()" & MSG_NOT_IMPLEMENTED
      
    ' ??? simulate if no async and coord version

    ScopePreSlew g_dTargetRA, g_dTargetDec, False, True, True
    g_Scope.SlewToTargetAsync
    ScopePostSlew True, True

    ' slew code in Startup.bas journals end of slew
End Sub

Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
     
    Dim RA As Double, Dec As Double

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SyncToAltAz:" & vbCrLf & _
            " Az " & FmtSexa(Azimuth, False) & vbCrLf & _
            " Alt " & FmtSexa(Altitude, False) & ":"
    End If
    
    check_connected True, False
    
    If (Not g_bCanSync) And (Not g_bCanSyncAltAz) Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToAltAz()" & MSG_NOT_IMPLEMENTED
    
    ' RA and Dec will be needed for dome at least...
    calc_radec Azimuth, Altitude, RA, Dec
    
    ScopePreSlew RA, Dec, True, False, False
    
    If g_bCanSyncAltAz Then
        g_Scope.SyncToAltAz Azimuth, Altitude
    Else
        g_Scope.SyncToCoordinates RA, Dec
    End If
    
    ScopePostSlew False, False
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
     
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SyncToCoordinates:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ":"
    End If
    
    check_connected True, True
    
    If Not g_bCanSync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToCoordinates()" & MSG_NOT_IMPLEMENTED
    
    ScopePreSlew RightAscension, Declination, True, False, False
    g_Scope.SyncToCoordinates RightAscension, Declination
    ScopePostSlew False, False
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SyncToTarget()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "SyncToTarget:"
    End If
    
    check_connected True, True
    
    If Not g_bCanSync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToTarget()" & MSG_NOT_IMPLEMENTED
                    
    ScopePreSlew g_dTargetRA, g_dTargetDec, False, False, False
    g_Scope.SyncToTarget
    ScopePostSlew False, False
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub Unpark()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "UnPark:"
    End If
    
    check_connected False, g_bTracking
    
    If Not g_bCanUnpark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Unpark()" & MSG_NOT_IMPLEMENTED
    
    ScopeUnpark
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

'-------------------
' Private Utilities
'-------------------

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the scope is not connected
'
'---------------------------------------------------------------------

Private Sub check_connected(Park As Boolean, Track As Boolean)

    If (Not g_bConnected) Or (g_Scope Is Nothing) Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    
    ' Global error checking for all slews and syncs
    If Park And g_bAtPark Then _
        Err.Raise SCODE_SLEW_WHILE_PARKED, ERR_SOURCE, _
            MSG_SLEW_WHILE_PARKED
    
    ' tracking checks was only refined for V2, only error in those cases
    If (g_iVersion >= 2) And (Track <> g_bTracking) Then _
        Err.Raise SCODE_WRONG_TRACKING, ERR_SOURCE, _
            MSG_WRONG_TRACKING
       
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
