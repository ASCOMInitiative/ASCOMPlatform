using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static ASCOM.Utilities.Serial;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

// These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

using Microsoft.Win32;
using System.Windows.Forms;
using static ASCOM.Utilities.RegistryAccess;
using ASCOM.Utilities.Exceptions;

namespace ASCOM.Utilities
{
    // Common constants for the ASCOM.Utilities name space

    static class Global
    {

        #region Constants and Enums

        internal const string SERIAL_FILE_NAME_VARNAME = "SerTraceFile"; // Constant naming the profile trace file variable name
        internal const string SERIAL_AUTO_FILENAME = @"C:\SerialTraceAuto.txt"; // Special value to indicate use of automatic trace filenames
        internal const string SERIAL_DEFAULT_FILENAME = @"C:\SerialTrace.txt"; // Default manual trace filename
        internal const string SERIAL_DEBUG_TRACE_VARNAME = "SerDebugTrace"; // Constant naming the profile trace file variable name
        internal const string SERIALPORT_COM_PORT_SETTINGS = "COMPortSettings";
        internal const string SERIAL_FORCED_COMPORTS_VARNAME = SERIALPORT_COM_PORT_SETTINGS + @"\ForceCOMPorts"; // Constant listing COM ports that will be forced to be present
        internal const string SERIAL_IGNORE_COMPORTS_VARNAME = SERIALPORT_COM_PORT_SETTINGS + @"\IgnoreCOMPorts"; // Constant listing COM ports that will be ignored if present

        // Utilities configuration constants
        internal const string TRACE_XMLACCESS = "Trace XMLAccess";
        internal const bool TRACE_XMLACCESS_DEFAULT = false;
        internal const string TRACE_PROFILE = "Trace Profile";
        internal const bool TRACE_PROFILE_DEFAULT = false;
        internal const string TRACE_UTIL = "Trace Util";
        internal const bool TRACE_UTIL_DEFAULT = false;
        internal const string TRACE_TIMER = "Trace Timer";
        internal const bool TRACE_TIMER_DEFAULT = false;
        internal const string SERIAL_TRACE_DEBUG = "Serial Trace Debug";
        internal const bool SERIAL_TRACE_DEBUG_DEFAULT = false;
        internal const string SIMULATOR_TRACE = "Trace Simulators";
        internal const bool SIMULATOR_TRACE_DEFAULT = false;
        internal const string DRIVERACCESS_TRACE = "Trace DriverAccess";
        internal const bool DRIVERACCESS_TRACE_DEFAULT = false;
        internal const string CHOOSER_USE_CREATEOBJECT = "Chooser Use CreateObject";
        internal const bool CHOOSER_USE_CREATEOBJECT_DEFAULT = false;
        internal const string ABANDONED_MUTEXT_TRACE = "Trace Abandoned Mutexes";
        internal const bool ABANDONED_MUTEX_TRACE_DEFAULT = false;
        internal const string ASTROUTILS_TRACE = "Trace Astro Utils";
        internal const bool ASTROUTILS_TRACE_DEFAULT = false;
        internal const string NOVAS_TRACE = "Trace NOVAS";
        internal const bool NOVAS_TRACE_DEFAULT = false;
        internal const string SERIAL_WAIT_TYPE = "Serial Wait Type";
        internal const Serial.WaitType SERIAL_WAIT_TYPE_DEFAULT = Serial.WaitType.WaitForSingleObject;
        internal const string SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE = "Suppress Alpaca Driver Admin Dialogue";
        internal const bool SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT = false;
        internal const string PROFILE_MUTEX_NAME = "ASCOMProfileMutex"; // Name and time-out value for the Profile mutex than ensures only one profile action happens at a time
        internal const int PROFILE_MUTEX_TIMEOUT = 5000;

        // Trace settings values, these are used to persist trace values on a per user basis
        internal const string TRACE_TRANSFORM = "Trace Transform";
        internal const bool TRACE_TRANSFORM_DEFAULT = false;
        internal const string REGISTRY_UTILITIES_FOLDER = @"Software\ASCOM\Utilities";
        internal const string TRACE_CACHE = "Trace Cache";
        internal const bool TRACE_CACHE_DEFAULT = false;
        internal const string TRACE_EARTHROTATION_DATA_FORM = "Trace Earth Rotation Data Form";
        internal const bool TRACE_EARTHROTATION_DATA_FORM_DEFAULT = false;

        // Settings for the ASCOM Windows event log
        internal const string EVENT_SOURCE = "ASCOM Platform"; // Name of the the event source
        internal const string EVENTLOG_NAME = "ASCOM"; // Name of the event log as it appears in Windows event viewer
        internal const string EVENTLOG_MESSAGES = @"ASCOM\EventLogMessages.txt";
        internal const string EVENTLOG_ERRORS = @"ASCOM\EventLogErrors.txt";

        // RegistryAccess constants
        internal const string REGISTRY_ROOT_KEY_NAME = @"SOFTWARE\ASCOM"; // Location of ASCOM profile in HKLM registry hive
        internal const string REGISTRY_5_BACKUP_SUBKEY = "Platform5Original"; // Location that the original Platform 5 Profile will be copied to before migrating the 5.5 Profile back to the registry
        internal const string REGISTRY_55_BACKUP_SUBKEY = "Platform55Original"; // Location that the original Platform 5.5 Profile will be copied to before removing Platform 5 and 5.5
        internal const string PLATFORM_VERSION_NAME = "PlatformVersion";
        // XML constants used by XMLAccess and RegistryAccess classes
        internal const string COLLECTION_DEFAULT_VALUE_NAME = "***** DefaultValueName *****"; // Name identifier label
        internal const string COLLECTION_DEFAULT_UNSET_VALUE = "===== ***** UnsetValue ***** ====="; // Value identifier label
        internal const string VALUES_FILENAME = "Profile.xml"; // Name of file to contain profile XML information
        internal const string VALUES_FILENAME_ORIGINAL = "ProfileOriginal.xml"; // Name of file to contain original profile XML information
        internal const string VALUES_FILENAME_NEW = "ProfileNew.xml"; // Name of file to contain original profile XML information

        internal const string PROFILE_NAME = "Profile"; // Name of top level XML element
        internal const string SUBKEY_NAME = "SubKey"; // Profile sub-key element name
        internal const string DEFAULT_ELEMENT_NAME = "DefaultElement"; // Default value label
        internal const string VALUE_ELEMENT_NAME = "Element"; // Profile value element name
        internal const string NAME_ATTRIBUTE_NAME = "Name"; // Profile value name attribute
        internal const string VALUE_ATTRIBUTE_NAME = "Value"; // Profile element value attribute

        // XML constants used by ASCOMProfile class to serialise and de-serialise a profile. These are public so that they can be used by applications to work directly with the returned XML
        public const string XML_SUBKEYNAME_ELEMENTNAME = "SubKeyName";
        public const string XML_DEFAULTVALUE_ELEMENTNAME = "DefaultValue";
        public const string XML_NAME_ELEMENTNAME = "Name";
        public const string XML_DATA_ELEMENTNAME = "Data";
        public const string XML_SUBKEY_ELEMENTNAME = "SubKey";
        public const string XML_VALUE_ELEMENTNAME = "Value";
        public const string XML_VALUES_ELEMENTNAME = "Values";

        // Location of the lists of 32bit and 64bit only drivers and PlatformVersion exception lists
        public const string DRIVERS_32BIT = "Drivers Not Compatible With 64bit Applications"; // 32bit only registry location
        public const string DRIVERS_64BIT = "Drivers Not Compatible With 32bit Applications"; // 64bit only registry location
        internal const string PLATFORM_VERSION_EXCEPTIONS = "ForcePlatformVersion";
        internal const string PLATFORM_VERSION_SEPARATOR_EXCEPTIONS = "ForcePlatformVersionSeparator";

        internal const string FORCE_SYSTEM_TIMER = "ForceSystemTimer"; // Location of executables for which we must force system timer rather than forms timer

