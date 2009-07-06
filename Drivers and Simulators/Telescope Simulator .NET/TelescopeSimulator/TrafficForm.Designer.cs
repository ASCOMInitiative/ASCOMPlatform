namespace ASCOM.TelescopeSimulator
{
    partial class TrafficForm
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
            this.checkBoxAllOther = new System.Windows.Forms.CheckBox();
            this.ButtonDisable = new System.Windows.Forms.Button();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.checkBoxTime = new System.Windows.Forms.CheckBox();
            this.checkBoxCapabilities = new System.Windows.Forms.CheckBox();
            this.checkBoxGets = new System.Windows.Forms.CheckBox();
            this.checkBoxPolls = new System.Windows.Forms.CheckBox();
            this.checkBoxSlew = new System.Windows.Forms.CheckBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTraffic = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.TableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxAllOther
            // 
            this.checkBoxAllOther.AutoSize = true;
            this.checkBoxAllOther.ForeColor = System.Drawing.Color.White;
            this.checkBoxAllOther.Location = new System.Drawing.Point(15, 134);
            this.checkBoxAllOther.Name = "checkBoxAllOther";
            this.checkBoxAllOther.Size = new System.Drawing.Size(66, 17);
            this.checkBoxAllOther.TabIndex = 20;
            this.checkBoxAllOther.Text = "All Other";
            this.checkBoxAllOther.UseVisualStyleBackColor = true;
            // 
            // ButtonDisable
            // 
            this.ButtonDisable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ButtonDisable.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ButtonDisable.ForeColor = System.Drawing.Color.Black;
            this.ButtonDisable.Location = new System.Drawing.Point(4, 3);
            this.ButtonDisable.Name = "ButtonDisable";
            this.ButtonDisable.Size = new System.Drawing.Size(67, 22);
            this.ButtonDisable.TabIndex = 0;
            this.ButtonDisable.Text = "Disable";
            this.ButtonDisable.UseVisualStyleBackColor = false;
            this.ButtonDisable.Click += new System.EventHandler(this.ButtonDisable_Click);
            // 
            // ButtonClear
            // 
            this.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ButtonClear.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ButtonClear.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClear.ForeColor = System.Drawing.Color.Black;
            this.ButtonClear.Location = new System.Drawing.Point(4, 31);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(67, 22);
            this.ButtonClear.TabIndex = 1;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = false;
            // 
            // checkBoxTime
            // 
            this.checkBoxTime.AutoSize = true;
            this.checkBoxTime.ForeColor = System.Drawing.Color.White;
            this.checkBoxTime.Location = new System.Drawing.Point(15, 110);
            this.checkBoxTime.Name = "checkBoxTime";
            this.checkBoxTime.Size = new System.Drawing.Size(114, 17);
            this.checkBoxTime.TabIndex = 19;
            this.checkBoxTime.Text = "UTC, Siderial Time";
            this.checkBoxTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxCapabilities
            // 
            this.checkBoxCapabilities.AutoSize = true;
            this.checkBoxCapabilities.ForeColor = System.Drawing.Color.White;
            this.checkBoxCapabilities.Location = new System.Drawing.Point(15, 87);
            this.checkBoxCapabilities.Name = "checkBoxCapabilities";
            this.checkBoxCapabilities.Size = new System.Drawing.Size(214, 17);
            this.checkBoxCapabilities.TabIndex = 18;
            this.checkBoxCapabilities.Text = "Capabilities: Can Flags, Alignment Mode";
            this.checkBoxCapabilities.UseVisualStyleBackColor = true;
            // 
            // checkBoxGets
            // 
            this.checkBoxGets.AutoSize = true;
            this.checkBoxGets.ForeColor = System.Drawing.Color.White;
            this.checkBoxGets.Location = new System.Drawing.Point(15, 64);
            this.checkBoxGets.Name = "checkBoxGets";
            this.checkBoxGets.Size = new System.Drawing.Size(204, 17);
            this.checkBoxGets.TabIndex = 17;
            this.checkBoxGets.Text = "Get: Alt/Az, RA/Dec, Target RA/Dec";
            this.checkBoxGets.UseVisualStyleBackColor = true;
            // 
            // checkBoxPolls
            // 
            this.checkBoxPolls.AutoSize = true;
            this.checkBoxPolls.ForeColor = System.Drawing.Color.White;
            this.checkBoxPolls.Location = new System.Drawing.Point(15, 41);
            this.checkBoxPolls.Name = "checkBoxPolls";
            this.checkBoxPolls.Size = new System.Drawing.Size(162, 17);
            this.checkBoxPolls.TabIndex = 16;
            this.checkBoxPolls.Text = "Polls: Tracking, Slewing, At\'s";
            this.checkBoxPolls.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlew
            // 
            this.checkBoxSlew.AutoSize = true;
            this.checkBoxSlew.ForeColor = System.Drawing.Color.White;
            this.checkBoxSlew.Location = new System.Drawing.Point(15, 18);
            this.checkBoxSlew.Name = "checkBoxSlew";
            this.checkBoxSlew.Size = new System.Drawing.Size(204, 17);
            this.checkBoxSlew.TabIndex = 15;
            this.checkBoxSlew.Text = "Slew, Sync, Park/Unpark, Find Home";
            this.checkBoxSlew.UseVisualStyleBackColor = true;
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.ErrorImage = null;
            this.picASCOM.Image = global::ASCOM.TelescopeSimulator.Properties.Resources.ASCOM;
            this.picASCOM.InitialImage = null;
            this.picASCOM.Location = new System.Drawing.Point(311, 18);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 14;
            this.picASCOM.TabStop = false;
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Controls.Add(this.ButtonDisable, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.ButtonClear, 0, 1);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(228, 18);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 2;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(76, 56);
            this.TableLayoutPanel1.TabIndex = 13;
            // 
            // txtTraffic
            // 
            this.txtTraffic.BackColor = System.Drawing.SystemColors.Info;
            this.txtTraffic.CausesValidation = false;
            this.txtTraffic.Location = new System.Drawing.Point(12, 157);
            this.txtTraffic.Multiline = true;
            this.txtTraffic.Name = "txtTraffic";
            this.txtTraffic.ReadOnly = true;
            this.txtTraffic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTraffic.Size = new System.Drawing.Size(347, 313);
            this.txtTraffic.TabIndex = 12;
            this.txtTraffic.Text = "Hello";
            // 
            // TrafficForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(372, 484);
            this.Controls.Add(this.checkBoxAllOther);
            this.Controls.Add(this.checkBoxTime);
            this.Controls.Add(this.checkBoxCapabilities);
            this.Controls.Add(this.checkBoxGets);
            this.Controls.Add(this.checkBoxPolls);
            this.Controls.Add(this.checkBoxSlew);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.TableLayoutPanel1);
            this.Controls.Add(this.txtTraffic);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TrafficForm";
            this.Text = "TrafficForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TrafficForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox checkBoxAllOther;
        internal System.Windows.Forms.Button ButtonDisable;
        internal System.Windows.Forms.Button ButtonClear;
        internal System.Windows.Forms.CheckBox checkBoxTime;
        internal System.Windows.Forms.CheckBox checkBoxCapabilities;
        internal System.Windows.Forms.CheckBox checkBoxGets;
        internal System.Windows.Forms.CheckBox checkBoxPolls;
        internal System.Windows.Forms.CheckBox checkBoxSlew;
        internal System.Windows.Forms.PictureBox picASCOM;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.TextBox txtTraffic;
    }
}