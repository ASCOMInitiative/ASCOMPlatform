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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorView));
            this.buttonSetup = new System.Windows.Forms.Button();
            this.cmbSwitch = new System.Windows.Forms.ComboBox();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.upDownSwitch = new System.Windows.Forms.NumericUpDown();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSwitch)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetup
            // 
            this.buttonSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetup.Image = ((System.Drawing.Image)(resources.GetObject("buttonSetup.Image")));
            this.buttonSetup.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSetup.Location = new System.Drawing.Point(374, -1);
            this.buttonSetup.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(33, 26);
            this.buttonSetup.TabIndex = 77;
            this.buttonSetup.Text = "C";
            this.toolTip.SetToolTip(this.buttonSetup, "Runs the Setup dialogue for the selected driver");
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // cmbSwitch
            // 
            this.cmbSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSwitch.FormattingEnabled = true;
            this.cmbSwitch.Location = new System.Drawing.Point(542, 2);
            this.cmbSwitch.Name = "cmbSwitch";
            this.cmbSwitch.Size = new System.Drawing.Size(350, 21);
            this.cmbSwitch.TabIndex = 76;
            this.cmbSwitch.SelectedIndexChanged += new System.EventHandler(this.cmbSwitch_SelectedIndexChanged);
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(0, 1);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(311, 21);
            this.cmbDevice.TabIndex = 74;
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
            // 
            // upDownSwitch
            // 
            this.upDownSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upDownSwitch.Location = new System.Drawing.Point(468, 2);
            this.upDownSwitch.Name = "upDownSwitch";
            this.upDownSwitch.Size = new System.Drawing.Size(39, 20);
            this.upDownSwitch.TabIndex = 78;
            this.upDownSwitch.ValueChanged += new System.EventHandler(this.upDownSwitch_ValueChanged);
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDescription.BackColor = System.Drawing.SystemColors.Window;
            this.labelDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDescription.Location = new System.Drawing.Point(542, 2);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(350, 21);
            this.labelDescription.TabIndex = 79;
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SensorView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.upDownSwitch);
            this.Controls.Add(this.buttonSetup);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.cmbSwitch);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(898, 23);
            ((System.ComponentModel.ISupportInitialize)(this.upDownSwitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.ComboBox cmbSwitch;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.NumericUpDown upDownSwitch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelDescription;
    }
}
