namespace ASCOM.SettingsProviderSample.CSharp
{
    partial class SetupDialog
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
            this.btnDefaults = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkOnOffSetting = new System.Windows.Forms.CheckBox();
            this.cbCommPort = new System.Windows.Forms.ComboBox();
            this.txtTextSetting = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDefaults
            // 
            this.btnDefaults.Location = new System.Drawing.Point(197, 66);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnDefaults.TabIndex = 11;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 37);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(197, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkOnOffSetting
            // 
            this.chkOnOffSetting.AutoSize = true;
            this.chkOnOffSetting.Checked = global::ASCOM.SettingsProviderSample.CSharp.Properties.Settings.Default.OnOffSetting;
            this.chkOnOffSetting.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.SettingsProviderSample.CSharp.Properties.Settings.Default, "OnOffSetting", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkOnOffSetting.Location = new System.Drawing.Point(12, 12);
            this.chkOnOffSetting.Name = "chkOnOffSetting";
            this.chkOnOffSetting.Size = new System.Drawing.Size(95, 17);
            this.chkOnOffSetting.TabIndex = 6;
            this.chkOnOffSetting.Text = "On/Off Setting";
            this.chkOnOffSetting.UseVisualStyleBackColor = true;
            // 
            // cbCommPort
            // 
            this.cbCommPort.FormattingEnabled = true;
            this.cbCommPort.Location = new System.Drawing.Point(12, 63);
            this.cbCommPort.Name = "cbCommPort";
            this.cbCommPort.Size = new System.Drawing.Size(121, 21);
            this.cbCommPort.TabIndex = 8;
            this.cbCommPort.Text = global::ASCOM.SettingsProviderSample.CSharp.Properties.Settings.Default.CommPort;
            // 
            // txtTextSetting
            // 
            this.txtTextSetting.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.SettingsProviderSample.CSharp.Properties.Settings.Default, "TextSetting", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTextSetting.Location = new System.Drawing.Point(12, 36);
            this.txtTextSetting.Name = "txtTextSetting";
            this.txtTextSetting.Size = new System.Drawing.Size(100, 20);
            this.txtTextSetting.TabIndex = 7;
            this.txtTextSetting.Text = global::ASCOM.SettingsProviderSample.CSharp.Properties.Settings.Default.TextSetting;
            // 
            // SetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 106);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbCommPort);
            this.Controls.Add(this.txtTextSetting);
            this.Controls.Add(this.chkOnOffSetting);
            this.Name = "SetupDialog";
            this.Text = "SettingsProvider Sample App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbCommPort;
        private System.Windows.Forms.TextBox txtTextSetting;
        private System.Windows.Forms.CheckBox chkOnOffSetting;
    }
}

