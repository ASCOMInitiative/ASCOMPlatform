namespace ASCOM.Simulator
{
	partial class frmImageSettings
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
            this.tbWhiteBalance = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbWhiteBalance)).BeginInit();
            this.SuspendLayout();
            // 
            // tbWhiteBalance
            // 
            this.tbWhiteBalance.Location = new System.Drawing.Point(12, 21);
            this.tbWhiteBalance.Maximum = 255;
            this.tbWhiteBalance.Name = "tbWhiteBalance";
            this.tbWhiteBalance.Size = new System.Drawing.Size(272, 42);
            this.tbWhiteBalance.TabIndex = 0;
            this.tbWhiteBalance.TickFrequency = 8;
            this.tbWhiteBalance.ValueChanged += new System.EventHandler(this.tbWhiteBalance_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(96, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "White Balance";
            // 
            // frmImageSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(303, 84);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWhiteBalance);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmImageSettings";
            this.Text = "Image Settings";
            this.Load += new System.EventHandler(this.frmImageSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbWhiteBalance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TrackBar tbWhiteBalance;
		private System.Windows.Forms.Label label1;
	}
}