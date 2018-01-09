'Implements the TraceLogger component

Imports System.IO
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
<Guid("A088DB9B-E081-4339-996E-191EB9A80844"), _
ComVisible(True), _
ClassInterface(ClassInterfaceType.None)> _
Public Class TraceLogger
    Implements IDisposable

    Private g_LogFileName, g_LogFileType As String
    Private g_LogFile As System.IO.StreamWriter
    Private g_LineStarted As Boolean
    Private g_Enabled As Boolean
    Private g_LogFilePath As String
    Private g_LogFileActualName As String 'Full name of the log file being created (includes automatic file name)

    'Private mut As System.Threading.Mutex
    'Private GotMutex As Boolean

#Region "New and IDisposable Support"
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Creates a new TraceLogger instance
    ''' </summary>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of 
    ''' several logs contains the information of interest.
    ''' <para>This call enables automatic logging and sets the filetype to "Default".</para></remarks>
    Public Sub New()
        MyBase.New()
        g_LogFileName = "" 'Set automatic filenames as default
        g_LogFileType = "Default" '"Set an arbitary name inc case someone forgets to call SetTraceLog
        g_LogFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")
        'mut = New System.Threading.Mutex(False, "ForceRemoveMutex")
    End Sub

    ''' <summary>
    ''' Creates a new TraceLogger instance and initialises filename and type
    ''' </summary>
    ''' <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
    ''' <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    ''' <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
    Public Sub New(ByVal LogFileName As String, ByVal LogFileType As String)
        MyBase.New()
        g_LogFileName = LogFileName 'Save parameters to use when the first call to write a record is made
        g_LogFileType = LogFileType
        g_LogFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")
        'mut = New System.Threading.Mutex
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
                If Not (g_LogFile Is Nothing) Then
                    Try : g_LogFile.Flush() : Catch : End Try
                    Try : g_LogFile.Close() : Catch : End Try
                    Try : g_LogFile.Dispose() : Catch : End Try
                    g_LogFile = Nothing
                End If
                'If Not (mut Is Nothing) Then
                'Try : mut.ReleaseMutex() : Catch : End Try
                'Try : mut.Close() : Catch : End Try
                '   mut = Nothing
                '   End If
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

#Region "ITraceLogger Implementation"

    ''' <summary>
    ''' Insert a blank line into the log file
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BlankLine()
        LogMessage("", "", False)
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
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean)
        Dim Msg As String = Message
        Try
            ' mut.WaitOne()
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                If HexDump Then Msg = Message & "  (HEX" & MakeHex(Message) & ")"
                LogMsgFormatter(Identifier, Msg, True, False)
            End If

        Finally
            'mut.ReleaseMutex()
        End Try
    End Sub

    ''' <summary>
    ''' Displays a message respecting carriage return and linefeed characters
    ''' </summary>
    ''' <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    ''' <param name="Message">The final message to terminate the line</param>
    ''' <remarks>
    ''' <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    ''' Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    ''' </remarks>
    Public Sub LogMessageCrLf(ByVal Identifier As String, ByVal Message As String)
        Try
            'mut.WaitOne()
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                LogMsgFormatter(Identifier, Message, True, True)
            End If

        Finally
            'mut.ReleaseMutex()
        End Try

    End Sub

    ''' <summary>
    ''' Enables or disables logging to the file.
    ''' </summary>
    ''' <value>True to enable logging</value>
    ''' <returns>Boolean, current logging status (enabled/disabled).</returns>
    ''' <remarks>If this property is false then calls to LogMsg, LogStart, LogContinue and LogFinish do nothing. If True, 
    ''' supplied messages are written to the log file.</remarks>
    Public Property Enabled() As Boolean
        Get
            Return g_Enabled
        End Get
        Set(ByVal value As Boolean)
            g_Enabled = value
        End Set
    End Property

#End Region

#Region "ITraceLoggerExtra Implementation"

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
    <ComVisible(False)> _
    Public Overloads Sub LogMessage(ByVal Identifier As String, ByVal Message As String)
        Try
            'mut.WaitOne()
            If g_Enabled Then
                If g_LogFile Is Nothing Then Call CreateLogFile()
                LogMsgFormatter(Identifier, Message, True, False)
            End If
        Finally
            'mut.ReleaseMutex()
        End Try
    End Sub

#End Region

#Region "TraceLogger Support"
    Private Sub CreateLogFile()
        Dim FileNameSuffix As Integer = 0, ok As Boolean = False, FileNameBase As String
        Select Case g_LogFileName
            'Case "" 'Do nothing - no log required
            '    Throw New HelperException("TRACELOGGER.CREATELOGFILE - Call made but no log filename has been set")
            Case ""
                If g_LogFileType = "" Then MsgBox("TRACELOGGER.CREATELOGFILE - Call made but no log filetype has been set")
                My.Computer.FileSystem.CreateDirectory(g_LogFilePath) 'Create the directory if it doesn't exist
                FileNameBase = g_LogFilePath & "\ASCOM." & g_LogFileType & "." & Format(Now, "HHmm.ssfff")
                Do 'Create a unique log file name based on date, time and required name
                    g_LogFileActualName = FileNameBase & FileNameSuffix.ToString & ".txt"
                    FileNameSuffix += 1 'Increment counter that ensures that no logfile can have the same name as any other
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
                    If Not ok Then MsgBox("TraceLogger:CreateLogFile - Unable to create log file" & ex.ToString)
                End Try
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

    Private Function MakePrintable(ByVal p_Msg As String, ByVal p_RespectCrLf As Boolean) As String
        Dim l_Msg As String = ""
        Dim i, CharNo As Integer
        'Present any unprintable charcters in [0xHH] format
        For i = 1 To Len(p_Msg)
            CharNo = Asc(Mid(p_Msg, i, 1))
            Select Case CharNo
                Case 10, 13 ' Either translate or respect CRLf depending on the setting of the respect parameter
                    If p_RespectCrLf Then
                        l_Msg = l_Msg & Mid(p_Msg, i, 1)
                    Else
                        l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
                    End If
                Case 0 - 9, 11, 12, 14 - 31, Is > 126 ' All other non-printables should be translated
                    l_Msg = l_Msg & "[" & Right("00" & Hex(CharNo), 2) & "]"
                Case Else'Everything else is printable and should be left as is
                    l_Msg = l_Msg & Mid(p_Msg, i, 1)
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
        Dim l_Msg As String = ""
        Try
            p_Test = Left(p_Test & Microsoft.VisualBasic.StrDup(30, " "), 25)

            l_Msg = Format(Now(), "HH:mm:ss.fff") & " " & MakePrintable(p_Test, p_RespectCrLf) & " " & MakePrintable(p_Msg, p_RespectCrLf)
            If Not g_LogFile Is Nothing Then
                If p_NewLine Then
                    g_LogFile.WriteLine(l_Msg) 'Update log file with newline terminator
                Else
                    g_LogFile.Write(l_Msg) 'Update log file without newline terminator
                End If
                g_LogFile.Flush()
            End If
        Catch ex As Exception
            'MsgBox("LogMsgFormatter exception: " & Len(l_Msg) & " *" & l_Msg & "* " & ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region

End Class