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
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnDeleteDrivers = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.LblVersionNumber = new System.Windows.Forms.Label();
            this.DynamicDriversCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(826, 430);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnDeleteDrivers
            // 
            this.BtnDeleteDrivers.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnDeleteDrivers.Location = new System.Drawing.Point(745, 430);
            this.BtnDeleteDrivers.Name = "BtnDeleteDrivers";
            this.BtnDeleteDrivers.Size = new System.Drawing.Size(75, 23);
            this.BtnDeleteDrivers.TabIndex = 1;
            this.BtnDeleteDrivers.Text = "Delete";
            this.BtnDeleteDrivers.UseVisualStyleBackColor = true;
            this.BtnDeleteDrivers.Click += new System.EventHandler(this.BtnDeleteDrivers_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(312, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(294, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "Remove Unwanted Dynamic Drivers";
            // 
            // LblVersionNumber
            // 
            this.LblVersionNumber.AutoSize = true;
            this.LblVersionNumber.Location = new System.Drawing.Point(12, 435);
            this.LblVersionNumber.Name = "LblVersionNumber";
            this.LblVersionNumber.Size = new System.Drawing.Size(131, 13);
            this.LblVersionNumber.TabIndex = 23;
            this.LblVersionNumber.Text = "Version Number Unknown";
            // 
            // DynamicDriversCheckedListBox
            // 
            this.DynamicDriversCheckedListBox.FormattingEnabled = true;
            this.DynamicDriversCheckedListBox.Location = new System.Drawing.Point(41, 65);
            this.DynamicDriversCheckedListBox.Name = "DynamicDriversCheckedListBox";
            this.DynamicDriversCheckedListBox.Size = new System.Drawing.Size(832, 349);
            this.DynamicDriversCheckedListBox.TabIndex = 24;
            this.DynamicDriversCheckedListBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 435);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Select drivers to be deleted by ticking their check boxes";
            // 
            // Form1
            // 
            this.AcceptButton = this.BtnDeleteDrivers;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnDeleteDrivers;
            this.ClientSize = new System.Drawing.Size(913, 465);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DynamicDriversCheckedListBox);
            this.Controls.Add(this.LblVersionNumber);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.BtnDeleteDrivers);
            this.Controls.Add(this.BtnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Dynamic Remote Driver Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnDeleteDrivers;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label LblVersionNumber;
        private System.Windows.Forms.CheckedListBox DynamicDriversCheckedListBox;
        private System.Windows.Forms.Label label1;
    }
}

