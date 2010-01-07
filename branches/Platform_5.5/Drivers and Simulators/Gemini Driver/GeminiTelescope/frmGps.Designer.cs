namespace ASCOM.GeminiTelescope
{
    partial class frmGps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGps));
            this.labelLatitude = new System.Windows.Forms.Label();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.checkBoxUpdateClock = new System.Windows.Forms.CheckBox();
            this.labelElevation = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.BackColor = System.Drawing.Color.Transparent;
            this.labelLatitude.ForeColor = System.Drawing.Color.White;
            this.labelLatitude.Location = new System.Drawing.Point(17, 9);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(48, 13);
            this.labelLatitude.TabIndex = 0;
            this.labelLatitude.Text = "Latitude:";
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.BackColor = System.Drawing.Color.Transparent;
            this.labelLongitude.ForeColor = System.Drawing.Color.White;
            this.labelLongitude.Location = new System.Drawing.Point(17, 41);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(57, 13);
            this.labelLongitude.TabIndex = 1;
            this.labelLongitude.Text = "Longitude:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Black;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Location = new System.Drawing.Point(221, 161);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Black;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Location = new System.Drawing.Point(221, 131);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(16, 125);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(89, 21);
            this.comboBoxComPort.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(13, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Com Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Baud Rate:";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "4800",
            "9600"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(16, 165);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(89, 21);
            this.comboBoxBaudRate.TabIndex = 26;
            // 
            // buttonQuery
            // 
            this.buttonQuery.BackColor = System.Drawing.Color.Black;
            this.buttonQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuery.ForeColor = System.Drawing.Color.White;
            this.buttonQuery.Location = new System.Drawing.Point(112, 161);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(59, 25);
            this.buttonQuery.TabIndex = 27;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = false;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // checkBoxUpdateClock
            // 
            this.checkBoxUpdateClock.AutoSize = true;
            this.checkBoxUpdateClock.ForeColor = System.Drawing.Color.White;
            this.checkBoxUpdateClock.Location = new System.Drawing.Point(112, 125);
            this.checkBoxUpdateClock.Name = "checkBoxUpdateClock";
            this.checkBoxUpdateClock.Size = new System.Drawing.Size(91, 17);
            this.checkBoxUpdateClock.TabIndex = 28;
            this.checkBoxUpdateClock.Text = "Update Clock";
            this.checkBoxUpdateClock.UseVisualStyleBackColor = true;
            // 
            // labelElevation
            // 
            this.labelElevation.AutoSize = true;
            this.labelElevation.BackColor = System.Drawing.Color.Transparent;
            this.labelElevation.ForeColor = System.Drawing.Color.White;
            this.labelElevation.Location = new System.Drawing.Point(17, 75);
            this.labelElevation.Name = "labelElevation";
            this.labelElevation.Size = new System.Drawing.Size(54, 13);
            this.labelElevation.TabIndex = 29;
            this.labelElevation.Text = "Elevation:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::ASCOM.GeminiTelescope.Properties.Resources.no_satellite;
            this.pictureBox1.Location = new System.Drawing.Point(258, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // frmGps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(292, 202);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelElevation);
            this.Controls.Add(this.checkBoxUpdateClock);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.labelLongitude);
            this.Controls.Add(this.labelLatitude);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GPS";
            this.Load += new System.EventHandler(this.frmGps_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.Label labelLongitude;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.CheckBox checkBoxUpdateClock;
        private System.Windows.Forms.Label labelElevation;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}