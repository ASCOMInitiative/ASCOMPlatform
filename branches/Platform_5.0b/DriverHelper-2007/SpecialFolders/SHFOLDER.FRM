VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "SHFolder sample from BlackBeltVB.com"
   ClientHeight    =   3570
   ClientLeft      =   2415
   ClientTop       =   1545
   ClientWidth     =   8760
   LinkTopic       =   "Form1"
   ScaleHeight     =   3570
   ScaleWidth      =   8760
   Begin VB.ListBox lstFolders 
      Height          =   2595
      Left            =   60
      TabIndex        =   1
      Top             =   300
      Width           =   3555
   End
   Begin VB.Label lblPath 
      BorderStyle     =   1  'Fixed Single
      Height          =   315
      Left            =   60
      TabIndex        =   3
      Top             =   3180
      Width           =   8655
   End
   Begin VB.Label Label2 
      Caption         =   "Path"
      Height          =   195
      Left            =   60
      TabIndex        =   2
      Top             =   2940
      Width           =   1215
   End
   Begin VB.Label Label1 
      Caption         =   "&Folders"
      Height          =   195
      Left            =   120
      TabIndex        =   0
      Top             =   60
      Width           =   1215
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

' SHFolder sample from BlackBeltVB.com
' http://blackbeltvb.com
'
' Written by Matt Hart
' Copyright 2001 by Matt Hart
'
' This software is FREEWARE. You may use it as you see fit for
' your own projects but you may not re-sell the original or the
' source code. Do not copy this sample to a collection, such as
' a CD-ROM archive. You may link directly to the original sample
' using "http://blackbeltvb.com/free/shfolder.htm"
'
' No warranty express or implied, is given as to the use of this
' program. Use at your own risk.
'
' Shows how to use the SHGetSpecialFolderLocation API function

Private Type SHITEMID
    cb As Long
    abID As Byte
End Type
Private Type ITEMIDLIST
    mkid As SHITEMID
End Type
Private Declare Function LocalFree Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Function SHGetPathFromIDList Lib "shell32.dll" Alias "SHGetPathFromIDListA" (ByVal pidl As Long, ByVal pszPath As String) As Long
Private Declare Function SHGetSpecialFolderLocation Lib "shell32.dll" (ByVal hWndOwner As Long, ByVal nFolder As Long, pidl As Any) As Long
Const MAX_PATH = 260

