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
            this.label4 = new System.Windows.Forms.Label();
            this.Relative_NUD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.RelativeForward_BTN = new System.Windows.Forms.Button();
            this.RelativeReverse_BTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Degree_LBL = new System.Windows.Forms.Label();
            this.AbsoluteMove_TB = new System.Windows.Forms.TextBox();
            this.AbsoluteMove_BTN = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagnosticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Relative_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(176, 469);
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
            this.cmdCancel.Location = new System.Drawing.Point(70, 469);
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
            this.textBox1.Location = new System.Drawing.Point(12, 103);
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
            this.textBox2.Location = new System.Drawing.Point(13, 228);
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
            this.CurrentPA_LBL.Location = new System.Drawing.Point(110, 226);
            this.CurrentPA_LBL.Name = "CurrentPA_LBL";
            this.CurrentPA_LBL.Size = new System.Drawing.Size(43, 15);
            this.CurrentPA_LBL.TabIndex = 9;
            this.CurrentPA_LBL.Text = "Angle°";
            // 
            // SetSkyPA_Btn
            // 
            this.SetSkyPA_Btn.Location = new System.Drawing.Point(168, 216);
            this.SetSkyPA_Btn.Name = "SetSkyPA_Btn";
            this.SetSkyPA_Btn.Size = new System.Drawing.Size(65, 37);
            this.SetSkyPA_Btn.TabIndex = 10;
            this.SetSkyPA_Btn.Text = "Set Sky PA...";
            this.SetSkyPA_Btn.UseVisualStyleBackColor = true;
            this.SetSkyPA_Btn.Click += new System.EventHandler(this.SetSkyPA_Btn_Click);
            // 
            // HomeBTN
            // 
            this.HomeBTN.Location = new System.Drawing.Point(101, 19);
            this.HomeBTN.Name = "HomeBTN";
            this.HomeBTN.Size = new System.Drawing.Size(47, 23);
            this.HomeBTN.TabIndex = 4;
            this.HomeBTN.Text = "Home";
            this.HomeBTN.UseVisualStyleBackColor = true;
            this.HomeBTN.Click += new System.EventHandler(this.HomeBTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Relative_NUD);
            this.groupBox1.Controls.Add(this.HomeBTN);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.RelativeForward_BTN);
            this.groupBox1.Controls.Add(this.RelativeReverse_BTN);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Degree_LBL);
            this.groupBox1.Controls.Add(this.AbsoluteMove_TB);
            this.groupBox1.Controls.Add(this.AbsoluteMove_BTN);
            this.groupBox1.Location = new System.Drawing.Point(13, 272);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 183);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motion Controls";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Home Rotator:";
            // 
            // Relative_NUD
            // 
            this.Relative_NUD.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Relative_NUD.DecimalPlaces = 2;
            this.Relative_NUD.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Relative_NUD.Location = new System.Drawing.Point(74, 153);
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
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Increment(°)";
            // 
            // RelativeForward_BTN
            // 
            this.RelativeForward_BTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.RelativeForward_BTN.Location = new System.Drawing.Point(174, 152);
            this.RelativeForward_BTN.Name = "RelativeForward_BTN";
            this.RelativeForward_BTN.Size = new System.Drawing.Size(30, 23);
            this.RelativeForward_BTN.TabIndex = 24;
            this.RelativeForward_BTN.Text = ">>";
            this.RelativeForward_BTN.UseVisualStyleBackColor = true;
            this.RelativeForward_BTN.Click += new System.EventHandler(this.RelativeForward_BTN_Click);
            // 
            // RelativeReverse_BTN
            // 
            this.RelativeReverse_BTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.RelativeReverse_BTN.Location = new System.Drawing.Point(16, 152);
            this.RelativeReverse_BTN.Name = "RelativeReverse_BTN";
            this.RelativeReverse_BTN.Size = new System.Drawing.Size(30, 23);
            this.RelativeReverse_BTN.TabIndex = 19;
            this.RelativeReverse_BTN.Text = "<<";
            this.RelativeReverse_BTN.UseVisualStyleBackColor = true;
            this.RelativeReverse_BTN.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Relative Move:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Move to Absolute Sky PA:";
            // 
            // Degree_LBL
            // 
            this.Degree_LBL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Degree_LBL.AutoSize = true;
            this.Degree_LBL.BackColor = System.Drawing.Color.White;
            this.Degree_LBL.Location = new System.Drawing.Point(100, 81);
            this.Degree_LBL.Name = "Degree_LBL";
            this.Degree_LBL.Size = new System.Drawing.Size(11, 13);
            this.Degree_LBL.TabIndex = 20;
            this.Degree_LBL.Text = "°";
            // 
            // AbsoluteMove_TB
            // 
            this.AbsoluteMove_TB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AbsoluteMove_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AbsoluteMove_TB.Location = new System.Drawing.Point(43, 79);
            this.AbsoluteMove_TB.Name = "AbsoluteMove_TB";
            this.AbsoluteMove_TB.Size = new System.Drawing.Size(73, 20);
            this.AbsoluteMove_TB.TabIndex = 18;
            this.AbsoluteMove_TB.Text = "0";
            this.AbsoluteMove_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AbsoluteMove_TB.Validating += new System.ComponentModel.CancelEventHandler(this.AbsoluteMove_TB_Validating);
            // 
            // AbsoluteMove_BTN
            // 
            this.AbsoluteMove_BTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AbsoluteMove_BTN.Location = new System.Drawing.Point(140, 77);
            this.AbsoluteMove_BTN.Name = "AbsoluteMove_BTN";
            this.AbsoluteMove_BTN.Size = new System.Drawing.Size(37, 23);
            this.AbsoluteMove_BTN.TabIndex = 19;
            this.AbsoluteMove_BTN.Text = "Go";
            this.AbsoluteMove_BTN.UseVisualStyleBackColor = true;
            this.AbsoluteMove_BTN.Click += new System.EventHandler(this.AbsoluteMove_BTN_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.PyxisLE_ASCOM.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(187, 41);
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
            this.pictureBox1.Location = new System.Drawing.Point(13, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
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
            this.diagnosticsToolStripMenuItem});
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
            // diagnosticsToolStripMenuItem
            // 
            this.diagnosticsToolStripMenuItem.Name = "diagnosticsToolStripMenuItem";
            this.diagnosticsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.diagnosticsToolStripMenuItem.Text = "Diagnostics";
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
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 502);
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
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Relative_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label Degree_LBL;
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
        private System.Windows.Forms.ToolStripMenuItem diagnosticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
    }
}