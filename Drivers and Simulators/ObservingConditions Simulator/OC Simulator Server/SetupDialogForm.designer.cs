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
            this.label15 = new System.Windows.Forms.Label();
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
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.numSensorQueryInterval = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.numAveragePeriod = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.numNumberOfReadingsToAverage = new System.Windows.Forms.NumericUpDown();
            this.sensorViewWindDirection = new ASCOM.Simulator.SensorView();
            this.sensorViewWindGust = new ASCOM.Simulator.SensorView();
            this.sensorViewWindSpeed = new ASCOM.Simulator.SensorView();
            this.sensorViewTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewHumidity = new ASCOM.Simulator.SensorView();
            this.sensorViewCloudCover = new ASCOM.Simulator.SensorView();
            this.sensorViewRainRate = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyBrightness = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyQuality = new ASCOM.Simulator.SensorView();
            this.sensorViewStarFWHM = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewPressure = new ASCOM.Simulator.SensorView();
            ((System.ComponentModel.ISupportInitialize)(this.numSensorQueryInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfReadingsToAverage)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(861, 528);
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
            this.cmdCancel.Location = new System.Drawing.Point(861, 558);
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
            this.chkTrace.Location = new System.Drawing.Point(73, 12);
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
            this.chkDebugTrace.Location = new System.Drawing.Point(73, 36);
            this.chkDebugTrace.Name = "chkDebugTrace";
            this.chkDebugTrace.Size = new System.Drawing.Size(121, 17);
            this.chkDebugTrace.TabIndex = 9;
            this.chkDebugTrace.Text = "Include debug trace";
            this.chkDebugTrace.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label15.Location = new System.Drawing.Point(296, 105);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(315, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Vary sensor value between these limits over (seconds)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(35, 131);
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
            this.label3.Location = new System.Drawing.Point(46, 158);
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
            this.label4.Location = new System.Drawing.Point(55, 216);
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
            this.label5.Location = new System.Drawing.Point(56, 185);
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
            this.label6.Location = new System.Drawing.Point(47, 246);
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
            this.label7.Location = new System.Drawing.Point(20, 275);
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
            this.label8.Location = new System.Drawing.Point(40, 306);
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
            this.label9.Location = new System.Drawing.Point(39, 366);
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
            this.label10.Location = new System.Drawing.Point(8, 336);
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
            this.label11.Location = new System.Drawing.Point(33, 396);
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
            this.label12.Location = new System.Drawing.Point(20, 426);
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
            this.label13.Location = new System.Drawing.Point(45, 456);
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
            this.label14.Location = new System.Drawing.Point(35, 485);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Wind Speed";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(430, 131);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Percent";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(430, 185);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(44, 13);
            this.label21.TabIndex = 35;
            this.label21.Text = "Percent";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(430, 216);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(26, 13);
            this.label22.TabIndex = 36;
            this.label22.Text = "hPa";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(430, 246);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 37;
            this.label23.Text = "mm / hr";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(430, 275);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 13);
            this.label24.TabIndex = 38;
            this.label24.Text = "Lux";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(430, 306);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(108, 13);
            this.label25.TabIndex = 39;
            this.label25.Text = "Mag / Square arcsec";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(430, 366);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 13);
            this.label26.TabIndex = 40;
            this.label26.Text = "Arcsec FHWM";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(430, 336);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 13);
            this.label27.TabIndex = 41;
            this.label27.Text = "Degrees Celsius";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(430, 396);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 13);
            this.label28.TabIndex = 42;
            this.label28.Text = "Degrees Celsius";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(430, 426);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(47, 13);
            this.label29.TabIndex = 43;
            this.label29.Text = "Degrees";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(430, 456);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(73, 13);
            this.label30.TabIndex = 44;
            this.label30.Text = "Miles per hour";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(430, 485);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(73, 13);
            this.label31.TabIndex = 45;
            this.label31.Text = "Miles per hour";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label32.Location = new System.Drawing.Point(689, 105);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(197, 13);
            this.label32.TabIndex = 60;
            this.label32.Text = "Simulate Not Ready for (seconds)";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label33.Location = new System.Drawing.Point(110, 105);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(96, 13);
            this.label33.TabIndex = 61;
            this.label33.Text = "Sensor Enabled";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(347, 158);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(207, 13);
            this.label35.TabIndex = 63;
            this.label35.Text = "Calculated from Temperature and Humidity";
            // 
            // numSensorQueryInterval
            // 
            this.numSensorQueryInterval.DecimalPlaces = 1;
            this.numSensorQueryInterval.Location = new System.Drawing.Point(10, 59);
            this.numSensorQueryInterval.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numSensorQueryInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSensorQueryInterval.Name = "numSensorQueryInterval";
            this.numSensorQueryInterval.Size = new System.Drawing.Size(75, 20);
            this.numSensorQueryInterval.TabIndex = 64;
            this.numSensorQueryInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSensorQueryInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(87, 61);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(155, 13);
            this.label36.TabIndex = 65;
            this.label36.Text = "Sensor query interval (seconds)";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(8, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(912, 2);
            this.label1.TabIndex = 66;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.Location = new System.Drawing.Point(10, 515);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(912, 2);
            this.label16.TabIndex = 67;
            // 
            // label17
            // 
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Location = new System.Drawing.Point(659, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(2, 419);
            this.label17.TabIndex = 68;
            // 
            // label18
            // 
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Location = new System.Drawing.Point(244, 95);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(2, 419);
            this.label18.TabIndex = 69;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(426, 26);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(124, 13);
            this.label20.TabIndex = 71;
            this.label20.Text = "Average period (minutes)";
            // 
            // numAveragePeriod
            // 
            this.numAveragePeriod.DecimalPlaces = 1;
            this.numAveragePeriod.Location = new System.Drawing.Point(345, 24);
            this.numAveragePeriod.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numAveragePeriod.Name = "numAveragePeriod";
            this.numAveragePeriod.Size = new System.Drawing.Size(75, 20);
            this.numAveragePeriod.TabIndex = 70;
            this.numAveragePeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numAveragePeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(426, 52);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(203, 13);
            this.label34.TabIndex = 73;
            this.label34.Text = "Number of readings within average period";
            // 
            // numNumberOfReadingsToAverage
            // 
            this.numNumberOfReadingsToAverage.Location = new System.Drawing.Point(345, 50);
            this.numNumberOfReadingsToAverage.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numNumberOfReadingsToAverage.Name = "numNumberOfReadingsToAverage";
            this.numNumberOfReadingsToAverage.Size = new System.Drawing.Size(75, 20);
            this.numNumberOfReadingsToAverage.TabIndex = 72;
            this.numNumberOfReadingsToAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNumberOfReadingsToAverage.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // sensorViewWindDirection
            // 
            this.sensorViewWindDirection.ConnectToDriver = false;
            this.sensorViewWindDirection.EnabledCheckboxVisible = true;
            this.sensorViewWindDirection.Location = new System.Drawing.Point(140, 420);
            this.sensorViewWindDirection.MaxValue = 0D;
            this.sensorViewWindDirection.MinValue = 0D;
            this.sensorViewWindDirection.Name = "sensorViewWindDirection";
            this.sensorViewWindDirection.NotReadyControlsEnabled = true;
            this.sensorViewWindDirection.NotReadyDelay = 0D;
            this.sensorViewWindDirection.SensorEnabled = true;
            this.sensorViewWindDirection.SensorName = null;
            this.sensorViewWindDirection.Size = new System.Drawing.Size(692, 24);
            this.sensorViewWindDirection.TabIndex = 54;
            this.sensorViewWindDirection.ValueCycleTime = 0D;
            // 
            // sensorViewWindGust
            // 
            this.sensorViewWindGust.ConnectToDriver = false;
            this.sensorViewWindGust.EnabledCheckboxVisible = true;
            this.sensorViewWindGust.Location = new System.Drawing.Point(140, 450);
            this.sensorViewWindGust.MaxValue = 0D;
            this.sensorViewWindGust.MinValue = 0D;
            this.sensorViewWindGust.Name = "sensorViewWindGust";
            this.sensorViewWindGust.NotReadyControlsEnabled = true;
            this.sensorViewWindGust.NotReadyDelay = 0D;
            this.sensorViewWindGust.SensorEnabled = true;
            this.sensorViewWindGust.SensorName = null;
            this.sensorViewWindGust.Size = new System.Drawing.Size(692, 24);
            this.sensorViewWindGust.TabIndex = 53;
            this.sensorViewWindGust.ValueCycleTime = 0D;
            // 
            // sensorViewWindSpeed
            // 
            this.sensorViewWindSpeed.ConnectToDriver = false;
            this.sensorViewWindSpeed.EnabledCheckboxVisible = true;
            this.sensorViewWindSpeed.Location = new System.Drawing.Point(140, 480);
            this.sensorViewWindSpeed.MaxValue = 0D;
            this.sensorViewWindSpeed.MinValue = 0D;
            this.sensorViewWindSpeed.Name = "sensorViewWindSpeed";
            this.sensorViewWindSpeed.NotReadyControlsEnabled = true;
            this.sensorViewWindSpeed.NotReadyDelay = 0D;
            this.sensorViewWindSpeed.SensorEnabled = true;
            this.sensorViewWindSpeed.SensorName = null;
            this.sensorViewWindSpeed.Size = new System.Drawing.Size(692, 24);
            this.sensorViewWindSpeed.TabIndex = 52;
            this.sensorViewWindSpeed.ValueCycleTime = 0D;
            // 
            // sensorViewTemperature
            // 
            this.sensorViewTemperature.ConnectToDriver = false;
            this.sensorViewTemperature.EnabledCheckboxVisible = true;
            this.sensorViewTemperature.Location = new System.Drawing.Point(140, 390);
            this.sensorViewTemperature.MaxValue = 0D;
            this.sensorViewTemperature.MinValue = 0D;
            this.sensorViewTemperature.Name = "sensorViewTemperature";
            this.sensorViewTemperature.NotReadyControlsEnabled = true;
            this.sensorViewTemperature.NotReadyDelay = 0D;
            this.sensorViewTemperature.SensorEnabled = true;
            this.sensorViewTemperature.SensorName = null;
            this.sensorViewTemperature.Size = new System.Drawing.Size(692, 24);
            this.sensorViewTemperature.TabIndex = 51;
            this.sensorViewTemperature.ValueCycleTime = 0D;
            // 
            // sensorViewHumidity
            // 
            this.sensorViewHumidity.ConnectToDriver = false;
            this.sensorViewHumidity.EnabledCheckboxVisible = true;
            this.sensorViewHumidity.Location = new System.Drawing.Point(140, 180);
            this.sensorViewHumidity.MaxValue = 0D;
            this.sensorViewHumidity.MinValue = 0D;
            this.sensorViewHumidity.Name = "sensorViewHumidity";
            this.sensorViewHumidity.NotReadyControlsEnabled = true;
            this.sensorViewHumidity.NotReadyDelay = 0D;
            this.sensorViewHumidity.SensorEnabled = true;
            this.sensorViewHumidity.SensorName = null;
            this.sensorViewHumidity.Size = new System.Drawing.Size(692, 24);
            this.sensorViewHumidity.TabIndex = 50;
            this.sensorViewHumidity.ValueCycleTime = 0D;
            // 
            // sensorViewCloudCover
            // 
            this.sensorViewCloudCover.ConnectToDriver = false;
            this.sensorViewCloudCover.EnabledCheckboxVisible = true;
            this.sensorViewCloudCover.Location = new System.Drawing.Point(140, 126);
            this.sensorViewCloudCover.MaxValue = 0D;
            this.sensorViewCloudCover.MinValue = 0D;
            this.sensorViewCloudCover.Name = "sensorViewCloudCover";
            this.sensorViewCloudCover.NotReadyControlsEnabled = true;
            this.sensorViewCloudCover.NotReadyDelay = 0D;
            this.sensorViewCloudCover.SensorEnabled = true;
            this.sensorViewCloudCover.SensorName = null;
            this.sensorViewCloudCover.Size = new System.Drawing.Size(692, 24);
            this.sensorViewCloudCover.TabIndex = 48;
            this.sensorViewCloudCover.ValueCycleTime = 0D;
            // 
            // sensorViewRainRate
            // 
            this.sensorViewRainRate.ConnectToDriver = false;
            this.sensorViewRainRate.EnabledCheckboxVisible = true;
            this.sensorViewRainRate.Location = new System.Drawing.Point(140, 240);
            this.sensorViewRainRate.MaxValue = 0D;
            this.sensorViewRainRate.MinValue = 0D;
            this.sensorViewRainRate.Name = "sensorViewRainRate";
            this.sensorViewRainRate.NotReadyControlsEnabled = true;
            this.sensorViewRainRate.NotReadyDelay = 0D;
            this.sensorViewRainRate.SensorEnabled = true;
            this.sensorViewRainRate.SensorName = null;
            this.sensorViewRainRate.Size = new System.Drawing.Size(692, 24);
            this.sensorViewRainRate.TabIndex = 59;
            this.sensorViewRainRate.ValueCycleTime = 0D;
            // 
            // sensorViewSkyBrightness
            // 
            this.sensorViewSkyBrightness.ConnectToDriver = false;
            this.sensorViewSkyBrightness.EnabledCheckboxVisible = true;
            this.sensorViewSkyBrightness.Location = new System.Drawing.Point(140, 270);
            this.sensorViewSkyBrightness.MaxValue = 0D;
            this.sensorViewSkyBrightness.MinValue = 0D;
            this.sensorViewSkyBrightness.Name = "sensorViewSkyBrightness";
            this.sensorViewSkyBrightness.NotReadyControlsEnabled = true;
            this.sensorViewSkyBrightness.NotReadyDelay = 0D;
            this.sensorViewSkyBrightness.SensorEnabled = true;
            this.sensorViewSkyBrightness.SensorName = null;
            this.sensorViewSkyBrightness.Size = new System.Drawing.Size(692, 24);
            this.sensorViewSkyBrightness.TabIndex = 58;
            this.sensorViewSkyBrightness.ValueCycleTime = 0D;
            // 
            // sensorViewSkyQuality
            // 
            this.sensorViewSkyQuality.ConnectToDriver = false;
            this.sensorViewSkyQuality.EnabledCheckboxVisible = true;
            this.sensorViewSkyQuality.Location = new System.Drawing.Point(140, 300);
            this.sensorViewSkyQuality.MaxValue = 0D;
            this.sensorViewSkyQuality.MinValue = 0D;
            this.sensorViewSkyQuality.Name = "sensorViewSkyQuality";
            this.sensorViewSkyQuality.NotReadyControlsEnabled = true;
            this.sensorViewSkyQuality.NotReadyDelay = 0D;
            this.sensorViewSkyQuality.SensorEnabled = true;
            this.sensorViewSkyQuality.SensorName = null;
            this.sensorViewSkyQuality.Size = new System.Drawing.Size(692, 24);
            this.sensorViewSkyQuality.TabIndex = 57;
            this.sensorViewSkyQuality.ValueCycleTime = 0D;
            // 
            // sensorViewStarFWHM
            // 
            this.sensorViewStarFWHM.ConnectToDriver = false;
            this.sensorViewStarFWHM.EnabledCheckboxVisible = true;
            this.sensorViewStarFWHM.Location = new System.Drawing.Point(140, 360);
            this.sensorViewStarFWHM.MaxValue = 0D;
            this.sensorViewStarFWHM.MinValue = 0D;
            this.sensorViewStarFWHM.Name = "sensorViewStarFWHM";
            this.sensorViewStarFWHM.NotReadyControlsEnabled = true;
            this.sensorViewStarFWHM.NotReadyDelay = 0D;
            this.sensorViewStarFWHM.SensorEnabled = true;
            this.sensorViewStarFWHM.SensorName = null;
            this.sensorViewStarFWHM.Size = new System.Drawing.Size(692, 24);
            this.sensorViewStarFWHM.TabIndex = 56;
            this.sensorViewStarFWHM.ValueCycleTime = 0D;
            // 
            // sensorViewSkyTemperature
            // 
            this.sensorViewSkyTemperature.ConnectToDriver = false;
            this.sensorViewSkyTemperature.EnabledCheckboxVisible = true;
            this.sensorViewSkyTemperature.Location = new System.Drawing.Point(140, 330);
            this.sensorViewSkyTemperature.MaxValue = 0D;
            this.sensorViewSkyTemperature.MinValue = 0D;
            this.sensorViewSkyTemperature.Name = "sensorViewSkyTemperature";
            this.sensorViewSkyTemperature.NotReadyControlsEnabled = true;
            this.sensorViewSkyTemperature.NotReadyDelay = 0D;
            this.sensorViewSkyTemperature.SensorEnabled = true;
            this.sensorViewSkyTemperature.SensorName = null;
            this.sensorViewSkyTemperature.Size = new System.Drawing.Size(692, 24);
            this.sensorViewSkyTemperature.TabIndex = 55;
            this.sensorViewSkyTemperature.ValueCycleTime = 0D;
            // 
            // sensorViewPressure
            // 
            this.sensorViewPressure.ConnectToDriver = false;
            this.sensorViewPressure.EnabledCheckboxVisible = true;
            this.sensorViewPressure.Location = new System.Drawing.Point(140, 210);
            this.sensorViewPressure.MaxValue = 0D;
            this.sensorViewPressure.MinValue = 0D;
            this.sensorViewPressure.Name = "sensorViewPressure";
            this.sensorViewPressure.NotReadyControlsEnabled = true;
            this.sensorViewPressure.NotReadyDelay = 0D;
            this.sensorViewPressure.SensorEnabled = true;
            this.sensorViewPressure.SensorName = null;
            this.sensorViewPressure.Size = new System.Drawing.Size(692, 24);
            this.sensorViewPressure.TabIndex = 46;
            this.sensorViewPressure.ValueCycleTime = 0D;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 595);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.numNumberOfReadingsToAverage);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.numAveragePeriod);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.numSensorQueryInterval);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkDebugTrace);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.sensorViewWindDirection);
            this.Controls.Add(this.sensorViewWindGust);
            this.Controls.Add(this.sensorViewWindSpeed);
            this.Controls.Add(this.sensorViewTemperature);
            this.Controls.Add(this.sensorViewHumidity);
            this.Controls.Add(this.sensorViewCloudCover);
            this.Controls.Add(this.sensorViewRainRate);
            this.Controls.Add(this.sensorViewSkyBrightness);
            this.Controls.Add(this.sensorViewSkyQuality);
            this.Controls.Add(this.sensorViewStarFWHM);
            this.Controls.Add(this.sensorViewSkyTemperature);
            this.Controls.Add(this.sensorViewPressure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Observing Conditions Simulator Configurtation";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSensorQueryInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfReadingsToAverage)).EndInit();
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
        private System.Windows.Forms.Label label15;
        private SensorView sensorViewPressure;
        private SensorView sensorViewCloudCover;
        private SensorView sensorViewHumidity;
        private SensorView sensorViewTemperature;
        private SensorView sensorViewWindSpeed;
        private SensorView sensorViewWindGust;
        private SensorView sensorViewWindDirection;
        private SensorView sensorViewSkyTemperature;
        private SensorView sensorViewStarFWHM;
        private SensorView sensorViewSkyQuality;
        private SensorView sensorViewSkyBrightness;
        private SensorView sensorViewRainRate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown numSensorQueryInterval;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown numAveragePeriod;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.NumericUpDown numNumberOfReadingsToAverage;
    }
}