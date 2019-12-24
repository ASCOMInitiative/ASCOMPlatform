namespace ASCOM.Remote
{
    partial class ServedDeviceClient
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
                profile.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.txtDeviceNumber = new System.Windows.Forms.TextBox();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.chkAllowSetConnectedTrue = new System.Windows.Forms.CheckBox();
            this.chkAllowSetConnectedFalse = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Location = new System.Drawing.Point(0, 1);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(165, 21);
            this.cmbDeviceType.TabIndex = 0;
            this.cmbDeviceType.SelectedIndexChanged += new System.EventHandler(this.CmbDeviceType_SelectedIndexChanged);
            // 
            // txtDeviceNumber
            // 
            this.txtDeviceNumber.Location = new System.Drawing.Point(200, 1);
            this.txtDeviceNumber.Name = "txtDeviceNumber";
            this.txtDeviceNumber.ReadOnly = true;
            this.txtDeviceNumber.Size = new System.Drawing.Size(25, 20);
            this.txtDeviceNumber.TabIndex = 1;
            this.txtDeviceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbDevice
            // 
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(290, 1);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(296, 21);
            this.cmbDevice.TabIndex = 3;
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.CmbDevice_SelectedIndexChanged);
            // 
            // chkAllowSetConnectedTrue
            // 
            this.chkAllowSetConnectedTrue.AutoSize = true;
            this.chkAllowSetConnectedTrue.Location = new System.Drawing.Point(724, 4);
            this.chkAllowSetConnectedTrue.Name = "chkAllowSetConnectedTrue";
            this.chkAllowSetConnectedTrue.Size = new System.Drawing.Size(15, 14);
            this.chkAllowSetConnectedTrue.TabIndex = 7;
            this.chkAllowSetConnectedTrue.UseVisualStyleBackColor = true;
            // 
            // chkAllowSetConnectedFalse
            // 
            this.chkAllowSetConnectedFalse.AutoSize = true;
            this.chkAllowSetConnectedFalse.Location = new System.Drawing.Point(664, 4);
            this.chkAllowSetConnectedFalse.Name = "chkAllowSetConnectedFalse";
            this.chkAllowSetConnectedFalse.Size = new System.Drawing.Size(15, 14);
            this.chkAllowSetConnectedFalse.TabIndex = 6;
            this.chkAllowSetConnectedFalse.UseVisualStyleBackColor = true;
            // 
            // ServedDeviceClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkAllowSetConnectedTrue);
            this.Controls.Add(this.chkAllowSetConnectedFalse);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.txtDeviceNumber);
            this.Controls.Add(this.cmbDeviceType);
            this.Name = "ServedDeviceClient";
            this.Size = new System.Drawing.Size(746, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.TextBox txtDeviceNumber;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.CheckBox chkAllowSetConnectedTrue;
        private System.Windows.Forms.CheckBox chkAllowSetConnectedFalse;
    }
}
