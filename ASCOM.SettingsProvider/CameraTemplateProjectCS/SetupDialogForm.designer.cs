namespace ASCOM.CameraTemplateProjectCS
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
			this.label1 = new System.Windows.Forms.Label();
			this.picASCOM = new System.Windows.Forms.PictureBox();
			this.lblCommPort = new System.Windows.Forms.Label();
			this.txtCommPort = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(208, 111);
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
			this.cmdCancel.Location = new System.Drawing.Point(208, 141);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(59, 25);
			this.cmdCancel.TabIndex = 1;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(123, 31);
			this.label1.TabIndex = 2;
			this.label1.Text = "Construct your driver\'s setup dialog here.";
			// 
			// picASCOM
			// 
			this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picASCOM.Image = global::ASCOM.CameraTemplateProjectCS.Properties.Resources.ASCOM;
			this.picASCOM.Location = new System.Drawing.Point(219, 9);
			this.picASCOM.Name = "picASCOM";
			this.picASCOM.Size = new System.Drawing.Size(48, 56);
			this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picASCOM.TabIndex = 3;
			this.picASCOM.TabStop = false;
			this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
			this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
			// 
			// lblCommPort
			// 
			this.lblCommPort.AutoSize = true;
			this.lblCommPort.Location = new System.Drawing.Point(12, 54);
			this.lblCommPort.Name = "lblCommPort";
			this.lblCommPort.Size = new System.Drawing.Size(58, 13);
			this.lblCommPort.TabIndex = 4;
			this.lblCommPort.Text = "Comm Port";
			// 
			// txtCommPort
			// 
			this.txtCommPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.CameraTemplateProjectCS.Properties.Settings.Default, "CommPortName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtCommPort.Location = new System.Drawing.Point(78, 51);
			this.txtCommPort.Name = "txtCommPort";
			this.txtCommPort.Size = new System.Drawing.Size(100, 20);
			this.txtCommPort.TabIndex = 5;
			this.txtCommPort.Text = global::ASCOM.CameraTemplateProjectCS.Properties.Settings.Default.CommPortName;
			// 
			// SetupDialogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(277, 174);
			this.Controls.Add(this.txtCommPort);
			this.Controls.Add(this.lblCommPort);
			this.Controls.Add(this.picASCOM);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetupDialogForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CameraTemplateProjectCS Setup";
			((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox picASCOM;
		private System.Windows.Forms.Label lblCommPort;
		private System.Windows.Forms.TextBox txtCommPort;
		}
	}