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
Attribute VB_Description = "ASCOM Telescope driver for Telescope Simulator"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the ASCOM telescope simulator Telescope class
'
' Written:  28-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 28-Jun-00 rbd     Initial edit
' 28-Jun-00 rbd     Remove serial I/O control, this simulator does not
'                   need serial port. Add support for Timer control,
'                   used to pace slewing.
' 30-Jun-00 rbd     Much more work...
' 24-Jul-00 rbd     More. Add SyncToCoordinates()
' 14-Oct-00 rbd     SlewSettleTime
' 16-Oct-00 rbd     Implement Slewing property
' 13-Nov-00 rbd     Name and Description return String not Double.
'                   Park() does not take a parameter. ByVal on
'                   all parameters & Lets
' 21-Nov-00 rbd     1.0.3 DriverInfo() corrections and additions.
' 25-Jan-01 rbd     New properties CanSlew, CanSlewAsync, CanSync,
'                   CanPark, CanFindHome, CanSetPark, CanUnpark
'                   Interface V1.1
' 28-Jan-00 rbd     Add description and version info display to
'                   SetupDialog box. Add a bunch of capability
'                   switches, so can test clients for varying
'                   levels of capability. Explicit "not implemented"
'                   messages like the other drivers. Fix missing
'                   source arg in errors.
' 06-Feb-01 rbd     Add Tracking, CanSetTracking
' 13-Mar-01 rbd     Add checkbox capability control for CanSetTracking
' 08-Jun-01 rbd     Telescope 1.4: Add PulseGuide() & CanPulseGuide
' 10-Aug-01 rbd     1.4.2: Make changes to Lat/Long/Elevation persistent
' 05-Oct-01 rbd     1.4.3: Add ByVals as needed
' 06-Mar-03 jab     Add date handling
' 08-Mar-03 jab     Add user setable aperture and focal length
' 15-Mar-03 jab     kill server if launched via client and last client bails,
'                   also only allow disconnect if last clients commands it,
'                   no disconnects allowed if launched manually
' 16-Jun-03 jab     ASCOM Journaling, besides actual journaling calls we
'                   had to unwind the nested slew calls so that they called
'                   a common routine
' 24-Jun-03 rbd     2.3.2 - Left version for Platform 2.3
' 03-Sep-03 rbd     3.0.1 - For Platform 3.0
' 05-Mar-04 jab     added V2, and finished missing V1 features
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 27-Jul-05 rbd     4.0.4 - AbortSlew raises error if parked (Enzerink)
' 12-Apr-07 rbd     5.0.1 - Remove old reentrancy test code from
'                   Declination, new startup refactoring.
' 21-May-07 rbd     5.0.2 - Implement new standard ITelescope interface
' 01-Jun-07 rbd     5.0.3 - OOps, many traffic loggers were broken!
' 02-Jun-07 rbd     5.0.4 - Oops again, many functions were broken! I
'                   really screwed up in my conversion technique!
' 03-Jun-07 jab     5.0.6 converted back to public as was, and ITelescope
'                   calls the old public code (fixed many issues)
' 03-Jul-07 jab     5.0.9 invalidate target coordinates on disconnect
'---------------------------------------------------------------------

Option Explicit
Implements ITelescope

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

