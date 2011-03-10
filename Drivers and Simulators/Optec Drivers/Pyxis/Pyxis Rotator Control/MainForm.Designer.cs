namespace Pyxis_Rotator_Control
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sleepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.instancesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEditRemoveInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.skyPADisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotatorDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.absoluteMoveControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relativeMoveControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PositionUpdateBGWorker = new System.ComponentModel.BackgroundWorker();
            this.RelativeMovePanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RelativeIncrement_NUD = new System.Windows.Forms.NumericUpDown();
            this.RelMoveFwd_BTN = new System.Windows.Forms.Button();
            this.RelMoveBack_BTN = new System.Windows.Forms.Button();
            this.AbsoluteMovePanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AdjustedTargetPA_TB = new System.Windows.Forms.TextBox();
            this.GoToAdjustedPA_BTN = new System.Windows.Forms.Button();
            this.Home_Btn = new System.Windows.Forms.Button();
            this.Halt_BTN = new System.Windows.Forms.Button();
            this.Park_BTN = new System.Windows.Forms.Button();
            this.RotatorDiagram_Panel = new System.Windows.Forms.Panel();
            this.RotatorDiagram = new System.Windows.Forms.PictureBox();
            this.SkyPA_Panel = new System.Windows.Forms.Panel();
            this.PowerLight_Pic = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SetSkyPA_BTN = new System.Windows.Forms.Button();
            this.CurrentPosition_LBL = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.RelativeMovePanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeIncrement_NUD)).BeginInit();
            this.AbsoluteMovePanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.RotatorDiagram_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotatorDiagram)).BeginInit();
            this.SkyPA_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight_Pic)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.deviceToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 41;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.homeToolStripMenuItem,
            this.parkToolStripMenuItem,
            this.sleepToolStripMenuItem,
            this.wakeToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // parkToolStripMenuItem
            // 
            this.parkToolStripMenuItem.Name = "parkToolStripMenuItem";
            this.parkToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.parkToolStripMenuItem.Text = "Park";
            this.parkToolStripMenuItem.Click += new System.EventHandler(this.parkToolStripMenuItem_Click);
            // 
            // sleepToolStripMenuItem
            // 
            this.sleepToolStripMenuItem.Name = "sleepToolStripMenuItem";
            this.sleepToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.sleepToolStripMenuItem.Text = "Sleep";
            this.sleepToolStripMenuItem.Click += new System.EventHandler(this.sleepToolStripMenuItem_Click);
            // 
            // wakeToolStripMenuItem
            // 
            this.wakeToolStripMenuItem.Name = "wakeToolStripMenuItem";
            this.wakeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.wakeToolStripMenuItem.Text = "Wake";
            this.wakeToolStripMenuItem.Click += new System.EventHandler(this.wakeToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMPortToolStripMenuItem,
            this.advancedSettingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.instancesToolStripMenuItem,
            this.addEditRemoveInstanceToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // cOMPortToolStripMenuItem
            // 
            this.cOMPortToolStripMenuItem.Name = "cOMPortToolStripMenuItem";
            this.cOMPortToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.cOMPortToolStripMenuItem.Text = "COM Port";
            this.cOMPortToolStripMenuItem.MouseEnter += new System.EventHandler(this.cOMPortToolStripMenuItem_MouseEnter);
            // 
            // advancedSettingsToolStripMenuItem
            // 
            this.advancedSettingsToolStripMenuItem.Name = "advancedSettingsToolStripMenuItem";
            this.advancedSettingsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.advancedSettingsToolStripMenuItem.Text = "Advanced Settings";
            this.advancedSettingsToolStripMenuItem.Click += new System.EventHandler(this.advancedSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(222, 6);
            // 
            // instancesToolStripMenuItem
            // 
            this.instancesToolStripMenuItem.Name = "instancesToolStripMenuItem";
            this.instancesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.instancesToolStripMenuItem.Text = "Change Instance";
            this.instancesToolStripMenuItem.MouseEnter += new System.EventHandler(this.instancesToolStripMenuItem_MouseEnter);
            // 
            // addEditRemoveInstanceToolStripMenuItem
            // 
            this.addEditRemoveInstanceToolStripMenuItem.Name = "addEditRemoveInstanceToolStripMenuItem";
            this.addEditRemoveInstanceToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.addEditRemoveInstanceToolStripMenuItem.Text = "Add/Edit/Remove Instance...";
            this.addEditRemoveInstanceToolStripMenuItem.Click += new System.EventHandler(this.addEditRemoveInstanceToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.skyPADisplayToolStripMenuItem,
            this.rotatorDiagramToolStripMenuItem,
            this.absoluteMoveControlsToolStripMenuItem,
            this.relativeMoveControlsToolStripMenuItem,
            this.toolStripSeparator2,
            this.alwaysOnTopToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.showAllToolStripMenuItem.Text = "Show All";
            this.showAllToolStripMenuItem.Click += new System.EventHandler(this.showAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // skyPADisplayToolStripMenuItem
            // 
            this.skyPADisplayToolStripMenuItem.Checked = true;
            this.skyPADisplayToolStripMenuItem.CheckOnClick = true;
            this.skyPADisplayToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skyPADisplayToolStripMenuItem.Name = "skyPADisplayToolStripMenuItem";
            this.skyPADisplayToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.skyPADisplayToolStripMenuItem.Text = "Sky PA Display";
            this.skyPADisplayToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewSettingItem_CheckedChanged);
            // 
            // rotatorDiagramToolStripMenuItem
            // 
            this.rotatorDiagramToolStripMenuItem.Checked = true;
            this.rotatorDiagramToolStripMenuItem.CheckOnClick = true;
            this.rotatorDiagramToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotatorDiagramToolStripMenuItem.Name = "rotatorDiagramToolStripMenuItem";
            this.rotatorDiagramToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.rotatorDiagramToolStripMenuItem.Text = "Rotator Diagram";
            this.rotatorDiagramToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewSettingItem_CheckedChanged);
            // 
            // absoluteMoveControlsToolStripMenuItem
            // 
            this.absoluteMoveControlsToolStripMenuItem.Checked = true;
            this.absoluteMoveControlsToolStripMenuItem.CheckOnClick = true;
            this.absoluteMoveControlsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.absoluteMoveControlsToolStripMenuItem.Name = "absoluteMoveControlsToolStripMenuItem";
            this.absoluteMoveControlsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.absoluteMoveControlsToolStripMenuItem.Text = "Absolute Move Controls";
            this.absoluteMoveControlsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewSettingItem_CheckedChanged);
            // 
            // relativeMoveControlsToolStripMenuItem
            // 
            this.relativeMoveControlsToolStripMenuItem.Checked = true;
            this.relativeMoveControlsToolStripMenuItem.CheckOnClick = true;
            this.relativeMoveControlsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.relativeMoveControlsToolStripMenuItem.Name = "relativeMoveControlsToolStripMenuItem";
            this.relativeMoveControlsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.relativeMoveControlsToolStripMenuItem.Text = "Relative Move Controls";
            this.relativeMoveControlsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ViewSettingItem_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 491);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(292, 22);
            this.statusStrip1.TabIndex = 42;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(118, 17);
            this.StatusLabel.Text = "toolStripStatusLabel1";
            // 
            // PositionUpdateBGWorker
            // 
            this.PositionUpdateBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PositionUpdateBGWorker_DoWork);
            // 
            // RelativeMovePanel
            // 
            this.RelativeMovePanel.Controls.Add(this.groupBox1);
            this.RelativeMovePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RelativeMovePanel.Location = new System.Drawing.Point(3, 386);
            this.RelativeMovePanel.Name = "RelativeMovePanel";
            this.RelativeMovePanel.Size = new System.Drawing.Size(261, 68);
            this.RelativeMovePanel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.RelativeIncrement_NUD);
            this.groupBox1.Controls.Add(this.RelMoveFwd_BTN);
            this.groupBox1.Controls.Add(this.RelMoveBack_BTN);
            this.groupBox1.Location = new System.Drawing.Point(8, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 57);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Relative Move";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Increment";
            // 
            // RelativeIncrement_NUD
            // 
            this.RelativeIncrement_NUD.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.RelativeIncrement_NUD.Location = new System.Drawing.Point(90, 27);
            this.RelativeIncrement_NUD.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RelativeIncrement_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RelativeIncrement_NUD.Name = "RelativeIncrement_NUD";
            this.RelativeIncrement_NUD.Size = new System.Drawing.Size(66, 20);
            this.RelativeIncrement_NUD.TabIndex = 28;
            this.RelativeIncrement_NUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RelativeIncrement_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // RelMoveFwd_BTN
            // 
            this.RelMoveFwd_BTN.Location = new System.Drawing.Point(174, 26);
            this.RelMoveFwd_BTN.Name = "RelMoveFwd_BTN";
            this.RelMoveFwd_BTN.Size = new System.Drawing.Size(63, 23);
            this.RelMoveFwd_BTN.TabIndex = 27;
            this.RelMoveFwd_BTN.Text = ">>";
            this.RelMoveFwd_BTN.UseVisualStyleBackColor = true;
            this.RelMoveFwd_BTN.Click += new System.EventHandler(this.RelMoveFwd_BTN_Click);
            // 
            // RelMoveBack_BTN
            // 
            this.RelMoveBack_BTN.Location = new System.Drawing.Point(9, 26);
            this.RelMoveBack_BTN.Name = "RelMoveBack_BTN";
            this.RelMoveBack_BTN.Size = new System.Drawing.Size(63, 23);
            this.RelMoveBack_BTN.TabIndex = 26;
            this.RelMoveBack_BTN.Text = "<<";
            this.RelMoveBack_BTN.UseVisualStyleBackColor = true;
            this.RelMoveBack_BTN.Click += new System.EventHandler(this.RelMoveBack_BTN_Click);
            // 
            // AbsoluteMovePanel
            // 
            this.AbsoluteMovePanel.Controls.Add(this.groupBox2);
            this.AbsoluteMovePanel.Controls.Add(this.Home_Btn);
            this.AbsoluteMovePanel.Controls.Add(this.Halt_BTN);
            this.AbsoluteMovePanel.Controls.Add(this.Park_BTN);
            this.AbsoluteMovePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AbsoluteMovePanel.Location = new System.Drawing.Point(3, 302);
            this.AbsoluteMovePanel.Name = "AbsoluteMovePanel";
            this.AbsoluteMovePanel.Size = new System.Drawing.Size(261, 78);
            this.AbsoluteMovePanel.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.AdjustedTargetPA_TB);
            this.groupBox2.Controls.Add(this.GoToAdjustedPA_BTN);
            this.groupBox2.Location = new System.Drawing.Point(8, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 46);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Absolute Move";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Go To PA:";
            // 
            // AdjustedTargetPA_TB
            // 
            this.AdjustedTargetPA_TB.Location = new System.Drawing.Point(70, 17);
            this.AdjustedTargetPA_TB.Name = "AdjustedTargetPA_TB";
            this.AdjustedTargetPA_TB.Size = new System.Drawing.Size(66, 20);
            this.AdjustedTargetPA_TB.TabIndex = 21;
            this.AdjustedTargetPA_TB.Click += new System.EventHandler(this.AdjustedTargetPA_TB_Click);
            this.AdjustedTargetPA_TB.Enter += new System.EventHandler(this.AdjustedTargetPA_TB_Enter);
            this.AdjustedTargetPA_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AdjustedTargetPA_TB_KeyPress);
            // 
            // GoToAdjustedPA_BTN
            // 
            this.GoToAdjustedPA_BTN.Location = new System.Drawing.Point(152, 15);
            this.GoToAdjustedPA_BTN.Name = "GoToAdjustedPA_BTN";
            this.GoToAdjustedPA_BTN.Size = new System.Drawing.Size(64, 23);
            this.GoToAdjustedPA_BTN.TabIndex = 22;
            this.GoToAdjustedPA_BTN.Text = "Go";
            this.GoToAdjustedPA_BTN.UseVisualStyleBackColor = true;
            this.GoToAdjustedPA_BTN.Click += new System.EventHandler(this.GoToAdjustedPA_BTN_Click);
            // 
            // Home_Btn
            // 
            this.Home_Btn.Location = new System.Drawing.Point(19, 53);
            this.Home_Btn.Name = "Home_Btn";
            this.Home_Btn.Size = new System.Drawing.Size(52, 23);
            this.Home_Btn.TabIndex = 32;
            this.Home_Btn.Text = "Home";
            this.Home_Btn.UseVisualStyleBackColor = true;
            this.Home_Btn.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // Halt_BTN
            // 
            this.Halt_BTN.Location = new System.Drawing.Point(169, 53);
            this.Halt_BTN.Name = "Halt_BTN";
            this.Halt_BTN.Size = new System.Drawing.Size(75, 23);
            this.Halt_BTN.TabIndex = 40;
            this.Halt_BTN.Text = "Halt";
            this.Halt_BTN.UseVisualStyleBackColor = true;
            this.Halt_BTN.Click += new System.EventHandler(this.Halt_BTN_Click);
            // 
            // Park_BTN
            // 
            this.Park_BTN.Location = new System.Drawing.Point(95, 53);
            this.Park_BTN.Name = "Park_BTN";
            this.Park_BTN.Size = new System.Drawing.Size(52, 23);
            this.Park_BTN.TabIndex = 33;
            this.Park_BTN.Text = "Park";
            this.Park_BTN.UseVisualStyleBackColor = true;
            this.Park_BTN.Click += new System.EventHandler(this.Park_BTN_Click);
            // 
            // RotatorDiagram_Panel
            // 
            this.RotatorDiagram_Panel.Controls.Add(this.RotatorDiagram);
            this.RotatorDiagram_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RotatorDiagram_Panel.Location = new System.Drawing.Point(3, 58);
            this.RotatorDiagram_Panel.Name = "RotatorDiagram_Panel";
            this.RotatorDiagram_Panel.Size = new System.Drawing.Size(261, 238);
            this.RotatorDiagram_Panel.TabIndex = 1;
            // 
            // RotatorDiagram
            // 
            this.RotatorDiagram.Cursor = System.Windows.Forms.Cursors.Cross;
            this.RotatorDiagram.Image = global::Pyxis_Rotator_Control.Properties.Resources.Rotator_FWD;
            this.RotatorDiagram.Location = new System.Drawing.Point(6, -6);
            this.RotatorDiagram.Name = "RotatorDiagram";
            this.RotatorDiagram.Size = new System.Drawing.Size(250, 250);
            this.RotatorDiagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RotatorDiagram.TabIndex = 0;
            this.RotatorDiagram.TabStop = false;
            this.RotatorDiagram.Click += new System.EventHandler(this.RotatorDiagram_Click);
            this.RotatorDiagram.Paint += new System.Windows.Forms.PaintEventHandler(this.RotatorDiagram_Paint);
            // 
            // SkyPA_Panel
            // 
            this.SkyPA_Panel.Controls.Add(this.PowerLight_Pic);
            this.SkyPA_Panel.Controls.Add(this.label1);
            this.SkyPA_Panel.Controls.Add(this.SetSkyPA_BTN);
            this.SkyPA_Panel.Controls.Add(this.CurrentPosition_LBL);
            this.SkyPA_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SkyPA_Panel.Location = new System.Drawing.Point(3, 3);
            this.SkyPA_Panel.Name = "SkyPA_Panel";
            this.SkyPA_Panel.Size = new System.Drawing.Size(261, 49);
            this.SkyPA_Panel.TabIndex = 0;
            // 
            // PowerLight_Pic
            // 
            this.PowerLight_Pic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PowerLight_Pic.Location = new System.Drawing.Point(8, 13);
            this.PowerLight_Pic.Name = "PowerLight_Pic";
            this.PowerLight_Pic.Size = new System.Drawing.Size(20, 20);
            this.PowerLight_Pic.TabIndex = 37;
            this.PowerLight_Pic.TabStop = false;
            this.PowerLight_Pic.Click += new System.EventHandler(this.PowerLight_Pic_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Sky PA:";
            // 
            // SetSkyPA_BTN
            // 
            this.SetSkyPA_BTN.Location = new System.Drawing.Point(137, 13);
            this.SetSkyPA_BTN.Name = "SetSkyPA_BTN";
            this.SetSkyPA_BTN.Size = new System.Drawing.Size(123, 23);
            this.SetSkyPA_BTN.TabIndex = 36;
            this.SetSkyPA_BTN.Text = "Set Current Sky PA...";
            this.SetSkyPA_BTN.UseVisualStyleBackColor = true;
            this.SetSkyPA_BTN.Click += new System.EventHandler(this.SetSkyPA_BTN_Click);
            // 
            // CurrentPosition_LBL
            // 
            this.CurrentPosition_LBL.AutoSize = true;
            this.CurrentPosition_LBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CurrentPosition_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentPosition_LBL.Location = new System.Drawing.Point(91, 18);
            this.CurrentPosition_LBL.Name = "CurrentPosition_LBL";
            this.CurrentPosition_LBL.Size = new System.Drawing.Size(35, 15);
            this.CurrentPosition_LBL.TabIndex = 30;
            this.CurrentPosition_LBL.Text = "000°";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.SkyPA_Panel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.RotatorDiagram_Panel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AbsoluteMovePanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.RelativeMovePanel, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 244F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(267, 461);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 513);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Pyxis Rotator Control";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.RelativeMovePanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeIncrement_NUD)).EndInit();
            this.AbsoluteMovePanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.RotatorDiagram_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RotatorDiagram)).EndInit();
            this.SkyPA_Panel.ResumeLayout(false);
            this.SkyPA_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLight_Pic)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sleepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wakeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.ComponentModel.BackgroundWorker PositionUpdateBGWorker;
        private System.Windows.Forms.ToolStripMenuItem advancedSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem skyPADisplayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotatorDiagramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem absoluteMoveControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem relativeMoveControlsToolStripMenuItem;
        private System.Windows.Forms.Panel RelativeMovePanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown RelativeIncrement_NUD;
        private System.Windows.Forms.Button RelMoveFwd_BTN;
        private System.Windows.Forms.Button RelMoveBack_BTN;
        private System.Windows.Forms.Panel AbsoluteMovePanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AdjustedTargetPA_TB;
        private System.Windows.Forms.Button GoToAdjustedPA_BTN;
        private System.Windows.Forms.Button Home_Btn;
        private System.Windows.Forms.Button Halt_BTN;
        private System.Windows.Forms.Button Park_BTN;
        private System.Windows.Forms.Panel RotatorDiagram_Panel;
        private System.Windows.Forms.PictureBox RotatorDiagram;
        private System.Windows.Forms.Panel SkyPA_Panel;
        private System.Windows.Forms.PictureBox PowerLight_Pic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SetSkyPA_BTN;
        private System.Windows.Forms.Label CurrentPosition_LBL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instancesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem addEditRemoveInstanceToolStripMenuItem;
    }
}

