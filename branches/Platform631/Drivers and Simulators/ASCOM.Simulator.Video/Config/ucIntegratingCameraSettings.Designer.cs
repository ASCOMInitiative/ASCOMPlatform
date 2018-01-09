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
			this.lblMaxExp = new System.Windows.Forms.Label();
			this.lblMinExp = new System.Windows.Forms.Label();
			this.nudMaxExposureSec = new System.Windows.Forms.NumericUpDown();
			this.nudMinExposureSec = new System.Windows.Forms.NumericUpDown();
			this.tbxSupportedExposures = new System.Windows.Forms.TextBox();
			this.lblSuppExps = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxExposureSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinExposureSec)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMaxExp
			// 
			this.lblMaxExp.AutoSize = true;
			this.lblMaxExp.ForeColor = System.Drawing.SystemColors.Window;
			this.lblMaxExp.Location = new System.Drawing.Point(98, 180);
			this.lblMaxExp.Name = "lblMaxExp";
			this.lblMaxExp.Size = new System.Drawing.Size(74, 13);
			this.lblMaxExp.TabIndex = 21;
			this.lblMaxExp.Text = "Max Exposure";
			// 
			// lblMinExp
			// 
			this.lblMinExp.AutoSize = true;
			this.lblMinExp.ForeColor = System.Drawing.SystemColors.Window;
			this.lblMinExp.Location = new System.Drawing.Point(6, 180);
			this.lblMinExp.Name = "lblMinExp";
			this.lblMinExp.Size = new System.Drawing.Size(71, 13);
			this.lblMinExp.TabIndex = 20;
			this.lblMinExp.Text = "Min Exposure";
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
			// lblSuppExps
			// 
			this.lblSuppExps.AutoSize = true;
			this.lblSuppExps.ForeColor = System.Drawing.SystemColors.Window;
			this.lblSuppExps.Location = new System.Drawing.Point(6, 0);
			this.lblSuppExps.Name = "lblSuppExps";
			this.lblSuppExps.Size = new System.Drawing.Size(134, 13);
			this.lblSuppExps.TabIndex = 16;
			this.lblSuppExps.Text = "Supported Exposure Rates";
			// 
			// ucIntegratingCameraSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlText;
			this.Controls.Add(this.lblMaxExp);
			this.Controls.Add(this.lblMinExp);
			this.Controls.Add(this.nudMaxExposureSec);
			this.Controls.Add(this.nudMinExposureSec);
			this.Controls.Add(this.tbxSupportedExposures);
			this.Controls.Add(this.lblSuppExps);
			this.ForeColor = System.Drawing.SystemColors.Window;
			this.Name = "ucIntegratingCameraSettings";
			this.Size = new System.Drawing.Size(292, 254);
			((System.ComponentModel.ISupportInitialize)(this.nudMaxExposureSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinExposureSec)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMaxExp;
		private System.Windows.Forms.Label lblMinExp;
		private System.Windows.Forms.NumericUpDown nudMaxExposureSec;
		private System.Windows.Forms.NumericUpDown nudMinExposureSec;
		private System.Windows.Forms.TextBox tbxSupportedExposures;
		private System.Windows.Forms.Label lblSuppExps;
	}
}
