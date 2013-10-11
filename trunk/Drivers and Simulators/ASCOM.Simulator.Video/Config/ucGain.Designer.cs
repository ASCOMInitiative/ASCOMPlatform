namespace ASCOM.Simulator.Config
{
	partial class ucGain
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
			this.pnlRangeGain = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.nudMinGain = new System.Windows.Forms.NumericUpDown();
			this.nudMaxGain = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			this.pnlDiscreteGain = new System.Windows.Forms.Panel();
			this.tbxSupportedGains = new System.Windows.Forms.TextBox();
			this.rbDiscreteGain = new System.Windows.Forms.RadioButton();
			this.rbGainRange = new System.Windows.Forms.RadioButton();
			this.pnlRangeGain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinGain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxGain)).BeginInit();
			this.pnlDiscreteGain.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlRangeGain
			// 
			this.pnlRangeGain.Controls.Add(this.label1);
			this.pnlRangeGain.Controls.Add(this.nudMinGain);
			this.pnlRangeGain.Controls.Add(this.nudMaxGain);
			this.pnlRangeGain.Controls.Add(this.label11);
			this.pnlRangeGain.Location = new System.Drawing.Point(21, 191);
			this.pnlRangeGain.Name = "pnlRangeGain";
			this.pnlRangeGain.Size = new System.Drawing.Size(179, 61);
			this.pnlRangeGain.TabIndex = 23;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(98, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Max Gain";
			// 
			// nudMinGain
			// 
			this.nudMinGain.Location = new System.Drawing.Point(9, 23);
			this.nudMinGain.Name = "nudMinGain";
			this.nudMinGain.Size = new System.Drawing.Size(72, 20);
			this.nudMinGain.TabIndex = 12;
			// 
			// nudMaxGain
			// 
			this.nudMaxGain.Location = new System.Drawing.Point(101, 23);
			this.nudMaxGain.Name = "nudMaxGain";
			this.nudMaxGain.Size = new System.Drawing.Size(72, 20);
			this.nudMaxGain.TabIndex = 13;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 6);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(49, 13);
			this.label11.TabIndex = 14;
			this.label11.Text = "Min Gain";
			// 
			// pnlDiscreteGain
			// 
			this.pnlDiscreteGain.Controls.Add(this.tbxSupportedGains);
			this.pnlDiscreteGain.Location = new System.Drawing.Point(18, 20);
			this.pnlDiscreteGain.Name = "pnlDiscreteGain";
			this.pnlDiscreteGain.Size = new System.Drawing.Size(207, 137);
			this.pnlDiscreteGain.TabIndex = 22;
			// 
			// tbxSupportedGains
			// 
			this.tbxSupportedGains.Location = new System.Drawing.Point(9, 3);
			this.tbxSupportedGains.Multiline = true;
			this.tbxSupportedGains.Name = "tbxSupportedGains";
			this.tbxSupportedGains.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbxSupportedGains.Size = new System.Drawing.Size(186, 129);
			this.tbxSupportedGains.TabIndex = 4;
			// 
			// rbDiscreteGain
			// 
			this.rbDiscreteGain.AutoSize = true;
			this.rbDiscreteGain.Location = new System.Drawing.Point(6, 0);
			this.rbDiscreteGain.Name = "rbDiscreteGain";
			this.rbDiscreteGain.Size = new System.Drawing.Size(121, 17);
			this.rbDiscreteGain.TabIndex = 21;
			this.rbDiscreteGain.Text = "Discrete gain values";
			this.rbDiscreteGain.UseVisualStyleBackColor = true;
			// 
			// rbGainRange
			// 
			this.rbGainRange.AutoSize = true;
			this.rbGainRange.Checked = true;
			this.rbGainRange.Location = new System.Drawing.Point(6, 171);
			this.rbGainRange.Name = "rbGainRange";
			this.rbGainRange.Size = new System.Drawing.Size(126, 17);
			this.rbGainRange.TabIndex = 20;
			this.rbGainRange.TabStop = true;
			this.rbGainRange.Text = "Range of gain values";
			this.rbGainRange.UseVisualStyleBackColor = true;
			// 
			// ucGain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlRangeGain);
			this.Controls.Add(this.pnlDiscreteGain);
			this.Controls.Add(this.rbDiscreteGain);
			this.Controls.Add(this.rbGainRange);
			this.Name = "ucGain";
			this.Size = new System.Drawing.Size(357, 366);
			this.pnlRangeGain.ResumeLayout(false);
			this.pnlRangeGain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinGain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxGain)).EndInit();
			this.pnlDiscreteGain.ResumeLayout(false);
			this.pnlDiscreteGain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlRangeGain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudMinGain;
		private System.Windows.Forms.NumericUpDown nudMaxGain;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel pnlDiscreteGain;
		private System.Windows.Forms.TextBox tbxSupportedGains;
		private System.Windows.Forms.RadioButton rbDiscreteGain;
		private System.Windows.Forms.RadioButton rbGainRange;
	}
}
