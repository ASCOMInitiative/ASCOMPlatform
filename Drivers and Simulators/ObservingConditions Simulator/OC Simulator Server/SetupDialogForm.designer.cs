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
            this.debugTrace = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtHighAveragePeriod = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtHighWindSpeed = new System.Windows.Forms.TextBox();
            this.txtHighWindGust = new System.Windows.Forms.TextBox();
            this.txtHighWindDirection = new System.Windows.Forms.TextBox();
            this.txtHighTemperature = new System.Windows.Forms.TextBox();
            this.txtHighSkyTemperature = new System.Windows.Forms.TextBox();
            this.txtHighSkySeeing = new System.Windows.Forms.TextBox();
            this.txtHighSkyQuality = new System.Windows.Forms.TextBox();
            this.txtHighSkyBrightness = new System.Windows.Forms.TextBox();
            this.txtHighRainRate = new System.Windows.Forms.TextBox();
            this.txtHighPressure = new System.Windows.Forms.TextBox();
            this.txtHighHumidity = new System.Windows.Forms.TextBox();
            this.txtHighDewPoint = new System.Windows.Forms.TextBox();
            this.txtHighCloudCover = new System.Windows.Forms.TextBox();
            this.txtLowWindSpeed = new System.Windows.Forms.TextBox();
            this.txtLowWindGust = new System.Windows.Forms.TextBox();
            this.txtLowWindDirection = new System.Windows.Forms.TextBox();
            this.txtLowTemperature = new System.Windows.Forms.TextBox();
            this.txtLowSkyTemperature = new System.Windows.Forms.TextBox();
            this.txtLowSkySeeing = new System.Windows.Forms.TextBox();
            this.txtLowSkyQuality = new System.Windows.Forms.TextBox();
            this.txtLowSkyBrightness = new System.Windows.Forms.TextBox();
            this.txtLowRainRate = new System.Windows.Forms.TextBox();
            this.txtLowPressure = new System.Windows.Forms.TextBox();
            this.txtLowHumidity = new System.Windows.Forms.TextBox();
            this.txtLowDewPoint = new System.Windows.Forms.TextBox();
            this.txtLowCloudCover = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLowAveragePeriod = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.sensorViewPressure = new ASCOM.Simulator.SensorView();
            this.sensorViewCloudCover = new ASCOM.Simulator.SensorView();
            this.sensorViewDewPoint = new ASCOM.Simulator.SensorView();
            this.sensorViewHumidity = new ASCOM.Simulator.SensorView();
            this.sensorViewTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewWindSpeed = new ASCOM.Simulator.SensorView();
            this.sensorViewWindGust = new ASCOM.Simulator.SensorView();
            this.sensorViewWindDirection = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewSkySeeing = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyQuality = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyBrightness = new ASCOM.Simulator.SensorView();
            this.sensorViewRainRate = new ASCOM.Simulator.SensorView();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(1125, 968);
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
            this.cmdCancel.Location = new System.Drawing.Point(1125, 998);
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
            this.chkTrace.Location = new System.Drawing.Point(58, 12);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // debugTrace
            // 
            this.debugTrace.AutoSize = true;
            this.debugTrace.Location = new System.Drawing.Point(58, 36);
            this.debugTrace.Name = "debugTrace";
            this.debugTrace.Size = new System.Drawing.Size(121, 17);
            this.debugTrace.TabIndex = 9;
            this.debugTrace.Text = "Include debug trace";
            this.debugTrace.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(366, 512);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(73, 13);
            this.label31.TabIndex = 45;
            this.label31.Text = "Miles per hour";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(366, 485);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(73, 13);
            this.label30.TabIndex = 44;
            this.label30.Text = "Miles per hour";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(366, 458);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(47, 13);
            this.label29.TabIndex = 43;
            this.label29.Text = "Degrees";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(366, 431);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 13);
            this.label28.TabIndex = 42;
            this.label28.Text = "Degrees Celsius";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(366, 405);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 13);
            this.label27.TabIndex = 41;
            this.label27.Text = "Degrees Celsius";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(366, 378);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 13);
            this.label26.TabIndex = 40;
            this.label26.Text = "Arcsec FHWM";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(366, 351);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(108, 13);
            this.label25.TabIndex = 39;
            this.label25.Text = "Mag / Square arcsec";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(366, 324);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 13);
            this.label24.TabIndex = 38;
            this.label24.Text = "Lux";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(366, 297);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 37;
            this.label23.Text = "mm / hr";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(366, 270);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(26, 13);
            this.label22.TabIndex = 36;
            this.label22.Text = "hPa";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(366, 244);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(44, 13);
            this.label21.TabIndex = 35;
            this.label21.Text = "Percent";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(366, 217);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 13);
            this.label20.TabIndex = 34;
            this.label20.Text = "Degrees Celsius";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(366, 190);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Percent";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(366, 163);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 32;
            this.label18.Text = "Seconds *";
            // 
            // txtHighAveragePeriod
            // 
            this.txtHighAveragePeriod.Location = new System.Drawing.Point(493, 160);
            this.txtHighAveragePeriod.Name = "txtHighAveragePeriod";
            this.txtHighAveragePeriod.Size = new System.Drawing.Size(100, 20);
            this.txtHighAveragePeriod.TabIndex = 31;
            this.txtHighAveragePeriod.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(162, 1016);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(270, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "* No high value, average period does not vary over time";
            // 
            // txtHighWindSpeed
            // 
            this.txtHighWindSpeed.Location = new System.Drawing.Point(493, 509);
            this.txtHighWindSpeed.Name = "txtHighWindSpeed";
            this.txtHighWindSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtHighWindSpeed.TabIndex = 26;
            // 
            // txtHighWindGust
            // 
            this.txtHighWindGust.Location = new System.Drawing.Point(493, 482);
            this.txtHighWindGust.Name = "txtHighWindGust";
            this.txtHighWindGust.Size = new System.Drawing.Size(100, 20);
            this.txtHighWindGust.TabIndex = 24;
            // 
            // txtHighWindDirection
            // 
            this.txtHighWindDirection.Location = new System.Drawing.Point(493, 455);
            this.txtHighWindDirection.Name = "txtHighWindDirection";
            this.txtHighWindDirection.Size = new System.Drawing.Size(100, 20);
            this.txtHighWindDirection.TabIndex = 22;
            // 
            // txtHighTemperature
            // 
            this.txtHighTemperature.Location = new System.Drawing.Point(493, 428);
            this.txtHighTemperature.Name = "txtHighTemperature";
            this.txtHighTemperature.Size = new System.Drawing.Size(100, 20);
            this.txtHighTemperature.TabIndex = 20;
            // 
            // txtHighSkyTemperature
            // 
            this.txtHighSkyTemperature.Location = new System.Drawing.Point(493, 402);
            this.txtHighSkyTemperature.Name = "txtHighSkyTemperature";
            this.txtHighSkyTemperature.Size = new System.Drawing.Size(100, 20);
            this.txtHighSkyTemperature.TabIndex = 18;
            // 
            // txtHighSkySeeing
            // 
            this.txtHighSkySeeing.Location = new System.Drawing.Point(493, 375);
            this.txtHighSkySeeing.Name = "txtHighSkySeeing";
            this.txtHighSkySeeing.Size = new System.Drawing.Size(100, 20);
            this.txtHighSkySeeing.TabIndex = 16;
            // 
            // txtHighSkyQuality
            // 
            this.txtHighSkyQuality.Location = new System.Drawing.Point(493, 348);
            this.txtHighSkyQuality.Name = "txtHighSkyQuality";
            this.txtHighSkyQuality.Size = new System.Drawing.Size(100, 20);
            this.txtHighSkyQuality.TabIndex = 14;
            // 
            // txtHighSkyBrightness
            // 
            this.txtHighSkyBrightness.Location = new System.Drawing.Point(493, 321);
            this.txtHighSkyBrightness.Name = "txtHighSkyBrightness";
            this.txtHighSkyBrightness.Size = new System.Drawing.Size(100, 20);
            this.txtHighSkyBrightness.TabIndex = 12;
            // 
            // txtHighRainRate
            // 
            this.txtHighRainRate.Location = new System.Drawing.Point(493, 294);
            this.txtHighRainRate.Name = "txtHighRainRate";
            this.txtHighRainRate.Size = new System.Drawing.Size(100, 20);
            this.txtHighRainRate.TabIndex = 10;
            // 
            // txtHighPressure
            // 
            this.txtHighPressure.Location = new System.Drawing.Point(493, 267);
            this.txtHighPressure.Name = "txtHighPressure";
            this.txtHighPressure.Size = new System.Drawing.Size(100, 20);
            this.txtHighPressure.TabIndex = 8;
            // 
            // txtHighHumidity
            // 
            this.txtHighHumidity.Location = new System.Drawing.Point(493, 241);
            this.txtHighHumidity.Name = "txtHighHumidity";
            this.txtHighHumidity.Size = new System.Drawing.Size(100, 20);
            this.txtHighHumidity.TabIndex = 6;
            // 
            // txtHighDewPoint
            // 
            this.txtHighDewPoint.Location = new System.Drawing.Point(493, 214);
            this.txtHighDewPoint.Name = "txtHighDewPoint";
            this.txtHighDewPoint.Size = new System.Drawing.Size(100, 20);
            this.txtHighDewPoint.TabIndex = 4;
            // 
            // txtHighCloudCover
            // 
            this.txtHighCloudCover.Location = new System.Drawing.Point(493, 187);
            this.txtHighCloudCover.Name = "txtHighCloudCover";
            this.txtHighCloudCover.Size = new System.Drawing.Size(100, 20);
            this.txtHighCloudCover.TabIndex = 2;
            // 
            // txtLowWindSpeed
            // 
            this.txtLowWindSpeed.Location = new System.Drawing.Point(260, 509);
            this.txtLowWindSpeed.Name = "txtLowWindSpeed";
            this.txtLowWindSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtLowWindSpeed.TabIndex = 25;
            // 
            // txtLowWindGust
            // 
            this.txtLowWindGust.Location = new System.Drawing.Point(260, 482);
            this.txtLowWindGust.Name = "txtLowWindGust";
            this.txtLowWindGust.Size = new System.Drawing.Size(100, 20);
            this.txtLowWindGust.TabIndex = 23;
            // 
            // txtLowWindDirection
            // 
            this.txtLowWindDirection.Location = new System.Drawing.Point(260, 455);
            this.txtLowWindDirection.Name = "txtLowWindDirection";
            this.txtLowWindDirection.Size = new System.Drawing.Size(100, 20);
            this.txtLowWindDirection.TabIndex = 21;
            // 
            // txtLowTemperature
            // 
            this.txtLowTemperature.Location = new System.Drawing.Point(260, 428);
            this.txtLowTemperature.Name = "txtLowTemperature";
            this.txtLowTemperature.Size = new System.Drawing.Size(100, 20);
            this.txtLowTemperature.TabIndex = 19;
            // 
            // txtLowSkyTemperature
            // 
            this.txtLowSkyTemperature.Location = new System.Drawing.Point(260, 402);
            this.txtLowSkyTemperature.Name = "txtLowSkyTemperature";
            this.txtLowSkyTemperature.Size = new System.Drawing.Size(100, 20);
            this.txtLowSkyTemperature.TabIndex = 17;
            // 
            // txtLowSkySeeing
            // 
            this.txtLowSkySeeing.Location = new System.Drawing.Point(260, 375);
            this.txtLowSkySeeing.Name = "txtLowSkySeeing";
            this.txtLowSkySeeing.Size = new System.Drawing.Size(100, 20);
            this.txtLowSkySeeing.TabIndex = 15;
            // 
            // txtLowSkyQuality
            // 
            this.txtLowSkyQuality.Location = new System.Drawing.Point(260, 348);
            this.txtLowSkyQuality.Name = "txtLowSkyQuality";
            this.txtLowSkyQuality.Size = new System.Drawing.Size(100, 20);
            this.txtLowSkyQuality.TabIndex = 13;
            // 
            // txtLowSkyBrightness
            // 
            this.txtLowSkyBrightness.Location = new System.Drawing.Point(260, 321);
            this.txtLowSkyBrightness.Name = "txtLowSkyBrightness";
            this.txtLowSkyBrightness.Size = new System.Drawing.Size(100, 20);
            this.txtLowSkyBrightness.TabIndex = 11;
            // 
            // txtLowRainRate
            // 
            this.txtLowRainRate.Location = new System.Drawing.Point(260, 294);
            this.txtLowRainRate.Name = "txtLowRainRate";
            this.txtLowRainRate.Size = new System.Drawing.Size(100, 20);
            this.txtLowRainRate.TabIndex = 9;
            // 
            // txtLowPressure
            // 
            this.txtLowPressure.Location = new System.Drawing.Point(260, 267);
            this.txtLowPressure.Name = "txtLowPressure";
            this.txtLowPressure.Size = new System.Drawing.Size(100, 20);
            this.txtLowPressure.TabIndex = 7;
            // 
            // txtLowHumidity
            // 
            this.txtLowHumidity.Location = new System.Drawing.Point(260, 241);
            this.txtLowHumidity.Name = "txtLowHumidity";
            this.txtLowHumidity.Size = new System.Drawing.Size(100, 20);
            this.txtLowHumidity.TabIndex = 5;
            // 
            // txtLowDewPoint
            // 
            this.txtLowDewPoint.Location = new System.Drawing.Point(260, 214);
            this.txtLowDewPoint.Name = "txtLowDewPoint";
            this.txtLowDewPoint.Size = new System.Drawing.Size(100, 20);
            this.txtLowDewPoint.TabIndex = 3;
            // 
            // txtLowCloudCover
            // 
            this.txtLowCloudCover.Location = new System.Drawing.Point(260, 187);
            this.txtLowCloudCover.Name = "txtLowCloudCover";
            this.txtLowCloudCover.Size = new System.Drawing.Size(100, 20);
            this.txtLowCloudCover.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label16.Location = new System.Drawing.Point(487, 139);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(114, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Simulator To Value";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label15.Location = new System.Drawing.Point(251, 139);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(126, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Simulator From Value";
            // 
            // txtLowAveragePeriod
            // 
            this.txtLowAveragePeriod.Location = new System.Drawing.Point(260, 160);
            this.txtLowAveragePeriod.Name = "txtLowAveragePeriod";
            this.txtLowAveragePeriod.Size = new System.Drawing.Size(100, 20);
            this.txtLowAveragePeriod.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label1.Location = new System.Drawing.Point(150, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Average Period";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(168, 190);
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
            this.label3.Location = new System.Drawing.Point(179, 217);
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
            this.label4.Location = new System.Drawing.Point(188, 270);
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
            this.label5.Location = new System.Drawing.Point(189, 244);
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
            this.label6.Location = new System.Drawing.Point(180, 297);
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
            this.label7.Location = new System.Drawing.Point(153, 324);
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
            this.label8.Location = new System.Drawing.Point(173, 351);
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
            this.label9.Location = new System.Drawing.Point(173, 378);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Sky Seeing";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label10.Location = new System.Drawing.Point(141, 405);
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
            this.label11.Location = new System.Drawing.Point(166, 431);
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
            this.label12.Location = new System.Drawing.Point(153, 458);
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
            this.label13.Location = new System.Drawing.Point(178, 485);
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
            this.label14.Location = new System.Drawing.Point(168, 512);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Wind Speed";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sensorViewPressure
            // 
            this.sensorViewPressure.ConnectToDriver = false;
            this.sensorViewPressure.Location = new System.Drawing.Point(193, 646);
            this.sensorViewPressure.Name = "sensorViewPressure";
            this.sensorViewPressure.SensorName = null;
            this.sensorViewPressure.Size = new System.Drawing.Size(814, 24);
            this.sensorViewPressure.TabIndex = 46;
            this.sensorViewPressure.Units = "label1";
            // 
            // sensorViewCloudCover
            // 
            this.sensorViewCloudCover.ConnectToDriver = false;
            this.sensorViewCloudCover.Location = new System.Drawing.Point(192, 556);
            this.sensorViewCloudCover.Name = "sensorViewCloudCover";
            this.sensorViewCloudCover.SensorName = null;
            this.sensorViewCloudCover.Size = new System.Drawing.Size(814, 24);
            this.sensorViewCloudCover.TabIndex = 48;
            this.sensorViewCloudCover.Units = "label1";
            // 
            // sensorViewDewPoint
            // 
            this.sensorViewDewPoint.ConnectToDriver = false;
            this.sensorViewDewPoint.Location = new System.Drawing.Point(193, 586);
            this.sensorViewDewPoint.Name = "sensorViewDewPoint";
            this.sensorViewDewPoint.SensorName = null;
            this.sensorViewDewPoint.Size = new System.Drawing.Size(814, 24);
            this.sensorViewDewPoint.TabIndex = 49;
            this.sensorViewDewPoint.Units = "label1";
            // 
            // sensorViewHumidity
            // 
            this.sensorViewHumidity.ConnectToDriver = false;
            this.sensorViewHumidity.Location = new System.Drawing.Point(192, 616);
            this.sensorViewHumidity.Name = "sensorViewHumidity";
            this.sensorViewHumidity.SensorName = null;
            this.sensorViewHumidity.Size = new System.Drawing.Size(814, 24);
            this.sensorViewHumidity.TabIndex = 50;
            this.sensorViewHumidity.Units = "label1";
            // 
            // sensorViewTemperature
            // 
            this.sensorViewTemperature.ConnectToDriver = false;
            this.sensorViewTemperature.Location = new System.Drawing.Point(193, 826);
            this.sensorViewTemperature.Name = "sensorViewTemperature";
            this.sensorViewTemperature.SensorName = null;
            this.sensorViewTemperature.Size = new System.Drawing.Size(814, 24);
            this.sensorViewTemperature.TabIndex = 51;
            this.sensorViewTemperature.Units = "label1";
            // 
            // sensorViewWindSpeed
            // 
            this.sensorViewWindSpeed.ConnectToDriver = false;
            this.sensorViewWindSpeed.Location = new System.Drawing.Point(194, 916);
            this.sensorViewWindSpeed.Name = "sensorViewWindSpeed";
            this.sensorViewWindSpeed.SensorName = null;
            this.sensorViewWindSpeed.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindSpeed.TabIndex = 52;
            this.sensorViewWindSpeed.Units = "label1";
            // 
            // sensorViewWindGust
            // 
            this.sensorViewWindGust.ConnectToDriver = false;
            this.sensorViewWindGust.Location = new System.Drawing.Point(193, 886);
            this.sensorViewWindGust.Name = "sensorViewWindGust";
            this.sensorViewWindGust.SensorName = null;
            this.sensorViewWindGust.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindGust.TabIndex = 53;
            this.sensorViewWindGust.Units = "label1";
            // 
            // sensorViewWindDirection
            // 
            this.sensorViewWindDirection.ConnectToDriver = false;
            this.sensorViewWindDirection.Location = new System.Drawing.Point(193, 856);
            this.sensorViewWindDirection.Name = "sensorViewWindDirection";
            this.sensorViewWindDirection.SensorName = null;
            this.sensorViewWindDirection.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindDirection.TabIndex = 54;
            this.sensorViewWindDirection.Units = "label1";
            // 
            // sensorViewSkyTemperature
            // 
            this.sensorViewSkyTemperature.ConnectToDriver = false;
            this.sensorViewSkyTemperature.Location = new System.Drawing.Point(192, 796);
            this.sensorViewSkyTemperature.Name = "sensorViewSkyTemperature";
            this.sensorViewSkyTemperature.SensorName = null;
            this.sensorViewSkyTemperature.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyTemperature.TabIndex = 55;
            this.sensorViewSkyTemperature.Units = "label1";
            // 
            // sensorViewSkySeeing
            // 
            this.sensorViewSkySeeing.ConnectToDriver = false;
            this.sensorViewSkySeeing.Location = new System.Drawing.Point(192, 766);
            this.sensorViewSkySeeing.Name = "sensorViewSkySeeing";
            this.sensorViewSkySeeing.SensorName = null;
            this.sensorViewSkySeeing.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkySeeing.TabIndex = 56;
            this.sensorViewSkySeeing.Units = "label1";
            // 
            // sensorViewSkyQuality
            // 
            this.sensorViewSkyQuality.ConnectToDriver = false;
            this.sensorViewSkyQuality.Location = new System.Drawing.Point(192, 736);
            this.sensorViewSkyQuality.Name = "sensorViewSkyQuality";
            this.sensorViewSkyQuality.SensorName = null;
            this.sensorViewSkyQuality.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyQuality.TabIndex = 57;
            this.sensorViewSkyQuality.Units = "label1";
            // 
            // sensorViewSkyBrightness
            // 
            this.sensorViewSkyBrightness.ConnectToDriver = false;
            this.sensorViewSkyBrightness.Location = new System.Drawing.Point(192, 706);
            this.sensorViewSkyBrightness.Name = "sensorViewSkyBrightness";
            this.sensorViewSkyBrightness.SensorName = null;
            this.sensorViewSkyBrightness.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyBrightness.TabIndex = 58;
            this.sensorViewSkyBrightness.Units = "label1";
            // 
            // sensorViewRainRate
            // 
            this.sensorViewRainRate.ConnectToDriver = false;
            this.sensorViewRainRate.Location = new System.Drawing.Point(193, 676);
            this.sensorViewRainRate.Name = "sensorViewRainRate";
            this.sensorViewRainRate.SensorName = null;
            this.sensorViewRainRate.Size = new System.Drawing.Size(814, 24);
            this.sensorViewRainRate.TabIndex = 59;
            this.sensorViewRainRate.Units = "label1";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 1037);
            this.Controls.Add(this.sensorViewRainRate);
            this.Controls.Add(this.sensorViewSkyBrightness);
            this.Controls.Add(this.sensorViewSkyQuality);
            this.Controls.Add(this.sensorViewSkySeeing);
            this.Controls.Add(this.sensorViewSkyTemperature);
            this.Controls.Add(this.sensorViewWindDirection);
            this.Controls.Add(this.sensorViewWindGust);
            this.Controls.Add(this.sensorViewWindSpeed);
            this.Controls.Add(this.sensorViewTemperature);
            this.Controls.Add(this.sensorViewHumidity);
            this.Controls.Add(this.sensorViewDewPoint);
            this.Controls.Add(this.sensorViewCloudCover);
            this.Controls.Add(this.sensorViewPressure);
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
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHighAveragePeriod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtHighWindSpeed);
            this.Controls.Add(this.debugTrace);
            this.Controls.Add(this.txtHighWindGust);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.txtHighWindDirection);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.txtHighTemperature);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.txtHighSkyTemperature);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtHighSkySeeing);
            this.Controls.Add(this.txtLowAveragePeriod);
            this.Controls.Add(this.txtHighSkyQuality);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtHighSkyBrightness);
            this.Controls.Add(this.txtLowCloudCover);
            this.Controls.Add(this.txtHighRainRate);
            this.Controls.Add(this.txtLowDewPoint);
            this.Controls.Add(this.txtHighPressure);
            this.Controls.Add(this.txtLowHumidity);
            this.Controls.Add(this.txtHighHumidity);
            this.Controls.Add(this.txtLowPressure);
            this.Controls.Add(this.txtHighDewPoint);
            this.Controls.Add(this.txtLowRainRate);
            this.Controls.Add(this.txtHighCloudCover);
            this.Controls.Add(this.txtLowSkyBrightness);
            this.Controls.Add(this.txtLowWindSpeed);
            this.Controls.Add(this.txtLowSkyQuality);
            this.Controls.Add(this.txtLowWindGust);
            this.Controls.Add(this.txtLowSkySeeing);
            this.Controls.Add(this.txtLowWindDirection);
            this.Controls.Add(this.txtLowSkyTemperature);
            this.Controls.Add(this.txtLowTemperature);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.CheckBox debugTrace;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.TextBox txtLowAveragePeriod;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtHighWindSpeed;
        private System.Windows.Forms.TextBox txtHighWindGust;
        private System.Windows.Forms.TextBox txtHighWindDirection;
        private System.Windows.Forms.TextBox txtHighTemperature;
        private System.Windows.Forms.TextBox txtHighSkyTemperature;
        private System.Windows.Forms.TextBox txtHighSkySeeing;
        private System.Windows.Forms.TextBox txtHighSkyQuality;
        private System.Windows.Forms.TextBox txtHighSkyBrightness;
        private System.Windows.Forms.TextBox txtHighRainRate;
        private System.Windows.Forms.TextBox txtHighPressure;
        private System.Windows.Forms.TextBox txtHighHumidity;
        private System.Windows.Forms.TextBox txtHighDewPoint;
        private System.Windows.Forms.TextBox txtHighCloudCover;
        private System.Windows.Forms.TextBox txtLowWindSpeed;
        private System.Windows.Forms.TextBox txtLowWindGust;
        private System.Windows.Forms.TextBox txtLowWindDirection;
        private System.Windows.Forms.TextBox txtLowTemperature;
        private System.Windows.Forms.TextBox txtLowSkyTemperature;
        private System.Windows.Forms.TextBox txtLowSkySeeing;
        private System.Windows.Forms.TextBox txtLowSkyQuality;
        private System.Windows.Forms.TextBox txtLowSkyBrightness;
        private System.Windows.Forms.TextBox txtLowRainRate;
        private System.Windows.Forms.TextBox txtLowPressure;
        private System.Windows.Forms.TextBox txtLowHumidity;
        private System.Windows.Forms.TextBox txtLowDewPoint;
        private System.Windows.Forms.TextBox txtLowCloudCover;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtHighAveragePeriod;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private SensorView sensorViewPressure;
        private SensorView sensorViewCloudCover;
        private SensorView sensorViewDewPoint;
        private SensorView sensorViewHumidity;
        private SensorView sensorViewTemperature;
        private SensorView sensorViewWindSpeed;
        private SensorView sensorViewWindGust;
        private SensorView sensorViewWindDirection;
        private SensorView sensorViewSkyTemperature;
        private SensorView sensorViewSkySeeing;
        private SensorView sensorViewSkyQuality;
        private SensorView sensorViewSkyBrightness;
        private SensorView sensorViewRainRate;
    }
}