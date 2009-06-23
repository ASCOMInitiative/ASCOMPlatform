namespace ASCOM.Controls.Demo
	{
	partial class frmDemo
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ctlCameraChooser = new ASCOM.Controls.Chooser();
			this.ctlTelescopeChooser = new ASCOM.Controls.Chooser();
			this.ledTelescopeChosen = new ASCOM.Controls.LEDIndicator();
			this.ledCameraChosen = new ASCOM.Controls.LEDIndicator();
			this.ledTelescopeConfigured = new ASCOM.Controls.LEDIndicator();
			this.ledCameraConfigured = new ASCOM.Controls.LEDIndicator();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(203, 331);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(284, 331);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ctlCameraChooser
			// 
			this.ctlCameraChooser.DeviceClass = "Camera";
			this.ctlCameraChooser.DeviceID = "";
			this.ctlCameraChooser.Location = new System.Drawing.Point(12, 122);
			this.ctlCameraChooser.Name = "ctlCameraChooser";
			this.ctlCameraChooser.Size = new System.Drawing.Size(344, 104);
			this.ctlCameraChooser.TabIndex = 3;
			this.ctlCameraChooser.SelectionChanged += new System.EventHandler(this.ctlCameraChooser_SelectionChanged);
			// 
			// ctlTelescopeChooser
			// 
			this.ctlTelescopeChooser.DeviceClass = "Telescope";
			this.ctlTelescopeChooser.DeviceID = "";
			this.ctlTelescopeChooser.Location = new System.Drawing.Point(12, 12);
			this.ctlTelescopeChooser.Name = "ctlTelescopeChooser";
			this.ctlTelescopeChooser.Size = new System.Drawing.Size(344, 104);
			this.ctlTelescopeChooser.TabIndex = 0;
			this.ctlTelescopeChooser.SelectionChanged += new System.EventHandler(this.ctlTelescopeChooser_SelectionChanged);
			// 
			// ledTelescopeChosen
			// 
			this.ledTelescopeChosen.Cadence = ASCOM.Controls.CadencePattern.Wink;
			this.ledTelescopeChosen.Green = false;
			this.ledTelescopeChosen.LabelText = "Telescope Chosen";
			this.ledTelescopeChosen.Location = new System.Drawing.Point(12, 232);
			this.ledTelescopeChosen.Name = "ledTelescopeChosen";
			this.ledTelescopeChosen.Red = true;
			this.ledTelescopeChosen.Size = new System.Drawing.Size(134, 16);
			this.ledTelescopeChosen.TabIndex = 4;
			// 
			// ledCameraChosen
			// 
			this.ledCameraChosen.Cadence = ASCOM.Controls.CadencePattern.Wink;
			this.ledCameraChosen.Green = false;
			this.ledCameraChosen.LabelText = "Camera Chosen";
			this.ledCameraChosen.Location = new System.Drawing.Point(12, 254);
			this.ledCameraChosen.Name = "ledCameraChosen";
			this.ledCameraChosen.Red = true;
			this.ledCameraChosen.Size = new System.Drawing.Size(134, 16);
			this.ledCameraChosen.TabIndex = 4;
			// 
			// ledTelescopeConfigured
			// 
			this.ledTelescopeConfigured.Cadence = ASCOM.Controls.CadencePattern.Wink;
			this.ledTelescopeConfigured.Green = false;
			this.ledTelescopeConfigured.LabelText = "Telescope Configured";
			this.ledTelescopeConfigured.Location = new System.Drawing.Point(152, 232);
			this.ledTelescopeConfigured.Name = "ledTelescopeConfigured";
			this.ledTelescopeConfigured.Red = true;
			this.ledTelescopeConfigured.Size = new System.Drawing.Size(134, 16);
			this.ledTelescopeConfigured.TabIndex = 4;
			// 
			// ledCameraConfigured
			// 
			this.ledCameraConfigured.Cadence = ASCOM.Controls.CadencePattern.Wink;
			this.ledCameraConfigured.Green = false;
			this.ledCameraConfigured.LabelText = "Camera Configured";
			this.ledCameraConfigured.Location = new System.Drawing.Point(152, 254);
			this.ledCameraConfigured.Name = "ledCameraConfigured";
			this.ledCameraConfigured.Red = true;
			this.ledCameraConfigured.Size = new System.Drawing.Size(134, 16);
			this.ledCameraConfigured.TabIndex = 4;
			// 
			// frmDemo
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(371, 366);
			this.Controls.Add(this.ledCameraConfigured);
			this.Controls.Add(this.ledCameraChosen);
			this.Controls.Add(this.ledTelescopeConfigured);
			this.Controls.Add(this.ledTelescopeChosen);
			this.Controls.Add(this.ctlCameraChooser);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.ctlTelescopeChooser);
			this.Name = "frmDemo";
			this.Text = "ASCOM Common Controls Demo";
			this.Load += new System.EventHandler(this.frmDemo_Load);
			this.ResumeLayout(false);

			}

		#endregion

		private Chooser ctlTelescopeChooser;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private Chooser ctlCameraChooser;
		private LEDIndicator ledTelescopeChosen;
		private LEDIndicator ledCameraChosen;
		private LEDIndicator ledTelescopeConfigured;
		private LEDIndicator ledCameraConfigured;
		}
	}

