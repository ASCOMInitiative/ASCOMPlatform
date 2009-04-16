Attribute VB_Name = "Config"
'---------------------------------------------------------------------
' Copyright © 2007, The ASCOM Initiative
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". THE ASCOM INITIATIVE MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ==========
'   CONFIG.BAS  Helper's profile storage layer - uses XML doc.
'   ==========
'
' Written:  03-Apr-2007   Robert B. Denny <rdenny@dc3.com>
'
' WARNING: THIS TAKES ADVANTAGE OF THE FACT THAT MSXML2 ALLOWS DOTS IN
' ELEMENT NAMES. THIS IS NOT LEGAL IN STRICT XML, BUT THE CONF FILE IS
' THE SOLE PROPERTY OF US, SO WHAT THE HECK? IT MAKES MIRRORING THE
' OLD REGISTRY FUNCTIONALITY SAFER BECAUSE WE DON'T SUBSTITUTE CHARS
' RISKING A CLASH BETWEEN TWO OTHERWISE DIFFERENT SYMBOLS.
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 03-Apr-07 rbd     Initial edit, for new XML config storage (Vista)
' 05-Apr-07 rbd     Code complete, all routines tested to first level.
' 06-Apr-07 rbd     LowerCase keys and value names to provide caseless
'                   behavior like the registry.
' 12-Apr-07 rbd     5.0.3 - EnumKeys returns empty Dictionary if reference
'                   key does not exist (old behavior, for e.g. Chooser)
' 17-Sep-07 rbd     5.1.0 - Fix CreateKey()
' -----------------------------------------------------------------------------
Option Explicit

'
' Win32 functions and constants needed for special folders
' The Shell API is the right way to get these! Do not use
' environment strings!!!
'
' CSIDLs are listed only for those that we might use
'
Private Const CSIDL_APPDATA As Long = &H1A                  ' Specific User\Application Data
Private Const CSIDL_LOCAL_APPDATA As Long = &H1C            ' Specific User\Local Settings\Application Data
Private Const CSIDL_COMMON_APPDATA As Long = &H23           ' All Users\Application Data
Private Const CSIDL_PROGRAM_FILES_COMMON As Long = &H2B     ' Program Files\Common Files
Private Const CSIDL_COMMON_DOCUMENTS As Long = &H2E         ' All Users\Documents
Private Type SHITEMID
    cb As Long
    abID As Byte
End Type
Private Type ITEMIDLIST
    mkid As SHITEMID
End Type
Private Declare Function LocalFree Lib "kernel32" (ByVal hMem As Long) As Long
Private Declare Function SHGetPathFromIDList Lib "shell32.dll" Alias "SHGetPathFromIDListA" _
                        (ByVal pidl As Long, ByVal pszPath As String) As Long
Private Declare Function SHGetSpecialFolderLocation Lib "shell32.dll" _
                        (ByVal hWndOwner As Long, ByVal nFolder As Long, pidl As Any) As Long
Const MAX_PATH = 260
'
' XSL to transform XML into readable indented format when written
' back to the config file. This simple XSLT was quite an exercise
' in just-in-time learning!
'
Private Const INDENT_XSL As String = _
    "<?xml version=""1.0"" encoding=""ISO-8859-1""?>" & _
    "<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" " & _
    "                  xmlns:fo=""http://www.w3.org/1999/XSL/Format"">                  " & _
    "    <xsl:output method=""xml"" encoding=""ISO-8859-1"" omit-xml-declaration=""no"" " & _
    "                  indent=""yes""/>                                                 " & _
    "    <xsl:template match=""/ | node() | @*"">                                       " & _
    "        <xsl:copy>                                                                 " & _
    "            <xsl:apply-templates select=""node() | @*""/>                          " & _
    "        </xsl:copy>                                                                " & _
    "    </xsl:template>                                                                " & _
    "</xsl:stylesheet>                                                                  "

Private m_sConfFilePath As String                           ' Path to config XML file
Private m_XML As MSXML2.DOMDocument40                       ' Holds doc reference for writing out

' ---------------------------
' PROCESS-WIDE PUBLIC LIBRARY
' ---------------------------

