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
Attribute VB_Description = "ASCOM Scope Driver Helper Utilities Object"
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
'   ========
'   UTIL.CLS
'   ========
'
' Implementation of the ASCOM DriverHelper Util class.
'
' Written:  21-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 21-Aug-00 rbd     Initial edit
' 15-Oct-00 rbd     Fix sexagesimal conversions
' 02-Nov-00 rbd     Slim down this object, remove gold-plate features
' 14-Nov-00 rbd     Allow negative values for HoursToHMS() (and thus
'                   for DegreesToHMS()). Add optional delimiters
'                   to DegToDMS, HoursToHMS(), and DegToHMS() making
'                   them MUCH more flexible. Retype the Format()
'                   optional arguments as the enums that I was
'                   supposed to use.
' 12-Dec-00 rbd     Add optional precision to sexagesimal formatters,
'                   add HMSToDegrees() for symmetry, eliminate
'                   duplicate code in sexagesimal formatters. Add
'                   DegreesToDM() and friends.
' 21-Jan-01 rbd     Add LoadDLL() method.
' 22-Jan-01 rbd     Implement IObjectSafety
' 19-Mar-03 rbd     2.2.1 Sleep 100 for all values of WaitForMilliseconds(),
'                   greatly reducing the CPU time.
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 28-Aug-03 rbd     2.4.1 - Platform 2.4, add hi-res timer for
'                   < 500 ms in WaitForMilliseconds()
' 02-Sep-03 rbd     3.0.1 - Version change for Platform 3.0
' 01-Feb-06 cdr(rbd) 4.1.5 - Detect decimal separator '.' or ',' and
'                   adjust DMS number RegExp accordingly. Clever Chris!
' 10-Apr-06 rbd     4.1.6 - Fix WaitForMilliseconds for exactly 500
'                   millisecond delay.
' 01-Jan-07 rbd     5.0.1 - Version change for Platform 5.0
'---------------------------------------------------------------------
Option Explicit

Implements IObjectSafety

Public Enum MessageBoxOptions
    mbOKOnly = &H0&
    mbOKCancel = &H1&
    mbAbortRetryIgnore = &H2&
    mbYesNoCancel = &H3&
    mbYesNo = &H4&
    mbRetryCancel = &H5&

    mbIconStop = &H10&
    mbIconQuestion = &H20&
    mbIconExclamation = &H30&
    mbIconInformation = &H40&

    mbDefButton1 = &H0&
    mbDefButton2 = &H100&
    mbDefButton3 = &H200&

    mbAbblicationModal = &H0&
    mbSystemModal = &H1000&
    
    mbNoFocus = &H8000&
    mbSetForeground = &H10000
    mbDefaultDesktopOnly = &H20000
End Enum

Public Enum MessageBoxAnswers
    mbOK = 1
    mbCancel = 2
    mbAbort = 3
    mbRetry = 4
    mbIgnore = 5
    mbYes = 6
    mbNo = 7
End Enum

Public Enum FirstDayOfWeek
    fmtUseSystemDayOfWeek = 0
    fmtSunday = 1
    fmtMonday = 2
    fmtTuesday = 3
    fmtWednesday = 4
    fmtThursday = 5
    fmtFriday = 6
    fmtSaturday = 7
End Enum

Public Enum FirstWeekOfYear
    fmtUseSystem = 0
    fmtFirstJan1 = 1
    fmtFirstFourDays = 2
    fmtFirstFullWeek = 3
End Enum

Private m_fSafeForScripting As Boolean

Private Declare Sub SleepEx Lib "kernel32" (ByVal dwMilliseconds As Long, _
                            ByVal bAlertable As Boolean)
Private Declare Function GetTickCount Lib "kernel32" () As Long
Private Declare Function LoadLibrary Lib "kernel32" Alias "LoadLibraryA" _
                            (ByVal path As String) As Long

Private Const MAX_HIRES_MILLISECONDS As Long = 500  ' Max interval to use hi-res timer

Private Const TIME_ONESHOT As Long = &H0
Private Const TIME_PERIODIC As Long = &H1
Private Const TIME_CALLBACK_FUNCTION As Long = &H0
Private Const TIME_CALLBACK_EVENT_SET As Long = &H10
Private Const TIME_CALLBACK_EVENT_PULSE As Long = &H20

