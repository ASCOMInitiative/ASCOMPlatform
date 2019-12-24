namespace ASCOM.DynamicRemoteClients
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnApply = new System.Windows.Forms.Button();
            this.NumTelescope = new System.Windows.Forms.NumericUpDown();
            this.NumCamera = new System.Windows.Forms.NumericUpDown();
            this.NumFilterWheel = new System.Windows.Forms.NumericUpDown();
            this.NumFocuser = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NumDome = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.NumObservingConditions = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.NumSafetyMonitor = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.NumRotator = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.NumSwitch = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.NumVideo = new System.Windows.Forms.NumericUpDown();
            this.LblVersionNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumTelescope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFilterWheel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFocuser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumDome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumObservingConditions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSafetyMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumRotator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSwitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(320, 330);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 0;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnApply
            // 
            this.BtnApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnApply.Location = new System.Drawing.Point(239, 330);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 23);
            this.BtnApply.TabIndex = 1;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // NumTelescope
            // 
            this.NumTelescope.Location = new System.Drawing.Point(190, 259);
            this.NumTelescope.Name = "NumTelescope";
            this.NumTelescope.Size = new System.Drawing.Size(39, 20);
            this.NumTelescope.TabIndex = 2;
            // 
            // NumCamera
            // 
            this.NumCamera.Location = new System.Drawing.Point(190, 51);
            this.NumCamera.Name = "NumCamera";
            this.NumCamera.Size = new System.Drawing.Size(39, 20);
            this.NumCamera.TabIndex = 3;
            // 
            // NumFilterWheel
            // 
            this.NumFilterWheel.Location = new System.Drawing.Point(190, 103);
            this.NumFilterWheel.Name = "NumFilterWheel";
            this.NumFilterWheel.Size = new System.Drawing.Size(39, 20);
            this.NumFilterWheel.TabIndex = 4;
            // 
            // NumFocuser
            // 
            this.NumFocuser.Location = new System.Drawing.Point(190, 129);
            this.NumFocuser.Name = "NumFocuser";
            this.NumFocuser.Size = new System.Drawing.Size(39, 20);
            this.NumFocuser.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Telescope";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Camera";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Filter Wheel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Focuser";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Dome";
            // 
            // NumDome
            // 
            this.NumDome.Location = new System.Drawing.Point(190, 77);
            this.NumDome.Name = "NumDome";
            this.NumDome.Size = new System.Drawing.Size(39, 20);
            this.NumDome.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(80, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Observing Conditions";
            // 
            // NumObservingConditions
            // 
            this.NumObservingConditions.Location = new System.Drawing.Point(190, 155);
            this.NumObservingConditions.Name = "NumObservingConditions";
            this.NumObservingConditions.Size = new System.Drawing.Size(39, 20);
            this.NumObservingConditions.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(109, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Safety Monitor";
            // 
            // NumSafetyMonitor
            // 
            this.NumSafetyMonitor.Location = new System.Drawing.Point(190, 207);
            this.NumSafetyMonitor.Name = "NumSafetyMonitor";
            this.NumSafetyMonitor.Size = new System.Drawing.Size(39, 20);
            this.NumSafetyMonitor.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Rotator";
            // 
            // NumRotator
            // 
            this.NumRotator.Location = new System.Drawing.Point(190, 181);
            this.NumRotator.Name = "NumRotator";
            this.NumRotator.Size = new System.Drawing.Size(39, 20);
            this.NumRotator.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(139, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Switch";
            // 
            // NumSwitch
            // 
            this.NumSwitch.Location = new System.Drawing.Point(190, 233);
            this.NumSwitch.Name = "NumSwitch";
            this.NumSwitch.Size = new System.Drawing.Size(39, 20);
            this.NumSwitch.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(77, 288);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(240, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Video - Not currently supported for remote access";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(74, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(255, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Number of Remote Device Drivers Required";
            // 
            // NumVideo
            // 
            this.NumVideo.Enabled = false;
            this.NumVideo.Location = new System.Drawing.Point(190, 285);
            this.NumVideo.Name = "NumVideo";
            this.NumVideo.Size = new System.Drawing.Size(39, 20);
            this.NumVideo.TabIndex = 22;
            this.NumVideo.Visible = false;
            // 
            // LblVersionNumber
            // 
            this.LblVersionNumber.AutoSize = true;
            this.LblVersionNumber.Location = new System.Drawing.Point(12, 335);
            this.LblVersionNumber.Name = "LblVersionNumber";
            this.LblVersionNumber.Size = new System.Drawing.Size(131, 13);
            this.LblVersionNumber.TabIndex = 23;
            this.LblVersionNumber.Text = "Version Number Unknown";
            // 
            // Form1
            // 
            this.AcceptButton = this.BtnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnApply;
            this.ClientSize = new System.Drawing.Size(407, 365);
            this.Controls.Add(this.LblVersionNumber);
            this.Controls.Add(this.NumVideo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.NumSwitch);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.NumRotator);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.NumSafetyMonitor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NumObservingConditions);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.NumDome);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumFocuser);
            this.Controls.Add(this.NumFilterWheel);
            this.Controls.Add(this.NumCamera);
            this.Controls.Add(this.NumTelescope);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.BtnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Dynamic Remote Driver Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.NumTelescope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFilterWheel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFocuser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumDome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumObservingConditions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSafetyMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumRotator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSwitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.NumericUpDown NumTelescope;
        private System.Windows.Forms.NumericUpDown NumCamera;
        private System.Windows.Forms.NumericUpDown NumFilterWheel;
        private System.Windows.Forms.NumericUpDown NumFocuser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NumDome;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NumObservingConditions;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown NumSafetyMonitor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown NumRotator;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown NumSwitch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown NumVideo;
        private System.Windows.Forms.Label LblVersionNumber;
    }
}

