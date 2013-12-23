namespace ASCOM.Simulator.Config
{
	partial class ucVideoSource
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.pnlBufferingControls = new System.Windows.Forms.Panel();
			this.lblBuff2 = new System.Windows.Forms.Label();
			this.nudBufferSize = new System.Windows.Forms.NumericUpDown();
			this.lblBuff1 = new System.Windows.Forms.Label();
			this.lblBmpLocation = new System.Windows.Forms.Label();
			this.cbxBuffering = new System.Windows.Forms.CheckBox();
			this.pnlUserBitmaps = new System.Windows.Forms.Panel();
			this.tbxBitmapFolder = new System.Windows.Forms.TextBox();
			this.btnBrowseForFolder = new System.Windows.Forms.Button();
			this.rbUserBitmaps = new System.Windows.Forms.RadioButton();
			this.rbDriverDefaultSource = new System.Windows.Forms.RadioButton();
			this.pnlBufferingControls.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBufferSize)).BeginInit();
			this.pnlUserBitmaps.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBufferingControls
			// 
			this.pnlBufferingControls.Controls.Add(this.lblBuff2);
			this.pnlBufferingControls.Controls.Add(this.nudBufferSize);
			this.pnlBufferingControls.Controls.Add(this.lblBuff1);
			this.pnlBufferingControls.Location = new System.Drawing.Point(7, 145);
			this.pnlBufferingControls.Name = "pnlBufferingControls";
			this.pnlBufferingControls.Size = new System.Drawing.Size(236, 39);
			this.pnlBufferingControls.TabIndex = 28;
			// 
			// lblBuff2
			// 
			this.lblBuff2.AutoSize = true;
			this.lblBuff2.Location = new System.Drawing.Point(136, 12);
			this.lblBuff2.Name = "lblBuff2";
			this.lblBuff2.Size = new System.Drawing.Size(38, 13);
			this.lblBuff2.TabIndex = 2;
			this.lblBuff2.Text = "frames";
			// 
			// nudBufferSize
			// 
			this.nudBufferSize.Enabled = false;
			this.nudBufferSize.Location = new System.Drawing.Point(75, 8);
			this.nudBufferSize.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.nudBufferSize.Name = "nudBufferSize";
			this.nudBufferSize.Size = new System.Drawing.Size(55, 20);
			this.nudBufferSize.TabIndex = 1;
			// 
			// lblBuff1
			// 
			this.lblBuff1.AutoSize = true;
			this.lblBuff1.Location = new System.Drawing.Point(14, 12);
			this.lblBuff1.Name = "lblBuff1";
			this.lblBuff1.Size = new System.Drawing.Size(59, 13);
			this.lblBuff1.TabIndex = 0;
			this.lblBuff1.Text = "Buffer size:";
			// 
			// lblBmpLocation
			// 
			this.lblBmpLocation.AutoSize = true;
			this.lblBmpLocation.Location = new System.Drawing.Point(21, 54);
			this.lblBmpLocation.Name = "lblBmpLocation";
			this.lblBmpLocation.Size = new System.Drawing.Size(255, 13);
			this.lblBmpLocation.TabIndex = 4;
			this.lblBmpLocation.Text = "Location of the BMP files to be used as video frames";
			// 
			// cbxBuffering
			// 
			this.cbxBuffering.AutoSize = true;
			this.cbxBuffering.ForeColor = System.Drawing.SystemColors.Window;
			this.cbxBuffering.Location = new System.Drawing.Point(6, 123);
			this.cbxBuffering.Name = "cbxBuffering";
			this.cbxBuffering.Size = new System.Drawing.Size(89, 17);
			this.cbxBuffering.TabIndex = 27;
			this.cbxBuffering.Text = "Use buffering";
			this.cbxBuffering.UseVisualStyleBackColor = true;
			this.cbxBuffering.CheckedChanged += new System.EventHandler(this.cbxBuffering_CheckedChanged);
			// 
			// pnlUserBitmaps
			// 
			this.pnlUserBitmaps.Controls.Add(this.tbxBitmapFolder);
			this.pnlUserBitmaps.Controls.Add(this.btnBrowseForFolder);
			this.pnlUserBitmaps.Enabled = false;
			this.pnlUserBitmaps.Location = new System.Drawing.Point(-3, 70);
			this.pnlUserBitmaps.Name = "pnlUserBitmaps";
			this.pnlUserBitmaps.Size = new System.Drawing.Size(422, 32);
			this.pnlUserBitmaps.TabIndex = 26;
			// 
			// tbxBitmapFolder
			// 
			this.tbxBitmapFolder.Location = new System.Drawing.Point(27, 6);
			this.tbxBitmapFolder.Name = "tbxBitmapFolder";
			this.tbxBitmapFolder.Size = new System.Drawing.Size(341, 20);
			this.tbxBitmapFolder.TabIndex = 5;
			// 
			// btnBrowseForFolder
			// 
			this.btnBrowseForFolder.BackColor = System.Drawing.SystemColors.Control;
			this.btnBrowseForFolder.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnBrowseForFolder.Location = new System.Drawing.Point(374, 4);
			this.btnBrowseForFolder.Name = "btnBrowseForFolder";
			this.btnBrowseForFolder.Size = new System.Drawing.Size(31, 23);
			this.btnBrowseForFolder.TabIndex = 6;
			this.btnBrowseForFolder.Text = "...";
			this.btnBrowseForFolder.UseVisualStyleBackColor = false;
			this.btnBrowseForFolder.Click += new System.EventHandler(this.btnBrowseForFolder_Click);
			// 
			// rbUserBitmaps
			// 
			this.rbUserBitmaps.AutoSize = true;
			this.rbUserBitmaps.ForeColor = System.Drawing.SystemColors.Window;
			this.rbUserBitmaps.Location = new System.Drawing.Point(6, 23);
			this.rbUserBitmaps.Name = "rbUserBitmaps";
			this.rbUserBitmaps.Size = new System.Drawing.Size(152, 17);
			this.rbUserBitmaps.TabIndex = 25;
			this.rbUserBitmaps.Text = "User supplied video frames";
			this.rbUserBitmaps.UseVisualStyleBackColor = true;
			// 
			// rbDriverDefaultSource
			// 
			this.rbDriverDefaultSource.AutoSize = true;
			this.rbDriverDefaultSource.Checked = true;
			this.rbDriverDefaultSource.ForeColor = System.Drawing.SystemColors.Window;
			this.rbDriverDefaultSource.Location = new System.Drawing.Point(6, 0);
			this.rbDriverDefaultSource.Name = "rbDriverDefaultSource";
			this.rbDriverDefaultSource.Size = new System.Drawing.Size(367, 17);
			this.rbDriverDefaultSource.TabIndex = 24;
			this.rbDriverDefaultSource.TabStop = true;
			this.rbDriverDefaultSource.Text = "Driver supplied - Occultation of TYC 6849-01777-1 by (25631) 2000AJ55";
			this.rbDriverDefaultSource.UseVisualStyleBackColor = true;
			this.rbDriverDefaultSource.CheckedChanged += new System.EventHandler(this.rbDriverDefaultSource_CheckedChanged);
			// 
			// ucVideoSource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlText;
			this.Controls.Add(this.pnlBufferingControls);
			this.Controls.Add(this.lblBmpLocation);
			this.Controls.Add(this.cbxBuffering);
			this.Controls.Add(this.pnlUserBitmaps);
			this.Controls.Add(this.rbUserBitmaps);
			this.Controls.Add(this.rbDriverDefaultSource);
			this.ForeColor = System.Drawing.SystemColors.Window;
			this.Name = "ucVideoSource";
			this.Size = new System.Drawing.Size(548, 281);
			this.pnlBufferingControls.ResumeLayout(false);
			this.pnlBufferingControls.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBufferSize)).EndInit();
			this.pnlUserBitmaps.ResumeLayout(false);
			this.pnlUserBitmaps.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Panel pnlBufferingControls;
		private System.Windows.Forms.Label lblBuff2;
		private System.Windows.Forms.NumericUpDown nudBufferSize;
		private System.Windows.Forms.Label lblBuff1;
		private System.Windows.Forms.CheckBox cbxBuffering;
		private System.Windows.Forms.Panel pnlUserBitmaps;
		private System.Windows.Forms.TextBox tbxBitmapFolder;
		private System.Windows.Forms.Label lblBmpLocation;
		private System.Windows.Forms.Button btnBrowseForFolder;
		private System.Windows.Forms.RadioButton rbUserBitmaps;
		private System.Windows.Forms.RadioButton rbDriverDefaultSource;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
	}
}
