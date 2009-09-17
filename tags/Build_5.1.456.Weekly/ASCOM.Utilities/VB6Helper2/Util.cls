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
'   ============
'   REGISTRY.BAS
'   ============
'
' Written:  31-Aug-2004   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-Aug-04 rbd     Initial edit, from the code in ACP.
' -----------------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Private m_SerTraceFile As String
Private m_fSafeForScripting As Boolean

Private Reg As Object

Private Sub Class_Initialize()
    Call InitTZ                                     ' Initialize Time Zone Info
    m_SerTraceFile = "C:\SerialTrace.txt"           ' Default trace file
    m_fSafeForScripting = True                      ' Mark safe for scripting from web pages
    
    Set Reg = CreateObject("DriverHelper.ProfileAccess")
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

    PlatformVersion = Reg.GetProfile("", "PlatformVersion", App.Title)
    
End Property


'---------------------------------------------------------------------------
' PROPERTY: SerialTraceFile
'
' DESCRIPTION:
'---------------------------------------------------------------------------
Public Property Get SerialTraceFile() As String
Attribute SerialTraceFile.VB_Description = "Change the serial trace file (default C:\\SerialTrace.txt)"

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

    If Reg.GetProfile("", "SerTraceFile", App.Title) <> "" Then
        SerialTrace = True
    Else
        SerialTrace = False
    End If
    
End Property

Public Property Let SerialTrace(ByVal newVal As Boolean)

    If newVal Then
        Reg.WriteProfile "", "SerTraceFile", m_SerTraceFile, App.Title
    Else
        Reg.WriteProfile "", "SerTraceFile", "", App.Title
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



