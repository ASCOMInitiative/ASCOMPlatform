namespace ASCOM.OptecTCF_Driver
{
    partial class LearnWizard2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LearnWizard2));
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.Next_Btn = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Out_Btn = new System.Windows.Forms.Button();
            this.In_Btn = new System.Windows.Forms.Button();
            this.Position_LB = new System.Windows.Forms.Label();
            this.Temp_LB = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Btn.Location = new System.Drawing.Point(199, 274);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 8;
            this.Cancel_Btn.Text = "Cancel";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            // 
            // Next_Btn
            // 
            this.Next_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Next_Btn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Next_Btn.Location = new System.Drawing.Point(298, 274);
            this.Next_Btn.Name = "Next_Btn";
            this.Next_Btn.Size = new System.Drawing.Size(75, 23);
            this.Next_Btn.TabIndex = 7;
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
            this.textBox2.TabIndex = 6;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Step 2 - Acquire First Datapoint";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.textBox1.Location = new System.Drawing.Point(37, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(343, 139);
            this.textBox1.TabIndex = 5;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // Out_Btn
            // 
            this.Out_Btn.Location = new System.Drawing.Point(7, 40);
            this.Out_Btn.Name = "Out_Btn";
            this.Out_Btn.Size = new System.Drawing.Size(58, 21);
            this.Out_Btn.TabIndex = 33;
            this.Out_Btn.Text = "Out";
            this.Out_Btn.UseVisualStyleBackColor = true;
            this.Out_Btn.Click += new System.EventHandler(this.Out_Btn_Click);
            // 
            // In_Btn
            // 
            this.In_Btn.Location = new System.Drawing.Point(7, 13);
            this.In_Btn.Name = "In_Btn";
            this.In_Btn.Size = new System.Drawing.Size(58, 21);
            this.In_Btn.TabIndex = 32;
            this.In_Btn.Text = "In";
            this.In_Btn.UseVisualStyleBackColor = true;
            this.In_Btn.Click += new System.EventHandler(this.In_Btn_Click);
            // 
            // Position_LB
            // 
            this.Position_LB.AutoSize = true;
            this.Position_LB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Position_LB.Location = new System.Drawing.Point(136, 40);
            this.Position_LB.Name = "Position_LB";
            this.Position_LB.Size = new System.Drawing.Size(25, 13);
            this.Position_LB.TabIndex = 31;
            this.Position_LB.Text = "Pos";
            // 
            // Temp_LB
            // 
            this.Temp_LB.AutoSize = true;
            this.Temp_LB.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Temp_LB.Location = new System.Drawing.Point(136, 21);
            this.Temp_LB.Name = "Temp_LB";
            this.Temp_LB.Size = new System.Drawing.Size(34, 13);
            this.Temp_LB.TabIndex = 30;
            this.Temp_LB.Text = "Temp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(89, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Position:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(79, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Temp (°C):";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Position_LB);
            this.panel2.Controls.Add(this.Out_Btn);
            this.panel2.Controls.Add(this.Temp_LB);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.In_Btn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(186, 184);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(191, 72);
            this.panel2.TabIndex = 34;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(68, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Current Datapoint";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.Optec_Logo_large_png;
            this.pictureBox1.Location = new System.Drawing.Point(20, 259);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(124, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // LearnWizard2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 306);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.Next_Btn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "LearnWizard2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Slope Learn Wizard";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.Button Out_Btn;
        private System.Windows.Forms.Button In_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer Timer1;
        public System.Windows.Forms.Label Position_LB;
        public System.Windows.Forms.Label Temp_LB;
    }
}