namespace ASCOM.Simulator
{
    partial class SensorView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSetup = new System.Windows.Forms.Button();
            this.cmbSwitch = new System.Windows.Forms.ComboBox();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.upDownSwitch = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSwitch)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetup
            // 
            this.buttonSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetup.Location = new System.Drawing.Point(412, 0);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(33, 22);
            this.buttonSetup.TabIndex = 77;
            this.buttonSetup.Text = "C";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // cmbSwitch
            // 
            this.cmbSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSwitch.FormattingEnabled = true;
            this.cmbSwitch.Location = new System.Drawing.Point(519, 0);
            this.cmbSwitch.Name = "cmbSwitch";
            this.cmbSwitch.Size = new System.Drawing.Size(293, 21);
            this.cmbSwitch.TabIndex = 76;
            this.cmbSwitch.SelectedIndexChanged += new System.EventHandler(this.cmbSwitch_SelectedIndexChanged);
            // 
            // cmbDevice
            // 
            this.cmbDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(0, 0);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(390, 21);
            this.cmbDevice.TabIndex = 74;
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
            // 
            // upDownSwitch
            // 
            this.upDownSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upDownSwitch.Location = new System.Drawing.Point(451, 0);
            this.upDownSwitch.Name = "upDownSwitch";
            this.upDownSwitch.Size = new System.Drawing.Size(39, 20);
            this.upDownSwitch.TabIndex = 78;
            this.upDownSwitch.ValueChanged += new System.EventHandler(this.upDownSwitch_ValueChanged);
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upDownSwitch);
            this.Controls.Add(this.buttonSetup);
            this.Controls.Add(this.cmbSwitch);
            this.Controls.Add(this.cmbDevice);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(814, 24);
            ((System.ComponentModel.ISupportInitialize)(this.upDownSwitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.ComboBox cmbSwitch;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.NumericUpDown upDownSwitch;
    }
}
