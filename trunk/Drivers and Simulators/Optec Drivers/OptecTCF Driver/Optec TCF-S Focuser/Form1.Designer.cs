namespace Optec_TCF_S_Focuser
{
    partial class Form1
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
            this.Temp_LBL = new System.Windows.Forms.Label();
            this.Pos_LBL = new System.Windows.Forms.Label();
            this.OUT_Btn = new System.Windows.Forms.Button();
            this.Center_Btn = new System.Windows.Forms.Button();
            this.StepSize_NUD = new System.Windows.Forms.NumericUpDown();
            this.In_BTN = new System.Windows.Forms.Button();
            this.StepSize_LBL = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterExitSleepModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterExitTempCompModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitOptecTCFSFocuserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFocusOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFocusOffsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAbsolutePresetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAbsolutePresetsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.positionAndTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relativeFocusAdjustToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureCompensationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbsoluteMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReletiveFocusOffsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.absoluteFocusPresetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbsPosTB = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AbsolutePresetsMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAbsolutePresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAbsolutePresetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeightToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.absolutePanelHeightTB = new System.Windows.Forms.ToolStripTextBox();
            this.FilterOffsetContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelHeightTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.TempDRO_TB = new System.Windows.Forms.Label();
            this.TempProbecontextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableProbeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableProbeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PosDRO_TB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PowerLight = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusBarToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.TempModeON_RB = new System.Windows.Forms.RadioButton();
            this.TempModeOFF_RB = new System.Windows.Forms.RadioButton();
            this.AbsolutePresetPanel = new System.Windows.Forms.Panel();
            this.FocusOffsetPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TempCompPanel = new System.Windows.Forms.Panel();
            this.NewVersionBGWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AbsoluteMovePanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.AbsolutePresetsMenuStrip1.SuspendLayout();
            this.FilterOffsetContextMenuStrip1.SuspendLayout();
            this.TempProbecontextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.TempCompPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.AbsoluteMovePanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Temp_LBL
            // 
            this.Temp_LBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Temp_LBL.AutoSize = true;
            this.Temp_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temp_LBL.Location = new System.Drawing.Point(75, 104);
            this.Temp_LBL.Name = "Temp_LBL";
            this.Temp_LBL.Size = new System.Drawing.Size(86, 16);
            this.Temp_LBL.TabIndex = 8;
            this.Temp_LBL.Text = "Temperature";
            // 
            // Pos_LBL
            // 
            this.Pos_LBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Pos_LBL.AutoSize = true;
            this.Pos_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pos_LBL.Location = new System.Drawing.Point(75, 42);
            this.Pos_LBL.Name = "Pos_LBL";
            this.Pos_LBL.Size = new System.Drawing.Size(56, 16);
            this.Pos_LBL.TabIndex = 7;
            this.Pos_LBL.Text = "Position";
            // 
            // OUT_Btn
            // 
            this.OUT_Btn.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.OUT_Btn.Enabled = false;
            this.OUT_Btn.Location = new System.Drawing.Point(166, 5);
            this.OUT_Btn.Name = "OUT_Btn";
            this.OUT_Btn.Size = new System.Drawing.Size(45, 45);
            this.OUT_Btn.TabIndex = 17;
            this.OUT_Btn.Text = "OUT";
            this.StatusBarToolTip.SetToolTip(this.OUT_Btn, "Move focus OUT");
            this.OUT_Btn.UseVisualStyleBackColor = false;
            this.OUT_Btn.Click += new System.EventHandler(this.OUT_Btn_Click);
            // 
            // Center_Btn
            // 
            this.Center_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Center_Btn.Location = new System.Drawing.Point(166, 7);
            this.Center_Btn.Name = "Center_Btn";
            this.Center_Btn.Size = new System.Drawing.Size(51, 23);
            this.Center_Btn.TabIndex = 16;
            this.Center_Btn.Text = "Center";
            this.StatusBarToolTip.SetToolTip(this.Center_Btn, "Go to center of focusers travel");
            this.Center_Btn.UseVisualStyleBackColor = false;
            this.Center_Btn.Click += new System.EventHandler(this.Center_Btn_Click);
            // 
            // StepSize_NUD
            // 
            this.StepSize_NUD.Enabled = false;
            this.StepSize_NUD.Location = new System.Drawing.Point(85, 27);
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
            this.StepSize_NUD.TabIndex = 14;
            this.StatusBarToolTip.SetToolTip(this.StepSize_NUD, "Set relative move increment");
            this.StepSize_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // In_BTN
            // 
            this.In_BTN.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.In_BTN.Enabled = false;
            this.In_BTN.Location = new System.Drawing.Point(26, 5);
            this.In_BTN.Name = "In_BTN";
            this.In_BTN.Size = new System.Drawing.Size(45, 45);
            this.In_BTN.TabIndex = 13;
            this.In_BTN.Text = "IN";
            this.StatusBarToolTip.SetToolTip(this.In_BTN, "Move focus IN");
            this.In_BTN.UseVisualStyleBackColor = false;
            this.In_BTN.Click += new System.EventHandler(this.In_BTN_Click);
            // 
            // StepSize_LBL
            // 
            this.StepSize_LBL.AutoSize = true;
            this.StepSize_LBL.Enabled = false;
            this.StepSize_LBL.Location = new System.Drawing.Point(90, 11);
            this.StepSize_LBL.Name = "StepSize_LBL";
            this.StepSize_LBL.Size = new System.Drawing.Size(57, 13);
            this.StepSize_LBL.TabIndex = 15;
            this.StepSize_LBL.Text = "Increment:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 565);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(262, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(77, 17);
            this.StatusLabel.Text = "Device Status";
            this.StatusLabel.Click += new System.EventHandler(this.StatusLabel_Click);
            this.StatusLabel.MouseEnter += new System.EventHandler(this.StatusLabel_MouseEnter);
            this.StatusLabel.MouseLeave += new System.EventHandler(this.StatusLabel_MouseLeave);
            this.StatusLabel.MouseHover += new System.EventHandler(this.StatusLabel_MouseHover);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(262, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.enterExitSleepModeToolStripMenuItem,
            this.enterExitTempCompModeToolStripMenuItem,
            this.exitOptecTCFSFocuserToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fileToolStripMenuItem.Text = "Device";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // enterExitSleepModeToolStripMenuItem
            // 
            this.enterExitSleepModeToolStripMenuItem.Name = "enterExitSleepModeToolStripMenuItem";
            this.enterExitSleepModeToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.enterExitSleepModeToolStripMenuItem.Text = "Enter/Exit Sleep Mode";
            this.enterExitSleepModeToolStripMenuItem.Click += new System.EventHandler(this.enterExitSleepModeToolStripMenuItem_Click);
            // 
            // enterExitTempCompModeToolStripMenuItem
            // 
            this.enterExitTempCompModeToolStripMenuItem.Name = "enterExitTempCompModeToolStripMenuItem";
            this.enterExitTempCompModeToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.enterExitTempCompModeToolStripMenuItem.Text = "Enter/Exit Temp Comp Mode";
            this.enterExitTempCompModeToolStripMenuItem.Click += new System.EventHandler(this.enterExitTempCompModeToolStripMenuItem_Click);
            // 
            // exitOptecTCFSFocuserToolStripMenuItem
            // 
            this.exitOptecTCFSFocuserToolStripMenuItem.Name = "exitOptecTCFSFocuserToolStripMenuItem";
            this.exitOptecTCFSFocuserToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.exitOptecTCFSFocuserToolStripMenuItem.Text = "Close Program";
            this.exitOptecTCFSFocuserToolStripMenuItem.Click += new System.EventHandler(this.exitOptecTCFSFocuserToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeCOMPortToolStripMenuItem,
            this.deviceSetupToolStripMenuItem,
            this.addFocusOffsetToolStripMenuItem,
            this.removeFocusOffsetsToolStripMenuItem,
            this.addAbsolutePresetToolStripMenuItem1,
            this.removeAbsolutePresetsToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // changeCOMPortToolStripMenuItem
            // 
            this.changeCOMPortToolStripMenuItem.Name = "changeCOMPortToolStripMenuItem";
            this.changeCOMPortToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.changeCOMPortToolStripMenuItem.Text = "Change COM Port";
            // 
            // deviceSetupToolStripMenuItem
            // 
            this.deviceSetupToolStripMenuItem.Name = "deviceSetupToolStripMenuItem";
            this.deviceSetupToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.deviceSetupToolStripMenuItem.Text = "Device Setup";
            this.deviceSetupToolStripMenuItem.Click += new System.EventHandler(this.deviceSetupToolStripMenuItem_Click);
            // 
            // addFocusOffsetToolStripMenuItem
            // 
            this.addFocusOffsetToolStripMenuItem.Name = "addFocusOffsetToolStripMenuItem";
            this.addFocusOffsetToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.addFocusOffsetToolStripMenuItem.Text = "Add Focus Offset...";
            this.addFocusOffsetToolStripMenuItem.Click += new System.EventHandler(this.addFocusOffsetToolStripMenuItem_Click);
            // 
            // removeFocusOffsetsToolStripMenuItem
            // 
            this.removeFocusOffsetsToolStripMenuItem.Name = "removeFocusOffsetsToolStripMenuItem";
            this.removeFocusOffsetsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.removeFocusOffsetsToolStripMenuItem.Text = "Remove Focus Offsets...";
            this.removeFocusOffsetsToolStripMenuItem.Click += new System.EventHandler(this.removeFocusOffsetsToolStripMenuItem_Click);
            // 
            // addAbsolutePresetToolStripMenuItem1
            // 
            this.addAbsolutePresetToolStripMenuItem1.Name = "addAbsolutePresetToolStripMenuItem1";
            this.addAbsolutePresetToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.addAbsolutePresetToolStripMenuItem1.Text = "Add Absolute Preset...";
            this.addAbsolutePresetToolStripMenuItem1.Click += new System.EventHandler(this.addAbsolutePresetToolStripMenuItem1_Click);
            // 
            // removeAbsolutePresetsToolStripMenuItem1
            // 
            this.removeAbsolutePresetsToolStripMenuItem1.Name = "removeAbsolutePresetsToolStripMenuItem1";
            this.removeAbsolutePresetsToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.removeAbsolutePresetsToolStripMenuItem1.Text = "Remove Absolute Presets...";
            this.removeAbsolutePresetsToolStripMenuItem1.Click += new System.EventHandler(this.removeAbsolutePresetsToolStripMenuItem1_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.positionAndTemperatureToolStripMenuItem,
            this.relativeFocusAdjustToolStripMenuItem,
            this.temperatureCompensationToolStripMenuItem,
            this.AbsoluteMoveToolStripMenuItem,
            this.ReletiveFocusOffsetsToolStripMenuItem,
            this.absoluteFocusPresetsToolStripMenuItem,
            this.toolStripSeparator1,
            this.alwaysOnTopToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // positionAndTemperatureToolStripMenuItem
            // 
            this.positionAndTemperatureToolStripMenuItem.Checked = true;
            this.positionAndTemperatureToolStripMenuItem.CheckOnClick = true;
            this.positionAndTemperatureToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.positionAndTemperatureToolStripMenuItem.Name = "positionAndTemperatureToolStripMenuItem";
            this.positionAndTemperatureToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.positionAndTemperatureToolStripMenuItem.Tag = "PosAndTemp";
            this.positionAndTemperatureToolStripMenuItem.Text = "Position and Temperature";
            // 
            // relativeFocusAdjustToolStripMenuItem
            // 
            this.relativeFocusAdjustToolStripMenuItem.Checked = true;
            this.relativeFocusAdjustToolStripMenuItem.CheckOnClick = true;
            this.relativeFocusAdjustToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.relativeFocusAdjustToolStripMenuItem.Name = "relativeFocusAdjustToolStripMenuItem";
            this.relativeFocusAdjustToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.relativeFocusAdjustToolStripMenuItem.Tag = "RelativeFocusAdjust";
            this.relativeFocusAdjustToolStripMenuItem.Text = "Relative Focus Adjust";
            // 
            // temperatureCompensationToolStripMenuItem
            // 
            this.temperatureCompensationToolStripMenuItem.Checked = true;
            this.temperatureCompensationToolStripMenuItem.CheckOnClick = true;
            this.temperatureCompensationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.temperatureCompensationToolStripMenuItem.Name = "temperatureCompensationToolStripMenuItem";
            this.temperatureCompensationToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.temperatureCompensationToolStripMenuItem.Tag = "TempComp";
            this.temperatureCompensationToolStripMenuItem.Text = "Temperature Compensation";
            // 
            // AbsoluteMoveToolStripMenuItem
            // 
            this.AbsoluteMoveToolStripMenuItem.Checked = true;
            this.AbsoluteMoveToolStripMenuItem.CheckOnClick = true;
            this.AbsoluteMoveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AbsoluteMoveToolStripMenuItem.Name = "AbsoluteMoveToolStripMenuItem";
            this.AbsoluteMoveToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.AbsoluteMoveToolStripMenuItem.Tag = "AbsoluteMove";
            this.AbsoluteMoveToolStripMenuItem.Text = "Absolute Move Controls";
            // 
            // ReletiveFocusOffsetsToolStripMenuItem
            // 
            this.ReletiveFocusOffsetsToolStripMenuItem.Checked = true;
            this.ReletiveFocusOffsetsToolStripMenuItem.CheckOnClick = true;
            this.ReletiveFocusOffsetsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReletiveFocusOffsetsToolStripMenuItem.Name = "ReletiveFocusOffsetsToolStripMenuItem";
            this.ReletiveFocusOffsetsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.ReletiveFocusOffsetsToolStripMenuItem.Tag = "RelativeFocusOffsets";
            this.ReletiveFocusOffsetsToolStripMenuItem.Text = "Relative Focus Offsets";
            // 
            // absoluteFocusPresetsToolStripMenuItem
            // 
            this.absoluteFocusPresetsToolStripMenuItem.Checked = true;
            this.absoluteFocusPresetsToolStripMenuItem.CheckOnClick = true;
            this.absoluteFocusPresetsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.absoluteFocusPresetsToolStripMenuItem.Name = "absoluteFocusPresetsToolStripMenuItem";
            this.absoluteFocusPresetsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.absoluteFocusPresetsToolStripMenuItem.Tag = "AbsoluteFocusPresets";
            this.absoluteFocusPresetsToolStripMenuItem.Text = "Absolute Focus Presets";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.alwaysOnTopToolStripMenuItem.Tag = "AlwaysOnTop";
            this.alwaysOnTopToolStripMenuItem.Text = "Always On Top";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.documentationToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
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
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.documentationToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // AbsPosTB
            // 
            this.AbsPosTB.Location = new System.Drawing.Point(16, 9);
            this.AbsPosTB.Mask = "00000";
            this.AbsPosTB.Name = "AbsPosTB";
            this.AbsPosTB.PromptChar = ' ';
            this.AbsPosTB.Size = new System.Drawing.Size(75, 20);
            this.AbsPosTB.TabIndex = 23;
            this.AbsPosTB.Text = "3500";
            this.AbsPosTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StatusBarToolTip.SetToolTip(this.AbsPosTB, "Set absolute position");
            this.AbsPosTB.ValidatingType = typeof(int);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(105, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Go To";
            this.StatusBarToolTip.SetToolTip(this.button1, "Go to absolute position");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AbsolutePresetsMenuStrip1
            // 
            this.AbsolutePresetsMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAbsolutePresetToolStripMenuItem,
            this.removeAbsolutePresetsToolStripMenuItem,
            this.panelHeightToolStripMenuItem1});
            this.AbsolutePresetsMenuStrip1.Name = "AbsolutePresetsMenuStrip1";
            this.AbsolutePresetsMenuStrip1.Size = new System.Drawing.Size(208, 92);
            // 
            // addAbsolutePresetToolStripMenuItem
            // 
            this.addAbsolutePresetToolStripMenuItem.Name = "addAbsolutePresetToolStripMenuItem";
            this.addAbsolutePresetToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.addAbsolutePresetToolStripMenuItem.Text = "Add Absolute Preset";
            this.addAbsolutePresetToolStripMenuItem.Click += new System.EventHandler(this.addAbsolutePresetToolStripMenuItem_Click);
            // 
            // removeAbsolutePresetsToolStripMenuItem
            // 
            this.removeAbsolutePresetsToolStripMenuItem.Name = "removeAbsolutePresetsToolStripMenuItem";
            this.removeAbsolutePresetsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.removeAbsolutePresetsToolStripMenuItem.Text = "Remove Absolute Presets";
            this.removeAbsolutePresetsToolStripMenuItem.Click += new System.EventHandler(this.removeAbsolutePresetsToolStripMenuItem_Click);
            // 
            // panelHeightToolStripMenuItem1
            // 
            this.panelHeightToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.absolutePanelHeightTB});
            this.panelHeightToolStripMenuItem1.Name = "panelHeightToolStripMenuItem1";
            this.panelHeightToolStripMenuItem1.Size = new System.Drawing.Size(207, 22);
            this.panelHeightToolStripMenuItem1.Text = "Panel Height";
            this.panelHeightToolStripMenuItem1.MouseEnter += new System.EventHandler(this.panelHeightToolStripMenuItem1_MouseEnter);
            // 
            // absolutePanelHeightTB
            // 
            this.absolutePanelHeightTB.Name = "absolutePanelHeightTB";
            this.absolutePanelHeightTB.Size = new System.Drawing.Size(100, 23);
            this.absolutePanelHeightTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.absolutePanelHeightTB_KeyPress);
            this.absolutePanelHeightTB.TextChanged += new System.EventHandler(this.absolutePanelHeightTB_TextChanged);
            // 
            // FilterOffsetContextMenuStrip1
            // 
            this.FilterOffsetContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.removeOffsetToolStripMenuItem,
            this.panelHeightToolStripMenuItem});
            this.FilterOffsetContextMenuStrip1.Name = "contextMenuStrip1";
            this.FilterOffsetContextMenuStrip1.ShowImageMargin = false;
            this.FilterOffsetContextMenuStrip1.Size = new System.Drawing.Size(128, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItem1.Text = "Add Offset";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.addRelativeOffsetContextMenu_Click);
            // 
            // removeOffsetToolStripMenuItem
            // 
            this.removeOffsetToolStripMenuItem.Name = "removeOffsetToolStripMenuItem";
            this.removeOffsetToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.removeOffsetToolStripMenuItem.Text = "Remove Offset";
            this.removeOffsetToolStripMenuItem.Click += new System.EventHandler(this.removeOffsetToolStripMenuItem_Click);
            // 
            // panelHeightToolStripMenuItem
            // 
            this.panelHeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PanelHeightTextBox1});
            this.panelHeightToolStripMenuItem.Name = "panelHeightToolStripMenuItem";
            this.panelHeightToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.panelHeightToolStripMenuItem.Text = "Panel Height";
            this.panelHeightToolStripMenuItem.MouseEnter += new System.EventHandler(this.panelHeightToolStripMenuItem_MouseEnter);
            // 
            // PanelHeightTextBox1
            // 
            this.PanelHeightTextBox1.Name = "PanelHeightTextBox1";
            this.PanelHeightTextBox1.Size = new System.Drawing.Size(100, 23);
            this.PanelHeightTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PanelHeightTextBox1_KeyPress);
            this.PanelHeightTextBox1.TextChanged += new System.EventHandler(this.PanelHeightTextBox1_TextChanged);
            // 
            // TempDRO_TB
            // 
            this.TempDRO_TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TempDRO_TB.BackColor = System.Drawing.Color.Black;
            this.TempDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TempDRO_TB.ContextMenuStrip = this.TempProbecontextMenuStrip1;
            this.TempDRO_TB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TempDRO_TB.Font = new System.Drawing.Font("Nina", 18F);
            this.TempDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.TempDRO_TB.Location = new System.Drawing.Point(35, 67);
            this.TempDRO_TB.Name = "TempDRO_TB";
            this.TempDRO_TB.Size = new System.Drawing.Size(167, 36);
            this.TempDRO_TB.TabIndex = 27;
            this.TempDRO_TB.Text = "------";
            this.TempDRO_TB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StatusBarToolTip.SetToolTip(this.TempDRO_TB, "Click to change units. Right-click to enable/diable.");
            this.TempDRO_TB.Click += new System.EventHandler(this.TempDRO_TB_DoubleClick);
            // 
            // TempProbecontextMenuStrip1
            // 
            this.TempProbecontextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableProbeToolStripMenuItem,
            this.disableProbeToolStripMenuItem});
            this.TempProbecontextMenuStrip1.Name = "TempProbecontextMenuStrip1";
            this.TempProbecontextMenuStrip1.Size = new System.Drawing.Size(147, 48);
            // 
            // enableProbeToolStripMenuItem
            // 
            this.enableProbeToolStripMenuItem.Name = "enableProbeToolStripMenuItem";
            this.enableProbeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.enableProbeToolStripMenuItem.Tag = "ENABLE";
            this.enableProbeToolStripMenuItem.Text = "Enable Probe";
            this.enableProbeToolStripMenuItem.Click += new System.EventHandler(this.EnableDisableProbeCTMS);
            // 
            // disableProbeToolStripMenuItem
            // 
            this.disableProbeToolStripMenuItem.Name = "disableProbeToolStripMenuItem";
            this.disableProbeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.disableProbeToolStripMenuItem.Tag = "DISABLE";
            this.disableProbeToolStripMenuItem.Text = "Disable Probe";
            this.disableProbeToolStripMenuItem.Click += new System.EventHandler(this.EnableDisableProbeCTMS);
            // 
            // PosDRO_TB
            // 
            this.PosDRO_TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PosDRO_TB.BackColor = System.Drawing.Color.Black;
            this.PosDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PosDRO_TB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PosDRO_TB.Font = new System.Drawing.Font("Nina", 18F);
            this.PosDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.PosDRO_TB.Location = new System.Drawing.Point(35, 6);
            this.PosDRO_TB.Name = "PosDRO_TB";
            this.PosDRO_TB.Size = new System.Drawing.Size(165, 36);
            this.PosDRO_TB.TabIndex = 28;
            this.PosDRO_TB.Text = "------";
            this.PosDRO_TB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StatusBarToolTip.SetToolTip(this.PosDRO_TB, "Click to change units");
            this.PosDRO_TB.Click += new System.EventHandler(this.PosDRO_TB_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::Optec_TCF_S_Focuser.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(206, -2);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(29, 36);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(29, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 39);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            this.StatusBarToolTip.SetToolTip(this.pictureBox1, "Goto ASCOM website");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // PowerLight
            // 
            this.PowerLight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PowerLight.Image = global::Optec_TCF_S_Focuser.Properties.Resources.RedLight;
            this.PowerLight.Location = new System.Drawing.Point(2, 7);
            this.PowerLight.MaximumSize = new System.Drawing.Size(22, 22);
            this.PowerLight.Name = "PowerLight";
            this.PowerLight.Size = new System.Drawing.Size(22, 22);
            this.PowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PowerLight.TabIndex = 21;
            this.PowerLight.TabStop = false;
            this.PowerLight.Click += new System.EventHandler(this.PowerLight_Click);
            this.PowerLight.MouseEnter += new System.EventHandler(this.PowerLight_MouseEnter);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::Optec_TCF_S_Focuser.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox2.Location = new System.Drawing.Point(-17, -2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(140, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            this.StatusBarToolTip.SetToolTip(this.pictureBox2, "Goto Optec website");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // StatusTimer
            // 
            this.StatusTimer.Enabled = true;
            this.StatusTimer.Interval = 2000;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // TempModeON_RB
            // 
            this.TempModeON_RB.AutoSize = true;
            this.TempModeON_RB.Location = new System.Drawing.Point(165, 3);
            this.TempModeON_RB.Name = "TempModeON_RB";
            this.TempModeON_RB.Size = new System.Drawing.Size(39, 17);
            this.TempModeON_RB.TabIndex = 30;
            this.TempModeON_RB.Tag = "True";
            this.TempModeON_RB.Text = "On";
            this.StatusBarToolTip.SetToolTip(this.TempModeON_RB, "Turn on Temp. Comp.");
            this.TempModeON_RB.UseVisualStyleBackColor = true;
            this.TempModeON_RB.CheckedChanged += new System.EventHandler(this.ToggleTempCompRB_Click);
            // 
            // TempModeOFF_RB
            // 
            this.TempModeOFF_RB.AutoSize = true;
            this.TempModeOFF_RB.Checked = true;
            this.TempModeOFF_RB.Location = new System.Drawing.Point(165, 22);
            this.TempModeOFF_RB.Name = "TempModeOFF_RB";
            this.TempModeOFF_RB.Size = new System.Drawing.Size(39, 17);
            this.TempModeOFF_RB.TabIndex = 31;
            this.TempModeOFF_RB.TabStop = true;
            this.TempModeOFF_RB.Tag = "False";
            this.TempModeOFF_RB.Text = "Off";
            this.StatusBarToolTip.SetToolTip(this.TempModeOFF_RB, "Turn off Temp. Comp.");
            this.TempModeOFF_RB.UseVisualStyleBackColor = true;
            this.TempModeOFF_RB.CheckedChanged += new System.EventHandler(this.ToggleTempCompRB_Click);
            // 
            // AbsolutePresetPanel
            // 
            this.AbsolutePresetPanel.AutoScroll = true;
            this.AbsolutePresetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AbsolutePresetPanel.ContextMenuStrip = this.AbsolutePresetsMenuStrip1;
            this.AbsolutePresetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AbsolutePresetPanel.Location = new System.Drawing.Point(3, 458);
            this.AbsolutePresetPanel.Name = "AbsolutePresetPanel";
            this.AbsolutePresetPanel.Size = new System.Drawing.Size(237, 69);
            this.AbsolutePresetPanel.TabIndex = 34;
            this.StatusBarToolTip.SetToolTip(this.AbsolutePresetPanel, "Right-click to add/remove");
            // 
            // FocusOffsetPanel
            // 
            this.FocusOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FocusOffsetPanel.AutoScroll = true;
            this.FocusOffsetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FocusOffsetPanel.ContextMenuStrip = this.FilterOffsetContextMenuStrip1;
            this.FocusOffsetPanel.Location = new System.Drawing.Point(3, 367);
            this.FocusOffsetPanel.Name = "FocusOffsetPanel";
            this.FocusOffsetPanel.Padding = new System.Windows.Forms.Padding(5);
            this.FocusOffsetPanel.Size = new System.Drawing.Size(237, 69);
            this.FocusOffsetPanel.TabIndex = 4;
            this.StatusBarToolTip.SetToolTip(this.FocusOffsetPanel, "Right-click to add/remove");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Temperature Compensation:";
            // 
            // TempCompPanel
            // 
            this.TempCompPanel.Controls.Add(this.label1);
            this.TempCompPanel.Controls.Add(this.TempModeOFF_RB);
            this.TempCompPanel.Controls.Add(this.TempModeON_RB);
            this.TempCompPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TempCompPanel.Location = new System.Drawing.Point(3, 235);
            this.TempCompPanel.Name = "TempCompPanel";
            this.TempCompPanel.Size = new System.Drawing.Size(237, 44);
            this.TempCompPanel.TabIndex = 32;
            // 
            // NewVersionBGWorker
            // 
            this.NewVersionBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NewVersionBGWorker_DoWork);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.TempCompPanel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.AbsoluteMovePanel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.AbsolutePresetPanel, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.FocusOffsetPanel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(243, 530);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Absolute Focus Adjust:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Relative Focus Offsets:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Absolute Focus Presets:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Temp_LBL);
            this.panel1.Controls.Add(this.Pos_LBL);
            this.panel1.Controls.Add(this.PowerLight);
            this.panel1.Controls.Add(this.TempDRO_TB);
            this.panel1.Controls.Add(this.PosDRO_TB);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 124);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.In_BTN);
            this.panel2.Controls.Add(this.StepSize_LBL);
            this.panel2.Controls.Add(this.StepSize_NUD);
            this.panel2.Controls.Add(this.OUT_Btn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 175);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(237, 54);
            this.panel2.TabIndex = 1;
            // 
            // AbsoluteMovePanel
            // 
            this.AbsoluteMovePanel.Controls.Add(this.AbsPosTB);
            this.AbsoluteMovePanel.Controls.Add(this.Center_Btn);
            this.AbsoluteMovePanel.Controls.Add(this.button1);
            this.AbsoluteMovePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AbsoluteMovePanel.Location = new System.Drawing.Point(3, 301);
            this.AbsoluteMovePanel.Name = "AbsoluteMovePanel";
            this.AbsoluteMovePanel.Size = new System.Drawing.Size(237, 44);
            this.AbsoluteMovePanel.TabIndex = 33;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(237, 36);
            this.panel3.TabIndex = 35;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 587);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = global::Optec_TCF_S_Focuser.Properties.Resources.TCF_S_2010;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(278, 1028);
            this.MinimumSize = new System.Drawing.Size(277, 38);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec TCF-S Focuser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.AbsolutePresetsMenuStrip1.ResumeLayout(false);
            this.FilterOffsetContextMenuStrip1.ResumeLayout(false);
            this.TempProbecontextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.TempCompPanel.ResumeLayout(false);
            this.TempCompPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.AbsoluteMovePanel.ResumeLayout(false);
            this.AbsoluteMovePanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Temp_LBL;
        private System.Windows.Forms.Label Pos_LBL;
        private System.Windows.Forms.Button OUT_Btn;
        private System.Windows.Forms.Button Center_Btn;
        private System.Windows.Forms.NumericUpDown StepSize_NUD;
        private System.Windows.Forms.Button In_BTN;
        private System.Windows.Forms.Label StepSize_LBL;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox PowerLight;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MaskedTextBox AbsPosTB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterExitSleepModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterExitTempCompModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitOptecTCFSFocuserToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip FilterOffsetContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AbsoluteMoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReletiveFocusOffsetsToolStripMenuItem;
        private System.Windows.Forms.Label TempDRO_TB;
        private System.Windows.Forms.Label PosDRO_TB;
        private System.Windows.Forms.ToolStripMenuItem addFocusOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFocusOffsetsToolStripMenuItem;
        private System.Windows.Forms.Timer StatusTimer;
        private System.Windows.Forms.ToolTip StatusBarToolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton TempModeON_RB;
        private System.Windows.Forms.RadioButton TempModeOFF_RB;
        private System.Windows.Forms.Panel TempCompPanel;
        private System.Windows.Forms.ToolStripMenuItem changeCOMPortToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker NewVersionBGWorker;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip AbsolutePresetsMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addAbsolutePresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAbsolutePresetsToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel FocusOffsetPanel;
        private System.Windows.Forms.Panel AbsoluteMovePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel AbsolutePresetPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem positionAndTemperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem relativeFocusAdjustToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureCompensationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem absoluteFocusPresetsToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip TempProbecontextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem enableProbeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableProbeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAbsolutePresetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeAbsolutePresetsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem panelHeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox PanelHeightTextBox1;
        private System.Windows.Forms.ToolStripMenuItem panelHeightToolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox absolutePanelHeightTB;
    }
}

