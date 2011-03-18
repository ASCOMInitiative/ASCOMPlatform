namespace ASCOM.OptecFocuserHub
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SerialRB = new System.Windows.Forms.RadioButton();
            this.EthernetRB = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.ComPortNameCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IPAddrTB = new System.Windows.Forms.TextBox();
            this.PortNumTB = new System.Windows.Forms.Label();
            this.TcpipPortNumberTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(214, 434);
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
            this.cmdCancel.Location = new System.Drawing.Point(214, 464);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.InitialImage = global::ASCOM.OptecFocuserHub.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(225, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 79);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note: Remember that both Focuser1 and Focuser2 are physically connected to one co" +
                "ntroller (hub). Changing the connection settings for one will affect both.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connection Method:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.EthernetRB);
            this.panel1.Controls.Add(this.SerialRB);
            this.panel1.Location = new System.Drawing.Point(117, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(124, 54);
            this.panel1.TabIndex = 7;
            // 
            // SerialRB
            // 
            this.SerialRB.AutoSize = true;
            this.SerialRB.Location = new System.Drawing.Point(12, 8);
            this.SerialRB.Name = "SerialRB";
            this.SerialRB.Size = new System.Drawing.Size(51, 17);
            this.SerialRB.TabIndex = 0;
            this.SerialRB.TabStop = true;
            this.SerialRB.Text = "Serial";
            this.SerialRB.UseVisualStyleBackColor = true;
            this.SerialRB.CheckedChanged += new System.EventHandler(this.SerialRB_CheckedChanged);
            // 
            // EthernetRB
            // 
            this.EthernetRB.AutoSize = true;
            this.EthernetRB.Location = new System.Drawing.Point(12, 31);
            this.EthernetRB.Name = "EthernetRB";
            this.EthernetRB.Size = new System.Drawing.Size(65, 17);
            this.EthernetRB.TabIndex = 0;
            this.EthernetRB.TabStop = true;
            this.EthernetRB.Text = "Ethernet";
            this.EthernetRB.UseVisualStyleBackColor = true;
            this.EthernetRB.CheckedChanged += new System.EventHandler(this.EthernetRB_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(12, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Com Port:";
            // 
            // ComPortNameCB
            // 
            this.ComPortNameCB.Enabled = false;
            this.ComPortNameCB.FormattingEnabled = true;
            this.ComPortNameCB.Location = new System.Drawing.Point(117, 142);
            this.ComPortNameCB.Name = "ComPortNameCB";
            this.ComPortNameCB.Size = new System.Drawing.Size(121, 21);
            this.ComPortNameCB.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(12, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP Address:";
            // 
            // IPAddrTB
            // 
            this.IPAddrTB.Enabled = false;
            this.IPAddrTB.Location = new System.Drawing.Point(117, 173);
            this.IPAddrTB.Name = "IPAddrTB";
            this.IPAddrTB.Size = new System.Drawing.Size(119, 20);
            this.IPAddrTB.TabIndex = 11;
            // 
            // PortNumTB
            // 
            this.PortNumTB.AutoSize = true;
            this.PortNumTB.Enabled = false;
            this.PortNumTB.Location = new System.Drawing.Point(12, 209);
            this.PortNumTB.Name = "PortNumTB";
            this.PortNumTB.Size = new System.Drawing.Size(68, 13);
            this.PortNumTB.TabIndex = 10;
            this.PortNumTB.Text = "TCP/IP Port:";
            // 
            // TcpipPortNumberTB
            // 
            this.TcpipPortNumberTB.Enabled = false;
            this.TcpipPortNumberTB.Location = new System.Drawing.Point(117, 206);
            this.TcpipPortNumberTB.Name = "TcpipPortNumberTB";
            this.TcpipPortNumberTB.Size = new System.Drawing.Size(119, 20);
            this.TcpipPortNumberTB.TabIndex = 11;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 497);
            this.Controls.Add(this.TcpipPortNumberTB);
            this.Controls.Add(this.IPAddrTB);
            this.Controls.Add(this.PortNumTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ComPortNameCB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec Focuser Hub Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton EthernetRB;
        private System.Windows.Forms.RadioButton SerialRB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComPortNameCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox IPAddrTB;
        private System.Windows.Forms.Label PortNumTB;
        private System.Windows.Forms.TextBox TcpipPortNumberTB;
    }
}