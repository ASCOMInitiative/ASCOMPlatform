namespace ASCOM.Utilities
{
    partial class Net35CompopnentUseForm
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
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnCreateReport = new System.Windows.Forms.Button();
            this.BtnClearData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(392, 229);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(92, 23);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnCreateReport
            // 
            this.BtnCreateReport.Location = new System.Drawing.Point(183, 92);
            this.BtnCreateReport.Name = "BtnCreateReport";
            this.BtnCreateReport.Size = new System.Drawing.Size(92, 23);
            this.BtnCreateReport.TabIndex = 1;
            this.BtnCreateReport.Text = "Create Report";
            this.BtnCreateReport.UseVisualStyleBackColor = true;
            this.BtnCreateReport.Click += new System.EventHandler(this.BtnCreateReport_Click);
            // 
            // BtnClearData
            // 
            this.BtnClearData.Location = new System.Drawing.Point(202, 121);
            this.BtnClearData.Name = "BtnClearData";
            this.BtnClearData.Size = new System.Drawing.Size(92, 41);
            this.BtnClearData.TabIndex = 2;
            this.BtnClearData.Text = "Clear Recorded Data";
            this.BtnClearData.UseVisualStyleBackColor = true;
            // 
            // Net35CompopnentUseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 264);
            this.Controls.Add(this.BtnClearData);
            this.Controls.Add(this.BtnCreateReport);
            this.Controls.Add(this.BtnClose);
            this.Name = "Net35CompopnentUseForm";
            this.Text = "Net35CompopnentUseForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnCreateReport;
        private System.Windows.Forms.Button BtnClearData;
    }
}