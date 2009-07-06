VERSION 5.00
Begin VB.Form frmMain 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Windows Special Folder Locations"
   ClientHeight    =   2985
   ClientLeft      =   4635
   ClientTop       =   3465
   ClientWidth     =   5115
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2985
   ScaleWidth      =   5115
   StartUpPosition =   2  'CenterScreen
   Begin VB.ListBox lstFolder 
      Height          =   1815
      Left            =   75
      Sorted          =   -1  'True
      TabIndex        =   0
      Top             =   375
      Width           =   4815
   End
   Begin VB.Label lblPath 
      BackColor       =   &H00FFFFFF&
      BorderStyle     =   1  'Fixed Single
      Height          =   315
      Left            =   75
      TabIndex        =   3
      Top             =   2550
      Width           =   4890
   End
   Begin VB.Label Label2 
      Caption         =   "Path:"
      Height          =   240
      Left            =   75
      TabIndex        =   2
      Top             =   2325
      Width           =   3165
   End
   Begin VB.Label Label1 
      Caption         =   "Choose a Special Folder:"
      Height          =   240
      Left            =   75
      TabIndex        =   1
      Top             =   150
      Width           =   2865
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const CSIDL_ADMINTOOLS           As Long = &H30   '{user}\Start Menu _                                                        '\Programs\Administrative Tools
Private Const CSIDL_COMMON_ADMINTOOLS    As Long = &H2F   '(all users)\Start Menu\Programs\Administrative Tools
Private Const CSIDL_APPDATA              As Long = &H1A   '{user}\Application Data
Private Const CSIDL_COMMON_APPDATA       As Long = &H23   '(all users)\Application Data
Private Const CSIDL_COMMON_DOCUMENTS     As Long = &H2E   '(all users)\Documents
Private Const CSIDL_COOKIES              As Long = &H21
Private Const CSIDL_HISTORY              As Long = &H22
Private Const CSIDL_INTERNET_CACHE       As Long = &H20   'Internet Cache folder
Private Const CSIDL_LOCAL_APPDATA        As Long = &H1C   '{user}\Local Settings\Application Data (non roaming)
Private Const CSIDL_MYPICTURES           As Long = &H27   'C:\Program Files\My Pictures
Private Const CSIDL_PERSONAL             As Long = &H5    'My Documents
Private Const CSIDL_PROGRAM_FILES        As Long = &H26   'Program Files folder
Private Const CSIDL_PROGRAM_FILES_COMMON As Long = &H2B   'Program Files\Common
Private Const CSIDL_SYSTEM               As Long = &H25   'system folder
Private Const CSIDL_WINDOWS              As Long = &H24   'Windows directory or SYSROOT()
Private Const CSIDL_FLAG_CREATE = &H8000&                 'combine with CSIDL_ value to force
Private Const MAX_PATH = 260

'
' Other Special Folder CSIDLs not supported
' by this API.
'
'Private Const CSIDL_ALTSTARTUP As Long = &H1D             'non localized startup
'Private Const CSIDL_BITBUCKET As Long = &HA               '{desktop}\Recycle Bin
'Private Const CSIDL_CONTROLS As Long = &H3                'My Computer\Control Panel
'Private Const CSIDL_DESKTOP As Long = &H0                 '{namespace root}
'Private Const CSIDL_DESKTOPDIRECTORY As Long = &H10       '{user}\Desktop
'Private Const CSIDL_FAVORITES As Long = &H6               '{user}\Favourites
'Private Const CSIDL_FONTS As Long = &H14                  'windows\fonts
'Private Const CSIDL_INTERNET As Long = &H1                'Internet virtual folder
'Private Const CSIDL_DRIVES As Long = &H11                 'My Computer
'Private Const CSIDL_NETHOOD As Long = &H13                '{user}\nethood
'Private Const CSIDL_NETWORK As Long = &H12                'Network Neighbourhood
'Private Const CSIDL_PRINTERS As Long = &H4                'My Computer\Printers
'Private Const CSIDL_PRINTHOOD As Long = &H1B              '{user}\PrintHood
'Private Const CSIDL_PROGRAM_FILESX86 As Long = &H2A       'Program Files folder for x86 apps (Alpha)
'Private Const CSIDL_PROGRAMS As Long = &H2                'Start Menu\Programs
'Private Const CSIDL_PROGRAM_FILES_COMMONX86 As Long = &H2C 'x86 \Program Files\Common on RISC
'Private Const CSIDL_RECENT As Long = &H8                  '{user}\Recent
'Private Const CSIDL_SENDTO As Long = &H9                  '{user}\SendTo
'Private Const CSIDL_STARTMENU As Long = &HB               '{user}\Start Menu
'Private Const CSIDL_STARTUP As Long = &H7                 'Start Menu\Programs\Startup
'Private Const CSIDL_SYSTEMX86 As Long = &H29              'system folder for x86 apps (Alpha)
'Private Const CSIDL_TEMPLATES As Long = &H15
'Private Const CSIDL_PROFILE As Long = &H28                'user's profile folder
'Private Const CSIDL_COMMON_ALTSTARTUP As Long = &H1E      'non localized common startup
'Private Const CSIDL_COMMON_DESKTOPDIRECTORY As Long = &H19 '(all users)\Desktop
'Private Const CSIDL_COMMON_FAVORITES As Long = &H1F       '(all users)\Favourites
'Private Const CSIDL_COMMON_PROGRAMS As Long = &H17        '(all users)\Programs
'Private Const CSIDL_COMMON_STARTMENU As Long = &H16       '(all users)\Start Menu
'Private Const CSIDL_COMMON_STARTUP As Long = &H18         '(all users)\Startup
'Private Const CSIDL_COMMON_TEMPLATES As Long = &H2D       '(all users)\Templates
'                                                          'create on SHGetSpecialFolderLocation()
'Private Const CSIDL_FLAG_DONT_VERIFY = &H4000             'combine with CSIDL_ value to force
'                                                          'create on SHGetSpecialFolderLocation()

