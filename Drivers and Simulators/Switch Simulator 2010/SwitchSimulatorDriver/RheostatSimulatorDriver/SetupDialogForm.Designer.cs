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
            this.vuMeter1 = new VU_MeterLibrary.VuMeter();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // vuMeter1
            // 
            this.vuMeter1.AnalogMeter = true;
            this.vuMeter1.BackColor = System.Drawing.Color.Black;
            this.vuMeter1.DialBackground = System.Drawing.Color.White;
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
            this.vuMeter1.LedSize = new System.Drawing.Size(1, 10);
            this.vuMeter1.LedSpace = 3;
            this.vuMeter1.Level = 0;
            this.vuMeter1.LevelMax = 65535;
            this.vuMeter1.Location = new System.Drawing.Point(50, 84);
            this.vuMeter1.MeterScale = VU_MeterLibrary.MeterScale.Log10;
            this.vuMeter1.Name = "vuMeter1";
            this.vuMeter1.NeedleColor = System.Drawing.Color.Black;
            this.vuMeter1.PeakHold = true;
            this.vuMeter1.Peakms = 1000;
            this.vuMeter1.PeakNeedleColor = System.Drawing.Color.Black;
            this.vuMeter1.ShowDialOnly = false;
            this.vuMeter1.ShowLedPeak = false;
            this.vuMeter1.ShowTextInDial = false;
            this.vuMeter1.Size = new System.Drawing.Size(147, 117);
            this.vuMeter1.TabIndex = 0;
            this.vuMeter1.TextInDial = new string[] {
        "0",
        "20",
        "40",
        "60",
        "80",
        "100"};
            this.vuMeter1.UseLedLight = true;
            this.vuMeter1.VerticalBar = false;
            this.vuMeter1.VuText = "Heater 1";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(73, 170);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(317, 272);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.vuMeter1);
            this.Name = "SetupDialogForm";
            this.Text = "SetupDialogForm";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VU_MeterLibrary.VuMeter vuMeter1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Timer timer1;
    }
}