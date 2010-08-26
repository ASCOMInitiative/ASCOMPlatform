using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace OptecHIDTools
{
    class OptecLogger
    {
        private static StreamWriter myStreamWriter;
        private static string MyDocsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static object LoggerLock = new object();

        private static bool verboseLoggingEnabled = true;
        private static bool basicLoggingEnabled = true;

        public static bool BasicLoggingEnabled
        {
            get { return OptecLogger.basicLoggingEnabled; }
            set { OptecLogger.basicLoggingEnabled = value; }
        }

        public static bool VerboseLoggingEnabled
        {
            get { return OptecLogger.verboseLoggingEnabled; }
            set 
            { 
                OptecLogger.verboseLoggingEnabled = value;
                // If verbose is enabled, enable basic as well.
                if (value) OptecLogger.BasicLoggingEnabled = value;
            }
        }
    
        public static void LogMessage(string assemblyName, string className, string msg, bool messageIsVerbose)
        {
            try
            {
                if (messageIsVerbose)
                {
                    if (!VerboseLoggingEnabled) return;
                }
                else
                {
                    if (!BasicLoggingEnabled) return;
                }

                lock (LoggerLock)
                {
                    OpenLog();

                    string logstring =
                        GetTimeString() + ((char)System.Windows.Forms.Keys.Tab).ToString() +
                        assemblyName + ((char)System.Windows.Forms.Keys.Tab).ToString() +
                        className + ((char)System.Windows.Forms.Keys.Tab).ToString() +
                        msg;
                    myStreamWriter.WriteLine(logstring);
                    Trace.WriteLine(logstring);
                    myStreamWriter.Flush();
                    CloseLog(); 
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        public static void LogException(Exception ex)
        {
            //EXCEPTIONS ARE ALWAYS LOGGED! 
            try
            {
                lock (LoggerLock)
                {
                    OpenLog();
                    string logstring =
                        GetTimeString() + ((char)System.Windows.Forms.Keys.Tab).ToString() +
                        ex.ToString();
                    myStreamWriter.WriteLine(logstring);
                    Trace.WriteLine(logstring);
                    myStreamWriter.Flush();
                    CloseLog(); 
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        private static void OpenLog()
        {
            // Log File Location: Users MyDocs Folder\Optec Logs\Logs YYYY-MM-DD\Hour xx.txt

            string LogPath = MyDocsFolder + "\\Optec Logs\\" + "Logs " + GetDateString();
            string filename = LogPath + "\\Hour " + DateTime.Now.Hour.ToString("00") + ".txt";
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if(!File.Exists(filename))
            {
                myStreamWriter =  File.CreateText(filename);
            }

            else myStreamWriter = new StreamWriter(filename, true);
        }

        private static void CloseLog()
        {
            myStreamWriter.Close();
        }

        private static string GetDateString()
        {
            // Returns the current time in format "YYYY-MM-DD"
            string d =
                DateTime.Now.Year.ToString("0000") + "-" +
                DateTime.Now.Month.ToString("00") + "-" +
                DateTime.Now.Date.ToString("00");
            return d;
        }

        private static string GetTimeString()
        {
            // Returns the current time in format "HH-MM-SS"
            string t =
                DateTime.Now.Hour.ToString("00") + ":" +
                DateTime.Now.Minute.ToString("00") + ":" +
                DateTime.Now.Second.ToString("00");
            return t;
        }
    }
}
