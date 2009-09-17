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
            this.buttonNorth = new System.Windows.Forms.Button();
            this.buttonEast = new System.Windows.Forms.Button();
            this.buttonWest = new System.Windows.Forms.Button();
            this.buttonSouth = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.checkBoxTrack = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTraffic = new System.Windows.Forms.Button();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.buttonUnpark = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPARK = new System.Windows.Forms.Label();
            this.lblHOME = new System.Windows.Forms.Label();
            this.lblSlew = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 115);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(163, 100);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // labelAlt
            // 
            this.labelAlt.AutoSize = true;
            this.labelAlt.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAlt.ForeColor = System.Drawing.Color.Red;
            this.labelAlt.Location = new System.Drawing.Point(96, 80);
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
            this.labelAz.Location = new System.Drawing.Point(90, 60);
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
            this.labelDec.Location = new System.Drawing.Point(90, 40);
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
            this.labelRa.Location = new System.Drawing.Point(96, 20);
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
            this.labelLst.Location = new System.Drawing.Point(96, 0);
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel2.Controls.Add(this.buttonNorth, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonEast, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonWest, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSouth, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonStop, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 221);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(162, 136);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // buttonNorth
            // 
            this.buttonNorth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNorth.Location = new System.Drawing.Point(58, 3);
            this.buttonNorth.Name = "buttonNorth";
            this.buttonNorth.Size = new System.Drawing.Size(49, 41);
            this.buttonNorth.TabIndex = 0;
            this.buttonNorth.Text = "N";
            this.buttonNorth.UseVisualStyleBackColor = true;
            // 
            // buttonEast
            // 
            this.buttonEast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEast.Location = new System.Drawing.Point(113, 50);
            this.buttonEast.Name = "buttonEast";
            this.buttonEast.Size = new System.Drawing.Size(46, 41);
            this.buttonEast.TabIndex = 1;
            this.buttonEast.Text = "E";
            this.buttonEast.UseVisualStyleBackColor = true;
            // 
            // buttonWest
            // 
            this.buttonWest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonWest.Location = new System.Drawing.Point(3, 50);
            this.buttonWest.Name = "buttonWest";
            this.buttonWest.Size = new System.Drawing.Size(49, 41);
            this.buttonWest.TabIndex = 2;
            this.buttonWest.Text = "W";
            this.buttonWest.UseVisualStyleBackColor = true;
            // 
            // buttonSouth
            // 
            this.buttonSouth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSouth.Location = new System.Drawing.Point(58, 97);
            this.buttonSouth.Name = "buttonSouth";
            this.buttonSouth.Size = new System.Drawing.Size(49, 36);
            this.buttonSouth.TabIndex = 3;
            this.buttonSouth.Text = "S";
            this.buttonSouth.UseVisualStyleBackColor = true;
            // 
            // buttonStop
            // 
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.Font = new System.Drawing.Font("Wingdings 2", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonStop.Location = new System.Drawing.Point(58, 50);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(49, 41);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Ä";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrack
            // 
            this.checkBoxTrack.AutoSize = true;
            this.checkBoxTrack.ForeColor = System.Drawing.Color.White;
            this.checkBoxTrack.Location = new System.Drawing.Point(13, 363);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(13, 391);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(162, 65);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // buttonTraffic
            // 
            this.buttonTraffic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTraffic.Location = new System.Drawing.Point(84, 35);
            this.buttonTraffic.Name = "buttonTraffic";
            this.buttonTraffic.Size = new System.Drawing.Size(75, 27);
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
            this.buttonSetup.Size = new System.Drawing.Size(75, 27);
            this.buttonSetup.TabIndex = 2;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonHome.Location = new System.Drawing.Point(84, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(75, 26);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            // 
            // buttonUnpark
            // 
            this.buttonUnpark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUnpark.Location = new System.Drawing.Point(3, 3);
            this.buttonUnpark.Name = "buttonUnpark";
            this.buttonUnpark.Size = new System.Drawing.Size(75, 26);
            this.buttonUnpark.TabIndex = 0;
            this.buttonUnpark.Text = "Unpark";
            this.buttonUnpark.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.lblPARK, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblHOME, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblSlew, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(13, 89);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(162, 20);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // lblPARK
            // 
            this.lblPARK.AutoSize = true;
            this.lblPARK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPARK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPARK.Location = new System.Drawing.Point(111, 0);
            this.lblPARK.Name = "lblPARK";
            this.lblPARK.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblPARK.Size = new System.Drawing.Size(38, 16);
            this.lblPARK.TabIndex = 2;
            this.lblPARK.Text = "PARK";
            // 
            // lblHOME
            // 
            this.lblHOME.AutoSize = true;
            this.lblHOME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblHOME.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHOME.Location = new System.Drawing.Point(57, 0);
            this.lblHOME.Name = "lblHOME";
            this.lblHOME.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblHOME.Size = new System.Drawing.Size(41, 16);
            this.lblHOME.TabIndex = 1;
            this.lblHOME.Text = "HOME";
            // 
            // lblSlew
            // 
            this.lblSlew.AutoSize = true;
            this.lblSlew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSlew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSlew.Location = new System.Drawing.Point(3, 0);
            this.lblSlew.Name = "lblSlew";
            this.lblSlew.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.lblSlew.Size = new System.Drawing.Size(40, 16);
            this.lblSlew.TabIndex = 0;
            this.lblSlew.Text = "SLEW";
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.TelescopeSimulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(127, 12);
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
            this.ClientSize = new System.Drawing.Size(186, 468);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.checkBoxTrack);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.picASCOM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "Telescope Simulator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
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
        private System.Windows.Forms.Button buttonNorth;
        private System.Windows.Forms.Button buttonEast;
        private System.Windows.Forms.Button buttonWest;
        private System.Windows.Forms.Button buttonSouth;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.CheckBox checkBoxTrack;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonTraffic;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Button buttonUnpark;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal System.Windows.Forms.Label lblPARK;
        internal System.Windows.Forms.Label lblHOME;
        internal System.Windows.Forms.Label lblSlew;


    }
}

