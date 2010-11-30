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

namespace UninstallAscom
{
    class Program
    {
        const string platform532 ="SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string platform564 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string platform5532 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";
        const string platform5564 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";
        const string uninstallString = "UninstallString";
        const string REGISTRY_ROOT_KEY_NAME = @"SOFTWARE\ASCOM"; //Location of ASCOM profile in HKLM registry hive

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        ); 
        static TraceLogger TL = new TraceLogger("","UninstallASCOM"); // Create a tracelogger so we can log what happens

        static void Main()
        {
            TL.Enabled = true; 
            LogMessage("Cleanup","Removing previous versions of ASCOM....");
            
            //Initial setup
            bool is64BitProcess = (IntPtr.Size == 8);
            bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();
            TL.LogMessage("Cleanup", "OS is 64bit: " + is64BitOperatingSystem.ToString() + ", Process is 64bit: " + is64BitProcess.ToString());

            string platform564KeyValue = null;
            string platform5564KeyValue = null;
            bool found = false;

            if (is64BitOperatingSystem)
            {
                platform5564KeyValue = Read(uninstallString, platform5564);
                platform564KeyValue =  Read(uninstallString, platform564);
            }

            var platform5532KeyValue = Read(uninstallString, platform5532);
            var platform532KeyValue = Read(uninstallString, platform532);

            // Migrate the profile based on the latest platform installed
            if ((platform5564KeyValue != null) | (platform5532KeyValue != null)) MigrateProfile("5.5");
            else if ((platform564KeyValue != null) | (platform532KeyValue != null)) MigrateProfile("5");
            else MigrateProfile("");
            
            //remove 5.5
            if (platform5564KeyValue != null)
            {
                LogMessage("Uninstall 5.5", "64 Removing ASCOM 5.5...");
                LogMessage("Uninstall 5.5",platform5564KeyValue);
                found = true;
                RunProcess(platform5564KeyValue, " /SILENT");
                RemoveAssembly("policy.1.0.ASCOM.DriverAccess"); // Remove left over policy file
            }
            else
            {
                if (platform5532KeyValue != null)
                {
                    LogMessage("Uninstall 5.5", "32 Removing ASCOM 5.5...");
                    LogMessage("Uninstall 5.5", platform5532KeyValue);
                    found = true;
                    RunProcess(platform5532KeyValue, " /SILENT");
                    RemoveAssembly("policy.1.0.ASCOM.DriverAccess"); // Remove left over policy file
                }
            }

            //remove 5.0
            if (platform564KeyValue != null)
            {
                LogMessage("Uninstall 5.0", "64 Removing ASCOM 5...");
                LogMessage("Uninstall 5.0", platform564KeyValue);
                found = true;
                RunProcess("MsiExec.exe", SplitKey(platform564KeyValue));
            }
            else
            {
                if (platform532KeyValue != null)
                {
                    LogMessage("Uninstall 5.0", "32 Removing ASCOM 5...");
                    LogMessage("Uninstall 5.0", platform532KeyValue);
                    found = true;
                    RunProcess("MsiExec.exe", SplitKey(platform532KeyValue));
                }
            }
            if (found == false)
            {
                CleanUp55();
                CleanUp5();
                LogMessage("Cleanup", "Nothing Found");
            }
           
            TL.Enabled = false; // Clean up tracelogger
            TL.Dispose();
            TL = null;

            Pic();
        }

        public static void MigrateProfile(string platformVersion)
        {
            // All previous Platforms are now unistalled but any previous ASCOM profile remains either in the registry from 
            // Platforms 4 and 5 or in the filestore from Platform 5.5. If this is a fresh install we need to create the new profile root key and
            // under all circulstances we need to set the appropriate security access ACL on the registry key. 

            // Additionally the any 5.5 Profile needs to be migrated back to the registry. All of this is acccomplished through the hidden 
            // Profile.Migrate method which takes a parameter indicating the type of Profile previously in use, if any.

            try
            {
                TL.LogMessage("MigrateProfile", "Creating profile object");
                Profile Prof = new Profile(true);
                TL.LogMessage("MigrateProfile", "Migrating Profile"); 
                Prof.MigrateProfile(platformVersion);
                TL.LogMessage("MigrateProfile", "Disposing of profile object");
                Prof.Dispose();
                Prof = null;
            }
            catch (Exception ex)
            {
                // Catch any migration exceptions and log them
                LogMessage("MigrationException", "The following unexpected exception occured during profile migration, please report this on the ASCOM Talk Yahoo Group");
                LogMessage("MigrationException", ex.ToString());
            }
        }