Public Property Get AlignmentMode() As AlignmentModes

    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "AlignmentMode: "
    End If
    
    If Not g_bCanAlignMode Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property AlignmentMode" & MSG_NOT_IMPLEMENTED

    AlignmentMode = g_eAlignMode

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then
            out = "unknown"
            Select Case AlignmentMode
                Case algAltAz:        out = "Alt-Azimuth"
                Case algPolar:        out = "Equatorial"
                Case algGermanPolar:  out = "German Equatorial"
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
    
    If Not g_bCanAltAz Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Altitude" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If (g_bAtPark Or g_Slewing = slewPark) And g_bNoCoordAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            "Property Altitude " & MSG_ILLEGAL_WHILE_PARKED
            
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
    
    If g_bV1 Or Not g_bCanOptics Then _
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
    
    If Not g_bCanOptics Then _
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
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property AtHome" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
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
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property AtPark" & MSG_NOT_IMPLEMENTED
            
    check_connected False, g_bTracking
    
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
    
    If Not g_bCanAltAz Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Azimuth" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If (g_bAtPark Or g_Slewing = slewPark) And g_bNoCoordAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            "Property Azimuth " & MSG_ILLEGAL_WHILE_PARKED
            
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
    
    CanFindHome = g_bCanFindHome
    
End Property

Public Property Get CanPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanPark: " & g_bCanPark
    End If
    
    CanPark = g_bCanPark
    
End Property

Public Property Get CanPulseGuide() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanPulseGuide: " & g_bCanPulseGuide
    End If
    
    CanPulseGuide = g_bCanPulseGuide
    
End Property

Public Property Get CanSetDeclinationRate() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetDeclinationRate: " & g_bCanSetEquRates
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSetDeclinationRate" & MSG_NOT_IMPLEMENTED
           
    CanSetDeclinationRate = g_bCanSetEquRates

End Property

Public Property Get CanSetGuideRates() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetGuideRates: " & g_bCanSetGuideRates
    End If

    CanSetGuideRates = g_bCanSetGuideRates

End Property

Public Property Get CanSetPark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetPark: " & g_bCanSetPark
    End If
    
    CanSetPark = g_bCanSetPark

End Property

Public Property Get CanSetRightAscensionRate() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetRightAscensionRate: " & g_bCanSetEquRates
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSetRightAscensionRate" & MSG_NOT_IMPLEMENTED
            
    CanSetRightAscensionRate = g_bCanSetEquRates

End Property

Public Property Get CanSetPierSide() As Boolean

    Dim CanDo As Boolean
    
    CanDo = IIf(g_eAlignMode = algGermanPolar, g_bCanSetSOP, False)

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetPierSide: " & CanDo
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSetPierSide" & MSG_NOT_IMPLEMENTED
            
    CanSetPierSide = CanDo

End Property

Public Property Get CanSetTracking() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSetTracking: " & g_bCanSetTracking
    End If
    
    CanSetTracking = g_bCanSetTracking

End Property

Public Property Get CanSlew() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlew: " & g_bCanSlew
    End If
    
    CanSlew = g_bCanSlew
    
End Property

Public Property Get CanSlewAltAz() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAltAz: " & g_bCanSlewAltAz
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSlewAltAz" & MSG_NOT_IMPLEMENTED
            
    CanSlewAltAz = g_bCanSlewAltAz
    
End Property

Public Property Get CanSlewAltAzAsync() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAltAzAsync: " & g_bCanSlewAltAzAsync
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSlewAltAzAsync" & MSG_NOT_IMPLEMENTED
            
    CanSlewAltAzAsync = g_bCanSlewAltAzAsync

End Property

Public Property Get CanSlewAsync() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSlewAsync: " & g_bCanSlewAsync
    End If
    
    CanSlewAsync = g_bCanSlewAsync

End Property

Public Property Get CanSync() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSync: " & g_bCanSync
    End If
    
    CanSync = g_bCanSync
    
End Property

Public Property Get CanSyncAltAz() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanSyncAltAz: " & g_bCanSyncAltAz
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property CanSyncAltAz" & MSG_NOT_IMPLEMENTED
            
    CanSyncAltAz = g_bCanSyncAltAz
    
End Property

