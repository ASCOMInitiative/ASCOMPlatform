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
            this.label1 = new System.Windows.Forms.Label();
            this.LabelMaxStep = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.RadioMoving = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            // 
            // LabelMaxStep
            // 
            this.LabelMaxStep.AutoSize = true;
            this.LabelMaxStep.Location = new System.Drawing.Point(254, 13);
            this.LabelMaxStep.Name = "LabelMaxStep";
            this.LabelMaxStep.Size = new System.Drawing.Size(37, 13);
            this.LabelMaxStep.TabIndex = 2;
            this.LabelMaxStep.Text = "30000";
            this.LabelMaxStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempCompAvailable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.label4.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempCompAvailable;
            this.label4.Location = new System.Drawing.Point(174, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Temperature";
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempCompAvailable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempCompAvailable;
            this.textBox1.Location = new System.Drawing.Point(247, 130);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(44, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "17.5°";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RadioMoving
            // 
            this.RadioMoving.AutoSize = true;
            this.RadioMoving.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.IsMoving;
            this.RadioMoving.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "IsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RadioMoving.Location = new System.Drawing.Point(12, 58);
            this.RadioMoving.Name = "RadioMoving";
            this.RadioMoving.Size = new System.Drawing.Size(60, 17);
            this.RadioMoving.TabIndex = 4;
            this.RadioMoving.TabStop = true;
            this.RadioMoving.Text = "Moving";
            this.RadioMoving.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.Location = new System.Drawing.Point(12, 29);
            this.progressBar1.Maximum = (int)global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxStep;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(279, 23);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = (int)global::ASCOM.FocuserSimulator.Properties.Settings.Default.sPosition;
            // 
            // button2
            // 
            this.button2.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "IsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button2.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.IsMoving;
            this.button2.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_cancel;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(226, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "HALT";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_configure;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(12, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Setup";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "label2";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 162);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.RadioMoving);
            this.Controls.Add(this.LabelMaxStep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "ASCOM Focuser Simulator View";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelMaxStep;
        private System.Windows.Forms.RadioButton RadioMoving;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;







    }
}