        //log messages and send to screen when appropriate
        public static void LogMessage(string section, string logMessage)
        {
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
        }

        //split the installer string
        public static string SplitKey(string keyToSplit)
        {

            string[] s = keyToSplit.Split(new[] {' '});
            s[1] = s[1].Replace("/I", "/q /x ");
            return s[1];
        }

        //run the uninstaller
        public static bool RunProcess(string processToRun, string args)
        {
            try
            {
                var startInfo = new ProcessStartInfo(processToRun) {Arguments = args};
                var myProcess = Process.Start(startInfo);
                myProcess.WaitForExit();
                myProcess.Close();
                myProcess.Dispose();
                myProcess = null;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //Read a key
        public static string Read(string keyName, string subKey)
        {
            // Opening the registry key
            var rk = Registry.LocalMachine;
            // Open a subKey as read-only
            var sk1 = rk.OpenSubKey(subKey,false);
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
                Console.WriteLine(e.Message);
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
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            return false;
        }

        //clean up any left over files from 5.0
        protected static void CleanUp5()
        {
            //start menu
            var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
            if (Directory.Exists(shortcut))
                DeleteDirectory(shortcut);

            //clea up prog files
            const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
            if (Directory.Exists(ascomDir))
                DeleteDirectory(ascomDir);

            //clean up files
            const string pathToAscom = @"C:\Program Files (x86)\Common Files\ASCOM";
            if (Directory.Exists(pathToAscom))
                DeleteDirectory(pathToAscom);
        }

        //clean up any left over files from 5.5
        public static void CleanUp55()
        {
                //start menu
                var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
                if (Directory.Exists(shortcut))
                    DeleteDirectory(shortcut);

                //clean up prog files
                const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
                if (Directory.Exists(ascomDir))
                    DeleteDirectory(ascomDir);

                //clean up files
                const string pathToAscom = @"C:\Program Files (x86)\Common Files\ASCOM";
                if (Directory.Exists(pathToAscom))
                    DeleteDirectory(pathToAscom);
                
            //clean up files
                const string pathToAscom1 = @"C:\Program Files (x86)\ASCOM";
                if (Directory.Exists(pathToAscom1))
                    DeleteDirectory(pathToAscom1);

                //clean up files
                const string pathToAscom2 = @"C:\Program Files\ASCOM";
                if (Directory.Exists(pathToAscom2))
                    DeleteDirectory(pathToAscom2);
        }

        //reset the file attributes and then deletes the file
        protected static bool DeleteDirectory(string targetDir)
        {
            try
            {
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
                Console.WriteLine(e.Message);
                   return false;
            }

        } 

        public static void Pic()
        {
            Console.WriteLine(" ");
            Console.WriteLine(@" @  *  .  . *       *    .        .        .   *    ..");
            Console.WriteLine(@" @. /\ *     ###     .      .        .            *");
            Console.WriteLine(@" @ /  \  *  #####   .     *      *        *    .");
            Console.WriteLine(@" ]/ [] \  ######### *    .  *       .  //    .  *   .");
            Console.WriteLine(@" / [][] \###\#|#/###   ..    *     .  //  *  .  ..  *");
            Console.WriteLine(@" |  __  | ###\|/###  *    *  ___o |==// .      *   *");
            Console.WriteLine(@" |  |!  |  # }|{  #         /\  \/  //|\");
            Console.WriteLine(@" |  ||  |    }|{    ejm97  / /        | \");
            Console.WriteLine(@"                           ` `        '  '");
            Console.WriteLine(" ");
            Console.WriteLine("Clear Skies:   Press any key to continue... ");
            //Console.Read();
        }

        protected static void SetRegistryACL()
        {
            //Subroutine to control the migration of a Platform 5.5 profile to Platform 6
            Stopwatch swLocal = null;

            swLocal = Stopwatch.StartNew();
            TL.BlankLine();
            TL.LogMessage("SetRegistryACL", @"Creating root key ""\""");
            RegistryKey Key = Registry.LocalMachine.CreateSubKey(REGISTRY_ROOT_KEY_NAME);

            //Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key
            TL.LogMessage("SetRegistryACL", "Creating security identifier");
            SecurityIdentifier DomainSid = new SecurityIdentifier("S-1-0-0"); //Create a starting point domain SID
            SecurityIdentifier Ident = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid); //Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule

            TL.LogMessage("SetRegistryACL", "Creating new ACL rule");
            RegistryAccessRule RegAccessRule = new RegistryAccessRule(Ident, 
                                                                      RegistryRights.FullControl, 
                                                                      InheritanceFlags.ContainerInherit, 
                                                                      PropagationFlags.None, 
                                                                      AccessControlType.Allow); // Create the new access permission rule

            TL.LogMessage("SetRegistryACL", "Retrieving current ACL rule");
            RegistrySecurity KeySec = Key.GetAccessControl(); // Get existing ACL rules on the key 

            AuthorizationRuleCollection Rules = KeySec.GetAccessRules(true, true, typeof(NTAccount)); //typeof(System.Security.Principal.SecurityIdentifier));

            foreach (RegistryAccessRule RegRule in Rules)
            {


                TL.LogMessage("RegistryRule", RegRule.AccessControlType.ToString() + " " +
                                              RegRule.IdentityReference.ToString() + " " +
                                              RegRule.RegistryRights.ToString() + " " +
                                              RegRule.IsInherited.ToString() + " " +
                                              RegRule.InheritanceFlags.ToString() + " " +
                                              RegRule.PropagationFlags.ToString());
            }


            TL.LogMessage("SetRegistryACL", "Adding new ACL rule");
            KeySec.AddAccessRule(RegAccessRule); //Add the new rule to the existing rules
            TL.LogMessage("SetRegistryACL", "Setting new ACL rule");
            Key.SetAccessControl(KeySec); //Apply the new rules to the Profile 
            TL.LogMessage("SetRegistryACL", "Flushing key");
            Key.Flush(); //Flush the key to make sure the permission is committed
            TL.LogMessage("SetRegistryACL", "Closing key");
            Key.Close(); //Close the key after migration

            swLocal.Stop();
            TL.LogMessage("SetRegistryACL", "ElapsedTime " + swLocal.ElapsedMilliseconds + " milliseconds");
            TL.BlankLine();
            swLocal = null;
        }

        protected static void RemoveAssembly(string AssemblyName)
        {
            LogMessage("RemoveAssembly", "Uninstalling: " + AssemblyName); 

            // Get an IAssemblyCache interface
            IAssemblyCache pCache = AssemblyCache.CreateAssemblyCache();

            IASSEMBLYCACHE_UNINSTALL_DISPOSITION puldisposition = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNKNOWN; // Initialise variable
            //Next line changed so that no install reference is provided which allows easy uninstall from Assembly explorer display
            //int result = pCache.UninstallAssembly(0, filename, installReference, out puldisposition);
            int result = pCache.UninstallAssembly(0, AssemblyName, null, out puldisposition);
            if (result == 0) Console.WriteLine("Uninstalled with no error!");
            else Console.WriteLine("Uninstall returned error code: " + result);

            switch (puldisposition)
            {
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED:
                    LogMessage("RemoveAssembly","Outcome: Assembly already uninstalled"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING:
                    LogMessage("RemoveAssembly","Outcome: Delete currently pending"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES:
                    LogMessage("RemoveAssembly","Outcome: Assembly has remaining install references"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND:
                    LogMessage("RemoveAssembly","Outcome: Unable to find assembly - " + AssemblyName); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE:
                    LogMessage("RemoveAssembly","Outcome: Assembly still in use"); break;
                case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED:
                    LogMessage("RemoveAssembly","Outcome: Assembly uninstalled"); break;
                default:
                    LogMessage("RemoveAssembly","Unknown uninstall outcome code: " + puldisposition); break;
            }

        }
    }
}
