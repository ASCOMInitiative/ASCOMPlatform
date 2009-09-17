namespace ASCOM.Optec_IFW
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DissConnBtn = new System.Windows.Forms.Button();
            this.HomeBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.WheelId_TB = new System.Windows.Forms.TextBox();
            this.ReadNames_Btn = new System.Windows.Forms.Button();
            this.FilterNames_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckConn_Btn = new System.Windows.Forms.Button();
            this.ConnStatus_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GoTo_Btn = new System.Windows.Forms.Button();
            this.GoToPos_CB = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(549, 298);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(549, 356);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Optec_IFW.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(462, 35);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(16, 86);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(100, 23);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DissConnBtn
            // 
            this.DissConnBtn.Location = new System.Drawing.Point(16, 265);
            this.DissConnBtn.Name = "DissConnBtn";
            this.DissConnBtn.Size = new System.Drawing.Size(100, 23);
            this.DissConnBtn.TabIndex = 5;
            this.DissConnBtn.Text = "Disconnect";
            this.DissConnBtn.UseVisualStyleBackColor = true;
            this.DissConnBtn.Click += new System.EventHandler(this.DissConnBtn_Click);
            // 
            // HomeBtn
            // 
            this.HomeBtn.Location = new System.Drawing.Point(16, 144);
            this.HomeBtn.Name = "HomeBtn";
            this.HomeBtn.Size = new System.Drawing.Size(100, 23);
            this.HomeBtn.TabIndex = 6;
            this.HomeBtn.Text = "Home Device";
            this.HomeBtn.UseVisualStyleBackColor = true;
            this.HomeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Wheel ID:";
            // 
            // WheelId_TB
            // 
            this.WheelId_TB.Location = new System.Drawing.Point(204, 141);
            this.WheelId_TB.Name = "WheelId_TB";
            this.WheelId_TB.Size = new System.Drawing.Size(26, 20);
            this.WheelId_TB.TabIndex = 8;
            // 
            // ReadNames_Btn
            // 
            this.ReadNames_Btn.Location = new System.Drawing.Point(16, 173);
            this.ReadNames_Btn.Name = "ReadNames_Btn";
            this.ReadNames_Btn.Size = new System.Drawing.Size(100, 23);
            this.ReadNames_Btn.TabIndex = 9;
            this.ReadNames_Btn.Text = "Read Names";
            this.ReadNames_Btn.UseVisualStyleBackColor = true;
            this.ReadNames_Btn.Click += new System.EventHandler(this.ReadNames_Btn_Click);
            // 
            // FilterNames_TB
            // 
            this.FilterNames_TB.Location = new System.Drawing.Point(204, 171);
            this.FilterNames_TB.Name = "FilterNames_TB";
            this.FilterNames_TB.Size = new System.Drawing.Size(303, 20);
            this.FilterNames_TB.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Filter Names:";
            // 
            // CheckConn_Btn
            // 
            this.CheckConn_Btn.Location = new System.Drawing.Point(16, 115);
            this.CheckConn_Btn.Name = "CheckConn_Btn";
            this.CheckConn_Btn.Size = new System.Drawing.Size(100, 23);
            this.CheckConn_Btn.TabIndex = 12;
            this.CheckConn_Btn.Text = "Check Connect";
            this.CheckConn_Btn.UseVisualStyleBackColor = true;
            this.CheckConn_Btn.Click += new System.EventHandler(this.CheckConn_Btn_Click);
            // 
            // ConnStatus_TB
            // 
            this.ConnStatus_TB.Location = new System.Drawing.Point(204, 115);
            this.ConnStatus_TB.Name = "ConnStatus_TB";
            this.ConnStatus_TB.Size = new System.Drawing.Size(39, 20);
            this.ConnStatus_TB.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Yes/No:";
            // 
            // GoTo_Btn
            // 
            this.GoTo_Btn.Location = new System.Drawing.Point(16, 202);
            this.GoTo_Btn.Name = "GoTo_Btn";
            this.GoTo_Btn.Size = new System.Drawing.Size(100, 23);
            this.GoTo_Btn.TabIndex = 15;
            this.GoTo_Btn.Text = "Go To Filter";
            this.GoTo_Btn.UseVisualStyleBackColor = true;
            this.GoTo_Btn.Click += new System.EventHandler(this.GoTo_Btn_Click);
            // 
            // GoToPos_CB
            // 
            this.GoToPos_CB.FormattingEnabled = true;
            this.GoToPos_CB.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.GoToPos_CB.Location = new System.Drawing.Point(123, 203);
            this.GoToPos_CB.Name = "GoToPos_CB";
            this.GoToPos_CB.Size = new System.Drawing.Size(36, 21);
            this.GoToPos_CB.TabIndex = 16;
            this.GoToPos_CB.Text = "1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(531, 356);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(523, 330);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Device Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ConnectButton);
            this.tabPage2.Controls.Add(this.GoToPos_CB);
            this.tabPage2.Controls.Add(this.GoTo_Btn);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.picASCOM);
            this.tabPage2.Controls.Add(this.ConnStatus_TB);
            this.tabPage2.Controls.Add(this.DissConnBtn);
            this.tabPage2.Controls.Add(this.CheckConn_Btn);
            this.tabPage2.Controls.Add(this.HomeBtn);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.FilterNames_TB);
            this.tabPage2.Controls.Add(this.WheelId_TB);
            this.tabPage2.Controls.Add(this.ReadNames_Btn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(523, 330);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Device Trouble Shooting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 391);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec_IFW Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DissConnBtn;
        private System.Windows.Forms.Button HomeBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WheelId_TB;
        private System.Windows.Forms.Button ReadNames_Btn;
        private System.Windows.Forms.TextBox FilterNames_TB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CheckConn_Btn;
        private System.Windows.Forms.TextBox ConnStatus_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button GoTo_Btn;
        private System.Windows.Forms.ComboBox GoToPos_CB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}