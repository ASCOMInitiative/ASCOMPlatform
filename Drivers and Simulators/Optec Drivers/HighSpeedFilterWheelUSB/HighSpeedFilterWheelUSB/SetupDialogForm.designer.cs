namespace ASCOM.HighSpeedFilterWheelUSB
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
            this.components = new System.ComponentModel.Container();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.Home_Btn = new System.Windows.Forms.Button();
            this.AvailableDevice_CB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.filterWheelsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.filterWheelsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterWheelsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterWheelsBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(183, 468);
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
            this.cmdCancel.Location = new System.Drawing.Point(107, 468);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // Home_Btn
            // 
            this.Home_Btn.Location = new System.Drawing.Point(161, 100);
            this.Home_Btn.Name = "Home_Btn";
            this.Home_Btn.Size = new System.Drawing.Size(83, 23);
            this.Home_Btn.TabIndex = 4;
            this.Home_Btn.Text = "Home Wheel";
            this.Home_Btn.UseVisualStyleBackColor = true;
            this.Home_Btn.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // AvailableDevice_CB
            // 
            this.AvailableDevice_CB.FormattingEnabled = true;
            this.AvailableDevice_CB.Location = new System.Drawing.Point(125, 71);
            this.AvailableDevice_CB.Name = "AvailableDevice_CB";
            this.AvailableDevice_CB.Size = new System.Drawing.Size(119, 21);
            this.AvailableDevice_CB.TabIndex = 5;
            this.AvailableDevice_CB.SelectionChangeCommitted += new System.EventHandler(this.AvailableDevice_CB_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Device Serial Numbers:";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(16, 129);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(226, 333);
            this.propertyGrid1.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.HighSpeedFilterWheelUSB.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(4, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.HighSpeedFilterWheelUSB.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(196, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // filterWheelsBindingSource
            // 
            this.filterWheelsBindingSource.DataSource = typeof(OptecHID_FilterWheelAPI.FilterWheels);
            // 
            // filterWheelsBindingSource1
            // 
            this.filterWheelsBindingSource1.DataSource = typeof(OptecHID_FilterWheelAPI.FilterWheels);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 504);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AvailableDevice_CB);
            this.Controls.Add(this.Home_Btn);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HighSpeedFilterWheelUSB Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterWheelsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterWheelsBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Button Home_Btn;
        private System.Windows.Forms.ComboBox AvailableDevice_CB;
        private System.Windows.Forms.BindingSource filterWheelsBindingSource;
        private System.Windows.Forms.BindingSource filterWheelsBindingSource1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}