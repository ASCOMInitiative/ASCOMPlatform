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
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.ChkResetToAmbientOnConnect = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).BeginInit();
            this.SuspendLayout();
            // 
            // NumAmbientTemperature
            // 
            this.NumAmbientTemperature.Location = new System.Drawing.Point(31, 28);
            this.NumAmbientTemperature.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.NumAmbientTemperature.Name = "NumAmbientTemperature";
            this.NumAmbientTemperature.Size = new System.Drawing.Size(55, 20);
            this.NumAmbientTemperature.TabIndex = 0;
            this.NumAmbientTemperature.ValueChanged += new System.EventHandler(this.NumAmbientTemperature_ValueChanged);
            // 
            // NumCCDSetPoint
            // 
            this.NumCCDSetPoint.Location = new System.Drawing.Point(32, 54);
            this.NumCCDSetPoint.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.NumCCDSetPoint.Name = "NumCCDSetPoint";
            this.NumCCDSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumCCDSetPoint.TabIndex = 3;
            this.NumCCDSetPoint.ValueChanged += new System.EventHandler(this.NumCCDSetPoint_ValueChanged);
            // 
            // NumCoolerDeltaTMax
            // 
            this.NumCoolerDeltaTMax.Location = new System.Drawing.Point(31, 80);
            this.NumCoolerDeltaTMax.Name = "NumCoolerDeltaTMax";
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
            this.NumTimeToSetPoint.Location = new System.Drawing.Point(31, 106);
            this.NumTimeToSetPoint.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumTimeToSetPoint.Name = "NumTimeToSetPoint";
            this.NumTimeToSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumTimeToSetPoint.TabIndex = 6;
            this.NumTimeToSetPoint.ValueChanged += new System.EventHandler(this.NumTimeToSetPoint_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Ambient (heat sink) temperature (C)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Cooler set point (C)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(92, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(194, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Cooler maximum delta t from ambient (C)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(243, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Cooler characteristics";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(92, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Time to cool from ambient to the set point (seconds)";
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
            this.ChkResetToAmbientOnConnect.Location = new System.Drawing.Point(32, 199);
            this.ChkResetToAmbientOnConnect.Name = "ChkResetToAmbientOnConnect";
            this.ChkResetToAmbientOnConnect.Size = new System.Drawing.Size(257, 17);
            this.ChkResetToAmbientOnConnect.TabIndex = 7;
            this.ChkResetToAmbientOnConnect.Text = "Reset cooler to ambient when cooler is turned on";
            this.ChkResetToAmbientOnConnect.UseVisualStyleBackColor = true;
            // 
            // CoolerSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 279);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChkResetToAmbientOnConnect);
            this.Controls.Add(this.NumTimeToSetPoint);
            this.Controls.Add(this.cmbCoolerModes);
            this.Controls.Add(this.NumCoolerDeltaTMax);
            this.Controls.Add(this.NumCCDSetPoint);
            this.Controls.Add(this.NumAmbientTemperature);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoolerSetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cooler Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnClose;
        internal System.Windows.Forms.NumericUpDown NumAmbientTemperature;
        internal System.Windows.Forms.NumericUpDown NumCCDSetPoint;
        internal System.Windows.Forms.NumericUpDown NumCoolerDeltaTMax;
        internal System.Windows.Forms.ComboBox cmbCoolerModes;
        internal System.Windows.Forms.NumericUpDown NumTimeToSetPoint;
        internal System.Windows.Forms.CheckBox ChkResetToAmbientOnConnect;
    }
}