namespace ASCOM.Simulator
{
    partial class CoolerSetupForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoolerSetupForm));
            this.NumAmbientTemperature = new System.Windows.Forms.NumericUpDown();
            this.NumCCDSetPoint = new System.Windows.Forms.NumericUpDown();
            this.NumCoolerDeltaTMax = new System.Windows.Forms.NumericUpDown();
            this.cmbCoolerModes = new System.Windows.Forms.ComboBox();
            this.NumTimeToSetPoint = new System.Windows.Forms.NumericUpDown();
            this.LblAmbientTemperature = new System.Windows.Forms.Label();
            this.LblCCDSetPoint = new System.Windows.Forms.Label();
            this.LblCoolerDeltaTMax = new System.Windows.Forms.Label();
            this.LblTimeToSetPoint = new System.Windows.Forms.Label();
            this.BtnOK = new System.Windows.Forms.Button();
            this.ChkResetToAmbientOnConnect = new System.Windows.Forms.CheckBox();
            this.CoolingHelp = new System.Windows.Forms.HelpProvider();
            this.NumFluctuations = new System.Windows.Forms.NumericUpDown();
            this.NumOvershoot = new System.Windows.Forms.NumericUpDown();
            this.ChkPowerUpState = new System.Windows.Forms.CheckBox();
            this.NumUnderDampedCycles = new System.Windows.Forms.NumericUpDown();
            this.NumSetpointMinimum = new System.Windows.Forms.NumericUpDown();
            this.ChkFitCurveToScreen = new System.Windows.Forms.CheckBox();
            this.LblHelpText = new System.Windows.Forms.Label();
            this.BackgroundPictureBox = new System.Windows.Forms.PictureBox();
            this.LblFluctuations = new System.Windows.Forms.Label();
            this.LblOvershoot = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.LblOvershootCycles = new System.Windows.Forms.Label();
            this.CoolingChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LblSetpointMinimum = new System.Windows.Forms.Label();
            this.LblGraph = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFluctuations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOvershoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUnderDampedCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSetpointMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoolingChart)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // NumAmbientTemperature
            // 
            this.NumAmbientTemperature.DecimalPlaces = 1;
            this.NumAmbientTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumAmbientTemperature, "");
            this.NumAmbientTemperature.Location = new System.Drawing.Point(18, 19);
            this.NumAmbientTemperature.Name = "NumAmbientTemperature";
            this.CoolingHelp.SetShowHelp(this.NumAmbientTemperature, true);
            this.NumAmbientTemperature.Size = new System.Drawing.Size(55, 20);
            this.NumAmbientTemperature.TabIndex = 21;
            this.NumAmbientTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumCCDSetPoint
            // 
            this.NumCCDSetPoint.DecimalPlaces = 1;
            this.NumCCDSetPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumCCDSetPoint, "");
            this.NumCCDSetPoint.Location = new System.Drawing.Point(19, 45);
            this.NumCCDSetPoint.Name = "NumCCDSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumCCDSetPoint, true);
            this.NumCCDSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumCCDSetPoint.TabIndex = 23;
            this.NumCCDSetPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumCoolerDeltaTMax
            // 
            this.NumCoolerDeltaTMax.DecimalPlaces = 1;
            this.NumCoolerDeltaTMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumCoolerDeltaTMax, "");
            this.NumCoolerDeltaTMax.Location = new System.Drawing.Point(18, 71);
            this.NumCoolerDeltaTMax.Name = "NumCoolerDeltaTMax";
            this.CoolingHelp.SetShowHelp(this.NumCoolerDeltaTMax, true);
            this.NumCoolerDeltaTMax.Size = new System.Drawing.Size(55, 20);
            this.NumCoolerDeltaTMax.TabIndex = 25;
            this.NumCoolerDeltaTMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbCoolerModes
            // 
            this.cmbCoolerModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoolerModes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCoolerModes.Location = new System.Drawing.Point(18, 19);
            this.cmbCoolerModes.Name = "cmbCoolerModes";
            this.cmbCoolerModes.Size = new System.Drawing.Size(307, 21);
            this.cmbCoolerModes.TabIndex = 11;
            // 
            // NumTimeToSetPoint
            // 
            this.NumTimeToSetPoint.DecimalPlaces = 1;
            this.NumTimeToSetPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumTimeToSetPoint, "");
            this.NumTimeToSetPoint.Location = new System.Drawing.Point(18, 19);
            this.NumTimeToSetPoint.Name = "NumTimeToSetPoint";
            this.CoolingHelp.SetShowHelp(this.NumTimeToSetPoint, true);
            this.NumTimeToSetPoint.Size = new System.Drawing.Size(55, 20);
            this.NumTimeToSetPoint.TabIndex = 31;
            this.NumTimeToSetPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LblAmbientTemperature
            // 
            this.LblAmbientTemperature.AutoSize = true;
            this.LblAmbientTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAmbientTemperature.Location = new System.Drawing.Point(80, 21);
            this.LblAmbientTemperature.Name = "LblAmbientTemperature";
            this.LblAmbientTemperature.Size = new System.Drawing.Size(172, 13);
            this.LblAmbientTemperature.TabIndex = 22;
            this.LblAmbientTemperature.Text = "Ambient (heat sink) temperature (C)";
            // 
            // LblCCDSetPoint
            // 
            this.LblCCDSetPoint.AutoSize = true;
            this.LblCCDSetPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCCDSetPoint.Location = new System.Drawing.Point(80, 47);
            this.LblCCDSetPoint.Name = "LblCCDSetPoint";
            this.LblCCDSetPoint.Size = new System.Drawing.Size(93, 13);
            this.LblCCDSetPoint.TabIndex = 24;
            this.LblCCDSetPoint.Text = "Cooler setpoint (C)";
            // 
            // LblCoolerDeltaTMax
            // 
            this.LblCoolerDeltaTMax.AutoSize = true;
            this.LblCoolerDeltaTMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCoolerDeltaTMax.Location = new System.Drawing.Point(79, 73);
            this.LblCoolerDeltaTMax.Name = "LblCoolerDeltaTMax";
            this.LblCoolerDeltaTMax.Size = new System.Drawing.Size(194, 13);
            this.LblCoolerDeltaTMax.TabIndex = 26;
            this.LblCoolerDeltaTMax.Text = "Cooler maximum delta t from ambient (C)";
            // 
            // LblTimeToSetPoint
            // 
            this.LblTimeToSetPoint.AutoSize = true;
            this.LblTimeToSetPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTimeToSetPoint.Location = new System.Drawing.Point(79, 21);
            this.LblTimeToSetPoint.Name = "LblTimeToSetPoint";
            this.LblTimeToSetPoint.Size = new System.Drawing.Size(247, 13);
            this.LblTimeToSetPoint.TabIndex = 32;
            this.LblTimeToSetPoint.Text = "Time to cool from ambient to the setpoint (seconds)";
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(821, 485);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(59, 23);
            this.BtnOK.TabIndex = 54;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ChkResetToAmbientOnConnect
            // 
            this.ChkResetToAmbientOnConnect.AutoSize = true;
            this.ChkResetToAmbientOnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.ChkResetToAmbientOnConnect, "");
            this.ChkResetToAmbientOnConnect.Location = new System.Drawing.Point(62, 71);
            this.ChkResetToAmbientOnConnect.Name = "ChkResetToAmbientOnConnect";
            this.CoolingHelp.SetShowHelp(this.ChkResetToAmbientOnConnect, true);
            this.ChkResetToAmbientOnConnect.Size = new System.Drawing.Size(257, 17);
            this.ChkResetToAmbientOnConnect.TabIndex = 35;
            this.ChkResetToAmbientOnConnect.Text = "Reset cooler to ambient when cooler is turned on";
            this.ChkResetToAmbientOnConnect.UseVisualStyleBackColor = true;
            // 
            // NumFluctuations
            // 
            this.NumFluctuations.DecimalPlaces = 2;
            this.NumFluctuations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumFluctuations, "");
            this.NumFluctuations.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumFluctuations.Location = new System.Drawing.Point(18, 45);
            this.NumFluctuations.Name = "NumFluctuations";
            this.CoolingHelp.SetShowHelp(this.NumFluctuations, true);
            this.NumFluctuations.Size = new System.Drawing.Size(55, 20);
            this.NumFluctuations.TabIndex = 33;
            this.NumFluctuations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumOvershoot
            // 
            this.NumOvershoot.DecimalPlaces = 1;
            this.NumOvershoot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumOvershoot, "");
            this.NumOvershoot.Location = new System.Drawing.Point(18, 19);
            this.NumOvershoot.Name = "NumOvershoot";
            this.CoolingHelp.SetShowHelp(this.NumOvershoot, true);
            this.NumOvershoot.Size = new System.Drawing.Size(55, 20);
            this.NumOvershoot.TabIndex = 41;
            this.NumOvershoot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ChkPowerUpState
            // 
            this.ChkPowerUpState.AutoSize = true;
            this.ChkPowerUpState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.ChkPowerUpState, "");
            this.ChkPowerUpState.Location = new System.Drawing.Point(62, 94);
            this.ChkPowerUpState.Name = "ChkPowerUpState";
            this.CoolingHelp.SetShowHelp(this.ChkPowerUpState, true);
            this.ChkPowerUpState.Size = new System.Drawing.Size(140, 17);
            this.ChkPowerUpState.TabIndex = 36;
            this.ChkPowerUpState.Text = "Power up with cooler on";
            this.ChkPowerUpState.UseVisualStyleBackColor = true;
            // 
            // NumUnderDampedCycles
            // 
            this.NumUnderDampedCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumUnderDampedCycles, "");
            this.NumUnderDampedCycles.Location = new System.Drawing.Point(18, 45);
            this.NumUnderDampedCycles.Name = "NumUnderDampedCycles";
            this.CoolingHelp.SetShowHelp(this.NumUnderDampedCycles, true);
            this.NumUnderDampedCycles.Size = new System.Drawing.Size(55, 20);
            this.NumUnderDampedCycles.TabIndex = 43;
            this.NumUnderDampedCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumSetpointMinimum
            // 
            this.NumSetpointMinimum.DecimalPlaces = 1;
            this.NumSetpointMinimum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.NumSetpointMinimum, "");
            this.NumSetpointMinimum.Location = new System.Drawing.Point(19, 97);
            this.NumSetpointMinimum.Name = "NumSetpointMinimum";
            this.CoolingHelp.SetShowHelp(this.NumSetpointMinimum, true);
            this.NumSetpointMinimum.Size = new System.Drawing.Size(55, 20);
            this.NumSetpointMinimum.TabIndex = 27;
            this.NumSetpointMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ChkFitCurveToScreen
            // 
            this.ChkFitCurveToScreen.AutoSize = true;
            this.ChkFitCurveToScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoolingHelp.SetHelpString(this.ChkFitCurveToScreen, "");
            this.ChkFitCurveToScreen.Location = new System.Drawing.Point(42, 408);
            this.ChkFitCurveToScreen.Name = "ChkFitCurveToScreen";
            this.CoolingHelp.SetShowHelp(this.ChkFitCurveToScreen, true);
            this.ChkFitCurveToScreen.Size = new System.Drawing.Size(114, 17);
            this.ChkFitCurveToScreen.TabIndex = 51;
            this.ChkFitCurveToScreen.Text = "Fit curve to screen";
            this.ChkFitCurveToScreen.UseVisualStyleBackColor = true;
            this.ChkFitCurveToScreen.CheckedChanged += new System.EventHandler(this.ChkGraphFullRange_CheckedChanged);
            // 
            // LblHelpText
            // 
            this.LblHelpText.AutoSize = true;
            this.LblHelpText.Location = new System.Drawing.Point(10, 490);
            this.LblHelpText.Name = "LblHelpText";
            this.LblHelpText.Size = new System.Drawing.Size(408, 13);
            this.LblHelpText.TabIndex = 55;
            this.LblHelpText.Text = "Help - Press the ? button and click a control or hover the mouse over a chart ele" +
    "ment";
            // 
            // BackgroundPictureBox
            // 
            this.BackgroundPictureBox.Location = new System.Drawing.Point(-2, -2);
            this.BackgroundPictureBox.Name = "BackgroundPictureBox";
            this.BackgroundPictureBox.Size = new System.Drawing.Size(898, 524);
            this.BackgroundPictureBox.TabIndex = 17;
            this.BackgroundPictureBox.TabStop = false;
            // 
            // LblFluctuations
            // 
            this.LblFluctuations.AutoSize = true;
            this.LblFluctuations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFluctuations.Location = new System.Drawing.Point(79, 47);
            this.LblFluctuations.Name = "LblFluctuations";
            this.LblFluctuations.Size = new System.Drawing.Size(275, 13);
            this.LblFluctuations.TabIndex = 34;
            this.LblFluctuations.Text = "± Random fluctuation in CCD and heat sink temperatures";
            // 
            // LblOvershoot
            // 
            this.LblOvershoot.AutoSize = true;
            this.LblOvershoot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOvershoot.Location = new System.Drawing.Point(79, 21);
            this.LblOvershoot.Name = "LblOvershoot";
            this.LblOvershoot.Size = new System.Drawing.Size(153, 13);
            this.LblOvershoot.TabIndex = 42;
            this.LblOvershoot.Text = "Amount of cooler overshoot (C)";
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(756, 485);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(59, 23);
            this.BtnCancel.TabIndex = 53;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // LblOvershootCycles
            // 
            this.LblOvershootCycles.AutoSize = true;
            this.LblOvershootCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOvershootCycles.Location = new System.Drawing.Point(79, 47);
            this.LblOvershootCycles.Name = "LblOvershootCycles";
            this.LblOvershootCycles.Size = new System.Drawing.Size(180, 13);
            this.LblOvershootCycles.TabIndex = 44;
            this.LblOvershootCycles.Text = "Number of under damped half cycles";
            // 
            // CoolingChart
            // 
            chartArea1.Name = "ChartArea1";
            this.CoolingChart.ChartAreas.Add(chartArea1);
            this.CoolingChart.Location = new System.Drawing.Point(22, 19);
            this.CoolingChart.Name = "CoolingChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "CoolingCurve";
            this.CoolingChart.Series.Add(series1);
            this.CoolingChart.Size = new System.Drawing.Size(438, 352);
            this.CoolingChart.TabIndex = 56;
            this.CoolingChart.TabStop = false;
            this.CoolingChart.Text = "chart1";
            // 
            // LblSetpointMinimum
            // 
            this.LblSetpointMinimum.AutoSize = true;
            this.LblSetpointMinimum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSetpointMinimum.Location = new System.Drawing.Point(81, 99);
            this.LblSetpointMinimum.Name = "LblSetpointMinimum";
            this.LblSetpointMinimum.Size = new System.Drawing.Size(134, 13);
            this.LblSetpointMinimum.TabIndex = 28;
            this.LblSetpointMinimum.Text = "Lowest settable setpoint (C)";
            // 
            // LblGraph
            // 
            this.LblGraph.AutoSize = true;
            this.LblGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblGraph.Location = new System.Drawing.Point(186, 382);
            this.LblGraph.Name = "LblGraph";
            this.LblGraph.Size = new System.Drawing.Size(273, 65);
            this.LblGraph.TabIndex = 52;
            this.LblGraph.Text = resources.GetString("LblGraph.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.NumAmbientTemperature);
            this.groupBox1.Controls.Add(this.LblSetpointMinimum);
            this.groupBox1.Controls.Add(this.NumCCDSetPoint);
            this.groupBox1.Controls.Add(this.NumSetpointMinimum);
            this.groupBox1.Controls.Add(this.NumCoolerDeltaTMax);
            this.groupBox1.Controls.Add(this.LblAmbientTemperature);
            this.groupBox1.Controls.Add(this.LblCCDSetPoint);
            this.groupBox1.Controls.Add(this.LblCoolerDeltaTMax);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 129);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cooler Temperature Characteristics";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.NumTimeToSetPoint);
            this.groupBox2.Controls.Add(this.LblTimeToSetPoint);
            this.groupBox2.Controls.Add(this.NumFluctuations);
            this.groupBox2.Controls.Add(this.LblFluctuations);
            this.groupBox2.Controls.Add(this.ChkResetToAmbientOnConnect);
            this.groupBox2.Controls.Add(this.ChkPowerUpState);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(371, 123);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cooler Auxilliary Characteristics";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.NumOvershoot);
            this.groupBox3.Controls.Add(this.LblOvershoot);
            this.groupBox3.Controls.Add(this.NumUnderDampedCycles);
            this.groupBox3.Controls.Add(this.LblOvershootCycles);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 392);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(371, 82);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cooler Overshoot Characteristics";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CoolingChart);
            this.groupBox4.Controls.Add(this.ChkFitCurveToScreen);
            this.groupBox4.Controls.Add(this.LblGraph);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(397, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(483, 459);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cooler Time Temperature Curve";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmbCoolerModes);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(13, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(371, 53);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Cooler Overall Behaviour";
            // 
            // CoolerSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(896, 519);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.LblHelpText);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.BackgroundPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoolerSetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cooler Configuration";
            this.Load += new System.EventHandler(this.CoolerSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumAmbientTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCCDSetPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoolerDeltaTMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumTimeToSetPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFluctuations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOvershoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUnderDampedCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSetpointMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoolingChart)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LblAmbientTemperature;
        private System.Windows.Forms.Label LblCCDSetPoint;
        private System.Windows.Forms.Label LblCoolerDeltaTMax;
        private System.Windows.Forms.Label LblTimeToSetPoint;
        private System.Windows.Forms.Button BtnOK;
        internal System.Windows.Forms.NumericUpDown NumAmbientTemperature;
        internal System.Windows.Forms.NumericUpDown NumCCDSetPoint;
        internal System.Windows.Forms.NumericUpDown NumCoolerDeltaTMax;
        internal System.Windows.Forms.ComboBox cmbCoolerModes;
        internal System.Windows.Forms.NumericUpDown NumTimeToSetPoint;
        internal System.Windows.Forms.CheckBox ChkResetToAmbientOnConnect;
        private System.Windows.Forms.HelpProvider CoolingHelp;
        private System.Windows.Forms.Label LblHelpText;
        private System.Windows.Forms.PictureBox BackgroundPictureBox;
        internal System.Windows.Forms.NumericUpDown NumFluctuations;
        private System.Windows.Forms.Label LblFluctuations;
        internal System.Windows.Forms.NumericUpDown NumOvershoot;
        private System.Windows.Forms.Label LblOvershoot;
        private System.Windows.Forms.Button BtnCancel;
        internal System.Windows.Forms.CheckBox ChkPowerUpState;
        internal System.Windows.Forms.NumericUpDown NumUnderDampedCycles;
        private System.Windows.Forms.Label LblOvershootCycles;
        private System.Windows.Forms.DataVisualization.Charting.Chart CoolingChart;
        internal System.Windows.Forms.NumericUpDown NumSetpointMinimum;
        private System.Windows.Forms.Label LblSetpointMinimum;
        internal System.Windows.Forms.CheckBox ChkFitCurveToScreen;
        private System.Windows.Forms.Label LblGraph;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}