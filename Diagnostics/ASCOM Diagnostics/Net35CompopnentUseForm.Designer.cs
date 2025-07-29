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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Net35CompopnentUseForm));
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnCreateReport = new System.Windows.Forms.Button();
            this.BtnClearData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RadEnabled = new System.Windows.Forms.RadioButton();
            this.RadOff = new System.Windows.Forms.RadioButton();
            this.BtnViewRegedit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnClose.Location = new System.Drawing.Point(583, 227);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(135, 23);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnCreateReport
            // 
            this.BtnCreateReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCreateReport.Location = new System.Drawing.Point(583, 172);
            this.BtnCreateReport.Name = "BtnCreateReport";
            this.BtnCreateReport.Size = new System.Drawing.Size(136, 23);
            this.BtnCreateReport.TabIndex = 1;
            this.BtnCreateReport.Text = "Create a Report";
            this.BtnCreateReport.UseVisualStyleBackColor = true;
            this.BtnCreateReport.Click += new System.EventHandler(this.BtnCreateReport_Click);
            // 
            // BtnClearData
            // 
            this.BtnClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClearData.Location = new System.Drawing.Point(583, 89);
            this.BtnClearData.Name = "BtnClearData";
            this.BtnClearData.Size = new System.Drawing.Size(136, 23);
            this.BtnClearData.TabIndex = 2;
            this.BtnClearData.Text = "Reset Logging Data";
            this.BtnClearData.UseVisualStyleBackColor = true;
            this.BtnClearData.Click += new System.EventHandler(this.BtnClearData_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RadEnabled);
            this.groupBox1.Controls.Add(this.RadOff);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(583, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Component Logging";
            // 
            // RadEnabled
            // 
            this.RadEnabled.AutoSize = true;
            this.RadEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadEnabled.Location = new System.Drawing.Point(7, 48);
            this.RadEnabled.Name = "RadEnabled";
            this.RadEnabled.Size = new System.Drawing.Size(64, 17);
            this.RadEnabled.TabIndex = 1;
            this.RadEnabled.TabStop = true;
            this.RadEnabled.Text = "Enabled";
            this.RadEnabled.UseVisualStyleBackColor = true;
            this.RadEnabled.CheckedChanged += new System.EventHandler(this.RadEnabled_CheckedChanged);
            // 
            // RadOff
            // 
            this.RadOff.AutoSize = true;
            this.RadOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadOff.Location = new System.Drawing.Point(7, 20);
            this.RadOff.Name = "RadOff";
            this.RadOff.Size = new System.Drawing.Size(39, 17);
            this.RadOff.TabIndex = 0;
            this.RadOff.TabStop = true;
            this.RadOff.Text = "Off";
            this.RadOff.UseVisualStyleBackColor = true;
            this.RadOff.CheckedChanged += new System.EventHandler(this.RadOff_CheckedChanged);
            // 
            // BtnViewRegedit
            // 
            this.BtnViewRegedit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnViewRegedit.Location = new System.Drawing.Point(583, 143);
            this.BtnViewRegedit.Name = "BtnViewRegedit";
            this.BtnViewRegedit.Size = new System.Drawing.Size(136, 23);
            this.BtnViewRegedit.TabIndex = 4;
            this.BtnViewRegedit.Text = "View data in Regedit";
            this.BtnViewRegedit.UseVisualStyleBackColor = true;
            this.BtnViewRegedit.Click += new System.EventHandler(this.BtnViewRegedit_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(502, 104);
            this.label1.TabIndex = 5;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Location = new System.Drawing.Point(12, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Notes:\r\n";
            // 
            // Net35CompopnentUseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnClearData);
            this.Controls.Add(this.BtnViewRegedit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnCreateReport);
            this.Controls.Add(this.BtnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Net35CompopnentUseForm";
            this.Text = "Manage .Net 3.5 Compopnent Use Logging";
            this.Load += new System.EventHandler(this.Net35CompopnentUseForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnCreateReport;
        private System.Windows.Forms.Button BtnClearData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadEnabled;
        private System.Windows.Forms.RadioButton RadOff;
        private System.Windows.Forms.Button BtnViewRegedit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}