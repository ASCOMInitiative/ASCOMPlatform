using ASCOM.Common;
using ASCOM.Common.Alpaca;
using ASCOM.Common.Interfaces;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASCOM.DynamicClients
{
    public partial class SetupDialogForm : Form
    {
        #region Constants

        const string CLEAN_URL = "([a-zA-Z0-9[:]*)([%a-zA-Z0-9]*)([]])"; // The first group is the required IP address, the second group is the unwanted scope id

        #endregion

        #region Variables

        private readonly ILogger TL;

        private bool selectByMouse = false; // Variable to help select the whole contents of a numeric up-down box when tabbed into our selected by mouse

        // Create validating regular expression
        readonly Regex validHostnameRegex = new Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$", RegexOptions.IgnoreCase | RegexOptions.Compiled);//ValidateHostNameRegex();

        // Set up a regular expression to parse out the IP address from n IPV6 address string, removing the scope id %XX element.
        readonly Regex cleanIpV6Address = new Regex(@"([a-zA-Z0-9[:]*)([%a-zA-Z0-9]*)([]])", RegexOptions.IgnoreCase | RegexOptions.Compiled);//CleanIpV6Address();

        #endregion

        #region Public properties for setup form variables

        public string DriverDisplayName { get; set; }
        public ServiceType ServiceType { get; set; }
        public string IPAddressString { get; set; }
        public int PortNumber { get; set; }
        public int RemoteDeviceNumber { get; set; }
        public int EstablishConnectionTimeout { get; set; }
        public int StandardTimeout { get; set; }
        public int LongTimeout { get; set; }
        public string UserNameEncrypted { get; set; }
        public string PasswordEncrypted { get; set; }
        public bool TraceState { get; set; }
        public bool DebugTraceState { get; set; }
        public bool ManageConnectLocally { get; set; }
        public ImageArrayTransferType ImageArrayTransferType { get; set; }
        public ImageArrayCompression ImageArrayCompression { get; set; }
        public string DeviceType { get; set; }
        public bool EnableRediscovery { get; set; }
        public bool IpV4Enabled { get; set; }
        public bool IpV6Enabled { get; set; }
        public int DiscoveryPort { get; set; }
        public bool TrustUserGeneratedSslCertificates { get; set; }

        #endregion

        #region Initialisation and Form Load

        public SetupDialogForm()
        {
            InitializeComponent();

            // Event handlers to paint drop down lists white rather than the default grey.
            cmbServiceType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbServiceType.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            cmbServiceType.DrawMode = DrawMode.OwnerDrawFixed;
            CmbImageArrayTransferType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbImageArrayTransferType.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            CmbImageArrayTransferType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbImageArrayCompression.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
            cmbImageArrayCompression.DrawMode = DrawMode.OwnerDrawFixed;

            // Create event handlers to select the whole contents of the numeric up-down boxes when tabbed into or selected by mouse click
            numPort.Enter += NumericUpDown_Enter;
            numPort.MouseDown += NumericUpDown_MouseDown;
            numRemoteDeviceNumber.Enter += NumericUpDown_Enter;
            numRemoteDeviceNumber.MouseDown += NumericUpDown_MouseDown;
            numEstablishCommunicationsTimeout.Enter += NumericUpDown_Enter;
            numEstablishCommunicationsTimeout.MouseDown += NumericUpDown_MouseDown;
            numStandardTimeout.Enter += NumericUpDown_Enter;
            numStandardTimeout.MouseDown += NumericUpDown_MouseDown;
            numLongTimeout.Enter += NumericUpDown_Enter;
            numLongTimeout.MouseDown += NumericUpDown_MouseDown;

            // Add event handler to validate the supplied host name
            addressList.Validating += AddressList_Validating;

            // Add event handler to enable / disable the compression combo box depending on whether JSON or Base64HandOff is selected
            CmbImageArrayTransferType.SelectedValueChanged += CmbImageArrayTransferType_SelectedValueChanged;
        }

        public SetupDialogForm(ILogger TraceLogger) : this()
        {
            TL = TraceLogger;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                LogDebug( "SetupForm Load", "Start");

                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                this.Text = $"{DriverDisplayName} Configuration - Version {version} - {DeviceType}";

                // Initialise controls
                cmbServiceType.Text = ServiceType.ToString();
                numPort.Value = PortNumber;
                numRemoteDeviceNumber.Value = RemoteDeviceNumber;
                numEstablishCommunicationsTimeout.Value = Convert.ToDecimal(EstablishConnectionTimeout);
                numStandardTimeout.Value = Convert.ToDecimal(StandardTimeout);
                numLongTimeout.Value = Convert.ToDecimal(LongTimeout);

                // Decrypt the user name if present
                if (string.IsNullOrWhiteSpace(UserNameEncrypted)) txtUserName.Text = "";
                else txtUserName.Text = UserNameEncrypted.Unencrypt(TL);

                // Decrypt the password if present
                if (string.IsNullOrWhiteSpace(PasswordEncrypted)) txtPassword.Text = "";
                else txtPassword.Text = PasswordEncrypted.Unencrypt(TL);

                chkTrace.Checked = TraceState;
                chkDebugTrace.Checked = DebugTraceState;
                ChkEnableRediscovery.Checked = EnableRediscovery;
                NumDiscoveryPort.Value = Convert.ToDecimal(DiscoveryPort);

                ChkTrustSelfSignedCertificates.Checked = TrustUserGeneratedSslCertificates;

                // Set the IP v4 / v6 radio boxes
                if (IpV4Enabled & IpV6Enabled) // Both IPv4 and v6 are enabled so set the "both" button
                {
                    RadIpV4AndV6.Checked = true;
                }
                else // Only one of v4 or v6 is enabled so set accordingly 
                {
                    RadIpV4.Checked = IpV4Enabled;
                    RadIpV6.Checked = IpV6Enabled;
                }

                // Populate the address list combo box  
                PopulateAddressList();

                if (ManageConnectLocally)
                {
                    radManageConnectLocally.Checked = true;
                }
                else
                {
                    radManageConnectRemotely.Checked = true;
                }

                CmbImageArrayTransferType.Items.Add(ASCOM.Common.Alpaca.ImageArrayTransferType.JSON);
                CmbImageArrayTransferType.Items.Add(ASCOM.Common.Alpaca.ImageArrayTransferType.Base64HandOff);
                CmbImageArrayTransferType.Items.Add(ASCOM.Common.Alpaca.ImageArrayTransferType.ImageBytes);
                CmbImageArrayTransferType.Items.Add(ASCOM.Common.Alpaca.ImageArrayTransferType.BestAvailable);
                CmbImageArrayTransferType.SelectedItem = ImageArrayTransferType;

                cmbImageArrayCompression.Items.Add(ASCOM.Common.Alpaca.ImageArrayCompression.None);
                cmbImageArrayCompression.Items.Add(ASCOM.Common.Alpaca.ImageArrayCompression.Deflate);
                cmbImageArrayCompression.Items.Add(ASCOM.Common.Alpaca.ImageArrayCompression.GZip);
                cmbImageArrayCompression.Items.Add(ASCOM.Common.Alpaca.ImageArrayCompression.GZipOrDeflate);
                cmbImageArrayCompression.SelectedItem = ImageArrayCompression;

                // Make the ImageArray transfer configuration drop-downs visible only when a camera driver is being accessed.
                if (DeviceType == "Camera")
                {
                    cmbImageArrayCompression.Visible = true;
                    CmbImageArrayTransferType.Visible = true;
                    LabImageArrayConfiguration1.Visible = true;
                    LabImageArrayConfiguration2.Visible = true;
                }
                else
                {
                    cmbImageArrayCompression.Visible = false;
                    CmbImageArrayTransferType.Visible = false;
                    LabImageArrayConfiguration1.Visible = false;
                    LabImageArrayConfiguration2.Visible = false;
                }

                // Handle cases where the stored registry value is not one of the currently supported modes
                if (CmbImageArrayTransferType.SelectedItem == null) CmbImageArrayTransferType.SelectedItem = SharedConstants.DEFAULT_IMAGE_ARRAY_TRANSFER_TYPE;
                if (cmbImageArrayCompression.SelectedItem == null) cmbImageArrayCompression.SelectedItem = SharedConstants.DEFAULT_IMAGE_ARRAY_COMPRESSION;

                // Bring the setup dialogue to the front of the screen
                if (WindowState == FormWindowState.Minimized)
                    WindowState = FormWindowState.Normal;
                else
                {
                    TopMost = true;
                    Focus();
                    BringToFront();
                    TopMost = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception initialising Dynamic Driver: " + ex.ToString());
            }
        }

        #endregion

        #region Event handlers

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TraceState = chkTrace.Checked;
                DebugTraceState = chkDebugTrace.Checked;
                IPAddressString = addressList.Text.Trim();
                PortNumber = (int)numPort.Value;
                RemoteDeviceNumber = (int)numRemoteDeviceNumber.Value;
                ServiceType = cmbServiceType.Text.ToLowerInvariant() == "https" ? ServiceType.Https : ServiceType.Http;
                EstablishConnectionTimeout = Convert.ToInt32(numEstablishCommunicationsTimeout.Value);
                StandardTimeout = Convert.ToInt32(numStandardTimeout.Value);
                LongTimeout = Convert.ToInt32(numLongTimeout.Value);

                // Encrypt user name if present
                if (string.IsNullOrWhiteSpace(txtUserName.Text)) UserNameEncrypted = "";
                else UserNameEncrypted = txtUserName.Text.Encrypt(TL); // Encrypt the provided username

                // Encrypt password if present
                if (string.IsNullOrWhiteSpace(txtPassword.Text)) PasswordEncrypted = "";
                else PasswordEncrypted = txtPassword.Text.Encrypt(TL);  // Encrypt the provided password

                ManageConnectLocally = radManageConnectLocally.Checked;
                ImageArrayTransferType = (ImageArrayTransferType)CmbImageArrayTransferType.SelectedItem;
                ImageArrayCompression = (ImageArrayCompression)cmbImageArrayCompression.SelectedItem;
                EnableRediscovery = ChkEnableRediscovery.Checked;
                DiscoveryPort = Convert.ToInt32(NumDiscoveryPort.Value);
                TrustUserGeneratedSslCertificates = ChkTrustSelfSignedCertificates.Checked;

                // Set the IP v4 and v6 variables as necessary
                if (RadIpV4.Checked) // The IPv4 radio button is checked so set the IP v4 and IP v6 variables accordingly
                {
                    IpV4Enabled = true;
                    IpV6Enabled = false;
                }
                if (RadIpV6.Checked) // The IPv6 radio button is checked so set the IP v4 and IP v6 variables accordingly
                {
                    IpV4Enabled = false;
                    IpV6Enabled = true;
                }
                if (RadIpV4AndV6.Checked) // The IPv4 and IPV6 radio button is checked so set the IP v4 and IP v6 variables accordingly
                {
                    IpV4Enabled = true;
                    IpV6Enabled = true;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception on saving new configuration: {ex.Message}", "Error saving new configuration.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TL.LogMessage(LogLevel.Error, "Setup-OK-Button", $"Exception: {ex.Message}\r\n{ex}", includeLib: false);
            }

            Close();
        }

        private void BtnSetupUrlMain_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{cmbServiceType.Text.ToLowerInvariant()}://{CleanUrl(addressList.Text)}:{numPort.Value}/setup";
                LogDebug( "MainSetupURL", $"{setupUrl}");

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = setupUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                LogDebug( "MainSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSetupUrlDevice_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{cmbServiceType.Text.ToLowerInvariant()}://{CleanUrl(addressList.Text)}:{numPort.Value}/setup/v1/{DeviceType.ToLowerInvariant()}/{numRemoteDeviceNumber.Value}/setup";
                LogDebug( "DeviceSetupURL", $"{setupUrl}");

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = setupUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                LogDebug( "ASCOMSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler to paint the device list combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return; // Do not paint if no item is selected

            ComboBox combo = sender as ComboBox;

            if (!combo.Enabled) return; // Do not paint if the combo box is disabled

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw the selected item in menu highlight colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Event handler fired when entering a numeric up/down control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            NumericUpDown curBox = sender as NumericUpDown;
            curBox.Select();
            curBox.Select(0, curBox.Text.Length);
            if (MouseButtons == MouseButtons.Left)
            {
                selectByMouse = true;
            }
        }

        /// <summary>
        /// Event handler fired when a mouse down event happens in a numeric up/down control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_MouseDown(object sender, MouseEventArgs e)
        {
            NumericUpDown curBox = sender as NumericUpDown;
            if (selectByMouse)
            {
                curBox.Select(0, curBox.Text.Length);
                selectByMouse = false;
            }
        }

        private void AddressList_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isValid = false; // Assume that the address is invalid until proven otherwise
            LogDebug( "AddressList_Validating", $"Address item: {addressList.Text}");

            // Test whether the supplied IP address is valid and, if it is an IPv6 address, test whether it is in canonical form
            if (IPAddress.TryParse(addressList.Text.Trim(), out IPAddress ipAddress)) // The host name is an IP address
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6) // This is an IPv6 address
                {
                    LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is an IPv6 address");
                    if (addressList.Text.Trim().StartsWith("[") & addressList.Text.Trim().EndsWith("]"))
                    {
                        LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is a canonical IPv6 address");
                        // The IP v6 address is already in canonical form, no action required
                        isValid = true;
                    }
                    else // The IPv6 address is not in canonical form so we need to add square brackets
                    {
                        LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is NOT a canonical IPv6 address");
                        SetupErrorProvider.SetError(addressList, "IPv6 addresses must be in canonical form i.e. start with [ and end with ].");
                    }
                }
                else // This is an IPv4 address
                {
                    // The IP v4 address is already in canonical form, no action required
                    isValid = true;
                }
            }
            else // The host name is either an invalid IP address or a string so validate this
            {
                LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is NOT a valid IP address");
                MatchCollection matches = validHostnameRegex.Matches(addressList.Text);
                if (matches.Count == 0)
                {
                    LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is NOT a valid IP address or Host Name");
                    SetupErrorProvider.SetError(addressList, "Not a valid IP address or host name.");
                }
                else
                {
                    LogDebug( "AddressList_Validating", $"Address item: {addressList.Text} is a valid Host Name");
                    isValid = true;
                }
            }

            if (isValid)
            {
                SetupErrorProvider.Clear();
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }

        }

        private void CmbImageArrayTransferType_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((ASCOM.Common.Alpaca.ImageArrayTransferType)CmbImageArrayTransferType.SelectedItem == ASCOM.Common.Alpaca.ImageArrayTransferType.JSON)
            {
                cmbImageArrayCompression.Enabled = true;
            }
            else
            {
                cmbImageArrayCompression.Enabled = false;
            }
        }

        private void RadIpV4_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void RadIpV6_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void ChkRequest100Continue_CheckedChanged(object sender, EventArgs e)
        {

        }


        #endregion

        #region Support Code

        private void RadIpV4AndV6_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void PopulateAddressList()
        {
            bool foundAnIPAddress = false;
            bool foundTheIPAddress = false;
            int selectedIndex = 0;

            LogDebug( "PopulateAddressList", "Start");

            addressList.Items.Clear();

            // Add IPv4 addresses
            if (RadIpV4.Checked | RadIpV4AndV6.Checked) // IPv4 addresses are required
            {
                // Add a local host entry
                addressList.Items.Add(SharedConstants.LOCALHOST_NAME_IPV4); // Make "localhost" the first entry in the list of IPv4 addresses
                foreach (IPAddress ipAddress in HostPc.IpV4Addresses)
                {
                    addressList.Items.Add(ipAddress.ToString());
                    LogDebug( "PopulateAddressList", string.Format("  Added {0} Address: {1}", ipAddress.AddressFamily.ToString(), ipAddress.ToString()));

                    foundAnIPAddress = true;

                    if (ipAddress.ToString() == IPAddressString)
                    {
                        selectedIndex = addressList.Items.Count - 1;
                        foundTheIPAddress = true;
                    }
                }
            }

            // Add IPv6 addresses
            if (RadIpV6.Checked | RadIpV4AndV6.Checked) // IPv6 addresses are required
            {
                foreach (IPAddress ipAddress in HostPc.IpV6Addresses)
                {
                    addressList.Items.Add($"[{ipAddress}]");
                    LogDebug( "PopulateAddressList", string.Format("  Added {0} Address: {1}", ipAddress.AddressFamily.ToString(), ipAddress.ToString()));

                    foundAnIPAddress = true;

                    if ($"[{ipAddress}]" == IPAddressString)
                    {
                        selectedIndex = addressList.Items.Count - 1;
                        foundTheIPAddress = true;
                    }
                }
            }

            LogDebug( "PopulateAddressList", string.Format($"Found an IP address: {foundAnIPAddress}, Found the IP address: {foundTheIPAddress}, Stored IP Address: {IPAddressString}"));

            if ((!foundTheIPAddress) & (IPAddressString != "")) // Add the last stored IP address if it isn't found in the search above
            {
                if (IPAddressString == "+") // Handle the "all addresses special case
                {
                    addressList.Items.Add(IPAddressString); // Add the stored address to the list
                    selectedIndex = addressList.Items.Count - 1; // Select this item in the list
                }
                else  // One specific address so just add it as provided (it may be an IP address or could be a DNS style host name)
                {
                    addressList.Items.Add(IPAddressString); // Add the stored address to the list
                    selectedIndex = addressList.Items.Count - 1; // Select this item in the list
                }
            }

            // Add the wild card addresses at the end of the list

            // Include the strong wild card character in the list of addresses if not already in use
            if (IPAddressString != SharedConstants.STRONG_WILDCARD_NAME) addressList.Items.Add(SharedConstants.STRONG_WILDCARD_NAME);

            // Set the combo box selected item
            addressList.SelectedIndex = selectedIndex;
            SetupErrorProvider.Clear();
        }

        /// <summary>
        /// Remove the Scope ID from an IPV6 address string if present
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string CleanUrl(string url)
        {
            string cleanUrl = url;
            LogDebug( "CleanUrl", $"Input URL: {cleanUrl}");

            if (url.Contains("%"))
            {
                Match match = cleanIpV6Address.Match(cleanUrl);
                if (match.Success)
                {
                    cleanUrl = $"{match.Groups[1].Value}]";
                    LogDebug( "CleanUrl", $"Cleaned URL to: {cleanUrl}. Match 1: {match.Groups[1]}, Match 2: {match.Groups[2]}");
                }
            }

            LogDebug( "CleanUrl", $"Returned URL: {cleanUrl}");
            return cleanUrl;
        }

        /// <summary>
        /// Log a debug message without adding [Lib]
        /// </summary>
        /// <param name="method"></param>
        /// <param name="message"></param>
        private void LogDebug(string method, string message)
        {
            TL?.LogMessage(LogLevel.Debug, method, message, includeLib: false);
        }

        #endregion

        //#region Regex Partial Classes

        //[GeneratedRegex("^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\\-]*[A-Za-z0-9])$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-GB")]
        //private static partial Regex ValidateHostNameRegex();

        //[GeneratedRegex("([a-zA-Z0-9[:]*)([%a-zA-Z0-9]*)([]])", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-GB")]
        //private static partial Regex CleanIpV6Address();

        //#endregion

    }
}
