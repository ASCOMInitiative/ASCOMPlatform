Attribute VB_Name = "Registry"
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
'
'   ============
'   REGISTRY.BAS
'   ============
'
' Written:  24-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Aug-00 rbd     Initial edit, from the code in ACP.
' 21-Jan-01 rbd     Source for Err.Raise is now passed in since this is shared
'                   among several objects. Add creation and deletion functions.
'                   Rework and clean up variable naming, reorganize.
' 08-Jun-01 rbd     ROOT now just ASCOM, callers must supply the complete path.
'                   This is part of the generalization of the Chooser and
'                   Profile objects for any ASCOM driver type.
' 11-Jan-03 rbd     Fix DeleteRegValue()
' -----------------------------------------------------------------------------
Option Explicit

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
    
Declare Function RegSetValueEx Lib "advapi32" Alias "RegSetValueExA" _
   (ByVal hKey As Long, _
    ByVal lpValueName$, _
    ByVal lpdwReserved As Long, _
    ByVal dwType As Long, _
    ByVal lpData As String, _
    ByVal cbData As Long) As Long
    
Declare Function RegDeleteKey Lib "advapi32" Alias "RegDeleteKeyA" _
    (ByVal hKey As Long, ByVal lpszName As String) As Long

Declare Function RegDeleteValue Lib "advapi32" Alias "RegDeleteValueA" _
    (ByVal hKey As Long, ByVal lpszName As String) As Long
    
Declare Function RegCloseKey Lib "advapi32" _
   (ByVal hKey As Long) As Long
   
Declare Function ExpandEnvironmentStrings Lib "kernel32" _
                Alias "ExpandEnvironmentStringsA" _
                (ByVal lpSrc As String, _
                 ByVal lpDst As String, _
                 ByVal dwSize As Long) As Long

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
'   WriteProfile() - Write/Create a named value under the given subkey
'
'---------------------------------------------------------------------------
Public Sub WriteProfile(sSubKey As String, sName As String, sVal As String, _
                                sSrc As String)

    WriteRegString hKeyRoot, sSubKey, sName, sVal, sSrc
        
End Sub

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
'   DeleteProfile() - Delete the named value
'
'---------------------------------------------------------------------------
Public Sub DeleteProfile(sSubKey As String, sName As String, sSrc As String)

    DeleteRegValue hKeyRoot, sSubKey, sName, sSrc
    
End Sub

'---------------------------------------------------------------------------
'
'   CreateKey() - Create the named subkey
'
'---------------------------------------------------------------------------
Public Sub CreateKey(sSubKey As String, sSrc As String)

    CreateRegKey hKeyRoot, sSubKey, sSrc
    
End Sub

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
'   DeleteKey() - Delete the named subkey and everything under it
'
'---------------------------------------------------------------------------
Public Sub DeleteKey(sSubKey As String, sSrc As String)

    DeleteRegKey hKeyRoot, sSubKey, sSrc
    
End Sub

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
    '
    ' Expand environment strings if needed
    '
    If dwType = REG_EXPAND_SZ Then                  ' If needs env var expansion
        szBuf = String$(1024, Chr$(0))
        dwLen = ExpandEnvironmentStrings(GetRegString, szBuf, 1023)
        GetRegString = Left$(szBuf, (dwLen - 1))    ' Len includes null
    End If
    
End Function

'---------------------------------------------------------------------------
'
'   GetRegDword() - Get a DWORD (long) value from the registry
'
'---------------------------------------------------------------------------
Public Function GetRegDword(hKeyBase As Long, sSubKey As String, _
                                sName As String, sSrc As String) As Long
    Dim hKey As Long
    Dim dwType As Long
    Dim dwLen As Long
    Dim szBuf As String
    Dim i As Integer
    Dim keyVal As String

    InitReg sSrc                                ' Open master key if needed
    GetRegDword = 0
        
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegOpenKeyEx(hKeyBase, sSubKey, 0, KEY_READ, hKey) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open sub-key " & sSubKey & "."
        End If
    End If
    
    dwLen = 4
    szBuf = String$(dwLen, 0)
    If RegQueryValueEx(hKey, sName, 0, dwType, szBuf, dwLen) = 0 Then
        If dwType <> REG_SZ Then
            Err.Raise SCODE_REGERR, sSrc, _
                "Non-string found in " & sSubKey & " " & sName
        End If
        If (Asc(Mid(szBuf, dwLen, 1)) = 0) Then     ' Win95 Adds Null Terminated String...
            szBuf = Left(szBuf, dwLen - 1)          ' Null Found, Extract From String
        Else                                        ' WinNT Does NOT Null Terminate String...
            szBuf = Left(szBuf, dwLen)              ' Null Not Found, Extract String Only
        End If
    End If
    If hKey <> hKeyBase Then RegCloseKey (hKey)
   
    For i = Len(szBuf) To 1 Step -1                  ' Convert Each Byte
        keyVal = keyVal + Hex(Asc(Mid(szBuf, i, 1))) ' Build Value Char. By Char.
    Next
    GetRegDword = CLng("&H" + keyVal)
    
