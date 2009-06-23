'Contains interfaes and global constants

Imports System
Imports System.IO
Imports System.Reflection
Imports Microsoft.Win32
Imports ASCOM.HelperNET.Interfaces

#Region "Global Constants and Variables"
Module GlobalConstants
    Friend Const SERIAL_FILE_NAME_VARNAME As String = "SerTraceFile" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_AUTO_FILENAME As String = "C:\SerialTraceAuto.txt" 'Special value to indicate use of automatic trace filenames
    Friend Const SERIAL_DEFAULT_FILENAME As String = "C:\SerialTrace.txt" 'Default manual trace filename

    'HelperNET configuration constants
    Friend Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Friend Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
    Friend Const REGISTRY_CONFORM_FOLDER As String = "Software\ASCOM\HelperNET"

End Module
#End Region

#Region "Interfaces"
Namespace Interfaces
    ''' <summary>
    ''' Interface to the .NET TraceLogger component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ITraceLogger
        Inherits IDisposable
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
        Sub LogStart(ByVal Identifier As String, ByVal Message As String)
        ''' <summary>
        ''' Appends further message to a line started by LogStart, does not terminate the line.
        ''' </summary>
        ''' <param name="Message">The additional message to appear in the line</param>
        ''' <remarks>
        ''' <para>This can be called multiple times to build up a complex log line if required.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        Overloads Sub LogContinue(ByVal Message As String)
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
        Overloads Sub LogContinue(ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string without a new line
        ''' <summary>
        ''' Closes a line started by LogStart with the supplied message
        ''' </summary>
        ''' <param name="Message">The final message to terminate the line</param>
        ''' <remarks>
        ''' <para>Can only be called once for each line started by LogStart.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        Overloads Sub LogFinish(ByVal Message As String)
        ''' <summary>
        ''' Closes a line started by LogStart with option to append a translation of the supplied message into HEX
        ''' </summary>
        ''' <param name="Message">The final message to appear in the line</param>
        ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        ''' <remarks></remarks>
        Overloads Sub LogFinish(ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string with a new line
        ''' <summary>
        ''' Logs a complete message in one call
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">Message to log</param>
        ''' <remarks>
        ''' <para>Use this for straightforward logging requrements. Writes all information in one command.</para>
        ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        ''' </remarks>
        Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String)
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
        Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean) ' Append a full hex dump of the supplied string with a new line
        ''' <summary>
        ''' Enables or disables logging to the file.
        ''' </summary>
        ''' <value>True to enable logging</value>
        ''' <returns>Boolean, current logging status (enabled/disabled).</returns>
        ''' <remarks>If this property is false then calls to LogMsg, LogStart, LogContinue and LogFinish do nothing. If True, 
        ''' supplied messages are written to the log file.</remarks>
        Property Enabled() As Boolean
        ''' <summary>
        ''' Logs an issue, closing any open line and opening a continuation line if necessary after the 
        ''' issue message.
        ''' </summary>
        ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
        ''' <param name="Message">Message to log</param>
        ''' <remarks>Use this for reporting issues that you don't want to appear on a line already opened 
        ''' with StartLine</remarks>
        Sub LogIssue(ByVal Identifier As String, ByVal Message As String)
    End Interface ' Interface to HelperNET.TraceLogger

    ''' <summary>
    ''' Interface to the .NET Chooser component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IChooser
        Inherits IDisposable
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
        Property DeviceType() As String
        ''' <summary>
        ''' Select ASCOM driver to use without pre-selecting in the dropdown list
        ''' </summary>
        ''' <returns>Driver ID of chosen driver</returns>
        ''' <remarks>No driver will be pre-selected in the Chooser's list when the chooser window is first opened. 
        ''' </remarks>
        Overloads Function Choose() As String
        ''' <summary>
        ''' Select ASCOM driver to use including pre-selecting one in the dropdown list
        ''' </summary>
        ''' <param name="DriverProgID">Driver to preselect in the chooser dialogue</param>
        ''' <returns>Driver ID of chosen driver</returns>
        ''' <remarks>The supplied driver will be pre-selected in the Chooser's list when the chooser window is first opened.
        ''' </remarks>
        Overloads Function Choose(ByVal DriverProgID As String) As String
    End Interface 'Interface to HelperNET.Chooser

    ''' <summary>
    ''' Interface to the .NET Util component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IUtil
        'Interface for the new larger Util class including overloads to replace optional parameters
        Inherits IDisposable
        ''' <summary>
        ''' Pauses for a given interval in milliseconds.
        ''' </summary>
        ''' <param name="Milliseconds">The number of milliseconds to wait</param>
        ''' <remarks>Repeatedly puts the calling Win32 process to sleep, totally freezing it, for 10 milliseconds, 
        ''' then pumps events so the script or program calling it will receive its normal flow of events, until the 
        ''' pause interval elapses. If the pause interval is 20 milliseconds or less, the sleep interval is reduced 
        ''' to 0, causing the calling Win32 process to give up control to the kernel scheduler and then immediately 
        ''' become eligible for scheduling. </remarks>
        Sub WaitForMilliseconds(ByVal Milliseconds As Integer)
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
        Function DMSToDegrees(ByVal DMS As String) As Double
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
        Function HMSToHours(ByVal HMS As String) As Double
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
        Function HMSToDegrees(ByVal HMS As String) As Double

        ''' <summary>
        ''' Convert degrees to sexagesimal degrees, minutes and seconds with default delimiters DD° MM' SS" 
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        ''' characters.</para>
        ''' </remarks>
        Function DegreesToDMS(ByVal Degrees As Double) As String
        ''' <summary>
        '''  Convert degrees to sexagesimal degrees, minutes and seconds with with default minute and second delimiters MM' SS" 
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="DegDelim">The delimiter string separating degrees and minutes </param>
        ''' <returns>Sexagesimal representation of degrees input value, degrees, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single 
        ''' characters.</para>
        ''' </remarks>
        Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String) As String
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
        ''' </remarks>
        Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String
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
        ''' </remarks>
        Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String
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
        Function DegreesToDMS(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert hours to sexagesimal hours, minutes, and seconds with default delimiters HH:MM:SS
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHMS(ByVal Hours As Double) As String
        ''' <summary>
        ''' Convert hours to sexagesimal hours, minutes, and seconds with default minutes and seconds delimters MM:SS
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String) As String
        ''' <summary>
        ''' Convert hours to sexagesimal hours, minutes, and seconds with default second delimiter of null string
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <param name="MinDelim">The delimiter string separating minutes and seconds </param>
        ''' <returns>Sexagesimal representation of hours input value, hours, minutes and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        ''' </remarks>
        Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String
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
        Function HoursToHMS(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert degrees to sexagesimal degrees and minutes with default delimiters DD° MM'
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function DegreesToDM(ByVal Degrees As Double) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal degrees and minutes with the default minutes delimeter MM'
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="DegDelim">The delimiter string separating degrees </param>
        ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal degrees and minutes
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="DegDelim">The delimiter string separating degrees </param>
        ''' <param name="MinDelim">The delimiter string to append to the minutes </param>
        ''' <returns>Sexagesimal representation of degrees input value, as degrees and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String) As String
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
        Function DegreesToDM(ByVal Degrees As Double, ByVal DegDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert hours to sexagesimal hours and minutes with default delimiters HH:MM
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHM(ByVal Hours As Double) As String
        ''' <summary>
        ''' Convert hours to sexagesimal hours and minutes with default minutes delimiter MM (null string)
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String) As String
        ''' <summary>
        ''' Convert hours to sexagesimal hours and minutes
        ''' </summary>
        ''' <param name="Hours">The hours value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
        ''' <returns>Sexagesimal representation of hours input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters.</para>
        ''' </remarks>
        Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        Function HoursToHM(ByVal Hours As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert degrees to sexagesimal hours, minutes, and seconds with default delimters of HH:MM:SS
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself.</para>
        ''' </remarks>
        Function DegreesToHMS(ByVal Degrees As Double) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours, minutes, and seconds with the default second and minute delimiters of MM:SS
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        ''' </remarks>
        Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours, minutes, and seconds with the default second delimiter SS (null string)
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes</param>
        ''' <param name="MinDelim">The delimiter string separating minutes and seconds</param>
        ''' <returns>Sexagesimal representation of degrees input value, as hours, minutes, and seconds</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters. </para>
        ''' </remarks>
        Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        ''' </remarks>
        Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String) As String
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
        Function DegreesToHMS(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal SecDelim As String, ByVal SecDecimalDigits As Integer) As String

        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes with default delimiters HH:MM
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' </remarks>
        Function DegreesToHM(ByVal Degrees As Double) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes with default minute delimiter MM (null string)
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' </remarks>
        Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String) As String
        ''' <summary>
        ''' Convert degrees to sexagesimal hours and minutes
        ''' </summary>
        ''' <param name="Degrees">The degrees value to convert</param>
        ''' <param name="HrsDelim">The delimiter string separating hours and minutes </param>
        ''' <param name="MinDelim">The delimiter string to append to the minutes part </param>
        ''' <returns>Sexagesimal representation of degrees input value as hours and minutes</returns>
        ''' <remarks>
        ''' <para>If you need a leading plus sign, you must prepend it yourself. The delimiters are not restricted to single characters</para>
        ''' </remarks>
        Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String) As String
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
        Function DegreesToHM(ByVal Degrees As Double, ByVal HrsDelim As String, ByVal MinDelim As String, ByVal MinDecimalDigits As Integer) As String

        ''' <summary>
        ''' Current Platform version in m.n form
        ''' </summary>
        ''' <returns>Current Platform version in m.n form</returns>
        ''' <remarks></remarks>
        ReadOnly Property PlatformVersion() As String
        ''' <summary>
        ''' Change the serial trace file (default C:\SerialTrace.txt)
        ''' </summary>
        ''' <value>Serial trace file name including fully qualified path e.g. C:\SerialTrace.txt</value>
        ''' <returns>Serial trace file name </returns>
        ''' <remarks>Change this before setting the SerialTrace property to True. </remarks>
        Property SerialTraceFile() As String
        ''' <summary>
        ''' Enable/disable serial I/O tracing
        ''' </summary>
        ''' <value>Boolean - Enable/disable serial I/O tracing</value>
        ''' <returns>Enabled - disabled state of serial tracing</returns>
        ''' <remarks>If you want to change the serial trace file path, change the SerialTraceFile property before setting this to True. 
        ''' After setting this to True, serial trace info will be written to the last-set serial trace file. </remarks>
        Property SerialTrace() As Boolean
        ''' <summary>
        ''' The name of the computer's time zone
        ''' </summary>
        ''' <returns>The name of the computer's time zone</returns>
        ''' <remarks>This will be in the local language of the operating system, and will reflect any daylight/summer time that may be in 
        ''' effect. </remarks>
        ReadOnly Property TimeZoneName() As String
        ''' <summary>
        ''' UTC offset (hours) for the computer's clock
        ''' </summary>
        ''' <returns>UTC offset (hours) for the computer's clock</returns>
        ''' <remarks>The offset is in hours, such that UTC = local + offset. The offset includes any daylight/summer time that may be 
        ''' in effect.</remarks>
        ReadOnly Property TimeZoneOffset() As Double
        ''' <summary>
        ''' The current UTC Date
        ''' </summary>
        ''' <returns>The current UTC Date</returns>
        ''' <remarks></remarks>
        ReadOnly Property UTCDate() As Date
        ''' <summary>
        ''' Current Julian date
        ''' </summary>
        ''' <returns>Current Julian date</returns>
        ''' <remarks>This is quantised to the second in the COM component but to a small decimal fraction in the .NET component</remarks>
        ReadOnly Property JulianDate() As Double
        ''' <summary>
        ''' Convert local-time Date to Julian date
        ''' </summary>
        ''' <param name="LocalDate">Date in local-time</param>
        ''' <returns>Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        Function DateLocalToJulian(ByVal LocalDate As Date) As Double
        ''' <summary>
        ''' Convert Julian date to local-time Date
        ''' </summary>
        ''' <param name="JD">Julian date to convert</param>
        ''' <returns>Date in local-time for the given Julian date</returns>
        ''' <remarks>Julian dates are always in UTC</remarks>
        Function DateJulianToLocal(ByVal JD As Double) As Date
        ''' <summary>
        ''' Convert UTC Date to Julian date
        ''' </summary>
        ''' <param name="UTCDate">UTC date to convert</param>
        ''' <returns>Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        Function DateUTCToJulian(ByVal UTCDate As Date) As Double
        ''' <summary>
        ''' Convert Julian date to UTC Date
        ''' </summary>
        ''' <param name="JD">Julian date</param>
        ''' <returns>Date in UTC for the given Julian date</returns>
        ''' <remarks>Julian dates are always in UTC </remarks>
        Function DateJulianToUTC(ByVal JD As Double) As Date
        ''' <summary>
        ''' Convert UTC Date to local-time Date
        ''' </summary>
        ''' <param name="UTCDate">Date in UTC</param>
        ''' <returns>Date in local-time</returns>
        ''' <remarks></remarks>
        Function DateUTCToLocal(ByVal UTCDate As Date) As Date
        ''' <summary>
        ''' Convert local-time Date to UTC Date
        ''' </summary>
        ''' <param name="LocalDate">Date in local-time</param>
        ''' <returns> Date in UTC</returns>
        ''' <remarks></remarks>
        Function DateLocalToUTC(ByVal LocalDate As Date) As Date
    End Interface 'Interface to HelperNET.Util

    ''' <summary>
    ''' Interface to the .NET Timer component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ITimer
        'Interface for the Timer class
        Inherits IDisposable

        ''' <summary>
        ''' Fired once per Interval when timer is Enabled.
        ''' </summary>
        ''' <remarks>To sink this event in Visual Basic, declare the object variable using the WithEvents keyword.</remarks>
        Event Tick()
        ''' <summary>
        ''' The interval between Tick events when the timer is Enabled in milliseconds, (default = 1000)
        ''' </summary>
        ''' <value>The interval between Tick events when the timer is Enabled (milliseconds, default = 1000)</value>
        ''' <returns>The interval between Tick events when the timer is Enabled in milliseconds</returns>
        ''' <remarks></remarks>
        Property Interval() As Integer
        ''' <summary>
        ''' Enable the timer tick events
        ''' </summary>
        ''' <value>True means the timer is active and will deliver Tick events every Interval milliseconds.</value>
        ''' <returns>Enabled state of timer tick events</returns>
        ''' <remarks></remarks>
        Property Enabled() As Boolean
    End Interface 'Interface to HelperNET.Timer

    ''' <summary>
    ''' Interface to the .NET Profile component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IProfile
        Inherits IDisposable
        'Interface for the Profile class
        ''' <summary>
        ''' The type of ASCOM device for which profile data and registration services are provided 
        ''' (String, default = "Telescope")
        ''' </summary>
        ''' <value>String describing the type of device being accessed</value>
        ''' <returns>String describing the type of device being accessed</returns>
        ''' <remarks></remarks>
        Property DeviceType() As String

        ''' <summary>
        ''' List the device types registered in the Profile store
        ''' </summary>
        ''' <value>List of registered device types</value>
        ''' <returns>A sorted string list of device types</returns>
        ''' <remarks>Use this to find which types of device are registered in the Profile store.</remarks>
        ReadOnly Property RegisteredDeviceTypes() As Generic.List(Of String)

        ''' <summary>
        ''' List the devices of a given device type that are registered in the Profile store
        ''' </summary>
        ''' <param name="DeviceType">Type of devices to list</param>
        ''' <value>List of registered devices</value>
        ''' <returns>A sorted list of installed devices and associated device descriptions</returns>
        ''' <exception cref="Exceptions.InvalidValueException">Throw if the supplied DeviceType is empty string or 
        ''' null value.</exception>
        ''' <remarks>
        ''' Use this to find all the registerd devices of a given type that are in the Profile store.
        ''' <para>If a DeviceType is supplied, where no device of that type has been registered before on this system,
        ''' an empty list will be returned</para>
        ''' </remarks>
        ReadOnly Property RegisteredDevices(ByVal DeviceType As String) As Generic.SortedList(Of String, String)

        ''' <summary>
        ''' Confirms whether a specific driver is registered ort unregistered in the profile store
        ''' </summary>
        ''' <param name="DriverID">String reprsenting the device's ProgID</param>
        ''' <returns>Boolean indicating registered or unregisteredstate of the supplied DriverID</returns>
        ''' <remarks></remarks>
        Function IsRegistered(ByVal DriverID As String) As Boolean
        ''' <summary>
        ''' Registers a supplied DriverID and associates a descriptive name with the device
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to register</param>
        ''' <param name="DescriptiveName">Descriptive name of the device</param>
        ''' <remarks>Does nothing if already registered, so safe to call on driver load.</remarks>
        Sub Register(ByVal DriverID As String, ByVal DescriptiveName As String)
        ''' <summary>
        ''' Remove all data for the given DriverID from the registry.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to unregister</param>
        ''' <remarks>This deletes the entire device profile tree, including the DriverID root key.</remarks>
        Sub Unregister(ByVal DriverID As String)
        ''' <summary>
        ''' Retrieve a string value from the profile for the given Driver ID and variable name
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <returns>Retrieved variable value</returns>
        ''' <remarks>
        ''' <para>Name may be an empty string for the unnamed value. The unnamed value is also known as the "default" value for a registry key.</para>
        ''' <para>Does not provide access to other registry data types such as binary and doubleword. </para>
        ''' </remarks>
        Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String) As String
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
        Overloads Function GetValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String) As String
        ''' <summary>
        ''' Writes a string value to the profile using the given Driver ID and variable name.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="Value">The string value to be written</param>
        ''' <remarks></remarks>
        Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String)
        ''' <summary>
        ''' Writes a string value to the profile using the supplied subkey for the given Driver ID and variable name.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="Value">The string value to be written</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks></remarks>
        Overloads Sub WriteValue(ByVal DriverID As String, ByVal Name As String, ByVal Value As String, ByVal SubKey As String)
        ''' <summary>
        ''' Return a list of the (unnamed and named variables) under the given DriverID.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <returns>Generic Sorted List of KeyValuePairs</returns>
        ''' <remarks>The returned object contains entries for each value. For each entry, 
        ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        ''' Key property set to an empty string. </remarks>
        Overloads Function Values(ByVal DriverID As String) As Generic.SortedList(Of String, String)
        ''' <summary>
        ''' Return a list of the (unnamed and named variables) under the given DriverID subkey
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <returns>Generic Sorted List of KeyValuePairs</returns>
        ''' <remarks>The returned object contains entries for each value. For each entry, 
        ''' the Key property is the value's name, and the Value property is the string value itself. Note that the unnamed (default) 
        ''' value will be included if it has a value, even if the value is a blank string. The unnamed value will have its entry's 
        ''' Key property set to an empty string. </remarks>
        Overloads Function Values(ByVal DriverID As String, ByVal SubKey As String) As Generic.SortedList(Of String, String)
        ''' <summary>
        ''' Delete the value from the registry. Name may be an empty string for the unnamed value. 
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
        Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String)
        ''' <summary>
        ''' Delete the value from the registry. Name may be an empty string for the unnamed value. Value will be deleted from the subkey supplied.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="Name">Name of the variable whose value is retrieved</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>Specify "" to delete the unnamed value which is also known as the "default" value for a registry key. </remarks>
        Overloads Sub DeleteValue(ByVal DriverID As String, ByVal Name As String, ByVal SubKey As String)
        ''' <summary>
        ''' Create a registry key for the given DriverID.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>If the SubKey argument contains a \ separated path, the intermediate keys will be created if needed. </remarks>
        Sub CreateSubKey(ByVal DriverID As String, ByVal SubKey As String)
        ''' <summary>
        ''' Return a list of the sub-keys under the given DriverID.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <returns>Generic Sorted List of key-value pairs</returns>
        ''' <remarks>The returned Generic.SortedList object contains entries for each sub-key. For each KeyValuePair in the list, 
        ''' the Key property is the sub-key name, and the Value property is the value. The unnamed ("default") value for that key is also returned.</remarks>
        Overloads Function SubKeys(ByVal DriverID As String) As Generic.SortedList(Of String, String)
        ''' <summary>
        ''' Return a list of the sub-keys under the given DriverID and sub-key
        ''' </summary>
        ''' <param name="DriverID">ProgID of the driver</param>
        ''' <param name="SubKey">Subkey from the profile root in which to search for subkeys</param>
        ''' <returns>Generic Sorted List of key-value pairs</returns>
        ''' <remarks>The return type uses generics in order to specify the types of the key and value which are both string.</remarks>
        Overloads Function SubKeys(ByVal DriverID As String, ByVal SubKey As String) As Generic.SortedList(Of String, String)
        ''' <summary>
        ''' Delete a registry key for the given DriverID. SubKey may contain \ separated path to key to be deleted.
        ''' </summary>
        ''' <param name="DriverID">ProgID of the device to read from</param>
        ''' <param name="SubKey">Subkey from the profile root in which to write the value</param>
        ''' <remarks>The sub-key and all data and keys beneath it are deleted.</remarks>
        Sub DeleteSubKey(ByVal DriverID As String, ByVal SubKey As String)
    End Interface 'Interface to HelperNET.Profile

    ''' <summary>
    ''' Interface to the .NET Serial component
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ISerial
        Inherits IDisposable

        'Interface for the standard ASCOM serial functionality
        ''' <summary>
        ''' Returns a list of all available ASCOM serial ports with COMnnn ports sorted into ascending port number order
        ''' </summary>
        ''' <value>String array of available serial ports</value>
        ''' <returns>A string array of available serial ports</returns>
        ''' <remarks></remarks>
        ReadOnly Property AvailableComPorts() As String()
        ''' <summary>
        ''' Gets or sets the number of data bits in each byte
        ''' </summary>
        ''' <value>The number of data bits in each byte, default is 8 data bits</value>
        ''' <returns>Integer number of data bits in each byte</returns>
        ''' <exception cref="ArgumentOutOfRangeException">The data bits value is less than 5 or more than 8</exception>
        ''' <remarks></remarks>
        Property DataBits() As Integer
        ''' <summary>
        ''' Gets or sets the state of the DTR line
        ''' </summary>
        ''' <value>The state of the DTR line, default is enabled</value>
        ''' <returns>Boolean true/false indicating enabled/disabled</returns>
        ''' <remarks></remarks>
        Property DTREnable() As Boolean
        ''' <summary>
        ''' Gets or sets the type of parity check used over the serial link
        ''' </summary>
        ''' <value>The type of parity check used over the serial link, default none</value>
        ''' <returns>One of the Ports.Parity enumeration values</returns>
        ''' <remarks></remarks>
        Property Parity() As Ports.Parity
        ''' <summary>
        ''' Gets or sets the number of stop bits used on the serial link
        ''' </summary>
        ''' <value>the number of stop bits used on the serial link, default 1</value>
        ''' <returns>One of the Ports.StopBits enumeration values</returns>
        ''' <remarks></remarks>
        Property StopBits() As Ports.StopBits
        ''' <summary>
        ''' Gets or sets the type of serial handshake used on the serial line
        ''' </summary>
        ''' <value>The type of handshake used on the serial line, default is none</value>
        ''' <returns>One of the Ports.Handshake enumeration values</returns>
        ''' <remarks></remarks>
        Property Handshake() As Ports.Handshake
        ''' <summary>
        ''' Gets or sets the connected state of the ASCOM serial port.
        ''' </summary>
        ''' <value>Connected state of the serial port.</value>
        ''' <returns><c>True</c> if the serial port is connected.</returns>
        ''' <remarks>Set this property to True to connect to the serial (COM) port. You can read the property to determine if the object is connected. </remarks>
        Property Connected() As Boolean
        ''' <summary>
        ''' Gets or sets the number of the ASCOM serial port (Default is 1, giving COM1 as the serial port name).
        ''' </summary>
        ''' <value>COM port number of the ASCOM serial port.</value>
        ''' <returns>Integer, number of the ASCOM serial port</returns>
        ''' <remarks>This works for serial port names of the form COMnnn. Use PortName if your COM port name does not fit the form COMnnn.</remarks>
        Property Port() As Integer
        ''' <summary>
        ''' The maximum time that the ASCOM serial port will wait for incoming receive data (seconds, default = 5)
        ''' </summary>
        ''' <value>Integer, serial port timeout in seconds</value>
        ''' <returns>Integer, serial port timeout in seconds.</returns>
        ''' <remarks>The minimum delay timout that can be set through this command is 1 seconds. Use ReceiveTimeoutMs to set a timeout less than 1 second.</remarks>
        Property ReceiveTimeout() As Integer
        ''' <summary>
        ''' The maximum time that the ASCOM serial port will wait for incoming receive data (milliseconds, default = 5000)
        ''' </summary>
        ''' <value>Integer, serial port timeout in milli-seconds</value>
        ''' <returns>Integer, serial port timeout in milli-seconds</returns>
        ''' <remarks>If a timeout occurs, an IO timeout error is raised. See ReceiveTimeout for an alternate form 
        ''' using the timeout in seconds. </remarks>
        Property ReceiveTimeoutMs() As Integer
        ''' <summary>
        ''' Sets the ASCOM serial port name as a string
        ''' </summary>
        ''' <value>Serial port name to be used</value>
        ''' <returns>Current serial port name</returns>
        ''' <remarks>This property allows any serial port name to be used, even if it doesn't conform to a format of COMnnn
        ''' If the required port name is of the form COMnnn then Serial.Port=nnn and Serial.PortName="COMnnn" are 
        ''' equivalent.</remarks>
        Property PortName() As String
        ''' <summary>
        ''' Gets and sets the baud rate of the ASCOM serial port
        ''' </summary>
        ''' <value>Port speed using the PorSpeed enum</value>
        ''' <returns>Port speed using the PortSpeed enum</returns>
        ''' <remarks>This is modelled on the COM component with possible values enumerated in the PortSpeed enum.</remarks>
        Property Speed() As Serial.PortSpeed
        ''' <summary>
        ''' Clears the ASCOM serial port receive and transmit buffers
        ''' </summary>
        ''' <remarks></remarks>
        Sub ClearBuffers()
        ''' <summary>
        ''' Transmits a string through the ASCOM serial port
        ''' </summary>
        ''' <param name="Data">String to transmit</param>
        ''' <remarks></remarks>
        Sub Transmit(ByVal Data As String)
        ''' <summary>
        ''' Transmit an array of binary bytes through the ASCOM serial port 
        ''' </summary>
        ''' <param name="Data">Byte array to transmit</param>
        ''' <remarks></remarks>
        Sub TransmitBinary(ByVal Data As Byte())
        ''' <summary>
        ''' Adds a message to the ASCOM serial trace file
        ''' </summary>
        ''' <param name="Caller">String identifying the module logging the message</param>
        ''' <param name="Message">Message text to be logged.</param>
        ''' <remarks>
        ''' <para>This can be called regardless of whether logging is enabled</para>
        ''' </remarks>
        Sub LogMessage(ByVal Caller As String, ByVal Message As String)
        ''' <summary>
        ''' Receive at least one text character from the ASCOM serial port
        ''' </summary>
        ''' <returns>The characters received</returns>
        ''' <remarks>This method reads all of the characters currently in the serial receive buffer. It will not return 
        ''' unless it reads at least one character. A timeout will cause a TimeoutException to be raised. Use this for 
        ''' text data, as it returns a String. </remarks>
        Function Receive() As String
        ''' <summary>
        ''' Receive one binary byte from the ASCOM serial port
        ''' </summary>
        ''' <returns>The received byte</returns>
        ''' <remarks>Use this for 8-bit (binary data). If a timeout occurs, a TimeoutException is raised. </remarks>
        Function ReceiveByte() As Byte
        ''' <summary>
        ''' Receive exactly the given number of characters from the ASCOM serial port and return as a string
        ''' </summary>
        ''' <param name="Count">The number of characters to return</param>
        ''' <returns>String of length "Count" characters</returns>
        ''' <remarks>If a timeout occurs a TimeoutException is raised.</remarks>
        Function ReceiveCounted(ByVal Count As Integer) As String
        ''' <summary>
        ''' Receive exactly the given number of characters from the ASCOM serial port and return as a byte array
        ''' </summary>
        ''' <param name="Count">The number of characters to return</param>
        ''' <returns>Byte array of size "Count" elements</returns>
        ''' <remarks>
        ''' <para>If a timeout occurs, a TimeoutException is raised. </para>
        ''' <para>This function exists in the COM component but is not documented in the help file.</para>
        ''' </remarks>
        Function ReceiveCountedBinary(ByVal Count As Integer) As Byte()
        ''' <summary>
        ''' Receive characters from the ASCOM serial port until the given terminator string is seen
        ''' </summary>
        ''' <param name="Terminator">The character string that indicates end of message</param>
        ''' <returns>Received characters including the terminator string</returns>
        ''' <remarks>If a timeout occurs, a TimeoutException is raised.</remarks>
        Function ReceiveTerminated(ByVal Terminator As String) As String
        ''' <summary>
        ''' Receive characters from the ASCOM serial port until the given terminator bytes are seen, return as a byte array
        ''' </summary>
        ''' <param name="TerminatorBytes">Array of bytes that indicates end of message</param>
        ''' <returns>Byte array of received characters</returns>
        ''' <remarks>
        ''' <para>If a timeout occurs, a TimeoutException is raised.</para>
        ''' <para>This function exists in the COM component but is not documented in the help file.</para>
        ''' </remarks>
        Function ReceiveTerminatedBinary(ByVal TerminatorBytes() As Byte) As Byte()
    End Interface  'Interface to HelperNET.Serial

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
        Inherits IDisposable
        Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) As String
        Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String)
        Function EnumProfile(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String)
        Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String)
        Sub CreateKey(ByVal p_SubKeyName As String)
        Function EnumKeys(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String)
        Sub DeleteKey(ByVal p_SubKeyName As String)
        Sub RenameKey(ByVal CurrentSubKeyName As String, ByVal NewSubKeyName As String)
    End Interface 'Interface for a general profile store provider
