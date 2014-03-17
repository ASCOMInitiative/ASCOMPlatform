using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.IO;

//Implements the TraceLogger component

using System.Runtime.InteropServices;

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

    private string g_LogFileName;
    private string g_LogFileType;
    private System.IO.StreamWriter g_LogFile;
    private bool g_Enabled;
    private string g_LogFilePath;
    //Full name of the log file being created (includes automatic file name)
    private string g_LogFileActualName;

    //Private mut As System.Threading.Mutex
    //Private GotMutex As Boolean

    #region "New and IDisposable Support"
    // To detect redundant calls
    private bool disposedValue = false;

    /// <summary>
    /// Creates a new TraceLogger instance
    /// </summary>
    /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of 
    /// several logs contains the information of interest.
    /// <para>This call enables automatic logging and sets the filetype to "Default".</para></remarks>
    public TraceLogger()
        : base()
    {
        g_LogFileName = "";
        //Set automatic filenames as default
        g_LogFileType = "Default";
        //"Set an arbitary name inc case someone forgets to call SetTraceLog
        g_LogFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\Logs " + DateTime.Now.ToString("yyyy-MM-dd");
        //mut = New System.Threading.Mutex(False, "ForceRemoveMutex")
    }

    /// <summary>
    /// Creates a new TraceLogger instance and initialises filename and type
    /// </summary>
    /// <param name="LogFileName">Fully qualified trace file name or null string to use automatic file naming (recommended)</param>
    /// <param name="LogFileType">String identifying the type of log e,g, Focuser, LX200, GEMINI, MoonLite, G11</param>
    /// <remarks>The LogFileType is used in the file name to allow you to quickly identify which of several logs contains the information of interest.</remarks>
    public TraceLogger(string LogFileName, string LogFileType)
        : base()
    {
        g_LogFileName = LogFileName;
        //Save parameters to use when the first call to write a record is made
        g_LogFileType = LogFileType;
        g_LogFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ASCOM\\Logs " + DateTime.Now.ToString( "yyyy-MM-dd");
        //mut = New System.Threading.Mutex
    }

    // IDisposable
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
                if ((g_LogFile != null))
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
                //If Not (mut Is Nothing) Then
                //Try : mut.ReleaseMutex() : Catch : End Try
                //Try : mut.Close() : Catch : End Try
                //   mut = Nothing
                //   End If
            }
        }
        this.disposedValue = true;
    }
    // This code added by Visual Basic to correctly implement the disposable pattern.
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

    #region "ITraceLogger Implementation"

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
    /// <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    /// <param name="Message">Message to log</param>
    /// <param name="HexDump">True to append a hex translation of the message at the end of the message</param>
    /// <remarks>
    /// <para>Use this for straightforward logging requrements. Writes all information in one command.</para>
    /// <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    /// Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    /// </remarks>
    public void LogMessage(string Identifier, string Message, bool HexDump)
    {
        string Msg = Message;
        try
        {
            // mut.WaitOne()
            if (g_Enabled)
            {
                if (g_LogFile == null)
                    CreateLogFile();
                if (HexDump)
                    Msg = Message + "  (HEX" + MakeHex(Message) + ")";
                LogMsgFormatter(Identifier, Msg, true, false);
            }

        }
        finally
        {
            //mut.ReleaseMutex()
        }
    }

    /// <summary>
    /// Displays a message respecting carriage return and linefeed characters
    /// </summary>
    /// <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    /// <param name="Message">The final message to terminate the line</param>
    /// <remarks>
    /// <para>Will create a LOGISSUE message in the log if called before a line has been started with LogStart.  
    /// Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    /// </remarks>
    public void LogMessageCrLf(string Identifier, string Message)
    {
        try
        {
            //mut.WaitOne()
            if (g_Enabled)
            {
                if (g_LogFile == null)
                    CreateLogFile();
                LogMsgFormatter(Identifier, Message, true, true);
            }

        }
        finally
        {
            //mut.ReleaseMutex()
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
        get { return g_Enabled; }
        set { g_Enabled = value; }
    }

    #endregion

    #region "ITraceLoggerExtra Implementation"

    /// <summary>
    /// Logs a complete message in one call
    /// </summary>
    /// <param name="Identifier">Identifies the meaning of the the message e.g. name of modeule or method logging the message.</param>
    /// <param name="Message">Message to log</param>
    /// <remarks>
    /// <para>Use this for straightforward logging requrements. Writes all information in one command.</para>
    /// <para>Will create a LOGISSUE message in the log if called before a line started by LogStart has been closed with LogFinish. 
    /// Posible reasons for this are exceptions causing the normal flow of code to be bypassed or logic errors.</para>
    /// <para>This overload is not available through COM, please use 
    /// "LogMessage(ByVal Identifier As String, ByVal Message As String, ByVal HexDump As Boolean)"
    /// with HexDump set False to achieve this effect.</para>
    /// </remarks>
    [ComVisible(false)]
    public void LogMessage(string Identifier, string Message)
    {
        try
        {
            //mut.WaitOne()
            if (g_Enabled)
            {
                if (g_LogFile == null)
                    CreateLogFile();
                LogMsgFormatter(Identifier, Message, true, false);
            }
        }
        finally
        {
            //mut.ReleaseMutex()
        }
    }

    #endregion

    #region "TraceLogger Support"
    private void CreateLogFile()
    {
        int FileNameSuffix = 0;
        bool ok = false;
        string FileNameBase = null;
        switch (g_LogFileName)
        {
            //Case "" 'Do nothing - no log required
            //    Throw New HelperException("TRACELOGGER.CREATELOGFILE - Call made but no log filename has been set")
            case "":
                Directory.CreateDirectory(g_LogFilePath);
                //Create the directory if it doesn't exist
                FileNameBase = g_LogFilePath + "\\ASCOM." + g_LogFileType + "." + DateTime.Now.ToString("HHmm.ssfff");
                //Create a unique log file name based on date, time and required name
                do
                {
                    g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                    FileNameSuffix += 1;
                    //Increment counter that ensures that no logfile can have the same name as any other
                } while (!((!File.Exists(g_LogFileActualName))));

                try
                {
                    g_LogFile = new StreamWriter(g_LogFileActualName, false);
                    g_LogFile.AutoFlush = true;
                }
                catch (System.IO.IOException ex)
                {
                    ok = false;
                    do
                    {
                        try
                        {
                            g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                            g_LogFile = new StreamWriter(g_LogFileActualName, false);
                            g_LogFile.AutoFlush = true;
                            ok = true;
                        }
                        catch (System.IO.IOException ex1)
                        {
                            ex1.ToString();
                            //Ignore IO exceptions and try the next filename
                        }
                        FileNameSuffix += 1;
                    } while (!((ok | (FileNameSuffix == 20))));
                    if (!ok) throw new ApplicationException("TraceLogger:CreateLogFile - Unable to create log file" + ex.ToString());
                }
                break;
            default:
                //Create log file based on supplied name
                try
                {
                    g_LogFile = new StreamWriter(g_LogFileName + ".txt", false);
                    g_LogFile.AutoFlush = true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    throw;
                }
                break;
        }
    }

    private string MakePrintable(string p_Msg, bool p_RespectCrLf)
    {
        string l_Msg = "";
        int i = 0;
        int CharNo = 0;
        //Present any unprintable charcters in [0xHH] format
        for (i = 0; i < p_Msg.Length; i++)
        {
            string temp;
            CharNo = p_Msg.Substring( i, 1).ToCharArray()[0];
            switch (CharNo)
            {
                case 10:
                case 13:
                    // Either translate or respect CRLf depending on the setting of the respect parameter
                    if (p_RespectCrLf)
                    {
                        l_Msg = l_Msg + p_Msg.Substring( i, 1);
                    }
                    else
                    {
                        temp = "00" + CharNo.ToString("0X");
                        l_Msg = l_Msg + "[" + temp.Substring(temp.Length-3, 2) + "]";
                    }
                    break;
                case 0 - 9:
                case 11:
                case 12:
                case 14 - 31:
                case  126: // ERROR: Case labels with binary operators are unsupported : GreaterThan
                    // All other non-printables should be translated
                        temp = "00" + CharNo.ToString("0X");
                    l_Msg = l_Msg + "[" + temp.Substring(temp.Length-3, 2) + "]";
                    break;
                default:
                    //Everything else is printable and should be left as is
                    l_Msg = l_Msg + p_Msg.Substring( i, 1);
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

    private string MakeHex(string p_Msg)
    {
        string l_Msg = "";
        int i = 0;
        int CharNo = 0;
        //Present all characters in [0xHH] format
        for (i = 0; i < p_Msg.Length; i++)
        {
            CharNo = p_Msg.Substring( i, 1).ToCharArray()[0];
            string temp = "00" + CharNo.ToString("0X");
            l_Msg = l_Msg + "[" + temp.Substring(temp.Length-3, 2) + "]";
        }
        return l_Msg;
    }

    private void LogMsgFormatter(string p_Test, string p_Msg, bool p_NewLine, bool p_RespectCrLf)
    {
        string l_Msg = "";
        try
        {
            p_Test = (p_Test + "                              ").Substring(0, 25);

            l_Msg = DateTime.Now.ToString("HH:mm:ss.fff") + " " + MakePrintable(p_Test, p_RespectCrLf) + " " + MakePrintable(p_Msg, p_RespectCrLf);
            if ((g_LogFile != null))
            {
                if (p_NewLine)
                {
                    g_LogFile.WriteLine(l_Msg);
                    //Update log file with newline terminator
                }
                else
                {
                    g_LogFile.Write(l_Msg);
                    //Update log file without newline terminator
                }
                g_LogFile.Flush();
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
            throw;
            //MsgBox("LogMsgFormatter exception: " & Len(l_Msg) & " *" & l_Msg & "* " & ex.ToString, MsgBoxStyle.Critical)
        }
    }

    #endregion

}