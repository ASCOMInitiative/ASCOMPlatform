using Microsoft.Win32;
using System;
using System.IO;

// NOTE - This cut down TraceLogger source code is included in each of the Platform installer support utilities to ensure that
// they are self-contained with no external ASCOM dependencies. If the normal Utilities DLL was included as Platform content,
// any Utilities assembly in the GAC would be loaded instead of the intended version in the Platform content. This will
// result in the previous Platform's TraceLogger code being run instead of the intended version included as installer content.

namespace Utilities
{
    /// <summary>
    /// Creates a log file for a driver or application. Uses a similar file name and internal format to the serial logger. Multiple logs can be created simultaneously if needed.
    /// </summary>
    /// <remarks>
    ///<para>In automatic mode the file will be stored in an ASCOM folder within XP's My Documents folder or equivalent places 
    /// in other operating systems. Within the ASCOM folder will be a folder named Logs yyyy-mm-dd where yyyy, mm and dd are 
    /// today's year, month and day numbers. The trace file will appear within the day folder with the name 
    /// ASCOM.Identifier.hhmm.ssffff where hh, mm, ss and ffff are the current hour, minute, second and fraction of second 
    /// numbers at the time of file creation.
    /// </para> 
    /// <para>Within the file the format of each line is hh:mm:ss.fff Identifier Message where hh, mm, ss and fff are the hour, minute, second 
    /// and fractional second at the time that the message was logged, Identifier is the supplied identifier (usually the subroutine, 
    /// function, property or method from which the message is sent) and Message is the message to be logged.</para>
    ///</remarks>
    public class TraceLogger : IDisposable
    {

        private string logFileType;
        private StreamWriter logFileStream;
        private bool enabled;
        private string logFilePath;

        //Full name of the log file being created (includes automatic file name)
        private string logFileActualName;

        // TraceLogger - Per user configuration value names
        private const string TRACELOGGER_DEFAULT_FOLDER = "TraceLogger Default Folder";
        private const string REGISTRY_UTILITIES_FOLDER = @"Software\ASCOM\Utilities";
        private const string TRACE_LOGGER_PATH = @"\ASCOM"; // Path to TraceLogger directory from My Documents
        private const string TRACE_LOGGER_FILENAME_BASE = @"\Logs "; // Fixed part of TraceLogger file name.  Note: The trailing space must be retained!
        private const string TRACE_LOGGER_FILE_NAME_DATE_FORMAT = "yyyy-MM-dd";

        #region New and IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Creates a new TraceLogger instance and initialises filename and type
        /// </summary>
        /// <param name="logFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
        /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
        public TraceLogger(string logFileType) : base()
        {
            //Save parameters to use when the first call to write a record is made
            this.logFileType = logFileType;

            string defaultFolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + TRACE_LOGGER_PATH;
            string folderName = GetString(TRACELOGGER_DEFAULT_FOLDER, defaultFolderName);

            // Initialise the log file path to the default value
            logFilePath = folderName + TRACE_LOGGER_FILENAME_BASE + DateTime.Now.ToString(TRACE_LOGGER_FILE_NAME_DATE_FORMAT);
        }

