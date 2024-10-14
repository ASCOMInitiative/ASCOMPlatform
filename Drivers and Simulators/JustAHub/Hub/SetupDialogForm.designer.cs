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
            this.ChkLogDriverCallsCamera = new System.Windows.Forms.CheckBox();
            this.BtnChooseCamera = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblCurrentCameraDevice = new System.Windows.Forms.Label();
            this.LblCurrentFilterWheelDevice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnChooseFilterWheel = new System.Windows.Forms.Button();
            this.TabDevices = new System.Windows.Forms.TabControl();
            this.Camera = new System.Windows.Forms.TabPage();
            this.FilterWheel = new System.Windows.Forms.TabPage();
            this.ChkDebugLoggingCamera = new System.Windows.Forms.CheckBox();
            this.ChkDebugLoggingFilterWheel = new System.Windows.Forms.CheckBox();
            this.ChkLogDriverCallsFilterWheel = new System.Windows.Forms.CheckBox();
            this.ChkLocalServerDebugLog = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.TabDevices.SuspendLayout();
            this.Camera.SuspendLayout();
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
            // ChkLogDriverCallsCamera
            // 
            this.ChkLogDriverCallsCamera.AutoSize = true;
            this.ChkLogDriverCallsCamera.Location = new System.Drawing.Point(61, 125);
            this.ChkLogDriverCallsCamera.Name = "ChkLogDriverCallsCamera";
            this.ChkLogDriverCallsCamera.Size = new System.Drawing.Size(97, 17);
            this.ChkLogDriverCallsCamera.TabIndex = 6;
            this.ChkLogDriverCallsCamera.Text = "Log driver calls";
            this.ChkLogDriverCallsCamera.UseVisualStyleBackColor = true;
            // 
            // BtnChooseCamera
            // 
            this.BtnChooseCamera.Location = new System.Drawing.Point(222, 79);
            this.BtnChooseCamera.Name = "BtnChooseCamera";
            this.BtnChooseCamera.Size = new System.Drawing.Size(122, 23);
            this.BtnChooseCamera.TabIndex = 7;
            this.BtnChooseCamera.Text = "Choose Camera";
            this.BtnChooseCamera.UseVisualStyleBackColor = true;
            this.BtnChooseCamera.Click += new System.EventHandler(this.BtnChooseCamera_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(58, 81);
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
            this.LblCurrentCameraDevice.Size = new System.Drawing.Size(171, 18);
            this.LblCurrentCameraDevice.TabIndex = 10;
            this.LblCurrentCameraDevice.Text = "ASCOM.Simulator.Camera";
            this.LblCurrentCameraDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCurrentFilterWheelDevice
            // 
            this.LblCurrentFilterWheelDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentFilterWheelDevice.Location = new System.Drawing.Point(164, 43);
            this.LblCurrentFilterWheelDevice.Name = "LblCurrentFilterWheelDevice";
            this.LblCurrentFilterWheelDevice.Size = new System.Drawing.Size(197, 18);
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
            this.label6.Location = new System.Drawing.Point(55, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 19);
            this.label6.TabIndex = 12;
            this.label6.Text = "Select the driver to be hosted";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnChooseFilterWheel
            // 
            this.BtnChooseFilterWheel.Location = new System.Drawing.Point(222, 79);
            this.BtnChooseFilterWheel.Name = "BtnChooseFilterWheel";
            this.BtnChooseFilterWheel.Size = new System.Drawing.Size(122, 23);
            this.BtnChooseFilterWheel.TabIndex = 11;
            this.BtnChooseFilterWheel.Text = "Choose Filter Wheel";
            this.BtnChooseFilterWheel.UseVisualStyleBackColor = true;
            this.BtnChooseFilterWheel.Click += new System.EventHandler(this.BtnChooseFilterWheel_Click);
            // 
            // TabDevices
            // 
            this.TabDevices.Controls.Add(this.Camera);
            this.TabDevices.Controls.Add(this.FilterWheel);
            this.TabDevices.Location = new System.Drawing.Point(12, 12);
            this.TabDevices.Name = "TabDevices";
            this.TabDevices.SelectedIndex = 0;
            this.TabDevices.Size = new System.Drawing.Size(426, 212);
            this.TabDevices.TabIndex = 15;
            // 
            // Camera
            // 
            this.Camera.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Camera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Camera.Controls.Add(this.ChkLocalServerDebugLog);
            this.Camera.Controls.Add(this.ChkDebugLoggingCamera);
            this.Camera.Controls.Add(this.LblCurrentCameraDevice);
            this.Camera.Controls.Add(this.BtnChooseCamera);
            this.Camera.Controls.Add(this.ChkLogDriverCallsCamera);
            this.Camera.Controls.Add(this.label3);
            this.Camera.Controls.Add(this.label2);
            this.Camera.Location = new System.Drawing.Point(4, 22);
            this.Camera.Name = "Camera";
            this.Camera.Padding = new System.Windows.Forms.Padding(3);
            this.Camera.Size = new System.Drawing.Size(418, 186);
            this.Camera.TabIndex = 0;
            this.Camera.Text = "Camera";
            // 
            // FilterWheel
            // 
            this.FilterWheel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.FilterWheel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilterWheel.Controls.Add(this.ChkDebugLoggingFilterWheel);
            this.FilterWheel.Controls.Add(this.ChkLogDriverCallsFilterWheel);
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
            // ChkDebugLoggingCamera
            // 
            this.ChkDebugLoggingCamera.AutoSize = true;
            this.ChkDebugLoggingCamera.Location = new System.Drawing.Point(222, 125);
            this.ChkDebugLoggingCamera.Name = "ChkDebugLoggingCamera";
            this.ChkDebugLoggingCamera.Size = new System.Drawing.Size(75, 17);
            this.ChkDebugLoggingCamera.TabIndex = 16;
            this.ChkDebugLoggingCamera.Text = "Debug log";
            this.ChkDebugLoggingCamera.UseVisualStyleBackColor = true;
            // 
            // ChkDebugLoggingFilterWheel
            // 
            this.ChkDebugLoggingFilterWheel.AutoSize = true;
            this.ChkDebugLoggingFilterWheel.Location = new System.Drawing.Point(222, 125);
            this.ChkDebugLoggingFilterWheel.Name = "ChkDebugLoggingFilterWheel";
            this.ChkDebugLoggingFilterWheel.Size = new System.Drawing.Size(75, 17);
            this.ChkDebugLoggingFilterWheel.TabIndex = 18;
            this.ChkDebugLoggingFilterWheel.Text = "Debug log";
            this.ChkDebugLoggingFilterWheel.UseVisualStyleBackColor = true;
            // 
            // ChkLogDriverCallsFilterWheel
            // 
            this.ChkLogDriverCallsFilterWheel.AutoSize = true;
            this.ChkLogDriverCallsFilterWheel.Location = new System.Drawing.Point(61, 125);
            this.ChkLogDriverCallsFilterWheel.Name = "ChkLogDriverCallsFilterWheel";
            this.ChkLogDriverCallsFilterWheel.Size = new System.Drawing.Size(97, 17);
            this.ChkLogDriverCallsFilterWheel.TabIndex = 17;
            this.ChkLogDriverCallsFilterWheel.Text = "Log driver calls";
            this.ChkLogDriverCallsFilterWheel.UseVisualStyleBackColor = true;
            // 
            // ChkLocalServerDebugLog
            // 
            this.ChkLocalServerDebugLog.AutoSize = true;
            this.ChkLocalServerDebugLog.Location = new System.Drawing.Point(61, 148);
            this.ChkLocalServerDebugLog.Name = "ChkLocalServerDebugLog";
            this.ChkLocalServerDebugLog.Size = new System.Drawing.Size(283, 17);
            this.ChkLocalServerDebugLog.TabIndex = 17;
            this.ChkLocalServerDebugLog.Text = "Local server debug log (for all devices, requires restart)";
            this.ChkLocalServerDebugLog.UseVisualStyleBackColor = true;
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
            this.Camera.ResumeLayout(false);
            this.Camera.PerformLayout();
            this.FilterWheel.ResumeLayout(false);
            this.FilterWheel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox ChkLogDriverCallsCamera;
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
        private System.Windows.Forms.CheckBox ChkDebugLoggingCamera;
        private System.Windows.Forms.CheckBox ChkDebugLoggingFilterWheel;
        private System.Windows.Forms.CheckBox ChkLogDriverCallsFilterWheel;
        private System.Windows.Forms.CheckBox ChkLocalServerDebugLog;
    }
}