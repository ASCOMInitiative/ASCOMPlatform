namespace ASCOM.Simulator.Config
{
	partial class ucGeneral
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
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.nudPixelSizeY = new System.Windows.Forms.NumericUpDown();
            this.nudPixelSizeX = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxSupportedActionsList = new System.Windows.Forms.TextBox();
            this.cbxSensorType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxSensorName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAnalogueNonIntegrating = new System.Windows.Forms.RadioButton();
            this.rbAnalogueIntegrating = new System.Windows.Forms.RadioButton();
            this.rbVideoSystem = new System.Windows.Forms.RadioButton();
            this.rbDigitalCamera = new System.Windows.Forms.RadioButton();
            this.cbxBitDepth = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelSizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelSizeX)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.Window;
            this.label13.Location = new System.Drawing.Point(94, 208);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Pixel Size Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.Window;
            this.label14.Location = new System.Drawing.Point(2, 208);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Pixel Size X";
            // 
            // nudPixelSizeY
            // 
            this.nudPixelSizeY.DecimalPlaces = 1;
            this.nudPixelSizeY.Location = new System.Drawing.Point(97, 225);
            this.nudPixelSizeY.Name = "nudPixelSizeY";
            this.nudPixelSizeY.Size = new System.Drawing.Size(72, 20);
            this.nudPixelSizeY.TabIndex = 30;
            // 
            // nudPixelSizeX
            // 
            this.nudPixelSizeX.DecimalPlaces = 1;
            this.nudPixelSizeX.Location = new System.Drawing.Point(5, 225);
            this.nudPixelSizeX.Name = "nudPixelSizeX";
            this.nudPixelSizeX.Size = new System.Drawing.Size(72, 20);
            this.nudPixelSizeX.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.Window;
            this.label7.Location = new System.Drawing.Point(2, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Sensor Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(219, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Supported Actions";
            // 
            // tbxSupportedActionsList
            // 
            this.tbxSupportedActionsList.Location = new System.Drawing.Point(222, 142);
            this.tbxSupportedActionsList.Multiline = true;
            this.tbxSupportedActionsList.Name = "tbxSupportedActionsList";
            this.tbxSupportedActionsList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxSupportedActionsList.Size = new System.Drawing.Size(247, 112);
            this.tbxSupportedActionsList.TabIndex = 23;
            // 
            // cbxSensorType
            // 
            this.cbxSensorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSensorType.FormattingEnabled = true;
            this.cbxSensorType.Location = new System.Drawing.Point(80, 168);
            this.cbxSensorType.Name = "cbxSensorType";
            this.cbxSensorType.Size = new System.Drawing.Size(121, 21);
            this.cbxSensorType.TabIndex = 27;
            this.cbxSensorType.SelectedIndexChanged += new System.EventHandler(this.cbxSensorType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(3, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Sensor Name";
            // 
            // tbxSensorName
            // 
            this.tbxSensorName.Location = new System.Drawing.Point(80, 142);
            this.tbxSensorName.Name = "tbxSensorName";
            this.tbxSensorName.Size = new System.Drawing.Size(121, 20);
            this.tbxSensorName.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAnalogueNonIntegrating);
            this.groupBox1.Controls.Add(this.rbAnalogueIntegrating);
            this.groupBox1.Controls.Add(this.rbVideoSystem);
            this.groupBox1.Controls.Add(this.rbDigitalCamera);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Location = new System.Drawing.Point(6, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 75);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulated Camera Type";
            // 
            // rbAnalogueNonIntegrating
            // 
            this.rbAnalogueNonIntegrating.AutoSize = true;
            this.rbAnalogueNonIntegrating.Checked = true;
            this.rbAnalogueNonIntegrating.ForeColor = System.Drawing.SystemColors.Window;
            this.rbAnalogueNonIntegrating.Location = new System.Drawing.Point(18, 25);
            this.rbAnalogueNonIntegrating.Name = "rbAnalogueNonIntegrating";
            this.rbAnalogueNonIntegrating.Size = new System.Drawing.Size(146, 17);
            this.rbAnalogueNonIntegrating.TabIndex = 8;
            this.rbAnalogueNonIntegrating.TabStop = true;
            this.rbAnalogueNonIntegrating.Text = "Analogue Non Integrating";
            this.rbAnalogueNonIntegrating.UseVisualStyleBackColor = true;
            this.rbAnalogueNonIntegrating.CheckedChanged += new System.EventHandler(this.CameraTypeChanged);
            // 
            // rbAnalogueIntegrating
            // 
            this.rbAnalogueIntegrating.AutoSize = true;
            this.rbAnalogueIntegrating.ForeColor = System.Drawing.SystemColors.Window;
            this.rbAnalogueIntegrating.Location = new System.Drawing.Point(18, 48);
            this.rbAnalogueIntegrating.Name = "rbAnalogueIntegrating";
            this.rbAnalogueIntegrating.Size = new System.Drawing.Size(123, 17);
            this.rbAnalogueIntegrating.TabIndex = 9;
            this.rbAnalogueIntegrating.Text = "Analogue Integrating";
            this.rbAnalogueIntegrating.UseVisualStyleBackColor = true;
            this.rbAnalogueIntegrating.CheckedChanged += new System.EventHandler(this.CameraTypeChanged);
            // 
            // rbVideoSystem
            // 
            this.rbVideoSystem.AutoSize = true;
            this.rbVideoSystem.ForeColor = System.Drawing.SystemColors.Window;
            this.rbVideoSystem.Location = new System.Drawing.Point(216, 48);
            this.rbVideoSystem.Name = "rbVideoSystem";
            this.rbVideoSystem.Size = new System.Drawing.Size(89, 17);
            this.rbVideoSystem.TabIndex = 11;
            this.rbVideoSystem.Text = "Video System";
            this.rbVideoSystem.UseVisualStyleBackColor = true;
            this.rbVideoSystem.CheckedChanged += new System.EventHandler(this.CameraTypeChanged);
            // 
            // rbDigitalCamera
            // 
            this.rbDigitalCamera.AutoSize = true;
            this.rbDigitalCamera.ForeColor = System.Drawing.SystemColors.Window;
            this.rbDigitalCamera.Location = new System.Drawing.Point(216, 25);
            this.rbDigitalCamera.Name = "rbDigitalCamera";
            this.rbDigitalCamera.Size = new System.Drawing.Size(54, 17);
            this.rbDigitalCamera.TabIndex = 10;
            this.rbDigitalCamera.Text = "Digital";
            this.rbDigitalCamera.UseVisualStyleBackColor = true;
            this.rbDigitalCamera.CheckedChanged += new System.EventHandler(this.CameraTypeChanged);
            // 
            // cbxBitDepth
            // 
            this.cbxBitDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBitDepth.FormattingEnabled = true;
            this.cbxBitDepth.Items.AddRange(new object[] {
            "8 bit",
            "12 bit",
            "16 bit"});
            this.cbxBitDepth.Location = new System.Drawing.Point(80, 95);
            this.cbxBitDepth.Name = "cbxBitDepth";
            this.cbxBitDepth.Size = new System.Drawing.Size(93, 21);
            this.cbxBitDepth.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.Window;
            this.label8.Location = new System.Drawing.Point(3, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Bit Depth";
            // 
            // ucGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.Controls.Add(this.cbxBitDepth);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.nudPixelSizeY);
            this.Controls.Add(this.nudPixelSizeX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxSupportedActionsList);
            this.Controls.Add(this.cbxSensorType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxSensorName);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Name = "ucGeneral";
            this.Size = new System.Drawing.Size(530, 280);
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelSizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelSizeX)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.NumericUpDown nudPixelSizeY;
		private System.Windows.Forms.NumericUpDown nudPixelSizeX;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbxSupportedActionsList;
		private System.Windows.Forms.ComboBox cbxSensorType;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbxSensorName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cbxBitDepth;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RadioButton rbAnalogueNonIntegrating;
		private System.Windows.Forms.RadioButton rbAnalogueIntegrating;
		private System.Windows.Forms.RadioButton rbVideoSystem;
		private System.Windows.Forms.RadioButton rbDigitalCamera;
	}
}
