using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using static ASCOM.Utilities.Global;

namespace ASCOM.Utilities
{

    /// <summary>
    /// Form displayed to enable the user to select an ASCOM device.
    /// </summary>
    internal partial class ChooserForm : Form
    {

        #region Constants

        // General constants
        private const string ALERT_MESSAGEBOX_TITLE = "ASCOM Chooser";
        private const string DRIVER_INITIALISATION_ERROR_MESSAGEBOX_TITLE = "Driver Initialization Error";
        private const string SETUP_DIALOGUE_ERROR_MESSAGEBOX_TITLE = "Driver Setup Dialog Error";
        private const int PROPERTIES_TOOLTIP_DISPLAY_TIME = 5000; // Time to display the Properties tooltip (milliseconds)
        private const int FORM_LOAD_WARNING_MESSAGE_DELAY_TIME = 250; // Delay time before any warning message is displayed on form load
        private const int ALPACA_STATUS_BLINK_TIME = 100; // Length of time the Alpaca status indicator spends in the on and off state (ms)
        private const string TOOLTIP_PROPERTIES_TITLE = "Driver Setup";
        private const string TOOLTIP_PROPERTIES_MESSAGE = "Check or change driver Properties (configuration)";
        private const string TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE = "You must check driver configuration before first time use, please click the Properties... button." + Microsoft.VisualBasic.Constants.vbCrLf + "The OK button will remain greyed out until this is done.";
        private const string TOOLTIP_CREATE_ALPACA_DEVICE_TITLE = "Alpaca Device Selected";
        private const string TOOLTIP_CREATE_ALPACA_DEVICE_MESSAGE = "Please click this button to create the Alpaca Dynamic driver";
        private const int TOOLTIP_CREATE_ALPACA_DEVICE_DISPLAYTIME = 5; // Number of seconds to display the Create Alpaca Device informational message
        private const int CHOOSER_LIST_WIDTH_NEW_ALPACA = 600; // Width of the Chooser list when new Alpaca devices are present

        // Chooser persistence constants
        internal const string CONFIGRATION_SUBKEY = @"Chooser\Configuration"; // Store configuration in a sub-key under the Chooser key
        private const string ALPACA_ENABLED = "Alpaca enabled";
        private const bool ALPACA_ENABLED_DEFAULT = false;
        internal const string ALPACA_DISCOVERY_PORT = "Alpaca discovery port";
        internal const int ALPACA_DISCOVERY_PORT_DEFAULT = 32227;
        private const string ALPACA_NUMBER_OF_BROADCASTS = "Alpaca number of broadcasts";
        private const int ALPACA_NUMBER_OF_BROADCASTS_DEFAULT = 2;
        private const string ALPACA_TIMEOUT = "Alpaca timeout";
        private const double ALPACA_TIMEOUT_DEFAULT = 1.0d;
        private const string ALPACA_DNS_RESOLUTION = "Alpaca DNS resolution";
        private const bool ALPACA_DNS_RESOLUTION_DEFAULT = false;
        private const string ALPACA_SHOW_DISCOVERED_DEVICES = "Show discovered Alpaca devices";
        private const bool ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT = false;
        private const string ALPACA_SHOW_DEVICE_DETAILS = "Show Alpaca device details";
        private const bool ALPACA_SHOW_DEVICE_DETAILS_DEFAULT = false;
        private const string ALPACA_CHOOSER_WIDTH = "Alpaca Chooser width";
        private const int ALPACA_CHOOSER_WIDTH_DEFAULT = 0;
        private const string ALPACA_USE_IPV4 = "Use IPv4";
        private const bool ALPACA_USE_IPV4_DEFAULT = true;
        private const string ALPACA_USE_IPV6 = "Use IPv6";
        private const bool ALPACA_USE_IPV6_DEFAULT = false;
        private const string ALPACA_MULTI_THREADED_CHOOSER = "Multi Threaded Chooser";
        private const bool ALPACA_MULTI_THREADED_CHOOSER_DEFAULT = true;

        // Alpaca integration constants
        private const string ALPACA_DYNAMIC_CLIENT_MANAGER_RELATIVE_PATH = @"ASCOM\Platform 7\Tools\AlpacaDynamicClientManager";
        private const string ALPACA_DYNAMIC_CLIENT_MANAGER_EXE_NAME = "ASCOM.AlpacaDynamicClientManager.exe";
        private const string DRIVER_PROGID_BASE = "ASCOM.AlpacaDynamic"; // This value must match the same named constant in the Dynamic Client Local Server project LocalServer.cs file

        // Alpaca driver Profile store value names
        private const string PROFILE_VALUE_NAME_UNIQUEID = "UniqueID"; // Prefix applied to all COM drivers created to front Alpaca devices
        private const string PROFILE_VALUE_NAME_IP_ADDRESS = "IP Address";
        private const string PROFILE_VALUE_NAME_PORT_NUMBER = "Port Number";
        private const string PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER = "Remote Device Number";
        private const string PROFILE_VALUE_NAME_COM_GUID = "COM Guid"; // This value must match the same named constant in the Dynamic Client Local Server project LocalServer.cs file

        #endregion

        #region Variables

        // Chooser variables
        private string deviceTypeValue, selectedProgIdValue;
        private List<ChooserItem> chooserList;
        private string driverIsCompatible = "";
        private string currentWarningTitle, currentWarningMesage;
        private List<AscomDevice> alpacaDevices = new();
        private ChooserItem selectedChooserItem;
        private Process _ClientManagerProcess;
        private Type ProgIdType;

        private Process ClientManagerProcess
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ClientManagerProcess;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ClientManagerProcess != null)
                {
                    _ClientManagerProcess.Exited -= DriverGeneration_Complete;
                }

