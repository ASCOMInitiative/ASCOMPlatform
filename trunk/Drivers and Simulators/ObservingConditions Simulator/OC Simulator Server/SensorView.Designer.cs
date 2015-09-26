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
            this.chkNotReady = new System.Windows.Forms.CheckBox();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(97, 2);
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(100, 20);
            this.txtMinValue.TabIndex = 0;
            // 
            // txtMaxValue
            // 
            this.txtMaxValue.Location = new System.Drawing.Point(325, 2);
            this.txtMaxValue.Name = "txtMaxValue";
            this.txtMaxValue.Size = new System.Drawing.Size(100, 20);
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
            // chkNotReady
            // 
            this.chkNotReady.AutoSize = true;
            this.chkNotReady.Location = new System.Drawing.Point(537, 5);
            this.chkNotReady.Name = "chkNotReady";
            this.chkNotReady.Size = new System.Drawing.Size(15, 14);
            this.chkNotReady.TabIndex = 3;
            this.chkNotReady.UseVisualStyleBackColor = true;
            // 
            // numDelay
            // 
            this.numDelay.DecimalPlaces = 1;
            this.numDelay.Location = new System.Drawing.Point(649, 3);
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(55, 20);
            this.numDelay.TabIndex = 4;
            this.numDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numDelay);
            this.Controls.Add(this.chkNotReady);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.txtMaxValue);
            this.Controls.Add(this.txtMinValue);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(814, 24);
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox txtMinValue;
        private System.Windows.Forms.TextBox txtMaxValue;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.CheckBox chkNotReady;
        private System.Windows.Forms.NumericUpDown numDelay;
    }
}
