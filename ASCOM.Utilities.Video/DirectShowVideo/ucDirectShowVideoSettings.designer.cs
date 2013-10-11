namespace ASCOM.Utilities.Video.DirectShowVideo
{
	partial class ucDirectShowVideoSettings
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbxCrossbarInput = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.gbxCompression = new System.Windows.Forms.GroupBox();
			this.rbCompressionUnsupported = new System.Windows.Forms.RadioButton();
			this.cbxOtherCodecs = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.rbCompressionHuffyuv211 = new System.Windows.Forms.RadioButton();
			this.rbCompressionXviD = new System.Windows.Forms.RadioButton();
			this.rbCompressionDV = new System.Windows.Forms.RadioButton();
			this.rbCompressionUncompressed = new System.Windows.Forms.RadioButton();
			this.btnCompressorProps = new System.Windows.Forms.Button();
			this.btnInputPros = new System.Windows.Forms.Button();
			this.cbxCaptureDevices = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cbxVideoFormats = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbxSensorType = new System.Windows.Forms.ComboBox();
			this.pnlBWPixels = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.cbxMonochromePixelsFrom = new System.Windows.Forms.ComboBox();
			this.gbxCompression.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.pnlBWPixels.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbxCrossbarInput
			// 
			this.cbxCrossbarInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxCrossbarInput.Enabled = false;
			this.cbxCrossbarInput.Items.AddRange(new object[] {
            "Automatic"});
			this.cbxCrossbarInput.Location = new System.Drawing.Point(7, 66);
			this.cbxCrossbarInput.Name = "cbxCrossbarInput";
			this.cbxCrossbarInput.Size = new System.Drawing.Size(220, 21);
			this.cbxCrossbarInput.TabIndex = 41;
			this.cbxCrossbarInput.SelectedIndexChanged += new System.EventHandler(this.cbxCrossbarInput_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(4, 50);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 13);
			this.label5.TabIndex = 40;
			this.label5.Text = "Crossbar Input Source";
			// 
			// gbxCompression
			// 
			this.gbxCompression.Controls.Add(this.rbCompressionUnsupported);
			this.gbxCompression.Controls.Add(this.cbxOtherCodecs);
			this.gbxCompression.Controls.Add(this.label1);
			this.gbxCompression.Controls.Add(this.label4);
			this.gbxCompression.Controls.Add(this.rbCompressionHuffyuv211);
			this.gbxCompression.Controls.Add(this.rbCompressionXviD);
			this.gbxCompression.Controls.Add(this.rbCompressionDV);
			this.gbxCompression.Controls.Add(this.rbCompressionUncompressed);
			this.gbxCompression.Controls.Add(this.btnCompressorProps);
			this.gbxCompression.Location = new System.Drawing.Point(7, 147);
			this.gbxCompression.Name = "gbxCompression";
			this.gbxCompression.Size = new System.Drawing.Size(468, 193);
			this.gbxCompression.TabIndex = 39;
			this.gbxCompression.TabStop = false;
			this.gbxCompression.Text = "Video Compression";
			// 
			// rbCompressionUnsupported
			// 
			this.rbCompressionUnsupported.AutoSize = true;
			this.rbCompressionUnsupported.Location = new System.Drawing.Point(17, 162);
			this.rbCompressionUnsupported.Name = "rbCompressionUnsupported";
			this.rbCompressionUnsupported.Size = new System.Drawing.Size(14, 13);
			this.rbCompressionUnsupported.TabIndex = 32;
			this.rbCompressionUnsupported.UseVisualStyleBackColor = true;
			this.rbCompressionUnsupported.CheckedChanged += new System.EventHandler(this.rbCompressionUnsupported_CheckedChanged);
			// 
			// cbxOtherCodecs
			// 
			this.cbxOtherCodecs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxOtherCodecs.Enabled = false;
			this.cbxOtherCodecs.Location = new System.Drawing.Point(37, 159);
			this.cbxOtherCodecs.Name = "cbxOtherCodecs";
			this.cbxOtherCodecs.Size = new System.Drawing.Size(238, 21);
			this.cbxOtherCodecs.TabIndex = 31;
			this.cbxOtherCodecs.SelectedIndexChanged += new System.EventHandler(this.cbxOtherCodecs_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(436, 39);
			this.label1.TabIndex = 24;
			this.label1.Text = "The following codecs are supported by this driver. If any of the items appear dis" +
    "abled this means that the corresponding codecs have not been installed on this s" +
    "ystem.";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(305, 13);
			this.label4.TabIndex = 30;
			this.label4.Text = "The following codecs may work but there is no support for them";
			// 
			// rbCompressionHuffyuv211
			// 
			this.rbCompressionHuffyuv211.AutoSize = true;
			this.rbCompressionHuffyuv211.Location = new System.Drawing.Point(142, 97);
			this.rbCompressionHuffyuv211.Name = "rbCompressionHuffyuv211";
			this.rbCompressionHuffyuv211.Size = new System.Drawing.Size(95, 17);
			this.rbCompressionHuffyuv211.TabIndex = 3;
			this.rbCompressionHuffyuv211.Text = "Huffyuv v2.1.1";
			this.rbCompressionHuffyuv211.UseVisualStyleBackColor = true;
			this.rbCompressionHuffyuv211.CheckedChanged += new System.EventHandler(this.SelectedCodecChanged);
			// 
			// rbCompressionXviD
			// 
			this.rbCompressionXviD.AutoSize = true;
			this.rbCompressionXviD.Location = new System.Drawing.Point(142, 74);
			this.rbCompressionXviD.Name = "rbCompressionXviD";
			this.rbCompressionXviD.Size = new System.Drawing.Size(123, 17);
			this.rbCompressionXviD.TabIndex = 2;
			this.rbCompressionXviD.Text = "Xvid MPEG-4 Codec";
			this.rbCompressionXviD.UseVisualStyleBackColor = true;
			this.rbCompressionXviD.CheckedChanged += new System.EventHandler(this.SelectedCodecChanged);
			// 
			// rbCompressionDV
			// 
			this.rbCompressionDV.AutoSize = true;
			this.rbCompressionDV.Location = new System.Drawing.Point(17, 97);
			this.rbCompressionDV.Name = "rbCompressionDV";
			this.rbCompressionDV.Size = new System.Drawing.Size(113, 17);
			this.rbCompressionDV.TabIndex = 1;
			this.rbCompressionDV.Text = "DV Video Encoder";
			this.rbCompressionDV.UseVisualStyleBackColor = true;
			this.rbCompressionDV.CheckedChanged += new System.EventHandler(this.SelectedCodecChanged);
			// 
			// rbCompressionUncompressed
			// 
			this.rbCompressionUncompressed.AutoSize = true;
			this.rbCompressionUncompressed.Checked = true;
			this.rbCompressionUncompressed.Location = new System.Drawing.Point(17, 74);
			this.rbCompressionUncompressed.Name = "rbCompressionUncompressed";
			this.rbCompressionUncompressed.Size = new System.Drawing.Size(96, 17);
			this.rbCompressionUncompressed.TabIndex = 0;
			this.rbCompressionUncompressed.TabStop = true;
			this.rbCompressionUncompressed.Text = "Uncompressed";
			this.rbCompressionUncompressed.UseVisualStyleBackColor = true;
			this.rbCompressionUncompressed.CheckedChanged += new System.EventHandler(this.SelectedCodecChanged);
			// 
			// btnCompressorProps
			// 
			this.btnCompressorProps.Location = new System.Drawing.Point(343, 116);
			this.btnCompressorProps.Name = "btnCompressorProps";
			this.btnCompressorProps.Size = new System.Drawing.Size(117, 43);
			this.btnCompressorProps.TabIndex = 23;
			this.btnCompressorProps.Text = "Codec properties";
			this.btnCompressorProps.Click += new System.EventHandler(this.btnCompressorProps_Click);
			// 
			// btnInputPros
			// 
			this.btnInputPros.Location = new System.Drawing.Point(249, 18);
			this.btnInputPros.Name = "btnInputPros";
			this.btnInputPros.Size = new System.Drawing.Size(133, 24);
			this.btnInputPros.TabIndex = 35;
			this.btnInputPros.Text = "Show property pages";
			this.btnInputPros.Click += new System.EventHandler(this.btnInputPros_Click);
			// 
			// cbxCaptureDevices
			// 
			this.cbxCaptureDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxCaptureDevices.Location = new System.Drawing.Point(6, 21);
			this.cbxCaptureDevices.Name = "cbxCaptureDevices";
			this.cbxCaptureDevices.Size = new System.Drawing.Size(221, 21);
			this.cbxCaptureDevices.TabIndex = 34;
			this.cbxCaptureDevices.SelectedIndexChanged += new System.EventHandler(this.cbxCaptureDevices_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 5);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(61, 13);
			this.label8.TabIndex = 33;
			this.label8.Text = "Video Input";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(4, 97);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(166, 13);
			this.label6.TabIndex = 43;
			this.label6.Text = "Video Resolution and Frame Rate";
			// 
			// cbxVideoFormats
			// 
			this.cbxVideoFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxVideoFormats.FormattingEnabled = true;
			this.cbxVideoFormats.Location = new System.Drawing.Point(7, 115);
			this.cbxVideoFormats.Name = "cbxVideoFormats";
			this.cbxVideoFormats.Size = new System.Drawing.Size(220, 21);
			this.cbxVideoFormats.TabIndex = 42;
			this.cbxVideoFormats.SelectedIndexChanged += new System.EventHandler(this.cbxVideoFormats_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pnlBWPixels);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cbxSensorType);
			this.groupBox1.Location = new System.Drawing.Point(245, 57);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(230, 84);
			this.groupBox1.TabIndex = 44;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ImageArray Format";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 39;
			this.label2.Text = "Sensor Type";
			// 
			// cbxSensorType
			// 
			this.cbxSensorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSensorType.Location = new System.Drawing.Point(82, 24);
			this.cbxSensorType.Name = "cbxSensorType";
			this.cbxSensorType.Size = new System.Drawing.Size(140, 21);
			this.cbxSensorType.TabIndex = 38;
			this.cbxSensorType.SelectedIndexChanged += new System.EventHandler(this.cbSensorType_SelectedIndexChanged);
			// 
			// pnlBWPixels
			// 
			this.pnlBWPixels.Controls.Add(this.label3);
			this.pnlBWPixels.Controls.Add(this.cbxMonochromePixelsFrom);
			this.pnlBWPixels.Location = new System.Drawing.Point(6, 50);
			this.pnlBWPixels.Name = "pnlBWPixels";
			this.pnlBWPixels.Size = new System.Drawing.Size(218, 27);
			this.pnlBWPixels.TabIndex = 40;
			this.pnlBWPixels.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(33, 13);
			this.label3.TabIndex = 28;
			this.label3.Text = "Luma";
			// 
			// cbxMonochromePixelsFrom
			// 
			this.cbxMonochromePixelsFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxMonochromePixelsFrom.Location = new System.Drawing.Point(76, 3);
			this.cbxMonochromePixelsFrom.Name = "cbxMonochromePixelsFrom";
			this.cbxMonochromePixelsFrom.Size = new System.Drawing.Size(140, 21);
			this.cbxMonochromePixelsFrom.TabIndex = 27;
			// 
			// ucDirectShowVideoSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.cbxVideoFormats);
			this.Controls.Add(this.cbxCrossbarInput);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gbxCompression);
			this.Controls.Add(this.btnInputPros);
			this.Controls.Add(this.cbxCaptureDevices);
			this.Controls.Add(this.label8);
			this.Name = "ucDirectShowVideoSettings";
			this.Size = new System.Drawing.Size(481, 346);
			this.Load += new System.EventHandler(this.ucDirectShowVideoSettings_Load);
			this.gbxCompression.ResumeLayout(false);
			this.gbxCompression.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.pnlBWPixels.ResumeLayout(false);
			this.pnlBWPixels.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbxCrossbarInput;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox gbxCompression;
		private System.Windows.Forms.RadioButton rbCompressionUnsupported;
		private System.Windows.Forms.ComboBox cbxOtherCodecs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbCompressionHuffyuv211;
		private System.Windows.Forms.RadioButton rbCompressionXviD;
		private System.Windows.Forms.RadioButton rbCompressionDV;
		private System.Windows.Forms.RadioButton rbCompressionUncompressed;
		private System.Windows.Forms.Button btnCompressorProps;
		private System.Windows.Forms.Button btnInputPros;
		private System.Windows.Forms.ComboBox cbxCaptureDevices;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbxVideoFormats;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel pnlBWPixels;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbxMonochromePixelsFrom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbxSensorType;
	}
}
