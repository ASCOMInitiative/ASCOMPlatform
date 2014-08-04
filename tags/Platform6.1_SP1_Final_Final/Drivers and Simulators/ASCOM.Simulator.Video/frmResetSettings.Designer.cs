namespace ASCOM.Simulator
{
	partial class frmResetSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResetSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDriverDefaults = new System.Windows.Forms.RadioButton();
            this.rbVideoSystem = new System.Windows.Forms.RadioButton();
            this.rbDigitalVideoCamera = new System.Windows.Forms.RadioButton();
            this.rbAnalogueIntegrating = new System.Windows.Forms.RadioButton();
            this.rbAnalogueNonIntegrating = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDriverDefaults);
            this.groupBox1.Controls.Add(this.rbVideoSystem);
            this.groupBox1.Controls.Add(this.rbDigitalVideoCamera);
            this.groupBox1.Controls.Add(this.rbAnalogueIntegrating);
            this.groupBox1.Controls.Add(this.rbAnalogueNonIntegrating);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 156);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Preset Simulated Camera Type";
            // 
            // rbDriverDefaults
            // 
            this.rbDriverDefaults.AutoSize = true;
            this.rbDriverDefaults.Checked = true;
            this.rbDriverDefaults.Location = new System.Drawing.Point(21, 23);
            this.rbDriverDefaults.Name = "rbDriverDefaults";
            this.rbDriverDefaults.Size = new System.Drawing.Size(171, 17);
            this.rbDriverDefaults.TabIndex = 4;
            this.rbDriverDefaults.TabStop = true;
            this.rbDriverDefaults.Text = "Video Driver Simulator Defaults";
            this.rbDriverDefaults.UseVisualStyleBackColor = true;
            // 
            // rbVideoSystem
            // 
            this.rbVideoSystem.AutoSize = true;
            this.rbVideoSystem.Location = new System.Drawing.Point(21, 127);
            this.rbVideoSystem.Name = "rbVideoSystem";
            this.rbVideoSystem.Size = new System.Drawing.Size(89, 17);
            this.rbVideoSystem.TabIndex = 3;
            this.rbVideoSystem.Text = "Video System";
            this.rbVideoSystem.UseVisualStyleBackColor = true;
            // 
            // rbDigitalVideoCamera
            // 
            this.rbDigitalVideoCamera.AutoSize = true;
            this.rbDigitalVideoCamera.Location = new System.Drawing.Point(21, 104);
            this.rbDigitalVideoCamera.Name = "rbDigitalVideoCamera";
            this.rbDigitalVideoCamera.Size = new System.Drawing.Size(123, 17);
            this.rbDigitalVideoCamera.TabIndex = 2;
            this.rbDigitalVideoCamera.Text = "Digital Video Camera";
            this.rbDigitalVideoCamera.UseVisualStyleBackColor = true;
            // 
            // rbAnalogueIntegrating
            // 
            this.rbAnalogueIntegrating.AutoSize = true;
            this.rbAnalogueIntegrating.Location = new System.Drawing.Point(21, 81);
            this.rbAnalogueIntegrating.Name = "rbAnalogueIntegrating";
            this.rbAnalogueIntegrating.Size = new System.Drawing.Size(192, 17);
            this.rbAnalogueIntegrating.TabIndex = 1;
            this.rbAnalogueIntegrating.Text = "Analogue Integrating Video Camera";
            this.rbAnalogueIntegrating.UseVisualStyleBackColor = true;
            // 
            // rbAnalogueNonIntegrating
            // 
            this.rbAnalogueNonIntegrating.AutoSize = true;
            this.rbAnalogueNonIntegrating.Location = new System.Drawing.Point(21, 58);
            this.rbAnalogueNonIntegrating.Name = "rbAnalogueNonIntegrating";
            this.rbAnalogueNonIntegrating.Size = new System.Drawing.Size(215, 17);
            this.rbAnalogueNonIntegrating.TabIndex = 0;
            this.rbAnalogueNonIntegrating.Text = "Analogue Non Integrating Video Camera";
            this.rbAnalogueNonIntegrating.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(204, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.Control;
            this.btnReset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReset.Location = new System.Drawing.Point(123, 181);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // frmResetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(289, 216);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResetSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reset Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbDigitalVideoCamera;
		private System.Windows.Forms.RadioButton rbAnalogueIntegrating;
		private System.Windows.Forms.RadioButton rbAnalogueNonIntegrating;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.RadioButton rbVideoSystem;
		private System.Windows.Forms.RadioButton rbDriverDefaults;
	}
}