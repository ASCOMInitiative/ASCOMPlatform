namespace ASCOM.DriverConnect
{
    partial class ConnectForm
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
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGetProfile = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.chkConnect = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Location = new System.Drawing.Point(12, 28);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(247, 21);
            this.cmbDeviceType.TabIndex = 0;
            this.cmbDeviceType.Click += new System.EventHandler(this.cmbDeviceType_Click);
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(12, 73);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(247, 20);
            this.txtDevice.TabIndex = 1;
            // 
            // txtStatus
            // 
            this.txtStatus.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(12, 127);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(855, 579);
            this.txtStatus.TabIndex = 2;
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(273, 27);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(80, 21);
            this.btnChoose.TabIndex = 3;
            this.btnChoose.Text = "Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(359, 27);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(80, 21);
            this.btnProperties.TabIndex = 5;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(273, 72);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 21);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnGetProfile
            // 
            this.btnGetProfile.Location = new System.Drawing.Point(359, 72);
            this.btnGetProfile.Name = "btnGetProfile";
            this.btnGetProfile.Size = new System.Drawing.Size(80, 21);
            this.btnGetProfile.TabIndex = 7;
            this.btnGetProfile.Text = "Get Profile";
            this.btnGetProfile.UseVisualStyleBackColor = true;
            this.btnGetProfile.Click += new System.EventHandler(this.btnGetProfile_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(9, 12);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(101, 13);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "Select Device Type";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkConnect
            // 
            this.chkConnect.AutoSize = true;
            this.chkConnect.Location = new System.Drawing.Point(445, 30);
            this.chkConnect.Name = "chkConnect";
            this.chkConnect.Size = new System.Drawing.Size(165, 17);
            this.chkConnect.TabIndex = 11;
            this.chkConnect.Text = "Connect to retrieve properties";
            this.chkConnect.UseVisualStyleBackColor = true;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 718);
            this.Controls.Add(this.chkConnect);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.btnGetProfile);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.cmbDeviceType);
            this.Name = "ConnectForm";
            this.Text = "ConnectForm";
            this.Load += new System.EventHandler(this.ConnectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnGetProfile;
        internal System.Windows.Forms.Label Label2;
        private System.Windows.Forms.CheckBox chkConnect;
    }
}