                _ClientManagerProcess = value;
                if (_ClientManagerProcess != null)
                {
                    _ClientManagerProcess.Exited += DriverGeneration_Complete;
                }
            }
        }
        private bool driverGenerationComplete;
        private bool currentOkButtonEnabledState;
        private bool currentPropertiesButtonEnabledState;

        // Component variables
        private TraceLogger TL;
        private ToolTip chooserWarningToolTip;
        private ToolTip chooserPropertiesToolTip;
        private ToolTip createAlpacaDeviceToolTip;
        private ToolStripLabel alpacaStatusToolstripLabel;
        private System.Windows.Forms.Timer _AlpacaStatusIndicatorTimer;

        private System.Windows.Forms.Timer AlpacaStatusIndicatorTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AlpacaStatusIndicatorTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AlpacaStatusIndicatorTimer != null)
                {
                    _AlpacaStatusIndicatorTimer.Tick -= AlpacaStatusIndicatorTimerEventHandler;
                }

                _AlpacaStatusIndicatorTimer = value;
                if (_AlpacaStatusIndicatorTimer != null)
                {
                    _AlpacaStatusIndicatorTimer.Tick += AlpacaStatusIndicatorTimerEventHandler;
                }
            }
        }
        private Profile profile;
        private RegistryAccess registryAccess;

        // Persistence variables
        internal bool AlpacaEnabled;
        internal int AlpacaDiscoveryPort;
        internal int AlpacaNumberOfBroadcasts;
        internal double AlpacaTimeout;
        internal bool AlpacaDnsResolution;
        internal bool AlpacaShowDiscoveredDevices;
        internal bool AlpacaShowDeviceDetails;
        internal int AlpacaChooserIncrementalWidth;
        internal bool AlpacaUseIpV4;
        internal bool AlpacaUseIpV6;
        internal bool AlpacaMultiThreadedChooser;

        // Delegates
        private MethodInvoker PopulateDriverComboBoxDelegate; // Device list combo box delegate
        private MethodInvoker SetStateNoAlpacaDelegate;
        private MethodInvoker SetStateAlpacaDiscoveringDelegate;
        private MethodInvoker SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate;
        private MethodInvoker SetStateAlpacaDiscoveryCompleteNoDevicesDelegate;

        // Chooser form control positions
        private int OriginalForm1Width;
        private Point OriginalBtnCancelPosition;
        private Point OriginalBtnOKPosition;
        private Point OriginalBtnPropertiesPosition;
        private int OriginalCmbDriverSelectorWidth;
        private int OriginalLblAlpacaDiscoveryPosition;
        private int OriginalAlpacaStatusPosition;
        private int OriginalDividerLineWidth;

        #endregion

        #region Form load, close, paint and dispose event handlers

        public ChooserForm() : base()
        {
            PopulateDriverComboBoxDelegate = PopulateDriverComboBox;
            SetStateNoAlpacaDelegate = SetStateNoAlpaca;
            SetStateAlpacaDiscoveringDelegate = SetStateAlpacaDiscovering;
            SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate = SetStateAlpacaDiscoveryCompleteFoundDevices;
            SetStateAlpacaDiscoveryCompleteNoDevicesDelegate = SetStateAlpacaDiscoveryCompleteNoDevices;
            displayCreateAlpacDeviceTooltip = new NoParameterDelegate(DisplayAlpacaDeviceToolTip);
            InitializeComponent();

            // Record initial control positions
            OriginalForm1Width = Width;
            OriginalBtnCancelPosition = BtnCancel.Location;
            OriginalBtnOKPosition = BtnOK.Location;
            OriginalBtnPropertiesPosition = BtnProperties.Location;
            OriginalCmbDriverSelectorWidth = CmbDriverSelector.Width;
            OriginalLblAlpacaDiscoveryPosition = LblAlpacaDiscovery.Left;
            OriginalAlpacaStatusPosition = AlpacaStatus.Left;
            OriginalDividerLineWidth = DividerLine.Width;

            // Get access to the profile registry area
            registryAccess = new RegistryAccess(VB6COMErrors.ERR_SOURCE_CHOOSER);

            ReadState(); // Read in the state variables from persisted storage
            ResizeChooser();

            // Create a trace logger
            TL = new TraceLogger("", "ChooserForm");
            TL.IdentifierWidth = 50;
            TL.Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT); // Enable the trace logger if Utility trace is enabled

            profile = new Profile();

        }

        private void ChooserForm_Load(object eventSender, EventArgs eventArgs)
        {

            try
            {

                // Initialise form title and message text
                Text = "ASCOM " + deviceTypeValue + " Chooser";
                lblTitle.Text = "Select the type of " + Strings.LCase(deviceTypeValue) + " you have, then be " + "sure to click the Properties... button to configure the driver for your " + Strings.LCase(deviceTypeValue) + ".";

                // Initialise the Profile component with the supplied device type
                profile.DeviceType = deviceTypeValue;

                // Initialise the tooltip warning for 32/64bit driver compatibility messages
                chooserWarningToolTip = new ToolTip();

                CmbDriverSelector.DropDownWidth = CHOOSER_LIST_WIDTH_NEW_ALPACA;

                // Configure the Properties button tooltip
                chooserPropertiesToolTip = new ToolTip();
                chooserPropertiesToolTip.IsBalloon = true;
                chooserPropertiesToolTip.ToolTipIcon = ToolTipIcon.Info;
                chooserPropertiesToolTip.UseFading = true;
                chooserPropertiesToolTip.ToolTipTitle = TOOLTIP_PROPERTIES_TITLE;
                chooserPropertiesToolTip.SetToolTip(BtnProperties, TOOLTIP_PROPERTIES_MESSAGE);

                // Create Alpaca information tooltip 
                createAlpacaDeviceToolTip = new ToolTip();

                // Set a custom rendered for the tool strip so that colours and appearance can be controlled better
                ChooserMenu.Renderer = new ChooserCustomToolStripRenderer();

                // Create a tool strip label whose background colour can  be changed and add it at the top of the Alpaca menu
                alpacaStatusToolstripLabel = new ToolStripLabel("Discovery status unknown");
                MnuAlpaca.DropDownItems.Insert(0, alpacaStatusToolstripLabel);

                RefreshTraceMenu(); // Refresh the trace menu

                // Set up the Alpaca status blink timer but make sure its not running
                AlpacaStatusIndicatorTimer = new System.Windows.Forms.Timer();
                AlpacaStatusIndicatorTimer.Interval = ALPACA_STATUS_BLINK_TIME; // Set it to fire after 250ms
                AlpacaStatusIndicatorTimer.Stop();

                TL.LogMessage("ChooserForm_Load", $"UI thread: {Thread.CurrentThread.ManagedThreadId}");

                InitialiseComboBox(); // ' Kick off a discover and populate the combo box or just populate the combo box if no discovery is required
            }

            catch (Exception ex)
            {
                Interaction.MsgBox("ChooserForm Load " + ex.ToString());
                LogEvent("ChooserForm Load ", ex.ToString(), EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString());
            }
        }

        private void ChooserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Clean up the trace logger
            TL.Enabled = false;
        }

        /// <summary>
        /// Dispose of disposable components
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
                if (TL is not null)
                {
                    try
                    {
                        TL.Dispose();
                    }
                    catch
                    {
                    }
                }
                if (chooserWarningToolTip is not null)
                {
                    try
                    {
                        chooserWarningToolTip.Dispose();
                    }
                    catch
                    {
                    }
                }
                if (chooserPropertiesToolTip is not null)
                {
                    try
                    {
                        chooserPropertiesToolTip.Dispose();
                    }
                    catch
                    {
                    }
                }
                if (alpacaStatusToolstripLabel is not null)
                {
                    try
                    {
                        alpacaStatusToolstripLabel.Dispose();
                    }
                    catch
                    {
                    }
                }
                if (profile is not null)
                {
                    try
                    {
                        profile.Dispose();
                    }
                    catch
                    {
                    }
                }
                if (registryAccess is not null)
                {
                    try
                    {
                        registryAccess.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
            base.Dispose(Disposing);
        }

        #endregion

        #region Public methods

        public string DeviceType
        {
            set
            {

                // Clean up the supplied device type to consistent values
                switch (value.ToLowerInvariant() ?? "")
                {
                    case "camera":
                        {
                            deviceTypeValue = "Camera";
                            break;
                        }
                    case "covercalibrator":
                        {
                            deviceTypeValue = "CoverCalibrator";
                            break;
                        }
                    case "dome":
                        {
                            deviceTypeValue = "Dome";
                            break;
                        }
                    case "filterwheel":
                        {
                            deviceTypeValue = "FilterWheel";
                            break;
                        }
                    case "focuser":
                        {
                            deviceTypeValue = "Focuser";
                            break;
                        }
                    case "observingconditions":
                        {
                            deviceTypeValue = "ObservingConditions";
                            break;
                        }
                    case "rotator":
                        {
                            deviceTypeValue = "Rotator";
                            break;
                        }
                    case "safetymonitor":
                        {
                            deviceTypeValue = "SafetyMonitor";
                            break;
                        }
                    case "switch":
                        {
                            deviceTypeValue = "Switch";
                            break;
                        }
                    case "telescope":
                        {
                            deviceTypeValue = "Telescope";
                            break;
                        }
                    case "video":
                        {
                            deviceTypeValue = "Video"; // If not recognised just use as supplied for backward compatibility
                            break;
                        }

                    default:
                        {
                            deviceTypeValue = value;
                            break;
                        }
                }

                TL.LogMessage("DeviceType Set", deviceTypeValue);
                ReadState(deviceTypeValue);
            }
        }

        public string SelectedProgId
        {
            get
            {
                return selectedProgIdValue;
            }
            set
            {
                selectedProgIdValue = value;
                TL.LogMessage("InitiallySelectedProgId Set", selectedProgIdValue);
            }
        }

        #endregion

        #region Form, button, control and timer event handlers

        private void ComboProduct_DrawItem(object sender, DrawItemEventArgs e) // Handles CmbDriverSelector.DrawItem
        {
            Brush brush;
            Color colour;
            ComboBox combo;
            string text = "";

            try
            {
                e.DrawBackground();
                combo = (ComboBox)sender;

                brush = Brushes.White;
                colour = Color.White;

                if (combo.SelectedIndex >= 0)
                {
                    ChooserItem chooseritem = (ChooserItem)combo.Items[e.Index];

                    TL.LogMessage("comboProduct_DrawItem", $"IsComDriver: {chooseritem.IsComDriver} {chooseritem.AscomName}");
                    text = chooseritem.AscomName;
                    if (chooseritem.IsComDriver)
                    {
                        if (chooseritem.ProgID.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant()))
                        {
                            brush = Brushes.Red;
                            colour = Color.Red;
                        }
                        else
                        {
                            brush = Brushes.LightPink;
                            colour = Color.LightPink;
                        }
                    }
                    else
                    {
                        brush = Brushes.LightGreen;
                        colour = Color.LightGreen;
                    }
                }

                e.Graphics.DrawRectangle(new Pen(Color.Black), e.Bounds);
                e.Graphics.FillRectangle(brush, e.Bounds);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawString(text, combo.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void ChooserFormMoveEventHandler(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentWarningMesage))
                WarningToolTipShow(currentWarningTitle, currentWarningMesage);
            DisplayAlpacaDeviceToolTip();
        }

        private void AlpacaStatusIndicatorTimerEventHandler(object myObject, EventArgs myEventArgs)
        {
            if (AlpacaStatus.BackColor == Color.Orange)
            {
                AlpacaStatus.BackColor = Color.DimGray;
            }
            else
            {
                AlpacaStatus.BackColor = Color.Orange;
            }

        }

        /// <summary>
        /// Click in Properties... button. Loads the currently selected driver and activate its setup dialogue.
        /// </summary>
        /// <param name="eventSender"></param>
        /// <param name="eventArgs"></param>
        private void CmdProperties_Click(object eventSender, EventArgs eventArgs)
        {
            object oDrv = null; // The driver
            bool bConnected;
            string sProgID;
            bool UseCreateObject = false;

            // Find ProgID corresponding to description
            sProgID = ((ChooserItem)CmbDriverSelector.SelectedItem).ProgID;

            TL.LogMessage("PropertiesClick", "ProgID:" + sProgID);
            try
            {
                // Mechanic to revert to Platform 5 behaviour in the event that Activator.CreateInstance has unforeseen consequences
                try
                {
                    UseCreateObject = GetBool(CHOOSER_USE_CREATEOBJECT, CHOOSER_USE_CREATEOBJECT_DEFAULT);
                }
                catch
                {
                }

                ProgIdType = Type.GetTypeFromProgID(sProgID);

                if (UseCreateObject) // Platform 5 behaviour
                {
                    LogEvent("ChooserForm", "Using CreateObject for driver: \"" + sProgID + "\"", EventLogEntryType.Information, EventLogErrors.ChooserSetupFailed, "");
                    oDrv = Interaction.CreateObject(sProgID); // Rob suggests that Activator.CreateInstance gives better error diagnostics
                }
                else // New Platform 6 behaviour
                {
                    oDrv = Activator.CreateInstance(ProgIdType);
                }

                // Here we try to see if a device is already connected. If so, alert and just turn on the OK button.
                bConnected = false;
                try
                {
                    bConnected = (bool)ProgIdType.InvokeMember("Connected", BindingFlags.GetProperty, null, oDrv, new object[0]);
                }
                catch
                {
                    try
                    {
                        bConnected = (bool)ProgIdType.InvokeMember("Link", BindingFlags.GetProperty, null, oDrv, new object[0]);
                    }
                    catch
                    {
                    }
                }

                if (bConnected) // Already connected so cannot show the Setup dialogue
                {
                    Interaction.MsgBox("The device is already connected. Just click OK.", (MsgBoxStyle)((int)MsgBoxStyle.OkOnly + (int)MsgBoxStyle.Information + (int)MsgBoxStyle.MsgBoxSetForeground), ALERT_MESSAGEBOX_TITLE);
                }
                else // Not connected, so call the SetupDialog method
                {
                    try
                    {
                        WarningTooltipClear(); // Clear warning tool tip before entering setup so that the dialogue doesn't interfere with or obscure the setup dialogue.
                        ProgIdType.InvokeMember("SetupDialog", BindingFlags.InvokeMethod, null, oDrv, new object[0]);
                    }
                    catch (Exception ex) // Something went wrong in the SetupDialog method so display an error message.
                    {
                        Interaction.MsgBox($"The SetupDialog method of driver \"{sProgID}\" threw an exception when called.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"This means that the setup dialogue would not start properly.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"Please screen print or use CTRL+C to copy all of this message and report it to the driver author with a request for assistance.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"{ex.GetType().Name} - {ex.Message}", MsgBoxStyle.OkOnly | MsgBoxStyle.Critical | MsgBoxStyle.MsgBoxSetForeground, SETUP_DIALOGUE_ERROR_MESSAGEBOX_TITLE);
                        LogEvent("ChooserForm", "Driver setup method failed for driver: \"" + sProgID + "\"", EventLogEntryType.Error, EventLogErrors.ChooserSetupFailed, $"{ex.GetType().Name} - {ex.Message}");
                    }
                }

                registryAccess.WriteProfile("Chooser", sProgID + " Init", "True"); // Remember it has been initialized
                EnableOkButton(true);
                WarningTooltipClear();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox($"The driver \"{sProgID}\" threw an exception when loaded.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"This means that the driver would not start properly.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"Please screen print or use CTRL+C to copy all of this message and report it to the driver author with a request for assistance.{Microsoft.VisualBasic.Constants.vbCrLf}{Microsoft.VisualBasic.Constants.vbCrLf}" + $"{ex}", MsgBoxStyle.OkOnly | MsgBoxStyle.Critical | MsgBoxStyle.MsgBoxSetForeground, DRIVER_INITIALISATION_ERROR_MESSAGEBOX_TITLE);
                LogEvent("ChooserForm", "Failed to load driver:  \"" + sProgID + "\"", EventLogEntryType.Error, EventLogErrors.ChooserDriverFailed, ex.ToString());
            }

            // Clean up and release resources
            try
            {
                ProgIdType.InvokeMember("Dispose", BindingFlags.InvokeMethod, null, oDrv, new object[0]);
            }
            catch (Exception)
            {
            }
            try
            {
                Marshal.ReleaseComObject(oDrv);
            }
            catch (Exception)
            {
            }

        }

        private void CmdCancel_Click(object eventSender, EventArgs eventArgs)
        {
            selectedProgIdValue = "";
            Hide();
        }

        private void CmdOK_Click(object eventSender, EventArgs eventArgs)
        {
            string newProgId;
            DialogResult userResponse;

            TL.LogMessage("OK Click", $"Combo box selected index = {CmbDriverSelector.SelectedIndex}");

            if (selectedChooserItem.IsComDriver) // User has selected an existing COM driver so return its ProgID
            {
                selectedProgIdValue = selectedChooserItem.ProgID;

                TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'");

                // Close the UI because the COM driver is selected
                Hide();
            }
            else // User has selected a new Alpaca device so we need to create a new COM driver for this
            {

                // SHow the admin request dialogue if it has not been suppressed by the user
                if (!GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT)) // The admin request coming dialogue has not been suppressed so show the dialogue
                {
                    using (var checkedMessageBox = new CheckedMessageBox())
                    {
                        userResponse = checkedMessageBox.ShowDialog();
                    }
                }
                else // The admin request coming dialogue has been suppressed so flag the user response as OK
                {
                    userResponse = DialogResult.OK;
                }

                // Test whether the user clicked the OK button or pressed the "x" cancel icon in the top right of the form
                if (userResponse == DialogResult.OK) // User pressed the OK button
                {

                    try
                    {
                        string tempProgId;
                        int deviceNumber;
                        Type typeFromProgId;

                        // Initialise to a starting value
                        deviceNumber = 0;

                        // Try successive ProgIDs until one is found that is not COM registered
                        do
                        {
                            deviceNumber += 1; // Increment the device number
                            tempProgId = $"{DRIVER_PROGID_BASE}{deviceNumber}.{deviceTypeValue}"; // Create the new ProgID to be tested
                            typeFromProgId = Type.GetTypeFromProgID(tempProgId); // Try to get the type with the new ProgID
                            TL.LogMessage("CreateAlpacaClient", $"Testing ProgID: {tempProgId} Type name: {typeFromProgId?.Name}");
                        }
                        while (typeFromProgId is not null); // Loop until the returned type is null indicating that this type is not COM registered
                        newProgId = tempProgId;
                        TL.LogMessage("CreateAlpacaClient", $"Creating new ProgID: {newProgId}");

                        // Configure the IP address, port number and Alpaca device number in the newly registered driver
                        TL.LogMessage("OK Click", $"ProgID: {newProgId}");
                        TL.LogMessage("OK Click", $"Display name: {selectedChooserItem.AscomName}");
                        TL.LogMessage("OK Click", $"Display name: {selectedChooserItem.DisplayName}");

                        profile.DeviceType = deviceTypeValue;
                        profile.Register(newProgId, selectedChooserItem.AscomName);
                        profile.WriteValue(newProgId, PROFILE_VALUE_NAME_IP_ADDRESS, selectedChooserItem.HostName);
                        profile.WriteValue(newProgId, PROFILE_VALUE_NAME_PORT_NUMBER, selectedChooserItem.Port.ToString());
                        profile.WriteValue(newProgId, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER, selectedChooserItem.DeviceNumber.ToString());
                        profile.WriteValue(newProgId, PROFILE_VALUE_NAME_UNIQUEID, selectedChooserItem.DeviceUniqueID.ToString());

                        // Create a new COM GUID for this driver if one does not already exist.
                        // At this point, we aren't interested in the returned value, only that a value exists. This is ensured by use of the default value: Guid.NewGuid().
                        profile.GetValue(newProgId, PROFILE_VALUE_NAME_COM_GUID, "", Guid.NewGuid().ToString());

                        // Create a new Alpaca driver of the current ASCOM device type
                        newProgId = CreateNewAlpacaDriver(newProgId, deviceNumber, selectedChooserItem.AscomName);

                        // Flag the driver as being already configured so that it can be used immediately
                        registryAccess.WriteProfile("Chooser", $"{newProgId} Init", "True");

                        // Select the new driver in the Chooser combo box list
                        selectedProgIdValue = newProgId;
                        InitialiseComboBox();

                        TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'");
                    }
                    catch (Win32Exception ex) when (ex.ErrorCode == int.MinValue + 0x00004005)
                    {
                        TL.LogMessage("OK Click", $"Driver creation cancelled: {ex.Message}");
                        MessageBox.Show($"Driver creation cancelled: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex}");
                    }
                }

                // Don't exit the Chooser but instead return to the UI so that the user can see that a new driver has been created and selected
            }
        }

        /// <summary>
        /// Driver generation completion event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DriverGeneration_Complete(object sender, EventArgs e)
        {
            driverGenerationComplete = true; // Flag that driver generation is complete
        }

        private void CbDriverSelector_SelectionChangeCommitted(object eventSender, EventArgs eventArgs)
        {
            if (CmbDriverSelector.SelectedIndex >= 0)
            {

                // Save the newly selected chooser item
                selectedChooserItem = (ChooserItem)CmbDriverSelector.SelectedItem;
                selectedProgIdValue = selectedChooserItem.ProgID;

                // Validate the driver if it is a COM driver
                if (selectedChooserItem.IsComDriver) // This is a COM driver
                {
                    TL.LogMessage("SelectedIndexChanged", $"New COM driver selected. ProgID: {selectedChooserItem.ProgID} {selectedChooserItem.AscomName}");
                    ValidateDriver(selectedChooserItem.ProgID);
                }
                else // This is a new Alpaca driver
                {
                    TL.LogMessage("SelectedIndexChanged", $"New Alpaca driver selected : {selectedChooserItem.AscomName}");
                    EnablePropertiesButton(false); // Disable the Properties button because there is not yet a COM driver to configure
                    WarningTooltipClear();
                    EnableOkButton(true);
                    DisplayAlpacaDeviceToolTip();
                }
            }

            else // Selected index is negative
            {
                TL.LogMessage("SelectedIndexChanged", $"Ignoring index changed event because no item is selected: {CmbDriverSelector.SelectedIndex}");
            }
        }

        private void PicASCOM_Click(object eventSender, EventArgs eventArgs)
        {
            try
            {
                Process.Start("https://ASCOM-Standards.org/");
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Unable to display ASCOM-Standards web site in your browser: " + ex.Message, (MsgBoxStyle)((int)MsgBoxStyle.OkOnly + (int)MsgBoxStyle.Exclamation + (int)MsgBoxStyle.MsgBoxSetForeground), ALERT_MESSAGEBOX_TITLE);
            }
        }

        #endregion

        #region Menu code and event handlers

        private void RefreshTraceMenu()
        {
            string TraceFileName;


            TraceFileName = registryAccess.GetProfile("", SERIAL_FILE_NAME_VARNAME);
            switch (TraceFileName ?? "") // Trace is disabled
            {
                case var @case when @case == "":
                    {
                        MenuSerialTraceEnabled.Checked = false; // The trace enabled flag is unchecked and disabled
                        MenuSerialTraceEnabled.Enabled = true;
                        break;
                    }
                case SERIAL_AUTO_FILENAME: // Tracing is on using an automatic filename
                    {
                        MenuSerialTraceEnabled.Checked = true; // The trace enabled flag is checked and enabled
                        MenuSerialTraceEnabled.Enabled = true; // Tracing using some other fixed filename
                        break;
                    }

                default:
                    {
                        MenuSerialTraceEnabled.Checked = true; // The trace enabled flag is checked and enabled
                        MenuSerialTraceEnabled.Enabled = true;
                        break;
                    }
            }

            // Set Profile trace checked state on menu item 
            MenuProfileTraceEnabled.Checked = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT);
            MenuRegistryTraceEnabled.Checked = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT);
            MenuUtilTraceEnabled.Checked = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT);
            MenuTransformTraceEnabled.Checked = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT);
            MenuSimulatorTraceEnabled.Checked = GetBool(SIMULATOR_TRACE, SIMULATOR_TRACE_DEFAULT);
            MenuDriverAccessTraceEnabled.Checked = GetBool(DRIVERACCESS_TRACE, DRIVERACCESS_TRACE_DEFAULT);
            MenuAstroUtilsTraceEnabled.Checked = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT);
            MenuNovasTraceEnabled.Checked = GetBool(NOVAS_TRACE, NOVAS_TRACE_DEFAULT);
            MenuCacheTraceEnabled.Checked = GetBool(TRACE_CACHE, TRACE_CACHE_DEFAULT);
            MenuEarthRotationDataFormTraceEnabled.Checked = GetBool(TRACE_EARTHROTATION_DATA_FORM, TRACE_EARTHROTATION_DATA_FORM_DEFAULT);

        }

        private void MenuAutoTraceFilenames_Click(object sender, EventArgs e)
        {
            // Auto filenames currently disabled, so enable them
            MenuSerialTraceEnabled.Enabled = true; // Set the trace enabled flag
            MenuSerialTraceEnabled.Checked = true; // Enable the trace enabled flag
            registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME);
        }

        private void MenuSerialTraceFile_Click(object sender, EventArgs e)
        {
            DialogResult RetVal;

            RetVal = SerialTraceFileName.ShowDialog();
            switch (RetVal)
            {
                case DialogResult.OK:
                    {
                        // Save the result
                        registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName);
                        // Check and enable the serial trace enabled flag
                        MenuSerialTraceEnabled.Enabled = true;
                        // Enable manual serial trace file flag
                        MenuSerialTraceEnabled.Checked = true; // Ignore everything else
                        break;
                    }

                default:
                    {
                        break;
                    }

            }
        }

        private void MenuSerialTraceEnabled_Click(object sender, EventArgs e)
        {

            if (MenuSerialTraceEnabled.Checked) // Auto serial trace is on so turn it off
            {
                MenuSerialTraceEnabled.Checked = false;
                registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "");
            }
            else // Auto serial trace is off so turn it on
            {
                MenuSerialTraceEnabled.Checked = true;
                registryAccess.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME);
            }
        }

        private void MenuProfileTraceEnabled_Click_1(object sender, EventArgs e)
        {
            MenuProfileTraceEnabled.Checked = !MenuProfileTraceEnabled.Checked; // Invert the selection
            SetName(TRACE_PROFILE, MenuProfileTraceEnabled.Checked.ToString());
        }

        private void MenuRegistryTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuRegistryTraceEnabled.Checked = !MenuRegistryTraceEnabled.Checked; // Invert the selection
            SetName(TRACE_XMLACCESS, MenuRegistryTraceEnabled.Checked.ToString());
        }

        private void MenuUtilTraceEnabled_Click_1(object sender, EventArgs e)
        {
            MenuUtilTraceEnabled.Checked = !MenuUtilTraceEnabled.Checked; // Invert the selection
            SetName(TRACE_UTIL, MenuUtilTraceEnabled.Checked.ToString());
        }

        private void MenuTransformTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuTransformTraceEnabled.Checked = !MenuTransformTraceEnabled.Checked; // Invert the selection
            SetName(TRACE_TRANSFORM, MenuTransformTraceEnabled.Checked.ToString());
        }

        private void MenuIncludeSerialTraceDebugInformation_Click(object sender, EventArgs e)
        {
            // MenuIncludeSerialTraceDebugInformation.Checked = Not MenuIncludeSerialTraceDebugInformation.Checked 'Invert selection
            // SetName(SERIAL_TRACE_DEBUG, MenuIncludeSerialTraceDebugInformation.Checked.ToString)
        }

        private void MenuSimulatorTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuSimulatorTraceEnabled.Checked = !MenuSimulatorTraceEnabled.Checked; // Invert selection
            SetName(SIMULATOR_TRACE, MenuSimulatorTraceEnabled.Checked.ToString());
        }

        private void MenuCacheTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuCacheTraceEnabled.Checked = !MenuCacheTraceEnabled.Checked; // Invert selection
            SetName(TRACE_CACHE, MenuCacheTraceEnabled.Checked.ToString());
        }

        private void MenuEarthRotationDataTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuEarthRotationDataFormTraceEnabled.Checked = !MenuEarthRotationDataFormTraceEnabled.Checked; // Invert selection
            SetName(TRACE_EARTHROTATION_DATA_FORM, MenuEarthRotationDataFormTraceEnabled.Checked.ToString());
        }

        private void MenuTrace_DropDownOpening(object sender, EventArgs e)
        {
            RefreshTraceMenu();
        }

        private void MenuDriverAccessTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuDriverAccessTraceEnabled.Checked = !MenuDriverAccessTraceEnabled.Checked; // Invert selection
            SetName(DRIVERACCESS_TRACE, MenuDriverAccessTraceEnabled.Checked.ToString());
        }

        private void MenuAstroUtilsTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuAstroUtilsTraceEnabled.Checked = !MenuAstroUtilsTraceEnabled.Checked; // Invert selection
            SetName(ASTROUTILS_TRACE, MenuAstroUtilsTraceEnabled.Checked.ToString());
        }

        private void MenuNovasTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuNovasTraceEnabled.Checked = !MenuNovasTraceEnabled.Checked; // Invert selection
            SetName(NOVAS_TRACE, MenuNovasTraceEnabled.Checked.ToString());
        }

        private void MnuEnableDiscovery_Click(object sender, EventArgs e)
        {
            AlpacaEnabled = true;
            WriteState(deviceTypeValue);
            InitialiseComboBox();
        }

        private void MnuDisableDiscovery_Click(object sender, EventArgs e)
        {
            AlpacaEnabled = false;
            WriteState(deviceTypeValue);
            InitialiseComboBox();
            SetStateNoAlpaca();
        }

        private void MnuDiscoverNow_Click(object sender, EventArgs e)
        {
            InitialiseComboBox();
        }

        private void MnuConfigureDiscovery_Click(object sender, EventArgs e)
        {
            ChooserAlpacaConfigurationForm alpacaConfigurationForm;

            TL.LogMessage("ConfigureDiscovery", $"About to create Alpaca configuration form");
            alpacaConfigurationForm = new ChooserAlpacaConfigurationForm(this); // Create a new configuration form
            alpacaConfigurationForm.ShowDialog(); // Display the form as a modal dialogue box
            TL.LogMessage("ConfigureDiscovery", $"Exited Alpaca configuration form. Result: {alpacaConfigurationForm.DialogResult}");

            if (alpacaConfigurationForm.DialogResult == DialogResult.OK) // If the user clicked OK then persist the new state
            {
                TL.LogMessage("ConfigureDiscovery", $"Persisting new configuration for {deviceTypeValue}");
                WriteState(deviceTypeValue);

                ResizeChooser(); // Resize the chooser to reflect any configuration change

                InitialiseComboBox(); // ' Kick off a discover and populate the combo box or just populate the combo box if no discovery is required

            }

            alpacaConfigurationForm.Dispose(); // Dispose of the configuration form

        }

        private void MnuManageAlpacaDevices_Click(object sender, EventArgs e)
        {
            bool deviceWasRegistered;

            // Get the current registration state for the selected ProgID
            deviceWasRegistered = profile.IsRegistered(selectedProgIdValue);

            TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} of type {profile.DeviceType} is registered: {deviceWasRegistered}");

            // Run the client manager in manage mode
            RunDynamicClientManager("ManageDevices");

            // Test whether the selected ProgID has just been deleted and if so unselect the ProgID
            if (deviceWasRegistered)
            {
                // Unselect the ProgID if it has just been deleted
                if (!profile.IsRegistered(selectedProgIdValue))
                {
                    selectedChooserItem = null;
                    TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} was registered but has been deleted");
                }
                else
                {
                    TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} is still registered - no action");
                }
            }
            else
            {
                TL.LogMessage("ManageAlpacaDevicesClick", $"ProgID {selectedProgIdValue} was NOT registered - no action");
            }

            // Refresh the driver list after any changes made by the management tool
            InitialiseComboBox();

        }

        /// <summary>
        /// Creates a new Alpaca driver instance with the given descriptive name
        /// </summary>
        /// <param name="newProgId">Driver ProgID</param>
        /// <param name="deviceNumber">Device number</param>
        /// <param name="deviceDescription">Device description</param>
        /// <returns>The device ProgID</returns>
        private string CreateNewAlpacaDriver(string newProgId, int deviceNumber, string deviceDescription)
        {
            // Create the new Alpaca Client appending the device description if required 
            if (string.IsNullOrEmpty(deviceDescription))
            {
                RunDynamicClientManager($@"\CreateNamedClient {deviceTypeValue} {deviceNumber} {newProgId}");
            }
            else
            {
                RunDynamicClientManager($@"\CreateAlpacaClient {deviceTypeValue} {deviceNumber} {newProgId} ""{deviceDescription}""");
            }

            return newProgId; // Return the new ProgID
        }

        private void MnuCreateAlpacaDriver_Click(object sender, EventArgs e)
        {
            string newProgId;

            string tempProgId;
            int deviceNumber;
            Type typeFromProgId;

            // Initialise to a starting value
            deviceNumber = 0;

            // Try successive ProgIDs until one is found that is not COM registered
            do
            {
                deviceNumber += 1; // Increment the device number
                tempProgId = $"{DRIVER_PROGID_BASE}{deviceNumber}.{deviceTypeValue}"; // Create the new ProgID to be tested
                typeFromProgId = Type.GetTypeFromProgID(tempProgId); // Try to get the type with the new ProgID
                TL.LogMessage("CreateAlpacaClient", $"Testing ProgID: {tempProgId} Type name: {typeFromProgId?.Name}");
            }
            while (typeFromProgId is not null); // Loop until the returned type is null indicating that this type is not COM registered
            newProgId = tempProgId;
            TL.LogMessage("CreateAlpacaClient", $"Creating new ProgID: {newProgId}");

            // Create a new Alpaca driver of the current ASCOM device type
            newProgId = CreateNewAlpacaDriver(newProgId, deviceNumber, "");

            // Select the new driver in the Chooser combo box list
            selectedProgIdValue = newProgId;
            InitialiseComboBox();

            TL.LogMessage("OK Click", $"Returning ProgID: '{selectedProgIdValue}'");

        }

        #endregion

        #region State Persistence

        private void ReadState()
        {
            ReadState("Telescope");
        }

        private void ReadState(string DeviceType)
        {
            try
            {
                TL?.LogMessageCrLf("ChooserReadState", $"Reading state for device type: {DeviceType}. Configuration key: {CONFIGRATION_SUBKEY}, Alpaca enabled: {$"{DeviceType} {ALPACA_ENABLED}"}, ALapca default: {ALPACA_ENABLED_DEFAULT}");

                // The enabled state is per device type
                AlpacaEnabled = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", ALPACA_ENABLED_DEFAULT.ToString()), CultureInfo.InvariantCulture);

                // These values are for all Alpaca devices
                AlpacaDiscoveryPort = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, ALPACA_DISCOVERY_PORT_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaNumberOfBroadcasts = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, ALPACA_NUMBER_OF_BROADCASTS_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaTimeout = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, ALPACA_TIMEOUT_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaDnsResolution = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, ALPACA_DNS_RESOLUTION_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaShowDeviceDetails = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, ALPACA_SHOW_DEVICE_DETAILS_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaShowDiscoveredDevices = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, ALPACA_SHOW_DISCOVERED_DEVICES_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaChooserIncrementalWidth = Convert.ToInt32(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, ALPACA_CHOOSER_WIDTH_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaUseIpV4 = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV4, ALPACA_USE_IPV4_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaUseIpV6 = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV6, ALPACA_USE_IPV6_DEFAULT.ToString()), CultureInfo.InvariantCulture);
                AlpacaMultiThreadedChooser = Convert.ToBoolean(registryAccess.GetProfile(CONFIGRATION_SUBKEY, ALPACA_MULTI_THREADED_CHOOSER, ALPACA_MULTI_THREADED_CHOOSER_DEFAULT.ToString()), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Chooser Read State " + ex.ToString());
                LogEvent("Chooser Read State ", ex.ToString(), EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString());
                TL?.LogMessageCrLf("ChooserReadState", ex.ToString());
            }
        }

        private void WriteState(string DeviceType)
        {

            try
            {

                // Save the enabled state per "device type" 
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, $"{DeviceType} {ALPACA_ENABLED}", AlpacaEnabled.ToString(CultureInfo.InvariantCulture));

                // Save other states for all Alpaca devices 
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DISCOVERY_PORT, AlpacaDiscoveryPort.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_NUMBER_OF_BROADCASTS, AlpacaNumberOfBroadcasts.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_TIMEOUT, AlpacaTimeout.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_DNS_RESOLUTION, AlpacaDnsResolution.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DEVICE_DETAILS, AlpacaShowDeviceDetails.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_SHOW_DISCOVERED_DEVICES, AlpacaShowDiscoveredDevices.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_CHOOSER_WIDTH, AlpacaChooserIncrementalWidth.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV4, AlpacaUseIpV4.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_USE_IPV6, AlpacaUseIpV6.ToString(CultureInfo.InvariantCulture));
                registryAccess.WriteProfile(CONFIGRATION_SUBKEY, ALPACA_MULTI_THREADED_CHOOSER, AlpacaMultiThreadedChooser.ToString(CultureInfo.InvariantCulture));
            }

            catch (Exception ex)
            {
                Interaction.MsgBox("Chooser Write State " + ex.ToString());
                LogEvent("Chooser Write State ", ex.ToString(), EventLogEntryType.Error, EventLogErrors.ChooserFormLoad, ex.ToString());
                TL?.LogMessageCrLf("ChooserWriteState", ex.ToString());
            }

        }

        #endregion

        #region Support code

        /// <summary>
        /// Run the Alpaca dynamic client manager application with the supplied parameters
        /// </summary>
        /// <param name="parameterString">Parameter string to pass to the application</param>
        private void RunDynamicClientManager(string parameterString)
        {
            string clientManagerWorkingDirectory, clientManagerExeFile;
            ProcessStartInfo clientManagerProcessStartInfo;

            // Construct path to the executable that will dynamically create a new Alpaca COM client
            clientManagerWorkingDirectory = $@"{Get32BitProgramFilesPath()}\{ALPACA_DYNAMIC_CLIENT_MANAGER_RELATIVE_PATH}";
            clientManagerExeFile = $@"{clientManagerWorkingDirectory}\{ALPACA_DYNAMIC_CLIENT_MANAGER_EXE_NAME}";

            TL.LogMessage("RunDynamicClientManager", $"Generator parameters: '{parameterString}'");
            TL.LogMessage("RunDynamicClientManager", $"Managing drivers using the {clientManagerExeFile} executable in working directory {clientManagerWorkingDirectory}");

            if (!File.Exists(clientManagerExeFile))
            {
                Interaction.MsgBox("The client generator executable can not be found, please repair the ASCOM Platform.", MsgBoxStyle.Critical, "Alpaca Client Generator Not Found");
                TL.LogMessage("RunDynamicClientManager", $"ERROR - Unable to find the client generator executable at {clientManagerExeFile}, cannot create a new Alpaca client.");
                selectedProgIdValue = "";
                return;
            }

            // Set the process run time environment and parameters
            clientManagerProcessStartInfo = new ProcessStartInfo(clientManagerExeFile, parameterString); // Run the executable with no parameters in order to show the management GUI
            clientManagerProcessStartInfo.WorkingDirectory = clientManagerWorkingDirectory;

            // Create the management process
            ClientManagerProcess = new Process();
            ClientManagerProcess.StartInfo = clientManagerProcessStartInfo;
            ClientManagerProcess.EnableRaisingEvents = true;

            // Initialise the process complete flag to false
            driverGenerationComplete = false;

            // Run the process
            TL.LogMessage("RunDynamicClientManager", $"Starting driver management process");
            ClientManagerProcess.Start();

            // Wait for the process to complete at which point the process complete event will fire and driverGenerationComplete will be set true
            do
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }
            while (!driverGenerationComplete);

            TL.LogMessage("RunDynamicClientManager", $"Completed driver management process");

            ClientManagerProcess.Dispose();

        }

        /// <summary>
        /// Get the 32bit ProgramFiles path on both 32bit and 64bit systems
        /// </summary>
        /// <returns></returns>
        private string Get32BitProgramFilesPath()
        {
            // Try to get the 64bit path
            string returnValue = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

            // If no path is returned get the 32bit path
            if (string.IsNullOrEmpty(returnValue))
            {
                returnValue = Environment.GetEnvironmentVariable("ProgramFiles");
            }

            TL.LogMessage("Get32BitProgramFilesPath", $"Returned path: {returnValue}");
            return returnValue;
        }

        private void InitialiseComboBox()
        {

            TL.LogMessage("InitialiseComboBox", $"Arrived at InitialiseComboBox - Running On thread: {Thread.CurrentThread.ManagedThreadId}.");

            if (AlpacaMultiThreadedChooser) // Multi-threading behaviour where the Chooser UI is displayed immediately while discovery runs in the background
            {
                TL.LogMessage("InitialiseComboBox", $"Creating discovery thread...");
                var discoveryThread = new Thread(DiscoverAlpacaDevicesAndPopulateDriverComboBox);
                TL.LogMessage("InitialiseComboBox", $"Successfully created discovery thread, about to start discovery on thread {discoveryThread.ManagedThreadId}...");
                discoveryThread.Start();
                TL.LogMessage("InitialiseComboBox", $"Discovery thread started OK");
            }
            else // Single threaded behaviour where the Chooser UI is not displayed until discovery completes
            {
                TL.LogMessage("InitialiseComboBox", $"Starting single threaded discovery...");
                DiscoverAlpacaDevicesAndPopulateDriverComboBox();
                TL.LogMessage("InitialiseComboBox", $"Completed single threaded discovery");
            }

            TL.LogMessage("InitialiseComboBox", $"Exiting InitialiseComboBox on thread: {Thread.CurrentThread.ManagedThreadId}.");
        }

        private void DiscoverAlpacaDevicesAndPopulateDriverComboBox()
        {
            try
            {

                TL.LogMessage("DiscoverAlpacaDevices", $"Running On thread: {Thread.CurrentThread.ManagedThreadId}.");

                chooserList = new List<ChooserItem>();

                // Enumerate the available drivers, and load their descriptions and ProgIDs into the driversList generic sorted list collection. Key is ProgID, value is friendly name.
                try
                {
                    // Get Key-Class pairs in the sub-key "{DeviceType} Drivers" e.g. "Telescope Drivers"
                    var driverList = registryAccess.EnumKeys(deviceTypeValue + " Drivers");
                    TL.LogMessage("DiscoverAlpacaDevices", $"Returned {driverList.Count} COM drivers");

                    foreach (KeyValuePair<string, string> driver in driverList)
                    {
                        string driverProgId, driverName;

                        driverProgId = driver.Key;
                        driverName = driver.Value;

                        TL.LogMessage("PopulateDriverComboBox", $"Found ProgID: {driverProgId} , Description: '{driverName}'");

                        if (string.IsNullOrEmpty(driverName)) // Description Is missing
                        {
                            TL.LogMessage("PopulateDriverComboBox", $"  ***** Description missing for ProgID: {driverProgId}");
                        }

                        // Annotate the device description as configured
                        if (driverProgId.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant())) // This is a COM driver for an Alpaca device
                        {
                            if (AlpacaShowDeviceDetails) // Get device details from the Profile and display these
                            {
                                driverName = $"{driverName}    ({driverProgId} ==> {profile.GetValue(driverProgId, PROFILE_VALUE_NAME_IP_ADDRESS, null)}:" + $"{profile.GetValue(driverProgId, PROFILE_VALUE_NAME_PORT_NUMBER, null)}/api/v1/{deviceTypeValue}/{profile.GetValue(driverProgId, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER, null)}" + $") - {profile.GetValue(driverProgId, PROFILE_VALUE_NAME_UNIQUEID)}"; // Annotate as Alpaca Dynamic driver to differentiate from other COM drivers
                            }
                            else // Just annotate as an Alpaca device
                            {
                                driverName = $"{driverName}    (Alpaca)";
                            } // Annotate as an Alpaca device
                        }
                        else if (AlpacaShowDeviceDetails) // This is not an Alpaca COM driver
                                                          // Get device details from the Profile and display these
                        {
                            driverName = $"{driverName}    ({driverProgId})"; // Annotate with ProgID
                        }
                        else
                        {
                            // No action just use the driver description as is
                        }

                        chooserList.Add(new ChooserItem(driverProgId, driverName));
                    }
                }

                catch (Exception ex1)
                {
                    TL.LogMessageCrLf("DiscoverAlpacaDevices", "Exception: " + ex1.ToString());
                    // Ignore any exceptions from this call e.g. if there are no devices of that type installed just create an empty list
                }

                TL.LogMessage("DiscoverAlpacaDevices", $"Completed COM driver enumeration");

                if (AlpacaEnabled)
                {
                    alpacaDevices = new List<AscomDevice>(); // Initialise to a clear list with no Alpaca devices

                    // Render the user interface unresponsive while discovery is underway, except for the Cancel button.
                    SetStateAlpacaDiscovering();

                    // Initiate discovery and wait for it to complete
                    using (var discovery = new AlpacaDiscovery(TL))
                    {
                        TL.LogMessage("DiscoverAlpacaDevices", $"AlpacaDiscovery created");
                        discovery.StartDiscovery(AlpacaNumberOfBroadcasts, 200, AlpacaDiscoveryPort, AlpacaTimeout, AlpacaDnsResolution, AlpacaUseIpV4, AlpacaUseIpV6);
                        TL.LogMessage("DiscoverAlpacaDevices", $"AlpacaDiscovery started");

                        // Keep the UI alive while the discovery is running
                        do
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }
                        while (!discovery.DiscoveryComplete);
                        TL.LogMessage("DiscoverAlpacaDevices", $"Discovery phase has finished");

                        TL.LogMessage("DiscoverAlpacaDevices", $"Discovered {discovery.GetAscomDevices("").Count} devices");

                        // List discovered devices to the log
                        foreach (AscomDevice ascomDevice in discovery.GetAscomDevices(""))
                            TL.LogMessage("DiscoverAlpacaDevices", $"FOUND {ascomDevice.AscomDeviceType} {ascomDevice.AscomDeviceName} {ascomDevice.IPEndPoint}");

                        TL.LogMessage("DiscoverAlpacaDevices", $"Discovered {discovery.GetAscomDevices(deviceTypeValue).Count} {deviceTypeValue} devices");

                        // Get discovered devices of the requested ASCOM device type
                        alpacaDevices = discovery.GetAscomDevices(deviceTypeValue);
                    }

                    // Add any Alpaca devices to the list
                    foreach (AscomDevice device in alpacaDevices)
                    {
                        TL.LogMessage("DiscoverAlpacaDevices", $"Discovered Alpaca device: {device.AscomDeviceType} {device.AscomDeviceName} {device.UniqueId} at  http://{device.HostName}:{device.IPEndPoint.Port}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber}");

                        string displayHostName = Conversions.ToString(Interaction.IIf((device.HostName ?? "") == (device.IPEndPoint.Address.ToString() ?? ""), device.IPEndPoint.Address.ToString(), $"{device.HostName} ({device.IPEndPoint.Address})"));
                        string displayName;

                        string deviceUniqueId, deviceHostName;
                        int deviceIPPort, deviceNumber;

                        // Get a list of dynamic drivers already configured on the system
                        bool foundDriver = false;

                        foreach (KeyValuePair arrayListDevice in profile.RegisteredDevices(deviceTypeValue)) // Iterate over a list of all devices of the current device type
                        {
                            if (arrayListDevice.Key.ToLowerInvariant().StartsWith(DRIVER_PROGID_BASE.ToLowerInvariant())) // This is a dynamic Alpaca COM driver
                            {

                                // Get and validate the device values to compare with the discovered device
                                try
                                {
                                    deviceUniqueId = profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_UNIQUEID);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device unique ID. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    continue; // Don't process this driver further, move on to the next driver
                                }

                                try
                                {
                                    deviceHostName = profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_IP_ADDRESS);
                                    if (string.IsNullOrEmpty(deviceHostName))
                                    {
                                        MessageBox.Show($"{arrayListDevice.Key} - The device IP address is blank. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        continue; // Don't process this driver further, move on to the next driver
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device IP address. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    continue; // Don't process this driver further, move on to the next driver
                                }

                                try
                                {
                                    deviceIPPort = Convert.ToInt32(profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_PORT_NUMBER));
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device IP Port. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    continue; // Don't process this driver further, move on to the next driver
                                }

                                try
                                {
                                    deviceNumber = Convert.ToInt32(profile.GetValue(arrayListDevice.Key, PROFILE_VALUE_NAME_REMOTE_DEVICER_NUMBER));
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show($"{arrayListDevice.Key} - Unable to read the device number. This driver should be deleted and re-created", "Dynamic Driver Corrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    continue; // Don't process this driver further, move on to the next driver
                                }

                                TL.LogMessage("DiscoverAlpacaDevices", $"Found existing COM dynamic driver for device {deviceUniqueId} at http://{deviceHostName}:{deviceIPPort}/api/v1/{deviceTypeValue}/{deviceNumber}");
                                TL.LogMessage("DiscoverAlpacaDevices", $"{device.UniqueId} {deviceUniqueId} {(device.UniqueId ?? "") == (deviceUniqueId ?? "")} {(device.HostName ?? "") == (deviceHostName ?? "")} {device.IPEndPoint.Port == deviceIPPort} {device.AlpacaDeviceNumber == deviceNumber}");

                                if ((device.UniqueId ?? "") == (deviceUniqueId ?? "") & (device.HostName ?? "") == (deviceHostName ?? "") & device.IPEndPoint.Port == deviceIPPort & device.AlpacaDeviceNumber == deviceNumber)
                                {
                                    foundDriver = true;
                                    TL.LogMessage("DiscoverAlpacaDevices", $"    Found existing COM driver match!");
                                }
                            }
                        }

                        if (foundDriver)
                        {
                            TL.LogMessage("DiscoverAlpacaDevices", $"Found driver match for {device.AscomDeviceName}");
                            if (AlpacaShowDiscoveredDevices)
                            {
                                TL.LogMessage("DiscoverAlpacaDevices", $"Showing KNOWN ALPACA DEVICE entry for {device.AscomDeviceName}");
                                displayName = $"* KNOWN ALPACA DEVICE   {device.AscomDeviceName}   {displayHostName}:{device.IPEndPoint.Port}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber} - {device.UniqueId}";
                                chooserList.Add(new ChooserItem(device.UniqueId, device.AlpacaDeviceNumber, device.HostName, device.IPEndPoint.Port, device.AscomDeviceName, displayName));
                            }
                            else
                            {
                                TL.LogMessage("DiscoverAlpacaDevices", $"This device MATCHES an existing COM driver so NOT adding it to the Combo box list");
                            }
                        }

                        else
                        {
                            TL.LogMessage("DiscoverAlpacaDevices", $"This device does NOT match an existing COM driver so ADDING it to the Combo box list");
                            displayName = $"* NEW ALPACA DEVICE   {device.AscomDeviceName}   {displayHostName}:{device.IPEndPoint.Port}/api/v1/{deviceTypeValue}/{device.AlpacaDeviceNumber} - {device.UniqueId}";
                            chooserList.Add(new ChooserItem(device.UniqueId, device.AlpacaDeviceNumber, device.HostName, device.IPEndPoint.Port, device.AscomDeviceName, displayName));
                        }

                    }
                }

                // List the ChooserList contents
                TL.LogMessage("DiscoverAlpacaDevices", $"Start of Chooser List");
                foreach (ChooserItem item in chooserList)
                    TL.LogMessage("DiscoverAlpacaDevices", $"List includes device {item.AscomName}");
                TL.LogMessage("DiscoverAlpacaDevices", $"End of Chooser List");

                // Populate the device list combo box with COM and Alpaca devices.
                // This Is implemented as an independent method because it interacts with UI controls And will self invoke if required
                PopulateDriverComboBox();
            }

            catch (Exception ex)
            {
                TL.LogMessageCrLf("DiscoverAlpacaDevices", ex.ToString());
            }
            finally
            {
                // Restore a usable user interface
                if (AlpacaEnabled)
                {
                    if (alpacaDevices.Count > 0)
                    {
                        SetStateAlpacaDiscoveryCompleteFoundDevices();
                    }
                    else
                    {
                        SetStateAlpacaDiscoveryCompleteNoDevices();
                    }
                }
                else
                {
                    SetStateNoAlpaca();
                }
                DisplayAlpacaDeviceToolTip();
            }
        }

        private void PopulateDriverComboBox()
        {
            // Only proceed if there are drivers or Alpaca devices to display
            if (chooserList.Count == 0 & alpacaDevices.Count == 0) // No drivers to add to the combo box 
            {
                Interaction.MsgBox("There are no ASCOM " + deviceTypeValue + " drivers installed.", (MsgBoxStyle)((int)MsgBoxStyle.OkOnly + (int)MsgBoxStyle.Exclamation + (int)MsgBoxStyle.MsgBoxSetForeground), ALERT_MESSAGEBOX_TITLE);
                return;
            }

            if (CmbDriverSelector.InvokeRequired) // We are not running on the UI thread
            {
                TL.LogMessage("PopulateDriverComboBox", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}");
                CmbDriverSelector.Invoke(PopulateDriverComboBoxDelegate);
            }
            else // We are running on the UI thread
            {
                try
                {
                    TL.LogMessage("PopulateDriverComboBox", $"Running on thread: {Thread.CurrentThread.ManagedThreadId}");

                    // Clear the combo box list, sort the discovered drivers / devices and add them to the Chooser's combo box list
                    CmbDriverSelector.Items.Clear(); // Clear the combo box list
                    CmbDriverSelector.SelectedIndex = -1;
                    chooserList.Sort(); // Sort the ChooserItem instances into alphabetical order
                    CmbDriverSelector.Items.AddRange(chooserList.ToArray()); // Add the ChooserItem instances to the combo box
                    CmbDriverSelector.DisplayMember = "DisplayName"; // Set the name of the ChooserItem property whose contents should be displayed in the drop-down list

                    CmbDriverSelector.DropDownWidth = DropDownWidth(CmbDriverSelector); // AutoSize the combo box width

                    // If a ProgID has been provided, test whether it matches a ProgID in the driver list
                    if (!string.IsNullOrEmpty(selectedProgIdValue)) // A progID was provided
                    {

                        // Select the current device in the list
                        foreach (ChooserItem driver in CmbDriverSelector.Items)
                        {
                            TL.LogMessage("PopulateDriverComboBox", $"Searching for ProgID: {selectedProgIdValue}, found ProgID: {driver.ProgID}");
                            if ((driver.ProgID.ToLowerInvariant() ?? "") == (selectedProgIdValue.ToLowerInvariant() ?? ""))
                            {
                                TL.LogMessage("PopulateDriverComboBox", $"*** Found ProgID: {selectedProgIdValue}");
                                CmbDriverSelector.SelectedItem = driver;
                                selectedChooserItem = driver;
                                EnableOkButton(true); // Enable the OK button
                            }
                        }
                    }

                    if (selectedChooserItem is null) // The requested driver was not found so display a blank Chooser item
                    {
                        TL.LogMessage("PopulateDriverComboBox", $"Selected ProgID {selectedProgIdValue} WAS NOT found, displaying a blank combo list item");

                        CmbDriverSelector.ResetText();
                        CmbDriverSelector.SelectedIndex = -1;

                        EnablePropertiesButton(false);
                        EnableOkButton(false);
                    }
                    else
                    {
                        TL.LogMessage("PopulateDriverComboBox", $"Selected ProgID {selectedProgIdValue} WAS found. Device is: {selectedChooserItem.AscomName}, Is COM driver: {selectedChooserItem.IsComDriver}");

                        // Validate the selected driver if it is a COM driver
                        if (selectedChooserItem.IsComDriver) // This is a COM driver so validate that it is functional
                        {
                            ValidateDriver(selectedChooserItem.ProgID);
                        }
                        else // This is a new Alpaca driver
                        {
                            WarningTooltipClear();
                            EnablePropertiesButton(false); // Disable the Properties button because there is not yet a COM driver to configure
                            EnableOkButton(true);

                        }
                    }
                }

                catch (Exception ex)
                {
                    TL.LogMessageCrLf("PopulateDriverComboBox Top", "Exception: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Return the maximum width of a combo box's drop-down items
        /// </summary>
        /// <param name="comboBox">Combo box to inspect</param>
        /// <returns>Maximum width of supplied combo box drop-down items</returns>
        private int DropDownWidth(ComboBox comboBox)
        {
            int maxWidth;
            int temp;
            var label1 = new Label();

            maxWidth = comboBox.Width; // Ensure that the minimum width is the width of the combo box
            TL.LogMessage("DropDownWidth", $"Combo box: {comboBox.Name} Number of items: {comboBox.Items.Count} ");

            foreach (ChooserItem obj in comboBox.Items)
            {
                label1.Text = obj.AscomName;
                temp = label1.PreferredWidth;

                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }

            label1.Dispose();

            return maxWidth;
        }

        private void SetStateNoAlpaca()
        {
            if (CmbDriverSelector.InvokeRequired)
            {
                TL.LogMessage("SetStateNoAlpaca", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}");
                CmbDriverSelector.Invoke(SetStateNoAlpacaDelegate);
            }
            else
            {
                TL.LogMessage("SetStateNoAlpaca", $"Running on thread {Thread.CurrentThread.ManagedThreadId}");

                LblAlpacaDiscovery.Visible = false;
                CmbDriverSelector.Enabled = true;
                alpacaStatusToolstripLabel.Text = "Discovery Disabled";
                alpacaStatusToolstripLabel.BackColor = Color.Salmon;
                MnuDiscoverNow.Enabled = false;
                MnuEnableDiscovery.Enabled = true;
                MnuDisableDiscovery.Enabled = false;
                MnuConfigureChooser.Enabled = true;
                BtnProperties.Enabled = currentPropertiesButtonEnabledState;
                BtnOK.Enabled = currentOkButtonEnabledState;
                AlpacaStatus.Visible = false;
                AlpacaStatusIndicatorTimer.Stop();
            }
        }

        private void SetStateAlpacaDiscovering()
        {
            if (CmbDriverSelector.InvokeRequired)
            {
                TL.LogMessage("SetStateAlpacaDiscovering", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}");
                CmbDriverSelector.Invoke(SetStateAlpacaDiscoveringDelegate);
            }
            else
            {
                TL.LogMessage("SetStateAlpacaDiscovering", $"Running on thread {Thread.CurrentThread.ManagedThreadId} OK button enabled state: {currentOkButtonEnabledState}");
                LblAlpacaDiscovery.Visible = true;
                CmbDriverSelector.Enabled = false;
                alpacaStatusToolstripLabel.Text = "Discovery Enabled";
                alpacaStatusToolstripLabel.BackColor = Color.LightGreen;
                MnuDiscoverNow.Enabled = false;
                MnuEnableDiscovery.Enabled = false;
                MnuDisableDiscovery.Enabled = false;
                MnuConfigureChooser.Enabled = false;
                BtnProperties.Enabled = false;
                BtnOK.Enabled = false;
                AlpacaStatus.Visible = true;
                AlpacaStatus.BackColor = Color.Orange;
                AlpacaStatusIndicatorTimer.Start();
            }
        }

        private void SetStateAlpacaDiscoveryCompleteFoundDevices()
        {
            if (CmbDriverSelector.InvokeRequired)
            {
                TL.LogMessage("SetStateAlpacaDiscoveryCompleteFoundDevices", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}");
                CmbDriverSelector.Invoke(SetStateAlpacaDiscoveryCompleteFoundDevicesDelegate);
            }
            else
            {
                TL.LogMessage("SetStateAlpacaDiscoveryCompleteFoundDevices", $"Running on thread {Thread.CurrentThread.ManagedThreadId}");
                LblAlpacaDiscovery.Visible = true;
                alpacaStatusToolstripLabel.Text = "Discovery Enabled";
                alpacaStatusToolstripLabel.BackColor = Color.LightGreen;
                CmbDriverSelector.Enabled = true;
                MnuDiscoverNow.Enabled = true;
                MnuEnableDiscovery.Enabled = false;
                MnuDisableDiscovery.Enabled = true;
                MnuConfigureChooser.Enabled = true;
                BtnProperties.Enabled = currentPropertiesButtonEnabledState;
                BtnOK.Enabled = currentOkButtonEnabledState;
                AlpacaStatus.Visible = true;
                AlpacaStatus.BackColor = Color.Lime;
                AlpacaStatusIndicatorTimer.Stop();
            }
        }

        private void SetStateAlpacaDiscoveryCompleteNoDevices()
        {
            if (CmbDriverSelector.InvokeRequired)
            {
                TL.LogMessage("SetStateAlpacaDiscoveryCompleteNoDevices", $"InvokeRequired from thread {Thread.CurrentThread.ManagedThreadId}");
                CmbDriverSelector.Invoke(SetStateAlpacaDiscoveryCompleteNoDevicesDelegate);
            }
            else
            {
                TL.LogMessage("SetStateAlpacaDiscoveryCompleteNoDevices", $"Running on thread {Thread.CurrentThread.ManagedThreadId}");
                LblAlpacaDiscovery.Visible = true;
                alpacaStatusToolstripLabel.Text = "Discovery Enabled";
                alpacaStatusToolstripLabel.BackColor = Color.LightGreen;
                CmbDriverSelector.Enabled = true;
                MnuDiscoverNow.Enabled = true;
                MnuEnableDiscovery.Enabled = false;
                MnuDisableDiscovery.Enabled = true;
                MnuConfigureChooser.Enabled = true;
                BtnProperties.Enabled = currentPropertiesButtonEnabledState;
                BtnOK.Enabled = currentOkButtonEnabledState;
                AlpacaStatus.Visible = true;
                AlpacaStatus.BackColor = Color.Red;
                AlpacaStatusIndicatorTimer.Stop();
            }
        }

        private void ValidateDriver(string progId)
        {
            string deviceInitialised;

            if (!string.IsNullOrEmpty(progId))
            {

                if (!string.IsNullOrEmpty(progId)) // Something selected
                {

                    WarningTooltipClear(); // Hide any previous message

                    TL.LogMessage("ValidateDriver", "ProgID:" + progId + ", Bitness: " + ApplicationBits().ToString());
                    driverIsCompatible = DriverCompatibilityMessage(progId, ApplicationBits(), TL); // Get compatibility warning message, if any

                    if (!string.IsNullOrEmpty(driverIsCompatible)) // This is an incompatible driver so we need to prevent access
                    {
                        EnablePropertiesButton(false);
                        EnableOkButton(false);
                        TL.LogMessage("ValidateDriver", "Showing incompatible driver message");
                        WarningToolTipShow("Incompatible Driver (" + progId + ")", driverIsCompatible);
                    }
                    else // This is a compatible driver
                    {
                        EnablePropertiesButton(true); // Turn on Properties
                        deviceInitialised = registryAccess.GetProfile("Chooser", progId + " Init");
                        if (Strings.LCase(deviceInitialised) == "true") // This device has been initialized
                        {
                            EnableOkButton(true);
                            currentWarningMesage = "";
                            TL.LogMessage("ValidateDriver", "Driver is compatible and configured so no message");
                        }
                        else // This device has not been initialised
                        {
                            selectedProgIdValue = "";
                            EnableOkButton(false); // Ensure OK is disabled
                            TL.LogMessage("ValidateDriver", "Showing first time configuration required message");
                            WarningToolTipShow(TOOLTIP_PROPERTIES_TITLE, TOOLTIP_PROPERTIES_FIRST_TIME_MESSAGE);
                        }
                    }
                }
                else // Nothing has been selected
                {
                    TL.LogMessage("ValidateDriver", "Nothing has been selected");
                    selectedProgIdValue = "";
                    EnablePropertiesButton(false);
                    EnableOkButton(false);
                } // Ensure OK is disabled
            }

        }

        private void WarningToolTipShow(string Title, string Message)
        {
            const int MESSAGE_X_POSITION = 120; // Was 18

            WarningTooltipClear();
            chooserWarningToolTip.UseAnimation = true;
            chooserWarningToolTip.UseFading = false;
            chooserWarningToolTip.ToolTipIcon = ToolTipIcon.Warning;
            chooserWarningToolTip.AutoPopDelay = 5000;
            chooserWarningToolTip.InitialDelay = 0;
            chooserWarningToolTip.IsBalloon = false;
            chooserWarningToolTip.ReshowDelay = 0;
            chooserWarningToolTip.OwnerDraw = false;
            chooserWarningToolTip.ToolTipTitle = Title;
            currentWarningTitle = Title;
            currentWarningMesage = Message;

            if (Message.Contains(Microsoft.VisualBasic.Constants.vbCrLf))
            {
                chooserWarningToolTip.Show(Message, this, MESSAGE_X_POSITION, 24); // Display at position for a two line message
            }
            else
            {
                chooserWarningToolTip.Show(Message, this, MESSAGE_X_POSITION, 50);
            } // Display at position for a one line message
        }

        private delegate void NoParameterDelegate();
        private NoParameterDelegate displayCreateAlpacDeviceTooltip;

        private void DisplayAlpacaDeviceToolTip()
        {
            ChooserItem selectedItem;

            // Only consider displaying the tooltip if it has been instantiated
            if (createAlpacaDeviceToolTip is not null)
            {

                // Only display the tooltip if Alpaca discovery is enabled and the Alpaca dialogues have NOT been suppressed
                if (AlpacaEnabled & !GetBool(SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE, SUPPRESS_ALPACA_DRIVER_ADMIN_DIALOGUE_DEFAULT))
                {

                    // The tooltip code must be executed by the UI thread so invoke this if required
                    if (BtnOK.InvokeRequired)
                    {
                        TL.LogMessage("DisplayAlpacaDeviceToolTip", $"Invoke required on thread {Thread.CurrentThread.ManagedThreadId}");
                        BtnOK.Invoke(displayCreateAlpacDeviceTooltip);
                    }
                    // Only display the tooltip if a device has been selected
                    else if (CmbDriverSelector.SelectedItem is not null)
                    {
                        selectedItem = (ChooserItem)CmbDriverSelector.SelectedItem;

                        // Only display the tooltip if the an Alpaca driver has been selected
                        if (!selectedItem.IsComDriver)
                        {

                            createAlpacaDeviceToolTip.RemoveAll();

                            createAlpacaDeviceToolTip.UseAnimation = true;
                            createAlpacaDeviceToolTip.UseFading = false;
                            createAlpacaDeviceToolTip.ToolTipIcon = ToolTipIcon.Info;
                            createAlpacaDeviceToolTip.AutoPopDelay = 5000;
                            createAlpacaDeviceToolTip.InitialDelay = 0;
                            createAlpacaDeviceToolTip.IsBalloon = true;
                            createAlpacaDeviceToolTip.ReshowDelay = 0;
                            createAlpacaDeviceToolTip.OwnerDraw = false;
                            createAlpacaDeviceToolTip.ToolTipTitle = TOOLTIP_CREATE_ALPACA_DEVICE_TITLE;

                            createAlpacaDeviceToolTip.Show(TOOLTIP_CREATE_ALPACA_DEVICE_MESSAGE, BtnOK, 45, -60, TOOLTIP_CREATE_ALPACA_DEVICE_DISPLAYTIME * 1000); // Display at position for a two line message
                            TL.LogMessage("DisplayAlpacaDeviceToolTip", $"Set tooltip on thread {Thread.CurrentThread.ManagedThreadId}");
                        }

                    }
                }
            }
        }

        private void WarningTooltipClear()
        {
            chooserWarningToolTip.RemoveAll();
            currentWarningTitle = "";
            currentWarningMesage = "";
        }


        private void ResizeChooser()
        {
            // Position controls if the Chooser has an increased width
            Width = OriginalForm1Width + AlpacaChooserIncrementalWidth;
            BtnCancel.Location = new Point(OriginalBtnCancelPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnCancelPosition.Y);
            BtnOK.Location = new Point(OriginalBtnOKPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnOKPosition.Y);
            BtnProperties.Location = new Point(OriginalBtnPropertiesPosition.X + AlpacaChooserIncrementalWidth, OriginalBtnPropertiesPosition.Y);
            CmbDriverSelector.Width = OriginalCmbDriverSelectorWidth + AlpacaChooserIncrementalWidth;
            LblAlpacaDiscovery.Left = OriginalLblAlpacaDiscoveryPosition + AlpacaChooserIncrementalWidth;
            AlpacaStatus.Left = OriginalAlpacaStatusPosition + AlpacaChooserIncrementalWidth;
            DividerLine.Width = OriginalDividerLineWidth + AlpacaChooserIncrementalWidth;

        }

        /// <summary>
        /// Set the enabled state of the OK button and record this as the current state
        /// </summary>
        /// <param name="state"></param>
        private void EnableOkButton(bool state)
        {
            BtnOK.Enabled = state;
            currentOkButtonEnabledState = state;
        }

        /// <summary>
        /// Set the enabled state of the Properties button and record this as the current state
        /// </summary>
        /// <param name="state"></param>
        private void EnablePropertiesButton(bool state)
        {
            BtnProperties.Enabled = state;
            currentPropertiesButtonEnabledState = state;
        }

        #endregion

    }
}