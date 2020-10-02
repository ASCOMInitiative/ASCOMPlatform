﻿'Implements the TraceLogger component

Imports System.IO
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions
Imports System.Runtime.InteropServices

''' <summary>
''' Creates a log file for a driver or application. Uses a similar file name and internal format to the serial logger. Multiple logs can be created simultaneously if needed.
''' </summary>
''' <remarks>
'''<para>In automatic mode the file will be stored in an ASCOM folder within XP's My Documents folder or equivalent places 
''' in other operating systems. Within the ASCOM folder will be a folder named Logs yyyy-mm-dd where yyyy, mm and dd are 
''' today's year, month and day numbers. The trace file will appear within the day folder with the name 
''' ASCOM.Identifier.hhmm.ssffff where hh, mm, ss and ffff are the current hour, minute, second and fraction of second 
''' numbers at the time of file creation.
''' </para> 
''' <para>Within the file the format of each line is hh:mm:ss.fff Identifier Message where hh, mm, ss and fff are the hour, minute, second 
''' and fractional second at the time that the message was logged, Identifier is the supplied identifier (usually the subroutine, 
''' function, property or method from which the message is sent) and Message is the message to be logged.</para>
'''</remarks>
Public Class TraceLogger
    Implements ITraceLogger, ITraceLoggerExtra, IDisposable

    Private Const IDENTIFIER_WIDTH_DEFAULT As Integer = 25 ' Default width of the identifier field. The width can be changed through the TraceLogger.IdentifierWidth property

    Private g_LogFileName, g_LogFileType As String
    Private g_LogFile As System.IO.StreamWriter
    Private g_LineStarted As Boolean
    Private g_Enabled As Boolean
    Private g_DefaultLogFilePath As String ' Variable to hold the default log file path determined by TraceLogger at run time
    Private g_LogFileActualName As String 'Full name of the log file being created (includes automatic file name)
    Private g_LogFilePath As String ' Variable to hold a user specified log file path
    Private g_IdentifierWidth As Integer ' Variable to hold the current identifier field width

    Private mut As System.Threading.Mutex
    Private GotMutex As Boolean

#Region "New and IDisposable Support"
    Private traceLoggerHasBeenDisposed As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Creates a new TraceLogger instance
    ''' </summary>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of 
    ''' several logs contains the information of interest.
    ''' <para>This call enables automatic logging and sets the file type to "Default".</para></remarks>
    Public Sub New()
        MyBase.New()
        g_IdentifierWidth = IDENTIFIER_WIDTH_DEFAULT
        g_LogFileName = "" 'Set automatic filenames as default
        g_LogFileType = "Default" '"Set an arbitrary name in case someone forgets to call SetTraceLog
        g_DefaultLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & TRACE_LOGGER_PATH & TRACE_LOGGER_FILENAME_BASE & Format(Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT)
        mut = New Threading.Mutex(False, "TraceLoggerMutex")
    End Sub

    ''' <summary>
    ''' Creates a new TraceLogger instance and initialises filename and type
    ''' </summary>
    ''' <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
    ''' <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
    Public Sub New(ByVal LogFileName As String, ByVal LogFileType As String)
        MyBase.New()
        g_IdentifierWidth = IDENTIFIER_WIDTH_DEFAULT
        g_LogFileName = LogFileName 'Save parameters to use when the first call to write a record is made
        g_LogFileType = LogFileType
        g_DefaultLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & TRACE_LOGGER_PATH & TRACE_LOGGER_FILENAME_BASE & Format(Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT)
        mut = New Threading.Mutex
    End Sub

    ''' <summary>
    ''' Create and enable a new TraceLogger instance with automatic naming based on the supplied log file type
    ''' </summary>
    ''' <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
    Public Sub New(ByVal LogFileType As String)
        MyBase.New()
        g_IdentifierWidth = IDENTIFIER_WIDTH_DEFAULT
        g_LogFileName = "" 'Set automatic filenames as default
        g_LogFileType = LogFileType
        g_DefaultLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & TRACE_LOGGER_PATH & TRACE_LOGGER_FILENAME_BASE & Format(Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT)
        mut = New Threading.Mutex
        g_Enabled = True ' Enable the log
    End Sub

    ' IDisposable
    ''' <summary>
    ''' Disposes of the TraceLogger object
    ''' </summary>
    ''' <param name="disposing">True if being disposed by the application, False if disposed by the finaliser.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.traceLoggerHasBeenDisposed Then
            Me.traceLoggerHasBeenDisposed = True
            If disposing Then
                If Not (g_LogFile Is Nothing) Then
                    Try : g_LogFile.Flush() : Catch : End Try
                    Try : g_LogFile.Close() : Catch : End Try
                    Try : g_LogFile.Dispose() : Catch : End Try
                    g_LogFile = Nothing
                End If
                If Not (mut Is Nothing) Then
                    'Try : mut.ReleaseMutex() : Catch : End Try
                    Try : mut.Close() : Catch : End Try
                    mut = Nothing
                End If
            End If
        End If
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of the TraceLogger object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Finalizes the TraceLogger object
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "ITraceLogger Implementation"

    ''' <summary>
    ''' Logs an issue, closing any open line and opening a continuation line if necessary after the 
    ''' issue message.
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <remarks>Use this for reporting issues that you don't want to appear on a line already opened 
    ''' with StartLine</remarks>
    Public Sub LogIssue(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogIssue
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogIssue", """" & Identifier & """, """ & Message & """")
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                If g_LineStarted Then g_LogFile.WriteLine()
                LogMsgFormatter(Identifier, Message, True, False)
                If g_LineStarted Then LogMsgFormatter("Continuation", "", False, False)
            End If
        Finally
            mut.ReleaseMutex()
        End Try

    End Sub

    ''' <summary>
    ''' Insert a blank line into the log file
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BlankLine() Implements ITraceLogger.BlankLine
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        LogMessage("", "", False)
    End Sub

    ''' <summary>
    ''' Logs a complete message in one call, including a hex translation of the message
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    ''' <remarks>
    ''' <para>Use this for straightforward logging requirements. Writes all information in one command.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogMessage
        Dim Msg As String = Message

        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogMessage", """" & Identifier & """, """ & Message & """, " & HexDump.ToString & """")
            If g_LineStarted Then LogFinish(" ") ' 1/10/09 PWGS Silently close the open line

            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
                LogMsgFormatter(Identifier, Msg, True, False)
            End If

        Finally
            mut.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Displays a message respecting carriage return and linefeed characters
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <remarks>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Sub LogMessageCrLf(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogMessageCrLf
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogMessage", """" & Identifier & """, """ & Message & """")
            If g_LineStarted Then LogFinish(" ") ' 1/10/09 PWGS Silently close the open line

            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                LogMsgFormatter(Identifier, Message, True, True)
            End If

        Finally
            mut.ReleaseMutex()
        End Try

    End Sub

    ''' <summary>
    ''' Writes the time and identifier to the log, leaving the line ready for further content through LogContinue and LogFinish
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <remarks><para>Use this to start a log line where you want to write further information on the line at a later time.</para>
    ''' <para>E.g. You might want to use this to record that an action has started and then append the word OK if all went well.
    '''  You would then end up with just one line to record the whole transaction even though you didn't know that it would be 
    ''' successful when you started. If you just used LogMsg you would have ended up with two log lines, one showing 
    ''' the start of the transaction and the next the outcome.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Sub LogStart(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogStart
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogStart", """" & Identifier & """, """ & Message & """")
            If g_LineStarted Then
                LogFinish("LOGISSUE: LogStart has been called before LogFinish. Parameters: " & Identifier & " " & Message)
            Else
                g_LineStarted = True
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    LogMsgFormatter(Identifier, Message, False, False)
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Appends further message to a line started by LogStart, appends a hex translation of the message to the line, does not terminate the line.
    ''' </summary>
    ''' <param name="Message">The additional message to appear in the line</param>
    ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    ''' <remarks>
    ''' <para>This can be called multiple times to build up a complex log line if required.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogContinue(ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogContinue
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        ' Append a full hex dump of the supplied string if p_Hex is true
        Dim Msg As String = Message
        If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
        LogContinue(Msg)
    End Sub

    ''' <summary>
    ''' Closes a line started by LogStart with the supplied message and a hex translation of the message
    ''' </summary>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    ''' <remarks>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogFinish(ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogFinish
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        ' Append a full hex dump of the supplied string if p_Hex is true
        Dim Msg As String = Message
        If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
        LogFinish(Msg)
    End Sub

    ''' <summary>
    ''' Enables or disables logging to the file.
    ''' </summary>
    ''' <value>True to enable logging</value>
    ''' <returns>Boolean, current logging status (enabled/disabled).</returns>
    ''' <remarks>If this property is false then calls to LogMsg, LogStart, LogContinue and LogFinish do nothing. If True, 
    ''' supplied messages are written to the log file.</remarks>
    Public Property Enabled() As Boolean Implements ITraceLogger.Enabled
        Get
            Return g_Enabled
        End Get
        Set(ByVal value As Boolean)
            g_Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the log filename and type if the constructor is called without parameters
    ''' </summary>
    ''' <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
    ''' <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the 
    ''' information of interest.
    ''' <para><b>Note </b>This command is only required if the trace logger constructor is called with no
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
    Public Sub SetLogFile(ByVal LogFileName As String, ByVal LogFileType As String) Implements ITraceLogger.SetLogFile
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        g_LogFileName = LogFileName 'Save parameters to use when the first call to write a record is made
        g_LogFileType = LogFileType
    End Sub

    ''' <summary>
    ''' Return the full filename of the log file being created
    ''' </summary>
    ''' <value>Full filename of the log file</value>
    ''' <returns>String filename</returns>
    ''' <remarks>This call will return an empty string until the first line has been written to the log file
    ''' as the file is not created until required.</remarks>
    Public ReadOnly Property LogFileName() As String Implements ITraceLogger.LogFileName
        Get
            Return g_LogFileActualName
        End Get
    End Property

    ''' <summary>
    ''' Set or return the path to a directory in which the log file will be created
    ''' </summary>
    ''' <returns>String path</returns>
    ''' <remarks>Introduced with Platform 6.4.<para>If set, this path will be used instead of the user's Documents directory default path. This must be Set before the first message Is logged.</para></remarks>
    Public Property LogFilePath() As String Implements ITraceLogger.LogFilePath
        Get
            Return g_LogFilePath
        End Get
        Set(value As String)
            g_LogFilePath = value.TrimEnd("\".ToCharArray) ' Save the value and remove any trailing \ characters that will mess up file name creation later
        End Set
    End Property

    ''' <summary>
    ''' Set or return the width of the identifier field in the log message
    ''' </summary>
    ''' <value>Width of the identifier field</value>
    ''' <returns>Integer width</returns>
    ''' <remarks>Introduced with Platform 6.4.<para>If set, this width will be used instead of the default identifier field width.</para></remarks>
    Public Property IdentifierWidth As Integer Implements ITraceLogger.IdentifierWidth
        Get
            Return g_IdentifierWidth
        End Get
        Set(value As Integer)
            g_IdentifierWidth = value
        End Set
    End Property
#End Region

#Region "ITraceLoggerExtra Implementation"

    ''' <summary>
    ''' Logs a complete message in one call
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <remarks>
    ''' <para>Use this for straightforward logging requirements. Writes all information in one command.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean)"
    ''' with HexDump set False to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)>
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String) Implements ITraceLoggerExtra.LogMessage
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogMessage", """" & Identifier & """, """ & Message & """")
            If g_LineStarted Then LogFinish(" ") ' 1/10/09 PWGS Made line closure silent
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                LogMsgFormatter(Identifier, Message, True, False)
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Appends further message to a line started by LogStart, does not terminate the line.
    ''' </summary>
    ''' <param name="Message">The additional message to appear in the line</param>
    ''' <remarks>
    ''' <para>This can be called multiple times to build up a complex log line if required.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "LogContinue(ByVal Message As String, ByVal HexDump As Boolean)"
    ''' with HexDump set False to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)>
    Public Overloads Sub LogContinue(ByVal Message As String) Implements ITraceLoggerExtra.LogContinue
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogContinue", """" & Message & """")
            If Not g_LineStarted Then
                LogMessage("LOGISSUE", "LogContinue has been called before LogStart. Parameter: " & Message)
            Else
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    g_LogFile.Write(MakePrintable(Message, False)) 'Update log file without newline terminator
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Closes a line started by LogStart with the supplied message
    ''' </summary>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <remarks>
    ''' <para>Can only be called once for each line started by LogStart.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' <para>This overload is not available through COM, please use 
    ''' "LogFinish(ByVal Message As String, ByVal HexDump As Boolean)"
    ''' with HexDump set False to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)>
    Public Overloads Sub LogFinish(ByVal Message As String) Implements ITraceLoggerExtra.LogFinish
        If Me.traceLoggerHasBeenDisposed Then Return ' Ignore this call if the trace logger has been disposed

        Try
            GetTraceLoggerMutex("LogFinish", """" & Message & """")
            If Not g_LineStarted Then
                LogMessage("LOGISSUE", "LogFinish has been called before LogStart. Parameter: " & Message)
            Else
                g_LineStarted = False
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    g_LogFile.WriteLine(MakePrintable(Message, False)) 'Update log file with newline terminator
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub

#End Region

#Region "TraceLogger Support"
    Private Sub CreateLogFile()
        Dim FileNameSuffix As Integer = 0, ok As Boolean = False, FileNameBase, TodaysLogFilePath As String
        Select Case g_LogFileName
            'Case "" 'Do nothing - no log required
            '    Throw New HelperException("TRACELOGGER.CREATELOGFILE - Call made but no log filename has been set")
            Case "", SERIAL_AUTO_FILENAME
                If g_LogFileType = "" Then Throw New ValueNotSetException("TRACELOGGER.CREATELOGFILE - Call made but no log file type has been set")
                If g_LogFilePath = "" Then ' Default behaviour using the current user's Document directory
                    My.Computer.FileSystem.CreateDirectory(g_DefaultLogFilePath) 'Create the directory if it doesn't exist
                    FileNameBase = g_DefaultLogFilePath & "\ASCOM." & g_LogFileType & "." & Format(Now, "HHmm.ssfff")
                Else ' User has given a specific path so use that
                    TodaysLogFilePath = g_LogFilePath & TRACE_LOGGER_FILENAME_BASE & Format(Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT) ' Append Logs yyyy-mm-dd to the user supplied log file
                    My.Computer.FileSystem.CreateDirectory(TodaysLogFilePath) 'Create the directory if it doesn't exist
                    FileNameBase = TodaysLogFilePath & "\ASCOM." & g_LogFileType & "." & Format(Now, "HHmm.ssfff")
                End If
                Do 'Create a unique log file name based on date, time and required name
                    g_LogFileActualName = FileNameBase & FileNameSuffix.ToString & ".txt"
                    FileNameSuffix += 1 'Increment counter that ensures that no log file can have the same name as any other
                Loop Until (Not File.Exists(g_LogFileActualName))
                Try
                    g_LogFile = New StreamWriter(g_LogFileActualName, False)
                    g_LogFile.AutoFlush = True
                Catch ex As System.IO.IOException
                    ok = False
                    Do
                        Try
                            g_LogFileActualName = FileNameBase & FileNameSuffix.ToString & ".txt"
                            g_LogFile = New StreamWriter(g_LogFileActualName, False)
                            g_LogFile.AutoFlush = True
                            ok = True
                        Catch ex1 As System.IO.IOException
                            'Ignore IO exceptions and try the next filename
                        End Try
                        FileNameSuffix += 1
                    Loop Until (ok Or (FileNameSuffix = 20))
                    If Not ok Then Throw New ASCOM.Utilities.Exceptions.HelperException("TraceLogger:CreateLogFile - Unable to create log file", ex)
                End Try
            Case Else 'Create log file based on supplied name
                Try
                    g_LogFile = New StreamWriter(g_LogFileName & ".txt", False)
                    g_LogFile.AutoFlush = True
                    g_LogFileActualName = g_LogFileName & ".txt"
                Catch ex As Exception
                    MsgBox("CreateLogFile Exception - #" & g_LogFileName & "# " & ex.ToString)
                    Throw
                End Try
        End Select
    End Sub

    Private Function MakePrintable(ByVal p_Msg As String, ByVal p_RespectCrLf As Boolean) As String
        Dim l_Msg As String = ""
        Dim i, CharNo As Integer
        'Present any unprintable characters in [0xHH] format
        For i = 1 To Len(p_Msg)
            CharNo = Asc(Mid(p_Msg, i, 1))
            Select Case CharNo
                Case 10, 13 ' Either translate or respect CRLf depending on the setting of the respect parameter
                    If p_RespectCrLf Then
                        l_Msg &= Mid(p_Msg, i, 1)
                    Else
                        l_Msg &= "[" & Right("00" & Hex(CharNo), 2) & "]"
                    End If
                Case 0 - 9, 11, 12, 14 - 31, Is > 126 ' All other non-printables should be translated
                    l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
                Case Else 'Everything else is printable and should be left as is
                    l_Msg &= Mid(p_Msg, i, 1)
            End Select
            If CharNo < 32 Or CharNo > 126 Then
            Else
            End If
        Next
        Return l_Msg
    End Function

    Private Function MakeHex(ByVal p_Msg As String) As String
        Dim l_Msg As String = ""
        Dim i, CharNo As Integer
        'Present all characters in [0xHH] format
        For i = 1 To Len(p_Msg)
            CharNo = Asc(Mid(p_Msg, i, 1))
            l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
        Next
        Return l_Msg
    End Function

    Private Sub LogMsgFormatter(ByVal p_Test As String, ByVal p_Msg As String, ByVal p_NewLine As Boolean, ByVal p_RespectCrLf As Boolean)
        Try
            p_Test = Left(p_Test & StrDup(g_IdentifierWidth, " "), g_IdentifierWidth)

            Dim l_Msg As String = Format(Now(), "HH:mm:ss.fff") & " " & MakePrintable(p_Test, p_RespectCrLf) & " " & MakePrintable(p_Msg, p_RespectCrLf)
            If Not g_LogFile Is Nothing Then
                If p_NewLine Then
                    g_LogFile.WriteLine(l_Msg) 'Update log file with newline terminator
                Else
                    g_LogFile.Write(l_Msg) 'Update log file without newline terminator
                End If
                g_LogFile.Flush()
            End If
        Catch ex As Exception
            LogEvent("LogMsgFormatter", "Exception", EventLogEntryType.Error, EventLogErrors.TraceLoggerException, ex.ToString)
            'MsgBox("LogMsgFormatter exception: " & Len(l_Msg) & " *" & l_Msg & "* " & ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub GetTraceLoggerMutex(ByVal Method As String, ByVal Parameters As String)
        'Get the profile mutex or log an error and throw an exception that will terminate this profile call and return to the calling application
        Try
            'Try to acquire the mutex
            GotMutex = mut.WaitOne(PROFILE_MUTEX_TIMEOUT, False)
            'Catch the AbandonedMutexException but not any others, these are passed to the calling routine
        Catch ex As System.Threading.AbandonedMutexException
            ' We've received this exception but it indicates an issue in a PREVIOUS thread not this one. Log it and we have also got the mutex; so continue!
            LogEvent("TraceLogger", "AbandonedMutexException in " & Method & ", parameters: " & Parameters, EventLogEntryType.Error, EventLogErrors.TraceLoggerMutexAbandoned, ex.ToString)
            If GetBool(ABANDONED_MUTEXT_TRACE, ABANDONED_MUTEX_TRACE_DEFAULT) Then
                LogEvent("TraceLogger", "AbandonedMutexException in " & Method & ": Throwing exception to application", EventLogEntryType.Warning, EventLogErrors.TraceLoggerMutexAbandoned, Nothing)
                Throw 'Throw the exception in order to report it
            Else
                LogEvent("TraceLogger", "AbandonedMutexException in " & Method & ": Absorbing exception, continuing normal execution", EventLogEntryType.Warning, EventLogErrors.TraceLoggerMutexAbandoned, Nothing)
                GotMutex = True 'Flag that we have got the mutex.
            End If
        End Try

        'Check whether we have the mutex, throw an error if not
        If Not GotMutex Then
            LogEvent(Method, "Timed out waiting for TraceLogger mutex in " & Method & ", parameters: " & Parameters, EventLogEntryType.Error, EventLogErrors.TraceLoggerMutexTimeOut, Nothing)
            Throw New ProfilePersistenceException("Timed out waiting for TraceLogger mutex in " & Method & ", parameters: " & Parameters)
        End If
    End Sub

#End Region

End Class
