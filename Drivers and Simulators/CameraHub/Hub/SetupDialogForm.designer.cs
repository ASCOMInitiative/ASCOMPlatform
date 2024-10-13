namespace ASCOM.CameraHub
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
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.BtnChooseCamera = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblCurrentCameraDevice = new System.Windows.Forms.Label();
            this.LblCurrentFilterWheelDevice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnChooseFilterWheel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(478, 268);
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
            this.cmdCancel.Location = new System.Drawing.Point(478, 298);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(47, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(458, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "NOTE - Use the Chooser\'s Properties button to change the served device configurat" +
    "ion";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = ((System.Drawing.Image)(resources.GetObject("picASCOM.Image")));
            this.picASCOM.Location = new System.Drawing.Point(489, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(245, 273);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // BtnChooseCamera
            // 
            this.BtnChooseCamera.Location = new System.Drawing.Point(338, 108);
            this.BtnChooseCamera.Name = "BtnChooseCamera";
            this.BtnChooseCamera.Size = new System.Drawing.Size(122, 23);
            this.BtnChooseCamera.TabIndex = 7;
            this.BtnChooseCamera.Text = "Choose Camera";
            this.BtnChooseCamera.UseVisualStyleBackColor = true;
            this.BtnChooseCamera.Click += new System.EventHandler(this.BtnChooseCamera_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(78, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(244, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select the camera to be served using the Chooser";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(78, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Current camera device:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCurrentCameraDevice
            // 
            this.LblCurrentCameraDevice.Location = new System.Drawing.Point(164, 94);
            this.LblCurrentCameraDevice.Name = "LblCurrentCameraDevice";
            this.LblCurrentCameraDevice.Size = new System.Drawing.Size(244, 15);
            this.LblCurrentCameraDevice.TabIndex = 10;
            this.LblCurrentCameraDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCurrentFilterWheelDevice
            // 
            this.LblCurrentFilterWheelDevice.Location = new System.Drawing.Point(185, 153);
            this.LblCurrentFilterWheelDevice.Name = "LblCurrentFilterWheelDevice";
            this.LblCurrentFilterWheelDevice.Size = new System.Drawing.Size(244, 15);
            this.LblCurrentFilterWheelDevice.TabIndex = 14;
            this.LblCurrentFilterWheelDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(78, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "Current filter wheel";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(64, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(268, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Select the filter wheel to be served using the Chooser";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnChooseFilterWheel
            // 
            this.BtnChooseFilterWheel.Location = new System.Drawing.Point(338, 164);
            this.BtnChooseFilterWheel.Name = "BtnChooseFilterWheel";
            this.BtnChooseFilterWheel.Size = new System.Drawing.Size(122, 23);
            this.BtnChooseFilterWheel.TabIndex = 11;
            this.BtnChooseFilterWheel.Text = "Choose Filter Wheel";
            this.BtnChooseFilterWheel.UseVisualStyleBackColor = true;
            this.BtnChooseFilterWheel.Click += new System.EventHandler(this.BtnChooseFilterWheel_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 331);
            this.Controls.Add(this.LblCurrentFilterWheelDevice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BtnChooseFilterWheel);
            this.Controls.Add(this.LblCurrentCameraDevice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnChooseCamera);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Hub Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Button BtnChooseCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblCurrentCameraDevice;
        private System.Windows.Forms.Label LblCurrentFilterWheelDevice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnChooseFilterWheel;
    }
}