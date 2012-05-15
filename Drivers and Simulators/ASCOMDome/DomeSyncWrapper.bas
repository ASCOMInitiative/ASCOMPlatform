Attribute VB_Name = "DomeSyncWrapper"
'---------------------------------------------------------------------
' Copyright © 2003 Diffraction Limited
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DIFFRACTION LIMITED. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ===================
'   DomeSyncWrapper.bas
'   ===================
'
' Written:  2003/07/11   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/07/11 dbg     Initial edit
' 2005/02/14 dbg     Improved sidereal time algorithm, V2 SideOfPier
' 2006/04/10 dbg     Corrected sign convention error for pier offset parameters
' 2008/01/10 dbg     Adjusted SideOfPier calculation, corrected for below pole condition
' 2012/06/06 cdr     Use DomeControl class to get the dome target Altitude and Azimuth
' -----------------------------------------------------------------------------

' This module sets up parameters for DomeSync and handles
' pier flipping and special cases

Option Explicit

Public Sub CalcDomeAltAz(ByVal IsV2 As Boolean, ByVal SideOfPier As PierSide)
    Dim phi As Double
    Dim HArad As Double, DECrad As Double
    
    phi = degrad(Latitude)
    ' Calculate hour angle based on local sidereal time
    HArad = hrrad(SiderealTime() - ScopeRA)
    If (HArad < -PI) Then HArad = HArad + PI * 2
    If (HArad > PI) Then HArad = HArad - PI * 2
    DECrad = degrad(ScopeDec)
    On Error GoTo Err
    ' Calculate telescope alt/az
    hadec_aa phi, HArad, DECrad, ScopeAlt, ScopeAz
    ScopeAlt = raddeg(ScopeAlt)
    ScopeAz = raddeg(ScopeAz)

    ' Check for pole flipping
    Dim Flip As Boolean
    Flip = False
    If IsV2 Then
        If SideOfPier = pierEast Then Flip = True
    Else
        ' Lacking SideOfPier property, using simple telescope azimuth calculation
        Flip = ScopeAz > 180
    End If
    
    ' use DomeControl class to determine the dome Alt Az
    Dim dc As DomeControl
    Set dc = New DomeControl
    dc.InitDome DomeRadius, OffsetOptical, OffsetNorth, OffsetEast, -OffsetHeight
    TargetAz = dc.DomeAzimuth(ScopeAz, ScopeAlt, raddeg(HArad), Not Flip)
    TargetAlt = dc.DomeAltitude
    Exit Sub
Err:
    LoadDLL "astro32.dll"
End Sub

' returns the local sidereal time in degrees
Private Function SiderealTime() As Double
    ' Get UT from the operating system (assumes time zone set correctly)
    Dim SysTime As SYSTEMTIME
    GetSystemTime SysTime
    Dim a, b, c, d, jd, jt, GMST As Double
    
    Dim Second As Double
    Second = SysTime.wSecond + SysTime.wMilliseconds / 1000
   
    If SysTime.wMonth = 1 Or SysTime.wMonth = 2 Then
        SysTime.wYear = SysTime.wYear - 1
        SysTime.wMonth = SysTime.wMonth + 12
    End If
    
    a = Int(SysTime.wYear / 100)
    b = 2 - a + Int(a / 4)
    c = Int(365.25 * SysTime.wYear)
    d = Int(30.6001 * (SysTime.wMonth + 1))

    ' Days since J2000.0
    jd = b + c + d - 730550.5 + SysTime.wDay + (SysTime.wHour + SysTime.wMinute / 60# + SysTime.wSecond / 3600#) / 24#
    
    ' julian centuries since J2000.0
    jt = jd / 36525#
    GMST = 280.46061837 + 360.98564736629 * jd + 0.000387933 * jt * jt - jt * jt * jt / 38710000 + Longitude
    If GMST > 0# Then
        While (GMST > 360#)
            GMST = GMST - 360#
        Wend
    Else
        While (GMST < 0#)
            GMST = GMST + 360#
        Wend
    End If
    
    SiderealTime = GMST / 15
   
End Function

