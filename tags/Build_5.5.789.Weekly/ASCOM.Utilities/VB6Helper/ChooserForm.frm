VERSION 5.00
Begin VB.Form ChooserForm 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ASCOM <runtime> Chooser"
   ClientHeight    =   2535
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4920
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2535
   ScaleWidth      =   4920
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   210
      MouseIcon       =   "ChooserForm.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "ChooserForm.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   5
      Top             =   1500
      Width           =   720
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   3525
      TabIndex        =   4
      Top             =   1995
      Width           =   1185
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Enabled         =   0   'False
      Height          =   345
      Left            =   3525
      TabIndex        =   3
      Top             =   1500
      Width           =   1185
   End
   Begin VB.CommandButton cmdProperties 
      Caption         =   "&Properties..."
      Enabled         =   0   'False
      Height          =   345
      Left            =   3525
      TabIndex        =   1
      Top             =   705
      Width           =   1185
   End
   Begin VB.ComboBox cbDriverSelector 
      Height          =   315
      ItemData        =   "ChooserForm.frx":1016
      Left            =   210
      List            =   "ChooserForm.frx":1018
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   720
      Width           =   3135
   End
   Begin VB.Label Label1 
      Caption         =   "Click the logo to learn more about ASCOM, a set of standards for inter-operation of astronomy software."
      Height          =   810
      Left            =   1095
      TabIndex        =   6
      Top             =   1530
      Width           =   2040
   End
   Begin VB.Line Line1 
      X1              =   195
      X2              =   4695
      Y1              =   1275
      Y2              =   1275
   End
   Begin VB.Label lblTitle 
      Caption         =   "<runtime>"
      Height          =   465
      Left            =   210
      TabIndex        =   2
      Top             =   165
      Width           =   4590
   End
End
Attribute VB_Name = "ChooserForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
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
'   ==============
'   CHOOSERFRM.FRM
'   ==============
'
' Implementation of the ASCOM telescope driver Chooser form.
'
' Written:  24-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Aug-00 rbd     Initial edit
' 07-Jun-01 rbd     Generalize Chooser for various devices
' 06-Nov-01 rbd     Sort list of telescopes
' 07-Dec-01 rbd     Fix "no drivers" message
' 26-Jul-02 rbd     ASCOM logo and hotlink
' 16-Feb-03 rbd     2.1.3 - Don't open SetupDialog on connected drivers.
' 09-Mar-03 rbd     2.2.0 - Fix ASCOM logo - it never worked!
' 17-Mar-03 rbd     2.2.1 - Remember whether driver has ever been
'                   initalized and if so, light OK. Don't force
'                   Properties in this case.
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 01-Jan-07 rbd     5.0.1 - Version change for Platform 5.0
'---------------------------------------------------------------------
Option Explicit

Private Const ALERT_TITLE As String = "ASCOM Chooser"

Private m_sDeviceType As String
Private m_sResult As String
Private m_sStartSel As String
Private m_sProgID() As String

Private Const SWP_NOSIZE            As Long = &H1
Private Const SWP_NOMOVE            As Long = &H2
Private Const SWP_NOZORDER          As Long = &H4
Private Const SWP_NOREDRAW          As Long = &H8
Private Const SWP_NOACTIVATE        As Long = &H10
Private Const SWP_FRAMECHANGED      As Long = &H20
Private Const SWP_SHOWWINDOW        As Long = &H40
Private Const SWP_HIDEWINDOW        As Long = &H80
Private Const SWP_NOCOPYBITS        As Long = &H100
Private Const SWP_NOOWNERZORDER     As Long = &H200
Private Const SWP_NOSENDCHANGING    As Long = &H400

Private Const SWP_DRAWFRAME         As Long = SWP_FRAMECHANGED
Private Const SWP_NOREPOSITION      As Long = SWP_NOOWNERZORDER

Private Const HWND_TOP              As Long = 0
Private Const HWND_BOTTOM           As Long = 1
Private Const HWND_TOPMOST          As Long = -1
Private Const HWND_NOTOPMOST        As Long = -2

Private Const SW_SHOWNORMAL         As Long = 1

Private Declare Function SetWindowPos Lib "user32.dll" ( _
                ByVal hWnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal Y As Long, _
                ByVal cx As Long, _
                ByVal cy As Long, _
                ByVal uFLags As Long) As Long

Private Declare Function ShellExecute Lib "shell32" Alias "ShellExecuteA" ( _
                ByVal hWnd As Long, _
                ByVal lpOperation As String, _
                ByVal lpFile As String, _
                ByVal lpParameters As String, _
                ByVal lpDirectory As String, _
                ByVal nShowCmd As Long) As Long

Private Reg As Object

