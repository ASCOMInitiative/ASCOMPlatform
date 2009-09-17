using System;

namespace ASCOM.GeminiTelescope
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDec = new System.Windows.Forms.Label();
            this.labelRa = new System.Windows.Forms.Label();
            this.labelLst = new System.Windows.Forms.Label();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSlew = new System.Windows.Forms.Label();
            this.labelPARK = new System.Windows.Forms.Label();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonPark = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ButtonSetup = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setupDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.focuserSetupDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonFlip = new System.Windows.Forms.Button();
            this.CheckBoxFlipDec = new System.Windows.Forms.CheckBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioButtonSlew = new System.Windows.Forms.RadioButton();
            this.RadioButtonCenter = new System.Windows.Forms.RadioButton();
            this.RadioButtonGuide = new System.Windows.Forms.RadioButton();
            this.CheckBoxFlipRa = new System.Windows.Forms.CheckBox();
            this.BaloonIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pbStop = new System.Windows.Forms.Button();
            this.buttonSlew3 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew2 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew4 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew1 = new ASCOM.GeminiTelescope.TButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "LST";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Dec";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.8125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.1875F));
            this.tableLayoutPanel1.Controls.Add(this.labelDec, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRa, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLst, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(126, 55);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelDec
            // 
            this.labelDec.AutoSize = true;
            this.labelDec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Location = new System.Drawing.Point(53, 34);
            this.labelDec.Name = "labelDec";
            this.labelDec.Size = new System.Drawing.Size(70, 21);
            this.labelDec.TabIndex = 11;
            this.labelDec.Text = "+00:00:00:00";
            // 
            // labelRa
            // 
            this.labelRa.AutoSize = true;
            this.labelRa.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRa.ForeColor = System.Drawing.Color.Red;
            this.labelRa.Location = new System.Drawing.Point(59, 17);
            this.labelRa.Name = "labelRa";
            this.labelRa.Size = new System.Drawing.Size(64, 17);
            this.labelRa.TabIndex = 10;
            this.labelRa.Text = "00:00:00:00";
            // 
            // labelLst
            // 
            this.labelLst.AutoSize = true;
            this.labelLst.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLst.ForeColor = System.Drawing.Color.Red;
            this.labelLst.Location = new System.Drawing.Point(59, 0);
            this.labelLst.Name = "labelLst";
            this.labelLst.Size = new System.Drawing.Size(64, 17);
            this.labelLst.TabIndex = 9;
            this.labelLst.Text = "00:00:00:00";
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AccessibleDescription = "Tracking";
            this.checkBoxTrack.AutoCheck = false;
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(6, 311);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(54, 17);
            this.checkBoxTrack.TabIndex = 10;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkBoxTrack.UseVisualStyleBackColor = false;
            this.checkBoxTrack.Click += new System.EventHandler(this.checkBoxTrack_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.labelSlew, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelPARK, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(126, 20);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // labelSlew
            // 
            this.labelSlew.AutoSize = true;
            this.labelSlew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelSlew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSlew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelSlew.Location = new System.Drawing.Point(3, 0);
            this.labelSlew.Name = "labelSlew";
            this.labelSlew.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.labelSlew.Size = new System.Drawing.Size(57, 20);
            this.labelSlew.TabIndex = 3;
            this.labelSlew.Text = "SLEW";
            // 
            // labelPARK
            // 
            this.labelPARK.AutoSize = true;
            this.labelPARK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelPARK.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelPARK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPARK.Location = new System.Drawing.Point(85, 0);
            this.labelPARK.Name = "labelPARK";
            this.labelPARK.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.labelPARK.Size = new System.Drawing.Size(38, 20);
            this.labelPARK.TabIndex = 2;
            this.labelPARK.Text = "PARK";
            // 
            // TableLayoutPanel3
            // 
            this.TableLayoutPanel3.ColumnCount = 3;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33313F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33344F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33344F));
            this.TableLayoutPanel3.Controls.Add(this.ButtonPark, 1, 0);
            this.TableLayoutPanel3.Controls.Add(this.ButtonConnect, 0, 1);
            this.TableLayoutPanel3.Controls.Add(this.ButtonSetup, 2, 0);
            this.TableLayoutPanel3.Controls.Add(this.ButtonFlip, 0, 0);
            this.TableLayoutPanel3.Location = new System.Drawing.Point(6, 334);
            this.TableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 2;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.TableLayoutPanel3.Size = new System.Drawing.Size(135, 57);
            this.TableLayoutPanel3.TabIndex = 14;
            // 
            // ButtonPark
            // 
            this.ButtonPark.AccessibleDescription = "Park Mount";
            this.ButtonPark.BackColor = System.Drawing.Color.Black;
            this.ButtonPark.ContextMenuStrip = this.contextMenuStrip2;
            this.ButtonPark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPark.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ButtonPark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPark.ForeColor = System.Drawing.Color.White;
            this.ButtonPark.Location = new System.Drawing.Point(45, 3);
            this.ButtonPark.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.ButtonPark.Name = "ButtonPark";
            this.ButtonPark.Size = new System.Drawing.Size(43, 24);
            this.ButtonPark.TabIndex = 6;
            this.ButtonPark.Text = "Park";
            this.ButtonPark.UseVisualStyleBackColor = false;
            this.ButtonPark.Click += new System.EventHandler(this.ButtonPark_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(241, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(240, 22);
            this.toolStripMenuItem1.Text = "Park at Start-up Position (CWD)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuParkCWD_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(240, 22);
            this.toolStripMenuItem2.Text = "Park at Home Position";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuParkHome_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(240, 22);
            this.toolStripMenuItem3.Text = "Park at current mount position";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuParkHere_Click);
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.AccessibleDescription = "Connect to Gemini";
            this.ButtonConnect.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TableLayoutPanel3.SetColumnSpan(this.ButtonConnect, 3);
            this.ButtonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonConnect.Location = new System.Drawing.Point(0, 33);
            this.ButtonConnect.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(135, 21);
            this.ButtonConnect.TabIndex = 5;
            this.ButtonConnect.Text = "&Connect";
            this.ButtonConnect.UseVisualStyleBackColor = false;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ButtonSetup
            // 
            this.ButtonSetup.AccessibleDescription = "Setup";
            this.ButtonSetup.BackColor = System.Drawing.Color.Black;
            this.ButtonSetup.ContextMenuStrip = this.contextMenuStrip1;
            this.ButtonSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonSetup.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ButtonSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSetup.ForeColor = System.Drawing.Color.White;
            this.ButtonSetup.Location = new System.Drawing.Point(90, 3);
            this.ButtonSetup.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            this.ButtonSetup.Name = "ButtonSetup";
            this.ButtonSetup.Size = new System.Drawing.Size(45, 24);
            this.ButtonSetup.TabIndex = 4;
            this.ButtonSetup.Text = "Setup";
            this.ButtonSetup.UseVisualStyleBackColor = false;
            this.ButtonSetup.Click += new System.EventHandler(this.ButtonSetup_Click_1);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupDialogToolStripMenuItem,
            this.focuserSetupDialogToolStripMenuItem,
            this.mountParametersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(223, 70);
            // 
            // setupDialogToolStripMenuItem
            // 
            this.setupDialogToolStripMenuItem.Name = "setupDialogToolStripMenuItem";
            this.setupDialogToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.setupDialogToolStripMenuItem.Text = "Telescope Setup Dialog...";
            this.setupDialogToolStripMenuItem.Click += new System.EventHandler(this.setupDialogToolStripMenuItem_Click);
            // 
            // focuserSetupDialogToolStripMenuItem
            // 
            this.focuserSetupDialogToolStripMenuItem.Name = "focuserSetupDialogToolStripMenuItem";
            this.focuserSetupDialogToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.focuserSetupDialogToolStripMenuItem.Text = "Focuser Setup Dialog...";
            this.focuserSetupDialogToolStripMenuItem.Click += new System.EventHandler(this.focuserSetupDialogToolStripMenuItem_Click);
            // 
            // mountParametersToolStripMenuItem
            // 
            this.mountParametersToolStripMenuItem.Name = "mountParametersToolStripMenuItem";
            this.mountParametersToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.mountParametersToolStripMenuItem.Text = "Advanced Gemini Settings...";
            this.mountParametersToolStripMenuItem.Click += new System.EventHandler(this.mountParametersToolStripMenuItem_Click);
            // 
            // ButtonFlip
            // 
            this.ButtonFlip.AccessibleDescription = "Do a Meridian Flip";
            this.ButtonFlip.BackColor = System.Drawing.Color.Black;
            this.ButtonFlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonFlip.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ButtonFlip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFlip.ForeColor = System.Drawing.Color.White;
            this.ButtonFlip.Location = new System.Drawing.Point(1, 3);
            this.ButtonFlip.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.ButtonFlip.Name = "ButtonFlip";
            this.ButtonFlip.Size = new System.Drawing.Size(42, 24);
            this.ButtonFlip.TabIndex = 3;
            this.ButtonFlip.Text = "Flip";
            this.ButtonFlip.UseVisualStyleBackColor = false;
            this.ButtonFlip.Click += new System.EventHandler(this.ButtonFlip_Click);
            // 
            // CheckBoxFlipDec
            // 
            this.CheckBoxFlipDec.AccessibleDescription = "Flip Dec Direction";
            this.CheckBoxFlipDec.AutoSize = true;
            this.CheckBoxFlipDec.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipDec.Location = new System.Drawing.Point(90, 231);
            this.CheckBoxFlipDec.Name = "CheckBoxFlipDec";
            this.CheckBoxFlipDec.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CheckBoxFlipDec.Size = new System.Drawing.Size(46, 30);
            this.CheckBoxFlipDec.TabIndex = 17;
            this.CheckBoxFlipDec.Text = "Flip\r\nDec";
            this.CheckBoxFlipDec.UseVisualStyleBackColor = false;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.RadioButtonSlew);
            this.GroupBox1.Controls.Add(this.RadioButtonCenter);
            this.GroupBox1.Controls.Add(this.RadioButtonGuide);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Location = new System.Drawing.Point(6, 267);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(135, 38);
            this.GroupBox1.TabIndex = 16;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Speed";
            // 
            // RadioButtonSlew
            // 
            this.RadioButtonSlew.AccessibleDescription = "Slew Speed";
            this.RadioButtonSlew.AutoSize = true;
            this.RadioButtonSlew.ForeColor = System.Drawing.Color.White;
            this.RadioButtonSlew.Location = new System.Drawing.Point(90, 15);
            this.RadioButtonSlew.Name = "RadioButtonSlew";
            this.RadioButtonSlew.Size = new System.Drawing.Size(32, 17);
            this.RadioButtonSlew.TabIndex = 2;
            this.RadioButtonSlew.Text = "S";
            this.RadioButtonSlew.UseVisualStyleBackColor = false;
            // 
            // RadioButtonCenter
            // 
            this.RadioButtonCenter.AccessibleDescription = "Cetering Speed";
            this.RadioButtonCenter.AutoSize = true;
            this.RadioButtonCenter.ForeColor = System.Drawing.Color.White;
            this.RadioButtonCenter.Location = new System.Drawing.Point(52, 15);
            this.RadioButtonCenter.Name = "RadioButtonCenter";
            this.RadioButtonCenter.Size = new System.Drawing.Size(32, 17);
            this.RadioButtonCenter.TabIndex = 1;
            this.RadioButtonCenter.Text = "C";
            this.RadioButtonCenter.UseVisualStyleBackColor = false;
            // 
            // RadioButtonGuide
            // 
            this.RadioButtonGuide.AccessibleDescription = "Guiding Speed";
            this.RadioButtonGuide.AutoSize = true;
            this.RadioButtonGuide.Checked = true;
            this.RadioButtonGuide.ForeColor = System.Drawing.Color.White;
            this.RadioButtonGuide.Location = new System.Drawing.Point(13, 15);
            this.RadioButtonGuide.Name = "RadioButtonGuide";
            this.RadioButtonGuide.Size = new System.Drawing.Size(33, 17);
            this.RadioButtonGuide.TabIndex = 0;
            this.RadioButtonGuide.TabStop = true;
            this.RadioButtonGuide.Text = "G";
            this.RadioButtonGuide.UseVisualStyleBackColor = false;
            // 
            // CheckBoxFlipRa
            // 
            this.CheckBoxFlipRa.AccessibleDescription = "Flip RA Direction";
            this.CheckBoxFlipRa.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipRa.Location = new System.Drawing.Point(15, 231);
            this.CheckBoxFlipRa.Name = "CheckBoxFlipRa";
            this.CheckBoxFlipRa.Size = new System.Drawing.Size(42, 30);
            this.CheckBoxFlipRa.TabIndex = 15;
            this.CheckBoxFlipRa.Text = "Flip\r\nRA";
            this.CheckBoxFlipRa.UseVisualStyleBackColor = false;
            // 
            // BaloonIcon
            // 
            this.BaloonIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("BaloonIcon.Icon")));
            this.BaloonIcon.Text = "Gemini Driver Status";
            this.BaloonIcon.Visible = true;
            // 
            // pbStop
            // 
            this.pbStop.AccessibleDescription = "Stop Slew";
            this.pbStop.BackColor = System.Drawing.Color.DarkRed;
            this.pbStop.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.pbStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbStop.ForeColor = System.Drawing.Color.White;
            this.pbStop.Location = new System.Drawing.Point(74, 309);
            this.pbStop.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            this.pbStop.Name = "pbStop";
            this.pbStop.Size = new System.Drawing.Size(67, 24);
            this.pbStop.TabIndex = 22;
            this.pbStop.Text = "Stop!";
            this.pbStop.UseVisualStyleBackColor = false;
            this.pbStop.Visible = false;
            this.pbStop.Click += new System.EventHandler(this.pbStop_Click);
            // 
            // buttonSlew3
            // 
            this.buttonSlew3.AccessibleDescription = "Slew East";
            this.buttonSlew3.FlatAppearance.BorderSize = 0;
            this.buttonSlew3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.buttonSlew3.ImageIndex = 0;
            this.buttonSlew3.Location = new System.Drawing.Point(6, 134);
            this.buttonSlew3.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew3.Name = "buttonSlew3";
            this.buttonSlew3.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew3.TabIndex = 19;
            this.buttonSlew3.Text = "E";
            this.buttonSlew3.UseVisualStyleBackColor = true;
            this.buttonSlew3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseDown);
            this.buttonSlew3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseUp);
            // 
            // buttonSlew2
            // 
            this.buttonSlew2.AccessibleDescription = "Slew South";
            this.buttonSlew2.FlatAppearance.BorderSize = 0;
            this.buttonSlew2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.buttonSlew2.ImageIndex = 0;
            this.buttonSlew2.Location = new System.Drawing.Point(42, 168);
            this.buttonSlew2.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew2.Name = "buttonSlew2";
            this.buttonSlew2.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew2.TabIndex = 20;
            this.buttonSlew2.Text = "S";
            this.buttonSlew2.UseVisualStyleBackColor = true;
            this.buttonSlew2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseDown);
            this.buttonSlew2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseUp);
            // 
            // buttonSlew4
            // 
            this.buttonSlew4.AccessibleDescription = "Slew West";
            this.buttonSlew4.FlatAppearance.BorderSize = 0;
            this.buttonSlew4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.buttonSlew4.ImageIndex = 0;
            this.buttonSlew4.Location = new System.Drawing.Point(77, 134);
            this.buttonSlew4.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew4.Name = "buttonSlew4";
            this.buttonSlew4.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew4.TabIndex = 21;
            this.buttonSlew4.Text = "W";
            this.buttonSlew4.UseVisualStyleBackColor = true;
            this.buttonSlew4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseDown);
            this.buttonSlew4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseUp);
            // 
            // buttonSlew1
            // 
            this.buttonSlew1.AccessibleDescription = "Slew North";
            this.buttonSlew1.FlatAppearance.BorderSize = 0;
            this.buttonSlew1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.buttonSlew1.ImageIndex = 0;
            this.buttonSlew1.Location = new System.Drawing.Point(42, 99);
            this.buttonSlew1.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew1.Name = "buttonSlew1";
            this.buttonSlew1.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew1.TabIndex = 18;
            this.buttonSlew1.Text = "N";
            this.buttonSlew1.UseVisualStyleBackColor = false;
            this.buttonSlew1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseDown);
            this.buttonSlew1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseUp);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(147, 400);
            this.Controls.Add(this.pbStop);
            this.Controls.Add(this.buttonSlew3);
            this.Controls.Add(this.buttonSlew2);
            this.Controls.Add(this.buttonSlew4);
            this.Controls.Add(this.buttonSlew1);
            this.Controls.Add(this.CheckBoxFlipDec);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.CheckBoxFlipRa);
            this.Controls.Add(this.TableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.checkBoxTrack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.Text = "Gemini";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel3.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelLst;
        private System.Windows.Forms.Label labelDec;
        private System.Windows.Forms.Label labelRa;
        private System.Windows.Forms.CheckBox checkBoxTrack;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal System.Windows.Forms.Label labelPARK;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        private System.Windows.Forms.Button ButtonPark;
        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.Button ButtonFlip;
        private System.Windows.Forms.Button ButtonSetup;
        internal System.Windows.Forms.CheckBox CheckBoxFlipDec;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton RadioButtonSlew;
        internal System.Windows.Forms.RadioButton RadioButtonCenter;
        internal System.Windows.Forms.RadioButton RadioButtonGuide;
        internal System.Windows.Forms.CheckBox CheckBoxFlipRa;
        internal System.Windows.Forms.Label labelSlew;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mountParametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem focuserSetupDialogToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon BaloonIcon;
        private TButton buttonSlew1;
        private TButton buttonSlew4;
        private TButton buttonSlew2;
        private TButton buttonSlew3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Button pbStop;


    }
}

