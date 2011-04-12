namespace ASCOM.OptecFocuserHub
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.EthernetRB = new System.Windows.Forms.RadioButton();
            this.SerialRB = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.ComPortNameCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IPAddrTB = new System.Windows.Forms.TextBox();
            this.PortNumTB = new System.Windows.Forms.Label();
            this.TcpipPortNumberTB = new System.Windows.Forms.TextBox();
            this.ConnectionSetupGB = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Home_BTN = new System.Windows.Forms.Button();
            this.TempCompDescLabel = new System.Windows.Forms.Label();
            this.tempCompModeLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.BacklashCompStepsNUD = new System.Windows.Forms.NumericUpDown();
            this.BacklashCompCB = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.FocuserTypeCB = new System.Windows.Forms.ComboBox();
            this.NicknameTB = new System.Windows.Forms.TextBox();
            this.SetupTempCompBtn = new System.Windows.Forms.Button();
            this.ChangeFocuserTypeBTN = new System.Windows.Forms.Button();
            this.ChangeNicknameBTN = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LEDTrackbar = new System.Windows.Forms.TrackBar();
            this.ConnectBTN = new System.Windows.Forms.Button();
            this.DisconnectBTN = new System.Windows.Forms.Button();
            this.ConnectionMonitor = new System.Windows.Forms.Timer(this.components);
            this.OkBTN = new System.Windows.Forms.Button();
            this.PowerLight = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.ConnectionSetupGB.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BacklashCompStepsNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LEDTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(140, 498);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 79);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note: Remember that both Focuser1 and Focuser2 are physically connected to one co" +
                "ntroller (hub). Changing the connection settings for one will affect both.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connection Method:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.EthernetRB);
            this.panel1.Controls.Add(this.SerialRB);
            this.panel1.Location = new System.Drawing.Point(113, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 47);
            this.panel1.TabIndex = 7;
            // 
            // EthernetRB
            // 
            this.EthernetRB.AutoSize = true;
            this.EthernetRB.Location = new System.Drawing.Point(12, 26);
            this.EthernetRB.Name = "EthernetRB";
            this.EthernetRB.Size = new System.Drawing.Size(65, 17);
            this.EthernetRB.TabIndex = 0;
            this.EthernetRB.TabStop = true;
            this.EthernetRB.Text = "Ethernet";
            this.EthernetRB.UseVisualStyleBackColor = true;
            this.EthernetRB.CheckedChanged += new System.EventHandler(this.EthernetRB_CheckedChanged);
            // 
            // SerialRB
            // 
            this.SerialRB.AutoSize = true;
            this.SerialRB.Location = new System.Drawing.Point(12, 4);
            this.SerialRB.Name = "SerialRB";
            this.SerialRB.Size = new System.Drawing.Size(51, 17);
            this.SerialRB.TabIndex = 0;
            this.SerialRB.TabStop = true;
            this.SerialRB.Text = "Serial";
            this.SerialRB.UseVisualStyleBackColor = true;
            this.SerialRB.CheckedChanged += new System.EventHandler(this.SerialRB_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(4, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Com Port:";
            // 
            // ComPortNameCB
            // 
            this.ComPortNameCB.Enabled = false;
            this.ComPortNameCB.FormattingEnabled = true;
            this.ComPortNameCB.Location = new System.Drawing.Point(113, 69);
            this.ComPortNameCB.Name = "ComPortNameCB";
            this.ComPortNameCB.Size = new System.Drawing.Size(129, 21);
            this.ComPortNameCB.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(4, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP Address:";
            // 
            // IPAddrTB
            // 
            this.IPAddrTB.Enabled = false;
            this.IPAddrTB.Location = new System.Drawing.Point(113, 94);
            this.IPAddrTB.Name = "IPAddrTB";
            this.IPAddrTB.Size = new System.Drawing.Size(129, 20);
            this.IPAddrTB.TabIndex = 11;
            // 
            // PortNumTB
            // 
            this.PortNumTB.AutoSize = true;
            this.PortNumTB.Enabled = false;
            this.PortNumTB.Location = new System.Drawing.Point(4, 121);
            this.PortNumTB.Name = "PortNumTB";
            this.PortNumTB.Size = new System.Drawing.Size(68, 13);
            this.PortNumTB.TabIndex = 10;
            this.PortNumTB.Text = "TCP/IP Port:";
            // 
            // TcpipPortNumberTB
            // 
            this.TcpipPortNumberTB.Enabled = false;
            this.TcpipPortNumberTB.Location = new System.Drawing.Point(113, 118);
            this.TcpipPortNumberTB.Name = "TcpipPortNumberTB";
            this.TcpipPortNumberTB.Size = new System.Drawing.Size(129, 20);
            this.TcpipPortNumberTB.TabIndex = 11;
            // 
            // ConnectionSetupGB
            // 
            this.ConnectionSetupGB.Controls.Add(this.label2);
            this.ConnectionSetupGB.Controls.Add(this.TcpipPortNumberTB);
            this.ConnectionSetupGB.Controls.Add(this.panel1);
            this.ConnectionSetupGB.Controls.Add(this.IPAddrTB);
            this.ConnectionSetupGB.Controls.Add(this.label3);
            this.ConnectionSetupGB.Controls.Add(this.PortNumTB);
            this.ConnectionSetupGB.Controls.Add(this.ComPortNameCB);
            this.ConnectionSetupGB.Controls.Add(this.label4);
            this.ConnectionSetupGB.Location = new System.Drawing.Point(15, 74);
            this.ConnectionSetupGB.Name = "ConnectionSetupGB";
            this.ConnectionSetupGB.Size = new System.Drawing.Size(258, 144);
            this.ConnectionSetupGB.TabIndex = 12;
            this.ConnectionSetupGB.TabStop = false;
            this.ConnectionSetupGB.Text = "Connection Setup";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Home_BTN);
            this.groupBox2.Controls.Add(this.TempCompDescLabel);
            this.groupBox2.Controls.Add(this.tempCompModeLabel);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.BacklashCompStepsNUD);
            this.groupBox2.Controls.Add(this.BacklashCompCB);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.FocuserTypeCB);
            this.groupBox2.Controls.Add(this.NicknameTB);
            this.groupBox2.Controls.Add(this.SetupTempCompBtn);
            this.groupBox2.Controls.Add(this.ChangeFocuserTypeBTN);
            this.groupBox2.Controls.Add(this.ChangeNicknameBTN);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.LEDTrackbar);
            this.groupBox2.Location = new System.Drawing.Point(15, 255);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 237);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Focuser Settings:";
            // 
            // Home_BTN
            // 
            this.Home_BTN.Location = new System.Drawing.Point(9, 206);
            this.Home_BTN.Name = "Home_BTN";
            this.Home_BTN.Size = new System.Drawing.Size(87, 23);
            this.Home_BTN.TabIndex = 11;
            this.Home_BTN.Text = "Home Device";
            this.Home_BTN.UseVisualStyleBackColor = true;
            this.Home_BTN.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // TempCompDescLabel
            // 
            this.TempCompDescLabel.AutoSize = true;
            this.TempCompDescLabel.Location = new System.Drawing.Point(82, 89);
            this.TempCompDescLabel.Name = "TempCompDescLabel";
            this.TempCompDescLabel.Size = new System.Drawing.Size(100, 13);
            this.TempCompDescLabel.TabIndex = 10;
            this.TempCompDescLabel.Text = "Mode A Description";
            // 
            // tempCompModeLabel
            // 
            this.tempCompModeLabel.AutoSize = true;
            this.tempCompModeLabel.Location = new System.Drawing.Point(82, 74);
            this.tempCompModeLabel.Name = "tempCompModeLabel";
            this.tempCompModeLabel.Size = new System.Drawing.Size(14, 13);
            this.tempCompModeLabel.TabIndex = 9;
            this.tempCompModeLabel.Text = "A";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(4, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 33);
            this.label9.TabIndex = 8;
            this.label9.Text = "Temp. Comp. Mode:";
            // 
            // BacklashCompStepsNUD
            // 
            this.BacklashCompStepsNUD.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BacklashCompStepsNUD.Location = new System.Drawing.Point(129, 145);
            this.BacklashCompStepsNUD.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.BacklashCompStepsNUD.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.BacklashCompStepsNUD.Name = "BacklashCompStepsNUD";
            this.BacklashCompStepsNUD.Size = new System.Drawing.Size(81, 20);
            this.BacklashCompStepsNUD.TabIndex = 7;
            this.BacklashCompStepsNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BacklashCompStepsNUD.ValueChanged += new System.EventHandler(this.BacklashCompStepsNUD_ValueChanged);
            // 
            // BacklashCompCB
            // 
            this.BacklashCompCB.AutoSize = true;
            this.BacklashCompCB.Location = new System.Drawing.Point(7, 118);
            this.BacklashCompCB.Name = "BacklashCompCB";
            this.BacklashCompCB.Size = new System.Drawing.Size(182, 17);
            this.BacklashCompCB.TabIndex = 6;
            this.BacklashCompCB.Text = "Backlash Compensation Enabled";
            this.BacklashCompCB.UseVisualStyleBackColor = true;
            this.BacklashCompCB.CheckedChanged += new System.EventHandler(this.BacklashCompCB_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Backlash Comp. Steps:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "LED Brightness:";
            // 
            // FocuserTypeCB
            // 
            this.FocuserTypeCB.Enabled = false;
            this.FocuserTypeCB.FormattingEnabled = true;
            this.FocuserTypeCB.Location = new System.Drawing.Point(100, 44);
            this.FocuserTypeCB.Name = "FocuserTypeCB";
            this.FocuserTypeCB.Size = new System.Drawing.Size(110, 21);
            this.FocuserTypeCB.TabIndex = 3;
            this.FocuserTypeCB.SelectedIndexChanged += new System.EventHandler(this.FocuserTypeCB_SelectedIndexChanged);
            // 
            // NicknameTB
            // 
            this.NicknameTB.Location = new System.Drawing.Point(98, 17);
            this.NicknameTB.Name = "NicknameTB";
            this.NicknameTB.ReadOnly = true;
            this.NicknameTB.Size = new System.Drawing.Size(112, 20);
            this.NicknameTB.TabIndex = 2;
            this.NicknameTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NicknameTB_KeyPress);
            // 
            // SetupTempCompBtn
            // 
            this.SetupTempCompBtn.Image = global::ASCOM.OptecFocuserHub.Properties.Resources.gear_16;
            this.SetupTempCompBtn.Location = new System.Drawing.Point(216, 68);
            this.SetupTempCompBtn.Name = "SetupTempCompBtn";
            this.SetupTempCompBtn.Size = new System.Drawing.Size(36, 25);
            this.SetupTempCompBtn.TabIndex = 1;
            this.SetupTempCompBtn.UseVisualStyleBackColor = true;
            this.SetupTempCompBtn.Click += new System.EventHandler(this.SetupTempCompBtn_Click);
            // 
            // ChangeFocuserTypeBTN
            // 
            this.ChangeFocuserTypeBTN.Image = global::ASCOM.OptecFocuserHub.Properties.Resources.gear_16;
            this.ChangeFocuserTypeBTN.Location = new System.Drawing.Point(216, 41);
            this.ChangeFocuserTypeBTN.Name = "ChangeFocuserTypeBTN";
            this.ChangeFocuserTypeBTN.Size = new System.Drawing.Size(36, 25);
            this.ChangeFocuserTypeBTN.TabIndex = 1;
            this.ChangeFocuserTypeBTN.UseVisualStyleBackColor = true;
            this.ChangeFocuserTypeBTN.Click += new System.EventHandler(this.DeviceTypeChangeBtn_Click);
            // 
            // ChangeNicknameBTN
            // 
            this.ChangeNicknameBTN.Image = global::ASCOM.OptecFocuserHub.Properties.Resources.gear_16;
            this.ChangeNicknameBTN.Location = new System.Drawing.Point(216, 14);
            this.ChangeNicknameBTN.Name = "ChangeNicknameBTN";
            this.ChangeNicknameBTN.Size = new System.Drawing.Size(36, 25);
            this.ChangeNicknameBTN.TabIndex = 1;
            this.ChangeNicknameBTN.UseVisualStyleBackColor = true;
            this.ChangeNicknameBTN.Click += new System.EventHandler(this.ChangeNicknameBTN_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Focuser Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Nickname:";
            // 
            // LEDTrackbar
            // 
            this.LEDTrackbar.Location = new System.Drawing.Point(100, 171);
            this.LEDTrackbar.Maximum = 100;
            this.LEDTrackbar.Name = "LEDTrackbar";
            this.LEDTrackbar.Size = new System.Drawing.Size(130, 45);
            this.LEDTrackbar.TabIndex = 4;
            this.LEDTrackbar.TickFrequency = 10;
            this.LEDTrackbar.Scroll += new System.EventHandler(this.LEDTrackbar_Scroll);
            // 
            // ConnectBTN
            // 
            this.ConnectBTN.Location = new System.Drawing.Point(59, 225);
            this.ConnectBTN.Name = "ConnectBTN";
            this.ConnectBTN.Size = new System.Drawing.Size(75, 23);
            this.ConnectBTN.TabIndex = 12;
            this.ConnectBTN.Text = "Connect";
            this.ConnectBTN.UseVisualStyleBackColor = true;
            this.ConnectBTN.Click += new System.EventHandler(this.ConnectBTN_Click);
            // 
            // DisconnectBTN
            // 
            this.DisconnectBTN.Location = new System.Drawing.Point(148, 225);
            this.DisconnectBTN.Name = "DisconnectBTN";
            this.DisconnectBTN.Size = new System.Drawing.Size(75, 23);
            this.DisconnectBTN.TabIndex = 12;
            this.DisconnectBTN.Text = "Disconnect";
            this.DisconnectBTN.UseVisualStyleBackColor = true;
            this.DisconnectBTN.Click += new System.EventHandler(this.DisconnectBTN_Click);
            // 
            // ConnectionMonitor
            // 
            this.ConnectionMonitor.Enabled = true;
            this.ConnectionMonitor.Interval = 250;
            this.ConnectionMonitor.Tick += new System.EventHandler(this.ConnectionMonitor_Tick);
            // 
            // OkBTN
            // 
            this.OkBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBTN.Location = new System.Drawing.Point(214, 498);
            this.OkBTN.Name = "OkBTN";
            this.OkBTN.Size = new System.Drawing.Size(59, 25);
            this.OkBTN.TabIndex = 1;
            this.OkBTN.Text = "Ok";
            this.OkBTN.UseVisualStyleBackColor = true;
            this.OkBTN.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // PowerLight
            // 
            this.PowerLight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PowerLight.Image = global::ASCOM.OptecFocuserHub.Properties.Resources.GreyLight;
            this.PowerLight.Location = new System.Drawing.Point(15, 226);
            this.PowerLight.MaximumSize = new System.Drawing.Size(22, 22);
            this.PowerLight.Name = "PowerLight";
            this.PowerLight.Size = new System.Drawing.Size(22, 22);
            this.PowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PowerLight.TabIndex = 22;
            this.PowerLight.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.InitialImage = global::ASCOM.OptecFocuserHub.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(225, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 529);
            this.Controls.Add(this.DisconnectBTN);
            this.Controls.Add(this.PowerLight);
            this.Controls.Add(this.ConnectBTN);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ConnectionSetupGB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.OkBTN);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Focuser Hub Setup - Focuser 1";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ConnectionSetupGB.ResumeLayout(false);
            this.ConnectionSetupGB.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BacklashCompStepsNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LEDTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton EthernetRB;
        private System.Windows.Forms.RadioButton SerialRB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComPortNameCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox IPAddrTB;
        private System.Windows.Forms.Label PortNumTB;
        private System.Windows.Forms.TextBox TcpipPortNumberTB;
        private System.Windows.Forms.GroupBox ConnectionSetupGB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DisconnectBTN;
        private System.Windows.Forms.Button ConnectBTN;
        private System.Windows.Forms.Timer ConnectionMonitor;
        private System.Windows.Forms.PictureBox PowerLight;
        private System.Windows.Forms.Button OkBTN;
        private System.Windows.Forms.Button ChangeNicknameBTN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox FocuserTypeCB;
        private System.Windows.Forms.TextBox NicknameTB;
        private System.Windows.Forms.Button ChangeFocuserTypeBTN;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar LEDTrackbar;
        private System.Windows.Forms.CheckBox BacklashCompCB;
        private System.Windows.Forms.NumericUpDown BacklashCompStepsNUD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label tempCompModeLabel;
        private System.Windows.Forms.Button SetupTempCompBtn;
        private System.Windows.Forms.Label TempCompDescLabel;
        private System.Windows.Forms.Button Home_BTN;
    }
}