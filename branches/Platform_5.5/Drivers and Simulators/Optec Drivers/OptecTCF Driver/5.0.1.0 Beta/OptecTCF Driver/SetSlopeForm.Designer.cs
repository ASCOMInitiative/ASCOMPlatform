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
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.SetSlope_BTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Delay_NUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Delay_NUD)).BeginInit();
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
            this.label1.Location = new System.Drawing.Point(11, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Temperature Coefficient = ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Slope_TB
            // 
            this.Slope_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Slope_TB.Location = new System.Drawing.Point(242, 32);
            this.Slope_TB.Name = "Slope_TB";
            this.Slope_TB.ReadOnly = true;
            this.Slope_TB.Size = new System.Drawing.Size(70, 22);
            this.Slope_TB.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ModeA_RB);
            this.groupBox1.Controls.Add(this.ModeB_RB);
            this.groupBox1.Location = new System.Drawing.Point(10, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Focus Mode";
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.Location = new System.Drawing.Point(156, 210);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 5;
            this.Cancel_Btn.Text = "Cancel";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // SetSlope_BTN
            // 
            this.SetSlope_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetSlope_BTN.Location = new System.Drawing.Point(243, 210);
            this.SetSlope_BTN.Name = "SetSlope_BTN";
            this.SetSlope_BTN.Size = new System.Drawing.Size(75, 23);
            this.SetSlope_BTN.TabIndex = 6;
            this.SetSlope_BTN.Text = "Save";
            this.SetSlope_BTN.UseVisualStyleBackColor = true;
            this.SetSlope_BTN.Click += new System.EventHandler(this.SetSlope_BTN_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Update Delay:";
            // 
            // Delay_NUD
            // 
            this.Delay_NUD.DecimalPlaces = 2;
            this.Delay_NUD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Delay_NUD.Location = new System.Drawing.Point(178, 164);
            this.Delay_NUD.Maximum = new decimal(new int[] {
            1099,
            0,
            0,
            131072});
            this.Delay_NUD.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.Delay_NUD.Name = "Delay_NUD";
            this.Delay_NUD.Size = new System.Drawing.Size(50, 20);
            this.Delay_NUD.TabIndex = 8;
            this.Delay_NUD.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(234, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "What\'s this?";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // SetSlopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 242);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Delay_NUD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SetSlope_BTN);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Slope_TB);
            this.Controls.Add(this.label1);
            this.Name = "SetSlopeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manually Set Slope";
            this.Load += new System.EventHandler(this.SetSlopeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Delay_NUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton ModeA_RB;
        private System.Windows.Forms.RadioButton ModeB_RB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Slope_TB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Button SetSlope_BTN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Delay_NUD;
        private System.Windows.Forms.Label label3;
    }
}