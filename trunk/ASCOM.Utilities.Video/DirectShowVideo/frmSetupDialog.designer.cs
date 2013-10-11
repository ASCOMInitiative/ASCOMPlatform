namespace ASCOM.Utilities.Video.DirectShowVideo
{
	partial class frmSetupDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupDialog));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.picASCOM = new System.Windows.Forms.PictureBox();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblVersion = new System.Windows.Forms.Label();
			this.ucDirectShowVideoSettings = new ASCOM.Utilities.Video.DirectShowVideo.ucDirectShowVideoSettings();
			((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(16, 369);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(120, 24);
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(142, 368);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(120, 25);
			this.cmdCancel.TabIndex = 1;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// picASCOM
			// 
			this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picASCOM.Image = ((System.Drawing.Image)(resources.GetObject("picASCOM.Image")));
			this.picASCOM.Location = new System.Drawing.Point(437, 7);
			this.picASCOM.Name = "picASCOM";
			this.picASCOM.Size = new System.Drawing.Size(48, 56);
			this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picASCOM.TabIndex = 3;
			this.picASCOM.TabStop = false;
			this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
			this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.WindowText;
			this.panel1.Location = new System.Drawing.Point(16, 359);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(468, 1);
			this.panel1.TabIndex = 29;
			// 
			// lblVersion
			// 
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(337, 368);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(148, 23);
			this.lblVersion.TabIndex = 30;
			this.lblVersion.Text = "v1.0.0";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ucDirectShowVideoSettings
			// 
			this.ucDirectShowVideoSettings.Location = new System.Drawing.Point(10, 10);
			this.ucDirectShowVideoSettings.Name = "ucDirectShowVideoSettings";
			this.ucDirectShowVideoSettings.Size = new System.Drawing.Size(481, 346);
			this.ucDirectShowVideoSettings.TabIndex = 31;
			// 
			// frmSetupDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 408);
			this.Controls.Add(this.picASCOM);
			this.Controls.Add(this.ucDirectShowVideoSettings);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSetupDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Video Capture Setup";
			this.Load += new System.EventHandler(this.frmSetupDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.PictureBox picASCOM;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblVersion;
		private ucDirectShowVideoSettings ucDirectShowVideoSettings;
	}
}