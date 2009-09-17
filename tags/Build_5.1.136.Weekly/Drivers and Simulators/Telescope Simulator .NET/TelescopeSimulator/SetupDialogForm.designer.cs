namespace ASCOM.TelescopeSimulator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLongitudeMinutes = new System.Windows.Forms.TextBox();
            this.textBoxLatitudeMinutes = new System.Windows.Forms.TextBox();
            this.textBoxLatitudeDegrees = new System.Windows.Forms.TextBox();
            this.comboBoxLatitude = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxElevation = new System.Windows.Forms.TextBox();
            this.comboBoxLongitude = new System.Windows.Forms.ComboBox();
            this.textBoxLongitudeDegress = new System.Windows.Forms.TextBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.checkBoxAutoUnpark = new System.Windows.Forms.CheckBox();
            this.checkBoxCoordinatesOnPark = new System.Windows.Forms.CheckBox();
            this.checkBoxDisconnectOnPark = new System.Windows.Forms.CheckBox();
            this.buttonSetParkPosition = new System.Windows.Forms.Button();
            this.buttonResetParkPosition = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonGermanEquatorial = new System.Windows.Forms.RadioButton();
            this.radioButtonEquatorial = new System.Windows.Forms.RadioButton();
            this.radioButtonAltAzimuth = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFocalLength = new System.Windows.Forms.TextBox();
            this.textBoxApertureArea = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAperture = new System.Windows.Forms.TextBox();
            this.numericUpDownSlewRate = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxCanPulseGuide = new System.Windows.Forms.CheckBox();
            this.checkBoxCanGuideRates = new System.Windows.Forms.CheckBox();
            this.checkBoxCanTrackingRates = new System.Windows.Forms.CheckBox();
            this.checkBoxCanTrackOnOff = new System.Windows.Forms.CheckBox();
            this.checkBoxCanAlignMode = new System.Windows.Forms.CheckBox();
            this.checkBoxEquatorialAsync = new System.Windows.Forms.CheckBox();
            this.checkBoxCanAltAz = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSlewAltAz = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSlewAltAzAsync = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSyncAltAz = new System.Windows.Forms.CheckBox();
            this.checkBoxCanEquatorial = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSlewEquatorial = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSyncEquatorial = new System.Windows.Forms.CheckBox();
            this.checkBoxCanFindHome = new System.Windows.Forms.CheckBox();
            this.checkBoxCanPark = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSetParkPosition = new System.Windows.Forms.CheckBox();
            this.checkBoxCanUnpark = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxNumberMoveAxis = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxCanSideOfPier = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSetSideOfPier = new System.Windows.Forms.CheckBox();
            this.checkBoxCanRefraction = new System.Windows.Forms.CheckBox();
            this.checkBoxCanPulseGuideDualAxis = new System.Windows.Forms.CheckBox();
            this.checkBoxCanRaDecRate = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxEquatorialSystem = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxCanLatLongElev = new System.Windows.Forms.CheckBox();
            this.checkBoxCanOptics = new System.Windows.Forms.CheckBox();
            this.checkBoxCanDateTime = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSiderealTime = new System.Windows.Forms.CheckBox();
            this.checkBoxVersionOne = new System.Windows.Forms.CheckBox();
            this.checkBoxOnTop = new System.Windows.Forms.CheckBox();
            this.checkBoxRefraction = new System.Windows.Forms.CheckBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSlewRate)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(557, 169);
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
            this.cmdCancel.Location = new System.Drawing.Point(557, 199);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 105);
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
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeMinutes, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeMinutes, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeDegrees, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLatitude, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxElevation, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxLongitude, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeDegress, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(194, 86);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxLongitudeMinutes
            // 
            this.textBoxLongitudeMinutes.Location = new System.Drawing.Point(153, 31);
            this.textBoxLongitudeMinutes.Name = "textBoxLongitudeMinutes";
            this.textBoxLongitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeMinutes.TabIndex = 12;
            // 
            // textBoxLatitudeMinutes
            // 
            this.textBoxLatitudeMinutes.Location = new System.Drawing.Point(153, 3);
            this.textBoxLatitudeMinutes.Name = "textBoxLatitudeMinutes";
            this.textBoxLatitudeMinutes.Size = new System.Drawing.Size(34, 20);
            this.textBoxLatitudeMinutes.TabIndex = 10;
            // 
            // textBoxLatitudeDegrees
            // 
            this.textBoxLatitudeDegrees.Location = new System.Drawing.Point(113, 3);
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
            this.comboBoxLatitude.Location = new System.Drawing.Point(69, 3);
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
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Longitude:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Elevation:";
            // 
            // textBoxElevation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxElevation, 2);
            this.textBoxElevation.Location = new System.Drawing.Point(69, 59);
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
            this.comboBoxLongitude.Location = new System.Drawing.Point(69, 31);
            this.comboBoxLongitude.Name = "comboBoxLongitude";
            this.comboBoxLongitude.Size = new System.Drawing.Size(38, 21);
            this.comboBoxLongitude.TabIndex = 8;
            // 
            // textBoxLongitudeDegress
            // 
            this.textBoxLongitudeDegress.Location = new System.Drawing.Point(113, 31);
            this.textBoxLongitudeDegress.Name = "textBoxLongitudeDegress";
            this.textBoxLongitudeDegress.Size = new System.Drawing.Size(34, 20);
            this.textBoxLongitudeDegress.TabIndex = 11;
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.TelescopeSimulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(568, 19);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // checkBoxAutoUnpark
            // 
            this.checkBoxAutoUnpark.AutoSize = true;
            this.checkBoxAutoUnpark.ForeColor = System.Drawing.Color.White;
            this.checkBoxAutoUnpark.Location = new System.Drawing.Point(236, 29);
            this.checkBoxAutoUnpark.Name = "checkBoxAutoUnpark";
            this.checkBoxAutoUnpark.Size = new System.Drawing.Size(159, 17);
            this.checkBoxAutoUnpark.TabIndex = 5;
            this.checkBoxAutoUnpark.Text = "Auto Unpark/Track on Start";
            this.checkBoxAutoUnpark.UseVisualStyleBackColor = true;
            // 
            // checkBoxCoordinatesOnPark
            // 
            this.checkBoxCoordinatesOnPark.AutoSize = true;
            this.checkBoxCoordinatesOnPark.ForeColor = System.Drawing.Color.White;
            this.checkBoxCoordinatesOnPark.Location = new System.Drawing.Point(236, 52);
            this.checkBoxCoordinatesOnPark.Name = "checkBoxCoordinatesOnPark";
            this.checkBoxCoordinatesOnPark.Size = new System.Drawing.Size(165, 17);
            this.checkBoxCoordinatesOnPark.TabIndex = 6;
            this.checkBoxCoordinatesOnPark.Text = "No Coordinates when Parked";
            this.checkBoxCoordinatesOnPark.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisconnectOnPark
            // 
            this.checkBoxDisconnectOnPark.AutoSize = true;
            this.checkBoxDisconnectOnPark.ForeColor = System.Drawing.Color.White;
            this.checkBoxDisconnectOnPark.Location = new System.Drawing.Point(236, 75);
            this.checkBoxDisconnectOnPark.Name = "checkBoxDisconnectOnPark";
            this.checkBoxDisconnectOnPark.Size = new System.Drawing.Size(120, 17);
            this.checkBoxDisconnectOnPark.TabIndex = 7;
            this.checkBoxDisconnectOnPark.Text = "Disconnect on Park";
            this.checkBoxDisconnectOnPark.UseVisualStyleBackColor = true;
            // 
            // buttonSetParkPosition
            // 
            this.buttonSetParkPosition.Location = new System.Drawing.Point(441, 19);
            this.buttonSetParkPosition.Name = "buttonSetParkPosition";
            this.buttonSetParkPosition.Size = new System.Drawing.Size(75, 44);
            this.buttonSetParkPosition.TabIndex = 8;
            this.buttonSetParkPosition.Text = "Set Park Position";
            this.buttonSetParkPosition.UseVisualStyleBackColor = true;
            // 
            // buttonResetParkPosition
            // 
            this.buttonResetParkPosition.Location = new System.Drawing.Point(441, 69);
            this.buttonResetParkPosition.Name = "buttonResetParkPosition";
            this.buttonResetParkPosition.Size = new System.Drawing.Size(75, 45);
            this.buttonResetParkPosition.TabIndex = 9;
            this.buttonResetParkPosition.Text = "Reset Park Position";
            this.buttonResetParkPosition.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 90);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mount Type";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.radioButtonGermanEquatorial, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonEquatorial, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonAltAzimuth, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(121, 71);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // radioButtonGermanEquatorial
            // 
            this.radioButtonGermanEquatorial.AutoSize = true;
            this.radioButtonGermanEquatorial.ForeColor = System.Drawing.Color.White;
            this.radioButtonGermanEquatorial.Location = new System.Drawing.Point(3, 53);
            this.radioButtonGermanEquatorial.Name = "radioButtonGermanEquatorial";
            this.radioButtonGermanEquatorial.Size = new System.Drawing.Size(112, 15);
            this.radioButtonGermanEquatorial.TabIndex = 13;
            this.radioButtonGermanEquatorial.TabStop = true;
            this.radioButtonGermanEquatorial.Text = "German Equatorial";
            this.radioButtonGermanEquatorial.UseVisualStyleBackColor = true;
            // 
            // radioButtonEquatorial
            // 
            this.radioButtonEquatorial.AutoSize = true;
            this.radioButtonEquatorial.ForeColor = System.Drawing.Color.White;
            this.radioButtonEquatorial.Location = new System.Drawing.Point(3, 28);
            this.radioButtonEquatorial.Name = "radioButtonEquatorial";
            this.radioButtonEquatorial.Size = new System.Drawing.Size(72, 17);
            this.radioButtonEquatorial.TabIndex = 12;
            this.radioButtonEquatorial.TabStop = true;
            this.radioButtonEquatorial.Text = "Equatorial";
            this.radioButtonEquatorial.UseVisualStyleBackColor = true;
            // 
            // radioButtonAltAzimuth
            // 
            this.radioButtonAltAzimuth.AutoSize = true;
            this.radioButtonAltAzimuth.ForeColor = System.Drawing.Color.White;
            this.radioButtonAltAzimuth.Location = new System.Drawing.Point(3, 3);
            this.radioButtonAltAzimuth.Name = "radioButtonAltAzimuth";
            this.radioButtonAltAzimuth.Size = new System.Drawing.Size(77, 17);
            this.radioButtonAltAzimuth.TabIndex = 11;
            this.radioButtonAltAzimuth.TabStop = true;
            this.radioButtonAltAzimuth.Text = "Alt-Azimuth";
            this.radioButtonAltAzimuth.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel3);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(168, 134);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 90);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Optics";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.textBoxFocalLength, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.textBoxApertureArea, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBoxAperture, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(176, 71);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // textBoxFocalLength
            // 
            this.textBoxFocalLength.Location = new System.Drawing.Point(91, 53);
            this.textBoxFocalLength.Name = "textBoxFocalLength";
            this.textBoxFocalLength.Size = new System.Drawing.Size(82, 20);
            this.textBoxFocalLength.TabIndex = 11;
            // 
            // textBoxApertureArea
            // 
            this.textBoxApertureArea.Location = new System.Drawing.Point(91, 28);
            this.textBoxApertureArea.Name = "textBoxApertureArea";
            this.textBoxApertureArea.Size = new System.Drawing.Size(82, 20);
            this.textBoxApertureArea.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 21);
            this.label6.TabIndex = 8;
            this.label6.Text = "Focal Length (m):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Aperture (m):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Aperture Area (m^2)";
            // 
            // textBoxAperture
            // 
            this.textBoxAperture.Location = new System.Drawing.Point(91, 3);
            this.textBoxAperture.Name = "textBoxAperture";
            this.textBoxAperture.Size = new System.Drawing.Size(82, 20);
            this.textBoxAperture.TabIndex = 9;
            // 
            // numericUpDownSlewRate
            // 
            this.numericUpDownSlewRate.Location = new System.Drawing.Point(350, 95);
            this.numericUpDownSlewRate.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownSlewRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSlewRate.Name = "numericUpDownSlewRate";
            this.numericUpDownSlewRate.Size = new System.Drawing.Size(67, 20);
            this.numericUpDownSlewRate.TabIndex = 12;
            this.numericUpDownSlewRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(236, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Slew Rate (deg/sec):";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel4);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(12, 234);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(604, 228);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Interface Capabilities";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanPulseGuide, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanGuideRates, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanTrackingRates, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanTrackOnOff, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanAlignMode, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxEquatorialAsync, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanAltAz, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSlewAltAz, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSlewAltAzAsync, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSyncAltAz, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanEquatorial, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSlewEquatorial, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSyncEquatorial, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanFindHome, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanPark, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSetParkPosition, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanUnpark, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSideOfPier, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSetSideOfPier, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanRefraction, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanPulseGuideDualAxis, 3, 4);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanRaDecRate, 3, 5);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 2, 4);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanLatLongElev, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanOptics, 1, 8);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanDateTime, 2, 7);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxCanSiderealTime, 2, 8);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxVersionOne, 3, 8);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 9;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(598, 209);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // checkBoxCanPulseGuide
            // 
            this.checkBoxCanPulseGuide.AutoSize = true;
            this.checkBoxCanPulseGuide.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanPulseGuide.Location = new System.Drawing.Point(450, 72);
            this.checkBoxCanPulseGuide.Name = "checkBoxCanPulseGuide";
            this.checkBoxCanPulseGuide.Size = new System.Drawing.Size(83, 17);
            this.checkBoxCanPulseGuide.TabIndex = 31;
            this.checkBoxCanPulseGuide.Text = "Pulse Guide";
            this.checkBoxCanPulseGuide.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanGuideRates
            // 
            this.checkBoxCanGuideRates.AutoSize = true;
            this.checkBoxCanGuideRates.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanGuideRates.Location = new System.Drawing.Point(450, 49);
            this.checkBoxCanGuideRates.Name = "checkBoxCanGuideRates";
            this.checkBoxCanGuideRates.Size = new System.Drawing.Size(85, 17);
            this.checkBoxCanGuideRates.TabIndex = 30;
            this.checkBoxCanGuideRates.Text = "Guide Rates";
            this.checkBoxCanGuideRates.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanTrackingRates
            // 
            this.checkBoxCanTrackingRates.AutoSize = true;
            this.checkBoxCanTrackingRates.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanTrackingRates.Location = new System.Drawing.Point(450, 26);
            this.checkBoxCanTrackingRates.Name = "checkBoxCanTrackingRates";
            this.checkBoxCanTrackingRates.Size = new System.Drawing.Size(99, 17);
            this.checkBoxCanTrackingRates.TabIndex = 29;
            this.checkBoxCanTrackingRates.Text = "Tracking Rates";
            this.checkBoxCanTrackingRates.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanTrackOnOff
            // 
            this.checkBoxCanTrackOnOff.AutoSize = true;
            this.checkBoxCanTrackOnOff.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanTrackOnOff.Location = new System.Drawing.Point(450, 3);
            this.checkBoxCanTrackOnOff.Name = "checkBoxCanTrackOnOff";
            this.checkBoxCanTrackOnOff.Size = new System.Drawing.Size(132, 17);
            this.checkBoxCanTrackOnOff.TabIndex = 28;
            this.checkBoxCanTrackOnOff.Text = "Track on / off Support";
            this.checkBoxCanTrackOnOff.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanAlignMode
            // 
            this.checkBoxCanAlignMode.AutoSize = true;
            this.checkBoxCanAlignMode.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanAlignMode.Location = new System.Drawing.Point(301, 3);
            this.checkBoxCanAlignMode.Name = "checkBoxCanAlignMode";
            this.checkBoxCanAlignMode.Size = new System.Drawing.Size(102, 17);
            this.checkBoxCanAlignMode.TabIndex = 24;
            this.checkBoxCanAlignMode.Text = "Alignment Mode";
            this.checkBoxCanAlignMode.UseVisualStyleBackColor = true;
            // 
            // checkBoxEquatorialAsync
            // 
            this.checkBoxEquatorialAsync.AutoSize = true;
            this.checkBoxEquatorialAsync.ForeColor = System.Drawing.Color.White;
            this.checkBoxEquatorialAsync.Location = new System.Drawing.Point(3, 187);
            this.checkBoxEquatorialAsync.Name = "checkBoxEquatorialAsync";
            this.checkBoxEquatorialAsync.Size = new System.Drawing.Size(143, 17);
            this.checkBoxEquatorialAsync.TabIndex = 22;
            this.checkBoxEquatorialAsync.Text = "Equatorial Asynchronous";
            this.checkBoxEquatorialAsync.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanAltAz
            // 
            this.checkBoxCanAltAz.AutoSize = true;
            this.checkBoxCanAltAz.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanAltAz.Location = new System.Drawing.Point(3, 3);
            this.checkBoxCanAltAz.Name = "checkBoxCanAltAz";
            this.checkBoxCanAltAz.Size = new System.Drawing.Size(120, 17);
            this.checkBoxCanAltAz.TabIndex = 11;
            this.checkBoxCanAltAz.Text = "Alt / Az Coordinates";
            this.checkBoxCanAltAz.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSlewAltAz
            // 
            this.checkBoxCanSlewAltAz.AutoSize = true;
            this.checkBoxCanSlewAltAz.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSlewAltAz.Location = new System.Drawing.Point(3, 26);
            this.checkBoxCanSlewAltAz.Name = "checkBoxCanSlewAltAz";
            this.checkBoxCanSlewAltAz.Size = new System.Drawing.Size(101, 17);
            this.checkBoxCanSlewAltAz.TabIndex = 12;
            this.checkBoxCanSlewAltAz.Text = "Alt / Az Slewing";
            this.checkBoxCanSlewAltAz.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSlewAltAzAsync
            // 
            this.checkBoxCanSlewAltAzAsync.AutoSize = true;
            this.checkBoxCanSlewAltAzAsync.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSlewAltAzAsync.Location = new System.Drawing.Point(3, 72);
            this.checkBoxCanSlewAltAzAsync.Name = "checkBoxCanSlewAltAzAsync";
            this.checkBoxCanSlewAltAzAsync.Size = new System.Drawing.Size(131, 17);
            this.checkBoxCanSlewAltAzAsync.TabIndex = 13;
            this.checkBoxCanSlewAltAzAsync.Text = "Alt / Az Asynchronous";
            this.checkBoxCanSlewAltAzAsync.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSyncAltAz
            // 
            this.checkBoxCanSyncAltAz.AutoSize = true;
            this.checkBoxCanSyncAltAz.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSyncAltAz.Location = new System.Drawing.Point(3, 49);
            this.checkBoxCanSyncAltAz.Name = "checkBoxCanSyncAltAz";
            this.checkBoxCanSyncAltAz.Size = new System.Drawing.Size(88, 17);
            this.checkBoxCanSyncAltAz.TabIndex = 14;
            this.checkBoxCanSyncAltAz.Text = "Alt / Az Sync";
            this.checkBoxCanSyncAltAz.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanEquatorial
            // 
            this.checkBoxCanEquatorial.AutoSize = true;
            this.checkBoxCanEquatorial.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanEquatorial.Location = new System.Drawing.Point(3, 118);
            this.checkBoxCanEquatorial.Name = "checkBoxCanEquatorial";
            this.checkBoxCanEquatorial.Size = new System.Drawing.Size(132, 17);
            this.checkBoxCanEquatorial.TabIndex = 15;
            this.checkBoxCanEquatorial.Text = "Equatorial Coordinates";
            this.checkBoxCanEquatorial.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSlewEquatorial
            // 
            this.checkBoxCanSlewEquatorial.AutoSize = true;
            this.checkBoxCanSlewEquatorial.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSlewEquatorial.Location = new System.Drawing.Point(3, 141);
            this.checkBoxCanSlewEquatorial.Name = "checkBoxCanSlewEquatorial";
            this.checkBoxCanSlewEquatorial.Size = new System.Drawing.Size(113, 17);
            this.checkBoxCanSlewEquatorial.TabIndex = 16;
            this.checkBoxCanSlewEquatorial.Text = "Equatorial Slewing";
            this.checkBoxCanSlewEquatorial.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSyncEquatorial
            // 
            this.checkBoxCanSyncEquatorial.AutoSize = true;
            this.checkBoxCanSyncEquatorial.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSyncEquatorial.Location = new System.Drawing.Point(3, 164);
            this.checkBoxCanSyncEquatorial.Name = "checkBoxCanSyncEquatorial";
            this.checkBoxCanSyncEquatorial.Size = new System.Drawing.Size(100, 17);
            this.checkBoxCanSyncEquatorial.TabIndex = 18;
            this.checkBoxCanSyncEquatorial.Text = "Equatorial Sync";
            this.checkBoxCanSyncEquatorial.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanFindHome
            // 
            this.checkBoxCanFindHome.AutoSize = true;
            this.checkBoxCanFindHome.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanFindHome.Location = new System.Drawing.Point(152, 3);
            this.checkBoxCanFindHome.Name = "checkBoxCanFindHome";
            this.checkBoxCanFindHome.Size = new System.Drawing.Size(77, 17);
            this.checkBoxCanFindHome.TabIndex = 17;
            this.checkBoxCanFindHome.Text = "Find Home";
            this.checkBoxCanFindHome.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanPark
            // 
            this.checkBoxCanPark.AutoSize = true;
            this.checkBoxCanPark.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanPark.Location = new System.Drawing.Point(152, 26);
            this.checkBoxCanPark.Name = "checkBoxCanPark";
            this.checkBoxCanPark.Size = new System.Drawing.Size(62, 17);
            this.checkBoxCanPark.TabIndex = 19;
            this.checkBoxCanPark.Text = "Parking";
            this.checkBoxCanPark.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSetParkPosition
            // 
            this.checkBoxCanSetParkPosition.AutoSize = true;
            this.checkBoxCanSetParkPosition.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSetParkPosition.Location = new System.Drawing.Point(152, 49);
            this.checkBoxCanSetParkPosition.Name = "checkBoxCanSetParkPosition";
            this.checkBoxCanSetParkPosition.Size = new System.Drawing.Size(107, 17);
            this.checkBoxCanSetParkPosition.TabIndex = 20;
            this.checkBoxCanSetParkPosition.Text = "Set Park Position";
            this.checkBoxCanSetParkPosition.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanUnpark
            // 
            this.checkBoxCanUnpark.AutoSize = true;
            this.checkBoxCanUnpark.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanUnpark.Location = new System.Drawing.Point(152, 72);
            this.checkBoxCanUnpark.Name = "checkBoxCanUnpark";
            this.checkBoxCanUnpark.Size = new System.Drawing.Size(75, 17);
            this.checkBoxCanUnpark.TabIndex = 21;
            this.checkBoxCanUnpark.Text = "Unparking";
            this.checkBoxCanUnpark.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.13986F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.86014F));
            this.tableLayoutPanel5.Controls.Add(this.comboBoxNumberMoveAxis, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(152, 95);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel4.SetRowSpan(this.tableLayoutPanel5, 2);
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(143, 40);
            this.tableLayoutPanel5.TabIndex = 23;
            // 
            // comboBoxNumberMoveAxis
            // 
            this.comboBoxNumberMoveAxis.FormattingEnabled = true;
            this.comboBoxNumberMoveAxis.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboBoxNumberMoveAxis.Location = new System.Drawing.Point(88, 3);
            this.comboBoxNumberMoveAxis.Name = "comboBoxNumberMoveAxis";
            this.comboBoxNumberMoveAxis.Size = new System.Drawing.Size(52, 21);
            this.comboBoxNumberMoveAxis.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 26);
            this.label8.TabIndex = 14;
            this.label8.Text = "Number of Axis For Move:";
            // 
            // checkBoxCanSideOfPier
            // 
            this.checkBoxCanSideOfPier.AutoSize = true;
            this.checkBoxCanSideOfPier.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSideOfPier.Location = new System.Drawing.Point(301, 26);
            this.checkBoxCanSideOfPier.Name = "checkBoxCanSideOfPier";
            this.checkBoxCanSideOfPier.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCanSideOfPier.TabIndex = 25;
            this.checkBoxCanSideOfPier.Text = "Side of Pier";
            this.checkBoxCanSideOfPier.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSetSideOfPier
            // 
            this.checkBoxCanSetSideOfPier.AutoSize = true;
            this.checkBoxCanSetSideOfPier.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSetSideOfPier.Location = new System.Drawing.Point(301, 49);
            this.checkBoxCanSetSideOfPier.Name = "checkBoxCanSetSideOfPier";
            this.checkBoxCanSetSideOfPier.Size = new System.Drawing.Size(99, 17);
            this.checkBoxCanSetSideOfPier.TabIndex = 26;
            this.checkBoxCanSetSideOfPier.Text = "Set Side of Pier";
            this.checkBoxCanSetSideOfPier.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanRefraction
            // 
            this.checkBoxCanRefraction.AutoSize = true;
            this.checkBoxCanRefraction.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanRefraction.Location = new System.Drawing.Point(301, 72);
            this.checkBoxCanRefraction.Name = "checkBoxCanRefraction";
            this.checkBoxCanRefraction.Size = new System.Drawing.Size(112, 17);
            this.checkBoxCanRefraction.TabIndex = 27;
            this.checkBoxCanRefraction.Text = "RefractionSupport";
            this.checkBoxCanRefraction.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanPulseGuideDualAxis
            // 
            this.checkBoxCanPulseGuideDualAxis.AutoSize = true;
            this.checkBoxCanPulseGuideDualAxis.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanPulseGuideDualAxis.Location = new System.Drawing.Point(450, 95);
            this.checkBoxCanPulseGuideDualAxis.Name = "checkBoxCanPulseGuideDualAxis";
            this.checkBoxCanPulseGuideDualAxis.Size = new System.Drawing.Size(130, 17);
            this.checkBoxCanPulseGuideDualAxis.TabIndex = 32;
            this.checkBoxCanPulseGuideDualAxis.Text = "Dual Axis Pulse Guide";
            this.checkBoxCanPulseGuideDualAxis.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanRaDecRate
            // 
            this.checkBoxCanRaDecRate.AutoSize = true;
            this.checkBoxCanRaDecRate.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanRaDecRate.Location = new System.Drawing.Point(450, 118);
            this.checkBoxCanRaDecRate.Name = "checkBoxCanRaDecRate";
            this.checkBoxCanRaDecRate.Size = new System.Drawing.Size(103, 17);
            this.checkBoxCanRaDecRate.TabIndex = 33;
            this.checkBoxCanRaDecRate.Text = "RA / Dec Rates";
            this.checkBoxCanRaDecRate.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.35664F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.64336F));
            this.tableLayoutPanel6.Controls.Add(this.comboBoxEquatorialSystem, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(301, 95);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel4.SetRowSpan(this.tableLayoutPanel6, 2);
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(143, 40);
            this.tableLayoutPanel6.TabIndex = 34;
            // 
            // comboBoxEquatorialSystem
            // 
            this.comboBoxEquatorialSystem.FormattingEnabled = true;
            this.comboBoxEquatorialSystem.Items.AddRange(new object[] {
            "Local",
            "B1950",
            "J2000",
            "J2050",
            "Other"});
            this.comboBoxEquatorialSystem.Location = new System.Drawing.Point(64, 3);
            this.comboBoxEquatorialSystem.Name = "comboBoxEquatorialSystem";
            this.comboBoxEquatorialSystem.Size = new System.Drawing.Size(75, 21);
            this.comboBoxEquatorialSystem.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 26);
            this.label9.TabIndex = 14;
            this.label9.Text = "Equatorial System:";
            // 
            // checkBoxCanLatLongElev
            // 
            this.checkBoxCanLatLongElev.AutoSize = true;
            this.checkBoxCanLatLongElev.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanLatLongElev.Location = new System.Drawing.Point(152, 164);
            this.checkBoxCanLatLongElev.Name = "checkBoxCanLatLongElev";
            this.checkBoxCanLatLongElev.Size = new System.Drawing.Size(119, 17);
            this.checkBoxCanLatLongElev.TabIndex = 35;
            this.checkBoxCanLatLongElev.Text = "Lat/Long/Elevation";
            this.checkBoxCanLatLongElev.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanOptics
            // 
            this.checkBoxCanOptics.AutoSize = true;
            this.checkBoxCanOptics.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanOptics.Location = new System.Drawing.Point(152, 187);
            this.checkBoxCanOptics.Name = "checkBoxCanOptics";
            this.checkBoxCanOptics.Size = new System.Drawing.Size(56, 17);
            this.checkBoxCanOptics.TabIndex = 36;
            this.checkBoxCanOptics.Text = "Optics";
            this.checkBoxCanOptics.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanDateTime
            // 
            this.checkBoxCanDateTime.AutoSize = true;
            this.checkBoxCanDateTime.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanDateTime.Location = new System.Drawing.Point(301, 164);
            this.checkBoxCanDateTime.Name = "checkBoxCanDateTime";
            this.checkBoxCanDateTime.Size = new System.Drawing.Size(114, 17);
            this.checkBoxCanDateTime.TabIndex = 37;
            this.checkBoxCanDateTime.Text = "Date / Time (UTC)";
            this.checkBoxCanDateTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSiderealTime
            // 
            this.checkBoxCanSiderealTime.AutoSize = true;
            this.checkBoxCanSiderealTime.ForeColor = System.Drawing.Color.White;
            this.checkBoxCanSiderealTime.Location = new System.Drawing.Point(301, 187);
            this.checkBoxCanSiderealTime.Name = "checkBoxCanSiderealTime";
            this.checkBoxCanSiderealTime.Size = new System.Drawing.Size(90, 17);
            this.checkBoxCanSiderealTime.TabIndex = 38;
            this.checkBoxCanSiderealTime.Text = "Sidereal Time";
            this.checkBoxCanSiderealTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxVersionOne
            // 
            this.checkBoxVersionOne.AutoSize = true;
            this.checkBoxVersionOne.ForeColor = System.Drawing.Color.White;
            this.checkBoxVersionOne.Location = new System.Drawing.Point(450, 187);
            this.checkBoxVersionOne.Name = "checkBoxVersionOne";
            this.checkBoxVersionOne.Size = new System.Drawing.Size(94, 17);
            this.checkBoxVersionOne.TabIndex = 39;
            this.checkBoxVersionOne.Text = "Version 1 Only";
            this.checkBoxVersionOne.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnTop
            // 
            this.checkBoxOnTop.AutoSize = true;
            this.checkBoxOnTop.ForeColor = System.Drawing.Color.White;
            this.checkBoxOnTop.Location = new System.Drawing.Point(382, 207);
            this.checkBoxOnTop.Name = "checkBoxOnTop";
            this.checkBoxOnTop.Size = new System.Drawing.Size(98, 17);
            this.checkBoxOnTop.TabIndex = 16;
            this.checkBoxOnTop.Text = "Always On Top";
            this.checkBoxOnTop.UseVisualStyleBackColor = true;
            // 
            // checkBoxRefraction
            // 
            this.checkBoxRefraction.AutoSize = true;
            this.checkBoxRefraction.ForeColor = System.Drawing.Color.White;
            this.checkBoxRefraction.Location = new System.Drawing.Point(382, 184);
            this.checkBoxRefraction.Name = "checkBoxRefraction";
            this.checkBoxRefraction.Size = new System.Drawing.Size(92, 17);
            this.checkBoxRefraction.TabIndex = 17;
            this.checkBoxRefraction.Text = "Refraction On";
            this.checkBoxRefraction.UseVisualStyleBackColor = true;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(18, 473);
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
            this.labelTime.Location = new System.Drawing.Point(431, 473);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(185, 13);
            this.labelTime.TabIndex = 19;
            this.labelTime.Text = "<run time - time zone and UTC offset>";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(628, 495);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.checkBoxRefraction);
            this.Controls.Add(this.checkBoxOnTop);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDownSlewRate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonResetParkPosition);
            this.Controls.Add(this.buttonSetParkPosition);
            this.Controls.Add(this.checkBoxDisconnectOnPark);
            this.Controls.Add(this.checkBoxCoordinatesOnPark);
            this.Controls.Add(this.checkBoxAutoUnpark);
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
            this.Text = "ASCOM Telescope Simulator Setup";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSlewRate)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
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
        private System.Windows.Forms.TextBox textBoxLongitudeDegress;
        private System.Windows.Forms.CheckBox checkBoxAutoUnpark;
        private System.Windows.Forms.CheckBox checkBoxCoordinatesOnPark;
        private System.Windows.Forms.CheckBox checkBoxDisconnectOnPark;
        private System.Windows.Forms.Button buttonSetParkPosition;
        private System.Windows.Forms.Button buttonResetParkPosition;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonAltAzimuth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButtonGermanEquatorial;
        private System.Windows.Forms.RadioButton radioButtonEquatorial;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox textBoxFocalLength;
        private System.Windows.Forms.TextBox textBoxApertureArea;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAperture;
        private System.Windows.Forms.NumericUpDown numericUpDownSlewRate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox checkBoxOnTop;
        private System.Windows.Forms.CheckBox checkBoxRefraction;
        private System.Windows.Forms.CheckBox checkBoxCanAltAz;
        private System.Windows.Forms.CheckBox checkBoxCanSlewAltAz;
        private System.Windows.Forms.CheckBox checkBoxCanSlewAltAzAsync;
        private System.Windows.Forms.CheckBox checkBoxCanSyncAltAz;
        private System.Windows.Forms.CheckBox checkBoxCanEquatorial;
        private System.Windows.Forms.CheckBox checkBoxCanSlewEquatorial;
        private System.Windows.Forms.CheckBox checkBoxCanFindHome;
        private System.Windows.Forms.CheckBox checkBoxCanSyncEquatorial;
        private System.Windows.Forms.CheckBox checkBoxCanPark;
        private System.Windows.Forms.CheckBox checkBoxCanSetParkPosition;
        private System.Windows.Forms.CheckBox checkBoxCanUnpark;
        private System.Windows.Forms.CheckBox checkBoxEquatorialAsync;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ComboBox comboBoxNumberMoveAxis;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxCanAlignMode;
        private System.Windows.Forms.CheckBox checkBoxCanSideOfPier;
        private System.Windows.Forms.CheckBox checkBoxCanSetSideOfPier;
        private System.Windows.Forms.CheckBox checkBoxCanRefraction;
        private System.Windows.Forms.CheckBox checkBoxCanPulseGuide;
        private System.Windows.Forms.CheckBox checkBoxCanGuideRates;
        private System.Windows.Forms.CheckBox checkBoxCanTrackingRates;
        private System.Windows.Forms.CheckBox checkBoxCanTrackOnOff;
        private System.Windows.Forms.CheckBox checkBoxCanPulseGuideDualAxis;
        private System.Windows.Forms.CheckBox checkBoxCanRaDecRate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.ComboBox comboBoxEquatorialSystem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxCanLatLongElev;
        private System.Windows.Forms.CheckBox checkBoxCanOptics;
        private System.Windows.Forms.CheckBox checkBoxCanDateTime;
        private System.Windows.Forms.CheckBox checkBoxCanSiderealTime;
        private System.Windows.Forms.CheckBox checkBoxVersionOne;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelTime;
    }
}