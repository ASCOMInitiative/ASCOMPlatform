Attribute VB_Name = "Registry"
'
'   ============
'   REGISTRY.BAS (read-only subset)
'   ============
'
' Stolen:  05-Mar-07   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 05-Mar-07 rbd     Initial edit, stole from old DriverHelper (reg based)
' -----------------------------------------------------------------------------
Option Explicit

Private Const SCODE_REGERR As Long = vbObjectError + &H400

' =========================
' ROOT OF OUR REGISTRY AREA
' =========================
'
Private Const ROOT_KEY As String = "Software\ASCOM"


' Reg Key Security Options...
Private Const READ_CONTROL As Long = &H20000
Private Const KEY_QUERY_VALUE As Long = &H1
Private Const KEY_SET_VALUE As Long = &H2
Private Const KEY_CREATE_SUB_KEY As Long = &H4
Private Const KEY_ENUMERATE_SUB_KEYS As Long = &H8
Private Const KEY_NOTIFY As Long = &H10
Private Const KEY_CREATE_LINK As Long = &H20
'
Public Const KEY_READ As Long = KEY_QUERY_VALUE + KEY_ENUMERATE_SUB_KEYS + _
                        READ_CONTROL
Public Const KEY_WRITE As Long = KEY_SET_VALUE + KEY_CREATE_SUB_KEY + _
                        READ_CONTROL
Public Const KEY_ALL_ACCESS As Long = KEY_QUERY_VALUE + KEY_SET_VALUE + _
                       KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + _
                       KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

Public Const HKEY_CLASSES_ROOT As Long = &H80000000
Public Const HKEY_LOCAL_MACHINE As Long = &H80000002
Public Const HKEY_CURRENT_USER As Long = &H80000001

Public Const REG_SZ As Long = 1                    ' Unicode nul terminated string
Public Const REG_EXPAND_SZ As Long = 2             ' REG_SZ with env var references
Public Const REG_DWORD As Long = 4                 ' 32-bit number

Public Const REG_OPTION_NON_VOLATILE As Long = 0
Public Const REG_CREATED_NEW_KEY As Long = 1


Public Const ENUM_BUF_SIZE = 32768             ' Key enumeration buffer, see GetProfile()

Type FILETIME
        dwLowDateTime As Long
        dwHighDateTime As Long
End Type

'
' lpClass may be Null so we define it as Any
'

Declare Function RegCreateKeyEx Lib "advapi32" Alias "RegCreateKeyExA" _
   (ByVal hKey As Long, _
    ByVal lpSubKey As String, _
    ByVal dwReserved As Long, _
    ByVal lpClass As String, _
    ByVal dwOptions As Long, _
    ByVal samDesired As Long, _
    ByVal lpSecurityAttributes As Any, _
    ByRef phkResult As Long, _
    ByRef lpDisposition As Long) As Long
    
Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" _
   (ByVal hKey As Long, _
    ByVal lpSubKey As String, _
    ByVal dwReserved As Long, _
    ByVal samDesired As Long, _
    ByRef phkResult As Long) As Long

Declare Function RegEnumValue Lib "advapi32" Alias "RegEnumValueA" _
   (ByVal hKey As Long, _
    ByVal dwIndex As Long, _
    ByVal lpszName As String, _
    ByRef dwNameLen As Long, _
    ByVal lpReserved As Long, _
    ByRef lpdwType As Long, _
    ByVal lpszValue As String, _
    ByRef lpdwValLen As Long) As Long
    
Declare Function RegEnumKey Lib "advapi32" Alias "RegEnumKeyA" _
   (ByVal hKey As Long, _
    ByVal dwIndex As Long, _
    ByVal lpszName As String, _
    ByVal dwNameLen As Long) As Long

Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" _
   (ByVal hKey As Long, _
    ByVal lpValueName$, _
    ByVal lpdwReserved As Long, _
    ByRef lpdwType As Long, _
    ByVal lpData As String, _
    ByRef lpcbData As Long) As Long
        
Declare Function RegCloseKey Lib "advapi32" _
   (ByVal hKey As Long) As Long

Public hKeyRoot As Long             ' Handle to ASCOM/Telescope registry root


'---------------------------------------------------------------------------
'
'   GetProfile() - Get a named value under the given subkey
'
'---------------------------------------------------------------------------
Public Function GetProfile(sSubKey As String, sName As String, _
                                sSrc As String) As String

    GetProfile = GetRegString(hKeyRoot, sSubKey, sName, sSrc)
    
End Function

'---------------------------------------------------------------------------
'
'   EnumProfile() - Enumerate string values under named subkey or root
'
'---------------------------------------------------------------------------
Public Function EnumProfile(sSubKey As String, sSrc As String) As Dictionary

    Set EnumProfile = EnumRegStrings(hKeyRoot, sSubKey, sSrc)
    
End Function

'---------------------------------------------------------------------------
'
'   EnumKeys() - Enumerate keys and class strings from named subkey or root
'
'---------------------------------------------------------------------------
Public Function EnumKeys(sSubKey As String, sSrc As String) As Dictionary

    Set EnumKeys = EnumRegKeys(hKeyRoot, sSubKey, sSrc)
    
End Function


