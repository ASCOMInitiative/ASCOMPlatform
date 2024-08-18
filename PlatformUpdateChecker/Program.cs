using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using Semver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Runtime.InteropServices;

namespace PlatformUpdateChecker
{
    internal class Program
    {
        #region Constants

        // Name of the Scheduler Task process, this should be the same on all Windows versions regardless of localisation
        internal const string SCHEDULER_SERVICE_NAME = "Schedule";

        // Task name and task location in the Root folder
        internal const string UPDATE_TASK_NAME = "ASCOM - Platform Update Check"; // Name of the schedule job that runs the update check
        internal const string UPDATE_TASK_FULL_PATH = @"\" + UPDATE_TASK_NAME; // Full schedule job path within the scheduler job tree. Has to be in the root for backward compatibility with XP!

        // Toast button parameter names
        private const string COMMAND_DOWNLOAD = "Download";
        private const string COMMAND_REMIND_LATER = "RemindLater";
        private const string COMMAND_SKIP_RELEASE = "SkipRelease";
        private const string COMMAND_HELP_MINUS_MINUS = "--help";
        private const string COMMAND_HELP_SLASH_H = "/h";

        #endregion

        // Trace logger
        static TraceLogger TL;

        /// <summary>
        /// Main entry point for command and event response initiations
        /// </summary>
        /// <param name="args">Supplied arguments</param>
        /// <returns>Error code (always 0)</returns>
        static int Main(string[] args)
        {
            string arg = "";

            // Create the trace logger
            TL = new TraceLogger("PlatformUpdateChecker")
            {
                Enabled = true
            };

            // Add a listener for activation notifications
            LogMessage("Main", $"Adding OnActivated event handler...");
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                LogMessage("OnActivated", $"Entered - Arguments: {toastArgs.Argument}");

                string[] arguments = toastArgs.Argument.Split();
                if (arguments.Length > 0)
                {
                    switch (arguments[0])
                    {
                        case COMMAND_DOWNLOAD:
                            Download();
                            break;

                        case COMMAND_REMIND_LATER:
                            RemindLater();
                            break;

                        case COMMAND_SKIP_RELEASE:
                            if (arguments.Length > 1)
                                SkipRelease(arguments[1]);
                            break;

                        default:
                            break;
                    }
                }
                LogMessage("OnActivated", $"Exited");
            };
            LogMessage("Main", $"Added OnActivated event handler");

            // We only support 1 command line argument so extract the first argument if present
            if (args.Length > 0)
                arg = args[0];

            LogMessage("Main", $"Supplied argument: {arg}");

            // Check whether this was activated by the user or scheduler or whether it was activated by the Toast system
            if (!ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) // Activated by the user or scheduler
            {
                // Handle the supplied argument
                switch (arg.Trim().ToUpperInvariant())
                {
                    // Provide explanatory help to a command line user
                    case "--HELP":
                    case "/H":
                        MessageBox.Show($"Platform Update Checker - Queries the list of ASCOM Platform GitHub releases to determine whether a later Platform version is available.\r\n\r\n" +
                                        $"Supported commands: /InstallTask, /RemoveTask, /Version, /ResetSkipped, /CheckForUpdates\r\n\r\n" +
                                        @"Output is written to a log file in the Documents\ASCOM\Logs YYYY.MM.DD folder where YYYY, MM and DD are the current year, month and day numbers.",
                                        "Platform Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    // Install the scheduled update check task
                    case "/INSTALLTASK":
                        CreateScheduledTask();
                        break;

                    // Uninstall the scheduled update check task
                    case "/REMOVETASK":
                        RemoveScheduledTask();
                        break;

                    // List the current Platform version
                    case "/VERSION":
                        ReportVersion();
                        break;

                    // Reset the list of skipped Platform versions
                    case "/RESETSKIPPED":
                        ResetSkipped();
                        break;

                    // No argument supplied or unknown argument so check to see if there are any updates and create a toast notification if required
                    default:
                        CheckForUpdates();
                        break;
                }
            }
            else // Activated by the Toast system
            {
                // No actions here because Toast events are handled in the ToastNotificationManagerCompat.OnActivated event handler

                // Wait for a short time to allow any ToastNotificationManagerCompat.OnActivated events to fire
                LogMessage("Main", $"Staring wait so that any ToastNotificationManagerCompat.OnActivated events can fire...");
                System.Threading.Thread.Sleep(1000);
                LogMessage("Main", $"Wait completed - application finishing.");
            }

            return 0;
        }

