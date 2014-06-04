'Implements the Util component

Option Strict On
Option Explicit On
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices

''' <summary>
''' Provides a set of utility functions for ASCOM clients and drivers
''' </summary>
''' <remarks></remarks>
<Guid("E861C6D8-B55B-494a-BC59-0F27F981CA98"), _
ComVisible(True), _
ClassInterface(ClassInterfaceType.None)> _
Public Class Util
    Implements IUtil, IUtilExtra, IDisposable
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
    ' 23-Feb-09 pwgs    5.1.0 - Refactored for Utilities
    '---------------------------------------------------------------------

    Private m_StopWatch As Stopwatch = New Stopwatch 'Create a high resolution timing device
    Private m_SerTraceFile As String = SERIAL_DEFAULT_FILENAME 'Set the default trace file name
    Private TL As TraceLogger

    Private myProfile As RegistryAccess 'Hold the access object for the ASCOM profile store

#Region "New and IDisposable Support"
    Private disposedValue As Boolean = False        ' To detect redundant calls

    Public Sub New()
        MyBase.New()
        myProfile = New RegistryAccess
        WaitForMilliseconds(1) 'Fire off the first instance which always takes longer than the others!
        TL = New TraceLogger("", "Util")
        TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT) 'Get enabled / disabled state from the user registry
        TL.LogMessage("New", "Trace logger created OK")
    End Sub

    ' IDisposable
    ''' <summary>
    ''' Disposes of resources used by the profile object - called by IDisposable interface
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
            If Not (myProfile Is Nothing) Then
                myProfile.Dispose()
                myProfile = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of resources used by the profile object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "Util Implementation"
    ''' <summary>
    ''' Pauses for a given interval in milliseconds.
    ''' </summary>
    ''' <param name="Milliseconds">The number of milliseconds to wait</param>
    ''' <remarks>Repeatedly puts the calling Win32 process to sleep, totally freezing it, for 10 milliseconds, 
    ''' then pumps events so the script or program calling it will receive its normal flow of events, until the 
    ''' pause interval elapses. If the pause interval is 20 milliseconds or less, the sleep interval is reduced 
    ''' to 0, causing the calling Win32 process to give up control to the kernel scheduler and then immediately 
    ''' become eligible for scheduling. </remarks>
    Public Sub WaitForMilliseconds(ByVal Milliseconds As Integer) Implements IUtil.WaitForMilliseconds
        Dim EndPoint As Double
        m_StopWatch.Reset() 'Initialise from last times use
        m_StopWatch.Start() 'Start timer straight away

        If Milliseconds > 20 Then 'Wait most of the time (to within the last 20 ms) using sleep(10) to reduce CPU usage
            EndPoint = CDbl(Milliseconds - 20) * Stopwatch.Frequency / 1000.0
            Do
                Thread.Sleep(10)
                Application.DoEvents()
            Loop Until m_StopWatch.ElapsedTicks >= EndPoint
        End If
        'Calculate the final tick end point and wait using sleep(0) for maximum accuracy
        EndPoint = CDbl(Milliseconds) * Stopwatch.Frequency / 1000.0
        Do While m_StopWatch.ElapsedTicks < EndPoint
            Thread.Sleep(0)
        Loop
    End Sub
    '
    ' This will convert virtually anything resembling a sexagesimal
    ' format number into a real number. The input may even be missing
    ' the seconds or even the minutes part.
    '
    ''' <summary>
    ''' Convert sexagesimal degrees to binary double-precision degrees
    ''' </summary>
    ''' <param name="DMS">The sexagesimal input string (degrees)</param>
    ''' <returns>The double-precision binary value (degrees) represented by the sexagesimal input</returns>
    ''' <remarks><para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just 
    ''' about anything that resembles sexagesimal. Thee way they operate is to first separate the input string 
    ''' into numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
    ''' characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
    ''' numerics. </para>
    ''' <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
    ''' are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
    ''' minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
    ''' only one token is present, it is assumed to be units. Any token can have a fractionsl part. Of course it 
    ''' would not be normal (for example) for both the minutes and seconds parts to have fractional parts, but it 
    ''' would be legal. So 00:30.5:30 would convert to 1.0 unit. </para>
    ''' <para>Note that plain units, for example 23.128734523 are acceptable to the method. </para>
    ''' </remarks>
    Public Function DMSToDegrees(ByVal DMS As String) As Double Implements IUtil.DMSToDegrees
        'Refactored to use .NET regular expressions
        Dim sg As Short
        Dim rx As System.Text.RegularExpressions.Regex
        Dim ms As System.Text.RegularExpressions.MatchCollection
        Dim Pattern As String
        DMS = Trim(DMS) ' Just in case...
        If Left(DMS, 1) = "-" Then
            sg = -1
            DMS = Right(DMS, Len(DMS) - 1) ' Remove '-'
        Else
            sg = 1
        End If
        If InStr(CStr(1.1), ",") > 0 Then
            Pattern = "[0-9\,]+"
        Else
            Pattern = "[0-9\.]+"
        End If

        rx = New Regex(Pattern)
        '    rx.Pattern = "[0-9\.]+"                             ' RX for number groups
        '
        ' Thanks to Chris Rowland, this allows conversions for systems
        ' which use ',' -or '.' as the decimal point! Quite clever!!
        '
        'rx.IgnoreCas = True
        'rx.Global = True
        ms = rx.Matches(DMS) ' Find all number groups
        DMSToDegrees = 0.0# ' Assume no numbers at all
        If ms.Count > 0 Then ' At least one numeric part
            DMSToDegrees = CDbl(ms.Item(0).Value) ' Degrees
            If ms.Count > 1 Then ' At least 2 numeric parts
                DMSToDegrees = DMSToDegrees + (CDbl(ms.Item(1).Value) / 60.0#) ' Minutes
                If ms.Count > 2 Then ' All three parts present
                    DMSToDegrees = DMSToDegrees + (CDbl(ms.Item(2).Value) / 3600.0#) ' Seconds
                End If
            End If
        End If
        DMSToDegrees = sg * DMSToDegrees ' Apply sign

    End Function

    ''' <summary>
    ''' Convert sexagesimal hours to binary double-precision hours
    ''' </summary>
    ''' <param name="HMS">The sexagesimal input string (hours)</param>
    ''' <returns>The double-precision binary value (hours) represented by the sexagesimal input </returns>
    ''' <remarks>
    ''' <para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just about 
    ''' anything that resembles sexagesimal. Thee way they operate is to first separate the input string into 
    ''' numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
    ''' characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
    ''' numerics. </para>
    ''' 
    ''' <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
    ''' are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
    ''' minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
    ''' only one token is present, it is assumed to be units. Any token can have a fractionsl part. </para>
    ''' 
    ''' <para>Of course it would not be normal (for example) for both the minutes and seconds parts to have 
    ''' fractional parts, but it would be legal. So 00:30.5:30 would convert to 1.0 unit. Note that plain units, 
    ''' for example 23.128734523 are acceptable to the method. </para>
    ''' </remarks>
    Public Function HMSToHours(ByVal HMS As String) As Double Implements IUtil.HMSToHours
        HMSToHours = DMSToDegrees(HMS)
    End Function

    ''' <summary>
    ''' Convert sexagesimal hours to binary double-precision hours
    ''' </summary>
    ''' <param name="HMS">The sexagesimal input string (hours)</param>
    ''' <returns>The double-precision binary value (hours) represented by the sexagesimal input</returns>
    ''' <remarks>
    ''' <para>The sexagesimal to real conversion methods such as this one are flexible enough to convert just about 
    ''' anything that resembles sexagesimal. Thee way they operate is to first separate the input string into 
    ''' numeric "tokens", strings consisting only of the numerals 0-9, plus and minus, and period. All other 
    ''' characters are considered delimiters. Once the input string is parsed into tokens they are converted to 
    ''' numerics. </para>
    ''' 
    ''' <para>If there are more than three numeric tokens, only the first three are considered, the remainder 
    ''' are ignored. Left to right positionally, the tokens are assumed to represent units (degrees or hours), 
    ''' minutes, and seconds. If only two tokens are present, they are assumed to be units and minutes, and if 
    ''' only one token is present, it is assumed to be units. Any token can have a fractionsl part. </para>
    ''' 
    ''' <para>Of course it would not be normal (for example) for both the minutes and seconds parts to have 
    ''' fractional parts, but it would be legal. So 00:30.5:30 would convert to 1.0 unit. Note that plain units, 
    ''' for example 23.128734523 are acceptable to the method. </para>
    ''' </remarks>
    Public Function HMSToDegrees(ByVal HMS As String) As Double Implements IUtil.HMSToDegrees
        HMSToDegrees = DMSToDegrees(HMS) * 15.0#
    End Function

#Region "DegreesToDMS"
    '
    ' Convert a real number to sexagesimal whole, minutes, seconds. Allow
    ' specifying the number of decimal digits on seconds. Called by
    ' HoursToHMS below, which just has different default delimiters.
    '
    ''' <summary>
    ''' Convert degrees to sexagesimal degrees, minutes and seconds with default delimiters DD° MM' SS" 
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
    ''' characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDMS(ByVal Degrees As Double) As String Implements IUtilExtra.DegreesToDMS
        Return DegreesToDMS(Degrees, "° ", "' ", """", 0)
    End Function

    ''' <summary>
    '''  Convert degrees to sexagesimal degrees, minutes and seconds with with default minute and second delimiters MM' SS" 
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees and minutes </param>
    ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
    ''' characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String) As String Implements IUtilExtra.DegreesToDMS
        Return DegreesToDMS(Degrees, DegDelim, "' ", """", 0)
    End Function

    ''' <summary>
    '''  Convert degrees to sexagesimal degrees, minutes and seconds with default second delimiter SS" 
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees and minutes </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
    ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
    ''' characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.DegreesToDMS
        Return DegreesToDMS(Degrees, DegDelim, MinDelim, """", 0)
    End Function

    ''' <summary>
    '''  Convert degrees to sexagesimal degrees, minutes and seconds
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees and minutes </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part</param>
    ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
    ''' characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String Implements IUtilExtra.DegreesToDMS
        Return DegreesToDMS(Degrees, DegDelim, MinDelim, SecDelim, 0)
    End Function

    ''' <summary>
    '''  Convert degrees to sexagesimal degrees, minutes and seconds with specified second decimal places
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees and minutes </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part</param>
    ''' <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
    ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
    ''' characters.</para>
    ''' </remarks>
    Public Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String Implements IUtil.DegreesToDMS
        Dim w, m, s As String, n As Boolean, f As String, i As Integer

        If Degrees < 0.0# Then
            Degrees = -Degrees
            n = True
        Else
            n = False
        End If

        w = Format(Fix(Degrees), "00") ' Whole part
        Degrees = (Degrees - CDbl(w)) * 60.0# ' Minutes
        m = Format(Fix(Degrees), "00") ' Integral minutes
        Degrees = (Degrees - CDbl(m)) * 60.0# ' Seconds

        If SecDecimalDigits = 0 Then ' If no decimal digits wanted
            f = "00" ' No decimal point or decimal digits
        Else ' Decimal digits on seconds
            f = "00." ' Format$ string
            For i = 1 To SecDecimalDigits
                f = f & "0"
            Next
        End If

        s = Format(Degrees, f) ' Format seconds with requested decimal digits
        If Left(s, 2) = "60" Then ' If seconds got rounded to 60
            s = Format(0, f) ' Seconds are 0
            m = Format(CShort(m) + 1, "00") ' Carry to minutes
            If m = "60" Then ' If minutes got rounded to 60
                m = "00" ' Minutes are 0
                w = Format(CShort(w) + 1, "00") ' Carry to whole part
            End If
        End If

        DegreesToDMS = w & DegDelim & m & MinDelim & s & SecDelim
        If n Then DegreesToDMS = "-" & DegreesToDMS

    End Function
#End Region

#Region "HoursToHMS"
    ''' <summary>
    ''' Convert hours to sexagesimal hours, minutes, and seconds with default delimiters HH:MM:SS
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function HoursToHMS(ByVal Hours As Double) As String Implements IUtilExtra.HoursToHMS
        Return DegreesToDMS(Hours, ":", ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours, minutes, and seconds with default minutes and seconds delimters MM:SS
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
    ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String) As String Implements IUtilExtra.HoursToHMS
        Return DegreesToDMS(Hours, HrsDelim, ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours, minutes, and seconds with default second delimiter of null string
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds </param>
    ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.HoursToHMS
        Return DegreesToDMS(Hours, HrsDelim, MinDelim, "", 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours, minutes, and seconds
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds </param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part </param>
    ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String Implements IUtilExtra.HoursToHMS
        Return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours, minutes, and seconds with specified number of second decimal places
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds </param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part </param>
    ''' <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
    ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' </remarks>
    Public Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String Implements IUtil.HoursToHMS
        Return DegreesToDMS(Hours, HrsDelim, MinDelim, SecDelim, SecDecimalDigits)
    End Function
#End Region

#Region "DegreesToHMS"
    'Public Overloads Function DegreesToHMS(ByVal Degrees As Double, Optional ByVal HrsDelim As String = ":", Optional ByVal MinDelim As String = ":", Optional ByVal SecDelim As String = "", Optional ByVal SecDecimalDigits As Short = 0) As String Implements IUtil.DegreesToHMS
    ''' <summary>
    ''' Convert degrees to sexagesimal hours, minutes, and seconds with default delimters of HH:MM:SS
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToHMS(ByVal Degrees As Double) As String Implements IUtilExtra.DegreesToHMS
        Return DegreesToHMS(Degrees, ":", ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours, minutes, and seconds with the default second and minute delimiters of MM:SS
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String) As String Implements IUtilExtra.DegreesToHMS
        Return DegreesToHMS(Degrees, HrsDelim, ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours, minutes, and seconds with the default second delimiter SS (null string)
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds</param>
    ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.DegreesToHMS
        Return DegreesToHMS(Degrees, HrsDelim, MinDelim, "", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours, minutes, and seconds
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds</param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part </param>
    ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String Implements IUtilExtra.DegreesToHMS
        Return DegreesToHMS(Degrees, HrsDelim, MinDelim, SecDelim, 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours, minutes, and seconds with the specified number of second decimal places
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string separating minutes and seconds</param>
    ''' <param name="SecDelim">The delimiter string to append to the seconds part </param>
    ''' <param name="SecDecimalDigits">The number of digits after the decimal point on the seconds part </param>
    ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
    ''' </remarks>
    Public Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String Implements IUtil.DegreesToHMS
        Return DegreesToDMS(Degrees / 15.0#, HrsDelim, MinDelim, SecDelim, SecDecimalDigits)
    End Function

#End Region

#Region "DegreesToDM"
    ' Convert a real number to sexagesimal whole, minutes. Allow
    ' specifying the number of decimal digits on minutes. Called by
    ' HoursToHM below, which just has different default delimiters.

    ''' <summary>
    ''' Convert degrees to sexagesimal degrees and minutes with default delimiters DD° MM'
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDM(ByVal Degrees As Double) As String Implements IUtilExtra.DegreesToDM
        Return DegreesToDM(Degrees, "° ", "'", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal degrees and minutes with the default minutes delimeter MM'
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees </param>
    ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String) As String Implements IUtilExtra.DegreesToDM
        Return DegreesToDM(Degrees, DegDelim, "'", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal degrees and minutes
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes </param>
    ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.DegreesToDM
        Return DegreesToDM(Degrees, DegDelim, MinDelim, 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal degrees and minutes with the specified number of minute decimal places
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="DegDelim">The delimiter string separating degrees </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes </param>
    ''' <param name="MinDecimalDigits">The number of digits after the decimal point on the minutes part </param>
    ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' </remarks>
    Public Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String Implements IUtil.DegreesToDM
        Dim w, m, f As String, n As Boolean, i As Integer

        If Degrees < 0.0# Then
            Degrees = -Degrees
            n = True
        Else
            n = False
        End If

        w = Format(Fix(Degrees), "00") ' Whole part
        Degrees = (Degrees - CDbl(w)) * 60.0# ' Minutes

        If MinDecimalDigits = 0 Then ' If no decimal digits wanted
            f = "00" ' No decimal point or decimal digits
        Else ' Decimal digits on minutes
            f = "00." ' Format$ string
            For i = 1 To MinDecimalDigits
                f = f & "0"
            Next
        End If

        m = Format(Degrees, f) ' Format minutes with requested decimal digits
        If Left(m, 2) = "60" Then ' If minutes got rounded to 60
            m = Format(0, f) ' minutes are 0
            w = Format(CShort(w) + 1, "00") ' Carry to whole part
        End If

        DegreesToDM = w & DegDelim & m & MinDelim
        If n Then DegreesToDM = "-" & DegreesToDM

    End Function
#End Region

#Region "HoursToHM"
    ''' <summary>
    ''' Convert hours to sexagesimal hours and minutes with default delimiters HH:MM
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with an suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function HoursToHM(ByVal Hours As Double) As String Implements IUtilExtra.HoursToHM
        Return DegreesToDM(Hours, ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours and minutes with default minutes delimiter MM (null string)
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with an suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String) As String Implements IUtilExtra.HoursToHM
        Return DegreesToDM(Hours, HrsDelim, "", 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours and minutes
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
    ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with an suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.HoursToHM
        Return DegreesToDM(Hours, HrsDelim, MinDelim, 0)
    End Function

    ''' <summary>
    ''' Convert hours to sexagesimal hours and minutes with supplied number of minute decimal places
    ''' </summary>
    ''' <param name="Hours">The hours value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours </param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
    ''' <param name="MinDecimalDigits">The number of digits after the decimal point on the minutes part </param>
    ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
    ''' </remarks>
    Public Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String Implements IUtil.HoursToHM
        Return DegreesToDM(Hours, HrsDelim, MinDelim, MinDecimalDigits)
    End Function

#End Region

#Region "DegreesToHM"
    'Public Function DegreesToHM(ByVal Degrees As Double, Optional ByVal HrsDelim As String = ":", Optional ByVal MinDelim As String = "", Optional ByVal MinDecimalDigits As Short = 0) As String Implements IUtil.DegreesToHM
    ''' <summary>
    ''' Convert degrees to sexagesimal hours and minutes with default delimiters HH:MM
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function DegreesToHM(ByVal Degrees As Double) As String Implements IUtilExtra.DegreesToHM
        Return DegreesToHM(Degrees, ":", "", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours and minutes with default minute delimiter MM (null string)
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String) As String Implements IUtilExtra.DegreesToHM
        Return DegreesToHM(Degrees, HrsDelim, "", 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours and minutes
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part</param>
    ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
    ''' with suitable parameters to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String Implements IUtilExtra.DegreesToHM
        Return DegreesToHM(Degrees, HrsDelim, MinDelim, 0)
    End Function

    ''' <summary>
    ''' Convert degrees to sexagesimal hours and minutes with supplied number of minute decimal places
    ''' </summary>
    ''' <param name="Degrees">The degrees value to convert</param>
    ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
    ''' <param name="MinDelim">The delimiter string to append to the minutes part</param>
    ''' <param name="MinDecimalDigits">Number of minutes decimal places</param>
    ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
    ''' <remarks>
    ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
    ''' </remarks>
    Public Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String Implements IUtil.DegreesToHM
        Return DegreesToDM(Degrees / 15.0#, HrsDelim, MinDelim, MinDecimalDigits)
    End Function

#End Region

#End Region

#Region "Util 2 Implementation"

    ''' <summary>
    ''' Current Platform version in Major.Minor form
    ''' </summary>
    ''' <returns>Current Platform version in Major.Minor form</returns>
    ''' <remarks>Please be careful if you wish to convert this string into a number within your application 
    ''' because the ASCOM Platform is used internationally and some countries use characters other 
    ''' than point as the decimal separator. 
    ''' <para>If your application tries to convert 5.5 into a Double value when running on a PC localised to 
    ''' France, you will get an exception because the French decimal separater is comma and 5.5 is not 
    ''' a valid representation of a decimal number in that locale.</para>
    ''' <para>If you still wish to turn the Platform Version into a Double value, you can use an 
    ''' approach such as this:</para>
    ''' <code>If Double.Parse(Util.PlatformVersion, CultureInfo.InvariantCulture) &lt; 5.5 Then...</code>
    ''' <para>If you just wish to test whether the platform is greater than a particular level,
    ''' you can use the <see cref="IsMinimumRequiredVersion">IsMinimumRequiredVersion</see> method.</para>
    ''' </remarks>
    Public ReadOnly Property PlatformVersion() As String Implements IUtil.PlatformVersion
        Get
            PlatformVersion = myProfile.GetProfile("", "PlatformVersion")
            PlatformVersion = AscomSharedCode.ConditionPlatformVersion(PlatformVersion, myProfile, TL) ' Check for Forced Platform versions
            TL.LogMessage("PlatformVersion Get", PlatformVersion)
        End Get
    End Property

    ''' <summary>
    ''' Tests whether the current platform version is at least equal to the supplied major and minor 
    ''' version numbers, returns false if this is not the case
    ''' </summary>
    ''' <param name="RequiredMajorVersion">The required major version number</param>
    ''' <param name="RequiredMinorVersion">The required minor version number</param>
    ''' <returns>True if the current platform version equals or exceeds the major and minor values provided</returns>
    ''' <remarks>This function provides a simple way to test for a minimum platform level.
    ''' If for example, your application requires at least platform version 5.5 then you can use 
    ''' code such as this to make a test and display information as appropriate.
    ''' <code > Const requiredMajorVersion as Integer = 5
    ''' Const requiredMinorVersion as Integer = 5 ' Requires Platform version 5.5
    ''' Dim Utils as New ASCOM.Utilities.Util
    ''' isOK = Utils.IsMinimumRequiredVersion(requiredMajorVersion, requiredMinorVersion)
    ''' If Not isOK Then 
    '''    ' Abort, throw exception, print an error or whatever.
    '''    End
    ''' EndIf
    ''' 
    ''' </code></remarks>
    Function IsMinimumRequiredVersion(ByVal RequiredMajorVersion As Integer, ByVal RequiredMinorVersion As Integer) As Boolean Implements IUtil.IsMinimumRequiredVersion
        Dim PlatformVersion, RequiredVersion As Version
        'Create a version object from the platform version string
        PlatformVersion = New Version(myProfile.GetProfile("", "PlatformVersion"))
        'Create a version object from the supplied major and minor required version numbers
        RequiredVersion = New Version(RequiredMajorVersion, RequiredMinorVersion)

        If (PlatformVersion.CompareTo(RequiredVersion) >= 0) Then
            Return True 'Platform version is equal to or greater than the required version
        Else
            Return False 'Platform version is less than the required version
        End If
    End Function

    ''' <summary>
    ''' Change the serial trace file (default C:\SerialTrace.txt)
    ''' </summary>
    ''' <value>Serial trace file name including fully qualified path e.g. C:\SerialTrace.txt</value>
    ''' <returns>Serial trace file name </returns>
    ''' <remarks>Change this before setting the SerialTrace property to True. </remarks>
    Public Property SerialTraceFile() As String Implements IUtil.SerialTraceFile
        Get
            Return m_SerTraceFile
        End Get
        Set(ByVal Value As String)
            m_SerTraceFile = Value
        End Set
    End Property

    ''' <summary>
    ''' Enable/disable serial I/O tracing
    ''' </summary>
    ''' <value>Boolean - Enable/disable serial I/O tracing</value>
    ''' <returns>Enabled - disabled state of serial tracing</returns>
    ''' <remarks>If you want to change the serial trace file path, change the SerialTraceFile property before setting this to True. 
    ''' After setting this to True, serial trace info will be written to the last-set serial trace file. </remarks>
    Public Property SerialTrace() As Boolean Implements IUtil.SerialTrace
        Get
            If myProfile.GetProfile("", SERIAL_FILE_NAME_VARNAME) <> "" Then 'Thereis a filename so tracing is enabled
                Return True
            Else 'No filename so tracing is disabled
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            If Value Then 'We are enabling tracing so write the filename to profile
                myProfile.WriteProfile("", SERIAL_FILE_NAME_VARNAME, m_SerTraceFile)
            Else 'Disabling so write a null string instead
                myProfile.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "")
            End If
        End Set
    End Property

    ''' <summary>
    ''' The name of the computer's time zone
    ''' </summary>
    ''' <returns>The name of the computer's time zone</returns>
    ''' <remarks>This will be in the local language of the operating system, and will reflect any daylight/summer time that may be in 
    ''' effect. </remarks>
    Public ReadOnly Property TimeZoneName() As String Implements IUtil.TimeZoneName
        Get
            Return GetTimeZoneName()
        End Get
    End Property

    ''' <summary>
    ''' UTC offset (hours) for the computer's clock
    ''' </summary>
    ''' <returns>UTC offset (hours) for the computer's clock</returns>
    ''' <remarks>The offset is in hours, such that UTC = local + offset. The offset includes any daylight/summer time that may be 
    ''' in effect.</remarks>
    Public ReadOnly Property TimeZoneOffset() As Double Implements IUtil.TimeZoneOffset
        Get
            Return GetTimeZoneOffset()
        End Get
    End Property

    ''' <summary>
    ''' The current UTC Date
    ''' </summary>
    ''' <returns>The current UTC Date</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property UTCDate() As Date Implements IUtil.UTCDate
        Get
            Return Date.UtcNow
        End Get
    End Property

    ''' <summary>
    ''' Current Julian date
    ''' </summary>
    ''' <returns>Current Julian date</returns>
    ''' <remarks>This is quantised to the second in the COM component but to a small decimal fraction in the .NET component</remarks>
    Public ReadOnly Property JulianDate() As Double Implements IUtil.JulianDate
        Get
            Return Me.DateUTCToJulian(Date.UtcNow)
        End Get
    End Property

    ''' <summary>
    ''' Convert local-time Date to Julian date
    ''' </summary>
    ''' <param name="LocalDate">Date in local-time</param>
    ''' <returns>Julian date</returns>
    ''' <remarks>Julian dates are always in UTC </remarks>
    Public Function DateLocalToJulian(ByVal LocalDate As Date) As Double Implements IUtil.DateLocalToJulian
        Return Me.DateUTCToJulian(CvtUTC(LocalDate))
    End Function

    ''' <summary>
    ''' Convert Julian date to local-time Date
    ''' </summary>
    ''' <param name="JD">Julian date to convert</param>
    ''' <returns>Date in local-time for the given Julian date</returns>
    ''' <remarks>Julian dates are always in UTC</remarks>
    Public Function DateJulianToLocal(ByVal JD As Double) As Date Implements IUtil.DateJulianToLocal
        Return CvtLocal(Me.DateJulianToUTC(JD))
    End Function

    ''' <summary>
    ''' Convert UTC Date to Julian date
    ''' </summary>
    ''' <param name="UTCDate">UTC date to convert</param>
    ''' <returns>Julian date</returns>
    ''' <remarks>Julian dates are always in UTC </remarks>
    Public Function DateUTCToJulian(ByVal UTCDate As Date) As Double Implements IUtil.DateUTCToJulian
        Return UTCDate.ToOADate() + 2415018.5
    End Function

    ''' <summary>
    ''' Convert Julian date to UTC Date
    ''' </summary>
    ''' <param name="JD">Julian date</param>
    ''' <returns>Date in UTC for the given Julian date</returns>
    ''' <remarks>Julian dates are always in UTC </remarks>
    Public Function DateJulianToUTC(ByVal JD As Double) As Date Implements IUtil.DateJulianToUTC
        Return System.DateTime.FromOADate(JD - 2415018.5)
    End Function

    ''' <summary>
    ''' Convert UTC Date to local-time Date
    ''' </summary>
    ''' <param name="UTCDate">Date in UTC</param>
    ''' <returns>Date in local-time</returns>
    ''' <remarks></remarks>
    Public Function DateUTCToLocal(ByVal UTCDate As Date) As Date Implements IUtil.DateUTCToLocal
        Return CvtLocal(UTCDate)
    End Function

    ''' <summary>
    ''' Convert local-time Date to UTC Date
    ''' </summary>
    ''' <param name="LocalDate">Date in local-time</param>
    ''' <returns> Date in UTC</returns>
    ''' <remarks></remarks>
    Public Function DateLocalToUTC(ByVal LocalDate As Date) As Date Implements IUtil.DateLocalToUTC
        Return CvtUTC(LocalDate)
    End Function

    ''' <summary>
    ''' Convert a string safearray to an ArrayList that can be used in scripting languages
    ''' </summary>
    ''' <param name="stringArray">Array of strings</param>
    ''' <returns>Collection of integers</returns>
    ''' <remarks></remarks>
    Public Function ToStringCollection(ByVal stringArray As String()) As ArrayList Implements IUtil.ToStringCollection
        ToStringCollection = New ArrayList()
        For Each item As String In stringArray
            ToStringCollection.Add(item)
        Next
    End Function

    ''' <summary>
    ''' Convert an integer safearray to an ArrayList collection that can be used in scripting languages
    ''' </summary>
    ''' <param name="integerArray">Safearray of integers</param>
    ''' <returns>Colection of integers</returns>
    ''' <remarks></remarks>
    Public Function ToIntegerCollection(ByVal integerArray As Integer()) As ArrayList Implements IUtil.ToIntegerCollection
        ToIntegerCollection = New ArrayList()
        For Each item As Integer In integerArray
            ToIntegerCollection.Add(item)
        Next
    End Function

#End Region

#Region "Array To ArrAyVariant Code"

    ''' <summary>
    ''' Convert an array of .NET built-in types to an equivalent Variant arrray (array of .NET Objects)
    ''' </summary>
    ''' <param name="SuppliedObject">The array to convert to variant types</param>
    ''' <returns>A Variant array</returns>
    ''' <exception cref="InvalidValueException">If the supplied array contains elements of an unsuported Type.</exception>
    ''' <exception cref="InvalidValueException">If the array rank is outside the range 1 to 5.</exception>
    ''' <exception cref="InvalidValueException">If the supplied object is not an array.</exception>
    ''' <remarks>This function will primarily be of use to Scripting Language programmers who need to convert Camera and Video ImageArrays from their native types to Variant types. If this is not done, 
    ''' the scripting language will throw a type mismatch exception when it receives, for example, Int32 element types instead of the expected Variant types.
    ''' <para>A VBScript Camera usage example is: Image = UTIL.ArrayToVariantArray(CAMERA.ImageArray) This example assumes that the camera and utilities objects have already been created with CreateObject statements.</para>
    ''' <para>The supported .NET types are:
    ''' <list type="bullet">
    ''' <item><description>Int16</description></item>
    ''' <item><description>Int32</description></item>
    ''' <item><description>UInt16</description></item>
    ''' <item><description>UInt32</description></item>
    ''' <item><description>UInt64</description></item>
    ''' <item><description>Byte</description></item>
    ''' <item><description>SByte</description></item>
    ''' <item><description>Single</description></item>
    ''' <item><description>Double</description></item>
    ''' <item><description>Boolean</description></item>
    ''' <item><description>DateTime</description></item>
    ''' <item><description>String</description></item>
    ''' </list>
    ''' </para>
    ''' <para>The function supports arrays with 1 to 5 dimensions (Rank = 1 to 5). If the supplied array already contains elements of Variant type, it is returned as-is without any processing.</para></remarks>
    Public Function ArrayToVariantArray(ByVal SuppliedObject As Object) As <MarshalAs(UnmanagedType.SafeArray, SafeArraySubtype:=VarEnum.VT_VARIANT)> Object Implements IUtil.ArrayToVariantArray
        Dim ReturnObject As Object ' An object tp represent the Variant array
        Dim TypeOfSuppliedObject, ArrayType As Type ' Variables to hold the Type of the Array and the Type of its elements
        Dim SuppliedArray As Array ' Variable to hold the supplied array as an Array type (as opposed to Object)
        Dim ElementTypeName As String ' Variable to hold the name of the type of elements in the array
        Dim Sw As Stopwatch = New Stopwatch

        Sw.Start()

        Try
            TypeOfSuppliedObject = SuppliedObject.GetType ' Get the Type of the supplied object

            If TypeOfSuppliedObject.IsArray Then ' If the object is an array then process
                SuppliedArray = CType(SuppliedObject, Array) 'Convert the Object to an Array type
                ArrayType = SuppliedArray.GetType() ' Get the type of the array elements
                ElementTypeName = ArrayType.GetElementType.Name
                TL.LogMessage("ArrayToVariantArray", "Array Type: " & ArrayType.Name & ", Element Type: " & ElementTypeName & ", Array Rank: " & SuppliedArray.Rank)

                Select Case ElementTypeName ' Compare the supplied element type with the list of support types
                    Case GetType(Object).Name : ReturnObject = SuppliedObject ' Already a variant array so just return the original array
                    Case GetType(Int16).Name : ReturnObject = ProcessArray(Of Int16)(SuppliedObject, SuppliedArray)
                    Case GetType(Int32).Name : ReturnObject = ProcessArray(Of Int32)(SuppliedObject, SuppliedArray)
                    Case GetType(Int64).Name : ReturnObject = ProcessArray(Of Int64)(SuppliedObject, SuppliedArray)
                    Case GetType(UInt16).Name : ReturnObject = ProcessArray(Of UInt16)(SuppliedObject, SuppliedArray)
                    Case GetType(UInt32).Name : ReturnObject = ProcessArray(Of UInt32)(SuppliedObject, SuppliedArray)
                    Case GetType(UInt64).Name : ReturnObject = ProcessArray(Of UInt64)(SuppliedObject, SuppliedArray)
                    Case GetType(Byte).Name : ReturnObject = ProcessArray(Of Byte)(SuppliedObject, SuppliedArray)
                    Case GetType(SByte).Name : ReturnObject = ProcessArray(Of SByte)(SuppliedObject, SuppliedArray)
                    Case GetType(Single).Name : ReturnObject = ProcessArray(Of Single)(SuppliedObject, SuppliedArray)
                    Case GetType(Double).Name : ReturnObject = ProcessArray(Of Double)(SuppliedObject, SuppliedArray)
                    Case GetType(Boolean).Name : ReturnObject = ProcessArray(Of Boolean)(SuppliedObject, SuppliedArray)
                    Case GetType(DateTime).Name : ReturnObject = ProcessArray(Of DateTime)(SuppliedObject, SuppliedArray)
                    Case GetType(String).Name : ReturnObject = ProcessArray(Of String)(SuppliedObject, SuppliedArray)

                    Case Else ' We have a non-supported element type so throw an exception
                        TL.LogMessage("ArrayToVariantArray", "Unsupported array type: " & ElementTypeName & ", throwing exception")
                        Throw New ASCOM.InvalidValueException("Unsupported array type: " & ElementTypeName)
                End Select
            Else ' Not an array so throw an exception
                TL.LogMessage("ArrayToVariantArray", "Supplied object is not an array, throwing exception")
                Throw New ASCOM.InvalidValueException("Supplied object is not an array")
            End If

            Sw.Stop()
            TL.LogMessage("ArrayToVariantArray", "Completed processing in " & Sw.Elapsed.TotalMilliseconds.ToString("0.0") & " milliseconds")

            Return ReturnObject ' Return the variant array

        Catch ex As Exception ' Catch any exceptions, log them and return to the calling application
            TL.LogMessageCrLf("ArrayToVariantArray", "Exception: " & ex.ToString())
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Turns an array of type T into a variant array of Object
    ''' </summary>
    ''' <typeparam name="T">The type to convert to Variant</typeparam>
    ''' <param name="SuppliedObject">The supplied array of Type T as an Object</param>
    ''' <param name="SuppliedArray">The supplied array of Type T as an Array</param>
    ''' <returns>The array with all elements represented as Variant objects</returns>
    ''' <remarks>Works for 1 to 5 dimensional arrays of any Type</remarks>
    Private Function ProcessArray(Of T)(SuppliedObject As Object, SuppliedArray As Array) As Object
        Dim ReturnArray As Object
        Dim ObjectArray1 As Object(), ObjectArray2 As Object(,), ObjectArray3 As Object(,,), ObjectArray4 As Object(,,,), ObjectArray5 As Object(,,,,)

        Select Case SuppliedArray.Rank
            Case 1
                Dim OneDimArray() As T = CType(SuppliedObject, T())
                ReDim ObjectArray1(SuppliedArray.GetLength(0) - 1)
                TL.LogMessage("ProcessArray", "Array Rank 1: " & OneDimArray.GetLength(0))
                Array.Copy(OneDimArray, ObjectArray1, OneDimArray.LongLength)
                ReturnArray = ObjectArray1
            Case 2
                Dim TwoDimArray(,) As T = CType(SuppliedObject, T(,))
                ReDim ObjectArray2(TwoDimArray.GetLength(0) - 1, TwoDimArray.GetLength(1) - 1)
                TL.LogMessage("ProcessArray", "Array Rank 2: " & TwoDimArray.GetLength(0) & " x " & TwoDimArray.GetLength(1))
                Array.Copy(TwoDimArray, ObjectArray2, TwoDimArray.LongLength)
                ReturnArray = ObjectArray2
            Case 3
                Dim ThreeDimArray(,,) As T = CType(SuppliedObject, T(,,))
                ReDim ObjectArray3(ThreeDimArray.GetLength(0) - 1, ThreeDimArray.GetLength(1) - 1, ThreeDimArray.GetLength(2) - 1)
                TL.LogMessage("ProcessArray", "Array Rank 3: " & ThreeDimArray.GetLength(0) & " x " & ThreeDimArray.GetLength(1) & " x " & ThreeDimArray.GetLength(2))
                Array.Copy(ThreeDimArray, ObjectArray3, ThreeDimArray.LongLength)
                ReturnArray = ObjectArray3
            Case 4
                Dim FourDimArray(,,,) As T = CType(SuppliedObject, T(,,,))
                ReDim ObjectArray4(FourDimArray.GetLength(0) - 1, FourDimArray.GetLength(1) - 1, FourDimArray.GetLength(2) - 1, FourDimArray.GetLength(3) - 1)
                TL.LogMessage("ProcessArray", "Array Rank 4: " & FourDimArray.GetLength(0) & " x " & FourDimArray.GetLength(1) & " x " & FourDimArray.GetLength(2) & " x " & FourDimArray.GetLength(3))
                Array.Copy(FourDimArray, ObjectArray4, FourDimArray.LongLength)
                ReturnArray = ObjectArray4
            Case 5
                Dim FiveDimArray(,,,,) As T = CType(SuppliedObject, T(,,,,))
                ReDim ObjectArray5(FiveDimArray.GetLength(0) - 1, FiveDimArray.GetLength(1) - 1, FiveDimArray.GetLength(2) - 1, FiveDimArray.GetLength(3) - 1, FiveDimArray.GetLength(4) - 1)
                TL.LogMessage("ProcessArray", "Array Rank 5: " & FiveDimArray.GetLength(0) & " x " & FiveDimArray.GetLength(1) & " x " & FiveDimArray.GetLength(2) & " x " & FiveDimArray.GetLength(3) & " x " & FiveDimArray.GetLength(4))
                Array.Copy(FiveDimArray, ObjectArray5, FiveDimArray.LongLength)
                ReturnArray = ObjectArray5

            Case Else
                TL.LogMessage("ProcessArrayOfType", "Array rank is outside the range 1..5: " & SuppliedArray.Rank & ", throwing exception")
                Throw New ASCOM.InvalidValueException("Array rank is outside the range 1..5: " & SuppliedArray.Rank)
        End Select

        Return ReturnArray
    End Function

#End Region

#Region "Platform version properties"

    ''' <summary>
    ''' Platform major version number
    ''' </summary>
    ''' <value>Platform major version number</value>
    ''' <returns>Integer version number</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MajorVersion As Integer Implements IUtil.MajorVersion
        Get
            Dim AscomVersion As Version
            AscomVersion = New Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE))
            TL.LogMessage("MajorVersion Get", AscomVersion.Major.ToString())
            Return AscomVersion.Major
        End Get
    End Property

    ''' <summary>
    ''' Platform minor version number
    ''' </summary>
    ''' <value>Platform minor version number</value>
    ''' <returns>Integer version number</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MinorVersion As Integer Implements IUtil.MinorVersion
        Get
            Dim AscomVersion As Version
            AscomVersion = New Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE))
            TL.LogMessage("MinorVersion Get", AscomVersion.Minor.ToString())
            Return AscomVersion.Minor
        End Get
    End Property

    ''' <summary>
    ''' Platform service pack number
    ''' </summary>
    ''' <value>Platform service pack number</value>
    ''' <returns>Integer service pack number</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ServicePack As Integer Implements IUtil.ServicePack
        Get
            Dim AscomVersion As Version
            AscomVersion = New Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE))
            TL.LogMessage("ServicePack Get", AscomVersion.Build.ToString())
            Return AscomVersion.Build
        End Get
    End Property

    ''' <summary>
    ''' Platform build number
    ''' </summary>
    ''' <value>Platform build number</value>
    ''' <returns>Integer build number</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BuildNumber As Integer Implements IUtil.BuildNumber
        Get
            Dim AscomVersion As Version
            AscomVersion = New Version(myProfile.GetProfile(PLATFORM_INFORMATION_SUBKEY, PLATFORM_VERSION, PLATFORM_VERSION_DEFAULT_BAD_VALUE))
            TL.LogMessage("BuildNumber Get", AscomVersion.Revision.ToString())
            Return AscomVersion.Revision
        End Get
    End Property

#End Region

#Region "Time Support Functions"
    '------------------------------------------------------------------------
    ' FUNCTION    : GetTimeZoneOffset()
    '
    ' PURPOSE     : Return the time zone offset in hours, such that
    '               UTC - local + offset
    '------------------------------------------------------------------------
    Private Function GetTimeZoneOffset() As Double
        Dim x As Double
        '5.0.2 Added sign as the ASCOM standard is opposite to Windows
        x = 23.45 * 43.67
        x = -x
        Return -CDbl(TimeZone.CurrentTimeZone.GetUtcOffset(Now).Hours)
    End Function
    '------------------------------------------------------------------------
    ' FUNCTION    : GetTimeZoneName()
    '
    ' PURPOSE     : Use GetTimeZoneInfo to determine the time zone for this
    '               system, including daylight effects, if any.
    '------------------------------------------------------------------------
    Private Function GetTimeZoneName() As String
        If TimeZone.CurrentTimeZone.IsDaylightSavingTime(Now) Then
            Return TimeZone.CurrentTimeZone.DaylightName
        Else
            Return TimeZone.CurrentTimeZone.StandardName
        End If
    End Function
    '------------------------------------------------------------------------
    ' FUNCTION    : CvtUTC()
    '
    ' PURPOSE     : Returns a UTC Date for the given local Date
    '------------------------------------------------------------------------
    Private Function CvtUTC(ByRef d As Date) As Date
        Return CDate(System.DateTime.FromOADate(d.ToOADate + (GetTimeZoneOffset() / 24.0#)))
    End Function

    '------------------------------------------------------------------------
    ' FUNCTION    : CvtLocal()
    '
    ' PURPOSE     : Returns a Local Date for the given UTC Date
    '------------------------------------------------------------------------
    Private Function CvtLocal(ByRef d As Date) As Date
        Return CDate(System.DateTime.FromOADate(d.ToOADate - (GetTimeZoneOffset() / 24.0#)))
    End Function
#End Region

End Class