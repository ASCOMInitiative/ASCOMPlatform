'Public and private interfaces for the ASCOM.Utilities namesapce

Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

Namespace Interfaces

#Region "Utilities Public Interfaces"

    ''' <summary>
    ''' Interface for KeyValuePair class
    ''' </summary>
    ''' <remarks>This is a return type only used by a small number of the Profile.XXXCOM commands. Including
    ''' <see cref="IProfile.RegisteredDevices">IProfile.RegisteredDevices</see>, 
    ''' <see cref="IProfile.SubKeys">IProfile.SubKeys</see> and 
    ''' <see cref="IProfile.Values">IProfile.Values</see>.</remarks>
    <Guid("CA653783-E47D-4e9d-9759-3B91BE0F4340"), _
    ComVisible(True)> _
    Public Interface IKeyValuePair
        ''' <summary>
        ''' Key member of a key value pair
        ''' </summary>
        ''' <value>Key</value>
        ''' <returns>Ky as string</returns>
        ''' <remarks></remarks>
        <DispId(1)> _
        Property Key() As String
        ''' <summary>
        ''' Value memeber of a key value pair
        ''' </summary>
        ''' <value>Value</value>
        ''' <returns>Value as string</returns>
        ''' <remarks></remarks>
        <DispId(0)> _
        Property Value() As String
    End Interface

    ''' <summary>
    ''' Interface to the TraceLogger component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("1C7ABC95-8B63-475e-B5DB-D0CE7ADC436B"), _
    ComVisible(True)> _
    Public Interface ITraceLogger
        ''' <summary>
        ''' Writes the time and identifier to the log, leaving the line ready for further content through LogContinue and LogFinish
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">Message to log</param>
        ''' <remarks><para>Use this to start a log line where you want to write further information on the line at a later time.</para>
        ''' <para>E.g. You might want to use this to record that an action has started and then append the word OK if all went well.
        '''  You would then end up with just one line to record the whole transaction even though you didn't know that it would be 
        ''' successful when you started. If you just used LogMsg you would have ended up with two log lines, one showing 
        ''' the start of the transaction and the next the outcome.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        <DispId(1)> Sub LogStart(ByVal Identifier As String, ByVal Message As String)

        ''' <summary>
        ''' Appends further message to a line started by LogStart, appends a hex translation of the message to the line, does not terminate the line.
        ''' </summary>
        ''' <param name="Message">The additional message to appear in the line</param>
        ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        ''' <remarks>
        ''' <para>This can be called multiple times to build up a complex log line if required.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        <DispId(2)> Overloads Sub LogContinue(ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string without a new line

        ''' <summary>
        ''' Closes a line started by LogStart with option to append a translation of the supplied message into HEX
        ''' </summary>
        ''' <param name="Message">The final message to appear in the line</param>
        ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        ''' <remarks></remarks>
        <DispId(3)> Overloads Sub LogFinish(ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string with a new line

        ''' <summary>
        ''' Closes a line started by LogStart with the supplied message and a hex translation of the message
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">The final message to terminate the line</param>
        ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        ''' <remarks>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        <DispId(4)> Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string with a new line

        ''' <summary>
        ''' Enables or disables logging to the file.
        ''' </summary>
        ''' <value>True to enable logging</value>
        ''' <returns>Boolean, current logging status (enabled/disabled).</returns>
        ''' <remarks>If this property is false then calls to LogMsg, LogStart, LogContinue and LogFinish do nothing. If True, 
        ''' supplied messages are written to the log file.</remarks>
        <DispId(5)> Property Enabled() As Boolean

        ''' <summary>
        ''' Logs an issue, closing any open line and opening a continuation line if necessary after the 
        ''' issue message.
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">Message to log</param>
        ''' <remarks>Use this for reporting issues that you don't want to appear on a line already opened 
        ''' with StartLine</remarks>
        <DispId(6)> Sub LogIssue(ByVal Identifier As String, ByVal Message As String)

        ''' <summary>
        ''' Sets the log filename and type if the constructor is called without parameters
        ''' </summary>
        ''' <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
        ''' <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
        ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the 
        ''' information of interest.
        ''' <para><b>Note </b>This command is only required if the tracelogger constructor is called with no
        ''' parameters. It is provided for use in COM clients that can not call constructors with parameters.
        ''' If you are writing a COM client then create the trace logger as:</para>
        ''' <code>
        ''' TL = New TraceLogger()
        ''' TL.SetLogFile("","TraceName")
        ''' </code>
        ''' <para>If you are writing a .NET client then you can achieve the same end in one call:</para>
        ''' <code>
        ''' TL = New TraceLogger("",TraceName")
        ''' </code>
        ''' </remarks>
        <DispId(7)> Sub SetLogFile(ByVal LogFileName As String, ByVal LogFileType As String)

    End Interface ' Interface to Utilities.TraceLogger

    <ComVisible(False)> Public Interface ITraceLoggerExtra
        ''' <summary>
        ''' Appends further message to a line started by LogStart, does not terminate the line.
        ''' </summary>
        ''' <param name="Message">The additional message to appear in the line</param>
        ''' <remarks>
        ''' <para>This can be called multiple times to build up a complex log line if required.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' <para>This overload is not available through COM, please use 
        ''' "LogContinue(ByVal Message As String, ByVal HexDump As Boolean)"
        ''' with HexDump set False to achieve this effect.</para>
        ''' </remarks>
        Overloads Sub LogContinue(ByVal Message As String)

        ''' <summary>
        ''' Closes a line started by LogStart with the supplied message
        ''' </summary>
        ''' <param name="Message">The final message to terminate the line</param>
        ''' <remarks>
        ''' <para>Can only be called once for each line started by LogStart.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' <para>This overload is not available through COM, please use 
        ''' "LogFinish(ByVal Message As String, ByVal HexDump As Boolean)"
        ''' with HexDump set False to achieve this effect.</para>
        ''' </remarks>
        Overloads Sub LogFinish(ByVal Message As String)

        ''' <summary>
        ''' Logs a complete message in one call
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">Message to log</param>
        ''' <remarks>
        ''' <para>Use this for straightforward logging requrements. Writes all information in one command.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' <para>This overload is not available through COM, please use 
        ''' "LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean)"
        ''' with HexDump set False to achieve this effect.</para>
        ''' </remarks>
        Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String)
    End Interface

    ''' <summary>
    ''' Interface to the .NET Chooser component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("D398FD76-F4B8-48a2-9CA3-2EF0DD8B98E1"), _
    ComVisible(True)> _
    Public Interface IChooser
        ''' <summary>
        ''' The type of device for which the Chooser will select a driver. (String, default = "Telescope")
        ''' </summary>
        ''' <value>The type of device for which the Chooser will select a driver. (String, default = "Telescope") 
        '''</value>
        ''' <returns>The device type that has been set</returns>
        ''' <remarks>This property changes the "personality" of the Chooser, allowing it to be used to select a driver for any arbitrary 
        ''' ASCOM device type. The default value for this is "Telescope", but it could be "Focuser", "Camera", etc. 
        ''' <para>This property is independent of the Profile object's DeviceType property. Setting Chooser's DeviceType 
        ''' property doesn't set the DeviceType property in Profile, you must set that also when needed.</para>
        '''</remarks>
        <DispId(1)> Property DeviceType() As String

        ''' <summary>
        ''' Select ASCOM driver to use including pre-selecting one in the dropdown list
        ''' </summary>
        ''' <param name="DriverProgID">Driver to preselect in the chooser dialogue</param>
        ''' <returns>Driver ID of chosen driver</returns>
        ''' <remarks>The supplied driver will be pre-selected in the Chooser's list when the chooser window is first opened.
        ''' </remarks>
        <DispId(2)> Overloads Function Choose(ByVal DriverProgID As String) As String
    End Interface 'Interface to Utilities.Chooser

    <ComVisible(False)> _
    Interface IChooserExtra
        ''' <summary>
        ''' Select ASCOM driver to use without pre-selecting in the dropdown list
        ''' </summary>
        ''' <returns>Driver ID of chosen driver</returns>
        ''' <remarks>No driver will be pre-selected in the Chooser's list when the chooser window is first opened. 
        ''' <para>This overload is not available through COM, please use "Choose(ByVal DriverProgID As String)"
        ''' with an empty string parameter to achieve this effect.</para>
        ''' </remarks>
        Overloads Function Choose() As String
    End Interface

    ''' <summary>
    ''' Interface to the .NET Util component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("DF41946E-EE14-40f7-AA66-DD8A92E36EF2"), _
    ComVisible(True)> _
    Public Interface IUtil
        'Interface for the new larger Util class including overloads to replace optional parameters
        ''' <summary>
        ''' Pauses for a given interval in milliseconds.
        ''' </summary>
        ''' <param name="Milliseconds">The number of milliseconds to wait</param>
        ''' <remarks>Repeatedly puts the calling Win32 process to sleep, totally freezing it, for 10 milliseconds, 
        ''' then pumps events so the script or program calling it will receive its normal flow of events, until the 
        ''' pause interval elapses. If the pause interval is 20 milliseconds or less, the sleep interval is reduced 
        ''' to 0, causing the calling Win32 process to give up control to the kernel scheduler and then immediately 
        ''' become eligible for scheduling. </remarks>
        <DispId(1)> Sub WaitForMilliseconds(ByVal Milliseconds As Integer)

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
        <DispId(2)> Function DMSToDegrees(ByVal DMS As String) As Double

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
        <DispId(3)> Function HMSToHours(ByVal HMS As String) As Double

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
        <DispId(4)> Function HMSToDegrees(ByVal HMS As String) As Double

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
        ''' <para>This overload is not available through COM, please use 
        ''' "DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String)"
        ''' with suitable parameters to achieve this effect.</para>
        ''' </remarks>
        <DispId(5)> Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

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
        <DispId(6)> Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

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
        <DispId(7)> Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

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
        <DispId(8)> Overloads Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

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
        <DispId(9)> Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes with supplied number of minute decimal places
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        ''' <param name="MinDelim">The delimiter string to append to the minutes part</param>
        ''' <param name="MinDecimalDigits">The number of minutes decimal places</param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' </remarks>
        <DispId(10)> Overloads Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

        ''' <summary>
        ''' Current Platform version in m.n form
        ''' </summary>
        ''' <returns>Current Platform version in m.n form</returns>
        ''' <remarks></remarks>
        <DispId(11)> ReadOnly Property PlatformVersion() As String

        ''' <summary>
        ''' Change the serial trace file (default C:\SerialTrace.txt)
        ''' </summary>
        ''' <value>Serial trace file name including fully qualified path e.g. C:\SerialTrace.txt</value>
        ''' <returns>Serial trace file name </returns>
        ''' <remarks>Change this before setting the SerialTrace property to True. </remarks>
        <DispId(12)> Property SerialTraceFile() As String

        ''' <summary>
        ''' Enable/disable serial I/O tracing
        ''' </summary>
        ''' <value>Boolean - Enable/disable serial I/O tracing</value>
        ''' <returns>Enabled - disabled state of serial tracing</returns>
        ''' <remarks>If you want to change the serial trace file path, change the SerialTraceFile property before setting this to True. 
        ''' After setting this to True, serial trace info will be written to the last-set serial trace file. </remarks>
        <DispId(13)> Property SerialTrace() As Boolean

        ''' <summary>
        ''' The name of the computer's time zone
        ''' </summary>
        ''' <returns>The name of the computer's time zone</returns>
        ''' <remarks>This will be in the local language of the operating system, and will reflect any daylight/summer time that may be in 
        ''' effect. </remarks>
        <DispId(14)> ReadOnly Property TimeZoneName() As String

        ''' <summary>
        ''' UTC offset (hours) for the computer's clock
        ''' </summary>
        ''' <returns>UTC offset (hours) for the computer's clock</returns>
        ''' <remarks>The offset is in hours, such that UTC = local + offset. The offset includes any daylight/summer time that may be 
        ''' in effect.</remarks>
        <DispId(15)> ReadOnly Property TimeZoneOffset() As Double

        ''' <summary>
        ''' The current UTC Date
        ''' </summary>
        ''' <returns>The current UTC Date</returns>
        ''' <remarks></remarks>
        <DispId(16)> ReadOnly Property UTCDate() As Date

        ''' <summary>
        ''' Current Julian date
        ''' </summary>
        ''' <returns>Current Julian date</returns>
        ''' <remarks>This is quantised to the second in the COM component but to a small decimal fraction in the .NET component</remarks>
        <DispId(17)> ReadOnly Property JulianDate() As Double

        ''' <summary>
        ''' Convert local-time Date to Julian date
        ''' </summary>
        ''' <param name="LocalDate">Date in local-time</param>
        ''' <returns>Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        <DispId(18)> Function DateLocalToJulian(ByVal LocalDate As Date) As Double

        ''' <summary>
        ''' Convert Julian date to local-time Date
        ''' </summary>
        ''' <param name="JD">Julian date to convert</param>
        ''' <returns>Date in local-time for the given Julian date</returns>
        ''' <remarks>Julian dates are always in UTC</remarks>
        <DispId(19)> Function DateJulianToLocal(ByVal JD As Double) As Date

        ''' <summary>
        ''' Convert UTC Date to Julian date
        ''' </summary>
        ''' <param name="UTCDate">UTC date to convert</param>
        ''' <returns>Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        <DispId(20)> Function DateUTCToJulian(ByVal UTCDate As Date) As Double

        ''' <summary>
        ''' Convert Julian date to UTC Date
        ''' </summary>
        ''' <param name="JD">Julian date</param>
        ''' <returns>Date in UTC for the given Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        <DispId(21)> Function DateJulianToUTC(ByVal JD As Double) As Date

        ''' <summary>
        ''' Convert UTC Date to local-time Date
        ''' </summary>
        ''' <param name="UTCDate">Date in UTC</param>
        ''' <returns>Date in local-time</returns>
        ''' <remarks></remarks>
        <DispId(22)> Function DateUTCToLocal(ByVal UTCDate As Date) As Date

        ''' <summary>
        ''' Convert local-time Date to UTC Date
        ''' </summary>
        ''' <param name="LocalDate">Date in local-time</param>
        ''' <returns> Date in UTC</returns>
        ''' <remarks></remarks>
        <DispId(23)> Function DateLocalToUTC(ByVal LocalDate As Date) As Date
    End Interface 'Interface to Utilities.Util

    <ComVisible(False)> _
    Interface IUtilExtra
        ''' <summary>
        ''' Convert degrees to sexagesimal degrees, minutes and seconds with default delimiters DD° MM' SS" 
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        ''' characters.</para>
        ''' <para>This overload is not available through COM, please use "Choose(ByVal DriverProgID As String)"
        ''' with an empty string parameter to achieve this effect.</para>
        ''' </remarks>
        Overloads Function DegreesToDMS(ByVal Degrees As Double) As String
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
        Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String) As String
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
        Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String
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
        Overloads Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String

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
        Overloads Function HoursToHMS(ByVal Hours As Double) As String
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
        Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String) As String
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
        Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        Overloads Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String

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
        Overloads Function DegreesToDM(ByVal Degrees As Double) As String
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
        Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String) As String
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
        Overloads Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String

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
        Overloads Function HoursToHM(ByVal Hours As Double) As String
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
        Overloads Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String) As String
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
        Overloads Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String

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
        Overloads Function DegreesToHMS(ByVal Degrees As Double) As String
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
        Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String) As String
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
        Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        Overloads Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String

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
        Overloads Function DegreesToHM(ByVal Degrees As Double) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes with default minute delimiter MM (null string)
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' <para>This overload is not available through COM, please use 
        ''' "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        ''' with suitable parameters to achieve this effect.</para>
        ''' </remarks>
        Overloads Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' <para>This overload is not available through COM, please use 
        ''' "DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer)"
        ''' with suitable parameters to achieve this effect.</para>
        ''' </remarks>
        Overloads Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
    End Interface

    ''' <summary>
    ''' Interface to the .NET Timer component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("23A8A279-FB8E-4b3c-8F2E-010AC0F98588"), _
    ComVisible(True)> _
    Public Interface ITimer
        'Interface for the Timer class

        ''' <summary>
        ''' The interval between Tick events when the timer is Enabled in milliseconds, (default = 1000)
        ''' </summary>
        ''' <value>The interval between Tick events when the timer is Enabled (milliseconds, default = 1000)</value>
        ''' <returns>The interval between Tick events when the timer is Enabled in milliseconds</returns>
        ''' <remarks></remarks>
        <DispId(1)> Property Interval() As Integer
        ''' <summary>
        ''' Enable the timer tick events
        ''' </summary>
        ''' <value>True means the timer is active and will deliver Tick events every Interval milliseconds.</value>
        ''' <returns>Enabled state of timer tick events</returns>
        ''' <remarks></remarks>
        <DispId(2)> Property Enabled() As Boolean
    End Interface 'Interface to Utilities.Timer

    <Guid("BDDA4DFD-77F8-4bd2-ACC0-AF32B4F8B9C2"), _
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch), _
    ComVisible(True)> _
    Public Interface ITimerEvent
        ''' <summary>
        ''' Fired once per Interval when timer is Enabled.
        ''' </summary>
        ''' <remarks>To sink this event in Visual Basic, declare the object variable using the WithEvents keyword.</remarks>
        <DispId(1)> Sub Tick()
    End Interface

    ''' <summary>
    ''' Interface to the .NET Profile component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("3503C303-B268-4da8-A0AA-CD6530B802AA"), _
    ComVisible(True)> _
    Public Interface IProfile
        'Interface for the Profile class
        ''' <summary>
        ''' The type of ASCOM device for which profile data and registration services are provided 
        ''' (String, default = "Telescope")
        ''' </summary>
        ''' <value>String describing the type of device being accessed</value>
        ''' <returns>String describing the type of device being accessed</returns>
        ''' <remarks></remarks>
        <DispId(1)> Property DeviceType() As String

        ''' <summary>
        ''' List the device types registered in the Profile store
        ''' </summary>
        ''' <value>List of registered device types</value>
        ''' <returns>A sorted string array of device types</returns>
        ''' <remarks>Use this to find which types of device are registered in the Profile store.</remarks>
        <DispId(2)> ReadOnly Property RegisteredDeviceTypes() As String()

        ''' <summary>
        ''' List the devices of a given device type that are registered in the Profile store
        ''' </summary>
        ''' <param name="DeviceType">Type of devices to list</param>
        ''' <value>List of registered devices</value>
        ''' <returns>An ArrayList of KeyValuePair objects of installed devices and associated device descriptions</returns>
        ''' <exception cref="Exceptions.InvalidValueException">Throw if the supplied DeviceType is empty string or 
        ''' null value.</exception>
        ''' <remarks>
        ''' Use this to find all the registerd devices of a given type that are in the Profile store.
        ''' <para>If a DeviceType is supplied, where no device of that type has been registered before on this system,
        ''' an empty list will be returned</para>
        ''' </remarks>
        <DispId(3)> ReadOnly Property RegisteredDevices(ByVal DeviceType As String) As ArrayList

        ''' <summary>
        ''' Confirms whether a specific driver is registered ort unregistered in the profile store
        ''' </summary>
        ''' <param name="DriverID">String reprsenting the device's ProgID</param>
        ''' <returns>Boolean indicating registered or unregisteredstate of the supplied DriverID</returns>
        ''' <remarks></remarks>
        <DispId(4)> Function IsRegistered(ByVal DriverID As String) As Boolean

        ''' <summary>
        ''' Registers a supplied DriverID and associates a descriptive name with the device
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to register</param>
        ''' <param name="DescriptiveName">Descriptive name of the device</param>
        ''' <remarks>Does nothing if already registered, so safe to call on driver load.</remarks>
        <DispId(5)> Sub Register(ByVal DriverID As String, ByVal DescriptiveName As String)

        ''' <summary>
        ''' Remove all data for the given DriverID from the registry.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to unregister</param>
        ''' <remarks>This deletes the entire device profile tree, including the DriverID root key.</remarks>
        <DispId(6)> Sub Unregister(ByVal DriverID As String)

        ''' <summary>
        ''' Retrieve a string value from the profile using the supplied subkey for the given Driver ID and variable name.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="SubKey">Subkey from the profile root from which to read the value</param>
        ''' <returns>Retrieved variable value</returns>
        ''' <remarks>
        ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
        ''' </remarks>
        <DispId(7)> Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String) As String

        ''' <summary>
        ''' Writes a string value to the profile using the supplied subkey for the given Driver ID and variable name.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="Value">The string value to be written</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks></remarks>
        <DispId(8)> Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String)

        ''' <summary>
        ''' Return a list of the (unnamed and named variables) under the given DriverID and subkey.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root from which to return values</param>
        ''' <returns>An ArrayList of KeyValuePair objects.</returns>
        ''' <remarks>The returned object contains entries for each value. For each entry, 
        ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        ''' Key property set to an empty string.
        ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        '''  </remarks>
        <DispId(9)> Function Values(ByVal DriverID As String, ByVal SubKey As String) As ArrayList

        ''' <summary>
        ''' Delete the value from the registry. Name may be an empty string for the unnamed value. Value will be deleted from the subkey supplied.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
        <DispId(10)> Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)

        ''' <summary>
        ''' Create a registry key for the given DriverID.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>If the SubKey argument contains a \ separated path, the intermediate keys will be created if needed. </remarks>
        <DispId(11)> Sub CreateSubKey(ByVal DriverID As String, ByVal SubKey As String)

        ''' <summary>
        ''' Return a list of the sub-keys under the given DriverID (for COM clients)
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to search for subkeys</param>
        ''' <returns>An ArrayList of key-value pairs</returns>
        ''' <remarks>The returned object (scripting.dictionary) contains entries for each sub-key. For each KeyValuePair in the list, 
        ''' the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
        ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        ''' </remarks>
        <DispId(12)> Overloads Function SubKeys(ByVal DriverID As String, ByVal SubKey As String) As ArrayList

        ''' <summary>
        ''' Delete a registry key for the given DriverID. SubKey may contain \ separated path to key to be deleted.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>The sub-key and all data and keys beneath it are deleted.</remarks>
        <DispId(13)> Sub DeleteSubKey(ByVal DriverID As String, ByVal SubKey As String)
    End Interface 'Interface to Utilities.Profile

    <ComVisible(False)> _
    Public Interface IProfileExtra

        Sub MigrateProfile()

        ''' <summary>
        ''' Delete the value from the registry. Name may be an empty string for the unnamed value. 
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key.
        ''' <para>This overload is not available through COM, please use 
        ''' "DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
        ''' with SubKey set to empty string achieve this effect.</para>
        ''' </remarks>
        Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String)

        ''' <summary>
        ''' Retrieve a string value from the profile for the given Driver ID and variable name
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <returns>Retrieved variable value</returns>
        ''' <remarks>
        ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
        ''' <para>This overload is not available through COM, please use 
        ''' "GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)"
        ''' with SubKey set to empty string achieve this effect.</para>
        ''' </remarks>
        Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String) As String

        ''' <summary>
        ''' Return a list of the sub-keys under the root of the given DriverID
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <returns>An ArrayList of key-value pairs</returns>
        ''' <remarks>The returned object (scripting.dictionary) contains entries for each sub-key. For each KeyValuePair in the list, 
        ''' the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.
        ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        ''' </remarks>
        Overloads Function SubKeys(ByVal DriverID As String) As ArrayList

        ''' <summary>
        ''' Return a list of the (unnamed and named variables) under the given DriverID.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <returns>An ArrayList of KeyValuePair objects.</returns>
        ''' <remarks>The returned object contains entries for each value. For each entry, 
        ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        ''' Key property set to an empty string.
        ''' <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
        '''  </remarks>
        Overloads Function Values(ByVal DriverID As String) As ArrayList

        ''' <summary>
        ''' Writes a string value to the profile using the given Driver ID and variable name.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="Value">The string value to be written</param>
        ''' <remarks>
        ''' This overload is not available through COM, please use 
        ''' "WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String)"
        ''' with SubKey set to empty string achieve this effect.
        ''' </remarks>
        Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String)
    End Interface

    ''' <summary>
    ''' Interface to the .NET Serial component
    ''' </summary>
    ''' <remarks></remarks>
    <Guid("8828511A-05C1-43c7-8970-00D23595930A"), _
    ComVisible(True)> _
    Public Interface ISerial

        'Interface for the standard ASCOM serial functionality
        ''' <summary>
        ''' Returns a list of all available ASCOM serial ports with COMnnn ports sorted into ascending port number order
        ''' </summary>
        ''' <value>String array of available serial ports</value>
        ''' <returns>A string array of available serial ports</returns>
        ''' <remarks></remarks>
        <DispId(1)> ReadOnly Property AvailableComPorts() As String()
        ''' <summary>
        ''' Gets or sets the number of data bits in each byte
        ''' </summary>
        ''' <value>The number of data bits in each byte, default is 8 data bits</value>
        ''' <returns>Integer number of data bits in each byte</returns>
        ''' <exception cref="ArgumentOutOfRangeException">The data bits value is less than 5 or more than 8</exception>
        ''' <remarks></remarks>
        <DispId(2)> Property DataBits() As Integer
        ''' <summary>
        ''' Gets or sets the state of the DTR line
        ''' </summary>
        ''' <value>The state of the DTR line, default is enabled</value>
        ''' <returns>Boolean true/false indicating enabled/disabled</returns>
        ''' <remarks></remarks>
        <DispId(3)> Property DTREnable() As Boolean
        ''' <summary>
        ''' Gets or sets the type of parity check used over the serial link
        ''' </summary>
        ''' <value>The type of parity check used over the serial link, default none</value>
        ''' <returns>One of the Ports.Parity enumeration values</returns>
        ''' <remarks></remarks>
        <DispId(4)> Property Parity() As SerialParity
        ''' <summary>
        ''' Gets or sets the number of stop bits used on the serial link
        ''' </summary>
        ''' <value>the number of stop bits used on the serial link, default 1</value>
        ''' <returns>One of the Ports.StopBits enumeration values</returns>
        ''' <remarks></remarks>
        <DispId(5)> Property StopBits() As SerialStopBits
        ''' <summary>
        ''' Gets or sets the type of serial handshake used on the serial line
        ''' </summary>
        ''' <value>The type of handshake used on the serial line, default is none</value>
        ''' <returns>One of the Ports.Handshake enumeration values</returns>
        ''' <remarks></remarks>
        <DispId(6)> Property Handshake() As SerialHandshake
        ''' <summary>
        ''' Gets or sets the connected state of the ASCOM serial port.
        ''' </summary>
        ''' <value>Connected state of the serial port.</value>
        ''' <returns><c>True</c> if the serial port is connected.</returns>
        ''' <remarks>Set this property to True to connect to the serial (COM) port. You can read the property to determine if the object is connected. </remarks>
        <DispId(7)> Property Connected() As Boolean
        ''' <summary>
        ''' Gets or sets the number of the ASCOM serial port (Default is 1, giving COM1 as the serial port name).
        ''' </summary>
        ''' <value>COM port number of the ASCOM serial port.</value>
        ''' <returns>Integer, number of the ASCOM serial port</returns>
        ''' <remarks>This works for serial port names of the form COMnnn. Use PortName if your COM port name does not fit the form COMnnn.</remarks>
        <DispId(8)> Property Port() As Integer
        ''' <summary>
        ''' The maximum time that the ASCOM serial port will wait for incoming receive data (seconds, default = 5)
        ''' </summary>
        ''' <value>Integer, serial port timeout in seconds</value>
        ''' <returns>Integer, serial port timeout in seconds.</returns>
        ''' <remarks>The minimum delay timout that can be set through this command is 1 seconds. Use ReceiveTimeoutMs to set a timeout less than 1 second.</remarks>
        <DispId(9)> Property ReceiveTimeout() As Integer
        ''' <summary>
        ''' The maximum time that the ASCOM serial port will wait for incoming receive data (milliseconds, default = 5000)
        ''' </summary>
        ''' <value>Integer, serial port timeout in milli-seconds</value>
        ''' <returns>Integer, serial port timeout in milli-seconds</returns>
        ''' <remarks>If a timeout occurs, an IO timeout error is raised. See ReceiveTimeout for an alternate form 
        ''' using the timeout in seconds. </remarks>
        <DispId(10)> Property ReceiveTimeoutMs() As Integer
        ''' <summary>
        ''' Sets the ASCOM serial port name as a string
        ''' </summary>
        ''' <value>Serial port name to be used</value>
        ''' <returns>Current serial port name</returns>
        ''' <remarks>This property allows any serial port name to be used, even if it doesn't conform to a format of COMnnn
        ''' If the required port name is of the form COMnnn then Serial.Port=nnn and Serial.PortName="COMnnn" are 
        ''' equivalent.</remarks>
        <DispId(11)> Property PortName() As String
        ''' <summary>
        ''' Gets and sets the baud rate of the ASCOM serial port
        ''' </summary>
        ''' <value>Port speed using the PorSpeed enum</value>
        ''' <returns>Port speed using the PortSpeed enum</returns>
        ''' <remarks>This is modelled on the COM component with possible values enumerated in the PortSpeed enum.</remarks>
        <DispId(12)> Property Speed() As SerialSpeed
        ''' <summary>
        ''' Clears the ASCOM serial port receive and transmit buffers
        ''' </summary>
        ''' <remarks></remarks>
        <DispId(13)> Sub ClearBuffers()
        ''' <summary>
        ''' Transmits a string through the ASCOM serial port
        ''' </summary>
        ''' <param name="Data">String to transmit</param>
        ''' <remarks></remarks>
        <DispId(14)> Sub Transmit(ByVal Data As String)
        ''' <summary>
        ''' Transmit an array of binary bytes through the ASCOM serial port 
        ''' </summary>
        ''' <param name="Data">Byte array to transmit</param>
        ''' <remarks></remarks>
        <DispId(15)> Sub TransmitBinary(ByVal Data As Byte())
        ''' <summary>
        ''' Adds a message to the ASCOM serial trace file
        ''' </summary>
        ''' <param name="Caller">String identifying the module logging the message</param>
        ''' <param name="Message">Message text to be logged.</param>
        ''' <remarks>
        ''' <para>This can be called regardless of whether logging is enabled</para>
        ''' </remarks>
        <DispId(16)> Sub LogMessage(ByVal Caller As String, ByVal Message As String)
        ''' <summary>
        ''' Receive at least one text character from the ASCOM serial port
        ''' </summary>
        ''' <returns>The characters received</returns>
        ''' <remarks>This method reads all of the characters currently in the serial receive buffer. It will not return 
        ''' unless it reads at least one character. A timeout will cause a TimeoutException to be raised. Use this for 
        ''' text data, as it returns a String. </remarks>
        <DispId(17)> Function Receive() As String
        ''' <summary>
        ''' Receive one binary byte from the ASCOM serial port
        ''' </summary>
        ''' <returns>The received byte</returns>
        ''' <remarks>Use this for 8-bit (binary data). If a timeout occurs, a TimeoutException is raised. </remarks>
        <DispId(18)> Function ReceiveByte() As Byte
        ''' <summary>
        ''' Receive exactly the given number of characters from the ASCOM serial port and return as a string
        ''' </summary>
        ''' <param name="Count">The number of characters to return</param>
        ''' <returns>String of length "Count" characters</returns>
        ''' <remarks>If a timeout occurs a TimeoutException is raised.</remarks>
        <DispId(19)> Function ReceiveCounted(ByVal Count As Integer) As String
        ''' <summary>
        ''' Receive exactly the given number of characters from the ASCOM serial port and return as a byte array
        ''' </summary>
        ''' <param name="Count">The number of characters to return</param>
        ''' <returns>Byte array of size "Count" elements</returns>
        ''' <remarks>
        ''' <para>If a timeout occurs, a TimeoutException is raised. </para>
        ''' <para>This function exists in the COM component but is not documented in the help file.</para>
        ''' </remarks>
        <DispId(20)> Function ReceiveCountedBinary(ByVal Count As Integer) As Byte()
        ''' <summary>
        ''' Receive characters from the ASCOM serial port until the given terminator string is seen
        ''' </summary>
        ''' <param name="Terminator">The character string that indicates end of message</param>
        ''' <returns>Received characters including the terminator string</returns>
        ''' <remarks>If a timeout occurs, a TimeoutException is raised.</remarks>
        <DispId(21)> Function ReceiveTerminated(ByVal Terminator As String) As String
        ''' <summary>
        ''' Receive characters from the ASCOM serial port until the given terminator bytes are seen, return as a byte array
        ''' </summary>
        ''' <param name="TerminatorBytes">Array of bytes that indicates end of message</param>
        ''' <returns>Byte array of received characters</returns>
        ''' <remarks>
        ''' <para>If a timeout occurs, a TimeoutException is raised.</para>
        ''' <para>This function exists in the COM component but is not documented in the help file.</para>
        ''' </remarks>
        <DispId(22)> Function ReceiveTerminatedBinary(ByVal TerminatorBytes() As Byte) As Byte()
    End Interface  'Interface to Utilities.Serial

#End Region

#Region "Utilities Private Interfaces"

    Friend Interface IFileStoreProvider
        'Interface that a file store provider must implement to support a storage provider
        Sub CreateDirectory(ByVal p_SubKeyName As String)
        Sub DeleteDirectory(ByVal p_SubKeyName As String)
        Sub EraseFileStore()
        ReadOnly Property GetDirectoryNames(ByVal p_SubKeyName As String) As String()
        ReadOnly Property Exists(ByVal p_FileName As String) As Boolean
        ReadOnly Property FullPath(ByVal p_FileName As String) As String
        ReadOnly Property BasePath() As String
        Sub Rename(ByVal p_CurrentName As String, ByVal p_NewName As String)
        Sub RenameDirectory(ByVal CurrentName As String, ByVal NewName As String)
    End Interface 'Interface that a file store provider must implement to support a store provider

    Friend Interface IAccess
        'Interface for a general profile store provider, this is independent of the actual mechanic used to store the profile
        Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) As String
        Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String)
        Function EnumProfile(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String)
        Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String)
        Sub CreateKey(ByVal p_SubKeyName As String)
        Function EnumKeys(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String)
        Sub DeleteKey(ByVal p_SubKeyName As String)
        Sub RenameKey(ByVal CurrentSubKeyName As String, ByVal NewSubKeyName As String)
        Sub MigrateProfile()
    End Interface 'Interface for a general profile store provider

#End Region

End Namespace