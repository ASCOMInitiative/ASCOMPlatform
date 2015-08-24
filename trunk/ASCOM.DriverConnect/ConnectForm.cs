using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Diagnostics;

namespace ASCOM.DriverConnect
{
    public partial class ConnectForm : Form
    {

        #region Constants and Variables

        private const string DEFAULT_DEVICE_TYPE = "Telescope";

        private const string DEFAULT_DEVICE_TELESCOPE = "ASCOM.Simulator.Telescope";
        private const string DEFAULT_DEVICE_FOCUSER = "ASCOM.Simulator.Focuser";
        private const string DEFAULT_DEVICE_FILTER_WHEEL = "ASCOM.Simulator.FilterWheel";
        private const string DEFAULT_DEVICE_ROTATOR = "ASCOM.Simulator.Rotator";
        private const string DEFAULT_DEVICE_DOME = "ASCOM.Simulator.Dome";
        private const string DEFAULT_DEVICE_CAMERA = "ASCOM.Simulator.Camera";
        private const string DEFAULT_DEVICE_VIDEO = "ASCOM.Simulator.Video";
        private const string DEFAULT_DEVICE_SWITCH = "ASCOM.Simulator.Switch";
        private const string DEFAULT_DEVICE_SAFETY_MONITOR = "ASCOM.Simulator.SafetyMonitor";

        private string CurrentDevice;
        private string CurrentDeviceType;

        private SortedList<string, string> DefaultDevices = new SortedList<string, string>();

        private TraceLogger TL;

        private ASCOM.Utilities.Util Util;

        //API for auto drop down combo
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        private const int CB_SHOWDROPDOWN = 0x14f;

        private const string TRACE_LOGGER_NAME = "ConnectForm";

        #endregion

        #region Form load and Initialise

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            ArrayList DeviceTypes = new ArrayList();

            cmbDeviceType.SelectedIndexChanged += new EventHandler(cmbDeviceType_SelectedIndexChanged); // Add an event handler for devicetype combo box 
            this.Resize += new EventHandler(ConnectForm_Resize); // Add an event handler for form resize.

