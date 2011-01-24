using ASCOM.Controls;
using ASCOM.DeviceInterface;

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
            this.buttonOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.but1 = new System.Windows.Forms.Button();
            this.but2 = new System.Windows.Forms.Button();
            this.but3 = new System.Windows.Forms.Button();
            this.but4 = new System.Windows.Forms.Button();
            this.but5 = new System.Windows.Forms.Button();
            this.but6 = new System.Windows.Forms.Button();
            this.but0 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.aGauge2 = new ASCOM.Controls.AGauge();
            this.aGauge1 = new ASCOM.Controls.AGauge();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(513, 109);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(50, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.InitialImage = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(513, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 60);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 219);
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
            this.label2.Location = new System.Drawing.Point(71, 9);
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
            this.label3.Location = new System.Drawing.Point(333, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "2";
            // 
            // but1
            // 
            this.but1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but1.Location = new System.Drawing.Point(41, 187);
            this.but1.Name = "but1";
            this.but1.Size = new System.Drawing.Size(28, 23);
            this.but1.TabIndex = 17;
            this.but1.Text = "1";
            this.but1.UseVisualStyleBackColor = true;
            this.but1.Click += new System.EventHandler(this.But1Click);
            // 
            // but2
            // 
            this.but2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but2.Location = new System.Drawing.Point(75, 187);
            this.but2.Name = "but2";
            this.but2.Size = new System.Drawing.Size(28, 23);
            this.but2.TabIndex = 18;
            this.but2.Text = "2";
            this.but2.UseVisualStyleBackColor = true;
            this.but2.Click += new System.EventHandler(this.But2Click);
            // 
            // but3
            // 
            this.but3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but3.Location = new System.Drawing.Point(109, 187);
            this.but3.Name = "but3";
            this.but3.Size = new System.Drawing.Size(28, 23);
            this.but3.TabIndex = 19;
            this.but3.Text = "3";
            this.but3.UseVisualStyleBackColor = true;
            this.but3.Click += new System.EventHandler(this.But3Click);
            // 
            // but4
            // 
            this.but4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but4.Location = new System.Drawing.Point(143, 187);
            this.but4.Name = "but4";
            this.but4.Size = new System.Drawing.Size(28, 23);
            this.but4.TabIndex = 20;
            this.but4.Text = "4";
            this.but4.UseVisualStyleBackColor = true;
            this.but4.Click += new System.EventHandler(this.But4Click);
            // 
            // but5
            // 
            this.but5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but5.Location = new System.Drawing.Point(177, 187);
            this.but5.Name = "but5";
            this.but5.Size = new System.Drawing.Size(28, 23);
            this.but5.TabIndex = 21;
            this.but5.Text = "5";
            this.but5.UseVisualStyleBackColor = true;
            this.but5.Click += new System.EventHandler(this.But5Click);
            // 
            // but6
            // 
            this.but6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but6.Location = new System.Drawing.Point(211, 187);
            this.but6.Name = "but6";
            this.but6.Size = new System.Drawing.Size(28, 23);
            this.but6.TabIndex = 22;
            this.but6.Text = "6";
            this.but6.UseVisualStyleBackColor = true;
            this.but6.Click += new System.EventHandler(this.But6Click);
            // 
            // but0
            // 
            this.but0.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.but0.Location = new System.Drawing.Point(7, 187);
            this.but0.Name = "but0";
            this.but0.Size = new System.Drawing.Size(28, 23);
            this.but0.TabIndex = 23;
            this.but0.Text = "0";
            this.but0.UseVisualStyleBackColor = true;
            this.but0.Click += new System.EventHandler(this.But0Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(265, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(469, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 23);
            this.button2.TabIndex = 30;
            this.button2.Text = "6";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(435, 187);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(28, 23);
            this.button3.TabIndex = 29;
            this.button3.Text = "5";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(401, 187);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(28, 23);
            this.button4.TabIndex = 28;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(367, 187);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(28, 23);
            this.button5.TabIndex = 27;
            this.button5.Text = "3";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button6.Location = new System.Drawing.Point(333, 187);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(28, 23);
            this.button6.TabIndex = 26;
            this.button6.Text = "2";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button7.Location = new System.Drawing.Point(299, 187);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(28, 23);
            this.button7.TabIndex = 25;
            this.button7.Text = "1";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(267, 219);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "label4";
            // 
            // aGauge2
            // 
            this.aGauge2.BackColor = System.Drawing.Color.LightCyan;
            this.aGauge2.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge2.BaseArcRadius = 90;
            this.aGauge2.BaseArcStart = 214;
            this.aGauge2.BaseArcSweep = 110;
            this.aGauge2.BaseArcWidth = 2;
            this.aGauge2.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.aGauge2.CapIdx = ((byte)(1));
            this.aGauge2.CapPosition = new System.Drawing.Point(10, 10);
            this.aGauge2.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.aGauge2.CapsText = new string[] {
        "",
        "Volts",
        "",
        "",
        ""};
            this.aGauge2.CapText = "Volts";
            this.aGauge2.Center = new System.Drawing.Point(110, 130);
            this.aGauge2.Location = new System.Drawing.Point(270, 30);
            this.aGauge2.MaxValue = 6F;
            this.aGauge2.MinValue = 0F;
            this.aGauge2.Name = "aGauge2";
            this.aGauge2.NeedleColor1 = ASCOM.Controls.AGauge.NeedleColorEnum.Yellow;
            this.aGauge2.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge2.NeedleRadius = 80;
            this.aGauge2.NeedleType = 0;
            this.aGauge2.NeedleWidth = 2;
            this.aGauge2.RangeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.aGauge2.RangeEnabled = true;
            this.aGauge2.RangeEndValue = 6F;
            this.aGauge2.RangeIdx = ((byte)(1));
            this.aGauge2.RangeInnerRadius = 10;
            this.aGauge2.RangeOuterRadius = 90;
            this.aGauge2.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.LightGreen,
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))),
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge2.RangesEnabled = new bool[] {
        false,
        true,
        false,
        false,
        false};
            this.aGauge2.RangesEndValue = new float[] {
        300F,
        6F,
        0F,
        0F,
        0F};
            this.aGauge2.RangesInnerRadius = new int[] {
        70,
        10,
        70,
        70,
        70};
            this.aGauge2.RangesOuterRadius = new int[] {
        80,
        90,
        80,
        80,
        80};
            this.aGauge2.RangesStartValue = new float[] {
        -100F,
        5F,
        0F,
        0F,
        0F};
            this.aGauge2.RangeStartValue = 5F;
            this.aGauge2.ScaleLinesInterColor = System.Drawing.Color.Red;
            this.aGauge2.ScaleLinesInterInnerRadius = 90;
            this.aGauge2.ScaleLinesInterOuterRadius = 93;
            this.aGauge2.ScaleLinesInterWidth = 2;
            this.aGauge2.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleLinesMajorInnerRadius = 90;
            this.aGauge2.ScaleLinesMajorOuterRadius = 100;
            this.aGauge2.ScaleLinesMajorStepValue = 1F;
            this.aGauge2.ScaleLinesMajorWidth = 2;
            this.aGauge2.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge2.ScaleLinesMinorInnerRadius = 90;
            this.aGauge2.ScaleLinesMinorNumOf = 5;
            this.aGauge2.ScaleLinesMinorOuterRadius = 93;
            this.aGauge2.ScaleLinesMinorWidth = 1;
            this.aGauge2.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleNumbersFormat = null;
            this.aGauge2.ScaleNumbersRadius = 110;
            this.aGauge2.ScaleNumbersRotation = 10;
            this.aGauge2.ScaleNumbersStartScaleLine = 1;
            this.aGauge2.ScaleNumbersStepScaleLines = 10;
            this.aGauge2.Size = new System.Drawing.Size(221, 151);
            this.aGauge2.TabIndex = 11;
            this.aGauge2.Text = "aGauge2";
            this.aGauge2.Value = 0F;
            // 
            // aGauge1
            // 
            this.aGauge1.BackColor = System.Drawing.Color.LightCyan;
            this.aGauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge1.BaseArcRadius = 90;
            this.aGauge1.BaseArcStart = 214;
            this.aGauge1.BaseArcSweep = 110;
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
        "Volts",
        "",
        "",
        ""};
            this.aGauge1.CapText = "Volts";
            this.aGauge1.Center = new System.Drawing.Point(110, 130);
            this.aGauge1.Location = new System.Drawing.Point(12, 30);
            this.aGauge1.MaxValue = 6F;
            this.aGauge1.MinValue = 0F;
            this.aGauge1.Name = "aGauge1";
            this.aGauge1.NeedleColor1 = ASCOM.Controls.AGauge.NeedleColorEnum.Yellow;
            this.aGauge1.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge1.NeedleRadius = 80;
            this.aGauge1.NeedleType = 0;
            this.aGauge1.NeedleWidth = 2;
            this.aGauge1.RangeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.aGauge1.RangeEnabled = true;
            this.aGauge1.RangeEndValue = 6F;
            this.aGauge1.RangeIdx = ((byte)(1));
            this.aGauge1.RangeInnerRadius = 10;
            this.aGauge1.RangeOuterRadius = 90;
            this.aGauge1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.LightGreen,
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))),
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.aGauge1.RangesEnabled = new bool[] {
        false,
        true,
        false,
        false,
        false};
            this.aGauge1.RangesEndValue = new float[] {
        300F,
        6F,
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
        90,
        80,
        80,
        80};
            this.aGauge1.RangesStartValue = new float[] {
        -100F,
        5F,
        0F,
        0F,
        0F};
            this.aGauge1.RangeStartValue = 5F;
            this.aGauge1.ScaleLinesInterColor = System.Drawing.Color.Red;
            this.aGauge1.ScaleLinesInterInnerRadius = 90;
            this.aGauge1.ScaleLinesInterOuterRadius = 93;
            this.aGauge1.ScaleLinesInterWidth = 2;
            this.aGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesMajorInnerRadius = 90;
            this.aGauge1.ScaleLinesMajorOuterRadius = 100;
            this.aGauge1.ScaleLinesMajorStepValue = 1F;
            this.aGauge1.ScaleLinesMajorWidth = 2;
            this.aGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge1.ScaleLinesMinorInnerRadius = 90;
            this.aGauge1.ScaleLinesMinorNumOf = 5;
            this.aGauge1.ScaleLinesMinorOuterRadius = 93;
            this.aGauge1.ScaleLinesMinorWidth = 1;
            this.aGauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleNumbersFormat = null;
            this.aGauge1.ScaleNumbersRadius = 110;
            this.aGauge1.ScaleNumbersRotation = 10;
            this.aGauge1.ScaleNumbersStartScaleLine = 1;
            this.aGauge1.ScaleNumbersStepScaleLines = 10;
            this.aGauge1.Size = new System.Drawing.Size(221, 151);
            this.aGauge1.TabIndex = 7;
            this.aGauge1.Text = "aGauge1";
            this.aGauge1.Value = 0F;
            // 
            // SetupDialogForm
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(582, 235);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.but0);
            this.Controls.Add(this.but6);
            this.Controls.Add(this.but5);
            this.Controls.Add(this.but4);
            this.Controls.Add(this.but3);
            this.Controls.Add(this.but2);
            this.Controls.Add(this.but1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.aGauge2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.aGauge1);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ASCOM NWaySwitch Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button but1;
        private System.Windows.Forms.Button but2;
        private System.Windows.Forms.Button but3;
        private System.Windows.Forms.Button but4;
        private System.Windows.Forms.Button but5;
        private System.Windows.Forms.Button but6;
        private AGauge aGauge1;
        private AGauge aGauge2;
        private System.Windows.Forms.Button but0;
        private readonly ISwitchV2 Switches = new NWaySwitchDriver();
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label4;
    }
}