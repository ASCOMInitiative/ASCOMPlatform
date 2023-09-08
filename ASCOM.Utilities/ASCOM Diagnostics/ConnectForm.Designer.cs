using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ConnectForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectForm));
            cmbDeviceType = new ComboBox();
            cmbDeviceType.MouseClick += new MouseEventHandler(cmbDeviceType_Click);
            btnChoose = new Button();
            btnChoose.Click += new EventHandler(btnChoose_Click);
            txtDevice = new TextBox();
            txtStatus = new TextBox();
            btnConnect = new Button();
            btnConnect.Click += new EventHandler(btnConnect_Click);
            btnProperties = new Button();
            btnProperties.Click += new EventHandler(btnProperties_Click);
            btnScript = new Button();
            btnScript.Click += new EventHandler(btnScript_Click);
            Label1 = new Label();
            btnGetProfile = new Button();
            btnGetProfile.Click += new EventHandler(btnGetProfile_Click);
            Label2 = new Label();
            SuspendLayout();
            // 
            // cmbDeviceType
            // 
            cmbDeviceType.FormattingEnabled = true;
            cmbDeviceType.Location = new Point(12, 29);
            cmbDeviceType.Name = "cmbDeviceType";
            cmbDeviceType.Size = new Size(246, 21);
            cmbDeviceType.TabIndex = 0;
            // 
            // btnChoose
            // 
            btnChoose.Location = new Point(275, 27);
            btnChoose.Name = "btnChoose";
            btnChoose.Size = new Size(75, 23);
            btnChoose.TabIndex = 1;
            btnChoose.Text = "Choose";
            btnChoose.UseVisualStyleBackColor = true;
            // 
            // txtDevice
            // 
            txtDevice.Enabled = false;
            txtDevice.Location = new Point(12, 74);
            txtDevice.Name = "txtDevice";
            txtDevice.Size = new Size(246, 20);
            txtDevice.TabIndex = 2;
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(12, 130);
            txtStatus.Multiline = true;
            txtStatus.Name = "txtStatus";
            txtStatus.ScrollBars = ScrollBars.Vertical;
            txtStatus.Size = new Size(770, 245);
            txtStatus.TabIndex = 3;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(275, 72);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnProperties
            // 
            btnProperties.Location = new Point(356, 72);
            btnProperties.Name = "btnProperties";
            btnProperties.Size = new Size(75, 23);
            btnProperties.TabIndex = 5;
            btnProperties.Text = "Properties";
            btnProperties.UseVisualStyleBackColor = true;
            // 
            // btnScript
            // 
            btnScript.Location = new Point(438, 72);
            btnScript.Name = "btnScript";
            btnScript.Size = new Size(75, 23);
            btnScript.TabIndex = 6;
            btnScript.Text = "Run Script";
            btnScript.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(519, 77);
            Label1.Name = "Label1";
            Label1.Size = new Size(136, 13);
            Label1.TabIndex = 7;
            Label1.Text = "(Script is for telescope only)";
            // 
            // btnGetProfile
            // 
            btnGetProfile.Location = new Point(438, 101);
            btnGetProfile.Name = "btnGetProfile";
            btnGetProfile.Size = new Size(75, 23);
            btnGetProfile.TabIndex = 8;
            btnGetProfile.Text = "GetProfile";
            btnGetProfile.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 13);
            Label2.Name = "Label2";
            Label2.Size = new Size(101, 13);
            Label2.TabIndex = 9;
            Label2.Text = "Select Device Type";
            Label2.TextAlign = ContentAlignment.TopRight;
            // 
            // ConnectForm
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(794, 442);
            Controls.Add(Label2);
            Controls.Add(btnGetProfile);
            Controls.Add(Label1);
            Controls.Add(btnScript);
            Controls.Add(btnProperties);
            Controls.Add(btnConnect);
            Controls.Add(txtStatus);
            Controls.Add(txtDevice);
            Controls.Add(btnChoose);
            Controls.Add(cmbDeviceType);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ConnectForm";
            Text = "Diagnostics Connect";
            Load += new EventHandler(ConnectForm_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal ComboBox cmbDeviceType;
        internal Button btnChoose;
        internal TextBox txtDevice;
        internal TextBox txtStatus;
        internal Button btnConnect;
        internal Button btnProperties;
        internal Button btnScript;
        internal Label Label1;
        internal Button btnGetProfile;
        internal Label Label2;
    }
}