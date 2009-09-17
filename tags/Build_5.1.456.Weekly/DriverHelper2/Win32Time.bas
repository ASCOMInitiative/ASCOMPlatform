Attribute VB_Name = "Win32Time"
'---------------------------------------------------------------------
' Copyright © 2004 DC-3 Dreams, SP, Mesa, AZ
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DC-3 DREAMS, SP MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   =============
'   WIN32TIME.BAS
'   =============
'
' System time zone functions
'
' Written:  31-Aug-2004   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-Aug-04 rbd     Initial edit
' 05-Apr-05 rbd     Fix GetTimeZoneOffset() for DST #$%^&*
' 07-Apr-05 rbd     Fix other conversions to use the fixed
'                   GetTimeZoneOffset().
'---------------------------------------------------------------------
Option Explicit

Type SYSTEMTIME
    wYear As Integer
    wMonth As Integer
    wDayOfWeek As Integer
    wDay As Integer
    wHour As Integer
    wMinute As Integer
    wSecond As Integer
    wMilliseconds As Integer
End Type

Private Const TIME_ZONE_ID_UNKNOWN As Long = 0
Private Const TIME_ZONE_ID_STANDARD As Long = 1
Private Const TIME_ZONE_ID_DAYLIGHT As Long = 2

Private Type TIME_ZONE_INFORMATION
    Bias As Long
    StandardName As String * 64         ' Unicode string
    StandardDate As SYSTEMTIME
    StandardBias As Long
    DaylightName As String * 64         ' Unicode string
    DaylightDate As SYSTEMTIME
    DaylightBias As Long
End Type

Private Declare Function GetTimeZoneInformation Lib "kernel32" _
                (ByRef tzi As TIME_ZONE_INFORMATION) As Long

Private m_TZInfo As TIME_ZONE_INFORMATION
Private m_TZID As Long

'------------------------------------------------------------------------
' FUNCTION    : InitTZ()
'
' PURPOSE     : Initialize TimeZoneINfo just once (called from Util
'               object's Class_Init())
'------------------------------------------------------------------------
Public Sub InitTZ()

     m_TZID = GetTimeZoneInformation(m_TZInfo)
     
End Sub

'------------------------------------------------------------------------
' FUNCTION    : GetTimeZoneOffset()
'
' PURPOSE     : Return the time zone offset in hours, such that
'               UTC - local + offset
'------------------------------------------------------------------------
Public Function GetTimeZoneOffset() As Double

    GetTimeZoneOffset = m_TZInfo.Bias
    Select Case m_TZID
        Case TIME_ZONE_ID_STANDARD:
            GetTimeZoneOffset = GetTimeZoneOffset + m_TZInfo.StandardBias
        Case TIME_ZONE_ID_DAYLIGHT:
            GetTimeZoneOffset = GetTimeZoneOffset + m_TZInfo.DaylightBias
    End Select
    GetTimeZoneOffset = GetTimeZoneOffset / 60#     ' Return hours
    
End Function

'------------------------------------------------------------------------
' FUNCTION    : GetTimeZoneName()
'
' PURPOSE     : Use GetTimeZoneInfo to determine the time zone for this
'               system, including daylight effects, if any.
'------------------------------------------------------------------------
Public Function GetTimeZoneName() As String
    
    Select Case m_TZID
        Case TIME_ZONE_ID_UNKNOWN:
            GetTimeZoneName = StrConv(m_TZInfo.StandardName, vbFromUnicode) ' No daylight in current zone
        Case TIME_ZONE_ID_STANDARD:
            GetTimeZoneName = StrConv(m_TZInfo.StandardName, vbFromUnicode)
        Case TIME_ZONE_ID_DAYLIGHT:
            GetTimeZoneName = StrConv(m_TZInfo.DaylightName, vbFromUnicode)
    End Select
    
    '
    ' Trim trailing garbage
    '
    GetTimeZoneName = Left$(GetTimeZoneName, _
                            InStr(GetTimeZoneName, Chr$(0)) - 1)
    
End Function

'------------------------------------------------------------------------
' FUNCTION    : NowUTC()
'
' PURPOSE     : Returns a current UTC Date
'------------------------------------------------------------------------
Public Function NowUTC() As Date
    
    NowUTC = CDate(Now + (GetTimeZoneOffset() / 24#))
    
End Function

'------------------------------------------------------------------------
' FUNCTION    : CvtUTC()
'
' PURPOSE     : Returns a UTC Date for the given local Date
'------------------------------------------------------------------------
Public Function CvtUTC(d As Date) As Date

    CvtUTC = CDate(d + (GetTimeZoneOffset() / 24#))
    
End Function

'------------------------------------------------------------------------
' FUNCTION    : CvtLocal()
'
' PURPOSE     : Returns a Local Date for the given UTC Date
'------------------------------------------------------------------------
Public Function CvtLocal(d As Date) As Date

    CvtLocal = CDate(d - (GetTimeZoneOffset() / 24#))
    
End Function


