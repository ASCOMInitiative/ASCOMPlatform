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
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.labelStatusData = new System.Windows.Forms.Label();
            this.labelDateTimeData = new System.Windows.Forms.Label();
            this.labelLatitudeData = new System.Windows.Forms.Label();
            this.labelLongitudeData = new System.Windows.Forms.Label();
            this.labelElevationData = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.BackColor = System.Drawing.Color.Transparent;
            this.labelLatitude.ForeColor = System.Drawing.Color.White;
            this.labelLatitude.Location = new System.Drawing.Point(13, 20);
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
            this.labelLongitude.Location = new System.Drawing.Point(13, 47);
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
            this.cmdCancel.Location = new System.Drawing.Point(221, 212);
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
            this.cmdOK.Location = new System.Drawing.Point(221, 182);
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
            this.comboBoxComPort.Location = new System.Drawing.Point(16, 176);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(89, 21);
            this.comboBoxComPort.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(13, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Com Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 200);
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
            this.comboBoxBaudRate.Location = new System.Drawing.Point(16, 216);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(89, 21);
            this.comboBoxBaudRate.TabIndex = 26;
            // 
            // buttonQuery
            // 
            this.buttonQuery.BackColor = System.Drawing.Color.Black;
            this.buttonQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuery.ForeColor = System.Drawing.Color.White;
            this.buttonQuery.Location = new System.Drawing.Point(112, 212);
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
            this.checkBoxUpdateClock.Location = new System.Drawing.Point(112, 176);
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
            this.labelElevation.Location = new System.Drawing.Point(13, 74);
            this.labelElevation.Name = "labelElevation";
            this.labelElevation.Size = new System.Drawing.Size(54, 13);
            this.labelElevation.TabIndex = 29;
            this.labelElevation.Text = "Elevation:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(13, 128);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 31;
            this.labelStatus.Text = "Status:";
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.BackColor = System.Drawing.Color.Transparent;
            this.labelDateTime.ForeColor = System.Drawing.Color.White;
            this.labelDateTime.Location = new System.Drawing.Point(13, 101);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(86, 13);
            this.labelDateTime.TabIndex = 32;
            this.labelDateTime.Text = "GPS Date/Time:";
            // 
            // labelStatusData
            // 
            this.labelStatusData.AutoSize = true;
            this.labelStatusData.ForeColor = System.Drawing.Color.Red;
            this.labelStatusData.Location = new System.Drawing.Point(105, 128);
            this.labelStatusData.Name = "labelStatusData";
            this.labelStatusData.Size = new System.Drawing.Size(35, 13);
            this.labelStatusData.TabIndex = 37;
            this.labelStatusData.Text = "status";
            // 
            // labelDateTimeData
            // 
            this.labelDateTimeData.AutoSize = true;
            this.labelDateTimeData.ForeColor = System.Drawing.Color.Red;
            this.labelDateTimeData.Location = new System.Drawing.Point(105, 101);
            this.labelDateTimeData.Name = "labelDateTimeData";
            this.labelDateTimeData.Size = new System.Drawing.Size(52, 13);
            this.labelDateTimeData.TabIndex = 36;
            this.labelDateTimeData.Text = "date/time";
            // 
            // labelLatitudeData
            // 
            this.labelLatitudeData.AutoSize = true;
            this.labelLatitudeData.ForeColor = System.Drawing.Color.Red;
            this.labelLatitudeData.Location = new System.Drawing.Point(105, 20);
            this.labelLatitudeData.Name = "labelLatitudeData";
            this.labelLatitudeData.Size = new System.Drawing.Size(41, 13);
            this.labelLatitudeData.TabIndex = 33;
            this.labelLatitudeData.Text = "latitude";
            // 
            // labelLongitudeData
            // 
            this.labelLongitudeData.AutoSize = true;
            this.labelLongitudeData.ForeColor = System.Drawing.Color.Red;
            this.labelLongitudeData.Location = new System.Drawing.Point(105, 47);
            this.labelLongitudeData.Name = "labelLongitudeData";
            this.labelLongitudeData.Size = new System.Drawing.Size(50, 13);
            this.labelLongitudeData.TabIndex = 34;
            this.labelLongitudeData.Text = "longitude";
            // 
            // labelElevationData
            // 
            this.labelElevationData.AutoSize = true;
            this.labelElevationData.ForeColor = System.Drawing.Color.Red;
            this.labelElevationData.Location = new System.Drawing.Point(105, 74);
            this.labelElevationData.Name = "labelElevationData";
            this.labelElevationData.Size = new System.Drawing.Size(50, 13);
            this.labelElevationData.TabIndex = 35;
            this.labelElevationData.Text = "elevation";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::ASCOM.GeminiTelescope.Properties.Resources.no_satellite;
            this.pictureBox1.Location = new System.Drawing.Point(263, 1);
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
            this.ClientSize = new System.Drawing.Size(295, 251);
            this.Controls.Add(this.labelStatusData);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelDateTimeData);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelLatitude);
            this.Controls.Add(this.labelDateTime);
            this.Controls.Add(this.checkBoxUpdateClock);
            this.Controls.Add(this.labelElevation);
            this.Controls.Add(this.labelElevationData);
            this.Controls.Add(this.labelLongitude);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.labelLongitudeData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.labelLatitudeData);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GPS";
            this.Load += new System.EventHandler(this.frmGps_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGps_FormClosing);
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
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Label labelStatusData;
        private System.Windows.Forms.Label labelDateTimeData;
        private System.Windows.Forms.Label labelLatitudeData;
        private System.Windows.Forms.Label labelLongitudeData;
        private System.Windows.Forms.Label labelElevationData;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}