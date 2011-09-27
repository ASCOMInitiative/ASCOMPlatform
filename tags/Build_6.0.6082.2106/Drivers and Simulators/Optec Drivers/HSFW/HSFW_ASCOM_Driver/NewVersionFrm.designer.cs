namespace ASCOM.HSFW_ASCOM_Driver
{
    partial class NewVersionFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewVersionFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.url_link = new System.Windows.Forms.LinkLabel();
            this.LatestVersion_Lbl = new System.Windows.Forms.Label();
            this.InstalledVer_lbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bookman Old Style", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "A new version of this driver is available for download!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "The latest version number is:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(57, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "The installed version is:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(276, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "You can download the newest version now at:";
            // 
            // url_link
            // 
            this.url_link.AutoSize = true;
            this.url_link.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.url_link.LinkArea = new System.Windows.Forms.LinkArea(0, 26);
            this.url_link.Location = new System.Drawing.Point(296, 109);
            this.url_link.Name = "url_link";
            this.url_link.Size = new System.Drawing.Size(157, 20);
            this.url_link.TabIndex = 3;
            this.url_link.TabStop = true;
            this.url_link.Text = "The Optec Download Site";
            this.url_link.UseCompatibleTextRendering = true;
            this.url_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.url_link_LinkClicked);
            // 
            // LatestVersion_Lbl
            // 
            this.LatestVersion_Lbl.AutoSize = true;
            this.LatestVersion_Lbl.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatestVersion_Lbl.Location = new System.Drawing.Point(201, 49);
            this.LatestVersion_Lbl.Name = "LatestVersion_Lbl";
            this.LatestVersion_Lbl.Size = new System.Drawing.Size(43, 14);
            this.LatestVersion_Lbl.TabIndex = 4;
            this.LatestVersion_Lbl.Text = "label5";
            // 
            // InstalledVer_lbl
            // 
            this.InstalledVer_lbl.AutoSize = true;
            this.InstalledVer_lbl.Font = new System.Drawing.Font("Bookman Old Style", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstalledVer_lbl.Location = new System.Drawing.Point(200, 74);
            this.InstalledVer_lbl.Name = "InstalledVer_lbl";
            this.InstalledVer_lbl.Size = new System.Drawing.Size(43, 14);
            this.InstalledVer_lbl.TabIndex = 5;
            this.InstalledVer_lbl.Text = "label6";
            this.InstalledVer_lbl.Click += new System.EventHandler(this.label6_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(299, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(392, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // NewVersionFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 174);
            this.Controls.Add(this.url_link);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.InstalledVer_lbl);
            this.Controls.Add(this.LatestVersion_Lbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewVersionFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Update Available";
            this.Load += new System.EventHandler(this.NewVersionFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel url_link;
        private System.Windows.Forms.Label LatestVersion_Lbl;
        private System.Windows.Forms.Label InstalledVer_lbl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
    }
}