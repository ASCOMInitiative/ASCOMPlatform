VERSION 5.00
Begin VB.Form frmMain 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "ASCOM Dome Control Panel"
   ClientHeight    =   4320
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   5490
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   4320
   ScaleWidth      =   5490
   Begin VB.CommandButton SetParkButton 
      Caption         =   "Set"
      Height          =   375
      Left            =   4695
      TabIndex        =   14
      Top             =   1590
      Width           =   585
   End
   Begin VB.CommandButton OpenShutterButton 
      Caption         =   "Open"
      Height          =   375
      Left            =   4080
      TabIndex        =   13
      Top             =   150
      Width           =   1215
   End
   Begin VB.CommandButton CloseShutterButton 
      Caption         =   "Close"
      Height          =   375
      Left            =   4080
      TabIndex        =   12
      Top             =   630
      Width           =   1215
   End
   Begin VB.CommandButton ParkButton 
      Caption         =   "Park"
      Height          =   375
      Left            =   4080
      TabIndex        =   11
      Top             =   1590
      Width           =   585
   End
   Begin VB.CommandButton HomeButton 
      Caption         =   "Home"
      Height          =   375
      Left            =   4080
      TabIndex        =   1
      Top             =   1100
      Width           =   1215
   End
   Begin VB.CheckBox SlaveCheck 
      Caption         =   "Slave Dome to Scope"
      Height          =   855
      Left            =   120
      TabIndex        =   10
      Top             =   1170
      Width           =   855
   End
   Begin VB.CommandButton AbortButton 
      Caption         =   "ABORT"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   800
      Left            =   105
      TabIndex        =   0
      Top             =   3090
      Width           =   870
   End
   Begin VB.CommandButton ConnectButton 
      Caption         =   "Connect"
      Height          =   375
      Left            =   4080
      TabIndex        =   3
      Top             =   2070
      Width           =   1215
   End
   Begin VB.CommandButton GoToButton 
      Caption         =   "Go To"
      Height          =   800
      Left            =   105
      TabIndex        =   2
      Top             =   2130
      Width           =   870
   End
   Begin VB.CommandButton DisconnectButton 
      Caption         =   "Disconnect"
      Height          =   375
      Left            =   4080
      TabIndex        =   4
      Top             =   2550
      Width           =   1215
   End
   Begin VB.CommandButton CloseButton 
      Caption         =   "Exit"
      Height          =   375
      Left            =   4080
      TabIndex        =   6
      Top             =   3510
      Width           =   1215
   End
   Begin VB.Timer Timer1 
      Interval        =   1000
      Left            =   3210
      Top             =   465
   End
   Begin VB.TextBox txtStatus 
      BackColor       =   &H8000000F&
      Height          =   3795
      Left            =   1065
      MultiLine       =   -1  'True
      TabIndex        =   8
      Text            =   "frmMain.frx":0442
      Top             =   120
      Width           =   2775
   End
   Begin VB.PictureBox Picture1 
      AutoSize        =   -1  'True
      Height          =   900
      Left            =   105
      MouseIcon       =   "frmMain.frx":0474
      MousePointer    =   99  'Custom
      Picture         =   "frmMain.frx":05C6
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   7
      Top             =   105
      Width           =   780
   End
   Begin VB.CommandButton SetupButton 
      Caption         =   "Setup"
      Height          =   375
      Left            =   4080
      TabIndex        =   5
      Top             =   3030
      Width           =   1215
   End
   Begin VB.Label Label1 
      Caption         =   "Copyright (c) 2003 Diffraction Limited  http://www.cyanogen.com"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H8000000D&
      Height          =   255
      Left            =   360
      MouseIcon       =   "frmMain.frx":148A
      MousePointer    =   99  'Custom
      TabIndex        =   9
      Top             =   3990
      Width           =   4695
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
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
' Written:  2003/06/24   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/06/24 dbg     Initial edit
' 2004/11/16 dbg     Don't force home on startup; dome might have been
'                    previously homed and this could be irritating to the user.
'                    Allow changing setup while connected except for
'                    dome and telescope selection
' 2004/11/23 rbd     Version 4.0.1
' 2005/01/16 dbg     Removed check for Homed on dome slew command
' 2005/02/14 dbg     Added V2 SideOfPier checking and improved status display
' 2006/04/10 dbg     Corrected sign convention error for pier offset parameters
' 2008/01/01 dbg     Version 5.0.0
' -----------------------------------------------------------------------------

