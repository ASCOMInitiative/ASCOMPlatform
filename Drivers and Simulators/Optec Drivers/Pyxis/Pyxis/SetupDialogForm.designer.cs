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
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sleepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentPosition_LBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TargetPA_TB = new System.Windows.Forms.TextBox();
            this.GoToPA_BTN = new System.Windows.Forms.Button();
            this.Home_Btn = new System.Windows.Forms.Button();
            this.Park_BTN = new System.Windows.Forms.Button();
            this.Sleep_BTN = new System.Windows.Forms.Button();
            this.Wake_BTN = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ChangeDirection_BTN = new System.Windows.Forms.Button();
            this.StepRate_BTN = new System.Windows.Forms.Button();
            this.SetSkyPA_BTN = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AdjustedPosition_LBL = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AdjustedTargetPA_TB = new System.Windows.Forms.TextBox();
            this.GoToAdjustedPA_BTN = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.FirmwareVer_LBL = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(205, 376);
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
            this.cmdCancel.Location = new System.Drawing.Point(125, 376);
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
            this.setupToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(273, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
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
            // CurrentPosition_LBL
            // 
            this.CurrentPosition_LBL.AutoSize = true;
            this.CurrentPosition_LBL.Location = new System.Drawing.Point(102, 33);
            this.CurrentPosition_LBL.Name = "CurrentPosition_LBL";
            this.CurrentPosition_LBL.Size = new System.Drawing.Size(13, 13);
            this.CurrentPosition_LBL.TabIndex = 5;
            this.CurrentPosition_LBL.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Current Position:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Go To PA:";
            // 
            // TargetPA_TB
            // 
            this.TargetPA_TB.Location = new System.Drawing.Point(75, 120);
            this.TargetPA_TB.Name = "TargetPA_TB";
            this.TargetPA_TB.Size = new System.Drawing.Size(52, 20);
            this.TargetPA_TB.TabIndex = 8;
            this.TargetPA_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetPA_TB_KeyPress);
            // 
            // GoToPA_BTN
            // 
            this.GoToPA_BTN.Location = new System.Drawing.Point(137, 118);
            this.GoToPA_BTN.Name = "GoToPA_BTN";
            this.GoToPA_BTN.Size = new System.Drawing.Size(37, 23);
            this.GoToPA_BTN.TabIndex = 9;
            this.GoToPA_BTN.Text = "Go";
            this.GoToPA_BTN.UseVisualStyleBackColor = true;
            this.GoToPA_BTN.Click += new System.EventHandler(this.GoToPA_BTN_Click);
            // 
            // Home_Btn
            // 
            this.Home_Btn.Location = new System.Drawing.Point(15, 220);
            this.Home_Btn.Name = "Home_Btn";
            this.Home_Btn.Size = new System.Drawing.Size(52, 23);
            this.Home_Btn.TabIndex = 10;
            this.Home_Btn.Text = "Home";
            this.Home_Btn.UseVisualStyleBackColor = true;
            this.Home_Btn.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // Park_BTN
            // 
            this.Park_BTN.Location = new System.Drawing.Point(74, 220);
            this.Park_BTN.Name = "Park_BTN";
            this.Park_BTN.Size = new System.Drawing.Size(52, 23);
            this.Park_BTN.TabIndex = 11;
            this.Park_BTN.Text = "Park";
            this.Park_BTN.UseVisualStyleBackColor = true;
            this.Park_BTN.Click += new System.EventHandler(this.Park_BTN_Click);
            // 
            // Sleep_BTN
            // 
            this.Sleep_BTN.Location = new System.Drawing.Point(133, 220);
            this.Sleep_BTN.Name = "Sleep_BTN";
            this.Sleep_BTN.Size = new System.Drawing.Size(52, 23);
            this.Sleep_BTN.TabIndex = 12;
            this.Sleep_BTN.Text = "Sleep";
            this.Sleep_BTN.UseVisualStyleBackColor = true;
            this.Sleep_BTN.Click += new System.EventHandler(this.Sleep_BTN_Click);
            // 
            // Wake_BTN
            // 
            this.Wake_BTN.Location = new System.Drawing.Point(192, 220);
            this.Wake_BTN.Name = "Wake_BTN";
            this.Wake_BTN.Size = new System.Drawing.Size(52, 23);
            this.Wake_BTN.TabIndex = 13;
            this.Wake_BTN.Text = "Wake";
            this.Wake_BTN.UseVisualStyleBackColor = true;
            this.Wake_BTN.Click += new System.EventHandler(this.Wake_BTN_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 404);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(273, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(77, 17);
            this.StatusLabel.Text = "Device Status";
            // 
            // ChangeDirection_BTN
            // 
            this.ChangeDirection_BTN.Location = new System.Drawing.Point(15, 260);
            this.ChangeDirection_BTN.Name = "ChangeDirection_BTN";
            this.ChangeDirection_BTN.Size = new System.Drawing.Size(111, 23);
            this.ChangeDirection_BTN.TabIndex = 15;
            this.ChangeDirection_BTN.Text = "Change Direction...";
            this.ChangeDirection_BTN.UseVisualStyleBackColor = true;
            this.ChangeDirection_BTN.Click += new System.EventHandler(this.ChangeDirectionBtn_Click);
            // 
            // StepRate_BTN
            // 
            this.StepRate_BTN.Location = new System.Drawing.Point(131, 260);
            this.StepRate_BTN.Name = "StepRate_BTN";
            this.StepRate_BTN.Size = new System.Drawing.Size(119, 23);
            this.StepRate_BTN.TabIndex = 16;
            this.StepRate_BTN.Text = "Change Step Rate...";
            this.StepRate_BTN.UseVisualStyleBackColor = true;
            this.StepRate_BTN.Click += new System.EventHandler(this.StepRate_BTN_Click);
            // 
            // SetSkyPA_BTN
            // 
            this.SetSkyPA_BTN.Location = new System.Drawing.Point(15, 74);
            this.SetSkyPA_BTN.Name = "SetSkyPA_BTN";
            this.SetSkyPA_BTN.Size = new System.Drawing.Size(138, 23);
            this.SetSkyPA_BTN.TabIndex = 17;
            this.SetSkyPA_BTN.Text = "Set Current Sky PA...";
            this.SetSkyPA_BTN.UseVisualStyleBackColor = true;
            this.SetSkyPA_BTN.Click += new System.EventHandler(this.SetSkyPA_BTN_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Pyxis.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(224, 1);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Adjusted Position:";
            // 
            // AdjustedPosition_LBL
            // 
            this.AdjustedPosition_LBL.AutoSize = true;
            this.AdjustedPosition_LBL.Location = new System.Drawing.Point(109, 52);
            this.AdjustedPosition_LBL.Name = "AdjustedPosition_LBL";
            this.AdjustedPosition_LBL.Size = new System.Drawing.Size(13, 13);
            this.AdjustedPosition_LBL.TabIndex = 19;
            this.AdjustedPosition_LBL.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Go To Adjusted PA:";
            // 
            // AdjustedTargetPA_TB
            // 
            this.AdjustedTargetPA_TB.Location = new System.Drawing.Point(122, 160);
            this.AdjustedTargetPA_TB.Name = "AdjustedTargetPA_TB";
            this.AdjustedTargetPA_TB.Size = new System.Drawing.Size(52, 20);
            this.AdjustedTargetPA_TB.TabIndex = 21;
            // 
            // GoToAdjustedPA_BTN
            // 
            this.GoToAdjustedPA_BTN.Location = new System.Drawing.Point(192, 158);
            this.GoToAdjustedPA_BTN.Name = "GoToAdjustedPA_BTN";
            this.GoToAdjustedPA_BTN.Size = new System.Drawing.Size(37, 23);
            this.GoToAdjustedPA_BTN.TabIndex = 22;
            this.GoToAdjustedPA_BTN.Text = "Go";
            this.GoToAdjustedPA_BTN.UseVisualStyleBackColor = true;
            this.GoToAdjustedPA_BTN.Click += new System.EventHandler(this.GoToAdjustedPA_BTN_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 320);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Firmware Version: ";
            // 
            // FirmwareVer_LBL
            // 
            this.FirmwareVer_LBL.AutoSize = true;
            this.FirmwareVer_LBL.Location = new System.Drawing.Point(102, 320);
            this.FirmwareVer_LBL.Name = "FirmwareVer_LBL";
            this.FirmwareVer_LBL.Size = new System.Drawing.Size(19, 13);
            this.FirmwareVer_LBL.TabIndex = 24;
            this.FirmwareVer_LBL.Text = "??";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 426);
            this.Controls.Add(this.FirmwareVer_LBL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GoToAdjustedPA_BTN);
            this.Controls.Add(this.AdjustedTargetPA_TB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AdjustedPosition_LBL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SetSkyPA_BTN);
            this.Controls.Add(this.StepRate_BTN);
            this.Controls.Add(this.ChangeDirection_BTN);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Wake_BTN);
            this.Controls.Add(this.Sleep_BTN);
            this.Controls.Add(this.Park_BTN);
            this.Controls.Add(this.Home_Btn);
            this.Controls.Add(this.GoToPA_BTN);
            this.Controls.Add(this.TargetPA_TB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentPosition_LBL);
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
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.Shown += new System.EventHandler(this.SetupDialogForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TargetPA_TB;
        private System.Windows.Forms.Button GoToPA_BTN;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parkToolStripMenuItem;
        private System.Windows.Forms.Button Home_Btn;
        private System.Windows.Forms.Button Park_BTN;
        private System.Windows.Forms.ToolStripMenuItem sleepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wakeToolStripMenuItem;
        private System.Windows.Forms.Button Sleep_BTN;
        private System.Windows.Forms.Button Wake_BTN;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Button ChangeDirection_BTN;
        private System.Windows.Forms.Button StepRate_BTN;
        private System.Windows.Forms.Button SetSkyPA_BTN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label AdjustedPosition_LBL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AdjustedTargetPA_TB;
        private System.Windows.Forms.Button GoToAdjustedPA_BTN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label FirmwareVer_LBL;
    }
}