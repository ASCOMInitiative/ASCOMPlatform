'Implements the Util component

Option Strict On
Option Explicit On
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

''' <summary>
''' Provides a set of utility functions for ASCOM clients and drivers
''' </summary>
''' <remarks></remarks>
<Guid("E861C6D8-B55B-494a-BC59-0F27F981CA98"),
ComVisible(True),
ClassInterface(ClassInterfaceType.None)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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
    <ComVisible(False)>
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

    ''' <summary>
    ''' Convert from one set of speed / temperature / pressure rain rate units to another
    ''' </summary>
    ''' <param name="InputValue">Value to convert</param>
    ''' <param name="FromUnits">Integer value from the Units enum indicating the value's current units</param>
    ''' <param name="ToUnits">Integer value from the Units enum indicating the units to which the input value should be converted</param>
    ''' <returns>Input value expressed in the new units</returns>
    ''' <exception cref="InvalidValueException">When the specified from and to units can not refer to the same value. e.g. attempting to convert miles per hour to degrees Celsius</exception>
    ''' <remarks>
    ''' <para>Conversions available:</para>
    ''' <list type="bullet">
    ''' <item>metres per second &lt;==&gt; miles per hour &lt;==&gt; knots</item>
    ''' <item>Celsius &lt;==&gt; Farenheit &lt;==&gt; Kelvin</item>
    ''' <item>hecto Pascals (hPa) &lt;==&gt; milli bar(mbar) &lt;==&gt; mm of mercury &lt;==&gt; inches of mercury</item>
    ''' <item>mm per hour &lt;==&gt; inches per hour</item>
    ''' </list>
    ''' <para>Knots conversions use the international nautical mile definition (1 nautical mile = 1852m) rather than the orginal UK or US Admiralty definitions.</para>
    ''' <para>For convenience, milli bar and hecto Pascals are shown as separate units even though they have numerically identical values and there is a 1:1 conversion between them.</para>
    ''' </remarks>
    Function ConvertUnits(InputValue As Double, FromUnits As Units, ToUnits As Units) As Double Implements IUtil.ConvertUnits
        Dim intermediateValue, finalValue As Double

        If (FromUnits >= Units.metresPerSecond) And (FromUnits <= Units.knots) And (ToUnits >= Units.metresPerSecond) And (ToUnits <= Units.knots) Then        ' Speed conversion
            ' First convert the input to metres per second
            Select Case FromUnits
                Case Units.metresPerSecond
                    intermediateValue = InputValue
                Case Units.milesPerHour
                    intermediateValue = InputValue * 0.44704
                Case Units.knots
                    intermediateValue = InputValue * 0.514444444
                Case Else
                    Throw New InvalidValueException("Unknown ""From"" speed units: " & FromUnits.ToString())
            End Select

            ' Now convert metres per second to the output value
            Select Case ToUnits
                Case Units.metresPerSecond
                    finalValue = intermediateValue
                Case Units.milesPerHour
                    finalValue = intermediateValue / 0.44704
                Case Units.knots
                    finalValue = intermediateValue / 0.514444444
                Case Else
                    Throw New InvalidValueException("Unknown ""To"" speed units: " & ToUnits.ToString())
            End Select

            Return finalValue

        ElseIf (FromUnits >= Units.degreesCelsius) And (FromUnits <= Units.degreesKelvin) And (ToUnits >= Units.degreesCelsius) And (ToUnits <= Units.degreesKelvin) Then ' Temperature conversion

            ' First convert the input to degrees K
            Select Case FromUnits
                Case Units.degreesCelsius
                    intermediateValue = InputValue - ABSOLUTE_ZERO_CELSIUS
                Case Units.degreesFarenheit
                    intermediateValue = (InputValue + 459.67) * 5.0 / 9.0
                Case Units.degreesKelvin
                    intermediateValue = InputValue
                Case Else
                    Throw New InvalidValueException("Unknown ""From"" temperature units: " & FromUnits.ToString())
            End Select

            ' Now convert degrees K to the output value
            Select Case ToUnits
                Case Units.degreesCelsius
                    finalValue = intermediateValue + ABSOLUTE_ZERO_CELSIUS
                Case Units.degreesFarenheit
                    finalValue = (intermediateValue * 9.0 / 5.0) - 459.67
                Case Units.degreesKelvin
                    finalValue = intermediateValue
                Case Else
                    Throw New InvalidValueException("Unknown ""To"" temperature units: " & ToUnits.ToString())
            End Select

            Return finalValue
        ElseIf (FromUnits >= Units.hPa) And (FromUnits <= Units.inHg) And (ToUnits >= Units.hPa) And (ToUnits <= Units.inHg) Then ' Pressure conversion
            ' First convert the input to hPa
            Select Case FromUnits
                Case Units.hPa
                    intermediateValue = InputValue
                Case Units.mBar
                    intermediateValue = InputValue
                Case Units.mmHg
                    intermediateValue = InputValue * 1.33322368
                Case Units.inHg
                    intermediateValue = InputValue * 33.8638816
                Case Else
                    Throw New InvalidValueException("Unknown ""From"" pressure units: " & FromUnits.ToString())
            End Select

            ' Now convert hPa to the output value
            Select Case ToUnits
                Case Units.hPa
                    finalValue = intermediateValue
                Case Units.mBar
                    finalValue = intermediateValue
                Case Units.mmHg
                    finalValue = intermediateValue / 1.33322368
                Case Units.inHg
                    finalValue = intermediateValue / 33.8638816
                Case Else
                    Throw New InvalidValueException("Unknown ""To"" pressure units: " & ToUnits.ToString())
            End Select

            Return finalValue

        ElseIf (FromUnits >= Units.mmPerHour) And (FromUnits <= Units.inPerHour) And (ToUnits >= Units.mmPerHour) And (ToUnits <= Units.inPerHour) Then ' Rain rate conversion
            ' First convert the input to mm
            Select Case FromUnits
                Case Units.mmPerHour
                    intermediateValue = InputValue
                Case Units.inPerHour
                    intermediateValue = InputValue * 25.4
                Case Else
                    Throw New InvalidValueException("Unknown ""From"" rain rate units: " & FromUnits.ToString())
            End Select

            ' Now convert mm to the output value
            Select Case ToUnits
                Case Units.mmPerHour
                    finalValue = intermediateValue
                Case Units.inPerHour
                    finalValue = intermediateValue / 25.4
                Case Else
                    Throw New InvalidValueException("Unknown ""To"" rain rate units: " & ToUnits.ToString())
            End Select

            Return finalValue

        Else
            Throw New InvalidOperationException("From and to units are not of the same type. From: " & FromUnits.ToString() & ", To: " & ToUnits.ToString())
        End If

        Return 0.0
    End Function

    ''' <summary>
    ''' Calculate the dew point (°Celsius) given the ambient temperature (°Celsius) and relative humidity (%)
    ''' </summary>
    ''' <param name="RelativeHumidity">Relative humidity expressed in percent (0.0 .. 100.0)</param>
    ''' <param name="AmbientTemperature">Ambient temperature (°Celsius)</param>
    ''' <returns>Dew point (°Celsius)</returns>
    ''' <exception cref="InvalidValueException">When relative humidity &lt; 0.0% or &gt; 100.0%></exception>
    ''' <exception cref="InvalidValueException">When ambient temperature &lt; absolute zero or &gt; 100.0C></exception>
    '''  <remarks>'Calculation uses Vaisala formula for water vapour saturation pressure and is accurate to 0.083% over -20C - +50°C
    ''' <para>http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf </para>
    ''' </remarks>
    Function Humidity2DewPoint(RelativeHumidity As Double, AmbientTemperature As Double) As Double Implements IUtil.Humidity2DewPoint
        ' Formulae taken from Vaisala: http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf 
        Dim Pws, Pw, Td As Double

        ' Constants from Vaisala document
        Const A As Double = 6.116441
        Const m As Double = 7.591386
        Const Tn As Double = 240.7263

        'Validate input values
        If (RelativeHumidity < 0.0) Or (RelativeHumidity > 100.0) Then Throw New InvalidValueException("Humidity2DewPoint - Relative humidity is < 0.0% or > 100.0%: " + RelativeHumidity.ToString())
        If (AmbientTemperature < ABSOLUTE_ZERO_CELSIUS) Or (AmbientTemperature > 100.0) Then Throw New InvalidValueException("Humidity2DewPoint - Ambient temperature is < " & ABSOLUTE_ZERO_CELSIUS & "C or > 100.0C: " + AmbientTemperature.ToString())

        Pws = A * Math.Pow(10.0, m * AmbientTemperature / (AmbientTemperature + Tn)) 'Calculate water vapor saturation pressure, Pws, from Vaisala formula (6) - In hPa
        Pw = Pws * RelativeHumidity / 100.0 'Calculate measured vapor pressure, Pw
        Td = Tn / ((m / Math.Log10(Pw / A)) - 1.0) ' Finally, calculate dew-point in °C

        TL.LogMessage("Humidity2DewPoint", "DewPoint: " & Td & ", Given Relative Humidity: " & RelativeHumidity & ", Given Ambient temperaure: " & AmbientTemperature & ", Pws: " & Pws & ", Pw: " & Pw)

        Return Td
    End Function

    ''' <summary>
    ''' Calculate the relative humidity (%) given the ambient temperature (°Celsius) and dew point (°Celsius)
    ''' </summary>
    ''' <param name="DewPoint">Dewpoint in (°Celsius)</param>
    ''' <param name="AmbientTemperature">Ambient temperature (°Celsius)</param>
    ''' <returns>Humidity expressed in percent (0.0 .. 100.0)</returns>
    ''' <exception cref="InvalidValueException">When dew point &lt; absolute zero or &gt; 100.0C></exception>
    ''' <exception cref="InvalidValueException">When ambient temperature &lt; absolute zero or &gt; 100.0C></exception>
    ''' <remarks>'Calculation uses the Vaisala formula for water vapour saturation pressure and is accurate to 0.083% over -20C - +50°C
    ''' <para>http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf </para>
    ''' </remarks>
    Function DewPoint2Humidity(DewPoint As Double, AmbientTemperature As Double) As Double Implements IUtil.DewPoint2Humidity
        ' Formulae taken from Vaisala: http://www.vaisala.com/Vaisala%20Documents/Application%20notes/Humidity_Conversion_Formulas_B210973EN-F.pdf 
        Dim RH As Double

        ' Constants from Vaisala document
        Const m As Double = 7.591386
        Const Tn As Double = 240.7263

        'Validate input values
        If (DewPoint < ABSOLUTE_ZERO_CELSIUS) Or (DewPoint > 100.0) Then Throw New InvalidValueException("DewPoint2Humidity - Dew point is < " & ABSOLUTE_ZERO_CELSIUS & "C or > 100.0C: " + DewPoint.ToString())
        If (AmbientTemperature < ABSOLUTE_ZERO_CELSIUS) Or (AmbientTemperature > 100.0) Then Throw New InvalidValueException("DewPoint2Humidity - Ambient temperature is < " & ABSOLUTE_ZERO_CELSIUS & "C or > 100.0C: " + AmbientTemperature.ToString())

        RH = 100.0 * Math.Pow(10.0, m * ((DewPoint / (DewPoint + Tn)) - (AmbientTemperature / (AmbientTemperature + Tn))))
        TL.LogMessage("DewPoint2Humidity", "RH: " & RH & ", Given Dew point: " & DewPoint & ", Given Ambient temperaure: " & AmbientTemperature)

        Return RH
    End Function

    ''' <summary>
    ''' Convert atmospheric pressure from one altitude above mean sea level to another
    ''' </summary>
    ''' <param name="Pressure">Measured pressure in hPa (mBar) at the "From" altitude</param>
    ''' <param name="FromAltitudeAboveMeanSeaLevel">"Altitude at which the input pressure was measured (metres)</param>
    ''' <param name="ToAltitudeAboveMeanSeaLevel">Altitude to which the pressure is to be converted (metres)</param>
    ''' <returns>Pressure in hPa at the "To" altitude</returns>
    ''' <remarks>Uses the equation: p = p0 * (1.0 - 2.25577E-05 h)^5.25588</remarks>
    Function ConvertPressure(Pressure As Double, FromAltitudeAboveMeanSeaLevel As Double, ToAltitudeAboveMeanSeaLevel As Double) As Double Implements IUtil.ConvertPressure
        ' Convert supplied pressure to sea level then convert again to the required altitude using this equation:
        ' p = p0 (1 - 2.25577 10-5 h)5.25588
        Dim SeaLevelPressure, ActualPressure As Double
        SeaLevelPressure = Pressure / Math.Pow(1.0 - 0.0000225577 * FromAltitudeAboveMeanSeaLevel, 5.25588)
        ActualPressure = SeaLevelPressure * Math.Pow(1.0 - 0.0000225577 * ToAltitudeAboveMeanSeaLevel, 5.25588)

        TL.LogMessage("ConvertPressure", "SeaLevelPressure: " & SeaLevelPressure & ", ActualPressure: " & ActualPressure & ", Given Presure: " & Pressure & ", Given FromAltitudeAboveMeanSeaLevel: " & FromAltitudeAboveMeanSeaLevel & ", Given ToAltitudeAboveMeanSeaLevel: " & ToAltitudeAboveMeanSeaLevel)

        Return ActualPressure
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
    Public Function ArrayToVariantArray(ByVal SuppliedObject As Object) As <MarshalAs(UnmanagedType.SafeArray, SafeArraySubType:=VarEnum.VT_VARIANT)> Object Implements IUtil.ArrayToVariantArray
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
        '6.1 SP2 - Revised to return .TotalHours value instead of .Hours value so that fractional time zone values are returned accurately.
        Return -TimeZone.CurrentTimeZone.GetUtcOffset(Now).TotalHours
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

#Region "COM Registration"
    ''' <summary>
    ''' Function that is called by RegAsm when the assembly is registered for COM
    ''' </summary>
    ''' <remarks>This is necessary to ensure that the mscoree.dll can be found when the SetSearchDirectories function has been called in an application e.g. by Inno installer post v5.5.9</remarks>
    <ComRegisterFunction>
    Private Shared Sub COMRegisterActions(typeToRegister As Type)
        COMRegistrationSupport.COMRegister(typeToRegister)
    End Sub

    ''' <summary>
    ''' Function that is called by RegAsm when the assembly is registered for COM
    ''' </summary>
    <ComUnregisterFunction>
    Private Shared Sub COMUnRegisterActions(typeToRegister As Type)
        ' No action on unregister, this method has been included to remove a compiler warning
    End Sub

#End Region

End Class