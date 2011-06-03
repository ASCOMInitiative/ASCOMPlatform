namespace ASCOM.Simulator
{
    partial class FocuserSettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaxStepPosition = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxIncrement = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStepSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radRelativeFocuser = new System.Windows.Forms.RadioButton();
            this.radAbsoluteFocuser = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkIsSynchronous = new System.Windows.Forms.CheckBox();
            this.chkCanHalt = new System.Windows.Forms.CheckBox();
            this.chkCanChangeStepSize = new System.Windows.Forms.CheckBox();
            this.chkHasTempComp = new System.Windows.Forms.CheckBox();
            this.chkHasTempProbe = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStepsPerDegree = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUpdatePeriod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMinimumTemperature = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaximumTemperature = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurrentTemperature = new System.Windows.Forms.TextBox();
            this.btnON = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Maximum Step Pos:";
            // 
            // txtMaxStepPosition
            // 
            this.txtMaxStepPosition.Location = new System.Drawing.Point(112, 19);
            this.txtMaxStepPosition.MaxLength = 7;
            this.txtMaxStepPosition.Name = "txtMaxStepPosition";
            this.txtMaxStepPosition.Size = new System.Drawing.Size(100, 20);
            this.txtMaxStepPosition.TabIndex = 1;
            this.txtMaxStepPosition.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(5, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 58);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxIncrement);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtStepSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMaxStepPosition);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(60, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 121);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // txtMaxIncrement
            // 
            this.txtMaxIncrement.Location = new System.Drawing.Point(112, 87);
            this.txtMaxIncrement.Name = "txtMaxIncrement";
            this.txtMaxIncrement.Size = new System.Drawing.Size(100, 20);
            this.txtMaxIncrement.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(9, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Maximum Increment:";
            // 
            // txtStepSize
            // 
            this.txtStepSize.Location = new System.Drawing.Point(112, 52);
            this.txtStepSize.Name = "txtStepSize";
            this.txtStepSize.Size = new System.Drawing.Size(100, 20);
            this.txtStepSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(9, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Step Size (Microns):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radRelativeFocuser);
            this.groupBox2.Controls.Add(this.radAbsoluteFocuser);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(310, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 121);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Type";
            // 
            // radRelativeFocuser
            // 
            this.radRelativeFocuser.AutoSize = true;
            this.radRelativeFocuser.Location = new System.Drawing.Point(18, 76);
            this.radRelativeFocuser.Name = "radRelativeFocuser";
            this.radRelativeFocuser.Size = new System.Drawing.Size(64, 17);
            this.radRelativeFocuser.TabIndex = 1;
            this.radRelativeFocuser.TabStop = true;
            this.radRelativeFocuser.Text = "Relative";
            this.radRelativeFocuser.UseVisualStyleBackColor = true;
            // 
            // radAbsoluteFocuser
            // 
            this.radAbsoluteFocuser.AutoSize = true;
            this.radAbsoluteFocuser.Location = new System.Drawing.Point(18, 37);
            this.radAbsoluteFocuser.Name = "radAbsoluteFocuser";
            this.radAbsoluteFocuser.Size = new System.Drawing.Size(66, 17);
            this.radAbsoluteFocuser.TabIndex = 0;
            this.radAbsoluteFocuser.TabStop = true;
            this.radAbsoluteFocuser.Text = "Absolute";
            this.radAbsoluteFocuser.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkIsSynchronous);
            this.groupBox3.Controls.Add(this.chkCanHalt);
            this.groupBox3.Controls.Add(this.chkCanChangeStepSize);
            this.groupBox3.Controls.Add(this.chkHasTempComp);
            this.groupBox3.Controls.Add(this.chkHasTempProbe);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(19, 148);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(185, 168);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Capabilities";
            // 
            // chkIsSynchronous
            // 
            this.chkIsSynchronous.AutoSize = true;
            this.chkIsSynchronous.Location = new System.Drawing.Point(13, 119);
            this.chkIsSynchronous.Name = "chkIsSynchronous";
            this.chkIsSynchronous.Size = new System.Drawing.Size(88, 17);
            this.chkIsSynchronous.TabIndex = 4;
            this.chkIsSynchronous.Text = "Synchronous";
            this.chkIsSynchronous.UseVisualStyleBackColor = true;
            // 
            // chkCanHalt
            // 
            this.chkCanHalt.AutoSize = true;
            this.chkCanHalt.Location = new System.Drawing.Point(13, 94);
            this.chkCanHalt.Name = "chkCanHalt";
            this.chkCanHalt.Size = new System.Drawing.Size(45, 17);
            this.chkCanHalt.TabIndex = 3;
            this.chkCanHalt.Text = "Halt";
            this.chkCanHalt.UseVisualStyleBackColor = true;
            // 
            // chkCanChangeStepSize
            // 
            this.chkCanChangeStepSize.AutoSize = true;
            this.chkCanChangeStepSize.Location = new System.Drawing.Point(13, 69);
            this.chkCanChangeStepSize.Name = "chkCanChangeStepSize";
            this.chkCanChangeStepSize.Size = new System.Drawing.Size(71, 17);
            this.chkCanChangeStepSize.TabIndex = 2;
            this.chkCanChangeStepSize.Text = "Step Size";
            this.chkCanChangeStepSize.UseVisualStyleBackColor = true;
            this.chkCanChangeStepSize.CheckedChanged += new System.EventHandler(this.chkCanChangeStepSize_CheckedChanged);
            // 
            // chkHasTempComp
            // 
            this.chkHasTempComp.AutoSize = true;
            this.chkHasTempComp.Location = new System.Drawing.Point(13, 44);
            this.chkHasTempComp.Name = "chkHasTempComp";
            this.chkHasTempComp.Size = new System.Drawing.Size(156, 17);
            this.chkHasTempComp.TabIndex = 1;
            this.chkHasTempComp.Text = "Temperature Compensation";
            this.chkHasTempComp.UseVisualStyleBackColor = true;
            this.chkHasTempComp.CheckedChanged += new System.EventHandler(this.chkHasTempComp_CheckedChanged);
            // 
            // chkHasTempProbe
            // 
            this.chkHasTempProbe.AutoSize = true;
            this.chkHasTempProbe.Location = new System.Drawing.Point(13, 19);
            this.chkHasTempProbe.Name = "chkHasTempProbe";
            this.chkHasTempProbe.Size = new System.Drawing.Size(117, 17);
            this.chkHasTempProbe.TabIndex = 0;
            this.chkHasTempProbe.Text = "Temperature Probe";
            this.chkHasTempProbe.UseVisualStyleBackColor = true;
            this.chkHasTempProbe.CheckedChanged += new System.EventHandler(this.chkHasTempProbe_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.txtStepsPerDegree);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txtUpdatePeriod);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txtMinimumTemperature);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtMaximumTemperature);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtCurrentTemperature);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(210, 148);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(205, 168);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Temperature Simulator";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Steps / C:";
            // 
            // txtStepsPerDegree
            // 
            this.txtStepsPerDegree.Location = new System.Drawing.Point(128, 137);
            this.txtStepsPerDegree.Name = "txtStepsPerDegree";
            this.txtStepsPerDegree.Size = new System.Drawing.Size(68, 20);
            this.txtStepsPerDegree.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Cycle Period (sec):";
            // 
            // txtUpdatePeriod
            // 
            this.txtUpdatePeriod.Location = new System.Drawing.Point(128, 107);
            this.txtUpdatePeriod.Name = "txtUpdatePeriod";
            this.txtUpdatePeriod.Size = new System.Drawing.Size(68, 20);
            this.txtUpdatePeriod.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Minimum Temperature:";
            // 
            // txtMinimumTemperature
            // 
            this.txtMinimumTemperature.Location = new System.Drawing.Point(128, 78);
            this.txtMinimumTemperature.Name = "txtMinimumTemperature";
            this.txtMinimumTemperature.Size = new System.Drawing.Size(68, 20);
            this.txtMinimumTemperature.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Maximum Temperature:";
            // 
            // txtMaximumTemperature
            // 
            this.txtMaximumTemperature.Location = new System.Drawing.Point(128, 49);
            this.txtMaximumTemperature.Name = "txtMaximumTemperature";
            this.txtMaximumTemperature.Size = new System.Drawing.Size(68, 20);
            this.txtMaximumTemperature.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Current Temperature:";
            // 
            // txtCurrentTemperature
            // 
            this.txtCurrentTemperature.Location = new System.Drawing.Point(128, 19);
            this.txtCurrentTemperature.Name = "txtCurrentTemperature";
            this.txtCurrentTemperature.Size = new System.Drawing.Size(68, 20);
            this.txtCurrentTemperature.TabIndex = 0;
            // 
            // btnON
            // 
            this.btnON.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnON.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnON.Location = new System.Drawing.Point(172, 333);
            this.btnON.Name = "btnON";
            this.btnON.Size = new System.Drawing.Size(75, 23);
            this.btnON.TabIndex = 8;
            this.btnON.Text = "OK";
            this.btnON.UseVisualStyleBackColor = true;
            this.btnON.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FocuserSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(421, 368);
            this.Controls.Add(this.btnON);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "FocuserSettingsForm";
            this.Text = "Focuser Simulator Settings";
            this.Load += new System.EventHandler(this.SettingsFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaxStepPosition;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxIncrement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStepSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radRelativeFocuser;
        private System.Windows.Forms.RadioButton radAbsoluteFocuser;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkIsSynchronous;
        private System.Windows.Forms.CheckBox chkCanHalt;
        private System.Windows.Forms.CheckBox chkCanChangeStepSize;
        private System.Windows.Forms.CheckBox chkHasTempComp;
        private System.Windows.Forms.CheckBox chkHasTempProbe;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStepsPerDegree;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUpdatePeriod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinimumTemperature;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaximumTemperature;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurrentTemperature;
        private System.Windows.Forms.Button btnON;
    }
}