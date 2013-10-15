namespace ASCOM.Simulator.Config
{
	partial class ucGamma
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.rbDiscreteGamma = new System.Windows.Forms.RadioButton();
            this.tbxSupportedGammas = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 79);
            this.label1.TabIndex = 33;
            this.label1.Text = "The simulator supports discrete gamma values of \'LO\', \'HI\', \'OFF\', numbers: 0.33 " +
    "or a name and a number in brackets: MAX (0.25)\r\n\r\nRange of gamma values is not s" +
    "upported.";
            // 
            // rbDiscreteGamma
            // 
            this.rbDiscreteGamma.AutoSize = true;
            this.rbDiscreteGamma.Checked = true;
            this.rbDiscreteGamma.Location = new System.Drawing.Point(6, 0);
            this.rbDiscreteGamma.Name = "rbDiscreteGamma";
            this.rbDiscreteGamma.Size = new System.Drawing.Size(135, 17);
            this.rbDiscreteGamma.TabIndex = 30;
            this.rbDiscreteGamma.TabStop = true;
            this.rbDiscreteGamma.Text = "Discrete gamma values";
            this.rbDiscreteGamma.UseVisualStyleBackColor = true;
            // 
            // tbxSupportedGammas
            // 
            this.tbxSupportedGammas.Location = new System.Drawing.Point(27, 23);
            this.tbxSupportedGammas.Multiline = true;
            this.tbxSupportedGammas.Name = "tbxSupportedGammas";
            this.tbxSupportedGammas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxSupportedGammas.Size = new System.Drawing.Size(186, 129);
            this.tbxSupportedGammas.TabIndex = 29;
            // 
            // ucGamma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbDiscreteGamma);
            this.Controls.Add(this.tbxSupportedGammas);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Name = "ucGamma";
            this.Size = new System.Drawing.Size(326, 350);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbDiscreteGamma;
		private System.Windows.Forms.TextBox tbxSupportedGammas;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label1;
	}
}
