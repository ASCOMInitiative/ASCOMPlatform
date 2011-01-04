namespace ASCOM.Pyxis
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
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
            this.instancesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whatsThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.instance1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pyxis2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentPosition_LBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Home_Btn = new System.Windows.Forms.Button();
            this.Park_BTN = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SetSkyPA_BTN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.AdjustedTargetPA_TB = new System.Windows.Forms.TextBox();
            this.GoToAdjustedPA_BTN = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.Wake_BTN = new System.Windows.Forms.Button();
            this.Sleep_BTN = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Halt_BTN = new System.Windows.Forms.Button();
            this.PositionUpdateBGWorker = new System.ComponentModel.BackgroundWorker();
            this.RelMoveBack_BTN = new System.Windows.Forms.Button();
            this.RelMoveFwd_BTN = new System.Windows.Forms.Button();
            this.RelativeIncrement_NUD = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeIncrement_NUD)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(246, 518);
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
            this.cmdCancel.Location = new System.Drawing.Point(166, 518);
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
            this.fileToolStripMenuItem,
            this.deviceToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.instancesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(314, 24);
            this.menuStrip1.TabIndex = 4;
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
            this.parkToolStripMenuItem.Click += new System.EventHandler(this.Park_BTN_Click);
            // 
            // sleepToolStripMenuItem
            // 
            this.sleepToolStripMenuItem.Name = "sleepToolStripMenuItem";
            this.sleepToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.sleepToolStripMenuItem.Text = "Sleep";
            this.sleepToolStripMenuItem.Click += new System.EventHandler(this.Sleep_BTN_Click);
            // 
            // wakeToolStripMenuItem
            // 
            this.wakeToolStripMenuItem.Name = "wakeToolStripMenuItem";
            this.wakeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.wakeToolStripMenuItem.Text = "Wake";
            this.wakeToolStripMenuItem.Click += new System.EventHandler(this.Wake_BTN_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMPortToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // cOMPortToolStripMenuItem
            // 
            this.cOMPortToolStripMenuItem.Name = "cOMPortToolStripMenuItem";
            this.cOMPortToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.cOMPortToolStripMenuItem.Text = "COM Port";
            this.cOMPortToolStripMenuItem.MouseEnter += new System.EventHandler(this.cOMPortToolStripMenuItem_MouseEnter);
            // 
            // instancesToolStripMenuItem
            // 
            this.instancesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatsThisToolStripMenuItem,
            this.createNewToolStripMenuItem,
            this.editNameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.instance1ToolStripMenuItem,
            this.pyxis2ToolStripMenuItem});
            this.instancesToolStripMenuItem.Name = "instancesToolStripMenuItem";
            this.instancesToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.instancesToolStripMenuItem.Text = "Instances";
            this.instancesToolStripMenuItem.Visible = false;
            // 
            // whatsThisToolStripMenuItem
            // 
            this.whatsThisToolStripMenuItem.Name = "whatsThisToolStripMenuItem";
            this.whatsThisToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.whatsThisToolStripMenuItem.Text = "What\'s This?";
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.createNewToolStripMenuItem.Text = "Create new...";
            // 
            // editNameToolStripMenuItem
            // 
            this.editNameToolStripMenuItem.Name = "editNameToolStripMenuItem";
            this.editNameToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.editNameToolStripMenuItem.Text = "Edit name...";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.deleteToolStripMenuItem.Text = "Delete...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // instance1ToolStripMenuItem
            // 
            this.instance1ToolStripMenuItem.Name = "instance1ToolStripMenuItem";
            this.instance1ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.instance1ToolStripMenuItem.Text = "Pyxis 1";
            // 
            // pyxis2ToolStripMenuItem
            // 
            this.pyxis2ToolStripMenuItem.Name = "pyxis2ToolStripMenuItem";
            this.pyxis2ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.pyxis2ToolStripMenuItem.Text = "Pyxis 2";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.documentationToolStripMenuItem,
            this.checkForUpdateToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // CurrentPosition_LBL
            // 
            this.CurrentPosition_LBL.AutoSize = true;
            this.CurrentPosition_LBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CurrentPosition_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentPosition_LBL.Location = new System.Drawing.Point(71, 37);
            this.CurrentPosition_LBL.Name = "CurrentPosition_LBL";
            this.CurrentPosition_LBL.Size = new System.Drawing.Size(35, 15);
            this.CurrentPosition_LBL.TabIndex = 5;
            this.CurrentPosition_LBL.Text = "000°";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sky PA:";
            // 
            // Home_Btn
            // 
            this.Home_Btn.Location = new System.Drawing.Point(20, 182);
            this.Home_Btn.Name = "Home_Btn";
            this.Home_Btn.Size = new System.Drawing.Size(52, 23);
            this.Home_Btn.TabIndex = 10;
            this.Home_Btn.Text = "Home";
            this.Home_Btn.UseVisualStyleBackColor = true;
            this.Home_Btn.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // Park_BTN
            // 
            this.Park_BTN.Location = new System.Drawing.Point(97, 182);
            this.Park_BTN.Name = "Park_BTN";
            this.Park_BTN.Size = new System.Drawing.Size(52, 23);
            this.Park_BTN.TabIndex = 11;
            this.Park_BTN.Text = "Park";
            this.Park_BTN.UseVisualStyleBackColor = true;
            this.Park_BTN.Click += new System.EventHandler(this.Park_BTN_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 546);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(314, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(77, 17);
            this.StatusLabel.Text = "Device Status";
            // 
            // SetSkyPA_BTN
            // 
            this.SetSkyPA_BTN.Location = new System.Drawing.Point(128, 35);
            this.SetSkyPA_BTN.Name = "SetSkyPA_BTN";
            this.SetSkyPA_BTN.Size = new System.Drawing.Size(129, 23);
            this.SetSkyPA_BTN.TabIndex = 17;
            this.SetSkyPA_BTN.Text = "Set Current Sky PA...";
            this.SetSkyPA_BTN.UseVisualStyleBackColor = true;
            this.SetSkyPA_BTN.Click += new System.EventHandler(this.SetSkyPA_BTN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Go To PA:";
            // 
            // AdjustedTargetPA_TB
            // 
            this.AdjustedTargetPA_TB.Location = new System.Drawing.Point(100, 17);
            this.AdjustedTargetPA_TB.Name = "AdjustedTargetPA_TB";
            this.AdjustedTargetPA_TB.Size = new System.Drawing.Size(66, 20);
            this.AdjustedTargetPA_TB.TabIndex = 21;
            this.AdjustedTargetPA_TB.Click += new System.EventHandler(this.AdjustedTargetPA_TB_Click);
            this.AdjustedTargetPA_TB.Enter += new System.EventHandler(this.AdjustedTargetPA_TB_Enter);
            this.AdjustedTargetPA_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AdjustedTargetPA_TB_KeyPress);
            // 
            // GoToAdjustedPA_BTN
            // 
            this.GoToAdjustedPA_BTN.Location = new System.Drawing.Point(182, 15);
            this.GoToAdjustedPA_BTN.Name = "GoToAdjustedPA_BTN";
            this.GoToAdjustedPA_BTN.Size = new System.Drawing.Size(64, 23);
            this.GoToAdjustedPA_BTN.TabIndex = 22;
            this.GoToAdjustedPA_BTN.Text = "Go";
            this.GoToAdjustedPA_BTN.UseVisualStyleBackColor = true;
            this.GoToAdjustedPA_BTN.Click += new System.EventHandler(this.GoToAdjustedPA_BTN_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(20, 248);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(285, 263);
            this.propertyGrid1.TabIndex = 25;
            // 
            // Wake_BTN
            // 
            this.Wake_BTN.Location = new System.Drawing.Point(251, 182);
            this.Wake_BTN.Name = "Wake_BTN";
            this.Wake_BTN.Size = new System.Drawing.Size(52, 23);
            this.Wake_BTN.TabIndex = 13;
            this.Wake_BTN.Text = "Wake";
            this.Wake_BTN.UseVisualStyleBackColor = true;
            this.Wake_BTN.Click += new System.EventHandler(this.Wake_BTN_Click);
            // 
            // Sleep_BTN
            // 
            this.Sleep_BTN.Location = new System.Drawing.Point(174, 182);
            this.Sleep_BTN.Name = "Sleep_BTN";
            this.Sleep_BTN.Size = new System.Drawing.Size(52, 23);
            this.Sleep_BTN.TabIndex = 12;
            this.Sleep_BTN.Text = "Sleep";
            this.Sleep_BTN.UseVisualStyleBackColor = true;
            this.Sleep_BTN.Click += new System.EventHandler(this.Sleep_BTN_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.AdjustedTargetPA_TB);
            this.groupBox2.Controls.Add(this.GoToAdjustedPA_BTN);
            this.groupBox2.Location = new System.Drawing.Point(20, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 46);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Absolute Move";
            // 
            // Halt_BTN
            // 
            this.Halt_BTN.Location = new System.Drawing.Point(120, 221);
            this.Halt_BTN.Name = "Halt_BTN";
            this.Halt_BTN.Size = new System.Drawing.Size(75, 23);
            this.Halt_BTN.TabIndex = 29;
            this.Halt_BTN.Text = "Halt";
            this.Halt_BTN.UseVisualStyleBackColor = true;
            this.Halt_BTN.Click += new System.EventHandler(this.Halt_BTN_Click);
            // 
            // PositionUpdateBGWorker
            // 
            this.PositionUpdateBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PositionUpdateBGWorker_DoWork);
            // 
            // RelMoveBack_BTN
            // 
            this.RelMoveBack_BTN.Location = new System.Drawing.Point(27, 26);
            this.RelMoveBack_BTN.Name = "RelMoveBack_BTN";
            this.RelMoveBack_BTN.Size = new System.Drawing.Size(63, 23);
            this.RelMoveBack_BTN.TabIndex = 26;
            this.RelMoveBack_BTN.Text = "<<";
            this.RelMoveBack_BTN.UseVisualStyleBackColor = true;
            this.RelMoveBack_BTN.Click += new System.EventHandler(this.RelMoveBack_BTN_Click);
            // 
            // RelMoveFwd_BTN
            // 
            this.RelMoveFwd_BTN.Location = new System.Drawing.Point(192, 26);
            this.RelMoveFwd_BTN.Name = "RelMoveFwd_BTN";
            this.RelMoveFwd_BTN.Size = new System.Drawing.Size(63, 23);
            this.RelMoveFwd_BTN.TabIndex = 27;
            this.RelMoveFwd_BTN.Text = ">>";
            this.RelMoveFwd_BTN.UseVisualStyleBackColor = true;
            this.RelMoveFwd_BTN.Click += new System.EventHandler(this.RelMoveFwd_BTN_Click);
            // 
            // RelativeIncrement_NUD
            // 
            this.RelativeIncrement_NUD.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.RelativeIncrement_NUD.Location = new System.Drawing.Point(108, 27);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Increment";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.RelativeIncrement_NUD);
            this.groupBox1.Controls.Add(this.RelMoveFwd_BTN);
            this.groupBox1.Controls.Add(this.RelMoveBack_BTN);
            this.groupBox1.Location = new System.Drawing.Point(20, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 57);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Relative Move";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Pyxis.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(266, 0);
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
            this.ClientSize = new System.Drawing.Size(314, 568);
            this.Controls.Add(this.Halt_BTN);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CurrentPosition_LBL);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.SetSkyPA_BTN);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Wake_BTN);
            this.Controls.Add(this.Sleep_BTN);
            this.Controls.Add(this.Park_BTN);
            this.Controls.Add(this.Home_Btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pyxis Setup";
            this.Shown += new System.EventHandler(this.SetupDialogForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelativeIncrement_NUD)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMPortToolStripMenuItem;
        private System.Windows.Forms.Label CurrentPosition_LBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parkToolStripMenuItem;
        private System.Windows.Forms.Button Home_Btn;
        private System.Windows.Forms.Button Park_BTN;
        private System.Windows.Forms.ToolStripMenuItem sleepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wakeToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button SetSkyPA_BTN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AdjustedTargetPA_TB;
        private System.Windows.Forms.Button GoToAdjustedPA_BTN;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button Wake_BTN;
        private System.Windows.Forms.Button Sleep_BTN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Halt_BTN;
        private System.ComponentModel.BackgroundWorker PositionUpdateBGWorker;
        private System.Windows.Forms.Button RelMoveBack_BTN;
        private System.Windows.Forms.Button RelMoveFwd_BTN;
        private System.Windows.Forms.NumericUpDown RelativeIncrement_NUD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instancesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem instance1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pyxis2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whatsThisToolStripMenuItem;
    }
}