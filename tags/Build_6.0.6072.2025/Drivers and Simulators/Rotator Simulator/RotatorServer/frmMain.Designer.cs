using System;

namespace ASCOM.Simulator
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
			this.lblPosition = new System.Windows.Forms.Label();
			this.cmdMove = new System.Windows.Forms.Button();
			this.txtPosition = new System.Windows.Forms.TextBox();
			this.chkConnected = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCON = new System.Windows.Forms.Label();
			this.lblREV = new System.Windows.Forms.Label();
			this.lblMOVE = new System.Windows.Forms.Label();
			this.cmdSetup = new System.Windows.Forms.Button();
			this.tmrMain = new System.Windows.Forms.Timer(this.components);
			this.cmdHalt = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(11, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Current PA:";
			// 
			// lblPosition
			// 
			this.lblPosition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.lblPosition.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
			this.lblPosition.Location = new System.Drawing.Point(71, 61);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(51, 23);
			this.lblPosition.TabIndex = 1;
			this.lblPosition.Text = "---.-";
			this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmdMove
			// 
			this.cmdMove.BackColor = System.Drawing.SystemColors.Control;
			this.cmdMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdMove.Location = new System.Drawing.Point(13, 127);
			this.cmdMove.Name = "cmdMove";
			this.cmdMove.Size = new System.Drawing.Size(54, 22);
			this.cmdMove.TabIndex = 2;
			this.cmdMove.Text = "Move";
			this.cmdMove.UseVisualStyleBackColor = false;
			this.cmdMove.Click += new System.EventHandler(this.cmdMove_Click);
			// 
			// txtPosition
			// 
			this.txtPosition.BackColor = System.Drawing.SystemColors.Window;
			this.txtPosition.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPosition.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtPosition.Location = new System.Drawing.Point(74, 128);
			this.txtPosition.Name = "txtPosition";
			this.txtPosition.Size = new System.Drawing.Size(46, 20);
			this.txtPosition.TabIndex = 3;
			// 
			// chkConnected
			// 
			this.chkConnected.AutoSize = true;
			this.chkConnected.ForeColor = System.Drawing.Color.White;
			this.chkConnected.Location = new System.Drawing.Point(14, 189);
			this.chkConnected.Name = "chkConnected";
			this.chkConnected.Size = new System.Drawing.Size(78, 17);
			this.chkConnected.TabIndex = 4;
			this.chkConnected.Text = "Connected";
			this.chkConnected.UseVisualStyleBackColor = true;
			this.chkConnected.CheckedChanged += new System.EventHandler(this.chkConnected_CheckedChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::ASCOM.Simulator.Properties.Resources.saturnc;
			this.pictureBox1.InitialImage = global::ASCOM.Simulator.Properties.Resources.saturnc;
			this.pictureBox1.Location = new System.Drawing.Point(28, 14);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(78, 37);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.panel1.Controls.Add(this.lblCON);
			this.panel1.Controls.Add(this.lblREV);
			this.panel1.Controls.Add(this.lblMOVE);
			this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
			this.panel1.Location = new System.Drawing.Point(14, 92);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(108, 25);
			this.panel1.TabIndex = 6;
			// 
			// lblCON
			// 
			this.lblCON.AutoSize = true;
			this.lblCON.BackColor = System.Drawing.Color.Transparent;
			this.lblCON.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCON.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
			this.lblCON.Location = new System.Drawing.Point(3, 6);
			this.lblCON.Name = "lblCON";
			this.lblCON.Size = new System.Drawing.Size(31, 13);
			this.lblCON.TabIndex = 4;
			this.lblCON.Text = "CON";
			this.lblCON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblREV
			// 
			this.lblREV.AutoSize = true;
			this.lblREV.BackColor = System.Drawing.Color.Transparent;
			this.lblREV.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblREV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
			this.lblREV.Location = new System.Drawing.Point(76, 6);
			this.lblREV.Name = "lblREV";
			this.lblREV.Size = new System.Drawing.Size(31, 13);
			this.lblREV.TabIndex = 3;
			this.lblREV.Text = "REV";
			this.lblREV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMOVE
			// 
			this.lblMOVE.AutoSize = true;
			this.lblMOVE.BackColor = System.Drawing.Color.Transparent;
			this.lblMOVE.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMOVE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
			this.lblMOVE.Location = new System.Drawing.Point(37, 6);
			this.lblMOVE.Name = "lblMOVE";
			this.lblMOVE.Size = new System.Drawing.Size(39, 13);
			this.lblMOVE.TabIndex = 2;
			this.lblMOVE.Text = "MOVE";
			this.lblMOVE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmdSetup
			// 
			this.cmdSetup.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdSetup.Location = new System.Drawing.Point(30, 215);
			this.cmdSetup.Name = "cmdSetup";
			this.cmdSetup.Size = new System.Drawing.Size(75, 23);
			this.cmdSetup.TabIndex = 7;
			this.cmdSetup.Text = "Setup";
			this.cmdSetup.UseVisualStyleBackColor = false;
			this.cmdSetup.Click += new System.EventHandler(this.cmdSetup_Click);
			// 
			// tmrMain
			// 
			this.tmrMain.Interval = 250;
			this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
			// 
			// cmdHalt
			// 
			this.cmdHalt.BackColor = System.Drawing.SystemColors.Control;
			this.cmdHalt.Enabled = false;
			this.cmdHalt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdHalt.Location = new System.Drawing.Point(13, 157);
			this.cmdHalt.Name = "cmdHalt";
			this.cmdHalt.Size = new System.Drawing.Size(54, 22);
			this.cmdHalt.TabIndex = 8;
			this.cmdHalt.Text = "Halt";
			this.cmdHalt.UseVisualStyleBackColor = false;
			this.cmdHalt.Click += new System.EventHandler(this.cmdHalt_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(135, 252);
			this.Controls.Add(this.cmdHalt);
			this.Controls.Add(this.cmdSetup);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.chkConnected);
			this.Controls.Add(this.txtPosition);
			this.Controls.Add(this.cmdMove);
			this.Controls.Add(this.lblPosition);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmMain";
			this.Text = "Rotator Simulator";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.Button cmdMove;
		private System.Windows.Forms.TextBox txtPosition;
		private System.Windows.Forms.CheckBox chkConnected;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblMOVE;
		private System.Windows.Forms.Label lblREV;
		private System.Windows.Forms.Label lblCON;
		private System.Windows.Forms.Button cmdSetup;
		private System.Windows.Forms.Timer tmrMain;
		private System.Windows.Forms.Button cmdHalt;


	}
}

