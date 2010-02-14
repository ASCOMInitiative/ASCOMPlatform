using System;
using ASCOM.GeminiTelescope.Properties;

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
            this.labelLimit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.parkAtCustomParkPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuParkHere = new System.Windows.Forms.ToolStripMenuItem();
            this.unparkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.setCustomParkPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ButtonSetup = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setupDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.focuserSetupDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.observationLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureCatalogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGeminiDriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitDriverMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FuncMenu = new System.Windows.Forms.Button();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.keepThisWindowOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckBoxFlipDec = new System.Windows.Forms.CheckBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioButtonSlew = new System.Windows.Forms.RadioButton();
            this.RadioButtonCenter = new System.Windows.Forms.RadioButton();
            this.RadioButtonGuide = new System.Windows.Forms.RadioButton();
            this.CheckBoxFlipRa = new System.Windows.Forms.CheckBox();
            this.BalloonIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pbStop = new System.Windows.Forms.Button();
            this.checkboxPEC = new System.Windows.Forms.CheckBox();
            this.buttonSlew2 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew3 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew4 = new ASCOM.GeminiTelescope.TButton();
            this.buttonSlew1 = new ASCOM.GeminiTelescope.TButton();
            this.label5 = new System.Windows.Forms.Label();
            this.labelHA = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "LST";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Dec";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.30159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.69841F));
            this.tableLayoutPanel1.Controls.Add(this.labelHA, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelLimit, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelDec, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRa, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLst, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(126, 84);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelLimit
            // 
            this.labelLimit.AutoSize = true;
            this.labelLimit.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLimit.ForeColor = System.Drawing.Color.Red;
            this.labelLimit.Location = new System.Drawing.Point(74, 64);
            this.labelLimit.Name = "labelLimit";
            this.labelLimit.Size = new System.Drawing.Size(49, 20);
            this.labelLimit.TabIndex = 13;
            this.labelLimit.Text = "00:00:00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "To Limit";
            // 
            // labelDec
            // 
            this.labelDec.AutoSize = true;
            this.labelDec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Location = new System.Drawing.Point(53, 32);
            this.labelDec.Name = "labelDec";
            this.labelDec.Size = new System.Drawing.Size(70, 16);
            this.labelDec.TabIndex = 11;
            this.labelDec.Text = "+00:00:00:00";
            // 
            // labelRa
            // 
            this.labelRa.AutoSize = true;
            this.labelRa.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRa.ForeColor = System.Drawing.Color.Red;
            this.labelRa.Location = new System.Drawing.Point(59, 16);
            this.labelRa.Name = "labelRa";
            this.labelRa.Size = new System.Drawing.Size(64, 16);
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
            this.labelLst.Size = new System.Drawing.Size(64, 16);
            this.labelLst.TabIndex = 9;
            this.labelLst.Text = "00:00:00:00";
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AccessibleDescription = "Tracking";
            this.checkBoxTrack.AutoCheck = false;
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(7, 342);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(54, 17);
            this.checkBoxTrack.TabIndex = 8;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkBoxTrack.UseVisualStyleBackColor = false;
            this.checkBoxTrack.Click += new System.EventHandler(this.checkBoxTrack_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
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
            this.labelSlew.Text = "STOP";
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
            this.TableLayoutPanel3.Controls.Add(this.FuncMenu, 0, 0);
            this.TableLayoutPanel3.Location = new System.Drawing.Point(6, 362);
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
            this.ButtonPark.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPark.ForeColor = System.Drawing.Color.White;
            this.ButtonPark.Location = new System.Drawing.Point(45, 3);
            this.ButtonPark.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.ButtonPark.Name = "ButtonPark";
            this.ButtonPark.Size = new System.Drawing.Size(43, 24);
            this.ButtonPark.TabIndex = 11;
            this.ButtonPark.Text = "Park";
            this.ButtonPark.UseVisualStyleBackColor = false;
            this.ButtonPark.Click += new System.EventHandler(this.ButtonPark_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.Color.Black;
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parkAtCustomParkPositionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuParkHere,
            this.unparkToolStripMenuItem,
            this.toolStripSeparator4,
            this.setCustomParkPositionToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(218, 142);
            // 
            // parkAtCustomParkPositionToolStripMenuItem
            // 
            this.parkAtCustomParkPositionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.parkAtCustomParkPositionToolStripMenuItem.Name = "parkAtCustomParkPositionToolStripMenuItem";
            this.parkAtCustomParkPositionToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.parkAtCustomParkPositionToolStripMenuItem.Text = "Park";
            this.parkAtCustomParkPositionToolStripMenuItem.Click += new System.EventHandler(this.parkAtCustomParkPositionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 22);
            this.toolStripMenuItem1.Text = "Park at Start-up Position (CWD)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuParkCWD_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(217, 22);
            this.toolStripMenuItem2.Text = "Park at Home Position";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuParkHome_Click);
            // 
            // toolStripMenuParkHere
            // 
            this.toolStripMenuParkHere.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuParkHere.Name = "toolStripMenuParkHere";
            this.toolStripMenuParkHere.Size = new System.Drawing.Size(217, 22);
            this.toolStripMenuParkHere.Text = "Park at Current Mount Position";
            this.toolStripMenuParkHere.Click += new System.EventHandler(this.toolStripMenuParkHere_Click);
            // 
            // unparkToolStripMenuItem
            // 
            this.unparkToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.unparkToolStripMenuItem.Name = "unparkToolStripMenuItem";
            this.unparkToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.unparkToolStripMenuItem.Text = "Unpark";
            this.unparkToolStripMenuItem.Click += new System.EventHandler(this.unparkToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(214, 6);
            // 
            // setCustomParkPositionToolStripMenuItem
            // 
            this.setCustomParkPositionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.setCustomParkPositionToolStripMenuItem.Name = "setCustomParkPositionToolStripMenuItem";
            this.setCustomParkPositionToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.setCustomParkPositionToolStripMenuItem.Text = "Configure Driver Park Position...";
            this.setCustomParkPositionToolStripMenuItem.Click += new System.EventHandler(this.setCustomParkPositionToolStripMenuItem_Click);
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.AccessibleDescription = "Connect";
            this.ButtonConnect.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TableLayoutPanel3.SetColumnSpan(this.ButtonConnect, 3);
            this.ButtonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonConnect.Location = new System.Drawing.Point(0, 33);
            this.ButtonConnect.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(135, 21);
            this.ButtonConnect.TabIndex = 13;
            this.ButtonConnect.Text = "Connect";
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
            this.ButtonSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonSetup.ForeColor = System.Drawing.Color.White;
            this.ButtonSetup.Location = new System.Drawing.Point(90, 3);
            this.ButtonSetup.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            this.ButtonSetup.Name = "ButtonSetup";
            this.ButtonSetup.Size = new System.Drawing.Size(45, 24);
            this.ButtonSetup.TabIndex = 12;
            this.ButtonSetup.Text = "Setup";
            this.ButtonSetup.UseVisualStyleBackColor = false;
            this.ButtonSetup.Click += new System.EventHandler(this.ButtonSetup_Click_1);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.Black;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupDialogToolStripMenuItem,
            this.focuserSetupDialogToolStripMenuItem,
            this.mountParametersToolStripMenuItem,
            this.toolStripSeparator3,
            this.observationLogToolStripMenuItem,
            this.configureCatalogsToolStripMenuItem,
            this.toolStripSeparator5,
            this.viewHelpToolStripMenuItem,
            this.aboutGeminiDriverToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitDriverMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 198);
            // 
            // setupDialogToolStripMenuItem
            // 
            this.setupDialogToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setupDialogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.setupDialogToolStripMenuItem.Name = "setupDialogToolStripMenuItem";
            this.setupDialogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.setupDialogToolStripMenuItem.Text = "Configure Telescope...";
            this.setupDialogToolStripMenuItem.Click += new System.EventHandler(this.setupDialogToolStripMenuItem_Click);
            // 
            // focuserSetupDialogToolStripMenuItem
            // 
            this.focuserSetupDialogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.focuserSetupDialogToolStripMenuItem.Name = "focuserSetupDialogToolStripMenuItem";
            this.focuserSetupDialogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.focuserSetupDialogToolStripMenuItem.Text = "Configure Focuser...";
            this.focuserSetupDialogToolStripMenuItem.Click += new System.EventHandler(this.focuserSetupDialogToolStripMenuItem_Click);
            // 
            // mountParametersToolStripMenuItem
            // 
            this.mountParametersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.mountParametersToolStripMenuItem.Name = "mountParametersToolStripMenuItem";
            this.mountParametersToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.mountParametersToolStripMenuItem.Text = "Advanced Gemini Settings...";
            this.mountParametersToolStripMenuItem.Click += new System.EventHandler(this.mountParametersToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            // 
            // observationLogToolStripMenuItem
            // 
            this.observationLogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.observationLogToolStripMenuItem.Name = "observationLogToolStripMenuItem";
            this.observationLogToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.observationLogToolStripMenuItem.Text = "Observation Log...";
            this.observationLogToolStripMenuItem.Click += new System.EventHandler(this.observationLogToolStripMenuItem_Click);
            // 
            // configureCatalogsToolStripMenuItem
            // 
            this.configureCatalogsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.configureCatalogsToolStripMenuItem.Name = "configureCatalogsToolStripMenuItem";
            this.configureCatalogsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.configureCatalogsToolStripMenuItem.Text = "Manage Catalogs...";
            this.configureCatalogsToolStripMenuItem.Click += new System.EventHandler(this.configureCatalogsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(194, 6);
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewHelpToolStripMenuItem.Text = global::ASCOM.GeminiTelescope.Properties.Resources.HelpMenu;
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // aboutGeminiDriverToolStripMenuItem
            // 
            this.aboutGeminiDriverToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutGeminiDriverToolStripMenuItem.Name = "aboutGeminiDriverToolStripMenuItem";
            this.aboutGeminiDriverToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.aboutGeminiDriverToolStripMenuItem.Text = "About Gemini Driver...";
            this.aboutGeminiDriverToolStripMenuItem.Click += new System.EventHandler(this.aboutGeminiDriverToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // exitDriverMenuItem
            // 
            this.exitDriverMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitDriverMenuItem.Name = "exitDriverMenuItem";
            this.exitDriverMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitDriverMenuItem.Text = "Exit";
            this.exitDriverMenuItem.Click += new System.EventHandler(this.exitDriverMenuItem_Click);
            // 
            // FuncMenu
            // 
            this.FuncMenu.AccessibleDescription = "Function Menu";
            this.FuncMenu.BackColor = System.Drawing.Color.Black;
            this.FuncMenu.ContextMenuStrip = this.contextMenuStrip3;
            this.FuncMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FuncMenu.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.FuncMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FuncMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FuncMenu.ForeColor = System.Drawing.Color.White;
            this.FuncMenu.Location = new System.Drawing.Point(1, 3);
            this.FuncMenu.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.FuncMenu.Name = "FuncMenu";
            this.FuncMenu.Size = new System.Drawing.Size(42, 24);
            this.FuncMenu.TabIndex = 10;
            this.FuncMenu.Text = "Func";
            this.FuncMenu.UseVisualStyleBackColor = false;
            this.FuncMenu.Click += new System.EventHandler(this.FuncMenu_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.BackColor = System.Drawing.Color.Black;
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem4,
            this.toolStripSeparator2,
            this.keepThisWindowOnTopToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip3.Size = new System.Drawing.Size(286, 98);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(285, 22);
            this.toolStripMenuItem5.Text = "  Sync at current coordinates";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(285, 22);
            this.toolStripMenuItem6.Text = "  Additional Align at current coordinates";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.buttonAddlAlign_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(285, 22);
            this.toolStripMenuItem4.Text = "  Perform a Meridian Flip";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.ButtonFlip_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(282, 6);
            // 
            // keepThisWindowOnTopToolStripMenuItem
            // 
            this.keepThisWindowOnTopToolStripMenuItem.Checked = true;
            this.keepThisWindowOnTopToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepThisWindowOnTopToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.keepThisWindowOnTopToolStripMenuItem.Name = "keepThisWindowOnTopToolStripMenuItem";
            this.keepThisWindowOnTopToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
            this.keepThisWindowOnTopToolStripMenuItem.Text = "  Keep this window On Top";
            this.keepThisWindowOnTopToolStripMenuItem.Click += new System.EventHandler(this.keepThisWindowOnTopToolStripMenuItem_Click);
            // 
            // CheckBoxFlipDec
            // 
            this.CheckBoxFlipDec.AccessibleDescription = "Flip Dec Direction";
            this.CheckBoxFlipDec.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipDec.Location = new System.Drawing.Point(95, 263);
            this.CheckBoxFlipDec.Name = "CheckBoxFlipDec";
            this.CheckBoxFlipDec.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CheckBoxFlipDec.Size = new System.Drawing.Size(46, 30);
            this.CheckBoxFlipDec.TabIndex = 6;
            this.CheckBoxFlipDec.Text = "Rev\r\nDec";
            this.CheckBoxFlipDec.UseVisualStyleBackColor = false;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.RadioButtonSlew);
            this.GroupBox1.Controls.Add(this.RadioButtonCenter);
            this.GroupBox1.Controls.Add(this.RadioButtonGuide);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Location = new System.Drawing.Point(7, 298);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(135, 38);
            this.GroupBox1.TabIndex = 7;
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
            this.CheckBoxFlipRa.Location = new System.Drawing.Point(7, 263);
            this.CheckBoxFlipRa.Name = "CheckBoxFlipRa";
            this.CheckBoxFlipRa.Size = new System.Drawing.Size(53, 30);
            this.CheckBoxFlipRa.TabIndex = 5;
            this.CheckBoxFlipRa.Text = "RevRA";
            this.CheckBoxFlipRa.UseVisualStyleBackColor = false;
            // 
            // BalloonIcon
            // 
            this.BalloonIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("BalloonIcon.Icon")));
            this.BalloonIcon.Text = "Gemini Driver Status";
            this.BalloonIcon.Visible = true;
            // 
            // pbStop
            // 
            this.pbStop.AccessibleDescription = "Stop Slew";
            this.pbStop.BackColor = System.Drawing.Color.DarkRed;
            this.pbStop.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.pbStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbStop.ForeColor = System.Drawing.Color.White;
            this.pbStop.Location = new System.Drawing.Point(50, 269);
            this.pbStop.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            this.pbStop.Name = "pbStop";
            this.pbStop.Size = new System.Drawing.Size(44, 24);
            this.pbStop.TabIndex = 9;
            this.pbStop.Text = "Stop!";
            this.pbStop.UseVisualStyleBackColor = false;
            this.pbStop.Visible = false;
            this.pbStop.Click += new System.EventHandler(this.pbStop_Click);
            // 
            // checkboxPEC
            // 
            this.checkboxPEC.AccessibleDescription = "Tracking";
            this.checkboxPEC.AutoCheck = false;
            this.checkboxPEC.AutoSize = true;
            this.checkboxPEC.ForeColor = System.Drawing.Color.White;
            this.checkboxPEC.Location = new System.Drawing.Point(94, 342);
            this.checkboxPEC.Name = "checkboxPEC";
            this.checkboxPEC.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxPEC.Size = new System.Drawing.Size(47, 17);
            this.checkboxPEC.TabIndex = 16;
            this.checkboxPEC.Text = "PEC";
            this.checkboxPEC.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkboxPEC.UseVisualStyleBackColor = false;
            this.checkboxPEC.Click += new System.EventHandler(this.checkboxPEC_Clicked);
            // 
            // buttonSlew2
            // 
            this.buttonSlew2.AccessibleDescription = "Slew South";
            this.buttonSlew2.FlatAppearance.BorderSize = 0;
            this.buttonSlew2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew2.Font = new System.Drawing.Font("Wingdings 3", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew2.ImageIndex = 0;
            this.buttonSlew2.Location = new System.Drawing.Point(42, 199);
            this.buttonSlew2.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew2.Name = "buttonSlew2";
            this.buttonSlew2.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew2.TabIndex = 4;
            this.buttonSlew2.UseVisualStyleBackColor = true;
            this.buttonSlew2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseDown);
            this.buttonSlew2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseUp);
            // 
            // buttonSlew3
            // 
            this.buttonSlew3.AccessibleDescription = "Slew East";
            this.buttonSlew3.FlatAppearance.BorderSize = 0;
            this.buttonSlew3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew3.Font = new System.Drawing.Font("Wingdings 3", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew3.ImageIndex = 0;
            this.buttonSlew3.Location = new System.Drawing.Point(6, 165);
            this.buttonSlew3.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew3.Name = "buttonSlew3";
            this.buttonSlew3.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew3.TabIndex = 2;
            this.buttonSlew3.UseVisualStyleBackColor = true;
            this.buttonSlew3.Click += new System.EventHandler(this.buttonSlew3_Click);
            this.buttonSlew3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseDown);
            this.buttonSlew3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseUp);
            // 
            // buttonSlew4
            // 
            this.buttonSlew4.AccessibleDescription = "Slew West";
            this.buttonSlew4.FlatAppearance.BorderSize = 0;
            this.buttonSlew4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew4.Font = new System.Drawing.Font("Wingdings 3", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew4.ImageIndex = 0;
            this.buttonSlew4.Location = new System.Drawing.Point(77, 165);
            this.buttonSlew4.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew4.Name = "buttonSlew4";
            this.buttonSlew4.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew4.TabIndex = 3;
            this.buttonSlew4.UseVisualStyleBackColor = true;
            this.buttonSlew4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseDown);
            this.buttonSlew4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseUp);
            // 
            // buttonSlew1
            // 
            this.buttonSlew1.AccessibleDescription = "Slew North";
            this.buttonSlew1.FlatAppearance.BorderSize = 0;
            this.buttonSlew1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSlew1.Font = new System.Drawing.Font("Wingdings 3", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew1.ImageIndex = 0;
            this.buttonSlew1.Location = new System.Drawing.Point(42, 130);
            this.buttonSlew1.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSlew1.Name = "buttonSlew1";
            this.buttonSlew1.Size = new System.Drawing.Size(64, 64);
            this.buttonSlew1.TabIndex = 1;
            this.buttonSlew1.UseVisualStyleBackColor = false;
            this.buttonSlew1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseDown);
            this.buttonSlew1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "HA";
            // 
            // labelHA
            // 
            this.labelHA.AutoSize = true;
            this.labelHA.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelHA.ForeColor = System.Drawing.Color.Red;
            this.labelHA.Location = new System.Drawing.Point(74, 48);
            this.labelHA.Name = "labelHA";
            this.labelHA.Size = new System.Drawing.Size(49, 16);
            this.labelHA.TabIndex = 15;
            this.labelHA.Text = "00:00:00";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(147, 426);
            this.Controls.Add(this.buttonSlew2);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonSlew3);
            this.Controls.Add(this.checkboxPEC);
            this.Controls.Add(this.pbStop);
            this.Controls.Add(this.buttonSlew4);
            this.Controls.Add(this.buttonSlew1);
            this.Controls.Add(this.CheckBoxFlipDec);
            this.Controls.Add(this.CheckBoxFlipRa);
            this.Controls.Add(this.TableLayoutPanel3);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Gemini";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.VisibleChanged += new System.EventHandler(this.frmMain_VisibleChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel3.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
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
        private System.Windows.Forms.Button FuncMenu;
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
        private System.Windows.Forms.NotifyIcon BalloonIcon;
        private TButton buttonSlew1;
        private TButton buttonSlew4;
        private TButton buttonSlew2;
        private TButton buttonSlew3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuParkHere;
        private System.Windows.Forms.Button pbStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutGeminiDriverToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkboxPEC;
        private System.Windows.Forms.ToolStripMenuItem exitDriverMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelLimit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem keepThisWindowOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureCatalogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem observationLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCustomParkPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parkAtCustomParkPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem unparkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
        private System.Windows.Forms.Label labelHA;
        private System.Windows.Forms.Label label5;


    }
}

