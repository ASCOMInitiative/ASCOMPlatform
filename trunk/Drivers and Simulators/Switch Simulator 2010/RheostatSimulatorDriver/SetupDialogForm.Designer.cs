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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vuMeter3 = new ASCOM.Controls.VuMeter();
            this.vuMeter2 = new ASCOM.Controls.VuMeter();
            this.vuMeter1 = new ASCOM.Controls.VuMeter();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(297, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.Color.Black;
            this.trackBar1.Location = new System.Drawing.Point(14, 93);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(142, 25);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.ValueChanged += new System.EventHandler(this.SetSwitch1);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(14, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(30, 13);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(300, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 58);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Black;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(124, 78);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(30, 13);
            this.textBox2.TabIndex = 5;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Black;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(124, 212);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(30, 13);
            this.textBox3.TabIndex = 9;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.Black;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.ForeColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(14, 210);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(30, 13);
            this.textBox4.TabIndex = 8;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.BackColor = System.Drawing.Color.Black;
            this.trackBar2.Location = new System.Drawing.Point(14, 227);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(142, 25);
            this.trackBar2.TabIndex = 7;
            this.trackBar2.ValueChanged += new System.EventHandler(this.SetSwitch2);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(204, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Temperature";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(248, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "label3";
            // 
            // vuMeter3
            // 
            this.vuMeter3.AnalogMeter = false;
            this.vuMeter3.BackColor = System.Drawing.Color.Black;
            this.vuMeter3.BorderColor = System.Drawing.Color.DimGray;
            this.vuMeter3.DialBackground = System.Drawing.Color.WhiteSmoke;
            this.vuMeter3.DialTextNegative = System.Drawing.Color.Red;
            this.vuMeter3.DialTextPositive = System.Drawing.Color.Black;
            this.vuMeter3.DialTextZero = System.Drawing.Color.DarkGreen;
            this.vuMeter3.Led1ColorOff = System.Drawing.Color.DarkGreen;
            this.vuMeter3.Led1ColorOn = System.Drawing.Color.LimeGreen;
            this.vuMeter3.Led1Count = 6;
            this.vuMeter3.Led2ColorOff = System.Drawing.Color.Olive;
            this.vuMeter3.Led2ColorOn = System.Drawing.Color.Yellow;
            this.vuMeter3.Led2Count = 6;
            this.vuMeter3.Led3ColorOff = System.Drawing.Color.Maroon;
            this.vuMeter3.Led3ColorOn = System.Drawing.Color.Red;
            this.vuMeter3.Led3Count = 4;
            this.vuMeter3.LedSize = new System.Drawing.Size(6, 10);
            this.vuMeter3.LedSpace = 3;
            this.vuMeter3.Level = 0;
            this.vuMeter3.LevelMax = 100;
            this.vuMeter3.Location = new System.Drawing.Point(230, 10);
            this.vuMeter3.MeterScale = ASCOM.Controls.MeterScale.Log10;
            this.vuMeter3.Name = "vuMeter3";
            this.vuMeter3.NeedleColor = System.Drawing.Color.Black;
            this.vuMeter3.PeakHold = true;
            this.vuMeter3.Peakms = 1000;
            this.vuMeter3.PeakNeedleColor = System.Drawing.Color.Red;
            this.vuMeter3.ShowDialOnly = false;
            this.vuMeter3.ShowLedPeak = true;
            this.vuMeter3.ShowTextInDial = true;
            this.vuMeter3.Size = new System.Drawing.Size(12, 211);
            this.vuMeter3.TabIndex = 10;
            this.vuMeter3.TextInDial = new string[] {
        "0",
        "20",
        "40",
        "60",
        "80",
        "100"};
            this.vuMeter3.UseLedLight = true;
            this.vuMeter3.VerticalBar = true;
            this.vuMeter3.VuText = "Local Temp";
            // 
            // vuMeter2
            // 
            this.vuMeter2.AnalogMeter = true;
            this.vuMeter2.BackColor = System.Drawing.Color.Black;
            this.vuMeter2.BorderColor = System.Drawing.Color.DimGray;
            this.vuMeter2.DialBackground = System.Drawing.Color.LightBlue;
            this.vuMeter2.DialTextNegative = System.Drawing.Color.Red;
            this.vuMeter2.DialTextPositive = System.Drawing.Color.Black;
            this.vuMeter2.DialTextZero = System.Drawing.Color.DarkGreen;
            this.vuMeter2.Led1ColorOff = System.Drawing.Color.DarkGreen;
            this.vuMeter2.Led1ColorOn = System.Drawing.Color.LimeGreen;
            this.vuMeter2.Led1Count = 6;
            this.vuMeter2.Led2ColorOff = System.Drawing.Color.Olive;
            this.vuMeter2.Led2ColorOn = System.Drawing.Color.Yellow;
            this.vuMeter2.Led2Count = 6;
            this.vuMeter2.Led3ColorOff = System.Drawing.Color.Maroon;
            this.vuMeter2.Led3ColorOn = System.Drawing.Color.Red;
            this.vuMeter2.Led3Count = 4;
            this.vuMeter2.LedSize = new System.Drawing.Size(2, 11);
            this.vuMeter2.LedSpace = 3;
            this.vuMeter2.Level = 0;
            this.vuMeter2.LevelMax = 100;
            this.vuMeter2.Location = new System.Drawing.Point(9, 144);
            this.vuMeter2.MeterScale = ASCOM.Controls.MeterScale.Log10;
            this.vuMeter2.Name = "vuMeter2";
            this.vuMeter2.NeedleColor = System.Drawing.Color.Black;
            this.vuMeter2.PeakHold = true;
            this.vuMeter2.Peakms = 10000;
            this.vuMeter2.PeakNeedleColor = System.Drawing.Color.Red;
            this.vuMeter2.ShowDialOnly = false;
            this.vuMeter2.ShowLedPeak = true;
            this.vuMeter2.ShowTextInDial = true;
            this.vuMeter2.Size = new System.Drawing.Size(147, 117);
            this.vuMeter2.TabIndex = 6;
            this.vuMeter2.TextInDial = new string[] {
        "0",
        "20",
        "40",
        "60",
        "80",
        "100"};
            this.vuMeter2.UseLedLight = true;
            this.vuMeter2.VerticalBar = false;
            this.vuMeter2.VuText = "2";
            // 
            // vuMeter1
            // 
            this.vuMeter1.AnalogMeter = true;
            this.vuMeter1.BackColor = System.Drawing.Color.Black;
            this.vuMeter1.BorderColor = System.Drawing.Color.DimGray;
            this.vuMeter1.DialBackground = System.Drawing.Color.LightBlue;
            this.vuMeter1.DialTextNegative = System.Drawing.Color.Red;
            this.vuMeter1.DialTextPositive = System.Drawing.Color.Black;
            this.vuMeter1.DialTextZero = System.Drawing.Color.DarkGreen;
            this.vuMeter1.Led1ColorOff = System.Drawing.Color.DarkGreen;
            this.vuMeter1.Led1ColorOn = System.Drawing.Color.LimeGreen;
            this.vuMeter1.Led1Count = 6;
            this.vuMeter1.Led2ColorOff = System.Drawing.Color.Olive;
            this.vuMeter1.Led2ColorOn = System.Drawing.Color.Yellow;
            this.vuMeter1.Led2Count = 6;
            this.vuMeter1.Led3ColorOff = System.Drawing.Color.Maroon;
            this.vuMeter1.Led3ColorOn = System.Drawing.Color.Red;
            this.vuMeter1.Led3Count = 4;
            this.vuMeter1.LedSize = new System.Drawing.Size(2, 11);
            this.vuMeter1.LedSpace = 3;
            this.vuMeter1.Level = 0;
            this.vuMeter1.LevelMax = 100;
            this.vuMeter1.Location = new System.Drawing.Point(9, 10);
            this.vuMeter1.MeterScale = ASCOM.Controls.MeterScale.Log10;
            this.vuMeter1.Name = "vuMeter1";
            this.vuMeter1.NeedleColor = System.Drawing.Color.Black;
            this.vuMeter1.PeakHold = true;
            this.vuMeter1.Peakms = 10000;
            this.vuMeter1.PeakNeedleColor = System.Drawing.Color.Red;
            this.vuMeter1.ShowDialOnly = false;
            this.vuMeter1.ShowLedPeak = true;
            this.vuMeter1.ShowTextInDial = true;
            this.vuMeter1.Size = new System.Drawing.Size(147, 117);
            this.vuMeter1.TabIndex = 1;
            this.vuMeter1.TextInDial = new string[] {
        "0",
        "20",
        "40",
        "60",
        "80",
        "100"};
            this.vuMeter1.UseLedLight = true;
            this.vuMeter1.VerticalBar = false;
            this.vuMeter1.VuText = "1";
            // 
            // SetupDialogForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(377, 301);
            this.Controls.Add(this.vuMeter3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vuMeter2);
            this.Controls.Add(this.vuMeter1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ASCOM Rheostat Switch";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private Controls.VuMeter vuMeter1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TrackBar trackBar2;
        private Controls.VuMeter vuMeter2;
        private Controls.VuMeter vuMeter3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private readonly ISwitchV2 Switches = new RheostatSwitch();
    }
}