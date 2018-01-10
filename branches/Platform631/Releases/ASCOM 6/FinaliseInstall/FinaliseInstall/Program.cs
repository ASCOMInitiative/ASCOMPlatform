using System;
using System.IO;
using System.Security.Permissions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Management;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ConsoleApplication1
{
    class Program
    {

        [DllImport("ole32.dll")]
        static extern int CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid pclsid);

        static private Type tProfile; // Late bound Helper.Profile type
        static private object oProfile; // Late bound Helper.Profile
        static private TraceLogger TL; // Trace logger
        private const string sMsgTitle = "ASCOM Platform 6 Install Finaliser";
        static private int ReturnCode = 0; // Code to return to the calling application

        // SID constants for well known accounts and groups
        private const string CreatorOwnerSID = "S-1-3-0";
        private const string NTAuthoritySystemSID = "S-1-5-18";
        private const string BuiltInAdministratorsSID = "S-1-5-32-544";
        private const string BuiltInUsersSID = "S-1-5-32-545";

        // Readonly and full registry access rights lists. These are as implemented by Windows after an "out of the box" install.
        static private AccessRights FullRights = AccessRights.Query |
                                                 AccessRights.SetKey |
                                                 AccessRights.CreateSubKey |
                                                 AccessRights.EnumSubkey |
                                                 AccessRights.Notify |
                                                 AccessRights.CreateLink |
                                                 AccessRights.StandardDelete |
                                                 AccessRights.StandardReadControl |
                                                 AccessRights.StandardWriteDAC |
                                                 AccessRights.StandardWriteOwner;

        static private AccessRights ReadRights = AccessRights.Query |
                                                 AccessRights.EnumSubkey |
                                                 AccessRights.Notify |
                                                 AccessRights.StandardReadControl;

        /// <summary>
        /// Enum containing all the possible registry access rights values. The buit-in RegistryRights enum only has a partial collection
        /// and often returns values such as -1 or large positive and negative integer values when converted to a string
        /// The Flags attribute ensures that the ToString operation returns an aggregate list of discrete values
        /// </summary>
        [Flags]
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

        static int Main(string[] args)
        {
            try
            {
                TL = new TraceLogger("", "FinaliseInstall"); // Create a tracelogger so we can log what happens
                TL.Enabled = true;

                LogMessage("FinaliseInstall", "Starting finalise process");

                try
                {

                    CheckHKCRPermissions(false); // Check that required pernissions are present on HKCR

                    tProfile = Type.GetTypeFromProgID("DriverHelper.Profile"); // Create a late bound Helper.Profile
                    if (tProfile == null) // Check whether the Type.GetTypeFromProgID method was successful
                    {
                        // Unable to get the Helper.Profile type, this usually occurs because of missing registry permissions on HKCR
                        // Report the error, restore standard permissions and try again to get the type

                        LogMessage("FinaliseInstall", "BAD - DriverHelper.Profile returned Type is null, attempting to fix registry permissions on HKCR!");
                        CheckHKCRPermissions(true); // Fix any missing pernissions on HKCR
                        LogMessage("FinaliseInstall", " ");
                        LogMessage("FinaliseInstall", "Making check that new permissions are OK");
                        CheckHKCRPermissions(false); // Confirm that the new permissions fix the issue
                        tProfile = Type.GetTypeFromProgID("DriverHelper.Profile"); // Try again to create a late bound Helper.Profile

                        if (tProfile == null)
                        {
                            // Bad news - again unable to create the type so log this as an error
                            LogError("FinaliseInstall", "DriverHelper.Profile returned Type is still null on second attmept!");
                        }
                        else
                        {
                            // Good news, successful on second attempt, registry permissions fixes worked!
                            LogMessage("FinaliseInstall", "Successfully got type for DriverHelper.Profile after fixing registry permissions");
                            oProfile = Activator.CreateInstance(tProfile); // Create the Profile object
                            LogMessage("FinaliseInstall", "Successfully created DriverHelper.Profile object");
                        }

                    }
                    else // Got the type OK!
                    {
                        LogMessage("FinaliseInstall", "Successfully got type for DriverHelper.Profile");
                        oProfile = Activator.CreateInstance(tProfile); // Create the Profile object
                        LogMessage("FinaliseInstall", "Successfully created DriverHelper.Profile object");
                    }
                }
                catch (Exception ex)
                {
                    SetReturnCode(6);
                    LogError("FinaliseInstall", "Exception 6: " + ex.ToString());
                }

                //
                // Force TypeLib linkages to PIAs (bug on repair & upgrades).
                //
                LogMessage("FinaliseInstall", "Fixing PIA Linkages");
                FixPiaLinkage("{12DC26BD-E8F7-4328-8A8B-B16C6D87A077}", "ASCOM.Helper");
                FixPiaLinkage("{55B16037-26C0-4F1E-A6DC-4C0E702D9FBE}", "ASCOM.Helper2");
                FixPiaLinkage("{76618F90-032F-4424-A680-802467A55742}", "ASCOM.Interfaces");

                //
                // Remove Platform 4.1 installer entry in Add/Remove Programs (TESTED)
                //
                LogMessage("FinaliseInstall", "Removing Platform 4.1 installer entries");
                RegistryKey rkAddRem = null;
                try
                {
                    rkAddRem = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall", RegistryKeyPermissionCheck.ReadWriteSubTree);

                    rkAddRem.DeleteSubKeyTree("ASCOM Platform 4.1");
                    rkAddRem.DeleteSubKeyTree("ASCOM Platform 4.0");
                }
                catch (Exception) { }
                finally { if (rkAddRem != null) 	rkAddRem.Close(); }

                //
                // Attempt to remove some old Platform Start Menu items
                // No special folder for all users start menu?
                //
                LogMessage("FinaliseInstall", "Cleaning old start menu items");
                string startPath = "C:\\Documents and Settings\\All Users\\Start Menu\\Programs\\ASCOM Platform";
                try
                {
                    File.Delete(startPath + "\\Other Drivers (web).url");
                    File.Delete(startPath + "\\RoboFocus Control.lnk");
                }
                catch (Exception) { }
                startPath = Environment.SpecialFolder.Programs + "\\ASCOM Platform";	// Try individual user too
                try
                {
                    File.Delete(startPath + "\\Other Drivers (web).url");
                    File.Delete(startPath + "\\RoboFocus Control.lnk");
                }
                catch (Exception) { }

                //
                // Set up some paths...
                //
                string ascomPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM";
                //string xmlProfPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ASCOM";

                //
                // Remove old Wise uninstaller and install log. It's weird, but this
                // stuff was installed in the Telescope subfolder. Always in program
                // files, common files, ASCOM\Telescope. 
                LogMessage("FinaliseInstall", "Removing old Wise installer debris");
                try
                {
                    File.Delete(ascomPath + "\\Telescope\\INSTALL.LOG");
                    File.Delete(ascomPath + "\\Telescope\\UNWISE.EXE");
                }
                catch (Exception) { }
                //
                // Register all of the simulators here, keeping MSI's dirty mits off them 
                // so they can be worked on and replaced with uupdates, etc.
                //
                // MIRROR CALLS IN UNINSTALL!
                //
                /*RegUnregExe(ascomPath + "\\Dome\\DomeSim.exe", true);
                RegUnregExe(ascomPath + "\\Dome\\ASCOMDome.exe", true);
                RegUnregExe(ascomPath + "\\Focuser\\FocusSim.exe", true);
                RegUnregExe(ascomPath + "\\Telescope\\ScopeSim.exe", true);
                RegUnregDll(ascomPath + "\\Camera\\CCDSimulator.dll", true);
                RegUnregExe(ascomPath + "\\Telescope\\POTH.exe", true);
                RegUnregExe(ascomPath + "\\Telescope\\Pipe.exe", true);
                RegUnregExe(ascomPath + "\\Telescope\\Hub.exe", true);
                RegUnregExe(ascomPath + "\\Switch\\SwitchSim.exe", true);
                */

                //
                // Add AppID stuff for EXE simulators Must be in commit because
                // these things must be registered now! Don't bother for Switch.
                //
                // MIRROR CALLS IN UNINSTALL!
                //
                LogMessage("FinaliseInstall", "Adding Appid information for EXE simulators");
                AddAppID("ScopeSim.Telescope", "ScopeSim.exe", "{4597A685-11FD-47ae-A5D2-A8DC54C90CDC}");
                AddAppID("FocusSim.Focuser", "FocusSim.exe", "{289BFF60-3B9E-417c-8DA1-F96773679342}");
                AddAppID("DomeSim.Dome", "DomeSim.exe", "{C47DAA29-5788-464e-BBA7-9711FBC7A01F}");
                AddAppID("POTH.Telescope", "POTH.exe", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
                AddAppID("POTH.Dome", "", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
                AddAppID("POTH.Focuser", "", "{0A21726F-E58B-4461-85A9-9AA739E6E42F}");
                AddAppID("Pipe.Telescope", "Pipe.exe", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
                AddAppID("Pipe.Focuser", "", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
                AddAppID("Pipe.Dome", "", "{080B23A0-3190-45e5-A7C8-53C61F22DFF2}");
                AddAppID("Hub.Telescope", "Hub.exe", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
                AddAppID("Hub.Focuser", "", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
                AddAppID("Hub.Dome", "", "{3213D452-2A8F-4624-9D65-E04E175E4CB2}");
                AddAppID("ASCOMDome.Telescope", "ASCOMDome.exe", "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}");
                AddAppID("ASCOMDome.Dome", "", "{B5863239-0A6E-48d4-A9EA-0DDA4D942390}");

                // New Platform 6 simulator exes - add Appid information
                AddAppID("ASCOM.Simulator.FilterWheel", "ASCOM.FilterWheelSim.exe", "{AE139A96-FF4D-4F22-A44C-141A9873E823}");
                AddAppID("ASCOM.Simulator.Rotator", "ASCOM.RotatorSimulator.exe", "{5D4BBF44-2573-401A-AEE1-F9716D0BAEC3}");
                AddAppID("ASCOM.Simulator.Telescope", "ASCOM.TelescopeSimulator.exe", "{1620DCB8-0352-4717-A966-B174AC868FA0}");

                //
                // Register VB6 simulators for ASCOM. I set these friendly names.
                //
                LogMessage("FinaliseInstall", "Registering ASCOMDome");
                RegAscom("ASCOMDome.Telescope", "ASCOM Dome Control");
                RegAscom("ASCOMDome.Dome", "ASCOM Dome Control");

                // This has been removed because it destroys the ability to remove 5.5 after use and does NOT restore all the 
                // Platform 5 files resulting in an unexpected automatic repair. Its left here just in case, Please DO NOT RE-ENABLE THIS FEATURE unless have a way round the resulting issues
                //FinaliseRestorePoint();


                //Clean up objects before close
                try
                {
                    int rc = 0;
                    int loopcount = 0;
                    do
                    {
                        rc = System.Runtime.InteropServices.Marshal.ReleaseComObject(oProfile);
                        loopcount += 1;
                        LogMessage("FinaliseInstall", "Releasing Profile object" + rc.ToString() + " " + loopcount.ToString());
                    } while ((rc > 0) & (loopcount < 10));

                }
                catch { }

                LogMessage("FinaliseInstall", "Completed finalise process, ReturnCode: " + ReturnCode.ToString());

                try
                {
                    TL.Enabled = false;
                    TL.Dispose();
                    TL = null;
                }
                catch { }
            }
            catch (Exception ex) //Exception in top level routine
            {
                SetReturnCode(4);
                LogError("FinaliseInstall", "Exception: " + ex.ToString());
            }

            return ReturnCode;
        }

        //
        // Set the return code to the first error that occurs
        //
        static void SetReturnCode(int RC)
        {
            if (ReturnCode == 0) ReturnCode = RC;
        }

        //
        // Add AppId stuff for the given ProgID
        // http://go.microsoft.com/fwlink/?linkid=32831
        //
        static private void AddAppID(string progID, string exeName, string sAPPID)
        {
            LogMessage("AddAppID", "ProgID: " + progID + ", ExeName: " + exeName + ", Appid: " + sAPPID);
            Guid gCLSID;
            int hr = CLSIDFromProgID(progID, out gCLSID);
            string sCLSID = "{" + new GuidConverter().ConvertToString(gCLSID) + "}";
            LogMessage("AddAppID", "  CLSID: " + sCLSID);

            RegistryKey rkCLSID = null;
            RegistryKey rkAPPID = null;
            RegistryKey rkAPPIDExe = null;
            try
            {
                rkCLSID = Registry.ClassesRoot.OpenSubKey(
                                "CLSID\\" + sCLSID,
                                RegistryKeyPermissionCheck.ReadWriteSubTree);
                rkCLSID.SetValue("AppId", sAPPID);
                rkAPPID = Registry.ClassesRoot.CreateSubKey(
                                "APPID\\" + sAPPID,
                                RegistryKeyPermissionCheck.ReadWriteSubTree);
                rkAPPID.SetValue("", rkCLSID.GetValue(""));				// Same description as class
                rkAPPID.SetValue("AppId", sAPPID);
                rkAPPID.SetValue("AuthenticationLevel", 3, RegistryValueKind.DWord);	// Changed to RPC_C_AUTHN_LEVEL_USER (3) from RPC_C_AUTHN_LEVEL_NONE (1) becuase of issue caused by Microsoft security update

                if (exeName != "")										// If want AppId\Exe.name
                {
                    rkAPPIDExe = Registry.ClassesRoot.CreateSubKey(
                                    "AppId\\" + exeName,
                                    RegistryKeyPermissionCheck.ReadWriteSubTree);
                    rkAPPIDExe.SetValue("", rkCLSID.GetValue(""));		// Same description as class
                    rkAPPIDExe.SetValue("AppId", sAPPID);
                }
                LogMessage("AddAppID", "  OK - Completed");
            }
            catch (Exception ex)
            {
                SetReturnCode(1);
                LogError("AddAppID", "Failed to add AppID info for " + progID + ": " + ex.ToString());
            }
            finally
            {
                if (rkCLSID != null) rkCLSID.Close();
                if (rkAPPID != null) rkAPPID.Close();
                if (rkAPPIDExe != null) rkAPPIDExe.Close();
            }
        }

        //
        // Remove AppId stuff for the given APPID
        // http://go.microsoft.com/fwlink/?linkid=32831
        //
        static private void RemAppID(string exeName, string sAPPID)
        {
            if (exeName == "" || sAPPID == "") return;						// Ohmygod exit

            RegistryKey rkAPPID = null;
            try
            {
                rkAPPID = Registry.ClassesRoot.OpenSubKey(
                                "APPID",
                                RegistryKeyPermissionCheck.ReadWriteSubTree);
                rkAPPID.DeleteSubKey(sAPPID);
                rkAPPID.DeleteSubKey(exeName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to remove AppID info for " + exeName + ": " + ex.Message,
                    sMsgTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (rkAPPID != null) rkAPPID.Close();
            }
        }

        //
        // Do COM (un)registration of EXE simulators here so they can be worked
        // on while the platform is installed. Letting MSI do this causes it
        // to lock out any work on the simulators!
        //
        static private void RegUnregExe(string ExePath, bool Register)
        {
            Process proc = null;
            try
            {
                proc = Process.Start(ExePath, (Register ? "/" : "/un") + "regserver");	// Works for .NET and VB6 localservers
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to " + (Register ? "" : "un") + "register " + Path.GetFileName(ExePath) + ": " + ex.Message,
                    sMsgTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally { if (proc != null) proc.Close(); }
        }

        //
        // Do COM (un)registration of DLL simulators here so they can be worked
        // on while the platform is installed. Letting MSI do this causes it
        // to lock out any work on the simulators!
        //
        static private void RegUnregDll(string DllPath, bool Register)
        {
            Process proc = null;
            try
            {
                proc = Process.Start("regsvr32.exe", (Register ? "-s " : "-u -s ") + "\"" + DllPath + "\"");
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to " + (Register ? "" : "un") + "register " + Path.GetFileName(DllPath) + ": " + ex.Message,
                    sMsgTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally { if (proc != null) proc.Close(); }
        }

        //
        // ASCOM Register/Unregister - Use Helper.Profile so that we're
        // protected against moving from registry to XML or ???
        //
        static private void RegAscom(string ProgID, string Desc)
        {
            string sType = ProgID.Substring(ProgID.IndexOf('.') + 1);
            LogMessage("RegAscom", "ProgID: " + ProgID + ", Description: " + Desc + ", DeviceType: " + sType);

            try
            {
                LogMessage("RegAscom", "  Setting device type");
                tProfile.InvokeMember("DeviceType",
                    BindingFlags.Default | BindingFlags.SetProperty,
                    null, oProfile, new object[] { sType });

                LogMessage("RegAscom", "  Registering device");
                tProfile.InvokeMember("Register",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, oProfile, new object[] { ProgID, Desc });
            }
            catch (Exception ex)
            {
                SetReturnCode(2);
                LogError("RegAscom", "Failed to register " + ProgID + "  for ASCOM: " + ex.ToString());
            }
        }

        static private void UnregAscom(string ProgID)
        {
            string sType = ProgID.Substring(ProgID.IndexOf('.') + 1);

            try
            {
                tProfile.InvokeMember("DeviceType",
                    BindingFlags.Default | BindingFlags.SetProperty,
                    null, oProfile, new object[] { sType });

                tProfile.InvokeMember("Unregister",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, oProfile, new object[] { ProgID });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to unregister " + ProgID + "  for ASCOM: " + ex.Message,
                    sMsgTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        static private void FixPiaLinkage(string LibID, string AssyClass)
        {
            LogMessage("FixPiaLinkage", "LibID: " + LibID + ", AssyClass: " + AssyClass);

            RegistryKey rkTLIB = null;
            try
            {
                rkTLIB = Registry.ClassesRoot.OpenSubKey(
                                "TypeLib\\" + LibID + "\\1.0",
                                RegistryKeyPermissionCheck.ReadWriteSubTree);
                rkTLIB.SetValue("PrimaryInteropAssemblyName",
                                AssyClass + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7");
                LogMessage("FixPiaLinkage", "  Fixed OK");
            }
            catch (Exception ex)
            {
                SetReturnCode(3);
                LogError("RegAscom", "Failed to fix up TypeLib for " + AssyClass + ": " + ex.ToString());
            }
            finally
            {
                if (rkTLIB != null) rkTLIB.Close();
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
                TL.LogMessageCrLf(section, logMessage); // The CrLf version is used in order properly to format exception messages
            }
            catch { }
            try
            {
                EventLogCode.LogEvent("FinaliseInstall", logMessage, EventLogEntryType.Information, GlobalConstants.EventLogErrors.UninstallASCOMInfo, "");
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
                EventLogCode.LogEvent("FinaliseInstall", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.UninstallASCOMError, logMessage);
            }
            catch { }
        }

        protected static void FinaliseRestorePoint()
        {
            try
            {
                LogMessage("FinaliseRestorePoint", "Creating Restore Point");
                ManagementScope oScope = new ManagementScope("\\\\localhost\\root\\default");
                ManagementPath oPath = new ManagementPath("SystemRestore");
                ObjectGetOptions oGetOp = new ObjectGetOptions();
                ManagementClass oProcess = new ManagementClass(oScope, oPath, oGetOp);

                ManagementBaseObject oInParams = oProcess.GetMethodParameters("CreateRestorePoint");
                oInParams["Description"] = "ASCOM Platform 6";
                oInParams["RestorePointType"] = 0;
                oInParams["EventType"] = 101;

                ManagementBaseObject oOutParams = oProcess.InvokeMethod("CreateRestorePoint", oInParams, null);
                LogMessage("FinaliseRestorePoint", "Returned from FinaliseRestorePoint method");
            }
            catch (Exception ex)
            {
                LogError("FinaliseRestorePoint", ex.ToString());
            }

        }

        /// <summary>
        /// Check and if required, fix permissions on HKEY_CLASSES_ROOT
        /// </summary>
        /// <param name="FixPermissions">True to fix missing permissions, flase just to check and report permission state</param>
        protected static void CheckHKCRPermissions(bool FixPermissions)
        {
            try
            {
                LogMessage("CheckHKCRPermissions", "Starting HKCR permission check, FixPermissions: " + FixPermissions.ToString());

                bool FoundCreatorOwnerGenericAccess = false;
                bool FoundSystemGenericAccess = false;
                bool FoundSystemSpecificAccess = false;
                bool FoundAdministratorGenericAccess = false;
                bool FoundAdministratorSpecificAccess = false;
                bool FoundUserGenericAccess = false;
                bool FoundUserSpecificAccess = false;
                AccessRights Rights;

                RegistrySecurity HKCRAccessControl = Registry.ClassesRoot.GetAccessControl();

                //Iterate over the rule set and list them for Builtin users
                foreach (RegistryAccessRule RegRule in HKCRAccessControl.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    Rights = (AccessRights)RegRule.RegistryRights;

                    LogMessage("CheckHKCRPermissions", "Found rule: " + RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + Rights.ToString() + " / " + (RegRule.IsInherited ? "Inherited" : "NotInherited") + " / " + RegRule.InheritanceFlags.ToString() + " / " + RegRule.PropagationFlags.ToString());

                    // Allow CREATOR OWNER GenericAll / NotInherited / ContainerInherit / InheritOnly
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(CreatorOwnerSID).ToUpper()) &
                         Rights == AccessRights.GenericAll & RegRule.InheritanceFlags == InheritanceFlags.ContainerInherit &
                         RegRule.PropagationFlags == PropagationFlags.InheritOnly) FoundCreatorOwnerGenericAccess = true;

                    // Allow NT AUTHORITY\SYSTEM GenericAll / NotInherited / ContainerInherit / InheritOnly
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(NTAuthoritySystemSID).ToUpper()) &
                         Rights == AccessRights.GenericAll & RegRule.InheritanceFlags == InheritanceFlags.ContainerInherit &
                         RegRule.PropagationFlags == PropagationFlags.InheritOnly) FoundSystemGenericAccess = true;

                    // Allow NT AUTHORITY\SYSTEM Query, SetKey, CreateSubKey, EnumSubkey, Notify, CreateLink, StandardDelete, StandardReadControl, StandardWriteDAC, StandardWriteOwner / NotInherited / None / None
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(NTAuthoritySystemSID).ToUpper()) &
                         Rights == FullRights &
                         RegRule.InheritanceFlags == InheritanceFlags.None &
                         RegRule.PropagationFlags == PropagationFlags.None) FoundSystemSpecificAccess = true;

                    // Allow BUILTIN\Administrators GenericAll / NotInherited / ContainerInherit / InheritOnly
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(BuiltInAdministratorsSID).ToUpper()) &
                         Rights == AccessRights.GenericAll & RegRule.InheritanceFlags == InheritanceFlags.ContainerInherit &
                         RegRule.PropagationFlags == PropagationFlags.InheritOnly) FoundAdministratorGenericAccess = true;

                    // Allow BUILTIN\Administrators Query, SetKey, CreateSubKey, EnumSubkey, Notify, CreateLink, StandardDelete, StandardReadControl, StandardWriteDAC, StandardWriteOwner / NotInherited / None / None
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(BuiltInAdministratorsSID).ToUpper()) &
                         Rights == FullRights &
                         RegRule.InheritanceFlags == InheritanceFlags.None &
                         RegRule.PropagationFlags == PropagationFlags.None) FoundAdministratorSpecificAccess = true;

                    // Allow BUILTIN\Users GenericRead / NotInherited / ContainerInherit / InheritOnly
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(BuiltInUsersSID).ToUpper()) &
                         Rights == AccessRights.GenericRead & RegRule.InheritanceFlags == InheritanceFlags.ContainerInherit &
                         RegRule.PropagationFlags == PropagationFlags.InheritOnly) FoundUserGenericAccess = true;

                    // Allow BUILTIN\Users Query, EnumSubkey, Notify, StandardReadControl / NotInherited / None / None
                    if ((RegRule.IdentityReference.ToString().ToUpper() == GetLocalAccountName(BuiltInUsersSID).ToUpper()) &
                         Rights == ReadRights &
                         RegRule.InheritanceFlags == InheritanceFlags.None &
                         RegRule.PropagationFlags == PropagationFlags.None) FoundUserSpecificAccess = true;

                }
                LogMessage("CheckHKCRPermissions", " ");

                if (FoundCreatorOwnerGenericAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have CreatorOwnerGenericAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have CreatorOwnerGenericAccess!");

                if (FoundSystemGenericAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have SystemGenericAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have SystemGenericAccess!");
                if (FoundSystemSpecificAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have SystemSpecificAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have SystemSpecificAccess!");

                if (FoundAdministratorGenericAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have AdministratorGenericAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have AdministratorGenericAccess!");
                if (FoundAdministratorSpecificAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have AdministratorSpecificAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have AdministratorSpecificAccess!");

                if (FoundUserGenericAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have UserGenericAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have UserGenericAccess!");
                if (FoundUserSpecificAccess) LogMessage("CheckHKCRPermissions", "OK - HKCR\\ does have UserSpecificAccess");
                else LogError("CheckHKCRPermissions", "HKCR\\ does not have UserSpecificAccess!");

                LogMessage("CheckHKCRPermissions", " ");

                if (FixPermissions)
                {
                    Stopwatch swLocal = null;

                    try
                    {
                        swLocal = Stopwatch.StartNew();
                        LogMessage("SetRegistryACL", "Fixing registry permissions");

                        //Set a security ACL on the ASCOM Profile key giving the Users group Full Control of the key
                        LogMessage("SetRegistryACL", "Creating security identifiers");
                        SecurityIdentifier DomainSid = new SecurityIdentifier("S-1-0-0"); //Create a starting point domain SID

                        //Create security identifiers for the various accounts to be passed to the new accessrule
                        SecurityIdentifier BuiltinUsersIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid);
                        SecurityIdentifier AdministratorsIdentifier = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, DomainSid);
                        SecurityIdentifier CreatorOwnerIdentifier = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, DomainSid);
                        SecurityIdentifier SystemIdentifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, DomainSid);

                        LogMessage("SetRegistryACL", "Creating new ACL rules"); // Create the new access permission rules
                        RegistryAccessRule CreatorOwnerGenericAccessRule = new RegistryAccessRule(CreatorOwnerIdentifier,
                                                                                                  (RegistryRights)AccessRights.GenericAll,
                                                                                                  InheritanceFlags.ContainerInherit,
                                                                                                  PropagationFlags.InheritOnly,
                                                                                                  AccessControlType.Allow);

                        RegistryAccessRule SystemGenericAccessRule = new RegistryAccessRule(SystemIdentifier,
                                                                                            (RegistryRights)AccessRights.GenericAll,
                                                                                            InheritanceFlags.ContainerInherit,
                                                                                            PropagationFlags.InheritOnly,
                                                                                            AccessControlType.Allow);

                        RegistryAccessRule SystemFullAccessRule = new RegistryAccessRule(SystemIdentifier,
                                                                                         (RegistryRights)FullRights,
                                                                                         InheritanceFlags.None,
                                                                                         PropagationFlags.None,
                                                                                         AccessControlType.Allow);

                        RegistryAccessRule AdministratorsGenericAccessRule = new RegistryAccessRule(AdministratorsIdentifier,
                                                                                                    (RegistryRights)AccessRights.GenericAll,
                                                                                                    InheritanceFlags.ContainerInherit,
                                                                                                    PropagationFlags.InheritOnly,
                                                                                                    AccessControlType.Allow);

                        RegistryAccessRule AdministratorsFullAccessRule = new RegistryAccessRule(AdministratorsIdentifier,
                                                                                                 (RegistryRights)FullRights,
                                                                                                 InheritanceFlags.None,
                                                                                                 PropagationFlags.None,
                                                                                                 AccessControlType.Allow);

                        RegistryAccessRule BuiltinUsersGenericAccessRule = new RegistryAccessRule(BuiltinUsersIdentifier,
                                                                                                  unchecked((RegistryRights)AccessRights.GenericRead),
                                                                                                  InheritanceFlags.ContainerInherit,
                                                                                                  PropagationFlags.InheritOnly,
                                                                                                  AccessControlType.Allow);

                        RegistryAccessRule BuiltinUsersReadAccessRule = new RegistryAccessRule(BuiltinUsersIdentifier,
                                                                                               (RegistryRights)ReadRights,
                                                                                               InheritanceFlags.None,
                                                                                               PropagationFlags.None,
                                                                                               AccessControlType.Allow);

                        LogMessage("SetRegistryACL", "Retrieving current ACL rule");
                        LogMessage("SetRegistryACL", " ");
                        RegistrySecurity KeySec = Registry.ClassesRoot.GetAccessControl(); // Get existing ACL rules on the key 

                        //Iterate over the rule set and list them
                        foreach (RegistryAccessRule RegRule in KeySec.GetAccessRules(true, true, typeof(NTAccount)))
                        {
                            LogMessage("SetRegistryACL Before", RegRule.AccessControlType.ToString() + " " +
                                                             RegRule.IdentityReference.ToString() + " " +
                                                             ((AccessRights)RegRule.RegistryRights).ToString() + " " +
                                                             RegRule.IsInherited.ToString() + " " +
                                                             RegRule.InheritanceFlags.ToString() + " " +
                                                             RegRule.PropagationFlags.ToString());
                        }

                        LogMessage("SetRegistryACL", "Adding new ACL rules");
                        LogMessage("SetRegistryACL", " ");

                        // Remove old rules
                        KeySec.PurgeAccessRules(CreatorOwnerIdentifier);
                        KeySec.PurgeAccessRules(AdministratorsIdentifier);
                        KeySec.PurgeAccessRules(BuiltinUsersIdentifier);
                        KeySec.PurgeAccessRules(SystemIdentifier);

                        //Add the new rules to the existing rules
                        KeySec.AddAccessRule(CreatorOwnerGenericAccessRule);
                        KeySec.AddAccessRule(SystemGenericAccessRule);
                        KeySec.AddAccessRule(SystemFullAccessRule);
                        KeySec.AddAccessRule(AdministratorsGenericAccessRule);
                        KeySec.AddAccessRule(AdministratorsFullAccessRule);
                        KeySec.AddAccessRule(BuiltinUsersGenericAccessRule);
                        KeySec.AddAccessRule(BuiltinUsersReadAccessRule);

                        //Iterate over the new rule set and list them
                        foreach (RegistryAccessRule RegRule in KeySec.GetAccessRules(true, true, typeof(NTAccount)))
                        {
                            LogMessage("SetRegistryACL After", RegRule.AccessControlType.ToString() + " " +
                                                            RegRule.IdentityReference.ToString() + " " +
                                                            ((AccessRights)RegRule.RegistryRights).ToString() + " " +
                                                            RegRule.IsInherited.ToString() + " " +
                                                            RegRule.InheritanceFlags.ToString() + " " +
                                                            RegRule.PropagationFlags.ToString());
                        }

                        LogMessage("SetRegistryACL", "Setting new ACL rule");
                        Registry.ClassesRoot.SetAccessControl(KeySec); //Apply the new rules to the Profile key

                        LogMessage("SetRegistryACL", "Rerieving new ruleset from key");

                        // Retrieve the new ruleset and and list them
                        foreach (RegistryAccessRule RegRule in Registry.ClassesRoot.GetAccessControl().GetAccessRules(true, true, typeof(NTAccount)))
                        {
                            LogMessage("SetRegistryACL New", RegRule.AccessControlType.ToString() + " " +
                                                          RegRule.IdentityReference.ToString() + " " +
                                                          ((AccessRights)RegRule.RegistryRights).ToString() + " " +
                                                          RegRule.IsInherited.ToString() + " " +
                                                          RegRule.InheritanceFlags.ToString() + " " +
                                                          RegRule.PropagationFlags.ToString());
                        }

                        swLocal.Stop();
                        LogMessage("SetRegistryACL", "ElapsedTime " + swLocal.ElapsedMilliseconds + " milliseconds");
                        swLocal = null;
                    }
                    catch (Exception ex)
                    {
                        LogMessage("SetRegistryACLException", ex.ToString());
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                SetReturnCode(5);
                LogError("CheckHKCRPermissions", "HKCR\\ does not exist. " + ex.ToString()); // Should never happen!
            }
            catch (SecurityException ex)
            {
                SetReturnCode(5);
                LogError("CheckHKCRPermissions", "Security exception when accessing HKCR\\ " + ex.ToString()); // Should never happen!
            }
            catch (Exception ex)
            {
                SetReturnCode(5);
                LogError("CheckHKCRPermissions", "Unexpected exception: " + ex.ToString());
            }

        }

        /// <summary>
        /// Returns the localised text name of the BUILTIN\Users group. This varies by locale so has to be derrived on the users system.
        /// </summary>
        /// <returns>Localised name of the BUILTIN\Users group</returns>
        /// <remarks>This uses the WMI features and is pretty obscure - sorry, it was the only way I could find to do this! Peter</remarks>
        protected static string GetBuiltInGroup(string DomainSID, string GroupSID)
        {
            ManagementObjectSearcher Searcher = default(ManagementObjectSearcher);
            string Group = "Unknown";
            // Initialise to some values
            string Name = "Unknown";
            PropertyDataCollection p = default(PropertyDataCollection);

            //LogMessage("", "Start: " + DomainSID + " " + GroupSID);

            try
            {
                //Searcher = new ManagementObjectSearcher(new ManagementScope("\\\\localhost\\root\\cimv2"), new WqlObjectQuery("Select * From Win32_Account "), null);
                Searcher = new ManagementObjectSearcher(new ManagementScope("\\\\localhost\\root\\cimv2"),
                    new WqlObjectQuery("Select * From Win32_Account Where SID = '" + DomainSID + "'"), null);
                //new WqlObjectQuery("Select * From Win32_Account Where SID = 'S-1-5-32'"), null);
                //new WqlObjectQuery("Select * From Win32_Account"), null);

                //LogMessage("GetBuiltInGroup", "Found " + Searcher.Get().Count + " entries");
                int count = 0;
                foreach (ManagementBaseObject wmiClass in Searcher.Get())
                {
                    count += 1;
                    p = wmiClass.Properties;
                    foreach (PropertyData pr in p)
                    {
                        if (pr.Name == "Name")
                        {
                            Group = pr.Value.ToString();
                            //LogMessage("GetBuiltInGroup Name " + count, pr.Name + " = " + pr.Value.ToString());
                        }
                    }
                }
                Searcher.Dispose();
            }
            catch (Exception ex)
            {
                LogMessage("GetBuiltInUsers 1", ex.ToString());
            }

            try
            {
                Searcher = new ManagementObjectSearcher(new ManagementScope("\\\\localhost\\root\\cimv2"),
                    new WqlObjectQuery("Select * From Win32_Group Where SID = '" + GroupSID + "'"), null);

                foreach (ManagementBaseObject wmiClass in Searcher.Get())
                {
                    p = wmiClass.Properties;
                    foreach (PropertyData pr in p)
                    {
                        if (pr.Name == "Name") Name = pr.Value.ToString();
                    }
                }
                Searcher.Dispose();
            }
            catch (Exception ex)
            {
                LogMessage("GetBuiltInUsers 2", ex.ToString());
            }
            // LogMessage("GetBuiltInUsers", "Returning: " + Group + "\\" + Name);
            return Group + "\\" + Name;
        }

        /// <summary>
        /// Returns the localised text name of the requested account. This varies by locale so has to be derrived on the users system.
        /// </summary>
        /// <param name="AccountSID">SID of the account whose localised name is required</param>
        /// <returns>Localised name of the account</returns>
        /// <remarks>The SID shold be in a form similar to S-1-5-18</remarks>
        protected static string GetLocalAccountName(string AccountSID)
        {
            SecurityIdentifier sid = new SecurityIdentifier(AccountSID);
            NTAccount acct = (NTAccount)sid.Translate(typeof(NTAccount));
            LogMessage("GetLocalAccountName", "SID: " + sid + ", Returning:" + acct.Value);
            return acct.Value;
        }

    }

}