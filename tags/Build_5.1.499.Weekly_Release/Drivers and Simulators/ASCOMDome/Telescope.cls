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
Attribute VB_Description = "ASCOM Dome Control Telescope Hub"
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
'   =============
'   Telescope.cls
'   =============
'
' Written:  2003/06/24   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/06/24 dbg     Initial edit
' 2003/09/03 rbd     Version 3.0.1 for Platform 3.0.1, add Binary compat.
' 2004/01/16 dbg     Added code to check for dome connected in Slewing property
' 2005/02/14 dbg     Added Telescope V2 support
' -----------------------------------------------------------------------------

' This module operates as a multiplexing ASCOM telescope hub

Option Explicit

Public Enum AlignmentModes
    algUnknown = -1
    algAltAz = 0
    algPolar = 1
    algGermanPolar = 2
End Enum

Public Enum GuideDirections
    guideNorth = 0
    guideSouth = 1
    guideEast = 2
    guideWest = 3
End Enum


Public Enum DriveRates
    driveSidereal = 0
    driveLunar = 1
    driveSolar = 2
    driveKing = 3
End Enum

Public Enum EquatorialCoordinateType
    equOther = 0
    equLocalTopocentric = 1
    equJ2000 = 2
    equJ2050 = 3
    equB1950 = 4
End Enum

Public Enum PierSide
    pierEast = 0
    pierWest = 1
End Enum

Public Enum TelescopeAxes
    axisPrimary = 0
    axisSecondary = 1
    axisTertiary = 2
End Enum

Private CurrentRA As Double
Private CurrentDec As Double
Private ThisObjectConnected As Boolean

Private Sub Class_Initialize()

'    Profile.DeviceType = "Telescope"          ' Don't register here
'    Profile.Register ID, DESC
    
    frmMain.UpdateStatus
    
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Private Sub Class_Terminate()
    
    On Error Resume Next
    
    If ThisObjectConnected Then
        Connected = False
        ScopeConnectedCount = ScopeConnectedCount - 1
        If ScopeConnectedCount <= 0 Then
            ScopeConnectedCount = 0 ' Paranoia
            Set TheScope = Nothing
        End If
    End If
    frmMain.UpdateStatus
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Property Get AlignmentMode() As AlignmentModes
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    AlignmentMode = TheScope.AlignmentMode
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Altitude() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    Altitude = TheScope.Altitude
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get ApertureDiameter() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ApertureDiameter = TheScope.ApertureDiameter

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Azimuth() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    Azimuth = TheScope.Azimuth

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanFindHome() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanFindHome = TheScope.CanFindHome
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanPark() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanPark = TheScope.CanPark

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSetPark() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetPark = TheScope.CanSetPark

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSetTracking() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetTracking = TheScope.CanSetTracking

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSlew() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSlew = TheScope.CanSlew

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSlewAsync() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSlewAsync = TheScope.CanSlewAsync

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSync() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSync = TheScope.CanSync

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanSyncAltAz() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSyncAltAz = TheScope.CanSyncAltAz

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get CanUnpark() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanUnpark = TheScope.CanUnpark

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanPulseGuide() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanPulseGuide = TheScope.CanPulseGuide

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    On Error GoTo ErrorMsg
    If newVal Then
        ' If someone else has already connected, just increment a reference count
        If ScopeIsConnected Then
            ScopeConnectedCount = ScopeConnectedCount + 1
            ThisObjectConnected = True
            GoTo Done
        End If
        
        ' Fetch the selected telescope
        Dim ScopeProgID As String
        ScopeProgID = Profile.GetValue(DOMEID, "Scope")
        If Len(ScopeProgID) = 0 Then
            Err.Raise SCODE_NOT_SELECTED, ERR_SOURCE, MSG_NOT_SELECTED
            GoTo Done
        End If
        
        ' Create the telescope object
        Set TheScope = CreateObject(ScopeProgID)
        TheScope.Connected = True
        ScopeIsConnected = True
        ThisObjectConnected = True
        ScopeConnectedCount = ScopeConnectedCount + 1
        
        IsSlewing = False
        
        ' Force a status update
        Dim Temp As Double
        Temp = RightAscension
        Temp = Declination
        
    ElseIf ScopeIsConnected Then
        ' Check reference count and disconnect only if no applications connected
        ScopeConnectedCount = ScopeConnectedCount - 1
        ThisObjectConnected = False
        
        ' If last connection, disconnect the telescope
        If ScopeConnectedCount <= 0 Then
            ScopeConnectedCount = 0 ' Paranoid
            TheScope.Connected = False
            ScopeIsConnected = False
            Set TheScope = Nothing
        End If
    End If

