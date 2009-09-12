Attribute VB_Name = "ScopeHW"
' -----------------------------------------------------------------------------
'  ===========
'  ScopeHW.BAS
'  ===========
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
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 10-Jan-06 dpp     Bug correction : POTH forgets to slave dome after a scope sync
' 30-Mar-06 dpp     Before to park scope, set Tracking False if scope supports it.
' 07-Apr-06 dpp     Save in registry the Geometry
' 30-Aug-06 jab     Move sync fix from 10 Jan 06 down into the sync only section
' 30-Aug-06 jab     ScopeSetSOP did not send the dome to the new location until
'                   after the scope arrived.  Now it sends the dome right away.
' 08-Sep-06 jab     Tuned off stop tracking before park.  Possible side effects
' -----------------------------------------------------------------------------

Option Explicit

Public Enum slewingType
    slewInit = 0
    slewSet = 1
    slewFetch = 2
End Enum

Public Sub ScopeClean(clear As Boolean)

    g_bConnected = False
    g_bTracking = False
    g_bAsyncSlewing = False
    g_bMonSlewing = False
    g_bAtHome = False
    g_bAtPark = False
    g_SOP = pierUnknown
    g_lPulseGuideTix = 0                            ' not currently pulse guiding
    
    g_iVersion = 0
    g_eAlignMode = ALG_UNKNOWN
    g_bCanAlignMode = False
    g_bCanAltAz = False
    g_bCanDoesRefraction = False
    g_bCanEqu = False
    g_bCanEquSystem = False
    g_bCanOptics = False
    g_bCanLatLong = False
    g_bCanElevation = False
    g_bCanDateTime = False
    g_bCanSideOfPier = False
    g_bCanSiderealTime = False
    
    g_bCanFindHome = False
    g_bCanPark = False
    g_bCanPulseGuide = False
    g_bCanSetDeclinationRate = False
    g_bCanSetGuideRates = False
    g_bCanSetPark = False
    g_bCanSetPierSide = False
    g_bCanSetRightAscensionRate = False
    g_bCanSetTracking = False
    g_bCanSlew = False
    g_bCanSlewAsync = False
    g_bCanSlewAltAz = False
    g_bCanSlewAltAzAsync = False
    g_bCanSync = False
    g_bCanSyncAltAz = False
    g_bCanUnpark = False

    If clear Then
        g_dAperture = val(g_Profile.GetValue(ID, "Aperture"))
        g_dApertureArea = val(g_Profile.GetValue(ID, "ApertureArea"))
        g_dFocalLength = val(g_Profile.GetValue(ID, "FocalLength"))
        g_dLatitude = val(g_Profile.GetValue(ID, "Latitude"))
        g_dLongitude = val(g_Profile.GetValue(ID, "Longitude"))
        g_dElevation = val(g_Profile.GetValue(ID, "Elevation"))
        g_lSlewSettleTime = CLng(g_Profile.GetValue(ID, "SlewSettleTime"))
        g_dMeridianDelay = val(g_Profile.GetValue(ID, "MeridianDelay"))
        g_dMeridianDelayEast = val(g_Profile.GetValue(ID, "MeridianDelayEast"))
        g_eDoesRefraction = CLng(g_Profile.GetValue(ID, "DoesRefraction"))
        g_eEquSystem = CLng(g_Profile.GetValue(ID, "EquSystem"))
        g_bAutoUnpark = CBool(g_Profile.GetValue(ID, "AutoUnpark"))
        g_bBacklash = CBool(g_Profile.GetValue(ID, "Backlash"))
        g_bSimple = CBool(g_Profile.GetValue(ID, "Simple"))
        If g_bSimple Then _
            g_bAutoUnpark = False
        g_bQuiet = CBool(g_Profile.GetValue(ID, "QuietMode"))
    End If
    
    g_dRightAscension = INVALID_PARAMETER
    g_dDeclination = INVALID_PARAMETER
    g_dAltitude = INVALID_PARAMETER
    g_dAzimuth = INVALID_PARAMETER
    g_dTargetRA = INVALID_PARAMETER
    g_dTargetDec = INVALID_PARAMETER
    
