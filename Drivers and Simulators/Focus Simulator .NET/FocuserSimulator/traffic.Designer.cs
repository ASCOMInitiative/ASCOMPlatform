namespace ASCOM.FocuserSimulator
{
    partial class TrafficForm
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckOthers = new System.Windows.Forms.CheckBox();
            this.CheckIsMoving = new System.Windows.Forms.CheckBox();
            this.CheckTempRelated = new System.Windows.Forms.CheckBox();
            this.CheckHaltMove = new System.Windows.Forms.CheckBox();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_cancel;
            this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CloseButton.Location = new System.Drawing.Point(218, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(62, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Close";
            this.CloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(218, 64);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(62, 23);
            this.ClearButton.TabIndex = 3;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckOthers);
            this.groupBox1.Controls.Add(this.CheckIsMoving);
            this.groupBox1.Controls.Add(this.CheckTempRelated);
            this.groupBox1.Controls.Add(this.CheckHaltMove);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 75);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracked events";
            // 
            // CheckOthers
            // 
            this.CheckOthers.AutoSize = true;
            this.CheckOthers.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogOther;
            this.CheckOthers.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogOther", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckOthers.Location = new System.Drawing.Point(125, 48);
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
            this.CheckIsMoving.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogIsMoving", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckIsMoving.Location = new System.Drawing.Point(125, 25);
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
            this.CheckTempRelated.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogTempRelated", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckTempRelated.Location = new System.Drawing.Point(17, 48);
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
            this.CheckHaltMove.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogHaltMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CheckHaltMove.Location = new System.Drawing.Point(17, 25);
            this.CheckHaltMove.Name = "CheckHaltMove";
            this.CheckHaltMove.Size = new System.Drawing.Size(75, 17);
            this.CheckHaltMove.TabIndex = 0;
            this.CheckHaltMove.Text = "Halt/Move";
            this.CheckHaltMove.UseVisualStyleBackColor = true;
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.Window;
            this.LogBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "LogTxt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LogBox.Location = new System.Drawing.Point(12, 93);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(268, 233);
            this.LogBox.TabIndex = 1;
            this.LogBox.Text = global::ASCOM.FocuserSimulator.Properties.Settings.Default.LogTxt;
            // 
            // TrafficForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 338);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.LogBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TrafficForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ASCOM Focuser Simulator traffic viewer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CheckOthers;
        private System.Windows.Forms.CheckBox CheckIsMoving;
        private System.Windows.Forms.CheckBox CheckTempRelated;
        private System.Windows.Forms.CheckBox CheckHaltMove;
    }
}