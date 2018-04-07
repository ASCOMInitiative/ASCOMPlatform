Attribute VB_Name = "DomeHW"
' -----------------------------------------------------------------------------
'  ==========
'  DomeHW.BAS
'  ==========
'
' Dome hardware abstraction layer.
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     -----------------------------------------------------------
' 15-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 06-May-12 cdr     Use DomeControl class to determine the dome alt and Azm
' -----------------------------------------------------------------------------

Option Explicit

Public Sub DomeClean(clear As Boolean)

    g_bDomeConnected = False
    g_bSlaved = False
    g_bSlaveSlew = False
    
    g_bDomeFindHome = False
    g_bDomePark = False
    g_bDomeSetAltitude = False
    g_bDomeSetAzimuth = False
    g_bDomeSetPark = False
    g_bDomeSetShutter = False
    g_bDomeSyncAzimuth = False
    
    If clear Then
        g_dRadius = val(g_DomeProfile.GetValue(IDDOME, "Radius"))
        g_iPosEW = CInt(g_DomeProfile.GetValue(IDDOME, "PosEW"))
        g_iPosNS = CInt(g_DomeProfile.GetValue(IDDOME, "PosNS"))
        g_iPosUD = CInt(g_DomeProfile.GetValue(IDDOME, "PosUD"))
        g_iGEMOffset = CInt(g_DomeProfile.GetValue(IDDOME, "GEMOffset"))
        g_iSlop = CInt(g_DomeProfile.GetValue(IDDOME, "Slop"))
        slave_time_reset = val(g_DomeProfile.GetValue(IDDOME, "Freq"))
    End If
          
    g_dDomeAltitude = INVALID_PARAMETER
    g_dDomeAzimuth = INVALID_PARAMETER
   
End Sub

Public Sub DomeCreate(ID As String)
  
    If g_Dome Is Nothing Then
    
        If ID = "" Then _
            Err.Raise SCODE_NO_DOME, ERR_SOURCE, _
                "No Dome. " & MSG_NO_DOME
                
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IDome = CreateObject(ID)
            Set g_Dome = g_IDome
            On Error GoTo 0
        End If
        
        If g_Dome Is Nothing Then _
            Set g_Dome = CreateObject(ID)
    
        If g_Dome Is Nothing Then
            g_handBox.ErrorLEDDome True
        Else
            g_handBox.ErrorLEDDome False
            g_handBox.ErrorLED False
        End If
        
        g_sDomeName = "(None)"
        
        On Error Resume Next
            g_sDomeName = Trim(g_Dome.Name)
        On Error GoTo 0
        
    End If
    
End Sub

Public Sub DomeDelete()
  
    On Error Resume Next
    
    If Not g_Dome Is Nothing Then
        
        If g_bDomeConnected Then _
            g_Dome.Connected = False
        g_bDomeConnected = False
        Set g_Dome = Nothing
        Set g_IDome = Nothing
         
    End If
    
    On Error GoTo 0
    
End Sub

Public Sub DomeConnected()
    
    On Error GoTo ErrorHandler
    
    If g_bDomeConnected = g_Dome.Connected Then _
        Exit Sub
        
ErrorHandler:

    If g_bDomeConnected Then
        g_handBox.ErrorLEDDome True
        g_setupDlg.ConnectDome False
    End If
    
    On Error GoTo 0

End Sub

Public Sub DomeAbortSlew()

    If g_bSlaved Then
        g_bSlaved = False
        g_bSlaveSlew = False
        g_handBox.Slave
    End If

    g_Dome.AbortSlew
            
End Sub

Public Sub DomeFindHome()

    If g_bSlaved Then
        g_bSlaved = False
        g_handBox.Slave
    End If
    
    g_Dome.FindHome
        
End Sub

Public Sub DomePark()

    If g_bSlaved Then
        g_bSlaved = False
        g_handBox.Slave
    End If
    
    g_Dome.Park
        
End Sub

