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
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
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
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
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
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.sensorViewRainRate = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyBrightness = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyQuality = new ASCOM.Simulator.SensorView();
            this.sensorViewSkySeeing = new ASCOM.Simulator.SensorView();
            this.sensorViewSkyTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewWindDirection = new ASCOM.Simulator.SensorView();
            this.sensorViewWindGust = new ASCOM.Simulator.SensorView();
            this.sensorViewWindSpeed = new ASCOM.Simulator.SensorView();
            this.sensorViewTemperature = new ASCOM.Simulator.SensorView();
            this.sensorViewHumidity = new ASCOM.Simulator.SensorView();
            this.sensorViewDewPoint = new ASCOM.Simulator.SensorView();
            this.sensorViewCloudCover = new ASCOM.Simulator.SensorView();
            this.sensorViewPressure = new ASCOM.Simulator.SensorView();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(994, 531);
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
            this.cmdCancel.Location = new System.Drawing.Point(994, 561);
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
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(166, 540);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(270, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "* No high value, average period does not vary over time";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label16.Location = new System.Drawing.Point(570, 139);
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
            this.label15.Location = new System.Drawing.Point(336, 139);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(126, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Simulator From Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label1.Location = new System.Drawing.Point(153, 76);
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
            this.label2.Location = new System.Drawing.Point(142, 165);
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
            this.label3.Location = new System.Drawing.Point(153, 192);
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
            this.label4.Location = new System.Drawing.Point(162, 245);
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
            this.label5.Location = new System.Drawing.Point(163, 219);
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
            this.label6.Location = new System.Drawing.Point(154, 272);
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
            this.label7.Location = new System.Drawing.Point(127, 299);
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
            this.label8.Location = new System.Drawing.Point(147, 326);
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
            this.label9.Location = new System.Drawing.Point(147, 353);
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
            this.label10.Location = new System.Drawing.Point(115, 380);
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
            this.label11.Location = new System.Drawing.Point(140, 406);
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
            this.label12.Location = new System.Drawing.Point(127, 433);
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
            this.label13.Location = new System.Drawing.Point(152, 460);
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
            this.label14.Location = new System.Drawing.Point(142, 487);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Wind Speed";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(297, 76);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 32;
            this.label18.Text = "Seconds *";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(457, 165);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Percent";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(457, 192);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 13);
            this.label20.TabIndex = 34;
            this.label20.Text = "Degrees Celsius";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(457, 219);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(44, 13);
            this.label21.TabIndex = 35;
            this.label21.Text = "Percent";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(457, 245);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(26, 13);
            this.label22.TabIndex = 36;
            this.label22.Text = "hPa";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(457, 272);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 37;
            this.label23.Text = "mm / hr";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(457, 299);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 13);
            this.label24.TabIndex = 38;
            this.label24.Text = "Lux";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(457, 326);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(108, 13);
            this.label25.TabIndex = 39;
            this.label25.Text = "Mag / Square arcsec";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(457, 353);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 13);
            this.label26.TabIndex = 40;
            this.label26.Text = "Arcsec FHWM";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(457, 380);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 13);
            this.label27.TabIndex = 41;
            this.label27.Text = "Degrees Celsius";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(457, 406);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 13);
            this.label28.TabIndex = 42;
            this.label28.Text = "Degrees Celsius";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(457, 433);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(47, 13);
            this.label29.TabIndex = 43;
            this.label29.Text = "Degrees";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(457, 460);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(73, 13);
            this.label30.TabIndex = 44;
            this.label30.Text = "Miles per hour";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(457, 487);
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
            this.label32.Location = new System.Drawing.Point(735, 139);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(119, 13);
            this.label32.TabIndex = 60;
            this.label32.Text = "Simulate Not Ready";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label33.Location = new System.Drawing.Point(223, 139);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(96, 13);
            this.label33.TabIndex = 61;
            this.label33.Text = "Sensor Enabled";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label34.Location = new System.Drawing.Point(882, 139);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(93, 13);
            this.label34.TabIndex = 62;
            this.label34.Text = "Not ready Time";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(224, 219);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(100, 13);
            this.label35.TabIndex = 63;
            this.label35.Text = "Matches Dew Point";
            // 
            // sensorViewRainRate
            // 
            this.sensorViewRainRate.ConnectToDriver = false;
            this.sensorViewRainRate.EnabledCheckboxVisible = true;
            this.sensorViewRainRate.Location = new System.Drawing.Point(250, 266);
            this.sensorViewRainRate.Name = "sensorViewRainRate";
            this.sensorViewRainRate.NotReadyControlsEnabled = true;
            this.sensorViewRainRate.NotReadyDelay = 0D;
            this.sensorViewRainRate.SensorEnabled = false;
            this.sensorViewRainRate.SensorName = null;
            this.sensorViewRainRate.ShowNotReady = false;
            this.sensorViewRainRate.Size = new System.Drawing.Size(814, 24);
            this.sensorViewRainRate.TabIndex = 59;
            // 
            // sensorViewSkyBrightness
            // 
            this.sensorViewSkyBrightness.ConnectToDriver = false;
            this.sensorViewSkyBrightness.EnabledCheckboxVisible = true;
            this.sensorViewSkyBrightness.Location = new System.Drawing.Point(250, 294);
            this.sensorViewSkyBrightness.Name = "sensorViewSkyBrightness";
            this.sensorViewSkyBrightness.NotReadyControlsEnabled = true;
            this.sensorViewSkyBrightness.NotReadyDelay = 0D;
            this.sensorViewSkyBrightness.SensorEnabled = false;
            this.sensorViewSkyBrightness.SensorName = null;
            this.sensorViewSkyBrightness.ShowNotReady = false;
            this.sensorViewSkyBrightness.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyBrightness.TabIndex = 58;
            // 
            // sensorViewSkyQuality
            // 
            this.sensorViewSkyQuality.ConnectToDriver = false;
            this.sensorViewSkyQuality.EnabledCheckboxVisible = true;
            this.sensorViewSkyQuality.Location = new System.Drawing.Point(250, 321);
            this.sensorViewSkyQuality.Name = "sensorViewSkyQuality";
            this.sensorViewSkyQuality.NotReadyControlsEnabled = true;
            this.sensorViewSkyQuality.NotReadyDelay = 0D;
            this.sensorViewSkyQuality.SensorEnabled = false;
            this.sensorViewSkyQuality.SensorName = null;
            this.sensorViewSkyQuality.ShowNotReady = false;
            this.sensorViewSkyQuality.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyQuality.TabIndex = 57;
            // 
            // sensorViewSkySeeing
            // 
            this.sensorViewSkySeeing.ConnectToDriver = false;
            this.sensorViewSkySeeing.EnabledCheckboxVisible = true;
            this.sensorViewSkySeeing.Location = new System.Drawing.Point(250, 347);
            this.sensorViewSkySeeing.Name = "sensorViewSkySeeing";
            this.sensorViewSkySeeing.NotReadyControlsEnabled = true;
            this.sensorViewSkySeeing.NotReadyDelay = 0D;
            this.sensorViewSkySeeing.SensorEnabled = false;
            this.sensorViewSkySeeing.SensorName = null;
            this.sensorViewSkySeeing.ShowNotReady = false;
            this.sensorViewSkySeeing.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkySeeing.TabIndex = 56;
            // 
            // sensorViewSkyTemperature
            // 
            this.sensorViewSkyTemperature.ConnectToDriver = false;
            this.sensorViewSkyTemperature.EnabledCheckboxVisible = true;
            this.sensorViewSkyTemperature.Location = new System.Drawing.Point(250, 375);
            this.sensorViewSkyTemperature.Name = "sensorViewSkyTemperature";
            this.sensorViewSkyTemperature.NotReadyControlsEnabled = true;
            this.sensorViewSkyTemperature.NotReadyDelay = 0D;
            this.sensorViewSkyTemperature.SensorEnabled = false;
            this.sensorViewSkyTemperature.SensorName = null;
            this.sensorViewSkyTemperature.ShowNotReady = false;
            this.sensorViewSkyTemperature.Size = new System.Drawing.Size(814, 24);
            this.sensorViewSkyTemperature.TabIndex = 55;
            // 
            // sensorViewWindDirection
            // 
            this.sensorViewWindDirection.ConnectToDriver = false;
            this.sensorViewWindDirection.EnabledCheckboxVisible = true;
            this.sensorViewWindDirection.Location = new System.Drawing.Point(250, 428);
            this.sensorViewWindDirection.Name = "sensorViewWindDirection";
            this.sensorViewWindDirection.NotReadyControlsEnabled = true;
            this.sensorViewWindDirection.NotReadyDelay = 0D;
            this.sensorViewWindDirection.SensorEnabled = false;
            this.sensorViewWindDirection.SensorName = null;
            this.sensorViewWindDirection.ShowNotReady = false;
            this.sensorViewWindDirection.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindDirection.TabIndex = 54;
            // 
            // sensorViewWindGust
            // 
            this.sensorViewWindGust.ConnectToDriver = false;
            this.sensorViewWindGust.EnabledCheckboxVisible = true;
            this.sensorViewWindGust.Location = new System.Drawing.Point(250, 454);
            this.sensorViewWindGust.Name = "sensorViewWindGust";
            this.sensorViewWindGust.NotReadyControlsEnabled = true;
            this.sensorViewWindGust.NotReadyDelay = 0D;
            this.sensorViewWindGust.SensorEnabled = false;
            this.sensorViewWindGust.SensorName = null;
            this.sensorViewWindGust.ShowNotReady = false;
            this.sensorViewWindGust.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindGust.TabIndex = 53;
            // 
            // sensorViewWindSpeed
            // 
            this.sensorViewWindSpeed.ConnectToDriver = false;
            this.sensorViewWindSpeed.EnabledCheckboxVisible = true;
            this.sensorViewWindSpeed.Location = new System.Drawing.Point(250, 482);
            this.sensorViewWindSpeed.Name = "sensorViewWindSpeed";
            this.sensorViewWindSpeed.NotReadyControlsEnabled = true;
            this.sensorViewWindSpeed.NotReadyDelay = 0D;
            this.sensorViewWindSpeed.SensorEnabled = false;
            this.sensorViewWindSpeed.SensorName = null;
            this.sensorViewWindSpeed.ShowNotReady = false;
            this.sensorViewWindSpeed.Size = new System.Drawing.Size(814, 24);
            this.sensorViewWindSpeed.TabIndex = 52;
            // 
            // sensorViewTemperature
            // 
            this.sensorViewTemperature.ConnectToDriver = false;
            this.sensorViewTemperature.EnabledCheckboxVisible = true;
            this.sensorViewTemperature.Location = new System.Drawing.Point(250, 401);
            this.sensorViewTemperature.Name = "sensorViewTemperature";
            this.sensorViewTemperature.NotReadyControlsEnabled = true;
            this.sensorViewTemperature.NotReadyDelay = 0D;
            this.sensorViewTemperature.SensorEnabled = false;
            this.sensorViewTemperature.SensorName = null;
            this.sensorViewTemperature.ShowNotReady = false;
            this.sensorViewTemperature.Size = new System.Drawing.Size(814, 24);
            this.sensorViewTemperature.TabIndex = 51;
            // 
            // sensorViewHumidity
            // 
            this.sensorViewHumidity.ConnectToDriver = false;
            this.sensorViewHumidity.EnabledCheckboxVisible = true;
            this.sensorViewHumidity.Location = new System.Drawing.Point(250, 214);
            this.sensorViewHumidity.Name = "sensorViewHumidity";
            this.sensorViewHumidity.NotReadyControlsEnabled = true;
            this.sensorViewHumidity.NotReadyDelay = 0D;
            this.sensorViewHumidity.SensorEnabled = false;
            this.sensorViewHumidity.SensorName = null;
            this.sensorViewHumidity.ShowNotReady = false;
            this.sensorViewHumidity.Size = new System.Drawing.Size(814, 24);
            this.sensorViewHumidity.TabIndex = 50;
            // 
            // sensorViewDewPoint
            // 
            this.sensorViewDewPoint.ConnectToDriver = false;
            this.sensorViewDewPoint.EnabledCheckboxVisible = true;
            this.sensorViewDewPoint.Location = new System.Drawing.Point(250, 187);
            this.sensorViewDewPoint.Name = "sensorViewDewPoint";
            this.sensorViewDewPoint.NotReadyControlsEnabled = true;
            this.sensorViewDewPoint.NotReadyDelay = 0D;
            this.sensorViewDewPoint.SensorEnabled = false;
            this.sensorViewDewPoint.SensorName = null;
            this.sensorViewDewPoint.ShowNotReady = false;
            this.sensorViewDewPoint.Size = new System.Drawing.Size(814, 24);
            this.sensorViewDewPoint.TabIndex = 49;
            // 
            // sensorViewCloudCover
            // 
            this.sensorViewCloudCover.ConnectToDriver = false;
            this.sensorViewCloudCover.EnabledCheckboxVisible = true;
            this.sensorViewCloudCover.Location = new System.Drawing.Point(250, 160);
            this.sensorViewCloudCover.Name = "sensorViewCloudCover";
            this.sensorViewCloudCover.NotReadyControlsEnabled = true;
            this.sensorViewCloudCover.NotReadyDelay = 0D;
            this.sensorViewCloudCover.SensorEnabled = false;
            this.sensorViewCloudCover.SensorName = null;
            this.sensorViewCloudCover.ShowNotReady = false;
            this.sensorViewCloudCover.Size = new System.Drawing.Size(814, 24);
            this.sensorViewCloudCover.TabIndex = 48;
            // 
            // sensorViewPressure
            // 
            this.sensorViewPressure.ConnectToDriver = false;
            this.sensorViewPressure.EnabledCheckboxVisible = true;
            this.sensorViewPressure.Location = new System.Drawing.Point(250, 240);
            this.sensorViewPressure.Name = "sensorViewPressure";
            this.sensorViewPressure.NotReadyControlsEnabled = true;
            this.sensorViewPressure.NotReadyDelay = 0D;
            this.sensorViewPressure.SensorEnabled = false;
            this.sensorViewPressure.SensorName = null;
            this.sensorViewPressure.ShowNotReady = false;
            this.sensorViewPressure.Size = new System.Drawing.Size(814, 24);
            this.sensorViewPressure.TabIndex = 46;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 600);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label34);
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
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.debugTrace);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
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
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
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
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
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
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
    }
}