Private Sub Form_Load()
    Dim i As Long, iSel As Long
    Dim d As Dictionary
    Dim cb As ComboBox
    
    Set Reg = CreateObject("DriverHelper.ProfileAccess")
    '
    ' Enumerate the available ASCOM scope drivers, and
    ' load their descriptions and ProgIDs into the
    ' list box. Key is ProgID, value is friendly name.
    '
    Set d = Reg.EnumKeys(m_sDeviceType & " Drivers", ERR_SOURCE_CHOOSER)    ' Get Key-Class pairs
    Set cb = Me.cbDriverSelector                 ' Handy shortcut
    If d.Count = 0 Then
        MsgBox "There are no ASCOM " & m_sDeviceType & " drivers installed.", _
                (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), _
                ALERT_TITLE
    Else
        ReDim m_sProgID(0 To (d.Count - 1)) As String
        For i = 0 To d.Count - 1
            m_sProgID(i) = d.Keys(i)            ' Remember the ProgIDs for later
            cb.AddItem d.Items(i)               ' Add items & allow to sort
            cb.ItemData(cb.NewIndex) = i        ' Index in m_sProgID() of ProgID for this list entry
        Next
    End If
    
    Me.Caption = "ASCOM " & m_sDeviceType & " Chooser"
    Me.lblTitle.Caption = "Select the type of " & LCase$(m_sDeviceType) & " you have, then be " & _
                        "sure to click the Properties... button to configure the driver for your " & _
                        LCase$(m_sDeviceType) & "."
    m_sResult = ""
    cbDriverSelector_Click                       ' Also dims OK
    
    '
    ' Now items in list are sorted. Preselect.
    '
    iSel = -1                                   ' Start selected index
    For i = 0 To cb.ListCount - 1
        If LCase$(m_sProgID(cb.ItemData(i))) = LCase$(m_sStartSel) Then
            iSel = i
        End If
    Next
    If iSel >= 0 Then                           ' Start selection?
        cb.ListIndex = iSel                     ' Jump list to that
        Me.cmdOK.Enabled = True                 ' Allow OK, probably already conf'd
    End If
    
    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)
    
End Sub

Public Property Let DeviceType(newVal As String)

    m_sDeviceType = newVal
    
End Property

Public Property Let StartSel(newVal As String)

    m_sStartSel = newVal
    
End Property
'
' Isolate this form from the rest of the component
' Return of "" indicates error or Cancel clicked
'
Public Property Get Result() As String
    
    Result = m_sResult
    
End Property
'
' Click in Properties... button. Load the currently selected
' driver and activate its setup dialog.
'
Private Sub cmdProperties_Click()
    Dim oDrv As Object                      ' The driver
    Dim cb As ComboBox
    Dim bConnected As Boolean
    Dim sProgID As String
    
    Set cb = Me.cbDriverSelector            ' Convenient shortcut
    sProgID = m_sProgID(cbDriverSelector.ItemData(cbDriverSelector.ListIndex))
    On Error GoTo LOAD_FAIL
    Set oDrv = CreateObject(sProgID)
    On Error GoTo SETUP_FAIL

    '
    ' Here we try to see if a device is already connected. If so, alert
    ' and just turn on the OK button.
    '
    bConnected = False
    On Error Resume Next
    bConnected = oDrv.Connected
    bConnected = oDrv.Link
    On Error GoTo 0
    If bConnected Then
        MsgBox "The device is already connected. Just click OK.", _
            (vbOKOnly + vbInformation + vbMsgBoxSetForeground), ALERT_TITLE
    Else
        SetWindowPos Me.hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)
        oDrv.SetupDialog
        SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)
    End If
    
    Reg.WriteProfile "Chooser", sProgID & " Init", "True", ERR_SOURCE_CHOOSER   ' Remember it has been initialized
    
    Me.cmdOK.Enabled = True
    Exit Sub

LOAD_FAIL:
    MsgBox "Failed to load driver: " & Err.Description, _
            (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ALERT_TITLE
    Exit Sub
    
SETUP_FAIL:
    MsgBox "Driver setup method failed: " & Err.Description, _
            (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ALERT_TITLE
    Exit Sub
End Sub

Private Sub cmdCancel_Click()

    m_sResult = ""
    Me.Hide
    
End Sub

Private Sub cmdOK_Click()
    Dim cb As ComboBox
    
    Set cb = Me.cbDriverSelector                    ' Convenient shortcut
    m_sResult = m_sProgID(cbDriverSelector.ItemData(cbDriverSelector.ListIndex))
    Me.Hide

End Sub

Private Sub cbDriverSelector_Click()
    Dim buf As String
    
    If Me.cbDriverSelector.ListIndex >= 0 Then      ' Something selected
        Me.cmdProperties.Enabled = True             ' Turn on Properties
        buf = Reg.GetProfile("Chooser", m_sProgID(cbDriverSelector.ItemData(cbDriverSelector.ListIndex)) & " Init", _
                        ERR_SOURCE_CHOOSER)
        If LCase$(buf) = "true" Then
            Me.cmdOK.Enabled = True                 ' This device has been initialized
        Else
            Me.cmdOK.Enabled = False                ' Never been initialized
        End If
    Else
        Me.cmdProperties.Enabled = False
        Me.cmdOK.Enabled = False
    End If
    
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
Set Reg = Nothing
End Sub

Private Sub picASCOM_Click()
    Dim z As Long

    z = ShellExecute(0, "Open", "http://ASCOM-Standards.org/", 0, 0, SW_SHOWNORMAL)
    If (z > 0) And (z <= 32) Then
        MsgBox _
            "It doesn't appear that you have a web browser installed " & _
            "on your system.", (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ALERT_TITLE
        Exit Sub
    End If

End Sub