End Function

'---------------------------------------------------------------------------
'
'   WriteRegString() - Write/Create a string in the registry
'
'---------------------------------------------------------------------------
Public Sub WriteRegString(hKeyBase As Long, sSubKey As String, _
                                sName As String, val As String, sSrc As String)
    Dim hKey As Long
    Dim l As Long
    Dim lRes As Long
    
    '
    ' Unless I do this, if val comes from the 'text' property
    ' of a textbox control, the RegSetValueEx returns Win32 error 87
    ' (Invalid parameter). Maybe it's variant vs real string, and the
    ' control returns a variant null instead of an empty string.
    '
    If val = "" Then val = ""
    
    InitReg sSrc                             ' Open master key if needed
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegCreateKeyEx(hKeyBase, sSubKey, 0&, 0&, 0&, KEY_WRITE, 0&, hKey, lRes) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open/create sub-key " & sSubKey & "."
        End If
    End If
    
    l = (Len(val) + 1)
    If RegSetValueEx(hKey, sName, 0, REG_SZ, val, l) <> 0 Then
        If hKey = hKeyBase Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to write config value " & sName & "."
        Else
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to write config value " & sName & _
                    " to registry sub-key " & sSubKey & "."
        End If
    End If
    
    If hKey <> hKeyBase Then RegCloseKey (hKey)
    
End Sub

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
'   DeleteRegValue() - Delete a named value
'
'---------------------------------------------------------------------------
Public Sub DeleteRegValue(hKeyBase As Long, sSubKey, sName As String, sSrc As String)
    Dim hKey As Long
    
    InitReg sSrc
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegOpenKeyEx(hKeyBase, sSubKey, 0, KEY_WRITE, hKey) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to open key " & sSubKey & "."
        End If
    End If
    
    If RegDeleteValue(hKey, sName) <> 0 Then
        Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to delete value " & sName & " under " & sSubKey & "."
    End If
    
End Sub

'---------------------------------------------------------------------------
'
'   CreateRegKey() - Create a subkey
'
'---------------------------------------------------------------------------
Public Sub CreateRegKey(hKeyBase As Long, sSubKey As String, sSrc As String)
    Dim hKey As Long
    Dim lRes As Long

    InitReg sSrc                             ' Open master key if needed
    
    If sSubKey = "" Then
        hKey = hKeyBase
    Else
        If RegCreateKeyEx(hKeyBase, sSubKey, 0&, 0&, 0&, KEY_WRITE, 0&, hKey, lRes) <> 0 Then
            Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to create sub-key " & sSubKey & "."
        End If
    End If
    
    If hKey <> hKeyBase Then RegCloseKey hKey
    
End Sub

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
'   DeleteRegKey() - Delete a key and everything under it
'
'---------------------------------------------------------------------------
Public Sub DeleteRegKey(hKeyBase As Long, sSubKey As String, sSrc As String)
    Dim d As Dictionary
    Dim i As Integer
    Dim k As Variant
    
    InitReg sSrc
    
    Set d = EnumRegKeys(hKeyBase, sSubKey, sSrc)
    k = d.Keys
    For i = 0 To d.Count - 1
        DeleteRegKey hKeyBase, sSubKey & "\" & k(i), sSrc   ' RECURSION
    Next
    If RegDeleteKey(hKeyBase, sSubKey) <> 0 Then
        Err.Raise SCODE_REGERR, sSrc, _
                    "Failed to delete key " & sSubKey & "."
    End If
    
End Sub

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


