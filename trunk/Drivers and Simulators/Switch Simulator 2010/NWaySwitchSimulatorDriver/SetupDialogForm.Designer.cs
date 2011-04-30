using ASCOM.Controls;
using ASCOM.DeviceInterface;

namespace ASCOM.NWaySwitchSimulator
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.aGauge1 = new ASCOM.Controls.AGauge();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.button8 = new System.Windows.Forms.Button();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.aGauge3 = new ASCOM.Controls.AGauge();
            this.txtPowerWarning = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(443, 410);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(50, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "Exit";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = global::ASCOM.NWaySwitchSimulator.Properties.Resources.ASCOM;
            this.pictureBox1.Image = global::ASCOM.NWaySwitchSimulator.Properties.Resources.ASCOM;
            this.pictureBox1.InitialImage = global::ASCOM.NWaySwitchSimulator.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(443, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 60);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(BrowseToAscom);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 427);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "v";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(32, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(36, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "2";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox1.Location = new System.Drawing.Point(351, 64);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(42, 21);
            this.comboBox1.TabIndex = 31;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBox2.Location = new System.Drawing.Point(351, 104);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(42, 21);
            this.comboBox2.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(344, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(348, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "label5";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(270, 41);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 148);
            this.trackBar1.TabIndex = 35;
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar1MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // aGauge1
            // 
            this.aGauge1.BackColor = System.Drawing.Color.Black;
            this.aGauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge1.BaseArcRadius = 40;
            this.aGauge1.BaseArcStart = -90;
            this.aGauge1.BaseArcSweep = 360;
            this.aGauge1.BaseArcWidth = 2;
            this.aGauge1.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.aGauge1.CapIdx = ((byte)(1));
            this.aGauge1.CapPosition = new System.Drawing.Point(10, 10);
            this.aGauge1.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.aGauge1.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.aGauge1.CapText = "";
            this.aGauge1.Center = new System.Drawing.Point(70, 70);
            this.aGauge1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aGauge1.Location = new System.Drawing.Point(115, 43);
            this.aGauge1.MaxValue = 10F;
            this.aGauge1.MinValue = 0F;
            this.aGauge1.Name = "aGauge1";
            this.aGauge1.NeedleColor1 = ASCOM.Controls.AGauge.NeedleColorEnum.Red;
            this.aGauge1.NeedleColor2 = System.Drawing.Color.Black;
            this.aGauge1.NeedleRadius = 40;
            this.aGauge1.NeedleType = 0;
            this.aGauge1.NeedleWidth = 10;
            this.aGauge1.RangeColor = System.Drawing.Color.Red;
            this.aGauge1.RangeEnabled = false;
            this.aGauge1.RangeEndValue = 300F;
            this.aGauge1.RangeIdx = ((byte)(0));
            this.aGauge1.RangeInnerRadius = 70;
            this.aGauge1.RangeOuterRadius = 80;
            this.aGauge1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.Red,
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))),
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge1.RangesEnabled = new bool[] {
        false,
        false,
        false,
        false,
        false};
            this.aGauge1.RangesEndValue = new float[] {
        300F,
        400F,
        0F,
        0F,
        0F};
            this.aGauge1.RangesInnerRadius = new int[] {
        70,
        10,
        70,
        70,
        70};
            this.aGauge1.RangesOuterRadius = new int[] {
        80,
        40,
        80,
        80,
        80};
            this.aGauge1.RangesStartValue = new float[] {
        -100F,
        300F,
        0F,
        0F,
        0F};
            this.aGauge1.RangeStartValue = -100F;
            this.aGauge1.ScaleLinesInterColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.aGauge1.ScaleLinesInterInnerRadius = 42;
            this.aGauge1.ScaleLinesInterOuterRadius = 50;
            this.aGauge1.ScaleLinesInterWidth = 1;
            this.aGauge1.ScaleLinesMajorColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.aGauge1.ScaleLinesMajorInnerRadius = 40;
            this.aGauge1.ScaleLinesMajorOuterRadius = 50;
            this.aGauge1.ScaleLinesMajorStepValue = 1F;
            this.aGauge1.ScaleLinesMajorWidth = 2;
            this.aGauge1.ScaleLinesMinorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.aGauge1.ScaleLinesMinorInnerRadius = 43;
            this.aGauge1.ScaleLinesMinorNumOf = 1;
            this.aGauge1.ScaleLinesMinorOuterRadius = 50;
            this.aGauge1.ScaleLinesMinorWidth = 1;
            this.aGauge1.ScaleNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.aGauge1.ScaleNumbersFormat = null;
            this.aGauge1.ScaleNumbersRadius = 62;
            this.aGauge1.ScaleNumbersRotation = 0;
            this.aGauge1.ScaleNumbersStartScaleLine = 2;
            this.aGauge1.ScaleNumbersStepScaleLines = 2;
            this.aGauge1.Size = new System.Drawing.Size(149, 148);
            this.aGauge1.TabIndex = 15;
            this.aGauge1.Text = "Volts";
            this.aGauge1.Value = 0F;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(321, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Min";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(318, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Max";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(318, 294);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "Max";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(321, 254);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Min";
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 1;
            this.trackBar2.Location = new System.Drawing.Point(76, 379);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(221, 45);
            this.trackBar2.TabIndex = 41;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackBar2MouseUp);
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button8.Location = new System.Drawing.Point(344, 333);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(57, 23);
            this.button8.TabIndex = 40;
            this.button8.Text = "Update";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8Click);
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBox4.Location = new System.Drawing.Point(351, 291);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(42, 21);
            this.comboBox4.TabIndex = 39;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox3.Location = new System.Drawing.Point(351, 251);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(42, 21);
            this.comboBox3.TabIndex = 38;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(348, 360);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "label10";
            // 
            // aGauge3
            // 
            this.aGauge3.BackColor = System.Drawing.Color.Cornsilk;
            this.aGauge3.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge3.BaseArcRadius = 90;
            this.aGauge3.BaseArcStart = 214;
            this.aGauge3.BaseArcSweep = 110;
            this.aGauge3.BaseArcWidth = 2;
            this.aGauge3.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.aGauge3.CapIdx = ((byte)(1));
            this.aGauge3.CapPosition = new System.Drawing.Point(10, 10);
            this.aGauge3.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.aGauge3.CapsText = new string[] {
        "",
        "Volts",
        "",
        "",
        ""};
            this.aGauge3.CapText = "Volts";
            this.aGauge3.Center = new System.Drawing.Point(110, 130);
            this.aGauge3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.aGauge3.Location = new System.Drawing.Point(76, 222);
            this.aGauge3.MaxValue = 100F;
            this.aGauge3.MinValue = 0F;
            this.aGauge3.Name = "aGauge3";
            this.aGauge3.NeedleColor1 = ASCOM.Controls.AGauge.NeedleColorEnum.Yellow;
            this.aGauge3.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge3.NeedleRadius = 80;
            this.aGauge3.NeedleType = 0;
            this.aGauge3.NeedleWidth = 3;
            this.aGauge3.RangeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.aGauge3.RangeEnabled = true;
            this.aGauge3.RangeEndValue = 101F;
            this.aGauge3.RangeIdx = ((byte)(1));
            this.aGauge3.RangeInnerRadius = 10;
            this.aGauge3.RangeOuterRadius = 90;
            this.aGauge3.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.LightGreen,
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))),
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge3.RangesEnabled = new bool[] {
        false,
        true,
        false,
        false,
        false};
            this.aGauge3.RangesEndValue = new float[] {
        300F,
        101F,
        0F,
        0F,
        0F};
            this.aGauge3.RangesInnerRadius = new int[] {
        70,
        10,
        70,
        70,
        70};
            this.aGauge3.RangesOuterRadius = new int[] {
        80,
        90,
        80,
        80,
        80};
            this.aGauge3.RangesStartValue = new float[] {
        -100F,
        80F,
        0F,
        0F,
        0F};
            this.aGauge3.RangeStartValue = 80F;
            this.aGauge3.ScaleLinesInterColor = System.Drawing.Color.Red;
            this.aGauge3.ScaleLinesInterInnerRadius = 90;
            this.aGauge3.ScaleLinesInterOuterRadius = 97;
            this.aGauge3.ScaleLinesInterWidth = 2;
            this.aGauge3.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge3.ScaleLinesMajorInnerRadius = 90;
            this.aGauge3.ScaleLinesMajorOuterRadius = 100;
            this.aGauge3.ScaleLinesMajorStepValue = 10F;
            this.aGauge3.ScaleLinesMajorWidth = 2;
            this.aGauge3.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge3.ScaleLinesMinorInnerRadius = 90;
            this.aGauge3.ScaleLinesMinorNumOf = 5;
            this.aGauge3.ScaleLinesMinorOuterRadius = 95;
            this.aGauge3.ScaleLinesMinorWidth = 1;
            this.aGauge3.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge3.ScaleNumbersFormat = "";
            this.aGauge3.ScaleNumbersRadius = 110;
            this.aGauge3.ScaleNumbersRotation = 90;
            this.aGauge3.ScaleNumbersStartScaleLine = 1;
            this.aGauge3.ScaleNumbersStepScaleLines = 2;
            this.aGauge3.Size = new System.Drawing.Size(221, 151);
            this.aGauge3.TabIndex = 45;
            this.aGauge3.Text = "aGauge3";
            this.aGauge3.Value = 0F;
            // 
            // txtPowerWarning
            // 
            this.txtPowerWarning.Location = new System.Drawing.Point(324, 222);
            this.txtPowerWarning.Name = "txtPowerWarning";
            this.txtPowerWarning.Size = new System.Drawing.Size(100, 20);
            this.txtPowerWarning.TabIndex = 46;
            // 
            // SetupDialogForm
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(505, 445);
            this.Controls.Add(this.txtPowerWarning);
            this.Controls.Add(this.aGauge3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.aGauge1);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ASCOM NWaySwitch Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private AGauge aGauge1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label10;
        private AGauge aGauge3;
        private System.Windows.Forms.TextBox txtPowerWarning;
    }
}