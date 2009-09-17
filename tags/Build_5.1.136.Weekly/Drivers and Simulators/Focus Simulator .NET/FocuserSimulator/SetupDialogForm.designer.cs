namespace ASCOM.FocuserSimulator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioRelative = new System.Windows.Forms.RadioButton();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.IsTempComp = new System.Windows.Forms.CheckBox();
            this.StepsPerDeg = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.IsTemperature = new System.Windows.Forms.CheckBox();
            this.IsHalt = new System.Windows.Forms.CheckBox();
            this.IsStepSize = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.IsTempCompAvailable = new System.Windows.Forms.CheckBox();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.LabelPosition = new System.Windows.Forms.Label();
            this.RadioAbsolute = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepsPerDeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDown4);
            this.groupBox1.Controls.Add(this.LabelPosition);
            this.groupBox1.Controls.Add(this.RadioRelative);
            this.groupBox1.Controls.Add(this.RadioAbsolute);
            this.groupBox1.Controls.Add(this.shapeContainer1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 79);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // RadioRelative
            // 
            this.RadioRelative.AutoSize = true;
            this.RadioRelative.Checked = true;
            this.RadioRelative.Location = new System.Drawing.Point(6, 42);
            this.RadioRelative.Name = "RadioRelative";
            this.RadioRelative.Size = new System.Drawing.Size(64, 17);
            this.RadioRelative.TabIndex = 1;
            this.RadioRelative.TabStop = true;
            this.RadioRelative.Text = "Relative";
            this.RadioRelative.UseVisualStyleBackColor = true;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 16);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(257, 60);
            this.shapeContainer1.TabIndex = 9;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Enabled = false;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 80;
            this.lineShape1.X2 = 80;
            this.lineShape1.Y1 = -2;
            this.lineShape1.Y2 = 47;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.IsTemperature);
            this.groupBox2.Controls.Add(this.IsHalt);
            this.groupBox2.Controls.Add(this.IsStepSize);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.IsTempCompAvailable);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 143);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Capabilities";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max Increment";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.FocuserSimulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(311, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_cancel;
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(293, 115);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(72, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::ASCOM.FocuserSimulator.Properties.Resources.button_ok;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(293, 85);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(72, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numericUpDown5);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.IsTempComp);
            this.groupBox4.Controls.Add(this.StepsPerDeg);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.textBox3);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempCompAvailable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.groupBox4.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempCompAvailable;
            this.groupBox4.Location = new System.Drawing.Point(12, 246);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(263, 120);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Temperature compensation";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(204, 65);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown5.TabIndex = 16;
            this.numericUpDown5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown5.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Period (sec)";
            // 
            // IsTempComp
            // 
            this.IsTempComp.AutoSize = true;
            this.IsTempComp.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempComp;
            this.IsTempComp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsTempComp.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempComp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTempComp.Location = new System.Drawing.Point(6, 91);
            this.IsTempComp.Name = "IsTempComp";
            this.IsTempComp.Size = new System.Drawing.Size(185, 17);
            this.IsTempComp.TabIndex = 14;
            this.IsTempComp.Text = "Enable temperature compensation";
            this.IsTempComp.UseVisualStyleBackColor = true;
            // 
            // StepsPerDeg
            // 
            this.StepsPerDeg.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxIncrement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.StepsPerDeg.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sStepPerDeg", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.StepsPerDeg.Location = new System.Drawing.Point(65, 65);
            this.StepsPerDeg.Maximum = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxIncrement;
            this.StepsPerDeg.Name = "StepsPerDeg";
            this.StepsPerDeg.Size = new System.Drawing.Size(53, 20);
            this.StepsPerDeg.TabIndex = 13;
            this.StepsPerDeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StepsPerDeg.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sStepPerDeg;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Steps / °C";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Minimum";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(140, 32);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(44, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "4.0";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Maximum";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(74, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(44, 20);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "20.0";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Current";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(44, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "17.5°";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IsTemperature
            // 
            this.IsTemperature.AutoSize = true;
            this.IsTemperature.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sIsTemperature;
            this.IsTemperature.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsTemperature.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sIsTemperature", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTemperature.Location = new System.Drawing.Point(6, 73);
            this.IsTemperature.Name = "IsTemperature";
            this.IsTemperature.Size = new System.Drawing.Size(129, 17);
            this.IsTemperature.TabIndex = 11;
            this.IsTemperature.Text = "Temperature available";
            this.IsTemperature.UseVisualStyleBackColor = true;
            this.IsTemperature.Click += new System.EventHandler(this.IsTemperature_Click);
            // 
            // IsHalt
            // 
            this.IsHalt.AutoSize = true;
            this.IsHalt.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sEnableHalt;
            this.IsHalt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsHalt.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sEnableHalt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsHalt.Location = new System.Drawing.Point(6, 119);
            this.IsHalt.Name = "IsHalt";
            this.IsHalt.Size = new System.Drawing.Size(88, 17);
            this.IsHalt.TabIndex = 10;
            this.IsHalt.Text = "Enable HALT";
            this.IsHalt.UseVisualStyleBackColor = true;
            // 
            // IsStepSize
            // 
            this.IsStepSize.AutoSize = true;
            this.IsStepSize.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sIsStepSize;
            this.IsStepSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsStepSize.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sIsStepSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsStepSize.Location = new System.Drawing.Point(6, 19);
            this.IsStepSize.Name = "IsStepSize";
            this.IsStepSize.Size = new System.Drawing.Size(66, 17);
            this.IsStepSize.TabIndex = 9;
            this.IsStepSize.Text = "StepSize";
            this.IsStepSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IsStepSize.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxIncrement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown2.Location = new System.Drawing.Point(86, 44);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown2.TabIndex = 8;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxIncrement;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sStepSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sIsStepSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sIsStepSize;
            this.numericUpDown1.Location = new System.Drawing.Point(86, 18);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sStepSize;
            // 
            // IsTempCompAvailable
            // 
            this.IsTempCompAvailable.AutoSize = true;
            this.IsTempCompAvailable.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempCompAvailable;
            this.IsTempCompAvailable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsTempCompAvailable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempCompAvailable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTempCompAvailable.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sIsTemperature", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTempCompAvailable.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sIsTemperature;
            this.IsTempCompAvailable.Location = new System.Drawing.Point(6, 96);
            this.IsTempCompAvailable.Name = "IsTempCompAvailable";
            this.IsTempCompAvailable.Size = new System.Drawing.Size(198, 17);
            this.IsTempCompAvailable.TabIndex = 1;
            this.IsTempCompAvailable.Text = "Temperature compensation available";
            this.IsTempCompAvailable.UseVisualStyleBackColor = true;
            this.IsTempCompAvailable.Click += new System.EventHandler(this.IsTempCompAvailable_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown3.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown3.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.numericUpDown3.Location = new System.Drawing.Point(194, 44);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown3.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxStep;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.label3.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.label3.Location = new System.Drawing.Point(98, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Maximum position";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown4.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown4.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown4.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.numericUpDown4.Location = new System.Drawing.Point(194, 18);
            this.numericUpDown4.Maximum = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxStep;
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown4.TabIndex = 5;
            this.numericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown4.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sPosition;
            // 
            // LabelPosition
            // 
            this.LabelPosition.AutoSize = true;
            this.LabelPosition.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LabelPosition.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.LabelPosition.Location = new System.Drawing.Point(108, 20);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Size = new System.Drawing.Size(80, 13);
            this.LabelPosition.TabIndex = 3;
            this.LabelPosition.Text = "Current position";
            // 
            // RadioAbsolute
            // 
            this.RadioAbsolute.AutoSize = true;
            this.RadioAbsolute.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.RadioAbsolute.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RadioAbsolute.Location = new System.Drawing.Point(6, 19);
            this.RadioAbsolute.Name = "RadioAbsolute";
            this.RadioAbsolute.Size = new System.Drawing.Size(66, 17);
            this.RadioAbsolute.TabIndex = 0;
            this.RadioAbsolute.TabStop = true;
            this.RadioAbsolute.Text = "Absolute";
            this.RadioAbsolute.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 374);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
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
            this.Text = "ASCOM Focuser Simulator Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepsPerDeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioRelative;
        private System.Windows.Forms.RadioButton RadioAbsolute;
        private System.Windows.Forms.Label LabelPosition;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox IsTempCompAvailable;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown StepsPerDeg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox IsTempComp;
        private System.Windows.Forms.CheckBox IsStepSize;
        private System.Windows.Forms.CheckBox IsHalt;
        private System.Windows.Forms.CheckBox IsTemperature;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Label label1;
    }
}