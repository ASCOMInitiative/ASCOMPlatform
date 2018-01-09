using System;

namespace ASCOM.Simulator
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnShutDown = new System.Windows.Forms.Button();
            this.lblNumberOfConnections = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnEnable = new System.Windows.Forms.Button();
            this.chkMinimise = new System.Windows.Forms.CheckBox();
            this.overrideViewWindSpeed = new ASCOM.Simulator.OverrideView();
            this.overrideViewWindGust = new ASCOM.Simulator.OverrideView();
            this.overrideViewWindDirection = new ASCOM.Simulator.OverrideView();
            this.overrideViewTemperature = new ASCOM.Simulator.OverrideView();
            this.overrideViewSkyTemperature = new ASCOM.Simulator.OverrideView();
            this.overrideViewStarFWHM = new ASCOM.Simulator.OverrideView();
            this.overrideViewSkyQuality = new ASCOM.Simulator.OverrideView();
            this.overrideViewSkyBrightness = new ASCOM.Simulator.OverrideView();
            this.overrideViewRainRate = new ASCOM.Simulator.OverrideView();
            this.overrideViewPressure = new ASCOM.Simulator.OverrideView();
            this.overrideViewHumidity = new ASCOM.Simulator.OverrideView();
            this.overrideViewCloudCover = new ASCOM.Simulator.OverrideView();
            this.SuspendLayout();
            // 
            // btnShutDown
            // 
            this.btnShutDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShutDown.Location = new System.Drawing.Point(605, 516);
            this.btnShutDown.Name = "btnShutDown";
            this.btnShutDown.Size = new System.Drawing.Size(100, 23);
            this.btnShutDown.TabIndex = 1;
            this.btnShutDown.Text = "ShutDown";
            this.btnShutDown.UseVisualStyleBackColor = true;
            this.btnShutDown.Click += new System.EventHandler(this.btnShutDown_Click);
            // 
            // lblNumberOfConnections
            // 
            this.lblNumberOfConnections.AutoSize = true;
            this.lblNumberOfConnections.Location = new System.Drawing.Point(18, 521);
            this.lblNumberOfConnections.Name = "lblNumberOfConnections";
            this.lblNumberOfConnections.Size = new System.Drawing.Size(129, 13);
            this.lblNumberOfConnections.TabIndex = 2;
            this.lblNumberOfConnections.Text = "Number of connections: 0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label14.Location = new System.Drawing.Point(45, 455);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 37;
            this.label14.Text = "Wind Speed";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label13.Location = new System.Drawing.Point(55, 421);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "Wind Gust";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label12.Location = new System.Drawing.Point(30, 387);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Wind Direction";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label11.Location = new System.Drawing.Point(43, 353);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Temperature";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label10.Location = new System.Drawing.Point(18, 285);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Sky Temperature";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label9.Location = new System.Drawing.Point(50, 319);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Star FWHM";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label8.Location = new System.Drawing.Point(50, 251);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Sky Quality";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label7.Location = new System.Drawing.Point(30, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Sky Brightness";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label6.Location = new System.Drawing.Point(57, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Rain Rate";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label5.Location = new System.Drawing.Point(66, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Humidity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label4.Location = new System.Drawing.Point(65, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Pressure";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label3.Location = new System.Drawing.Point(56, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Dew Point";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.label2.Location = new System.Drawing.Point(45, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Cloud Cover";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(268, 79);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(207, 13);
            this.label35.TabIndex = 64;
            this.label35.Text = "Calculated from Temperature and Humidity";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label33.Location = new System.Drawing.Point(117, 20);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(55, 13);
            this.label33.TabIndex = 65;
            this.label33.Text = "Override";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(222, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "From";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label15.Location = new System.Drawing.Point(496, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(22, 13);
            this.label15.TabIndex = 67;
            this.label15.Text = "To";
            // 
            // btnEnable
            // 
            this.btnEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnable.Location = new System.Drawing.Point(499, 516);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(100, 23);
            this.btnEnable.TabIndex = 68;
            this.btnEnable.Text = "Enable Changes";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // chkMinimise
            // 
            this.chkMinimise.AutoSize = true;
            this.chkMinimise.Location = new System.Drawing.Point(500, 491);
            this.chkMinimise.Name = "chkMinimise";
            this.chkMinimise.Size = new System.Drawing.Size(104, 17);
            this.chkMinimise.TabIndex = 69;
            this.chkMinimise.Text = "Minimise on start";
            this.chkMinimise.UseVisualStyleBackColor = true;
            // 
            // overrideViewWindSpeed
            // 
            this.overrideViewWindSpeed.Location = new System.Drawing.Point(127, 446);
            this.overrideViewWindSpeed.Name = "overrideViewWindSpeed";
            this.overrideViewWindSpeed.Size = new System.Drawing.Size(551, 28);
            this.overrideViewWindSpeed.TabIndex = 16;
            // 
            // overrideViewWindGust
            // 
            this.overrideViewWindGust.Location = new System.Drawing.Point(127, 412);
            this.overrideViewWindGust.Name = "overrideViewWindGust";
            this.overrideViewWindGust.Size = new System.Drawing.Size(551, 28);
            this.overrideViewWindGust.TabIndex = 15;
            // 
            // overrideViewWindDirection
            // 
            this.overrideViewWindDirection.Location = new System.Drawing.Point(127, 378);
            this.overrideViewWindDirection.Name = "overrideViewWindDirection";
            this.overrideViewWindDirection.Size = new System.Drawing.Size(551, 28);
            this.overrideViewWindDirection.TabIndex = 14;
            // 
            // overrideViewTemperature
            // 
            this.overrideViewTemperature.Location = new System.Drawing.Point(127, 344);
            this.overrideViewTemperature.Name = "overrideViewTemperature";
            this.overrideViewTemperature.Size = new System.Drawing.Size(551, 28);
            this.overrideViewTemperature.TabIndex = 13;
            // 
            // overrideViewSkyTemperature
            // 
            this.overrideViewSkyTemperature.Location = new System.Drawing.Point(127, 276);
            this.overrideViewSkyTemperature.Name = "overrideViewSkyTemperature";
            this.overrideViewSkyTemperature.Size = new System.Drawing.Size(551, 28);
            this.overrideViewSkyTemperature.TabIndex = 12;
            // 
            // overrideViewStarFWHM
            // 
            this.overrideViewStarFWHM.Location = new System.Drawing.Point(127, 310);
            this.overrideViewStarFWHM.Name = "overrideViewStarFWHM";
            this.overrideViewStarFWHM.Size = new System.Drawing.Size(551, 28);
            this.overrideViewStarFWHM.TabIndex = 11;
            // 
            // overrideViewSkyQuality
            // 
            this.overrideViewSkyQuality.Location = new System.Drawing.Point(127, 242);
            this.overrideViewSkyQuality.Name = "overrideViewSkyQuality";
            this.overrideViewSkyQuality.Size = new System.Drawing.Size(551, 28);
            this.overrideViewSkyQuality.TabIndex = 9;
            // 
            // overrideViewSkyBrightness
            // 
            this.overrideViewSkyBrightness.Location = new System.Drawing.Point(127, 208);
            this.overrideViewSkyBrightness.Name = "overrideViewSkyBrightness";
            this.overrideViewSkyBrightness.Size = new System.Drawing.Size(551, 28);
            this.overrideViewSkyBrightness.TabIndex = 8;
            // 
            // overrideViewRainRate
            // 
            this.overrideViewRainRate.Location = new System.Drawing.Point(127, 172);
            this.overrideViewRainRate.Name = "overrideViewRainRate";
            this.overrideViewRainRate.Size = new System.Drawing.Size(551, 28);
            this.overrideViewRainRate.TabIndex = 6;
            // 
            // overrideViewPressure
            // 
            this.overrideViewPressure.Location = new System.Drawing.Point(127, 138);
            this.overrideViewPressure.Name = "overrideViewPressure";
            this.overrideViewPressure.Size = new System.Drawing.Size(551, 28);
            this.overrideViewPressure.TabIndex = 5;
            // 
            // overrideViewHumidity
            // 
            this.overrideViewHumidity.Location = new System.Drawing.Point(127, 104);
            this.overrideViewHumidity.Name = "overrideViewHumidity";
            this.overrideViewHumidity.Size = new System.Drawing.Size(551, 28);
            this.overrideViewHumidity.TabIndex = 4;
            // 
            // overrideViewCloudCover
            // 
            this.overrideViewCloudCover.Location = new System.Drawing.Point(127, 36);
            this.overrideViewCloudCover.Name = "overrideViewCloudCover";
            this.overrideViewCloudCover.Size = new System.Drawing.Size(551, 28);
            this.overrideViewCloudCover.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 551);
            this.Controls.Add(this.chkMinimise);
            this.Controls.Add(this.btnEnable);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.overrideViewWindSpeed);
            this.Controls.Add(this.overrideViewWindGust);
            this.Controls.Add(this.overrideViewWindDirection);
            this.Controls.Add(this.overrideViewTemperature);
            this.Controls.Add(this.overrideViewSkyTemperature);
            this.Controls.Add(this.overrideViewStarFWHM);
            this.Controls.Add(this.overrideViewSkyQuality);
            this.Controls.Add(this.overrideViewSkyBrightness);
            this.Controls.Add(this.overrideViewRainRate);
            this.Controls.Add(this.overrideViewPressure);
            this.Controls.Add(this.overrideViewHumidity);
            this.Controls.Add(this.overrideViewCloudCover);
            this.Controls.Add(this.lblNumberOfConnections);
            this.Controls.Add(this.btnShutDown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Observing Conditions Simulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnShutDown;
        private System.Windows.Forms.Label lblNumberOfConnections;
        private OverrideView overrideViewCloudCover;
        private OverrideView overrideViewHumidity;
        private OverrideView overrideViewPressure;
        private OverrideView overrideViewRainRate;
        private OverrideView overrideViewSkyBrightness;
        private OverrideView overrideViewSkyQuality;
        private OverrideView overrideViewWindSpeed;
        private OverrideView overrideViewWindGust;
        private OverrideView overrideViewWindDirection;
        private OverrideView overrideViewTemperature;
        private OverrideView overrideViewSkyTemperature;
        private OverrideView overrideViewStarFWHM;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkMinimise;
        public System.Windows.Forms.Button btnEnable;
    }
}