'---------------------------------------------------------------------------
'
'   GetRegString() - Get a string value from the registry
'
' Expand REG_EXPAND_SZ automatically
'---------------------------------------------------------------------------
Public Function GetRegString(hKeyBase As Long, sSubKey As String, _
                                sName As String, sSrc As String) As String
    Dim hKey As Long
    Dim dwType As Long
    Dim dwLen As Long
    Dim szBuf As String

    InitReg sSrc                                    ' Open master key if needed
    GetRegString = ""                               ' Assume failure
    dwType = -1
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegOpenKeyEx(hKeyBase, sSubKey, 0, KEY_READ, hKey) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open sub-key " & sSubKey & "."
        End If
    End If
    
    dwLen = 1023
    szBuf = String$(dwLen, Chr$(0))
    If RegQueryValueEx(hKey, sName, 0, dwType, szBuf, dwLen) = 0 Then
        If dwType <> REG_SZ Then
            Err.Raise SCODE_REGERR, sSrc, _
                "Non-string found in " & sSubKey & " " & sName
        End If
        If (Asc(Mid(szBuf, dwLen, 1)) = 0) Then     ' Win95 Adds Null Terminated String...
            GetRegString = Left(szBuf, dwLen - 1)   ' Null Found, Extract From String
        Else                                        ' WinNT Does NOT Null Terminate String...
            GetRegString = Left(szBuf, dwLen)       ' Null Not Found, Extract String Only
        End If
    End If
    
    If hKey <> hKeyBase Then RegCloseKey (hKey)
    
End Function


'---------------------------------------------------------------------------
'
'   EnumRegStrings() - Create a dictionary of string key-value pairs
'
'---------------------------------------------------------------------------
Public Function EnumRegStrings(hKeyBase As Long, sSubKey As String, _
                                sSrc As String) As Dictionary
    Dim d As Dictionary
    Dim i As Long
    Dim Key As String, val As String
    Dim klen As Long, vlen As Long, typ As Long
    Dim hKey As Long
    
    Set EnumRegStrings = Nothing
    
    InitReg sSrc
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegOpenKeyEx(hKeyBase, sSubKey, 0, KEY_READ, hKey) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open sub-key " & sSubKey & "."
        End If
    End If
    '
    ' This assumes that the values are REG_SZ. If not, an error occurs.
    '
    Set d = New Scripting.Dictionary
    i = 0                                       ' Enum index
    Key = String(255, Chr$(0))
    klen = 255
    val = String(255, Chr$(0))
    vlen = 255
    Do While RegEnumValue(hKey, i, Key, klen, 0&, typ, val, vlen) = 0
        If typ <> REG_SZ Then
            Err.Raise SCODE_REGERR, sSrc, _
                "Non-string found in " & sSubKey
        End If
        
        Key = Trim$(Left$(Key, InStr(Key, Chr$(0)) - 1))
        val = Trim$(Left$(val, InStr(val, Chr$(0)) - 1))
        d.Add Key, val
        
        Key = String(255, Chr$(0))
        klen = 255
        val = String(255, Chr$(0))
        vlen = 255
        i = i + 1
    Loop
    
    If hKey <> hKeyBase Then RegCloseKey (hKey)

    Set EnumRegStrings = d
    
End Function

'---------------------------------------------------------------------------
'
'   EnumRegKeys() - Create a dictionary of key-name/class-name pairs
'
'---------------------------------------------------------------------------
Public Function EnumRegKeys(hKeyBase As Long, sSubKey As String, _
                                sSrc As String) As Dictionary
    Dim d As Dictionary
    Dim i As Long
    Dim Key As String, cls As String
    Dim klen As Long, clen As Long, typ As Long
    Dim hKey As Long
    Dim ft As FILETIME
    
    Set EnumRegKeys = Nothing
    
    InitReg sSrc
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegOpenKeyEx(hKeyBase, sSubKey, 0, KEY_READ, hKey) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open sub-key " & sSubKey & "."
        End If
    End If
    '
    ' This assumes that the values are REG_SZ. If not, an error occurs.
    '
    Set d = New Scripting.Dictionary
    i = 0                                       ' Enum index
    Key = String(255, Chr$(0))
    klen = 255
    cls = String(255, Chr$(0))
    clen = 255
    Do While RegEnumKey(hKey, i, Key, klen) = 0
        
        Key = Trim$(Left$(Key, InStr(Key, Chr$(0)) - 1))
        'cls = Trim$(Left$(cls, InStr(cls, Chr$(0)) - 1))
        cls = GetRegString(hKey, Key, "", sSrc)
        d.Add Key, cls
        
        Key = String(255, Chr$(0))
        klen = 255
        cls = String(255, Chr$(0))
        clen = 255
        i = i + 1
    Loop
    
    If hKey <> hKeyBase Then RegCloseKey (hKey)

    Set EnumRegKeys = d
    
End Function


'---------------------------------------------------------------------------
'
' InitReg - Open the master registry key if needed
'
'---------------------------------------------------------------------------
Private Sub InitReg(sSrc As String)
    Dim lRes As Long, lStat As Long
    Dim c As String
    
    If hKeyRoot <> 0 Then Exit Sub
    
    lStat = RegCreateKeyEx(HKEY_LOCAL_MACHINE, ROOT_KEY, 0&, 0&, 0&, _
                    (KEY_READ + KEY_WRITE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS), _
                    0&, hKeyRoot, lRes)
'    lStat = RegCreateKeyEx(HKEY_LOCAL_MACHINE, ROOT_KEY, 0&, 0&, 0&, _
'                    (KEY_ALL_ACCESS), 0&, hKeyRoot, lRes)
    If lStat <> 0 Then
        Err.Raise SCODE_REGERR, sSrc, _
            "Failed to open/create root registry key (" & lStat & ")"
    End If
End Sub


