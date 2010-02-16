namespace ASCOM.OptecTCF_Driver
{
    partial class LearnWizard1_5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LearnWizard1_5));
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.Next_Btn = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ModeA_RB = new System.Windows.Forms.RadioButton();
            this.ModeB_RB = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Btn.Location = new System.Drawing.Point(199, 240);
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
            this.Next_Btn.Enabled = false;
            this.Next_Btn.Location = new System.Drawing.Point(298, 240);
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
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(14, 9);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(304, 31);
            this.textBox2.TabIndex = 6;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Step 1 - Select Mode A or B";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.textBox1.Location = new System.Drawing.Point(37, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(343, 124);
            this.textBox1.TabIndex = 5;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.Optec_Logo_large_png;
            this.pictureBox1.Location = new System.Drawing.Point(20, 225);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(124, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // ModeA_RB
            // 
            this.ModeA_RB.AutoSize = true;
            this.ModeA_RB.Location = new System.Drawing.Point(166, 171);
            this.ModeA_RB.Name = "ModeA_RB";
            this.ModeA_RB.Size = new System.Drawing.Size(62, 17);
            this.ModeA_RB.TabIndex = 10;
            this.ModeA_RB.TabStop = true;
            this.ModeA_RB.Text = "Mode A";
            this.ModeA_RB.UseVisualStyleBackColor = true;
            this.ModeA_RB.CheckedChanged += new System.EventHandler(this.ModeSelected);
            // 
            // ModeB_RB
            // 
            this.ModeB_RB.AutoSize = true;
            this.ModeB_RB.Location = new System.Drawing.Point(166, 194);
            this.ModeB_RB.Name = "ModeB_RB";
            this.ModeB_RB.Size = new System.Drawing.Size(62, 17);
            this.ModeB_RB.TabIndex = 11;
            this.ModeB_RB.TabStop = true;
            this.ModeB_RB.Text = "Mode B";
            this.ModeB_RB.UseVisualStyleBackColor = true;
            this.ModeB_RB.CheckedChanged += new System.EventHandler(this.ModeSelected);
            // 
            // LearnWizard1_5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 272);
            this.Controls.Add(this.ModeB_RB);
            this.Controls.Add(this.ModeA_RB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.Next_Btn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "LearnWizard1_5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Temp Comp Slope Learn Wizard";
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
        public System.Windows.Forms.RadioButton ModeA_RB;
        public System.Windows.Forms.RadioButton ModeB_RB;
    }
}