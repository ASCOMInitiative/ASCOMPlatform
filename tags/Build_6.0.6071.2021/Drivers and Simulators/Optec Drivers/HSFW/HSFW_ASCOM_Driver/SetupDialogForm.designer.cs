namespace ASCOM.HSFW_ASCOM_Driver
{
    partial class SetupDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.CurrentWheelID_LBL = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Status_LBL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CurrWheName_LBL = new System.Windows.Forms.Label();
            this.AttDev_CB = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CurrentFilter_CB = new System.Windows.Forms.ComboBox();
            this.SettingsBTN = new System.Windows.Forms.Button();
            this.ReHome_Btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.NewVersionCheckerBGW = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(259, 314);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(259, 344);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Available Devices";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(245, 90);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(65, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "What\'s this?";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 14);
            this.label5.TabIndex = 10;
            this.label5.Text = "Current Wheel ID";
            // 
            // CurrentWheelID_LBL
            // 
            this.CurrentWheelID_LBL.AutoSize = true;
            this.CurrentWheelID_LBL.Location = new System.Drawing.Point(147, 122);
            this.CurrentWheelID_LBL.Name = "CurrentWheelID_LBL";
            this.CurrentWheelID_LBL.Size = new System.Drawing.Size(35, 13);
            this.CurrentWheelID_LBL.TabIndex = 7;
            this.CurrentWheelID_LBL.Text = "label3";
            this.CurrentWheelID_LBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 14);
            this.label4.TabIndex = 10;
            this.label4.Text = "Status";
            // 
            // Status_LBL
            // 
            this.Status_LBL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Status_LBL.Location = new System.Drawing.Point(147, 218);
            this.Status_LBL.Multiline = true;
            this.Status_LBL.Name = "Status_LBL";
            this.Status_LBL.ReadOnly = true;
            this.Status_LBL.Size = new System.Drawing.Size(160, 78);
            this.Status_LBL.TabIndex = 11;
            this.Status_LBL.Text = "Status Message...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 14);
            this.label3.TabIndex = 10;
            this.label3.Text = "Current Wheel Name";
            // 
            // CurrWheName_LBL
            // 
            this.CurrWheName_LBL.AutoSize = true;
            this.CurrWheName_LBL.Location = new System.Drawing.Point(147, 154);
            this.CurrWheName_LBL.Name = "CurrWheName_LBL";
            this.CurrWheName_LBL.Size = new System.Drawing.Size(35, 13);
            this.CurrWheName_LBL.TabIndex = 7;
            this.CurrWheName_LBL.Text = "label3";
            // 
            // AttDev_CB
            // 
            this.AttDev_CB.FormattingEnabled = true;
            this.AttDev_CB.Location = new System.Drawing.Point(147, 87);
            this.AttDev_CB.Name = "AttDev_CB";
            this.AttDev_CB.Size = new System.Drawing.Size(92, 21);
            this.AttDev_CB.TabIndex = 8;
            this.AttDev_CB.SelectionChangeCommitted += new System.EventHandler(this.AttDev_CB_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(16, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "Current Filter";
            // 
            // CurrentFilter_CB
            // 
            this.CurrentFilter_CB.FormattingEnabled = true;
            this.CurrentFilter_CB.Location = new System.Drawing.Point(147, 183);
            this.CurrentFilter_CB.Name = "CurrentFilter_CB";
            this.CurrentFilter_CB.Size = new System.Drawing.Size(166, 21);
            this.CurrentFilter_CB.TabIndex = 12;
            this.CurrentFilter_CB.SelectionChangeCommitted += new System.EventHandler(this.CurrentFilter_CB_SelectionChangeCommitted);
            // 
            // SettingsBTN
            // 
            this.SettingsBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SettingsBTN.Location = new System.Drawing.Point(19, 326);
            this.SettingsBTN.Name = "SettingsBTN";
            this.SettingsBTN.Size = new System.Drawing.Size(149, 23);
            this.SettingsBTN.TabIndex = 6;
            this.SettingsBTN.Text = "Advanced Settings...";
            this.SettingsBTN.UseVisualStyleBackColor = true;
            this.SettingsBTN.Click += new System.EventHandler(this.SettingsBTN_Click);
            // 
            // ReHome_Btn
            // 
            this.ReHome_Btn.Location = new System.Drawing.Point(19, 248);
            this.ReHome_Btn.Name = "ReHome_Btn";
            this.ReHome_Btn.Size = new System.Drawing.Size(100, 23);
            this.ReHome_Btn.TabIndex = 13;
            this.ReHome_Btn.Text = "Refresh Device";
            this.ReHome_Btn.UseVisualStyleBackColor = true;
            this.ReHome_Btn.Visible = false;
            this.ReHome_Btn.Click += new System.EventHandler(this.ReHome_Btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.HSFW_ASCOM_Driver.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(12, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 39);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.HSFW_ASCOM_Driver.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(270, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // NewVersionCheckerBGW
            // 
            this.NewVersionCheckerBGW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NewVersionCheckerBGW_DoWork);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 377);
            this.Controls.Add(this.ReHome_Btn);
            this.Controls.Add(this.CurrentFilter_CB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CurrWheName_LBL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CurrentWheelID_LBL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Status_LBL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SettingsBTN);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AttDev_CB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "High Speed Filter Wheel Setup Dialog";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.Shown += new System.EventHandler(this.SetupDialogForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupDialogForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox AttDev_CB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label CurrentWheelID_LBL;
        private System.Windows.Forms.ComboBox CurrentFilter_CB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CurrWheName_LBL;
        private System.Windows.Forms.Button SettingsBTN;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Status_LBL;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ReHome_Btn;
        private System.ComponentModel.BackgroundWorker NewVersionCheckerBGW;
    }
}