        // Installer Variables
        public const string PLATFORM_INSTALLER_PROPDUCT_CODE = "{8961E141-B307-4882-ABAD-77A3E76A40C1}"; // {8961E141-B307-4882-ABAD-77A3E76A40C1}
        public const string DEVELOPER_INSTALLER_PROPDUCT_CODE = "{4A195DC6-7DF9-459E-8F93-60B61EB45288}";

        // Contact driver author message
        internal const string DRIVER_AUTHOR_MESSAGE_DRIVER = "Please contact the driver author and request an updated driver.";
        internal const string DRIVER_AUTHOR_MESSAGE_INSTALLER = "Please contact the driver author and request an updated installer.";

        // Location of Platform version in Profile
        internal const string PLATFORM_INFORMATION_SUBKEY = "Platform";
        internal const string PLATFORM_VERSION = "Platform Version";
        internal const string PLATFORM_VERSION_DEFAULT_BAD_VALUE = "0.0.0.0";

        // TraceLogger - Per user configuration value names
        internal const string TRACELOGGER_DEFAULT_FOLDER = "TraceLogger Default Folder";

        // Check for updates constants
        internal const string CHECK_FOR_RELEASE_UPDATES = "Check for Release Updates"; // Name of flag indicating whether or not to check for new releases
        internal const bool CHECK_FOR_RELEASE_UPDATES_DEFAULT = true;
        internal const string CHECK_FOR_RELEASE_CANDIDATES = "Check for Release Candidate Updates"; // Name of flag indicating whether or not to check for new "release candidate" releases
        internal const bool CHECK_FOR_RELEASE_CANDIDATES_DEFAULT = false;

        // Other constants
        internal const double ABSOLUTE_ZERO_CELSIUS = -273.15d;
        internal const string TRACE_LOGGER_PATH = @"\ASCOM"; // Path to TraceLogger directory from My Documents
        internal const string TRACE_LOGGER_FILENAME_BASE = @"\Logs "; // Fixed part of TraceLogger file name.  Note: The trailing space must be retained!
        internal const string TRACE_LOGGER_FILE_NAME_DATE_FORMAT = "yyyy-MM-dd";
        internal const string TRACE_LOGGER_SYSTEM_PATH = @"\ASCOM\SystemLogs"; // Location where "System" user logs will be placed

        internal enum EventLogErrors : int
        {
            EventLogCreated = 0,
            ChooserFormLoad = 1,
            MigrateProfileVersions = 2,
            MigrateProfileRegistryKey = 3,
            RegistryProfileMutexTimeout = 4,
            XMLProfileMutexTimeout = 5,
            XMLAccessReadError = 6,
            XMLAccessRecoveryPreviousVersion = 7,
            XMLAccessRecoveredOK = 8,
            ChooserSetupFailed = 9,
            ChooserDriverFailed = 10,
            ChooserException = 11,
            Chooser32BitOnlyException = 12,
            Chooser64BitOnlyException = 13,
            FocusSimulatorNew = 14,
            FocusSimulatorSetup = 15,
            TelescopeSimulatorNew = 16,
            TelescopeSimulatorSetup = 17,
            VB6HelperProfileException = 18,
            DiagnosticsLoadException = 19,
            DriverCompatibilityException = 20,
            TimerSetupException = 21,
            DiagnosticsHijackedCOMRegistration = 22,
            UninstallASCOMInfo = 23,
            UninstallASCOMError = 24,
            ProfileExplorerException = 25,
            InstallTemplatesInfo = 26,
            InstallTemplatesError = 27,
            TraceLoggerException = 28,
            TraceLoggerMutexTimeOut = 29,
            TraceLoggerMutexAbandoned = 30,
            RegistryProfileMutexAbandoned = 31,
            EarthRotationUpdate = 32,
            ManageScheduledTask = 33,
            Sofa = 34
        }

        #endregion

        #region COM Registration Support

