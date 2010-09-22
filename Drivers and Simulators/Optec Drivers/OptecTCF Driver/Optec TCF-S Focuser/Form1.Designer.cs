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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideAbsoluteMoveControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideFocusOffsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbsPosTB = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AbsoluteGB = new System.Windows.Forms.GroupBox();
            this.FocusOffsetsGB = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FocusOffsetPanel = new System.Windows.Forms.Panel();
            this.TempDRO_TB = new System.Windows.Forms.Label();
            this.PosDRO_TB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PowerLight = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusBarToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.TempCompPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.AbsoluteGB.SuspendLayout();
            this.FocusOffsetsGB.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.TempCompPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Temp_LBL
            // 
            this.Temp_LBL.AutoSize = true;
            this.Temp_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temp_LBL.Location = new System.Drawing.Point(78, 177);
            this.Temp_LBL.Name = "Temp_LBL";
            this.Temp_LBL.Size = new System.Drawing.Size(86, 16);
            this.Temp_LBL.TabIndex = 8;
            this.Temp_LBL.Text = "Temperature";
            // 
            // Pos_LBL
            // 
            this.Pos_LBL.AutoSize = true;
            this.Pos_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pos_LBL.Location = new System.Drawing.Point(73, 115);
            this.Pos_LBL.Name = "Pos_LBL";
            this.Pos_LBL.Size = new System.Drawing.Size(56, 16);
            this.Pos_LBL.TabIndex = 7;
            this.Pos_LBL.Text = "Position";
            // 
            // OUT_Btn
            // 
            this.OUT_Btn.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.OUT_Btn.Enabled = false;
            this.OUT_Btn.Location = new System.Drawing.Point(169, 201);
            this.OUT_Btn.Name = "OUT_Btn";
            this.OUT_Btn.Size = new System.Drawing.Size(45, 45);
            this.OUT_Btn.TabIndex = 17;
            this.OUT_Btn.Text = "OUT";
            this.OUT_Btn.UseVisualStyleBackColor = false;
            this.OUT_Btn.Click += new System.EventHandler(this.OUT_Btn_Click);
            // 
            // Center_Btn
            // 
            this.Center_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Center_Btn.Location = new System.Drawing.Point(160, 23);
            this.Center_Btn.Name = "Center_Btn";
            this.Center_Btn.Size = new System.Drawing.Size(51, 23);
            this.Center_Btn.TabIndex = 16;
            this.Center_Btn.Text = "Center";
            this.Center_Btn.UseVisualStyleBackColor = false;
            this.Center_Btn.Click += new System.EventHandler(this.Center_Btn_Click);
            // 
            // StepSize_NUD
            // 
            this.StepSize_NUD.Enabled = false;
            this.StepSize_NUD.Location = new System.Drawing.Point(88, 226);
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
            this.In_BTN.Location = new System.Drawing.Point(29, 201);
            this.In_BTN.Name = "In_BTN";
            this.In_BTN.Size = new System.Drawing.Size(45, 45);
            this.In_BTN.TabIndex = 13;
            this.In_BTN.Text = "IN";
            this.In_BTN.UseVisualStyleBackColor = false;
            this.In_BTN.Click += new System.EventHandler(this.In_BTN_Click);
            // 
            // StepSize_LBL
            // 
            this.StepSize_LBL.AutoSize = true;
            this.StepSize_LBL.Enabled = false;
            this.StepSize_LBL.Location = new System.Drawing.Point(94, 210);
            this.StepSize_LBL.Name = "StepSize_LBL";
            this.StepSize_LBL.Size = new System.Drawing.Size(55, 13);
            this.StepSize_LBL.TabIndex = 15;
            this.StepSize_LBL.Text = "Step Size:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 489);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(242, 22);
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
            this.menuStrip1.Size = new System.Drawing.Size(242, 24);
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
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeCOMPortToolStripMenuItem,
            this.deviceSetupToolStripMenuItem,
            this.addFocusOffsetToolStripMenuItem,
            this.removeFocusOffsetsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // changeCOMPortToolStripMenuItem
            // 
            this.changeCOMPortToolStripMenuItem.Name = "changeCOMPortToolStripMenuItem";
            this.changeCOMPortToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.changeCOMPortToolStripMenuItem.Text = "Change COM Port";
            // 
            // deviceSetupToolStripMenuItem
            // 
            this.deviceSetupToolStripMenuItem.Name = "deviceSetupToolStripMenuItem";
            this.deviceSetupToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.deviceSetupToolStripMenuItem.Text = "Device Setup";
            this.deviceSetupToolStripMenuItem.Click += new System.EventHandler(this.deviceSetupToolStripMenuItem_Click);
            // 
            // addFocusOffsetToolStripMenuItem
            // 
            this.addFocusOffsetToolStripMenuItem.Name = "addFocusOffsetToolStripMenuItem";
            this.addFocusOffsetToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.addFocusOffsetToolStripMenuItem.Text = "Add Focus Offset...";
            this.addFocusOffsetToolStripMenuItem.Click += new System.EventHandler(this.addFocusOffsetToolStripMenuItem_Click);
            // 
            // removeFocusOffsetsToolStripMenuItem
            // 
            this.removeFocusOffsetsToolStripMenuItem.Name = "removeFocusOffsetsToolStripMenuItem";
            this.removeFocusOffsetsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.removeFocusOffsetsToolStripMenuItem.Text = "Remove Focus Offsets...";
            this.removeFocusOffsetsToolStripMenuItem.Click += new System.EventHandler(this.removeFocusOffsetsToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideAbsoluteMoveControlsToolStripMenuItem,
            this.showHideFocusOffsetsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showHideAbsoluteMoveControlsToolStripMenuItem
            // 
            this.showHideAbsoluteMoveControlsToolStripMenuItem.Name = "showHideAbsoluteMoveControlsToolStripMenuItem";
            this.showHideAbsoluteMoveControlsToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.showHideAbsoluteMoveControlsToolStripMenuItem.Text = "Show/Hide Absolute Move Controls";
            this.showHideAbsoluteMoveControlsToolStripMenuItem.Click += new System.EventHandler(this.showHideAbsoluteMoveControlsToolStripMenuItem_Click);
            // 
            // showHideFocusOffsetsToolStripMenuItem
            // 
            this.showHideFocusOffsetsToolStripMenuItem.Name = "showHideFocusOffsetsToolStripMenuItem";
            this.showHideFocusOffsetsToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.showHideFocusOffsetsToolStripMenuItem.Text = "Show/Hide Focus Offsets";
            this.showHideFocusOffsetsToolStripMenuItem.Click += new System.EventHandler(this.showHideFocusOffsetsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.documentationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            // 
            // AbsPosTB
            // 
            this.AbsPosTB.Location = new System.Drawing.Point(6, 25);
            this.AbsPosTB.Mask = "00000";
            this.AbsPosTB.Name = "AbsPosTB";
            this.AbsPosTB.PromptChar = ' ';
            this.AbsPosTB.Size = new System.Drawing.Size(75, 20);
            this.AbsPosTB.TabIndex = 23;
            this.AbsPosTB.Text = "3500";
            this.AbsPosTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AbsPosTB.ValidatingType = typeof(int);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(95, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Go To";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AbsoluteGB
            // 
            this.AbsoluteGB.Controls.Add(this.AbsPosTB);
            this.AbsoluteGB.Controls.Add(this.button1);
            this.AbsoluteGB.Controls.Add(this.Center_Btn);
            this.AbsoluteGB.Enabled = false;
            this.AbsoluteGB.Location = new System.Drawing.Point(11, 303);
            this.AbsoluteGB.Name = "AbsoluteGB";
            this.AbsoluteGB.Size = new System.Drawing.Size(221, 63);
            this.AbsoluteGB.TabIndex = 25;
            this.AbsoluteGB.TabStop = false;
            this.AbsoluteGB.Text = "Absolute Move";
            // 
            // FocusOffsetsGB
            // 
            this.FocusOffsetsGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.FocusOffsetsGB.ContextMenuStrip = this.contextMenuStrip1;
            this.FocusOffsetsGB.Controls.Add(this.FocusOffsetPanel);
            this.FocusOffsetsGB.Enabled = false;
            this.FocusOffsetsGB.Location = new System.Drawing.Point(11, 374);
            this.FocusOffsetsGB.Name = "FocusOffsetsGB";
            this.FocusOffsetsGB.Size = new System.Drawing.Size(221, 112);
            this.FocusOffsetsGB.TabIndex = 26;
            this.FocusOffsetsGB.TabStop = false;
            this.FocusOffsetsGB.Text = "Filter Focus Offsets";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.removeOffsetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItem1.Text = "Add Offset";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // removeOffsetToolStripMenuItem
            // 
            this.removeOffsetToolStripMenuItem.Name = "removeOffsetToolStripMenuItem";
            this.removeOffsetToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.removeOffsetToolStripMenuItem.Text = "Remove Offset";
            this.removeOffsetToolStripMenuItem.Click += new System.EventHandler(this.removeOffsetToolStripMenuItem_Click);
            // 
            // FocusOffsetPanel
            // 
            this.FocusOffsetPanel.AutoScroll = true;
            this.FocusOffsetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FocusOffsetPanel.Location = new System.Drawing.Point(3, 16);
            this.FocusOffsetPanel.Name = "FocusOffsetPanel";
            this.FocusOffsetPanel.Size = new System.Drawing.Size(215, 93);
            this.FocusOffsetPanel.TabIndex = 4;
            // 
            // TempDRO_TB
            // 
            this.TempDRO_TB.BackColor = System.Drawing.Color.Black;
            this.TempDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TempDRO_TB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TempDRO_TB.Font = new System.Drawing.Font("Nina", 18F);
            this.TempDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.TempDRO_TB.Location = new System.Drawing.Point(61, 140);
            this.TempDRO_TB.Name = "TempDRO_TB";
            this.TempDRO_TB.Size = new System.Drawing.Size(120, 36);
            this.TempDRO_TB.TabIndex = 27;
            this.TempDRO_TB.Text = "------";
            this.TempDRO_TB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TempDRO_TB.Click += new System.EventHandler(this.TempDRO_TB_DoubleClick);
            this.TempDRO_TB.MouseEnter += new System.EventHandler(this.PosDRO_TB_MouseEnter);
            // 
            // PosDRO_TB
            // 
            this.PosDRO_TB.BackColor = System.Drawing.Color.Black;
            this.PosDRO_TB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PosDRO_TB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PosDRO_TB.Font = new System.Drawing.Font("Nina", 18F);
            this.PosDRO_TB.ForeColor = System.Drawing.Color.Red;
            this.PosDRO_TB.Location = new System.Drawing.Point(61, 79);
            this.PosDRO_TB.Name = "PosDRO_TB";
            this.PosDRO_TB.Size = new System.Drawing.Size(120, 36);
            this.PosDRO_TB.TabIndex = 28;
            this.PosDRO_TB.Text = "------";
            this.PosDRO_TB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PosDRO_TB.Click += new System.EventHandler(this.PosDRO_TB_Click);
            this.PosDRO_TB.MouseEnter += new System.EventHandler(this.PosDRO_TB_MouseEnter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::Optec_TCF_S_Focuser.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(204, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // PowerLight
            // 
            this.PowerLight.Enabled = false;
            this.PowerLight.Image = global::Optec_TCF_S_Focuser.Properties.Resources.RedLight;
            this.PowerLight.Location = new System.Drawing.Point(26, 80);
            this.PowerLight.Name = "PowerLight";
            this.PowerLight.Size = new System.Drawing.Size(16, 16);
            this.PowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PowerLight.TabIndex = 21;
            this.PowerLight.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::Optec_TCF_S_Focuser.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox2.Location = new System.Drawing.Point(9, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(141, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // StatusTimer
            // 
            this.StatusTimer.Enabled = true;
            this.StatusTimer.Interval = 2000;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Temperature Compensation";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(153, 7);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(39, 17);
            this.radioButton1.TabIndex = 30;
            this.radioButton1.Tag = "True";
            this.radioButton1.Text = "On";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.ToggleTempCompRB_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(153, 23);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(39, 17);
            this.radioButton2.TabIndex = 31;
            this.radioButton2.TabStop = true;
            this.radioButton2.Tag = "False";
            this.radioButton2.Text = "Off";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.ToggleTempCompRB_Click);
            // 
            // TempCompPanel
            // 
            this.TempCompPanel.Controls.Add(this.label1);
            this.TempCompPanel.Controls.Add(this.radioButton2);
            this.TempCompPanel.Controls.Add(this.radioButton1);
            this.TempCompPanel.Location = new System.Drawing.Point(21, 250);
            this.TempCompPanel.Name = "TempCompPanel";
            this.TempCompPanel.Size = new System.Drawing.Size(200, 47);
            this.TempCompPanel.TabIndex = 32;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 511);
            this.Controls.Add(this.TempCompPanel);
            this.Controls.Add(this.PosDRO_TB);
            this.Controls.Add(this.TempDRO_TB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.PowerLight);
            this.Controls.Add(this.FocusOffsetsGB);
            this.Controls.Add(this.AbsoluteGB);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.OUT_Btn);
            this.Controls.Add(this.StepSize_NUD);
            this.Controls.Add(this.In_BTN);
            this.Controls.Add(this.StepSize_LBL);
            this.Controls.Add(this.Temp_LBL);
            this.Controls.Add(this.Pos_LBL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(258, 1028);
            this.MinimumSize = new System.Drawing.Size(258, 38);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec TCF-S Focuser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.StepSize_NUD)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.AbsoluteGB.ResumeLayout(false);
            this.AbsoluteGB.PerformLayout();
            this.FocusOffsetsGB.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.TempCompPanel.ResumeLayout(false);
            this.TempCompPanel.PerformLayout();
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
        private System.Windows.Forms.GroupBox AbsoluteGB;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterExitSleepModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterExitTempCompModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitOptecTCFSFocuserToolStripMenuItem;
        private System.Windows.Forms.GroupBox FocusOffsetsGB;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeOffsetToolStripMenuItem;
        private System.Windows.Forms.Panel FocusOffsetPanel;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideAbsoluteMoveControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideFocusOffsetsToolStripMenuItem;
        private System.Windows.Forms.Label TempDRO_TB;
        private System.Windows.Forms.Label PosDRO_TB;
        private System.Windows.Forms.ToolStripMenuItem addFocusOffsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFocusOffsetsToolStripMenuItem;
        private System.Windows.Forms.Timer StatusTimer;
        private System.Windows.Forms.ToolTip StatusBarToolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel TempCompPanel;
        private System.Windows.Forms.ToolStripMenuItem changeCOMPortToolStripMenuItem;
    }
}

