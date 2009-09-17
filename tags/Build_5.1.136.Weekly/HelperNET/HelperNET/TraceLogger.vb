'1/4/09 Converted LogFinishHex to overload of LogFinish
Imports System.IO
Imports ASCOM.HelperNET.Interfaces
Imports ASCOM.HelperNET.Exceptions

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
    Implements ITraceLogger, IDisposable

    Private g_LogFileName, g_LogFileType As String
    Private g_LogFile As System.IO.StreamWriter
    Private g_LineStarted As Boolean
    Private g_Enabled As Boolean
    Private g_LogFilePath As String

    Private mut As System.Threading.Mutex

#Region "New and IDisposable Support"
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Creates a new TraceLogger instance
    ''' </summary>
    ''' <param name="p_LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
    ''' <param name="p_LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
    Sub New(ByVal p_LogFileName As String, ByVal p_LogFileType As String)
        MyBase.New()
        g_LogFileName = p_LogFileName 'Save parameters to use when the first call to write a record is made
        g_LogFileType = p_LogFileType
        g_LogFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")
        mut = New System.Threading.Mutex
    End Sub

    ' IDisposable
    ''' <summary>
    ''' Disposes of the TraceLogger object
    ''' </summary>
    ''' <param name="disposing">True if being disposed by the application, False if disposed by the finalizer.</param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
            If Not (g_LogFile Is Nothing) Then
                Try : g_LogFile.Flush() : Catch : End Try
                Try : g_LogFile.Close() : Catch : End Try
                Try : g_LogFile.Dispose() : Catch : End Try
                g_LogFile = Nothing
            End If
            If Not (mut Is Nothing) Then
                Try : mut.ReleaseMutex() : Catch : End Try
                Try : mut.Close() : Catch : End Try
                mut = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Disposes of the TraceLogger object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Finalizes the TraceLogger object
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "TraceLogger"

    ''' <summary>
    ''' Logs an issue, closing any open line and opening a continuation line if necessary after the 
    ''' issue message.
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <remarks>Use this for reporting issues that you don't want to appear on a line already opened 
    ''' with StartLine</remarks>
    Public Sub LogIssue(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogIssue
        Try
            mut.WaitOne()
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                If g_LineStarted Then g_LogFile.WriteLine()
                LogMsgFormatter(Identifier, Message, True)
                If g_LineStarted Then LogMsgFormatter("Continuation", "", False)
            End If
        Finally
            mut.ReleaseMutex()
        End Try

    End Sub

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
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogMessage
        Try
            mut.WaitOne()
            If g_LineStarted Then
                LogFinish("LOGISSUE: LogMessage has been called before LogFinish. Parameters: " & Identifier & " " & Message)
            Else
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    LogMsgFormatter(Identifier, Message, True)
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub
    ''' <summary>
    ''' Logs a complete message in one call, including a hex translation of the message
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    ''' <param name="Message">Message to log</param>
    ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    ''' <remarks>
    ''' <para>Use this for straightforward logging requrements. Writes all information in one command.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogMessage
        Dim Msg As String = Message
        Try
            mut.WaitOne()
            If g_LineStarted Then
                LogFinish("LOGISSUE: LogMessage has been called before LogFinish. Parameters: " & Identifier & " " & Message)
            Else
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
                    LogMsgFormatter(Identifier, Msg, True)
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub
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
    Public Sub LogStart(ByVal Identifier As String, ByVal Message As String) Implements ITraceLogger.LogStart
        Try
            mut.WaitOne()
            If g_LineStarted Then
                LogFinish("LOGISSUE: LogStart has been called before LogFinish. Parameters: " & Identifier & " " & Message)
            Else
                g_LineStarted = True
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    LogMsgFormatter(Identifier, Message, False)
                End If
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
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogContinue(ByVal Message As String) Implements ITraceLogger.LogContinue
        Try
            mut.WaitOne()
            If Not g_LineStarted Then
                LogMessage("LOGISSUE", "LogContinue has been called before LogStart. Parameter: " & Message)
            Else
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    g_LogFile.Write(MakePrintable(Message)) 'Update log file without newline terminator
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
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogContinue(ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogContinue
        ' Append a full hex dump of the supplied string if p_Hex is true
        Dim Msg As String = Message
        If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
        LogContinue(Msg)
    End Sub
    ''' <summary>
    ''' Closes a line started by LogStart with the supplied message
    ''' </summary>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <remarks>
    ''' <para>Can only be called once for each line started by LogStart.</para>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogFinish(ByVal Message As String) Implements ITraceLogger.LogFinish
        Try
            mut.WaitOne()
            If Not g_LineStarted Then
                LogMessage("LOGISSUE", "LogFinish has been called before LogStart. Parameter: " & Message)
            Else
                g_LineStarted = False
                If g_Enabled Then
                    If g_LogFile Is Nothing Then Call CreateLogFile()
                    g_LogFile.WriteLine(MakePrintable(Message)) 'Update log file with newline terminator
                End If
            End If
        Finally
            mut.ReleaseMutex()
        End Try
    End Sub
    ''' <summary>
    ''' Closes a line started by LogStart with the supplied message and a hex translation of the message
    ''' </summary>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    ''' <remarks>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Overloads Sub LogFinish(ByVal Message As String, ByVal HexDump As Boolean) Implements ITraceLogger.LogFinish
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

#End Region

#Region "TraceLogger Support"
    Private Sub CreateLogFile()
        Dim LogFileName As String = "", FileNameSuffix As Integer = 0
        Select Case g_LogFileName
            'Case "" 'Do nothing - no log required
            '    Throw New HelperException("TRACELOGGER.CREATELOGFILE - Call made but no log filename has been set")
            Case "", SERIAL_AUTO_FILENAME
                If g_LogFileType = "" Then Throw New InvalidValueException("TRACELOGGER.CREATELOGFILE - Call made but no log filetype has been set")
                My.Computer.FileSystem.CreateDirectory(g_LogFilePath) 'Create the directory if it doesn't exist
                Do 'Create a unique log file name based on date, time and required name
                    LogFileName = g_LogFilePath & "\ASCOM." & g_LogFileType & "." & Format(Now, "HHmm.ssfff") & FileNameSuffix.ToString & ".txt"
                    FileNameSuffix += 1 'Increment counter that ensures that no logfile can have the same name as any other
                Loop Until Not File.Exists(LogFileName)
                g_LogFile = New StreamWriter(LogFileName, False)

                g_LogFile.AutoFlush = True
            Case Else 'Create log file based on supplied name
                Try
                    g_LogFile = New StreamWriter(g_LogFileName & ".txt", False)
                    g_LogFile.AutoFlush = True
                Catch ex As Exception
                    MsgBox("CreateLogFile Exception - #" & g_LogFileName & "# " & ex.ToString)
                    Throw
                End Try
        End Select
    End Sub

    Private Function MakePrintable(ByVal p_Msg As String) As String
        Dim l_Msg As String = ""
        Dim i, CharNo As Integer
        'Present any unprintable charcters in [0xHH] format
        For i = 1 To Len(p_Msg)
            CharNo = Asc(Mid(p_Msg, i, 1))
            If CharNo < 32 Or CharNo > 126 Then
                l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
            Else
                l_Msg = l_Msg & Mid(p_Msg, i, 1)
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

    Private Sub LogMsgFormatter(ByVal p_Test As String, ByVal p_Msg As String, ByVal p_NewLine As Boolean)
        Dim l_Msg As String = ""
        Try
            p_Test = Left(p_Test & Microsoft.VisualBasic.StrDup(30, " "), 25)

            l_Msg = Format(Now(), "HH:mm:ss.fff") & " " & p_Test & " " & MakePrintable(p_Msg)
            If Not g_LogFile Is Nothing Then
                If p_NewLine Then
                    g_LogFile.WriteLine(l_Msg) 'Update log file with newline terminator
                Else
                    g_LogFile.Write(l_Msg) 'Update log file without newline terminator
                End If
            End If
            g_LogFile.Flush()
        Catch ex As Exception
            MsgBox("LogMsgFormatter exception: " & Len(l_Msg) & " *" & l_Msg & "* " & ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub












#End Region

End Class
