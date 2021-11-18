using ASCOM.Common.Alpaca;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASCOM.DynamicRemoteClients
{
    public partial class SetupDialogForm : Form
    {

        #region Variables

        private TraceLoggerPlus TL;

        private bool selectByMouse = false; // Variable to help select the whole contents of a numeric up-down box when tabbed into our selected by mouse

        // Create validating regular expression
        Regex validHostnameRegex = new Regex(AlpacaConstants.ValidHostnameRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //Regex validIpAddressRegex = new Regex(AlpacaConstants.ValidIpAddressRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Public Properties

        public string DriverDisplayName { get; set; }
        public string ServiceType { get; set; }
        public string IPAddressString { get; set; }
        public decimal PortNumber { get; set; }
        public decimal RemoteDeviceNumber { get; set; }
        public decimal EstablishConnectionTimeout { get; set; }
        public decimal StandardTimeout { get; set; }
        public decimal LongTimeout { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool TraceState { get; set; }
        public bool DebugTraceState { get; set; }
        public bool ManageConnectLocally { get; set; }
        public ASCOM.Common.Alpaca.ImageArrayTransferType ImageArrayTransferType { get; set; }
        public ASCOM.Common.Alpaca.ImageArrayCompression ImageArrayCompression { get; set; }
        public string DeviceType { get; set; }
        public bool EnableRediscovery { get; set; }
        public bool IpV4Enabled { get; set; }
        public bool IpV6Enabled { get; set; }
        public int DiscoveryPort { get; set; }

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

        public SetupDialogForm(TraceLoggerPlus TraceLogger) : this()
        {
            TL = TraceLogger;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                TL.LogMessage("SetupForm Load", "Start");

                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                this.Text = $"{DriverDisplayName} Configuration - Version {version} - {DeviceType}";

                // Initialise controls
                cmbServiceType.Text = ServiceType;
                numPort.Value = PortNumber;
                numRemoteDeviceNumber.Value = RemoteDeviceNumber;
                numEstablishCommunicationsTimeout.Value = Convert.ToDecimal(EstablishConnectionTimeout);
                numStandardTimeout.Value = Convert.ToDecimal(StandardTimeout);
                numLongTimeout.Value = Convert.ToDecimal(LongTimeout);
                txtUserName.Text = UserName.Unencrypt(TL);
                txtPassword.Text = Password.Unencrypt(TL);
                chkTrace.Checked = TraceState;
                chkDebugTrace.Checked = DebugTraceState;
                ChkEnableRediscovery.Checked = EnableRediscovery;
                NumDiscoveryPort.Value = Convert.ToDecimal(DiscoveryPort);

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
                if (CmbImageArrayTransferType.SelectedItem == null) CmbImageArrayTransferType.SelectedItem = AlpacaConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT;
                if (cmbImageArrayCompression.SelectedItem == null) cmbImageArrayCompression.SelectedItem = AlpacaConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT;

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
            TraceState = chkTrace.Checked;
            DebugTraceState = chkDebugTrace.Checked;
            IPAddressString = addressList.Text.Trim();
            PortNumber = numPort.Value;
            RemoteDeviceNumber = numRemoteDeviceNumber.Value;
            ServiceType = cmbServiceType.Text;
            EstablishConnectionTimeout = Convert.ToInt32(numEstablishCommunicationsTimeout.Value);
            StandardTimeout = Convert.ToInt32(numStandardTimeout.Value);
            LongTimeout = Convert.ToInt32(numLongTimeout.Value);
            UserName = txtUserName.Text.Encrypt(TL); // Encrypt the provided username and password
            Password = txtPassword.Text.Encrypt(TL);
            ManageConnectLocally = radManageConnectLocally.Checked;
            ImageArrayTransferType = (ASCOM.Common.Alpaca.ImageArrayTransferType)CmbImageArrayTransferType.SelectedItem;
            ImageArrayCompression = (ASCOM.Common.Alpaca.ImageArrayCompression)cmbImageArrayCompression.SelectedItem;
            EnableRediscovery = ChkEnableRediscovery.Checked;
            DiscoveryPort = Convert.ToInt32(NumDiscoveryPort.Value);

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
            Close();
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
            TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text}");

            // Test whether the supplied IP address is valid and, if it is an IPv6 address, test whether it is in canonical form
            if (IPAddress.TryParse(addressList.Text.Trim(), out IPAddress ipAddress)) // The host name is an IP address
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6) // This is an IPv6 address
                {
                    TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is an IPv6 address");
                    if (addressList.Text.Trim().StartsWith("[") & addressList.Text.Trim().EndsWith("]"))
                    {
                        TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is a canonical IPv6 address");
                        // The IP v6 address is already in canonical form, no action required
                        isValid = true;
                    }
                    else // The IPv6 address is not in canonical form so we need to add square brackets
                    {
                        TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is NOT a canonical IPv6 address");
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
                TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is NOT a valid IP address");
                MatchCollection matches = validHostnameRegex.Matches(addressList.Text);
                if (matches.Count == 0)
                {
                    TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is NOT a valid IP address or Host Name");
                    SetupErrorProvider.SetError(addressList, "Not a valid IP address or host name.");
                }
                else
                {
                    TL.LogMessage("AddressList_Validating", $"Address item: {addressList.Text} is a valid Host Name");
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

        #endregion

        #region Support Code
        #endregion

        private void RadIpV4_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void RadIpV6_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void RadIpV4AndV6_CheckedChanged(object sender, EventArgs e)
        {
            PopulateAddressList();
        }

        private void PopulateAddressList()
        {
            bool foundAnIPAddress = false;
            bool foundTheIPAddress = false;
            int selectedIndex = 0;

            TL.LogMessage(0, 0, 0, "PopulateAddressList", "Start");

            addressList.Items.Clear();

            // Add IPv4 addresses
            if (RadIpV4.Checked | RadIpV4AndV6.Checked) // IPv4 addresses are required
            {
                // Add a local host entry
                addressList.Items.Add(AlpacaConstants.LOCALHOST_NAME_IPV4); // Make "localhost" the first entry in the list of IPv4 addresses
                foreach (IPAddress ipAddress in HostPc.IpV4Addresses)
                {
                    addressList.Items.Add(ipAddress.ToString());
                    TL.LogMessage(0, 0, 0, "PopulateAddressList", string.Format("  Added {0} Address: {1}", ipAddress.AddressFamily.ToString(), ipAddress.ToString()));

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
                    TL.LogMessage(0, 0, 0, "PopulateAddressList", string.Format("  Added {0} Address: {1}", ipAddress.AddressFamily.ToString(), ipAddress.ToString()));

                    foundAnIPAddress = true;

                    if ($"[{ipAddress}]" == IPAddressString)
                    {
                        selectedIndex = addressList.Items.Count - 1;
                        foundTheIPAddress = true;
                    }
                }
            }

            TL.LogMessage(0, 0, 0, "PopulateAddressList", string.Format($"Found an IP address: {foundAnIPAddress}, Found the IP address: {foundTheIPAddress}, Stored IP Address: {IPAddressString}"));

            if ((!foundTheIPAddress) & (IPAddressString != "")) // Add the last stored IP address if it isn't found in the search above
            {
                if (IPAddressString == "+") // Handle the "all addresses special case
                {
                    addressList.Items.Add(IPAddressString); // Add the stored address to the list
                    selectedIndex = addressList.Items.Count - 1; // Select this item in the list
                }
                else  // One specific address so add it if it parses OK
                {
                    IPAddress serverIpAddress = IPAddress.Parse(IPAddressString);
                    if (
                            ((serverIpAddress.AddressFamily == AddressFamily.InterNetwork) & ((RadIpV4.Checked | RadIpV4AndV6.Checked))) |
                            ((serverIpAddress.AddressFamily == AddressFamily.InterNetworkV6) & ((RadIpV6.Checked | RadIpV4AndV6.Checked)))
                       )
                    {
                        addressList.Items.Add(IPAddressString); // Add the stored address to the list
                        selectedIndex = addressList.Items.Count - 1; // Select this item in the list
                    }
                    else selectedIndex = 0;
                }
            }

            // Add the wild card addresses at the end of the list

            // Include the strong wild card character in the list of addresses if not already in use
            if (IPAddressString != AlpacaConstants.STRONG_WILDCARD_NAME) addressList.Items.Add(AlpacaConstants.STRONG_WILDCARD_NAME);

            // Set the combo box selected item
            addressList.SelectedIndex = selectedIndex;
            SetupErrorProvider.Clear();
        }

        private void BtnSetupUrlMain_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{cmbServiceType.Text}://{addressList.Text}:{numPort.Value}/setup";
                TL.LogMessageCrLf("MainSetupURL", $"{setupUrl}");

                System.Diagnostics.Process.Start(setupUrl);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("MainSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSetupUrlDevice_Click(object sender, EventArgs e)
        {
            try
            {
                string setupUrl = $"{cmbServiceType.Text}://{addressList.Text}:{numPort.Value}/setup/v1/{DeviceType.ToLowerInvariant()}/{numRemoteDeviceNumber.Value}/setup";
                TL.LogMessageCrLf("DeviceSetupURL", $"{setupUrl}");

                System.Diagnostics.Process.Start(setupUrl);
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ASCOMSetup Exception", ex.ToString());
                MessageBox.Show($"An error occurred when contacting the Alpaca device: {ex.Message}", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
