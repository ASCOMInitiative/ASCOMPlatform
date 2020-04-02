namespace ASCOM.DynamicRemoteClients
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.addressList = new System.Windows.Forms.ComboBox();
            this.numRemoteDeviceNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbServiceType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numStandardTimeout = new System.Windows.Forms.NumericUpDown();
            this.numLongTimeout = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numEstablishCommunicationsTimeout = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.chkDebugTrace = new System.Windows.Forms.CheckBox();
            this.groupBoxConnectDisconnect = new System.Windows.Forms.GroupBox();
            this.radManageConnectRemotely = new System.Windows.Forms.RadioButton();
            this.radManageConnectLocally = new System.Windows.Forms.RadioButton();
            this.SetupErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.CmbImageArrayTransferType = new System.Windows.Forms.ComboBox();
            this.LabImageArrayConfiguration1 = new System.Windows.Forms.Label();
            this.cmbImageArrayCompression = new System.Windows.Forms.ComboBox();
            this.LabImageArrayConfiguration2 = new System.Windows.Forms.Label();
            this.ChkEnableRediscovery = new System.Windows.Forms.CheckBox();
            this.GrpIpVersionSelector = new System.Windows.Forms.GroupBox();
            this.RadIpV4AndV6 = new System.Windows.Forms.RadioButton();
            this.RadIpV6 = new System.Windows.Forms.RadioButton();
            this.RadIpV4 = new System.Windows.Forms.RadioButton();
            this.NumDiscoveryPort = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.BtnSetupUrlMain = new System.Windows.Forms.Button();
            this.BtnSetupUrlDevice = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteDeviceNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStandardTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLongTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEstablishCommunicationsTimeout)).BeginInit();
            this.groupBoxConnectDisconnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetupErrorProvider)).BeginInit();
            this.GrpIpVersionSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDiscoveryPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(610, 35);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(87, 20);
            this.numPort.TabIndex = 2;
            this.numPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // addressList
            // 
            this.addressList.FormattingEnabled = true;
            this.addressList.Location = new System.Drawing.Point(126, 34);
            this.addressList.Name = "addressList";
            this.addressList.Size = new System.Drawing.Size(463, 21);
            this.addressList.TabIndex = 1;
            // 
            // numRemoteDeviceNumber
            // 
            this.numRemoteDeviceNumber.Location = new System.Drawing.Point(721, 35);
            this.numRemoteDeviceNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numRemoteDeviceNumber.Name = "numRemoteDeviceNumber";
            this.numRemoteDeviceNumber.Size = new System.Drawing.Size(87, 20);
            this.numRemoteDeviceNumber.TabIndex = 3;
            this.numRemoteDeviceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(708, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Remote device number";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(745, 414);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(745, 443);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Remote Device Host Name or IP Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(621, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Alpaca Port";
            // 
            // cmbServiceType
            // 
            this.cmbServiceType.FormattingEnabled = true;
            this.cmbServiceType.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cmbServiceType.Location = new System.Drawing.Point(22, 34);
            this.cmbServiceType.MaxDropDownItems = 2;
            this.cmbServiceType.Name = "cmbServiceType";
            this.cmbServiceType.Size = new System.Drawing.Size(79, 21);
            this.cmbServiceType.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "HTTP Service Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(104, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "://";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(595, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = ":";
            // 
            // numStandardTimeout
            // 
            this.numStandardTimeout.Location = new System.Drawing.Point(53, 312);
            this.numStandardTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numStandardTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStandardTimeout.Name = "numStandardTimeout";
            this.numStandardTimeout.Size = new System.Drawing.Size(87, 20);
            this.numStandardTimeout.TabIndex = 9;
            this.numStandardTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStandardTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLongTimeout
            // 
            this.numLongTimeout.Location = new System.Drawing.Point(53, 338);
            this.numLongTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLongTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLongTimeout.Name = "numLongTimeout";
            this.numLongTimeout.Size = new System.Drawing.Size(87, 20);
            this.numLongTimeout.TabIndex = 10;
            this.numLongTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLongTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 314);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(322, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Standard response timeout (Properties and asynchronous methods)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(146, 340);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(302, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Long response timeout (for long running synchronous methods)";
            // 
            // numEstablishCommunicationsTimeout
            // 
            this.numEstablishCommunicationsTimeout.Location = new System.Drawing.Point(53, 286);
            this.numEstablishCommunicationsTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEstablishCommunicationsTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEstablishCommunicationsTimeout.Name = "numEstablishCommunicationsTimeout";
            this.numEstablishCommunicationsTimeout.Size = new System.Drawing.Size(87, 20);
            this.numEstablishCommunicationsTimeout.TabIndex = 8;
            this.numEstablishCommunicationsTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEstablishCommunicationsTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(146, 288);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(222, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Establish communications with device timeout";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(188, 168);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(210, 20);
            this.txtUserName.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(188, 194);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(210, 20);
            this.txtPassword.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(408, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(184, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Web server authentication user name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(408, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(180, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Web server authentication password";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(219, 152);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(286, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Authentication is only required if you are using a web server";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Communication Time-outs";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(54, 170);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(90, 17);
            this.chkTrace.TabIndex = 4;
            this.chkTrace.Text = "Enable Trace";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // chkDebugTrace
            // 
            this.chkDebugTrace.AutoSize = true;
            this.chkDebugTrace.Location = new System.Drawing.Point(54, 196);
            this.chkDebugTrace.Name = "chkDebugTrace";
            this.chkDebugTrace.Size = new System.Drawing.Size(125, 17);
            this.chkDebugTrace.TabIndex = 6;
            this.chkDebugTrace.Text = "Enable Debug Trace";
            this.chkDebugTrace.UseVisualStyleBackColor = true;
            // 
            // groupBoxConnectDisconnect
            // 
            this.groupBoxConnectDisconnect.Controls.Add(this.radManageConnectRemotely);
            this.groupBoxConnectDisconnect.Controls.Add(this.radManageConnectLocally);
            this.groupBoxConnectDisconnect.Location = new System.Drawing.Point(53, 407);
            this.groupBoxConnectDisconnect.Name = "groupBoxConnectDisconnect";
            this.groupBoxConnectDisconnect.Size = new System.Drawing.Size(415, 66);
            this.groupBoxConnectDisconnect.TabIndex = 11;
            this.groupBoxConnectDisconnect.TabStop = false;
            this.groupBoxConnectDisconnect.Text = "Connect / Disconnect Options";
            // 
            // radManageConnectRemotely
            // 
            this.radManageConnectRemotely.AutoSize = true;
            this.radManageConnectRemotely.Location = new System.Drawing.Point(6, 18);
            this.radManageConnectRemotely.Name = "radManageConnectRemotely";
            this.radManageConnectRemotely.Size = new System.Drawing.Size(379, 17);
            this.radManageConnectRemotely.TabIndex = 0;
            this.radManageConnectRemotely.TabStop = true;
            this.radManageConnectRemotely.Text = "Manage connect / disconnect remotely - send commands to remote device";
            this.radManageConnectRemotely.UseVisualStyleBackColor = true;
            // 
            // radManageConnectLocally
            // 
            this.radManageConnectLocally.AutoSize = true;
            this.radManageConnectLocally.Location = new System.Drawing.Point(6, 41);
            this.radManageConnectLocally.Name = "radManageConnectLocally";
            this.radManageConnectLocally.Size = new System.Drawing.Size(395, 17);
            this.radManageConnectLocally.TabIndex = 1;
            this.radManageConnectLocally.TabStop = true;
            this.radManageConnectLocally.Text = "Manage connect / disconnect locally - don\'t send commands to remote device";
            this.radManageConnectLocally.UseVisualStyleBackColor = true;
            // 
            // SetupErrorProvider
            // 
            this.SetupErrorProvider.ContainerControl = this;
            // 
            // CmbImageArrayTransferType
            // 
            this.CmbImageArrayTransferType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbImageArrayTransferType.FormattingEnabled = true;
            this.CmbImageArrayTransferType.Location = new System.Drawing.Point(545, 288);
            this.CmbImageArrayTransferType.Name = "CmbImageArrayTransferType";
            this.CmbImageArrayTransferType.Size = new System.Drawing.Size(159, 21);
            this.CmbImageArrayTransferType.TabIndex = 30;
            // 
            // LabImageArrayConfiguration1
            // 
            this.LabImageArrayConfiguration1.AutoSize = true;
            this.LabImageArrayConfiguration1.Location = new System.Drawing.Point(542, 270);
            this.LabImageArrayConfiguration1.Name = "LabImageArrayConfiguration1";
            this.LabImageArrayConfiguration1.Size = new System.Drawing.Size(138, 13);
            this.LabImageArrayConfiguration1.TabIndex = 31;
            this.LabImageArrayConfiguration1.Text = "Image array transfer method";
            // 
            // cmbImageArrayCompression
            // 
            this.cmbImageArrayCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageArrayCompression.FormattingEnabled = true;
            this.cmbImageArrayCompression.Location = new System.Drawing.Point(545, 340);
            this.cmbImageArrayCompression.Name = "cmbImageArrayCompression";
            this.cmbImageArrayCompression.Size = new System.Drawing.Size(159, 21);
            this.cmbImageArrayCompression.TabIndex = 32;
            // 
            // LabImageArrayConfiguration2
            // 
            this.LabImageArrayConfiguration2.AutoSize = true;
            this.LabImageArrayConfiguration2.Location = new System.Drawing.Point(542, 324);
            this.LabImageArrayConfiguration2.Name = "LabImageArrayConfiguration2";
            this.LabImageArrayConfiguration2.Size = new System.Drawing.Size(162, 13);
            this.LabImageArrayConfiguration2.TabIndex = 33;
            this.LabImageArrayConfiguration2.Text = "Image array transfer compression";
            // 
            // ChkEnableRediscovery
            // 
            this.ChkEnableRediscovery.AutoSize = true;
            this.ChkEnableRediscovery.Location = new System.Drawing.Point(126, 89);
            this.ChkEnableRediscovery.Name = "ChkEnableRediscovery";
            this.ChkEnableRediscovery.Size = new System.Drawing.Size(324, 17);
            this.ChkEnableRediscovery.TabIndex = 34;
            this.ChkEnableRediscovery.Text = "Enable rediscovery if device cannot be found at this IP address";
            this.ChkEnableRediscovery.UseVisualStyleBackColor = true;
            // 
            // GrpIpVersionSelector
            // 
            this.GrpIpVersionSelector.Controls.Add(this.RadIpV4AndV6);
            this.GrpIpVersionSelector.Controls.Add(this.RadIpV6);
            this.GrpIpVersionSelector.Controls.Add(this.RadIpV4);
            this.GrpIpVersionSelector.Location = new System.Drawing.Point(642, 127);
            this.GrpIpVersionSelector.Name = "GrpIpVersionSelector";
            this.GrpIpVersionSelector.Size = new System.Drawing.Size(133, 103);
            this.GrpIpVersionSelector.TabIndex = 52;
            this.GrpIpVersionSelector.TabStop = false;
            this.GrpIpVersionSelector.Text = "Supported IP Version(s)";
            // 
            // RadIpV4AndV6
            // 
            this.RadIpV4AndV6.AutoSize = true;
            this.RadIpV4AndV6.Location = new System.Drawing.Point(6, 75);
            this.RadIpV4AndV6.Name = "RadIpV4AndV6";
            this.RadIpV4AndV6.Size = new System.Drawing.Size(88, 17);
            this.RadIpV4AndV6.TabIndex = 2;
            this.RadIpV4AndV6.TabStop = true;
            this.RadIpV4AndV6.Text = "IP V4 and V6";
            this.RadIpV4AndV6.UseVisualStyleBackColor = true;
            this.RadIpV4AndV6.CheckedChanged += new System.EventHandler(this.RadIpV4AndV6_CheckedChanged);
            // 
            // RadIpV6
            // 
            this.RadIpV6.AutoSize = true;
            this.RadIpV6.Location = new System.Drawing.Point(6, 49);
            this.RadIpV6.Name = "RadIpV6";
            this.RadIpV6.Size = new System.Drawing.Size(75, 17);
            this.RadIpV6.TabIndex = 1;
            this.RadIpV6.TabStop = true;
            this.RadIpV6.Text = "IP V6 Only";
            this.RadIpV6.UseVisualStyleBackColor = true;
            this.RadIpV6.CheckedChanged += new System.EventHandler(this.RadIpV6_CheckedChanged);
            // 
            // RadIpV4
            // 
            this.RadIpV4.AutoSize = true;
            this.RadIpV4.Location = new System.Drawing.Point(6, 23);
            this.RadIpV4.Name = "RadIpV4";
            this.RadIpV4.Size = new System.Drawing.Size(75, 17);
            this.RadIpV4.TabIndex = 0;
            this.RadIpV4.TabStop = true;
            this.RadIpV4.Text = "IP V4 Only";
            this.RadIpV4.UseVisualStyleBackColor = true;
            this.RadIpV4.CheckedChanged += new System.EventHandler(this.RadIpV4_CheckedChanged);
            // 
            // NumDiscoveryPort
            // 
            this.NumDiscoveryPort.Location = new System.Drawing.Point(126, 63);
            this.NumDiscoveryPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumDiscoveryPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumDiscoveryPort.Name = "NumDiscoveryPort";
            this.NumDiscoveryPort.Size = new System.Drawing.Size(87, 20);
            this.NumDiscoveryPort.TabIndex = 53;
            this.NumDiscoveryPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(219, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 54;
            this.label14.Text = "Discovery Port";
            // 
            // BtnSetupUrlMain
            // 
            this.BtnSetupUrlMain.Location = new System.Drawing.Point(6, 19);
            this.BtnSetupUrlMain.Name = "BtnSetupUrlMain";
            this.BtnSetupUrlMain.Size = new System.Drawing.Size(75, 52);
            this.BtnSetupUrlMain.TabIndex = 0;
            this.BtnSetupUrlMain.Text = "Whole Alpaca Device";
            this.BtnSetupUrlMain.UseVisualStyleBackColor = true;
            this.BtnSetupUrlMain.Click += new System.EventHandler(this.BtnSetupUrlMain_Click);
            // 
            // BtnSetupUrlDevice
            // 
            this.BtnSetupUrlDevice.Location = new System.Drawing.Point(101, 19);
            this.BtnSetupUrlDevice.Name = "BtnSetupUrlDevice";
            this.BtnSetupUrlDevice.Size = new System.Drawing.Size(75, 52);
            this.BtnSetupUrlDevice.TabIndex = 1;
            this.BtnSetupUrlDevice.Text = "Specific ASCOM Device";
            this.BtnSetupUrlDevice.UseVisualStyleBackColor = true;
            this.BtnSetupUrlDevice.Click += new System.EventHandler(this.BtnSetupUrlDevice_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnSetupUrlDevice);
            this.groupBox1.Controls.Add(this.BtnSetupUrlMain);
            this.groupBox1.Location = new System.Drawing.Point(520, 395);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 78);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alpaca Web Configuration";
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(832, 483);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.NumDiscoveryPort);
            this.Controls.Add(this.GrpIpVersionSelector);
            this.Controls.Add(this.ChkEnableRediscovery);
            this.Controls.Add(this.LabImageArrayConfiguration2);
            this.Controls.Add(this.cmbImageArrayCompression);
            this.Controls.Add(this.LabImageArrayConfiguration1);
            this.Controls.Add(this.CmbImageArrayTransferType);
            this.Controls.Add(this.groupBoxConnectDisconnect);
            this.Controls.Add(this.chkDebugTrace);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numEstablishCommunicationsTimeout);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numLongTimeout);
            this.Controls.Add(this.numStandardTimeout);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbServiceType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numRemoteDeviceNumber);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.addressList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupDialogForm";
            this.Text = "SetupDialogForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteDeviceNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStandardTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLongTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEstablishCommunicationsTimeout)).EndInit();
            this.groupBoxConnectDisconnect.ResumeLayout(false);
            this.groupBoxConnectDisconnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetupErrorProvider)).EndInit();
            this.GrpIpVersionSelector.ResumeLayout(false);
            this.GrpIpVersionSelector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDiscoveryPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.ComboBox addressList;
        private System.Windows.Forms.NumericUpDown numRemoteDeviceNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbServiceType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numStandardTimeout;
        private System.Windows.Forms.NumericUpDown numLongTimeout;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numEstablishCommunicationsTimeout;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.CheckBox chkDebugTrace;
        private System.Windows.Forms.GroupBox groupBoxConnectDisconnect;
        private System.Windows.Forms.RadioButton radManageConnectRemotely;
        private System.Windows.Forms.RadioButton radManageConnectLocally;
        private System.Windows.Forms.ErrorProvider SetupErrorProvider;
        private System.Windows.Forms.Label LabImageArrayConfiguration1;
        private System.Windows.Forms.ComboBox CmbImageArrayTransferType;
        private System.Windows.Forms.Label LabImageArrayConfiguration2;
        private System.Windows.Forms.ComboBox cmbImageArrayCompression;
        private System.Windows.Forms.CheckBox ChkEnableRediscovery;
        private System.Windows.Forms.GroupBox GrpIpVersionSelector;
        private System.Windows.Forms.RadioButton RadIpV4AndV6;
        private System.Windows.Forms.RadioButton RadIpV6;
        private System.Windows.Forms.RadioButton RadIpV4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown NumDiscoveryPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnSetupUrlDevice;
        private System.Windows.Forms.Button BtnSetupUrlMain;
    }
}