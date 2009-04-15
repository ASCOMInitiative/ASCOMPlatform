namespace ASCOM.LocalServerCOM
{
	partial class frmMain
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
			this.txtTrace = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtTrace
			// 
			this.txtTrace.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTrace.Location = new System.Drawing.Point(13, 14);
			this.txtTrace.Multiline = true;
			this.txtTrace.Name = "txtTrace";
			this.txtTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTrace.Size = new System.Drawing.Size(584, 229);
			this.txtTrace.TabIndex = 0;
			this.txtTrace.WordWrap = false;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(609, 256);
			this.Controls.Add(this.txtTrace);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmMain";
			this.Text = "ASCOM LocalServerCOM Sample";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtTrace;
	}
}

