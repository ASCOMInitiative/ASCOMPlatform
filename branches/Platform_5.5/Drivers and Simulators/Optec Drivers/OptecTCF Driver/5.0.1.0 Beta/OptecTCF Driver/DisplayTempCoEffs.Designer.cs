namespace ASCOM.OptecTCF_Driver
{
    partial class DisplayTempCoEffs
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ModeATempCoEff_LBL = new System.Windows.Forms.Label();
            this.ModeAName_LBL = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ModeBTempCoEff_LBL = new System.Windows.Forms.Label();
            this.ModeBName_LBL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(184, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(370, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ASCOM.OptecTCF_Driver.Properties.Resources.TCF_S3_TCF_S_1_Medium;
            this.pictureBox2.Location = new System.Drawing.Point(271, 79);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(174, 136);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Temperature Coefficient:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ModeATempCoEff_LBL);
            this.groupBox1.Controls.Add(this.ModeAName_LBL);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(27, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 81);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode A";
            // 
            // ModeATempCoEff_LBL
            // 
            this.ModeATempCoEff_LBL.AutoSize = true;
            this.ModeATempCoEff_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.ModeATempCoEff_LBL.Location = new System.Drawing.Point(156, 46);
            this.ModeATempCoEff_LBL.Name = "ModeATempCoEff_LBL";
            this.ModeATempCoEff_LBL.Size = new System.Drawing.Size(31, 15);
            this.ModeATempCoEff_LBL.TabIndex = 8;
            this.ModeATempCoEff_LBL.Text = "086";
            // 
            // ModeAName_LBL
            // 
            this.ModeAName_LBL.AutoSize = true;
            this.ModeAName_LBL.Location = new System.Drawing.Point(13, 24);
            this.ModeAName_LBL.Name = "ModeAName_LBL";
            this.ModeAName_LBL.Size = new System.Drawing.Size(111, 16);
            this.ModeAName_LBL.TabIndex = 8;
            this.ModeAName_LBL.Text = "Mode A Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ModeBTempCoEff_LBL);
            this.groupBox2.Controls.Add(this.ModeBName_LBL);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(27, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 81);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode B";
            // 
            // ModeBTempCoEff_LBL
            // 
            this.ModeBTempCoEff_LBL.AutoSize = true;
            this.ModeBTempCoEff_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.ModeBTempCoEff_LBL.Location = new System.Drawing.Point(156, 46);
            this.ModeBTempCoEff_LBL.Name = "ModeBTempCoEff_LBL";
            this.ModeBTempCoEff_LBL.Size = new System.Drawing.Size(31, 15);
            this.ModeBTempCoEff_LBL.TabIndex = 8;
            this.ModeBTempCoEff_LBL.Text = "086";
            // 
            // ModeBName_LBL
            // 
            this.ModeBName_LBL.AutoSize = true;
            this.ModeBName_LBL.Location = new System.Drawing.Point(13, 24);
            this.ModeBName_LBL.Name = "ModeBName_LBL";
            this.ModeBName_LBL.Size = new System.Drawing.Size(111, 16);
            this.ModeBName_LBL.TabIndex = 8;
            this.ModeBName_LBL.Text = "Mode A Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Temperature Coefficient:";
            // 
            // DisplayTempCoEffs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(457, 339);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Name = "DisplayTempCoEffs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Temperature Coefficients";
            this.Load += new System.EventHandler(this.DisplayTempCoEffs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label ModeATempCoEff_LBL;
        private System.Windows.Forms.Label ModeAName_LBL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label ModeBTempCoEff_LBL;
        private System.Windows.Forms.Label ModeBName_LBL;
        private System.Windows.Forms.Label label5;
    }
}