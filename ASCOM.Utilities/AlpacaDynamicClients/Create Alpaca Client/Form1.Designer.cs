namespace ASCOM.DynamicRemoteClients
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnApply = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.LblVersionNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(320, 330);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 0;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnApply
            // 
            this.BtnApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnApply.Location = new System.Drawing.Point(239, 330);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 23);
            this.BtnApply.TabIndex = 1;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(74, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(255, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Number of Remote Device Drivers Required";
            // 
            // LblVersionNumber
            // 
            this.LblVersionNumber.AutoSize = true;
            this.LblVersionNumber.Location = new System.Drawing.Point(12, 335);
            this.LblVersionNumber.Name = "LblVersionNumber";
            this.LblVersionNumber.Size = new System.Drawing.Size(131, 13);
            this.LblVersionNumber.TabIndex = 23;
            this.LblVersionNumber.Text = "Version Number Unknown";
            // 
            // Form1
            // 
            this.AcceptButton = this.BtnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnApply;
            this.ClientSize = new System.Drawing.Size(407, 365);
            this.Controls.Add(this.LblVersionNumber);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.BtnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Dynamic Remote Driver Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label LblVersionNumber;
    }
}

