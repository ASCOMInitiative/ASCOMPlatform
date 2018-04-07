namespace ASCOM.Simulator
{
    partial class SensorView
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
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtMinValue = new System.Windows.Forms.TextBox();
            this.txtMaxValue = new System.Windows.Forms.TextBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.numValueCycleTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValueCycleTime)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(143, 2);
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(60, 20);
            this.txtMinValue.TabIndex = 0;
            // 
            // txtMaxValue
            // 
            this.txtMaxValue.Location = new System.Drawing.Point(219, 2);
            this.txtMaxValue.Name = "txtMaxValue";
            this.txtMaxValue.Size = new System.Drawing.Size(60, 20);
            this.txtMaxValue.TabIndex = 1;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(9, 5);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkEnabled.TabIndex = 2;
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // numDelay
            // 
            this.numDelay.DecimalPlaces = 1;
            this.numDelay.Location = new System.Drawing.Point(618, 3);
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(55, 20);
            this.numDelay.TabIndex = 4;
            this.numDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numValueCycleTime
            // 
            this.numValueCycleTime.Location = new System.Drawing.Point(409, 2);
            this.numValueCycleTime.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numValueCycleTime.Name = "numValueCycleTime";
            this.numValueCycleTime.Size = new System.Drawing.Size(72, 20);
            this.numValueCycleTime.TabIndex = 5;
            this.numValueCycleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numValueCycleTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numValueCycleTime);
            this.Controls.Add(this.numDelay);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.txtMaxValue);
            this.Controls.Add(this.txtMinValue);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(814, 24);
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValueCycleTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox txtMinValue;
        private System.Windows.Forms.TextBox txtMaxValue;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.NumericUpDown numValueCycleTime;
    }
}
