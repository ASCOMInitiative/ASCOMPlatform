namespace ASCOM.Setup
{
	partial class DeviceDriverForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceDriverForm));
			this.cbDeviceClass = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDeviceName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkUseVsSettings = new System.Windows.Forms.CheckBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.lblDeviceId = new System.Windows.Forms.Label();
			this.txtOrganizationName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnCreate = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// cbDeviceClass
			// 
			this.cbDeviceClass.FormattingEnabled = true;
			this.cbDeviceClass.Items.AddRange(new object[] {
            "Camera",
            "Dome",
            "FilterWheel",
            "Focuser",
            "PortSelector",
            "SafetyMonitor",
            "Switch",
            "Telescope"});
			this.cbDeviceClass.Location = new System.Drawing.Point(181, 36);
			this.cbDeviceClass.Name = "cbDeviceClass";
			this.cbDeviceClass.Size = new System.Drawing.Size(174, 21);
			this.cbDeviceClass.Sorted = true;
			this.cbDeviceClass.TabIndex = 2;
			this.toolTip.SetToolTip(this.cbDeviceClass, "The DeviceClass specifies the general type of device\r\nand determines which interf" +
					"ace the driver must implement.\r\nThis field is required.");
			this.cbDeviceClass.SelectedIndexChanged += new System.EventHandler(this.cbDeviceClass_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Device Class";
			// 
			// txtDeviceName
			// 
			this.txtDeviceName.CausesValidation = false;
			this.txtDeviceName.Location = new System.Drawing.Point(181, 89);
			this.txtDeviceName.Name = "txtDeviceName";
			this.txtDeviceName.Size = new System.Drawing.Size(174, 20);
			this.txtDeviceName.TabIndex = 4;
			this.toolTip.SetToolTip(this.txtDeviceName, "Specifies the unique name for the driver. This field must\r\nbe completed and shoul" +
					"d reflect the device model\r\nname or number.\r\nExample: LX200");
			this.txtDeviceName.TextChanged += new System.EventHandler(this.txtDeviceName_TextChanged);
			this.txtDeviceName.Validated += new System.EventHandler(this.txtDeviceName_Validated);
			this.txtDeviceName.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeviceName_Validating);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Device Name/Model";
			// 
			// chkUseVsSettings
			// 
			this.chkUseVsSettings.AutoSize = true;
			this.chkUseVsSettings.Checked = true;
			this.chkUseVsSettings.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUseVsSettings.Enabled = false;
			this.chkUseVsSettings.Location = new System.Drawing.Point(181, 116);
			this.chkUseVsSettings.Name = "chkUseVsSettings";
			this.chkUseVsSettings.Size = new System.Drawing.Size(180, 17);
			this.chkUseVsSettings.TabIndex = 5;
			this.chkUseVsSettings.Text = "Use Visual Studio Settings Editor";
			this.toolTip.SetToolTip(this.chkUseVsSettings, "Select this option if you want your driver to use the\r\nsettings editor built into" +
					" Visual Studio (recommended).\r\nClear the box if you prefer to code your settings" +
					"\r\nlogic manually, using ASCOM.Profile.");
			this.chkUseVsSettings.UseVisualStyleBackColor = true;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(392, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Please specify the DeviceName and DeviceClass of the driver you wish to create:";
			// 
			// toolTip
			// 
			this.toolTip.AutoPopDelay = 20000;
			this.toolTip.InitialDelay = 500;
			this.toolTip.IsBalloon = true;
			this.toolTip.ReshowDelay = 100;
			// 
			// lblDeviceId
			// 
			this.lblDeviceId.AutoSize = true;
			this.lblDeviceId.Location = new System.Drawing.Point(181, 140);
			this.lblDeviceId.Name = "lblDeviceId";
			this.lblDeviceId.Size = new System.Drawing.Size(196, 13);
			this.lblDeviceId.TabIndex = 7;
			this.lblDeviceId.Text = "ASCOM.<DeviceName>.<DeviceClass>";
			this.toolTip.SetToolTip(this.lblDeviceId, "This is the fully qualified ASCOM DeviceId\r\n(COM ProgID) of your new driver.");
			// 
			// txtOrganizationName
			// 
			this.txtOrganizationName.Location = new System.Drawing.Point(181, 63);
			this.txtOrganizationName.Name = "txtOrganizationName";
			this.txtOrganizationName.Size = new System.Drawing.Size(174, 20);
			this.txtOrganizationName.TabIndex = 4;
			this.toolTip.SetToolTip(this.txtOrganizationName, resources.GetString("txtOrganizationName.ToolTip"));
			this.txtOrganizationName.TextChanged += new System.EventHandler(this.txtDeviceName_TextChanged);
			this.txtOrganizationName.Validated += new System.EventHandler(this.txtOrganizationName_Validated);
			this.txtOrganizationName.Validating += new System.ComponentModel.CancelEventHandler(this.txtOrganizationName_Validating);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(19, 139);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(91, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "ASCOM DeviceId";
			// 
			// btnCreate
			// 
			this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnCreate.Location = new System.Drawing.Point(181, 188);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(75, 23);
			this.btnCreate.TabIndex = 8;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(105, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Vendor/Organization";
			// 
			// UserInputForm
			// 
			this.AcceptButton = this.btnCreate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 247);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblDeviceId);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chkUseVsSettings);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtOrganizationName);
			this.Controls.Add(this.txtDeviceName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbDeviceClass);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "UserInputForm";
			this.Text = "ASCOM Driver Project Wizard";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbDeviceClass;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDeviceName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkUseVsSettings;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblDeviceId;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtOrganizationName;
	}
}