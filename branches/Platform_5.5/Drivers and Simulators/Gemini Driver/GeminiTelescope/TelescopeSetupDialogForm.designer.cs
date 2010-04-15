namespace ASCOM.GeminiTelescope
{
    partial class TelescopeSetupDialogForm
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
            System.Windows.Forms.Button buttonGps;
            System.Windows.Forms.Button pbGeminiSettings;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelescopeSetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxSites = new System.Windows.Forms.ComboBox();
            this.pbSiteConfig = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxLongitudeMinutes = new System.Windows.Forms.TextBox();
            this.textBoxLatitudeMinutes = new System.Windows.Forms.TextBox();
            this.textBoxLatitudeDegrees = new System.Windows.Forms.TextBox();
            this.comboBoxLatitude = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxElevation = new System.Windows.Forms.TextBox();
            this.comboBoxLongitude = new System.Windows.Forms.ComboBox();
            this.textBoxLongitudeDegrees = new System.Windows.Forms.TextBox();
            this.pbSetSiteNow = new System.Windows.Forms.Button();
            this.checkBoxUseDriverSite = new System.Windows.Forms.CheckBox();
            this.comboBoxTZ = new System.Windows.Forms.ComboBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pbSetTimeNow = new System.Windows.Forms.Button();
            this.checkBoxUseDriverTime = new System.Windows.Forms.CheckBox();
            this.labelUtc = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxShowHandbox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.buttonVirtualPort = new System.Windows.Forms.Button();
            this.chkPortScan = new System.Windows.Forms.CheckBox();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonPrompt = new System.Windows.Forms.RadioButton();
            this.radioButtonColdStart = new System.Windows.Forms.RadioButton();
            this.radioButtonWarmStart = new System.Windows.Forms.RadioButton();
            this.radioButtonWarmRestart = new System.Windows.Forms.RadioButton();
            this.chkJoystick = new System.Windows.Forms.CheckBox();
            this.cmbJoystick = new System.Windows.Forms.ComboBox();
            this.chkVoice = new System.Windows.Forms.CheckBox();
            this.btnJoysticConfig = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbLogging = new System.Windows.Forms.ComboBox();
            this.chkAsyncPulseGuide = new System.Windows.Forms.CheckBox();
            buttonGps = new System.Windows.Forms.Button();
            pbGeminiSettings = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGps
            // 
            buttonGps.BackColor = System.Drawing.Color.Black;
            buttonGps.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            buttonGps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonGps.ForeColor = System.Drawing.Color.White;
            buttonGps.Location = new System.Drawing.Point(263, 419);
            buttonGps.Name = "buttonGps";
            buttonGps.Size = new System.Drawing.Size(105, 23);
            buttonGps.TabIndex = 23;
            buttonGps.Text = "GPS Settings...";
            buttonGps.UseVisualStyleBackColor = false;
            buttonGps.Click += new System.EventHandler(this.buttonGps_Click);
            // 
            // pbGeminiSettings
            // 
            pbGeminiSettings.BackColor = System.Drawing.Color.Black;
            pbGeminiSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            pbGeminiSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            pbGeminiSettings.ForeColor = System.Drawing.Color.White;
            pbGeminiSettings.Location = new System.Drawing.Point(263, 390);
            pbGeminiSettings.Name = "pbGeminiSettings";
            pbGeminiSettings.Size = new System.Drawing.Size(105, 23);
            pbGeminiSettings.TabIndex = 30;
            pbGeminiSettings.Text = "Gemini Settings...";
            pbGeminiSettings.UseVisualStyleBackColor = false;
            pbGeminiSettings.Click += new System.EventHandler(this.pbGeminiSettings_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.BackColor = System.Drawing.Color.Black;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Location = new System.Drawing.Point(430, 398);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.BackColor = System.Drawing.Color.Black;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Location = new System.Drawing.Point(430, 427);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 199);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Site Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.31422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.79664F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.63332F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.25581F));
            this.tableLayoutPanel1.Controls.Add(this.comboBoxSites, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbSiteConfig, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeMinutes, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeMinutes, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeDegrees, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLatitude, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxElevation, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLongitude, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeDegrees, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.pbSetSiteNow, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxUseDriverSite, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTZ, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(217, 180);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBoxSites
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxSites, 3);
            this.comboBoxSites.DisplayMember = "Name";
            this.comboBoxSites.FormattingEnabled = true;
            this.comboBoxSites.Location = new System.Drawing.Point(70, 3);
            this.comboBoxSites.Name = "comboBoxSites";
            this.comboBoxSites.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSites.TabIndex = 38;
            // 
            // pbSiteConfig
            // 
            this.pbSiteConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pbSiteConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbSiteConfig.Location = new System.Drawing.Point(3, 3);
            this.pbSiteConfig.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pbSiteConfig.Name = "pbSiteConfig";
            this.pbSiteConfig.Size = new System.Drawing.Size(61, 22);
            this.pbSiteConfig.TabIndex = 37;
            this.pbSiteConfig.Text = "Set Site";
            this.pbSiteConfig.UseVisualStyleBackColor = false;
            this.pbSiteConfig.Click += new System.EventHandler(this.pbSiteConfig_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 25);
            this.label10.TabIndex = 35;
            this.label10.Text = "Time Zone:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxLongitudeMinutes
            // 
            this.textBoxLongitudeMinutes.Location = new System.Drawing.Point(167, 53);
            this.textBoxLongitudeMinutes.Name = "textBoxLongitudeMinutes";
            this.textBoxLongitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeMinutes.TabIndex = 12;
            // 
            // textBoxLatitudeMinutes
            // 
            this.textBoxLatitudeMinutes.Location = new System.Drawing.Point(167, 28);
            this.textBoxLatitudeMinutes.Name = "textBoxLatitudeMinutes";
            this.textBoxLatitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLatitudeMinutes.TabIndex = 10;
            // 
            // textBoxLatitudeDegrees
            // 
            this.textBoxLatitudeDegrees.Location = new System.Drawing.Point(121, 28);
            this.textBoxLatitudeDegrees.Name = "textBoxLatitudeDegrees";
            this.textBoxLatitudeDegrees.Size = new System.Drawing.Size(34, 20);
            this.textBoxLatitudeDegrees.TabIndex = 9;
            // 
            // comboBoxLatitude
            // 
            this.comboBoxLatitude.FormattingEnabled = true;
            this.comboBoxLatitude.Items.AddRange(new object[] {
            "N",
            "S"});
            this.comboBoxLatitude.Location = new System.Drawing.Point(70, 28);
            this.comboBoxLatitude.Name = "comboBoxLatitude";
            this.comboBoxLatitude.Size = new System.Drawing.Size(38, 21);
            this.comboBoxLatitude.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Latitude:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Longitude:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Elevation:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxElevation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxElevation, 2);
            this.textBoxElevation.Location = new System.Drawing.Point(70, 78);
            this.textBoxElevation.Name = "textBoxElevation";
            this.textBoxElevation.Size = new System.Drawing.Size(85, 20);
            this.textBoxElevation.TabIndex = 5;
            // 
            // comboBoxLongitude
            // 
            this.comboBoxLongitude.FormattingEnabled = true;
            this.comboBoxLongitude.Items.AddRange(new object[] {
            "E",
            "W"});
            this.comboBoxLongitude.Location = new System.Drawing.Point(70, 53);
            this.comboBoxLongitude.Name = "comboBoxLongitude";
            this.comboBoxLongitude.Size = new System.Drawing.Size(38, 21);
            this.comboBoxLongitude.TabIndex = 8;
            // 
            // textBoxLongitudeDegrees
            // 
            this.textBoxLongitudeDegrees.Location = new System.Drawing.Point(121, 53);
            this.textBoxLongitudeDegrees.Name = "textBoxLongitudeDegrees";
            this.textBoxLongitudeDegrees.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeDegrees.TabIndex = 11;
            // 
            // pbSetSiteNow
            // 
            this.pbSetSiteNow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pbSetSiteNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbSetSiteNow.Location = new System.Drawing.Point(3, 150);
            this.pbSetSiteNow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.pbSetSiteNow.Name = "pbSetSiteNow";
            this.pbSetSiteNow.Size = new System.Drawing.Size(61, 23);
            this.pbSetSiteNow.TabIndex = 34;
            this.pbSetSiteNow.Text = "Set Now";
            this.pbSetSiteNow.UseVisualStyleBackColor = false;
            this.pbSetSiteNow.Visible = false;
            this.pbSetSiteNow.Click += new System.EventHandler(this.pbSetSiteNow_Click);
            // 
            // checkBoxUseDriverSite
            // 
            this.checkBoxUseDriverSite.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxUseDriverSite, 4);
            this.checkBoxUseDriverSite.ForeColor = System.Drawing.Color.White;
            this.checkBoxUseDriverSite.Location = new System.Drawing.Point(3, 128);
            this.checkBoxUseDriverSite.Name = "checkBoxUseDriverSite";
            this.checkBoxUseDriverSite.Size = new System.Drawing.Size(156, 17);
            this.checkBoxUseDriverSite.TabIndex = 22;
            this.checkBoxUseDriverSite.Text = "Set Gemini Site on Connect";
            this.checkBoxUseDriverSite.UseVisualStyleBackColor = false;
            this.checkBoxUseDriverSite.CheckedChanged += new System.EventHandler(this.checkBoxUseGeminiSite_CheckedChanged);
            // 
            // comboBoxTZ
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxTZ, 3);
            this.comboBoxTZ.DisplayMember = "Id";
            this.comboBoxTZ.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTZ.FormattingEnabled = true;
            this.comboBoxTZ.Location = new System.Drawing.Point(70, 103);
            this.comboBoxTZ.Name = "comboBoxTZ";
            this.comboBoxTZ.Size = new System.Drawing.Size(144, 23);
            this.comboBoxTZ.TabIndex = 36;
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.AutoSize = true;
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(9, 460);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(120, 13);
            this.labelVersion.TabIndex = 18;
            this.labelVersion.Text = "<run time - version etc.>";
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.AutoSize = true;
            this.labelTime.ForeColor = System.Drawing.Color.White;
            this.labelTime.Location = new System.Drawing.Point(311, 460);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(185, 13);
            this.labelTime.TabIndex = 19;
            this.labelTime.Text = "<run time - time zone and UTC offset>";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "xxxxxxxxxx";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(103, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(90, 21);
            this.comboBox1.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 1);
            this.label8.TabIndex = 6;
            this.label8.Text = "Com Port:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 351);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 98);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.71859F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.28141F));
            this.tableLayoutPanel4.Controls.Add(this.pbSetTimeNow, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxUseDriverTime, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.labelUtc, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.37975F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.70886F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(217, 79);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // pbSetTimeNow
            // 
            this.pbSetTimeNow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pbSetTimeNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbSetTimeNow.Location = new System.Drawing.Point(3, 52);
            this.pbSetTimeNow.Name = "pbSetTimeNow";
            this.pbSetTimeNow.Size = new System.Drawing.Size(62, 23);
            this.pbSetTimeNow.TabIndex = 34;
            this.pbSetTimeNow.Text = "Set Now";
            this.pbSetTimeNow.UseVisualStyleBackColor = false;
            this.pbSetTimeNow.Visible = false;
            this.pbSetTimeNow.Click += new System.EventHandler(this.pbSetTimeNow_Click_1);
            // 
            // checkBoxUseDriverTime
            // 
            this.checkBoxUseDriverTime.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.checkBoxUseDriverTime, 4);
            this.checkBoxUseDriverTime.ForeColor = System.Drawing.Color.White;
            this.checkBoxUseDriverTime.Location = new System.Drawing.Point(3, 29);
            this.checkBoxUseDriverTime.Name = "checkBoxUseDriverTime";
            this.checkBoxUseDriverTime.Size = new System.Drawing.Size(161, 17);
            this.checkBoxUseDriverTime.TabIndex = 23;
            this.checkBoxUseDriverTime.Text = "Set Gemini Time on Connect";
            this.checkBoxUseDriverTime.UseVisualStyleBackColor = false;
            this.checkBoxUseDriverTime.CheckedChanged += new System.EventHandler(this.checkBoxUseDriverTime_CheckedChanged);
            // 
            // labelUtc
            // 
            this.labelUtc.AutoSize = true;
            this.labelUtc.Location = new System.Drawing.Point(3, 0);
            this.labelUtc.Name = "labelUtc";
            this.labelUtc.Size = new System.Drawing.Size(49, 13);
            this.labelUtc.TabIndex = 7;
            this.labelUtc.Text = "00:00:00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(97, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Universal Time:";
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 100);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.comboBox2, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(103, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(90, 21);
            this.comboBox2.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 6);
            this.label6.TabIndex = 6;
            this.label6.Text = "Com Port:";
            // 
            // checkBoxShowHandbox
            // 
            this.checkBoxShowHandbox.AutoSize = true;
            this.checkBoxShowHandbox.ForeColor = System.Drawing.Color.White;
            this.checkBoxShowHandbox.Location = new System.Drawing.Point(263, 235);
            this.checkBoxShowHandbox.Name = "checkBoxShowHandbox";
            this.checkBoxShowHandbox.Size = new System.Drawing.Size(125, 17);
            this.checkBoxShowHandbox.TabIndex = 24;
            this.checkBoxShowHandbox.Text = "Show Handbox Form";
            this.checkBoxShowHandbox.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 128);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communications";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxComPort, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxBaudRate, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonVirtualPort, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkPortScan, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(217, 109);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Baud Rate:";
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(111, 3);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(89, 21);
            this.comboBoxComPort.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Com Port:";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "28800",
            "38400"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(111, 30);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(89, 21);
            this.comboBoxBaudRate.TabIndex = 24;
            // 
            // buttonVirtualPort
            // 
            this.buttonVirtualPort.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel2.SetColumnSpan(this.buttonVirtualPort, 2);
            this.buttonVirtualPort.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonVirtualPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVirtualPort.Location = new System.Drawing.Point(3, 84);
            this.buttonVirtualPort.Name = "buttonVirtualPort";
            this.buttonVirtualPort.Size = new System.Drawing.Size(211, 22);
            this.buttonVirtualPort.TabIndex = 25;
            this.buttonVirtualPort.Text = "Configure Pass-Through Port...";
            this.buttonVirtualPort.UseVisualStyleBackColor = false;
            this.buttonVirtualPort.Click += new System.EventHandler(this.buttonVirtualPort_Click);
            // 
            // chkPortScan
            // 
            this.chkPortScan.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkPortScan, 2);
            this.chkPortScan.Location = new System.Drawing.Point(3, 57);
            this.chkPortScan.Name = "chkPortScan";
            this.chkPortScan.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.chkPortScan.Size = new System.Drawing.Size(189, 17);
            this.chkPortScan.TabIndex = 26;
            this.chkPortScan.Text = "Auto-detect Gemini on other ports";
            this.chkPortScan.UseVisualStyleBackColor = true;
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 1000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tableLayoutPanel6);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(263, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(160, 127);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Boot Mode";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.radioButtonPrompt, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.radioButtonColdStart, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.radioButtonWarmStart, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.radioButtonWarmRestart, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(154, 108);
            this.tableLayoutPanel6.TabIndex = 4;
            // 
            // radioButtonPrompt
            // 
            this.radioButtonPrompt.AutoSize = true;
            this.radioButtonPrompt.Location = new System.Drawing.Point(3, 84);
            this.radioButtonPrompt.Name = "radioButtonPrompt";
            this.radioButtonPrompt.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.radioButtonPrompt.Size = new System.Drawing.Size(131, 17);
            this.radioButtonPrompt.TabIndex = 3;
            this.radioButtonPrompt.TabStop = true;
            this.radioButtonPrompt.Text = "Prompt if not Started";
            this.radioButtonPrompt.UseVisualStyleBackColor = true;
            // 
            // radioButtonColdStart
            // 
            this.radioButtonColdStart.AutoSize = true;
            this.radioButtonColdStart.Location = new System.Drawing.Point(3, 57);
            this.radioButtonColdStart.Name = "radioButtonColdStart";
            this.radioButtonColdStart.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.radioButtonColdStart.Size = new System.Drawing.Size(81, 17);
            this.radioButtonColdStart.TabIndex = 2;
            this.radioButtonColdStart.TabStop = true;
            this.radioButtonColdStart.Text = "Cold Start";
            this.radioButtonColdStart.UseVisualStyleBackColor = true;
            // 
            // radioButtonWarmStart
            // 
            this.radioButtonWarmStart.AutoSize = true;
            this.radioButtonWarmStart.Location = new System.Drawing.Point(3, 30);
            this.radioButtonWarmStart.Name = "radioButtonWarmStart";
            this.radioButtonWarmStart.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.radioButtonWarmStart.Size = new System.Drawing.Size(88, 17);
            this.radioButtonWarmStart.TabIndex = 1;
            this.radioButtonWarmStart.TabStop = true;
            this.radioButtonWarmStart.Text = "Warm Start";
            this.radioButtonWarmStart.UseVisualStyleBackColor = true;
            // 
            // radioButtonWarmRestart
            // 
            this.radioButtonWarmRestart.AutoSize = true;
            this.radioButtonWarmRestart.Location = new System.Drawing.Point(3, 3);
            this.radioButtonWarmRestart.Name = "radioButtonWarmRestart";
            this.radioButtonWarmRestart.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.radioButtonWarmRestart.Size = new System.Drawing.Size(100, 17);
            this.radioButtonWarmRestart.TabIndex = 0;
            this.radioButtonWarmRestart.TabStop = true;
            this.radioButtonWarmRestart.Text = "Warm Restart";
            this.radioButtonWarmRestart.UseVisualStyleBackColor = true;
            // 
            // chkJoystick
            // 
            this.chkJoystick.ForeColor = System.Drawing.Color.White;
            this.chkJoystick.Location = new System.Drawing.Point(263, 170);
            this.chkJoystick.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.chkJoystick.Name = "chkJoystick";
            this.chkJoystick.Size = new System.Drawing.Size(107, 28);
            this.chkJoystick.TabIndex = 31;
            this.chkJoystick.Text = "Enable Joystick";
            this.chkJoystick.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkJoystick.UseVisualStyleBackColor = false;
            this.chkJoystick.CheckedChanged += new System.EventHandler(this.chkJoystick_CheckedChanged);
            // 
            // cmbJoystick
            // 
            this.cmbJoystick.BackColor = System.Drawing.Color.Black;
            this.cmbJoystick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoystick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbJoystick.ForeColor = System.Drawing.Color.White;
            this.cmbJoystick.FormattingEnabled = true;
            this.cmbJoystick.Location = new System.Drawing.Point(263, 204);
            this.cmbJoystick.Name = "cmbJoystick";
            this.cmbJoystick.Size = new System.Drawing.Size(163, 21);
            this.cmbJoystick.TabIndex = 32;
            // 
            // chkVoice
            // 
            this.chkVoice.AutoSize = true;
            this.chkVoice.ForeColor = System.Drawing.Color.White;
            this.chkVoice.Location = new System.Drawing.Point(263, 258);
            this.chkVoice.Name = "chkVoice";
            this.chkVoice.Size = new System.Drawing.Size(130, 17);
            this.chkVoice.TabIndex = 34;
            this.chkVoice.Text = "Use Voice Announcer";
            this.chkVoice.UseVisualStyleBackColor = true;
            this.chkVoice.CheckedChanged += new System.EventHandler(this.chkVoice_CheckedChanged);
            // 
            // btnJoysticConfig
            // 
            this.btnJoysticConfig.AutoSize = true;
            this.btnJoysticConfig.BackColor = System.Drawing.Color.Transparent;
            this.btnJoysticConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnJoysticConfig.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnJoysticConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoysticConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnJoysticConfig.Image")));
            this.btnJoysticConfig.Location = new System.Drawing.Point(377, 160);
            this.btnJoysticConfig.Name = "btnJoysticConfig";
            this.btnJoysticConfig.Size = new System.Drawing.Size(45, 41);
            this.btnJoysticConfig.TabIndex = 33;
            this.btnJoysticConfig.UseVisualStyleBackColor = false;
            this.btnJoysticConfig.Click += new System.EventHandler(this.btnJoysticConfig_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.GeminiTelescope.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(441, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(263, 305);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(142, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "Logging/Diagnostic Options:";
            // 
            // cbLogging
            // 
            this.cbLogging.BackColor = System.Drawing.Color.Black;
            this.cbLogging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLogging.ForeColor = System.Drawing.Color.White;
            this.cbLogging.FormattingEnabled = true;
            this.cbLogging.Items.AddRange(new object[] {
            "1: No Logging",
            "2: Serial Communications",
            "3: Some Detail",
            "4: More Detail",
            "5: Very Detailed",
            "6: All Available"});
            this.cbLogging.Location = new System.Drawing.Point(263, 324);
            this.cbLogging.Name = "cbLogging";
            this.cbLogging.Size = new System.Drawing.Size(163, 21);
            this.cbLogging.TabIndex = 36;
            // 
            // chkAsyncPulseGuide
            // 
            this.chkAsyncPulseGuide.AutoSize = true;
            this.chkAsyncPulseGuide.ForeColor = System.Drawing.Color.White;
            this.chkAsyncPulseGuide.Location = new System.Drawing.Point(263, 281);
            this.chkAsyncPulseGuide.Name = "chkAsyncPulseGuide";
            this.chkAsyncPulseGuide.Size = new System.Drawing.Size(153, 17);
            this.chkAsyncPulseGuide.TabIndex = 37;
            this.chkAsyncPulseGuide.Text = "Asynchronous Pulse Guide";
            this.chkAsyncPulseGuide.UseVisualStyleBackColor = true;
            this.chkAsyncPulseGuide.CheckedChanged += new System.EventHandler(this.chkAsyncPulseGuide_CheckedChanged);
            // 
            // TelescopeSetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(501, 482);
            this.Controls.Add(this.chkAsyncPulseGuide);
            this.Controls.Add(this.cbLogging);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chkVoice);
            this.Controls.Add(this.btnJoysticConfig);
            this.Controls.Add(this.cmbJoystick);
            this.Controls.Add(this.chkJoystick);
            this.Controls.Add(pbGeminiSettings);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkBoxShowHandbox);
            this.Controls.Add(buttonGps);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TelescopeSetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gemini Telescope Setup";
            this.Load += new System.EventHandler(this.TelescopeSetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxElevation;
        private System.Windows.Forms.ComboBox comboBoxLatitude;
        private System.Windows.Forms.ComboBox comboBoxLongitude;
        private System.Windows.Forms.TextBox textBoxLongitudeMinutes;
        private System.Windows.Forms.TextBox textBoxLatitudeMinutes;
        private System.Windows.Forms.TextBox textBoxLatitudeDegrees;
        private System.Windows.Forms.TextBox textBoxLongitudeDegrees;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.CheckBox checkBoxUseDriverSite;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelUtc;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxUseDriverTime;
        private System.Windows.Forms.CheckBox checkBoxShowHandbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Button buttonVirtualPort;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton radioButtonWarmRestart;
        private System.Windows.Forms.RadioButton radioButtonWarmStart;
        private System.Windows.Forms.RadioButton radioButtonPrompt;
        private System.Windows.Forms.RadioButton radioButtonColdStart;
        private System.Windows.Forms.CheckBox chkJoystick;
        private System.Windows.Forms.ComboBox cmbJoystick;
        private System.Windows.Forms.Button btnJoysticConfig;
        private System.Windows.Forms.Button pbSetTimeNow;
        private System.Windows.Forms.Button pbSetSiteNow;
        private System.Windows.Forms.CheckBox chkVoice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxTZ;
        private System.Windows.Forms.ComboBox comboBoxSites;
        private System.Windows.Forms.Button pbSiteConfig;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbLogging;
        private System.Windows.Forms.CheckBox chkPortScan;
        private System.Windows.Forms.CheckBox chkAsyncPulseGuide;
    }
}