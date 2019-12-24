namespace ASCOM.Remote
{
    partial class ServerConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigurationForm));
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.ServedDevice0 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice1 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice2 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice3 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice4 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice5 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice6 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice7 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice8 = new ASCOM.Remote.ServedDeviceClient();
            this.ServedDevice9 = new ASCOM.Remote.ServedDeviceClient();
            this.BtnGetRemoteConfiguration = new System.Windows.Forms.Button();
            this.BtnSetRemoteConfiguration = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnReloadConfiguration = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(731, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "True";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(666, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "False";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(649, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Allow Connected to be set";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(192, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Device Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(431, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Device";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Device Type";
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(709, 438);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 47;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(709, 467);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 46;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // ServedDevice0
            // 
            this.ServedDevice0.AllowConnectedSetFalse = false;
            this.ServedDevice0.AllowConnectedSetTrue = false;
            this.ServedDevice0.Description = "";
            this.ServedDevice0.DeviceNumber = 0;
            this.ServedDevice0.DeviceType = "None";
            this.ServedDevice0.Location = new System.Drawing.Point(12, 59);
            this.ServedDevice0.Name = "ServedDevice0";
            this.ServedDevice0.ProgID = "";
            this.ServedDevice0.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice0.TabIndex = 36;
            // 
            // ServedDevice1
            // 
            this.ServedDevice1.AllowConnectedSetFalse = false;
            this.ServedDevice1.AllowConnectedSetTrue = false;
            this.ServedDevice1.Description = "";
            this.ServedDevice1.DeviceNumber = 0;
            this.ServedDevice1.DeviceType = "None";
            this.ServedDevice1.Location = new System.Drawing.Point(12, 87);
            this.ServedDevice1.Name = "ServedDevice1";
            this.ServedDevice1.ProgID = "";
            this.ServedDevice1.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice1.TabIndex = 37;
            // 
            // ServedDevice2
            // 
            this.ServedDevice2.AllowConnectedSetFalse = false;
            this.ServedDevice2.AllowConnectedSetTrue = false;
            this.ServedDevice2.Description = "";
            this.ServedDevice2.DeviceNumber = 0;
            this.ServedDevice2.DeviceType = "None";
            this.ServedDevice2.Location = new System.Drawing.Point(12, 115);
            this.ServedDevice2.Name = "ServedDevice2";
            this.ServedDevice2.ProgID = "";
            this.ServedDevice2.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice2.TabIndex = 38;
            // 
            // ServedDevice3
            // 
            this.ServedDevice3.AllowConnectedSetFalse = false;
            this.ServedDevice3.AllowConnectedSetTrue = false;
            this.ServedDevice3.Description = "";
            this.ServedDevice3.DeviceNumber = 0;
            this.ServedDevice3.DeviceType = "None";
            this.ServedDevice3.Location = new System.Drawing.Point(12, 143);
            this.ServedDevice3.Name = "ServedDevice3";
            this.ServedDevice3.ProgID = "";
            this.ServedDevice3.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice3.TabIndex = 39;
            // 
            // ServedDevice4
            // 
            this.ServedDevice4.AllowConnectedSetFalse = false;
            this.ServedDevice4.AllowConnectedSetTrue = false;
            this.ServedDevice4.Description = "";
            this.ServedDevice4.DeviceNumber = 0;
            this.ServedDevice4.DeviceType = "None";
            this.ServedDevice4.Location = new System.Drawing.Point(12, 171);
            this.ServedDevice4.Name = "ServedDevice4";
            this.ServedDevice4.ProgID = "";
            this.ServedDevice4.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice4.TabIndex = 40;
            // 
            // ServedDevice5
            // 
            this.ServedDevice5.AllowConnectedSetFalse = false;
            this.ServedDevice5.AllowConnectedSetTrue = false;
            this.ServedDevice5.Description = "";
            this.ServedDevice5.DeviceNumber = 0;
            this.ServedDevice5.DeviceType = "None";
            this.ServedDevice5.Location = new System.Drawing.Point(12, 199);
            this.ServedDevice5.Name = "ServedDevice5";
            this.ServedDevice5.ProgID = "";
            this.ServedDevice5.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice5.TabIndex = 41;
            // 
            // ServedDevice6
            // 
            this.ServedDevice6.AllowConnectedSetFalse = false;
            this.ServedDevice6.AllowConnectedSetTrue = false;
            this.ServedDevice6.Description = "";
            this.ServedDevice6.DeviceNumber = 0;
            this.ServedDevice6.DeviceType = "None";
            this.ServedDevice6.Location = new System.Drawing.Point(12, 227);
            this.ServedDevice6.Name = "ServedDevice6";
            this.ServedDevice6.ProgID = "";
            this.ServedDevice6.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice6.TabIndex = 42;
            // 
            // ServedDevice7
            // 
            this.ServedDevice7.AllowConnectedSetFalse = false;
            this.ServedDevice7.AllowConnectedSetTrue = false;
            this.ServedDevice7.Description = "";
            this.ServedDevice7.DeviceNumber = 0;
            this.ServedDevice7.DeviceType = "None";
            this.ServedDevice7.Location = new System.Drawing.Point(12, 255);
            this.ServedDevice7.Name = "ServedDevice7";
            this.ServedDevice7.ProgID = "";
            this.ServedDevice7.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice7.TabIndex = 43;
            // 
            // ServedDevice8
            // 
            this.ServedDevice8.AllowConnectedSetFalse = false;
            this.ServedDevice8.AllowConnectedSetTrue = false;
            this.ServedDevice8.Description = "";
            this.ServedDevice8.DeviceNumber = 0;
            this.ServedDevice8.DeviceType = "None";
            this.ServedDevice8.Location = new System.Drawing.Point(12, 283);
            this.ServedDevice8.Name = "ServedDevice8";
            this.ServedDevice8.ProgID = "";
            this.ServedDevice8.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice8.TabIndex = 44;
            // 
            // ServedDevice9
            // 
            this.ServedDevice9.AllowConnectedSetFalse = false;
            this.ServedDevice9.AllowConnectedSetTrue = false;
            this.ServedDevice9.Description = "";
            this.ServedDevice9.DeviceNumber = 0;
            this.ServedDevice9.DeviceType = "None";
            this.ServedDevice9.Location = new System.Drawing.Point(12, 311);
            this.ServedDevice9.Name = "ServedDevice9";
            this.ServedDevice9.ProgID = "";
            this.ServedDevice9.Size = new System.Drawing.Size(746, 22);
            this.ServedDevice9.TabIndex = 45;
            // 
            // BtnGetRemoteConfiguration
            // 
            this.BtnGetRemoteConfiguration.Location = new System.Drawing.Point(117, 453);
            this.BtnGetRemoteConfiguration.Name = "BtnGetRemoteConfiguration";
            this.BtnGetRemoteConfiguration.Size = new System.Drawing.Size(75, 23);
            this.BtnGetRemoteConfiguration.TabIndex = 48;
            this.BtnGetRemoteConfiguration.Text = "Get";
            this.BtnGetRemoteConfiguration.UseVisualStyleBackColor = true;
            this.BtnGetRemoteConfiguration.Click += new System.EventHandler(this.BtnGetRemoteConfiguration_Click);
            // 
            // BtnSetRemoteConfiguration
            // 
            this.BtnSetRemoteConfiguration.Location = new System.Drawing.Point(198, 453);
            this.BtnSetRemoteConfiguration.Name = "BtnSetRemoteConfiguration";
            this.BtnSetRemoteConfiguration.Size = new System.Drawing.Size(75, 23);
            this.BtnSetRemoteConfiguration.TabIndex = 49;
            this.BtnSetRemoteConfiguration.Text = "Set Configuration";
            this.BtnSetRemoteConfiguration.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(142, 427);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Remote Configuration";
            // 
            // BtnReloadConfiguration
            // 
            this.BtnReloadConfiguration.Location = new System.Drawing.Point(434, 438);
            this.BtnReloadConfiguration.Name = "BtnReloadConfiguration";
            this.BtnReloadConfiguration.Size = new System.Drawing.Size(88, 38);
            this.BtnReloadConfiguration.TabIndex = 51;
            this.BtnReloadConfiguration.Text = "Reload Configuration";
            this.BtnReloadConfiguration.UseVisualStyleBackColor = true;
            this.BtnReloadConfiguration.Click += new System.EventHandler(this.BtnReloadConfiguration_Click);
            // 
            // ServerConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 502);
            this.Controls.Add(this.BtnReloadConfiguration);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BtnSetRemoteConfiguration);
            this.Controls.Add(this.BtnGetRemoteConfiguration);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.ServedDevice9);
            this.Controls.Add(this.ServedDevice8);
            this.Controls.Add(this.ServedDevice7);
            this.Controls.Add(this.ServedDevice6);
            this.Controls.Add(this.ServedDevice5);
            this.Controls.Add(this.ServedDevice4);
            this.Controls.Add(this.ServedDevice3);
            this.Controls.Add(this.ServedDevice2);
            this.Controls.Add(this.ServedDevice1);
            this.Controls.Add(this.ServedDevice0);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerConfigurationForm";
            this.Text = "ServerConfigurationForm";
            this.Load += new System.EventHandler(this.ServerConfigurationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private ServedDeviceClient ServedDevice0;
        private ServedDeviceClient ServedDevice1;
        private ServedDeviceClient ServedDevice2;
        private ServedDeviceClient ServedDevice3;
        private ServedDeviceClient ServedDevice4;
        private ServedDeviceClient ServedDevice5;
        private ServedDeviceClient ServedDevice6;
        private ServedDeviceClient ServedDevice7;
        private ServedDeviceClient ServedDevice8;
        private ServedDeviceClient ServedDevice9;
        private System.Windows.Forms.Button BtnGetRemoteConfiguration;
        private System.Windows.Forms.Button BtnSetRemoteConfiguration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnReloadConfiguration;
    }
}