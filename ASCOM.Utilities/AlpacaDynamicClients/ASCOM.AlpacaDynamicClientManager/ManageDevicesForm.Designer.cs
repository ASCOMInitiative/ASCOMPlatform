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
            BtnClose = new System.Windows.Forms.Button();
            BtnDeleteDrivers = new System.Windows.Forms.Button();
            LblTitle = new System.Windows.Forms.Label();
            LblVersionNumber = new System.Windows.Forms.Label();
            LblDriversToBeDeleted = new System.Windows.Forms.Label();
            ChkSelectAll = new System.Windows.Forms.CheckBox();
            ChkIncludeAscomRemoteDrivers = new System.Windows.Forms.CheckBox();
            ChkIncludeAlpacaDynamicDrivers = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // BtnClose
            // 
            BtnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnClose.Location = new System.Drawing.Point(964, 502);
            BtnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new System.Drawing.Size(88, 27);
            BtnClose.TabIndex = 0;
            BtnClose.Text = "Close";
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += BtnCancel_Click;
            // 
            // BtnDeleteDrivers
            // 
            BtnDeleteDrivers.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnDeleteDrivers.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            BtnDeleteDrivers.Location = new System.Drawing.Point(869, 502);
            BtnDeleteDrivers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnDeleteDrivers.Name = "BtnDeleteDrivers";
            BtnDeleteDrivers.Size = new System.Drawing.Size(88, 27);
            BtnDeleteDrivers.TabIndex = 1;
            BtnDeleteDrivers.Text = "Delete";
            BtnDeleteDrivers.UseVisualStyleBackColor = true;
            BtnDeleteDrivers.Click += BtnDeleteDrivers_Click;
            // 
            // LblTitle
            // 
            LblTitle.AutoSize = true;
            LblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            LblTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            LblTitle.Location = new System.Drawing.Point(364, 29);
            LblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LblTitle.Name = "LblTitle";
            LblTitle.Size = new System.Drawing.Size(294, 20);
            LblTitle.TabIndex = 21;
            LblTitle.Text = "Remove Unwanted Dynamic Drivers";
            // 
            // LblVersionNumber
            // 
            LblVersionNumber.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblVersionNumber.AutoSize = true;
            LblVersionNumber.Location = new System.Drawing.Point(14, 508);
            LblVersionNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LblVersionNumber.Name = "LblVersionNumber";
            LblVersionNumber.Size = new System.Drawing.Size(146, 15);
            LblVersionNumber.TabIndex = 23;
            LblVersionNumber.Text = "Version Number Unknown";
            // 
            // LblDriversToBeDeleted
            // 
            LblDriversToBeDeleted.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblDriversToBeDeleted.AutoSize = true;
            LblDriversToBeDeleted.Location = new System.Drawing.Point(378, 508);
            LblDriversToBeDeleted.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LblDriversToBeDeleted.Name = "LblDriversToBeDeleted";
            LblDriversToBeDeleted.Size = new System.Drawing.Size(298, 15);
            LblDriversToBeDeleted.TabIndex = 25;
            LblDriversToBeDeleted.Text = "Select drivers to be deleted by ticking their check boxes";
            // 
            // ChkSelectAll
            // 
            ChkSelectAll.AutoSize = true;
            ChkSelectAll.Location = new System.Drawing.Point(45, 53);
            ChkSelectAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ChkSelectAll.Name = "ChkSelectAll";
            ChkSelectAll.Size = new System.Drawing.Size(72, 19);
            ChkSelectAll.TabIndex = 26;
            ChkSelectAll.Text = "Select all";
            ChkSelectAll.UseVisualStyleBackColor = true;
            ChkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;
            // 
            // ChkIncludeAscomRemoteDrivers
            // 
            ChkIncludeAscomRemoteDrivers.AutoSize = true;
            ChkIncludeAscomRemoteDrivers.Location = new System.Drawing.Point(779, 53);
            ChkIncludeAscomRemoteDrivers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ChkIncludeAscomRemoteDrivers.Name = "ChkIncludeAscomRemoteDrivers";
            ChkIncludeAscomRemoteDrivers.Size = new System.Drawing.Size(183, 19);
            ChkIncludeAscomRemoteDrivers.TabIndex = 27;
            ChkIncludeAscomRemoteDrivers.Text = "Show ASCOM Remote Drivers";
            ChkIncludeAscomRemoteDrivers.UseVisualStyleBackColor = true;
            // 
            // ChkIncludeAlpacaDynamicDrivers
            // 
            ChkIncludeAlpacaDynamicDrivers.AutoSize = true;
            ChkIncludeAlpacaDynamicDrivers.Location = new System.Drawing.Point(779, 26);
            ChkIncludeAlpacaDynamicDrivers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ChkIncludeAlpacaDynamicDrivers.Name = "ChkIncludeAlpacaDynamicDrivers";
            ChkIncludeAlpacaDynamicDrivers.Size = new System.Drawing.Size(183, 19);
            ChkIncludeAlpacaDynamicDrivers.TabIndex = 28;
            ChkIncludeAlpacaDynamicDrivers.Text = "Show Alpaca Dynamic Drivers";
            ChkIncludeAlpacaDynamicDrivers.UseVisualStyleBackColor = true;
            // 
            // ManageDevicesForm
            // 
            AcceptButton = BtnDeleteDrivers;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = BtnDeleteDrivers;
            ClientSize = new System.Drawing.Size(1065, 542);
            Controls.Add(ChkIncludeAlpacaDynamicDrivers);
            Controls.Add(ChkIncludeAscomRemoteDrivers);
            Controls.Add(ChkSelectAll);
            Controls.Add(LblDriversToBeDeleted);
            Controls.Add(LblVersionNumber);
            Controls.Add(LblTitle);
            Controls.Add(BtnDeleteDrivers);
            Controls.Add(BtnClose);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(697, 340);
            Name = "ManageDevicesForm";
            Text = "Dynamic Remote Driver Configuration";
            ResumeLayout(false);
            PerformLayout();
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

