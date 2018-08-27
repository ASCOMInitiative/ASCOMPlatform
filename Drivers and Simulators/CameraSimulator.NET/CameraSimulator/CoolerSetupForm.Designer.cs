namespace ASCOM.Simulator
{
    partial class CoolerSetupForm
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
            this.NumAmbientTemperature = new System.Windows.Forms.NumericUpDown();
            this.NumCCDSetPoint = new System.Windows.Forms.NumericUpDown();
            this.NumCoolerDeltaTMax = new System.Windows.Forms.NumericUpDown();
            this.cmbCoolerModes = new System.Windows.Forms.ComboBox();
            this.NumTimeToSetPoint = new System.Windows.Forms.NumericUpDown();
            this.LblAmbientTemperature = new System.Windows.Forms.Label();
            this.LblCCDSetPoint = new System.Windows.Forms.Label();
            this.LblCoolerDeltaTMax = new System.Windows.Forms.Label();
            this.LblCoolerModes = new System.Windows.Forms.Label();
            this.LblTimeToSetPoint = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.ChkResetToAmbientOnConnect = new System.Windows.Forms.CheckBox();
            this.CoolingHelp = new System.Windows.Forms.HelpProvider();
            this.label2 = new System.Windows.Forms.Label();
            this.BackgroundPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NumAmbientTemperature
            // 
            this.CoolingHelp.SetHelpString(this.NumAmbientTemperature, "");
            this.NumAmbientTemperature.Location = new System.Drawing.Point(31, 28);
            this.NumAmbientTemperature.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.NumAmbientTemperature.Name = "NumAmbientTemperature";
            this.CoolingHelp.SetShowHelp(this.NumAmbientTemperature, true);
            this.NumAmbientTemperature.Size = new System.Drawing.Size(55, 20);
            this.NumAmbientTemperature.TabIndex = 0;
            this.NumAmbientTemperature.ValueChanged += new System.EventHandler(this.NumAmbientTemperature_ValueChanged);
            // 
            // NumCCDSetPoint
            // 
            this.CoolingHelp.SetHelpString(this.NumCCDSetPoint, "");
            this.NumCCDSetPoint.Location = new System.Drawing.Point(32, 54);
            this.NumCCDSetPoint.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.NumCCDSetPoint.Name = "NumCCDSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumCCDSetPoint, true);
            this.NumCCDSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumCCDSetPoint.TabIndex = 3;
            this.NumCCDSetPoint.ValueChanged += new System.EventHandler(this.NumCCDSetPoint_ValueChanged);
            // 
            // NumCoolerDeltaTMax
            // 
            this.CoolingHelp.SetHelpString(this.NumCoolerDeltaTMax, "");
            this.NumCoolerDeltaTMax.Location = new System.Drawing.Point(31, 80);
            this.NumCoolerDeltaTMax.Name = "NumCoolerDeltaTMax";
            this.CoolingHelp.SetShowHelp(this.NumCoolerDeltaTMax, true);
            this.NumCoolerDeltaTMax.Size = new System.Drawing.Size(55, 20);
            this.NumCoolerDeltaTMax.TabIndex = 4;
            this.NumCoolerDeltaTMax.ValueChanged += new System.EventHandler(this.NumCoolerDeltaTMax_ValueChanged);
            // 
            // cmbCoolerModes
            // 
            this.cmbCoolerModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoolerModes.FormattingEnabled = true;
            this.cmbCoolerModes.Location = new System.Drawing.Point(31, 150);
            this.cmbCoolerModes.Name = "cmbCoolerModes";
            this.cmbCoolerModes.Size = new System.Drawing.Size(206, 21);
            this.cmbCoolerModes.TabIndex = 5;
            // 
            // NumTimeToSetPoint
            // 
            this.CoolingHelp.SetHelpString(this.NumTimeToSetPoint, "");
            this.NumTimeToSetPoint.Location = new System.Drawing.Point(31, 106);
            this.NumTimeToSetPoint.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumTimeToSetPoint.Name = "NumTimeToSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumTimeToSetPoint, true);
            this.NumTimeToSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumTimeToSetPoint.TabIndex = 6;
            this.NumTimeToSetPoint.ValueChanged += new System.EventHandler(this.NumTimeToSetPoint_ValueChanged);
            // 
            // LblAmbientTemperature
            // 
            this.LblAmbientTemperature.AutoSize = true;
            this.LblAmbientTemperature.Location = new System.Drawing.Point(93, 30);
            this.LblAmbientTemperature.Name = "LblAmbientTemperature";
            this.LblAmbientTemperature.Size = new System.Drawing.Size(172, 13);
            this.LblAmbientTemperature.TabIndex = 8;
            this.LblAmbientTemperature.Text = "Ambient (heat sink) temperature (C)";
            // 
            // LblCCDSetPoint
            // 
            this.LblCCDSetPoint.AutoSize = true;
            this.LblCCDSetPoint.Location = new System.Drawing.Point(93, 56);
            this.LblCCDSetPoint.Name = "LblCCDSetPoint";
            this.LblCCDSetPoint.Size = new System.Drawing.Size(96, 13);
            this.LblCCDSetPoint.TabIndex = 11;
            this.LblCCDSetPoint.Text = "Cooler set point (C)";
            // 
            // LblCoolerDeltaTMax
            // 
            this.LblCoolerDeltaTMax.AutoSize = true;
            this.LblCoolerDeltaTMax.Location = new System.Drawing.Point(92, 82);
            this.LblCoolerDeltaTMax.Name = "LblCoolerDeltaTMax";
            this.LblCoolerDeltaTMax.Size = new System.Drawing.Size(194, 13);
            this.LblCoolerDeltaTMax.TabIndex = 12;
            this.LblCoolerDeltaTMax.Text = "Cooler maximum delta t from ambient (C)";
            // 
            // LblCoolerModes
            // 
            this.LblCoolerModes.AutoSize = true;
            this.LblCoolerModes.Location = new System.Drawing.Point(243, 153);
            this.LblCoolerModes.Name = "LblCoolerModes";
            this.LblCoolerModes.Size = new System.Drawing.Size(108, 13);
            this.LblCoolerModes.TabIndex = 13;
            this.LblCoolerModes.Text = "Cooler characteristics";
            // 
            // LblTimeToSetPoint
            // 
            this.LblTimeToSetPoint.AutoSize = true;
            this.LblTimeToSetPoint.Location = new System.Drawing.Point(92, 108);
            this.LblTimeToSetPoint.Name = "LblTimeToSetPoint";
            this.LblTimeToSetPoint.Size = new System.Drawing.Size(250, 13);
            this.LblTimeToSetPoint.TabIndex = 14;
            this.LblTimeToSetPoint.Text = "Time to cool from ambient to the set point (seconds)";
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(298, 244);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 15;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // ChkResetToAmbientOnConnect
            // 
            this.ChkResetToAmbientOnConnect.AutoSize = true;
            this.CoolingHelp.SetHelpString(this.ChkResetToAmbientOnConnect, "");
            this.ChkResetToAmbientOnConnect.Location = new System.Drawing.Point(32, 199);
            this.ChkResetToAmbientOnConnect.Name = "ChkResetToAmbientOnConnect";
            this.CoolingHelp.SetShowHelp(this.ChkResetToAmbientOnConnect, true);
            this.ChkResetToAmbientOnConnect.Size = new System.Drawing.Size(257, 17);
            this.ChkResetToAmbientOnConnect.TabIndex = 7;
            this.ChkResetToAmbientOnConnect.Text = "Reset cooler to ambient when cooler is turned on";
            this.ChkResetToAmbientOnConnect.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 249);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Help - Press the ? button and click a control";
            // 
            // BackgroundPictureBox
            // 
            this.BackgroundPictureBox.Location = new System.Drawing.Point(-2, 0);
            this.BackgroundPictureBox.Name = "BackgroundPictureBox";
            this.BackgroundPictureBox.Size = new System.Drawing.Size(388, 280);
            this.BackgroundPictureBox.TabIndex = 17;
            this.BackgroundPictureBox.TabStop = false;
            // 
            // CoolerSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 279);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.LblTimeToSetPoint);
            this.Controls.Add(this.LblCoolerModes);
            this.Controls.Add(this.LblCoolerDeltaTMax);
            this.Controls.Add(this.LblCCDSetPoint);
            this.Controls.Add(this.LblAmbientTemperature);
            this.Controls.Add(this.ChkResetToAmbientOnConnect);
            this.Controls.Add(this.NumTimeToSetPoint);
            this.Controls.Add(this.cmbCoolerModes);
            this.Controls.Add(this.NumCoolerDeltaTMax);
            this.Controls.Add(this.NumCCDSetPoint);
            this.Controls.Add(this.NumAmbientTemperature);
            this.Controls.Add(this.BackgroundPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoolerSetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cooler Configuration";
            this.Load += new System.EventHandler(this.CoolerSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblAmbientTemperature;
        private System.Windows.Forms.Label LblCCDSetPoint;
        private System.Windows.Forms.Label LblCoolerDeltaTMax;
        private System.Windows.Forms.Label LblCoolerModes;
        private System.Windows.Forms.Label LblTimeToSetPoint;
        private System.Windows.Forms.Button BtnClose;
        internal System.Windows.Forms.NumericUpDown NumAmbientTemperature;
        internal System.Windows.Forms.NumericUpDown NumCCDSetPoint;
        internal System.Windows.Forms.NumericUpDown NumCoolerDeltaTMax;
        internal System.Windows.Forms.ComboBox cmbCoolerModes;
        internal System.Windows.Forms.NumericUpDown NumTimeToSetPoint;
        internal System.Windows.Forms.CheckBox ChkResetToAmbientOnConnect;
        private System.Windows.Forms.HelpProvider CoolingHelp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox BackgroundPictureBox;
    }
}