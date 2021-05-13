//Comment to force recompilation
using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO;
using ASCOM.Utilities;
using System.Security.AccessControl;
using System.Security.Principal;
using System.GAC;
using System.Text;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices.WindowsRuntime;

namespace UninstallAscom
{
    class Program
    {
        const string PLATFORM_4132 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM Platform 4.1";
        const string PLATFORM_4164 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM Platform 4.1";
        const string PLATFORM_532a = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{075F543B-97C5-4118-9D54-93910DE03FE9}";
        const string PLATFORM_564a = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{075F543B-97C5-4118-9D54-93910DE03FE9}";
        const string PLATFORM_532b = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string PLATFORM_564b = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string PLATFORM_5532 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";
        const string PLATFORM_5564 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";

        const string UNINSTALL_STRING = "UninstallString";
        const string REGISTRY_ROOT_KEY_NAME = @"SOFTWARE\ASCOM"; //Location of ASCOM profile in HKLM registry hive

        //Constants for use with SHGetSpecialFolderPath
        const int CSIDL_PROGRAM_FILES = 38; //0x0026 
        const int CSIDL_PROGRAM_FILESX86 = 42; //0x002a
        const int CSIDL_WINDOWS = 36; // 0x0024
        const int CSIDL_PROGRAM_FILES_COMMONX86 = 44; // 0x002c
        const int CSIDL_SYSTEM = 37;// 0x0025,
        const int CSIDL_SYSTEMX86 = 41; // 0x0029,

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        [DllImport("Shell32.dll")]
        static extern int SHGetSpecialFolderPath([In] IntPtr hwndOwner,
                                                 [Out] StringBuilder lpszPath,
                                                 [In] int nFolder,
                                                 [In] int fCreate);

        static TraceLogger TL;
        static RegistryAccess regAccess;
        static string ascomDirectory;
        static int returnCode = 0;

