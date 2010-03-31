namespace ASCOM.GeminiTelescope
{
    partial class frmParkPosition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmParkPosition));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbCWD = new System.Windows.Forms.RadioButton();
            this.rbAltAz = new System.Windows.Forms.RadioButton();
            this.rbHome = new System.Windows.Forms.RadioButton();
            this.pbGetPos = new System.Windows.Forms.Button();
            this.txtAlt = new System.Windows.Forms.TextBox();
            this.rbNoSlew = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAz = new System.Windows.Forms.TextBox();
            this.pbOK = new System.Windows.Forms.Button();
            this.pbCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbCWD);
            this.groupBox1.Controls.Add(this.rbAltAz);
            this.groupBox1.Controls.Add(this.rbHome);
            this.groupBox1.Controls.Add(this.pbGetPos);
            this.groupBox1.Controls.Add(this.txtAlt);
            this.groupBox1.Controls.Add(this.rbNoSlew);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAz);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 195);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set Coordinates for Park Position";
            // 
            // rbCWD
            // 
            this.rbCWD.AutoSize = true;
            this.rbCWD.Location = new System.Drawing.Point(16, 65);
            this.rbCWD.Name = "rbCWD";
            this.rbCWD.Size = new System.Drawing.Size(161, 17);
            this.rbCWD.TabIndex = 7;
            this.rbCWD.TabStop = true;
            this.rbCWD.Text = "Slew to CWD before Parking";
            this.rbCWD.UseVisualStyleBackColor = true;
            // 
            // rbAltAz
            // 
            this.rbAltAz.AutoSize = true;
            this.rbAltAz.Location = new System.Drawing.Point(16, 88);
            this.rbAltAz.Name = "rbAltAz";
            this.rbAltAz.Size = new System.Drawing.Size(222, 17);
            this.rbAltAz.TabIndex = 8;
            this.rbAltAz.TabStop = true;
            this.rbAltAz.Text = "Slew to Alt/Az coordinates before Parking";
            this.rbAltAz.UseVisualStyleBackColor = true;
            // 
            // rbHome
            // 
            this.rbHome.AutoSize = true;
            this.rbHome.Location = new System.Drawing.Point(16, 42);
            this.rbHome.Name = "rbHome";
            this.rbHome.Size = new System.Drawing.Size(151, 17);
            this.rbHome.TabIndex = 6;
            this.rbHome.TabStop = true;
            this.rbHome.Text = "Slew Home before Parking";
            this.rbHome.UseVisualStyleBackColor = true;
            // 
            // pbGetPos
            // 
            this.pbGetPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbGetPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbGetPos.ForeColor = System.Drawing.Color.White;
            this.pbGetPos.Location = new System.Drawing.Point(146, 120);
            this.pbGetPos.Name = "pbGetPos";
            this.pbGetPos.Size = new System.Drawing.Size(126, 54);
            this.pbGetPos.TabIndex = 4;
            this.pbGetPos.Text = "Get Current Alt/Az Coordinates";
            this.pbGetPos.UseVisualStyleBackColor = true;
            this.pbGetPos.Click += new System.EventHandler(this.pbGetPos_Click);
            // 
            // txtAlt
            // 
            this.txtAlt.Location = new System.Drawing.Point(63, 120);
            this.txtAlt.Name = "txtAlt";
            this.txtAlt.Size = new System.Drawing.Size(67, 20);
            this.txtAlt.TabIndex = 2;
            // 
            // rbNoSlew
            // 
            this.rbNoSlew.AutoSize = true;
            this.rbNoSlew.Location = new System.Drawing.Point(16, 19);
            this.rbNoSlew.Name = "rbNoSlew";
            this.rbNoSlew.Size = new System.Drawing.Size(135, 17);
            this.rbNoSlew.TabIndex = 5;
            this.rbNoSlew.TabStop = true;
            this.rbNoSlew.Text = "No slew before Parking";
            this.rbNoSlew.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Altitude:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Azimuth:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAz
            // 
            this.txtAz.Location = new System.Drawing.Point(63, 157);
            this.txtAz.Name = "txtAz";
            this.txtAz.Size = new System.Drawing.Size(67, 20);
            this.txtAz.TabIndex = 3;
            // 
            // pbOK
            // 
            this.pbOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.pbOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbOK.ForeColor = System.Drawing.Color.White;
            this.pbOK.Location = new System.Drawing.Point(323, 22);
            this.pbOK.Name = "pbOK";
            this.pbOK.Size = new System.Drawing.Size(75, 23);
            this.pbOK.TabIndex = 1;
            this.pbOK.Text = "OK";
            this.pbOK.UseVisualStyleBackColor = true;
            this.pbOK.Click += new System.EventHandler(this.pbOK_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.pbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbCancel.ForeColor = System.Drawing.Color.White;
            this.pbCancel.Location = new System.Drawing.Point(323, 51);
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.Size = new System.Drawing.Size(75, 23);
            this.pbCancel.TabIndex = 1;
            this.pbCancel.Text = "Cancel";
            this.pbCancel.UseVisualStyleBackColor = true;
            // 
            // frmParkPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(413, 221);
            this.Controls.Add(this.pbCancel);
            this.Controls.Add(this.pbOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmParkPosition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Driver Park Behavior Settings";
            this.Load += new System.EventHandler(this.frmParkPosition_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmParkPosition_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button pbOK;
        private System.Windows.Forms.Button pbCancel;
        private System.Windows.Forms.Button pbGetPos;
        private System.Windows.Forms.TextBox txtAz;
        private System.Windows.Forms.TextBox txtAlt;
        private System.Windows.Forms.RadioButton rbAltAz;
        private System.Windows.Forms.RadioButton rbHome;
        private System.Windows.Forms.RadioButton rbNoSlew;
        private System.Windows.Forms.RadioButton rbCWD;
    }
}