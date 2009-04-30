VERSION 5.00
Begin VB.Form frmMain 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Migrate ASCOM Configuration"
   ClientHeight    =   1275
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5055
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1275
   ScaleWidth      =   5055
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdGo 
      Caption         =   "Click to Migrate to XML"
      Height          =   375
      Left            =   1462
      TabIndex        =   0
      Top             =   690
      Width           =   2130
   End
   Begin VB.Label lblProgress 
      Alignment       =   2  'Center
      Caption         =   "Make sure the new Helper is registered!"
      Height          =   300
      Left            =   135
      TabIndex        =   1
      Top             =   210
      Width           =   4785
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const SRC As String = "ASCOM Config Migration Tool"

Private Sub cmdGo_Click()
    Dim H As DriverHelper.Profile
    Dim RootKeys As Dictionary
    Dim DriverKeys As Dictionary
    Dim DriverSubKeys As Dictionary
    Dim DriverValues As Dictionary
    Dim i As Integer, j As Integer, k As Integer, l As Integer, z As Integer
    Dim buf As String, drvrID As String, drvrKey As String
    Dim name As String, val As String, subKey As String
    
    Set H = New DriverHelper.Profile
    If H.IsRegistered("ScopeSim.Telescope") Then
        MsgBox "It looks like there is already data in the XML config file!", _
            (vbOKOnly + vbExclamation), App.Title
        Set H = Nothing
        Exit Sub
    End If
    
    Screen.MousePointer = vbHourglass
    Me.cmdGo.Enabled = False
    
    buf = GetProfile("", "PlatformVersion", SRC)        ' Special Case
    If buf <> "" Then H.WriteValue "#root#", "PlatformVersion", buf
    
    Set RootKeys = EnumKeys("", SRC)
    DoEvents
    '
    ' Driver CLASS loop (class = Dome, Telescope, etc.)
    '
    For i = 0 To RootKeys.Count - 1
        z = InStr(RootKeys.Keys(i), " Drivers")         ' Look for driver class root
        If z > 0 Then                                   ' If it is one
            H.DeviceType = Left$(RootKeys.Keys(i), z - 1) ' Strip " Drivers" for new class name
            buf = RootKeys.Keys(i)                      ' Can't pass directly, type mismatch
            Set DriverKeys = EnumKeys(buf, SRC)         ' Drivers in this class
            '
            ' Driver IN class loop (Joe.Dome, Susie.Dome, etc.)
            '
            For j = 0 To DriverKeys.Count - 1
                drvrID = DriverKeys.Keys(j)             ' ProgID of driver
                H.Register drvrID, DriverKeys.Items(j)  ' Register it in XML
                drvrKey = RootKeys.Keys(i) & "\" & drvrID
                '
                ' Subkeys of this driver
                '
                Set DriverSubKeys = EnumKeys(drvrKey, SRC)
                DriverSubKeys.Add "", ""                ' Add fake "subkey" for driver root values
                '
                ' Settings for Driver loop - Note that the addition
                ' of the fake "subkey" above will permit this loop
                ' to migrate the settings directly under the driver's
                ' top level key.
                '
                For k = 0 To DriverSubKeys.Count - 1
                    buf = drvrKey
                    If DriverSubKeys.Keys(k) <> "" Then
                        buf = buf & "\" & DriverSubKeys.Keys(k)
                    End If
                    lblProgress.Caption = buf
                    Set DriverValues = EnumProfile(buf, SRC)
                    '
                    ' Values under driver root or subkey
                    '
                    For l = 0 To DriverValues.Count - 1
                        DoEvents
                        name = DriverValues.Keys(l)     ' Name of this value
                        val = DriverValues.Items(l)
                        subKey = DriverSubKeys.Keys(k)
                        '
                        ' Skip writing the unnamed value at the driver
                        ' level, this is prohibited. It's the driver's
                        ' display name for the chooser.
                        '
                        If subKey <> "" Or name <> "" Then
                            H.WriteValue drvrID, name, val, subKey
                        End If
                    Next
                Next
            Next
        End If
    Next
    
    Set H = Nothing
    lblProgress.Caption = "Migration Complete"
    Screen.MousePointer = vbDefault

End Sub