Done:
    frmMain.UpdateStatus
    Exit Property

ErrorMsg:
    frmMain.UpdateStatus
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get Connected() As Boolean
    Connected = ScopeIsConnected
    frmMain.UpdateStatus
End Property

Public Property Get Declination() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Dim Temp As Double
    Temp = TheScope.Declination
    Declination = Temp
    
    ' Update scope position for the dome only if we're not in mid-slew
    If Not IsSlewing Then
        CurrentDec = Temp
        frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, False
    End If
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get DeclinationRate() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    DeclinationRate = TheScope.DeclinationRate

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let DeclinationRate(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.DeclinationRate = newVal

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Description() As String
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    Description = TheScope.Description
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get DriverInfo() As String
    On Error GoTo ErrorMsg
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
        
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get FocalLength() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    FocalLength = TheScope.FocalLength

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Name() As String
    On Error GoTo ErrorMsg

    Name = "SirusDome"                        ' 1-word name
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get RightAscension() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    Dim Temp As Double
    Temp = TheScope.RightAscension
    RightAscension = Temp
    
    ' Update scope position for the dome only if we're not in mid-slew
    If Not IsSlewing Then
        CurrentRA = Temp
        frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, False
    End If
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get RightAscensionRate() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    RightAscensionRate = TheScope.RightAscensionRate
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let RightAscensionRate(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.RightAscensionRate = newVal
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get SiderealTime() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SiderealTime = TheScope.SiderealTime

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get SiteElevation() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SiteElevation = TheScope.SiteElevation

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let SiteElevation(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SiteElevation = newVal

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get SiteLatitude() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SiteLatitude = TheScope.SiteLatitude

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let SiteLatitude(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SiteLatitude = newVal

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get SiteLongitude() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SiteLongitude = TheScope.SiteLongitude
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let SiteLongitude(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SiteLongitude = newVal
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Slewing() As Boolean
    Dim DomeIsMoving As Boolean
    
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    If Not DomeIsConnected Then
        Slewing = TheScope.Slewing
        IsSlewing = Slewing
    End If
    

    ' Check if we had been slewing, but the dome is still moving
    DomeIsMoving = TheDome.Connected
    If DomeIsMoving Then
        DomeIsMoving = TheDome.Slewing
    End If
    If IsSlewing And DomeIsMoving Then
        Slewing = True
        Exit Property
    End If
    
    ' Get the status from the telescope
    Slewing = TheScope.Slewing
    
    ' Indicate to frmMain that the telescope is no longer slewing, so it
    ' can start polling for position again
    IsSlewing = Slewing
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get SlewSettleTime() As Integer
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SlewSettleTime = TheScope.SlewSettleTime
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let SlewSettleTime(ByVal newVal As Integer)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SlewSettleTime = newVal
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get TargetDeclination() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TargetDeclination = TheScope.TargetDeclination
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let TargetDeclination(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.TargetDeclination = newVal
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get TargetRightAscension() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TargetRightAscension = TheScope.TargetRightAscension
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let TargetRightAscension(ByVal newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.TargetRightAscension = newVal
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get Tracking() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    Tracking = TheScope.Tracking
    
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let Tracking(ByVal newVal As Boolean)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.Tracking = newVal

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Get UTCDate() As Date
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    UTCDate = TheScope.UTCDate

    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Property Let UTCDate(ByVal newVal As Date)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.UTCDate = newVal
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property


Public Sub AbortSlew()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ' We should not wait for the dome under these conditions
    IsSlewing = False
    
    TheScope.AbortSlew
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Sub CommandBlind(ByVal Command As String)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.CommandBlind Command
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Function CommandBool(ByVal Command As String) As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CommandBool = TheScope.CommandBool(Command)
    Exit Function

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Function

Public Function CommandString(ByVal Command As String) As String
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CommandString = TheScope.CommandString(Command)
    Exit Function

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Function


Public Sub FindHome()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.FindHome
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Sub


Public Sub Park()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.Park
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub

End Sub


Public Sub PulseGuide(ByVal Direction As GuideDirections, _
                                            ByVal Duration As Long)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.PulseGuide Direction, Duration
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Sub SetPark()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SetPark
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext

End Sub


Public Sub SetupDialog()
    MsgBox "Please set up telescope and dome configuration using the ASCOM Dome Control Panel directly.", vbOKOnly + vbSystemModal, "ASCOM Dome"
    ' Do nothing
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ' Rotate the dome to the target position
    IsSlewing = True
    CurrentRA = RightAscension
    CurrentDec = Declination
    frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, True
    
    ' Slew the telescope
    TheScope.SlewToCoordinates RightAscension, Declination
    
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    ' Rotate the dome to the target position
    IsSlewing = True
    CurrentRA = RightAscension
    CurrentDec = Declination
    frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, True
    
    TheScope.SlewToCoordinatesAsync RightAscension, Declination
    
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Sub SlewToTarget()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ' Rotate the dome to the target position
    IsSlewing = True
    CurrentRA = TheScope.TargetRightAscension
    CurrentDec = TheScope.TargetDeclination
    frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, True
    
    TheScope.SlewToTarget
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub


Public Sub SlewToTargetAsync()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ' Rotate the dome to the target position
    IsSlewing = True
    CurrentRA = TheScope.TargetRightAscension
    CurrentDec = TheScope.TargetDeclination
    frmMain.UpdateTelescopeStatus CurrentRA, CurrentDec, True
    
    TheScope.SlewToTargetAsync
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SyncToCoordinates RightAscension, Declination
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SyncToTarget()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SyncToTarget
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub Unpark()
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.Unpark
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

' V2
Public Property Get ApertureArea() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ApertureArea = TheScope.ApertureArea
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get AtHome() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    AtHome = TheScope.AtHome
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get AtPark() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    AtPark = TheScope.AtPark
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSetDeclinationRate() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetDeclinationRate = TheScope.CanSetDeclinationRate
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSetGuideRates() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetGuideRates = TheScope.CanSetGuideRates
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSetPierSide() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetPierSide = TheScope.CanSetPierSide
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSetRightAscensionRate() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSetRightAscensionRate = TheScope.CanSetRightAscensionRate
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSlewAltAz() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSlewAltAz = TheScope.CanSlewAltAz
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get CanSlewAltAzAsync() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanSlewAltAzAsync = TheScope.CanSlewAltAzAsync
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get DoesRefraction() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    DoesRefraction = TheScope.DoesRefraction
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Let DoesRefraction(newVal As Boolean)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.DoesRefraction (newVal)
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Property Get DriverVersion() As String
    
    DriverVersion = App.Major & "." & App.Minor
        
End Property

' V2
Public Property Get EquatorialSystem() As EquatorialCoordinateType
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    EquatorialSystem = TheScope.EquatorialSystem
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get GuideRateDeclination() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    GuideRateDeclination = TheScope.GuideRateDeclination
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Let GuideRateDeclination(newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.GuideRateDeclination (newVal)
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Property Get GuideRateRightAscension() As Double
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    GuideRateRightAscension = TheScope.GuideRateRightAscension
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Let GuideRateRightAscension(newVal As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.GuideRateRightAscension (newVal)
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Property Get InterfaceVersion() As Integer
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    ' Reports back the version from the actual driver, not the dome control panel
    InterfaceVersion = TheScope.InterfaceVersion
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get IsPulseGuiding() As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    IsPulseGuiding = TheScope.IsPulseGuiding
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Get SideOfPier() As PierSide
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    SideOfPier = TheScope.SideOfPier
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Let SideOfPier(newVal As PierSide)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SideOfPier (newVal)
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Property Get TrackingRate() As DriveRates
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TrackingRate = TheScope.TrackingRate
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Property

' V2
Public Property Let TrackingRate(ByVal newVal As DriveRates)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.TrackingRate = TrackingRate
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Property Get TrackingRates() As Object
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TrackingRates = TheScope.TrackingRates
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Property
End Property

' V2
Public Function AxisRates(ByVal Axis As TelescopeAxes) As Object
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    AxisRates = TheScope.AxisRates(Axis)
    Exit Function

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Function
End Function

' V2
Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    CanMoveAxis = TheScope.CanMoveAxis(Axis)
    Exit Function

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Function
End Function

' V2
Public Function DestinationSideOfPier(ByVal RightAscension As Double, _
                             ByVal Declination As Double) As PierSide
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    DestinationSideOfPier = TheScope.DestinationSideOfPier(RightAscension, Declination)
    Exit Function

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Function
End Function

' V2
Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.MoveAxis Axis, Rate
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Sub

' V2
Public Sub SlewToAltAz(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SlewToAltAz Azimuth, Altitude
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Sub

' V2
Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SlewToAltAzAsync Azimuth, Altitude
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Sub

' V2
Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    If Not ScopeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg

    TheScope.SyncToAltAz Azimuth, Altitude
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
    Exit Sub
End Sub


