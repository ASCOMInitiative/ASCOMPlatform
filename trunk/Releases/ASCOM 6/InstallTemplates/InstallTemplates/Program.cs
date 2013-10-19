using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace InstallTemplates
{
    class Program
    {
        static TraceLogger TL;
        static int ReturnCode = 0;

        static int Main(string[] args)
        {

            string TemplateSourceDirectory = "";
            Dictionary<string, string> vsTemplateDirectoryList = new Dictionary<string, string>();

            try
            {
                TL = new TraceLogger("", "InstallTemplates"); // Create a tracelogger so we can log what happens
                TL.Enabled = true;

                LogMessage("InstallTemplates", "installing new templates....");

                TemplateSourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\ASCOM\Platform 6 Developer Components\Templates";
                LogMessage("InstallTemplates", "Template Source Directory: " + TemplateSourceDirectory);

                // Search registry for template directories
                vsTemplateDirectoryList = AddTemplateDirectories(vsTemplateDirectoryList, FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\VisualStudio"));
                vsTemplateDirectoryList = AddTemplateDirectories(vsTemplateDirectoryList, FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\WDExpress"));

                // Search file system for template directories
                vsTemplateDirectoryList = AddTemplateDirectories(vsTemplateDirectoryList, FindTemplateDirectoriesInFileSystem(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio*"));

                LogMessage("Main", " ");
                foreach (KeyValuePair<string, string> templateDir in vsTemplateDirectoryList)
                {
                    LogMessage("Main", "Installing templates in directory: " + templateDir.Key.ToString());
                    InstallTemplates(templateDir.Key.ToString(), TemplateSourceDirectory);
                }

            }
            catch (Exception ex)
            {
                LogError("InstallTemplates", ex.ToString());
                ReturnCode = 1;
            }

            TL.Enabled = false; // Clean up tracelogger
            TL.Dispose();
            TL = null;

            return ReturnCode;
        }

        static private Dictionary<string, string> AddTemplateDirectories(Dictionary<string, string> overallList, List<string> additionList)
        {
            foreach (string templateDirectory in additionList)
            {
                if (!overallList.ContainsKey(templateDirectory)) overallList.Add(templateDirectory, "");
            }

            return overallList;
        }

        static private List<string> FindTemplateDirectoriesInFileSystem(string searchDir, string searchPattern)
        {
            List<string> foundTempateDirectories = new List<string>();
            string[] vsDirectories = new string[0];
            const string projectTemplateDirectory = @"\Templates\ProjectTemplates";
            LogMessage("FindInFileSystem", "Searching directory: " + searchDir + " for: " + searchPattern);

            try
            {
                vsDirectories = Directory.GetDirectories(searchDir, searchPattern);
            }
            catch { }

            if (vsDirectories.Length > 0)
            {

                foreach (string vsDirectory in vsDirectories)
                {
                    //LogMessage("FindTemplateDirectoriesInFileSystem", "Found: " + vsDirectory);

                    if (Directory.Exists(vsDirectory + projectTemplateDirectory))
                    {
                        LogMessage("FindInFileSystem", "    Found Template Directory: " + vsDirectory + projectTemplateDirectory);
                        foundTempateDirectories.Add(vsDirectory + projectTemplateDirectory);
                    }
                }
            }
            else
            {
                LogMessage("FindInFileSystem", "    VS directories not present");
            }
            return foundTempateDirectories;
        }

        static private List<string> FindTemplateDirectoriesInRegistry(RegistryKey rKey, string searchKey)
        {
            RegistryKey VSKey = Registry.CurrentUser;
            List<string> foundTempateDirectories = new List<string>();

            LogMessage("FindInRegistry", "Searching key: " + rKey.ToString() + searchKey);

            try
            {
                VSKey = rKey.OpenSubKey(searchKey, false); //VS BAse Location
            }
            catch { }

            if (VSKey != null)
            {
                string[] vsVersions = VSKey.GetSubKeyNames();

                foreach (string vsversion in vsVersions)
                {
                    //LogMessage("FindTemplateDirectoriesInRegistry", "Found: " + vsversion);
                    string keyname = searchKey + @"\" + vsversion;
                    //LogMessage("FindTemplateDirectoriesInRegistry", "Searching for Key: " + keyname);
                    VSKey = Registry.CurrentUser.OpenSubKey(keyname);
                    string templateDir = (string)VSKey.GetValue(@"UserProjectTemplatesLocation");
                    if (!string.IsNullOrEmpty(templateDir))
                    {
                        LogMessage("FindInRegistry", "    Found Template Directory: " + templateDir);
                        foundTempateDirectories.Add(templateDir);
                    }
                }
            }
            else
            {
                LogMessage("FindInRegistry", "    Registry key not present");
            }
            return foundTempateDirectories;
        }

        //log messages and send to screen when appropriate
        static public void LogMessage(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            EventLogCode.LogEvent("InstallTemplates", logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.InstallTemplatesInfo, "");
        }

        //log error messages and send to screen when appropriate
        static public void LogError(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            EventLogCode.LogEvent("InstallTemplates", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.InstallTemplatesError, logMessage);
        }

        /// <summary>
        /// Removes templates within a particular version of Visual Studio
        /// </summary>
        /// <param name="Name">Descriptive name of the Visual Studio release</param>
        /// <param name="TemplateBasePath">Base path to its location in the HKCU</param>
        /// <returns></returns>
        static int InstallTemplates(string TemplateBasePath, string TemplateSourceDirectory)
        {
            string Platform5VB = TemplateBasePath + @"\Visual Basic\"; // Set up expected paths
            string Platform5CSharp = TemplateBasePath + @"\Visual C#\";
            string Platform6VB = TemplateBasePath + @"\Visual Basic\ASCOM6\";
            string Platform6CSharp = TemplateBasePath + @"\Visual C#\ASCOM6\";

            LogMessage("InstallTemplates", "VB Path (Platform 5): " + Platform5VB);
            LogMessage("InstallTemplates", "C# Path (Platform 5): " + Platform5CSharp);
            LogMessage("InstallTemplates", "VB Path (Platform 6): " + Platform6VB);
            LogMessage("InstallTemplates", "C# Path (Platform 6): " + Platform6CSharp);

            if (Directory.Exists(Platform6CSharp))
            {
                LogMessage("InstallTemplates", "Path: " + Platform6CSharp + " already exists");
            }
            else
            {
                LogMessage("InstallTemplates", "Path: " + Platform6CSharp + " does not exist, creating directory");
                Directory.CreateDirectory(Platform6CSharp);
            }

            if (Directory.Exists(Platform6VB))
            {
                LogMessage("InstallTemplates", "Path: " + Platform6VB + " already exists");
            }
            else
            {
                LogMessage("InstallTemplates", "Path: " + Platform6VB + " does not exist, creating directory");
                Directory.CreateDirectory(Platform6VB);
            }

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

            foreach (string item in Directory.GetFiles(TemplateSourceDirectory, "*.zip"))
            {
                if (item.ToUpper().Contains("CS")) // CSharp item
                {
                    File.Copy(item, Platform6CSharp + Path.GetFileName(item), true);
                    LogMessage("InstallTemplates", "Copying C# template: " + item + " as: " + Platform6CSharp + Path.GetFileName(item));
                }
                if (item.ToUpper().Contains("VB")) // VBitem
                {
                    File.Copy(item, Platform6VB + Path.GetFileName(item), true);
                    LogMessage("InstallTemplates", "Copying VB template: " + item + " as: " + Platform6VB + Path.GetFileName(item));
                }
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
                // LogMessage("FileDelete", "  Deleting file: " + DeletePath + DeleteFile + " " + FileExists);
                if (FileExists) File.Delete(DeletePath + DeleteFile); // Only delete it if it exists!
            }
            catch (Exception ex)
            {
                LogError("FileDelete", "  Exception Deleting file: " + FileExists.ToString() + ", " + DeletePath + DeleteFile + " " + ex.ToString());
            }
        }

    }
}
