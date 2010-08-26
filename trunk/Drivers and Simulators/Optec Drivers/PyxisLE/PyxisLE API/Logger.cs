using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OptecHIDTools;

namespace PyxisLE_API
{
    public class Logger : OptecHIDTools.OptecLogger
    {
        //public static void LogMessage(string assemblyName, string className, string msg, bool msgIsVerbose)
        //{

        //}

        //private static string LogFileName = "PyxisLE_API_Log";
        //private static StreamWriter myStreamWriter;
        //private static string LogPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Optec Logs\\";

        //public static void LogMessage(string msg)
        //{
        //    try
        //    {
        //        OpenLog();
        //        myStreamWriter.WriteLine(msg);
        //        myStreamWriter.Flush();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Log File Error. " + ex.Message);
        //    }
        //}

        //public static void LogException(Exception ex)
        //{
        //    //OptecLogger.
        //}

        //private static void OpenLog()
        //{
        //    if(myStreamWriter == null)
        //    {
        //        string datestring = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        //        datestring += "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //        string filename = LogPath + LogFileName + "_" + datestring + ".txt";

        //        if (!Directory.Exists(LogPath))
        //        {
        //            Directory.CreateDirectory(LogPath);
        //        }

        //        if(!File.Exists(filename))
        //        {
        //           myStreamWriter =  File.CreateText(filename);
        //        }

        //        else myStreamWriter = new StreamWriter(filename, true);
        //    }

        //}
    }
}
