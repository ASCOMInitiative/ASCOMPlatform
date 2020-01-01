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
        private const string IP_ADDRESS_VALUE_NAME = "IP Address"; // Regular expression device type placeholder name
        private const string PORT_NUMBER_VALUE_NAME = "Port Number"; // Regular expression device type placeholder name
        private const string REMOTE_DEVICE_NUMBER_VALUE_NAME = "Remote Device Number"; // Regular expression device type placeholder name
        private const string UNIQAUEID_VALUE_NAME = "UniqueID"; // Regular expression device type placeholder name

        private const string PROGID_PARSE_REGEX_STRING = @"^ascom\.alpacadynamic(?'" + DEVICE_NUMBER + @"'\d+)\.(?'" + DEVICE_TYPE + @"'[a-z]+)$"; // Regular expression for extracting device type and number

        // Constants shared with the main program
        internal const string REMOTE_SERVER_PATH = @"\ASCOM\AlpacaDynamicClients\"; // Relative path from CommonFiles
        internal const string REMOTE_SERVER = @"ASCOM.AlpacaClientLocalServer.exe"; // Name of the remote client local server application
        internal const string REMOTE_CLIENT_DRIVER_NAME_TEMPLATE = @"ASCOM.AlpacaDynamic*.{0}.*"; // Template for finding remote client driver files

        // List of supported device types - this must be kept in sync with the device type numeric up-down controls on the form dialogue!
        private readonly List<string> supportedDeviceTypes = new List<string>() { "Camera", "Dome", "FilterWheel", "Focuser", "ObservingConditions", "Rotator", "SafetyMonitor", "Switch", "Telescope" };

        // Global variables within this class
        TraceLogger TL;
        Profile profile;
        List<DynamicDriverRegistration> remoteDrivers;
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
                remoteDrivers = new List<DynamicDriverRegistration>();
                deviceTypeSummary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase); // Create a dictionary using a case insensitive key comparer

                ReadConfiguration(); // Get the current configuration

                foreach (DynamicDriverRegistration driver in remoteDrivers)
                {
                    TL.LogMessage("Initialise", $"Found remote {driver.DeviceType} driver: {driver.Description}");
                }

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

            Regex progidParseRegex = new Regex(PROGID_PARSE_REGEX_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            deviceTypeSummary.Clear();
            remoteDrivers.Clear();
            checkedListBox1.Items.Clear();

            // Extract a list of the remote client drivers from the list of devices in the Profile
            foreach (string deviceType in deviceTypes)
            {
                ArrayList devices = profile.RegisteredDevices(deviceType);
                foreach (KeyValuePair device in devices)
                {
                    Match match = progidParseRegex.Match(device.Key);
                    if (match.Success)
                    {
                        DynamicDriverRegistration foundDriver = new DynamicDriverRegistration();
                        foundDriver.ProgId = match.Groups["0"].Value;
                        foundDriver.Number = int.Parse(match.Groups[DEVICE_NUMBER].Value, CultureInfo.InvariantCulture);
                        foundDriver.DeviceType = match.Groups[DEVICE_TYPE].Value;

                        profile.DeviceType = foundDriver.DeviceType;
                        foundDriver.IPAdrress = profile.GetValue(foundDriver.ProgId, IP_ADDRESS_VALUE_NAME);
                        foundDriver.PortNumber = Convert.ToInt32(profile.GetValue(foundDriver.ProgId, PORT_NUMBER_VALUE_NAME));
                        foundDriver.RemoteDeviceNumber = Convert.ToInt32(profile.GetValue(foundDriver.ProgId, REMOTE_DEVICE_NUMBER_VALUE_NAME));
                        foundDriver.UniqueID = profile.GetValue(foundDriver.ProgId, UNIQAUEID_VALUE_NAME);
                        foundDriver.Name = device.Value;
                        foundDriver.Description = $"{foundDriver.Name} ({foundDriver.ProgId}) - {foundDriver.IPAdrress}:{foundDriver.PortNumber}/api/v1/{foundDriver.DeviceType}/{foundDriver.Number} - {foundDriver.UniqueID}";

                        remoteDrivers.Add(foundDriver);
                        checkedListBox1.Items.Add(foundDriver);

                        TL.LogMessage("ReadConfiguration", string.Format("{0} - {1} - {2}", foundDriver.ProgId, foundDriver.Number, foundDriver.DeviceType));
                    }
                }
            }

            // List the remote client drivers and create summary counts of client drivers of each device type 
            foreach (string deviceType in deviceTypes)
            {
                List<DynamicDriverRegistration> result = (from s in remoteDrivers where s.DeviceType.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
                foreach (DynamicDriverRegistration driver in result)
                {
                    TL.LogMessage("ReadConfiguration", string.Format("{0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                }

                deviceTypeSummary.Add(deviceType, result.Count);
            }

        }

        /// <summary>
        /// Exit button handler - just closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                TL.LogMessage("Exit", "Closing the application");

                TL.Enabled = false;
                TL.Dispose();
                TL = null;
            }
            finally
            {
                Application.Exit();
            }
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
        private void BtnDeleteDrivers_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls so that the process can't be stopped part way through 
                BtnDeleteDrivers.Enabled = false;

                string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH;
                string localServerExe = localServerPath + REMOTE_SERVER;

                if (File.Exists(localServerExe)) // Local server does exist
                {
                    TL.LogMessage("DeleteDrivers", string.Format("Found local server {0}", localServerExe));
                    ArrayList deviceTypes = profile.RegisteredDeviceTypes;

                    // Unregister all current drivers
                    CreateAlpacaClients.RunLocalServer(localServerExe, "-unregserver", TL);

                    foreach (DynamicDriverRegistration driver in checkedListBox1.CheckedItems)
                    {
                        TL.LogMessage("DeleteDrivers", $"Deleting driver {driver.Description}");
                        string driverFileName = $"{localServerPath}{driver.ProgId}.dll";
                        string pdbFileName= $"{localServerPath}{driver.ProgId}.pdb";
                        TL.LogMessage("DeleteDrivers", $"Deleting driver file {driverFileName}");
                        try
                        {
                            File.Delete(driverFileName);
                            File.Delete(pdbFileName);
                            TL.LogMessage("DeleteDrivers", string.Format("Successfully deleted driver file {0}", driverFileName));
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = string.Format("Unable to delete driver file {0} - {1}", driverFileName, ex.Message);
                            TL.LogMessage("DeleteDrivers", errorMessage);
                            MessageBox.Show(errorMessage);
                        }

                        TL.LogMessage("DeleteDrivers", $"Removing driver Profile registration for {driver.DeviceType} driver: {driver.ProgId}");
                        profile.DeviceType = driver.DeviceType;
                        profile.Unregister(driver.ProgId);
                    }

                    // Re-register the remaining drivers
                    CreateAlpacaClients.RunLocalServer(localServerExe, "-regserver", TL);
                    ReadConfiguration();
                }
                else // Local server can not be found
                {
                    string errorMessage = string.Format("Could not find local server {0}", localServerExe);
                    TL.LogMessage("Apply", errorMessage);
                    MessageBox.Show(errorMessage);
                } // Local server cannot be found
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Apply - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred during Apply, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnDeleteDrivers.Enabled = true;
            }
        }
    }
}
