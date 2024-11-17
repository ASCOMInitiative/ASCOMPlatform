using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// Creates a log file for a driver or application. Uses a similar file name and internal format to the serial logger. Multiple logs can be created simultaneously if needed.
    /// </summary>
    /// <remarks>
    /// <para>In automatic mode the file will be stored in an ASCOM folder within XP's My Documents folder or equivalent places 
    /// in other operating systems. Within the ASCOM folder will be a folder named Logs yyyy-mm-dd where yyyy, mm and dd are 
    /// today's year, month and day numbers. The trace file will appear within the day folder with the name 
    /// ASCOM.Identifier.hhmm.ssffff where hh, mm, ss and ffff are the current hour, minute, second and fraction of second 
    /// numbers at the time of file creation.
    /// </para> 
    /// <para>Within the file the format of each line is hh:mm:ss.fff Identifier Message where hh, mm, ss and fff are the hour, minute, second 
    /// and fractional second at the time that the message was logged, Identifier is the supplied identifier (usually the subroutine, 
    /// function, property or method from which the message is sent) and Message is the message to be logged.</para>
    /// </remarks>
    [Guid("A088DB9B-E081-4339-996E-191EB9A80844")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class TraceLogger : ITraceLogger, ITraceLoggerExtra, IDisposable
    {

        private const int IDENTIFIER_WIDTH_DEFAULT = 25; // Default width of the identifier field. The width can be changed through the TraceLogger.IdentifierWidth property

        private string g_LogFileName, g_LogFileType;
        private StreamWriter g_LogFile;
        private bool g_LineStarted;
        private bool g_Enabled;
        private string g_DefaultLogFilePath; // Variable to hold the default log file path determined by TraceLogger at run time
        private string g_LogFileActualName; // Full name of the log file being created (includes automatic file name)
        private string g_LogFilePath; // Variable to hold a user specified log file path
        private int g_IdentifierWidth; // Variable to hold the current identifier field width
        private bool autoLogFilePath;

        private System.Threading.Mutex mut;
        private bool GotMutex;

        #region New and IDisposable Support
        private bool traceLoggerHasBeenDisposed = false;        // To detect redundant calls

        /// <summary>
        /// Creates a new TraceLogger instance
        /// </summary>
        /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of 
        /// several logs contains the information of interest.
        /// <para>This call enables automatic logging and sets the file type to "Default".</para></remarks>
        public TraceLogger() : base()
        {

            g_IdentifierWidth = IDENTIFIER_WIDTH_DEFAULT;
            g_LogFileName = ""; // Set automatic filenames as default
            autoLogFilePath = true; // Set automatic filenames as default
            g_LogFileType = "Default"; // "Set an arbitrary name in case someone forgets to call SetTraceLog

            // Get the correct log file path depending on whether we are running as the "System" user that has no documents folder or a regular user who does
            if (string.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))) // We are running as the "System" user
            {
                g_DefaultLogFilePath = GetCommonProgramFilesx86() + TRACE_LOGGER_SYSTEM_PATH + TRACE_LOGGER_FILENAME_BASE + Strings.Format(DateTime.Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT);
            }
            else // We are running as a normal user
            {
                // Create a fallback folder name within the Documents folder: Documents\ASCOM
                string fallbackFolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + TRACE_LOGGER_PATH;

                // Get the user configured TraceLogger default folder name. Fall back to the Documents\ASCOM folder if no default folder has been set by the user
                string configuredFolderName = Global.GetString(TRACELOGGER_DEFAULT_FOLDER, fallbackFolderName);

                // Set the default folder name variable that is used with the TraceLogger application
                g_DefaultLogFilePath = configuredFolderName + TRACE_LOGGER_FILENAME_BASE + Strings.Format(DateTime.Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT);
            }

            g_LogFilePath = g_DefaultLogFilePath; // Initialise the log file path to the default value
            mut = new System.Threading.Mutex(false, "TraceLoggerMutex");

            // Set default behaviour for handling Unicode characters
            UnicodeEnabled = false;
        }

        /// <summary>
        /// Creates a new TraceLogger instance and initialises filename and type
        /// </summary>
        /// <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
        /// <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
        /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
        public TraceLogger(string LogFileName, string LogFileType) : this()
        {
            g_LogFileName = LogFileName; // Save parameters to use when the first call to write a record is made
            g_LogFileType = LogFileType;
        }

        /// <summary>
        /// Create and enable a new TraceLogger instance with automatic naming based on the supplied log file type
        /// </summary>
        /// <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
        /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
        public TraceLogger(string LogFileType) : this()
        {
            g_LogFileType = LogFileType;
            g_Enabled = true; // Enable the log
        }

        // IDisposable
        /// <summary>
        /// Disposes of the TraceLogger object
        /// </summary>
        /// <param name="disposing">True if being disposed by the application, False if disposed by the finaliser.</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!traceLoggerHasBeenDisposed)
            {
                traceLoggerHasBeenDisposed = true;
                if (disposing)
                {
                    if (g_LogFile is not null)
                    {
                        try
                        {
                            g_LogFile.Flush();
                        }
                        catch
                        {
                        }
                        try
                        {
                            g_LogFile.Close();
                        }
                        catch
                        {
                        }
                        try
                        {
                            g_LogFile.Dispose();
                        }
                        catch
                        {
                        }
                        g_LogFile = null;
                    }
                    if (mut is not null)
                    {
                        // Try : mut.ReleaseMutex() : Catch : End Try
                        try
                        {
                            mut.Close();
                        }
                        catch
                        {
                        }
                        mut = null;
                    }
                }
            }
        }
        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// Disposes of the TraceLogger object
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes the TraceLogger object
        /// </summary>
        /// <remarks></remarks>
        ~TraceLogger()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }

        #endregion

        #region ITraceLogger Implementation

        /// <summary>
        /// Logs an issue, closing any open line and opening a continuation line if necessary after the 
        /// issue message.
        /// </summary>
        /// <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
        /// <param name="Message">Message to log</param>
        /// <remarks>Use this for reporting issues that you don't want to appear on a line already opened 
        /// with StartLine</remarks>
        public void LogIssue(string Identifier, string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogIssue", "\"" + Identifier + "\", \"" + Message + "\"");
                if (g_Enabled)
                {
                    if (g_LogFile is null)
                        CreateLogFile();
                    if (g_LineStarted)
                        g_LogFile.WriteLine();
                    LogMsgFormatter(Identifier, Message, true, false);
                    if (g_LineStarted)
                        LogMsgFormatter("Continuation", "", false, false);
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }

        }

        /// <summary>
        /// Insert a blank line into the log file
        /// </summary>
        /// <remarks></remarks>
        public void BlankLine()
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            LogMessage("", "", false);
        }

        /// <summary>
        /// Logs a complete message in one call, including a hex translation of the message
        /// </summary>
        /// <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
        /// <param name="Message">Message to log</param>
        /// <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        /// <remarks>
        /// <para>Use this for straightforward logging requirements. Writes all information in one command.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// </remarks>
        public void LogMessage(string Identifier, string Message, bool HexDump)
        {
            string Msg = Message;

            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogMessage", "\"" + Identifier + "\", \"" + Message + "\", " + HexDump.ToString() + "\"");
                if (g_LineStarted)
                    LogFinish(" "); // 1/10/09 PWGS Silently close the open line

                if (g_Enabled)
                {
                    if (g_LogFile is null)
                        CreateLogFile();
                    if (HexDump)
                        Msg = Message + "  (HEX" + MakeHex(Message) + ")";
                    LogMsgFormatter(Identifier, Msg, true, false);
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        /// <summary>
        /// Displays a message respecting carriage return and linefeed characters
        /// </summary>
        /// <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
        /// <param name="Message">The final message to terminate the line</param>
        /// <remarks>
        /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// </remarks>
        public void LogMessageCrLf(string Identifier, string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogMessage", "\"" + Identifier + "\", \"" + Message + "\"");
                if (g_LineStarted)
                    LogFinish(" "); // 1/10/09 PWGS Silently close the open line

                if (g_Enabled)
                {
                    if (g_LogFile is null)
                        CreateLogFile();
                    LogMsgFormatter(Identifier, Message, true, true);
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }

        }

        /// <summary>
        /// Writes the time and identifier to the log, leaving the line ready for further content through LogContinue and LogFinish
        /// </summary>
        /// <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
        /// <param name="Message">Message to log</param>
        /// <remarks><para>Use this to start a log line where you want to write further information on the line at a later time.</para>
        /// <para>E.g. You might want to use this to record that an action has started and then append the word OK if all went well.
        /// You would then end up with just one line to record the whole transaction even though you didn't know that it would be 
        /// successful when you started. If you just used LogMsg you would have ended up with two log lines, one showing 
        /// the start of the transaction and the next the outcome.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// </remarks>
        public void LogStart(string Identifier, string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogStart", "\"" + Identifier + "\", \"" + Message + "\"");
                if (g_LineStarted)
                {
                    LogFinish("LOGISSUE: LogStart has been called before LogFinish. Parameters: " + Identifier + " " + Message);
                }
                else
                {
                    g_LineStarted = true;
                    if (g_Enabled)
                    {
                        if (g_LogFile is null)
                            CreateLogFile();
                        LogMsgFormatter(Identifier, Message, false, false);
                    }
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        /// <summary>
        /// Appends further message to a line started by LogStart, appends a hex translation of the message to the line, does not terminate the line.
        /// </summary>
        /// <param name="Message">The additional message to appear in the line</param>
        /// <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        /// <remarks>
        /// <para>This can be called multiple times to build up a complex log line if required.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// </remarks>
        public void LogContinue(string Message, bool HexDump)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            // Append a full hex dump of the supplied string if p_Hex is true
            string Msg = Message;
            if (HexDump)
                Msg = Message + "  (HEX" + MakeHex(Message) + ")";
            LogContinue(Msg);
        }

        /// <summary>
        /// Closes a line started by LogStart with the supplied message and a hex translation of the message
        /// </summary>
        /// <param name="Message">The final message to terminate the line</param>
        /// <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
        /// <remarks>
        /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// </remarks>
        public void LogFinish(string Message, bool HexDump)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            // Append a full hex dump of the supplied string if p_Hex is true
            string Msg = Message;
            if (HexDump)
                Msg = Message + "  (HEX" + MakeHex(Message) + ")";
            LogFinish(Msg);
        }

        /// <summary>
        /// Enables or disables logging to the file.
        /// </summary>
        /// <value>True to enable logging</value>
        /// <returns>Boolean, current logging status (enabled/disabled).</returns>
        /// <remarks>If this property is false then calls to LogMsg, LogStart, LogContinue and LogFinish do nothing. If True, 
        /// supplied messages are written to the log file.</remarks>
        public bool Enabled
        {
            get
            {
                return g_Enabled;
            }
            set
            {
                g_Enabled = value;
            }
        }

        /// <summary>
        /// Sets the log filename and type if the constructor is called without parameters
        /// </summary>
        /// <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
        /// <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
        /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the 
        /// information of interest.
        /// <para><b>Note </b>This command is only required if the trace logger constructor is called with no
        /// parameters. It is provided for use in COM clients that can not call constructors with parameters.
        /// If you are writing a COM client then create the trace logger as:</para>
        /// <code>
        /// TL = New TraceLogger()
        /// TL.SetLogFile("","TraceName")
        /// </code>
        /// <para>If you are writing a .NET client then you can achieve the same end in one call:</para>
        /// <code>
        /// TL = New TraceLogger("",TraceName")
        /// </code>
        /// </remarks>
        public void SetLogFile(string LogFileName, string LogFileType)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            g_LogFileName = LogFileName; // Save parameters to use when the first call to write a record is made
            g_LogFileType = LogFileType;
        }

        /// <summary>
        /// Return the full filename of the log file being created
        /// </summary>
        /// <value>Full filename of the log file</value>
        /// <returns>String filename</returns>
        /// <remarks>This call will return an empty string until the first line has been written to the log file
        /// as the file is not created until required.</remarks>
        public string LogFileName
        {
            get
            {
                return g_LogFileActualName;
            }
        }

        /// <summary>
        /// Set or return the path to a directory in which the log file will be created
        /// </summary>
        /// <returns>String path</returns>
        /// <remarks>Introduced with Platform 6.4.<para>If set, this path will be used instead of the user's Documents directory default path. This must be Set before the first message Is logged.</para></remarks>
        public string LogFilePath
        {
            get
            {
                return g_LogFilePath;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) // Use auto log file name
                {
                    autoLogFilePath = true;
                }
                else // Use provided log file path
                {
                    autoLogFilePath = false;
                }
                g_LogFilePath = value.TrimEnd(@"\".ToCharArray()); // Save the value and remove any trailing \ characters that will mess up file name creation later
            }
        }

        /// <summary>
        /// Set or return the width of the identifier field in the log message
        /// </summary>
        /// <value>Width of the identifier field</value>
        /// <returns>Integer width</returns>
        /// <remarks>Introduced with Platform 6.4.<para>If set, this width will be used instead of the default identifier field width.</para></remarks>
        public int IdentifierWidth
        {
            get
            {
                return g_IdentifierWidth;
            }
            set
            {
                g_IdentifierWidth = value;
            }
        }
        #endregion

        #region ITraceLoggerExtra Implementation

        /// <summary>
        /// Logs a complete message in one call
        /// </summary>
        /// <param name="Identifier">Identifies the meaning of the message e.g. name of module or method logging the message.</param>
        /// <param name="Message">Message to log</param>
        /// <remarks>
        /// <para>Use this for straightforward logging requirements. Writes all information in one command.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// <para>This overload is not available through COM, please use 
        /// "LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean)"
        /// with HexDump set False to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public void LogMessage(string Identifier, string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogMessage", "\"" + Identifier + "\", \"" + Message + "\"");
                if (g_LineStarted)
                    LogFinish(" "); // 1/10/09 PWGS Made line closure silent
                if (g_Enabled)
                {
                    if (g_LogFile is null)
                        CreateLogFile();
                    LogMsgFormatter(Identifier, Message, true, false);
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        /// <summary>
        /// Appends further message to a line started by LogStart, does not terminate the line.
        /// </summary>
        /// <param name="Message">The additional message to appear in the line</param>
        /// <remarks>
        /// <para>This can be called multiple times to build up a complex log line if required.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart. 
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// <para>This overload is not available through COM, please use 
        /// "LogContinue(ByVal Message As String, ByVal HexDump As Boolean)"
        /// with HexDump set False to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public void LogContinue(string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogContinue", "\"" + Message + "\"");
                if (!g_LineStarted)
                {
                    LogMessage("LOGISSUE", "LogContinue has been called before LogStart. Parameter: " + Message);
                }
                else if (g_Enabled)
                {
                    if (g_LogFile is null)
                        CreateLogFile();
                    g_LogFile.Write(MakePrintable(Message, false)); // Update log file without newline terminator
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        /// <summary>
        /// Closes a line started by LogStart with the supplied message
        /// </summary>
        /// <param name="Message">The final message to terminate the line</param>
        /// <remarks>
        /// <para>Can only be called once for each line started by LogStart.</para>
        /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
        /// Possible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
        /// <para>This overload is not available through COM, please use 
        /// "LogFinish(ByVal Message As String, ByVal HexDump As Boolean)"
        /// with HexDump set False to achieve this effect.</para>
        /// </remarks>
        [ComVisible(false)]
        public void LogFinish(string Message)
        {
            if (traceLoggerHasBeenDisposed)
                return; // Ignore this call if the trace logger has been disposed

            try
            {
                GetTraceLoggerMutex("LogFinish", "\"" + Message + "\"");
                if (!g_LineStarted)
                {
                    LogMessage("LOGISSUE", "LogFinish has been called before LogStart. Parameter: " + Message);
                }
                else
                {
                    g_LineStarted = false;
                    if (g_Enabled)
                    {
                        if (g_LogFile is null)
                            CreateLogFile();
                        g_LogFile.WriteLine(MakePrintable(Message, false)); // Update log file with newline terminator
                    }
                }
            }
            finally
            {
                mut.ReleaseMutex();
            }
        }

        /// <inheritdoc/>
        public bool UnicodeEnabled { get; set; }

        #endregion

        #region TraceLogger Support
        private void CreateLogFile()
        {
            int FileNameSuffix = 0;
            string FileNameBase, TodaysLogFilePath;
            switch (g_LogFileName ?? "")
            {
                case var @case when @case == "":

                // No filename has been specified so use the automatically generated name
                case SERIAL_AUTO_FILENAME:
                    if (string.IsNullOrEmpty(g_LogFileType))
                        throw new ValueNotSetException("TRACELOGGER.CREATELOGFILE - Call made but no log file type has been set");

                    if (autoLogFilePath) // Default behaviour using the current user's Document directory
                    {
                        Directory.CreateDirectory(g_DefaultLogFilePath); // Create the directory if it doesn't exist
                        FileNameBase = g_DefaultLogFilePath + @"\ASCOM." + g_LogFileType + "." + Strings.Format(DateTime.Now, "HHmm.ssfff");
                    }
                    else // User has given a specific path so use that
                    {
                        TodaysLogFilePath = g_LogFilePath + TRACE_LOGGER_FILENAME_BASE + Strings.Format(DateTime.Now, TRACE_LOGGER_FILE_NAME_DATE_FORMAT); // Append Logs yyyy-mm-dd to the user supplied log file
                        Directory.CreateDirectory(TodaysLogFilePath); // Create the directory if it doesn't exist
                        FileNameBase = TodaysLogFilePath + @"\ASCOM." + g_LogFileType + "." + Strings.Format(DateTime.Now, "HHmm.ssfff");
                    }

                    do // Create a unique log file name based on date, time and required name
                    {
                        g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                        FileNameSuffix += 1; // Increment counter that ensures that no log file can have the same name as any other
                    }
                    while (File.Exists(g_LogFileActualName));

                    try
                    {
                        g_LogFile = new StreamWriter(g_LogFileActualName, false);
                        g_LogFile.AutoFlush = true;
                    }
                    catch (IOException ex)
                    {
                        bool ok = false;
                        do
                        {
                            try
                            {
                                g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                                g_LogFile = new StreamWriter(g_LogFileActualName, false);
                                g_LogFile.AutoFlush = true;
                                ok = true;
                            }
                            catch (IOException)
                            {
                                // Ignore IO exceptions and try the next filename
                            }
                            FileNameSuffix += 1;
                        }
                        while (!(ok | FileNameSuffix == 20));

                        if (!ok)
                        {
                            Console.WriteLine($"TraceLogger:CreateLogFile - Unable to create log file: {ex}");
                            throw new HelperException("TraceLogger:CreateLogFile - IOException - Unable to create log file", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Interaction.MsgBox("CreateLogFile Auto filename exception - #" + g_LogFileName + "# " + ex.ToString());
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine($"TraceLogger:CreateLogFile - Auto filename exception trying to show message box: {ex1}");
                        }
                        Console.WriteLine($"TraceLogger:CreateLogFile - Auto filename exception creating trace logger: {ex}");
                        throw;

                    }
                    break;

                // The user has provided a specific filename so create log file based on supplied name
                default:
                    try
                    {
                        g_LogFile = new StreamWriter(g_LogFileName + ".txt", false);
                        g_LogFile.AutoFlush = true;
                        g_LogFileActualName = g_LogFileName + ".txt";
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Interaction.MsgBox("CreateLogFile Manual filename exception - #" + g_LogFileName + "# " + ex.ToString());
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine($"TraceLogger:CreateLogFile - Manual filename exception trying to show message box: {ex1}");
                        }
                        Console.WriteLine($"TraceLogger:CreateLogFile - Manual  filename exception creating trace logger: {ex}");
                        throw;
                    }
                    break;
            }
        }

        private string MakePrintable(string message, bool respectCrLf)
        {
            string formattedMessage = string.Empty;

            // Present any unprintable characters in [0xHH] format
            for (int i = 0; i < message.Length; i++)
            {
                int unicodeCodePoint = Strings.AscW(message.Substring(i, 1));
                switch (unicodeCodePoint)
                {
                    // Either translate or respect CRLf depending on the setting of the respect parameter
                    case 10:
                    case 13:
                        if (respectCrLf)
                            formattedMessage = formattedMessage + message.Substring(i, 1);
                        else
                            formattedMessage = formattedMessage + "[" + Strings.Right("00" + Conversion.Hex(unicodeCodePoint), 2) + "]";
                        break;

                    // All other non-printables should be converted to hex
                    case 0 - 9:
                    case 11:
                    case 12:
                    case 14 - 31:
                    case int @asd when (@asd > 126) & !UnicodeEnabled: // Higher code points when Unicode support is disabled
                        formattedMessage = formattedMessage + "[" + Strings.Right("00" + Conversion.Hex(unicodeCodePoint), 2) + "]";
                        break;

                    // Handle higher Unicode code points when Unicode is enabled as printable characters
                    case int @asd when (@asd > 126) & UnicodeEnabled:
                        formattedMessage = formattedMessage + message.Substring(i, 1);
                        break;

                    // Handle everything else, which should be printable
                    default:
                        formattedMessage = formattedMessage + message.Substring(i, 1);
                        break;
                }
            }

            return formattedMessage;
        }

        private string MakeHex(string p_Msg)
        {
            string l_Msg = "";
            int i, CharNo;
            // Present all characters in [0xHH] format
            var loopTo = Strings.Len(p_Msg);
            for (i = 1; i <= loopTo; i++)
            {
                CharNo = Strings.Asc(Strings.Mid(p_Msg, i, 1));
                l_Msg = l_Msg + "[" + Strings.Right("00" + Conversion.Hex(CharNo), 2) + "]";
            }
            return l_Msg;
        }

        private void LogMsgFormatter(string p_Test, string p_Msg, bool p_NewLine, bool p_RespectCrLf)
        {
            try
            {
                p_Test = Strings.Left(p_Test + Strings.StrDup(g_IdentifierWidth, " "), g_IdentifierWidth);

                string l_Msg = Strings.Format(DateTime.Now, "HH:mm:ss.fff") + " " + MakePrintable(p_Test, p_RespectCrLf) + " " + MakePrintable(p_Msg, p_RespectCrLf);
                if (g_LogFile is not null)
                {
                    if (p_NewLine)
                    {
                        g_LogFile.WriteLine(l_Msg); // Update log file with newline terminator
                    }
                    else
                    {
                        g_LogFile.Write(l_Msg);
                    } // Update log file without newline terminator
                    g_LogFile.Flush();
                }
            }
            catch (Exception ex)
            {
                LogEvent("LogMsgFormatter", "Exception", EventLogEntryType.Error, EventLogErrors.TraceLoggerException, $"Test: \"{p_Test}\", Message: \"{p_Msg}\"{Environment.NewLine}TraceLogger has been Disposed: {traceLoggerHasBeenDisposed}{Environment.NewLine}Entry assembly: {Assembly.GetEntryAssembly()}{Environment.NewLine}Calling assembly: {Assembly.GetCallingAssembly()}{Environment.NewLine}{ex}");
                // MsgBox("LogMsgFormatter exception: " & Len(l_Msg) & " *" & l_Msg & "* " & ex.ToString, MsgBoxStyle.Critical)
            }
        }

        private void GetTraceLoggerMutex(string Method, string Parameters)
        {
            // Get the profile mutex or log an error and throw an exception that will terminate this profile call and return to the calling application
            try
            {
                // Try to acquire the mutex
                GotMutex = mut.WaitOne(PROFILE_MUTEX_TIMEOUT, false);
            }
            // Catch the AbandonedMutexException but not any others, these are passed to the calling routine
            catch (System.Threading.AbandonedMutexException ex)
            {
                // We've received this exception but it indicates an issue in a PREVIOUS thread not this one. Log it and we have also got the mutex; so continue!
                LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, EventLogErrors.TraceLoggerMutexAbandoned, ex.ToString());
                if (GetBool(ABANDONED_MUTEXT_TRACE, ABANDONED_MUTEX_TRACE_DEFAULT))
                {
                    LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ": Throwing exception to application", EventLogEntryType.Warning, EventLogErrors.TraceLoggerMutexAbandoned, null);
                    throw; // Throw the exception in order to report it
                }
                else
                {
                    LogEvent("TraceLogger", "AbandonedMutexException in " + Method + ": Absorbing exception, continuing normal execution", EventLogEntryType.Warning, EventLogErrors.TraceLoggerMutexAbandoned, null);
                    GotMutex = true;
                } // Flag that we have got the mutex.
            }

            // Check whether we have the mutex, throw an error if not
            if (!GotMutex)
            {
                LogEvent(Method, "Timed out waiting for TraceLogger mutex in " + Method + ", parameters: " + Parameters, EventLogEntryType.Error, EventLogErrors.TraceLoggerMutexTimeOut, null);
                throw new ProfilePersistenceException("Timed out waiting for TraceLogger mutex in " + Method + ", parameters: " + Parameters);
            }
        }

        #endregion

    }
}