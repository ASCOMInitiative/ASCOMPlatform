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
            this.lblUnits = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(67, 1);
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(100, 20);
            this.txtMinValue.TabIndex = 0;
            // 
            // txtMaxValue
            // 
            this.txtMaxValue.Location = new System.Drawing.Point(357, 2);
            this.txtMaxValue.Name = "txtMaxValue";
            this.txtMaxValue.Size = new System.Drawing.Size(100, 20);
            this.txtMaxValue.TabIndex = 1;
            // 
            // lblUnits
            // 
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(174, 4);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(35, 13);
            this.lblUnits.TabIndex = 2;
            this.lblUnits.Text = "label1";
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblUnits);
            this.Controls.Add(this.txtMaxValue);
            this.Controls.Add(this.txtMinValue);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(814, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox txtMinValue;
        private System.Windows.Forms.TextBox txtMaxValue;
        private System.Windows.Forms.Label lblUnits;
    }
}
