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
            this.LabelMinPosition = new System.Windows.Forms.Label();
            this.TrafficButton = new System.Windows.Forms.Button();
            this.SetupButton = new System.Windows.Forms.Button();
            this.TimerTempComp = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ClearLogButton = new System.Windows.Forms.PictureBox();
            this.CheckOthers = new System.Windows.Forms.CheckBox();
            this.CheckIsMoving = new System.Windows.Forms.CheckBox();
            this.CheckTempRelated = new System.Windows.Forms.CheckBox();
            this.CheckHaltMove = new System.Windows.Forms.CheckBox();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.CheckTempComp = new System.Windows.Forms.CheckBox();
            this.HaltButton = new System.Windows.Forms.Button();
            this.RadioMoving = new System.Windows.Forms.RadioButton();
            this.LabelMaxStep = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LabelPosition = new System.Windows.Forms.Label();
            this.TextTemp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClearLogButton)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelMinPosition
            // 
            this.LabelMinPosition.AutoSize = true;
            this.LabelMinPosition.Location = new System.Drawing.Point(12, 36);
            this.LabelMinPosition.Name = "LabelMinPosition";
            this.LabelMinPosition.Size = new System.Drawing.Size(13, 13);
            this.LabelMinPosition.TabIndex = 1;
            this.LabelMinPosition.Text = "0";
            // 
            // TrafficButton
            // 
            this.TrafficButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.log_16x16;
            this.TrafficButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TrafficButton.Location = new System.Drawing.Point(12, 154);
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
            this.SetupButton.Location = new System.Drawing.Point(186, 154);
            this.SetupButton.Name = "SetupButton";
            this.SetupButton.Size = new System.Drawing.Size(60, 23);
            this.SetupButton.TabIndex = 8;
            this.SetupButton.Text = "Setup";
            this.SetupButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SetupButton.UseVisualStyleBackColor = true;
            this.SetupButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // TimerTempComp
            // 
            this.TimerTempComp.Tick += new System.EventHandler(this.TimerTempComp_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ClearLogButton);
            this.groupBox1.Controls.Add(this.CheckOthers);
            this.groupBox1.Controls.Add(this.CheckIsMoving);
            this.groupBox1.Controls.Add(this.CheckTempRelated);
            this.groupBox1.Controls.Add(this.CheckHaltMove);
            this.groupBox1.Location = new System.Drawing.Point(12, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 75);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracked events";
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_cancel;
            this.ClearLogButton.Location = new System.Drawing.Point(211, 50);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(17, 19);
            this.ClearLogButton.TabIndex = 4;
            this.ClearLogButton.TabStop = false;
            this.ClearLogButton.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // CheckOthers
            // 
            this.CheckOthers.AutoSize = true;
            this.CheckOthers.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogOther;
            this.CheckOthers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckOthers.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogOther", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckOthers.Location = new System.Drawing.Point(121, 42);
            this.CheckOthers.Name = "CheckOthers";
            this.CheckOthers.Size = new System.Drawing.Size(55, 17);
            this.CheckOthers.TabIndex = 3;
            this.CheckOthers.Text = "Others";
            this.CheckOthers.UseVisualStyleBackColor = true;
            // 
            // CheckIsMoving
            // 
            this.CheckIsMoving.AutoSize = true;
            this.CheckIsMoving.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogIsMoving;
            this.CheckIsMoving.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckIsMoving.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogIsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckIsMoving.Location = new System.Drawing.Point(121, 19);
            this.CheckIsMoving.Name = "CheckIsMoving";
            this.CheckIsMoving.Size = new System.Drawing.Size(67, 17);
            this.CheckIsMoving.TabIndex = 2;
            this.CheckIsMoving.Text = "IsMoving";
            this.CheckIsMoving.UseVisualStyleBackColor = true;
            // 
            // CheckTempRelated
            // 
            this.CheckTempRelated.AutoSize = true;
            this.CheckTempRelated.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogTempRelated;
            this.CheckTempRelated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckTempRelated.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogTempRelated", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckTempRelated.Location = new System.Drawing.Point(17, 42);
            this.CheckTempRelated.Name = "CheckTempRelated";
            this.CheckTempRelated.Size = new System.Drawing.Size(86, 17);
            this.CheckTempRelated.TabIndex = 1;
            this.CheckTempRelated.Text = "Temp related";
            this.CheckTempRelated.UseVisualStyleBackColor = true;
            // 
            // CheckHaltMove
            // 
            this.CheckHaltMove.AutoSize = true;
            this.CheckHaltMove.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogHaltMove;
            this.CheckHaltMove.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckHaltMove.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogHaltMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckHaltMove.Location = new System.Drawing.Point(17, 19);
            this.CheckHaltMove.Name = "CheckHaltMove";
            this.CheckHaltMove.Size = new System.Drawing.Size(75, 17);
            this.CheckHaltMove.TabIndex = 0;
            this.CheckHaltMove.Text = "Halt/Move";
            this.CheckHaltMove.UseVisualStyleBackColor = true;
            // 
            // LogBox
            // 
            this.LogBox.AcceptsReturn = true;
            this.LogBox.HideSelection = false;
            this.LogBox.Location = new System.Drawing.Point(12, 264);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(234, 134);
            this.LogBox.TabIndex = 16;
            this.LogBox.WordWrap = false;
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
            this.CheckTempComp.Location = new System.Drawing.Point(12, 117);
            this.CheckTempComp.Name = "CheckTempComp";
            this.CheckTempComp.Size = new System.Drawing.Size(153, 17);
            this.CheckTempComp.TabIndex = 13;
            this.CheckTempComp.Text = "Temperature compensation";
            this.CheckTempComp.UseVisualStyleBackColor = true;
            // 
            // HaltButton
            // 
            this.HaltButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HaltButton.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "IsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.HaltButton.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.IsMoving;
            this.HaltButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HaltButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.ledred_16x16;
            this.HaltButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HaltButton.Location = new System.Drawing.Point(181, 82);
            this.HaltButton.Name = "HaltButton";
            this.HaltButton.Size = new System.Drawing.Size(65, 23);
            this.HaltButton.TabIndex = 9;
            this.HaltButton.Text = "HALT";
            this.HaltButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HaltButton.UseVisualStyleBackColor = true;
            this.HaltButton.Click += new System.EventHandler(this.button2_Click);
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
            this.LabelMaxStep.Location = new System.Drawing.Point(209, 36);
            this.LabelMaxStep.Name = "LabelMaxStep";
            this.LabelMaxStep.Size = new System.Drawing.Size(37, 13);
            this.LabelMaxStep.TabIndex = 2;
            this.LabelMaxStep.Text = "30000";
            this.LabelMaxStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progressBar1.Location = new System.Drawing.Point(12, 53);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(234, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 3500;
            // 
            // LabelPosition
            // 
            this.LabelPosition.Location = new System.Drawing.Point(48, 36);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Size = new System.Drawing.Size(160, 13);
            this.LabelPosition.TabIndex = 18;
            this.LabelPosition.Text = "N/A";
            this.LabelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextTemp
            // 
            this.TextTemp.BackColor = System.Drawing.SystemColors.Window;
            this.TextTemp.Location = new System.Drawing.Point(171, 115);
            this.TextTemp.Name = "TextTemp";
            this.TextTemp.ReadOnly = true;
            this.TextTemp.Size = new System.Drawing.Size(35, 20);
            this.TextTemp.TabIndex = 19;
            this.TextTemp.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 410);
            this.ControlBox = false;
            this.Controls.Add(this.TextTemp);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LabelMinPosition);
            this.Controls.Add(this.LabelMaxStep);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CheckTempComp);
            this.Controls.Add(this.TrafficButton);
            this.Controls.Add(this.HaltButton);
            this.Controls.Add(this.SetupButton);
            this.Controls.Add(this.RadioMoving);
            this.Controls.Add(this.LabelPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "ASCOM Focuser Simulator View";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClearLogButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label LabelMinPosition;
        private System.Windows.Forms.Label LabelMaxStep;
        private System.Windows.Forms.RadioButton RadioMoving;
        private System.Windows.Forms.Button SetupButton;
        private System.Windows.Forms.Button HaltButton;
        private System.Windows.Forms.Button TrafficButton;
        private System.Windows.Forms.Timer TimerTempComp;
        private System.Windows.Forms.CheckBox CheckTempComp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CheckOthers;
        private System.Windows.Forms.CheckBox CheckIsMoving;
        private System.Windows.Forms.CheckBox CheckTempRelated;
        private System.Windows.Forms.CheckBox CheckHaltMove;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.PictureBox ClearLogButton;
        private System.Windows.Forms.Label LabelPosition;
        private System.Windows.Forms.TextBox TextTemp;







    }
}