End Sub

Public Sub ScopeCreate(ID As String)
  
    If g_Scope Is Nothing Then
    
        If ID = "" Then _
            Err.Raise SCODE_NO_SCOPE, ERR_SOURCE, _
                "No Scope. " & MSG_NO_SCOPE
                
        
        If Not g_bForceLate Then
            On Error Resume Next
            Set g_IScope = CreateObject(ID)
            Set g_Scope = g_IScope
            On Error GoTo 0
        End If
        
        If g_Scope Is Nothing Then _
            Set g_Scope = CreateObject(ID)
        
        If g_Scope Is Nothing Then
            g_handBox.ErrorLEDScope True
        Else
            g_handBox.ErrorLEDScope False
        End If
        
        g_sScopeName = "(None)"
        
        On Error Resume Next
            g_sScopeName = Trim(g_Scope.Name)
        On Error GoTo 0
        
    End If
    
End Sub

Public Sub ScopeConnected()
    
    On Error GoTo ErrorHandler
    
    If g_bConnected = g_Scope.Connected Then _
        Exit Sub
        
ErrorHandler:

    If g_bConnected Then
        g_handBox.ErrorLEDScope True
        g_setupDlg.ConnectScope False
    End If
    
    On Error GoTo 0
    
End Sub

Public Sub ScopeDelete()
      
    On Error Resume Next
    
    If Not g_Scope Is Nothing Then
        
        If g_bConnected Then _
            g_Scope.Connected = False
        g_bConnected = False
        Set g_Scope = Nothing
        Set g_IScope = Nothing
         
    End If
    
    On Error GoTo 0
    
End Sub

'---------------------------------------------------------------------
'
' ScopeCoords() - Quiet mode requires that coordinates be fetched
' after any real change.  The INVALID_PARAMETER sets are because
' some scopes can't report sometimes.
'
'---------------------------------------------------------------------
Public Sub ScopeCoords(lost As Boolean, frcSOP As Boolean)

    On Error Resume Next
    
    If (lost Or frcSOP) And Not g_Scope Is Nothing Then
    
        ' g_dRightAscension = INVALID_PARAMETER
        ' g_dDeclination = INVALID_PARAMETER
        If g_bCanEqu Then
            g_dRightAscension = g_Scope.RightAscension
            g_dDeclination = g_Scope.Declination
        End If
        
        ' g_dAzimuth = INVALID_PARAMETER
        ' g_dAltitude = INVALID_PARAMETER
        If g_bCanAltAz Then
            g_dAzimuth = g_Scope.Azimuth
            g_dAltitude = g_Scope.Altitude
        End If
        
        ' don't do these as else's of above - order would be wrong
        If Not g_bCanEqu Then _
            calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination
        If Not g_bCanAltAz Then _
            calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude
    
        ' Don't get pier side while slewing if we're simulating pier side
        ' or we will loose proper destination pier side on overlapping
        ' east and west delays
        If (g_bSlewing Or (g_lPulseGuideTix > 0)) Then
            If g_bCanSideOfPier Then _
                ScopeSOP frcSOP
        Else
            ScopeSOP frcSOP
        End If
        
'        If (g_bSlewing Or (g_lPulseGuideTix > 0)) And g_bCanSideOfPier Then
'            ScopeSOP frcSOP
'        End If
    End If
    
    On Error GoTo 0
        
End Sub

