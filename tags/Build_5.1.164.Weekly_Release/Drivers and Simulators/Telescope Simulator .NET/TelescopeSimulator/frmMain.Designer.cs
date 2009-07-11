using System;

namespace ASCOM.TelescopeSimulator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAlt = new System.Windows.Forms.Label();
            this.labelAz = new System.Windows.Forms.Label();
            this.labelDec = new System.Windows.Forms.Label();
            this.labelRa = new System.Windows.Forms.Label();
            this.labelLst = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxPierSideWest = new System.Windows.Forms.PictureBox();
            this.buttonSlew1 = new System.Windows.Forms.Button();
            this.buttonSlew3 = new System.Windows.Forms.Button();
            this.buttonSlew4 = new System.Windows.Forms.Button();
            this.buttonSlew2 = new System.Windows.Forms.Button();
            this.buttonSlew0 = new System.Windows.Forms.Button();
            this.pictureBoxPierSideEast = new System.Windows.Forms.PictureBox();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTraffic = new System.Windows.Forms.Button();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.buttonUnpark = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPARK = new System.Windows.Forms.Label();
            this.lblHOME = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSlew = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
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
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Dec";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Az";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.8125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.1875F));
            this.tableLayoutPanel1.Controls.Add(this.labelAlt, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelAz, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDec, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelRa, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLst, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(116, 100);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelAlt
            // 
            this.labelAlt.AutoSize = true;
            this.labelAlt.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAlt.ForeColor = System.Drawing.Color.Red;
            this.labelAlt.Location = new System.Drawing.Point(49, 80);
            this.labelAlt.Name = "labelAlt";
            this.labelAlt.Size = new System.Drawing.Size(64, 20);
            this.labelAlt.TabIndex = 13;
            this.labelAlt.Text = "00:00:00:00";
            // 
            // labelAz
            // 
            this.labelAz.AutoSize = true;
            this.labelAz.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAz.ForeColor = System.Drawing.Color.Red;
            this.labelAz.Location = new System.Drawing.Point(43, 60);
            this.labelAz.Name = "labelAz";
            this.labelAz.Size = new System.Drawing.Size(70, 20);
            this.labelAz.TabIndex = 12;
            this.labelAz.Text = "000:00:00:00";
            // 
            // labelDec
            // 
            this.labelDec.AutoSize = true;
            this.labelDec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDec.ForeColor = System.Drawing.Color.Red;
            this.labelDec.Location = new System.Drawing.Point(43, 40);
            this.labelDec.Name = "labelDec";
            this.labelDec.Size = new System.Drawing.Size(70, 20);
            this.labelDec.TabIndex = 11;
            this.labelDec.Text = "+00:00:00:00";
            // 
            // labelRa
            // 
            this.labelRa.AutoSize = true;
            this.labelRa.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelRa.ForeColor = System.Drawing.Color.Red;
            this.labelRa.Location = new System.Drawing.Point(49, 20);
            this.labelRa.Name = "labelRa";
            this.labelRa.Size = new System.Drawing.Size(64, 20);
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
            this.labelLst.Size = new System.Drawing.Size(64, 20);
            this.labelLst.TabIndex = 9;
            this.labelLst.Text = "00:00:00:00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Alt";
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 206);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(115, 111);
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
            // pictureBoxPierSideEast
            // 
            this.pictureBoxPierSideEast.Location = new System.Drawing.Point(79, 3);
            this.pictureBoxPierSideEast.Name = "pictureBoxPierSideEast";
            this.pictureBoxPierSideEast.Size = new System.Drawing.Size(33, 31);
            this.pictureBoxPierSideEast.TabIndex = 5;
            this.pictureBoxPierSideEast.TabStop = false;
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(17, 323);
            this.checkBoxTrack.Name = "checkBoxTrack";
            this.checkBoxTrack.Size = new System.Drawing.Size(54, 17);
            this.checkBoxTrack.TabIndex = 10;
            this.checkBoxTrack.Text = "Track";
            this.checkBoxTrack.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonTraffic, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonSetup, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonHome, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonUnpark, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 346);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(115, 65);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // buttonTraffic
            // 
            this.buttonTraffic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTraffic.Location = new System.Drawing.Point(60, 35);
            this.buttonTraffic.Name = "buttonTraffic";
            this.buttonTraffic.Size = new System.Drawing.Size(52, 27);
            this.buttonTraffic.TabIndex = 3;
            this.buttonTraffic.Text = "Traffic";
            this.buttonTraffic.UseVisualStyleBackColor = true;
            this.buttonTraffic.Click += new System.EventHandler(this.buttonTraffic_Click);
            // 
            // buttonSetup
            // 
            this.buttonSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetup.Location = new System.Drawing.Point(3, 35);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(51, 27);
            this.buttonSetup.TabIndex = 2;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHome.Location = new System.Drawing.Point(60, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(52, 26);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            // 
            // buttonUnpark
            // 
            this.buttonUnpark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUnpark.Location = new System.Drawing.Point(3, 3);
            this.buttonUnpark.Name = "buttonUnpark";
            this.buttonUnpark.Size = new System.Drawing.Size(51, 26);
            this.buttonUnpark.TabIndex = 0;
            this.buttonUnpark.Text = "Unpark";
            this.buttonUnpark.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.lblHOME, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPARK, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(12, 74);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(115, 20);
            this.tableLayoutPanel4.TabIndex = 12;
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
            // lblHOME
            // 
            this.lblHOME.AutoSize = true;
            this.lblHOME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblHOME.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblHOME.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHOME.Location = new System.Drawing.Point(3, 0);
            this.lblHOME.Name = "lblHOME";
            this.lblHOME.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblHOME.Size = new System.Drawing.Size(41, 20);
            this.lblHOME.TabIndex = 1;
            this.lblHOME.Text = "HOME";
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.TelescopeSimulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(79, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 4;
            this.picASCOM.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.labelSlew, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(78, 320);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(49, 20);
            this.tableLayoutPanel5.TabIndex = 13;
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
            this.labelSlew.Size = new System.Drawing.Size(43, 20);
            this.labelSlew.TabIndex = 0;
            this.labelSlew.Text = "SLEW";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(139, 419);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picASCOM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "Telescope Simulator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPierSideEast)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelLst;
        private System.Windows.Forms.Label labelAlt;
        private System.Windows.Forms.Label labelAz;
        private System.Windows.Forms.Label labelDec;
        private System.Windows.Forms.Label labelRa;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonSlew1;
        private System.Windows.Forms.Button buttonSlew3;
        private System.Windows.Forms.Button buttonSlew4;
        private System.Windows.Forms.Button buttonSlew2;
        private System.Windows.Forms.Button buttonSlew0;
        private System.Windows.Forms.CheckBox checkBoxTrack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonTraffic;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Button buttonUnpark;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal System.Windows.Forms.Label lblPARK;
        internal System.Windows.Forms.Label lblHOME;
        private System.Windows.Forms.PictureBox pictureBoxPierSideWest;
        private System.Windows.Forms.PictureBox pictureBoxPierSideEast;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        internal System.Windows.Forms.Label labelSlew;


    }
}