'---------------------------------------------------------------------------
'
'   InitConfig() - Initialize this service at server startup
'
'---------------------------------------------------------------------------
Public Sub InitConfig()
    Set m_XML = Nothing                                     ' [sentinel]
End Sub


'---------------------------------------------------------------------------
'
'   SaveConfig() - Save XML config data to file
'
' Called to write current config out whenever a Profile object is released.
'---------------------------------------------------------------------------
Public Sub SaveConfig(sSrc As String)
    If Not m_XML Is Nothing Then UpdateConfig sSrc          ' Only if was loaded in first place!
End Sub

'---------------------------------------------------------------------------
'
'   GetProfile() - Get a named value under the given subkey
'
' Per old behavior, returns "" if not found, instead of raising error
'---------------------------------------------------------------------------
Public Function GetProfile(sSubKey As String, sName As String, _
                                sSrc As String) As String
    Dim rNode As MSXML2.IXMLDOMNode
    Dim vNode As MSXML2.IXMLDOMNode
    
    Set rNode = GetRefNode(sSubKey, sSrc)                   ' Load and get the ref node (typ.)
    If sName <> "" Then                                     ' Named value, get 'value' node text
        Set vNode = rNode.selectSingleNode("value[@name='" & LCase$(sName) & "']")
        If vNode Is Nothing Then
            GetProfile = ""
        Else
            GetProfile = vNode.Text                         ' Return the text of the value node
        End If
    ElseIf rNode.Attributes.length <> 0 Then                ' Unnamed value
        GetProfile = rNode.Attributes(0).Text               ' Attrib text is value (could be "")
    Else
        GetProfile = ""
    End If
    
End Function

'---------------------------------------------------------------------------
'
'   WriteProfile() - Write/Create a named value under the given subkey
'
' Auto-creates any needed subkeys.
'---------------------------------------------------------------------------
Public Sub WriteProfile(sSubKey As String, sName As String, sVal As String, _
                                sSrc As String)
    Dim rNode As MSXML2.IXMLDOMNode
    Dim vNode As MSXML2.IXMLDOMNode
    Dim aNode As MSXML2.IXMLDOMAttribute
    
    Set rNode = CreateKey(sSubKey, sSrc)                    ' Auto-create needed keys
    If sName <> "" Then                                     ' Named value, get 'value' node
        Set vNode = rNode.selectSingleNode("value[@name='" & LCase$(sName) & "']")
        If vNode Is Nothing Then                            ' Value doesn't already exist
            Set vNode = m_XML.createNode(1, "value", "")    ' Create new value node
            vNode.Text = Trim$(sVal)                        ' Set it's test to the value
            Set aNode = m_XML.createAttribute("name")       ' Create name attribute
            aNode.Value = LCase$(sName)                     ' Set it to the LC name
            vNode.Attributes.setNamedItem aNode             ' Add attribute to new value node
            rNode.appendChild vNode                         ' Add attribute to the reference node
        Else
            vNode.Text = Trim$(sVal)                        ' Update the value
        End If
    Else                                                    ' Unnamed value
        If rNode.Attributes.length <> 0 Then                ' Already have attribute?
            rNode.Attributes(0).Text = Trim$(sVal)          ' Attrib text is value
        Else
            Set aNode = m_XML.createAttribute("desc")       ' Need to add "desc" attribute
            aNode.Value = Trim$(sVal)                       ' Value is value of unnamed itam
            rNode.Attributes.setNamedItem aNode             ' Add new attrib to ref node
        End If
    End If
    
End Sub

'---------------------------------------------------------------------------
'
'   EnumProfile() - Enumerate string values under named subkey or root
'
'---------------------------------------------------------------------------
Public Function EnumProfile(sSubKey As String, sSrc As String) As Scripting.Dictionary
    Dim rNode As MSXML2.IXMLDOMNode
    Dim vNodes As MSXML2.IXMLDOMNodeList
    Dim vNode As MSXML2.IXMLDOMNode
    
    Set rNode = GetRefNode(sSubKey, sSrc)
    Set EnumProfile = New Scripting.Dictionary              ' We return a Dictionary
    '
    ' This handles the behavior of the "unnamed" or (default) value
    ' under a key. It is mapped as an attribute. Any attribute name
    ' will work. If no attribute is present, nothing will be added
    ' to the returned Dictionary. But if there is one, even if it
    ' has a value of an empty string, it will be added. The key in
    ' the dictionary is "". See the Helper docs.
    '
    If rNode.Attributes.length > 0 Then                     ' Maybe unnamed value
        EnumProfile.Add "", rNode.Attributes(0).Text        ' Accept any attribute name
    End If
    '
    ' Now add all of the named values
    '
    Set vNodes = rNode.selectNodes("value")
    For Each vNode In vNodes
        EnumProfile.Add vNode.Attributes(0).Text, vNode.Text
    Next
    
