namespace ASCOM.Simulator.Config
{
	partial class ucAnalogueCameraSettings
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
			this.lblDeviceName = new System.Windows.Forms.Label();
			this.gpxFrameRate = new System.Windows.Forms.GroupBox();
			this.rbVideoFrameRateNTSC = new System.Windows.Forms.RadioButton();
			this.rbVideoFrameRatePAL = new System.Windows.Forms.RadioButton();
			this.tbxVideoCaptureDeviceName = new System.Windows.Forms.TextBox();
			this.gpxFrameRate.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblDeviceName
			// 
			this.lblDeviceName.AutoSize = true;
			this.lblDeviceName.ForeColor = System.Drawing.SystemColors.Window;
			this.lblDeviceName.Location = new System.Drawing.Point(6, 0);
			this.lblDeviceName.Name = "lblDeviceName";
			this.lblDeviceName.Size = new System.Drawing.Size(142, 13);
			this.lblDeviceName.TabIndex = 5;
			this.lblDeviceName.Text = "Video Capture Device Name";
			// 
			// gpxFrameRate
			// 
			this.gpxFrameRate.Controls.Add(this.rbVideoFrameRateNTSC);
			this.gpxFrameRate.Controls.Add(this.rbVideoFrameRatePAL);
			this.gpxFrameRate.ForeColor = System.Drawing.SystemColors.Window;
			this.gpxFrameRate.Location = new System.Drawing.Point(9, 61);
			this.gpxFrameRate.Name = "gpxFrameRate";
			this.gpxFrameRate.Size = new System.Drawing.Size(247, 48);
			this.gpxFrameRate.TabIndex = 7;
			this.gpxFrameRate.TabStop = false;
			this.gpxFrameRate.Text = "Video Frame Rate";
			// 
			// rbVideoFrameRateNTSC
			// 
			this.rbVideoFrameRateNTSC.AutoSize = true;
			this.rbVideoFrameRateNTSC.ForeColor = System.Drawing.SystemColors.Window;
			this.rbVideoFrameRateNTSC.Location = new System.Drawing.Point(123, 20);
			this.rbVideoFrameRateNTSC.Name = "rbVideoFrameRateNTSC";
			this.rbVideoFrameRateNTSC.Size = new System.Drawing.Size(107, 17);
			this.rbVideoFrameRateNTSC.TabIndex = 1;
			this.rbVideoFrameRateNTSC.Text = "29.97 fps (NTSC)";
			this.rbVideoFrameRateNTSC.UseVisualStyleBackColor = true;
			// 
			// rbVideoFrameRatePAL
			// 
			this.rbVideoFrameRatePAL.AutoSize = true;
			this.rbVideoFrameRatePAL.Checked = true;
			this.rbVideoFrameRatePAL.ForeColor = System.Drawing.SystemColors.Window;
			this.rbVideoFrameRatePAL.Location = new System.Drawing.Point(16, 20);
			this.rbVideoFrameRatePAL.Name = "rbVideoFrameRatePAL";
			this.rbVideoFrameRatePAL.Size = new System.Drawing.Size(83, 17);
			this.rbVideoFrameRatePAL.TabIndex = 0;
			this.rbVideoFrameRatePAL.TabStop = true;
			this.rbVideoFrameRatePAL.Text = "25 fps (PAL)";
			this.rbVideoFrameRatePAL.UseVisualStyleBackColor = true;
			// 
			// tbxVideoCaptureDeviceName
			// 
			this.tbxVideoCaptureDeviceName.Location = new System.Drawing.Point(9, 16);
			this.tbxVideoCaptureDeviceName.Name = "tbxVideoCaptureDeviceName";
			this.tbxVideoCaptureDeviceName.Size = new System.Drawing.Size(247, 20);
			this.tbxVideoCaptureDeviceName.TabIndex = 6;
			// 
			// ucAnalogueCameraSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlText;
			this.Controls.Add(this.lblDeviceName);
			this.Controls.Add(this.gpxFrameRate);
			this.Controls.Add(this.tbxVideoCaptureDeviceName);
			this.ForeColor = System.Drawing.SystemColors.Window;
			this.Name = "ucAnalogueCameraSettings";
			this.Size = new System.Drawing.Size(290, 205);
			this.gpxFrameRate.ResumeLayout(false);
			this.gpxFrameRate.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDeviceName;
		private System.Windows.Forms.GroupBox gpxFrameRate;
		private System.Windows.Forms.RadioButton rbVideoFrameRateNTSC;
		private System.Windows.Forms.RadioButton rbVideoFrameRatePAL;
		private System.Windows.Forms.TextBox tbxVideoCaptureDeviceName;
	}
}
