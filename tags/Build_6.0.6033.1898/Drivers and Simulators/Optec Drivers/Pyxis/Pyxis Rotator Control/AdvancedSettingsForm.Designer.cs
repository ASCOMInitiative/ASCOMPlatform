namespace Pyxis_Rotator_Control
{
    partial class AdvancedSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettingsForm));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.RestoreDefaults_BTN = new System.Windows.Forms.Button();
            this.Close_BTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(12, 28);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(277, 398);
            this.propertyGrid1.TabIndex = 0;
            // 
            // RestoreDefaults_BTN
            // 
            this.RestoreDefaults_BTN.Location = new System.Drawing.Point(12, 432);
            this.RestoreDefaults_BTN.Name = "RestoreDefaults_BTN";
            this.RestoreDefaults_BTN.Size = new System.Drawing.Size(120, 38);
            this.RestoreDefaults_BTN.TabIndex = 1;
            this.RestoreDefaults_BTN.Text = "Restore Device Defaults";
            this.RestoreDefaults_BTN.UseVisualStyleBackColor = true;
            this.RestoreDefaults_BTN.Click += new System.EventHandler(this.RestoreDefaults_BTN_Click);
            // 
            // Close_BTN
            // 
            this.Close_BTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close_BTN.Location = new System.Drawing.Point(214, 447);
            this.Close_BTN.Name = "Close_BTN";
            this.Close_BTN.Size = new System.Drawing.Size(75, 23);
            this.Close_BTN.TabIndex = 2;
            this.Close_BTN.Text = "Close";
            this.Close_BTN.UseVisualStyleBackColor = true;
            // 
            // AdvancedSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 477);
            this.Controls.Add(this.Close_BTN);
            this.Controls.Add(this.RestoreDefaults_BTN);
            this.Controls.Add(this.propertyGrid1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdvancedSettingsForm";
            this.Text = "Advanced Pyxis Settings";
            this.Load += new System.EventHandler(this.AdvancedSettingsFormcs_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button RestoreDefaults_BTN;
        private System.Windows.Forms.Button Close_BTN;
    }
}