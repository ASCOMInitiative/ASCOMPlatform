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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDec = new System.Windows.Forms.Label();
            this.labelRa = new System.Windows.Forms.Label();
            this.labelLst = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSlew1 = new System.Windows.Forms.Button();
            this.buttonSlew3 = new System.Windows.Forms.Button();
            this.buttonSlew4 = new System.Windows.Forms.Button();
            this.buttonSlew2 = new System.Windows.Forms.Button();
            this.buttonSlew0 = new System.Windows.Forms.Button();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSlew = new System.Windows.Forms.Label();
            this.lblPARK = new System.Windows.Forms.Label();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonPark = new System.Windows.Forms.Button();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ButtonSetup = new System.Windows.Forms.Button();
            this.ButtonFlip = new System.Windows.Forms.Button();
            this.CheckBoxFlipDeck = new System.Windows.Forms.CheckBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioButtonSlew = new System.Windows.Forms.RadioButton();
            this.RadioButtonCenter = new System.Windows.Forms.RadioButton();
            this.RadioButtonGuide = new System.Windows.Forms.RadioButton();
            this.CheckBoxFlipRa = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setupDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxPierSideWest = new System.Windows.Forms.PictureBox();
            this.pictureBoxPierSideEast = new System.Windows.Forms.PictureBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(116, 55);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelDec
            // 
            this.labelDec.AutoSize = true;
            this.labelDec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Location = new System.Drawing.Point(43, 34);
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
            this.labelRa.Location = new System.Drawing.Point(49, 17);
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
            this.labelLst.Location = new System.Drawing.Point(49, 0);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 161);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(115, 111);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // buttonSlew1
            // 
            this.buttonSlew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew1.Location = new System.Drawing.Point(41, 3);
            this.buttonSlew1.Name = "buttonSlew1";
            this.buttonSlew1.Size = new System.Drawing.Size(32, 31);
            this.buttonSlew1.TabIndex = 0;
            this.buttonSlew1.Text = "N";
            this.buttonSlew1.UseVisualStyleBackColor = true;
            // 
            // buttonSlew3
            // 
            this.buttonSlew3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew3.Location = new System.Drawing.Point(79, 40);
            this.buttonSlew3.Name = "buttonSlew3";
            this.buttonSlew3.Size = new System.Drawing.Size(33, 31);
            this.buttonSlew3.TabIndex = 1;
            this.buttonSlew3.Text = "E";
            this.buttonSlew3.UseVisualStyleBackColor = true;
            // 
            // buttonSlew4
            // 
            this.buttonSlew4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew4.Location = new System.Drawing.Point(3, 40);
            this.buttonSlew4.Name = "buttonSlew4";
            this.buttonSlew4.Size = new System.Drawing.Size(32, 31);
            this.buttonSlew4.TabIndex = 2;
            this.buttonSlew4.Text = "W";
            this.buttonSlew4.UseVisualStyleBackColor = true;
            // 
            // buttonSlew2
            // 
            this.buttonSlew2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew2.Location = new System.Drawing.Point(41, 77);
            this.buttonSlew2.Name = "buttonSlew2";
            this.buttonSlew2.Size = new System.Drawing.Size(32, 31);
            this.buttonSlew2.TabIndex = 3;
            this.buttonSlew2.Text = "S";
            this.buttonSlew2.UseVisualStyleBackColor = true;
            // 
            // buttonSlew0
            // 
            this.buttonSlew0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSlew0.Font = new System.Drawing.Font("Wingdings 2", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSlew0.Location = new System.Drawing.Point(41, 40);
            this.buttonSlew0.Name = "buttonSlew0";
            this.buttonSlew0.Size = new System.Drawing.Size(32, 31);
            this.buttonSlew0.TabIndex = 4;
            this.buttonSlew0.Text = "Ä";
            this.buttonSlew0.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(10, 358);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(54, 17);
            this.checkBoxTrack.TabIndex = 10;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.labelSlew, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPARK, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(12, 74);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(115, 20);
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
            this.labelSlew.Size = new System.Drawing.Size(51, 20);
            this.labelSlew.TabIndex = 3;
            this.labelSlew.Text = "SLEW";
            // 
            // lblPARK
            // 
            this.lblPARK.AutoSize = true;
            this.lblPARK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPARK.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPARK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPARK.Location = new System.Drawing.Point(74, 0);
            this.lblPARK.Name = "lblPARK";
            this.lblPARK.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblPARK.Size = new System.Drawing.Size(38, 20);
            this.lblPARK.TabIndex = 2;
            this.lblPARK.Text = "PARK";
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
            this.TableLayoutPanel3.Location = new System.Drawing.Point(11, 384);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 2;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel3.Size = new System.Drawing.Size(117, 54);
            this.TableLayoutPanel3.TabIndex = 14;
            // 
            // ButtonPark
            // 
            this.ButtonPark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPark.Enabled = false;
            this.ButtonPark.Location = new System.Drawing.Point(25, 3);
            this.ButtonPark.Name = "ButtonPark";
            this.ButtonPark.Size = new System.Drawing.Size(16, 21);
            this.ButtonPark.TabIndex = 6;
            this.ButtonPark.Text = "P";
            this.ButtonPark.UseVisualStyleBackColor = true;
            // 
            // ButtonConnect
            // 
            this.TableLayoutPanel3.SetColumnSpan(this.ButtonConnect, 5);
            this.ButtonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonConnect.Enabled = false;
            this.ButtonConnect.Location = new System.Drawing.Point(3, 30);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(111, 21);
            this.ButtonConnect.TabIndex = 5;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            // 
            // ButtonSetup
            // 
            this.ButtonSetup.ContextMenuStrip = this.contextMenuStrip1;
            this.ButtonSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonSetup.Location = new System.Drawing.Point(97, 3);
            this.ButtonSetup.Name = "ButtonSetup";
            this.ButtonSetup.Size = new System.Drawing.Size(17, 21);
            this.ButtonSetup.TabIndex = 4;
            this.ButtonSetup.Text = "S";
            this.ButtonSetup.UseVisualStyleBackColor = true;
            this.ButtonSetup.Click += new System.EventHandler(this.ButtonSetup_Click_1);
            // 
            // ButtonFlip
            // 
            this.ButtonFlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonFlip.Enabled = false;
            this.ButtonFlip.Location = new System.Drawing.Point(3, 3);
            this.ButtonFlip.Name = "ButtonFlip";
            this.ButtonFlip.Size = new System.Drawing.Size(16, 21);
            this.ButtonFlip.TabIndex = 3;
            this.ButtonFlip.Text = "F";
            this.ButtonFlip.UseVisualStyleBackColor = true;
            // 
            // CheckBoxFlipDeck
            // 
            this.CheckBoxFlipDeck.AutoSize = true;
            this.CheckBoxFlipDeck.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipDeck.Location = new System.Drawing.Point(81, 278);
            this.CheckBoxFlipDeck.Name = "CheckBoxFlipDeck";
            this.CheckBoxFlipDeck.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CheckBoxFlipDeck.Size = new System.Drawing.Size(46, 30);
            this.CheckBoxFlipDeck.TabIndex = 17;
            this.CheckBoxFlipDeck.Text = "Flip\r\nDec";
            this.CheckBoxFlipDeck.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.RadioButtonSlew);
            this.GroupBox1.Controls.Add(this.RadioButtonCenter);
            this.GroupBox1.Controls.Add(this.RadioButtonGuide);
            this.GroupBox1.Location = new System.Drawing.Point(10, 314);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(117, 38);
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
            this.RadioButtonSlew.UseVisualStyleBackColor = true;
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
            this.RadioButtonCenter.UseVisualStyleBackColor = true;
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
            this.RadioButtonGuide.UseVisualStyleBackColor = true;
            // 
            // CheckBoxFlipRa
            // 
            this.CheckBoxFlipRa.AutoSize = true;
            this.CheckBoxFlipRa.ForeColor = System.Drawing.Color.White;
            this.CheckBoxFlipRa.Location = new System.Drawing.Point(12, 278);
            this.CheckBoxFlipRa.Name = "CheckBoxFlipRa";
            this.CheckBoxFlipRa.Size = new System.Drawing.Size(42, 30);
            this.CheckBoxFlipRa.TabIndex = 15;
            this.CheckBoxFlipRa.Text = "Flip\r\nRa";
            this.CheckBoxFlipRa.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupDialogToolStripMenuItem,
            this.mountParametersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 70);
            // 
            // setupDialogToolStripMenuItem
            // 
            this.setupDialogToolStripMenuItem.Name = "setupDialogToolStripMenuItem";
            this.setupDialogToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.setupDialogToolStripMenuItem.Text = "Setup Dialog";
            this.setupDialogToolStripMenuItem.Click += new System.EventHandler(this.setupDialogToolStripMenuItem_Click);
            // 
            // mountParametersToolStripMenuItem
            // 
            this.mountParametersToolStripMenuItem.Name = "mountParametersToolStripMenuItem";
            this.mountParametersToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.mountParametersToolStripMenuItem.Text = "Mount Parameters";
            this.mountParametersToolStripMenuItem.Click += new System.EventHandler(this.mountParametersToolStripMenuItem_Click);
            // 
            // pictureBoxPierSideWest
            // 
            this.pictureBoxPierSideWest.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxPierSideWest.Name = "pictureBoxPierSideWest";
            this.pictureBoxPierSideWest.Size = new System.Drawing.Size(32, 31);
            this.pictureBoxPierSideWest.TabIndex = 6;
            this.pictureBoxPierSideWest.TabStop = false;
            // 
            // pictureBoxPierSideEast
            // 
            this.pictureBoxPierSideEast.Location = new System.Drawing.Point(79, 3);
            this.pictureBoxPierSideEast.Name = "pictureBoxPierSideEast";
            this.pictureBoxPierSideEast.Size = new System.Drawing.Size(33, 31);
            this.pictureBoxPierSideEast.TabIndex = 5;
            this.pictureBoxPierSideEast.TabStop = false;
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.GeminiTelescope.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(79, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 4;
            this.picASCOM.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(139, 450);
            this.Controls.Add(this.CheckBoxFlipDeck);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.CheckBoxFlipRa);
            this.Controls.Add(this.TableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picASCOM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "Gemini Telescope";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel3.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
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
        internal System.Windows.Forms.Label lblPARK;
        private System.Windows.Forms.PictureBox pictureBoxPierSideWest;
        private System.Windows.Forms.PictureBox pictureBoxPierSideEast;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        private System.Windows.Forms.Button ButtonPark;
        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.Button ButtonFlip;
        private System.Windows.Forms.Button ButtonSetup;
        internal System.Windows.Forms.CheckBox CheckBoxFlipDeck;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton RadioButtonSlew;
        internal System.Windows.Forms.RadioButton RadioButtonCenter;
        internal System.Windows.Forms.RadioButton RadioButtonGuide;
        internal System.Windows.Forms.CheckBox CheckBoxFlipRa;
        internal System.Windows.Forms.Label labelSlew;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mountParametersToolStripMenuItem;


    }
}

