namespace ASCOM.DynamicRemoteClients
{
    partial class ManageDevicesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageDevicesForm));
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnDeleteDrivers = new System.Windows.Forms.Button();
            this.LblTitle = new System.Windows.Forms.Label();
            this.LblVersionNumber = new System.Windows.Forms.Label();
            this.LblDriversToBeDeleted = new System.Windows.Forms.Label();
            this.ChkSelectAll = new System.Windows.Forms.CheckBox();
            this.ChkIncludeAscomRemoteDrivers = new System.Windows.Forms.CheckBox();
            this.ChkIncludeAlpacaDynamicDrivers = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(826, 435);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnDeleteDrivers
            // 
            this.BtnDeleteDrivers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDeleteDrivers.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnDeleteDrivers.Location = new System.Drawing.Point(745, 435);
            this.BtnDeleteDrivers.Name = "BtnDeleteDrivers";
            this.BtnDeleteDrivers.Size = new System.Drawing.Size(75, 23);
            this.BtnDeleteDrivers.TabIndex = 1;
            this.BtnDeleteDrivers.Text = "Delete";
            this.BtnDeleteDrivers.UseVisualStyleBackColor = true;
            this.BtnDeleteDrivers.Click += new System.EventHandler(this.BtnDeleteDrivers_Click);
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.LblTitle.Location = new System.Drawing.Point(312, 25);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(294, 20);
            this.LblTitle.TabIndex = 21;
            this.LblTitle.Text = "Remove Unwanted Dynamic Drivers";
            // 
            // LblVersionNumber
            // 
            this.LblVersionNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblVersionNumber.AutoSize = true;
            this.LblVersionNumber.Location = new System.Drawing.Point(12, 440);
            this.LblVersionNumber.Name = "LblVersionNumber";
            this.LblVersionNumber.Size = new System.Drawing.Size(131, 13);
            this.LblVersionNumber.TabIndex = 23;
            this.LblVersionNumber.Text = "Version Number Unknown";
            // 
            // LblDriversToBeDeleted
            // 
            this.LblDriversToBeDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblDriversToBeDeleted.AutoSize = true;
            this.LblDriversToBeDeleted.Location = new System.Drawing.Point(324, 440);
            this.LblDriversToBeDeleted.Name = "LblDriversToBeDeleted";
            this.LblDriversToBeDeleted.Size = new System.Drawing.Size(271, 13);
            this.LblDriversToBeDeleted.TabIndex = 25;
            this.LblDriversToBeDeleted.Text = "Select drivers to be deleted by ticking their check boxes";
            // 
            // ChkSelectAll
            // 
            this.ChkSelectAll.AutoSize = true;
            this.ChkSelectAll.Location = new System.Drawing.Point(44, 69);
            this.ChkSelectAll.Name = "ChkSelectAll";
            this.ChkSelectAll.Size = new System.Drawing.Size(69, 17);
            this.ChkSelectAll.TabIndex = 26;
            this.ChkSelectAll.Text = "Select all";
            this.ChkSelectAll.UseVisualStyleBackColor = true;
            this.ChkSelectAll.CheckedChanged += new System.EventHandler(this.ChkSelectAll_CheckedChanged);
            // 
            // ChkIncludeAscomRemoteDrivers
            // 
            this.ChkIncludeAscomRemoteDrivers.AutoSize = true;
            this.ChkIncludeAscomRemoteDrivers.Location = new System.Drawing.Point(674, 69);
            this.ChkIncludeAscomRemoteDrivers.Name = "ChkIncludeAscomRemoteDrivers";
            this.ChkIncludeAscomRemoteDrivers.Size = new System.Drawing.Size(170, 17);
            this.ChkIncludeAscomRemoteDrivers.TabIndex = 27;
            this.ChkIncludeAscomRemoteDrivers.Text = "Show ASCOM Remote Drivers";
            this.ChkIncludeAscomRemoteDrivers.UseVisualStyleBackColor = true;
            // 
            // ChkIncludeAlpacaDynamicDrivers
            // 
            this.ChkIncludeAlpacaDynamicDrivers.AutoSize = true;
            this.ChkIncludeAlpacaDynamicDrivers.Location = new System.Drawing.Point(674, 46);
            this.ChkIncludeAlpacaDynamicDrivers.Name = "ChkIncludeAlpacaDynamicDrivers";
            this.ChkIncludeAlpacaDynamicDrivers.Size = new System.Drawing.Size(169, 17);
            this.ChkIncludeAlpacaDynamicDrivers.TabIndex = 28;
            this.ChkIncludeAlpacaDynamicDrivers.Text = "Show Alpaca Dynamic Drivers";
            this.ChkIncludeAlpacaDynamicDrivers.UseVisualStyleBackColor = true;
            // 
            // ManageDevicesForm
            // 
            this.AcceptButton = this.BtnDeleteDrivers;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnDeleteDrivers;
            this.ClientSize = new System.Drawing.Size(913, 470);
            this.Controls.Add(this.ChkIncludeAlpacaDynamicDrivers);
            this.Controls.Add(this.ChkIncludeAscomRemoteDrivers);
            this.Controls.Add(this.ChkSelectAll);
            this.Controls.Add(this.LblDriversToBeDeleted);
            this.Controls.Add(this.LblVersionNumber);
            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.BtnDeleteDrivers);
            this.Controls.Add(this.BtnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "ManageDevicesForm";
            this.Text = "Dynamic Remote Driver Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnDeleteDrivers;
        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Label LblVersionNumber;
        //private System.Windows.Forms.CheckedListBox DynamicDriversCheckedListBox;
        private System.Windows.Forms.Label LblDriversToBeDeleted;
        private System.Windows.Forms.CheckBox ChkSelectAll;
        private System.Windows.Forms.CheckBox ChkIncludeAscomRemoteDrivers;
        private System.Windows.Forms.CheckBox ChkIncludeAlpacaDynamicDrivers;
    }
}

