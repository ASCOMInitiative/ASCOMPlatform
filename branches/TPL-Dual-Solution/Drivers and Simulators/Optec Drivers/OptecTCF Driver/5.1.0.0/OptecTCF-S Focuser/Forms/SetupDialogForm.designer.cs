namespace ASCOM.OptecTCF_S
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnterSleepModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitSleepModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureCoefficientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setStartPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEndPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driverDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureCoefficientsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.FocStatusControls_GB = new System.Windows.Forms.GroupBox();
            this.Center_Btn = new System.Windows.Forms.Button();
            this.Out_BTN = new System.Windows.Forms.Button();
            this.StepSize_NUD = new System.Windows.Forms.NumericUpDown();
            this.PosDRO_TB = new System.Windows.Forms.TextBox();
            this.In_BTN = new System.Windows.Forms.Button();
            this.TempDRO_TB = new System.Windows.Forms.TextBox();
            this.Temp_LBL = new System.Windows.Forms.Label();
            this.Pos_LBL = new System.Windows.Forms.Label();
            this.StepSize_LBL = new System.Windows.Forms.Label();
            this.TempCompMode_GB = new System.Windows.Forms.GroupBox();
            this.Test_Btn = new System.Windows.Forms.Button();
            this.ModeBName_LB = new System.Windows.Forms.Label();
            this.ModeAName_LB = new System.Windows.Forms.Label();
            this.ModeB_RB = new System.Windows.Forms.RadioButton();
            this.ModeA_RB = new System.Windows.Forms.RadioButton();
            this.TempBGWorker = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusBarToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PowerLight = new System.Windows.Forms.PictureBox();
            this.Sleeping_TB = new System.Windows.Forms.TextBox();
            this.ConnectionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.VersionCheckerBGWorker = new System.ComponentModel.BackgroundWorker();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.menuStrip1.SuspendLayout();
            this.FocStatusControls_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).BeginInit();
            this.TempCompMode_GB.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(161, 320);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(232, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.EnterSleepModeToolStripMenuItem,
            this.exitSleepModeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fileToolStripMenuItem.Text = "Device";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // EnterSleepModeToolStripMenuItem
            // 
            this.EnterSleepModeToolStripMenuItem.Name = "EnterSleepModeToolStripMenuItem";
            this.EnterSleepModeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.EnterSleepModeToolStripMenuItem.Text = "Enter Sleep Mode";
            this.EnterSleepModeToolStripMenuItem.Click += new System.EventHandler(this.putToSleepToolStripMenuItem_Click);
            // 
            // exitSleepModeToolStripMenuItem
            // 
            this.exitSleepModeToolStripMenuItem.Name = "exitSleepModeToolStripMenuItem";
            this.exitSleepModeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exitSleepModeToolStripMenuItem.Text = "Exit Sleep Mode";
            this.exitSleepModeToolStripMenuItem.Click += new System.EventHandler(this.exitSleepModeToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceSettingsToolStripMenuItem,
            this.temperatureCoefficientsToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // deviceSettingsToolStripMenuItem
            // 
            this.deviceSettingsToolStripMenuItem.Name = "deviceSettingsToolStripMenuItem";
            this.deviceSettingsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.deviceSettingsToolStripMenuItem.Text = "Device Settings";
            this.deviceSettingsToolStripMenuItem.Click += new System.EventHandler(this.deviceSettingsToolStripMenuItem_Click);
            // 
            // temperatureCoefficientsToolStripMenuItem
            // 
            this.temperatureCoefficientsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setStartPointToolStripMenuItem,
            this.setEndPointToolStripMenuItem});
            this.temperatureCoefficientsToolStripMenuItem.Name = "temperatureCoefficientsToolStripMenuItem";
            this.temperatureCoefficientsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.temperatureCoefficientsToolStripMenuItem.Text = "Temperature Coefficients";
            // 
            // setStartPointToolStripMenuItem
            // 
            this.setStartPointToolStripMenuItem.Name = "setStartPointToolStripMenuItem";
            this.setStartPointToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.setStartPointToolStripMenuItem.Text = "Set Start Point";
            this.setStartPointToolStripMenuItem.Click += new System.EventHandler(this.setStartPointToolStripMenuItem_Click);
            // 
            // setEndPointToolStripMenuItem
            // 
            this.setEndPointToolStripMenuItem.Name = "setEndPointToolStripMenuItem";
            this.setEndPointToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.setEndPointToolStripMenuItem.Text = "Set End Point";
            this.setEndPointToolStripMenuItem.Click += new System.EventHandler(this.setEndPointToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.driverDocumentationToolStripMenuItem,
            this.temperatureCoefficientsToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // driverDocumentationToolStripMenuItem
            // 
            this.driverDocumentationToolStripMenuItem.Name = "driverDocumentationToolStripMenuItem";
            this.driverDocumentationToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.driverDocumentationToolStripMenuItem.Text = "Driver Documentation";
            this.driverDocumentationToolStripMenuItem.Click += new System.EventHandler(this.driverDocumentationToolStripMenuItem_Click);
            // 
            // temperatureCoefficientsToolStripMenuItem1
            // 
            this.temperatureCoefficientsToolStripMenuItem1.Name = "temperatureCoefficientsToolStripMenuItem1";
            this.temperatureCoefficientsToolStripMenuItem1.Size = new System.Drawing.Size(237, 22);
            this.temperatureCoefficientsToolStripMenuItem1.Text = "Calculating Temp. Coefficients";
            this.temperatureCoefficientsToolStripMenuItem1.Click += new System.EventHandler(this.temperatureCoefficientsToolStripMenuItem1_Click);
            // 
            // FocStatusControls_GB
            // 
            this.FocStatusControls_GB.Controls.Add(this.Center_Btn);
            this.FocStatusControls_GB.Controls.Add(this.Out_BTN);
            this.FocStatusControls_GB.Controls.Add(this.StepSize_NUD);
            this.FocStatusControls_GB.Controls.Add(this.PosDRO_TB);
            this.FocStatusControls_GB.Controls.Add(this.In_BTN);
            this.FocStatusControls_GB.Controls.Add(this.TempDRO_TB);
            this.FocStatusControls_GB.Controls.Add(this.Temp_LBL);
            this.FocStatusControls_GB.Controls.Add(this.Pos_LBL);
            this.FocStatusControls_GB.Controls.Add(this.StepSize_LBL);
            this.FocStatusControls_GB.Location = new System.Drawing.Point(7, 69);
            this.FocStatusControls_GB.Name = "FocStatusControls_GB";
            this.FocStatusControls_GB.Size = new System.Drawing.Size(216, 163);
            this.FocStatusControls_GB.TabIndex = 9;
            this.FocStatusControls_GB.TabStop = false;
            this.FocStatusControls_GB.Text = "Focuser Status";
            // 
            // Center_Btn
            // 
            this.Center_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Center_Btn.Location = new System.Drawing.Point(149, 126);
            this.Center_Btn.Name = "Center_Btn";
            this.Center_Btn.Size = new System.Drawing.Size(61, 25);
            this.Center_Btn.TabIndex = 11;
            this.Center_Btn.Text = "Center";
            this.Center_Btn.UseVisualStyleBackColor = false;
            this.Center_Btn.Click += new System.EventHandler(this.Center_Btn_Click);
            // 
            // Out_BTN
            // 
            this.Out_BTN.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Out_BTN.Location = new System.Drawing.Point(120, 77);
            this.Out_BTN.Name = "Out_BTN";
            this.Out_BTN.Size = new System.Drawing.Size(40, 40);
            this.Out_BTN.TabIndex = 9;
            this.Out_BTN.Text = "OUT";
            this.Out_BTN.UseVisualStyleBackColor = false;
            this.Out_BTN.Click += new System.EventHandler(this.Out_BTN_Click);
            // 
            // StepSize_NUD
            // 
            this.StepSize_NUD.Location = new System.Drawing.Point(61, 130);
            this.StepSize_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.StepSize_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StepSize_NUD.Name = "StepSize_NUD";
            this.StepSize_NUD.Size = new System.Drawing.Size(66, 20);
            this.StepSize_NUD.TabIndex = 8;
            this.StepSize_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PosDRO_TB
            // 
            this.PosDRO_TB.BackColor = System.Drawing.SystemColors.MenuText;
            this.PosDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PosDRO_TB.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PosDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.PosDRO_TB.Location = new System.Drawing.Point(121, 20);
            this.PosDRO_TB.Name = "PosDRO_TB";
            this.PosDRO_TB.Size = new System.Drawing.Size(58, 24);
            this.PosDRO_TB.TabIndex = 6;
            this.PosDRO_TB.Text = "------";
            this.PosDRO_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // In_BTN
            // 
            this.In_BTN.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.In_BTN.Location = new System.Drawing.Point(56, 77);
            this.In_BTN.Name = "In_BTN";
            this.In_BTN.Size = new System.Drawing.Size(40, 40);
            this.In_BTN.TabIndex = 4;
            this.In_BTN.Text = "IN";
            this.In_BTN.UseVisualStyleBackColor = false;
            this.In_BTN.Click += new System.EventHandler(this.In_BTN_Click);
            // 
            // TempDRO_TB
            // 
            this.TempDRO_TB.BackColor = System.Drawing.SystemColors.MenuText;
            this.TempDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TempDRO_TB.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TempDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.TempDRO_TB.Location = new System.Drawing.Point(121, 48);
            this.TempDRO_TB.Name = "TempDRO_TB";
            this.TempDRO_TB.Size = new System.Drawing.Size(58, 24);
            this.TempDRO_TB.TabIndex = 3;
            this.TempDRO_TB.Text = "----°C";
            this.TempDRO_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Temp_LBL
            // 
            this.Temp_LBL.AutoSize = true;
            this.Temp_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temp_LBL.Location = new System.Drawing.Point(37, 51);
            this.Temp_LBL.Name = "Temp_LBL";
            this.Temp_LBL.Size = new System.Drawing.Size(82, 13);
            this.Temp_LBL.TabIndex = 1;
            this.Temp_LBL.Text = "Temperature:";
            // 
            // Pos_LBL
            // 
            this.Pos_LBL.AutoSize = true;
            this.Pos_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pos_LBL.Location = new System.Drawing.Point(37, 23);
            this.Pos_LBL.Name = "Pos_LBL";
            this.Pos_LBL.Size = new System.Drawing.Size(56, 13);
            this.Pos_LBL.TabIndex = 0;
            this.Pos_LBL.Text = "Position:";
            // 
            // StepSize_LBL
            // 
            this.StepSize_LBL.AutoSize = true;
            this.StepSize_LBL.Location = new System.Drawing.Point(3, 132);
            this.StepSize_LBL.Name = "StepSize_LBL";
            this.StepSize_LBL.Size = new System.Drawing.Size(55, 13);
            this.StepSize_LBL.TabIndex = 10;
            this.StepSize_LBL.Text = "Step Size:";
            // 
            // TempCompMode_GB
            // 
            this.TempCompMode_GB.Controls.Add(this.Test_Btn);
            this.TempCompMode_GB.Controls.Add(this.ModeBName_LB);
            this.TempCompMode_GB.Controls.Add(this.ModeAName_LB);
            this.TempCompMode_GB.Controls.Add(this.ModeB_RB);
            this.TempCompMode_GB.Controls.Add(this.ModeA_RB);
            this.TempCompMode_GB.Location = new System.Drawing.Point(7, 238);
            this.TempCompMode_GB.Name = "TempCompMode_GB";
            this.TempCompMode_GB.Size = new System.Drawing.Size(216, 68);
            this.TempCompMode_GB.TabIndex = 8;
            this.TempCompMode_GB.TabStop = false;
            this.TempCompMode_GB.Text = "Temperature Compensation";
            // 
            // Test_Btn
            // 
            this.Test_Btn.Location = new System.Drawing.Point(154, 19);
            this.Test_Btn.Name = "Test_Btn";
            this.Test_Btn.Size = new System.Drawing.Size(46, 39);
            this.Test_Btn.TabIndex = 5;
            this.Test_Btn.Text = "Test";
            this.Test_Btn.UseVisualStyleBackColor = true;
            this.Test_Btn.Click += new System.EventHandler(this.Test_Btn_Click);
            // 
            // ModeBName_LB
            // 
            this.ModeBName_LB.AutoSize = true;
            this.ModeBName_LB.Location = new System.Drawing.Point(43, 45);
            this.ModeBName_LB.Name = "ModeBName_LB";
            this.ModeBName_LB.Size = new System.Drawing.Size(104, 13);
            this.ModeBName_LB.TabIndex = 4;
            this.ModeBName_LB.Text = "Temp Comp Mode B";
            // 
            // ModeAName_LB
            // 
            this.ModeAName_LB.AutoSize = true;
            this.ModeAName_LB.Location = new System.Drawing.Point(43, 22);
            this.ModeAName_LB.Name = "ModeAName_LB";
            this.ModeAName_LB.Size = new System.Drawing.Size(104, 13);
            this.ModeAName_LB.TabIndex = 3;
            this.ModeAName_LB.Text = "Temp Comp Mode A";
            // 
            // ModeB_RB
            // 
            this.ModeB_RB.AutoSize = true;
            this.ModeB_RB.Location = new System.Drawing.Point(7, 43);
            this.ModeB_RB.Name = "ModeB_RB";
            this.ModeB_RB.Size = new System.Drawing.Size(32, 17);
            this.ModeB_RB.TabIndex = 1;
            this.ModeB_RB.TabStop = true;
            this.ModeB_RB.Text = "B";
            this.ModeB_RB.UseVisualStyleBackColor = true;
            this.ModeB_RB.Click += new System.EventHandler(this.ModeB_RB_Click);
            // 
            // ModeA_RB
            // 
            this.ModeA_RB.AutoSize = true;
            this.ModeA_RB.Location = new System.Drawing.Point(7, 20);
            this.ModeA_RB.Name = "ModeA_RB";
            this.ModeA_RB.Size = new System.Drawing.Size(32, 17);
            this.ModeA_RB.TabIndex = 0;
            this.ModeA_RB.TabStop = true;
            this.ModeA_RB.Text = "A";
            this.ModeA_RB.UseVisualStyleBackColor = true;
            this.ModeA_RB.Click += new System.EventHandler(this.ModeA_RB_Click);
            // 
            // TempBGWorker
            // 
            this.TempBGWorker.WorkerReportsProgress = true;
            this.TempBGWorker.WorkerSupportsCancellation = true;
            this.TempBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TempBGWorker_DoWork);
            this.TempBGWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TempBGWorker_RunWorkerCompleted);
            this.TempBGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TempBGWorker_ProgressChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 355);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(232, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(74, 17);
            this.StatusLabel.Text = "DeviceStatus";
            this.StatusLabel.MouseHover += new System.EventHandler(this.StatusLabel_MouseHover);
            this.StatusLabel.MouseEnter += new System.EventHandler(this.StatusLabel_MouseEnter);
            this.StatusLabel.MouseLeave += new System.EventHandler(this.StatusLabel_MouseLeave);
            this.StatusLabel.Click += new System.EventHandler(this.StatusLabel_Click);
            // 
            // StatusTimer
            // 
            this.StatusTimer.Enabled = true;
            this.StatusTimer.Interval = 2000;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // PowerLight
            // 
            this.PowerLight.Image = global::ASCOM.OptecTCF_S.Properties.Resources.RedLight;
            this.PowerLight.Location = new System.Drawing.Point(14, 40);
            this.PowerLight.Name = "PowerLight";
            this.PowerLight.Size = new System.Drawing.Size(16, 16);
            this.PowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PowerLight.TabIndex = 12;
            this.PowerLight.TabStop = false;
            // 
            // Sleeping_TB
            // 
            this.Sleeping_TB.Location = new System.Drawing.Point(12, 98);
            this.Sleeping_TB.Multiline = true;
            this.Sleeping_TB.Name = "Sleeping_TB";
            this.Sleeping_TB.ReadOnly = true;
            this.Sleeping_TB.Size = new System.Drawing.Size(209, 199);
            this.Sleeping_TB.TabIndex = 14;
            this.Sleeping_TB.Text = "The TCF-S device is currently in sleep mode.  Click \'Exit Sleep Mode\' under the \'" +
                "Device\' menu to wake the device and return to Serial Mode.";
            this.Sleeping_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sleeping_TB.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::ASCOM.OptecTCF_S.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(192, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::ASCOM.OptecTCF_S.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox2.Location = new System.Drawing.Point(63, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(107, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.BrowseToOptec);
            // 
            // VersionCheckerBGWorker
            // 
            this.VersionCheckerBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.VersionCheckerBGWorker_DoWork);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 377);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.PowerLight);
            this.Controls.Add(this.FocStatusControls_GB);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.TempCompMode_GB);
            this.Controls.Add(this.Sleeping_TB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec TCF-S Focuser Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SetupDialogForm_HelpRequested);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.FocStatusControls_GB.ResumeLayout(false);
            this.FocStatusControls_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).EndInit();
            this.TempCompMode_GB.ResumeLayout(false);
            this.TempCompMode_GB.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //fields used for temperature polling
        private bool GetTemps = false;
        private bool StoppedGettingTemps = false;
  //      private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureCoefficientsToolStripMenuItem;
        private System.Windows.Forms.GroupBox FocStatusControls_GB;
        private System.Windows.Forms.Button Center_Btn;
        private System.Windows.Forms.Button Out_BTN;
        private System.Windows.Forms.NumericUpDown StepSize_NUD;
        private System.Windows.Forms.TextBox PosDRO_TB;
        private System.Windows.Forms.Button In_BTN;
        private System.Windows.Forms.TextBox TempDRO_TB;
        private System.Windows.Forms.Label Temp_LBL;
        private System.Windows.Forms.Label Pos_LBL;
        private System.Windows.Forms.Label StepSize_LBL;
        private System.Windows.Forms.GroupBox TempCompMode_GB;
        private System.Windows.Forms.ToolStripMenuItem deviceSettingsToolStripMenuItem;
        private System.Windows.Forms.RadioButton ModeB_RB;
        private System.Windows.Forms.RadioButton ModeA_RB;
        private System.Windows.Forms.Label ModeBName_LB;
        private System.Windows.Forms.Label ModeAName_LB;
        private System.ComponentModel.BackgroundWorker TempBGWorker;

        private System.Windows.Forms.PictureBox PowerLight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer StatusTimer;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolTip StatusBarToolTip;
        private System.Windows.Forms.ToolStripMenuItem setStartPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setEndPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem driverDocumentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureCoefficientsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem EnterSleepModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitSleepModeToolStripMenuItem;
        private System.Windows.Forms.TextBox Sleeping_TB;
        private System.Windows.Forms.ToolTip ConnectionToolTip;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button Test_Btn;
        private System.ComponentModel.BackgroundWorker VersionCheckerBGWorker;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}