Public Property Get CanUnpark() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "CanUnpark: " & g_bCanUnpark
    End If
    
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
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Connected: " & g_bConnected & " -> " & newVal
    End If
    
    If g_bConnected = newVal Then
        If Not g_show Is Nothing Then
            If g_show.chkOther.Value = 1 Then _
                g_show.TrafficEnd " (no change)"
        End If
        Exit Property
    End If
    
    If newVal Then
        g_bConnected = True
        If Not g_show Is Nothing Then
            If g_show.chkOther.Value = 1 Then _
                g_show.TrafficEnd " (connected)"
        End If
    Else
        If App.StartMode = vbSModeStandalone Then      ' ignore clear if run as executable
            If Not g_show Is Nothing Then
                If g_show.chkOther.Value = 1 Then _
                    g_show.TrafficEnd " (nope, manual mode)"
            End If
        Else
            If g_iConnections < 2 Then
                g_bConnected = False      ' allow clear if last client
                
                ' Invalidate target coordinates
                g_dTargetRightAscension = INVALID_COORDINATE
                g_dTargetDeclination = INVALID_COORDINATE
                
                If Not g_show Is Nothing Then
                    If g_show.chkOther.Value = 1 Then _
                        g_show.TrafficEnd " (disconnected)"
                End If
            Else
                If Not g_show Is Nothing Then
                    If g_show.chkOther.Value = 1 Then _
                        g_show.TrafficEnd " (nope, not last)"
                End If
            End If
        End If
    End If
    
End Property

Public Property Get Declination() As Double

    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficStart "Declination: "
    End If
    
    If Not g_bCanEqu Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Declination" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If (g_bAtPark Or g_Slewing = slewPark) And g_bNoCoordAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            "Property Declination " & MSG_ILLEGAL_WHILE_PARKED
            
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
    
