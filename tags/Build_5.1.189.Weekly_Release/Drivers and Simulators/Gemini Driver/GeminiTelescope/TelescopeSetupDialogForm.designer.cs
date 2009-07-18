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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxUseGeminiSite = new System.Windows.Forms.CheckBox();
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
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelLst = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(449, 99);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(449, 129);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 128);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Site Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.02062F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.68041F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.61856F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.16495F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxUseGeminiSite, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeMinutes, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeMinutes, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeDegrees, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLatitude, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxElevation, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLongitude, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeDegrees, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(199, 109);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkBoxUseGeminiSite
            // 
            this.checkBoxUseGeminiSite.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxUseGeminiSite, 4);
            this.checkBoxUseGeminiSite.ForeColor = System.Drawing.Color.White;
            this.checkBoxUseGeminiSite.Location = new System.Drawing.Point(3, 90);
            this.checkBoxUseGeminiSite.Name = "checkBoxUseGeminiSite";
            this.checkBoxUseGeminiSite.Size = new System.Drawing.Size(122, 16);
            this.checkBoxUseGeminiSite.TabIndex = 22;
            this.checkBoxUseGeminiSite.Text = "Use Gemini Site Info";
            this.checkBoxUseGeminiSite.UseVisualStyleBackColor = true;
            // 
            // textBoxLongitudeMinutes
            // 
            this.textBoxLongitudeMinutes.Location = new System.Drawing.Point(157, 32);
            this.textBoxLongitudeMinutes.Name = "textBoxLongitudeMinutes";
            this.textBoxLongitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeMinutes.TabIndex = 12;
            // 
            // textBoxLatitudeMinutes
            // 
            this.textBoxLatitudeMinutes.Location = new System.Drawing.Point(157, 3);
            this.textBoxLatitudeMinutes.Name = "textBoxLatitudeMinutes";
            this.textBoxLatitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLatitudeMinutes.TabIndex = 10;
            // 
            // textBoxLatitudeDegrees
            // 
            this.textBoxLatitudeDegrees.Location = new System.Drawing.Point(116, 3);
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
            this.comboBoxLatitude.Location = new System.Drawing.Point(71, 3);
            this.comboBoxLatitude.Name = "comboBoxLatitude";
            this.comboBoxLatitude.Size = new System.Drawing.Size(38, 21);
            this.comboBoxLatitude.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Latitude:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Longitude:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Elevation:";
            // 
            // textBoxElevation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxElevation, 2);
            this.textBoxElevation.Location = new System.Drawing.Point(71, 61);
            this.textBoxElevation.Name = "textBoxElevation";
            this.textBoxElevation.Size = new System.Drawing.Size(78, 20);
            this.textBoxElevation.TabIndex = 5;
            // 
            // comboBoxLongitude
            // 
            this.comboBoxLongitude.FormattingEnabled = true;
            this.comboBoxLongitude.Items.AddRange(new object[] {
            "E",
            "W"});
            this.comboBoxLongitude.Location = new System.Drawing.Point(71, 32);
            this.comboBoxLongitude.Name = "comboBoxLongitude";
            this.comboBoxLongitude.Size = new System.Drawing.Size(38, 21);
            this.comboBoxLongitude.TabIndex = 8;
            // 
            // textBoxLongitudeDegrees
            // 
            this.textBoxLongitudeDegrees.Location = new System.Drawing.Point(116, 32);
            this.textBoxLongitudeDegrees.Name = "textBoxLongitudeDegrees";
            this.textBoxLongitudeDegrees.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeDegrees.TabIndex = 11;
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.GeminiTelescope.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(460, 18);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.AutoSize = true;
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(9, 219);
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
            this.labelTime.Location = new System.Drawing.Point(323, 219);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(185, 13);
            this.labelTime.TabIndex = 19;
            this.labelTime.Text = "<run time - time zone and UTC offset>";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 50);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communications";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.comboBoxComPort, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.49057F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.50943F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(199, 31);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(102, 3);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(90, 21);
            this.comboBoxComPort.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Com Port:";
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
            this.groupBox3.Location = new System.Drawing.Point(235, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(205, 65);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.78894F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.21106F));
            this.tableLayoutPanel4.Controls.Add(this.checkBox1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.labelLst, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.30435F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.69565F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(199, 46);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Local Sidereal Time:";
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
            // labelLst
            // 
            this.labelLst.AutoSize = true;
            this.labelLst.Location = new System.Drawing.Point(117, 0);
            this.labelLst.Name = "labelLst";
            this.labelLst.Size = new System.Drawing.Size(49, 13);
            this.labelLst.TabIndex = 7;
            this.labelLst.Text = "00:00:00";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.checkBox1, 4);
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(3, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(106, 17);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Use Gemini Time";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(238, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Query GPS";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(520, 241);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gemini Telescope Setup";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.CheckBox checkBoxUseGeminiSite;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelLst;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}