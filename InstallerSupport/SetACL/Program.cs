using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Utilities;

namespace SetACL
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

        static TraceLogger TL;
        static RegistryAccess regAccess;
        static int returnCode = 0;

        static int Main(string[] args)
        {
            if (args[0].ToUpperInvariant() == "/SETPROFILEACL") // Set and check the Profile registry ACL
            {
                try
                {
                    TL = new TraceLogger("SetProfileACL"); // Create a trace logger so we can log what happens
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
                Global.LogEvent("UninstallAscom", logMessage, EventLogEntryType.Information, EventLogErrors.UninstallASCOMInfo, "");
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
                Global.LogEvent("UninstallAscom", "Exception", EventLogEntryType.Error, EventLogErrors.UninstallASCOMError, logMessage);
            }
            catch { }
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
    }
}