        /// <summary>
        /// Disposes of the TraceLogger object
        /// </summary>
        /// <param name="disposing">True if being disposed by the application, False if disposed by the finalizer.</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if ((logFileStream != null))
                    {
                        try { logFileStream.Flush(); } catch { }
                        try { logFileStream.Close(); } catch { }
                        try { logFileStream.Dispose(); } catch { }
                        logFileStream = null;
                    }
                }
            }
            this.disposedValue = true;
        }

        /// <summary>
        /// Disposes of the TraceLogger object
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region TraceLogger Implementation

        /// <summary>
        /// Insert a blank line into the log file
        /// </summary>
        /// <remarks></remarks>
        public void BlankLine()
        {
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
            if (enabled)
            {
                if (logFileStream == null)
                    CreateLogFile();
                if (HexDump)
                    Msg = Message + "  (HEX" + MakeHex(Message) + ")";
                LogMsgFormatter(Identifier, Msg, true, false);
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
            //mut.WaitOne()
            if (enabled)
            {
                if (logFileStream == null)
                    CreateLogFile();
                LogMsgFormatter(Identifier, Message, true, true);
            }
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
            get { return enabled; }
            set { enabled = value; }
        }

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
        public void LogMessage(string Identifier, string Message)
        {
            if (enabled)
            {
                if (logFileStream == null)
                    CreateLogFile();
                LogMsgFormatter(Identifier, Message, true, false);
            }
        }

        #endregion

        #region TraceLogger Support

        private void CreateLogFile()
        {
            int fileNameSuffix = 0;
            //Create the directory if it doesn't exist
            Directory.CreateDirectory(logFilePath);

            //Create a unique log file name based on date, time and required name
            string fileNameBase = logFilePath + TRACE_LOGGER_PATH + "." + logFileType + "." + DateTime.Now.ToString("HHmm.ssfff");

            do
            {
                logFileActualName = fileNameBase + fileNameSuffix.ToString() + ".txt";

                //Increment counter that ensures that no log file can have the same name as any other
                fileNameSuffix += 1;
            } while (File.Exists(logFileActualName));

            try
            {
                logFileStream = new StreamWriter(logFileActualName, false)
                {
                    AutoFlush = true
                };
            }
            catch (IOException ex)
            {
                bool ok = false;
                do
                {
                    try
                    {
                        logFileActualName = fileNameBase + fileNameSuffix.ToString() + ".txt";
                        logFileStream = new StreamWriter(logFileActualName, false)
                        {
                            AutoFlush = true
                        };
                        ok = true;
                    }
                    catch (IOException ex1)
                    {
                        //Ignore IO exceptions and try the next filename
                        ex1.ToString();
                    }
                    fileNameSuffix += 1;
                } while (!((ok | (fileNameSuffix == 20))));
                if (!ok) throw new ApplicationException("TraceLogger:CreateLogFile - Unable to create log file" + ex.ToString());
            }
        }

        private string MakePrintable(string message, bool respectCrLf)
        {
            string l_Msg = "";
            int i;
            //Present any unprintable characters in [0xHH] format
            for (i = 0; i < message.Length; i++)
            {
                string temp;
                int CharNo = message.Substring(i, 1).ToCharArray()[0];
                switch (CharNo)
                {
                    case 10:
                    case 13:
                        // Either translate or respect CRLf depending on the setting of the respect parameter
                        if (respectCrLf)
                        {
                            l_Msg = l_Msg + message.Substring(i, 1);
                        }
                        else
                        {
                            temp = "00" + CharNo.ToString("0X");
                            l_Msg = l_Msg + "[" + temp.Substring(temp.Length - 3, 2) + "]";
                        }
                        break;
                    case 0 - 9:
                    case 11:
                    case 12:
                    case 14 - 31:
                    case 126: // ERROR: Case labels with binary operators are unsupported : GreaterThan
                              // All other non-printables should be translated
                        temp = "00" + CharNo.ToString("0X");
                        l_Msg = l_Msg + "[" + temp.Substring(temp.Length - 3, 2) + "]";
                        break;
                    default:
                        //Everything else is printable and should be left as is
                        l_Msg = l_Msg + message.Substring(i, 1);
                        break;
                }
                if (CharNo < 32 | CharNo > 126)
                {
                }
                else
                {
                }
            }
            return l_Msg;
        }

        private string MakeHex(string message)
        {
            string l_Msg = "";
            int i;
            //Present all characters in [0xHH] format
            for (i = 0; i < message.Length; i++)
            {
                int CharNo = message.Substring(i, 1).ToCharArray()[0];
                string temp = "00" + CharNo.ToString("0X");
                l_Msg = l_Msg + "[" + temp.Substring(temp.Length - 3, 2) + "]";
            }
            return l_Msg;
        }

        private void LogMsgFormatter(string test, string message, bool newLine, bool respectCrLf)
        {
            if (test is null)
                test = "";

            if (message is null)
                message = "";

            try
            {
                test = (test + "                              ").Substring(0, 25);

                string l_Msg = DateTime.Now.ToString("HH:mm:ss.fff") + " " + MakePrintable(test, respectCrLf) + " " + MakePrintable(message, respectCrLf);
                if ((logFileStream != null))
                {
                    if (newLine)
                    {
                        //Update log file with newline terminator
                        logFileStream.WriteLine(l_Msg);
                    }
                    else
                    {
                        //Update log file without newline terminator
                        logFileStream.Write(l_Msg);
                    }
                    logFileStream.Flush();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }

        internal static string GetString(string name, string defaultValue)
        {
            string returnValue = "";
            RegistryKey hKeyCurrentUser, settingsKey;

            hKeyCurrentUser = Registry.CurrentUser;
            hKeyCurrentUser.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            settingsKey = hKeyCurrentUser.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            try
            {
                if (settingsKey.GetValueKind(name) == RegistryValueKind.String) // Value does exist
                {
                    returnValue = settingsKey.GetValue(name).ToString();
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(name, defaultValue.ToString());
                    returnValue = defaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    returnValue = defaultValue;
                }
            }
            catch (Exception)
            {
                returnValue = defaultValue;
            }

            // Clean up registry keys
            settingsKey.Flush();
            settingsKey.Close();
            hKeyCurrentUser.Flush();
            hKeyCurrentUser.Close();

            return returnValue;
        }

        internal static void SetName(string name, string value)
        {
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            m_SettingsKey.SetValue(name, value.ToString(), RegistryValueKind.String);
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();

            m_HKCU.Flush();
            m_HKCU.Close();
        }

        #endregion

    }
}