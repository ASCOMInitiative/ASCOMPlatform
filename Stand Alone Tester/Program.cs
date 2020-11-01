using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Stand_Alone_Tester
{
    class Program
    {
        private static string g_DefaultLogFilePath;
        private static string g_LogFileActualName;
        const string g_LogFileType = "ASCOMTEST";
        private static StreamWriter g_LogFile;
        const int g_IdentifierWidth = 30;
        private static StandAloneLogger SAL;

        /// <summary>
        /// Enum containing all the possible registry access rights values. The built-in RegistryRights enum only has a partial collection
        /// and often returns values such as -1 or large positive and negative integer values when converted to a string
        ///The Flags attribute ensures that the ToString operation returns an aggregate list of discrete values
        ///</summary>

        [Flags()]
        enum AccessRights : uint
        {
            Query = 1,
            SetKey = 2,
            CreateSubKey = 4,
            EnumSubkey = 8,
            Notify = 0x10,
            CreateLink = 0x20,
            Unknown40 = 0x40,
            Unknown80 = 0x80,
            Wow64_64Key = 0x100,
            Wow64_32Key = 0x200,
            Unknown400 = 0x400,
            Unknown800 = 0x800,
            Unknown1000 = 0x1000,
            Unknown2000 = 0x2000,
            Unknown4000 = 0x4000,
            Unknown8000 = 0x8000,
            StandardDelete = 0x10000,
            StandardReadControl = 0x20000,
            StandardWriteDAC = 0x40000,
            StandardWriteOwner = 0x80000,
            StandardSynchronize = 0x100000,
            Unknown200000 = 0x200000,
            Unknown400000 = 0x400000,
            AuditAccess = 0x800000,
            AccessSystemSecurity = 0x1000000,
            MaximumAllowed = 0x2000000,
            Unknown4000000 = 0x4000000,
            Unknown8000000 = 0x8000000,
            GenericAll = 0x10000000,
            GenericExecute = 0x20000000,
            GenericWrite = 0x40000000,
            GenericRead = 0x80000000
        }

        static void Main(string[] args)
        {
            const string TRACE_LOGGER_PATH = @"\ASCOMTEST"; // ' Path to TraceLogger directory from My Documents
            const string TRACE_LOGGER_FILENAME_BASE = @"\Logs "; // ' Fixed part of TraceLogger file name.  Note: The trailing space must be retained!
            const string TRACE_LOGGER_FILE_NAME_DATE_FORMAT = @"yyyy-MM-dd";
            const string TRACE_LOGGER_SYSTEM_PATH = @"\ASCOM\SystemLogs"; // ' Location where "System" user logs will be placed

            try
            {
                string exeName = Assembly.GetExecutingAssembly().Location;
                Console.WriteLine($"Stand alone tester assembly location: # {exeName} #");

                string exePath = Path.GetDirectoryName(exeName);
                Console.WriteLine($"Stand alone tester is running in directory: # {exePath} #");

                SAL = new StandAloneLogger(exePath);

                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"); // Open the OS version registry key
                string productName = regKey.GetValue("ProductName", "").ToString();
                string currentMajorVersionNumber = regKey.GetValue("CurrentMajorVersionNumber", "").ToString();
                string currentMinorVersionNumber = regKey.GetValue("CurrentMinorVersionNumber", "").ToString();
                string currentType = regKey.GetValue("CurrentType", "").ToString();
                string currentBuildNumber = regKey.GetValue("currentBuildNumber", "").ToString();
                string ubr = regKey.GetValue("UBR", "").ToString();

                SAL.LogMessage("OS Version", $"{productName} {currentType} {currentMajorVersionNumber}.{currentMinorVersionNumber}.{currentBuildNumber}.{ubr}");




                SAL.LogMessage("Environment", $"Documents folder: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                SAL.LogMessage("Environment", $"Variable HOMEDRIVE: {Environment.GetEnvironmentVariable("HOMEDRIVE")}");
                SAL.LogMessage("Environment", $"Variable HOMEPATH: {Environment.GetEnvironmentVariable("HOMEPATH")}");
                SAL.LogMessage("Environment", $"Variable OneDrive: {Environment.GetEnvironmentVariable("OneDrive")}");
                SAL.LogMessage("Environment", $"Variable OneDriveConsumer: {Environment.GetEnvironmentVariable("OneDriveConsumer")}");
                SAL.LogMessage("Environment", $"Variable TEMP: {Environment.GetEnvironmentVariable("TEMP")}");
                SAL.LogMessage("Environment", $"Variable TMP: {Environment.GetEnvironmentVariable("TMP")}");
                SAL.LogMessage("Environment", $"Variable HOMEPATH: {Environment.GetEnvironmentVariable("HOMEPATH")}");
                SAL.LogMessage("Environment", $"Variable USERDOMAIN: {Environment.GetEnvironmentVariable("USERDOMAIN")}");
                SAL.LogMessage("Environment", $"Variable USERDOMAIN_ROAMINGPROFILE: {Environment.GetEnvironmentVariable("USERDOMAIN_ROAMINGPROFILE")}");
                SAL.LogMessage("Environment", $"Variable USERNAME: {Environment.GetEnvironmentVariable("USERNAME")}");
                SAL.LogMessage("Environment", $"Variable USERPROFILE: {Environment.GetEnvironmentVariable("USERPROFILE")}");
                SAL.LogMessage("Environment", $"Variable HOMEPATH: {Environment.GetEnvironmentVariable("HOMEPATH")}");

                // Get the correct log file path depending on whether we are running as the "System" user that has no documents folder or a regular user who does
                if (String.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))) //' We are running as the "System" user
                {
                    g_DefaultLogFilePath = $"{GetCommonProgramFilesx86()}{TRACE_LOGGER_SYSTEM_PATH}{TRACE_LOGGER_FILENAME_BASE}{DateTime.Now.ToString(TRACE_LOGGER_FILE_NAME_DATE_FORMAT)}";
                }
                else //' We are running as a normal user
                {
                    g_DefaultLogFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{TRACE_LOGGER_PATH}{TRACE_LOGGER_FILENAME_BASE}{DateTime.Now.ToString(TRACE_LOGGER_FILE_NAME_DATE_FORMAT)}";
                }

                SAL.LogMessage("TraceLogger", $"Default path: {g_DefaultLogFilePath}");
                SAL.LogMessage("TraceLogger", $"");


                try
                {
                    SAL.LogMessage("TraceLogger", $"Setting registry ACL");
                    SetRegistryACL();
                    SAL.LogMessage("TraceLogger", $"Set registry ACL");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occurred when setting the registry");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("");
                }
                SAL.LogMessage("TraceLogger", $"");


                SAL.LogMessage("TraceLogger", $"Creating Log file");
                CreateLogFile();
                SAL.LogMessage("TraceLogger", $"Log file created OK");
                SAL.LogMessage("TraceLogger", $"Writing log messages");

                LogMessage("Test", "Message 1", false);
                LogMessage("Test", "Message 2", false);
                LogMessage("Test", "Message 3", false);

                SAL.LogMessage("TraceLogger", $"Completed writing log messages OK");
                SAL.LogMessage("TraceLogger", $"Disposing of  logger");

                LogFileDispose();

                SAL.LogMessage("TraceLogger", $"Logger disposed OK");
                SAL.LogMessage("TraceLogger", $"");
                SAL.LogMessage("TraceLogger", $"Tests complete");

                SAL.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred");
                Console.WriteLine("");
                Console.WriteLine(ex);
                Console.WriteLine("");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        internal static string GetCommonProgramFilesx86()
        {
            if ((IntPtr.Size == 8) | (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                return Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
            else
                return Environment.GetEnvironmentVariable("CommonProgramFiles");
        }

        private static void CreateLogFile()
        {
            int FileNameSuffix = 0;
            bool ok = false;
            string FileNameBase;

            //My.Computer.FileSystem.CreateDirectory(g_DefaultLogFilePath); // Create the directory if it doesn't exist
            SAL.LogMessage("CreateLogFile", "Creating directory: {g_DefaultLogFilePath}");

            DirectoryInfo dirInfo = Directory.CreateDirectory(g_DefaultLogFilePath);
            SAL.LogMessage("CreateLogFile", "Created directory OK: {dirInfo.FullName}");

            FileNameBase = g_DefaultLogFilePath + @"\ASCOM." + g_LogFileType + "." + DateTime.Now.ToString("HHmm.ssfff");
            SAL.LogMessage("CreateLogFile", "File name base: {FileNameBase}");

            do
            {
                g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                FileNameSuffix += 1;
            }
            while (!(!File.Exists(g_LogFileActualName))); // Increment counter that ensures that no log file can have the same name as any other
            SAL.LogMessage("CreateLogFile", "Actual file name base: {g_LogFileActualName}");

            try
            {
                SAL.LogMessage("CreateLogFile", "About to open stream writer on file: {g_LogFileActualName}");
                g_LogFile = new StreamWriter(g_LogFileActualName, false);
                g_LogFile.AutoFlush = true;
                SAL.LogMessage("CreateLogFile", "File opened OK: {g_LogFileActualName}");
            }
            catch (IOException ex)
            {
                SAL.LogMessage("CreateLogFile", "CAUGHT AN EXCEPTION OPENING: {g_LogFileActualName}");
                SAL.LogMessage("CreateLogFile", ex.ToString());

                ok = false;
                do
                {
                    try
                    {
                        g_LogFileActualName = FileNameBase + FileNameSuffix.ToString() + ".txt";
                        SAL.LogMessage("CreateLogFile", "About to open stream writer on file: {g_LogFileActualName}");
                        g_LogFile = new StreamWriter(g_LogFileActualName, false);
                        g_LogFile.AutoFlush = true;
                        SAL.LogMessage("CreateLogFile", "File opened OK: {g_LogFileActualName}");
                        ok = true;
                    }
                    catch (IOException)
                    {
                        SAL.LogMessage("CreateLogFile", "CAUGHT AN EXCEPTION OPENING: {g_LogFileActualName}");
                        SAL.LogMessage("CreateLogFile", ex.ToString());
                    }

                    FileNameSuffix += 1;
                }
                while (!(ok | (FileNameSuffix == 20)));

                if (!ok)
                    throw new Exception("TraceLogger:CreateLogFile - Unable to create log file", ex);
            }
            SAL.LogMessage("CreateLogFile", "Completed");

        }

        public static void LogMessage(string Identifier, string Message, bool HexDump)
        {
            string Msg = Message;
            SAL.LogMessage("LogMessage", $"Logging message: {Identifier} {Message}");

            if (g_LogFile == null) CreateLogFile();
            LogMsgFormatter(Identifier, Msg, true, false);
        }

        private static void LogMsgFormatter(string p_Test, string p_Msg, bool p_NewLine, bool p_RespectCrLf)
        {
            string l_Msg = "";
            SAL.LogMessage("LogMsgFormatter", $"Logging message: {p_Test} {p_Msg}");

            p_Test = (p_Test + new string(' ', g_IdentifierWidth)).Substring(0, g_IdentifierWidth);

            l_Msg = DateTime.Now.ToString("HH:mm:ss.fff") + " " + p_Test + " " + p_Msg;
            if (g_LogFile != null)
            {
                if (p_NewLine)
                {
                    g_LogFile.WriteLine(l_Msg); // Update log file with newline terminator
                }
                else
                {
                    g_LogFile.Write(l_Msg);// Update log file without newline terminator
                }

                g_LogFile.Flush();
            }
            SAL.LogMessage("LogMsgFormatter", $"Completed");
        }

        public static void LogFileDispose()
        {
            SAL.LogMessage("LogFileDispose", $"Starting");
            g_LogFile.Flush();
            g_LogFile.Close();
            g_LogFile.Dispose();
            SAL.LogMessage("LogFileDispose", $"Completed");
        }

        internal static void SetRegistryACL() // ByVal CurrentPlatformVersion As String)
        {
            // Subroutine to control the migration of a Platform 5.5 profile to Platform 6
            System.Collections.Generic.SortedList<string, string> Values = new System.Collections.Generic.SortedList<string, string>();
            Stopwatch swLocal;
            RegistryKey Key;
            RegistrySecurity KeySec;
            RegistryAccessRule RegAccessRule;
            SecurityIdentifier DomainSid, Ident;
            AuthorizationRuleCollection RuleCollection;

            swLocal = Stopwatch.StartNew();

            // Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key

            SAL.LogMessage("SetRegistryACL", "Creating security identifier");
            DomainSid = new SecurityIdentifier("S-1-0-0"); // Create a starting point domain SID
            Ident = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid); // Create a security Identifier for the BuiltinUsers Group to be passed to the new access rule

            SAL.LogMessage("SetRegistryACL", "Creating FullControl ACL rule");
            RegAccessRule = new RegistryAccessRule(Ident, RegistryRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow); // Create the new access permission rule

            SAL.LogMessage("SetRegistryACL", @"Creating root ASCOM key ""\""");
            //Key = OpenSubKey3264(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, true, RegWow64Options.KEY_WOW64_32KEY); // Always create the key in the 32bit portion of the registry for backward compatibility
            Key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\ASCOM", true);

            SAL.LogMessage("SetRegistryACL", "Retrieving ASCOM key ACL rule");
            KeySec = Key.GetAccessControl(); // Get existing ACL rules on the key 
            SAL.LogMessage("SetRegistryACL", "Access Rules are Canonical before adding full access rule: " + KeySec.AreAccessRulesCanonical);

            RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules
            foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
            {
                try
                {
                    SAL.LogMessage("SetRegistryACL Before", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
                }
                catch (Exception ex)
                {
                    SAL.LogMessage("SetRegistryACL Before", ex.ToString());
                }
            }
            // Ensure that the ACLs are canonical before writing them back.
            // key.GetAccessControl sometimes provides ACLs in a non-canonical format, which causes failure when we write them back - UGH!

            SAL.LogMessage("SetRegistryACL", $"Access rules on the ASCOM profile key are canonical: {KeySec.AreAccessRulesCanonical}");

            SAL.LogMessage("SetRegistryACL", "Adding new ACL rule");
            KeySec.AddAccessRule(RegAccessRule); // Add the new rule to the existing rules
            SAL.LogMessage("SetRegistryACL", "Access Rules are Canonical after adding full access rule: " + KeySec.AreAccessRulesCanonical);

            RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules after adding the new one
            foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
            {
                try
                {
                    SAL.LogMessage("SetRegistryACL After", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
                }
                catch (Exception ex)
                {
                    SAL.LogMessage("SetRegistryACL After", ex.ToString());
                }
            }

            SAL.LogMessage("SetRegistryACL", "Applying new ACL rule to the Profile key");
            Key.SetAccessControl(KeySec); // Apply the new rules to the Profile key

            SAL.LogMessage("SetRegistryACL", "Flushing key");
            Key.Flush(); // Flush the key to make sure the permission is committed
            SAL.LogMessage("SetRegistryACL", "Closing key");
            Key.Close(); // Close the key after migration

            swLocal.Stop(); SAL.LogMessage("SetRegistryACL", "ElapsedTime " + swLocal.ElapsedMilliseconds + " milliseconds");
        }







    }
}
