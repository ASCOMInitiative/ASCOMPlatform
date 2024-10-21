using ASCOM.Utilities;
using Microsoft.Win32;
using Semver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformUpdateChecker
{
    internal class UpdateCheck
    {
        // Path and file name in the local application data folder for the file containing "skip this release" data
        private const string SKIP_DATA_PATH = @"ASCOM\Platform";
        private const string SKIP_RELEASE_FILENAME = @"SkipRelease.txt";

        // Registry key and values for "check for releases" configuration information that is set by the user through the Diagnostics application
        private const string REGISTRY_CONFIGURATION_PATH = @"Software\ASCOM\Utilities"; //Computer\HKEY_CURRENT_USER\Software\ASCOM\Utilities
        private const string REGISTRY_CHECK_FOR_RELEASE_UPDATES = "Check for Release Updates"; //Check for Release Updates
        private const string REGISTRY_CHECK_FOR_PRERELEASE_UPDATES = "Check for Release Candidate Updates"; // Check for pre-release updates
        private const bool CHECK_FOR_RELEASE_UPDATES_DEFAULT = true;
        private const bool CHECK_FOR_PRERELEASE_UPDATES_DEFAULT = true;

        // URL of the source of Platform updates
        private const string DOWNLOAD_URL = @"https://github.com/ASCOMInitiative/ASCOMPlatform/releases";

        // Define the ASCOM Platform GitHub repository to check for updates
        const string OWNER = "ASCOMInitiative"; // Test owner: "Peter-Simpson"
        const string REPOSITORY = "ASCOMPlatform"; // Test repository: "SafetyMonitorTester"

        // TraceLogger instance
        private static TraceLogger TL;

        /// <summary>
        /// Check GitHub for Platform updates and notify the user via a Toast if any are available and this application is configured to do so.
        /// </summary>
        public static void CheckForUpdates(Action<SemVersion> releaseAction, TraceLogger traceLogger)
        {
            TL = traceLogger;

            // initialise Booleans controlling whether or not checks are enabled as set through the Diagnostics app
            bool checksEnabledReleaseUpdate = true;
            bool checksEnabledPreReleaseUpdate = false;

            try
            {
                // Initialise the list of skipped releases
                List<string> skippedReleases = new List<string>();

                try
                {
                    LogMessage("CheckForUpdates", $"Getting list of skipped releases.");

                    // Create paths to the folder and file where the skipped list is stored
                    string skipListPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SKIP_DATA_PATH);
                    string skipListFile = Path.Combine(skipListPath, SKIP_RELEASE_FILENAME);

                    // Create the folder in case it does not already exist
                    Directory.CreateDirectory(skipListPath);

                    // Check whether the data file exists
                    if (File.Exists(skipListFile))
                    {
                        // Data file already exists so read it in and initialise the List
                        skippedReleases.AddRange(File.ReadAllLines(skipListFile));
                    }

                    // List the skipped releases to the log
                    foreach (string release in skippedReleases)
                    {
                        LogMessage("CheckForUpdates", $"Skipping release: {release}");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("SkipRelease", $"Exception {ex.Message}\r\n{ex}");
                }

                // Get the release / pre-release update check enabled / disabled states from the registry
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_CONFIGURATION_PATH))
                {
                    if (key != null)
                    {
                        checksEnabledReleaseUpdate = Convert.ToBoolean(key.GetValue(REGISTRY_CHECK_FOR_RELEASE_UPDATES, CHECK_FOR_RELEASE_UPDATES_DEFAULT));
                        checksEnabledPreReleaseUpdate = Convert.ToBoolean(key.GetValue(REGISTRY_CHECK_FOR_PRERELEASE_UPDATES, CHECK_FOR_PRERELEASE_UPDATES_DEFAULT));
                        LogMessage("CheckForUpdates", $"Release updates enabled: {checksEnabledReleaseUpdate}, pre-release updates enabled: {checksEnabledPreReleaseUpdate}");
                    }
                    else
                    {
                        LogMessage("CheckForUpdates", $"Registry key is null");
                    }
                }

                // Get a SemVersion class representing the currently installed Platform version
                SemVersion currentPlatform = SemVersion.Parse(GetCurrentPlatformVersion(), SemVersionStyles.Any);
                LogMessage("CheckForUpdates", $"Running update check. Current Platform: {GetCurrentPlatformVersion()}. SemVersion: {currentPlatform}");

                // Initialise the latest release and pre-release variables
                SemVersion latestRelease = new SemVersion(0);
                SemVersion latestPreRelease = new SemVersion(0);

                // Create a GitHub client to query available Platform versions.
                Octokit.GitHubClient gitHubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue($@"{OWNER}-UpdateCheck"));

                // Get the list of releases
                IReadOnlyList<Octokit.Release> results = gitHubClient.Repository.Release.GetAll(OWNER, REPOSITORY).Result;

                // Check whether some results were obtained
                if (results.Count > 0) // Some results were returned
                {
                    // Iterate over the results and process one at a time to find the latest release and pre-release versions
                    foreach (Octokit.Release release in results)
                    {
                        try
                        {
                            LogMessage("CheckForUpdates", $"Found {release.Name,-45} " +
                                $"TagName: {release.TagName,-22} " +
                                $"Pre-release: {release.Prerelease,-6} " +
                                $"ID: {release.Id,-20} " +
                                $"Published at (UTC): {release.PublishedAt.Value.UtcDateTime,-12} " +
                                $"Published at: {release.PublishedAt,-12}");

                            bool semverStringValid = SemVersion.TryParse(release.TagName, SemVersionStyles.AllowV, out SemVersion version);
                            LogMessage("CheckForUpdates", $"Valid: {semverStringValid} SemVersion: {version}");

                            // Check whether this release has a valid SemVer string
                            if (semverStringValid) // String is valid
                            {
                                LogMessage("CheckForUpdates", $"{version} is later than {latestRelease}: {version.IsNewerThan(latestRelease)}");
                                LogMessage("CheckForUpdates", $"{version} is later than {latestPreRelease}: {version.IsNewerThan(latestPreRelease)}");

                                // Check whether this is a release and is later than latest release version found to date. If so save this as the latest release version
                                if ((SemVersion.ComparePrecedence(version, latestRelease) == 1) & version.IsRelease)
                                {
                                    latestRelease = version;
                                    LogMessage("CheckForUpdates", $"Making {version} the latest release version");
                                }

                                // Check whether this is a pre-release and is later than latest pre-release version found to date. If so save this as the latest pre-release version
                                if ((SemVersion.ComparePrecedence(version, latestPreRelease) == 1) & version.IsPrerelease)
                                {
                                    latestPreRelease = version;
                                    LogMessage("CheckForUpdates", $"Making {version} the latest pre-release version");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage("CheckForUpdates", ex.ToString());
                        }
                    }

                    // List the latest release and pre-release versions
                    LogMessage("CheckForUpdates", $"Latest release: {latestRelease}");
                    LogMessage("CheckForUpdates", $"Latest pre-release: {latestPreRelease}");

                    // List whether the latest releases are ahead of the current Platform version
                    LogMessage("CheckForUpdates", $"Latest release is newer than Platform ({currentPlatform}): {latestRelease.IsNewerThan(currentPlatform)}");
                    LogMessage("CheckForUpdates", $"Latest pre-release is newer than Platform ({currentPlatform}): {latestPreRelease.IsNewerThan(currentPlatform)}");

                    // Check whether this release has already been skipped by the user
                    if (skippedReleases.Contains(latestRelease.ToString())) // Release has been skipped so do not notify again
                    {
                        LogMessage("CheckForUpdates", $"Skipping the latest release because the user pressed the toast Skip button at some point.");
                    }
                    else // Release has not been skipped 
                    {
                        // Check whether release checking has been enabled by the user
                        if (checksEnabledReleaseUpdate) // Release checking has been enabled
                        {
                            // Check whether this release is newer than the currently installed Platform version
                            if (latestRelease.IsNewerThan(currentPlatform)) // This production release is newer than the current Platform release so notify the user through a Toast message
                            {
                                releaseAction(latestRelease);
                            }
                        }
                    }

                    // Check whether this pre-release has already been skipped by the user
                    if (skippedReleases.Contains(latestPreRelease.ToString())) // Release has been skipped so do not notify again
                    {
                        LogMessage("CheckForUpdates", $"Skipping the latest pre-release because the user pressed the toast Skip button at some point.");
                    }
                    else // Release has not been skipped 
                    {
                        // Check whether pre-release checking has been enabled by the user
                        if (checksEnabledPreReleaseUpdate)// Pre-release checking has been enabled
                        {
                            // Check whether this pre-release is newer than the currently installed Platform version
                            if (latestPreRelease.IsNewerThan(currentPlatform)) // This pre-release is newer than the current Platform
                            {
                                // Check whether this pre-release is newer than the latest production release (no point in showing it if it is earlier than the current release)
                                if (latestPreRelease.IsNewerThan(latestRelease)) // This pre-release is newer than the latest production release so notify the user through a Toast message
                                {
                                    releaseAction(latestPreRelease);
                                }
                            }
                        }
                    }
                }
                else // No Platform versions were returned from GitHub
                {
                    LogMessage("CheckForUpdates", $"No releases found!");
                }
            }
            catch (Exception ex)
            {
                LogMessage("", $"Exception - {ex.Message}\r\n{ex}");
            }
        }

        /// <summary>
        /// Handler for the "Reset the list of skipped Platform updates" command line command
        /// </summary>
        public static void ResetSkippedReleases()
        {
            try
            {
                LogMessage("ResetSkippedReleases", $"Resetting skipped Platform versions...");

                // Create the path to the file containing the list
                string skipListFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SKIP_DATA_PATH, SKIP_RELEASE_FILENAME);

                // Delete the file
                File.Delete(skipListFile);

                // Confirm that the list has been reset
                LogMessage("ResetSkippedReleases", $"Skipped Platform versions reset successfully");
            }
            catch (Exception ex)
            {
                LogMessage("ResetSkippedReleases", $"Exception resetting skipped versions: {ex.Message}\r\n{ex}");
            }
        }

        /// <summary>
        /// Handler for the "Skip this release" button
        /// </summary>
        /// <param name="releaseToSkip">The release that the user wants to skip</param>
        public static void SkipThisRelease(string releaseToSkip)
        {
            try
            {
                // IOnitialise the list of skipped releases as an empty list
                List<string> skippedReleases = new List<string>();

                LogMessage("SkipThisRelease", $"Skipping version: {releaseToSkip}!");

                // Create paths to the folder and file where the skipped list is stored
                string skipListPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SKIP_DATA_PATH);
                string skipListFile = Path.Combine(skipListPath, SKIP_RELEASE_FILENAME);

                // Create the folder in case it does not already exist
                Directory.CreateDirectory(skipListPath);
                LogMessage("SkipThisRelease", $"Path: {skipListPath} @@@ File: {skipListFile}");

                // Check whether the data file exists
                if (File.Exists(skipListFile)) // File does exist so add its contents to the skipped files list
                {
                    // Data file already exists so read it in and initialise the List
                    skippedReleases.AddRange(File.ReadAllLines(skipListFile));
                }

                // Add the current release to the list
                skippedReleases.Add(releaseToSkip);

                // Persist the updated list back to its file
                File.WriteAllLines(skipListFile, skippedReleases.ToArray());
            }
            catch (Exception ex)
            {
                LogMessage("SkipThisRelease", $"Exception {ex.Message}\r\n{ex}");
            }
        }

        /// <summary>
        /// Gets the current Platform version from the application's assembly Product Version field
        /// </summary>
        /// <returns>The current Platform's SemVer version</returns>
        public static string GetCurrentPlatformVersion()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                return fileVersionInfo.ProductVersion;
            }
            catch (Exception ex)
            {
                return $"Unknown - {ex.Message}";
            }
        }

        /// <summary>
        /// Get the URL from which Platform release updates can be downloaded
        /// </summary>
        /// <returns>A fully qualified download URL</returns>
        public static string GetPlatformDownloadUrl()
        {
            return DOWNLOAD_URL;
        }

        #region Support Code

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="module">Module where the message originated</param>
        /// <param name="message">Message to log</param>
        private static void LogMessage(string module, string message)
        {
            //Console.WriteLine($"{module} - {message}");
            //Debug.WriteLine($"{module} - {message}");
            TL?.LogMessageCrLf(module, message);
        }

        #endregion

    }
}
