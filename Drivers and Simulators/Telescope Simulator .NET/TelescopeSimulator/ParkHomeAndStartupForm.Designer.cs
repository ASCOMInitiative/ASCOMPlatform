namespace ASCOM.Simulator
{
    partial class ParkHomeAndStartupForm
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
            this.BtnOK = new System.Windows.Forms.Button();
            this.txtStartAzimuth = new System.Windows.Forms.TextBox();
            this.cmbStartupMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartAltitude = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtParkAltitude = new System.Windows.Forms.TextBox();
            this.txtParkAzimuth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtHomeAltitude = new System.Windows.Forms.TextBox();
            this.TxtHomeAzimuth = new System.Windows.Forms.TextBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ErrorFlag = new System.Windows.Forms.ErrorProvider(this.components);
            this.BtnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorFlag)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(279, 314);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 0;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // txtStartAzimuth
            // 
            this.txtStartAzimuth.Location = new System.Drawing.Point(39, 108);
            this.txtStartAzimuth.Name = "txtStartAzimuth";
            this.txtStartAzimuth.Size = new System.Drawing.Size(105, 20);
            this.txtStartAzimuth.TabIndex = 1;
            // 
            // cmbStartupMode
            // 
            this.cmbStartupMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartupMode.FormattingEnabled = true;
            this.cmbStartupMode.Location = new System.Drawing.Point(33, 39);
            this.cmbStartupMode.Name = "cmbStartupMode";
            this.cmbStartupMode.Size = new System.Drawing.Size(208, 21);
            this.cmbStartupMode.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(111, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Statup Mode";
            // 
            // txtStartAltitude
            // 
            this.txtStartAltitude.Location = new System.Drawing.Point(39, 134);
            this.txtStartAltitude.Name = "txtStartAltitude";
            this.txtStartAltitude.Size = new System.Drawing.Size(105, 20);
            this.txtStartAltitude.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(163, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Azimuth (deg)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(163, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Altitude (deg)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(163, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Altitude (deg)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(163, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Azimuth (deg)";
            // 
            // txtParkAltitude
            // 
            this.txtParkAltitude.Location = new System.Drawing.Point(39, 222);
            this.txtParkAltitude.Name = "txtParkAltitude";
            this.txtParkAltitude.Size = new System.Drawing.Size(105, 20);
            this.txtParkAltitude.TabIndex = 9;
            // 
            // txtParkAzimuth
            // 
            this.txtParkAzimuth.Location = new System.Drawing.Point(39, 196);
            this.txtParkAzimuth.Name = "txtParkAzimuth";
            this.txtParkAzimuth.Size = new System.Drawing.Size(105, 20);
            this.txtParkAzimuth.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(163, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Altitude (deg)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(163, 286);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Azimuth (deg)";
            // 
            // TxtHomeAltitude
            // 
            this.TxtHomeAltitude.Location = new System.Drawing.Point(39, 309);
            this.TxtHomeAltitude.Name = "TxtHomeAltitude";
            this.TxtHomeAltitude.Size = new System.Drawing.Size(105, 20);
            this.TxtHomeAltitude.TabIndex = 14;
            // 
            // TxtHomeAzimuth
            // 
            this.TxtHomeAzimuth.Location = new System.Drawing.Point(39, 283);
            this.TxtHomeAzimuth.Name = "TxtHomeAzimuth";
            this.TxtHomeAzimuth.Size = new System.Drawing.Size(105, 20);
            this.TxtHomeAzimuth.TabIndex = 13;
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(291, 23);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 18;
            this.picASCOM.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(39, 267);
            this.label8.MinimumSize = new System.Drawing.Size(105, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Home Position";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(39, 180);
            this.label9.MinimumSize = new System.Drawing.Size(105, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Park Position";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(39, 92);
            this.label10.MinimumSize = new System.Drawing.Size(105, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Startup Position";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ErrorFlag
            // 
            this.ErrorFlag.ContainerControl = this;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(279, 285);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 23;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ParkHomeAndStartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(370, 349);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TxtHomeAltitude);
            this.Controls.Add(this.TxtHomeAzimuth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtParkAltitude);
            this.Controls.Add(this.txtParkAzimuth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStartAltitude);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbStartupMode);
            this.Controls.Add(this.txtStartAzimuth);
            this.Controls.Add(this.BtnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParkHomeAndStartupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Startup, Park and Home Options";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorFlag)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.TextBox txtStartAzimuth;
        private System.Windows.Forms.ComboBox cmbStartupMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStartAltitude;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtParkAltitude;
        private System.Windows.Forms.TextBox txtParkAzimuth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtHomeAltitude;
        private System.Windows.Forms.TextBox TxtHomeAzimuth;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider ErrorFlag;
        private System.Windows.Forms.Button BtnCancel;
    }
}