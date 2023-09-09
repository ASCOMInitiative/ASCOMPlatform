using System;
using System.Collections.Generic;
using System.Diagnostics;
// Class to read and write profile values to the registry

using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace UninstallAscom
{

    internal class RegistryAccess : IDisposable
    {
        internal const string REGISTRY_ROOT_KEY_NAME = @"SOFTWARE\ASCOM"; // Location of ASCOM profile in HKLM registry hive

        private RegistryKey ProfileRegKey;

        private TraceLogger TL;
        private bool disposedValue = false;        // To detect redundant calls to IDisposable
        private bool DisableTLOnExit = true;

        private Stopwatch sw, swSupport;

        /// <summary>
        /// Enum containing all the possible registry access rights values. The built-in RegistryRights enum only has a partial collection
        /// and often returns values such as -1 or large positive and negative integer values when converted to a string
        /// The Flags attribute ensures that the ToString operation returns an aggregate list of discrete values
        /// </summary>
        [Flags()]
        public enum AccessRights
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
            GenericRead = int.MinValue + 0x00000000
        }

        #region New and IDisposable Support

        public RegistryAccess(TraceLogger TraceLoggerToUse)
        {
            // This initiator is called by UninstallASCOM in order to pass in its TraceLogger so that all output appears in one file
            TL = TraceLoggerToUse;
            DisableTLOnExit = false;

            sw = new Stopwatch(); // Create the stopwatch instances
            swSupport = new Stopwatch();

            ProfileRegKey = null;
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    sw.Stop();
                }
                catch
                {
                } // Clean up the stopwatches
                try
                {
                    sw = null;
                }
                catch
                {
                }
                try
                {
                    swSupport.Stop();
                }
                catch
                {
                }
                try
                {
                    swSupport = null;
                }
                catch
                {
                }
                try
                {
                    ProfileRegKey.Close();
                }
                catch
                {
                }

                try
                {
                    ProfileRegKey.Close();
                }
                catch
                {
                }
                try
                {
                    ProfileRegKey = null;
                }
                catch
                {
                }

                if (DisableTLOnExit)
                {
                    try
                    {
                        TL.Enabled = false;
                    }
                    catch
                    {
                    } // Clean up the logger
                    try
                    {
                        TL.Dispose();
                    }
                    catch
                    {
                    }
                    try
                    {
                        TL = null;
                    }
                    catch
                    {
                    }
                }
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
        }

        #endregion


        #region Support Functions

        // log messages and send to screen when appropriate
        private void LogMessage(string section, string logMessage)
        {
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            Global.LogEvent(section, logMessage, EventLogEntryType.Information, EventLogErrors.MigrateProfileRegistryKey, "");
        }

        // log error messages and send to screen when appropriate
        private void LogError(string section, string logMessage)
        {
            TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            Global.LogEvent(section, "Exception", EventLogEntryType.Error, EventLogErrors.MigrateProfileRegistryKey, logMessage);
        }

        internal void ListRegistryACLs(RegistryKey Key, string Description)
        {
            RegistrySecurity KeySec;
            AuthorizationRuleCollection RuleCollection;

            LogMessage("ListRegistryACLs", Description + ", Key: " + Key.Name);
            KeySec = Key.GetAccessControl(); // Get existing ACL rules on the key 

            RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules

            foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
                LogMessage("ListRegistryACLs", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited?"Inherited":"NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
            TL.BlankLine();
        }


        internal void SetRegistryACL() // ByVal CurrentPlatformVersion As String)
        {
            // Subroutine to control the migration of a Platform 5.5 profile to Platform 6
            var Values = new SortedList<string, string>();
            Stopwatch swLocal;
            RegistryKey Key;
            RegistrySecurity KeySec;
            RegistryAccessRule RegAccessRule;
            SecurityIdentifier DomainSid, Ident;
            AuthorizationRuleCollection RuleCollection;

            swLocal = Stopwatch.StartNew();

            // Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key

            LogMessage("SetRegistryACL", "Creating security identifier");
            DomainSid = new SecurityIdentifier("S-1-0-0"); // Create a starting point domain SID
            Ident = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid); // Create a security Identifier for the BuiltinUsers Group to be passed to the new access rule

            LogMessage("SetRegistryACL", "Creating FullControl ACL rule");
            RegAccessRule = new RegistryAccessRule(Ident, RegistryRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow); // Create the new access permission rule

            // List the ACLs on the standard registry keys before we do anything
            LogMessage("SetRegistryACL", "Listing base key ACLs");

            if (Environment.Is64BitProcess)
            {
                LogMessage("SetRegistryACL", "Listing base key ACLs in 64bit mode");
                ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT");
                ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE"), @"HKEY_LOCAL_MACHINE\SOFTWARE");
                ListRegistryACLs(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft"), @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft");
                ListRegistryACLs(OpenSubKey3264(RegistryHive.LocalMachine, "SOFTWARE", true, RegWow64Options.KEY_WOW64_64KEY), @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node");
                ListRegistryACLs(OpenSubKey3264(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft", true, RegWow64Options.KEY_WOW64_32KEY), @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft");
            }
            else
            {
                LogMessage("SetRegistryACL", "Listing base key ACLS in 32bit mode");
                ListRegistryACLs(Registry.ClassesRoot, "HKEY_CLASSES_ROOT");
                ListRegistryACLs(Registry.LocalMachine.OpenSubKey("SOFTWARE"), @"HKEY_LOCAL_MACHINE\SOFTWARE");
                ListRegistryACLs(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft"), @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft");
            }

            LogMessage("SetRegistryACL", @"Creating root ASCOM key ""\""");
            Key = OpenSubKey3264(RegistryHive.LocalMachine, REGISTRY_ROOT_KEY_NAME, true, RegWow64Options.KEY_WOW64_32KEY); // Always create the key in the 32bit portion of the registry for backward compatibility

            LogMessage("SetRegistryACL", "Retrieving ASCOM key ACL rule");
            TL.BlankLine();
            KeySec = Key.GetAccessControl(); // Get existing ACL rules on the key 

            RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules
            foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
            {
                try
                {
                    LogMessage("SetRegistryACL Before", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
                }
                catch (Exception ex) // Report but ignore errors when logging information
                {
                    LogMessage("SetRegistryACL Before", ex.ToString());
                }
            }
            // Ensure that the ACLs are canonical before writing them back.
            // key.GetAccessControl sometimes provides ACLs in a non-canonical format, which causes failure when we write them back - UGH!

            TL.BlankLine();
            if (KeySec.AreAccessRulesCanonical)
            {
                LogMessage("SetRegistryACL", "Current access rules on the ASCOM profile key are canonical, no fix-up action required");
            }
            else
            {
                LogMessage("SetRegistryACL", "***** Current access rules on the ASCOM profile key are NOT canonical, fixing them");
                CanonicalizeDacl(KeySec); // Ensure that the ACLs are canonical
                LogMessage("SetRegistryACL", "Access Rules are Canonical after fix: " + KeySec.AreAccessRulesCanonical);

                // List the rules post canonicalisation
                RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules
                foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
                {
                    try
                    {
                        LogMessage("SetRegistryACL Canon", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
                    }
                    catch (Exception ex) // Report but ignore errors when logging information
                    {
                        LogMessage("SetRegistryACL Canon", ex.ToString());
                    }
                }

            }
            TL.BlankLine();

            LogMessage("SetRegistryACL", "Adding new ACL rule");
            KeySec.AddAccessRule(RegAccessRule); // Add the new rule to the existing rules
            LogMessage("SetRegistryACL", "Access Rules are Canonical after adding full access rule: " + KeySec.AreAccessRulesCanonical);
            TL.BlankLine();
            RuleCollection = KeySec.GetAccessRules(true, true, typeof(NTAccount)); // Get the access rules after adding the new one
            foreach (RegistryAccessRule RegRule in RuleCollection) // Iterate over the rule set and list them
            {
                try
                {
                    LogMessage("SetRegistryACL After", RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + ((AccessRights)RegRule.RegistryRights).ToString() + " " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " " + RegRule.InheritanceFlags.ToString() + " " + RegRule.PropagationFlags.ToString());
                }
                catch (Exception ex) // Report but ignore errors when logging information
                {
                    LogMessage("SetRegistryACL After", ex.ToString());
                }
            }

            TL.BlankLine();
            LogMessage("SetRegistryACL", "Applying new ACL rule to the Profile key");
            Key.SetAccessControl(KeySec); // Apply the new rules to the Profile key

            LogMessage("SetRegistryACL", "Flushing key");
            Key.Flush(); // Flush the key to make sure the permission is committed
            LogMessage("SetRegistryACL", "Closing key");
            Key.Close(); // Close the key after migration

            swLocal.Stop();
            LogMessage("SetRegistryACL", "ElapsedTime " + swLocal.ElapsedMilliseconds + " milliseconds");
        }


        internal void CanonicalizeDacl(RegistrySecurity objectSecurity)
        {

            // A canonical ACL must have ACES sorted according to the following order:
            // 1. Access-denied on the object
            // 2. Access-denied on a child or property
            // 3. Access-allowed on the object
            // 4. Access-allowed on a child or property
            // 5. All inherited ACEs 
            var descriptor = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access));

            var implicitDenyDacl = new List<CommonAce>();
            var implicitDenyObjectDacl = new List<CommonAce>();
            var inheritedDacl = new List<CommonAce>();
            var implicitAllowDacl = new List<CommonAce>();
            var implicitAllowObjectDacl = new List<CommonAce>();
            int aceIndex = 0;
            var newDacl = new RawAcl(descriptor.DiscretionaryAcl.Revision, descriptor.DiscretionaryAcl.Count);
            int count = 0; // Counter for ACEs as they are processed

            try
            {

                if (objectSecurity is null)
                {
                    throw new ArgumentNullException("objectSecurity");
                }
                if (objectSecurity.AreAccessRulesCanonical)
                {
                    LogMessage("CanonicalizeDacl", "Rules are already canonical, no action taken");
                    return;
                }

                TL.BlankLine();
                LogMessage("CanonicalizeDacl", "***** Rules are not canonical, restructuring them *****");
                TL.BlankLine();

                foreach (CommonAce ace in descriptor.DiscretionaryAcl)
                {
                    count += 1;
                    LogMessage("CanonicalizeDacl", "Processing ACE " + count);

                    if ((ace.AceFlags & AceFlags.Inherited) == AceFlags.Inherited)
                    {
                        try
                        {
                            LogMessage("CanonicalizeDacl", "Found Inherited Ace");
                            LogMessage("CanonicalizeDacl", "Found Inherited Ace,                  " + (ace.AceType == AceType.AccessAllowed ? "Allow" : "Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                            inheritedDacl.Add(ace);
                        }
                        catch (Exception ex1)
                        {
                            LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan");
                            LogError("CanonicalizeDacl", ex1.ToString());
                        }
                    }
                    else
                    {
                        switch (ace.AceType)
                        {
                            case AceType.AccessAllowed:
                                {
                                    try
                                    {
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Allow");
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace,               Allow: " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                                        implicitAllowDacl.Add(ace);
                                    }
                                    catch (IdentityNotMappedException ex1)
                                    {
                                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan");
                                        LogError("CanonicalizeDacl", ex1.ToString());
                                    }
                                    break;
                                }

                            case AceType.AccessDenied:
                                {
                                    try
                                    {
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace Deny");
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace,                Deny: " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                                        implicitDenyDacl.Add(ace);
                                    }
                                    catch (IdentityNotMappedException ex1)
                                    {
                                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan");
                                        LogError("CanonicalizeDacl", ex1.ToString());
                                    }
                                    break;
                                }

                            case AceType.AccessAllowedObject:
                                {
                                    try
                                    {
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object Allow");
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object        Allow:" + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                                        implicitAllowObjectDacl.Add(ace);
                                    }
                                    catch (IdentityNotMappedException ex1)
                                    {
                                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan");
                                        LogError("CanonicalizeDacl", ex1.ToString());
                                    }
                                    break;
                                }

                            case AceType.AccessDeniedObject:
                                {
                                    try
                                    {
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object Deny");
                                        LogMessage("CanonicalizeDacl", "Found NotInherited Ace, Object         Deny: " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                                        implicitDenyObjectDacl.Add(ace);
                                        break;
                                    }
                                    catch (IdentityNotMappedException ex1)
                                    {
                                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this ACE because it is an orphan");
                                        LogError("CanonicalizeDacl", ex1.ToString());
                                    }

                                    break;
                                }
                        }
                    }
                }

                TL.BlankLine();
                LogMessage("CanonicalizeDacl", "Rebuilding in correct order...");
                TL.BlankLine();

                // List the number of entries in each category
                LogMessage("CanonicalizeDacl", string.Format("There are {0} Implicit Deny ACLs", implicitDenyDacl.Count));
                LogMessage("CanonicalizeDacl", string.Format("There are {0} Implicit Deny Object ACLs", implicitDenyObjectDacl.Count));
                LogMessage("CanonicalizeDacl", string.Format("There are {0} Implicit Allow ACLs", implicitAllowDacl.Count));
                LogMessage("CanonicalizeDacl", string.Format("There are {0} Implicit Allow Object ACLs", implicitAllowObjectDacl.Count));
                LogMessage("CanonicalizeDacl", string.Format("There are {0} Inherited ACLs", inheritedDacl.Count));
                TL.BlankLine();

                // Rebuild the access list in the correct order
                foreach (CommonAce ace in implicitDenyDacl)
                {
                    try
                    {
                        LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Deny Ace");
                        newDacl.InsertAce(aceIndex, ace);
                        aceIndex += 1;
                        LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Deny Ace,         " + (ace.AceType == AceType.AccessAllowed ? "Allow" : " Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                    }
                    catch (IdentityNotMappedException ex1)
                    {
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE");
                        LogError("CanonicalizeDacl", ex1.ToString());
                    }
                }

                foreach (CommonAce ace in implicitDenyObjectDacl)
                {
                    try
                    {
                        LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Deny Object Ace");
                        newDacl.InsertAce(aceIndex, ace);
                        aceIndex += 1;
                        LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Deny Object Ace,  " + (ace.AceType == AceType.AccessAllowed ? "Allow" : " Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                    }
                    catch (IdentityNotMappedException ex1)
                    {
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE");
                        LogError("CanonicalizeDacl", ex1.ToString());
                    }
                }

                foreach (CommonAce ace in implicitAllowDacl)
                {
                    try
                    {
                        LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Allow Ace");
                        newDacl.InsertAce(aceIndex, ace);
                        aceIndex += 1;
                        LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Allow Ace,        " + (ace.AceType == AceType.AccessAllowed ? "Allow" : " Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                    }
                    catch (IdentityNotMappedException ex1)
                    {
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE");
                        LogError("CanonicalizeDacl", ex1.ToString());
                    }
                }

                foreach (CommonAce ace in implicitAllowObjectDacl)
                {
                    try
                    {
                        LogMessage("CanonicalizeDacl", "Adding NonInherited Implicit Allow Object Ace");
                        newDacl.InsertAce(aceIndex, ace);
                        aceIndex += 1;
                        LogMessage("CanonicalizeDacl", "Added NonInherited Implicit Allow Object Ace, " + (ace.AceType == AceType.AccessAllowed ? "Allow" : " Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                    }
                    catch (IdentityNotMappedException ex1)
                    {
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE");
                        LogError("CanonicalizeDacl", ex1.ToString());
                    }
                }

                foreach (CommonAce ace in inheritedDacl)
                {
                    try
                    {
                        LogMessage("CanonicalizeDacl", "Adding Inherited Ace");
                        newDacl.InsertAce(aceIndex, ace);
                        aceIndex += 1;
                        LogMessage("CanonicalizeDacl", "Added Inherited Ace,                 " + (ace.AceType == AceType.AccessAllowed ? "Allow" : " Deny") + ": " + ace.SecurityIdentifier.Translate(Type.GetType("System.Security.Principal.NTAccount")).ToString() + " " + ((AccessRights)ace.AccessMask).ToString().ToString() + " " + ace.AceFlags.ToString());
                    }
                    catch (IdentityNotMappedException ex1)
                    {
                        LogError("CanonicalizeDacl", "IdentityNotMappedException exception, ignoring this error because it says that this is an orphan ACE");
                        LogError("CanonicalizeDacl", ex1.ToString());
                    }
                }

                // If aceIndex <> descriptor.DiscretionaryAcl.Count Then
                // System.Diagnostics.Debug.Fail("The DACL cannot be canonicalized since it would potentially result in a loss of information")
                // Return
                // End If

                descriptor.DiscretionaryAcl = newDacl;
                objectSecurity.SetSecurityDescriptorSddlForm(descriptor.GetSddlForm(AccessControlSections.Access), AccessControlSections.Access);
                LogMessage("CanonicalizeDacl", "New ACL successfully applied to registry key.");
            }
            catch (Exception ex2)
            {
                LogError("CanonicalizeDacl", "Unexpected exception");
                LogError("CanonicalizeDacl", ex2.ToString());
                throw;
            }
        }

        #region 32/64bit registry access code
        // There is a better way to achieve a 64bit registry view in framework 4. 
        // Only OpenSubKey is used in the code, the rest of these routines are support for OpenSubKey.
        // OpenSubKey should be replaced with Microsoft.Win32.RegistryKey.OpenBaseKey method

        // <Obsolete("Replace with Microsoft.Win32.RegistryKey.OpenBaseKey method in Framework 4", False)> _
        internal RegistryKey OpenSubKey3264(RegistryHive ParentKey, string SubKeyName, bool Writeable, RegWow64Options Options)
        {
            int SubKeyHandle;
            var Result = default(int);
            RegistryKey resultKey;

            TL.LogMessage("OpenSubKey3264", $"Hive: {ParentKey}, SubKey: {SubKeyName}, Writeable: {Writeable}, Options: {Options}, Application: {(Environment.Is64BitProcess?"64bit":"32bit")}");

            // Handle the various registry hives
            switch (ParentKey)
            {
                case RegistryHive.ClassesRoot: // HKEY classes root
                    {
                        // Test whether the application is 32bit or 64bit
                        if (Environment.Is64BitProcess) // 64bit application
                        {
                            // Force access to the 32bit portion of the registry (different on a 64bit machine from the 64bit portion of the registry)
                            resultKey = Registry.ClassesRoot.OpenSubKey($@"WOW6432Node\{SubKeyName}", Writeable);
                        }

                        else // 32bit application
                        {
                            // Access the 32bit portion of the registry (only one version because 32bit only)
                            resultKey = Registry.ClassesRoot.OpenSubKey(SubKeyName, Writeable);
                        }

                        break;
                    }

                case RegistryHive.LocalMachine: // HKEY local machine
                    {
                        // Test whether the application is 32bit or 64bit
                        if (Environment.Is64BitProcess) // 64bit application
                        {
                            // Check whether we are accessing the SOFTWARE key
                            string cleanedKey = SubKeyName.Trim().TrimStart('\\'); // Remove leading and trailing spaces and leading \ characters

                            if (cleanedKey.StartsWith("SOFTWARE", StringComparison.OrdinalIgnoreCase)) // We need to access the 32bit version of the SOFTWARE key 
                            {
                                // Force access to the 32bit portion of the registry (different on a 64bit machine from the 64bit portion of the registry)
                                // extract any sub-key that follows SOFTWARE
                                if (cleanedKey.Length > 8) // There is something after the key SOFTWARE
                                {
                                    cleanedKey = $@"SOFTWARE\WOW6432Node\{cleanedKey.Substring(8)}";
                                }
                                else
                                {
                                    cleanedKey = $@"SOFTWARE\WOW6432Node\";
                                }
                                TL.LogMessage("OpenSubKey3264", $"Cleaned key: {cleanedKey}");

                                resultKey = Registry.LocalMachine.OpenSubKey(cleanedKey, Writeable);
                            }

                            else
                            {
                                resultKey = Registry.LocalMachine.OpenSubKey(SubKeyName, Writeable);
                            }
                        }

                        else // 32bit application
                        {
                            // Access the 32bit portion of the registry (only one version because 32bit only)
                            resultKey = Registry.LocalMachine.OpenSubKey(SubKeyName, Writeable);
                        } // Other hives are not supported

                        break;
                    }

                default:
                    {
                        throw new Exception($@"RegistryAccess - OpenSubKey3264 - The {ParentKey} hive is not yet supported.
");
                    }
            }

            // Test whether the sub-key was found
            if (resultKey is null) // Sub-key not found so throw an exception
            {
                throw new Exception("Cannot open key " + SubKeyName + " as it does not exist - Result: 0x" + Result.ToString("X8"));
            }

            // Sub-key found so return it
            return resultKey;
        }

        internal enum RegWow64Options : int
        {
            // Basis from: http://www.pinvoke.net/default.aspx/advapi32/RegOpenKeyEx.html
            None = 0,
            KEY_WOW64_64KEY = 0x100,
            KEY_WOW64_32KEY = 0x200
        }

        #endregion

        #endregion

    }
}