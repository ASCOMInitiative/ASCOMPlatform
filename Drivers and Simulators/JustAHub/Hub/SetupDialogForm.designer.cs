namespace ASCOM.JustAHub
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
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.ChkDriverLoggingCamera = new System.Windows.Forms.CheckBox();
            this.BtnChooseCamera = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblCurrentCameraDevice = new System.Windows.Forms.Label();
            this.LblCurrentFilterWheelDevice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnChooseFilterWheel = new System.Windows.Forms.Button();
            this.TabDevices = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.ChkLocalServerDebugLog = new System.Windows.Forms.CheckBox();
            this.Camera = new System.Windows.Forms.TabPage();
            this.ChkHardwareLoggingCamera = new System.Windows.Forms.CheckBox();
            this.CoverCalibrator = new System.Windows.Forms.TabPage();
            this.ChkHardwareLoggingCoverCalibrator = new System.Windows.Forms.CheckBox();
            this.LblCurrentCoverCalibratorDevice = new System.Windows.Forms.Label();
            this.BtnChooseCoverCalibrator = new System.Windows.Forms.Button();
            this.ChkDriverLoggingCoverCalibrator = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.FilterWheel = new System.Windows.Forms.TabPage();
            this.ChkHardwareLoggingFilterWheel = new System.Windows.Forms.CheckBox();
            this.ChkDriverLoggingFilterWheel = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.TabDevices.SuspendLayout();
            this.General.SuspendLayout();
            this.Camera.SuspendLayout();
            this.CoverCalibrator.SuspendLayout();
            this.FilterWheel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(447, 167);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.CmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(447, 197);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = ((System.Drawing.Image)(resources.GetObject("picASCOM.Image")));
            this.picASCOM.Location = new System.Drawing.Point(452, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // ChkDriverLoggingCamera
            // 
            this.ChkDriverLoggingCamera.AutoSize = true;
            this.ChkDriverLoggingCamera.Location = new System.Drawing.Point(61, 125);
            this.ChkDriverLoggingCamera.Name = "ChkDriverLoggingCamera";
            this.ChkDriverLoggingCamera.Size = new System.Drawing.Size(97, 17);
            this.ChkDriverLoggingCamera.TabIndex = 6;
            this.ChkDriverLoggingCamera.Text = "Log driver calls";
            this.ChkDriverLoggingCamera.UseVisualStyleBackColor = true;
            // 
            // BtnChooseCamera
            // 
            this.BtnChooseCamera.Location = new System.Drawing.Point(215, 79);
            this.BtnChooseCamera.Name = "BtnChooseCamera";
            this.BtnChooseCamera.Size = new System.Drawing.Size(136, 23);
            this.BtnChooseCamera.TabIndex = 7;
            this.BtnChooseCamera.Text = "Choose Camera";
            this.BtnChooseCamera.UseVisualStyleBackColor = true;
            this.BtnChooseCamera.Click += new System.EventHandler(this.BtnChooseCamera_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(51, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select the driver to be hosted ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(66, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Hosted driver:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblCurrentCameraDevice
            // 
            this.LblCurrentCameraDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentCameraDevice.Location = new System.Drawing.Point(164, 43);
            this.LblCurrentCameraDevice.Name = "LblCurrentCameraDevice";
            this.LblCurrentCameraDevice.Size = new System.Drawing.Size(246, 18);
            this.LblCurrentCameraDevice.TabIndex = 10;
            this.LblCurrentCameraDevice.Text = "ASCOM.Simulator.Camera";
            this.LblCurrentCameraDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCurrentFilterWheelDevice
            // 
            this.LblCurrentFilterWheelDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentFilterWheelDevice.Location = new System.Drawing.Point(164, 43);
            this.LblCurrentFilterWheelDevice.Name = "LblCurrentFilterWheelDevice";
            this.LblCurrentFilterWheelDevice.Size = new System.Drawing.Size(246, 18);
            this.LblCurrentFilterWheelDevice.TabIndex = 14;
            this.LblCurrentFilterWheelDevice.Text = "ASCOM.Simulator.FilterWheel";
            this.LblCurrentFilterWheelDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(66, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "Hosted driver:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(48, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 19);
            this.label6.TabIndex = 12;
            this.label6.Text = "Select the driver to be hosted";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnChooseFilterWheel
            // 
            this.BtnChooseFilterWheel.Location = new System.Drawing.Point(215, 79);
            this.BtnChooseFilterWheel.Name = "BtnChooseFilterWheel";
            this.BtnChooseFilterWheel.Size = new System.Drawing.Size(136, 23);
            this.BtnChooseFilterWheel.TabIndex = 11;
            this.BtnChooseFilterWheel.Text = "Choose Filter Wheel";
            this.BtnChooseFilterWheel.UseVisualStyleBackColor = true;
            this.BtnChooseFilterWheel.Click += new System.EventHandler(this.BtnChooseFilterWheel_Click);
            // 
            // TabDevices
            // 
            this.TabDevices.Controls.Add(this.General);
            this.TabDevices.Controls.Add(this.Camera);
            this.TabDevices.Controls.Add(this.CoverCalibrator);
            this.TabDevices.Controls.Add(this.FilterWheel);
            this.TabDevices.Location = new System.Drawing.Point(12, 12);
            this.TabDevices.Name = "TabDevices";
            this.TabDevices.SelectedIndex = 0;
            this.TabDevices.Size = new System.Drawing.Size(426, 212);
            this.TabDevices.TabIndex = 15;
            // 
            // General
            // 
            this.General.BackColor = System.Drawing.Color.LightSkyBlue;
            this.General.Controls.Add(this.ChkLocalServerDebugLog);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Padding = new System.Windows.Forms.Padding(3);
            this.General.Size = new System.Drawing.Size(418, 186);
            this.General.TabIndex = 2;
            this.General.Text = "General";
            // 
            // ChkLocalServerDebugLog
            // 
            this.ChkLocalServerDebugLog.AutoSize = true;
            this.ChkLocalServerDebugLog.Location = new System.Drawing.Point(68, 85);
            this.ChkLocalServerDebugLog.Name = "ChkLocalServerDebugLog";
            this.ChkLocalServerDebugLog.Size = new System.Drawing.Size(283, 17);
            this.ChkLocalServerDebugLog.TabIndex = 18;
            this.ChkLocalServerDebugLog.Text = "Local server debug log (for all devices, requires restart)";
            this.ChkLocalServerDebugLog.UseVisualStyleBackColor = true;
            // 
            // Camera
            // 
            this.Camera.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Camera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Camera.Controls.Add(this.ChkHardwareLoggingCamera);
            this.Camera.Controls.Add(this.LblCurrentCameraDevice);
            this.Camera.Controls.Add(this.BtnChooseCamera);
            this.Camera.Controls.Add(this.ChkDriverLoggingCamera);
            this.Camera.Controls.Add(this.label3);
            this.Camera.Controls.Add(this.label2);
            this.Camera.Location = new System.Drawing.Point(4, 22);
            this.Camera.Name = "Camera";
            this.Camera.Padding = new System.Windows.Forms.Padding(3);
            this.Camera.Size = new System.Drawing.Size(418, 186);
            this.Camera.TabIndex = 0;
            this.Camera.Text = "Camera";
            // 
            // ChkHardwareLoggingCamera
            // 
            this.ChkHardwareLoggingCamera.AutoSize = true;
            this.ChkHardwareLoggingCamera.Location = new System.Drawing.Point(222, 125);
            this.ChkHardwareLoggingCamera.Name = "ChkHardwareLoggingCamera";
            this.ChkHardwareLoggingCamera.Size = new System.Drawing.Size(75, 17);
            this.ChkHardwareLoggingCamera.TabIndex = 16;
            this.ChkHardwareLoggingCamera.Text = "Debug log";
            this.ChkHardwareLoggingCamera.UseVisualStyleBackColor = true;
            // 
            // CoverCalibrator
            // 
            this.CoverCalibrator.BackColor = System.Drawing.Color.LightSkyBlue;
            this.CoverCalibrator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CoverCalibrator.Controls.Add(this.ChkHardwareLoggingCoverCalibrator);
            this.CoverCalibrator.Controls.Add(this.LblCurrentCoverCalibratorDevice);
            this.CoverCalibrator.Controls.Add(this.BtnChooseCoverCalibrator);
            this.CoverCalibrator.Controls.Add(this.ChkDriverLoggingCoverCalibrator);
            this.CoverCalibrator.Controls.Add(this.label4);
            this.CoverCalibrator.Controls.Add(this.label7);
            this.CoverCalibrator.Location = new System.Drawing.Point(4, 22);
            this.CoverCalibrator.Name = "CoverCalibrator";
            this.CoverCalibrator.Padding = new System.Windows.Forms.Padding(3);
            this.CoverCalibrator.Size = new System.Drawing.Size(418, 186);
            this.CoverCalibrator.TabIndex = 3;
            this.CoverCalibrator.Text = "Cover Calibrator";
            // 
            // ChkHardwareLoggingCoverCalibrator
            // 
            this.ChkHardwareLoggingCoverCalibrator.AutoSize = true;
            this.ChkHardwareLoggingCoverCalibrator.Location = new System.Drawing.Point(222, 125);
            this.ChkHardwareLoggingCoverCalibrator.Name = "ChkHardwareLoggingCoverCalibrator";
            this.ChkHardwareLoggingCoverCalibrator.Size = new System.Drawing.Size(75, 17);
            this.ChkHardwareLoggingCoverCalibrator.TabIndex = 22;
            this.ChkHardwareLoggingCoverCalibrator.Text = "Debug log";
            this.ChkHardwareLoggingCoverCalibrator.UseVisualStyleBackColor = true;
            // 
            // LblCurrentCoverCalibratorDevice
            // 
            this.LblCurrentCoverCalibratorDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentCoverCalibratorDevice.Location = new System.Drawing.Point(164, 43);
            this.LblCurrentCoverCalibratorDevice.Name = "LblCurrentCoverCalibratorDevice";
            this.LblCurrentCoverCalibratorDevice.Size = new System.Drawing.Size(247, 18);
            this.LblCurrentCoverCalibratorDevice.TabIndex = 21;
            this.LblCurrentCoverCalibratorDevice.Text = "ASCOM.Simulator.Camera";
            this.LblCurrentCoverCalibratorDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnChooseCoverCalibrator
            // 
            this.BtnChooseCoverCalibrator.Location = new System.Drawing.Point(215, 79);
            this.BtnChooseCoverCalibrator.Name = "BtnChooseCoverCalibrator";
            this.BtnChooseCoverCalibrator.Size = new System.Drawing.Size(136, 23);
            this.BtnChooseCoverCalibrator.TabIndex = 18;
            this.BtnChooseCoverCalibrator.Text = "Choose Cover Calibrator";
            this.BtnChooseCoverCalibrator.UseVisualStyleBackColor = true;
            this.BtnChooseCoverCalibrator.Click += new System.EventHandler(this.BtnChooseCoverCalibrator_Click);
            // 
            // ChkDriverLoggingCoverCalibrator
            // 
            this.ChkDriverLoggingCoverCalibrator.AutoSize = true;
            this.ChkDriverLoggingCoverCalibrator.Location = new System.Drawing.Point(61, 125);
            this.ChkDriverLoggingCoverCalibrator.Name = "ChkDriverLoggingCoverCalibrator";
            this.ChkDriverLoggingCoverCalibrator.Size = new System.Drawing.Size(97, 17);
            this.ChkDriverLoggingCoverCalibrator.TabIndex = 17;
            this.ChkDriverLoggingCoverCalibrator.Text = "Log driver calls";
            this.ChkDriverLoggingCoverCalibrator.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(66, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "Hosted driver:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(51, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 19);
            this.label7.TabIndex = 19;
            this.label7.Text = "Select the driver to be hosted ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FilterWheel
            // 
            this.FilterWheel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.FilterWheel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilterWheel.Controls.Add(this.ChkHardwareLoggingFilterWheel);
            this.FilterWheel.Controls.Add(this.ChkDriverLoggingFilterWheel);
            this.FilterWheel.Controls.Add(this.LblCurrentFilterWheelDevice);
            this.FilterWheel.Controls.Add(this.BtnChooseFilterWheel);
            this.FilterWheel.Controls.Add(this.label5);
            this.FilterWheel.Controls.Add(this.label6);
            this.FilterWheel.Location = new System.Drawing.Point(4, 22);
            this.FilterWheel.Name = "FilterWheel";
            this.FilterWheel.Padding = new System.Windows.Forms.Padding(3);
            this.FilterWheel.Size = new System.Drawing.Size(418, 186);
            this.FilterWheel.TabIndex = 1;
            this.FilterWheel.Text = "Filter Wheel";
            // 
            // ChkHardwareLoggingFilterWheel
            // 
            this.ChkHardwareLoggingFilterWheel.AutoSize = true;
            this.ChkHardwareLoggingFilterWheel.Location = new System.Drawing.Point(222, 125);
            this.ChkHardwareLoggingFilterWheel.Name = "ChkHardwareLoggingFilterWheel";
            this.ChkHardwareLoggingFilterWheel.Size = new System.Drawing.Size(75, 17);
            this.ChkHardwareLoggingFilterWheel.TabIndex = 18;
            this.ChkHardwareLoggingFilterWheel.Text = "Debug log";
            this.ChkHardwareLoggingFilterWheel.UseVisualStyleBackColor = true;
            // 
            // ChkDriverLoggingFilterWheel
            // 
            this.ChkDriverLoggingFilterWheel.AutoSize = true;
            this.ChkDriverLoggingFilterWheel.Location = new System.Drawing.Point(61, 125);
            this.ChkDriverLoggingFilterWheel.Name = "ChkDriverLoggingFilterWheel";
            this.ChkDriverLoggingFilterWheel.Size = new System.Drawing.Size(97, 17);
            this.ChkDriverLoggingFilterWheel.TabIndex = 17;
            this.ChkDriverLoggingFilterWheel.Text = "Log driver calls";
            this.ChkDriverLoggingFilterWheel.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 234);
            this.Controls.Add(this.TabDevices);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JustAHub Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.TabDevices.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.General.PerformLayout();
            this.Camera.ResumeLayout(false);
            this.Camera.PerformLayout();
            this.CoverCalibrator.ResumeLayout(false);
            this.CoverCalibrator.PerformLayout();
            this.FilterWheel.ResumeLayout(false);
            this.FilterWheel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox ChkDriverLoggingCamera;
        private System.Windows.Forms.Button BtnChooseCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblCurrentCameraDevice;
        private System.Windows.Forms.Label LblCurrentFilterWheelDevice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnChooseFilterWheel;
        private System.Windows.Forms.TabControl TabDevices;
        private System.Windows.Forms.TabPage Camera;
        private System.Windows.Forms.TabPage FilterWheel;
        private System.Windows.Forms.CheckBox ChkHardwareLoggingCamera;
        private System.Windows.Forms.CheckBox ChkHardwareLoggingFilterWheel;
        private System.Windows.Forms.CheckBox ChkDriverLoggingFilterWheel;
        private System.Windows.Forms.TabPage General;
        private System.Windows.Forms.CheckBox ChkLocalServerDebugLog;
        private System.Windows.Forms.TabPage CoverCalibrator;
        private System.Windows.Forms.CheckBox ChkHardwareLoggingCoverCalibrator;
        private System.Windows.Forms.Label LblCurrentCoverCalibratorDevice;
        private System.Windows.Forms.Button BtnChooseCoverCalibrator;
        private System.Windows.Forms.CheckBox ChkDriverLoggingCoverCalibrator;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
    }
}