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
            this.Connect_Btn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LEDOn_RB = new System.Windows.Forms.RadioButton();
            this.LED_GB = new System.Windows.Forms.GroupBox();
            this.LEDOff_RB = new System.Windows.Forms.RadioButton();
            this.Offsets_Btn = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.LED_GB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(6, 337);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 25);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "Finished";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // Connect_Btn
            // 
            this.Connect_Btn.Location = new System.Drawing.Point(6, 12);
            this.Connect_Btn.Name = "Connect_Btn";
            this.Connect_Btn.Size = new System.Drawing.Size(64, 29);
            this.Connect_Btn.TabIndex = 4;
            this.Connect_Btn.Text = "Connect";
            this.Connect_Btn.UseVisualStyleBackColor = true;
            this.Connect_Btn.Click += new System.EventHandler(this.Connect_Btn_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox1.Location = new System.Drawing.Point(4, 75);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(103, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.Text = "Select Position...";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
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
            this.LED_GB.Controls.Add(this.pictureBox2);
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
            // LEDOff_RB
            // 
            this.LEDOff_RB.AutoSize = true;
            this.LEDOff_RB.Location = new System.Drawing.Point(9, 40);
            this.LEDOff_RB.Name = "LEDOff_RB";
            this.LEDOff_RB.Size = new System.Drawing.Size(39, 17);
            this.LEDOff_RB.TabIndex = 9;
            this.LEDOff_RB.TabStop = true;
            this.LEDOff_RB.Text = "Off";
            this.LEDOff_RB.UseVisualStyleBackColor = true;
            this.LEDOff_RB.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // Offsets_Btn
            // 
            this.Offsets_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Offsets_Btn.Location = new System.Drawing.Point(6, 306);
            this.Offsets_Btn.Name = "Offsets_Btn";
            this.Offsets_Btn.Size = new System.Drawing.Size(105, 25);
            this.Offsets_Btn.TabIndex = 10;
            this.Offsets_Btn.Text = "Offsets...";
            this.Offsets_Btn.UseVisualStyleBackColor = true;
            this.Offsets_Btn.Click += new System.EventHandler(this.Offsets_Btn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ASCOM.Optec.Properties.Resources.LEDOff;
            this.pictureBox2.Location = new System.Drawing.Point(53, 23);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(46, 34);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.Optec.Properties.Resources.Rotator1;
            this.pictureBox1.Location = new System.Drawing.Point(6, 102);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 109);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Optec.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(76, 1);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(35, 64);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(117, 367);
            this.Controls.Add(this.Offsets_Btn);
            this.Controls.Add(this.LED_GB);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Connect_Btn);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec Setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.LED_GB.ResumeLayout(false);
            this.LED_GB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Button Connect_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton LEDOn_RB;
        private System.Windows.Forms.GroupBox LED_GB;
        private System.Windows.Forms.RadioButton LEDOff_RB;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button Offsets_Btn;
	}
}