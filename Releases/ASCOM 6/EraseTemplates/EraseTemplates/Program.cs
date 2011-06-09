using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace EraseTemplates
{
    class Program
    {
        static TraceLogger TL;
        static int ReturnCode = 0;

        static int Main(string[] args)
        {
            string VS2005TemplateDir = "";
            string VS2008TemplateDir = "";
            string VS2010TemplateDir = "";
            RegistryKey VSKey;

            try
            {
                TL = new TraceLogger("", "EraseTemplates"); // Create a tracelogger so we can log what happens
                TL.Enabled = true;

                LogMessage("EraseTemplates", "Removing old templates....");

                VSKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\8.0", false); //VS2005 path location
                if (VSKey != null)
                {
                    VS2005TemplateDir = VSKey.GetValue("UserProjectTemplatesLocation", "").ToString();
                }
                if (VS2005TemplateDir == "") // Check whether it has been installed into the default location but not run yet!
                {
                    LogMessage("EraseTemplates", "VS2005 template directory not found, searching folder: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2005\Templates\ProjectTemplates");
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2005\Templates\ProjectTemplates"))
                    {
                        VS2005TemplateDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2005\Templates\ProjectTemplates";
                        LogMessage("EraseTemplates", "VS2005 template directory found: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2005\Templates\ProjectTemplates");
                    }
                }

                VSKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\9.0", false); //VS2008 path location
                if (VSKey != null)
                {
                    VS2008TemplateDir = VSKey.GetValue("UserProjectTemplatesLocation", "").ToString();
                }
                if (VS2008TemplateDir == "") // Check whether it has been installed into the default location but not run yet!
                {
                    LogMessage("EraseTemplates", "VS2008 template directory not found, searching folder: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2008\Templates\ProjectTemplates");
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2008\Templates\ProjectTemplates"))
                    {
                        VS2008TemplateDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2008\Templates\ProjectTemplates";
                        LogMessage("EraseTemplates", "VS2008 template directory found: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2008\Templates\ProjectTemplates");
                    }
                }

                VSKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\10.0", false); //VS2010 path location
                if (VSKey != null)
                {
                    VS2010TemplateDir = VSKey.GetValue("UserProjectTemplatesLocation", "").ToString();
                }
                if (VS2010TemplateDir == "") // Check whether it has been installed into the default location but not run yet!
                {
                    LogMessage("EraseTemplates", "VS2010 template directory not found, searching folder: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\Templates\ProjectTemplates");
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\Templates\ProjectTemplates"))
                    {
                        VS2010TemplateDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\Templates\ProjectTemplates";
                        LogMessage("EraseTemplates", "VS2010 template directory found: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\Templates\ProjectTemplates");
                    }
                }

                EraseTemplates("Visual Studio 2005", VS2005TemplateDir);
                EraseTemplates("Visual Studio 2008", VS2008TemplateDir);
                EraseTemplates("Visual Studio 2010", VS2010TemplateDir);

            }
            catch (Exception ex)
            {
                LogError("EraseTemplates", ex.ToString());
                ReturnCode = 1;
            }

            TL.Enabled = false; // Clean up tracelogger
            TL.Dispose();
            TL = null;

            return ReturnCode;
        }
        //log messages and send to screen when appropriate
        public static void LogMessage(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            EventLogCode.LogEvent("EraseTemplates", logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.EraseTemplatesInfo, "");
        }

        //log error messages and send to screen when appropriate
        public static void LogError(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            EventLogCode.LogEvent("EraseTemplates", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.EraseTemplatesError, logMessage);
        }

        /// <summary>
        /// Removes templates within a particular version of Visual Studio
        /// </summary>
        /// <param name="Name">Descriptive name of the Visual Studio release</param>
        /// <param name="TemplateBasePath">Base path to its location in the HKCU</param>
        /// <returns></returns>
        static int EraseTemplates(string Name, string TemplateBasePath)
        {
            string Platform5VB = TemplateBasePath + @"\Visual Basic\"; // Set up expected paths
            string Platform5CSharp = TemplateBasePath + @"\Visual C#\";
            string Platform6VB = TemplateBasePath + @"\Visual Basic\ASCOM6\";
            string Platform6CSharp = TemplateBasePath + @"\Visual C#\ASCOM6\";

            if (TemplateBasePath == "")
            {
                LogMessage("EraseTemplates", Name + " is not installed");
            }
            else
            {
                LogMessage("EraseTemplates", Name + " VB Path: " + Platform5VB);
                LogMessage("EraseTemplates", Name + " C# Path: " + Platform5CSharp);

                FileDelete(Platform5CSharp, "ASCOM Camera Driver (C#).zip"); //Platform 5 C#
                FileDelete(Platform5CSharp, "ASCOM Dome Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM FilterWheel Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM Focuser Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM Rotator Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM Switch Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM Telescope Driver (C#).zip");
                FileDelete(Platform5CSharp, "ASCOM Local Server (singleton).zip");

                FileDelete(Platform5VB, "ASCOM Camera Driver (VB).zip"); // Platform 5 VB
                FileDelete(Platform5VB, "ASCOM Dome Driver (VB).zip");
                FileDelete(Platform5VB, "ASCOM FilterWheel Driver (VB).zip");
                FileDelete(Platform5VB, "ASCOM Focuser Driver (VB).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(2).zip"); //Remove copies introduced by a bad content file in Platform 5 Templates SP1
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(3).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(4).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(5).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(6).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(7).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(8).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(9).zip");
                FileDelete(Platform5VB, "ASCOM Rotator Driver (VB)(10).zip");
                FileDelete(Platform5VB, "ASCOM Switch Driver (VB).zip");
                FileDelete(Platform5VB, "ASCOM Telescope Driver (VB).zip");

                FileDelete(Platform6CSharp, "ASCOM Camera Driver Template CS.zip"); //Platform 6 C#
                FileDelete(Platform6CSharp, "ASCOM Dome Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM FilterWheel Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM Focuser Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM Rotator Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM SafetyMonitor Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM Switch Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM Telescope Driver Template CS.zip");
                FileDelete(Platform6CSharp, "ASCOM LocalServer Template CS.zip");

                FileDelete(Platform6VB, "ASCOM Camera Driver Template VB.zip"); // Platform 6 VB
                FileDelete(Platform6VB, "ASCOM Dome Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM FilterWheel Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM Focuser Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM Rotator Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM SafetyMonitor Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM Switch Driver Template VB.zip");
                FileDelete(Platform6VB, "ASCOM Telescope Driver Template VB.zip");

            }

            return 0;
        }

        /// <summary>
        /// Erases a file, if it exists, silently logging any exceptions
        /// </summary>
        /// <param name="DeletePath">Path to the file to delete</param>
        /// <param name="DeleteFile">Name of the file to delete</param>
        static void FileDelete(string DeletePath, string DeleteFile)
        {
            bool FileExists = File.Exists(DeletePath + DeleteFile);

            try
            {
                LogMessage("FileDelete", "  Deleting file: " + DeletePath + DeleteFile + " " + FileExists);
                if (FileExists) File.Delete(DeletePath + DeleteFile); // Only delete it if it exists!
            }
            catch (Exception ex)
            {
                LogError("FileDelete", "  Exception Deleting file: " + DeletePath + DeleteFile + " " + ex.ToString());
            }
        }

    }
}
