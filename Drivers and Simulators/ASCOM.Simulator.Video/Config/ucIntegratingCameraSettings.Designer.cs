namespace ASCOM.Simulator.Config
{
	partial class ucIntegratingCameraSettings
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaxExposureSec = new System.Windows.Forms.NumericUpDown();
            this.nudMinExposureSec = new System.Windows.Forms.NumericUpDown();
            this.tbxSupportedExposures = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxExposureSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinExposureSec)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Max Exposure";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Min Exposure";
            // 
            // nudMaxExposureSec
            // 
            this.nudMaxExposureSec.DecimalPlaces = 3;
            this.nudMaxExposureSec.Location = new System.Drawing.Point(101, 197);
            this.nudMaxExposureSec.Name = "nudMaxExposureSec";
            this.nudMaxExposureSec.Size = new System.Drawing.Size(72, 20);
            this.nudMaxExposureSec.TabIndex = 19;
            // 
            // nudMinExposureSec
            // 
            this.nudMinExposureSec.DecimalPlaces = 3;
            this.nudMinExposureSec.Location = new System.Drawing.Point(9, 197);
            this.nudMinExposureSec.Name = "nudMinExposureSec";
            this.nudMinExposureSec.Size = new System.Drawing.Size(72, 20);
            this.nudMinExposureSec.TabIndex = 18;
            // 
            // tbxSupportedExposures
            // 
            this.tbxSupportedExposures.Location = new System.Drawing.Point(9, 16);
            this.tbxSupportedExposures.Multiline = true;
            this.tbxSupportedExposures.Name = "tbxSupportedExposures";
            this.tbxSupportedExposures.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxSupportedExposures.Size = new System.Drawing.Size(247, 146);
            this.tbxSupportedExposures.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Supported Exposure Rates";
            // 
            // ucIntegratingCameraSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudMaxExposureSec);
            this.Controls.Add(this.nudMinExposureSec);
            this.Controls.Add(this.tbxSupportedExposures);
            this.Controls.Add(this.label9);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Name = "ucIntegratingCameraSettings";
            this.Size = new System.Drawing.Size(292, 254);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxExposureSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinExposureSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nudMaxExposureSec;
		private System.Windows.Forms.NumericUpDown nudMinExposureSec;
		private System.Windows.Forms.TextBox tbxSupportedExposures;
		private System.Windows.Forms.Label label9;
	}
}
