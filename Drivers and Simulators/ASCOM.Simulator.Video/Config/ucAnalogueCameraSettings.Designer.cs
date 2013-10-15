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
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbVideoFrameRateNTSC = new System.Windows.Forms.RadioButton();
            this.rbVideoFrameRatePAL = new System.Windows.Forms.RadioButton();
            this.tbxVideoCaptureDeviceName = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Video Capture Device Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbVideoFrameRateNTSC);
            this.groupBox2.Controls.Add(this.rbVideoFrameRatePAL);
            this.groupBox2.Location = new System.Drawing.Point(9, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 48);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Video Frame Rate";
            // 
            // rbVideoFrameRateNTSC
            // 
            this.rbVideoFrameRateNTSC.AutoSize = true;
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbxVideoCaptureDeviceName);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Name = "ucAnalogueCameraSettings";
            this.Size = new System.Drawing.Size(290, 205);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbVideoFrameRateNTSC;
		private System.Windows.Forms.RadioButton rbVideoFrameRatePAL;
		private System.Windows.Forms.TextBox tbxVideoCaptureDeviceName;
	}
}
