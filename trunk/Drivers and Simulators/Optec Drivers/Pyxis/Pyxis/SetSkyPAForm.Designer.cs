namespace ASCOM.Pyxis
{
    partial class SetSkyPAForm
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
            this.CancelBTN = new System.Windows.Forms.Button();
            this.OK_BTN = new System.Windows.Forms.Button();
            this.Sec_TB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Min_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Degrees_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TotalLBL = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelBTN
            // 
            this.CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBTN.Location = new System.Drawing.Point(174, 104);
            this.CancelBTN.Name = "CancelBTN";
            this.CancelBTN.Size = new System.Drawing.Size(75, 23);
            this.CancelBTN.TabIndex = 20;
            this.CancelBTN.Text = "Cancel";
            this.CancelBTN.UseVisualStyleBackColor = true;
            // 
            // OK_BTN
            // 
            this.OK_BTN.Location = new System.Drawing.Point(276, 104);
            this.OK_BTN.Name = "OK_BTN";
            this.OK_BTN.Size = new System.Drawing.Size(75, 23);
            this.OK_BTN.TabIndex = 19;
            this.OK_BTN.Text = "OK";
            this.OK_BTN.UseVisualStyleBackColor = true;
            this.OK_BTN.Click += new System.EventHandler(this.OK_BTN_Click);
            // 
            // Sec_TB
            // 
            this.Sec_TB.Location = new System.Drawing.Point(270, 67);
            this.Sec_TB.Name = "Sec_TB";
            this.Sec_TB.Size = new System.Drawing.Size(41, 20);
            this.Sec_TB.TabIndex = 17;
            this.Sec_TB.Text = "00";
            this.Sec_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sec_TB.Leave += new System.EventHandler(this.Sec_TB_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "\"";
            // 
            // Min_TB
            // 
            this.Min_TB.Location = new System.Drawing.Point(215, 67);
            this.Min_TB.Name = "Min_TB";
            this.Min_TB.Size = new System.Drawing.Size(41, 20);
            this.Min_TB.TabIndex = 15;
            this.Min_TB.Text = "00";
            this.Min_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Min_TB.Leave += new System.EventHandler(this.Min_TB_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "\'";
            // 
            // Degrees_TB
            // 
            this.Degrees_TB.Location = new System.Drawing.Point(158, 67);
            this.Degrees_TB.Name = "Degrees_TB";
            this.Degrees_TB.Size = new System.Drawing.Size(41, 20);
            this.Degrees_TB.TabIndex = 13;
            this.Degrees_TB.Text = "00";
            this.Degrees_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Degrees_TB.Leave += new System.EventHandler(this.Degrees_TB_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Set Current Sky PA to:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(357, 49);
            this.label1.TabIndex = 11;
            this.label1.Text = "Enter the current Sky Position Angle. This will not cause the rotator to move.  C" +
                "hanging the Sky PA configures the program so that the current device PA is equal" +
                " to the value entered.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "°";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TotalLBL});
            this.statusStrip1.Location = new System.Drawing.Point(0, 136);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(360, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TotalLBL
            // 
            this.TotalLBL.Name = "TotalLBL";
            this.TotalLBL.Size = new System.Drawing.Size(45, 17);
            this.TotalLBL.Text = "000.00°";
            // 
            // SetSkyPAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 158);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelBTN);
            this.Controls.Add(this.OK_BTN);
            this.Controls.Add(this.Sec_TB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Min_TB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Degrees_TB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "SetSkyPAForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Sky PA";
            this.Load += new System.EventHandler(this.SetSkyPAForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBTN;
        private System.Windows.Forms.Button OK_BTN;
        private System.Windows.Forms.TextBox Sec_TB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Min_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Degrees_TB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel TotalLBL;
    }
}