Public Sub ScopeSave()

    g_Profile.WriteValue ID, "ScopeID", g_sScopeID
    g_Profile.WriteValue ID, "ScopeName", g_sScopeName
    g_Profile.WriteValue ID, "Aperture", Str(g_dAperture)
    g_Profile.WriteValue ID, "ApertureArea", Str(g_dApertureArea)
    g_Profile.WriteValue ID, "FocalLength", Str(g_dFocalLength)
    g_Profile.WriteValue ID, "Latitude", Str(g_dLatitude)
    g_Profile.WriteValue ID, "Longitude", Str(g_dLongitude)
    g_Profile.WriteValue ID, "Elevation", Str(g_dElevation)
    g_Profile.WriteValue ID, "SlewSettleTime", Str(g_lSlewSettleTime)
    g_Profile.WriteValue ID, "MeridianDelay", Str(g_dMeridianDelay)
    g_Profile.WriteValue ID, "MeridianDelayEast", Str(g_dMeridianDelayEast)
    g_Profile.WriteValue ID, "EquSystem", Str(g_eEquSystem)
    g_Profile.WriteValue ID, "DoesRefraction", Str(g_eDoesRefraction)
    g_Profile.WriteValue ID, "AutoUnpark", CStr(g_bAutoUnpark)
    g_Profile.WriteValue ID, "Backlash", CStr(g_bBacklash)
    g_Profile.WriteValue ID, "Simple", CStr(g_bSimple)
    g_Profile.WriteValue ID, "QuietMode", CStr(g_bQuiet)
    
End Sub

Public Function ScopeST() As Double
      
    ScopeST = INVALID_PARAMETER
    
    On Error Resume Next
    
    If g_bConnected Then
        If (g_bQuiet Or Not g_bCanSiderealTime) And _
                g_dLongitude >= -180 Then
            ScopeST = now_lst(g_dLongitude * DEG_RAD)
        Else
            If g_bCanSiderealTime Then _
                ScopeST = g_Scope.SiderealTime
        End If
    End If
    
    On Error GoTo 0
            
End Function

' Set global side of pier variable.  If frcSOP is set then if we are simulating
' SOP we'll recalc.  frcSOP means we must have done a goto.
Public Sub ScopeSOP(frcSOP As Boolean)

    Dim HA As Double
    Dim newVal As PierSide
    Dim out As String
    
    newVal = g_SOP      ' seed
    
    If g_eAlignMode = algGermanPolar Then
        
        If g_bCanSideOfPier Then
            On Error Resume Next
                newVal = g_Scope.SideOfPier
            On Error GoTo 0
        ElseIf frcSOP Then
            ' catch the case where we don't know RA
            If g_dRightAscension < -360 Then
                newVal = pierUnknown
            Else
                
                HA = HAScale(ScopeST() - g_dRightAscension)
                
                If HA > g_dMeridianDelay And HA > -g_dMeridianDelayEast Then
                    newVal = pierEast
                ElseIf HA < g_dMeridianDelay And HA < -g_dMeridianDelayEast Then
                    newVal = pierWest
                Else
                    ' two valid ways to get here OR
                    ' no way to slew to here (no man's land)
                    ' leave newVal as current from above
                    ' but it could have tracked in from pierWest
                End If
                