Option Explicit

Private Homed As Boolean
Private Homing As Boolean
Private Parking As Boolean
Private PollCount As Integer
Private FoundHomeOnce As Boolean

Private Sub Form_Initialize()
    On Error Resume Next
    ScopeIsConnected = False
    DomeIsConnected = False
    DomeStatus = ""
    ScopeRA = 0
    ScopeDec = 0
    ScopeAlt = 0
    ScopeAz = 0
    TargetAlt = 0
    TargetAz = 0
    PollCount = 0
    FoundHomeOnce = False
    
'    Profile.DeviceType = "Dome"               ' Don't register here
'    Profile.Register ID, DESC
    
    SlaveCheck.Value = val(Profile.GetValue(DOMEID, "Slave"))
    
    EnableButtons
    Homed = False
    Homing = False
    Parking = False
    
    ' Get setup data from the Registry
    frmSetup.LoadData
End Sub

Private Sub EnableButtons()
    DisconnectButton.Enabled = DomeIsConnected
    ConnectButton.Enabled = Not DomeIsConnected
    AbortButton.Enabled = DomeIsConnected
    HomeButton.Enabled = DomeIsConnected And DomeCanFindHome
    OpenShutterButton.Enabled = DomeIsConnected And DomeCanSetShutter
    CloseShutterButton.Enabled = DomeIsConnected And DomeCanSetShutter
    GoToButton.Enabled = DomeIsConnected
    ParkButton.Enabled = DomeCanPark And DomeIsConnected
    SetParkButton.Enabled = DomeCanPark And DomeIsConnected
    SlaveCheck.Enabled = DomeIsConnected
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    On Error Resume Next
    If UnloadMode <> 0 Then
        ' Exiting with extreme prejudice
        TheDome.AbortSlew
        TheScope.AbortSlew
        Exit Sub
    End If
    
    CloseButton_Click
    
    ' Check whether we aborted the shutdown
    If ScopeIsConnected Then Cancel = True
End Sub

Private Sub CloseButton_Click()
    ' Handle case we're disconnecting, but an outside application has a connection
    On Error Resume Next
    
    Dim TempDiscon As Boolean
    TempDiscon = False
    If ScopeIsConnected Then
        LocalScope.Connected = False
        Set LocalScope = Nothing
        If ScopeIsConnected Then TempDiscon = True
    End If
    
    ' If we're still connected, then an outside connection is in progress
    If ScopeIsConnected Then
        If Not MsgBox("Exiting will disconnect the telescope from your control application; do you want to proceed?", vbYesNo, "Warning") = vbYes Then
            
            ' Restore the local scope connection and quit
            If TempDiscon Then
                Set LocalScope = New Telescope
                LocalScope.Connected = True
            End If
            Exit Sub
        End If
    End If
    
    ' Save screen position if valid
    If WindowState = vbNormal Then        ' Save pos only if normal view
        If Left >= 0 Then _
            Profile.WriteValue DOMEID, "Left", CStr(Left \ Screen.TwipsPerPixelX)
        If Top >= 0 Then _
            Profile.WriteValue DOMEID, "Top", CStr(Top \ Screen.TwipsPerPixelY)
    End If
    
    ' Terminate everything
    TheDome.AbortSlew
    TheDome.Connected = False
    TheScope.AbortSlew
    TheScope.Connected = False
    Set TheDome = Nothing
    Unload Me
    End
End Sub

Private Sub AbortButton_Click()
    On Error Resume Next
    SlaveCheck.Value = False
    Homing = False
    HomeButton.SetFocus
    TheDome.AbortSlew
    UpdateStatus
End Sub

