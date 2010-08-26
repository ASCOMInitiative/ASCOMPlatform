using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace OptecLogging
{
    class OptecLogger
    {

        // NOTE: Log file location:
        // Users MyDocuments Folder\Optec Logs\Logs YYYY-MM-DD\ClassName.txt
        //        
        private static string LogFileName = "PyxisLE_Control_Log";
        private static StreamWriter myStreamWriter;
        private static string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Optec Logs\\";
        private bool verboseLogging = false;
        private bool basicLogging = true;

        public bool VerboseLogging
        {
            get { return verboseLogging; }
            set { verboseLogging = value; }
        }

        public bool BasicLogging
        {
          get { return basicLogging; }
          set { basicLogging = value; }
        }

        public static void LogMessageBasic(string msg)
        {
            try
            {
                OpenLogger();
                myStreamWriter.WriteLine(msg);
                myStreamWriter.Flush();
                CloseLogger();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        public static void LogMessageVerbose(string msg);

        public static string LogFileDirectory
        {
            get { return LogPath; }
        }

        private static void OpenLogger()
        {
            try
            {
                string datestring = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                datestring += "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string filename = LogPath + LogFileName + "_" + datestring + ".txt";

                // Verify that the directory exists. If not, create it.
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                // Verify that the file exists. If not, create it and open the streamwriter.
                if (!File.Exists(filename))
                {
                    myStreamWriter = File.CreateText(filename);
                }
                else myStreamWriter = new StreamWriter(filename, true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        private static void CloseLogger()
        {
            myStreamWriter.Close();
        }



        

        
    }
}