Private Const CSIDL_FLAG_MASK = &HFF00                    'mask for all possible flag values
Private Const SHGFP_TYPE_CURRENT = &H0                    'current value for user, verify it exists
Private Const SHGFP_TYPE_DEFAULT = &H1
Private Const S_OK = 0
Private Const S_FALSE = 1
Private Const E_INVALIDARG = &H80070057                   ' Invalid CSIDL Value

Private Declare Function SHGetFolderPath Lib "shfolder" _
        Alias "SHGetFolderPathA" (ByVal hwndOwner As Long, _
        ByVal nFolder As Long, ByVal hToken As Long, _
        ByVal dwFlags As Long, ByVal pszPath As String) As Long
Private Sub Form_Load()

    With lstFolder
        .AddItem ("CSIDL_ADMINTOOLS")
        .ItemData(.NewIndex) = CSIDL_ADMINTOOLS
    
        .AddItem ("CSIDL_COMMON_ADMINTOOLS")
        .ItemData(.NewIndex) = CSIDL_COMMON_ADMINTOOLS
    
        .AddItem ("CSIDL_APPDATA")
        .ItemData(.NewIndex) = CSIDL_APPDATA
    
        .AddItem ("CSIDL_COMMON_APPDATA")
        .ItemData(.NewIndex) = CSIDL_COMMON_APPDATA
    
        .AddItem ("CSIDL_COMMON_DOCUMENTS")
        .ItemData(.NewIndex) = CSIDL_COMMON_DOCUMENTS
    
        .AddItem ("CSIDL_COOKIES")
        .ItemData(.NewIndex) = CSIDL_COOKIES
    
        .AddItem ("CSIDL_HISTORY")
        .ItemData(.NewIndex) = CSIDL_HISTORY
    
        .AddItem ("CSIDL_INTERNET_CACHE")
        .ItemData(.NewIndex) = CSIDL_INTERNET_CACHE
    
        .AddItem ("CSIDL_LOCAL_APPDATA")
        .ItemData(.NewIndex) = CSIDL_LOCAL_APPDATA
    
        .AddItem ("CSIDL_MYPICTURES")
        .ItemData(.NewIndex) = CSIDL_MYPICTURES
    
        .AddItem ("CSIDL_PERSONAL")
        .ItemData(.NewIndex) = CSIDL_PERSONAL
    
        .AddItem ("CSIDL_PROGRAM_FILES")
        .ItemData(.NewIndex) = CSIDL_PROGRAM_FILES
    
        .AddItem ("CSIDL_PROGRAM_FILES_COMMON")
        .ItemData(.NewIndex) = CSIDL_PROGRAM_FILES_COMMON
    
        .AddItem ("CSIDL_SYSTEM")
        .ItemData(.NewIndex) = CSIDL_SYSTEM
    
        .AddItem ("CSIDL_WINDOWS")
        .ItemData(.NewIndex) = CSIDL_WINDOWS
    End With

End Sub
Private Sub lstFolder_Click()
Dim strBuffer  As String
Dim strPath    As String
Dim lngReturn  As Long
Dim lngCSIDL   As Long

    '
    ' When a listbox item is clicked, get the
    ' associated folder's path.
    '
    If lstFolder.ListIndex >= 0 Then
        lngCSIDL = lstFolder.ItemData(lstFolder.ListIndex)
        strPath = String(MAX_PATH, 0)
        
        '
        ' Get the folder's path. If the
        ' "Create" flag is used, the folder will be created
        ' if it does not exist.
        '
        lngReturn = SHGetFolderPath(0, lngCSIDL, 0, SHGFP_TYPE_CURRENT, strPath)
        'lngReturn = SHGetFolderPath(0, lngCSIDL Or CSIDL_FLAG_CREATE, 0, SHGFP_TYPE_CURRENT, strPath)
        
        Select Case lngReturn
            Case S_OK
                lblPath.Caption = Left$(strPath, InStr(1, strPath, Chr(0)) - 1)
            
            Case S_FALSE
                lblPath.Caption = "Folder Does Not Exist"
            
            Case E_INVALIDARG
                lblPath.Caption = "Folder Not Valid on this OS"
            
            Case Else
                lblPath.Caption = "Folder Not Valid on this OS"
        End Select
    End If
    
End Sub


