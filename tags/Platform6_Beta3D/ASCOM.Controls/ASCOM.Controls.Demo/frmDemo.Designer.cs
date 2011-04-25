namespace ASCOM.Controls.Demo
	{
	partial class frmDemo
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
                this.btnOK = new System.Windows.Forms.Button();
                this.btnCancel = new System.Windows.Forms.Button();
                this.anunciatorPanel1 = new ASCOM.Controls.AnnunciatorPanel();
                this.anunciator1 = new ASCOM.Controls.Annunciator();
                this.anunciator2 = new ASCOM.Controls.Annunciator();
                this.anunciator3 = new ASCOM.Controls.Annunciator();
                this.anunciator4 = new ASCOM.Controls.Annunciator();
                this.anunciator5 = new ASCOM.Controls.Annunciator();
                this.anunciator7 = new ASCOM.Controls.Annunciator();
                this.anunciator6 = new ASCOM.Controls.Annunciator();
                this.anunciator8 = new ASCOM.Controls.Annunciator();
                this.label1 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.ledIndicator1 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator2 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator4 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator5 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator6 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator3 = new ASCOM.Controls.LedIndicator();
                this.ledIndicator7 = new ASCOM.Controls.LedIndicator();
                this.anunciatorPanel1.SuspendLayout();
                this.SuspendLayout();
                // 
                // btnOK
                // 
                this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.btnOK.Location = new System.Drawing.Point(203, 331);
                this.btnOK.Name = "btnOK";
                this.btnOK.Size = new System.Drawing.Size(75, 23);
                this.btnOK.TabIndex = 1;
                this.btnOK.Text = "OK";
                this.btnOK.UseVisualStyleBackColor = true;
                this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
                // 
                // btnCancel
                // 
                this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.btnCancel.Location = new System.Drawing.Point(284, 331);
                this.btnCancel.Name = "btnCancel";
                this.btnCancel.Size = new System.Drawing.Size(75, 23);
                this.btnCancel.TabIndex = 2;
                this.btnCancel.Text = "Cancel";
                this.btnCancel.UseVisualStyleBackColor = true;
                this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
                // 
                // anunciatorPanel1
                // 
                this.anunciatorPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciatorPanel1.Controls.Add(this.anunciator1);
                this.anunciatorPanel1.Controls.Add(this.anunciator2);
                this.anunciatorPanel1.Controls.Add(this.anunciator3);
                this.anunciatorPanel1.Controls.Add(this.anunciator4);
                this.anunciatorPanel1.Controls.Add(this.anunciator5);
                this.anunciatorPanel1.Controls.Add(this.anunciator7);
                this.anunciatorPanel1.Controls.Add(this.anunciator6);
                this.anunciatorPanel1.Controls.Add(this.anunciator8);
                this.anunciatorPanel1.Location = new System.Drawing.Point(12, 50);
                this.anunciatorPanel1.Name = "anunciatorPanel1";
                this.anunciatorPanel1.Size = new System.Drawing.Size(291, 38);
                this.anunciatorPanel1.TabIndex = 3;
                // 
                // anunciator1
                // 
                this.anunciator1.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator1.AutoSize = true;
                this.anunciator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator1.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator1.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator1.Location = new System.Drawing.Point(3, 0);
                this.anunciator1.Mute = false;
                this.anunciator1.Name = "anunciator1";
                this.anunciator1.Size = new System.Drawing.Size(40, 17);
                this.anunciator1.TabIndex = 0;
                this.anunciator1.Text = "SLEW";
                // 
                // anunciator2
                // 
                this.anunciator2.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator2.AutoSize = true;
                this.anunciator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator2.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator2.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator2.Location = new System.Drawing.Point(49, 0);
                this.anunciator2.Mute = false;
                this.anunciator2.Name = "anunciator2";
                this.anunciator2.Size = new System.Drawing.Size(64, 17);
                this.anunciator2.TabIndex = 1;
                this.anunciator2.Text = "SHUTTER";
                // 
                // anunciator3
                // 
                this.anunciator3.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator3.AutoSize = true;
                this.anunciator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator3.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator3.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator3.Location = new System.Drawing.Point(119, 0);
                this.anunciator3.Mute = false;
                this.anunciator3.Name = "anunciator3";
                this.anunciator3.Size = new System.Drawing.Size(40, 17);
                this.anunciator3.TabIndex = 2;
                this.anunciator3.Text = "DOME";
                // 
                // anunciator4
                // 
                this.anunciator4.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator4.AutoSize = true;
                this.anunciator4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator4.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
                this.anunciator4.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator4.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator4.Location = new System.Drawing.Point(165, 0);
                this.anunciator4.Mute = false;
                this.anunciator4.Name = "anunciator4";
                this.anunciator4.Size = new System.Drawing.Size(48, 17);
                this.anunciator4.TabIndex = 3;
                this.anunciator4.Text = "ERROR";
                // 
                // anunciator5
                // 
                this.anunciator5.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator5.AutoSize = true;
                this.anunciator5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator5.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator5.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator5.Location = new System.Drawing.Point(219, 0);
                this.anunciator5.Mute = false;
                this.anunciator5.Name = "anunciator5";
                this.anunciator5.Size = new System.Drawing.Size(48, 17);
                this.anunciator5.TabIndex = 4;
                this.anunciator5.Text = "FOCUS";
                // 
                // anunciator7
                // 
                this.anunciator7.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                this.anunciator7.AutoSize = true;
                this.anunciator7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator7.Cadence = ASCOM.Controls.CadencePattern.Wink;
                this.anunciator7.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                this.anunciator7.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator7.Location = new System.Drawing.Point(3, 17);
                this.anunciator7.Mute = false;
                this.anunciator7.Name = "anunciator7";
                this.anunciator7.Size = new System.Drawing.Size(64, 17);
                this.anunciator7.TabIndex = 6;
                this.anunciator7.Text = "WEATHER";
                // 
                // anunciator6
                // 
                this.anunciator6.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator6.AutoSize = true;
                this.anunciator6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator6.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator6.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator6.Location = new System.Drawing.Point(73, 17);
                this.anunciator6.Mute = false;
                this.anunciator6.Name = "anunciator6";
                this.anunciator6.Size = new System.Drawing.Size(72, 17);
                this.anunciator6.TabIndex = 5;
                this.anunciator6.Text = "POINTING";
                // 
                // anunciator8
                // 
                this.anunciator8.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator8.AutoSize = true;
                this.anunciator8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.anunciator8.Cadence = ASCOM.Controls.CadencePattern.Strobe;
                this.anunciator8.Font = new System.Drawing.Font("Consolas", 10F);
                this.anunciator8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator8.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
                this.anunciator8.Location = new System.Drawing.Point(151, 17);
                this.anunciator8.Mute = false;
                this.anunciator8.Name = "anunciator8";
                this.anunciator8.Size = new System.Drawing.Size(40, 17);
                this.anunciator8.TabIndex = 7;
                this.anunciator8.Text = "SYNC";
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(18, 31);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(260, 13);
                this.label1.TabIndex = 4;
                this.label1.Text = "Annunciator Panel with Annunciators in various states";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(9, 127);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(238, 13);
                this.label2.TabIndex = 5;
                this.label2.Text = "LED Indicators with various states and cadences";
                // 
                // ledIndicator1
                // 
                this.ledIndicator1.AutoSize = true;
                this.ledIndicator1.Cadence = ASCOM.Controls.CadencePattern.SteadyOff;
                this.ledIndicator1.LabelText = "SteadyOff";
                this.ledIndicator1.Location = new System.Drawing.Point(18, 143);
                this.ledIndicator1.Name = "ledIndicator1";
                this.ledIndicator1.Size = new System.Drawing.Size(100, 16);
                this.ledIndicator1.TabIndex = 6;
                // 
                // ledIndicator2
                // 
                this.ledIndicator2.AutoSize = true;
                this.ledIndicator2.LabelText = "SteadyOn";
                this.ledIndicator2.Location = new System.Drawing.Point(132, 142);
                this.ledIndicator2.Name = "ledIndicator2";
                this.ledIndicator2.Size = new System.Drawing.Size(100, 16);
                this.ledIndicator2.TabIndex = 6;
                // 
                // ledIndicator4
                // 
                this.ledIndicator4.AutoSize = true;
                this.ledIndicator4.Cadence = ASCOM.Controls.CadencePattern.Strobe;
                this.ledIndicator4.LabelText = "Strobe";
                this.ledIndicator4.Location = new System.Drawing.Point(18, 165);
                this.ledIndicator4.Name = "ledIndicator4";
                this.ledIndicator4.Size = new System.Drawing.Size(68, 16);
                this.ledIndicator4.TabIndex = 6;
                // 
                // ledIndicator5
                // 
                this.ledIndicator5.AutoSize = true;
                this.ledIndicator5.Cadence = ASCOM.Controls.CadencePattern.Wink;
                this.ledIndicator5.LabelText = "Wink";
                this.ledIndicator5.Location = new System.Drawing.Point(132, 164);
                this.ledIndicator5.Name = "ledIndicator5";
                this.ledIndicator5.Size = new System.Drawing.Size(56, 16);
                this.ledIndicator5.TabIndex = 6;
                // 
                // ledIndicator6
                // 
                this.ledIndicator6.AutoSize = true;
                this.ledIndicator6.Cadence = ASCOM.Controls.CadencePattern.BlinkSlow;
                this.ledIndicator6.LabelText = "BlinkSlow";
                this.ledIndicator6.Location = new System.Drawing.Point(18, 187);
                this.ledIndicator6.Name = "ledIndicator6";
                this.ledIndicator6.Size = new System.Drawing.Size(113, 16);
                this.ledIndicator6.Status = ASCOM.Controls.TrafficLight.Yellow;
                this.ledIndicator6.TabIndex = 6;
                // 
                // ledIndicator3
                // 
                this.ledIndicator3.AutoSize = true;
                this.ledIndicator3.Cadence = ASCOM.Controls.CadencePattern.BlinkFast;
                this.ledIndicator3.LabelText = "BlinkFast";
                this.ledIndicator3.Location = new System.Drawing.Point(132, 187);
                this.ledIndicator3.Name = "ledIndicator3";
                this.ledIndicator3.Size = new System.Drawing.Size(113, 16);
                this.ledIndicator3.Status = ASCOM.Controls.TrafficLight.Yellow;
                this.ledIndicator3.TabIndex = 6;
                // 
                // ledIndicator7
                // 
                this.ledIndicator7.AutoSize = true;
                this.ledIndicator7.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
                this.ledIndicator7.LabelText = "BlinkAlarm";
                this.ledIndicator7.Location = new System.Drawing.Point(234, 187);
                this.ledIndicator7.Name = "ledIndicator7";
                this.ledIndicator7.Size = new System.Drawing.Size(113, 16);
                this.ledIndicator7.Status = ASCOM.Controls.TrafficLight.Red;
                this.ledIndicator7.TabIndex = 6;
                // 
                // frmDemo
                // 
                this.AcceptButton = this.btnOK;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.AutoSize = true;
                this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.CancelButton = this.btnCancel;
                this.ClientSize = new System.Drawing.Size(371, 366);
                this.Controls.Add(this.ledIndicator7);
                this.Controls.Add(this.ledIndicator3);
                this.Controls.Add(this.ledIndicator6);
                this.Controls.Add(this.ledIndicator5);
                this.Controls.Add(this.ledIndicator2);
                this.Controls.Add(this.ledIndicator4);
                this.Controls.Add(this.ledIndicator1);
                this.Controls.Add(this.label2);
                this.Controls.Add(this.label1);
                this.Controls.Add(this.anunciatorPanel1);
                this.Controls.Add(this.btnCancel);
                this.Controls.Add(this.btnOK);
                this.Name = "frmDemo";
                this.Text = "ASCOM Common Controls Demo";
                this.Load += new System.EventHandler(this.frmDemo_Load);
                this.anunciatorPanel1.ResumeLayout(false);
                this.anunciatorPanel1.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private AnnunciatorPanel anunciatorPanel1;
		private Annunciator anunciator1;
		private Annunciator anunciator2;
		private Annunciator anunciator3;
		private Annunciator anunciator4;
		private Annunciator anunciator5;
		private Annunciator anunciator6;
		private Annunciator anunciator7;
        private Annunciator anunciator8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private LedIndicator ledIndicator1;
        private LedIndicator ledIndicator2;
        private LedIndicator ledIndicator4;
        private LedIndicator ledIndicator5;
        private LedIndicator ledIndicator6;
        private LedIndicator ledIndicator3;
        private LedIndicator ledIndicator7;
		}
	}

