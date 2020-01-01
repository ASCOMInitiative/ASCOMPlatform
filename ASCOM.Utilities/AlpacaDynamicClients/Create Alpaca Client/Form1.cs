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

        // Global variables within this class
        private TraceLogger TL;
        private Profile profile;
        private List<DynamicDriverRegistration> dynamicDrivers;

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
                dynamicDrivers = new List<DynamicDriverRegistration>();

                ReadConfiguration(); // Get the current configuration

                foreach (DynamicDriverRegistration driver in dynamicDrivers)
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
        /// Creates a list of Alpaca dynamic drivers and their configuration information
        /// </summary>
        private void ReadConfiguration()
        {
            Regex progidParseRegex = new Regex(PROGID_PARSE_REGEX_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Initialise 
            dynamicDrivers.Clear();
            DynamicDriversCheckedListBox.Items.Clear();

            // Extract a list of the remote client drivers from the list of devices in the Profile
            ArrayList deviceTypes = profile.RegisteredDeviceTypes;
            foreach (string deviceType in deviceTypes)
            {
                ArrayList devices = profile.RegisteredDevices(deviceType);
                foreach (KeyValuePair device in devices)
                {
                    Match match = progidParseRegex.Match(device.Key); // Parse the ProgID to extract the device number and device type
                    if (match.Success)
                    {
                        // Create a new data class to hold information about this dynamic driver
                        DynamicDriverRegistration foundDriver = new DynamicDriverRegistration();

                        // Populate information from the dynamic driver's ProgID
                        foundDriver.ProgId = match.Groups["0"].Value;
                        foundDriver.Number = int.Parse(match.Groups[DEVICE_NUMBER].Value, CultureInfo.InvariantCulture);
                        foundDriver.DeviceType = deviceType;

                        // populate configuration information from the dynamic driver's Profile and 
                        profile.DeviceType = foundDriver.DeviceType;
                        foundDriver.IPAdrress = profile.GetValue(foundDriver.ProgId, IP_ADDRESS_VALUE_NAME);
                        foundDriver.PortNumber = Convert.ToInt32(profile.GetValue(foundDriver.ProgId, PORT_NUMBER_VALUE_NAME));
                        foundDriver.RemoteDeviceNumber = Convert.ToInt32(profile.GetValue(foundDriver.ProgId, REMOTE_DEVICE_NUMBER_VALUE_NAME));
                        foundDriver.UniqueID = profile.GetValue(foundDriver.ProgId, UNIQAUEID_VALUE_NAME);
                        foundDriver.Name = device.Value;
                        foundDriver.Description = $"{foundDriver.Name} ({foundDriver.ProgId}) - {foundDriver.IPAdrress}:{foundDriver.PortNumber}/api/v1/{foundDriver.DeviceType}/{foundDriver.Number} - {foundDriver.UniqueID}";

                        // Add the data class to the dynamic devices collection and to the form's checked list box
                        dynamicDrivers.Add(foundDriver);
                        DynamicDriversCheckedListBox.Items.Add(foundDriver);

                        TL.LogMessage("ReadConfiguration", string.Format("{0} - {1} - {2}", foundDriver.ProgId, foundDriver.Number, foundDriver.DeviceType));
                    }
                }
            }
        }

        /// <summary>
        /// Cancel button handler - just closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                TL.LogMessage("Cancel", "Closing the application");

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

            // Confirm whether the user really does want to delete the selected drivers
            DialogResult result = MessageBox.Show("Are you sure that you want to delete the checked drivers?", "Delete Dynamic Drivers", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return; // Give up if there is any outcome other than yes

            try
            {
                // Disable controls so that the process can't be stopped part way through 
                BtnDeleteDrivers.Enabled = false;

                // Create variables pointing to the dynamic driv er's local server folder and executable
                string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH;
                string localServerExe = localServerPath + REMOTE_SERVER;

                if (File.Exists(localServerExe)) // Local server does exist
                {
                    TL.LogMessage("DeleteDrivers", string.Format("Found local server {0}", localServerExe));

                    // Unregister all current drivers
                    CreateAlpacaClients.RunLocalServer(localServerExe, "-unregserver", TL);

                    // Iterate over each device that has been checked in the UI checked listbox
                    foreach (DynamicDriverRegistration driver in DynamicDriversCheckedListBox.CheckedItems)
                    {
                        // Delete the driver executable and it's PDB file
                        TL.LogMessage("DeleteDrivers", $"Deleting driver {driver.Description}");
                        string driverFileName = $"{localServerPath}{driver.ProgId}.dll";
                        string pdbFileName = $"{localServerPath}{driver.ProgId}.pdb";
                        TL.LogMessage("DeleteDrivers", $"Deleting driver files {driverFileName} and {pdbFileName}");
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
                            MessageBox.Show(errorMessage, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Remove the ASCOM Profile information that is not removed when the driver is unregistered
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
                    string errorMessage = $"Could not find local server: {localServerExe}";
                    TL.LogMessage("DeleteDrivers", errorMessage);
                    MessageBox.Show(errorMessage, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } // Local server can not be found
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("DeleteDrivers - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred during Apply, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnDeleteDrivers.Enabled = true;
            }
        }
    }
}