Attribute VB_Name = "Util"
' -----------------------------------------------------------------------------
' ==============
'  Util.BAS
' ==============
'
' General utility routines are all pulled in here
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     -----------------------------------------------------------
' 05-Mar-04 jab     initial edit, and V2 features
' -----------------------------------------------------------------------------

Option Explicit

Public Function FmtSexa(ByVal n As Double, ShowPlus As Boolean)
    Dim sg As String
    Dim us As String, ms As String, ss As String
    Dim u As Integer, m As Integer
    Dim fmt

    sg = "+"                                ' Assume positive
    If n < 0 Then                           ' Check neg.
        n = -n                              ' Make pos.
        sg = "-"                            ' Remember sign
    End If

    m = Fix(n)                              ' Units (deg or hr)
    us = Format$(m, "00")

    n = (n - m) * 60#
    m = Fix(n)                              ' Minutes
    ms = Format$(m, "00")

    n = (n - m) * 60#
    m = Fix(n)                              ' Minutes
    ss = Format$(m, "00")

    FmtSexa = us & ":" & ms & ":" & ss
    If ShowPlus Or (sg = "-") Then FmtSexa = sg & FmtSexa
    
End Function

'---------------------------------------------------------------------
'
' calc_altaz() - Calculate current azimuth and altitude
'
'---------------------------------------------------------------------
Public Sub calc_altaz()
    
    ' add ranging of HA to all other drivers / programs ???
    hadec_aa (g_dLatitude * DEG_RAD), _
            (RangeHA(g_dRightAscension - now_lst(g_dLongitude * DEG_RAD)) * HRS_RAD), _
             (g_dDeclination * DEG_RAD), _
             g_dAltitude, _
             g_dAzimuth
    g_dAltitude = g_dAltitude * RAD_DEG
    ' I think HA is backwards above, which is why this is reversed ???
    g_dAzimuth = 360# - (g_dAzimuth * RAD_DEG)
    
End Sub

