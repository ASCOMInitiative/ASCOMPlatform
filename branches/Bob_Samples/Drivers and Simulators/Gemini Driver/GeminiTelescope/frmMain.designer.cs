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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxPierSideWest = new System.Windows.Forms.PictureBox();
            this.buttonSlew1 = new System.Windows.Forms.Button();
            this.buttonSlew3 = new System.Windows.Forms.Button();
            this.buttonSlew4 = new System.Windows.Forms.Button();
            this.buttonSlew2 = new System.Windows.Forms.Button();
            this.buttonSlew0 = new System.Windows.Forms.Button();
            this.pictureBoxPierSideEast = new System.Windows.Forms.PictureBox();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSlew = new System.Windows.Forms.Label();
            this.labelPARK = new System.Windows.Forms.Label();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonPark = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxPierSideWest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlew1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlew3, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlew4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlew2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonSlew0, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxPierSideEast, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 99);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(126, 126);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // pictureBoxPierSideWest
            // 
            this.pictureBoxPierSideWest.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxPierSideWest.Name = "pictureBoxPierSideWest";
            this.pictureBoxPierSideWest.Size = new System.Drawing.Size(32, 31);
            this.pictureBoxPierSideWest.TabIndex = 6;
            this.pictureBoxPierSideWest.TabStop = false;
            // 
            // buttonSlew1
            // 
            this.buttonSlew1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSlew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew1.Location = new System.Drawing.Point(45, 3);
            this.buttonSlew1.Name = "buttonSlew1";
            this.buttonSlew1.Size = new System.Drawing.Size(36, 36);
            this.buttonSlew1.TabIndex = 0;
            this.buttonSlew1.Text = "N";
            this.buttonSlew1.UseVisualStyleBackColor = false;
            this.buttonSlew1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseDown);
            this.buttonSlew1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseUp);
            // 
            // buttonSlew3
            // 
            this.buttonSlew3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSlew3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew3.Location = new System.Drawing.Point(87, 45);
            this.buttonSlew3.Name = "buttonSlew3";
            this.buttonSlew3.Size = new System.Drawing.Size(36, 36);
            this.buttonSlew3.TabIndex = 1;
            this.buttonSlew3.Text = "E";
            this.buttonSlew3.UseVisualStyleBackColor = false;
            this.buttonSlew3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseDown);
            this.buttonSlew3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseUp);
            // 
            // buttonSlew4
            // 
            this.buttonSlew4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSlew4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew4.Location = new System.Drawing.Point(3, 45);
            this.buttonSlew4.Name = "buttonSlew4";
            this.buttonSlew4.Size = new System.Drawing.Size(36, 36);
            this.buttonSlew4.TabIndex = 2;
            this.buttonSlew4.Text = "W";
            this.buttonSlew4.UseVisualStyleBackColor = false;
            this.buttonSlew4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseDown);
            this.buttonSlew4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseUp);
            // 
            // buttonSlew2
            // 
            this.buttonSlew2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSlew2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew2.Location = new System.Drawing.Point(45, 87);
            this.buttonSlew2.Name = "buttonSlew2";
            this.buttonSlew2.Size = new System.Drawing.Size(36, 36);
            this.buttonSlew2.TabIndex = 3;
            this.buttonSlew2.Text = "S";
            this.buttonSlew2.UseVisualStyleBackColor = false;
            this.buttonSlew2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseDown);
            this.buttonSlew2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseUp);
            // 
            // buttonSlew0
            // 
            this.buttonSlew0.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSlew0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew0.Font = new System.Drawing.Font("Wingdings 2", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew0.Location = new System.Drawing.Point(45, 45);
            this.buttonSlew0.Name = "buttonSlew0";
            this.buttonSlew0.Size = new System.Drawing.Size(36, 36);
            this.buttonSlew0.TabIndex = 4;
            this.buttonSlew0.Text = "Ä";
            this.buttonSlew0.UseVisualStyleBackColor = false;
            this.buttonSlew0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSlew0_MouseClick);
            // 
            // pictureBoxPierSideEast
            // 
            this.pictureBoxPierSideEast.Location = new System.Drawing.Point(87, 3);
            this.pictureBoxPierSideEast.Name = "pictureBoxPierSideEast";
            this.pictureBoxPierSideEast.Size = new System.Drawing.Size(33, 31);
            this.pictureBoxPierSideEast.TabIndex = 5;
            this.pictureBoxPierSideEast.TabStop = false;
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(11, 311);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(54, 17);
            this.checkBoxTrack.TabIndex = 10;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.UseVisualStyleBackColor = false;
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
            this.TableLayoutPanel3.ColumnCount = 5;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.TableLayoutPanel3.Controls.Add(this.ButtonPark, 1, 0);
            this.TableLayoutPanel3.Controls.Add(this.ButtonConnect, 0, 1);
            this.TableLayoutPanel3.Controls.Add(this.ButtonSetup, 4, 0);
            this.TableLayoutPanel3.Controls.Add(this.ButtonFlip, 0, 0);
            this.TableLayoutPanel3.Location = new System.Drawing.Point(11, 334);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 2;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel3.Size = new System.Drawing.Size(127, 54);
            this.TableLayoutPanel3.TabIndex = 14;
            // 
            // ButtonPark
            // 
            this.ButtonPark.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ButtonPark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPark.Location = new System.Drawing.Point(30, 3);
            this.ButtonPark.Name = "ButtonPark";
            this.ButtonPark.Size = new System.Drawing.Size(21, 21);
            this.ButtonPark.TabIndex = 6;
            this.ButtonPark.Text = "P";
            this.ButtonPark.UseVisualStyleBackColor = false;
            this.ButtonPark.Click += new System.EventHandler(this.ButtonPark_Click);
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TableLayoutPanel3.SetColumnSpan(this.ButtonConnect, 5);
            this.ButtonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonConnect.Location = new System.Drawing.Point(3, 30);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(121, 21);
            this.ButtonConnect.TabIndex = 5;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = false;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ButtonSetup
            // 
            this.ButtonSetup.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ButtonSetup.ContextMenuStrip = this.contextMenuStrip1;
            this.ButtonSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonSetup.Location = new System.Drawing.Point(107, 3);
            this.ButtonSetup.Name = "ButtonSetup";
            this.ButtonSetup.Size = new System.Drawing.Size(17, 21);
            this.ButtonSetup.TabIndex = 4;
            this.ButtonSetup.Text = "S";
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 70);
            // 
            // setupDialogToolStripMenuItem
            // 
            this.setupDialogToolStripMenuItem.Name = "setupDialogToolStripMenuItem";
            this.setupDialogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.setupDialogToolStripMenuItem.Text = "Telescope Setup Dialog";
            this.setupDialogToolStripMenuItem.Click += new System.EventHandler(this.setupDialogToolStripMenuItem_Click);
            // 
            // focuserSetupDialogToolStripMenuItem
            // 
            this.focuserSetupDialogToolStripMenuItem.Name = "focuserSetupDialogToolStripMenuItem";
            this.focuserSetupDialogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.focuserSetupDialogToolStripMenuItem.Text = "Focuser Setup Dialog";
            this.focuserSetupDialogToolStripMenuItem.Click += new System.EventHandler(this.focuserSetupDialogToolStripMenuItem_Click);
            // 
            // mountParametersToolStripMenuItem
            // 
            this.mountParametersToolStripMenuItem.Name = "mountParametersToolStripMenuItem";
            this.mountParametersToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.mountParametersToolStripMenuItem.Text = "Mount Parameters";
            this.mountParametersToolStripMenuItem.Click += new System.EventHandler(this.mountParametersToolStripMenuItem_Click);
            // 
            // ButtonFlip
            // 
            this.ButtonFlip.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ButtonFlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonFlip.Location = new System.Drawing.Point(3, 3);
            this.ButtonFlip.Name = "ButtonFlip";
            this.ButtonFlip.Size = new System.Drawing.Size(21, 21);
            this.ButtonFlip.TabIndex = 3;
            this.ButtonFlip.Text = "F";
            this.ButtonFlip.UseVisualStyleBackColor = false;
            this.ButtonFlip.Click += new System.EventHandler(this.ButtonFlip_Click);
            // 
            // CheckBoxFlipDec
            // 
            this.CheckBoxFlipDec.AutoSize = true;
            this.CheckBoxFlipDec.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipDec.Location = new System.Drawing.Point(92, 231);
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
            this.GroupBox1.Location = new System.Drawing.Point(10, 267);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(128, 38);
            this.GroupBox1.TabIndex = 16;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Speed";
            // 
            // RadioButtonSlew
            // 
            this.RadioButtonSlew.AutoSize = true;
            this.RadioButtonSlew.ForeColor = System.Drawing.Color.White;
            this.RadioButtonSlew.Location = new System.Drawing.Point(81, 15);
            this.RadioButtonSlew.Name = "RadioButtonSlew";
            this.RadioButtonSlew.Size = new System.Drawing.Size(32, 17);
            this.RadioButtonSlew.TabIndex = 2;
            this.RadioButtonSlew.Text = "S";
            this.RadioButtonSlew.UseVisualStyleBackColor = false;
            // 
            // RadioButtonCenter
            // 
            this.RadioButtonCenter.AutoSize = true;
            this.RadioButtonCenter.ForeColor = System.Drawing.Color.White;
            this.RadioButtonCenter.Location = new System.Drawing.Point(43, 15);
            this.RadioButtonCenter.Name = "RadioButtonCenter";
            this.RadioButtonCenter.Size = new System.Drawing.Size(32, 17);
            this.RadioButtonCenter.TabIndex = 1;
            this.RadioButtonCenter.Text = "C";
            this.RadioButtonCenter.UseVisualStyleBackColor = false;
            // 
            // RadioButtonGuide
            // 
            this.RadioButtonGuide.AutoSize = true;
            this.RadioButtonGuide.Checked = true;
            this.RadioButtonGuide.ForeColor = System.Drawing.Color.White;
            this.RadioButtonGuide.Location = new System.Drawing.Point(4, 15);
            this.RadioButtonGuide.Name = "RadioButtonGuide";
            this.RadioButtonGuide.Size = new System.Drawing.Size(33, 17);
            this.RadioButtonGuide.TabIndex = 0;
            this.RadioButtonGuide.TabStop = true;
            this.RadioButtonGuide.Text = "G";
            this.RadioButtonGuide.UseVisualStyleBackColor = false;
            // 
            // CheckBoxFlipRa
            // 
            this.CheckBoxFlipRa.AutoSize = true;
            this.CheckBoxFlipRa.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipRa.Location = new System.Drawing.Point(15, 231);
            this.CheckBoxFlipRa.Name = "CheckBoxFlipRa";
            this.CheckBoxFlipRa.Size = new System.Drawing.Size(42, 30);
            this.CheckBoxFlipRa.TabIndex = 15;
            this.CheckBoxFlipRa.Text = "Flip\r\nRa";
            this.CheckBoxFlipRa.UseVisualStyleBackColor = false;
            // 
            // BaloonIcon
            // 
            this.BaloonIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("BaloonIcon.Icon")));
            this.BaloonIcon.Text = "Gemini Driver Status";
            this.BaloonIcon.Visible = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(147, 400);
            this.Controls.Add(this.CheckBoxFlipDec);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.CheckBoxFlipRa);
            this.Controls.Add(this.TableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.Text = "Gemini";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel3.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonSlew1;
        private System.Windows.Forms.Button buttonSlew3;
        private System.Windows.Forms.Button buttonSlew4;
        private System.Windows.Forms.Button buttonSlew2;
        private System.Windows.Forms.Button buttonSlew0;
        private System.Windows.Forms.CheckBox checkBoxTrack;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal System.Windows.Forms.Label labelPARK;
        private System.Windows.Forms.PictureBox pictureBoxPierSideWest;
        private System.Windows.Forms.PictureBox pictureBoxPierSideEast;
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


    }
}