Private Declare Function CreateEvent Lib "kernel32" Alias "CreateEventA" _
                            (ByVal lpEventAttributes As Long, ByVal bManualReset As Long, _
                             ByVal bInitialState As Long, ByVal lpName As Any) As Long
Private Declare Function WaitForSingleObject Lib "kernel32" _
                            (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" _
                            (ByVal hObject As Long) As Long
Private Declare Function TimeSetEvent Lib "winmm" Alias "timeSetEvent" _
                            (ByVal uDelay As Long, ByVal uResolution As Long, _
                             ByVal fptc As Long, ByVal dwUser As Long, _
                             ByVal fuEvent As Long) As Long
Private Declare Function TimeKillEvent Lib "winmm" Alias "timeKillEvent" _
                            (ByVal uTimerID As Long) As Long
Private m_hMultiTimerEvent As Long


Private Sub Class_Initialize()

    m_fSafeForScripting = True
    m_hMultiTimerEvent = CreateEvent(0&, 0&, 0&, 0&)    ' Auto-reset, unnamed, initially nonsignaled

End Sub

Private Sub Class_Terminate()

    Call CloseHandle(m_hMultiTimerEvent)
    
End Sub

'
' This will convert virtually anything resembling a sexagesimal
' format number into a real number. The input may even be missing
' the seconds or even the minutes part.
'
Public Function DMSToDegrees(ByVal DMS As String) As Double
Attribute DMSToDegrees.VB_Description = "Convert sexagesimal degrees to binary double-precision degrees"
    Dim sg As Integer
    Dim rx As RegExp
    Dim ms As MatchCollection
    
    DMS = Trim$(DMS)                        ' Just in case...
    If Left$(DMS, 1) = "-" Then
        sg = -1
        DMS = Right$(DMS, (Len(DMS) - 1))               ' Remove '-'
    Else
        sg = 1
    End If
    
    Set rx = New RegExp
'    rx.Pattern = "[0-9\.]+"                             ' RX for number groups
    '
    ' Thanks to Chris Rowland, this allows conversions for systems
    ' which use ',' -or '.' as the decimal point! Quite clever!!
    '
    If InStr(CStr(1.1), ",") > 0 Then
        rx.Pattern = "[0-9\,]+"
    Else
        rx.Pattern = "[0-9\.]+"
    End If
    rx.IgnoreCase = True
    rx.Global = True
    Set ms = rx.Execute(DMS)                            ' Find all number groups
    DMSToDegrees = 0#                                   ' Assume no numbers at all
    If ms.Count > 0 Then                                ' At least one numeric part
        DMSToDegrees = CDbl(ms.Item(0).Value)           ' Degrees
        If ms.Count > 1 Then                            ' At least 2 numeric parts
            DMSToDegrees = DMSToDegrees + (CDbl(ms.Item(1).Value) / 60#)    ' Minutes
            If ms.Count > 2 Then                        ' All three parts present
                DMSToDegrees = DMSToDegrees + (CDbl(ms.Item(2).Value) / 3600#)  ' Seconds
            End If
        End If
    End If
    DMSToDegrees = sg * DMSToDegrees                    ' Apply sign
    
End Function

Public Function HMSToHours(ByVal HMS As String) As Double
Attribute HMSToHours.VB_Description = "Convert sexagesimal hours to binary double-precision hours"
    
    HMSToHours = DMSToDegrees(HMS)

End Function

Public Function HMSToDegrees(ByVal HMS As String) As Double
Attribute HMSToDegrees.VB_Description = "Convert sexagesimal hours to binary double-precision degrees"

    HMSToDegrees = DMSToDegrees(HMS) * 15#
    
End Function

'
' Convert a real number to sexagesimal whole, minutes, seconds. Allow
' specifying the number of decimal digits on seconds. Called by
' HoursToHMS below, which just has different default delimiters.
'
Public Function DegreesToDMS(ByVal Degrees As Double, _
                            Optional ByVal DegDelim As String = "° ", _
                            Optional ByVal MinDelim As String = "' ", _
                            Optional ByVal SecDelim As String = """", _
                            Optional ByVal SecDecimalDigits As Integer = 0) As String
Attribute DegreesToDMS.VB_Description = "Convert degrees to sexagesimal degrees, minutes, and seconds"
    Dim w As String
    Dim m As String
    Dim s As String
    Dim n As Boolean
    Dim f As String
    Dim i As Integer
    
    If Degrees < 0# Then
        Degrees = -Degrees
        n = True
    Else
        n = False
    End If
    
    w = Format$(Fix(Degrees), "00")                     ' Whole part
    Degrees = (Degrees - CDbl(w)) * 60#                 ' Minutes
    m = Format$(Fix(Degrees), "00")                     ' Integral minutes
    Degrees = (Degrees - CDbl(m)) * 60#                 ' Seconds
    
    If SecDecimalDigits = 0 Then                        ' If no decimal digits wanted
        f = "00"                                        ' No decimal point or decimal digits
    Else                                                ' Decimal digits on seconds
        f = "00."                                       ' Format$ string
        For i = 1 To SecDecimalDigits
            f = f & "0"
        Next
    End If
    
    s = Format$(Degrees, f)                             ' Format seconds with requested decimal digits
    If Left$(s, 2) = "60" Then                          ' If seconds got rounded to 60
        s = Format$(0, f)                               ' Seconds are 0
        m = Format$((CInt(m) + 1), "00")                ' Carry to minutes
        If m = "60" Then                                ' If minutes got rounded to 60
            m = "00"                                    ' Minutes are 0
            w = Format$((CInt(w) + 1), "00")            ' Carry to whole part
        End If
    End If
    
    DegreesToDMS = w & DegDelim & m & MinDelim & s & SecDelim
    If n Then DegreesToDMS = "-" & DegreesToDMS
    
End Function

Public Function HoursToHMS(ByVal Hours As Double, _
                            Optional ByVal HrsDelim As String = ":", _
                            Optional ByVal MinDelim As String = ":", _
                            Optional ByVal SecDelim As String = "", _
                            Optional ByVal SecDecimalDigits As Integer = 0) As String
Attribute HoursToHMS.VB_Description = "Convert hours to sexagesimal hours, minutes, and seconds"
    
    HoursToHMS = DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, SecDecimalDigits)
    
End Function

Public Function DegreesToHMS(ByVal Degrees As Double, _
                            Optional ByVal HrsDelim As String = ":", _
                            Optional ByVal MinDelim As String = ":", _
                            Optional ByVal SecDelim As String = "", _
                            Optional ByVal SecDecimalDigits As Integer = 0) As String
Attribute DegreesToHMS.VB_Description = "Convert degrees to sexagesimal hours, minutes, and seconds"
    
    DegreesToHMS = DegreesToDMS(Degrees / 15#, HrsDelim, MinDelim, SecDelim, SecDecimalDigits)
       
End Function

'
' Convert a real number to sexagesimal whole, minutes. Allow
' specifying the number of decimal digits on minutes. Called by
' HoursToHM below, which just has different default delimiters.
'
Public Function DegreesToDM(ByVal Degrees As Double, _
                            Optional ByVal DegDelim As String = "° ", _
                            Optional ByVal MinDelim As String = "'", _
                            Optional ByVal MinDecimalDigits As Integer = 0) As String
Attribute DegreesToDM.VB_Description = "Convert degrees to sexagesimal degrees and minutes"
    Dim w As String
    Dim m As String
    Dim n As Boolean
    Dim f As String
    Dim i As Integer
    
    If Degrees < 0# Then
        Degrees = -Degrees
        n = True
    Else
        n = False
    End If
    
    w = Format$(Fix(Degrees), "00")                     ' Whole part
    Degrees = (Degrees - CDbl(w)) * 60#                 ' Minutes
    
    If MinDecimalDigits = 0 Then                        ' If no decimal digits wanted
        f = "00"                                        ' No decimal point or decimal digits
    Else                                                ' Decimal digits on minutes
        f = "00."                                       ' Format$ string
        For i = 1 To MinDecimalDigits
            f = f & "0"
        Next
    End If
    
    m = Format$(Degrees, f)                             ' Format minutes with requested decimal digits
    If Left$(m, 2) = "60" Then                          ' If minutes got rounded to 60
        m = Format$(0, f)                               ' minutes are 0
        w = Format$((CInt(w) + 1), "00")                ' Carry to whole part
    End If
    
    DegreesToDM = w & DegDelim & m & MinDelim
    If n Then DegreesToDM = "-" & DegreesToDM
    
End Function

Public Function HoursToHM(ByVal Hours As Double, _
                            Optional ByVal HrsDelim As String = ":", _
                            Optional ByVal MinDelim As String = "", _
                            Optional ByVal MinDecimalDigits As Integer = 0) As String
Attribute HoursToHM.VB_Description = "Convert hours to sexagesimal hours and minutes"
    
    HoursToHM = DegreesToDM(Hours, HrsDelim, MinDelim, MinDecimalDigits)
    
End Function

Public Function DegreesToHM(ByVal Degrees As Double, _
                            Optional ByVal HrsDelim As String = ":", _
                            Optional ByVal MinDelim As String = "", _
                            Optional ByVal MinDecimalDigits As Integer = 0) As String
Attribute DegreesToHM.VB_Description = "Convert degrees to sexagesimal hours and minutes"
    
    DegreesToHM = DegreesToDM(Degrees / 15#, HrsDelim, MinDelim, MinDecimalDigits)
       
End Function

Public Sub Yield()
Attribute Yield.VB_Description = "Yields for events and the user interface. Use this in scripts as often as practical."

    SleepEx 10, True
    DoEvents
    
End Sub

Public Sub WaitForMilliseconds(ByVal Milliseconds As Long)
Attribute WaitForMilliseconds.VB_Description = "Pauses for a given interval in milliseconds."
    Dim ts As Long
    Dim delay As Long
    Dim uEventID As Long
    
    If (Milliseconds = 0) Or (Milliseconds > MAX_HIRES_MILLISECONDS) Then
        ts = GetTickCount()
        While (GetTickCount() - ts) < Milliseconds
            Call Yield
        Wend
    Else
        '
        ' "Short" wait, use the multimedia timer and put the
        ' app to sleep waiting on the completion event.
        '
        uEventID = TimeSetEvent(Milliseconds, 0&, m_hMultiTimerEvent, 0&, _
                                (TIME_ONESHOT + TIME_CALLBACK_EVENT_SET))
        If uEventID = 0 Then Err.Raise SCODE_TIMER_FAIL, ERR_SOURCE_UTIL, MSG_TIMER_FAIL
        Call WaitForSingleObject(m_hMultiTimerEvent, 4 * MAX_HIRES_MILLISECONDS)
        Call TimeKillEvent(uEventID)
    End If
    
End Sub

'
' the default mbSetForeground forces the message box to the foreground. This is
' also OR'ed into the passed buttons to assure the box gets seen.
'
Function MessageBox(ByVal Message As String, _
                    Optional ByVal Options As MessageBoxOptions = mbSetForeground, _
                    Optional ByVal title As String = "ASCOM Util Object") As MessageBoxAnswers
Attribute MessageBox.VB_Description = "Display an alert box with configurable icon, text, caption, and button set. Return an answer appropriate to selected button set."
                    
    MessageBox = MsgBox(Message, (Options Or mbSetForeground), title)
    
End Function
                    
Public Function FormatVar(ByVal Value As Variant, ByVal FormatStr As String, _
                            Optional ByVal FirstDayOfWeek As FirstDayOfWeek = vbUseSystem, _
                            Optional ByVal FirstWeekOfYear As FirstWeekOfYear = vbUseSystem) As String
Attribute FormatVar.VB_Description = "Returns a string containing a value which is formatted according to instructions contained in a format expression (string)."
                            
    FormatVar = Format$(Value, FormatStr, FirstDayOfWeek, FirstWeekOfYear)
    
End Function

Public Function LTrimString(ByVal Text As String) As String
Attribute LTrimString.VB_Description = "Return a copy of a string with leading white-space removed"

    LTrimString = LTrim$(Text)
    
End Function

Public Function RTrimString(ByVal Text As String) As String
Attribute RTrimString.VB_Description = "Return a copy of a string with trailing white-space removed"

    RTrimString = RTrim$(Text)
    
End Function

Public Function TrimString(ByVal Text As String) As String
Attribute TrimString.VB_Description = "Return a copy of a string with leading and trailing white-space removed"

    TrimString = Trim$(Text)
    
End Function

'
' Load a DLL. Raise an error if loading fails.
'
Public Sub LoadDLL(ByVal File As String)
Attribute LoadDLL.VB_Description = "Load a dynamic link library, given its full file pathname."
    
    If (LoadLibrary(File) = 0) Then _
        Err.Raise SCODE_DLL_LOADFAIL, , File & " failed to load."
    
End Sub

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


