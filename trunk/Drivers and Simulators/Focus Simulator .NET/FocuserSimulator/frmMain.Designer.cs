using System;

namespace ASCOM.FocuserSimulator
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TrafficButton = new System.Windows.Forms.Button();
            this.SetupButton = new System.Windows.Forms.Button();
            this.CheckTempComp = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.RadioMoving = new System.Windows.Forms.RadioButton();
            this.LabelMaxStep = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.TimerTempComp = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.FocuserSimulator.Properties.Resources.saturnc;
            this.pictureBox1.Location = new System.Drawing.Point(82, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 38);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // TrafficButton
            // 
            this.TrafficButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.log_16x16;
            this.TrafficButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TrafficButton.Location = new System.Drawing.Point(12, 290);
            this.TrafficButton.Name = "TrafficButton";
            this.TrafficButton.Size = new System.Drawing.Size(61, 23);
            this.TrafficButton.TabIndex = 11;
            this.TrafficButton.Text = "Traffic";
            this.TrafficButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TrafficButton.UseVisualStyleBackColor = true;
            this.TrafficButton.Click += new System.EventHandler(this.ButtonTraffic_Click);
            // 
            // SetupButton
            // 
            this.SetupButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_configure;
            this.SetupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SetupButton.Location = new System.Drawing.Point(186, 290);
            this.SetupButton.Name = "SetupButton";
            this.SetupButton.Size = new System.Drawing.Size(60, 23);
            this.SetupButton.TabIndex = 8;
            this.SetupButton.Text = "Setup";
            this.SetupButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SetupButton.UseVisualStyleBackColor = true;
            this.SetupButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // CheckTempComp
            // 
            this.CheckTempComp.AutoCheck = false;
            this.CheckTempComp.AutoSize = true;
            this.CheckTempComp.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempComp;
            this.CheckTempComp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckTempComp.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempComp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckTempComp.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempComp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckTempComp.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempComp;
            this.CheckTempComp.Location = new System.Drawing.Point(15, 105);
            this.CheckTempComp.Name = "CheckTempComp";
            this.CheckTempComp.Size = new System.Drawing.Size(153, 17);
            this.CheckTempComp.TabIndex = 13;
            this.CheckTempComp.Text = "Temperature compensation";
            this.CheckTempComp.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "IsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button2.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.IsMoving;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = global::ASCOM.FocuserSimulator.Properties.Resources.ledred_16x16;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(181, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "HALT";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RadioMoving
            // 
            this.RadioMoving.AutoCheck = false;
            this.RadioMoving.AutoSize = true;
            this.RadioMoving.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.IsMoving;
            this.RadioMoving.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "IsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RadioMoving.Location = new System.Drawing.Point(13, 82);
            this.RadioMoving.Name = "RadioMoving";
            this.RadioMoving.Size = new System.Drawing.Size(60, 17);
            this.RadioMoving.TabIndex = 4;
            this.RadioMoving.TabStop = true;
            this.RadioMoving.Text = "Moving";
            this.RadioMoving.UseVisualStyleBackColor = true;
            // 
            // LabelMaxStep
            // 
            this.LabelMaxStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelMaxStep.AutoSize = true;
            this.LabelMaxStep.DataBindings.Add(new System.Windows.Forms.Binding("Visible", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LabelMaxStep.Location = new System.Drawing.Point(209, 37);
            this.LabelMaxStep.Name = "LabelMaxStep";
            this.LabelMaxStep.Size = new System.Drawing.Size(37, 13);
            this.LabelMaxStep.TabIndex = 2;
            this.LabelMaxStep.Text = "30000";
            this.LabelMaxStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelMaxStep.Visible = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.progressBar1.Location = new System.Drawing.Point(12, 53);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(234, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 3500;
            // 
            // TimerTempComp
            // 
            this.TimerTempComp.Tick += new System.EventHandler(this.TimerTempComp_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 325);
            this.ControlBox = false;
            this.Controls.Add(this.CheckTempComp);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TrafficButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SetupButton);
            this.Controls.Add(this.RadioMoving);
            this.Controls.Add(this.LabelMaxStep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "ASCOM Focuser Simulator View";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelMaxStep;
        private System.Windows.Forms.RadioButton RadioMoving;
        private System.Windows.Forms.Button SetupButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button TrafficButton;
        private System.Windows.Forms.Timer TimerTempComp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox CheckTempComp;







    }
}

