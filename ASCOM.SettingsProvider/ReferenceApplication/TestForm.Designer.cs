namespace ReferenceApplication
	{
	partial class TestForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
			this.btnChoose = new System.Windows.Forms.Button();
			this.lblSelectedDeviceID = new System.Windows.Forms.Label();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnUnload = new System.Windows.Forms.Button();
			this.cbDriverType = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnChoose
			// 
			this.btnChoose.Location = new System.Drawing.Point(150, 13);
			this.btnChoose.Name = "btnChoose";
			this.btnChoose.Size = new System.Drawing.Size(97, 23);
			this.btnChoose.TabIndex = 1;
			this.btnChoose.Text = "Choose Driver...";
			this.btnChoose.UseVisualStyleBackColor = true;
			this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
			// 
			// lblSelectedDeviceID
			// 
			this.lblSelectedDeviceID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblSelectedDeviceID.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ReferenceApplication.Properties.Settings.Default, "DeviceID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblSelectedDeviceID.Location = new System.Drawing.Point(12, 39);
			this.lblSelectedDeviceID.Name = "lblSelectedDeviceID";
			this.lblSelectedDeviceID.Size = new System.Drawing.Size(235, 23);
			this.lblSelectedDeviceID.TabIndex = 0;
			this.lblSelectedDeviceID.Text = global::ReferenceApplication.Properties.Settings.Default.DeviceID;
			this.lblSelectedDeviceID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(47, 72);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(97, 23);
			this.btnLoad.TabIndex = 2;
			this.btnLoad.Text = "Load Driver";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnUnload
			// 
			this.btnUnload.Location = new System.Drawing.Point(150, 72);
			this.btnUnload.Name = "btnUnload";
			this.btnUnload.Size = new System.Drawing.Size(97, 23);
			this.btnUnload.TabIndex = 2;
			this.btnUnload.Text = "Unload Driver";
			this.btnUnload.UseVisualStyleBackColor = true;
			// 
			// cbDriverType
			// 
			this.cbDriverType.FormattingEnabled = true;
			this.cbDriverType.Location = new System.Drawing.Point(13, 13);
			this.cbDriverType.Name = "cbDriverType";
			this.cbDriverType.Size = new System.Drawing.Size(131, 21);
			this.cbDriverType.TabIndex = 3;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(259, 118);
			this.Controls.Add(this.cbDriverType);
			this.Controls.Add(this.btnUnload);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnChoose);
			this.Controls.Add(this.lblSelectedDeviceID);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TestForm";
			this.Text = "ASCOM.SettingsProvider Test";
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.Label lblSelectedDeviceID;
		private System.Windows.Forms.Button btnChoose;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnUnload;
		private System.Windows.Forms.ComboBox cbDriverType;

		}
	}

