namespace ASCOM.OptecTCF_Driver
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.learnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuallyEnterSlopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseDeviceTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TempCompMode_GB = new System.Windows.Forms.GroupBox();
            this.EditNames_Btn = new System.Windows.Forms.Button();
            this.ModeBName_TB = new System.Windows.Forms.TextBox();
            this.ModeAName_TB = new System.Windows.Forms.TextBox();
            this.ModeB_RB = new System.Windows.Forms.RadioButton();
            this.ModeA_RB = new System.Windows.Forms.RadioButton();
            this.FocStatusControls = new System.Windows.Forms.GroupBox();
            this.Center_Btn = new System.Windows.Forms.Button();
            this.Out_BTN = new System.Windows.Forms.Button();
            this.Increment_NUD = new System.Windows.Forms.NumericUpDown();
            this.PowerLight = new System.Windows.Forms.PictureBox();
            this.Pos_TB = new System.Windows.Forms.TextBox();
            this.In_BTN = new System.Windows.Forms.Button();
            this.Temp_TB = new System.Windows.Forms.TextBox();
            this.Temp_LBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.backgroundWorkerTemp = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerPos = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.Timer_Temp = new System.Windows.Forms.Timer(this.components);
            this.DeviceType_CB = new System.Windows.Forms.ComboBox();
            this.DeviceType_LB = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProviderDT = new System.Windows.Forms.ErrorProvider(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.TempCompMode_GB.SuspendLayout();
            this.FocStatusControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDT)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(169, 414);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.toolTip1.SetToolTip(this.cmdOK, "You must select a COM Port and Device Type before continuing.");
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(92, 414);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(238, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseCOMPortToolStripMenuItem,
            this.learnToolStripMenuItem,
            this.chooseDeviceTypeToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // chooseCOMPortToolStripMenuItem
            // 
            this.chooseCOMPortToolStripMenuItem.Name = "chooseCOMPortToolStripMenuItem";
            this.chooseCOMPortToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.chooseCOMPortToolStripMenuItem.Text = "Choose COM Port";
            this.chooseCOMPortToolStripMenuItem.Click += new System.EventHandler(this.chooseCOMPortToolStripMenuItem_Click);
            // 
            // learnToolStripMenuItem
            // 
            this.learnToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.wizardToolStripMenuItem,
            this.firstPointToolStripMenuItem,
            this.endPointToolStripMenuItem,
            this.manuallyEnterSlopeToolStripMenuItem});
            this.learnToolStripMenuItem.Name = "learnToolStripMenuItem";
            this.learnToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.learnToolStripMenuItem.Text = "Temperature Coefficients...";
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.displayToolStripMenuItem.Text = "Display Current Coefficients";
            this.displayToolStripMenuItem.Click += new System.EventHandler(this.displayToolStripMenuItem_Click_1);
            // 
            // wizardToolStripMenuItem
            // 
            this.wizardToolStripMenuItem.Name = "wizardToolStripMenuItem";
            this.wizardToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.wizardToolStripMenuItem.Text = "Learn Wizard...";
            this.wizardToolStripMenuItem.Visible = false;
            this.wizardToolStripMenuItem.Click += new System.EventHandler(this.wizardToolStripMenuItem_Click);
            // 
            // firstPointToolStripMenuItem
            // 
            this.firstPointToolStripMenuItem.Name = "firstPointToolStripMenuItem";
            this.firstPointToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.firstPointToolStripMenuItem.Text = "Set Start Point";
            this.firstPointToolStripMenuItem.Click += new System.EventHandler(this.firstPointToolStripMenuItem_Click);
            // 
            // endPointToolStripMenuItem
            // 
            this.endPointToolStripMenuItem.Name = "endPointToolStripMenuItem";
            this.endPointToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.endPointToolStripMenuItem.Text = "Set End Point";
            this.endPointToolStripMenuItem.Click += new System.EventHandler(this.endPointToolStripMenuItem_Click);
            // 
            // manuallyEnterSlopeToolStripMenuItem
            // 
            this.manuallyEnterSlopeToolStripMenuItem.Name = "manuallyEnterSlopeToolStripMenuItem";
            this.manuallyEnterSlopeToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.manuallyEnterSlopeToolStripMenuItem.Text = "Manually Enter Coefficients";
            this.manuallyEnterSlopeToolStripMenuItem.Click += new System.EventHandler(this.manuallyEnterSlopeToolStripMenuItem_Click);
            // 
            // chooseDeviceTypeToolStripMenuItem
            // 
            this.chooseDeviceTypeToolStripMenuItem.Name = "chooseDeviceTypeToolStripMenuItem";
            this.chooseDeviceTypeToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.chooseDeviceTypeToolStripMenuItem.Text = "Choose Device Type";
            this.chooseDeviceTypeToolStripMenuItem.Click += new System.EventHandler(this.chooseDeviceTypeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 449);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(238, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(48, 17);
            this.StatusLabel.Text = "Status...";
            // 
            // TempCompMode_GB
            // 
            this.TempCompMode_GB.Controls.Add(this.EditNames_Btn);
            this.TempCompMode_GB.Controls.Add(this.ModeBName_TB);
            this.TempCompMode_GB.Controls.Add(this.ModeAName_TB);
            this.TempCompMode_GB.Controls.Add(this.ModeB_RB);
            this.TempCompMode_GB.Controls.Add(this.ModeA_RB);
            this.TempCompMode_GB.Location = new System.Drawing.Point(12, 320);
            this.TempCompMode_GB.Name = "TempCompMode_GB";
            this.TempCompMode_GB.Size = new System.Drawing.Size(216, 87);
            this.TempCompMode_GB.TabIndex = 6;
            this.TempCompMode_GB.TabStop = false;
            this.TempCompMode_GB.Text = "Temperature Compensation Modes";
            // 
            // EditNames_Btn
            // 
            this.EditNames_Btn.Location = new System.Drawing.Point(159, 28);
            this.EditNames_Btn.Name = "EditNames_Btn";
            this.EditNames_Btn.Size = new System.Drawing.Size(51, 43);
            this.EditNames_Btn.TabIndex = 4;
            this.EditNames_Btn.Text = "Edit Names";
            this.EditNames_Btn.UseVisualStyleBackColor = true;
            this.EditNames_Btn.Click += new System.EventHandler(this.EditNames_Btn_Click);
            // 
            // ModeBName_TB
            // 
            this.ModeBName_TB.Location = new System.Drawing.Point(46, 53);
            this.ModeBName_TB.Name = "ModeBName_TB";
            this.ModeBName_TB.ReadOnly = true;
            this.ModeBName_TB.Size = new System.Drawing.Size(104, 20);
            this.ModeBName_TB.TabIndex = 3;
            this.ModeBName_TB.Text = "Temp Comp Mode B";
            // 
            // ModeAName_TB
            // 
            this.ModeAName_TB.Location = new System.Drawing.Point(46, 27);
            this.ModeAName_TB.Name = "ModeAName_TB";
            this.ModeAName_TB.ReadOnly = true;
            this.ModeAName_TB.Size = new System.Drawing.Size(104, 20);
            this.ModeAName_TB.TabIndex = 2;
            this.ModeAName_TB.Text = "Temp Comp Mode A";
            // 
            // ModeB_RB
            // 
            this.ModeB_RB.AutoSize = true;
            this.ModeB_RB.Location = new System.Drawing.Point(11, 54);
            this.ModeB_RB.Name = "ModeB_RB";
            this.ModeB_RB.Size = new System.Drawing.Size(32, 17);
            this.ModeB_RB.TabIndex = 1;
            this.ModeB_RB.TabStop = true;
            this.ModeB_RB.Text = "B";
            this.ModeB_RB.UseVisualStyleBackColor = true;
            this.ModeB_RB.CheckedChanged += new System.EventHandler(this.ModeRBChecked_Changed);
            // 
            // ModeA_RB
            // 
            this.ModeA_RB.AutoSize = true;
            this.ModeA_RB.Location = new System.Drawing.Point(11, 28);
            this.ModeA_RB.Name = "ModeA_RB";
            this.ModeA_RB.Size = new System.Drawing.Size(32, 17);
            this.ModeA_RB.TabIndex = 0;
            this.ModeA_RB.TabStop = true;
            this.ModeA_RB.Text = "A";
            this.ModeA_RB.UseVisualStyleBackColor = true;
            this.ModeA_RB.CheckedChanged += new System.EventHandler(this.ModeRBChecked_Changed);
            // 
            // FocStatusControls
            // 
            this.FocStatusControls.Controls.Add(this.Center_Btn);
            this.FocStatusControls.Controls.Add(this.Out_BTN);
            this.FocStatusControls.Controls.Add(this.Increment_NUD);
            this.FocStatusControls.Controls.Add(this.PowerLight);
            this.FocStatusControls.Controls.Add(this.Pos_TB);
            this.FocStatusControls.Controls.Add(this.In_BTN);
            this.FocStatusControls.Controls.Add(this.Temp_TB);
            this.FocStatusControls.Controls.Add(this.Temp_LBL);
            this.FocStatusControls.Controls.Add(this.label1);
            this.FocStatusControls.Controls.Add(this.label3);
            this.FocStatusControls.Location = new System.Drawing.Point(12, 134);
            this.FocStatusControls.Name = "FocStatusControls";
            this.FocStatusControls.Size = new System.Drawing.Size(216, 178);
            this.FocStatusControls.TabIndex = 7;
            this.FocStatusControls.TabStop = false;
            this.FocStatusControls.Text = "Focuser Status";
            // 
            // Center_Btn
            // 
            this.Center_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Center_Btn.Location = new System.Drawing.Point(149, 136);
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
            this.Out_BTN.Location = new System.Drawing.Point(120, 87);
            this.Out_BTN.Name = "Out_BTN";
            this.Out_BTN.Size = new System.Drawing.Size(40, 40);
            this.Out_BTN.TabIndex = 9;
            this.Out_BTN.Text = "OUT";
            this.Out_BTN.UseVisualStyleBackColor = false;
            this.Out_BTN.Click += new System.EventHandler(this.Out_BTN_Click);
            // 
            // Increment_NUD
            // 
            this.Increment_NUD.Location = new System.Drawing.Point(61, 140);
            this.Increment_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Increment_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Increment_NUD.Name = "Increment_NUD";
            this.Increment_NUD.Size = new System.Drawing.Size(66, 20);
            this.Increment_NUD.TabIndex = 8;
            this.Increment_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PowerLight
            // 
            this.PowerLight.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.RedLight;
            this.PowerLight.Location = new System.Drawing.Point(11, 19);
            this.PowerLight.Name = "PowerLight";
            this.PowerLight.Size = new System.Drawing.Size(17, 16);
            this.PowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PowerLight.TabIndex = 7;
            this.PowerLight.TabStop = false;
            // 
            // Pos_TB
            // 
            this.Pos_TB.BackColor = System.Drawing.SystemColors.MenuText;
            this.Pos_TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pos_TB.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pos_TB.ForeColor = System.Drawing.Color.Red;
            this.Pos_TB.Location = new System.Drawing.Point(121, 30);
            this.Pos_TB.Name = "Pos_TB";
            this.Pos_TB.Size = new System.Drawing.Size(58, 24);
            this.Pos_TB.TabIndex = 6;
            this.Pos_TB.Text = "------";
            // 
            // In_BTN
            // 
            this.In_BTN.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.In_BTN.Location = new System.Drawing.Point(56, 87);
            this.In_BTN.Name = "In_BTN";
            this.In_BTN.Size = new System.Drawing.Size(40, 40);
            this.In_BTN.TabIndex = 4;
            this.In_BTN.Text = "IN";
            this.In_BTN.UseVisualStyleBackColor = false;
            this.In_BTN.Click += new System.EventHandler(this.In_BTN_Click);
            // 
            // Temp_TB
            // 
            this.Temp_TB.BackColor = System.Drawing.SystemColors.MenuText;
            this.Temp_TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Temp_TB.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temp_TB.ForeColor = System.Drawing.Color.Red;
            this.Temp_TB.Location = new System.Drawing.Point(121, 58);
            this.Temp_TB.Name = "Temp_TB";
            this.Temp_TB.Size = new System.Drawing.Size(58, 24);
            this.Temp_TB.TabIndex = 3;
            this.Temp_TB.Text = "----°C";
            // 
            // Temp_LBL
            // 
            this.Temp_LBL.AutoSize = true;
            this.Temp_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temp_LBL.Location = new System.Drawing.Point(37, 61);
            this.Temp_LBL.Name = "Temp_LBL";
            this.Temp_LBL.Size = new System.Drawing.Size(82, 13);
            this.Temp_LBL.TabIndex = 1;
            this.Temp_LBL.Text = "Temperature:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Step Size:";
            // 
            // backgroundWorkerTemp
            // 
            this.backgroundWorkerTemp.WorkerSupportsCancellation = true;
            this.backgroundWorkerTemp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerTemp_DoWork);
            // 
            // backgroundWorkerPos
            // 
            this.backgroundWorkerPos.WorkerSupportsCancellation = true;
            this.backgroundWorkerPos.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerPos_DoWork);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.Optec_Logo_large_png;
            this.pictureBox1.Location = new System.Drawing.Point(23, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(185, 28);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // Timer_Temp
            // 
            this.Timer_Temp.Interval = 3000;
            this.Timer_Temp.Tick += new System.EventHandler(this.Timer_Temp_Tick);
            // 
            // DeviceType_CB
            // 
            this.DeviceType_CB.Enabled = false;
            this.DeviceType_CB.FormattingEnabled = true;
            this.DeviceType_CB.Items.AddRange(new object[] {
            "Cancel",
            "TCF-S",
            "TCF-Si",
            "TCF-S3",
            "TCF-S3i"});
            this.DeviceType_CB.Location = new System.Drawing.Point(103, 100);
            this.DeviceType_CB.Name = "DeviceType_CB";
            this.DeviceType_CB.Size = new System.Drawing.Size(111, 21);
            this.DeviceType_CB.TabIndex = 8;
            this.DeviceType_CB.Text = "Not Connected";
            this.DeviceType_CB.Validating += new System.ComponentModel.CancelEventHandler(this.DeviceType_CB_Validating);
            this.DeviceType_CB.SelectedIndexChanged += new System.EventHandler(this.DeviceType_CB_SelectedIndexChanged);
            this.DeviceType_CB.Validated += new System.EventHandler(this.DeviceType_CB_Validated);
            // 
            // DeviceType_LB
            // 
            this.DeviceType_LB.AutoSize = true;
            this.DeviceType_LB.Enabled = false;
            this.DeviceType_LB.Location = new System.Drawing.Point(16, 103);
            this.DeviceType_LB.Name = "DeviceType_LB";
            this.DeviceType_LB.Size = new System.Drawing.Size(71, 13);
            this.DeviceType_LB.TabIndex = 9;
            this.DeviceType_LB.Text = "Device Type:";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 50;
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ShowAlways = true;
            // 
            // errorProviderDT
            // 
            this.errorProviderDT.ContainerControl = this;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(168, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "BETA 3";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 471);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeviceType_LB);
            this.Controls.Add(this.DeviceType_CB);
            this.Controls.Add(this.FocStatusControls);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TempCompMode_GB);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SetupDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec TCF Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.TempCompMode_GB.ResumeLayout(false);
            this.TempCompMode_GB.PerformLayout();
            this.FocStatusControls.ResumeLayout(false);
            this.FocStatusControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseCOMPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.GroupBox TempCompMode_GB;
        private System.Windows.Forms.RadioButton ModeB_RB;
        private System.Windows.Forms.RadioButton ModeA_RB;
        private System.Windows.Forms.ToolStripMenuItem learnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuallyEnterSlopeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.TextBox ModeBName_TB;
        private System.Windows.Forms.TextBox ModeAName_TB;
        private System.Windows.Forms.Button EditNames_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox FocStatusControls;
        private System.Windows.Forms.Label Temp_LBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Temp_TB;
        private System.Windows.Forms.Button In_BTN;
        private System.Windows.Forms.PictureBox PowerLight;
        private System.Windows.Forms.TextBox Pos_TB;
        private System.Windows.Forms.Button Out_BTN;
        private System.Windows.Forms.NumericUpDown Increment_NUD;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorkerTemp;
        private System.ComponentModel.BackgroundWorker backgroundWorkerPos;
        private System.Windows.Forms.Timer Timer_Temp;
        private System.Windows.Forms.ComboBox DeviceType_CB;
        private System.Windows.Forms.Label DeviceType_LB;
        private System.Windows.Forms.ToolStripMenuItem chooseDeviceTypeToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider errorProviderDT;
        private System.Windows.Forms.Button Center_Btn;
        private System.Windows.Forms.Label label4;
    }
}