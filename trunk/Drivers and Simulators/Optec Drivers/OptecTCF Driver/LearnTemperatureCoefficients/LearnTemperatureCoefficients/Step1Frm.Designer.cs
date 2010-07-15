namespace TempCoeffWizard
{
    partial class Step1Frm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Step1Frm));
            this.Capture_Btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Abort_Btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Capture_Btn
            // 
            this.Capture_Btn.Location = new System.Drawing.Point(356, 305);
            this.Capture_Btn.Name = "Capture_Btn";
            this.Capture_Btn.Size = new System.Drawing.Size(133, 33);
            this.Capture_Btn.TabIndex = 5;
            this.Capture_Btn.Text = "Capture Start Point...";
            this.Capture_Btn.UseVisualStyleBackColor = true;
            this.Capture_Btn.Click += new System.EventHandler(this.Capture_Btn_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(6, 61);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(480, 203);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // Abort_Btn
            // 
            this.Abort_Btn.Location = new System.Drawing.Point(8, 315);
            this.Abort_Btn.Name = "Abort_Btn";
            this.Abort_Btn.Size = new System.Drawing.Size(96, 23);
            this.Abort_Btn.TabIndex = 14;
            this.Abort_Btn.Text = "Abort Wizard";
            this.Abort_Btn.UseVisualStyleBackColor = true;
            this.Abort_Btn.Click += new System.EventHandler(this.Abort_Btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 21);
            this.label1.TabIndex = 18;
            this.label1.Text = "Optec TCF-S Temperature Coefficient Wizard";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TempCoeffWizard.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // Step1Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 342);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Abort_Btn);
            this.Controls.Add(this.Capture_Btn);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Step1Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Step 1 - Focus Scope and Capture Start Point";
            this.Load += new System.EventHandler(this.Step1Frm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Step1Frm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Capture_Btn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Abort_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}