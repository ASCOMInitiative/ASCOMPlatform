using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASCOM.Remote
{
    public partial class SetupDialogForm : Form
    {

        #region Variables

        private TraceLoggerPlus TL;

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
        public SharedConstants.ImageArrayTransferType ImageArrayTransferType { get; set; }
        public SharedConstants.ImageArrayCompression ImageArrayCompression { get; set; }
        public string DeviceType { get; set; }

        private bool selectByMouse = false; // Variable to help select the whole contents of a numeric up-down box when tabbed into our selected by mouse

        // Create validating regular expression
        Regex validHostnameRegex = new Regex(SharedConstants.ValidHostnameRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex validIpAddressRegex = new Regex(SharedConstants.ValidIpAddressRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

        private void CmbImageArrayTransferType_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((SharedConstants.ImageArrayTransferType)CmbImageArrayTransferType.SelectedItem == SharedConstants.ImageArrayTransferType.Base64HandOff)
            {
                cmbImageArrayCompression.Enabled = false;
            }
            else
            {
                cmbImageArrayCompression.Enabled = true;
            }
        }

        public SetupDialogForm(TraceLoggerPlus TraceLogger) : this()
        {
            TL = TraceLogger;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            TL.LogMessage("SetupForm Load", "Start");

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            this.Text = $"{DriverDisplayName} Configuration - Version {version} - {DeviceType}";
            addressList.Items.Add(SharedConstants.LOCALHOST_NAME);

            cmbServiceType.Text = ServiceType;

            int selectedIndex = 0;

            if (IPAddressString != SharedConstants.LOCALHOST_NAME)
            {
                addressList.Items.Add(IPAddressString);
                selectedIndex = 1;
            }

            IPHostEntry host;
            IPAddress localIP = null;
            host = Dns.GetHostEntry(Dns.GetHostName());
            bool found = false;
            foreach (IPAddress ip in host.AddressList)
            {
                if ((ip.AddressFamily == AddressFamily.InterNetwork) & !found)
                {
                    localIP = ip;
                    TL.LogMessage("GetIPAddress", "Found IP Address: " + ip.ToString());
                    found = true;
                    if (ip.ToString() != IPAddressString) // Only add addresses that are not the currently selected IP address
                    {
                        addressList.Items.Add(ip.ToString());
                    }
                }
                else
                {
                    TL.LogMessage("GetIPAddress", "Ignored IP Address: " + ip.ToString());
                }
            }
            if (localIP == null) throw new Exception("Cannot find IP address of this device");

            TL.LogMessage("GetIPAddress", localIP.ToString());
            addressList.SelectedIndex = selectedIndex;
            numPort.Value = PortNumber;
            numRemoteDeviceNumber.Value = RemoteDeviceNumber;
            numEstablishCommunicationsTimeout.Value = Convert.ToDecimal(EstablishConnectionTimeout);
            numStandardTimeout.Value = Convert.ToDecimal(StandardTimeout);
            numLongTimeout.Value = Convert.ToDecimal(LongTimeout);
            txtUserName.Text = UserName.Unencrypt(TL);
            txtPassword.Text = Password.Unencrypt(TL);
            chkTrace.Checked = TraceState;
            chkDebugTrace.Checked = DebugTraceState;
            if (ManageConnectLocally)
            {
                radManageConnectLocally.Checked = true;
            }
            else
            {
                radManageConnectRemotely.Checked = true;
            }

            CmbImageArrayTransferType.Items.Add(SharedConstants.ImageArrayTransferType.JSON);
            CmbImageArrayTransferType.Items.Add(SharedConstants.ImageArrayTransferType.Base64HandOff);
            CmbImageArrayTransferType.SelectedItem = ImageArrayTransferType;

            cmbImageArrayCompression.Items.Add(SharedConstants.ImageArrayCompression.None);
            cmbImageArrayCompression.Items.Add(SharedConstants.ImageArrayCompression.Deflate);
            cmbImageArrayCompression.Items.Add(SharedConstants.ImageArrayCompression.GZip);
            cmbImageArrayCompression.Items.Add(SharedConstants.ImageArrayCompression.GZipOrDeflate);
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
            if (CmbImageArrayTransferType.SelectedItem == null) CmbImageArrayTransferType.SelectedItem = SharedConstants.IMAGE_ARRAY_TRANSFER_TYPE_DEFAULT;
            if (cmbImageArrayCompression.SelectedItem == null) cmbImageArrayCompression.SelectedItem = SharedConstants.IMAGE_ARRAY_COMPRESSION_DEFAULT;

            this.BringToFront();
        }

        #endregion

        #region Event handlers

        private void BtnOK_Click(object sender, EventArgs e)
        {
            TraceState = chkTrace.Checked;
            DebugTraceState = chkDebugTrace.Checked;
            IPAddressString = addressList.Text;
            PortNumber = numPort.Value;
            RemoteDeviceNumber = numRemoteDeviceNumber.Value;
            ServiceType = cmbServiceType.Text;
            EstablishConnectionTimeout = Convert.ToInt32(numEstablishCommunicationsTimeout.Value);
            StandardTimeout = Convert.ToInt32(numStandardTimeout.Value);
            LongTimeout = Convert.ToInt32(numLongTimeout.Value);
            UserName = txtUserName.Text.Encrypt(TL); // Encrypt the provided username and password
            Password = txtPassword.Text.Encrypt(TL);
            ManageConnectLocally = radManageConnectLocally.Checked;
            ImageArrayTransferType = (SharedConstants.ImageArrayTransferType)CmbImageArrayTransferType.SelectedItem;
            ImageArrayCompression = (SharedConstants.ImageArrayCompression)cmbImageArrayCompression.SelectedItem;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Event handler to paint the device list combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
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

        private void BtnServerConfiguration_Click(object sender, EventArgs e)
        {
            ServerConfigurationForm configurationForm = new ServerConfigurationForm(TL, cmbServiceType.Text, addressList.Text, numPort.Value, txtUserName.Text, txtPassword.Text);
            configurationForm.ShowDialog();
            configurationForm.Dispose();
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
            bool isValid = false;

            if (IsIpAddress(addressList.Text)) // The host name is an IP address so test whether this is valid
            {
                MatchCollection matches = validIpAddressRegex.Matches(addressList.Text);
                if (matches.Count == 0)
                {
                    SetupErrorProvider.SetError(addressList, "IP addresses can only contain digits and the point character in the form WWW.XXX.YYY.ZZZ.");
                }
                else
                {
                    isValid = true;
                }
            }
            else // The host name is a string rather than an IP address so validate this
            {
                MatchCollection matches = validHostnameRegex.Matches(addressList.Text);
                if (matches.Count == 0)
                {
                    SetupErrorProvider.SetError(addressList, "Not a valid host name.");
                }
                else
                {
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

        #endregion

        #region Support Code
        public bool IsIpAddress(string s)
        {
            foreach (char c in s)
            {
                if ((!Char.IsDigit(c)) && (c != '.')) return false; // Make sure that the strong only contains digits and the point character
            }
            return true;
        }

        #endregion

    }
}
