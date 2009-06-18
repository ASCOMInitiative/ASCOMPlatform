Attribute VB_Name = "MainModule"
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
'   ==============
'   MainModule.bas
'   ==============
'
' Written:  2003/06/24   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/06/24 dbg     Initial edit
' -----------------------------------------------------------------------------

Sub Main()
    ' Initialize
    ConnectedCount = 0
    ScopeIsConnected = False
    DomeStatus = ""
    ScopeRA = 0
    ScopeDec = 0
    GotRADec = False
    IsSlewing = False
    Set Util = New DriverHelper.Util
    Set Profile = New DriverHelper.Profile
    
    Profile.DeviceType = "Telescope"          ' We're a Telescope driver
    Profile.Register SCOPEID, DESC            ' Self-register
    Profile.DeviceType = "Dome"               ' ... and we're a Dome driver
    Profile.Register DOMEID, DESC             ' Self-register
    
    ' Set position on screen
    Dim Temp
    Temp = Profile.GetValue(DOMEID, "Left")
    If Temp = "" Then Temp = 100
    frmMain.Left = CLng(Temp) * Screen.TwipsPerPixelX
    Temp = Profile.GetValue(DOMEID, "Top")
    If Temp = "" Then Temp = 100
    frmMain.Top = CLng(Temp) * Screen.TwipsPerPixelY
    '
    ' Fix bad positions (which shouldn't ever happen, ha ha)
    '
    If frmMain.Left < 0 Then
        frmMain.Left = 100 * Screen.TwipsPerPixelX
        Profile.WriteValue DOMEID, "Left", CStr(frmMain.Left \ Screen.TwipsPerPixelX)
    End If
    If frmMain.Top < 0 Then
        frmMain.Top = 100 * Screen.TwipsPerPixelY
        Profile.WriteValue DOMEID, "Top", CStr(frmMain.Top \ Screen.TwipsPerPixelY)
    End If
    
    ' Update and show form
    frmMain.UpdateStatus
    frmMain.Show
End Sub