        #region Command handlers

        /// <summary>
        /// Check GitHub for Platform updates and notify the user via a Toast if any are available and this application is configured to do so.
        /// </summary>
        private static void CheckForUpdates()
        {
            // Check for updates specifying the raiseToast action to be called if an update is found
            UpdateCheck.CheckForUpdates((x) => RaiseToast(x), TL);
        }

        /// <summary>
        /// Handler for the "Reset the list of skipped Platform updates" command line command
        /// </summary>
        private static void ResetSkipped()
        {
            try
            {
                LogMessage("ResetSkipped", $"Resetting skipped Platform versions...");

                // Ask the user whether they really want to delete the skipped Platform list
                DialogResult result = MessageBox.Show("Are you sure you want to reset skipped Platform versions?", "Reset Skipped Platforms", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                // Check the dialogue outcome
                if (result == DialogResult.Yes) // User said yes so delete the list of skipped Platforms
                {
                    // Reset the list of skipped releases
                    UpdateCheck.ResetSkippedReleases();

                    // Confirm that the list has been reset
                    LogMessage("ResetSkipped", $"Skipped Platform versions reset successfully");
                    MessageBox.Show("Skipped Platform versions reset.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("ResetSkipped", $"Exception resetting skipped versions: {ex.Message}\r\n{ex}");
            }
        }

        /// <summary>
        /// Handler for the command line Report the current Platform version command
        /// </summary>
        private static void ReportVersion()
        {
            string version = UpdateCheck.GetCurrentPlatformVersion();
            LogMessage("ReportVersion", $"The current Platform version is: {version}");
            MessageBox.Show($"Currently installed Platform version: {version}","Platform Version");

            SemVersion semver = SemVersion.Parse(version, SemVersionStyles.Any);
            LogMessage("ReportVersion", $"SemVersion Major: {semver.Major}, Minor: {semver.Minor}, Patch: {semver.Patch}, IsPreRelease: {semver.IsPrerelease}, Prerelease: {semver.Prerelease}, Metadata: {semver.Metadata}");
        }

        /// <summary>
        /// Handler for Install Task command line command - Create or update the Platform update task in the Windows scheduler
        /// </summary>
        private static void CreateScheduledTask()
        {
            TaskDefinition taskDefinition;
            WeeklyTrigger weeklyTrigger;
            string executableName;

            DateTime startDate = DateTime.Today.AddHours(8); // Set the start time to today at 08:00

            try
            {
                LogMessage("CreateScheduledTask", "Testing whether scheduler is running");

                // Test whether the Task Scheduler Service is running so that the ASCOM task can be managed
                using (var serviceController = new ServiceController(SCHEDULER_SERVICE_NAME)) // Create a new service controller for the scheduler service
                {

                    if (serviceController.Status == ServiceControllerStatus.Running) // The scheduler is running normally so proceed with creating or updating the task
                    {
                        LogMessage("CreateScheduledTaskRemov", $"Scheduler service is running OK - status: {serviceController.Status}. Obtaining Scheduler information...");
                        using (var service = new TaskService())
                        {
                            LogMessage("CreateScheduledTask", string.Format("Highest supported scheduler version: {0}, Library version: {1}, Connected: {2}", service.HighestSupportedVersion, TaskService.LibraryVersion, service.Connected));

                            // List current task state if any
                            var ASCOMTask = service.GetTask(UPDATE_TASK_FULL_PATH);
                            if (!(ASCOMTask is null))
                            {
                                LogMessage("CreateScheduledTask", string.Format("Found ASCOM task {0} last run: {1}, State: {2}, Enabled: {3}", ASCOMTask.Path, ASCOMTask.LastRunTime, ASCOMTask.State, ASCOMTask.Enabled));
                            }
                            else
                            {
                                LogMessage("CreateScheduledTask", "ASCOM task does not exist");
                            }
                            LogMessage("", "");

                            // Get the task definition to work on, either a new one or the existing task, if it exists
                            if (!(ASCOMTask is null))
                            {
                                LogMessage("CreateScheduledTask", $"Task exists so it will be deleted.");
                                service.RootFolder.DeleteTask(UPDATE_TASK_NAME);
                                LogMessage("CreateScheduledTask", string.Format("Task {0} deleted OK.", UPDATE_TASK_NAME));
                            }
                            LogMessage("CreateScheduledTask", string.Format("{0} task will be created.", UPDATE_TASK_NAME));
                            taskDefinition = service.NewTask();

                            taskDefinition.RegistrationInfo.Description = "ASCOM scheduled job to check for Platform updates.";

                            executableName = Process.GetCurrentProcess().MainModule.FileName; // Get the full path and name of the current executable
                            LogMessage("CreateScheduledTask", $"Platform update check process full name and path: {executableName}");

                            LogMessage("CreateScheduledTask", $"Platform update check process full name and path: {executableName}");

                            taskDefinition.Actions.Clear(); // Remove any existing actions and add the current one
                            taskDefinition.Actions.Add(new ExecAction(executableName, null, null)); // Add an action that will launch the updater application whenever the trigger fires
                            LogMessage("CreateScheduledTask", string.Format("Added scheduled job action to run {0}", executableName));

                            // Add settings appropriate to the task
                            try
                            {
                                taskDefinition.Settings.AllowDemandStart = true; // Requires a V2 task library (XP is only V1)
                                taskDefinition.Settings.StartWhenAvailable = true; // ' Requires a V2 task library (XP is only V1)
                                LogMessage("CreateScheduledTask", string.Format("Successfully added V2 AllowDemandStart and StartWhenAvailable settings."));
                            }
                            catch (NotV1SupportedException) // Swallow the not supported exception on XP
                            {
                                LogMessage("CreateScheduledTask", string.Format("This machine only has a V1 task scheduler - ignoring V2 AllowDemandStart and StartWhenAvailable settings."));
                            }
                            taskDefinition.Settings.ExecutionTimeLimit = new TimeSpan(0, 10, 0);
                            taskDefinition.Settings.StopIfGoingOnBatteries = false;
                            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
                            taskDefinition.Settings.Enabled = true;
                            LogMessage("CreateScheduledTask", string.Format("Allow demand on start: {0}, Start when available: {1}, Execution time limit: {2} minutes, Stop if going on batteries: {3}, Disallow start if on batteries: {4}, Enabled: {5}, Run only if logged on: {6}", taskDefinition.Settings.AllowDemandStart, taskDefinition.Settings.StartWhenAvailable, taskDefinition.Settings.ExecutionTimeLimit.TotalMinutes, taskDefinition.Settings.StopIfGoingOnBatteries, taskDefinition.Settings.DisallowStartIfOnBatteries, taskDefinition.Settings.Enabled, taskDefinition.Settings.RunOnlyIfLoggedOn));

                            taskDefinition.Triggers.Clear(); // Remove any previous triggers and add the new trigger to the task as the only trigger

                            weeklyTrigger = new WeeklyTrigger
                            {
                                StartBoundary = startDate, // Set the start time
                                DaysOfWeek = DaysOfTheWeek.Saturday // Set the repeat day of the ween to Saturday
                            };
                            taskDefinition.Triggers.Add(weeklyTrigger);
                            LogMessage("CreateScheduledTask", $"Set trigger to repeat the job weekly starting on day {startDate}.");

                            // Implement the new task in the root folder either by updating the existing task or creating a new task
                            LogMessage("CreateScheduledTask", $"Registering the {UPDATE_TASK_NAME} task.");
                            service.RootFolder.RegisterTaskDefinition(UPDATE_TASK_NAME, taskDefinition, TaskCreation.CreateOrUpdate, "USERS", null, TaskLogonType.Group);
                            LogMessage("CreateScheduledTask", string.Format("New task registered OK."));
                        }
                    }
                    else // The task scheduler is not running so provide a message
                    {
                        string message = $"The ASCOM Platform update check scheduled task cannot be created / updated because your PC's task scheduler is in the: {serviceController.Status} state. Please ensure that this service is running correctly, then repair the ASCOM installation.";
                        LogMessage("CreateScheduledTask", message);
                        MessageBox.Show(message);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    LogMessage("CreateScheduledTask Exception", ex.ToString());
                }
                catch (Exception) { }

                try
                {
                    MessageBox.Show(@"Something went wrong with the update, please report this on the ASCOM Talk Groups.IO forum, including a zip of your entire Documents\ASCOM folder and sub-folders." + "\r\n" + ex.ToString());
                }
                catch (Exception) { }

                Console.WriteLine($"CreateScheduledTask Exception - {ex})");
            }

            LogMessage("", "");
            LogMessage("CreateScheduledTask", string.Format("Platform update check configuration changes completed."));
        }

        /// <summary>
        /// Handler for RemoveTask command line command - Removes the update checker task from the Windows scheduler.
        /// </summary>
        private static void RemoveScheduledTask()
        {
            try
            {
                // Remove any remaining toast notifications that haven't yet fired, ignoring any errors (This fails and throws an exception on Windows 7 SP1)
                try { ToastNotificationManagerCompat.Uninstall(); } catch { };

                LogMessage("RemoveScheduledTask", "Testing whether scheduler is running");

                // Test whether the Task Scheduler Service is running so that the ASCOM task can be managed
                using (var serviceController = new ServiceController(SCHEDULER_SERVICE_NAME)) // Create a new service controller for the scheduler service
                {

                    if (serviceController.Status == ServiceControllerStatus.Running) // The scheduler is running normally so proceed with creating or updating the task
                    {
                        LogMessage("RemoveScheduledTask", $"Scheduler service is running OK - status: {serviceController.Status}. Obtaining Scheduler information...");
                        using (var service = new TaskService())
                        {
                            LogMessage("RemoveScheduledTask", $"Highest supported scheduler version: {service.HighestSupportedVersion}, Library version: {TaskService.LibraryVersion}, Connected: {service.Connected}");

                            // List current task state if any
                            var ASCOMTask = service.GetTask(UPDATE_TASK_FULL_PATH);

                            if (!(ASCOMTask is null))
                            {
                                LogMessage("RemoveScheduledTask", $"Deleting ASCOM task {ASCOMTask.Path} last run: {ASCOMTask.LastRunTime}, State: {ASCOMTask.State}, Enabled: {ASCOMTask.Enabled}");
                                service.RootFolder.DeleteTask(UPDATE_TASK_NAME);
                                LogMessage("RemoveScheduledTask", $"Task {UPDATE_TASK_NAME} deleted OK.");
                            }
                            else
                            {
                                LogMessage("RemoveScheduledTask", "ASCOM task does not exist, nothing to delete");
                            }

                            LogMessage("", "");
                        }
                    }
                    else // The task scheduler is not running so provide a message
                    {
                        string message = $"The ASCOM Platform update check scheduled task cannot be created / updated because your PC's task scheduler is in the: {serviceController.Status} state. Please ensure that this service is running correctly, then repair the ASCOM installation.";
                        LogMessage("RemoveScheduledTask", message);
                        MessageBox.Show(message);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    LogMessage("ManageScheduledTask Exception", ex.ToString());
                }
                catch (Exception) { }

                try
                {
                    MessageBox.Show(@"Something went wrong with the update, please report this on the ASCOM Talk Groups.IO forum, including a zip of your entire Documents\ASCOM folder and sub-folders." + "\r\n" + ex.ToString());
                }
                catch (Exception) { }

                Console.WriteLine($"ManageScheduledTask Exception - {ex})");
            }

            LogMessage("", "");
            LogMessage("RemoveScheduledTask", string.Format("Platform update check configuration changes completed."));
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handler for the "Remind me later" button
        /// </summary>
        private static void RemindLater()
        {
            // There is no action for this command: the toast will be raised again automatically when the update check scheduled job next runs
            LogMessage("RemindLater", "Remind later was clicked!");
        }

        /// <summary>
        /// Handler for the Download button - takes the user to the GitHub releases page to download the new Platform version
        /// </summary>
        private static void Download()
        {
            LogMessage("Download", $@"Starting browser to {UpdateCheck.GetPlatformDownloadUrl()}");

            // Run the GitHub URL to open a browser window on the default browser
            Process.Start(UpdateCheck.GetPlatformDownloadUrl());
        }

        /// <summary>
        /// Handler for the "Skip this release" button
        /// </summary>
        /// <param name="releaseToSkip">The release that the user wants to skip</param>
        private static void SkipRelease(string releaseToSkip)
        {
            LogMessage("SkipRelease", $"Don't show again was clicked for version: {releaseToSkip}!");
            UpdateCheck.SkipThisRelease(releaseToSkip);
        }

        #endregion

        #region Support Code

        /// <summary>
        /// Generate a Toast pop-up notification that a new release is available
        /// </summary>
        /// <param name="newRelease">New release sem version</param>
        private static void RaiseToast(SemVersion newRelease)
        {
            string toastHeaderText;
            string toastHeaderId;
            try
            {
                LogMessage("CreateToast", "Creating toast...");

                // Create the toast message text
                StringBuilder toastMessage = new StringBuilder();

                // Create the appropriate version description depending on whether this release is a production release or a release candidate 
                if (newRelease.IsRelease) // This is a production release
                {
                    toastHeaderId = "ASCOMPlatformUpdate";
                    toastHeaderText = "ASCOM Platform - NEW RELEASE";

                    // Add the new version description
                    if (newRelease.Patch == 0) // This is not a service pack release
                        toastMessage.AppendLine($"ASCOM Platform {newRelease.Major}.{newRelease.Minor}");
                    else // This is a service pack release
                        toastMessage.AppendLine($"ASCOM Platform {newRelease.Major}.{newRelease.Minor} Service Pack {newRelease.Patch}");
                }
                else // This is a release candidate release 
                {
                    toastHeaderId = "ASCOMPlatformRCUpdate";
                    toastHeaderText = "ASCOM PLATFORM - NEW RELEASE CANDIDATE";

                    if (newRelease.Patch == 0) // This is not a service pack release
                        toastMessage.AppendLine($"ASCOM Platform {newRelease.Major}.{newRelease.Minor}");
                    else // This is a service pack release
                        toastMessage.AppendLine($"ASCOM Platform {newRelease.Major}.{newRelease.Minor} Service Pack {newRelease.Patch}");

                    toastMessage.AppendLine($"Release candidate {newRelease.Prerelease.Trim('-', 'r', 'c', '.')}");
                }

                // Create and show the toast with an ASCOM logo, the message text and three action buttons for "Download", "Remind Later" and "Skip release"
                new ToastContentBuilder()
                    .AddHeader(toastHeaderId, toastHeaderText, "/HeaderArgument")
                    .AddAppLogoOverride(new Uri($"file://{Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ascom.jpg")}"))
                    .AddText(toastMessage.ToString())
                    .SetToastDuration(ToastDuration.Short)
                    .AddButton("Download", ToastActivationType.Background, COMMAND_DOWNLOAD)
                    .AddButton("Remind later", ToastActivationType.Background, COMMAND_REMIND_LATER)
                    .AddButton("Skip release", ToastActivationType.Background, $"{COMMAND_SKIP_RELEASE} {newRelease}")
                    .Show();

                LogMessage("CreateToast", "Toast shown OK");
            }
            catch (Exception ex)
            {
                LogMessage("CreateToast", $"Exception: {ex}");
            }

            LogMessage("CreateToast", "Notification finished");
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="module">Module where the message originated</param>
        /// <param name="message">Message to log</param>
        private static void LogMessage(string module, string message)
        {
            Console.WriteLine($"{module} - {message}");
            //Debug.WriteLine($"{module} - {message}");
            TL?.LogMessageCrLf(module, message);
        }

        #endregion
    }
}