        static int Main(string[] args)
        {
            if (args[0].ToUpperInvariant() == "/SETPROFILEACL") // Set and check the Profile registry ACL
            {
                try
                {
                    TL = new TraceLogger("", "SetProfileACL"); // Create a trace logger so we can log what happens
                    TL.Enabled = true;

                    // Create the Profile key if required and set its access rights
                    LogMessage("SetRegistryACL", "Creating RegistryAccess object");
                    using (regAccess = new RegistryAccess(TL))
                    {
                        LogMessage("SetRegistryACL", "Setting Profile registry ACL");
                        regAccess.SetRegistryACL();
                    }

                    TL.BlankLine();

                    // Check whether the Profile key has the required access rights
                    using (RegistryKey hklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                    {
                        if (!UserhasFullProfileAccessRights(hklm32, REGISTRY_ROOT_KEY_NAME)) returnCode = 99;
                    }

                }
                catch (Exception ex)
                {
                    LogError("SetProfileACL", ex.ToString());
                    returnCode = 1;
                }
            }
            else // Remove previous platforms, if present
            {
                try
                {

                    TL = new TraceLogger("", "UninstallASCOM"); // Create a trace logger so we can log what happens
                    TL.Enabled = true;

                    LogMessage("Uninstall", "Creating RegistryAccess object");
                    regAccess = new RegistryAccess(TL); //Create a RegistryAccess object that will use the UninstallASCOM trace logger to ensure all messages appear in one log file

                    // This has been removed because it destroys the ability to remove 5.5 after use and does NOT restore all the 
                    // Platform 5 files resulting in an unexpected automatic repair. Its left here just in case, Please DO NOT RE-ENABLE THIS FEATURE unless have a way round the resulting issues
                    // CreateRestorePoint(); 

                    LogMessage("Uninstall", "Removing previous versions of ASCOM....");

                    //Initial setup
                    bool is64BitProcess = (IntPtr.Size == 8);
                    bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();
                    LogMessage("Uninstall", "OS is 64bit: " + is64BitOperatingSystem.ToString() + ", Process is 64bit: " + is64BitProcess.ToString());

                    string platform4164KeyValue = null;
                    string platform564aKeyValue = null;
                    string platform564bKeyValue = null;
                    string platform564KeyValue = null;
                    string platform5564KeyValue = null;

                    bool found = false;

                    if (is64BitOperatingSystem) // Is a 64bit OS
                    {
                        platform4164KeyValue = Read(UNINSTALL_STRING, PLATFORM_4164); // Read the 4.1 uninstall string
                        platform5564KeyValue = Read(UNINSTALL_STRING, PLATFORM_5564); // Read the 5.5 uninstall string
                        platform564aKeyValue = Read(UNINSTALL_STRING, PLATFORM_564a); // Read the 5.0A uninstall string 
                        platform564bKeyValue = Read(UNINSTALL_STRING, PLATFORM_564b); // Read the 5.0B uninstall string

                        if (platform564bKeyValue != null) // We have a 5.0B value so go with this
                        {
                            platform564KeyValue = platform564bKeyValue;
                            LogMessage("Uninstall", "Found 64bit Platform 5.0B");
                        }
                        else if (platform564aKeyValue != null) // No 5.0B value so go  with 5.0A if it is present
                        {
                            platform564KeyValue = platform564aKeyValue;
                            LogMessage("Uninstall", "Found 64bit Platform 5.0A");
                            //Now have to fix a missing registry key that fouls up the uninstaller - this was fixed in 5B but prevents 5A from uninstalling on 64bit systems
                            RegistryKey RKey = Registry.ClassesRoot.CreateSubKey(@"AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");
                            RKey.SetValue("", "Fixed registry key value");
                            RKey.Close();
                            RKey = null;
                            LogMessage("Uninstall", @"Successfully set AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}");
                        }

                        StringBuilder Path = new StringBuilder(260);
                        int rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMONX86, 0);
                        ascomDirectory = Path.ToString() + @"\ASCOM";
                        LogMessage("Uninstall", "64bit Common Files Path: " + ascomDirectory);
                    }
                    else //32 bit OS
                    {
                        ascomDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\ASCOM";
                        LogMessage("Uninstall", "32bit Common Files Path: " + ascomDirectory);
                    }

                    string platform4132KeyValue = Read(UNINSTALL_STRING, PLATFORM_4132);
                    string platform5532KeyValue = Read(UNINSTALL_STRING, PLATFORM_5532);
                    string platform532aKeyValue = Read(UNINSTALL_STRING, PLATFORM_532a);
                    string platform532bKeyValue = Read(UNINSTALL_STRING, PLATFORM_532b);
                    string platform532KeyValue = null;

                    if (platform532bKeyValue != null) // We have a 5.0B value so go with this
                    {
                        platform532KeyValue = platform532bKeyValue;
                        LogMessage("Uninstall", "Found 32bit Platform 5,0B");
                    }
                    else if (platform532aKeyValue != null) // No 5.0B value so go  with 5.0A if it is present
                    {
                        platform532KeyValue = platform532aKeyValue;
                        LogMessage("Uninstall", "Found 32bit Platform 5,0A");
                    }

                    // Backup the profile based on the latest platform installed
                    if ((platform5564KeyValue != null) | (platform5532KeyValue != null)) regAccess.BackupProfile("5.5");
                    else if ((platform564KeyValue != null) | (platform532KeyValue != null)) regAccess.BackupProfile("5");
                    else regAccess.BackupProfile("");

                    //remove 5.5
                    if (platform5564KeyValue != null)
                    {
                        LogMessage("Uninstall", "64 Removing ASCOM 5.5... " + platform5564KeyValue);
                        found = true;
                        RunProcess(platform5564KeyValue, " /VERYSILENT /NORESTART /LOG");
                        RemoveAssembly("policy.1.0.ASCOM.DriverAccess"); // Remove left over policy file
                    }
                    else
                    {
                        if (platform5532KeyValue != null)
                        {
                            LogMessage("Uninstall", "32 Removing ASCOM 5.5... " + platform5532KeyValue);
                            found = true;
                            RunProcess(platform5532KeyValue, " /VERYSILENT /NORESTART /LOG");
                            RemoveAssembly("policy.1.0.ASCOM.DriverAccess"); // Remove left over policy file
                        }
                    }

                    //remove 5.0
                    if (platform564KeyValue != null)
                    {
                        FixHelper("Helper.dll", 5, 0);// Original helpers should be in place at this point, check and fix if not to prevent Platform 5 uninstaller from failing
                        FixHelper("Helper2.dll", 4, 0);
                        LogMessage("Uninstall", "64 Removing ASCOM 5... " + platform564KeyValue);
                        found = true;
                        RunProcess("MsiExec.exe", SplitKey(platform564KeyValue));
                    }
                    else
                    {
                        if (platform532KeyValue != null)
                        {
                            FixHelper("Helper.dll", 5, 0);
                            FixHelper("Helper2.dll", 4, 0);
                            LogMessage("Uninstall", "32 Removing ASCOM 5... " + platform532KeyValue);
                            found = true;
                            RunProcess("MsiExec.exe", SplitKey(platform532KeyValue));
                        }
                    }

                    //Remove 4.1
                    //remove 5.0
                    if (platform4164KeyValue != null)
                    {
                        LogMessage("Uninstall", "64 Removing ASCOM 4.1... " + platform4164KeyValue);
                        found = true;

                        string[] vals = platform4164KeyValue.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
                        LogMessage("Uninstall", @"Found uninstall values: """ + vals[0] + @""", """ + vals[1] + @"""");

                        RunProcess(vals[0], @"/S /Z " + vals[1]);
                        CleanUp4();
                    }
                    else
                    {
                        if (platform4132KeyValue != null)
                        {
                            LogMessage("Uninstall", "32 Removing ASCOM 4.1... " + platform4132KeyValue);
                            found = true;

                            string[] vals = platform4132KeyValue.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
                            LogMessage("Uninstall", @"Found uninstall values: """ + vals[0] + @""", """ + vals[1] + @"""");

                            RunProcess(vals[0], @"/S /Z " + vals[1]);
                            CleanUp4();
                        }
                    }

                    if (found == true)
                    {
                        CleanUp55();
                        CleanUp5();
                    }
                    else
                    {
                        LogMessage("Uninstall", "No previous platforms found");
                    }

                    LogMessage("Uninstall", "Setting Profile registry ACL");
                    regAccess.SetRegistryACL();

                    // Restore the relevant profile based on the latest platform installed
                    if ((platform5564KeyValue != null) | (platform5532KeyValue != null))
                    {
                        LogMessage("Uninstall", "Restoring Platform 5.5 Profile");
                        regAccess.RestoreProfile("5.5");
                    }
                    else if ((platform564KeyValue != null) | (platform532KeyValue != null))
                    {
                        LogMessage("Uninstall", "Restoring Platform 5 Profile");
                        regAccess.RestoreProfile("5");
                    }

                    LogMessage("Uninstall", "Disposing of registry access object");
                    regAccess.Dispose();
                    regAccess = null;
                    LogMessage("Uninstall", "Completed uninstall process");
                }
                catch (Exception ex)
                {
                    LogError("Uninstall", ex.ToString());
                    returnCode = 1;
                }
            }

            TL.Enabled = false; // Clean up trace logger
            TL.Dispose();

            return returnCode;
        }

        static private bool UserhasFullProfileAccessRights(RegistryKey key, string subKey)
        {
            RegistryKey sKey;
            bool foundFullAccess = false; // Initialise to the failed condition
            string builtInUsers;

            try
            {
                // Find the localised name of the BUILTIN\Users group
                try
                {
                    builtInUsers = new SecurityIdentifier("S-1-5-32-545").Translate(typeof(NTAccount)).ToString(); // S-1-5-32-545 is the locale independent descriptor for the BUILTIN\Users group
                    TL.LogMessage("RegistryRights", $"Localised name of BUILTIN\\users group is: '{builtInUsers}'");

                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("RegistryRights", $"Exception when getting BUILTIN\\Users localised name: {ex}");
                    builtInUsers = "BUILTIN\\Users"; // Initialise to a common value in case we get lucky and this is the correct localised name
                }

                TL.LogMessage("RegistryRights", (subKey == "") ? key.Name.ToString() : key.Name.ToString() + @"\" + subKey);

                if (subKey == "")
                    sKey = key;
                else
                    sKey = key.OpenSubKey(subKey);

                RegistrySecurity sec = sKey.GetAccessControl();

                foreach (RegistryAccessRule RegRule in sec.GetAccessRules(true, true, typeof(NTAccount))) // Iterate over the rule set and list them
                {
                    try
                    {
                        TL.LogMessage("RegistryRights", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + RegRule.RegistryRights.ToString() + " / " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " / " + RegRule.InheritanceFlags.ToString() + " / " + RegRule.PropagationFlags.ToString());
                    }
                    catch (Exception ex1)
                    {
                        TL.LogMessageCrLf("RegistryRights", "Issue formatting registry rights: " + ex1.ToString());
                    }

                    if ((RegRule.IdentityReference.ToString().ToUpperInvariant() == builtInUsers.ToUpperInvariant()) & (RegRule.RegistryRights == global::System.Security.AccessControl.RegistryRights.FullControl))
                        foundFullAccess = true;
                }

                if (foundFullAccess)
                {
                    TL.BlankLine();
                    TL.LogMessage("RegistryRights", "OK - SubKey " + subKey + @" does have full registry access rights for BUILTIN\Users");
                }
                else
                    LogError("RegistryRights", "Subkey " + subKey + @" does not have full access rights for BUILTIN\Users!");
            }
            catch (NullReferenceException)
            {
                TL.LogMessageCrLf("RegistryRights", "The subkey: " + key.Name + @"\" + subKey + " does not exist.");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RegistryRights", "Issue reading registry rights: " + ex.ToString());
            }

            TL.BlankLine();

            return foundFullAccess;
        }

        /// <summary>
        /// Returns the localised text name of the BUILTIN\Users group. This varies by locale so has to be derived on the users system.
        /// </summary>
        /// <returns>Localised name of the BUILTIN\Users group</returns>
        /// <remarks>
        /// This uses the WMI features and is pretty obscure but is the only way I could find to do this! Peter
        /// Now superseded by much better non WMI code: builtInUsers = new SecurityIdentifier("S-1-5-32-545").Translate(typeof(System.Security.Principal.NTAccount)).ToString();
        /// </remarks>
        //private static string GetBuiltInUsers()
        //{
        //    ManagementObjectSearcher searcher;
        //    string Group = "BUILTIN"; // Initialise to default values
        //    string Name = "Users";

        //    try
        //    {
        //        searcher = new ManagementObjectSearcher(new ManagementScope(@"\\localhost\root\cimv2"), new WqlObjectQuery("Select * From Win32_Account Where SID = 'S-1-5-32'"), null);
        //        TL.LogMessage("GetBuiltInUsers", $"Searcher object for S-1-5-32 is null: {searcher == null}");

        //        foreach (var wmiClass in searcher.Get())
        //        {
        //            PropertyDataCollection p;
        //            p = wmiClass.Properties;
        //            foreach (var pr in p)
        //            {
        //                if (pr.Name == "Name")
        //                    Group = pr.Value.ToString();
        //            }
        //        }
        //        searcher.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        TL.LogMessageCrLf("GetBuiltInUsers 1", ex.ToString());
        //    }

        //    try
        //    {
        //        searcher = new ManagementObjectSearcher(new ManagementScope(@"\\localhost\root\cimv2"), new WqlObjectQuery("Select * From Win32_Group Where SID = 'S-1-5-32-545'"), null);
        //        TL.LogMessage("GetBuiltInUsers", $"Searcher object for S-1-5-32-545 is null: {searcher == null}");

        //        foreach (var wmiClass in searcher.Get())
        //        {
        //            PropertyDataCollection p;
        //            p = wmiClass.Properties;
        //            foreach (var pr in p)
        //            {
        //                if (pr.Name == "Name")
        //                    Name = pr.Value.ToString();
        //            }
        //        }
        //        searcher.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        TL.LogMessageCrLf("GetBuiltInUsers 2", ex.ToString());
        //    }

        //    TL.LogMessage("GetBuiltInUsers", $"Returning {Group}\\{Name}");
        //    return Group + @"\" + Name;
        //}

        public static void FixHelper(string HelperName, int MajorVersion, int MinorVersion)
        {
            StringBuilder PathShell = new StringBuilder(260);
            try
            {
                string HelperFileName = ascomDirectory + @"\" + HelperName;
                LogMessage("FixHelper", "Ensuring " + HelperName + " is a Platform 5 version: " + HelperFileName);
                FileVersionInfo FVInfo = FileVersionInfo.GetVersionInfo(HelperFileName);
                LogMessage("FixHelper", "  Found version : " + FVInfo.FileMajorPart.ToString() + "." + FVInfo.FileMinorPart.ToString() + "." + FVInfo.FileBuildPart.ToString() + "." + FVInfo.FilePrivatePart.ToString());
                if ((((FVInfo.FileMajorPart * 0x1000) + FVInfo.FileMinorPart) > ((MajorVersion * 0x1000) + MinorVersion))) // Version is not 5.0.x.x so replace
                {
                    LogMessage("FixHelper", "  File does not match required version number: " + MajorVersion.ToString() + "." + MinorVersion.ToString() + ".x.x, restoring original Platform 5 file");
                    try
                    {
                        File.Copy(HelperName, HelperFileName, true); // Copy from the installer directory to the ASCOM directory on this system
                        LogMessage("FixHelper", "  File copied OK");
                        FVInfo = FileVersionInfo.GetVersionInfo(HelperFileName);
                        LogMessage("FixHelper", "  Restored version : " + FVInfo.FileMajorPart.ToString() + "." + FVInfo.FileMinorPart.ToString() + "." + FVInfo.FileBuildPart.ToString() + "." + FVInfo.FilePrivatePart.ToString());
                        if ((((FVInfo.FileMajorPart * 0x1000) + FVInfo.FileMinorPart) > ((MajorVersion * 0x1000) + MinorVersion))) // Version is not 5.0.x.x so replace
                        {
                            LogMessage("FixHelper", "  ERROR incorrect file still in place!");
                        }
                        else
                        {
                            LogMessage("FixHelper", "  OK correct file now in place");
                        }

                    }
                    catch (Exception ex)
                    {
                        LogError("FixHelper", "File copy exception: " + ex.ToString());
                    }
                }
                else
                {
                    LogMessage("FixHelper", "  OK leaving file in place");
                }

                // Now make sure our version of Helper is the COM registered version!
                LogMessage("FixHelper", "  Fixing COM Registration");
                if (VersionCode.OSBits() == VersionCode.Bitness.Bits64) // We are running on a 64bit OS
                {
                    SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEMX86, 0);
                }
                else // We are running on a 32bit OS
                {
                    SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEM, 0);// Get the system directory                 
                }
                string RegSvr32Path = PathShell.ToString() + "\\RegSvr32.exe"; //Construct the full path to RegSvr32.exe
                string AscomPath = ascomDirectory + "\\" + HelperName;
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.FileName = RegSvr32Path; //Populate the ProcessStartInfo with the full path to RegSvr32.exe 
                Info.Arguments = "/s \"" + AscomPath + "\""; // And the start parameter specifying the file to COM register

                LogMessage("FixHelper", "  RegSvr32 Path: \"" + RegSvr32Path + "\", COM Path: \"" + AscomPath + "\"");

                Process P = new Process(); // Create the process           
                P.StartInfo = Info; // Set the start info
                P.Start(); //Start the process and wait for it to finish
                LogMessage("FixHelper", "  Started registration");
                P.WaitForExit();
                LogMessage("FixHelper", "  Finished registration, Return code: " + P.ExitCode);
                P.Dispose();
            }
            catch (Exception ex)
            {
                LogError("FixHelper", "FixHelper exception: " + ex.ToString());
            }
        }

        //log messages and send to screen when appropriate
        public static void LogMessage(string section, string logMessage)
        {
            // Make sure none of these failing stops the overall migration process
            try
            {
                Console.WriteLine(logMessage);
            }
            catch { }
            try
            {
                TL.LogMessageCrLf(section, logMessage);  // The CrLf version is used in order properly to format exception messages
            }
            catch { }
            try
            {
                EventLogCode.LogEvent("UninstallAscom", logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.UninstallASCOMInfo, "");
            }
            catch { }
        }

        //log error messages and send to screen when appropriate
        public static void LogError(string section, string logMessage)
        {
            try
            {
                Console.WriteLine(logMessage);
            }
            catch { }
            try
            {
                TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            }
            catch { }
            try
            {
                EventLogCode.LogEvent("UninstallAscom", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.UninstallASCOMError, logMessage);
            }
            catch { }
        }

        //split the installer string
        public static string SplitKey(string keyToSplit)
        {

            string[] s = keyToSplit.Split(new[] { ' ' });
            s[1] = s[1].Replace("/I", "/q /x ");
            return s[1];
        }

        //run the uninstaller
        public static bool RunProcess(string processToRun, string args)
        {
            try
            {
                LogMessage("RunProcess", "  Starting process: " + processToRun + " " + args);
                var startInfo = new ProcessStartInfo(processToRun) { Arguments = args };
                var myProcess = Process.Start(startInfo);
                myProcess.WaitForExit();
                LogMessage("RunProcess", "  Exit code: " + myProcess.ExitCode);
                if (returnCode == 0) returnCode = myProcess.ExitCode; //Update return code if it doesn't already contain an error
                myProcess.Close();
                myProcess.Dispose();
                myProcess = null;
                return true;
            }
            catch (Exception e)
            {
                LogError("RunProcess", "Exception: " + e.ToString());
                return false;
            }
        }

        //Read a key
        public static string Read(string keyName, string subKey)
        {
            // Opening the registry key
            var rk = Registry.LocalMachine;
            // Open a subKey as read-only
            var sk1 = rk.OpenSubKey(subKey, false);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            try
            {
                // If the RegistryKey exists I get its value
                // or null is returned.
                return (string)sk1.GetValue(keyName);
            }
            catch (Exception e)
            {
                LogError("Read", "Exception: " + e.ToString());
                return null;
            }
        }

        //check the OS
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    if (!IsWow64Process(p.Handle, out bool retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            return false;
        }


        // Clean up debris left over from 4
        protected static void CleanUp4()
        {
            RegistryAccess RA = new RegistryAccess();
            RegistryKey RK = RA.OpenSubKey3264(Registry.LocalMachine, @"SOFTWARE\ASCOM", true, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
            try
            {
                RK.DeleteSubKeyTree(@"Telescope Drivers\SS2K.Telescope");
                LogMessage("CleanUp4", @"Deleted Registry: Telescope Drivers\SS2K.Telescope");
            }
            catch { }

            try
            {
                RK.DeleteSubKeyTree(@"Focuser Drivers\PCFocus.Focuser");
                LogMessage("CleanUp4", @"Deleted Registry: Focuser Drivers\PCFocus.Focuser");
            }
            catch { }
            RK.Close();
            RA.Dispose();
        }

        //clean up any left over files from 5.0
        protected static void CleanUp5()
        {
            //start menu
            var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
            LogMessage("CleanUp5", "Start Menu Path: " + shortcut);
            if (Directory.Exists(shortcut))
                DeleteDirectory(shortcut);

            //clea up prog files
            const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
            LogMessage("CleanUp5", "ProgramData Path: " + ascomDir);
            if (Directory.Exists(ascomDir))
                DeleteDirectory(ascomDir);

            //clean up files
            //const string pathToAscom = @"C:\Program Files (x86)\Common Files\ASCOM";
            //if (Directory.Exists(pathToAscom))
            //    DeleteDirectory(pathToAscom);
        }

        //clean up any left over files from 5.5
        public static void CleanUp55()
        {
            //start menu
            var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
            LogMessage("CleanUp55", "Start Menu Path: " + shortcut);
            if (Directory.Exists(shortcut))
                DeleteDirectory(shortcut);

            //clean up prog files
            const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
            LogMessage("CleanUp55", "ProgramData Path: " + ascomDir);
            if (Directory.Exists(ascomDir))
                DeleteDirectory(ascomDir);

            //clean up files
            /*string pathToAscom = AscomDirectory + @"\ASCOM\.net"; //Remove the .net directory to fully clean this up
            LogMessage("CleanUp55", "ASCOM .net Path: " + pathToAscom);
            if (Directory.Exists(pathToAscom))
                DeleteDirectory(pathToAscom);
            */
            //clean up files
            const string pathToAscom1 = @"C:\Program Files (x86)\ASCOM";
            LogMessage("CleanUp55", "ASCOM Program Files (x86) Path: " + pathToAscom1);
            if (Directory.Exists(pathToAscom1))
                DeleteDirectory(pathToAscom1);

            //clean up files
            const string pathToAscom2 = @"C:\Program Files\ASCOM";
            LogMessage("CleanUp55", "ASCOM Program Files Path: " + pathToAscom2);
            if (Directory.Exists(pathToAscom2))
                DeleteDirectory(pathToAscom2);
        }

        //reset the file attributes and then deletes the file
        protected static bool DeleteDirectory(string targetDir)
        {
            try
            {
                LogMessage("DeleteDirectory", "Deleting directory: " + targetDir);
                string[] files = Directory.GetFiles(targetDir);
                string[] dirs = Directory.GetDirectories(targetDir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(targetDir, true);

                return true;

            }
            catch (Exception e)
            {
                LogError("DeleteDirectory", "Exception: " + e.ToString());
                return false;
            }

        }

        protected static void RemoveAssembly(string AssemblyName)
        {
            LogMessage("RemoveAssembly", "Uninstalling: " + AssemblyName);

            // Get an IAssemblyCache interface
            IAssemblyCache pCache = AssemblyCache.CreateAssemblyCache();

            int result = pCache.UninstallAssembly(0, AssemblyName, null, out IASSEMBLYCACHE_UNINSTALL_DISPOSITION puldisposition);
            if (result == 0) LogMessage("RemoveAssembly", "Uninstalled without error!");
            else LogMessage("RemoveAssembly", "Uninstall returned status code: " + result);

            switch (puldisposition)
            {
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED:
                    LogMessage("RemoveAssembly", "Outcome: Assembly already uninstalled"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING:
                    LogMessage("RemoveAssembly", "Outcome: Delete currently pending"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES:
                    LogMessage("RemoveAssembly", "Outcome: Assembly has remaining install references"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND:
                    LogMessage("RemoveAssembly", "Outcome: Unable to find assembly - " + AssemblyName); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE:
                    LogMessage("RemoveAssembly", "Outcome: Assembly still in use"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED:
                    LogMessage("RemoveAssembly", "Outcome: Assembly uninstalled"); break;
                default:
                    LogMessage("RemoveAssembly", "Unknown uninstall outcome code: " + puldisposition); break;
            }

        }

        protected static void CreateRestorePoint()
        {
            try
            {
                LogMessage("CreateRestorePoint", "Creating Restore Point");
                ManagementScope oScope = new ManagementScope("\\\\localhost\\root\\default");
                ManagementPath oPath = new ManagementPath("SystemRestore");
                ObjectGetOptions oGetOp = new ObjectGetOptions();
                ManagementClass oProcess = new ManagementClass(oScope, oPath, oGetOp);

                ManagementBaseObject oInParams = oProcess.GetMethodParameters("CreateRestorePoint");
                oInParams["Description"] = "ASCOM Platform 6";
                oInParams["RestorePointType"] = 0;
                oInParams["EventType"] = 100;

                ManagementBaseObject oOutParams = oProcess.InvokeMethod("CreateRestorePoint", oInParams, null);
                LogMessage("CreateRestorePoint", "Returned from CreateRestorePoint method");
            }
            catch (Exception ex)
            {
                LogError("CreateRestorePoint", ex.ToString());
            }

        }
    }
}
