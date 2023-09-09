using System;
using System.Diagnostics;
using System.IO;

namespace FinaliseInstall
{
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

    static class Global
    {
        // Settings for the ASCOM Windows event log
        internal const string EVENT_SOURCE = "ASCOM Platform"; // Name of the the event source
        internal const string EVENTLOG_NAME = "ASCOM"; // Name of the event log as it appears in Windows event viewer
        internal const string EVENTLOG_MESSAGES = @"ASCOM\EventLogMessages.txt";
        internal const string EVENTLOG_ERRORS = @"ASCOM\EventLogErrors.txt";

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
                if (!(Except is null))
                    MsgTxt += "\r\n" + Except;
                ELog.WriteEntry(MsgTxt, Severity, (int)Id); // Write the message to the error log

                ELog.Close();
                ELog.Dispose();
            }
            catch (System.ComponentModel.Win32Exception ex) // Special handling because these exceptions contain error codes we may want to know
            {
                try
                {
                    string TodaysDateTime = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss.fff");
                    string ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_ERRORS;
                    string MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_MESSAGES;

                    // Write to backup eventlog message and error logs
                    File.AppendAllText(ErrorLog, TodaysDateTime + " ErrorCode: 0x" + ex.ErrorCode.ToString("X8") + " NativeErrorCode: 0x" + ex.NativeErrorCode.ToString("X8") + " " + ex.ToString() + "\r\n");
                    File.AppendAllText(MessageLog, TodaysDateTime + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + "\r\n");
                }
                catch (Exception ex1) // Ignore exceptions here, the PC seems to be in a catastrophic failure!
                {

                }
            }
            catch (Exception ex) // Catch all other exceptions
            {
                // Somthing bad happened when writing to the event log so try and log it in a log file on the file system
                try
                {
                    string TodaysDateTime = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss.fff");
                    string ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_ERRORS;
                    string MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + EVENTLOG_MESSAGES;

                    // Write to backup eventlog message and error logs
                    File.AppendAllText(ErrorLog, TodaysDateTime + " " + ex.ToString() + "\r\n");
                    File.AppendAllText(MessageLog, TodaysDateTime + " " + Caller + " " + Msg + " " + Severity.ToString() + " " + Id.ToString() + " " + Except + "\r\n");
                }
                catch (Exception ex1) // Ignore exceptions here, the PC seems to be in a catastrophic failure!
                {

                }
            }
        }

    }
}
