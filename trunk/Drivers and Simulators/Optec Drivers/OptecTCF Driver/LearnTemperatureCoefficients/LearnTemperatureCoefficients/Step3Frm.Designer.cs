namespace TempCoeffWizard
{
    partial class Step3Frm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Step3Frm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Capture_Btn = new System.Windows.Forms.Button();
            this.Abort_Btn = new System.Windows.Forms.Button();
            this.ModeA_rb = new System.Windows.Forms.RadioButton();
            this.ModeB_rb = new System.Windows.Forms.RadioButton();
            this.Both_rb = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "The calculated temp comp coefficient is...";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.textBox1.Location = new System.Drawing.Point(6, 116);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(480, 43);
            this.textBox1.TabIndex = 14;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Finally, select which temperature compensation mode you would like this coefficie" +
                "nt to be associated with and then press \'Finish\' to store the coefficient in the" +
                " device\'s memory.";
            // 
            // Capture_Btn
            // 
            this.Capture_Btn.Location = new System.Drawing.Point(356, 305);
            this.Capture_Btn.Name = "Capture_Btn";
            this.Capture_Btn.Size = new System.Drawing.Size(133, 33);
            this.Capture_Btn.TabIndex = 15;
            this.Capture_Btn.Text = "Finish";
            this.Capture_Btn.UseVisualStyleBackColor = true;
            this.Capture_Btn.Click += new System.EventHandler(this.Capture_Btn_Click);
            // 
            // Abort_Btn
            // 
            this.Abort_Btn.Location = new System.Drawing.Point(8, 315);
            this.Abort_Btn.Name = "Abort_Btn";
            this.Abort_Btn.Size = new System.Drawing.Size(96, 23);
            this.Abort_Btn.TabIndex = 16;
            this.Abort_Btn.Text = "Abort Wizard";
            this.Abort_Btn.UseVisualStyleBackColor = true;
            this.Abort_Btn.Click += new System.EventHandler(this.Abort_Btn_Click);
            // 
            // ModeA_rb
            // 
            this.ModeA_rb.AutoSize = true;
            this.ModeA_rb.Location = new System.Drawing.Point(37, 175);
            this.ModeA_rb.Name = "ModeA_rb";
            this.ModeA_rb.Size = new System.Drawing.Size(85, 17);
            this.ModeA_rb.TabIndex = 17;
            this.ModeA_rb.TabStop = true;
            this.ModeA_rb.Text = "radioButton1";
            this.ModeA_rb.UseVisualStyleBackColor = true;
            // 
            // ModeB_rb
            // 
            this.ModeB_rb.AutoSize = true;
            this.ModeB_rb.Location = new System.Drawing.Point(37, 198);
            this.ModeB_rb.Name = "ModeB_rb";
            this.ModeB_rb.Size = new System.Drawing.Size(85, 17);
            this.ModeB_rb.TabIndex = 17;
            this.ModeB_rb.TabStop = true;
            this.ModeB_rb.Text = "radioButton1";
            this.ModeB_rb.UseVisualStyleBackColor = true;
            // 
            // Both_rb
            // 
            this.Both_rb.AutoSize = true;
            this.Both_rb.Location = new System.Drawing.Point(37, 221);
            this.Both_rb.Name = "Both_rb";
            this.Both_rb.Size = new System.Drawing.Size(82, 17);
            this.Both_rb.TabIndex = 17;
            this.Both_rb.TabStop = true;
            this.Both_rb.Text = "Both Modes";
            this.Both_rb.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TempCoeffWizard.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(115, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(349, 21);
            this.label2.TabIndex = 19;
            this.label2.Text = "Optec TCF-S Temperature Coefficient Wizard";
            // 
            // Step3Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 342);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Both_rb);
            this.Controls.Add(this.ModeB_rb);
            this.Controls.Add(this.ModeA_rb);
            this.Controls.Add(this.Abort_Btn);
            this.Controls.Add(this.Capture_Btn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Step3Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Step 3 - Assign Coefficient";
            this.Load += new System.EventHandler(this.Step3Frm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Step3Frm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Capture_Btn;
        private System.Windows.Forms.Button Abort_Btn;
        private System.Windows.Forms.RadioButton ModeA_rb;
        private System.Windows.Forms.RadioButton ModeB_rb;
        private System.Windows.Forms.RadioButton Both_rb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
    }
}