Public Sub ConnectButton_Click()
    ' Create a dome object
    On Error GoTo Err1
    Dim DomeProgID As String
    DomeProgID = Profile.GetValue(DOMEID, "Dome")
    If Len(DomeProgID) = 0 Then
        Err.Raise SCODE_NOT_SELECTED, ERR_SOURCE, MSG_NOT_SELECTED
        GoTo Err1
    End If
    Set TheDome = CreateObject(DomeProgID)
    TheDome.Connected = True
    DomeIsConnected = True
    
    ' Create one of our OWN telescope objects, which will in turn create the telescope object
    On Error GoTo Err2
    Set LocalScope = New Telescope
    LocalScope.Connected = True
    
    ' There's not much point to using this hub if you can't slave it
    If Not TheDome.CanSetAzimuth Then
        MsgBox "ASCOM Dome Control Panel only supports domes with Azimuth control", vbCritical, "Error"
        GoTo Err
    End If
    
    ' Success!
    DomeCanFindHome = TheDome.CanFindHome
    DomeCanPark = TheDome.CanPark
    DomeCanSetAltitude = TheDome.CanSetAltitude
    DomeCanSyncAzimuth = TheDome.CanSyncAzimuth
    DomeCanSetShutter = TheDome.CanSetShutter
    DomeCanSetPark = TheDome.CanSetPark
    
    ' If dome cannot be homed, then force flag true
    Homed = Not DomeCanFindHome
    
    On Error Resume Next
    EnableButtons
    HomeButton.SetFocus
    Homing = False
    Parking = False
    
    ' Try to home the dome if required and possible
    If DomeCanFindHome And AutoHome And Not FoundHomeOnce Then
        SlaveCheck.Value = False
        Homed = False
        Homing = True
        TheDome.FindHome
    End If
    
    UpdateStatus
    
    Exit Sub
    
Err1:
    MsgBox "Could not open dome" + vbCr + vbLf + Err.Description, vbCritical, "Error"
    GoTo Err
    
Err2:
    MsgBox "Could not link to telescope" + vbCr + vbLf + Err.Description, vbCritical, "Error"
    GoTo Err
    
Err:
    On Error Resume Next
    'TheDome.Connected = False
    DomeIsConnected = False
    Set TheDome = Nothing
    
    'LocalScope.Connected = False
    Set LocalScope = Nothing
    
    EnableButtons
    
    UpdateStatus
End Sub

Public Sub DisconnectButton_Click()
    On Error Resume Next
    TheDome.AbortSlew
    TheDome.Connected = False
    DomeIsConnected = False
    Set TheDome = Nothing
    
    LocalScope.AbortSlew
    LocalScope.Connected = False
    Set LocalScope = Nothing
    
    EnableButtons
    
    UpdateStatus
End Sub

Private Sub GoToButton_Click()
    On Error GoTo Err
    
    frmGoTo.AltitudeEnabled = DomeCanSetAltitude
    frmGoTo.Show 1
    If Not frmGoTo.Ok Then Exit Sub
    
    SlaveCheck.Value = False
    Homing = False
    TheDome.SlewToAzimuth frmGoTo.Azimuth
    If DomeCanSetAltitude Then TheDome.SlewToAltitude frmGoTo.Altitude
    UpdateStatus
    Exit Sub
    
Err:
    MsgBox Err.Description, vbCritical, "Error"
End Sub

Private Sub HomeButton_Click()
    On Error GoTo Err
    
    SlaveCheck.Value = False
    Homed = False
    Homing = True
    TheDome.FindHome
    UpdateStatus
    Exit Sub

Err:
    MsgBox Err.Description, vbCritical, "Error"
End Sub

Private Sub OpenShutterButton_Click()
    On Error Resume Next
    TheDome.OpenShutter
    UpdateStatus
End Sub

Private Sub CloseShutterButton_Click()
    On Error Resume Next
    TheDome.CloseShutter
    UpdateStatus
End Sub

Private Sub ParkButton_Click()
    On Error GoTo Err
    
    SlaveCheck.Value = False
    TheDome.Park
    Parking = True
    UpdateStatus
    Exit Sub

Err:
    MsgBox Err.Description, vbCritical, "Error"
End Sub

Private Sub Label1_Click()
    DisplayWebPage "http://www.cyanogen.com/"
End Sub

Private Sub Picture1_Click()
    DisplayWebPage "http://ASCOM-Standards.org/"
End Sub

Private Sub SetParkButton_Click()
    If MsgBox("Set dome park position?", vbQuestion + vbYesNo, "ASCOM Dome Control Panel") = vbYes Then
        TheDome.SetPark
    End If
End Sub

Private Sub SetupButton_Click()
    frmSetup.Show 1
    UpdateStatus
End Sub

Public Function CrossRef(ByVal Obj As String) As String
    ' Cross-reference to actual name
    Dim d As Scripting.Dictionary
    Dim i As Integer
    
    Set d = EnumKeys("Telescope" & " Drivers", ERR_SOURCE_DOME)      ' Get Key-Class pairs
    For i = 0 To d.Count - 1
        
        If Obj = d.Keys(i) Then
             CrossRef = d.Items(i)
             Exit Function
        End If
    Next
    
    Set d = EnumKeys("Dome" & " Drivers", ERR_SOURCE_DOME)      ' Get Key-Class pairs
    For i = 0 To d.Count - 1
        
        If Obj = d.Keys(i) Then
             CrossRef = d.Items(i)
             Exit Function
        End If
    Next
    
    ' Fallback
    CrossRef = Obj
