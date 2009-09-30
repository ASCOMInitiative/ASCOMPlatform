namespace ASCOM.OptecTCF_Driver
{
    partial class LearnWizard3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LearnWizard3));
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.Next_Btn = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FirstTemp_Lb = new System.Windows.Forms.Label();
            this.FirstPos_LB = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.Increment_NUD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Position_LB = new System.Windows.Forms.Label();
            this.Out_Btn = new System.Windows.Forms.Button();
            this.Temp_LB = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.In_Btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Btn.Location = new System.Drawing.Point(199, 311);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 19;
            this.Cancel_Btn.Text = "Cancel";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            // 
            // Next_Btn
            // 
            this.Next_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Next_Btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Next_Btn.Location = new System.Drawing.Point(298, 311);
            this.Next_Btn.Name = "Next_Btn";
            this.Next_Btn.Size = new System.Drawing.Size(75, 23);
            this.Next_Btn.TabIndex = 18;
            this.Next_Btn.Text = "Next";
            this.Next_Btn.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(14, 9);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(304, 31);
            this.textBox2.TabIndex = 17;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Step 3 - Acquire End Datapoint";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.textBox1.Location = new System.Drawing.Point(37, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(343, 163);
            this.textBox1.TabIndex = 16;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(35, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Temp (°C):";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(45, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Position:";
            // 
            // FirstTemp_Lb
            // 
            this.FirstTemp_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FirstTemp_Lb.AutoSize = true;
            this.FirstTemp_Lb.Enabled = false;
            this.FirstTemp_Lb.Location = new System.Drawing.Point(92, 233);
            this.FirstTemp_Lb.Name = "FirstTemp_Lb";
            this.FirstTemp_Lb.Size = new System.Drawing.Size(34, 13);
            this.FirstTemp_Lb.TabIndex = 23;
            this.FirstTemp_Lb.Text = "Temp";
            // 
            // FirstPos_LB
            // 
            this.FirstPos_LB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FirstPos_LB.AutoSize = true;
            this.FirstPos_LB.Enabled = false;
            this.FirstPos_LB.Location = new System.Drawing.Point(92, 252);
            this.FirstPos_LB.Name = "FirstPos_LB";
            this.FirstPos_LB.Size = new System.Drawing.Size(25, 13);
            this.FirstPos_LB.TabIndex = 24;
            this.FirstPos_LB.Text = "Pos";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(35, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "First Datapoint";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.Increment_NUD);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.Position_LB);
            this.panel2.Controls.Add(this.Out_Btn);
            this.panel2.Controls.Add(this.Temp_LB);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.In_Btn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(155, 201);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 96);
            this.panel2.TabIndex = 35;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(55, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 20);
            this.label6.TabIndex = 36;
            this.label6.Text = "Focus Adjust";
            // 
            // Increment_NUD
            // 
            this.Increment_NUD.Location = new System.Drawing.Point(62, 35);
            this.Increment_NUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Increment_NUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Increment_NUD.Name = "Increment_NUD";
            this.Increment_NUD.Size = new System.Drawing.Size(45, 20);
            this.Increment_NUD.TabIndex = 34;
            this.Increment_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(4, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Increment:";
            // 
            // Position_LB
            // 
            this.Position_LB.AutoSize = true;
            this.Position_LB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Position_LB.Location = new System.Drawing.Point(186, 70);
            this.Position_LB.Name = "Position_LB";
            this.Position_LB.Size = new System.Drawing.Size(25, 13);
            this.Position_LB.TabIndex = 31;
            this.Position_LB.Text = "Pos";
            // 
            // Out_Btn
            // 
            this.Out_Btn.Location = new System.Drawing.Point(67, 66);
            this.Out_Btn.Name = "Out_Btn";
            this.Out_Btn.Size = new System.Drawing.Size(49, 21);
            this.Out_Btn.TabIndex = 33;
            this.Out_Btn.Text = "Out";
            this.Out_Btn.UseVisualStyleBackColor = true;
            this.Out_Btn.Click += new System.EventHandler(this.Out_Btn_Click);
            // 
            // Temp_LB
            // 
            this.Temp_LB.AutoSize = true;
            this.Temp_LB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Temp_LB.Location = new System.Drawing.Point(186, 51);
            this.Temp_LB.Name = "Temp_LB";
            this.Temp_LB.Size = new System.Drawing.Size(34, 13);
            this.Temp_LB.TabIndex = 30;
            this.Temp_LB.Text = "Temp";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(118, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Current Datapoint";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(139, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Position:";
            // 
            // In_Btn
            // 
            this.In_Btn.Location = new System.Drawing.Point(7, 66);
            this.In_Btn.Name = "In_Btn";
            this.In_Btn.Size = new System.Drawing.Size(49, 21);
            this.In_Btn.TabIndex = 32;
            this.In_Btn.Text = "In";
            this.In_Btn.UseVisualStyleBackColor = true;
            this.In_Btn.Click += new System.EventHandler(this.In_Btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(129, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Temp (°C):";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.Optec_Logo_large_png;
            this.pictureBox1.Location = new System.Drawing.Point(20, 296);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(124, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // LearnWizard3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 343);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FirstPos_LB);
            this.Controls.Add(this.FirstTemp_Lb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.Next_Btn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LearnWizard3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Temp Comp Slope Learn Wizard";
            this.Load += new System.EventHandler(this.LearnWizard3_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Increment_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Button Next_Btn;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Out_Btn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button In_Btn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label FirstTemp_Lb;
        public System.Windows.Forms.Label FirstPos_LB;
        public System.Windows.Forms.Label Position_LB;
        public System.Windows.Forms.Label Temp_LB;
        private System.Windows.Forms.NumericUpDown Increment_NUD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}