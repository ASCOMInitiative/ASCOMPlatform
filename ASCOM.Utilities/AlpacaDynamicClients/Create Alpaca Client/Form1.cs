// This application enables the user to specify the number and type of remote client drivers that will be configured on their client machine,
// the user thus ends up with only devices that they actually want and need.
// The application uses dynamic compilation i.e. the drivers are compiled on the user's machine at run time rather than being pre-compiled at installer build time.
// Most of the heavy lifting is done through pre-compiled base classes that are called from the dynamically compiled top level shell classes.
// This enables the user to specify what are normally hard coded specifics such as the device type, GUID and device number.

// The application generates required code and stores this in memory. When the class is complete it is compiled and the resultant assembly persisted to disk
// into the same directory as the remote client local server, which is then called to register the driver assembly.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;

namespace ASCOM.DynamicRemoteClients
{
    public partial class Form1 : Form
    {
        // Constants only used within this form
        private const string DEVICE_NUMBER = "DeviceNumber"; // Regular expression device number placeholder name
        private const string DEVICE_TYPE = "DeviceType"; // Regular expression device type placeholder name
        private const string NUMERIC_UPDOWN_CONTROLNAME_PREXIX = "Num"; // Prefix to numeric up-down controls that enables them to be identified

        private const string REGEX_FORMAT_STRING = @"^ascom\.alpacadynamic(?'" + DEVICE_NUMBER + @"'\d+)\.(?'" + DEVICE_TYPE + @"'[a-z]+)$"; // Regular expression for extracting device type and number

        // Constants shared with the main program
        internal const string REMOTE_SERVER_PATH = @"\ASCOM\AlpacaDynamicClients\"; // Relative path from CommonFiles
        internal const string REMOTE_SERVER = @"ASCOM.AlpacaClientLocalServer.exe"; // Name of the remote client local server application
        internal const string REMOTE_CLIENT_DRIVER_NAME_TEMPLATE = @"ASCOM.AlpacaDynamic*.{0}.*"; // Template for finding remote client driver files

        // List of supported device types - this must be kept in sync with the device type numeric up-down controls on the form dialogue!
        private readonly List<string> supportedDeviceTypes = new List<string>() { "Camera", "Dome", "FilterWheel", "Focuser", "ObservingConditions", "Rotator", "SafetyMonitor", "Switch", "Telescope" };

        // Global variables within this class
        TraceLogger TL;
        Profile profile;
        List<DriverRegistration> remoteDrivers;
        Dictionary<string, int> deviceTypeSummary;