            try
            {
                Util = new ASCOM.Utilities.Util();

                // Get the currently define device types
                ASCOM.Utilities.Profile Profile = new ASCOM.Utilities.Profile();
                DeviceTypes = Profile.RegisteredDeviceTypes;
                Profile.Dispose();

                DefaultDevices = new SortedList<string, string>(); // Initialise the sorted array of default drivers for each device type
                foreach (string DeviceType in DeviceTypes) // Populate the sorted array and the UI combo boxwith the default driver values
                {
                    cmbDeviceType.Items.Add(DeviceType);
                    switch (DeviceType.ToUpper())
                    {
                        case "TELESCOPE":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_TELESCOPE);
                            break;
                        case "FOCUSER":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_FOCUSER);
                            break;
                        case "FILTERWHEEL":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_FILTER_WHEEL);
                            break;
                        case "ROTATOR":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_ROTATOR);
                            break;
                        case "DOME":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_DOME);
                            break;
                        case "CAMERA":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_CAMERA);
                            break;
                        case "VIDEO":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_VIDEO);
                            break;
                        case "SWITCH":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_SWITCH);
                            break;
                        case "SAFETYMONITOR":
                            DefaultDevices.Add(DeviceType.ToUpper(), DEFAULT_DEVICE_SAFETY_MONITOR);
                            break;
                        default:
                            DefaultDevices.Add(DeviceType.ToUpper(), "");
                            break;
                    }
                }

                CurrentDevice = DEFAULT_DEVICE_TELESCOPE; // Initialise current value variables and UI display
                CurrentDeviceType = DEFAULT_DEVICE_TYPE;
                cmbDeviceType.SelectedItem = CurrentDeviceType;
                txtDevice.Text = CurrentDevice;

                // Supply an appropriate title for the form depending on the OS and application bitness
                if (Environment.Is64BitOperatingSystem) // 64bit OS
                {
                    if (Environment.Is64BitProcess) // 64bit process
                    {
                        this.Text = "Device Connection Tester - 64bit OS - Operating in 64bit mode";
                    }
                    else // 32bit process
                    {
                        this.Text = "Device Connection Tester - 64bit OS - Operating in 32bit mode";
                    }
                }
                else // 32bit OS
                {
                    this.Text = "Device Connection Tester - 32bit OS";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Button Event Handlers

        /// <summary>
        /// Event handler for Choose button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_Click(System.Object sender, System.EventArgs e)
        {
            Chooser chooser = new Chooser();
            string NewDevice = null;

            CurrentDeviceType = cmbDeviceType.SelectedItem.ToString();
            chooser.DeviceType = CurrentDeviceType;

            if (string.IsNullOrEmpty(CurrentDevice))
            {
                CurrentDevice = DefaultDevices[CurrentDeviceType.ToUpper()];
                txtDevice.Text = DefaultDevices[CurrentDeviceType.ToUpper()];
            }

            NewDevice = chooser.Choose(CurrentDevice);
            if (!string.IsNullOrEmpty(NewDevice)) CurrentDevice = NewDevice;

            txtDevice.Text = CurrentDevice;
            chooser.Dispose();

        }

        /// <summary>
        /// Event handler for Properties button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProperties_Click(System.Object sender, System.EventArgs e)
        {
            txtStatus.Clear();
            TL = new TraceLogger("", TRACE_LOGGER_NAME);
            TL.Enabled = true;
            dynamic driver = null;

            try
            {
                LogMsg("Create", "Creating device");

                Type type = Type.GetTypeFromProgID(CurrentDevice);
                driver = Activator.CreateInstance(type);

                if (chkConnect.Checked)
                {
                    LogMsg("Connected", "Setting Connected = True");
                    driver.Connected = true;
                    LogMsg("Setup", "Opening setup dialogue");
                    driver.SetupDialog();
                    LogMsg("Connected", "Setting Connected = False");
                    driver.Connected = false;
                }
                else
                {
                    LogMsg("Connected", "Not setting Connected = True");
                    LogMsg("Setup", "Opening setup dialogue");
                    driver.SetupDialog();
                }
            }
            catch (Exception ex)
            {
                LogMsg("SetupError", ex.Message);
            }
            finally
            {
                LogMsg("Dispose", "Disposing of device");
                try { driver.Dispose(); }
                catch { }
                try { Marshal.ReleaseComObject(driver); }
                catch { }
                LogMsg("Dispose", "Completed disposal of device");
            }

            TL.Enabled = false;
            TL.Dispose();

        }

        /// <summary>
        /// Event handler for Connect button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            bool usedConnected = true;

            TL = new TraceLogger("", TRACE_LOGGER_NAME);
            TL.Enabled = true;
            dynamic driver = null;

            try
            {
                txtStatus.Clear();

                LogMsg("Create", "Creating device");
                Type type = Type.GetTypeFromProgID(CurrentDevice);
                driver = Activator.CreateInstance(type);
                LogMsg("Connected", "Connecting to device");
                if (CurrentDeviceType.ToUpper() == "FOCUSER")
                {
                    try
                    {
                        LogMsg("Connected", "Trying Connected");
                        driver.Connected = true;
                    }
                    catch (Exception ex)
                    {
                        LogMsg("Connected", "Trying Link: " + ex.Message);
                        driver.Link = true;
                        usedConnected = false;
                    }
                }
                else
                {
                    driver.Connected = true;
                }

                LogMsg("", "");

                try { LogMsg("Name", driver.Name); }
                catch (Exception ex) { LogMsg("Name", "Property not available - " + ex.Message); ex.ToString(); }
                try { LogMsg("Description", driver.Description); }
                catch (Exception ex) { LogMsg("Description", "Property not available - " + ex.Message); ex.ToString(); }
                try { LogMsg("DriverInfo", driver.DriverInfo); }
                catch (Exception ex) { LogMsg("DriverInfo", "Property not available - " + ex.Message); ex.ToString(); }
                try { LogMsg("DriverVersion", driver.DriverVersion); }
                catch (Exception ex) { LogMsg("DriverVersion", "Property not available - " + ex.Message); ex.ToString(); }
                try { LogMsg("InterfaceVersion", driver.InterfaceVersion.ToString()); }
                catch (Exception ex) { LogMsg("InterfaceVersion", "Property not available - " + ex.Message); ex.ToString(); }

                // Device specific commands
                switch (CurrentDeviceType.ToUpper())
                {
                    case "TELESCOPE":
                        try { LogMsg("RA, Dec", Util.HoursToHMS(driver.RightAscension, ":", ":", "", 3) + " " + Util.DegreesToDMS(driver.Declination, ":", ":", "", 3)); }
                        catch (Exception ex) { LogMsg("RA, Dec", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("Latitude, Longitude", Util.DegreesToDMS(driver.SiteLatitude, ":", ":", "", 3) + " " + Util.DegreesToDMS(driver.SiteLongitude, ":", ":", "", 3)); }
                        catch (Exception ex) { LogMsg("Latitude, Longitude", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("Tracking", driver.Tracking.ToString()); }
                        catch (Exception ex) { LogMsg("Tracking", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "FOCUSER":
                        try { LogMsg("IsMoving", driver.IsMoving.ToString()); }
                        catch (Exception ex) { LogMsg("IsMoving", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("Position", driver.Position.ToString()); }
                        catch (Exception ex) { LogMsg("Position", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "FILTERWHEEL":
                        try { LogMsg("Position", driver.Position.ToString()); }
                        catch (Exception ex) { LogMsg("Position", "Property not available - " + ex.Message); ex.ToString(); }
                        try
                        {
                            string[] names = driver.Names;
                            foreach (string name in names)
                            {
                                LogMsg("Filter name", name);
                            }
                        }
                        catch (Exception ex) { LogMsg("Names", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "ROTATOR":
                        try { LogMsg("IsMoving", driver.IsMoving.ToString()); }
                        catch (Exception ex) { LogMsg("IsMoving", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("Position", driver.Position.ToString()); }
                        catch (Exception ex) { LogMsg("Position", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "DOME":
                        try { LogMsg("Azimuth, Altitude", Util.DegreesToDMS(driver.Azimuth, ":", ":", "", 3) + " " + Util.DegreesToDMS(driver.Altitude, ":", ":", "", 3)); }
                        catch (Exception ex) { LogMsg("Azimuth, Altitude", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("AtHome", driver.AtHome.ToString()); }
                        catch (Exception ex) { LogMsg("AtHome", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("AtPark", driver.AtPark.ToString()); }
                        catch (Exception ex) { LogMsg("AtPark", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "CAMERA":
                        try { LogMsg("CameraXSize", driver.CameraXSize.ToString()); }
                        catch (Exception ex) { LogMsg("CameraXSize", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("CameraYSize", driver.CameraYSize.ToString()); }
                        catch (Exception ex) { LogMsg("CameraYSize", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("BinX", driver.BinX.ToString()); }
                        catch (Exception ex) { LogMsg("BinX", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("BinY", driver.BinY.ToString()); }
                        catch (Exception ex) { LogMsg("BinY", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("MaxBinX", driver.MaxBinX.ToString()); }
                        catch (Exception ex) { LogMsg("MaxBinX", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("MaxBinY", driver.MaxBinY.ToString()); }
                        catch (Exception ex) { LogMsg("MaxBinY", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("HasShutter", driver.HasShutter.ToString()); }
                        catch (Exception ex) { LogMsg("HasShutter", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "VIDEO":
                        try { LogMsg("Width", driver.Width.ToString()); }
                        catch (Exception ex) { LogMsg("Width", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("Height", driver.Height.ToString()); }
                        catch (Exception ex) { LogMsg("Height", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("ExposureMax", driver.ExposureMax.ToString()); }
                        catch (Exception ex) { LogMsg("ExposureMax", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("ExposureMin", driver.ExposureMin.ToString()); }
                        catch (Exception ex) { LogMsg("ExposureMin", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("FrameRate", driver.FrameRate.ToString()); }
                        catch (Exception ex) { LogMsg("FrameRate", "Property not available - " + ex.Message); ex.ToString(); }
                        try { LogMsg("CanConfigureDevice", driver.CanConfigureDeviceProperties.ToString()); }
                        catch (Exception ex) { LogMsg("CanConfigureDevice", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "SWITCH":
                        try { LogMsg("MaxSwitch", driver.MaxSwitch.ToString()); }
                        catch (Exception ex) { LogMsg("MaxSwitch", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    case "SAFETYMONITOR":
                        try { LogMsg("IsSafe", driver.IsSafe.ToString()); }
                        catch (Exception ex) { LogMsg("IsSafe", "Property not available - " + ex.Message); ex.ToString(); }
                        break;
                    default:
                        break;
                }

                LogMsg("", "");
                LogMsg("Connected", "Disconnecting from device");
                if (usedConnected) driver.Connected = false;
                else driver.Link = false;
            }
            catch (Exception ex)
            {
                LogMsg("Error", ex.ToString());
            }
            finally
            {
                try
                {
                    LogMsg("Dispose", "Disposing of device with exception handling...");
                    driver.Dispose();
                    LogMsg("Dispose", "Completed disposal");
                }
                catch (Exception ex1)
                {
                    LogMsg("Dispose error", ex1.ToString());
                }
                try
                {
                    LogMsg("ReleaseComObject", "Releasing COM instance with exception handling...");
                    int count = Marshal.ReleaseComObject(driver);
                    LogMsg("ReleaseComObject", "Completed disposal. Count: " + count);
                }
                catch (Exception ex2)
                {
                    LogMsg("ReleaseComObject error", ex2.ToString());

                }
                LogMsg("Connected", "Completed disposal of device");
            }

            try
            {
                LogMsg("GC Collect", "Starting garbage collection");
                driver = null;
                GC.Collect();
                LogMsg("GC Collect", "Completed garbage collection");
            }
            catch (Exception ex)
            {
                LogMsg("GC Collect", ex.ToString());
            }

            TL.Enabled = false;
            TL.Dispose();
        }

        /// <summary>
        /// Event handler for Profile button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetProfile_Click(System.Object sender, System.EventArgs e)
        {
            TL = new TraceLogger("", TRACE_LOGGER_NAME);
            TL.Enabled = true;
            try
            {
                txtStatus.Clear(); // Clear the status screen

                Profile profile = new Profile(); // Create a Profile object
                profile.DeviceType = CurrentDeviceType;

                ASCOMProfile deviceProfile = profile.GetProfile(CurrentDevice); // Get the profile of the current device
                SortedList<string, SortedList<string, string>> results = deviceProfile.ProfileValues;

                // List each value of the root and all subkeys
                foreach (KeyValuePair<string, SortedList<string, string>> result in results)
                {
                    foreach (KeyValuePair<string, string> value in result.Value)
                    {
                        LogMsg(result.Key.ToString(), (value.Key.ToString() == "" ? "<Default>" : value.Key).PadRight(25) + " " + (value.Value == "" ? "<Empty>" : value.Value));
                    }
                }

                profile.Dispose();
            }
            catch (Exception ex)
            {
                LogMsg("Error", ex.ToString());
            }

            TL.Enabled = false;
            TL.Dispose();
        }

        #endregion

        #region Other event Handlers

        /// <summary>
        /// Event handler for form resize event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConnectForm_Resize(object sender, EventArgs e)
        {
            txtStatus.Width = this.Width - 38; // Set appropriate values for the tetx box width and height as the form is resized.
            txtStatus.Height = this.Height - 178;
        }

        /// <summary>
        /// Event handler for Device combobox click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This forces the combo box to open and display the options when clicked.</remarks>
        public void cmbDeviceType_Click(System.Object sender, System.EventArgs e)
        {
            SendMessage(cmbDeviceType.Handle, CB_SHOWDROPDOWN, (IntPtr)1, (IntPtr)0);
            cmbDeviceType.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Event handler for device type combobox value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentDeviceType = cmbDeviceType.SelectedItem.ToString();
            CurrentDevice = DefaultDevices[CurrentDeviceType.ToUpper()];
            txtDevice.Text = DefaultDevices[CurrentDeviceType.ToUpper()];
        }

        #endregion

        #region Support Code
        public void LogMsg(string Command, string Msg)
        {
            TL.LogMessage(Command, Msg);
            txtStatus.Text = txtStatus.Text + Command.PadRight(20) + Msg + "\r\n";
        }
        #endregion

    }
}