'    If Not g_bCanSetEquRates Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Property DeclinationRate" & MSG_NOT_IMPLEMENTED

    DeclinationRate = g_dDeclinationRate
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Let DeclinationRate(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DeclinationRate: " & _
                g_dDeclinationRate & " -> " & newVal
    End If
    
    If Not g_bCanSetEquRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
    
    ' implements a real coordinate change per time
    g_dDeclinationRate = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
        
End Property

Public Property Get Description() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Description:"
    End If
    
    Description = INSTRUMENT_DESCRIPTION
    
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
    
    If g_bV1 Or Not g_bCanDoesRefraction Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DoesRefraction" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
          
    DoesRefraction = g_bDoRefraction
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd CStr(g_bDoRefraction)
    End If
        
End Property

Public Property Let DoesRefraction(ByVal newVal As Boolean)

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficStart "DoesRefraction: " & g_bDoRefraction & " -> " & newVal
    End If
    
    If g_bV1 Or Not g_bCanDoesRefraction Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DoesRefraction" & MSG_NOT_IMPLEMENTED
            
    check_connected False, g_bTracking
    
    g_bDoRefraction = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
        
End Property

Public Property Get DriverInfo() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DriverInfo:"
    End If
    
    '
    ' Use the Project/Properties sheet, Make tab, to set these
    ' items. That way they will show in the Version tab of the
    ' Explorer property sheet, and the exact same data will
    ' show in Telescope.DriverInfo.
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

Public Property Get DriverVersion() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "DriverVersion: "
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DriverVersion" & MSG_NOT_IMPLEMENTED
            
    DriverVersion = App.Major & "." & App.Minor
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd DriverVersion
    End If
    
End Property

Public Property Get EquatorialSystem() As EquatorialCoordinateType

    Dim out As String
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "EquatorialSystem: "
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property EquatorialSystem" & MSG_NOT_IMPLEMENTED
            
    EquatorialSystem = g_eEquSystem
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then
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
    
    If Not g_bCanOptics Then _
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

    If g_bV1 Or Not g_bCanSetGuideRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateDeclination" & MSG_NOT_IMPLEMENTED

    check_connected False, g_bTracking
    
    GuideRateDeclination = Abs(g_dGuideRateDeclination)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(GuideRateDeclination)
    End If

End Property

Public Property Let GuideRateDeclination(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateDeclination: " & _
                g_dGuideRateDeclination & " -> " & newVal
    End If
    
    If g_bV1 Or Not g_bCanSetGuideRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateDeclination" & MSG_NOT_IMPLEMENTED
    
    If newVal < 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property GuideRateDeclination " & MSG_VAL_OUTOFRANGE
            
    g_dGuideRateDeclination = newVal
    
    ' Since this is guide rate adjustment, no coordinate changes will be done
    ' It is assumed that guiding calls are to maintain position
    
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

    If g_bV1 Or Not g_bCanSetGuideRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateRightAscension" & MSG_NOT_IMPLEMENTED

    check_connected False, g_bTracking

    GuideRateRightAscension = Abs(g_dGuideRateRightAscension)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(GuideRateRightAscension)
    End If

End Property

Public Property Let GuideRateRightAscension(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "GuideRateRightAscension: " & _
                g_dGuideRateRightAscension & " -> " & newVal
    End If
    
    If g_bV1 Or Not g_bCanSetGuideRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property GuideRateRightAscension" & MSG_NOT_IMPLEMENTED
    
    If newVal < 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property GuideRateRightAscension " & MSG_VAL_OUTOFRANGE
            
    g_dGuideRateRightAscension = newVal
    
    ' Since this is guide rate adjustment, no coordinate changes will be done
    ' It is assumed that guiding calls are to maintain position
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get InterfaceVersion() As Integer

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "InterfaceVersion: "
    End If
    
    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property InterfaceVersion" & MSG_NOT_IMPLEMENTED
            
    InterfaceVersion = 2
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(InterfaceVersion)
    End If
       
End Property

Public Property Get IsPulseGuiding() As Boolean

    Dim tmpTix As Long

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "IsPulseGuiding:"
    End If
      
    If Not g_bCanPulseGuide Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property IsPulseGuiding" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    ' pulse guiding is happening if Tix not cleared
    IsPulseGuiding = (g_lPulseGuideTixRA > 0) Or _
        (g_lPulseGuideTixDec > 0)
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(IsPulseGuiding, "True", "False")
    End If
    
End Property

Public Property Get Name() As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Name: "
    End If
    
    Name = INSTRUMENT_NAME
    
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
    
    If Not g_bCanEqu Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property RightAscension" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If (g_bAtPark Or g_Slewing = slewPark) And g_bNoCoordAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            "Property RightAscension " & MSG_ILLEGAL_WHILE_PARKED
            
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
    
'    If Not g_bCanSetEquRates Then _
'        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'            "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED

    RightAscensionRate = g_dRightAscensionRate
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "RightAscensionRate: " & _
                g_dRightAscensionRate & " -> " & newVal
    End If
    
    If Not g_bCanSetEquRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED

    ' implements a real coordinate change per time
    g_dRightAscensionRate = newVal
    
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
    
    If g_bV1 Or g_eAlignMode <> algGermanPolar Or Not g_bCanSOP Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SideOfPier" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
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
    
    ' see if its supported
    If g_bV1 Or g_eAlignMode <> algGermanPolar Or Not g_bCanSetSOP Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SideOfPier" & MSG_NOT_IMPLEMENTED
            
    check_connected True, g_bTracking
    
    ' check incomming parameter
    If newVal <> pierEast And newVal <> pierWest Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SideOfPier" & MSG_VAL_OUTOFRANGE
    
    ' whew, now see if we even are being asked to flip
    If g_SOP <> newVal Then
        doFlip newVal
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
    
    If Not g_bCanSiderealTime Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiderealTime" & MSG_NOT_IMPLEMENTED
    
    SiderealTime = now_lst(g_dLongitude * DEG_RAD)       ' Use astro32.dll for this
    
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
    
    If Not g_bCanLatLongElev Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteElevation" & MSG_NOT_IMPLEMENTED
    
    If g_dSiteElevation < -300 Or g_dSiteElevation > 10000 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Property SiteElevation " & MSG_VAL_OUTOFRANGE
                
    SiteElevation = g_dSiteElevation
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(SiteElevation)
    End If
    
End Property

Public Property Let SiteElevation(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SiteElevation: " & g_dSiteElevation & " -> " & newVal
    End If
    
    If Not g_bCanLatLongElev Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteElevation" & MSG_NOT_IMPLEMENTED
    
    If newVal < -300 Or newVal > 10000 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteElevation " & MSG_VAL_OUTOFRANGE
            
    g_dSiteElevation = newVal

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
    
    If Not g_bCanLatLongElev Then _
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
    
    If Not g_bCanLatLongElev Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteLatitude" & MSG_NOT_IMPLEMENTED
    
    If newVal < -90 Or newVal > 90 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLatitude " & MSG_VAL_OUTOFRANGE
            
    g_dLatitude = newVal

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
    
    If Not g_bCanLatLongElev Then _
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
    
    If Not g_bCanLatLongElev Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property SiteLongitude" & MSG_NOT_IMPLEMENTED
    
    If newVal < -180 Or newVal > 180 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SiteLongitude " & MSG_VAL_OUTOFRANGE
    
    g_dLongitude = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get Slewing() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "Slewing:"
    End If
        
    check_connected False, g_bTracking
    
    Slewing = g_Slewing <> slewNone
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(Slewing, "True", "False")
    End If
    
End Property

Public Property Get SlewSettleTime() As Integer

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SlewSettleTime: "
    End If
    
    SlewSettleTime = g_lSlewSettleTime / 1000
    
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
    
    If (newVal < 0) Or (newVal > 100) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property SlewSettleTime " & MSG_VAL_OUTOFRANGE
        
    g_lSlewSettleTime = newVal * 1000
    
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
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TargetDeclination" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If g_dTargetDeclination = INVALID_COORDINATE Then _
        Err.Raise SCODE_PROP_NOT_SET, ERR_SOURCE, _
            "Property Declination " & MSG_PROP_NOT_SET
        
    TargetDeclination = g_dTargetDeclination
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficEnd FmtSexa(TargetDeclination, False)
    End If
    
End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "TargetDeclination: " & _
                " -> " & FmtSexa(newVal, False)
    End If
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TargetDeclination" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If newVal > 90# Or newVal < -90# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetDeclination " & MSG_VAL_OUTOFRANGE
    g_dTargetDeclination = newVal
    
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
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TargetRightAscension" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If g_dTargetRightAscension = INVALID_COORDINATE Then _
        Err.Raise SCODE_PROP_NOT_SET, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_PROP_NOT_SET
        
    TargetRightAscension = g_dTargetRightAscension

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
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TargetRightAscension" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    If newVal >= 24# Or newVal < 0# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_VAL_OUTOFRANGE
    g_dTargetRightAscension = newVal
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get Tracking() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar _
                IIf(g_bTracking, "Tracking: True", "Tracking: False")
    End If
    
    check_connected False, g_bTracking
    
    Tracking = g_bTracking
    
End Property

Public Property Let Tracking(ByVal newVal As Boolean)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Tracking: " & g_bTracking & " -> " & newVal
    End If
    
    If Not g_bCanSetTracking Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Control of Tracking" & MSG_NOT_IMPLEMENTED

    check_connected newVal, g_bTracking
    
    If newVal Then
        ChangeHome False
        ChangePark False
    End If
    
    g_bTracking = newVal
    g_handBox.Tracking
    
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
    
    If g_bV1 Or Not g_bCanTrackingRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TrackingRate" & MSG_NOT_IMPLEMENTED
        
    TrackingRate = g_eTrackingRate

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
    
    If g_bV1 Or Not g_bCanTrackingRates Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TrackingRate" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    g_eTrackingRate = newVal
        
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

    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property TrackingRates" & MSG_NOT_IMPLEMENTED

    check_connected False, g_bTracking
    
    If g_bCanTrackingRates Then
        Set TrackingRates = g_TrackingRatesFull
    Else
        Set TrackingRates = g_TrackingRatesSimple
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
    
    If Not g_bCanDateTime Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property UTCDate" & MSG_NOT_IMPLEMENTED
    
    CurDate = CDbl(Now()) + (CDbl(utc_offs()) / 86400#)
    UTCDate = CDate(g_dDateDelta + CurDate)
    
    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficEnd CStr(UTCDate)
    End If
    
End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Dim CurDate As Double

    If Not g_show Is Nothing Then
        If g_show.chkTime.Value = 1 Then _
            g_show.TrafficStart "UTCDate: " & " -> " & newVal
    End If
    
    If Not g_bCanDateTime Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property UTCDate" & MSG_NOT_IMPLEMENTED
    
    CurDate = CDbl(Now()) + (CDbl(utc_offs()) / 86400#)
    g_dDateDelta = CDbl(newVal) - CurDate
    
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
    
    If g_bAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            "AbortSlew - " & MSG_ILLEGAL_WHILE_PARKED

    ChangeHome False
    ChangePark False
    g_Slewing = slewNone
    
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

    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method AxisRates" & MSG_NOT_IMPLEMENTED

    If Axis < 0 Or Axis > 2 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method AxisRates" & MSG_VAL_OUTOFRANGE
    
    ' for unsupported axis, return empty collection
    If Axis < g_iNumMoveAxis Then
        Set AxisRates = g_AxisRates
    Else
        Set AxisRates = g_AxisRatesEmpty
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

    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CanMoveAxis" & MSG_NOT_IMPLEMENTED
    
    If Axis < 0 Or Axis >= g_iNumMoveAxis Then
        CanMoveAxis = False
    Else
        CanMoveAxis = True
    End If

    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficEnd CStr(CanMoveAxis)
    End If

End Function

Public Sub CommandBlind(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False)

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "CommandBlind " & Command & " " & Raw & ": "
    End If
    
    If g_bV1 And Raw Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBlind" & MSG_NOT_IMPLEMENTED
        
    check_connected False, g_bTracking
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Function CommandBool(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "CommandBool " & Command & " " & Raw & ": "
    End If
    
    If g_bV1 And Raw Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBool" & MSG_NOT_IMPLEMENTED
        
    check_connected False, g_bTracking
    CommandBool = True

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(CommandBool)
    End If
    
End Function

Public Function CommandString(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "CommandString " & Command & " " & Raw & ":"
    End If

    If g_bV1 And Raw Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandString" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    CommandString = ""

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
            g_show.TrafficStart "DestinationSideOfPier:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, True) & ": "
    End If
    
    If g_bV1 Or g_eAlignMode <> algGermanPolar Or Not g_bCanSOP Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property DestinationSideOfPier" & MSG_NOT_IMPLEMENTED
            
    check_connected False, g_bTracking      ' according to spec last arg = True ???
    DestinationSideOfPier = SOPRADec(RightAscension, Declination)
                
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
    
    If Not g_bCanFindHome Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method FindHome" & MSG_NOT_IMPLEMENTED
        
    check_connected True, g_bTracking
    
    doHome
    
    While g_Slewing = slewHome
        DoEvents
    Wend
    
    ' AtHome state set in slewing code
        
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

    If g_bV1 Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method MoveAxis" & MSG_NOT_IMPLEMENTED

    check_connected True, g_bTracking

    If Axis < 0 Or Axis >= g_iNumMoveAxis Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method AxisRates Axis" & MSG_VAL_OUTOFRANGE
    
    If Not RateInRange(g_AxisRates, Rate) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Method AxisRates Rate" & MSG_VAL_OUTOFRANGE
    
    If g_Slewing = slewMoveAxis Or Rate <> 0# Then
        
        If g_Slewing <> slewMoveAxis Then
            g_Slewing = slewNone
            ChangeHome False
            ChangePark False
            g_dDeltaRA = 0#
            g_dDeltaDec = 0#
            g_dDeltaAlt = 0#
            g_dDeltaAz = 0#
        End If
        
        Select Case Axis
            Case axisPrimary:       g_dDeltaAz = Rate
            Case axisSecondary:     g_dDeltaAlt = Rate
            Case axisTertiary:      g_dDeltaDec = Rate
        End Select
        
        If g_dDeltaAz = 0# And g_dDeltaAlt = 0# And g_dDeltaDec = 0# Then
            If g_Slewing = slewMoveAxis Then
                g_Slewing = slewNone
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Value = 1 Then _
                        g_show.TrafficEnd " (MoveAxis stopped)"
                End If
                Exit Sub
            End If
        Else
            g_Slewing = slewMoveAxis
        End If
    
    End If
    
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
    
    If Not g_bCanPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Park" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    ' bail if already good
    If g_bAtPark Then _
        Exit Sub
        
    doPark
                       
    While g_Slewing = slewPark
        DoEvents
    Wend
    
    ' AtPark state set in slewing code
    
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
    
    If Not g_bCanPulseGuide Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method PulseGuide" & MSG_NOT_IMPLEMENTED

    check_connected True, g_bTracking
    
    ' check incomming parameter
    If Direction < 0 Or Direction > 3 Or Duration < 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property PulseGuide" & MSG_VAL_OUTOFRANGE
    
    ' set the tix count for the end of the guide
    tmpTix = GetTickCount() + Duration
    
    ' load up for the right direction
    Select Case Direction
        Case guideNorth:
            g_dGuideRateDeclination = Abs(g_dGuideRateDeclination)
            g_lPulseGuideTixDec = tmpTix
        Case guideSouth:
            g_dGuideRateDeclination = -Abs(g_dGuideRateDeclination)
            g_lPulseGuideTixDec = tmpTix
        Case guideEast:
            g_dGuideRateRightAscension = Abs(g_dGuideRateRightAscension)
            g_lPulseGuideTixRA = tmpTix
        Case guideWest:
            g_dGuideRateRightAscension = -Abs(g_dGuideRateRightAscension)
            g_lPulseGuideTixRA = tmpTix
    End Select
            
    out = " (initiated)"
    If Not g_bDualAxisPulseGuide Then
        g_Util.WaitForMilliseconds Duration
        out = " (done)"
    End If
        
    ' Since this is guiding, no coordinate changes will be done
    ' It is assumed that guiding calls are to maintain position
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Sub

Public Sub SetPark()

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetPark:"
    End If
    
    If Not g_bCanSetPark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SetPark" & MSG_NOT_IMPLEMENTED
        
    check_connected False, g_bTracking
    
    g_dParkAzimuth = g_dAzimuth
    g_dParkAltitude = g_dAltitude

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
    
    DoSetup
    
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
    
    If g_bV1 Or Not g_bCanSlewAltAz Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltAz()" & MSG_NOT_IMPLEMENTED
    
    check_connected True, False
    
    check_targetAltAz Altitude, Azimuth
    start_slewAltAz Altitude, Azimuth, True, g_SOP, slewAltAz
                       
    While g_Slewing = slewAltAz Or g_Slewing = slewSettle
        DoEvents
    Wend
 
    ' slew code in Startup.bas journals end of slew
    
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
        
    If g_bV1 Or Not g_bCanSlewAltAzAsync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToAltAzAsync()" & MSG_NOT_IMPLEMENTED
    
    check_connected True, False
    
    check_targetAltAz Altitude, Azimuth
    start_slewAltAz Altitude, Azimuth, True, g_SOP, slewAltAz
   
    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToCoordinates:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, False) & ":"
    End If
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToCoordinates" & MSG_NOT_IMPLEMENTED
    
    check_connected True, True
    
    check_target RightAscension, Declination
    g_dTargetRightAscension = RightAscension
    g_dTargetDeclination = Declination
    start_slew RightAscension, Declination, True, g_SOP
    
    While g_Slewing = slewRADec Or g_Slewing = slewSettle
        DoEvents
    Wend

    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "SlewToCoordinatesAsync:" & vbCrLf & _
            " RA " & FmtSexa(RightAscension, False) & vbCrLf & _
            " Dec " & FmtSexa(Declination, False) & ":"
    End If
    
    If Not g_bCanSlewAsync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToCoordinatesAsync" & MSG_NOT_IMPLEMENTED

    check_connected True, True
    
    check_target RightAscension, Declination
    g_dTargetRightAscension = RightAscension
    g_dTargetDeclination = Declination
    start_slew RightAscension, Declination, True, g_SOP
    
    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToTarget()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "SlewToTarget:"
    End If
    
    If Not g_bCanSlew Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToTarget" & MSG_NOT_IMPLEMENTED
    
    check_connected True, True
    
    check_target g_dTargetRightAscension, g_dTargetDeclination
    start_slew g_dTargetRightAscension, g_dTargetDeclination, True, g_SOP
    
    While g_Slewing = slewRADec Or g_Slewing = slewSettle
        DoEvents
    Wend
    
    ' slew code in Startup.bas journals end of slew
    
End Sub

Public Sub SlewToTargetAsync()

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "SlewToTargetAsync:"
    End If
    
    If Not g_bCanSlewAsync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SlewToTargetAsync" & MSG_NOT_IMPLEMENTED
    
    check_connected True, True
           
    check_target g_dTargetRightAscension, g_dTargetDeclination
    start_slew g_dTargetRightAscension, g_dTargetDeclination, True, g_SOP
    
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
    
    If g_bV1 Or Not g_bCanSyncAltAz Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToAltAz()" & MSG_NOT_IMPLEMENTED
    
    check_connected True, False
                    
    check_targetAltAz Altitude, Azimuth
    
    ChangeHome False
    ChangePark False
    g_dAltitude = Altitude
    g_dAzimuth = Azimuth
    
    calc_radec
    
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
            " Dec " & FmtSexa(Declination, False) & ":"
    End If
        
    If Not g_bCanSync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToCoordinates" & MSG_NOT_IMPLEMENTED
    
    check_connected True, True
    
    check_target RightAscension, Declination
    
    g_dTargetRightAscension = RightAscension
    g_dTargetDeclination = Declination
    
    ChangeHome False
    ChangePark False
    g_dRightAscension = g_dTargetRightAscension
    g_dDeclination = g_dTargetDeclination
    
    calc_altaz
    
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
        
    If Not g_bCanSync Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SyncToTarget" & MSG_NOT_IMPLEMENTED
    
    check_connected True, True
    
    check_target g_dTargetRightAscension, g_dTargetDeclination
    
    ChangeHome False
    ChangePark False
    g_dRightAscension = g_dTargetRightAscension
    g_dDeclination = g_dTargetDeclination
    
    calc_altaz
    
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
    
    If Not g_bCanUnpark Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method Unpark" & MSG_NOT_IMPLEMENTED
    
    check_connected False, g_bTracking
    
    doUnpark
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

' ================
' Private Routines
' ================

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the scope is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected(Park As Boolean, Track As Boolean)

    If Not g_bConnected Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    
    ' Global error checking for all slews and syncs
    If Park And g_bAtPark Then _
        Err.Raise SCODE_ILLEGAL_WHILE_PARKED, ERR_SOURCE, _
            MSG_ILLEGAL_WHILE_PARKED
    
    ' tracking checks was only refined for V2, only error in those cases
    If (Not g_bV1) And (Track <> g_bTracking) Then _
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