End Function

Private Sub SlaveCheck_Click()
    Profile.WriteValue DOMEID, "Slave", SlaveCheck.Value
    If SlaveCheck.Value And ScopeIsConnected And DomeIsConnected Then
       
        ' Start immediate dome slew
        CalcDomeAltAzWithFlipping
        
        On Error Resume Next
        TheDome.SlewToAzimuth TargetAz
        If DomeCanSetAltitude Then TheDome.SlewToAltitude TargetAlt
        
    End If
End Sub

' Send alt/az commands to the dome and update the screen display
Public Sub UpdateStatus()
    Dim Str As String
    On Error Resume Next
    
    ' Get all dome status
    If DomeCanSetAltitude Then DomeAltitude = TheDome.Altitude
    DomeAzimuth = TheDome.Azimuth
    DomeAtHome = TheDome.AtHome
    DomeAtPark = TheDome.AtPark
    DomeShutterStatus = TheDome.ShutterStatus
    DomeSlewing = TheDome.Slewing
   
    ' Check whether we have finished a home operation
    If DomeIsConnected And DomeAtHome Then
        If Homing And UseHomeAzimuth And DomeCanSyncAzimuth Then
            TheDome.SyncToAzimuth frmSetup.HomeAzimuth.Text
        End If
        Homing = False
        Homed = True
        FoundHomeOnce = True
    End If
    
    ' Check whether we have finished a park operation; turn off status indicator
    If Parking And DomeIsConnected And Not DomeSlewing Then
        Parking = False
    End If
    
    ' Clear the dome status string
    DomeStatus = ""
    
    ' Display the dome status
    Dim Temp As String
    Temp = Profile.GetValue(DOMEID, "Dome")
    DomeStatus = DomeStatus + "Dome:" + vbTab
    If DomeIsConnected Then
        DomeStatus = DomeStatus + "Connected"
    ElseIf Len(Temp) = 0 Then
        DomeStatus = DomeStatus + "None Selected"
    Else
        DomeStatus = DomeStatus + "Not Connected" + vbCrLf + vbTab + "(" + CrossRef(Temp) + ")"
    End If
    
    ' Display connected/unconnected and telescope driver name
    If Not ScopeIsConnected Then DomeStatus = DomeStatus + vbCrLf
    DomeStatus = DomeStatus + vbCrLf + "Scope: " + vbTab
    Temp = Profile.GetValue(DOMEID, "Scope")
    If ScopeIsConnected Then
        DomeStatus = DomeStatus + "Connected"
    ElseIf Len(Temp) = 0 Then
        DomeStatus = DomeStatus + "None Selected"
    Else
        DomeStatus = DomeStatus + "Not Connected" + vbCrLf + vbTab + "(" + CrossRef(Temp) + ")"
    End If
    
    ' Display current universal time
    
    If ScopeIsConnected Then
        ' Display current telescope position
        Dim DecStr As String
        Dim RAStr As String
        DecStr = Util.DegreesToDMS(ScopeDec)
        RAStr = Util.DegreesToHMS(ScopeRA * 15)
        DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Scope RA" + vbTab + RAStr
        DomeStatus = DomeStatus + vbCrLf + "Scope Dec" + vbTab + DecStr
       
        ' Display current telescope alt/az
        Str = Util.DegreesToDM(ScopeAlt)
        DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Scope Altitude" + vbTab + Str
        Str = Util.DegreesToDM(ScopeAz)
        DomeStatus = DomeStatus + vbCrLf + "Scope Azimuth" + vbTab + Str
        
        ' Display target dome azimuth
        If DomeCanSetAltitude Then
            Str = Util.DegreesToDM(TargetAlt)
        Else
            Str = "N/A"
        End If
        DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Target Altitude" + vbTab + Str
        Str = Util.DegreesToDM(TargetAz)
        DomeStatus = DomeStatus + vbCrLf + "Target Azimuth" + vbTab + Str
    Else
        DomeStatus = DomeStatus + vbCrLf + vbCrLf
    End If
    
    DomeStatus = DomeStatus + vbCrLf
    
    ' Display current dome azimuth
    If DomeIsConnected Then
        If DomeCanSetAltitude Then
            Str = Util.DegreesToDM(DomeAltitude)
        Else
            Str = "N/A"
        End If
        DomeStatus = DomeStatus + vbCrLf + "Current Altitude" + vbTab + Str
        Str = Util.DegreesToDM(DomeAzimuth)
        DomeStatus = DomeStatus + vbCrLf + "Current Azimuth" + vbTab + Str
    End If
    
    If DomeIsConnected Then
        If Homing Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is homing"
        ElseIf Parking Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is parking"
        ElseIf DomeSlewing Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is in motion"
        ElseIf DomeAtHome Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is at home"
        ElseIf DomeAtPark Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is parked"
        ElseIf Homed Then
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome is stationary"
        Else
            DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Dome stationary, has not been homed"
        End If
        
        If DomeCanSetShutter Then
            Select Case DomeShutterStatus
            Case shutterOpen
                DomeStatus = DomeStatus + vbCrLf + "Shutter is open"
            Case shutterClosed
                DomeStatus = DomeStatus + vbCrLf + "Shutter is closed"
            Case shutterOpening
                DomeStatus = DomeStatus + vbCrLf + "Shutter is opening"
            Case shutterClosing
                DomeStatus = DomeStatus + vbCrLf + "Shutter is closing"
            Case Error
                DomeStatus = DomeStatus + vbCrLf + "Shutter error"
            End Select
        Else
            DomeStatus = DomeStatus + vbCrLf + "No shutter control"
        End If
    End If
    
    If DomeIsConnected And ScopeIsConnected Then
        Dim TempSt As String
        TempSt = 9 - PollCount
        DomeStatus = DomeStatus + vbCrLf + vbCrLf + "Position Update in" + vbTab + TempSt + "  sec."
    End If
    
    frmMain.txtStatus = DomeStatus
    
