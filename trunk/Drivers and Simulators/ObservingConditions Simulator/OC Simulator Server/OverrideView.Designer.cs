namespace ASCOM.Simulator
{
    partial class OverrideView
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
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.trkOverride = new System.Windows.Forms.TrackBar();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkOverride)).BeginInit();
            this.SuspendLayout();
            // 
            // chkOverride
            // 
            this.chkOverride.AutoSize = true;
            this.chkOverride.Location = new System.Drawing.Point(9, 8);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(15, 14);
            this.chkOverride.TabIndex = 0;
            this.chkOverride.UseVisualStyleBackColor = true;
            // 
            // trkOverride
            // 
            this.trkOverride.AutoSize = false;
            this.trkOverride.Location = new System.Drawing.Point(117, 4);
            this.trkOverride.Name = "trkOverride";
            this.trkOverride.Size = new System.Drawing.Size(252, 25);
            this.trkOverride.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(81, 8);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "lblFrom";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(369, 8);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(30, 13);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "lblTo";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(439, 5);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(100, 20);
            this.txtValue.TabIndex = 4;
            // 
            // OverrideView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.trkOverride);
            this.Controls.Add(this.chkOverride);
            this.Name = "OverrideView";
            this.Size = new System.Drawing.Size(555, 34);
            ((System.ComponentModel.ISupportInitialize)(this.trkOverride)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkOverride;
        private System.Windows.Forms.TrackBar trkOverride;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.TextBox txtValue;
    }
}
