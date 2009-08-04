namespace ASCOM.InstallerGen
{
	partial class MainForm
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
			this.cmdSave = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cbDriverTechnology = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDriverVersion = new System.Windows.Forms.TextBox();
			this.cbDriverType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDriverName = new System.Windows.Forms.TextBox();
			this.cmdBrowseReadMe = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.txtReadMeFile = new System.Windows.Forms.TextBox();
			this.cmdBrowseDriverFile = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtDriverFile = new System.Windows.Forms.TextBox();
			this.cmdBrowseSourceFolder = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.txtSourceFolder = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtDeveloperEmail = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtDeveloperName = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.pbAscomLogo = new System.Windows.Forms.PictureBox();
			this.label18 = new System.Windows.Forms.Label();
			this.fldrBrowse = new System.Windows.Forms.FolderBrowserDialog();
			this.llblInno = new System.Windows.Forms.LinkLabel();
			this.lblVersion = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.txtDrvrShortName = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cbDriverType2 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pbAscomLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdSave
			// 
			this.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdSave.Location = new System.Drawing.Point(315, 461);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(75, 23);
			this.cmdSave.TabIndex = 15;
			this.cmdSave.Text = "Save";
			this.toolTip1.SetToolTip(this.cmdSave, "Create the Inno Setup script and open in Inno Setup for further editing");
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(315, 490);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 16;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cbDriverTechnology
			// 
			this.cbDriverTechnology.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDriverTechnology.FormattingEnabled = true;
			this.cbDriverTechnology.Items.AddRange(new object[] {
            "In-process COM  (dll)",
            "Local server COM  (exe)",
            ".NET assembly  (dll)",
            ".NET local server  (exe)"});
			this.cbDriverTechnology.Location = new System.Drawing.Point(129, 130);
			this.cbDriverTechnology.Name = "cbDriverTechnology";
			this.cbDriverTechnology.Size = new System.Drawing.Size(155, 21);
			this.cbDriverTechnology.TabIndex = 0;
			this.toolTip1.SetToolTip(this.cbDriverTechnology, "The technology and type of the driver");
			this.cbDriverTechnology.SelectedIndexChanged += new System.EventHandler(this.cbDriverTechnology_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(46, 133);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(66, 13);
			this.label8.TabIndex = 28;
			this.label8.Text = "Technology:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(214, 273);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 13);
			this.label5.TabIndex = 27;
			this.label5.Text = "(e.g., \"5.0.1\")";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(47, 273);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 13);
			this.label4.TabIndex = 26;
			this.label4.Text = "Version:";
			// 
			// txtDriverVersion
			// 
			this.txtDriverVersion.Location = new System.Drawing.Point(130, 270);
			this.txtDriverVersion.Name = "txtDriverVersion";
			this.txtDriverVersion.Size = new System.Drawing.Size(64, 20);
			this.txtDriverVersion.TabIndex = 5;
			this.toolTip1.SetToolTip(this.txtDriverVersion, "The version of the driver");
			this.txtDriverVersion.TextChanged += new System.EventHandler(this.txtDriverVersion_TextChanged);
			// 
			// cbDriverType
			// 
			this.cbDriverType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDriverType.FormattingEnabled = true;
			this.cbDriverType.Items.AddRange(new object[] {
            "Camera",
            "Dome",
            "FilterWheel",
            "Focuser",
            "Rotator",
            "Switches",
            "Telescope"});
			this.cbDriverType.Location = new System.Drawing.Point(130, 214);
			this.cbDriverType.Name = "cbDriverType";
			this.cbDriverType.Size = new System.Drawing.Size(115, 21);
			this.cbDriverType.Sorted = true;
			this.cbDriverType.TabIndex = 3;
			this.toolTip1.SetToolTip(this.cbDriverType, "The last part of the driver\'s primary ProgID");
			this.cbDriverType.SelectedIndexChanged += new System.EventHandler(this.cbDriverType_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(47, 217);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 13);
			this.label3.TabIndex = 23;
			this.label3.Text = "Type:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(47, 161);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Friendly Name:";
			// 
			// txtDriverName
			// 
			this.txtDriverName.Location = new System.Drawing.Point(130, 158);
			this.txtDriverName.Name = "txtDriverName";
			this.txtDriverName.Size = new System.Drawing.Size(260, 20);
			this.txtDriverName.TabIndex = 1;
			this.toolTip1.SetToolTip(this.txtDriverName, "(COM only) Descriptive title that users will see in the Chooser");
			this.txtDriverName.TextChanged += new System.EventHandler(this.txtDriverName_TextChanged);
			// 
			// cmdBrowseReadMe
			// 
			this.cmdBrowseReadMe.Location = new System.Drawing.Point(315, 384);
			this.cmdBrowseReadMe.Name = "cmdBrowseReadMe";
			this.cmdBrowseReadMe.Size = new System.Drawing.Size(75, 23);
			this.cmdBrowseReadMe.TabIndex = 11;
			this.cmdBrowseReadMe.Text = "Browse...";
			this.toolTip1.SetToolTip(this.cmdBrowseReadMe, "Select the driver read-me file (HTML or text)");
			this.cmdBrowseReadMe.UseVisualStyleBackColor = true;
			this.cmdBrowseReadMe.Click += new System.EventHandler(this.cmdBrowseReadMe_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(47, 389);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(70, 13);
			this.label11.TabIndex = 37;
			this.label11.Text = "ReadMe File:";
			// 
			// txtReadMeFile
			// 
			this.txtReadMeFile.Location = new System.Drawing.Point(130, 386);
			this.txtReadMeFile.Name = "txtReadMeFile";
			this.txtReadMeFile.Size = new System.Drawing.Size(179, 20);
			this.txtReadMeFile.TabIndex = 10;
			this.toolTip1.SetToolTip(this.txtReadMeFile, "The read-me file for the driver (HTML or text)");
			this.txtReadMeFile.TextChanged += new System.EventHandler(this.txtReadMeFile_TextChanged);
			// 
			// cmdBrowseDriverFile
			// 
			this.cmdBrowseDriverFile.Location = new System.Drawing.Point(315, 355);
			this.cmdBrowseDriverFile.Name = "cmdBrowseDriverFile";
			this.cmdBrowseDriverFile.Size = new System.Drawing.Size(75, 23);
			this.cmdBrowseDriverFile.TabIndex = 9;
			this.cmdBrowseDriverFile.Text = "Browse...";
			this.toolTip1.SetToolTip(this.cmdBrowseDriverFile, "Select the driver DLL or EXE file");
			this.cmdBrowseDriverFile.UseVisualStyleBackColor = true;
			this.cmdBrowseDriverFile.Click += new System.EventHandler(this.cmdBrowseDriverFile_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(47, 360);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(83, 13);
			this.label10.TabIndex = 34;
			this.label10.Text = "Main Driver File:";
			// 
			// txtDriverFile
			// 
			this.txtDriverFile.Location = new System.Drawing.Point(130, 357);
			this.txtDriverFile.Name = "txtDriverFile";
			this.txtDriverFile.Size = new System.Drawing.Size(179, 20);
			this.txtDriverFile.TabIndex = 8;
			this.toolTip1.SetToolTip(this.txtDriverFile, "The driver DLL or EXE file");
			this.txtDriverFile.TextChanged += new System.EventHandler(this.txtDriverFile_TextChanged);
			// 
			// cmdBrowseSourceFolder
			// 
			this.cmdBrowseSourceFolder.Location = new System.Drawing.Point(315, 326);
			this.cmdBrowseSourceFolder.Name = "cmdBrowseSourceFolder";
			this.cmdBrowseSourceFolder.Size = new System.Drawing.Size(75, 23);
			this.cmdBrowseSourceFolder.TabIndex = 7;
			this.cmdBrowseSourceFolder.Text = "Browse...";
			this.toolTip1.SetToolTip(this.cmdBrowseSourceFolder, "Select the folder containing the driver and (optionally) the read-me file");
			this.cmdBrowseSourceFolder.UseVisualStyleBackColor = true;
			this.cmdBrowseSourceFolder.Click += new System.EventHandler(this.cmdBrowseSourceFolder_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(47, 331);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(76, 13);
			this.label9.TabIndex = 31;
			this.label9.Text = "Source Folder:";
			// 
			// txtSourceFolder
			// 
			this.txtSourceFolder.Location = new System.Drawing.Point(130, 328);
			this.txtSourceFolder.Name = "txtSourceFolder";
			this.txtSourceFolder.Size = new System.Drawing.Size(179, 20);
			this.txtSourceFolder.TabIndex = 6;
			this.toolTip1.SetToolTip(this.txtSourceFolder, "Location of the driver itself and (optionally) the read-me for the driver.\r\nFor ." +
					"NET the driver assembly is assumed to be in the ..\\bin\\Release subfolder.");
			this.txtSourceFolder.TextChanged += new System.EventHandler(this.txtSourceFolder_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(86, 497);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 13);
			this.label7.TabIndex = 42;
			this.label7.Text = "Email:";
			// 
			// txtDeveloperEmail
			// 
			this.txtDeveloperEmail.Location = new System.Drawing.Point(130, 492);
			this.txtDeveloperEmail.Name = "txtDeveloperEmail";
			this.txtDeveloperEmail.Size = new System.Drawing.Size(179, 20);
			this.txtDeveloperEmail.TabIndex = 14;
			this.toolTip1.SetToolTip(this.txtDeveloperEmail, "Your email address");
			this.txtDeveloperEmail.TextChanged += new System.EventHandler(this.txtDeveloperEmail_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(86, 466);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 40;
			this.label6.Text = "Name:";
			// 
			// txtDeveloperName
			// 
			this.txtDeveloperName.Location = new System.Drawing.Point(130, 463);
			this.txtDeveloperName.Name = "txtDeveloperName";
			this.txtDeveloperName.Size = new System.Drawing.Size(179, 20);
			this.txtDeveloperName.TabIndex = 13;
			this.toolTip1.SetToolTip(this.txtDeveloperName, "Your name");
			this.txtDeveloperName.TextChanged += new System.EventHandler(this.txtDeveloperName_TextChanged);
			// 
			// label12
			// 
			this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label12.Location = new System.Drawing.Point(130, 309);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(260, 2);
			this.label12.TabIndex = 43;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(12, 302);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(81, 13);
			this.label13.TabIndex = 44;
			this.label13.Text = "Source Files:";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(12, 105);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(112, 13);
			this.label14.TabIndex = 45;
			this.label14.Text = "Driver Information:";
			// 
			// label15
			// 
			this.label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label15.Location = new System.Drawing.Point(130, 112);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(260, 2);
			this.label15.TabIndex = 46;
			// 
			// label16
			// 
			this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label16.Location = new System.Drawing.Point(130, 445);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(180, 2);
			this.label16.TabIndex = 47;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(12, 438);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(107, 13);
			this.label17.TabIndex = 48;
			this.label17.Text = "Driver Developer:";
			// 
			// pbAscomLogo
			// 
			this.pbAscomLogo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbAscomLogo.Image = global::ASCOM.InstallerGen.Properties.Resources.Bug72T_sm;
			this.pbAscomLogo.Location = new System.Drawing.Point(342, 16);
			this.pbAscomLogo.Name = "pbAscomLogo";
			this.pbAscomLogo.Size = new System.Drawing.Size(48, 56);
			this.pbAscomLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbAscomLogo.TabIndex = 49;
			this.pbAscomLogo.TabStop = false;
			this.pbAscomLogo.Click += new System.EventHandler(this.pbAscomLogo_Click);
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(12, 12);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(305, 29);
			this.label18.TabIndex = 50;
			this.label18.Text = "Generate an Inno Setup script for installing an ASCOM driver. You may need to adj" +
				"ust it for additional files to be installed.";
			// 
			// llblInno
			// 
			this.llblInno.AutoSize = true;
			this.llblInno.Location = new System.Drawing.Point(48, 79);
			this.llblInno.Name = "llblInno";
			this.llblInno.Size = new System.Drawing.Size(317, 13);
			this.llblInno.TabIndex = 51;
			this.llblInno.TabStop = true;
			this.llblInno.Text = "Inno Setup downloads (Inno Setup and QuickStart Pack required)";
			this.llblInno.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblInno_LinkClicked);
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(12, 509);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(53, 13);
			this.lblVersion.TabIndex = 52;
			this.lblVersion.Text = "<runtime>";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(291, 189);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(96, 13);
			this.label19.TabIndex = 55;
			this.label19.Text = "(e.g., \"OptecTCF\")";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(47, 189);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(66, 13);
			this.label20.TabIndex = 54;
			this.label20.Text = "Short Name:";
			// 
			// txtDrvrShortName
			// 
			this.txtDrvrShortName.Location = new System.Drawing.Point(130, 186);
			this.txtDrvrShortName.Name = "txtDrvrShortName";
			this.txtDrvrShortName.Size = new System.Drawing.Size(155, 20);
			this.txtDrvrShortName.TabIndex = 2;
			this.toolTip1.SetToolTip(this.txtDrvrShortName, "The first (COM) or second (.NET) part of the ProgID for the driver");
			this.txtDrvrShortName.TextChanged += new System.EventHandler(this.txtDrvrShortName_TextChanged);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 7000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 100;
			// 
			// cbDriverType2
			// 
			this.cbDriverType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDriverType2.FormattingEnabled = true;
			this.cbDriverType2.Items.AddRange(new object[] {
            "(none)",
            "Camera",
            "Dome",
            "FilterWheel",
            "Focuser",
            "Rotator",
            "Switches",
            "Telescope"});
			this.cbDriverType2.Location = new System.Drawing.Point(130, 242);
			this.cbDriverType2.Name = "cbDriverType2";
			this.cbDriverType2.Size = new System.Drawing.Size(115, 21);
			this.cbDriverType2.Sorted = true;
			this.cbDriverType2.TabIndex = 4;
			this.toolTip1.SetToolTip(this.cbDriverType2, "(optional) The last part of the driver\'s secondary ProgID");
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(301, 31);
			this.label2.TabIndex = 56;
			this.label2.Text = "Please do not change the graphics, we want the user experi- ence when installing " +
				"drivers to be consistent.";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(47, 245);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(55, 13);
			this.label21.TabIndex = 57;
			this.label21.Text = "Aux Type:";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(251, 245);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(106, 13);
			this.label22.TabIndex = 59;
			this.label22.Text = "(dual-interface driver)";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(251, 217);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(137, 13);
			this.label23.TabIndex = 60;
			this.label23.Text = "(required - primary interface)";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(131, 413);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(225, 17);
			this.checkBox1.TabIndex = 12;
			this.checkBox1.Text = "Include option to install driver source code";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AcceptButton = this.cmdSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(404, 524);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.cbDriverType2);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.txtDrvrShortName);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.llblInno);
			this.Controls.Add(this.cmdBrowseReadMe);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.pbAscomLogo);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtDeveloperEmail);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtDeveloperName);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.txtReadMeFile);
			this.Controls.Add(this.cmdBrowseDriverFile);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtDriverFile);
			this.Controls.Add(this.cmdBrowseSourceFolder);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtSourceFolder);
			this.Controls.Add(this.cbDriverTechnology);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDriverVersion);
			this.Controls.Add(this.cbDriverType);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtDriverName);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ASCOM Driver Installer Script Generator";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbAscomLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.ComboBox cbDriverTechnology;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtDriverVersion;
		private System.Windows.Forms.ComboBox cbDriverType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDriverName;
		private System.Windows.Forms.Button cmdBrowseReadMe;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtReadMeFile;
		private System.Windows.Forms.Button cmdBrowseDriverFile;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtDriverFile;
		private System.Windows.Forms.Button cmdBrowseSourceFolder;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtSourceFolder;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtDeveloperEmail;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtDeveloperName;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.PictureBox pbAscomLogo;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.FolderBrowserDialog fldrBrowse;
		private System.Windows.Forms.LinkLabel llblInno;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox txtDrvrShortName;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbDriverType2;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}

