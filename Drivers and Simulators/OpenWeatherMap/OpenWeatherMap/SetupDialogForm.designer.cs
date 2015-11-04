namespace ASCOM.OpenWeatherMap
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageApiKey = new System.Windows.Forms.TabPage();
            this.tabPageSelectSite = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.labelObtainApiKey = new System.Windows.Forms.Label();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.textBoxSiteLongitude = new System.Windows.Forms.TextBox();
            this.labelSiteLongitude = new System.Windows.Forms.Label();
            this.radioButtonLatLong = new System.Windows.Forms.RadioButton();
            this.radioButtonCity = new System.Windows.Forms.RadioButton();
            this.textBoxSiteElevation = new System.Windows.Forms.TextBox();
            this.labelSiteElevation = new System.Windows.Forms.Label();
            this.textBoxSiteLatitude = new System.Windows.Forms.TextBox();
            this.labelSiteLatitude = new System.Windows.Forms.Label();
            this.labelCityName = new System.Windows.Forms.Label();
            this.buttonObtainKey = new System.Windows.Forms.Button();
            this.textBoxCityName = new System.Windows.Forms.TextBox();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxApiUrl = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageApiKey.SuspendLayout();
            this.tabPageSelectSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(356, 291);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 25);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(356, 322);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.OpenWeatherMap.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(367, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // chkTrace
            // 
            this.chkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(356, 268);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageApiKey);
            this.tabControl.Controls.Add(this.tabPageSelectSite);
            this.tabControl.Location = new System.Drawing.Point(1, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(344, 343);
            this.tabControl.TabIndex = 19;
            // 
            // tabPageApiKey
            // 
            this.tabPageApiKey.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageApiKey.Controls.Add(this.label2);
            this.tabPageApiKey.Controls.Add(this.textBoxApiUrl);
            this.tabPageApiKey.Controls.Add(this.labelApiKey);
            this.tabPageApiKey.Controls.Add(this.textBoxApiKey);
            this.tabPageApiKey.Controls.Add(this.buttonObtainKey);
            this.tabPageApiKey.Controls.Add(this.labelObtainApiKey);
            this.tabPageApiKey.Location = new System.Drawing.Point(4, 22);
            this.tabPageApiKey.Name = "tabPageApiKey";
            this.tabPageApiKey.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApiKey.Size = new System.Drawing.Size(336, 317);
            this.tabPageApiKey.TabIndex = 0;
            this.tabPageApiKey.Text = "Get API Key";
            // 
            // tabPageSelectSite
            // 
            this.tabPageSelectSite.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSelectSite.Controls.Add(this.textBoxCityName);
            this.tabPageSelectSite.Controls.Add(this.radioButtonLatLong);
            this.tabPageSelectSite.Controls.Add(this.radioButtonCity);
            this.tabPageSelectSite.Controls.Add(this.textBoxSiteElevation);
            this.tabPageSelectSite.Controls.Add(this.labelSiteElevation);
            this.tabPageSelectSite.Controls.Add(this.textBoxSiteLatitude);
            this.tabPageSelectSite.Controls.Add(this.labelSiteLatitude);
            this.tabPageSelectSite.Controls.Add(this.labelCityName);
            this.tabPageSelectSite.Controls.Add(this.buttonCheck);
            this.tabPageSelectSite.Controls.Add(this.textBoxSiteLongitude);
            this.tabPageSelectSite.Controls.Add(this.labelSiteLongitude);
            this.tabPageSelectSite.Controls.Add(this.label1);
            this.tabPageSelectSite.Controls.Add(this.dataGridView);
            this.tabPageSelectSite.Location = new System.Drawing.Point(4, 22);
            this.tabPageSelectSite.Name = "tabPageSelectSite";
            this.tabPageSelectSite.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSelectSite.Size = new System.Drawing.Size(336, 317);
            this.tabPageSelectSite.TabIndex = 1;
            this.tabPageSelectSite.Text = "Select Site";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colLat,
            this.colLon});
            this.dataGridView.Location = new System.Drawing.Point(1, 144);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(328, 173);
            this.dataGridView.TabIndex = 18;
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colLat
            // 
            this.colLat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colLat.HeaderText = "Latitude";
            this.colLat.Name = "colLat";
            this.colLat.ReadOnly = true;
            this.colLat.Width = 70;
            // 
            // colLon
            // 
            this.colLon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colLon.HeaderText = "Longitude";
            this.colLon.Name = "colLon";
            this.colLon.ReadOnly = true;
            this.colLon.Width = 79;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 46);
            this.label1.TabIndex = 19;
            this.label1.Text = "Set the City or Location and click on \"Check\" to find a location in\r\nthe OpenWeat" +
    "herMap list.  Choose the one you want and click on \"OK\" to select it.\r\n";
            // 
            // labelObtainApiKey
            // 
            this.labelObtainApiKey.Location = new System.Drawing.Point(6, 3);
            this.labelObtainApiKey.Name = "labelObtainApiKey";
            this.labelObtainApiKey.Size = new System.Drawing.Size(270, 68);
            this.labelObtainApiKey.TabIndex = 19;
            this.labelObtainApiKey.Text = "You need to obtain an API key from OpenWeatherMap to use this.\r\nClick on Obtain K" +
    "ey to go to the OpenWeatherMap site, obtain a key and copy it to the API Key tex" +
    "t box.\r\n";
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(216, 59);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(61, 23);
            this.buttonCheck.TabIndex = 22;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // textBoxSiteLongitude
            // 
            this.textBoxSiteLongitude.Location = new System.Drawing.Point(87, 61);
            this.textBoxSiteLongitude.Name = "textBoxSiteLongitude";
            this.textBoxSiteLongitude.Size = new System.Drawing.Size(76, 20);
            this.textBoxSiteLongitude.TabIndex = 21;
            // 
            // labelSiteLongitude
            // 
            this.labelSiteLongitude.AutoSize = true;
            this.labelSiteLongitude.Location = new System.Drawing.Point(6, 64);
            this.labelSiteLongitude.Name = "labelSiteLongitude";
            this.labelSiteLongitude.Size = new System.Drawing.Size(75, 13);
            this.labelSiteLongitude.TabIndex = 20;
            this.labelSiteLongitude.Text = "Site Longitude";
            // 
            // radioButtonLatLong
            // 
            this.radioButtonLatLong.AutoSize = true;
            this.radioButtonLatLong.Location = new System.Drawing.Point(169, 48);
            this.radioButtonLatLong.Name = "radioButtonLatLong";
            this.radioButtonLatLong.Size = new System.Drawing.Size(14, 13);
            this.radioButtonLatLong.TabIndex = 30;
            this.radioButtonLatLong.TabStop = true;
            this.radioButtonLatLong.UseVisualStyleBackColor = true;
            this.radioButtonLatLong.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonCity
            // 
            this.radioButtonCity.AutoSize = true;
            this.radioButtonCity.Location = new System.Drawing.Point(169, 9);
            this.radioButtonCity.Name = "radioButtonCity";
            this.radioButtonCity.Size = new System.Drawing.Size(14, 13);
            this.radioButtonCity.TabIndex = 29;
            this.radioButtonCity.TabStop = true;
            this.radioButtonCity.UseVisualStyleBackColor = true;
            this.radioButtonCity.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // textBoxSiteElevation
            // 
            this.textBoxSiteElevation.Location = new System.Drawing.Point(233, 22);
            this.textBoxSiteElevation.Name = "textBoxSiteElevation";
            this.textBoxSiteElevation.Size = new System.Drawing.Size(44, 20);
            this.textBoxSiteElevation.TabIndex = 28;
            // 
            // labelSiteElevation
            // 
            this.labelSiteElevation.AutoSize = true;
            this.labelSiteElevation.Location = new System.Drawing.Point(199, 6);
            this.labelSiteElevation.Name = "labelSiteElevation";
            this.labelSiteElevation.Size = new System.Drawing.Size(89, 13);
            this.labelSiteElevation.TabIndex = 27;
            this.labelSiteElevation.Text = "Site Elevation (m)";
            // 
            // textBoxSiteLatitude
            // 
            this.textBoxSiteLatitude.Location = new System.Drawing.Point(87, 32);
            this.textBoxSiteLatitude.Name = "textBoxSiteLatitude";
            this.textBoxSiteLatitude.Size = new System.Drawing.Size(76, 20);
            this.textBoxSiteLatitude.TabIndex = 26;
            // 
            // labelSiteLatitude
            // 
            this.labelSiteLatitude.AutoSize = true;
            this.labelSiteLatitude.Location = new System.Drawing.Point(6, 35);
            this.labelSiteLatitude.Name = "labelSiteLatitude";
            this.labelSiteLatitude.Size = new System.Drawing.Size(66, 13);
            this.labelSiteLatitude.TabIndex = 25;
            this.labelSiteLatitude.Text = "Site Latitude";
            // 
            // labelCityName
            // 
            this.labelCityName.AutoSize = true;
            this.labelCityName.Location = new System.Drawing.Point(6, 9);
            this.labelCityName.Name = "labelCityName";
            this.labelCityName.Size = new System.Drawing.Size(55, 13);
            this.labelCityName.TabIndex = 23;
            this.labelCityName.Text = "City Name";
            // 
            // buttonObtainKey
            // 
            this.buttonObtainKey.Location = new System.Drawing.Point(9, 64);
            this.buttonObtainKey.Name = "buttonObtainKey";
            this.buttonObtainKey.Size = new System.Drawing.Size(73, 23);
            this.buttonObtainKey.TabIndex = 23;
            this.buttonObtainKey.Text = "Obtain Key";
            this.buttonObtainKey.UseVisualStyleBackColor = true;
            this.buttonObtainKey.Click += new System.EventHandler(this.buttonObtainKey_Click);
            // 
            // textBoxCityName
            // 
            this.textBoxCityName.Location = new System.Drawing.Point(67, 6);
            this.textBoxCityName.Name = "textBoxCityName";
            this.textBoxCityName.Size = new System.Drawing.Size(96, 20);
            this.textBoxCityName.TabIndex = 31;
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(67, 93);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(253, 20);
            this.textBoxApiKey.TabIndex = 25;
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(16, 96);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(45, 13);
            this.labelApiKey.TabIndex = 26;
            this.labelApiKey.Text = "API Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "API URL";
            // 
            // textBoxApiUrl
            // 
            this.textBoxApiUrl.Location = new System.Drawing.Point(67, 146);
            this.textBoxApiUrl.Name = "textBoxApiUrl";
            this.textBoxApiUrl.Size = new System.Drawing.Size(253, 20);
            this.textBoxApiUrl.TabIndex = 27;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 354);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenWeatherMap Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageApiKey.ResumeLayout(false);
            this.tabPageApiKey.PerformLayout();
            this.tabPageSelectSite.ResumeLayout(false);
            this.tabPageSelectSite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageApiKey;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Button buttonObtainKey;
        private System.Windows.Forms.Label labelObtainApiKey;
        private System.Windows.Forms.TabPage tabPageSelectSite;
        private System.Windows.Forms.TextBox textBoxCityName;
        private System.Windows.Forms.RadioButton radioButtonLatLong;
        private System.Windows.Forms.RadioButton radioButtonCity;
        private System.Windows.Forms.TextBox textBoxSiteElevation;
        private System.Windows.Forms.Label labelSiteElevation;
        private System.Windows.Forms.TextBox textBoxSiteLatitude;
        private System.Windows.Forms.Label labelSiteLatitude;
        private System.Windows.Forms.Label labelCityName;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.TextBox textBoxSiteLongitude;
        private System.Windows.Forms.Label labelSiteLongitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxApiUrl;
    }
}