End Function

'---------------------------------------------------------------------------
'
'   DeleteProfile() - Delete the named value
'
'---------------------------------------------------------------------------
Public Sub DeleteProfile(sSubKey As String, sName As String, sSrc As String)
    Dim rNode As MSXML2.IXMLDOMNode
    Dim rElem As MSXML2.IXMLDOMElement
    Dim vNode As MSXML2.IXMLDOMNode
    
    Set rNode = GetRefNode(sSubKey, sSrc)
    Set rElem = rNode
    If sName <> "" Then                                     ' Named value, get 'value' node
        Set vNode = rNode.selectSingleNode("value[@name='" & LCase$(sName) & "']")
        If vNode Is Nothing Then                            ' Value doesn't exist
            Err.Raise SCODE_NOEXIST_VAL, sSrc, MSG_NOEXIST_VAL
        End If
        rNode.removeChild vNode                             ' Remove this value node
    Else                                                    ' Unnamed value
        If rNode.Attributes.length = 0 Then                 ' No attrib
            Err.Raise SCODE_NOEXIST_VAL, sSrc, MSG_NOEXIST_VAL  ' Value doesn't exist
        End If
        rElem.removeAttribute "desc"
    End If
    
End Sub

'---------------------------------------------------------------------------
'
'   CreateKey() - Create the named subkey
'
' Function return used for WriteProfile()'s auto-create key
'---------------------------------------------------------------------------
Public Function CreateKey(sSubKey As String, sSrc As String) As MSXML2.IXMLDOMNode
    Dim rNode As MSXML2.IXMLDOMNode
    Dim kNode As MSXML2.IXMLDOMNode
    Dim pNode As MSXML2.IXMLDOMNode
    Dim eCode As Long
    Dim eDesc As String
    Dim bits() As String
    Dim keypath As String
    Dim i As Integer, j As Integer
    
    On Error Resume Next                                    ' I really hate VB's error handling
    Set rNode = GetRefNode(sSubKey, sSrc)                   ' This loads the XML into the engine at m_XML...
    If Err.Number = 0 Then                                  ' Key already exists!
        Set CreateKey = rNode                               ' Return key node
        Exit Function                                       ' This call was a no-op
    End If
    '
    ' At this point either the key doesn't exist, or there was
    ' some problem loading the XML config. Handle the latter
    ' problem first.
    '
    If Err.Number <> SCODE_NOEXIST_KEY Then                 ' Some other error, can't continue
        eCode = Err.Number                                  ' Save error info
        eDesc = Err.Description
        On Error GoTo 0                                     ' Re-arm
        Err.Raise eCode, sSrc, eDesc                        ' and re-raise it
    End If
    On Error GoTo 0                                         ' Re-arm raise
    '
    ' OK, we have good data in the XML engine. We walk down
    ' the path from the top ASCOM node. When we reach a point
    ' where the node doesn't exist, create it (appending).
    ' We repeat this continuing down the path till the end.
    '
    keypath = LCase$(Replace(sSubKey, "\", "/"))            ' LowerCase & convert slashes (preserve sSubKey)
    bits = Split(keypath, "/")                              ' Separate the elements
    Set pNode = m_XML.selectSingleNode("ASCOM")             ' ASCOM root element
    For i = 0 To UBound(bits)
        On Error Resume Next
        Set kNode = pNode.selectSingleNode(bits(i))
        If Err.Number <> 0 Then Set kNode = Nothing
        If kNode Is Nothing Then                            ' Reached a nonexistent key
            Set kNode = m_XML.createNode(1, bits(i), "")    ' Create new element
            pNode.appendChild kNode                         ' Append it to parent
        End If
        Set pNode = kNode                                   ' Go deeper
    Next
    Set CreateKey = pNode                                   ' Return deepest key node
    
End Function

'---------------------------------------------------------------------------
'
'   EnumKeys() - Enumerate keys and class strings from named subkey or root
'
'---------------------------------------------------------------------------
Public Function EnumKeys(sSubKey As String, sSrc As String) As Dictionary
    Dim rNode As MSXML2.IXMLDOMNode
    Dim sNodes As MSXML2.IXMLDOMNodeList
    Dim sNode As MSXML2.IXMLDOMNode
    
    Set EnumKeys = New Scripting.Dictionary                 ' We return a Dictionary
    On Error Resume Next
    Set rNode = GetRefNode(sSubKey, sSrc)
    If Err.Number <> 0 Then Exit Function                   ' Root key n/a, return empty Dict (old behavior)
    On Error GoTo 0
    '
    ' This handles the behavior of the "unnamed" or (default) value
    ' under a subkey. It is mapped as an attribute. Any attribute name
    ' will work. If no attribute exists (no unnamed value), then
    ' the dictionary value will be Null.
    '
    Set sNodes = rNode.selectNodes("*")
    For Each sNode In sNodes
        If sNode.baseName <> "value" Then                   ' Skip value nodes
            If sNode.Attributes.length > 0 Then             ' This is a sub-key
                EnumKeys.Add sNode.baseName, sNode.Attributes(0).Text
            Else
                EnumKeys.Add sNode.baseName, Null
            End If
        End If
    Next
    
End Function

'---------------------------------------------------------------------------
'
'   DeleteKey() - Delete the named subkey and everything under it
'
'---------------------------------------------------------------------------
Public Sub DeleteKey(sSubKey As String, sSrc As String)
    Dim rNode As MSXML2.IXMLDOMNode
    Dim pNode As MSXML2.IXMLDOMNode
    
    Set rNode = GetRefNode(sSubKey, sSrc)
    Set pNode = rNode.parentNode
    pNode.removeChild rNode
    
End Sub

' -----------------
' PRIVATE FUNCTIONS
' -----------------

'
' GetRefNode() - Load the current config file, return ref node.
'
' Loading of XML Config, if needed, is deferred here (rather than
' being done in Profile.Class_Initialize() because raised errors
' during construction are masked from the client app. This way,
' the real reason for laod failure is apparent to the client.
'
Private Function GetRefNode(sSubKey As String, sSrc As String) As MSXML2.IXMLDOMNode
    Dim tNode As MSXML2.IXMLDOMNode
    Dim buf As String
    Dim e As Long
    
    LoadConfigIf sSrc                                       ' Load (and create if none) XML config, if neeeded
    
    Set tNode = m_XML.documentElement                       ' Root element (ASCOM)
    buf = LCase$(Replace(sSubKey, "\", "/"))                ' LowerCase & convert slashes
    If buf = "" Then
        Set GetRefNode = tNode                              ' Root is ref node
    Else
    Set GetRefNode = tNode.selectSingleNode(buf)            ' Get the ref node
    End If
    If GetRefNode Is Nothing Then                           ' Ref node doesn't exist?
        Err.Raise SCODE_NOEXIST_KEY, sSrc, MSG_NOEXIST_KEY & sSubKey
    End If

End Function

'
' UpdateConfig() - Update the config file from DOMDocument
'
' Called each time a Profile object is released via
' Profile.Class_Terminate()
'
' This uses XSLT to "transform" the XML into indented XML for
' easier human reading. See the XSLT document (in a string)
' at the top of this source module.
'
' NOTE: The MSXML engine, combined with the XSLT above doesn't
' reduce empty elements to the preferred <xxx/> form. I don't
' know whether it's the XSLT or the engine itself. I couldn't
' figure out a solution in reasonable time, so ... **TODO**
'
Private Sub UpdateConfig(sSrc As String)
    Dim ss As MSXML2.DOMDocument40
    Dim res As MSXML2.DOMDocument40
    Dim U As New Util
    Dim i As Integer
    
    Set ss = New MSXML2.DOMDocument40                       ' Load up the XSL stylesheet
    ss.async = False
    ss.loadXML INDENT_XSL
    Set res = New MSXML2.DOMDocument40                      ' Set up the results DOM
    res.async = False
    m_XML.transformNodeToObject ss, res                     ' Transform with XSLT
    res.save m_sConfFilePath                                ' Save back to file (may raise!)
'     m_XML.save m_sConfFilePath
     
End Sub

'---------------------------------------------------------------------------
'
'   LoadConfig() - Load the XML config data if needed
'
' If needed, finds the special folder path and creates the config file,
' loads it into the DOMDocument m_XML.
'---------------------------------------------------------------------------
Public Sub LoadConfigIf(sSrc As String)
    Dim FSO As New Scripting.FileSystemObject
    Dim confStream As Scripting.TextStream
    Dim idl As Long, aPath As String, path As String
    Dim tNode As MSXML2.IXMLDOMNode
    Dim buf As String
    
    If Not m_XML Is Nothing Then Exit Sub                   ' [RET] Already loaded & ready
    
    '
    ' Get the special folder path for All Users\Application Data
    '
    If SHGetSpecialFolderLocation(0&, CSIDL_COMMON_APPDATA, idl) <> 0 Then    ' Get PIDL for folder
        Err.Raise SCODE_SPECFLD_FAIL, sSrc, MSG_SPECFLD_FAIL ' Oops, raise
    End If
    aPath = Space$(MAX_PATH)                                ' Make a big buffer for Win32/Shell
    If SHGetPathFromIDList(idl, aPath) <> 1 Then            ' Get the text path for the folder
        LocalFree idl                                       ' Oops, release this BSTR
        Err.Raise SCODE_SPECFLD_FAIL, sSrc, MSG_SPECFLD_FAIL ' Raise
    End If
    LocalFree idl                                           ' Release this BSTR
    path = Left$(aPath, InStr(aPath, Chr$(0)) - 1)          ' Extract path as string
    '
    ' Create the Application Date\ASCOM folder if needed
    '
    If Not FSO.FolderExists(path & "\ASCOM") Then           ' If ASCOM subfolder doesn't exist
        FSO.CreateFolder path & "\ASCOM"                    ' Make it
    End If
    m_sConfFilePath = path & "\ASCOM\Config.xml"
    '
    ' Now create the Application Data\ASCOM\Config.xml file if needed
    '
    If Not FSO.FileExists(m_sConfFilePath) Then
        Set confStream = FSO.CreateTextFile(m_sConfFilePath)
        confStream.WriteLine "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
        confStream.WriteLine "<ASCOM version=""1.0"">"
        confStream.WriteLine Chr$(9) & _
            "<description>ASCOM master driver configuration database</description>"
        confStream.WriteLine Chr$(9) & "<chooser/>"
        confStream.WriteLine "</ASCOM>"
        confStream.Close
    End If
    Set FSO = Nothing
    
    Set m_XML = New MSXML2.DOMDocument40
    m_XML.setProperty "SelectionLanguage", "XPath"          ' Enable XPath searching
    m_XML.async = False                                     ' Wait till load completes!
    If Not m_XML.Load(m_sConfFilePath) Then
        Err.Raise SCODE_XML_LOADFAIL, sSrc, MSG_XML_LOADFAIL & m_sConfFilePath
    End If

    If m_XML.parseError.errorCode = 0 Then                  ' If parsed OK
        'Parse data - get the root node
        Set tNode = m_XML.documentElement                   ' Root element (ASCOM)
        If tNode.baseName <> "ASCOM" Then                   ' If no ASCOM root node
            Err.Raise SCODE_CORRUPT_CONFIG, , _
                      MSG_CORRUPT_CONFIG & "The top level node is not ASCOM"
        End If
        Exit Sub                                       ' [RET] DONE!
    Else
        '
        ' The parseError message already has a CRLF at the end.
        ' Trim$() won't remove tabs @#$%^& so use RegExp.Replace()
        '
        Dim rx As New RegExp
        rx.Pattern = "^\s*"
        buf = rx.Replace(m_XML.parseError.srcText, "")
        Err.Raise SCODE_CORRUPT_CONFIG, , _
            MSG_CORRUPT_CONFIG & "invalid at line " & _
            CStr(m_XML.parseError.Line) & vbCrLf & _
            "Reason: " & m_XML.parseError.reason & _
            "Offending text: " & buf
    End If

End Sub