'---------------------------------------------------------------------
'
' calc_radec() - Calculate current right ascension and declination
'
'---------------------------------------------------------------------
Public Sub calc_radec()
    Dim dHA As Double
    
    aa_hadec (g_dLatitude * DEG_RAD), _
            (g_dAltitude * DEG_RAD), _
            ((360# - g_dAzimuth) * DEG_RAD), _
            dHA, _
            g_dDeclination
    
    ' I think HA is backwards, which is why this Azimuth is reversed ???
    g_dRightAscension = (dHA * RAD_HRS) + now_lst(g_dLongitude * DEG_RAD)
    
    If g_dRightAscension < 0 Then _
        g_dRightAscension = g_dRightAscension + 24#
    If g_dRightAscension >= 24 Then _
        g_dRightAscension = g_dRightAscension - 24#
    g_dDeclination = g_dDeclination * RAD_DEG
    
End Sub

Public Function SOP(Az As Double) As PierSide

    If g_eAlignMode <> algGermanPolar Then
        SOP = pierUnknown
    Else
        SOP = IIf(Az >= 180, pierEast, pierWest)
    End If

End Function

Public Function SOPRADec(RA As Double, Dec As Double) As PierSide

    Dim ha As Double
    
    If g_eAlignMode <> algGermanPolar Then
        SOPRADec = pierUnknown
    Else
        ha = RangeHA(now_lst(g_dLongitude * DEG_RAD) - RA)
        SOPRADec = IIf(ha >= 0, pierEast, pierWest)
    End If

End Function

Public Sub RangeRADec()

    If g_dDeclination > 90# Then
        g_dDeclination = 90#
    ElseIf g_dDeclination < -90# Then
        g_dDeclination = -90#
    End If
    
    While g_dRightAscension < 0
        g_dRightAscension = g_dRightAscension + 24#
    Wend
    While g_dRightAscension >= 24#
        g_dRightAscension = g_dRightAscension - 24#
    Wend
    
End Sub

Public Function RangeHA(ByVal ha As Double)
    
    While ha < -12#
        ha = ha + 24#
    Wend
    While ha >= 12#
        ha = ha - 24#
    Wend
    
    RangeHA = ha
    
End Function

Public Sub RangeAltAz()

    If g_dAltitude > 90# Then
        g_dAltitude = 90#
    ElseIf g_dAltitude < -90# Then
        g_dAltitude = -90#
    End If
    
    While g_dAzimuth < 0
        g_dAzimuth = g_dAzimuth + 360#
    Wend
    While g_dAzimuth >= 360#
        g_dAzimuth = g_dAzimuth - 360#
    Wend
    
End Sub

Public Sub ChangePark(newVal As Boolean)

    If newVal <> g_bAtPark Then
        g_bAtPark = newVal
        g_handBox.ParkCaption
        g_handBox.LEDPark newVal
        If newVal And g_bDiscPark Then g_bConnected = False
    End If
    
End Sub

Public Sub ChangeHome(newVal As Boolean)

    If newVal <> g_bAtHome Then
        g_bAtHome = newVal
        g_handBox.LEDHome newVal
    End If
    
End Sub

'---------------------------------------------------------------------
'
' check_target() - Raise an error if target coordinates not set
'
'---------------------------------------------------------------------

Public Sub check_target(RA As Double, Dec As Double)

    If (RA = INVALID_COORDINATE) Or (Dec = INVALID_COORDINATE) Then _
            Err.Raise SCODE_NO_TARGET_COORDS, ERR_SOURCE, MSG_NO_TARGET_COORDS
     
    If RA >= 24# Or RA < 0# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "TargetRightAscension " & MSG_VAL_OUTOFRANGE
    
    If Dec > 90# Or Dec < -90# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "TargetDeclination " & MSG_VAL_OUTOFRANGE

End Sub

Public Sub check_targetAltAz(Alt As Double, Az As Double)
      
    If Alt > 90# Or Alt < -90# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Altitude " & MSG_VAL_OUTOFRANGE
    
    If Az >= 360# Or Az < 0# Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Azimuth " & MSG_VAL_OUTOFRANGE
            
End Sub

Public Sub doFlip(newVal As PierSide)
                
    ' kick off a slew to our current location, but with a flip
    If g_bTracking Then
        check_target g_dRightAscension, g_dDeclination
        start_slew g_dRightAscension, g_dDeclination, False, newVal
    Else
        check_targetAltAz g_dAltitude, g_dAzimuth
        start_slewAltAz g_dAltitude, g_dAzimuth, False, newVal, slewAltAz
    End If
        
End Sub

Public Sub doHome()

    Dim Alt As Double
    Dim Az As Double
    
    ' calculate home position (same as default park position)
    If g_dLatitude >= 0 Then
        Az = 180#
    Else
        Az = 0#
    End If
    Alt = 90# - Abs(g_dLatitude)
    
    check_targetAltAz Alt, Az
    g_bTracking = False             ' Stop tracking
    g_handBox.Tracking
    start_slewAltAz Alt, Az, True, g_SOP, slewHome
    
End Sub

Public Sub doPark()
                
    check_targetAltAz g_dParkAltitude, g_dParkAzimuth
    g_bTracking = False             ' Stop tracking
    g_handBox.Tracking
    start_slewAltAz g_dParkAltitude, g_dParkAzimuth, True, g_SOP, slewPark
    
End Sub

Public Sub doUnpark()
    
    ChangePark False
    If g_bAutoTrack Then
        ChangeHome False
        g_bTracking = True
        g_handBox.Tracking
    End If
    
End Sub

'---------------------------------------------------------------------
'
' start_slew - all the equatorial slew routines call here...
'
'---------------------------------------------------------------------
Public Sub start_slew(ByVal RA As Double, ByVal Dec As Double, _
        doSOP As Boolean, nSOP As PierSide)

    Dim tSOP As PierSide
    
    g_Slewing = slewNone
    
    ' calculate pier side or use the one handed in
    If doSOP Then
        tSOP = SOPRADec(RA, Dec)
    Else
        tSOP = nSOP
    End If
    
    ' If we're pier flipping, make sure we have a long weird slew
    If tSOP <> g_SOP Then
        If RA >= 12# Then
            g_dRightAscension = RA - 12#
        Else
            g_dRightAscension = RA + 12#
        End If
        calc_altaz      ' stay in sync
        g_SOP = tSOP    ' flip
        g_handBox.LEDPier g_SOP
    End If
    
    g_dDeltaAlt = 0#
    g_dDeltaAz = 0#
    g_dDeltaRA = RA - g_dRightAscension
    g_dDeltaDec = Dec - g_dDeclination
    
    If g_dDeltaRA < -12# Then
        g_dDeltaRA = g_dDeltaRA + 24#
    ElseIf g_dDeltaRA > 12# Then
        g_dDeltaRA = g_dDeltaRA - 24#
    End If
    
    ChangeHome False
    ChangePark False
    g_Slewing = slewRADec
    
End Sub
 
Public Sub start_slewAltAz(ByVal Alt As Double, ByVal Az As Double, _
        doSOP As Boolean, nSOP As PierSide, slew As slewType)

    Dim tSOP As PierSide
    
    g_Slewing = slewNone
    
    ' calculate pier side or use the one handed in
    If doSOP Then
        tSOP = SOP(Az)
    Else
        tSOP = nSOP
    End If
    
    ' If we're pier flipping, make sure we have a long weird slew
    If tSOP <> g_SOP Then
        If Az >= 180# Then
            g_dAzimuth = Az - 180#
        Else
            g_dAzimuth = Az + 180#
        End If
        calc_radec      ' stay in sync
        g_SOP = tSOP    ' flip
        g_handBox.LEDPier g_SOP
    End If
    
    g_dDeltaRA = 0#
    g_dDeltaDec = 0#
    g_dDeltaAlt = Alt - g_dAltitude
    g_dDeltaAz = Az - g_dAzimuth
    
    If g_dDeltaAz < -180# Then
        g_dDeltaAz = g_dDeltaAz + 360#
    ElseIf g_dDeltaAz > 180# Then
        g_dDeltaAz = g_dDeltaAz - 360#
    End If
    
    ChangeHome False
    ChangePark False
    g_Slewing = slew
    
End Sub

Public Function RateInRange(domain As Object, checkRate As Double) As Boolean
    Dim r As Rate
    
    For Each r In domain
        If Abs(checkRate) <= r.Maximum And Abs(checkRate) >= r.Minimum Then
            RateInRange = True
            Exit Function
        End If
    Next
    RateInRange = False
    
End Function