End Sub

' Called whenever the telescope position is changed
Public Sub UpdateTelescopeStatus(ByVal RA As Double, ByVal dec As Double, ByVal IsGoTo As Boolean)
    ScopeRA = RA
    ScopeDec = dec
    GotRADec = True
    
    ' If it was a goto command, then start the dome rotating immediately; otherwise
    ' we wait for the next 10 second update (prevents jerky dome rotation if telescope
    ' is manually slewed)
    If IsGoTo Then
        ' Calculate new dome alt/az
        CalcDomeAltAzWithFlipping
    
        ' Restart poll timer
        PollCount = 0
        
        ' Move the dome
        MoveDome
    
        ' Update the screen display
        UpdateStatus
    End If
End Sub

' Called once a second to update display and to start dome alt/az slews
Private Sub UpdateDomeStatus()
    On Error Resume Next
    
    ' Update the position if we haven't already done so recently
    If ScopeIsConnected Then
        If Not IsSlewing And Not GotRADec Then
            ScopeRA = TheScope.RightAscension
            ScopeDec = TheScope.Declination
        End If
    End If
    GotRADec = False
    
    ' Update the dome target position
    CalcDomeAltAzWithFlipping
    
    ' Do the display update and GOTO if necessary
    UpdateStatus
    
End Sub

Private Sub MoveDome()
    ' Move the dome if needed, and if slaving enabled, and if both scope and dome connected
    If SlaveCheck.Value And ScopeIsConnected And DomeIsConnected Then
        If (Abs(TargetAz - DomeAzimuth) > 1) And Not DomeSlewing Then
            TheDome.SlewToAzimuth TargetAz
            If DomeCanSetAltitude Then TheDome.SlewToAltitude TargetAlt
        End If
    End If
End Sub

' Update screen display and follow non-GOTO telescope movement (sidereal tracking or GOTO)
Private Sub Timer1_Timer()

    ' Every 10 seconds, update scope position (n.b. will be reset to zero if GOTO occurs)
    PollCount = PollCount + 1
    If PollCount >= 10 Then
        PollCount = 0
        MoveDome
    End If
    
    ' Every second update status display
    UpdateDomeStatus
    
End Sub

Private Sub CalcDomeAltAzWithFlipping()
    Dim IsV2 As Boolean
    Dim SideOfPier As PierSide
    IsV2 = False
    SideOfPier = pierWest
    
    On Error GoTo JustDoIt
    If ScopeIsConnected Then
        If TheScope.InterfaceVersion > 1 Then
            SideOfPier = TheScope.SideOfPier
            IsV2 = True
        End If
    End If
    
JustDoIt:
    CalcDomeAltAz IsV2, SideOfPier
End Sub

