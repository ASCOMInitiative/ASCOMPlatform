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
            this.labelHA = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.objectAndCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // labelHA
            // 
            resources.ApplyResources(this.labelHA, "labelHA");
            this.labelHA.ForeColor = System.Drawing.Color.Red;
            this.labelHA.Name = "labelHA";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // labelLimit
            // 
            resources.ApplyResources(this.labelLimit, "labelLimit");
            this.labelLimit.ForeColor = System.Drawing.Color.Red;
            this.labelLimit.Name = "labelLimit";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // labelDec
            // 
            resources.ApplyResources(this.labelDec, "labelDec");
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Name = "labelDec";
            // 
            // labelRa
            // 
            resources.ApplyResources(this.labelRa, "labelRa");
            this.labelRa.ForeColor = System.Drawing.Color.Red;
            this.labelRa.Name = "labelRa";
            // 
            // labelLst
            // 
            resources.ApplyResources(this.labelLst, "labelLst");
            this.labelLst.ForeColor = System.Drawing.Color.Red;
            this.labelLst.Name = "labelLst";
            // 
            // checkBoxTrack
            // 
            resources.ApplyResources(this.checkBoxTrack, "checkBoxTrack");
            this.checkBoxTrack.AutoCheck = false;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.UseVisualStyleBackColor = false;
            this.checkBoxTrack.Click += new System.EventHandler(this.checkBoxTrack_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.labelSlew, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelPARK, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // labelSlew
            // 
            resources.ApplyResources(this.labelSlew, "labelSlew");
            this.labelSlew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelSlew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelSlew.Name = "labelSlew";
            // 
            // labelPARK
            // 
            resources.ApplyResources(this.labelPARK, "labelPARK");
            this.labelPARK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelPARK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelPARK.Name = "labelPARK";
            // 
            // TableLayoutPanel3
            // 
            resources.ApplyResources(this.TableLayoutPanel3, "TableLayoutPanel3");
            this.TableLayoutPanel3.Controls.Add(this.ButtonPark, 1, 0);
            this.TableLayoutPanel3.Controls.Add(this.ButtonConnect, 0, 1);
            this.TableLayoutPanel3.Controls.Add(this.ButtonSetup, 2, 0);
            this.TableLayoutPanel3.Controls.Add(this.FuncMenu, 0, 0);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            // 
            // ButtonPark
            // 
            this.ButtonPark.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.Park;
            this.ButtonPark.BackColor = System.Drawing.Color.Black;
            this.ButtonPark.ContextMenuStrip = this.contextMenuStrip2;
            resources.ApplyResources(this.ButtonPark, "ButtonPark");
            this.ButtonPark.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ButtonPark.ForeColor = System.Drawing.Color.White;
            this.ButtonPark.Name = "ButtonPark";
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
            resources.ApplyResources(this.contextMenuStrip2, "contextMenuStrip2");
            // 
            // parkAtCustomParkPositionToolStripMenuItem
            // 
            this.parkAtCustomParkPositionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.parkAtCustomParkPositionToolStripMenuItem.Name = "parkAtCustomParkPositionToolStripMenuItem";
            resources.ApplyResources(this.parkAtCustomParkPositionToolStripMenuItem, "parkAtCustomParkPositionToolStripMenuItem");
            this.parkAtCustomParkPositionToolStripMenuItem.Click += new System.EventHandler(this.parkAtCustomParkPositionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuParkCWD_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuParkHome_Click);
            // 
            // toolStripMenuParkHere
            // 
            this.toolStripMenuParkHere.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuParkHere.Name = "toolStripMenuParkHere";
            resources.ApplyResources(this.toolStripMenuParkHere, "toolStripMenuParkHere");
            this.toolStripMenuParkHere.Click += new System.EventHandler(this.toolStripMenuParkHere_Click);
            // 
            // unparkToolStripMenuItem
            // 
            this.unparkToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.unparkToolStripMenuItem.Name = "unparkToolStripMenuItem";
            resources.ApplyResources(this.unparkToolStripMenuItem, "unparkToolStripMenuItem");
            this.unparkToolStripMenuItem.Click += new System.EventHandler(this.unparkToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // setCustomParkPositionToolStripMenuItem
            // 
            this.setCustomParkPositionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.setCustomParkPositionToolStripMenuItem.Name = "setCustomParkPositionToolStripMenuItem";
            resources.ApplyResources(this.setCustomParkPositionToolStripMenuItem, "setCustomParkPositionToolStripMenuItem");
            this.setCustomParkPositionToolStripMenuItem.Click += new System.EventHandler(this.setCustomParkPositionToolStripMenuItem_Click);
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.Connect;
            this.ButtonConnect.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TableLayoutPanel3.SetColumnSpan(this.ButtonConnect, 3);
            resources.ApplyResources(this.ButtonConnect, "ButtonConnect");
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Text = global::ASCOM.GeminiTelescope.Properties.Resources.Connect;
            this.ButtonConnect.UseVisualStyleBackColor = false;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ButtonSetup
            // 
            this.ButtonSetup.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.Setup;
            this.ButtonSetup.BackColor = System.Drawing.Color.Black;
            this.ButtonSetup.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.ButtonSetup, "ButtonSetup");
            this.ButtonSetup.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.ButtonSetup.ForeColor = System.Drawing.Color.White;
            this.ButtonSetup.Name = "ButtonSetup";
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
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // setupDialogToolStripMenuItem
            // 
            this.setupDialogToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setupDialogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.setupDialogToolStripMenuItem.Name = "setupDialogToolStripMenuItem";
            resources.ApplyResources(this.setupDialogToolStripMenuItem, "setupDialogToolStripMenuItem");
            this.setupDialogToolStripMenuItem.Click += new System.EventHandler(this.setupDialogToolStripMenuItem_Click);
            // 
            // focuserSetupDialogToolStripMenuItem
            // 
            this.focuserSetupDialogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.focuserSetupDialogToolStripMenuItem.Name = "focuserSetupDialogToolStripMenuItem";
            resources.ApplyResources(this.focuserSetupDialogToolStripMenuItem, "focuserSetupDialogToolStripMenuItem");
            this.focuserSetupDialogToolStripMenuItem.Click += new System.EventHandler(this.focuserSetupDialogToolStripMenuItem_Click);
            // 
            // mountParametersToolStripMenuItem
            // 
            this.mountParametersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.mountParametersToolStripMenuItem.Name = "mountParametersToolStripMenuItem";
            resources.ApplyResources(this.mountParametersToolStripMenuItem, "mountParametersToolStripMenuItem");
            this.mountParametersToolStripMenuItem.Click += new System.EventHandler(this.mountParametersToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // observationLogToolStripMenuItem
            // 
            this.observationLogToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.observationLogToolStripMenuItem.Name = "observationLogToolStripMenuItem";
            resources.ApplyResources(this.observationLogToolStripMenuItem, "observationLogToolStripMenuItem");
            this.observationLogToolStripMenuItem.Click += new System.EventHandler(this.observationLogToolStripMenuItem_Click);
            // 
            // configureCatalogsToolStripMenuItem
            // 
            this.configureCatalogsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.configureCatalogsToolStripMenuItem.Name = "configureCatalogsToolStripMenuItem";
            resources.ApplyResources(this.configureCatalogsToolStripMenuItem, "configureCatalogsToolStripMenuItem");
            this.configureCatalogsToolStripMenuItem.Click += new System.EventHandler(this.configureCatalogsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            resources.ApplyResources(this.viewHelpToolStripMenuItem, "viewHelpToolStripMenuItem");
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // aboutGeminiDriverToolStripMenuItem
            // 
            this.aboutGeminiDriverToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutGeminiDriverToolStripMenuItem.Name = "aboutGeminiDriverToolStripMenuItem";
            resources.ApplyResources(this.aboutGeminiDriverToolStripMenuItem, "aboutGeminiDriverToolStripMenuItem");
            this.aboutGeminiDriverToolStripMenuItem.Click += new System.EventHandler(this.aboutGeminiDriverToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitDriverMenuItem
            // 
            this.exitDriverMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitDriverMenuItem.Name = "exitDriverMenuItem";
            resources.ApplyResources(this.exitDriverMenuItem, "exitDriverMenuItem");
            this.exitDriverMenuItem.Click += new System.EventHandler(this.exitDriverMenuItem_Click);
            // 
            // FuncMenu
            // 
            resources.ApplyResources(this.FuncMenu, "FuncMenu");
            this.FuncMenu.BackColor = System.Drawing.Color.Black;
            this.FuncMenu.ContextMenuStrip = this.contextMenuStrip3;
            this.FuncMenu.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.FuncMenu.ForeColor = System.Drawing.Color.White;
            this.FuncMenu.Name = "FuncMenu";
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
            this.objectAndCoordinatesToolStripMenuItem,
            this.toolStripSeparator2,
            this.keepThisWindowOnTopToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            resources.ApplyResources(this.contextMenuStrip3, "contextMenuStrip3");
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            this.toolStripMenuItem5.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            this.toolStripMenuItem6.Click += new System.EventHandler(this.buttonAddlAlign_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Click += new System.EventHandler(this.ButtonFlip_Click);
            // 
            // objectAndCoordinatesToolStripMenuItem
            // 
            this.objectAndCoordinatesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.objectAndCoordinatesToolStripMenuItem.Name = "objectAndCoordinatesToolStripMenuItem";
            resources.ApplyResources(this.objectAndCoordinatesToolStripMenuItem, "objectAndCoordinatesToolStripMenuItem");
            this.objectAndCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.objectAndCoordinatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // keepThisWindowOnTopToolStripMenuItem
            // 
            this.keepThisWindowOnTopToolStripMenuItem.Checked = true;
            this.keepThisWindowOnTopToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepThisWindowOnTopToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.keepThisWindowOnTopToolStripMenuItem.Name = "keepThisWindowOnTopToolStripMenuItem";
            resources.ApplyResources(this.keepThisWindowOnTopToolStripMenuItem, "keepThisWindowOnTopToolStripMenuItem");
            this.keepThisWindowOnTopToolStripMenuItem.Click += new System.EventHandler(this.keepThisWindowOnTopToolStripMenuItem_Click);
            // 
            // CheckBoxFlipDec
            // 
            resources.ApplyResources(this.CheckBoxFlipDec, "CheckBoxFlipDec");
            this.CheckBoxFlipDec.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipDec.Name = "CheckBoxFlipDec";
            this.CheckBoxFlipDec.UseVisualStyleBackColor = false;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.RadioButtonSlew);
            this.GroupBox1.Controls.Add(this.RadioButtonCenter);
            this.GroupBox1.Controls.Add(this.RadioButtonGuide);
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // RadioButtonSlew
            // 
            this.RadioButtonSlew.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.dispSLEW;
            resources.ApplyResources(this.RadioButtonSlew, "RadioButtonSlew");
            this.RadioButtonSlew.ForeColor = System.Drawing.Color.White;
            this.RadioButtonSlew.Name = "RadioButtonSlew";
            this.RadioButtonSlew.UseVisualStyleBackColor = false;
            // 
            // RadioButtonCenter
            // 
            this.RadioButtonCenter.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.dispCENTER;
            resources.ApplyResources(this.RadioButtonCenter, "RadioButtonCenter");
            this.RadioButtonCenter.ForeColor = System.Drawing.Color.White;
            this.RadioButtonCenter.Name = "RadioButtonCenter";
            this.RadioButtonCenter.UseVisualStyleBackColor = false;
            // 
            // RadioButtonGuide
            // 
            this.RadioButtonGuide.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.dispGUIDE;
            resources.ApplyResources(this.RadioButtonGuide, "RadioButtonGuide");
            this.RadioButtonGuide.Checked = true;
            this.RadioButtonGuide.ForeColor = System.Drawing.Color.White;
            this.RadioButtonGuide.Name = "RadioButtonGuide";
            this.RadioButtonGuide.TabStop = true;
            this.RadioButtonGuide.UseVisualStyleBackColor = false;
            // 
            // CheckBoxFlipRa
            // 
            resources.ApplyResources(this.CheckBoxFlipRa, "CheckBoxFlipRa");
            this.CheckBoxFlipRa.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipRa.Name = "CheckBoxFlipRa";
            this.CheckBoxFlipRa.UseVisualStyleBackColor = false;
            // 
            // BalloonIcon
            // 
            resources.ApplyResources(this.BalloonIcon, "BalloonIcon");
            // 
            // pbStop
            // 
            this.pbStop.AccessibleDescription = global::ASCOM.GeminiTelescope.Properties.Resources.StopSlew;
            this.pbStop.BackColor = System.Drawing.Color.DarkRed;
            this.pbStop.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.pbStop, "pbStop");
            this.pbStop.ForeColor = System.Drawing.Color.White;
            this.pbStop.Name = "pbStop";
            this.pbStop.UseVisualStyleBackColor = false;
            this.pbStop.Click += new System.EventHandler(this.pbStop_Click);
            // 
            // checkboxPEC
            // 
            resources.ApplyResources(this.checkboxPEC, "checkboxPEC");
            this.checkboxPEC.AutoCheck = false;
            this.checkboxPEC.ForeColor = System.Drawing.Color.White;
            this.checkboxPEC.Name = "checkboxPEC";
            this.checkboxPEC.UseVisualStyleBackColor = false;
            this.checkboxPEC.Click += new System.EventHandler(this.checkboxPEC_Clicked);
            // 
            // buttonSlew2
            // 
            resources.ApplyResources(this.buttonSlew2, "buttonSlew2");
            this.buttonSlew2.FlatAppearance.BorderSize = 0;
            this.buttonSlew2.Name = "buttonSlew2";
            this.buttonSlew2.UseVisualStyleBackColor = true;
            this.buttonSlew2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseDown);
            this.buttonSlew2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew2_MouseUp);
            // 
            // buttonSlew3
            // 
            resources.ApplyResources(this.buttonSlew3, "buttonSlew3");
            this.buttonSlew3.FlatAppearance.BorderSize = 0;
            this.buttonSlew3.Name = "buttonSlew3";
            this.buttonSlew3.UseVisualStyleBackColor = true;
            this.buttonSlew3.Click += new System.EventHandler(this.buttonSlew3_Click);
            this.buttonSlew3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseDown);
            this.buttonSlew3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew3_MouseUp);
            // 
            // buttonSlew4
            // 
            resources.ApplyResources(this.buttonSlew4, "buttonSlew4");
            this.buttonSlew4.FlatAppearance.BorderSize = 0;
            this.buttonSlew4.Name = "buttonSlew4";
            this.buttonSlew4.UseVisualStyleBackColor = true;
            this.buttonSlew4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseDown);
            this.buttonSlew4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew4_MouseUp);
            // 
            // buttonSlew1
            // 
            resources.ApplyResources(this.buttonSlew1, "buttonSlew1");
            this.buttonSlew1.FlatAppearance.BorderSize = 0;
            this.buttonSlew1.Name = "buttonSlew1";
            this.buttonSlew1.UseVisualStyleBackColor = false;
            this.buttonSlew1.Click += new System.EventHandler(this.buttonSlew1_Click);
            this.buttonSlew1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseDown);
            this.buttonSlew1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlew1_MouseUp);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
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
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
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
        private System.Windows.Forms.ToolStripMenuItem objectAndCoordinatesToolStripMenuItem;


    }
}

