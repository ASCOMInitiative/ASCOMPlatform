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
            this.BtnOK = new System.Windows.Forms.Button();
            this.ChkResetToAmbientOnConnect = new System.Windows.Forms.CheckBox();
            this.CoolingHelp = new System.Windows.Forms.HelpProvider();
            this.NumFluctuations = new System.Windows.Forms.NumericUpDown();
            this.NumOvershoot = new System.Windows.Forms.NumericUpDown();
            this.LblHelpText = new System.Windows.Forms.Label();
            this.BackgroundPictureBox = new System.Windows.Forms.PictureBox();
            this.LblFluctuations = new System.Windows.Forms.Label();
            this.LblOvershoot = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.ChkPowerUpState = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFluctuations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOvershoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NumAmbientTemperature
            // 
            this.NumAmbientTemperature.DecimalPlaces = 1;
            this.CoolingHelp.SetHelpString(this.NumAmbientTemperature, "");
            this.NumAmbientTemperature.Location = new System.Drawing.Point(31, 28);
            this.NumAmbientTemperature.Name = "NumAmbientTemperature";
            this.CoolingHelp.SetShowHelp(this.NumAmbientTemperature, true);
            this.NumAmbientTemperature.Size = new System.Drawing.Size(55, 20);
            this.NumAmbientTemperature.TabIndex = 0;
            this.NumAmbientTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumCCDSetPoint
            // 
            this.NumCCDSetPoint.DecimalPlaces = 1;
            this.CoolingHelp.SetHelpString(this.NumCCDSetPoint, "");
            this.NumCCDSetPoint.Location = new System.Drawing.Point(32, 54);
            this.NumCCDSetPoint.Name = "NumCCDSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumCCDSetPoint, true);
            this.NumCCDSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumCCDSetPoint.TabIndex = 3;
            this.NumCCDSetPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumCoolerDeltaTMax
            // 
            this.NumCoolerDeltaTMax.DecimalPlaces = 1;
            this.CoolingHelp.SetHelpString(this.NumCoolerDeltaTMax, "");
            this.NumCoolerDeltaTMax.Location = new System.Drawing.Point(31, 80);
            this.NumCoolerDeltaTMax.Name = "NumCoolerDeltaTMax";
            this.CoolingHelp.SetShowHelp(this.NumCoolerDeltaTMax, true);
            this.NumCoolerDeltaTMax.Size = new System.Drawing.Size(55, 20);
            this.NumCoolerDeltaTMax.TabIndex = 4;
            this.NumCoolerDeltaTMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbCoolerModes
            // 
            this.cmbCoolerModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoolerModes.FormattingEnabled = true;
            this.cmbCoolerModes.Location = new System.Drawing.Point(31, 202);
            this.cmbCoolerModes.Name = "cmbCoolerModes";
            this.cmbCoolerModes.Size = new System.Drawing.Size(206, 21);
            this.cmbCoolerModes.TabIndex = 5;
            // 
            // NumTimeToSetPoint
            // 
            this.NumTimeToSetPoint.DecimalPlaces = 1;
            this.CoolingHelp.SetHelpString(this.NumTimeToSetPoint, "");
            this.NumTimeToSetPoint.Location = new System.Drawing.Point(31, 106);
            this.NumTimeToSetPoint.Name = "NumTimeToSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumTimeToSetPoint, true);
            this.NumTimeToSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumTimeToSetPoint.TabIndex = 6;
            this.NumTimeToSetPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.LblCCDSetPoint.Size = new System.Drawing.Size(93, 13);
            this.LblCCDSetPoint.TabIndex = 11;
            this.LblCCDSetPoint.Text = "Cooler setpoint (C)";
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
            this.LblCoolerModes.Location = new System.Drawing.Point(243, 205);
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
            this.LblTimeToSetPoint.Size = new System.Drawing.Size(247, 13);
            this.LblTimeToSetPoint.TabIndex = 14;
            this.LblTimeToSetPoint.Text = "Time to cool from ambient to the setpoint (seconds)";
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(321, 290);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(59, 23);
            this.BtnOK.TabIndex = 15;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ChkResetToAmbientOnConnect
            // 
            this.ChkResetToAmbientOnConnect.AutoSize = true;
            this.CoolingHelp.SetHelpString(this.ChkResetToAmbientOnConnect, "");
            this.ChkResetToAmbientOnConnect.Location = new System.Drawing.Point(32, 248);
            this.ChkResetToAmbientOnConnect.Name = "ChkResetToAmbientOnConnect";
            this.CoolingHelp.SetShowHelp(this.ChkResetToAmbientOnConnect, true);
            this.ChkResetToAmbientOnConnect.Size = new System.Drawing.Size(257, 17);
            this.ChkResetToAmbientOnConnect.TabIndex = 7;
            this.ChkResetToAmbientOnConnect.Text = "Reset cooler to ambient when cooler is turned on";
            this.ChkResetToAmbientOnConnect.UseVisualStyleBackColor = true;
            // 
            // NumFluctuations
            // 
            this.NumFluctuations.DecimalPlaces = 2;
            this.CoolingHelp.SetHelpString(this.NumFluctuations, "");
            this.NumFluctuations.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumFluctuations.Location = new System.Drawing.Point(31, 158);
            this.NumFluctuations.Name = "NumFluctuations";
            this.CoolingHelp.SetShowHelp(this.NumFluctuations, true);
            this.NumFluctuations.Size = new System.Drawing.Size(55, 20);
            this.NumFluctuations.TabIndex = 18;
            this.NumFluctuations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumOvershoot
            // 
            this.NumOvershoot.DecimalPlaces = 2;
            this.CoolingHelp.SetHelpString(this.NumOvershoot, "");
            this.NumOvershoot.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumOvershoot.Location = new System.Drawing.Point(31, 132);
            this.NumOvershoot.Name = "NumOvershoot";
            this.CoolingHelp.SetShowHelp(this.NumOvershoot, true);
            this.NumOvershoot.Size = new System.Drawing.Size(55, 20);
            this.NumOvershoot.TabIndex = 20;
            this.NumOvershoot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LblHelpText
            // 
            this.LblHelpText.AutoSize = true;
            this.LblHelpText.Location = new System.Drawing.Point(28, 310);
            this.LblHelpText.Name = "LblHelpText";
            this.LblHelpText.Size = new System.Drawing.Size(214, 13);
            this.LblHelpText.TabIndex = 16;
            this.LblHelpText.Text = "Help - Press the ? button and click a control";
            // 
            // BackgroundPictureBox
            // 
            this.BackgroundPictureBox.Location = new System.Drawing.Point(0, -2);
            this.BackgroundPictureBox.Name = "BackgroundPictureBox";
            this.BackgroundPictureBox.Size = new System.Drawing.Size(391, 354);
            this.BackgroundPictureBox.TabIndex = 17;
            this.BackgroundPictureBox.TabStop = false;
            // 
            // LblFluctuations
            // 
            this.LblFluctuations.AutoSize = true;
            this.LblFluctuations.Location = new System.Drawing.Point(92, 160);
            this.LblFluctuations.Name = "LblFluctuations";
            this.LblFluctuations.Size = new System.Drawing.Size(275, 13);
            this.LblFluctuations.TabIndex = 19;
            this.LblFluctuations.Text = "± Random fluctuation in CCD and heat sink temperatures";
            // 
            // LblOvershoot
            // 
            this.LblOvershoot.AutoSize = true;
            this.LblOvershoot.Location = new System.Drawing.Point(93, 134);
            this.LblOvershoot.Name = "LblOvershoot";
            this.LblOvershoot.Size = new System.Drawing.Size(153, 13);
            this.LblOvershoot.TabIndex = 21;
            this.LblOvershoot.Text = "Amount of cooler overshoot (C)";
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(321, 319);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(59, 23);
            this.BtnCancel.TabIndex = 22;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ChkPowerUpState
            // 
            this.ChkPowerUpState.AutoSize = true;
            this.CoolingHelp.SetHelpString(this.ChkPowerUpState, "");
            this.ChkPowerUpState.Location = new System.Drawing.Point(31, 271);
            this.ChkPowerUpState.Name = "ChkPowerUpState";
            this.CoolingHelp.SetShowHelp(this.ChkPowerUpState, true);
            this.ChkPowerUpState.Size = new System.Drawing.Size(140, 17);
            this.ChkPowerUpState.TabIndex = 23;
            this.ChkPowerUpState.Text = "Power up with cooler on";
            this.ChkPowerUpState.UseVisualStyleBackColor = true;
            // 
            // CoolerSetupForm
            // 
            this.AcceptButton = this.BtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(392, 351);
            this.Controls.Add(this.ChkPowerUpState);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.LblOvershoot);
            this.Controls.Add(this.NumOvershoot);
            this.Controls.Add(this.LblFluctuations);
            this.Controls.Add(this.NumFluctuations);
            this.Controls.Add(this.LblHelpText);
            this.Controls.Add(this.BtnOK);
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
            ((System.ComponentModel.ISupportInitialize)(this.NumFluctuations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOvershoot)).EndInit();
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
        private System.Windows.Forms.Button BtnOK;
        internal System.Windows.Forms.NumericUpDown NumAmbientTemperature;
        internal System.Windows.Forms.NumericUpDown NumCCDSetPoint;
        internal System.Windows.Forms.NumericUpDown NumCoolerDeltaTMax;
        internal System.Windows.Forms.ComboBox cmbCoolerModes;
        internal System.Windows.Forms.NumericUpDown NumTimeToSetPoint;
        internal System.Windows.Forms.CheckBox ChkResetToAmbientOnConnect;
        private System.Windows.Forms.HelpProvider CoolingHelp;
        private System.Windows.Forms.Label LblHelpText;
        private System.Windows.Forms.PictureBox BackgroundPictureBox;
        internal System.Windows.Forms.NumericUpDown NumFluctuations;
        private System.Windows.Forms.Label LblFluctuations;
        internal System.Windows.Forms.NumericUpDown NumOvershoot;
        private System.Windows.Forms.Label LblOvershoot;
        private System.Windows.Forms.Button BtnCancel;
        internal System.Windows.Forms.CheckBox ChkPowerUpState;
    }
}