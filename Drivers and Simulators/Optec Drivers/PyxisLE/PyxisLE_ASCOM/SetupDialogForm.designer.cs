namespace ASCOM.PyxisLE_ASCOM
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AttachedDevices_CB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.CurrentPA_LBL = new System.Windows.Forms.Label();
            this.SetSkyPA_Btn = new System.Windows.Forms.Button();
            this.HomeBTN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Park_BTN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Relative_NUD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.RelativeForward_BTN = new System.Windows.Forms.Button();
            this.RelativeReverse_BTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AbsoluteMove_TB = new System.Windows.Forms.TextBox();
            this.AbsoluteMove_BTN = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.VersionCheckerBGWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Relative_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(176, 424);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(74, 423);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 100);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(223, 66);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Below is a list containing the serial numbers of all Pyxis LE Rotators currently " +
                "connected to your PC. Select the device that you wish to use.";
            // 
            // AttachedDevices_CB
            // 
            this.AttachedDevices_CB.DisplayMember = "SerialNumber";
            this.AttachedDevices_CB.FormattingEnabled = true;
            this.AttachedDevices_CB.Location = new System.Drawing.Point(111, 172);
            this.AttachedDevices_CB.Name = "AttachedDevices_CB";
            this.AttachedDevices_CB.Size = new System.Drawing.Size(122, 21);
            this.AttachedDevices_CB.TabIndex = 6;
            this.AttachedDevices_CB.SelectionChangeCommitted += new System.EventHandler(this.AttachedDevices_CB_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Attached Devices:";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(13, 216);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(94, 13);
            this.textBox2.TabIndex = 8;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Current Sky PA:";
            // 
            // CurrentPA_LBL
            // 
            this.CurrentPA_LBL.AutoSize = true;
            this.CurrentPA_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentPA_LBL.Location = new System.Drawing.Point(110, 214);
            this.CurrentPA_LBL.Name = "CurrentPA_LBL";
            this.CurrentPA_LBL.Size = new System.Drawing.Size(43, 15);
            this.CurrentPA_LBL.TabIndex = 9;
            this.CurrentPA_LBL.Text = "Angle°";
            // 
            // SetSkyPA_Btn
            // 
            this.SetSkyPA_Btn.Location = new System.Drawing.Point(168, 204);
            this.SetSkyPA_Btn.Name = "SetSkyPA_Btn";
            this.SetSkyPA_Btn.Size = new System.Drawing.Size(65, 37);
            this.SetSkyPA_Btn.TabIndex = 10;
            this.SetSkyPA_Btn.Text = "Set Sky PA...";
            this.SetSkyPA_Btn.UseVisualStyleBackColor = true;
            this.SetSkyPA_Btn.Click += new System.EventHandler(this.SetSkyPA_Btn_Click);
            // 
            // HomeBTN
            // 
            this.HomeBTN.Location = new System.Drawing.Point(100, 22);
            this.HomeBTN.Name = "HomeBTN";
            this.HomeBTN.Size = new System.Drawing.Size(47, 23);
            this.HomeBTN.TabIndex = 4;
            this.HomeBTN.Text = "Home";
            this.HomeBTN.UseVisualStyleBackColor = true;
            this.HomeBTN.Click += new System.EventHandler(this.HomeBTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Park_BTN);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Relative_NUD);
            this.groupBox1.Controls.Add(this.HomeBTN);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.RelativeForward_BTN);
            this.groupBox1.Controls.Add(this.RelativeReverse_BTN);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.AbsoluteMove_TB);
            this.groupBox1.Controls.Add(this.AbsoluteMove_BTN);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(13, 250);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 168);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motion Controls";
            // 
            // Park_BTN
            // 
            this.Park_BTN.Location = new System.Drawing.Point(157, 22);
            this.Park_BTN.Name = "Park_BTN";
            this.Park_BTN.Size = new System.Drawing.Size(47, 23);
            this.Park_BTN.TabIndex = 28;
            this.Park_BTN.Text = "Park";
            this.Park_BTN.UseVisualStyleBackColor = true;
            this.Park_BTN.Click += new System.EventHandler(this.Park_BTN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Home Rotator:";
            // 
            // Relative_NUD
            // 
            this.Relative_NUD.DecimalPlaces = 2;
            this.Relative_NUD.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Relative_NUD.Location = new System.Drawing.Point(74, 138);
            this.Relative_NUD.Maximum = new decimal(new int[] {
            35999,
            0,
            0,
            131072});
            this.Relative_NUD.Name = "Relative_NUD";
            this.Relative_NUD.Size = new System.Drawing.Size(73, 20);
            this.Relative_NUD.TabIndex = 25;
            this.Relative_NUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Relative_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Increment(°)";
            // 
            // RelativeForward_BTN
            // 
            this.RelativeForward_BTN.Location = new System.Drawing.Point(174, 137);
            this.RelativeForward_BTN.Name = "RelativeForward_BTN";
            this.RelativeForward_BTN.Size = new System.Drawing.Size(30, 23);
            this.RelativeForward_BTN.TabIndex = 24;
            this.RelativeForward_BTN.Text = ">>";
            this.RelativeForward_BTN.UseVisualStyleBackColor = true;
            this.RelativeForward_BTN.Click += new System.EventHandler(this.RelativeForward_BTN_Click);
            // 
            // RelativeReverse_BTN
            // 
            this.RelativeReverse_BTN.Location = new System.Drawing.Point(16, 137);
            this.RelativeReverse_BTN.Name = "RelativeReverse_BTN";
            this.RelativeReverse_BTN.Size = new System.Drawing.Size(30, 23);
            this.RelativeReverse_BTN.TabIndex = 19;
            this.RelativeReverse_BTN.Text = "<<";
            this.RelativeReverse_BTN.UseVisualStyleBackColor = true;
            this.RelativeReverse_BTN.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Relative Move:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Move to Absolute Sky PA:";
            // 
            // AbsoluteMove_TB
            // 
            this.AbsoluteMove_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AbsoluteMove_TB.Location = new System.Drawing.Point(37, 79);
            this.AbsoluteMove_TB.Name = "AbsoluteMove_TB";
            this.AbsoluteMove_TB.Size = new System.Drawing.Size(73, 20);
            this.AbsoluteMove_TB.TabIndex = 18;
            this.AbsoluteMove_TB.Text = "0";
            this.AbsoluteMove_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AbsoluteMove_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AbsoluteMove_TB_KeyPress);
            this.AbsoluteMove_TB.Validating += new System.ComponentModel.CancelEventHandler(this.AbsoluteMove_TB_Validating);
            // 
            // AbsoluteMove_BTN
            // 
            this.AbsoluteMove_BTN.Location = new System.Drawing.Point(129, 77);
            this.AbsoluteMove_BTN.Name = "AbsoluteMove_BTN";
            this.AbsoluteMove_BTN.Size = new System.Drawing.Size(48, 23);
            this.AbsoluteMove_BTN.TabIndex = 19;
            this.AbsoluteMove_BTN.Text = "Go";
            this.AbsoluteMove_BTN.UseVisualStyleBackColor = true;
            this.AbsoluteMove_BTN.Click += new System.EventHandler(this.AbsoluteMove_BTN_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "°";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(247, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advancedSettingsToolStripMenuItem,
            this.parkToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // advancedSettingsToolStripMenuItem
            // 
            this.advancedSettingsToolStripMenuItem.Name = "advancedSettingsToolStripMenuItem";
            this.advancedSettingsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.advancedSettingsToolStripMenuItem.Text = "Advanced Settings";
            this.advancedSettingsToolStripMenuItem.Click += new System.EventHandler(this.advancedSettingsToolStripMenuItem_Click);
            // 
            // parkToolStripMenuItem
            // 
            this.parkToolStripMenuItem.Name = "parkToolStripMenuItem";
            this.parkToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.parkToolStripMenuItem.Text = "Park";
            this.parkToolStripMenuItem.Click += new System.EventHandler(this.parkToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.documentationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.documentationToolStripMenuItem_Click);
            // 
            // StatusCheckTimer
            // 
            this.StatusCheckTimer.Enabled = true;
            this.StatusCheckTimer.Interval = 200;
            this.StatusCheckTimer.Tick += new System.EventHandler(this.StatusCheckTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 455);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(247, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(81, 17);
            this.StatusLabel.Text = "Rotator Status";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.PyxisLE_ASCOM.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(193, 31);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.PyxisLE_ASCOM.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(8, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // VersionCheckerBGWorker
            // 
            this.VersionCheckerBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.VersionCheckerBGWorker_DoWork);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 477);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SetSkyPA_Btn);
            this.Controls.Add(this.CurrentPA_LBL);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AttachedDevices_CB);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PyxisLE_ASCOM Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.Shown += new System.EventHandler(this.SetupDialogForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Relative_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox AttachedDevices_CB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label CurrentPA_LBL;
        private System.Windows.Forms.Button SetSkyPA_Btn;
        private System.Windows.Forms.Button HomeBTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox AbsoluteMove_TB;
        private System.Windows.Forms.Button AbsoluteMove_BTN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RelativeReverse_BTN;
        private System.Windows.Forms.NumericUpDown Relative_NUD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RelativeForward_BTN;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.Timer StatusCheckTimer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Label label6;
        private System.ComponentModel.BackgroundWorker VersionCheckerBGWorker;
        private System.Windows.Forms.Button Park_BTN;
        private System.Windows.Forms.ToolStripMenuItem parkToolStripMenuItem;
    }
}