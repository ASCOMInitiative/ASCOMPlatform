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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.groupBoxCCD = new System.Windows.Forms.GroupBox();
            this.checkBoxOmitOddBins = new System.Windows.Forms.CheckBox();
            this.textBoxCameraYSize = new System.Windows.Forms.TextBox();
            this.textBoxCameraXSize = new System.Windows.Forms.TextBox();
            this.textBoxMaxBinY = new System.Windows.Forms.TextBox();
            this.textBoxMaxBinX = new System.Windows.Forms.TextBox();
            this.textBoxSensorName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxBayerOffsetY = new System.Windows.Forms.TextBox();
            this.labelBayerOffsetY = new System.Windows.Forms.Label();
            this.textBoxBayerOffsetX = new System.Windows.Forms.TextBox();
            this.labelBayerOffsetX = new System.Windows.Forms.Label();
            this.comboBoxSensorType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxHasShutter = new System.Windows.Forms.CheckBox();
            this.checkBoxCanAsymmetricBin = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxGainSettings = new System.Windows.Forms.GroupBox();
            this.radioButtonNoGain = new System.Windows.Forms.RadioButton();
            this.textBoxGainMax = new System.Windows.Forms.TextBox();
            this.textBoxGainMin = new System.Windows.Forms.TextBox();
            this.radioButtonUseMinAndMax = new System.Windows.Forms.RadioButton();
            this.radioButtonUseGains = new System.Windows.Forms.RadioButton();
            this.groupBoxCooling = new System.Windows.Forms.GroupBox();
            this.BtnCoolerConfiguration = new System.Windows.Forms.Button();
            this.checkBoxHasCooler = new System.Windows.Forms.CheckBox();
            this.checkBoxCanGetCoolerPower = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSetCCDTemperature = new System.Windows.Forms.CheckBox();
            this.groupBoxExposure = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtSubExposure = new System.Windows.Forms.TextBox();
            this.ChkHasSubExposure = new System.Windows.Forms.CheckBox();
            this.textBoxMaxExposure = new System.Windows.Forms.TextBox();
            this.textBoxMinExposure = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxCanStopExposure = new System.Windows.Forms.CheckBox();
            this.checkBoxCanAbortExposure = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxPixelSizeY = new System.Windows.Forms.TextBox();
            this.textBoxPixelSizeX = new System.Windows.Forms.TextBox();
            this.textBoxElectronsPerADU = new System.Windows.Forms.TextBox();
            this.textBoxMaxADU = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSizeY = new System.Windows.Forms.Label();
            this.labelSizeX = new System.Windows.Forms.Label();
            this.buttonSetImageFile = new System.Windows.Forms.Button();
            this.checkBoxApplyNoise = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxGuiding = new System.Windows.Forms.GroupBox();
            this.checkBoxCanPulseGuide = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxUseReadoutModes = new System.Windows.Forms.CheckBox();
            this.checkBoxCanFastReadout = new System.Windows.Forms.CheckBox();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.groupBoxReadoutModes = new System.Windows.Forms.GroupBox();
            this.GrpOffset = new System.Windows.Forms.GroupBox();
            this.RadNoOffset = new System.Windows.Forms.RadioButton();
            this.TxtOffsetMax = new System.Windows.Forms.TextBox();
            this.TxtOffsetMin = new System.Windows.Forms.TextBox();
            this.RadOffsetMinMax = new System.Windows.Forms.RadioButton();
            this.RadOffsets = new System.Windows.Forms.RadioButton();
            this.GrpSimulatorSetup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NumInterfaceVersion = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBoxCCD.SuspendLayout();
            this.groupBoxGainSettings.SuspendLayout();
            this.groupBoxCooling.SuspendLayout();
            this.groupBoxExposure.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxGuiding.SuspendLayout();
            this.groupBoxReadoutModes.SuspendLayout();
            this.GrpOffset.SuspendLayout();
            this.GrpSimulatorSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumInterfaceVersion)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(675, 460);
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
            this.cmdCancel.Location = new System.Drawing.Point(675, 490);
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
            this.picASCOM.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(686, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // groupBoxCCD
            // 
            this.groupBoxCCD.Controls.Add(this.checkBoxOmitOddBins);
            this.groupBoxCCD.Controls.Add(this.textBoxCameraYSize);
            this.groupBoxCCD.Controls.Add(this.textBoxCameraXSize);
            this.groupBoxCCD.Controls.Add(this.textBoxMaxBinY);
            this.groupBoxCCD.Controls.Add(this.textBoxMaxBinX);
            this.groupBoxCCD.Controls.Add(this.textBoxSensorName);
            this.groupBoxCCD.Controls.Add(this.label13);
            this.groupBoxCCD.Controls.Add(this.textBoxBayerOffsetY);
            this.groupBoxCCD.Controls.Add(this.labelBayerOffsetY);
            this.groupBoxCCD.Controls.Add(this.textBoxBayerOffsetX);
            this.groupBoxCCD.Controls.Add(this.labelBayerOffsetX);
            this.groupBoxCCD.Controls.Add(this.comboBoxSensorType);
            this.groupBoxCCD.Controls.Add(this.label12);
            this.groupBoxCCD.Controls.Add(this.checkBoxHasShutter);
            this.groupBoxCCD.Controls.Add(this.checkBoxCanAsymmetricBin);
            this.groupBoxCCD.Controls.Add(this.label6);
            this.groupBoxCCD.Controls.Add(this.label4);
            this.groupBoxCCD.Controls.Add(this.label3);
            this.groupBoxCCD.Controls.Add(this.label2);
            this.groupBoxCCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCCD.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxCCD.Location = new System.Drawing.Point(12, 148);
            this.groupBoxCCD.Name = "groupBoxCCD";
            this.groupBoxCCD.Size = new System.Drawing.Size(200, 367);
            this.groupBoxCCD.TabIndex = 4;
            this.groupBoxCCD.TabStop = false;
            this.groupBoxCCD.Text = "CCD";
            // 
            // checkBoxOmitOddBins
            // 
            this.checkBoxOmitOddBins.AutoSize = true;
            this.checkBoxOmitOddBins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOmitOddBins.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxOmitOddBins.Location = new System.Drawing.Point(45, 134);
            this.checkBoxOmitOddBins.Name = "checkBoxOmitOddBins";
            this.checkBoxOmitOddBins.Size = new System.Drawing.Size(93, 17);
            this.checkBoxOmitOddBins.TabIndex = 29;
            this.checkBoxOmitOddBins.Text = "Omit Odd Bins";
            this.toolTip1.SetToolTip(this.checkBoxOmitOddBins, "Throw NotImplementedExceptions for odd bin values of 3 or greater. This has no ef" +
        "fect unless MaxBinX and MaxBinY are 4 or greater.");
            this.checkBoxOmitOddBins.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraYSize
            // 
            this.textBoxCameraYSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCameraYSize.Location = new System.Drawing.Point(117, 45);
            this.textBoxCameraYSize.Name = "textBoxCameraYSize";
            this.textBoxCameraYSize.Size = new System.Drawing.Size(47, 20);
            this.textBoxCameraYSize.TabIndex = 28;
            this.toolTip1.SetToolTip(this.textBoxCameraYSize, "The number of unbinned pixels down the the height of the CCD");
            // 
            // textBoxCameraXSize
            // 
            this.textBoxCameraXSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCameraXSize.Location = new System.Drawing.Point(117, 19);
            this.textBoxCameraXSize.Name = "textBoxCameraXSize";
            this.textBoxCameraXSize.Size = new System.Drawing.Size(47, 20);
            this.textBoxCameraXSize.TabIndex = 27;
            this.toolTip1.SetToolTip(this.textBoxCameraXSize, "The number of unbinned pixels across the width of the CCD");
            // 
            // textBoxMaxBinY
            // 
            this.textBoxMaxBinY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMaxBinY.Location = new System.Drawing.Point(148, 71);
            this.textBoxMaxBinY.Name = "textBoxMaxBinY";
            this.textBoxMaxBinY.Size = new System.Drawing.Size(16, 20);
            this.textBoxMaxBinY.TabIndex = 26;
            this.toolTip1.SetToolTip(this.textBoxMaxBinY, "The maximum bin value in Y");
            // 
            // textBoxMaxBinX
            // 
            this.textBoxMaxBinX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMaxBinX.Location = new System.Drawing.Point(104, 71);
            this.textBoxMaxBinX.Name = "textBoxMaxBinX";
            this.textBoxMaxBinX.Size = new System.Drawing.Size(16, 20);
            this.textBoxMaxBinX.TabIndex = 25;
            this.toolTip1.SetToolTip(this.textBoxMaxBinX, "The maximum X bin value");
            // 
            // textBoxSensorName
            // 
            this.textBoxSensorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSensorName.Location = new System.Drawing.Point(74, 196);
            this.textBoxSensorName.Name = "textBoxSensorName";
            this.textBoxSensorName.Size = new System.Drawing.Size(90, 20);
            this.textBoxSensorName.TabIndex = 24;
            this.toolTip1.SetToolTip(this.textBoxSensorName, "Set the Sensor Name.");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label13.Location = new System.Drawing.Point(33, 199);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Name";
            // 
            // textBoxBayerOffsetY
            // 
            this.textBoxBayerOffsetY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBayerOffsetY.Location = new System.Drawing.Point(153, 266);
            this.textBoxBayerOffsetY.Name = "textBoxBayerOffsetY";
            this.textBoxBayerOffsetY.Size = new System.Drawing.Size(16, 20);
            this.textBoxBayerOffsetY.TabIndex = 22;
            this.toolTip1.SetToolTip(this.textBoxBayerOffsetY, "Bayer Offset in Y");
            // 
            // labelBayerOffsetY
            // 
            this.labelBayerOffsetY.AutoSize = true;
            this.labelBayerOffsetY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBayerOffsetY.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelBayerOffsetY.Location = new System.Drawing.Point(137, 269);
            this.labelBayerOffsetY.Name = "labelBayerOffsetY";
            this.labelBayerOffsetY.Size = new System.Drawing.Size(14, 13);
            this.labelBayerOffsetY.TabIndex = 21;
            this.labelBayerOffsetY.Text = "Y";
            // 
            // textBoxBayerOffsetX
            // 
            this.textBoxBayerOffsetX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBayerOffsetX.Location = new System.Drawing.Point(104, 266);
            this.textBoxBayerOffsetX.Name = "textBoxBayerOffsetX";
            this.textBoxBayerOffsetX.Size = new System.Drawing.Size(16, 20);
            this.textBoxBayerOffsetX.TabIndex = 20;
            this.toolTip1.SetToolTip(this.textBoxBayerOffsetX, "Bayer offset in X");
            // 
            // labelBayerOffsetX
            // 
            this.labelBayerOffsetX.AutoSize = true;
            this.labelBayerOffsetX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBayerOffsetX.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelBayerOffsetX.Location = new System.Drawing.Point(21, 269);
            this.labelBayerOffsetX.Name = "labelBayerOffsetX";
            this.labelBayerOffsetX.Size = new System.Drawing.Size(81, 13);
            this.labelBayerOffsetX.TabIndex = 19;
            this.labelBayerOffsetX.Text = "Bayer Offset   X";
            this.toolTip1.SetToolTip(this.labelBayerOffsetX, "Set the offset in X and Y pixels to the first pixel in the Bayer array. For camer" +
        "as with a colour filter array only.");
            // 
            // comboBoxSensorType
            // 
            this.comboBoxSensorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSensorType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSensorType.FormattingEnabled = true;
            this.comboBoxSensorType.Items.AddRange(new object[] {
            "Monochrome",
            "Color",
            "RGGB",
            "CMYG",
            "CMYG2",
            "LRGB"});
            this.comboBoxSensorType.Location = new System.Drawing.Point(74, 225);
            this.comboBoxSensorType.Name = "comboBoxSensorType";
            this.comboBoxSensorType.Size = new System.Drawing.Size(90, 21);
            this.comboBoxSensorType.TabIndex = 16;
            this.toolTip1.SetToolTip(this.comboBoxSensorType, "Set the sensor type, Monochrome for a monochrome camera.");
            this.comboBoxSensorType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSensorType_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label12.Location = new System.Drawing.Point(37, 228);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Type";
            // 
            // checkBoxHasShutter
            // 
            this.checkBoxHasShutter.AutoSize = true;
            this.checkBoxHasShutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxHasShutter.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxHasShutter.Location = new System.Drawing.Point(45, 157);
            this.checkBoxHasShutter.Name = "checkBoxHasShutter";
            this.checkBoxHasShutter.Size = new System.Drawing.Size(82, 17);
            this.checkBoxHasShutter.TabIndex = 14;
            this.checkBoxHasShutter.Text = "Has Shutter";
            this.toolTip1.SetToolTip(this.checkBoxHasShutter, "Check this if the camera has a mechanical shutter.");
            this.checkBoxHasShutter.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanAsymmetricBin
            // 
            this.checkBoxCanAsymmetricBin.AutoSize = true;
            this.checkBoxCanAsymmetricBin.Checked = true;
            this.checkBoxCanAsymmetricBin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCanAsymmetricBin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanAsymmetricBin.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanAsymmetricBin.Location = new System.Drawing.Point(45, 111);
            this.checkBoxCanAsymmetricBin.Name = "checkBoxCanAsymmetricBin";
            this.checkBoxCanAsymmetricBin.Size = new System.Drawing.Size(119, 17);
            this.checkBoxCanAsymmetricBin.TabIndex = 13;
            this.checkBoxCanAsymmetricBin.Text = "Can Asymmetric Bin";
            this.toolTip1.SetToolTip(this.checkBoxCanAsymmetricBin, "Check this if the camera can have different X and Y bin values");
            this.checkBoxCanAsymmetricBin.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(132, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(44, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Max Bin  X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(37, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Height (Pixels)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(40, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Width (Pixels)";
            // 
            // groupBoxGainSettings
            // 
            this.groupBoxGainSettings.Controls.Add(this.radioButtonNoGain);
            this.groupBoxGainSettings.Controls.Add(this.textBoxGainMax);
            this.groupBoxGainSettings.Controls.Add(this.textBoxGainMin);
            this.groupBoxGainSettings.Controls.Add(this.radioButtonUseMinAndMax);
            this.groupBoxGainSettings.Controls.Add(this.radioButtonUseGains);
            this.groupBoxGainSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxGainSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxGainSettings.Location = new System.Drawing.Point(237, 148);
            this.groupBoxGainSettings.Name = "groupBoxGainSettings";
            this.groupBoxGainSettings.Size = new System.Drawing.Size(200, 114);
            this.groupBoxGainSettings.TabIndex = 5;
            this.groupBoxGainSettings.TabStop = false;
            this.groupBoxGainSettings.Text = "Gain Settings";
            // 
            // radioButtonNoGain
            // 
            this.radioButtonNoGain.AutoSize = true;
            this.radioButtonNoGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNoGain.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonNoGain.Location = new System.Drawing.Point(34, 20);
            this.radioButtonNoGain.Name = "radioButtonNoGain";
            this.radioButtonNoGain.Size = new System.Drawing.Size(99, 17);
            this.radioButtonNoGain.TabIndex = 29;
            this.radioButtonNoGain.TabStop = true;
            this.radioButtonNoGain.Text = "No Gain control";
            this.radioButtonNoGain.UseVisualStyleBackColor = true;
            // 
            // textBoxGainMax
            // 
            this.textBoxGainMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGainMax.Location = new System.Drawing.Point(108, 86);
            this.textBoxGainMax.Name = "textBoxGainMax";
            this.textBoxGainMax.Size = new System.Drawing.Size(40, 20);
            this.textBoxGainMax.TabIndex = 28;
            // 
            // textBoxGainMin
            // 
            this.textBoxGainMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGainMin.Location = new System.Drawing.Point(53, 86);
            this.textBoxGainMin.Name = "textBoxGainMin";
            this.textBoxGainMin.Size = new System.Drawing.Size(40, 20);
            this.textBoxGainMin.TabIndex = 27;
            // 
            // radioButtonUseMinAndMax
            // 
            this.radioButtonUseMinAndMax.AutoSize = true;
            this.radioButtonUseMinAndMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonUseMinAndMax.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonUseMinAndMax.Location = new System.Drawing.Point(34, 63);
            this.radioButtonUseMinAndMax.Name = "radioButtonUseMinAndMax";
            this.radioButtonUseMinAndMax.Size = new System.Drawing.Size(133, 17);
            this.radioButtonUseMinAndMax.TabIndex = 1;
            this.radioButtonUseMinAndMax.TabStop = true;
            this.radioButtonUseMinAndMax.Text = "Use Gain Min and Max";
            this.radioButtonUseMinAndMax.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseGains
            // 
            this.radioButtonUseGains.AutoSize = true;
            this.radioButtonUseGains.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonUseGains.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonUseGains.Location = new System.Drawing.Point(34, 40);
            this.radioButtonUseGains.Name = "radioButtonUseGains";
            this.radioButtonUseGains.Size = new System.Drawing.Size(132, 17);
            this.radioButtonUseGains.TabIndex = 0;
            this.radioButtonUseGains.TabStop = true;
            this.radioButtonUseGains.Text = "Use list of Gain Names";
            this.radioButtonUseGains.UseVisualStyleBackColor = true;
            // 
            // groupBoxCooling
            // 
            this.groupBoxCooling.Controls.Add(this.BtnCoolerConfiguration);
            this.groupBoxCooling.Controls.Add(this.checkBoxHasCooler);
            this.groupBoxCooling.Controls.Add(this.checkBoxCanGetCoolerPower);
            this.groupBoxCooling.Controls.Add(this.checkBoxCanSetCCDTemperature);
            this.groupBoxCooling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCooling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxCooling.Location = new System.Drawing.Point(237, 12);
            this.groupBoxCooling.Name = "groupBoxCooling";
            this.groupBoxCooling.Size = new System.Drawing.Size(200, 130);
            this.groupBoxCooling.TabIndex = 6;
            this.groupBoxCooling.TabStop = false;
            this.groupBoxCooling.Text = "Cooling";
            // 
            // BtnCoolerConfiguration
            // 
            this.BtnCoolerConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCoolerConfiguration.Location = new System.Drawing.Point(130, 94);
            this.BtnCoolerConfiguration.Name = "BtnCoolerConfiguration";
            this.BtnCoolerConfiguration.Size = new System.Drawing.Size(59, 24);
            this.BtnCoolerConfiguration.TabIndex = 14;
            this.BtnCoolerConfiguration.Text = "Setup";
            this.toolTip1.SetToolTip(this.BtnCoolerConfiguration, resources.GetString("BtnCoolerConfiguration.ToolTip"));
            this.BtnCoolerConfiguration.UseVisualStyleBackColor = true;
            this.BtnCoolerConfiguration.Click += new System.EventHandler(this.BtnCoolerConfiguration_Click);
            // 
            // checkBoxHasCooler
            // 
            this.checkBoxHasCooler.AutoSize = true;
            this.checkBoxHasCooler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxHasCooler.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxHasCooler.Location = new System.Drawing.Point(34, 20);
            this.checkBoxHasCooler.Name = "checkBoxHasCooler";
            this.checkBoxHasCooler.Size = new System.Drawing.Size(78, 17);
            this.checkBoxHasCooler.TabIndex = 2;
            this.checkBoxHasCooler.Text = "Has Cooler";
            this.toolTip1.SetToolTip(this.checkBoxHasCooler, "Check this if the camera has a CCD cooler that can be controlled.");
            this.checkBoxHasCooler.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanGetCoolerPower
            // 
            this.checkBoxCanGetCoolerPower.AutoSize = true;
            this.checkBoxCanGetCoolerPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanGetCoolerPower.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanGetCoolerPower.Location = new System.Drawing.Point(34, 60);
            this.checkBoxCanGetCoolerPower.Name = "checkBoxCanGetCoolerPower";
            this.checkBoxCanGetCoolerPower.Size = new System.Drawing.Size(131, 17);
            this.checkBoxCanGetCoolerPower.TabIndex = 1;
            this.checkBoxCanGetCoolerPower.Text = "Can Get Cooler Power";
            this.toolTip1.SetToolTip(this.checkBoxCanGetCoolerPower, "Check this if the cooler power can be read");
            this.checkBoxCanGetCoolerPower.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSetCCDTemperature
            // 
            this.checkBoxCanSetCCDTemperature.AutoSize = true;
            this.checkBoxCanSetCCDTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanSetCCDTemperature.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanSetCCDTemperature.Location = new System.Drawing.Point(34, 40);
            this.checkBoxCanSetCCDTemperature.Name = "checkBoxCanSetCCDTemperature";
            this.checkBoxCanSetCCDTemperature.Size = new System.Drawing.Size(152, 17);
            this.checkBoxCanSetCCDTemperature.TabIndex = 0;
            this.checkBoxCanSetCCDTemperature.Text = "Can Set CCD Temperature";
            this.toolTip1.SetToolTip(this.checkBoxCanSetCCDTemperature, "Check this if the CCD temperature can be set");
            this.checkBoxCanSetCCDTemperature.UseVisualStyleBackColor = true;
            // 
            // groupBoxExposure
            // 
            this.groupBoxExposure.Controls.Add(this.label9);
            this.groupBoxExposure.Controls.Add(this.TxtSubExposure);
            this.groupBoxExposure.Controls.Add(this.ChkHasSubExposure);
            this.groupBoxExposure.Controls.Add(this.textBoxMaxExposure);
            this.groupBoxExposure.Controls.Add(this.textBoxMinExposure);
            this.groupBoxExposure.Controls.Add(this.label16);
            this.groupBoxExposure.Controls.Add(this.label5);
            this.groupBoxExposure.Controls.Add(this.checkBoxCanStopExposure);
            this.groupBoxExposure.Controls.Add(this.checkBoxCanAbortExposure);
            this.groupBoxExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExposure.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxExposure.Location = new System.Drawing.Point(237, 343);
            this.groupBoxExposure.Name = "groupBoxExposure";
            this.groupBoxExposure.Size = new System.Drawing.Size(200, 172);
            this.groupBoxExposure.TabIndex = 7;
            this.groupBoxExposure.TabStop = false;
            this.groupBoxExposure.Text = "Exposure";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label9.Location = new System.Drawing.Point(31, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Sub exposure (s)";
            // 
            // TxtSubExposure
            // 
            this.TxtSubExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSubExposure.Location = new System.Drawing.Point(121, 140);
            this.TxtSubExposure.Name = "TxtSubExposure";
            this.TxtSubExposure.Size = new System.Drawing.Size(61, 20);
            this.TxtSubExposure.TabIndex = 7;
            // 
            // ChkHasSubExposure
            // 
            this.ChkHasSubExposure.AutoSize = true;
            this.ChkHasSubExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkHasSubExposure.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ChkHasSubExposure.Location = new System.Drawing.Point(34, 117);
            this.ChkHasSubExposure.Name = "ChkHasSubExposure";
            this.ChkHasSubExposure.Size = new System.Drawing.Size(111, 17);
            this.ChkHasSubExposure.TabIndex = 6;
            this.ChkHasSubExposure.Text = "Has sub exposure";
            this.ChkHasSubExposure.UseVisualStyleBackColor = true;
            this.ChkHasSubExposure.CheckedChanged += new System.EventHandler(this.ChkHasSubExposure_CheckedChanged);
            // 
            // textBoxMaxExposure
            // 
            this.textBoxMaxExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMaxExposure.Location = new System.Drawing.Point(121, 91);
            this.textBoxMaxExposure.Name = "textBoxMaxExposure";
            this.textBoxMaxExposure.Size = new System.Drawing.Size(61, 20);
            this.textBoxMaxExposure.TabIndex = 5;
            // 
            // textBoxMinExposure
            // 
            this.textBoxMinExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMinExposure.Location = new System.Drawing.Point(121, 65);
            this.textBoxMinExposure.Name = "textBoxMinExposure";
            this.textBoxMinExposure.Size = new System.Drawing.Size(61, 20);
            this.textBoxMinExposure.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label16.Location = new System.Drawing.Point(31, 94);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Max Exposure (s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(31, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Min exposure (s)";
            // 
            // checkBoxCanStopExposure
            // 
            this.checkBoxCanStopExposure.AutoSize = true;
            this.checkBoxCanStopExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanStopExposure.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanStopExposure.Location = new System.Drawing.Point(34, 42);
            this.checkBoxCanStopExposure.Name = "checkBoxCanStopExposure";
            this.checkBoxCanStopExposure.Size = new System.Drawing.Size(116, 17);
            this.checkBoxCanStopExposure.TabIndex = 1;
            this.checkBoxCanStopExposure.Text = "Can Stop exposure";
            this.checkBoxCanStopExposure.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanAbortExposure
            // 
            this.checkBoxCanAbortExposure.AutoSize = true;
            this.checkBoxCanAbortExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanAbortExposure.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanAbortExposure.Location = new System.Drawing.Point(34, 19);
            this.checkBoxCanAbortExposure.Name = "checkBoxCanAbortExposure";
            this.checkBoxCanAbortExposure.Size = new System.Drawing.Size(120, 17);
            this.checkBoxCanAbortExposure.TabIndex = 0;
            this.checkBoxCanAbortExposure.Text = "Can Abort Exposure";
            this.checkBoxCanAbortExposure.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxPixelSizeY);
            this.groupBox1.Controls.Add(this.textBoxPixelSizeX);
            this.groupBox1.Controls.Add(this.textBoxElectronsPerADU);
            this.groupBox1.Controls.Add(this.textBoxMaxADU);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelSizeY);
            this.groupBox1.Controls.Add(this.labelSizeX);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 130);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pixel";
            // 
            // textBoxPixelSizeY
            // 
            this.textBoxPixelSizeY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPixelSizeY.Location = new System.Drawing.Point(117, 44);
            this.textBoxPixelSizeY.Name = "textBoxPixelSizeY";
            this.textBoxPixelSizeY.Size = new System.Drawing.Size(47, 20);
            this.textBoxPixelSizeY.TabIndex = 17;
            this.toolTip1.SetToolTip(this.textBoxPixelSizeY, "Set the pixel height in microns");
            // 
            // textBoxPixelSizeX
            // 
            this.textBoxPixelSizeX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPixelSizeX.Location = new System.Drawing.Point(117, 18);
            this.textBoxPixelSizeX.Name = "textBoxPixelSizeX";
            this.textBoxPixelSizeX.Size = new System.Drawing.Size(47, 20);
            this.textBoxPixelSizeX.TabIndex = 16;
            this.toolTip1.SetToolTip(this.textBoxPixelSizeX, "Set the pixel width in microns.");
            // 
            // textBoxElectronsPerADU
            // 
            this.textBoxElectronsPerADU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxElectronsPerADU.Location = new System.Drawing.Point(117, 96);
            this.textBoxElectronsPerADU.Name = "textBoxElectronsPerADU";
            this.textBoxElectronsPerADU.Size = new System.Drawing.Size(47, 20);
            this.textBoxElectronsPerADU.TabIndex = 15;
            this.toolTip1.SetToolTip(this.textBoxElectronsPerADU, "The number of electrons for a change of one step on the ADU out");
            // 
            // textBoxMaxADU
            // 
            this.textBoxMaxADU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMaxADU.Location = new System.Drawing.Point(117, 70);
            this.textBoxMaxADU.Name = "textBoxMaxADU";
            this.textBoxMaxADU.Size = new System.Drawing.Size(47, 20);
            this.textBoxMaxADU.TabIndex = 14;
            this.toolTip1.SetToolTip(this.textBoxMaxADU, "This is the maximum ADU value that the camera can return");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label8.Location = new System.Drawing.Point(34, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Maximum ADU";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label7.Location = new System.Drawing.Point(48, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = " e- per ADU";
            // 
            // labelSizeY
            // 
            this.labelSizeY.AutoSize = true;
            this.labelSizeY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSizeY.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelSizeY.Location = new System.Drawing.Point(28, 47);
            this.labelSizeY.Name = "labelSizeY";
            this.labelSizeY.Size = new System.Drawing.Size(83, 13);
            this.labelSizeY.TabIndex = 9;
            this.labelSizeY.Text = "Height (microns)";
            // 
            // labelSizeX
            // 
            this.labelSizeX.AutoSize = true;
            this.labelSizeX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSizeX.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelSizeX.Location = new System.Drawing.Point(31, 21);
            this.labelSizeX.Name = "labelSizeX";
            this.labelSizeX.Size = new System.Drawing.Size(80, 13);
            this.labelSizeX.TabIndex = 8;
            this.labelSizeX.Text = "Width (microns)";
            // 
            // buttonSetImageFile
            // 
            this.buttonSetImageFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetImageFile.Location = new System.Drawing.Point(39, 48);
            this.buttonSetImageFile.Name = "buttonSetImageFile";
            this.buttonSetImageFile.Size = new System.Drawing.Size(82, 24);
            this.buttonSetImageFile.TabIndex = 2;
            this.buttonSetImageFile.Text = "Image File...";
            this.toolTip1.SetToolTip(this.buttonSetImageFile, resources.GetString("buttonSetImageFile.ToolTip"));
            this.buttonSetImageFile.UseVisualStyleBackColor = true;
            this.buttonSetImageFile.Click += new System.EventHandler(this.buttonSetImageFile_Click);
            // 
            // checkBoxApplyNoise
            // 
            this.checkBoxApplyNoise.AutoSize = true;
            this.checkBoxApplyNoise.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxApplyNoise.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxApplyNoise.Location = new System.Drawing.Point(39, 19);
            this.checkBoxApplyNoise.Name = "checkBoxApplyNoise";
            this.checkBoxApplyNoise.Size = new System.Drawing.Size(82, 17);
            this.checkBoxApplyNoise.TabIndex = 0;
            this.checkBoxApplyNoise.Text = "Apply Noise";
            this.toolTip1.SetToolTip(this.checkBoxApplyNoise, "Check this to apply noise to the simulated image.  The amount of noise will vary " +
        "depending on the CCD temperature, exposure time and image brightness.");
            this.checkBoxApplyNoise.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            // 
            // groupBoxGuiding
            // 
            this.groupBoxGuiding.Controls.Add(this.checkBoxCanPulseGuide);
            this.groupBoxGuiding.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxGuiding.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxGuiding.Location = new System.Drawing.Point(461, 268);
            this.groupBoxGuiding.Name = "groupBoxGuiding";
            this.groupBoxGuiding.Size = new System.Drawing.Size(200, 69);
            this.groupBoxGuiding.TabIndex = 11;
            this.groupBoxGuiding.TabStop = false;
            this.groupBoxGuiding.Text = "Guiding";
            // 
            // checkBoxCanPulseGuide
            // 
            this.checkBoxCanPulseGuide.AutoSize = true;
            this.checkBoxCanPulseGuide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanPulseGuide.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanPulseGuide.Location = new System.Drawing.Point(39, 19);
            this.checkBoxCanPulseGuide.Name = "checkBoxCanPulseGuide";
            this.checkBoxCanPulseGuide.Size = new System.Drawing.Size(105, 17);
            this.checkBoxCanPulseGuide.TabIndex = 0;
            this.checkBoxCanPulseGuide.Text = "Can Pulse Guide";
            this.toolTip1.SetToolTip(this.checkBoxCanPulseGuide, "Check this if the camera can accept pulse guide commands. They will have no effec" +
        "t.");
            this.checkBoxCanPulseGuide.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseReadoutModes
            // 
            this.checkBoxUseReadoutModes.AutoSize = true;
            this.checkBoxUseReadoutModes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUseReadoutModes.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxUseReadoutModes.Location = new System.Drawing.Point(34, 42);
            this.checkBoxUseReadoutModes.Name = "checkBoxUseReadoutModes";
            this.checkBoxUseReadoutModes.Size = new System.Drawing.Size(141, 17);
            this.checkBoxUseReadoutModes.TabIndex = 0;
            this.checkBoxUseReadoutModes.Text = "Multiple Readout Modes";
            this.toolTip1.SetToolTip(this.checkBoxUseReadoutModes, "Check this if the camera can accept pulse guide commands. They will have no effec" +
        "t.");
            this.checkBoxUseReadoutModes.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanFastReadout
            // 
            this.checkBoxCanFastReadout.AutoSize = true;
            this.checkBoxCanFastReadout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCanFastReadout.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxCanFastReadout.Location = new System.Drawing.Point(35, 19);
            this.checkBoxCanFastReadout.Name = "checkBoxCanFastReadout";
            this.checkBoxCanFastReadout.Size = new System.Drawing.Size(122, 17);
            this.checkBoxCanFastReadout.TabIndex = 1;
            this.checkBoxCanFastReadout.Text = "Can do Fast readout";
            this.toolTip1.SetToolTip(this.checkBoxCanFastReadout, "Check this if the camera can accept pulse guide commands. They will have no effec" +
        "t.");
            this.checkBoxCanFastReadout.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogging
            // 
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxLogging.ForeColor = System.Drawing.SystemColors.Highlight;
            this.checkBoxLogging.Location = new System.Drawing.Point(39, 18);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new System.Drawing.Size(64, 17);
            this.checkBoxLogging.TabIndex = 13;
            this.checkBoxLogging.Text = "Logging";
            this.toolTip1.SetToolTip(this.checkBoxLogging, "Check this to turn logging on. Log files are in the \"My Documents\\ASCOM\\<date>\" f" +
        "older.\r\n");
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.checkBoxLogging.CheckedChanged += new System.EventHandler(this.checkBoxLogging_CheckedChanged);
            // 
            // groupBoxReadoutModes
            // 
            this.groupBoxReadoutModes.Controls.Add(this.checkBoxCanFastReadout);
            this.groupBoxReadoutModes.Controls.Add(this.checkBoxUseReadoutModes);
            this.groupBoxReadoutModes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxReadoutModes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBoxReadoutModes.Location = new System.Drawing.Point(237, 268);
            this.groupBoxReadoutModes.Name = "groupBoxReadoutModes";
            this.groupBoxReadoutModes.Size = new System.Drawing.Size(200, 69);
            this.groupBoxReadoutModes.TabIndex = 12;
            this.groupBoxReadoutModes.TabStop = false;
            this.groupBoxReadoutModes.Text = "Readout Modes";
            // 
            // GrpOffset
            // 
            this.GrpOffset.Controls.Add(this.RadNoOffset);
            this.GrpOffset.Controls.Add(this.TxtOffsetMax);
            this.GrpOffset.Controls.Add(this.TxtOffsetMin);
            this.GrpOffset.Controls.Add(this.RadOffsetMinMax);
            this.GrpOffset.Controls.Add(this.RadOffsets);
            this.GrpOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpOffset.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GrpOffset.Location = new System.Drawing.Point(461, 148);
            this.GrpOffset.Name = "GrpOffset";
            this.GrpOffset.Size = new System.Drawing.Size(200, 114);
            this.GrpOffset.TabIndex = 14;
            this.GrpOffset.TabStop = false;
            this.GrpOffset.Text = "Offset Settings";
            // 
            // RadNoOffset
            // 
            this.RadNoOffset.AutoSize = true;
            this.RadNoOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadNoOffset.ForeColor = System.Drawing.SystemColors.Highlight;
            this.RadNoOffset.Location = new System.Drawing.Point(39, 19);
            this.RadNoOffset.Name = "RadNoOffset";
            this.RadNoOffset.Size = new System.Drawing.Size(105, 17);
            this.RadNoOffset.TabIndex = 29;
            this.RadNoOffset.TabStop = true;
            this.RadNoOffset.Text = "No Offset control";
            this.RadNoOffset.UseVisualStyleBackColor = true;
            // 
            // TxtOffsetMax
            // 
            this.TxtOffsetMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOffsetMax.Location = new System.Drawing.Point(114, 85);
            this.TxtOffsetMax.Name = "TxtOffsetMax";
            this.TxtOffsetMax.Size = new System.Drawing.Size(40, 20);
            this.TxtOffsetMax.TabIndex = 28;
            // 
            // TxtOffsetMin
            // 
            this.TxtOffsetMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOffsetMin.Location = new System.Drawing.Point(59, 85);
            this.TxtOffsetMin.Name = "TxtOffsetMin";
            this.TxtOffsetMin.Size = new System.Drawing.Size(40, 20);
            this.TxtOffsetMin.TabIndex = 27;
            // 
            // RadOffsetMinMax
            // 
            this.RadOffsetMinMax.AutoSize = true;
            this.RadOffsetMinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadOffsetMinMax.ForeColor = System.Drawing.SystemColors.Highlight;
            this.RadOffsetMinMax.Location = new System.Drawing.Point(39, 62);
            this.RadOffsetMinMax.Name = "RadOffsetMinMax";
            this.RadOffsetMinMax.Size = new System.Drawing.Size(139, 17);
            this.RadOffsetMinMax.TabIndex = 1;
            this.RadOffsetMinMax.TabStop = true;
            this.RadOffsetMinMax.Text = "Use Offset Min and Max";
            this.RadOffsetMinMax.UseVisualStyleBackColor = true;
            // 
            // RadOffsets
            // 
            this.RadOffsets.AutoSize = true;
            this.RadOffsets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadOffsets.ForeColor = System.Drawing.SystemColors.Highlight;
            this.RadOffsets.Location = new System.Drawing.Point(39, 39);
            this.RadOffsets.Name = "RadOffsets";
            this.RadOffsets.Size = new System.Drawing.Size(136, 17);
            this.RadOffsets.TabIndex = 0;
            this.RadOffsets.TabStop = true;
            this.RadOffsets.Text = "Use list of Offset names";
            this.RadOffsets.UseVisualStyleBackColor = true;
            // 
            // GrpSimulatorSetup
            // 
            this.GrpSimulatorSetup.Controls.Add(this.label1);
            this.GrpSimulatorSetup.Controls.Add(this.NumInterfaceVersion);
            this.GrpSimulatorSetup.Controls.Add(this.checkBoxLogging);
            this.GrpSimulatorSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpSimulatorSetup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GrpSimulatorSetup.Location = new System.Drawing.Point(461, 12);
            this.GrpSimulatorSetup.Name = "GrpSimulatorSetup";
            this.GrpSimulatorSetup.Size = new System.Drawing.Size(200, 130);
            this.GrpSimulatorSetup.TabIndex = 15;
            this.GrpSimulatorSetup.TabStop = false;
            this.GrpSimulatorSetup.Text = "Simulator Setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(95, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Interface Version";
            // 
            // NumInterfaceVersion
            // 
            this.NumInterfaceVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumInterfaceVersion.Location = new System.Drawing.Point(39, 41);
            this.NumInterfaceVersion.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NumInterfaceVersion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumInterfaceVersion.Name = "NumInterfaceVersion";
            this.NumInterfaceVersion.Size = new System.Drawing.Size(50, 20);
            this.NumInterfaceVersion.TabIndex = 14;
            this.NumInterfaceVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumInterfaceVersion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumInterfaceVersion.ValueChanged += new System.EventHandler(this.NumInterfaceVersion_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSetImageFile);
            this.groupBox2.Controls.Add(this.checkBoxApplyNoise);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(461, 344);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 171);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulated Image";
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(744, 523);
            this.Controls.Add(this.GrpSimulatorSetup);
            this.Controls.Add(this.GrpOffset);
            this.Controls.Add(this.groupBoxReadoutModes);
            this.Controls.Add(this.groupBoxGuiding);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxExposure);
            this.Controls.Add(this.groupBoxCooling);
            this.Controls.Add(this.groupBoxGainSettings);
            this.Controls.Add(this.groupBoxCCD);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulator Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBoxCCD.ResumeLayout(false);
            this.groupBoxCCD.PerformLayout();
            this.groupBoxGainSettings.ResumeLayout(false);
            this.groupBoxGainSettings.PerformLayout();
            this.groupBoxCooling.ResumeLayout(false);
            this.groupBoxCooling.PerformLayout();
            this.groupBoxExposure.ResumeLayout(false);
            this.groupBoxExposure.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxGuiding.ResumeLayout(false);
            this.groupBoxGuiding.PerformLayout();
            this.groupBoxReadoutModes.ResumeLayout(false);
            this.groupBoxReadoutModes.PerformLayout();
            this.GrpOffset.ResumeLayout(false);
            this.GrpOffset.PerformLayout();
            this.GrpSimulatorSetup.ResumeLayout(false);
            this.GrpSimulatorSetup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumInterfaceVersion)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.GroupBox groupBoxCCD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxCanAsymmetricBin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxHasShutter;
        private System.Windows.Forms.GroupBox groupBoxGainSettings;
        private System.Windows.Forms.GroupBox groupBoxCooling;
        private System.Windows.Forms.CheckBox checkBoxCanGetCoolerPower;
        private System.Windows.Forms.CheckBox checkBoxCanSetCCDTemperature;
        private System.Windows.Forms.GroupBox groupBoxExposure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSizeY;
        private System.Windows.Forms.Label labelSizeX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxMaxADU;
        private System.Windows.Forms.RadioButton radioButtonUseMinAndMax;
        private System.Windows.Forms.RadioButton radioButtonUseGains;
        private System.Windows.Forms.ComboBox comboBoxSensorType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxMaxBinY;
        private System.Windows.Forms.TextBox textBoxMaxBinX;
        private System.Windows.Forms.TextBox textBoxSensorName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxBayerOffsetY;
        private System.Windows.Forms.Label labelBayerOffsetY;
        private System.Windows.Forms.TextBox textBoxBayerOffsetX;
        private System.Windows.Forms.Label labelBayerOffsetX;
        private System.Windows.Forms.CheckBox checkBoxApplyNoise;
        private System.Windows.Forms.TextBox textBoxMaxExposure;
        private System.Windows.Forms.TextBox textBoxMinExposure;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxCanStopExposure;
        private System.Windows.Forms.CheckBox checkBoxCanAbortExposure;
        private System.Windows.Forms.CheckBox checkBoxHasCooler;
        private System.Windows.Forms.TextBox textBoxElectronsPerADU;
        private System.Windows.Forms.TextBox textBoxPixelSizeY;
        private System.Windows.Forms.TextBox textBoxPixelSizeX;
        private System.Windows.Forms.TextBox textBoxCameraYSize;
        private System.Windows.Forms.TextBox textBoxCameraXSize;
        private System.Windows.Forms.TextBox textBoxGainMax;
        private System.Windows.Forms.TextBox textBoxGainMin;
        private System.Windows.Forms.RadioButton radioButtonNoGain;
        private System.Windows.Forms.Button buttonSetImageFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBoxGuiding;
        private System.Windows.Forms.CheckBox checkBoxCanPulseGuide;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxOmitOddBins;
        private System.Windows.Forms.GroupBox groupBoxReadoutModes;
        private System.Windows.Forms.CheckBox checkBoxUseReadoutModes;
        private System.Windows.Forms.CheckBox checkBoxCanFastReadout;
        private System.Windows.Forms.CheckBox checkBoxLogging;
        private System.Windows.Forms.Button BtnCoolerConfiguration;
        private System.Windows.Forms.GroupBox GrpOffset;
        private System.Windows.Forms.RadioButton RadNoOffset;
        private System.Windows.Forms.TextBox TxtOffsetMax;
        private System.Windows.Forms.TextBox TxtOffsetMin;
        private System.Windows.Forms.RadioButton RadOffsetMinMax;
        private System.Windows.Forms.RadioButton RadOffsets;
        private System.Windows.Forms.GroupBox GrpSimulatorSetup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumInterfaceVersion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TxtSubExposure;
        private System.Windows.Forms.CheckBox ChkHasSubExposure;
    }
}