        /// <summary>
        /// Update a COM registration assembly executable reference (mscoree.dll) from a relative path to an absolute path
        /// </summary>
        /// <remarks>This is necessary to ensure that the mscoree.dll can be found when the SetSearchDirectories function has been called in an application e.g. by Inno installer post v5.5.9.
        /// The COM class name and ClassID are determined from the supplied type definition. If the ClassID cannot be determined it is looked up through the COM registration registry entry through the class's ProgID
        /// </remarks>
        internal static void COMRegister(Type typeToRegister)
        {
            string className, clsId, mscoree, fullPath;
            object[] attributes;
            TraceLogger TL = null;

            try
            {
                TL = new TraceLogger("", "COMRegister" + typeToRegister.Name);
                TL.Enabled = true;
                TL.LogMessage("COMRegisterActions", "Start");

                // Report the OS and application bitness
                if (OSBits() == Bitness.Bits64) // 64bit OS
                {
                    TL.LogMessage("OSBits", "64bit OS");
                }
                else
                {
                    TL.LogMessage("OSBits", "32bit OS");
                }

                if (ApplicationBits() == Bitness.Bits64) // 64bit application
                {
                    TL.LogMessage("ApplicationBits", "64bit application");
                }
                else
                {
                    TL.LogMessage("ApplicationBits", "32bit application");
                }

                // Create the fully qualified class name from the namespace and class name
                className = string.Format("{0}.{1}", typeToRegister.Namespace, typeToRegister.Name);
                TL.LogMessage("ClassName", className);

                // Determine the class GUID of the supplied type 
                attributes = typeToRegister.GetCustomAttributes(typeof(GuidAttribute), false); // Get any GUID references in the supplied type - there should always be just one reference

                // Act depending on whether we have found the GUID
                switch (attributes.Length)
                {
                    case 0: // No GUID attribute found - this should never happen
                        {
                            TL.LogMessage("COMRegisterActions", "GuidAttribute not found, obtaining the correct class GUID from the COM registration in the registry");

                            clsId = Conversions.ToString(Registry.ClassesRoot.OpenSubKey(className + @"\CLSID").GetValue("")); // Try plan B to get the GUID from the class's COM registration
                            if (!string.IsNullOrEmpty(clsId))
                            {
                                TL.LogMessage("ClassID", clsId);
                            }
                            else
                            {
                                TL.LogMessage("ClassID", "Could not find ClassID - returned value is null or an empty string");
                            }

                            break;
                        }

                    case 1: // Found the class GUID attribute so extract and use it
                        {
                            TL.LogMessage("COMRegisterActions", "Found a class GuidAttribute - using it to create the class GUID");

                            clsId = "{" + ((GuidAttribute)attributes[0]).Value + "}"; // Create the class ID by enclosing the class GUID in braces
                            if (!string.IsNullOrEmpty(clsId))
                            {
                                TL.LogMessage("ClassID", clsId);
                            }
                            else
                            {
                                TL.LogMessage("ClassID", "Could not find ClassID - returned value is null or an empty string");
                            } // More than 1 GUID attribute so ignore it and look up from the registry - this should never happen!

                            break;
                        }

                    default:
                        {
                            TL.LogMessage("COMRegisterActions", string.Format("{0} GuidAttributes found, obtaining the correct class GUID from the COM registration in the registry", attributes.Length));

                            clsId = Conversions.ToString(Registry.ClassesRoot.OpenSubKey(className + @"\CLSID").GetValue(""));
                            if (!string.IsNullOrEmpty(clsId))
                            {
                                TL.LogMessage("ClassID", clsId);
                            }
                            else
                            {
                                TL.LogMessage("ClassID", "Could not find ClassID - returned value is null or an empty string");
                            }

                            break;
                        }

                }

                // If we have a ClassID then use it to update the class's executable relative path to a full path
                if (!string.IsNullOrEmpty(clsId))
                {
                    mscoree = Conversions.ToString(Registry.ClassesRoot.OpenSubKey(string.Format(@"\CLSID\{0}\InProcServer32", clsId)).GetValue(""));
                    TL.LogMessage("COMRegisterActions", string.Format("Current mscoree.dll path: {0}", mscoree));

                    if (mscoree.ToUpperInvariant() == "MSCOREE.DLL") // This is a relative path so make it absolute
                    {
                        TL.LogMessage("COMRegisterActions", string.Format("The mscoree.dll path is relative: {0}", mscoree));

                        fullPath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), mscoree);
                        TL.LogMessage("COMRegisterActions", string.Format("Full path to the System32 directory: {0}", fullPath));

                        TL.LogMessage("COMRegisterActions", "Setting InProcServer32 value...");
                        Registry.ClassesRoot.OpenSubKey(string.Format(@"\CLSID\{0}\InProcServer32", clsId), true).SetValue("", fullPath);
                        TL.LogMessage("COMRegisterActions", string.Format("InProcServer32value set OK - {0}", fullPath));

                        mscoree = Conversions.ToString(Registry.ClassesRoot.OpenSubKey(string.Format(@"\CLSID\{0}\InProcServer32", clsId)).GetValue(""));
                        TL.LogMessage("COMRegisterActions", string.Format("New mscoree.dll path: {0}", mscoree));
                    }

                    else
                    {
                        TL.LogMessage("COMRegisterActions", "Path is already absolute - no action taken");
                    }
                }
                else
                {
                    TL.LogMessage("COMRegisterActions", "Unable to find the class's ClassID - no action taken.");
                }
            }
            catch (Exception ex)
            {
                if (TL is not null)
                    TL.LogMessageCrLf("Exception", ex.ToString());
            }

        }

        #endregion

        #region Registry Utility Code

        internal static WaitType GetWaitType(string p_Name, WaitType p_DefaultValue)
        {
            var l_Value = default(WaitType);
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = (WaitType)Conversions.ToInteger(Enum.Parse(typeof(WaitType), m_SettingsKey.GetValue(p_Name).ToString()));
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(p_Name, p_DefaultValue.ToString());
                    l_Value = p_DefaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    l_Value = p_DefaultValue;
                }
            }
            catch (Exception)
            {
                l_Value = p_DefaultValue;
            }

            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
            return l_Value;
        }

        internal static bool GetBool(string p_Name, bool p_DefaultValue)
        {
            var l_Value = default(bool);
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToBoolean(m_SettingsKey.GetValue(p_Name));
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(p_Name, p_DefaultValue.ToString());
                    l_Value = p_DefaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    l_Value = p_DefaultValue;
                }
            }
            catch (Exception)
            {
                l_Value = p_DefaultValue;
            }
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
            return l_Value;
        }

        internal static string GetString(string p_Name, string p_DefaultValue)
        {
            string l_Value = "";
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = m_SettingsKey.GetValue(p_Name).ToString();
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(p_Name, p_DefaultValue.ToString());
                    l_Value = p_DefaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    l_Value = p_DefaultValue;
                }
            }
            catch (Exception)
            {
                l_Value = p_DefaultValue;
            }
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
            return l_Value;
        }

        internal static double GetDouble(RegistryKey p_Key, string p_Name, double p_DefaultValue)
        {
            var l_Value = default(double);
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            // LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgDebug, p_Name.ToString & " " & p_DefaultValue.ToString)
            try
            {
                if (p_Key.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToDouble(p_Key.GetValue(p_Name));
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(p_Name, p_DefaultValue.ToString());
                    l_Value = p_DefaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    l_Value = p_DefaultValue;
                }
            }
            catch (Exception)
            {
                l_Value = p_DefaultValue;
            }
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
            return l_Value;
        }

        internal static DateTime GetDate(string p_Name, DateTime p_DefaultValue)
        {
            var l_Value = default(DateTime);
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            try
            {
                if (m_SettingsKey.GetValueKind(p_Name) == RegistryValueKind.String) // Value does exist
                {
                    l_Value = Conversions.ToDate(m_SettingsKey.GetValue(p_Name));
                }
            }
            catch (IOException) // Value doesn't exist so create it
            {
                try
                {
                    SetName(p_Name, p_DefaultValue.ToString());
                    l_Value = p_DefaultValue;
                }
                catch (Exception) // Unable to create value so just return the default value
                {
                    l_Value = p_DefaultValue;
                }
            }
            catch (Exception)
            {
                l_Value = p_DefaultValue;
            }
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
            return l_Value;
        }

        internal static void SetName(string p_Name, string p_Value)
        {
            RegistryKey m_HKCU, m_SettingsKey;

            m_HKCU = Registry.CurrentUser;
            m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER);
            m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, true);

            m_SettingsKey.SetValue(p_Name, p_Value.ToString(), RegistryValueKind.String);
            m_SettingsKey.Flush(); // Clean up registry keys
            m_SettingsKey.Close();
            m_HKCU.Flush();
            m_HKCU.Close();
        }

        #endregion

        #region Windows event log code

        /// <summary>
        /// Add an event record to the ASCOM Windows event log
        /// </summary>
        /// <param name="Caller">Name of routine creating the event</param>
        /// <param name="Msg">Event message</param>
        /// <param name="Severity">Event severity</param>
        /// <param name="Id">Id number</param>
        /// <param name="Except">Initiating exception or Nothing</param>
        /// <remarks></remarks>
        internal static void LogEvent(string Caller, string Msg, EventLogEntryType Severity, EventLogErrors Id, string Except)
        {
            EventLog ELog;
            string MsgTxt;

            // During Platform 6 RC testing a report was received showing that a failure in this code had caused a bad Profile migration
            // There was no problem with the migration code, the issue was caused by the event log code throwing an unexpected exception back to MigrateProfile
            // It is wrong that an error in logging code should cause a client process to fail, so this code has been 
            // made more robust and ultimately will swallow exceptions silently rather than throwing an unexpected exception back to the caller

            try
            {
                if (!EventLog.SourceExists(EVENT_SOURCE)) // Create the event log if it doesn't exist
                {
                    EventLog.CreateEventSource(EVENT_SOURCE, EVENTLOG_NAME);
                    ELog = new EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE); // Create a pointer to the event log
                    ELog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0); // Force the policy to overwrite oldest
                    ELog.MaximumKilobytes = 1024L; // Set the maximum log size to 1024kb, the Win 7 minimum size
                    ELog.Close(); // Force the log file to be created by closing the log
                    ELog.Dispose();
                    ELog = null;

                    // MSDN documentation advises waiting before writing, first time to a newly created event log file but doesn't say how long...
                    // Waiting 3 seconds to allow the log to be created by the OS
                    System.Threading.Thread.Sleep(3000);

                    // Try and create the initial log message
                    ELog = new EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE); // Create a pointer to the event log
                    ELog.WriteEntry("Successfully created event log - Policy: " + ELog.OverflowAction.ToString() + ", Size: " + ELog.MaximumKilobytes + "kb", EventLogEntryType.Information, (int)EventLogErrors.EventLogCreated);
                    ELog.Close();
                    ELog.Dispose();
                }

                // Write the event to the log
                ELog = new EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE); // Create a pointer to the event log

                MsgTxt = Caller + " - " + Msg; // Format the message to be logged
                if (Except is not null)
                    MsgTxt += Microsoft.VisualBasic.Constants.vbCrLf + Except;
                ELog.WriteEntry(MsgTxt, Severity, (int)Id); // Write the message to the error log

                ELog.Close();
                ELog.Dispose();
            }
            catch (System.ComponentModel.Win32Exception ex) // Special handling because these exceptions contain error codes we may want to know
            {
                try
                {
                    string TodaysDateTime = Strings.Format(DateTime.Now, "dd MMMM yyyy HH:mm:ss.fff");
                    string ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_ERRORS;
                    string MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_MESSAGES;

                    // Write to backup event log message and error logs
                    File.AppendAllText(ErrorLog, TodaysDateTime + " ErrorCode: 0x" + Conversion.Hex(ex.ErrorCode) + " NativeErrorCode: 0x" + Conversion.Hex(ex.NativeErrorCode) + " " + ex.ToString() + Microsoft.VisualBasic.Constants.vbCrLf);
                    File.AppendAllText(MessageLog, TodaysDateTime + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + Microsoft.VisualBasic.Constants.vbCrLf);
                }
                catch (Exception) // Ignore exceptions here, the PC seems to be in a catastrophic failure!
                {

                }
            }
            catch (Exception ex) // Catch all other exceptions
            {
                // Somthing bad happened when writing to the event log so try and log it in a log file on the file system
                try
                {
                    string TodaysDateTime = Strings.Format(DateTime.Now, "dd MMMM yyyy HH:mm:ss.fff");
                    string ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_ERRORS;
                    string MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_MESSAGES;

                    // Write to backup event log message and error logs
                    File.AppendAllText(ErrorLog, TodaysDateTime + " " + ex.ToString() + Microsoft.VisualBasic.Constants.vbCrLf);
                    File.AppendAllText(MessageLog, TodaysDateTime + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + Microsoft.VisualBasic.Constants.vbCrLf);
                }
                catch (Exception) // Ignore exceptions here, the PC seems to be in a catastrophic failure!
                {

                }
            }
        }

        #endregion

        #region Version Code

        internal static string GetCommonProgramFilesx86()
        {
            if (IntPtr.Size == 8 | !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
            {
                return Environment.GetEnvironmentVariable("CommonProgramFiles(x86)");
            }
            else
            {
                return Environment.GetEnvironmentVariable("CommonProgramFiles");
            }
        }

        internal static void RunningVersions(TraceLogger TL)
        {
            Assembly[] Assemblies; // Define an array of assembly information
            var AppDom = AppDomain.CurrentDomain;

            // Get Operating system information
            var OS = Environment.OSVersion;

            try
            {
                TL.LogMessage("Versions", "Run on: " + DateTime.Now.ToString("dddd dd MMMM yyyy"));
                TL.LogMessage("Versions", "Main Process: " + Process.GetCurrentProcess().MainModule.FileName); // Get the name of the executable without path or file extension
                FileVersionInfo FV;
                FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; // Get the name of the executable without path or file extension
                TL.LogMessageCrLf("Versions", "  Product:  " + FV.ProductName + " " + FV.ProductVersion);
                TL.LogMessageCrLf("Versions", "  File:     " + FV.FileDescription + " " + FV.FileVersion);
                TL.LogMessageCrLf("Versions", "  Language: " + FV.Language);
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                TL.LogMessage("Versions", "Exception EX0: " + ex.ToString());
            }

            try // Make sure this code never throws an exception back to the caller
            {
                TL.LogMessage("Versions", $"OS Platform:  {OS.Platform}, Service Pack: {OS.ServicePack}, Version string: {OS.VersionString}");
                switch (OSBits())
                {
                    case Bitness.Bits32:
                        {
                            TL.LogMessage("Versions", "Operating system is 32bit");
                            break;
                        }
                    case Bitness.Bits64:
                        {
                            TL.LogMessage("Versions", "Operating system is 64bit");
                            break;
                        }

                    default:
                        {
                            TL.LogMessage("Versions", "Operating system is unknown bits, PTR length is: " + IntPtr.Size);
                            break;
                        }
                }

                switch (ApplicationBits())
                {
                    case Bitness.Bits32:
                        {
                            TL.LogMessage("Versions", "Application is 32bit");
                            break;
                        }
                    case Bitness.Bits64:
                        {
                            TL.LogMessage("Versions", "Application is 64bit");
                            break;
                        }

                    default:
                        {
                            TL.LogMessage("Versions", "Application is unknown bits, PTR length is: " + IntPtr.Size);
                            break;
                        }
                }
                TL.LogMessage("Versions", "");

                // Get common language runtime version
                TL.LogMessage("Versions", "CLR version: " + Environment.Version.ToString());

                // Get file system information
                string UserDomainName = Environment.UserDomainName;
                string UserName = Environment.UserName;
                string MachineName = Environment.MachineName;
                int ProcCount = Environment.ProcessorCount;
                string SysDir = Environment.SystemDirectory;
                long WorkSet = Environment.WorkingSet;
                TL.LogMessage("Versions", "Machine name: " + MachineName + " UserName: " + UserName + " DomainName: " + UserDomainName);
                TL.LogMessage("Versions", "Number of processors: " + ProcCount + " System directory: " + SysDir + " Working set size: " + WorkSet + " bytes");
                TL.LogMessage("Versions", "");

                // Get fully qualified paths to particular directories in a non OS specific way
                // There are many more options in the SpecialFolders Enum than are shown here!
                TL.LogMessage("Versions", "My Documents:            " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                TL.LogMessage("Versions", "Application Data:        " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                TL.LogMessage("Versions", "Common Application Data: " + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                TL.LogMessage("Versions", "Program Files:           " + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                TL.LogMessage("Versions", "Common Files:            " + Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
                TL.LogMessage("Versions", "System:                  " + Environment.GetFolderPath(Environment.SpecialFolder.System));
                TL.LogMessage("Versions", "Current:                 " + Environment.CurrentDirectory);
                TL.LogMessage("Versions", "");

                // Get loaded assemblies
                Assemblies = AppDom.GetAssemblies(); // Get a list of loaded assemblies
                foreach (Assembly FoundAssembly in Assemblies)
                    TL.LogMessage("Versions", "Loaded Assemblies: " + FoundAssembly.GetName().Name + " " + FoundAssembly.GetName().Version.ToString());
                TL.LogMessage("Versions", "");

                // Get assembly versions
                AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly());
                AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly());
                TL.BlankLine();
            }
            catch (Exception ex) // Just log the exception, we don't want the caller to know this diagnostic code failed
            {
                TL.LogMessageCrLf("Versions", "Unexpected exception: " + ex.ToString());
            }
        }

        internal enum Bitness
        {
            Bits32,
            Bits64,
            BitsMSIL,
            BitsUnknown
        }

        internal static Bitness OSBits()
        {
            if (IsWow64()) // Application is under WoW64 so OS must be 64bit
            {
                return Bitness.Bits64;
            }
            else // Could be 32bit or 64bit Use IntPtr
            {
                switch (IntPtr.Size)
                {
                    case 4:
                        return Bitness.Bits32;

                    case 8:
                        return Bitness.Bits64;

                    default:
                        return Bitness.BitsUnknown;
                }
            }

        }

        internal static Bitness ApplicationBits()
        {
            switch (IntPtr.Size)
            {
                case 4:
                    return Bitness.Bits32;

                case 8:
                    return Bitness.Bits64;

                default:
                    return Bitness.BitsUnknown;
            }
        }

        internal static void AssemblyInfo(TraceLogger TL, string AssName, Assembly Ass)
        {
            FileVersionInfo FileVer;
            AssemblyName AssblyName;
            Version Vers;
            string VerString, FVer, FName;
            string Location = null;

            AssName = Strings.Left(AssName + ":" + Strings.Space(20), 19);

            if (Ass is not null)
            {
                try
                {
                    AssblyName = Ass.GetName();
                    if (AssblyName is null)
                    {
                        TL.LogMessage("Versions", AssName + " Assembly name is missing, cannot determine version");
                    }
                    else
                    {
                        Vers = AssblyName.Version;
                        if (Vers is null)
                        {
                            TL.LogMessage("Versions", AssName + " Assembly version is missing, cannot determine version");
                        }
                        else
                        {
                            VerString = Vers.ToString();
                            if (!string.IsNullOrEmpty(VerString))
                            {
                                TL.LogMessage("Versions", AssName + " AssemblyVersion: " + VerString);
                            }
                            else
                            {
                                TL.LogMessage("Versions", AssName + " Assembly version string is null or empty, cannot determine assembly version");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX1: " + ex.ToString());
                }

                try
                {
                    Location = Ass.Location;
                    if (string.IsNullOrEmpty(Location))
                    {
                        TL.LogMessage("Versions", AssName + "Assembly location is missing, cannot determine file version");
                    }
                    else
                    {
                        FileVer = FileVersionInfo.GetVersionInfo(Location);
                        if (FileVer is null)
                        {
                            TL.LogMessage("Versions", AssName + " File version object is null, cannot determine file version number");
                        }
                        else
                        {
                            FVer = FileVer.FileVersion;
                            if (!string.IsNullOrEmpty(FVer))
                            {
                                TL.LogMessage("Versions", AssName + " FileVersion: " + FVer);
                            }
                            else
                            {
                                TL.LogMessage("Versions", AssName + " File version string is null or empty, cannot determine file version");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX2: " + ex.ToString());
                }

                try
                {
                    AssblyName = Ass.GetName();
                    if (AssblyName is null)
                    {
                        TL.LogMessage("Versions", AssName + " Assembly name is missing, cannot determine full name");
                    }
                    else
                    {
                        FName = AssblyName.FullName;
                        if (!string.IsNullOrEmpty(FName))
                        {
                            TL.LogMessage("Versions", AssName + " Name: " + FName);
                        }
                        else
                        {
                            TL.LogMessage("Versions", AssName + " Full name string is null or empty, cannot determine full name");
                        }

                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX3: " + ex.ToString());
                }

                try
                {
                    TL.LogMessage("Versions", AssName + " CodeBase: " + Ass.GetName().CodeBase);
                }
                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX4: " + ex.ToString());
                }

                try
                {
                    if (!string.IsNullOrEmpty(Location))
                    {
                        TL.LogMessage("Versions", AssName + " Location: " + Location);
                    }
                    else
                    {
                        TL.LogMessage("Versions", AssName + " Location is null or empty, cannot display location");
                    }
                }

                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX5: " + ex.ToString());
                }

                try
                {
                    TL.LogMessage("Versions", AssName + " From GAC: " + Ass.GlobalAssemblyCache.ToString());
                }
                catch (Exception ex)
                {
                    TL.LogMessage("AssemblyInfo", "Exception EX6: " + ex.ToString());
                }
            }
            else
            {
                TL.LogMessage("Versions", AssName + " No assembly found");
            }
        }

        /// <summary>
        /// Returns true when the application is 32bit and running on a 64bit OS
        /// </summary>
        /// <returns>True when the application is 32bit and running on a 64bit OS</returns>
        /// <remarks></remarks>
        private static bool IsWow64()
        {
            IntPtr value;
            value = Process.GetCurrentProcess().Handle;

            var retVal = default(bool);
            if (IsWow64Process(value, ref retVal))
            {
                return retVal;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified process is running under WOW64 i.e. is a 32bit application running on a 64bit OS.
        /// </summary>
        /// <param name="hProcess">A handle to the process. The handle must have the PROCESS_QUERY_INFORMATION or PROCESS_QUERY_LIMITED_INFORMATION access right. 
        /// For more information, see Process Security and Access Rights.Windows Server 2003 and Windows XP:  
        /// The handle must have the PROCESS_QUERY_INFORMATION access right.</param>
        /// <param name="wow64Process">A pointer to a value that is set to TRUE if the process is running under WOW64. If the process is running under 
        /// 32-bit Windows, the value is set to FALSE. If the process is a 64-bit application running under 64-bit Windows, the value is also set to FALSE.</param>
        /// <returns>If the function succeeds, the return value is a nonzero value. If the function fails, the return value is zero. To get extended 
        /// error information, call GetLastError.</returns>
        /// <remarks></remarks>
        [DllImport("Kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        private static extern bool IsWow64Process(IntPtr hProcess, ref bool wow64Process);

        #endregion

        #region Driver Compatibility Checks

        /// <summary>
        /// Return a message when a driver is not compatible with the requested 32/64bit application type. Returns an empty string if the driver is compatible
        /// </summary>
        /// <param name="progID">ProgID of the driver to be assessed</param>
        /// <param name="applicationBitness">Application bitness for which application compatibility should be tested</param>
        /// <param name="TL">Trace logger for debugging</param>
        /// <returns>String compatibility message or empty string if driver is fully compatible</returns>
        /// <remarks></remarks>
        internal static string DriverCompatibilityMessage(string progID, Bitness applicationBitness, TraceLogger TL)
        {
            PEReader inProcServer = null;
            bool isRegistered64Bit;
            Bitness inprocServerBitness;
            RegistryKey RK, rkInprocServer32;
            string clsId, inprocFilePath, codeBase;
            RegistryKey RK32 = null;
            RegistryKey RK64 = null;
            string assemblyFullName;
            Assembly loadedAssembly;
            PortableExecutableKinds peKind;
            ImageFileMachine machine;
            Module[] modules;

            string CompatibilityMessage = ""; // Set default return value as OK

            TL.LogMessage("DriverCompatibility", $"     ProgID: {progID}, Testing for compatibility with application Bitness: {applicationBitness} on an OS with bitness: {OSBits()}");

            // Parse the COM registry section to determine whether this ProgID is an in-process DLL server.
            // If it is then parse the executable to determine whether it is a 32bit only driver and give a suitable message if it is
            // Picks up some COM registration issues as well as a by-product.

            // Confirm that the ProgID registry entry exists
            RK = Registry.ClassesRoot.OpenSubKey(progID); // Same path for both 32bit and 64bit applications because they share a single part of the HKCR hive.
            if (RK is not null) // ProgID is registered!
            {
                TL.LogMessage("DriverCompatibility", $"     Found the ProgID registry key for {progID}");

                // CLose the key so that OS handles are not retained unnecessarily
                RK.Close();

                // Now try to get the CLSID entry
                RK = Registry.ClassesRoot.OpenSubKey(progID + @"\CLSID"); // Same path for both 32bit and 64bit applications because they share a single part of the HKCR hive.

                // Check whether we found the CLSID
                if (RK is not null) // There is a CLSID for this ProgID!
                {
                    // Get the CLSID for this ProgID
                    clsId = RK.GetValue("").ToString();
                    RK.Close();

                    TL.LogMessage("DriverCompatibility", $"     Found the ProgID\\CLSID registry key for {progID}: {clsId}");

                    if (applicationBitness == Bitness.Bits64) // We are making a 64bit application check so assume we are on a 64bit OS and check to see whether this is a 32bit only driver
                    {
                        RK = CreateClsidKey(clsId, RegistryAccessRights.Wow64_64Key, TL); // Check the 64bit registry section for this CLSID
                        if (RK is null) // We don't have an entry in the 64bit CLSID registry section so try the 32bit section
                        {
                            TL.LogMessage("DriverCompatibility", "     No entry in the 64bit registry, checking the 32bit registry");
                            RK = CreateClsidKey(clsId, RegistryAccessRights.Wow64_32Key, TL); // Check the 32bit registry section
                            isRegistered64Bit = false;
                        }
                        else // Found entry in the 64bit section
                        {
                            TL.LogMessage("DriverCompatibility", "     Found entry in the 64bit registry");
                            isRegistered64Bit = true;
                        }

                        if (RK is not null) // We have a CLSID entry so process it
                        {
                            rkInprocServer32 = RK.OpenSubKey("InprocServer32");
                            RK.Close();
                            if (rkInprocServer32 is not null) // This is an in process server so test for compatibility
                            {
                                inprocFilePath = rkInprocServer32.GetValue("", "").ToString(); // Get the file location from the default position
                                codeBase = rkInprocServer32.GetValue("CodeBase", "").ToString(); // Get the codebase if present to override the default value
                                if (!string.IsNullOrEmpty(codeBase))
                                    inprocFilePath = codeBase;

                                if (Strings.Trim(inprocFilePath).ToUpperInvariant() == "MSCOREE.DLL") // We have an assembly, most likely in the GAC so get the actual file location of the assembly
                                {
                                    // If this assembly is in the GAC, we should have an "Assembly" registry entry with the full assembly name, 
                                    TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL");

                                    assemblyFullName = rkInprocServer32.GetValue("Assembly", "").ToString(); // Get the full name
                                    TL.LogMessage("DriverCompatibility", "     Found full name: " + assemblyFullName);
                                    if (!string.IsNullOrEmpty(assemblyFullName)) // We did get an assembly full name so now try and load it to the reflection only context
                                    {
                                        try
                                        {
                                            loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName);
                                            // OK that went well so we have an MSIL version!
                                            inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                            TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " + inprocFilePath);
                                            TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " + loadedAssembly.FullName + " ");
                                            modules = loadedAssembly.GetLoadedModules();
                                            modules[0].GetPEKind(out peKind, out machine);
                                            if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit");
                                            if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus");
                                            if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly");
                                            if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable");
                                        }

                                        catch (IOException ex)
                                        {
                                            // That failed so try to load an x86 version
                                            TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " + ex.Message);

                                            try
                                            {
                                                loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName + ", processorArchitecture=x86");
                                                // OK that went well so we have an x86 only version!
                                                inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                                TL.LogMessage("DriverCompatibilityX86", "     Found file path: " + inprocFilePath);
                                                modules = loadedAssembly.GetLoadedModules();
                                                modules[0].GetPEKind(out peKind, out machine);
                                                if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit");
                                                if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus");
                                                if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly");
                                                if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable");
                                            }

                                            catch (IOException ex1)
                                            {
                                                // That failed so try to load an x64 version
                                                TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " + ex.Message);

                                                try
                                                {
                                                    loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName + ", processorArchitecture=x64");
                                                    // OK that went well so we have an x64 only version!
                                                    inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                                    TL.LogMessage("DriverCompatibilityX64", "     Found file path: " + inprocFilePath);
                                                    modules = loadedAssembly.GetLoadedModules();
                                                    modules[0].GetPEKind(out peKind, out machine);
                                                    if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit");
                                                    if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus");
                                                    if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly");
                                                    if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable");
                                                }
                                                catch (Exception)
                                                {
                                                    // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                                    TL.LogMessageCrLf("DriverCompatibilityX64", ex1.ToString());
                                                }
                                            }

                                            catch (Exception ex1)
                                            {
                                                // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                                TL.LogMessageCrLf("DriverCompatibilityX32", ex1.ToString());
                                            }
                                        }

                                        catch (Exception ex)
                                        {
                                            // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                            TL.LogMessageCrLf("DriverCompatibility", ex.ToString());
                                        }
                                    }
                                    else
                                    {
                                        // No Assembly entry so we can't load the assembly, we'll just have to take a chance!
                                        TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!");
                                        inprocFilePath = ""; // Set to null to bypass tests
                                        TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string");
                                    }
                                }

                                if (Strings.Right(Strings.Trim(inprocFilePath), 4).ToUpperInvariant() == ".DLL") // We have a path to the server and it is a dll
                                {
                                    // We have an assembly or other technology DLL, outside the GAC, in the file system
                                    try
                                    {
                                        inProcServer = new PEReader(inprocFilePath, TL); // Get hold of the executable so we can determine its characteristics
                                        inprocServerBitness = inProcServer.BitNess;
                                        if (inprocServerBitness == Bitness.Bits32) // 32bit driver executable
                                        {
                                            if (isRegistered64Bit) // 32bit driver executable registered in 64bit COM
                                            {
                                                CompatibilityMessage = "This 32bit only driver won't work in a 64bit application even though it is registered as a 64bit COM driver." + Microsoft.VisualBasic.Constants.vbCrLf + DRIVER_AUTHOR_MESSAGE_DRIVER;
                                            }
                                            else // 32bit driver executable registered in 32bit COM
                                            {
                                                CompatibilityMessage = "This 32bit only driver won't work in a 64bit application even though it is correctly registered as a 32bit COM driver." + Microsoft.VisualBasic.Constants.vbCrLf + DRIVER_AUTHOR_MESSAGE_DRIVER;
                                            }
                                        }
                                        else if (isRegistered64Bit) // 64bit driver
                                                                    // 64bit driver executable registered in 64bit COM section
                                        {
                                        }
                                        // This is the only OK combination, no message for this!
                                        else // 64bit driver executable registered in 32bit COM
                                        {
                                            CompatibilityMessage = "This 64bit capable driver is only registered as a 32bit COM driver." + Microsoft.VisualBasic.Constants.vbCrLf + DRIVER_AUTHOR_MESSAGE_INSTALLER;
                                        }
                                    }
                                    catch (FileNotFoundException) // Cannot open the file
                                    {
                                        CompatibilityMessage = "Cannot find the driver executable: " + Microsoft.VisualBasic.Constants.vbCrLf + "\"" + inprocFilePath + "\"";
                                    }
                                    catch (Exception ex) // Some other exception so log it
                                    {
                                        LogEvent("CompatibilityMessage", "Exception parsing " + progID + ", \"" + inprocFilePath + "\"", EventLogEntryType.Error, EventLogErrors.DriverCompatibilityException, ex.ToString());
                                        CompatibilityMessage = "PEReader Exception, please check ASCOM application Event Log for details";
                                    }

                                    if (inProcServer is not null) // Clean up the PEReader class
                                    {
                                        inProcServer.Dispose();
                                    }
                                }
                                else
                                {
                                    // No codebase so can't test this driver, don't give an error message, just have to take a chance!
                                    TL.LogMessage("DriverCompatibility", "No codebase so can't test this driver, don't give an error message, just have to take a chance!");
                                }
                                rkInprocServer32.Close(); // Clean up the InProcServer registry key
                            }
                            else
                            {
                                // Please leave this empty clause here so the logic is clear!
                            } // This is not an in-process DLL so no need to test further and no error message to return
                        }
                        else // Cannot find a CLSID entry
                        {
                            CompatibilityMessage = "Unable to find a CLSID entry for this driver (64bit application), please re-install.";
                        }
                    } // We are making a 64bit application check so assume we are on a 64bit OS and check to see whether this is a 32bit only driver
                    else // We are running a 32bit application test so make sure the executable is not 64bit only
                    {
                        // Test if we are on a 64bit OS
                        if (OSBits() == Bitness.Bits64) // 64bit OS - Test as a 32bit app on a 64bit OS
                        {
                            try { RK32 = CreateClsidKey(clsId, RegistryAccessRights.Wow64_32Key, TL); } catch (Exception) { } // Ignore any exceptions, they just mean the operation wasn't successful
                            try { RK64 = CreateClsidKey(clsId, RegistryAccessRights.Wow64_64Key, TL); } catch (Exception) { } // Ignore any exceptions, they just mean the operation wasn't successful                            

                            TL.LogMessage("DriverCompatibility", "     Running on a 64bit OS, 32bit Registered: " + (RK32 is not null) + ", 64Bit Registered: " + (RK64 is not null));
                            if (RK32 is not null) // We are testing as a 32bit app so, if there is a 32bit key, return this
                                RK = RK32;
                            else // Otherwise return the 64bit key
                                RK = RK64;
                        }// 64bit OS - Test as a 32bit app on a 64bit OS
                        else // 32bit OS - Test as a 32bit app
                        {
                            RK = CreateClsidKey(clsId, RegistryAccessRights.Wow64_32Key, TL); // Check the 32bit registry section for this CLSID
                            TL.LogMessage("DriverCompatibility", "     Running on a 32bit OS, Is 32Bit Registered: " + (RK is not null));
                        } // We are running on a 32bit OS

                        if (RK is not null) // We have a CLSID entry so process it
                        {
                            TL.LogMessage("DriverCompatibility", "     Found CLSID entry");
                            rkInprocServer32 = RK.OpenSubKey("InprocServer32");
                            RK.Close();

                            if (rkInprocServer32 is not null) // This is an in process server so test for compatibility
                            {
                                inprocFilePath = rkInprocServer32.GetValue("", "").ToString(); // Get the file location from the default position
                                codeBase = rkInprocServer32.GetValue("CodeBase", "").ToString(); // Get the codebase if present to override the default value
                                if (!string.IsNullOrEmpty(codeBase))
                                    inprocFilePath = codeBase;

                                if (Strings.Trim(inprocFilePath).ToUpperInvariant() == "MSCOREE.DLL") // We have an assembly, most likely in the GAC so get the actual file location of the assembly
                                {
                                    // If this assembly is in the GAC, we should have an "Assembly" registry entry with the full assembly name, 
                                    TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL");

                                    assemblyFullName = rkInprocServer32.GetValue("Assembly", "").ToString(); // Get the full name
                                    TL.LogMessage("DriverCompatibility", "     Found full name: " + assemblyFullName);
                                    if (!string.IsNullOrEmpty(assemblyFullName)) // We did get an assembly full name so now try and load it to the reflection only context
                                    {
                                        try
                                        {
                                            loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName);
                                            // OK that went well so we have an MSIL version!
                                            inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                            TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " + inprocFilePath);
                                            TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " + loadedAssembly.FullName + " ");
                                            modules = loadedAssembly.GetLoadedModules();
                                            modules[0].GetPEKind(out peKind, out machine);
                                            if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit");
                                            if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus");
                                            if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly");
                                            if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable");
                                        }
                                        catch (IOException ex)
                                        {
                                            // That failed so try to load an x86 version
                                            TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " + ex.Message);

                                            try
                                            {
                                                loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName + ", processorArchitecture=x86");
                                                // OK that went well so we have an x86 only version!
                                                inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                                TL.LogMessage("DriverCompatibilityX86", "     Found file path: " + inprocFilePath);
                                                modules = loadedAssembly.GetLoadedModules();
                                                modules[0].GetPEKind(out peKind, out machine);
                                                if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit");
                                                if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus");
                                                if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly");
                                                if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                    TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable");
                                            }
                                            catch (IOException ex1)
                                            {
                                                // That failed so try to load an x64 version
                                                TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " + ex.Message);

                                                try
                                                {
                                                    loadedAssembly = Assembly.ReflectionOnlyLoad(assemblyFullName + ", processorArchitecture=x64");
                                                    // OK that went well so we have an x64 only version!
                                                    inprocFilePath = loadedAssembly.CodeBase; // Get the codebase for testing below
                                                    TL.LogMessage("DriverCompatibilityX64", "     Found file path: " + inprocFilePath);
                                                    modules = loadedAssembly.GetLoadedModules();
                                                    modules[0].GetPEKind(out peKind, out machine);
                                                    if ((peKind & PortableExecutableKinds.Required32Bit) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit");
                                                    if ((peKind & PortableExecutableKinds.PE32Plus) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus");
                                                    if ((peKind & PortableExecutableKinds.ILOnly) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly");
                                                    if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != 0)
                                                        TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable");
                                                }
                                                catch (Exception)
                                                {
                                                    // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                                    TL.LogMessageCrLf("DriverCompatibilityX64", ex1.ToString());
                                                }
                                            }
                                            catch (Exception ex1)
                                            {
                                                // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                                TL.LogMessageCrLf("DriverCompatibilityX32", ex1.ToString());
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                            TL.LogMessageCrLf("DriverCompatibility", ex.ToString());
                                        }
                                    } // We did get an assembly full name so now try and load it to the reflection only context
                                    else // Not an assembly so can't test
                                    {
                                        // No Assembly entry so we can't load the assembly, we'll just have to take a chance!
                                        TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!");
                                        inprocFilePath = ""; // Set to null to bypass tests
                                        TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string");
                                    } // Not an assembly so can't test
                                }// We have an assembly, most likely in the GAC so get the actual file location of the assembly

                                if (Strings.Right(Strings.Trim(inprocFilePath), 4).ToUpperInvariant() == ".DLL") // We do have a path to the server and it is a dll
                                {
                                    // We have an assembly or other technology DLL, outside the GAC, in the file system
                                    try
                                    {
                                        inProcServer = new PEReader(inprocFilePath, TL); // Get hold of the executable so we can determine its characteristics
                                        if (inProcServer.BitNess == Bitness.Bits64) // 64bit only driver executable
                                        {
                                            CompatibilityMessage = "This is a 64bit only driver and is not compatible with this 32bit application." + Microsoft.VisualBasic.Constants.vbCrLf + DRIVER_AUTHOR_MESSAGE_DRIVER;
                                        } // 64bit only driver executable
                                    }
                                    catch (FileNotFoundException) // Cannot open the file
                                    {
                                        CompatibilityMessage = "Cannot find the driver executable: " + Microsoft.VisualBasic.Constants.vbCrLf + "\"" + inprocFilePath + "\"";
                                    }
                                    catch (Exception ex) // Some other exception so log it
                                    {
                                        LogEvent("CompatibilityMessage", "Exception parsing " + progID + ", \"" + inprocFilePath + "\"", EventLogEntryType.Error, EventLogErrors.DriverCompatibilityException, ex.ToString());
                                        CompatibilityMessage = "PEReader Exception, please check ASCOM application Event Log for details";
                                    }

                                    if (inProcServer is not null) // Clean up the PEReader class
                                    {
                                        inProcServer.Dispose();
                                    } // Clean up the PEReader class
                                }// We do have a path to the server and it is a dll
                                else
                                {
                                    // No codebase or not a DLL so can't test this driver, don't give an error message, just have to take a chance!
                                    TL.LogMessage("DriverCompatibility", "No codebase or not a DLL so can't test this driver, don't give an error message, just have to take a chance!");
                                } // Process as a native executable

                                rkInprocServer32.Close(); // Clean up the InProcServer registry key
                            } // This is an in process server so test for compatibility
                            else // This is not an in-process DLL so no need to test further and no error message to return
                            {
                                CompatibilityMessage = "";
                                TL.LogMessage("DriverCompatibility", "     This is not an in-process DLL so no need to test further and no error message to return");
                            } // This is not an in-process DLL so no need to test further and no error message to return
                        } // We have a CLSID entry so process it
                        else // Cannot find a CLSID entry
                        {
                            // Check whether this is a 64bit local server driver running on a 64bit OS
                            if (OSBits() == Bitness.Bits64) // This is a 64bit OS
                            {
                                TL.LogMessage("DriverCompatibility", "     64bit OS - looking for a CLSID entry");

                                // Open a 64bit view of the registry
                                using (RegistryAccess registryAccess = new RegistryAccess(TL))
                                {
                                    RK = registryAccess.OpenSubKey3264(Registry.ClassesRoot, $"CLSID\\{clsId}", false, RegistryAccessRights.Wow64_64Key);
                                }

                                if (RK != null) // Found a CLSID entry
                                {
                                    TL.LogMessage("DriverCompatibility", "     Found CLSID key (32bit application running on a 64bit OS)");

                                    // Read the LocalServer32 key default value , if present
                                    RegistryKey registryKey = RK.OpenSubKey("LocalServer32");
                                    if (registryKey != null) // LocalServer32 key exists
                                    {
                                        TL.LogMessage("DriverCompatibility", "     Found CLSID\\LocalServer32 key (32bit application running on a 64bit OS)");
                                        string localServerExe = registryKey.GetValue(null).ToString();
                                        if (!string.IsNullOrEmpty(localServerExe)) // Local server file location is present - success
                                        {
                                            TL.LogMessage("DriverCompatibility", $"     Found LocalServer32 default entry {localServerExe}, returning empty driver compatibility message");
                                            CompatibilityMessage = "";
                                        }  // Local server file location is present - success
                                        else // No local server file location
                                        {
                                            TL.LogMessage("DriverCompatibility", $"     The LocalServer32 default entry is null or empty, returning empty driver compatibility message");
                                            CompatibilityMessage = "64bit LocalServer executable path in LocalServer32 is empty (32bit application, 64bit OS), please re-install.";
                                        }  // No local server file location
                                    } // LocalServer32 key exists
                                    else // LocalServer32 key does not exist
                                    {
                                        TL.LogMessage("DriverCompatibility", $"     The LocalServer32 default entry is null or empty, returning empty driver compatibility message");
                                        CompatibilityMessage = "Unable to find a path to a 64bit LocalServer executable (32bit application, 64bit OS), please re-install.";
                                    }  // LocalServer32 key does not exist
                                } // Found a CLSID entry
                                else // Cannot find a CLSID entry
                                {
                                    CompatibilityMessage = "Unable to find a CLSID entry for this driver (32bit application, 64bit OS), please re-install.";
                                } // Cannot find a CLSID entry
                            }
                            else // This is a 32bit OS so report that the CLSID is not found
                            {
                                CompatibilityMessage = "Unable to find a CLSID entry for this driver (32bit application, 32bit OS), please re-install.";
                                TL.LogMessage("DriverCompatibility", "     Could not find CLSID entry!");
                            } // This is a 32bit OS so report that the CLSID is not found
                        } // Cannot find a CLSID entry
                    } // We are running a 32bit application test so make sure the executable is not 64bit only
                } // There is a CLSID for this ProgID!
                else // Can't find the CLSID associated with this ProgID so report this
                {
                    CompatibilityMessage = $"This driver is not correctly registered for COM - Can't find the HKCR\\{progID}\\CLSID entry, please re-install.";
                }  // Can't find the CLSID associated with this ProgID so report this
            }// ProgID is registered!
            else // ProgID does not exist
            {
                CompatibilityMessage = $"This driver is not registered for COM - Can't find the HKCR\\{progID} ProgID entry, please re-install.";
            } // ProgID does not exist

            TL.LogMessage("DriverCompatibility", "     Returning: \"" + CompatibilityMessage + "\"");
            return CompatibilityMessage;
        }

        /// <summary>
        /// Create a registry key that points at an entry in the 32 or 64bit portions of the HKCR hive as determined by the supplied bitness parameter
        /// </summary>
        /// <param name="clsId">The CLSID without a CLSID\ prefix</param>
        /// <param name="requiredBitness">Required bitness 32bit or 64bit</param>
        /// <param name="TL">Trace logger instance</param>
        /// <returns>HKCR registry key</returns>
        public static RegistryKey CreateClsidKey(string clsId, RegistryAccessRights requiredBitness, TraceLogger TL)
        {
            // Add the CLSID prefix
            clsId = $@"CLSID\{clsId}";


            using RegistryAccess registryAccess = new RegistryAccess();
            {
                try
                {
                    return registryAccess.OpenSubKey3264(Registry.ClassesRoot, clsId, false, requiredBitness);
                }
                catch (ProfilePersistenceException)
                {
                    TL.LogMessageCrLf("CreateClsidKey", $"     Key HKeyClassesRoot\\{clsId} does not exist for view {requiredBitness}");
                    return null;
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("CreateClsidKey", $"Exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region Force Platform version code

        internal static string ConditionPlatformVersion(string PlatformVersion, RegistryAccess Profile, TraceLogger TL)
        {
            string ModuleFileName, ForcedFileNameKey;
            SortedList<string, string> ForcedFileNames, ForcedSeparators;
            PerformanceCounter PC;

            string ConditionPlatformVersionRet = PlatformVersion;
            try
            {
                ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName); // Get the name of the executable without path or file extension
                if (TL is not null)
                    TL.LogMessage("ConditionPlatformVersion", "  ModuleFileName: \"" + ModuleFileName + "\" \"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
                if (Strings.Left(ModuleFileName.ToUpperInvariant(), 3) == "IS-") // Likely to be an old Inno installer so try and get the parent's name
                {
                    if (TL is not null)
                        TL.LogMessage("ConditionPlatformVersion", "    Inno installer temporary executable detected, searching for parent process!");
                    if (TL is not null)
                        TL.LogMessage("ConditionPlatformVersion", "    Old Module Filename: " + ModuleFileName);
                    PC = new PerformanceCounter("Process", "Creating Process ID", Process.GetCurrentProcess().ProcessName);
                    ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetProcessById((int)Math.Round(PC.NextValue())).MainModule.FileName);
                    if (TL is not null)
                        TL.LogMessage("ConditionPlatformVersion", "    New Module Filename: " + ModuleFileName);
                    PC.Close();
                    PC.Dispose();
                }

                // Force any particular platform version number this application requires
                ForcedFileNames = Profile.EnumProfile(PLATFORM_VERSION_EXCEPTIONS); // Get the list of filenames requiring specific versions

                foreach (KeyValuePair<string, string> ForcedFileName in ForcedFileNames) // Check each forced file in turn 
                {
                    if (TL is not null)
                        TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: \"" + ForcedFileName.Key + "\" \"" + ForcedFileName.Value + "\" \"" + Strings.UCase(Path.GetFileNameWithoutExtension(ForcedFileName.Key)) + "\" \"" + Strings.UCase(Path.GetFileName(ForcedFileName.Key)) + "\" \"" + Strings.UCase(ForcedFileName.Key) + "\" \"" + ForcedFileName.Key + "\" \"" + Strings.UCase(ModuleFileName) + "\"");
                    if (ForcedFileName.Key.Contains("."))
                    {
                        ForcedFileNameKey = Path.GetFileNameWithoutExtension(ForcedFileName.Key);
                    }
                    else
                    {
                        ForcedFileNameKey = ForcedFileName.Key;
                    }

                    // If the current file matches a forced file name then return the required Platform version
                    // 6.0 SP1 Check now uses StartsWith in order to catch situations where people rename the installer after download
                    if (!string.IsNullOrEmpty(ForcedFileNameKey)) // Ignore the empty string "Default" value name
                    {
                        if (ModuleFileName.StartsWith(ForcedFileNameKey, StringComparison.OrdinalIgnoreCase))
                        {
                            ConditionPlatformVersionRet = ForcedFileName.Value;
                            if (TL is not null)
                                TL.LogMessage("ConditionPlatformVersion", "  Matched file: \"" + ModuleFileName + "\" \"" + ForcedFileNameKey + "\"");
                        }
                    }
                }

                ForcedSeparators = Profile.EnumProfile(PLATFORM_VERSION_SEPARATOR_EXCEPTIONS); // Get the list of filenames requiring specific versions

                foreach (KeyValuePair<string, string> ForcedSeparator in ForcedSeparators) // Check each forced file in turn 
                {
                    if (TL is not null)
                        TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: \"" + ForcedSeparator.Key + "\" \"" + ForcedSeparator.Value + "\" \"" + Strings.UCase(Path.GetFileNameWithoutExtension(ForcedSeparator.Key)) + "\" \"" + Strings.UCase(Path.GetFileName(ForcedSeparator.Key)) + "\" \"" + Strings.UCase(ForcedSeparator.Key) + "\" \"" + ForcedSeparator.Key + "\" \"" + Strings.UCase(ModuleFileName) + "\"");
                    if (ForcedSeparator.Key.Contains("."))
                    {
                    }
                    else
                    {
                    }

                    if ((Strings.UCase(Path.GetFileNameWithoutExtension(ForcedSeparator.Key)) ?? "") == (Strings.UCase(ModuleFileName) ?? "")) // If the current file matches a forced file name then return the required Platform version
                    {
                        if (string.IsNullOrEmpty(ForcedSeparator.Value)) // Replace with the current locale decimal separator
                        {
                            ConditionPlatformVersionRet = ConditionPlatformVersionRet.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            if (TL is not null)
                                TL.LogMessage("ConditionPlatformVersion", "  String IsNullOrEmpty: \"" + ConditionPlatformVersionRet + "\"");
                        }
                        else // Replace with the fixed value provided
                        {
                            ConditionPlatformVersionRet = ConditionPlatformVersionRet.Replace(".", ForcedSeparator.Value);
                            if (TL is not null)
                                TL.LogMessage("ConditionPlatformVersion", "  String Is: \"" + ForcedSeparator.Value + "\" \"" + ConditionPlatformVersionRet + "\"");
                        }

                        if (TL is not null)
                            TL.LogMessage("ConditionPlatformVersion", "  Matched file: \"" + ModuleFileName + "\" \"" + ForcedSeparator.Key + "\"");
                    }
                }
            }

            catch (Exception ex)
            {
                if (TL is not null)
                    TL.LogMessageCrLf("ConditionPlatformVersion", "Exception: " + ex.ToString());
                LogEvent("ConditionPlatformVersion", "Exception: ", EventLogEntryType.Error, EventLogErrors.VB6HelperProfileException, ex.ToString());
            }
            if (TL is not null)
                TL.LogMessage("ConditionPlatformVersion", "  Returning: \"" + ConditionPlatformVersionRet + "\"");
            return ConditionPlatformVersionRet;
        }

    }

    #endregion

}
