namespace ASCOM.OptecTCF_Driver
{
    partial class SetSlopeForm
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
            this.ModeA_RB = new System.Windows.Forms.RadioButton();
            this.ModeB_RB = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Slope_TB = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SetSlope_BTN = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModeA_RB
            // 
            this.ModeA_RB.AutoSize = true;
            this.ModeA_RB.Location = new System.Drawing.Point(34, 24);
            this.ModeA_RB.Name = "ModeA_RB";
            this.ModeA_RB.Size = new System.Drawing.Size(68, 17);
            this.ModeA_RB.TabIndex = 0;
            this.ModeA_RB.TabStop = true;
            this.ModeA_RB.Text = "Mode A (";
            this.ModeA_RB.UseVisualStyleBackColor = true;
            this.ModeA_RB.CheckedChanged += new System.EventHandler(this.ModeChecked_Changed);
            // 
            // ModeB_RB
            // 
            this.ModeB_RB.AutoSize = true;
            this.ModeB_RB.Location = new System.Drawing.Point(34, 48);
            this.ModeB_RB.Name = "ModeB_RB";
            this.ModeB_RB.Size = new System.Drawing.Size(68, 17);
            this.ModeB_RB.TabIndex = 1;
            this.ModeB_RB.TabStop = true;
            this.ModeB_RB.Text = "Mode B (";
            this.ModeB_RB.UseVisualStyleBackColor = true;
            this.ModeB_RB.CheckedChanged += new System.EventHandler(this.ModeChecked_Changed);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Calculated Slope = ";
            // 
            // Slope_TB
            // 
            this.Slope_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Slope_TB.Location = new System.Drawing.Point(152, 29);
            this.Slope_TB.Name = "Slope_TB";
            this.Slope_TB.ReadOnly = true;
            this.Slope_TB.Size = new System.Drawing.Size(70, 22);
            this.Slope_TB.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ModeA_RB);
            this.groupBox1.Controls.Add(this.ModeB_RB);
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Focus Mode";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetSlope_BTN
            // 
            this.SetSlope_BTN.Location = new System.Drawing.Point(92, 166);
            this.SetSlope_BTN.Name = "SetSlope_BTN";
            this.SetSlope_BTN.Size = new System.Drawing.Size(100, 44);
            this.SetSlope_BTN.TabIndex = 6;
            this.SetSlope_BTN.Text = "Set Slope";
            this.SetSlope_BTN.UseVisualStyleBackColor = true;
            this.SetSlope_BTN.Click += new System.EventHandler(this.SetSlope_BTN_Click);
            // 
            // SetSlopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 252);
            this.Controls.Add(this.SetSlope_BTN);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Slope_TB);
            this.Controls.Add(this.label1);
            this.Name = "SetSlopeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manually Set Slope";
            this.Load += new System.EventHandler(this.SetSlopeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton ModeA_RB;
        private System.Windows.Forms.RadioButton ModeB_RB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Slope_TB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button SetSlope_BTN;
    }
}