End Namespace
#End Region

#Region "Exceptions"
Namespace Exceptions
    ''' <summary>
    ''' Base exception for the HelperNET components
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class HelperException
        'Exception for Helper.NET component exceptions
        Inherits System.Exception

        ''' <summary>
        ''' Create a new exception with message
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message and inner exception
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub

    End Class

    ''' <summary>
    ''' Exception thrown when the profile is not found. This is internally trapped and should not appear externally to an application.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Friend Class ProfileNotFoundException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when an invalid value is passed to a HelperNET component
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class InvalidValueException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when a serial port method is already in progress and a second attempt is made to use the serial port.
    ''' </summary>
    ''' <remarks>This exception is only thrown after 5 attempts, each with a 1 second timeout, have been made to 
    ''' acquire the serial port. It may indicate that you have more than one thread attempting to access the serial 
    ''' port and that you have not synchronised these within your application. The serial port can only handle 
    ''' one transaction at a time e.g. Serial.Receive or Serial.Transmit etc.</remarks>
    <Serializable()> _
        Public Class SerialPortInUseException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                ByVal info As System.Runtime.Serialization.SerializationInfo, _
                ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown if there is any problem in reading or writing the profile from or to the file system
    ''' </summary>
    ''' <remarks>This is an ifrastructural exception indicatig that there is a problem at the file access layer
    ''' in the profile code. Possible underlying reasons are security access permissions, file system full and hardware failure.
    ''' </remarks>
    <Serializable()> _
        Public Class ProfilePersistenceException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when a profile request is made for a driver that is not registered
    ''' </summary>
    ''' <remarks>Drivers must be registered before other profile commands are used to manipulate their 
    ''' values.</remarks>
    <Serializable()> _
        Public Class DriverNotRegisteredException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    ''' <summary>
    ''' Exception thrown when an attempt is made to write to a protected part of the the Profile store that is 
    ''' reserved for Platform use. An example is attempting to write to the the default value of a device driver 
    ''' profile. This value is reserved for use by the Chooser to display the device description and is set by the 
    ''' Profile.Register method.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
        Public Class RestrictedAccessException
        'Exception for Helper.NET component exceptions
        Inherits HelperException

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Create a new exception with message 
        ''' </summary>
        ''' <param name="message">Message to be reported by the exception</param>
        ''' <param name="inner">Exception to be reported as the inner exception</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal inner As Exception)
            MyBase.New(message, inner)
        End Sub

        ''' <summary>
        ''' Serialise the exception
        ''' </summary>
        ''' <param name="info">Serialisation information</param>
        ''' <param name="context">Serialisation context</param>
        ''' <remarks></remarks>
        Public Sub New( _
                    ByVal info As System.Runtime.Serialization.SerializationInfo, _
                    ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class


End Namespace
#End Region

#Region "Registry Utility Code"
Module RegistryCommonCode
    Friend Function GetBool(ByVal p_Name As String, ByVal p_DefaultValue As Boolean) As Boolean
        Dim l_Value As Boolean
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CBool(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetBool", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetString(ByVal p_Name As String, ByVal p_DefaultValue As String) As String
        Dim l_Value As String = ""
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = m_SettingsKey.GetValue(p_Name).ToString
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetString", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetDouble(ByVal p_Key As RegistryKey, ByVal p_Name As String, ByVal p_DefaultValue As Double) As Double
        Dim l_Value As Double
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

        'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgDebug, p_Name.ToString & " " & p_DefaultValue.ToString)
        Try
            If p_Key.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDbl(p_Key.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetDate(ByVal p_Name As String, ByVal p_DefaultValue As Date) As Date
        Dim l_Value As Date
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDate(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDate", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Sub SetName(ByVal p_Name As String, ByVal p_Value As String)
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

        m_SettingsKey.SetValue(p_Name, p_Value.ToString, RegistryValueKind.String)
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

    End Sub
End Module
#End Region

#Region "Version Code"

Module VersionCode
    Friend Sub RunningVersions(ByVal TL As TraceLogger)
        Dim AssemblyNames() As AssemblyName
        TL.LogMessage("Versions", "HelperNET version: " & Assembly.GetExecutingAssembly.GetName.Version.ToString)
        TL.LogMessage("Versions", "CLR version: " & System.Environment.Version.ToString)
        AssemblyNames = Assembly.GetExecutingAssembly.GetReferencedAssemblies
        'For Each AssName AssName As AssemblyName In AssemblyNames
        ' TL.LogMessage("Versions", AssName.Name & " " & AssName.Version.ToString & " " & AssName.CodeBase)
        ' Next
    End Sub

End Module
#End Region
