VERSION 5.00
Begin VB.Form frmGoTo 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Go To"
   ClientHeight    =   1080
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   3225
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1080
   ScaleWidth      =   3225
   ShowInTaskbar   =   0   'False
   Begin VB.TextBox txtAltitude 
      Height          =   285
      Left            =   870
      TabIndex        =   5
      Text            =   "Text1"
      Top             =   180
      Width           =   825
   End
   Begin VB.TextBox txtAzimuth 
      Height          =   285
      Left            =   870
      TabIndex        =   2
      Text            =   "Text1"
      Top             =   630
      Width           =   825
   End
   Begin VB.CommandButton CancelButton 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   1905
      TabIndex        =   1
      Top             =   600
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   1905
      TabIndex        =   0
      Top             =   120
      Width           =   1215
   End
   Begin VB.Label Label2 
      Caption         =   "Altitude"
      Height          =   255
      Left            =   150
      TabIndex        =   4
      Top             =   210
      Width           =   690
   End
   Begin VB.Label Label1 
      Caption         =   "Azimuth"
      Height          =   255
      Left            =   150
      TabIndex        =   3
      Top             =   660
      Width           =   690
   End
End
Attribute VB_Name = "frmGoTo"
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
' -----------------------------------------------------------------------------

Option Explicit

Public Azimuth As Double
Public Altitude As Double
Public AltitudeEnabled As Boolean
Public Ok As Boolean

Private Sub CancelButton_Click()
    Ok = False
    Me.Hide
End Sub

Private Sub Form_Load()
    Dim Temp
    
    Temp = Profile.GetValue(DOMEID, "GoToAltitude")
    Altitude = val(Temp)
    If Altitude > 90 Then Altitude = 90
    If Altitude < 0 Then Altitude = 0
    txtAltitude = Str(Altitude)
    
    Temp = Profile.GetValue(DOMEID, "GoToAltitude")
    Azimuth = val(Temp)
    If Azimuth > 359.999 Then Azimuth = 359.999
    If Azimuth < 0 Then Azimuth = 0
    txtAzimuth = Str(Azimuth)
    
    txtAltitude.Enabled = AltitudeEnabled
End Sub

Private Sub OKButton_Click()
    Azimuth = val(txtAzimuth.Text)
    Altitude = val(txtAltitude.Text)
    If (Azimuth >= 360) Or (Azimuth < 0) Or (AltitudeEnabled And Altitude < 0) Or (AltitudeEnabled And Altitude > 90) Then
        Beep
        Exit Sub
    End If
    Ok = True
    Profile.WriteValue DOMEID, "GoToAzimuth", Azimuth
    Profile.WriteValue DOMEID, "GoToAltitude", Altitude
    Me.Hide
End Sub