Public Sub DomeSave()

    g_DomeProfile.WriteValue IDDOME, "DomeID", g_sDomeID
    g_DomeProfile.WriteValue IDDOME, "DomeName", g_sDomeName
    g_DomeProfile.WriteValue IDDOME, "Radius", Str(g_dRadius)
    g_DomeProfile.WriteValue IDDOME, "PosEW", Str(g_iPosEW)
    g_DomeProfile.WriteValue IDDOME, "PosNS", Str(g_iPosNS)
    g_DomeProfile.WriteValue IDDOME, "PosUD", Str(g_iPosUD)
    g_DomeProfile.WriteValue IDDOME, "GEMOffset", Str(g_iGEMOffset)
    g_DomeProfile.WriteValue IDDOME, "Slop", Str(g_iSlop)
    g_DomeProfile.WriteValue IDDOME, "Freq", Str(slave_time_reset)
   
End Sub

' slew dome to dome coords of specified scope RA, Dec
Public Sub DomeSlew(ByVal RA As Double, ByVal Dec As Double, SOP As PierSide)

    Dim Az As Double, alt As Double
    Dim doAlt As Boolean
    
    ' check for connected and valid scope coordinates
    If Not g_bDomeConnected Or RA < -360# Or Dec < -90# Then _
        Exit Sub
        
    ' do not combine with above, we now know that g_Dome should exist

'    If g_Dome.Slewing Then _
'        Exit Sub

    On Error Resume Next
    
    DomeCoord ScopeST(), RA, Dec, Az, alt, SOP
    
    doAlt = g_bDomeSetAltitude And (alt >= -90)
    If doAlt And g_bDomeSetShutter Then _
        doAlt = doAlt And (g_Dome.ShutterStatus = shutterOpen)
            
    If g_bDomeSetAzimuth And (Az >= -360) Then _
        g_Dome.SlewToAzimuth Az
        
    If doAlt Then _
        g_Dome.SlewToAltitude alt
        
    On Error GoTo 0

End Sub

Public Sub DomeCoord(SiderealTime As Double, _
        ScopeRA As Double, ScopeDec As Double, _
        DomeAz As Double, DomeAlt As Double, SOP As PierSide)
    
    Dim ha As Double, Dec As Double     ' hour angle and Dec in Rad
'    Dim GEMOffset As Double
    Dim alt As Double, Az As Double     ' return values
    
    DomeAz = INVALID_PARAMETER
    DomeAlt = INVALID_PARAMETER
    If ScopeRA < -360# Or ScopeDec < -90# Or _
            SiderealTime < 0# Or g_dLatitude < -90# Then
        Exit Sub
    End If
    
    ' calculate hour angle and Dec from the supplied values in Radians
    ha = HAScale(SiderealTime - ScopeRA) * HRS_RAD
    Dec = ScopeDec * DEG_RAD
    
    ' meridian flip
'    Select Case SOP
'        Case pierUnknown:    GEMOffset = 0#
'        Case pierEast:       GEMOffset = -g_iGEMOffset
'        Case pierWest:       GEMOffset = g_iGEMOffset
'    End Select
        
    ' do the dome conversion, lots of math under here
'    CalcDomeAzAlt HA, Dec, g_dLatitude * DEG_RAD, _
'        -CDbl(g_iPosNS), CDbl(g_iPosEW), CDbl(g_iPosUD), _
'        GEMOffset, g_dRadius * 1000, _
'        Az, Alt
'
'    DomeAz = Az * RAD_DEG
'    DomeAlt = Alt * RAD_DEG
    
    ' dome conversion using DomeControl
    Dim scopeAlt As Double, scopeAzm As Double
    hadec_aa g_dLatitude * DEG_RAD, ha, Dec, scopeAlt, scopeAzm
    scopeAzm = scopeAzm * RAD_DEG
    scopeAlt = scopeAlt * RAD_DEG
    
    Dim dc As New DomeControl
    dc.InitDome g_dRadius * 1000, CDbl(g_iGEMOffset), CDbl(g_iPosNS), CDbl(g_iPosEW), CDbl(g_iPosUD), g_dLatitude, g_iGEMOffset > 0
    DomeAz = dc.DomeAzimuth(scopeAzm, scopeAlt, ha * RAD_DEG, (SOP = pierWest))
    DomeAlt = dc.DomeAltitude
    
End Sub

