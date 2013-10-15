namespace ASCOM.Simulator
{
	partial class frmSetupDialog
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Video Source");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Analogue Cameras");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Integrating Cameras");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Gamma");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Gain");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupDialog));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlPropertyPage = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvSettings = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
            this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdOK.Location = new System.Drawing.Point(538, 374);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdCancel.Location = new System.Drawing.Point(603, 374);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(610, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(50, 58);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.Control;
            this.btnReset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReset.Location = new System.Drawing.Point(191, 374);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(59, 25);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // pnlPropertyPage
            // 
            this.pnlPropertyPage.Location = new System.Drawing.Point(191, 8);
            this.pnlPropertyPage.Name = "pnlPropertyPage";
            this.pnlPropertyPage.Size = new System.Drawing.Size(471, 349);
            this.pnlPropertyPage.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(191, 363);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 3);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // tvSettings
            // 
            this.tvSettings.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tvSettings.ForeColor = System.Drawing.SystemColors.Window;
            this.tvSettings.HideSelection = false;
            this.tvSettings.Location = new System.Drawing.Point(9, 7);
            this.tvSettings.Name = "tvSettings";
            treeNode1.Name = "ndGeneral";
            treeNode1.Tag = "0";
            treeNode1.Text = "General";
            treeNode2.Name = "ndVideoSource";
            treeNode2.Tag = "1";
            treeNode2.Text = "Video Source";
            treeNode3.Name = "ndAnalogueCameras";
            treeNode3.Tag = "2";
            treeNode3.Text = "Analogue Cameras";
            treeNode4.Name = "ndIntegratingCameras";
            treeNode4.Tag = "3";
            treeNode4.Text = "Integrating Cameras";
            treeNode5.Name = "ndGamma";
            treeNode5.Tag = "4";
            treeNode5.Text = "Gamma";
            treeNode6.Name = "ndGain";
            treeNode6.Tag = "5";
            treeNode6.Text = "Gain";
            this.tvSettings.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            this.tvSettings.Size = new System.Drawing.Size(176, 359);
            this.tvSettings.TabIndex = 12;
            this.tvSettings.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvSettings_BeforeSelect);
            this.tvSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSettings_AfterSelect);
            // 
            // frmSetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(671, 405);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.pnlPropertyPage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tvSettings);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetupDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Video Simulator Setup";
            this.Load += new System.EventHandler(this.frmSetupDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.PictureBox picASCOM;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Panel pnlPropertyPage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TreeView tvSettings;
	}
}