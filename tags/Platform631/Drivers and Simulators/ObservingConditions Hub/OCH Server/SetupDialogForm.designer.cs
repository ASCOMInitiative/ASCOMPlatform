namespace ASCOM.Simulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.chkDebugTrace = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chkConnectToDrivers = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblNumberOfReadingsToAverage = new System.Windows.Forms.Label();
            this.numNumberOfReadingsToAverage = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.numAveragePeriod = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.chkOverrideSafetyLimits = new System.Windows.Forms.CheckBox();
            this.sensorViewWindSpeed = new ASCOM.Simulator.SensorView();
            this.sensorViewWindGust = new ASCOM.Simulator.SensorView();
            this.sensorViewWindDirection = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewStarFWHM = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyQuality = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyBrightness = new ASCOM.Simulator.SensorView();
            this.sensorViewRainRate = new ASCOM.Simulator.SensorView();
            this.sensorViewPressure = new ASCOM.Simulator.SensorView();
            this.sensorViewHumidity = new ASCOM.Simulator.SensorView();
            this.sensorViewDewPoint = new ASCOM.Simulator.SensorView();
            this.sensorViewCloudCover = new ASCOM.Simulator.SensorView();
            this.sensorViewTemperature = new ASCOM.Simulator.SensorView();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfReadingsToAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(993, 503);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(993, 533);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(86, 13);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            this.chkTrace.CheckedChanged += new System.EventHandler(this.chkTrace_CheckedChanged);
            // 
            // chkDebugTrace
            // 
            this.chkDebugTrace.AutoSize = true;
            this.chkDebugTrace.Location = new System.Drawing.Point(86, 40);
            this.chkDebugTrace.Name = "chkDebugTrace";
            this.chkDebugTrace.Size = new System.Drawing.Size(121, 17);
            this.chkDebugTrace.TabIndex = 9;
            this.chkDebugTrace.Text = "Include debug trace";
            this.chkDebugTrace.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(21, 522);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(647, 13);
            this.label35.TabIndex = 63;
            this.label35.Text = "* Switch Number is only available when a Switch is selected, Sensor Description i" +
    "s only available when \"Connect to drivers\" is checked.";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label34.Location = new System.Drawing.Point(786, 107);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(119, 13);
            this.label34.TabIndex = 62;
            this.label34.Text = "Sensor Description*";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label33.Location = new System.Drawing.Point(574, 107);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(97, 13);
            this.label33.TabIndex = 61;
            this.label33.Text = "Switch Number*";
            this.label33.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label32.Location = new System.Drawing.Point(248, 107);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(95, 13);
            this.label32.TabIndex = 60;
            this.label32.Text = "Selected Driver";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(44, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Cloud Cover";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label3.Location = new System.Drawing.Point(55, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Dew Point";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label4.Location = new System.Drawing.Point(64, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Pressure";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label5.Location = new System.Drawing.Point(65, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Humidity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label6.Location = new System.Drawing.Point(56, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Rain Rate";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label7.Location = new System.Drawing.Point(29, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Sky Brightness";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label8.Location = new System.Drawing.Point(49, 297);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Sky Quality";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label9.Location = new System.Drawing.Point(48, 351);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Star FWHM";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label10.Location = new System.Drawing.Point(17, 324);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Sky Temperature";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label11.Location = new System.Drawing.Point(42, 379);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Temperature";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label12.Location = new System.Drawing.Point(29, 406);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Wind Direction";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label13.Location = new System.Drawing.Point(54, 433);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Wind Gust";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label14.Location = new System.Drawing.Point(44, 460);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Wind Speed";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkConnectToDrivers
            // 
            this.chkConnectToDrivers.AutoSize = true;
            this.chkConnectToDrivers.Location = new System.Drawing.Point(86, 64);
            this.chkConnectToDrivers.Name = "chkConnectToDrivers";
            this.chkConnectToDrivers.Size = new System.Drawing.Size(112, 17);
            this.chkConnectToDrivers.TabIndex = 25;
            this.chkConnectToDrivers.Text = "Connect to drivers";
            this.chkConnectToDrivers.UseVisualStyleBackColor = true;
            this.chkConnectToDrivers.CheckedChanged += new System.EventHandler(this.chkConnectToDrivers_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label15.Location = new System.Drawing.Point(501, 107);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 79;
            this.label15.Text = "Setup";
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Location = new System.Drawing.Point(474, 100);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(2, 385);
            this.label17.TabIndex = 81;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.Location = new System.Drawing.Point(24, 487);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(1020, 2);
            this.label16.TabIndex = 80;
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Location = new System.Drawing.Point(24, 95);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(1020, 2);
            this.label18.TabIndex = 82;
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label19.Location = new System.Drawing.Point(566, 100);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(2, 385);
            this.label19.TabIndex = 83;
            // 
            // lblNumberOfReadingsToAverage
            // 
            this.lblNumberOfReadingsToAverage.AutoSize = true;
            this.lblNumberOfReadingsToAverage.Location = new System.Drawing.Point(565, 41);
            this.lblNumberOfReadingsToAverage.Name = "lblNumberOfReadingsToAverage";
            this.lblNumberOfReadingsToAverage.Size = new System.Drawing.Size(203, 13);
            this.lblNumberOfReadingsToAverage.TabIndex = 87;
            this.lblNumberOfReadingsToAverage.Text = "Number of readings within average period";
            // 
            // numNumberOfReadingsToAverage
            // 
            this.numNumberOfReadingsToAverage.BackColor = System.Drawing.SystemColors.Window;
            this.numNumberOfReadingsToAverage.Location = new System.Drawing.Point(485, 38);
            this.numNumberOfReadingsToAverage.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numNumberOfReadingsToAverage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumberOfReadingsToAverage.Name = "numNumberOfReadingsToAverage";
            this.numNumberOfReadingsToAverage.Size = new System.Drawing.Size(75, 20);
            this.numNumberOfReadingsToAverage.TabIndex = 86;
            this.numNumberOfReadingsToAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNumberOfReadingsToAverage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(565, 15);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(147, 13);
            this.label20.TabIndex = 85;
            this.label20.Text = "minutes ( 0.0 = no averaging )";
            // 
            // numAveragePeriod
            // 
            this.numAveragePeriod.DecimalPlaces = 1;
            this.numAveragePeriod.Location = new System.Drawing.Point(485, 12);
            this.numAveragePeriod.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numAveragePeriod.Name = "numAveragePeriod";
            this.numAveragePeriod.Size = new System.Drawing.Size(75, 20);
            this.numAveragePeriod.TabIndex = 84;
            this.numAveragePeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label21.Location = new System.Drawing.Point(385, 15);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(94, 13);
            this.label21.TabIndex = 88;
            this.label21.Text = "Average Period";
            this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(766, 41);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(92, 13);
            this.lblWarning.TabIndex = 89;
            this.lblWarning.Text = "Warning message";
            // 
            // chkOverrideSafetyLimits
            // 
            this.chkOverrideSafetyLimits.AutoSize = true;
            this.chkOverrideSafetyLimits.Location = new System.Drawing.Point(548, 64);
            this.chkOverrideSafetyLimits.Name = "chkOverrideSafetyLimits";
            this.chkOverrideSafetyLimits.Size = new System.Drawing.Size(136, 17);
            this.chkOverrideSafetyLimits.TabIndex = 90;
            this.chkOverrideSafetyLimits.Text = "Override UI safety limits";
            this.chkOverrideSafetyLimits.UseVisualStyleBackColor = true;
            this.chkOverrideSafetyLimits.CheckedChanged += new System.EventHandler(this.chkOverrideSafetyLimits_CheckedChanged);
            // 
            // sensorViewWindSpeed
            // 
            this.sensorViewWindSpeed.ConnectToDriver = false;
            this.sensorViewWindSpeed.Location = new System.Drawing.Point(133, 454);
            this.sensorViewWindSpeed.Name = "sensorViewWindSpeed";
            this.sensorViewWindSpeed.SensorName = "WindSpeed";
            this.sensorViewWindSpeed.Size = new System.Drawing.Size(894, 24);
            this.sensorViewWindSpeed.TabIndex = 78;
            // 
            // sensorViewWindGust
            // 
            this.sensorViewWindGust.ConnectToDriver = false;
            this.sensorViewWindGust.Location = new System.Drawing.Point(133, 427);
            this.sensorViewWindGust.Name = "sensorViewWindGust";
            this.sensorViewWindGust.SensorName = "WindGust";
            this.sensorViewWindGust.Size = new System.Drawing.Size(894, 24);
            this.sensorViewWindGust.TabIndex = 77;
            // 
            // sensorViewWindDirection
            // 
            this.sensorViewWindDirection.ConnectToDriver = false;
            this.sensorViewWindDirection.Location = new System.Drawing.Point(133, 400);
            this.sensorViewWindDirection.Name = "sensorViewWindDirection";
            this.sensorViewWindDirection.SensorName = "WindDirection";
            this.sensorViewWindDirection.Size = new System.Drawing.Size(894, 24);
            this.sensorViewWindDirection.TabIndex = 76;
            // 
            // sensorViewSkyTemperature
            // 
            this.sensorViewSkyTemperature.ConnectToDriver = false;
            this.sensorViewSkyTemperature.Location = new System.Drawing.Point(133, 319);
            this.sensorViewSkyTemperature.Name = "sensorViewSkyTemperature";
            this.sensorViewSkyTemperature.SensorName = "SkyTemperature";
            this.sensorViewSkyTemperature.Size = new System.Drawing.Size(894, 24);
            this.sensorViewSkyTemperature.TabIndex = 75;
            // 
            // sensorViewStarFWHM
            // 
            this.sensorViewStarFWHM.ConnectToDriver = false;
            this.sensorViewStarFWHM.Location = new System.Drawing.Point(133, 346);
            this.sensorViewStarFWHM.Name = "sensorViewStarFWHM";
            this.sensorViewStarFWHM.SensorName = "StarFWHM";
            this.sensorViewStarFWHM.Size = new System.Drawing.Size(894, 24);
            this.sensorViewStarFWHM.TabIndex = 74;
            // 
            // sensorViewSkyQuality
            // 
            this.sensorViewSkyQuality.ConnectToDriver = false;
            this.sensorViewSkyQuality.Location = new System.Drawing.Point(133, 292);
            this.sensorViewSkyQuality.Name = "sensorViewSkyQuality";
            this.sensorViewSkyQuality.SensorName = "SkyQuality";
            this.sensorViewSkyQuality.Size = new System.Drawing.Size(894, 24);
            this.sensorViewSkyQuality.TabIndex = 73;
            // 
            // sensorViewSkyBrightness
            // 
            this.sensorViewSkyBrightness.ConnectToDriver = false;
            this.sensorViewSkyBrightness.Location = new System.Drawing.Point(133, 265);
            this.sensorViewSkyBrightness.Name = "sensorViewSkyBrightness";
            this.sensorViewSkyBrightness.SensorName = "SkyBrightness";
            this.sensorViewSkyBrightness.Size = new System.Drawing.Size(894, 24);
            this.sensorViewSkyBrightness.TabIndex = 72;
            // 
            // sensorViewRainRate
            // 
            this.sensorViewRainRate.ConnectToDriver = false;
            this.sensorViewRainRate.Location = new System.Drawing.Point(133, 238);
            this.sensorViewRainRate.Name = "sensorViewRainRate";
            this.sensorViewRainRate.SensorName = "RainRate";
            this.sensorViewRainRate.Size = new System.Drawing.Size(894, 24);
            this.sensorViewRainRate.TabIndex = 70;
            // 
            // sensorViewPressure
            // 
            this.sensorViewPressure.ConnectToDriver = false;
            this.sensorViewPressure.Location = new System.Drawing.Point(133, 211);
            this.sensorViewPressure.Name = "sensorViewPressure";
            this.sensorViewPressure.SensorName = "Pressure";
            this.sensorViewPressure.Size = new System.Drawing.Size(894, 24);
            this.sensorViewPressure.TabIndex = 69;
            // 
            // sensorViewHumidity
            // 
            this.sensorViewHumidity.ConnectToDriver = false;
            this.sensorViewHumidity.Location = new System.Drawing.Point(133, 184);
            this.sensorViewHumidity.Name = "sensorViewHumidity";
            this.sensorViewHumidity.SensorName = "Humidity";
            this.sensorViewHumidity.Size = new System.Drawing.Size(894, 24);
            this.sensorViewHumidity.TabIndex = 68;
            // 
            // sensorViewDewPoint
            // 
            this.sensorViewDewPoint.ConnectToDriver = false;
            this.sensorViewDewPoint.Location = new System.Drawing.Point(133, 157);
            this.sensorViewDewPoint.Name = "sensorViewDewPoint";
            this.sensorViewDewPoint.SensorName = "DewPoint";
            this.sensorViewDewPoint.Size = new System.Drawing.Size(894, 24);
            this.sensorViewDewPoint.TabIndex = 67;
            // 
            // sensorViewCloudCover
            // 
            this.sensorViewCloudCover.ConnectToDriver = false;
            this.sensorViewCloudCover.Location = new System.Drawing.Point(133, 130);
            this.sensorViewCloudCover.Name = "sensorViewCloudCover";
            this.sensorViewCloudCover.SensorName = "CloudCover";
            this.sensorViewCloudCover.Size = new System.Drawing.Size(894, 24);
            this.sensorViewCloudCover.TabIndex = 66;
            // 
            // sensorViewTemperature
            // 
            this.sensorViewTemperature.ConnectToDriver = false;
            this.sensorViewTemperature.Location = new System.Drawing.Point(133, 373);
            this.sensorViewTemperature.Name = "sensorViewTemperature";
            this.sensorViewTemperature.SensorName = "Temperature";
            this.sensorViewTemperature.Size = new System.Drawing.Size(894, 24);
            this.sensorViewTemperature.TabIndex = 64;
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(1064, 570);
            this.Controls.Add(this.chkOverrideSafetyLimits);
            this.Controls.Add(this.lblNumberOfReadingsToAverage);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.numNumberOfReadingsToAverage);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.numAveragePeriod);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.sensorViewWindSpeed);
            this.Controls.Add(this.chkConnectToDrivers);
            this.Controls.Add(this.sensorViewWindGust);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.sensorViewWindDirection);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.sensorViewSkyTemperature);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.sensorViewStarFWHM);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.sensorViewSkyQuality);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.sensorViewSkyBrightness);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.sensorViewRainRate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.sensorViewPressure);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.sensorViewHumidity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sensorViewDewPoint);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.sensorViewCloudCover);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sensorViewTemperature);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.chkDebugTrace);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Observing Conditions Hub Configurtation";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfReadingsToAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.CheckBox chkDebugTrace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkConnectToDrivers;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private SensorView sensorViewTemperature;
        private SensorView sensorViewRainRate;
        private SensorView sensorViewPressure;
        private SensorView sensorViewHumidity;
        private SensorView sensorViewDewPoint;
        private SensorView sensorViewCloudCover;
        private SensorView sensorViewWindSpeed;
        private SensorView sensorViewWindGust;
        private SensorView sensorViewWindDirection;
        private SensorView sensorViewSkyTemperature;
        private SensorView sensorViewStarFWHM;
        private SensorView sensorViewSkyQuality;
        private SensorView sensorViewSkyBrightness;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblNumberOfReadingsToAverage;
        private System.Windows.Forms.NumericUpDown numNumberOfReadingsToAverage;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown numAveragePeriod;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.CheckBox chkOverrideSafetyLimits;
    }
}