namespace TempCoeffWizard
{
    partial class Step2Frm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Step2Frm));
            this.Capture_Btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StartDate_Lb = new System.Windows.Forms.Label();
            this.StartTemp_Lb = new System.Windows.Forms.Label();
            this.StartPos_Lb = new System.Windows.Forms.Label();
            this.Abort_Btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Capture_Btn
            // 
            this.Capture_Btn.Location = new System.Drawing.Point(356, 305);
            this.Capture_Btn.Name = "Capture_Btn";
            this.Capture_Btn.Size = new System.Drawing.Size(133, 33);
            this.Capture_Btn.TabIndex = 7;
            this.Capture_Btn.Text = "Capture End Point...";
            this.Capture_Btn.UseVisualStyleBackColor = true;
            this.Capture_Btn.Click += new System.EventHandler(this.Capture_Btn_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.textBox1.Location = new System.Drawing.Point(6, 129);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(480, 139);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(243, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Start Date/Time:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(234, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Start Temperature:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(256, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start Position:";
            // 
            // StartDate_Lb
            // 
            this.StartDate_Lb.AutoSize = true;
            this.StartDate_Lb.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDate_Lb.Location = new System.Drawing.Point(347, 67);
            this.StartDate_Lb.Name = "StartDate_Lb";
            this.StartDate_Lb.Size = new System.Drawing.Size(40, 15);
            this.StartDate_Lb.TabIndex = 10;
            this.StartDate_Lb.Text = "label5";
            // 
            // StartTemp_Lb
            // 
            this.StartTemp_Lb.AutoSize = true;
            this.StartTemp_Lb.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartTemp_Lb.Location = new System.Drawing.Point(347, 85);
            this.StartTemp_Lb.Name = "StartTemp_Lb";
            this.StartTemp_Lb.Size = new System.Drawing.Size(40, 15);
            this.StartTemp_Lb.TabIndex = 11;
            this.StartTemp_Lb.Text = "label5";
            // 
            // StartPos_Lb
            // 
            this.StartPos_Lb.AutoSize = true;
            this.StartPos_Lb.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPos_Lb.Location = new System.Drawing.Point(347, 103);
            this.StartPos_Lb.Name = "StartPos_Lb";
            this.StartPos_Lb.Size = new System.Drawing.Size(40, 15);
            this.StartPos_Lb.TabIndex = 12;
            this.StartPos_Lb.Text = "label5";
            // 
            // Abort_Btn
            // 
            this.Abort_Btn.Location = new System.Drawing.Point(8, 315);
            this.Abort_Btn.Name = "Abort_Btn";
            this.Abort_Btn.Size = new System.Drawing.Size(96, 23);
            this.Abort_Btn.TabIndex = 13;
            this.Abort_Btn.Text = "Abort Wizard";
            this.Abort_Btn.UseVisualStyleBackColor = true;
            this.Abort_Btn.Click += new System.EventHandler(this.Abort_Btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(115, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(349, 21);
            this.label5.TabIndex = 18;
            this.label5.Text = "Optec TCF-S Temperature Coefficient Wizard";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.textBox2.Location = new System.Drawing.Point(12, 61);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(217, 21);
            this.textBox2.TabIndex = 19;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Start point successfully captured!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TempCoeffWizard.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // Step2Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 342);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Abort_Btn);
            this.Controls.Add(this.StartPos_Lb);
            this.Controls.Add(this.StartTemp_Lb);
            this.Controls.Add(this.StartDate_Lb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Capture_Btn);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Step2Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Step 2 - Capture End Point";
            this.Load += new System.EventHandler(this.Step2Frm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Step2Frm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Capture_Btn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label StartDate_Lb;
        private System.Windows.Forms.Label StartTemp_Lb;
        private System.Windows.Forms.Label StartPos_Lb;
        private System.Windows.Forms.Button Abort_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
    }
}