        /// <summary>
        /// Initialises the form
        /// </summary>
        public Form1(TraceLogger TLParameter)
        {
            try
            {
                InitializeComponent();

                TL = TLParameter; // Save the supplied trace logger

                Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                LblVersionNumber.Text = "Version " + assemblyVersion.ToString();
                TL.LogMessage("Initialise", string.Format("Application Version: {0}", assemblyVersion.ToString()));

                profile = new Profile();
                remoteDrivers = new List<DriverRegistration>();
                deviceTypeSummary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase); // Create a dictionary using a case insensitive key comparer

                ReadConfiguration(); // Get the current configuration

                TL.LogMessage("Initialise", string.Format("Initialisation completed"));
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("initialise - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred on start up, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (profile != null) try { profile.Dispose(); } catch { }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Reads the current device configuration from the Profile and saves this for use elsewhere
        /// </summary>
        private void ReadConfiguration()
        {
            ArrayList deviceTypes = profile.RegisteredDeviceTypes;

            Regex regex = new Regex(REGEX_FORMAT_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            deviceTypeSummary.Clear();
            remoteDrivers.Clear();

            // List all the up-down controls present
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(NumericUpDown))
                {
                    TL.LogMessage("ReadConfiguration", string.Format("Found NumericUpDown control {0}", ctrl.Name));
                    ctrl.BackColor = SystemColors.Window;
                }
            }

            // Extract a list of the remote client drivers from the list of devices in the Profile
            foreach (string deviceType in deviceTypes)
            {
                ArrayList devices = profile.RegisteredDevices(deviceType);
                foreach (KeyValuePair device in devices)
                {
                    Match match = regex.Match(device.Key);
                    if (match.Success)
                    {
                        DriverRegistration foundDriver = new DriverRegistration();
                        foundDriver.ProgId = match.Groups["0"].Value;
                        foundDriver.Number = int.Parse(match.Groups[DEVICE_NUMBER].Value, CultureInfo.InvariantCulture);
                        foundDriver.DeviceType = match.Groups[DEVICE_TYPE].Value;
                        remoteDrivers.Add(foundDriver);
                        TL.LogMessage("ReadConfiguration", string.Format("{0} - {1} - {2}", foundDriver.ProgId, foundDriver.Number, foundDriver.DeviceType));
                    }
                }

                TL.BlankLine();
            }

            // List the remote client drivers and create summary counts of client drivers of each device type 
            foreach (string deviceType in deviceTypes)
            {
                List<DriverRegistration> result = (from s in remoteDrivers where s.DeviceType.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
                foreach (DriverRegistration driver in result)
                {
                    TL.LogMessage("ReadConfiguration", string.Format("{0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                }

                deviceTypeSummary.Add(deviceType, result.Count);
            }

            // List the summary information
            foreach (string deviceType in deviceTypes)
            {
                TL.LogMessage("ReadConfiguration", string.Format("There are {0} {1} remote drivers", deviceTypeSummary[deviceType], deviceType));
            }

        }

/// <summary>
        /// Exit button handler - just closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            TL.LogMessage("Exit", "Closing the application");

            TL.Enabled = false;
            TL.Dispose();
            TL = null;

            Application.Exit();
        }

        /// <summary>
        /// Revise the number of remote clients and clean up the Profile if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks> Approach to revising the number of remote clients:
        /// Prerequisites
        ///   Confirm that local server exists
        ///     If not then exit
        ///   Unregister drivers
        /// Remove driver executables numbered higher than required
        ///   For each driver number > n
        ///     Delete driver
        /// Remove Profile information for drivers numbered higher than required
        ///   For each driver number > n
        ///     Unregister driver
        /// Install required drivers
        ///   For each driver 1..n
        ///     Check that driver file exists
        ///       If not then create driver
        ///       Else no action
        /// Register drivers
        /// </remarks>
        private void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls so that the process can't be stopped part way through 

                string localServerExe = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH + REMOTE_SERVER;
                if (File.Exists(localServerExe)) // Local server does exist
                {
                    TL.LogMessage("Apply", string.Format("Found local server {0}", localServerExe));
                    ArrayList deviceTypes = profile.RegisteredDeviceTypes;

                    // Unregister existing drivers
                    CreateAlpacaClients.RunLocalServer(localServerExe, "-unregserver", TL);

                    // Iterate over all of the installed device types
                    foreach (string deviceType in deviceTypes)
                    {
                        // Only attempt to process a device type if it is one that we support, otherwise ignore it
                        if (supportedDeviceTypes.Contains(deviceType, StringComparer.OrdinalIgnoreCase))
                        {
                            // This device type is recognised so process it
                            TL.LogMessage("Apply", string.Format("Processing device type: \"{0}\"", deviceType));
                            Control[] c = this.Controls.Find(NUMERIC_UPDOWN_CONTROLNAME_PREXIX + deviceType, true);
                            NumericUpDown numControl = (NumericUpDown)c[0];

                            // Delete files above the number required
                            string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH;
                            string searchPattern = string.Format(REMOTE_CLIENT_DRIVER_NAME_TEMPLATE, deviceType);

                            TL.LogMessage("Apply", string.Format("Searching for {0} driver files in {1} using pattern: {2}", deviceType, localServerPath, searchPattern));

                            List<string> files = Directory.GetFiles(localServerPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
                            TL.LogMessage("Apply", string.Format("Found {0} driver files", files.Count));

                            foreach (string file in files)
                            {
                                TL.LogMessage("Apply", string.Format("Found driver file {0}", file));
                                try
                                {
                                    File.Delete(file);
                                    TL.LogMessage("Apply", string.Format("Successfully deleted driver file {0}", file));
                                }
                                catch (Exception ex)
                                {
                                    string errorMessage = string.Format("Unable to delete driver file {0} - {1}", file, ex.Message);
                                    TL.LogMessage("Apply", errorMessage);
                                    MessageBox.Show(errorMessage);
                                }
                            }

                            // Unregister drivers
                            List<DriverRegistration> result = (from s in remoteDrivers where s.DeviceType.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
                            foreach (DriverRegistration driver in result)
                            {
                                if (driver.Number > numControl.Value)
                                {
                                    TL.LogMessage("Apply", string.Format("Removing driver Profile registration for {0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                                    profile.DeviceType = deviceType;
                                    profile.Unregister(driver.ProgId);
                                }
                                else
                                {
                                    TL.LogMessage("Apply", string.Format("Leaving driver Profile in place for {0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                                }
                            }

                            // Create required number of drivers
                            for (int i = 1; i <= numControl.Value; i++)
                            {
                                //CreateAlpacaClients.CreateDriver(deviceType, i, localServerPath, TL);
                            }
                        }
                        else
                        {
                            TL.LogMessage("Apply", string.Format("Ignoring unsupported device type: \"{0}\"", deviceType));
                            TL.BlankLine();
                        }
                    }

                    // Register the drivers
                    CreateAlpacaClients.RunLocalServer(localServerExe, "-regserver", TL);
                    ReadConfiguration();
                }
                else // Local server can not be found
                {
                    string errorMessage = string.Format("Could not find local server {0}", localServerExe);
                    TL.LogMessage("Apply", errorMessage);
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Apply - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred during Apply, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }
    }
}
