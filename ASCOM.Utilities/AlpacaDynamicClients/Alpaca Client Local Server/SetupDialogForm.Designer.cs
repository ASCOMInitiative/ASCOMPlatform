namespace ASCOM.Remote
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
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteDeviceNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStandardTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLongTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEstablishCommunicationsTimeout)).BeginInit();
            this.groupBoxConnectDisconnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetupErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(476, 35);
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
            this.addressList.Size = new System.Drawing.Size(328, 21);
            this.addressList.TabIndex = 1;
            // 
            // numRemoteDeviceNumber
            // 
            this.numRemoteDeviceNumber.Location = new System.Drawing.Point(587, 35);
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
            this.label1.Location = new System.Drawing.Point(574, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Remote device number";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(605, 336);
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
            this.btnCancel.Location = new System.Drawing.Point(605, 365);
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
            this.label2.Size = new System.Drawing.Size(212, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Remote Device Server Name or IP Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(498, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "IP Port";
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
            this.label5.Location = new System.Drawing.Point(104, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "://";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(461, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = ":";
            // 
            // numStandardTimeout
            // 
            this.numStandardTimeout.Location = new System.Drawing.Point(21, 237);
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
            this.numLongTimeout.Location = new System.Drawing.Point(21, 263);
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
            this.label8.Location = new System.Drawing.Point(114, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(322, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Standard response timeout (Properties and asynchronous methods)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(114, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(302, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Long response timeout (for long running synchronous methods)";
            // 
            // numEstablishCommunicationsTimeout
            // 
            this.numEstablishCommunicationsTimeout.Location = new System.Drawing.Point(21, 211);
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
            this.label10.Location = new System.Drawing.Point(114, 213);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(219, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Establish communications with server timeout";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(156, 115);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(210, 20);
            this.txtUserName.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(156, 141);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(210, 20);
            this.txtPassword.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(376, 118);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(184, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Web server authentication user name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(376, 144);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(180, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Web server authentication password";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(187, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(284, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Authentication is ony required if you are using a web server";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Communication Timeouts";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(22, 117);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(90, 17);
            this.chkTrace.TabIndex = 4;
            this.chkTrace.Text = "Enable Trace";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // chkDebugTrace
            // 
            this.chkDebugTrace.AutoSize = true;
            this.chkDebugTrace.Location = new System.Drawing.Point(22, 143);
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
            this.groupBoxConnectDisconnect.Location = new System.Drawing.Point(21, 324);
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
            this.radManageConnectRemotely.Size = new System.Drawing.Size(376, 17);
            this.radManageConnectRemotely.TabIndex = 0;
            this.radManageConnectRemotely.TabStop = true;
            this.radManageConnectRemotely.Text = "Manage connect / disconnect remotely - send commands to remote server";
            this.radManageConnectRemotely.UseVisualStyleBackColor = true;
            // 
            // radManageConnectLocally
            // 
            this.radManageConnectLocally.AutoSize = true;
            this.radManageConnectLocally.Location = new System.Drawing.Point(6, 41);
            this.radManageConnectLocally.Name = "radManageConnectLocally";
            this.radManageConnectLocally.Size = new System.Drawing.Size(392, 17);
            this.radManageConnectLocally.TabIndex = 1;
            this.radManageConnectLocally.TabStop = true;
            this.radManageConnectLocally.Text = "Manage connect / disconnect locally - don\'t send commands to remote server";
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
            this.CmbImageArrayTransferType.Location = new System.Drawing.Point(513, 213);
            this.CmbImageArrayTransferType.Name = "CmbImageArrayTransferType";
            this.CmbImageArrayTransferType.Size = new System.Drawing.Size(159, 21);
            this.CmbImageArrayTransferType.TabIndex = 30;
            // 
            // LabImageArrayConfiguration1
            // 
            this.LabImageArrayConfiguration1.AutoSize = true;
            this.LabImageArrayConfiguration1.Location = new System.Drawing.Point(510, 195);
            this.LabImageArrayConfiguration1.Name = "LabImageArrayConfiguration1";
            this.LabImageArrayConfiguration1.Size = new System.Drawing.Size(138, 13);
            this.LabImageArrayConfiguration1.TabIndex = 31;
            this.LabImageArrayConfiguration1.Text = "Image array transfer method";
            // 
            // cmbImageArrayCompression
            // 
            this.cmbImageArrayCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageArrayCompression.FormattingEnabled = true;
            this.cmbImageArrayCompression.Location = new System.Drawing.Point(513, 265);
            this.cmbImageArrayCompression.Name = "cmbImageArrayCompression";
            this.cmbImageArrayCompression.Size = new System.Drawing.Size(159, 21);
            this.cmbImageArrayCompression.TabIndex = 32;
            // 
            // LabImageArrayConfiguration2
            // 
            this.LabImageArrayConfiguration2.AutoSize = true;
            this.LabImageArrayConfiguration2.Location = new System.Drawing.Point(510, 249);
            this.LabImageArrayConfiguration2.Name = "LabImageArrayConfiguration2";
            this.LabImageArrayConfiguration2.Size = new System.Drawing.Size(162, 13);
            this.LabImageArrayConfiguration2.TabIndex = 33;
            this.LabImageArrayConfiguration2.Text = "Image array transfer compression";
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(697, 402);
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
    }
}