'                If HA > g_dMeridianDelay Then
'                    If HA < -g_dMeridianDelayEast Then
'                        ' ambiguous, leave newVal as current (from above)
'                    Else
'                        newVal = pierEast
'                    End If
'                ElseIf HA < -g_dMeridianDelayEast Then
'                    newVal = pierWest
'                Else
'                    ' no way to slew to here (no man's land)
'                    ' leave newVal as current from above
'                    ' but it could have tracked in from pierWest
'                End If
                
'                'old code when only one parameter
'                newVal = IIf(HAScale(ScopeST() - g_dRightAscension - g_dMeridianDelay) _
'                    >= 0#, pierEast, pierWest)
                
                If Not g_show Is Nothing Then
                    If g_show.chkCoord.Value = 1 Then
                        out = "really unknown"
                        Select Case newVal
                            Case pierUnknown:    out = "Unknown"
                            Case pierEast:       out = "East"
                            Case pierWest:       out = "West"
                        End Select
                        g_show.TrafficLine "Calculated SOP = " & out
                    End If
                End If
            End If
        End If
    Else
        newVal = pierUnknown
    End If
    
    If newVal <> g_SOP Then _
        g_handBox.LEDPier newVal
    
    g_SOP = newVal
        
End Sub

Public Sub ScopeSetSOP(newVal As PierSide)

    Dim Error As Boolean
    Dim buf As String, code As Long, src As String
    
    ' If we can't do it, don't try it.
    If Not g_bCanSetPierSide Then
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "SideOfPier" & MSG_NOT_IMPLEMENTED
        Exit Sub
    End If
    
    On Error Resume Next
    
    ' slave the dome (get it asynchronously on its way)
    If g_bSlaved Then
        g_bSlaveSlew = True
        DomeSlew g_dRightAscension, g_dDeclination, newVal
    End If
    
    ScopeAtHome False, True
    ScopeAtPark False, True
    
    ' set up monitors
    g_bMonSlewing = False
    g_bAsyncSlewing = False
    
    ' attempt to flip
    On Error GoTo ErrorHandler
    Error = False
    g_Scope.SideOfPier = newVal
    On Error Resume Next
    
    ScopeSlewing slewSet, True
    g_bAsyncSlewing = True
    g_bMonSlewing = True
            
    ' if failure, the set monitors accordingly
    If Error Then
        ScopeSlewing slewFetch
        If g_bSlewing Then _
            g_bAsyncSlewing = True
        g_bMonSlewing = True
        Err.Raise code, src, buf
    End If
    
    On Error GoTo 0
    Exit Sub
    
' don't just bail out on error, we have some clean up to do
ErrorHandler:

    Error = True
    buf = Err.Description
    code = Err.Number
    src = Err.Source
    Resume Next
    
End Sub

Public Function ScopeDSOP(ByVal RightAscension As Double, _
                             ByVal Declination As Double) As PierSide
    
    Dim HA As Double
    Dim SOP As PierSide
    Dim out As String
    
    SOP = pierUnknown
            
    If g_eAlignMode = algGermanPolar Then
    
        On Error Resume Next
        ' try to get answer from scope driver
        If g_bCanSideOfPier Then
            SOP = g_Scope.DestinationSideOfPier(RightAscension, Declination)
        End If
        
        ' simulate (sometimes DSOP not supported, but there is no Can flag)
        If SOP = pierUnknown Then
            
            HA = HAScale(ScopeST() - RightAscension)
            
            Select Case g_SOP
                Case pierUnknown:
                    ' check for unambiguous cases
                    If HA > g_dMeridianDelay And HA > -g_dMeridianDelayEast Then
                        SOP = pierEast
                    ElseIf HA < g_dMeridianDelay And HA < -g_dMeridianDelayEast Then
                        SOP = pierWest
                    Else
                        ' ambiguous, leave as unknown
                    End If
                Case pierEast:
                    If HA < -g_dMeridianDelayEast Then
                        SOP = pierWest
                    Else
                        SOP = pierEast
                    End If
                Case pierWest:
                    If HA > g_dMeridianDelay Then
                        SOP = pierEast
                    Else
                        SOP = pierWest
                    End If
            End Select
                    
'            ' are we going west of the west limit
'            If HA > g_dMeridianDelay Then
'                ' are we also
'                If HA < -g_dMeridianDelayEast Then
'                    ' valid from both sides, assume no flip
'                    SOP = g_SOP
'                Else
'                    ' only valid from the east side
'                    SOP = pierEast
'                End If
'            ElseIf HA < -g_dMeridianDelayEast Then
'                ' only valid from the west side
'                SOP = pierWest
'            Else
'                ' no way to slew to here
'                SOP = pierUnknown
'            End If
            
'            ' old approach
'            ScopeDSOP = _
'                IIf(HAScale(ScopeST() - RightAscension - g_dMeridianDelay) >= 0#, _
'                    pierEast, pierWest)
            
            If Not g_show Is Nothing Then
                If g_show.chkCoord.Value = 1 Then
                    out = "really unknown"
                    Select Case SOP
                        Case pierUnknown:    out = "Unknown"
                        Case pierEast:       out = "East"
                        Case pierWest:       out = "West"
                    End Select
                    g_show.TrafficLine "Calculated DSOP = " & out
                End If
            End If
        End If
        
        On Error GoTo 0
    End If
    
    ScopeDSOP = SOP
    
End Function

Public Sub ScopeAbortSlew()

    On Error Resume Next
    
    g_bMonSlewing = False   ' make sure Async doesn't get re-enabled
    g_bAsyncSlewing = False ' make sure Slewing doesn't get cleared too soon
    g_Scope.AbortSlew       ' ??? watch for failure
    ScopeTracking trackRead
    ScopeAtHome False, False
    ScopeAtPark False, False
    ScopeSlewing slewSet, False
    g_bMonSlewing = True
    If g_bSlaved Then
        DomeAbortSlew
        g_bSlaved = g_bDomeConnected    ' re-slave the dome (Halt unslaved)
        g_handBox.Slave
    End If

    On Error GoTo 0
    
End Sub

Public Sub ScopeHome()

    Dim Error As Boolean
    Dim buf As String, code As Long, src As String
    
    ScopeAtHome False, True
    ScopeAtPark False, True
    g_setupDlg.cmdHome.Caption = "Homing"
    
    ' set up monitors
    g_bMonSlewing = False
    g_bAsyncSlewing = False
    ScopeSlewing slewSet, True
    
    ' attempt to home
    On Error GoTo ErrorHandler
    Error = False
    g_Scope.FindHome
    On Error GoTo 0
    
    ' where are we ?
    ScopeCoords True, True
    ScopeTracking trackRead
    ScopeAtPark False, False
    
    ' slave the dome
    If g_bSlaved Then
        g_bSlaveSlew = True
        DomeSlew g_dRightAscension, g_dDeclination, g_SOP
    End If
    
    ' if failure, the set monitors accordingly
    If Error Then
        ScopeSlewing slewFetch
        If g_bSlewing Then _
            g_bAsyncSlewing = True
        g_bMonSlewing = True
        ScopeAtHome False, False
        g_setupDlg.cmdHome.Caption = "Error"
        Err.Raise code, src, buf
        Exit Sub
    End If
    
    ' clean up after successful home
    ScopeAtHome True, False
    g_setupDlg.cmdHome.Caption = "Home"
    ScopeSlewing slewSet, False
    g_bMonSlewing = True

    On Error GoTo 0
    Exit Sub
    
' don't just bail out on error, we have some clean up to do
ErrorHandler:

    Error = True
    buf = Err.Description
    code = Err.Number
    src = Err.Source
    Resume Next
    
End Sub

Public Sub ScopeAtHome(ByVal seed As Boolean, frc As Boolean)
    
    If Not frc Then
        If g_iVersion >= 2 Then
            If Not g_Scope Is Nothing Then _
                seed = g_Scope.AtHome
        Else
            If g_bTracking Then _
                seed = False
        End If
    End If
    
    If g_bAtHome <> seed Then
        g_bAtHome = seed
        g_setupDlg.LEDHome g_bAtHome
    End If
    
End Sub

Public Sub ScopeAtPark(ByVal seed As Boolean, frc As Boolean)
                                            ' frc =force
    If Not frc Then
        If g_iVersion >= 2 Then
            If Not g_Scope Is Nothing Then _
                seed = g_Scope.AtPark
        Else
            If g_bTracking Then _
                seed = False
        End If
    End If
        
    If seed <> g_bAtPark Then
        g_bAtPark = seed    ' set befor next call due to side effects
        g_handBox.ParkCaption = Not g_bAtPark
    End If
          
End Sub

Public Sub ScopePark()

    Dim Error As Boolean
    Dim buf As String, code As Long, src As String

    ScopeAtPark False, True
    ScopeAtHome False, True
    g_handBox.cmdParkScope.Caption = "Parking"
    
    ' set up monitors
    g_bMonSlewing = False
    g_bAsyncSlewing = False
    ScopeSlewing slewSet, True
    
    ' attempt to park
    On Error GoTo ErrorHandler
    Error = False
    
' This code disabled due to possible unintended system side effects - jab
'
'    ' first try to set Tracking False -- added dpp
'    If g_bCanSetTracking Then
'        g_handBox.chkTracking.Value = 0
'    End If
    
    g_Scope.Park
    
    ' where are we ?
    ScopeCoords True, True
    ScopeTracking trackRead
    ScopeAtHome False, False
        
    ' if failure, then set monitors accordingly
    If Error Then
        ScopeSlewing slewFetch
        If g_bSlewing Then _
            g_bAsyncSlewing = True
        g_bMonSlewing = True
        ScopeAtPark False, False
        g_handBox.cmdParkScope.Caption = "Park Error"
        
        On Error GoTo 0
        Err.Raise code, src, buf
    End If
    
    ' clean up after successful park
    ScopeAtPark True, False
    g_handBox.ParkCaption = Not g_bAtPark  ' get rid of "parking"
    ScopeSlewing slewSet, False
    g_bMonSlewing = True
    
    ' slave the dome
    If g_bSlaved Then
        g_bSlaveSlew = True
        DomeSlew g_dRightAscension, g_dDeclination, g_SOP
    End If
    
    On Error GoTo 0
    Exit Sub

' don't just bail out on error, we have some clean up to do
ErrorHandler:

    Error = True
    buf = Err.Description
    code = Err.Number
    src = Err.Source
    Resume Next

End Sub

Public Sub ScopeUnpark()
    Dim Error As Boolean
    Dim buf As String, code As Long, src As String

    g_handBox.cmdParkScope.Caption = "Un- Parking"
    
    On Error GoTo ErrorHandler
    Error = False
    
    g_Scope.Unpark
    
    If g_bAutoUnpark Then
        If g_Scope.CanSetTracking Then _
            g_Scope.Tracking = True
    End If
    
    ' where are we?
    ScopeCoords True, True
    ScopeTracking trackRead
    ScopeAtHome False, False
    ScopeAtPark False, False
    g_handBox.ParkCaption = Not g_bAtPark  ' get rid of "Un- Parking"
    
    If Error Then
        g_handBox.cmdParkScope.Caption = "UnPark Error"
        
        On Error GoTo 0
        Err.Raise code, src, buf
    End If
    
    ' slave the dome
    If g_bSlaved Then
        g_bSlaveSlew = True
        DomeSlew g_dRightAscension, g_dDeclination, g_SOP
    End If
    
    On Error GoTo 0
    Exit Sub

' don't just bail out on error, we have some clean up to do
ErrorHandler:

    Error = True
    buf = Err.Description
    code = Err.Number
    src = Err.Source
    Resume Next

End Sub

Public Sub ScopePreSlew(RA As Double, Dec As Double, _
        savetarget As Boolean, slew As Boolean, async As Boolean)

    Dim SOP As PierSide

    ' Global error checking for all slews and syncs
    If g_bAtPark And g_bCanUnpark Then _
        Err.Raise SCODE_SLEW_WHILE_PARKED, ERR_SOURCE, _
            MSG_SLEW_WHILE_PARKED
    
    If (RA < 0) Or (RA > 24) Or (Dec < -90) Or (Dec > 90) Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, MSG_VAL_OUTOFRANGE
    
    If savetarget Then
        g_dTargetRA = RA
        g_dTargetDec = Dec
    End If
    
    On Error Resume Next
    
    ' for slew, get destination SOP (sync has no SOP change)
    SOP = g_SOP
    If slew Then _
        SOP = ScopeDSOP(RA, Dec)

    ' slave the dome (get it asynchronously on its way)
    If g_bSlaved Then
        g_bSlaveSlew = True
        DomeSlew RA, Dec, SOP
    End If
        
    On Error GoTo 0
    
    ScopeAtHome False, True
    ScopeAtPark False, True
    
    ' set up the monitoring flags
    If slew Then
        g_bMonSlewing = False       ' don't watch for new asynchronous slews
        g_bAsyncSlewing = False     ' make sure we're not async
        If Not async Then _
            ScopeSlewing slewSet, True    ' declare slewing now for synchronous type
        If Not g_bCanSideOfPier Then
            g_SOP = SOP
            g_handBox.LEDPier g_SOP
        End If
    End If
    
End Sub

Public Sub ScopePostSlew(slew As Boolean, async As Boolean)

    ' clean up monitoring flags
    If slew Then
        If async Then
            ScopeSlewing slewSet, True
            g_bAsyncSlewing = True
            g_bMonSlewing = True
        Else
            g_bSlaveSlew = False
            ScopeSlewing slewSet, False
            g_bMonSlewing = True
            ' set the "where are we" flags
            ScopeCoords g_bQuiet, False
            ScopeTracking trackRead
            ScopeAtHome False, False
            ScopeAtPark False, False
        End If
    Else
        ' Turn back on slaving
        If g_bSlaved Then _
            g_bSlaveSlew = False

        ' set the "where are we" flags
        ScopeCoords g_bQuiet, False
        ScopeTracking trackRead
        ScopeAtHome False, False
        ScopeAtPark False, False
    End If
    
End Sub

Public Sub ScopeSlewing(Command As slewingType, Optional newVal As Boolean)
    
    If Command = slewInit Then
        g_bSlewing = False
        g_handBox.lblSlewing.Visible = False
        g_handBox.chkTracking.Visible = True
        Exit Sub
    End If
    
    If Command = slewFetch Then
'        If Not g_show Is Nothing Then
'            If g_show.chkPoll.Value = 1 Then _
'                g_show.TrafficChar "S "
'        End If
        newVal = g_Scope.Slewing
'        If Not g_show Is Nothing Then
'            If g_show.chkPoll.Value = 1 Then _
'                g_show.TrafficChar IIf(g_bSlewing, "T ", "F ")
'        End If
    End If
            
    If g_bSlewing <> newVal Then
        g_bSlewing = newVal
        If g_bSlewing Then
            g_handBox.chkTracking.Visible = False
            g_handBox.lblSlewing.Visible = True
        Else
            g_handBox.lblSlewing.Visible = False
            g_handBox.chkTracking.Visible = True
        End If
    End If
    
End Sub

Public Sub ScopeTracking(cmd As TrackCommand)

    Dim tmpTracking As Boolean
    
    Select Case cmd
    
        ' "zero" everything out
        Case trackClear
        
            g_bTracking = False
            g_handBox.chkTracking.Value = 2
        
        ' read and force the GUI
        Case trackInitial
            
            If Not g_Scope Is Nothing Then
                If g_bSimple Then
                    g_bTracking = True
                Else
                    g_bTracking = g_Scope.Tracking
                End If
            Else
                g_bTracking = False
            End If
            
            If g_bCanSetTracking Then _
                g_handBox.chkTracking.Value = IIf(g_bTracking, 1, 0)
        
        ' read, but only touch GUI if we have to
        Case trackRead
            If Not g_Scope Is Nothing Then
                If g_bSimple Then
                    tmpTracking = True
                Else
                    tmpTracking = g_Scope.Tracking
                End If
            Else
                tmpTracking = False
            End If
            
            If g_bCanSetTracking Then
                If tmpTracking <> g_bTracking Then
                    g_bTracking = tmpTracking
                    g_handBox.chkTracking.Value = IIf(tmpTracking, 1, 0)
                End If
            Else
                g_bTracking = tmpTracking
            End If
                        
    End Select
    
    ' check for illegal cases and freshen state
    If g_bTracking Then
        If g_bAtPark Then _
            ScopeAtPark False, False
        If g_bAtHome Then _
            ScopeAtHome False, False
    End If
        
End Sub

