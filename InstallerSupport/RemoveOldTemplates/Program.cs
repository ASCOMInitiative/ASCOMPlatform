using System;
using System.Collections.Generic;
//using ASCOM.Utilities;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
//using static ASCOM.Utilities.Global;

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
                string[] vsDirs = new string[0];

                Dictionary<string, string> vsTemplateDirectoryList; // Dictionary to hold the list of install directories holding templates
                vsTemplateDirectoryList = new Dictionary<string, string>();

                TL = new TraceLogger("", "InstallTemplates"); // Create a trace logger so we can log what happens
                TL.Enabled = true;

                string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                LogMessage("Main", "Installer version: " + fileVersion);
                LogMessage("Main", "Install date: " + DateTime.Now.ToLongDateString());
                LogMessage("Main", $"There are : {args.Length} arguments.");
                LogMessage("Main", $"Argument[0]: {args[0]}.");
                LogMessage("Main", "");

                // Handle command line parameters
                if (args[0].ToUpperInvariant() == "/CLEANUP") // Clean up previous template installation
                {
                    LogMessage("CleanUp", $"Cleaning previous template installation...");

                    // Find the roaming profile folder
                    string roamingApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    LogMessage("CleanUp", $"Roaming application data path: {roamingApplicationDataFolder}");

                    // Find the local profile folder
                    string localApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    LogMessage("CleanUp", $"Local application data path: {localApplicationDataFolder}");
                    
                    // Find the Visual Studio folder in the local profile
                    string vsPathLocal = $"{localApplicationDataFolder}\\Microsoft\\VisualStudio";
                    LogMessage("CleanUp", $"Local Visual Studio path: {vsPathLocal}");

                    // Find the project template cache in the local profile 
                    vsDirs = Directory.GetDirectories(vsPathLocal, "ProjectTemplatesCache*", SearchOption.AllDirectories);
                    foreach (string dir in vsDirs)
                    {
                        // Delete the cache file
                        try
                        {
                            LogMessage("CleanUp", $"Found local cache folder: {dir}, deleting file cache.bin");
                            File.Delete($"{dir}\\cache.bin");
                        }
                        catch (Exception ex)
                        {
                            LogMessage("CleanUp", "Delete cache.bin exception: " + ex.ToString());
                        }
                    }

                    // Find the project template cache in the roaming profile 
                    string searchPathRoaming = $"{roamingApplicationDataFolder}\\Microsoft\\VisualStudio";
                    LogMessage("CleanUp", $"Roaming search path: {searchPathRoaming}");

                    // Find the ASCOM cache contents
                    vsDirs = Directory.GetDirectories(searchPathRoaming, "ASCOM6", SearchOption.AllDirectories);
                    foreach (string dir in vsDirs)
                    {
                        // Delete the cached ASCOM project template files
                        try
                        {
                            LogMessage("CleanUp", $"Deleting roaming cache folder: {dir}");
                            File.SetAttributes(dir, FileAttributes.Normal);
                            Directory.Delete(dir, true);
                        }
                        catch (Exception ex)
                        {
                            LogMessage("CleanUp", "Delete roaming cache folder exception: " + ex.ToString());
                        }
                    }

                    // Delete the Visual Studio cache file.
                    vsDirs = Directory.GetDirectories(searchPathRoaming, "ProjectTemplatesCache", SearchOption.AllDirectories);
                    foreach (string dir in vsDirs)
                    {
                        try
                        {
                            LogMessage("CleanUp", $"Found roaming cache folder: {dir}, deleting file cache.bin");
                            File.Delete($"{dir}\\cache.bin");
                        }
                        catch (Exception ex)
                        {
                            LogMessage("CleanUp", "Delete roaming cache file cache.bin exception: " + ex.ToString());
                        }
                    }

                    // Find and delete any project template source files that may remain in the user's machine
                    try
                    {
                        // Search registry for template directories
                        vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\VisualStudio"), vsTemplateDirectoryList);
                        vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInRegistry(Registry.CurrentUser, @"Software\Microsoft\WDExpress"), vsTemplateDirectoryList);

                        // Search My Documents file system for template directories
                        vsTemplateDirectoryList = AddTemplateDirectories(FindTemplateDirectoriesInFileSystem(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Visual Studio*"), vsTemplateDirectoryList);

                        LogMessage("CleanUp", " ");
                        foreach (KeyValuePair<string, string> templateDir in vsTemplateDirectoryList) // Install new templates in every template directory on this machine
                        {
                            vsDirs = Directory.GetDirectories(templateDir.Key, "ASCOM6", SearchOption.AllDirectories);
                            foreach (string dir in vsDirs)
                            {
                                try
                                {
                                    LogMessage("CleanUp", $"Deleting templates in directory: {dir}");
                                    Directory.Delete(dir, true); ;
                                }
                                catch (Exception ex)
                                {
                                    LogMessage("CleanUp", "Delete roaming cache folder exception: " + ex.ToString());
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError("CleanUp", ex.ToString());
                        ReturnCode = 100;
                    }

                    LogMessage("CleanUp", $"Finished cleaning previous template installation.");
                }
                else // Install templates
                {
                    throw new ASCOM.InvalidOperationException($"The command: {args[0]} is not supported, only the /CLEANUP command is supported.");
                }

                // Clean up trace logger
                TL.Enabled = false;
                TL.Dispose();
                TL = null;
            }
            catch (Exception ex1)
            {
                Console.WriteLine("InstallTemplates exception: " + ex1.ToString());
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
            try
            {
                foreach (string templateDirectory in additionList)
                {
                    if (!overallList.ContainsKey(templateDirectory)) overallList.Add(templateDirectory, "");
                }

            }
            catch (Exception ex)
            {
                LogError("AddTemplateDirectories", "  Exception adding template directories: " + ex.ToString());
                ReturnCode = 2;
            }

            return overallList;
        }

        /// <summary>
        /// Scan a given directory for sub directories containing the search pattern, return entries where there is a template directory.
        /// </summary>
        /// <param name="searchDir">The base directory to search</param>
        /// <param name="searchPattern">The pattern to search for</param>
        /// <returns>A list of sub directories that match the pattern and that themselves contain template directories</returns>
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
            List<string> foundTempateDirectories = new List<string>();

            try
            {
                RegistryKey VSKey = Registry.CurrentUser;

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
            }
            catch (Exception ex)
            {
                LogError("AddTemplateDirectories", "  Exception adding template directories: " + ex.ToString());
                ReturnCode = 3;
            }

            return foundTempateDirectories;
        }

        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="section">Name of the code section that the message is from </param>
        /// <param name="logMessage">The message to record</param>
        static public void LogMessage(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            Global.LogEvent("InstallTemplates", logMessage, EventLogEntryType.Information, EventLogErrors.InstallTemplatesInfo, "");
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
            Global.LogEvent("InstallTemplates", "Exception", EventLogEntryType.Error, EventLogErrors.InstallTemplatesError, logMessage);
        }

    }
}
