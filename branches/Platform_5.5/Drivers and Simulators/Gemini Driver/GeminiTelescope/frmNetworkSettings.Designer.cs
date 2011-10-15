namespace ASCOM.GeminiTelescope
{
    partial class frmNetworkSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNetworkSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.rbUDP = new System.Windows.Forms.RadioButton();
            this.rbHTTP = new System.Windows.Forms.RadioButton();
            this.chkDHCP = new System.Windows.Forms.CheckBox();
            this.chkNoProxy = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.rbUDP);
            this.groupBox1.Controls.Add(this.rbHTTP);
            this.groupBox1.Controls.Add(this.chkDHCP);
            this.groupBox1.Controls.Add(this.chkNoProxy);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtPort
            // 
            this.txtPort.AccessibleDescription = null;
            this.txtPort.AccessibleName = null;
            resources.ApplyResources(this.txtPort, "txtPort");
            this.txtPort.BackgroundImage = null;
            this.txtPort.Font = null;
            this.txtPort.Name = "txtPort";
            // 
            // rbUDP
            // 
            this.rbUDP.AccessibleDescription = null;
            this.rbUDP.AccessibleName = null;
            resources.ApplyResources(this.rbUDP, "rbUDP");
            this.rbUDP.BackgroundImage = null;
            this.rbUDP.Font = null;
            this.rbUDP.Name = "rbUDP";
            this.rbUDP.TabStop = true;
            this.rbUDP.UseVisualStyleBackColor = true;
            // 
            // rbHTTP
            // 
            this.rbHTTP.AccessibleDescription = null;
            this.rbHTTP.AccessibleName = null;
            resources.ApplyResources(this.rbHTTP, "rbHTTP");
            this.rbHTTP.BackgroundImage = null;
            this.rbHTTP.Font = null;
            this.rbHTTP.Name = "rbHTTP";
            this.rbHTTP.TabStop = true;
            this.rbHTTP.UseVisualStyleBackColor = true;
            this.rbHTTP.CheckedChanged += new System.EventHandler(this.rbHTTP_CheckedChanged);
            // 
            // chkDHCP
            // 
            this.chkDHCP.AccessibleDescription = null;
            this.chkDHCP.AccessibleName = null;
            resources.ApplyResources(this.chkDHCP, "chkDHCP");
            this.chkDHCP.BackgroundImage = null;
            this.chkDHCP.Font = null;
            this.chkDHCP.Name = "chkDHCP";
            this.chkDHCP.UseVisualStyleBackColor = true;
            this.chkDHCP.CheckedChanged += new System.EventHandler(this.chkDHCP_CheckedChanged);
            // 
            // chkNoProxy
            // 
            this.chkNoProxy.AccessibleDescription = null;
            this.chkNoProxy.AccessibleName = null;
            resources.ApplyResources(this.chkNoProxy, "chkNoProxy");
            this.chkNoProxy.BackgroundImage = null;
            this.chkNoProxy.Font = null;
            this.chkNoProxy.Name = "chkNoProxy";
            this.chkNoProxy.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.AccessibleDescription = null;
            this.txtPassword.AccessibleName = null;
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.BackgroundImage = null;
            this.txtPassword.Font = null;
            this.txtPassword.Name = "txtPassword";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // txtUser
            // 
            this.txtUser.AccessibleDescription = null;
            this.txtUser.AccessibleName = null;
            resources.ApplyResources(this.txtUser, "txtUser");
            this.txtUser.BackgroundImage = null;
            this.txtUser.Font = null;
            this.txtUser.Name = "txtUser";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // txtIP
            // 
            this.txtIP.AccessibleDescription = null;
            this.txtIP.AccessibleName = null;
            resources.ApplyResources(this.txtIP, "txtIP");
            this.txtIP.BackgroundImage = null;
            this.txtIP.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.txtIP.Name = "txtIP";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.BackgroundImage = null;
            this.btnOK.Font = null;
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmNetworkSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = null;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.groupBox1);
            this.Font = null;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNetworkSettings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.MaskedTextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNoProxy;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDHCP;
        private System.Windows.Forms.TextBox txtPort;
        public System.Windows.Forms.RadioButton rbHTTP;
        public System.Windows.Forms.RadioButton rbUDP;
    }
}