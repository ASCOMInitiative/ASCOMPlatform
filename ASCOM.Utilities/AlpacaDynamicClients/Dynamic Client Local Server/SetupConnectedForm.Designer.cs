namespace ASCOM.DynamicClients
{
    partial class SetupConnectedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupConnectedForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnSetupUrlDevice = new System.Windows.Forms.Button();
            this.BtnSetupUrlMain = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDebugTrace = new System.Windows.Forms.CheckBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnSetupUrlDevice);
            this.groupBox1.Controls.Add(this.BtnSetupUrlMain);
            this.groupBox1.Location = new System.Drawing.Point(185, 143);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 62);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Web Configuration Pages";
            // 
            // BtnSetupUrlDevice
            // 
            this.BtnSetupUrlDevice.Location = new System.Drawing.Point(144, 20);
            this.BtnSetupUrlDevice.Name = "BtnSetupUrlDevice";
            this.BtnSetupUrlDevice.Size = new System.Drawing.Size(122, 31);
            this.BtnSetupUrlDevice.TabIndex = 1;
            this.BtnSetupUrlDevice.Text = "This Specific Device";
            this.BtnSetupUrlDevice.UseVisualStyleBackColor = true;
            this.BtnSetupUrlDevice.Click += new System.EventHandler(this.BtnSetupUrlDevice_Click);
            // 
            // BtnSetupUrlMain
            // 
            this.BtnSetupUrlMain.Location = new System.Drawing.Point(8, 19);
            this.BtnSetupUrlMain.Name = "BtnSetupUrlMain";
            this.BtnSetupUrlMain.Size = new System.Drawing.Size(130, 32);
            this.BtnSetupUrlMain.TabIndex = 0;
            this.BtnSetupUrlMain.Text = "Overall Alpaca Device";
            this.BtnSetupUrlMain.UseVisualStyleBackColor = true;
            this.BtnSetupUrlMain.Click += new System.EventHandler(this.BtnSetupUrlMain_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.AutoSize = true;
            this.BtnOK.Location = new System.Drawing.Point(542, 263);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 57;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(606, 20);
            this.label1.TabIndex = 60;
            this.label1.Text = "This Dynamic Client is connected and its configuration cannot be changed.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(43, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(542, 15);
            this.label2.TabIndex = 61;
            this.label2.Text = "It may be possible to configure the Alpaca device using its web configuration pag" +
    "es.";
            // 
            // chkDebugTrace
            // 
            this.chkDebugTrace.AutoSize = true;
            this.chkDebugTrace.Location = new System.Drawing.Point(33, 265);
            this.chkDebugTrace.Name = "chkDebugTrace";
            this.chkDebugTrace.Size = new System.Drawing.Size(125, 17);
            this.chkDebugTrace.TabIndex = 63;
            this.chkDebugTrace.Text = "Enable Debug Trace";
            this.chkDebugTrace.UseVisualStyleBackColor = true;
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(33, 242);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(90, 17);
            this.chkTrace.TabIndex = 62;
            this.chkTrace.Text = "Enable Trace";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(75, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(469, 15);
            this.label3.TabIndex = 64;
            this.label3.Text = "If supported, the web pages can be accessed through the buttons below.";
            // 
            // SetupConnectedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 298);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkDebugTrace);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(645, 337);
            this.MinimumSize = new System.Drawing.Size(645, 337);
            this.Name = "SetupConnectedForm";
            this.Text = "Configure Alpaca Device";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnSetupUrlDevice;
        private System.Windows.Forms.Button BtnSetupUrlMain;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDebugTrace;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Label label3;
    }
}