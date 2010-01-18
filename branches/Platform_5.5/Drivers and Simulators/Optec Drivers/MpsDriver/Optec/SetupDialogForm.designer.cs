namespace ASCOM.Optec
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
            this.CurrentPosition_CB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LEDOn_RB = new System.Windows.Forms.RadioButton();
            this.LED_GB = new System.Windows.Forms.GroupBox();
            this.LEDPicture = new System.Windows.Forms.PictureBox();
            this.LEDOff_RB = new System.Windows.Forms.RadioButton();
            this.PortPicture = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupOffsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LED_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LEDPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(7, 297);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 25);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "Finished";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // CurrentPosition_CB
            // 
            this.CurrentPosition_CB.Enabled = false;
            this.CurrentPosition_CB.FormattingEnabled = true;
            this.CurrentPosition_CB.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.CurrentPosition_CB.Location = new System.Drawing.Point(4, 75);
            this.CurrentPosition_CB.Name = "CurrentPosition_CB";
            this.CurrentPosition_CB.Size = new System.Drawing.Size(103, 21);
            this.CurrentPosition_CB.TabIndex = 6;
            this.CurrentPosition_CB.Text = "Select Position...";
            this.CurrentPosition_CB.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Active Port:";
            // 
            // LEDOn_RB
            // 
            this.LEDOn_RB.AutoSize = true;
            this.LEDOn_RB.Enabled = false;
            this.LEDOn_RB.Location = new System.Drawing.Point(9, 17);
            this.LEDOn_RB.Name = "LEDOn_RB";
            this.LEDOn_RB.Size = new System.Drawing.Size(39, 17);
            this.LEDOn_RB.TabIndex = 8;
            this.LEDOn_RB.TabStop = true;
            this.LEDOn_RB.Text = "On";
            this.LEDOn_RB.UseVisualStyleBackColor = true;
            this.LEDOn_RB.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // LED_GB
            // 
            this.LED_GB.Controls.Add(this.LEDPicture);
            this.LED_GB.Controls.Add(this.LEDOff_RB);
            this.LED_GB.Controls.Add(this.LEDOn_RB);
            this.LED_GB.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LED_GB.Location = new System.Drawing.Point(6, 217);
            this.LED_GB.Name = "LED_GB";
            this.LED_GB.Size = new System.Drawing.Size(105, 75);
            this.LED_GB.TabIndex = 9;
            this.LED_GB.TabStop = false;
            this.LED_GB.Text = "LED";
            this.LED_GB.UseCompatibleTextRendering = true;
            // 
            // LEDPicture
            // 
            this.LEDPicture.Enabled = false;
            this.LEDPicture.Image = global::ASCOM.Optec.Properties.Resources.LEDOff;
            this.LEDPicture.Location = new System.Drawing.Point(53, 23);
            this.LEDPicture.Name = "LEDPicture";
            this.LEDPicture.Size = new System.Drawing.Size(46, 34);
            this.LEDPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LEDPicture.TabIndex = 10;
            this.LEDPicture.TabStop = false;
            // 
            // LEDOff_RB
            // 
            this.LEDOff_RB.AutoSize = true;
            this.LEDOff_RB.Enabled = false;
            this.LEDOff_RB.Location = new System.Drawing.Point(9, 40);
            this.LEDOff_RB.Name = "LEDOff_RB";
            this.LEDOff_RB.Size = new System.Drawing.Size(39, 17);
            this.LEDOff_RB.TabIndex = 9;
            this.LEDOff_RB.TabStop = true;
            this.LEDOff_RB.Text = "Off";
            this.LEDOff_RB.UseVisualStyleBackColor = true;
            this.LEDOff_RB.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // PortPicture
            // 
            this.PortPicture.Enabled = false;
            this.PortPicture.Image = global::ASCOM.Optec.Properties.Resources.Rotator1;
            this.PortPicture.Location = new System.Drawing.Point(6, 102);
            this.PortPicture.Name = "PortPicture";
            this.PortPicture.Size = new System.Drawing.Size(103, 109);
            this.PortPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PortPicture.TabIndex = 5;
            this.PortPicture.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Optec.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(77, 27);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(35, 42);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(118, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // operationsToolStripMenuItem
            // 
            this.operationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.setupOffsetsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.selectCOMPortToolStripMenuItem});
            this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
            this.operationsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.operationsToolStripMenuItem.Text = "File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            // 
            // setupOffsetsToolStripMenuItem
            // 
            this.setupOffsetsToolStripMenuItem.Enabled = false;
            this.setupOffsetsToolStripMenuItem.Name = "setupOffsetsToolStripMenuItem";
            this.setupOffsetsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.setupOffsetsToolStripMenuItem.Text = "Setup Offsets";
            this.setupOffsetsToolStripMenuItem.Click += new System.EventHandler(this.SetupOffsets);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // selectCOMPortToolStripMenuItem
            // 
            this.selectCOMPortToolStripMenuItem.Name = "selectCOMPortToolStripMenuItem";
            this.selectCOMPortToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.selectCOMPortToolStripMenuItem.Text = "Select COM Port";
            this.selectCOMPortToolStripMenuItem.Click += new System.EventHandler(this.selectCOMPortToolStripMenuItem_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(118, 327);
            this.Controls.Add(this.LED_GB);
            this.Controls.Add(this.CurrentPosition_CB);
            this.Controls.Add(this.PortPicture);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec Setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            this.LED_GB.ResumeLayout(false);
            this.LED_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LEDPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.PictureBox PortPicture;
        private System.Windows.Forms.ComboBox CurrentPosition_CB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton LEDOn_RB;
        private System.Windows.Forms.GroupBox LED_GB;
        private System.Windows.Forms.RadioButton LEDOff_RB;
        private System.Windows.Forms.PictureBox LEDPicture;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupOffsetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCOMPortToolStripMenuItem;
	}
}