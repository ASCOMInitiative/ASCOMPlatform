VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Dome"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Dome Control Dome Hub"
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
'   Dome.cls
'   =============
'
' Written:  2003/07/25   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/07/25 dbg     Initial edit
' 2003/09/03 rbd     Version 3.0.1 for Platform 3.0.1, add Binary compat,
'                    ShutterState enum defined here instead of by ref
'                    from DomeSim.
' -----------------------------------------------------------------------------

' This module operates as a multiplexing ASCOM Dome hub

Option Explicit

Public Enum ShutterState
    shutterOpen = 0
    shutterClosed = 1
    shutterOpening = 2
    shutterClosing = 3
    shutterError = 4
End Enum

Private ThisObjectConnected As Boolean

Private Sub Class_Terminate()
    On Error Resume Next
    
    If ThisObjectConnected Then
        ThisObjectConnected = False
        DomeConnectedCount = DomeConnectedCount - 1
        If DomeConnectedCount <= 0 Then
            DomeConnectedCount = 0 ' Paranoid
            frmMain.DisconnectButton_Click
        End If
    End If
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub AbortSlew()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.AbortSlew
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Property Get Altitude() As Double
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Altitude = TheDome.Altitude
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get AtHome() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    AtHome = TheDome.AtHome
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get AtPark() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    AtPark = TheDome.AtPark
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get Azimuth() As Double
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Azimuth = TheDome.Azimuth
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanFindHome() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanFindHome = TheDome.CanFindHome
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanPark() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanPark = TheDome.CanPark
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanSetAltitude() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanSetAltitude = TheDome.CanSetAltitude
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanSetAzimuth() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanSetAzimuth = TheDome.CanSetAzimuith
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanSetPark() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanSetPark = TheDome.CanSetPark
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanSetShutter() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanSetShutter = TheDome.CanSetShutter
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get CanSyncAzimuth() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    CanSyncAzimuth = TheDome.CanSyncAzimuth
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Sub CloseShutter()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.CloseShutter
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Property Get Connected() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Connected = DomeIsConnected
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Let Connected(ByVal RHS As Boolean)
    On Error GoTo ErrorMsg
    If RHS Then
        ' If someone else has already connected, just increment a reference count
        If DomeIsConnected Then
            DomeConnectedCount = DomeConnectedCount + 1
            ThisObjectConnected = True
            frmMain.UpdateStatus
            Exit Property
        End If
        
        ' Ask the main form to open it
        frmMain.ConnectButton_Click
        
        ' Increment connected count (should have been zero)
        DomeConnectedCount = DomeConnectedCount + 1
        ThisObjectConnected = True
        
    ElseIf DomeIsConnected Then
        ' Check reference count and disconnect only if no applications connected
        DomeConnectedCount = DomeConnectedCount - 1
        ThisObjectConnected = False
        
        ' If last connection, disconnect the Dome
        If DomeConnectedCount <= 0 Then
            DomeConnectedCount = 0 ' Paranoid
            frmMain.DisconnectButton_Click
        End If
    End If

    frmMain.UpdateStatus
    Exit Property

ErrorMsg:
    frmMain.UpdateStatus
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get Description() As String
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Description = TheDome.Description
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get DriverInfo() As String
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    DriverInfo = TheDome.DriverInfo
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Sub FindHome()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.FindHome
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Property Get InterfaceVersion() As Integer
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    InterfaceVersion = TheDome.InterfaceVersion
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get Name() As String
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Name = TheDome.Name
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Sub OpenShutter()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.OpenShutter
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub Park()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.Park
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SetPark()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.SetPark
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SetupDialog()
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.SetupDialog
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Property Get ShutterStatus() As ShutterState
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    ShutterStatus = TheDome.ShutterStatus
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Get Slewing() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Slewing = TheDome.Slewing
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Sub SlewToAltitude(Alt As Double)
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.SlewToAltitude Alt
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SlewToAzimuth(Az As Double)
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.SlewToAzimuth Az
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub

Public Sub SyncToAzimuth(Az As Double)
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    TheDome.SyncToAzimuth Az
    Exit Sub

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Sub



Public Property Get Slaved() As Boolean
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    Slaved = frmMain.SlaveCheck.Value
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

Public Property Let Slaved(ByVal RHS As Boolean)
    If Not DomeIsConnected Then Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    On Error GoTo ErrorMsg
    
    If RHS Then
        If Not frmMain.SlaveCheck.Enabled Then
            Err.Raise SCODE_SLAVE_NOT_POSSIBLE, ERR_SOURCE, MSG_SLAVE_NOT_OPSSIBLE
        End If
        frmMain.SlaveCheck.Value = 1
    Else
        frmMain.SlaveCheck.Value = 0
    End If
    Exit Property

ErrorMsg:
    Err.Raise Err.Number, Err.Source, Err.Description, Err.HelpFile, Err.HelpContext
End Property

