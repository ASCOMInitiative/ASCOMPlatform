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
    /// <summary>
    /// Locate all Visual Studio template directories and install ASCOM templates in them, overwriting any current versions present.
    /// </summary>
    class Program
    {
        static TraceLogger TL;
        static int ReturnCode = 0;

        /// <summary>
        /// Main program entry point
        /// </summary>
        /// <param name="args">Supplied parameters (not used in this program)</param>
        /// <returns>0 for success, 1 for error</returns>
        static int Main(string[] args)
        {
            try
            {
                Dictionary<string, string> vsTemplateDirectoryList; // Dictionary to hold the list of install directories holding templates
                string TemplateSourceDirectory = "";
                vsTemplateDirectoryList = new Dictionary<string, string>();

                TL = new TraceLogger("", "InstallTemplates"); // Create a tracelogger so we can log what happens
                TL.Enabled = true;
                LogMessage("Main", "Install date: " + DateTime.Now.ToLongDateString());

                try
                {
                    LogMessage("Main", "Installing new templates...");

                    if (Environment.Is64BitOperatingSystem)
                    {
                        LogMessage("Main", "OS is 64bit");
                        TemplateSourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\ASCOM\Platform 6 Developer Components\Templates";
                    }
                    else
                    {
                        LogMessage("Main", "OS is 32bit");
                        TemplateSourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\ASCOM\Platform 6 Developer Components\Templates";
                    }

                    LogMessage("Main", "Template Source Directory: " + TemplateSourceDirectory);

                    // Search registry for template directories
                    vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\VisualStudio"), vsTemplateDirectoryList);
                    vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\WDExpress"), vsTemplateDirectoryList);

                    // Search My Documents file system for template directories
                    vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInFileSystem(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio*"), vsTemplateDirectoryList);

                    LogMessage("Main", " ");
                    foreach (KeyValuePair<string, string> templateDir in vsTemplateDirectoryList) // Install new templates in every template directory on this machine
                    {
                        LogMessage("Main", "Installing templates in directory: " + templateDir.Key.ToString());
                        InstallTemplates(templateDir.Key.ToString(), TemplateSourceDirectory);
                    }

                }
                catch (Exception ex)
                {
                    LogError("Main", ex.ToString());
                    ReturnCode = 1;
                }

                TL.Enabled = false; // Clean up tracelogger
                TL.Dispose();
                TL = null;
            }
            catch (Exception ex1)
            {
                Console.WriteLine("Exception creating TraceLogger: " + ex1.ToString());
                ReturnCode = 99;
            }
            return ReturnCode;
        }

        /// <summary>
        /// Add new template directory values in the addition list to the overall list ensuring that each value is unique.
        /// </summary>
        /// <param name="additionList">A list containing newly discovered template directories</param>
        /// <param name="overallList">The overall list of unique template install directories</param>
        /// <returns>An updated dictionary of unique template directory values.</returns>
        static private Dictionary<string, string> AddTemplateDirectories(List<string> additionList, Dictionary<string, string> overallList)
        {
            foreach (string templateDirectory in additionList)
            {
                if (!overallList.ContainsKey(templateDirectory)) overallList.Add(templateDirectory, "");
            }

            return overallList;
        }

        /// <summary>
        /// Scan a given directory for sub directories containing the search pattern, return entries where there is a template directory.
        /// </summary>
        /// <param name="searchDir">The base directory to search</param>
        /// <param name="searchPattern">The pattern to search for</param>
        /// <returns>A list of sub directories that match the pattern and that themselves contain template directroies</returns>
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

        /// <summary>
        /// Scan a given registry location for sub keys containing the search key, return entries where there is a template directory entry within that subkey.
        /// </summary>
        /// <param name="rKey">The base key to search from</param>
        /// <param name="searchKey">The subkey name pattern to match</param>
        /// <returns>A list of template directories found</returns>
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

        /// <summary>
        /// Lof an informational message
        /// </summary>
        /// <param name="section">Name of the code section that the message is from </param>
        /// <param name="logMessage">The message to record</param>
        static public void LogMessage(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            EventLogCode.LogEvent("InstallTemplates", logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.InstallTemplatesInfo, "");
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="section">Name of the code section that the message is from </param>
        /// <param name="logMessage">The message to record</param>
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
            const string spaces = "    ";

            LogMessage("InstallTemplates", "TemplateBasePath: " + TemplateBasePath);
            LogMessage("InstallTemplates", "TemplateSourceDirectory: " + TemplateSourceDirectory);

            string Platform5VB = TemplateBasePath + @"\Visual Basic\"; // Set up expected paths
            string Platform5CSharp = TemplateBasePath + @"\Visual C#\";
            string Platform6VB = TemplateBasePath + @"\Visual Basic\ASCOM6\";
            string Platform6CSharp = TemplateBasePath + @"\Visual C#\ASCOM6\";

            LogMessage("InstallTemplates", spaces + "VB Path (Platform 5): " + Platform5VB);
            LogMessage("InstallTemplates", spaces + "C# Path (Platform 5): " + Platform5CSharp);
            LogMessage("InstallTemplates", spaces + "VB Path (Platform 6): " + Platform6VB);
            LogMessage("InstallTemplates", spaces + "C# Path (Platform 6): " + Platform6CSharp);

            if (Directory.Exists(Platform6CSharp))
            {
                LogMessage("InstallTemplates", spaces + "Path: " + Platform6CSharp + " already exists");
            }
            else
            {
                LogMessage("InstallTemplates", spaces + "Path: " + Platform6CSharp + " does not exist, creating directory");
                Directory.CreateDirectory(Platform6CSharp);
            }
            CleanDirectory(Platform6CSharp); // Clean the C# directory

            if (Directory.Exists(Platform6VB))
            {
                LogMessage("InstallTemplates", spaces + "Path: " + Platform6VB + " already exists");
            }
            else
            {
                LogMessage("InstallTemplates", spaces + "Path: " + Platform6VB + " does not exist, creating directory");
                Directory.CreateDirectory(Platform6VB);
            }
            CleanDirectory(Platform6VB); // Clean the C# directory

            FileDelete(Platform5CSharp + "ASCOM Camera Driver (C#).zip"); //Platform 5 C#
            FileDelete(Platform5CSharp + "ASCOM Dome Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM FilterWheel Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM Focuser Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM Rotator Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM Switch Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM Telescope Driver (C#).zip");
            FileDelete(Platform5CSharp + "ASCOM Local Server (singleton).zip");

            FileDelete(Platform5VB + "ASCOM Camera Driver (VB).zip"); // Platform 5 VB
            FileDelete(Platform5VB + "ASCOM Dome Driver (VB).zip");
            FileDelete(Platform5VB + "ASCOM FilterWheel Driver (VB).zip");
            FileDelete(Platform5VB + "ASCOM Focuser Driver (VB).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(2).zip"); //Remove copies introduced by a bad content file in Platform 5 Templates SP1
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(3).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(4).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(5).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(6).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(7).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(8).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(9).zip");
            FileDelete(Platform5VB + "ASCOM Rotator Driver (VB)(10).zip");
            FileDelete(Platform5VB + "ASCOM Switch Driver (VB).zip");
            FileDelete(Platform5VB + "ASCOM Telescope Driver (VB).zip");

            foreach (string item in Directory.GetFiles(TemplateSourceDirectory, "*.zip"))
            {
                if (item.ToUpper().Contains("CS")) // CSharp item
                {
                    LogMessage("InstallTemplates", spaces + "Copying C# template: " + item + " as: " + Platform6CSharp + Path.GetFileName(item));
                    File.Copy(item, Platform6CSharp + Path.GetFileName(item), true);
                }
                if (item.ToUpper().Contains("VB")) // VBitem
                {
                    LogMessage("InstallTemplates", spaces + "Copying VB template: " + item + " as: " + Platform6VB + Path.GetFileName(item));
                    File.Copy(item, Platform6VB + Path.GetFileName(item), true);
                }
            }

            return 0;
        }

        /// <summary>
        /// Erases a file, if it exists, silently logging any exceptions
        /// </summary>
        /// <param name="DeletePath">Path to the file to delete</param>
        /// <param name="DeleteFile">Name of the file to delete</param>
        static void FileDelete(string DeleteFile)
        {
            bool FileExists = File.Exists(DeleteFile);

            try
            {

                if (FileExists)
                {
                    LogMessage("FileDelete", "  Deleting file: " + DeleteFile);
                    File.Delete(DeleteFile); // Only delete it if it exists!
                }
                else
                {
                    LogMessage("FileDelete", "  File: " + DeleteFile + " does not exist.");
                }
            }
            catch (Exception ex)
            {
                LogError("FileDelete", "  Exception Deleting file: " + FileExists.ToString() + ", " + DeleteFile + " " + ex.ToString());
            }
        }

        /// <summary>
        /// Erase all files in a directory
        /// </summary>
        /// <param name="DirectoryPath">Full path of the directory to clear</param>
        static void CleanDirectory(string DirectoryPath)
        {
            if (Directory.Exists(DirectoryPath))
            {
                LogMessage("CleanDirectory", "Cleaning directory: " + DirectoryPath);
                string[] directoryFiles = Directory.GetFiles(DirectoryPath);
                foreach (string file in directoryFiles)
                {
                    FileDelete(file);
                }
            }
            else
            {
                LogMessage("CleanDirectory", "Directory: " + DirectoryPath + " does not exist, no files to clean.");
            }
        }

    }
}