Private Sub Form_Load()
    lstFolders.AddItem "CSIDL_DESKTOP": lstFolders.ItemData(lstFolders.NewIndex) = &H0                           ' <desktop>"
    lstFolders.AddItem "CSIDL_INTERNET": lstFolders.ItemData(lstFolders.NewIndex) = &H1                          ' Internet Explorer (icon on desktop)"
    lstFolders.AddItem "CSIDL_PROGRAMS": lstFolders.ItemData(lstFolders.NewIndex) = &H2                          ' Start Menu\Programs"
    lstFolders.AddItem "CSIDL_CONTROLS": lstFolders.ItemData(lstFolders.NewIndex) = &H3                          ' My Computer\Control Panel"
    lstFolders.AddItem "CSIDL_PRINTERS": lstFolders.ItemData(lstFolders.NewIndex) = &H4                          ' My Computer\Printers"
    lstFolders.AddItem "CSIDL_PERSONAL": lstFolders.ItemData(lstFolders.NewIndex) = &H5                          ' My Documents"
    lstFolders.AddItem "CSIDL_FAVORITES": lstFolders.ItemData(lstFolders.NewIndex) = &H6                         ' <user name>\Favorites"
    lstFolders.AddItem "CSIDL_STARTUP": lstFolders.ItemData(lstFolders.NewIndex) = &H7                           ' Start Menu\Programs\Startup"
    lstFolders.AddItem "CSIDL_RECENT": lstFolders.ItemData(lstFolders.NewIndex) = &H8                            ' <user name>\Recent"
    lstFolders.AddItem "CSIDL_SENDTO": lstFolders.ItemData(lstFolders.NewIndex) = &H9                            ' <user name>\SendTo"
    lstFolders.AddItem "CSIDL_BITBUCKET": lstFolders.ItemData(lstFolders.NewIndex) = &HA                         ' <desktop>\Recycle Bin"
    lstFolders.AddItem "CSIDL_STARTMENU": lstFolders.ItemData(lstFolders.NewIndex) = &HB                         ' <user name>\Start Menu"
    lstFolders.AddItem "CSIDL_DESKTOPDIRECTORY": lstFolders.ItemData(lstFolders.NewIndex) = &H10                 ' <user name>\Desktop"
    lstFolders.AddItem "CSIDL_DRIVES": lstFolders.ItemData(lstFolders.NewIndex) = &H11                           ' My Computer"
    lstFolders.AddItem "CSIDL_NETWORK": lstFolders.ItemData(lstFolders.NewIndex) = &H12                          ' Network Neighborhood"
    lstFolders.AddItem "CSIDL_NETHOOD": lstFolders.ItemData(lstFolders.NewIndex) = &H13                          ' <user name>\nethood"
    lstFolders.AddItem "CSIDL_FONTS": lstFolders.ItemData(lstFolders.NewIndex) = &H14                            ' windows\fonts"
    lstFolders.AddItem "CSIDL_TEMPLATES": lstFolders.ItemData(lstFolders.NewIndex) = &H15
    lstFolders.AddItem "CSIDL_COMMON_STARTMENU": lstFolders.ItemData(lstFolders.NewIndex) = &H16                 ' All Users\Start Menu"
    lstFolders.AddItem "CSIDL_COMMON_PROGRAMS": lstFolders.ItemData(lstFolders.NewIndex) = &H17                  ' All Users\Programs"
    lstFolders.AddItem "CSIDL_COMMON_STARTUP": lstFolders.ItemData(lstFolders.NewIndex) = &H18                   ' All Users\Startup"
    lstFolders.AddItem "CSIDL_COMMON_DESKTOPDIRECTORY": lstFolders.ItemData(lstFolders.NewIndex) = &H19          ' All Users\Desktop"
    lstFolders.AddItem "CSIDL_APPDATA": lstFolders.ItemData(lstFolders.NewIndex) = &H1A                          ' <user name>\Application Data"
    lstFolders.AddItem "CSIDL_PRINTHOOD": lstFolders.ItemData(lstFolders.NewIndex) = &H1B                        ' <user name>\PrintHood"
    lstFolders.AddItem "CSIDL_LOCAL_APPDATA": lstFolders.ItemData(lstFolders.NewIndex) = &H1C                    ' <user name>\Local Settings\Applicaiton Data (non roaming)"
    lstFolders.AddItem "CSIDL_ALTSTARTUP": lstFolders.ItemData(lstFolders.NewIndex) = &H1D                       ' non localized startup"
    lstFolders.AddItem "CSIDL_COMMON_ALTSTARTUP": lstFolders.ItemData(lstFolders.NewIndex) = &H1E                ' non localized common startup"
    lstFolders.AddItem "CSIDL_COMMON_FAVORITES": lstFolders.ItemData(lstFolders.NewIndex) = &H1F
    lstFolders.AddItem "CSIDL_INTERNET_CACHE": lstFolders.ItemData(lstFolders.NewIndex) = &H20
    lstFolders.AddItem "CSIDL_COOKIES": lstFolders.ItemData(lstFolders.NewIndex) = &H21
    lstFolders.AddItem "CSIDL_HISTORY": lstFolders.ItemData(lstFolders.NewIndex) = &H22
    lstFolders.AddItem "CSIDL_COMMON_APPDATA": lstFolders.ItemData(lstFolders.NewIndex) = &H23                   ' All Users\Application Data"
    lstFolders.AddItem "CSIDL_WINDOWS": lstFolders.ItemData(lstFolders.NewIndex) = &H24                          ' GetWindowsDirectory()"
    lstFolders.AddItem "CSIDL_SYSTEM": lstFolders.ItemData(lstFolders.NewIndex) = &H25                           ' GetSystemDirectory()"
    lstFolders.AddItem "CSIDL_PROGRAM_FILES": lstFolders.ItemData(lstFolders.NewIndex) = &H26                    ' C:\Program Files"
    lstFolders.AddItem "CSIDL_MYPICTURES": lstFolders.ItemData(lstFolders.NewIndex) = &H27                       ' C:\Program Files\My Pictures"
    lstFolders.AddItem "CSIDL_PROFILE": lstFolders.ItemData(lstFolders.NewIndex) = &H28                          ' USERPROFILE"
    lstFolders.AddItem "CSIDL_SYSTEMX86": lstFolders.ItemData(lstFolders.NewIndex) = &H29                        ' x86 system directory on RISC"
    lstFolders.AddItem "CSIDL_PROGRAM_FILESX86": lstFolders.ItemData(lstFolders.NewIndex) = &H2A                 ' x86 C:\Program Files on RISC"
    lstFolders.AddItem "CSIDL_PROGRAM_FILES_COMMON": lstFolders.ItemData(lstFolders.NewIndex) = &H2B             ' C:\Program Files\Common"
    lstFolders.AddItem "CSIDL_PROGRAM_FILES_COMMONX86": lstFolders.ItemData(lstFolders.NewIndex) = &H2C          ' x86 Program Files\Common on RISC"
    lstFolders.AddItem "CSIDL_COMMON_TEMPLATES": lstFolders.ItemData(lstFolders.NewIndex) = &H2D                 ' All Users\Templates"
    lstFolders.AddItem "CSIDL_COMMON_DOCUMENTS": lstFolders.ItemData(lstFolders.NewIndex) = &H2E                 ' All Users\Documents"
    lstFolders.AddItem "CSIDL_COMMON_ADMINTOOLS": lstFolders.ItemData(lstFolders.NewIndex) = &H2F                ' All Users\Start Menu\Programs\Administrative Tools"
    lstFolders.AddItem "CSIDL_ADMINTOOLS": lstFolders.ItemData(lstFolders.NewIndex) = &H30                       ' <user name>\Start Menu\Programs\Administrative Tools"
    lstFolders.AddItem "CSIDL_CONNECTIONS": lstFolders.ItemData(lstFolders.NewIndex) = &H31                      ' Network and Dial-up Connections"
    lstFolders.ListIndex = 0
End Sub

Private Sub lstFolders_Click()
    Dim idl As Long, aPath As String
    aPath = Space$(MAX_PATH)
    
    lblPath.Caption = " * No Path *"
    If SHGetSpecialFolderLocation(hWnd, lstFolders.ItemData(lstFolders.ListIndex), idl) = 0 Then
        If SHGetPathFromIDList(idl, aPath) Then
            lblPath.Caption = Left$(aPath, InStr(aPath, Chr$(0)) - 1)
        End If
        LocalFree idl
    End If
End Sub
