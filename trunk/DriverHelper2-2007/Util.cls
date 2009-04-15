VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Util"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Driver Helper-2 Auxiliary Utility Object"
'---------------------------------------------------------------------
' Copyright © 2004 DC-3 Dreams, SP, Mesa, AZ
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DC-3 DREAMS, SP. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ========
'   UTIL.BAS
'   ========
'
' Written:  31-Aug-2004   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-Aug-04 rbd     Initial edit, from the code in ACP.
' 06-Apr-07 rbd     Support for new XML based Config layer. Change trace
'                   file default location to Desktop
' 12-Apr-07 rbd     Use new Helper.Profile's secret back door to ASCOM root.
'                   No longer need config R/W here (it's sentralized there
'                   for the new XML config info).
' -----------------------------------------------------------------------------
Option Explicit

'
' Win32 functions and constants needed for special folders
' The Shell API is the right way to get these! Do not use
' environment strings!!!
'
' CSIDLs are listed only for those that we might use
'
Private Const CSIDL_PERSONAL As Long = &H5                  ' Specific User\My Documents
Private Const CSIDL_DESKTOPDIRECTORY As Long = &H10         ' Specific User\Desktop
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

Implements IObjectSafety

Private m_fSafeForScripting As Boolean
Private m_SerTraceFile As String
Private m_Profile As DriverHelper.Profile

Private Sub Class_Initialize()
   
    Call InitTZ                                     ' Initialize Time Zone Info
    m_fSafeForScripting = True                      ' Mark safe for scripting from web pages
    m_SerTraceFile = ""                             ' Force generation of default file
    Set m_Profile = New DriverHelper.Profile        ' For XML config access

End Sub

'
' CreDefTracePath() = Create the default trace file pathname
'
' Allows deferred creation so doesn't always run when serial created
'
Private Sub CreateDefaultFilePathIf()
    Dim idl As Long, aPath As String

    If m_SerTraceFile <> "" Then Exit Sub                   ' Only if no path already
    '
    ' Get the special folder path for user's desktop, add the default file name
    '
    If SHGetSpecialFolderLocation(0&, CSIDL_DESKTOPDIRECTORY, idl) <> 0 Then    ' Get PIDL for folder
        Err.Raise SCODE_SPECFLD_FAIL, ERR_SOURCE_UTIL, MSG_SPECFLD_FAIL ' Oops, raise
    End If
    aPath = Space$(MAX_PATH)                                ' Make a big buffer for Win32/Shell
    If SHGetPathFromIDList(idl, aPath) <> 1 Then            ' Get the text path for the folder
        LocalFree idl                                       ' Oops, release this BSTR
        Err.Raise SCODE_SPECFLD_FAIL, ERR_SOURCE_UTIL, MSG_SPECFLD_FAIL ' Raise
    End If
    LocalFree idl                                           ' Release this BSTR
    m_SerTraceFile = Left$(aPath, InStr(aPath, Chr$(0)) - 1) ' Extract path as string
    m_SerTraceFile = m_SerTraceFile & "\SerialTrace.txt"

End Sub

' ================
' PUBLIC INTERFACE
' ================

'---------------------------------------------------------------------------
' PROPERTY: PlatformVersion
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get PlatformVersion() As String
Attribute PlatformVersion.VB_Description = "String containing the current Platform version in m.n form"

    PlatformVersion = m_Profile.getValue("#root#", "PlatformVersion")
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: SerialTraceFile
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get SerialTraceFile() As String
Attribute SerialTraceFile.VB_Description = "Change the serial trace file (default C:\\SerialTrace.txt)"

    CreateDefaultFilePathIf
    SerialTraceFile = m_SerTraceFile
    
End Property

Public Property Let SerialTraceFile(ByVal newVal As String)

    m_SerTraceFile = newVal

End Property


'---------------------------------------------------------------------------
' PROPERTY: SerialTrace
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get SerialTrace() As Boolean
Attribute SerialTrace.VB_Description = "Enable/disable serial I/O tracing"

    If m_Profile.getValue("#root#", "SerTraceFile") <> "" Then
        SerialTrace = True
    Else
        SerialTrace = False
    End If
    
End Property

Public Property Let SerialTrace(ByVal newVal As Boolean)

    CreateDefaultFilePathIf
    If newVal Then
        Call m_Profile.WriteValue("#root#", "SerTraceFile", m_SerTraceFile)
    Else
        Call m_Profile.WriteValue("#root#", "SerTraceFile", "")
    End If
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: TimeZoneName
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get TimeZoneName() As String
Attribute TimeZoneName.VB_Description = "String containing the name of the computer's time zone"

    TimeZoneName = GetTimeZoneName()
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: TimeZoneOffset
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get TimeZoneOffset() As Double
Attribute TimeZoneOffset.VB_Description = "UTC offset (hours) for the computer's clock such that UTC = local + offset"

    TimeZoneOffset = GetTimeZoneOffset()
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: UTCDate
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get UTCDate() As Date
Attribute UTCDate.VB_Description = "The current UTC Date"

    UTCDate = NowUTC()
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: JulianDate
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get JulianDate() As Double
Attribute JulianDate.VB_Description = "The current Julian date"

    JulianDate = Me.DateUTCToJulian(NowUTC())
    
End Property


'---------------------------------------------------------------------------
' METHOD: DateLocalToJulian()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateLocalToJulian(ByVal LocDate As Date) As Double
Attribute DateLocalToJulian.VB_Description = "Convert local-time Date to Julian date"

    DateLocalToJulian = Me.DateUTCToJulian(CvtUTC(LocDate))
    
End Function


'---------------------------------------------------------------------------
' METHOD: DateJulianToLocal()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateJulianToLocal(ByVal JD As Double) As Date
Attribute DateJulianToLocal.VB_Description = "Convert Julian date to local-time Date"

    DateJulianToLocal = CvtLocal(Me.DateJulianToUTC(JD))
    
End Function


'---------------------------------------------------------------------------
' METHOD: DateUTCToJulian()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateUTCToJulian(ByVal UTCDate As Date) As Double
Attribute DateUTCToJulian.VB_Description = "Convert UTC Date toJulian date"
    
    DateUTCToJulian = CDbl(UTCDate) + 2415018.5
    
End Function


'---------------------------------------------------------------------------
' METHOD: DateJulianToUTC()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateJulianToUTC(ByVal JD As Double) As Date
Attribute DateJulianToUTC.VB_Description = "Convert Julian date to UTC Date"

    DateJulianToUTC = CDate(JD - 2415018.5)
    
End Function


'---------------------------------------------------------------------------
' METHOD: DateUTCToLocal()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateUTCToLocal(ByVal d As Date) As Date
Attribute DateUTCToLocal.VB_Description = "Convert UTC Date to local-time Date"

    DateUTCToLocal = CvtLocal(d)
    
End Function


'---------------------------------------------------------------------------
' METHOD: DateLocalToUTC()
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Function DateLocalToUTC(ByVal d As Date) As Date
Attribute DateLocalToUTC.VB_Description = "Convert local-time Date to UTC Date"

    DateLocalToUTC = CvtUTC(d)
    
End Function


Private Sub IObjectSafety_GetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByRef pdwSupportedOptions As Long, _
                                                    ByRef pdwEnabledOptions As Long)

    Call GetInterfaceSafetyOptions(riid, _
                                pdwSupportedOptions, _
                                pdwEnabledOptions, _
                                m_fSafeForScripting)
                            
End Sub

Private Sub IObjectSafety_SetInterfaceSafetyOptions(ByVal riid As Long, _
                                                    ByVal dwOptionsSetMask As Long, _
                                                    ByVal dwEnabledOptions As Long)

    Call SetInterfaceSafetyOptions(riid, _
                                dwOptionsSetMask, _
                                dwEnabledOptions, _
                                m_fSafeForScripting)

End Sub



