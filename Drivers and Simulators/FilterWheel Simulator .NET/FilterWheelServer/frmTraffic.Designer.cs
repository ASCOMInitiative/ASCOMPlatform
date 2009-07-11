namespace ASCOM.FilterWheelSim
{
    partial class frmTraffic
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkOther = new System.Windows.Forms.CheckBox();
            this.chkName = new System.Windows.Forms.CheckBox();
            this.chkPosition = new System.Windows.Forms.CheckBox();
            this.chkMoving = new System.Windows.Forms.CheckBox();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.txtTraffic = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.68932F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.31068F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.Controls.Add(this.chkOther, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkPosition, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkMoving, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDisable, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClear, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.picASCOM, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(353, 65);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Checked = true;
            this.chkOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOther.Location = new System.Drawing.Point(3, 35);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(76, 17);
            this.chkOther.TabIndex = 7;
            this.chkOther.Text = "Other calls";
            this.chkOther.UseVisualStyleBackColor = true;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Checked = true;
            this.chkName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkName.Location = new System.Drawing.Point(92, 35);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(87, 17);
            this.chkName.TabIndex = 6;
            this.chkName.Text = "Name/Offset";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // chkPosition
            // 
            this.chkPosition.AutoSize = true;
            this.chkPosition.Checked = true;
            this.chkPosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPosition.Location = new System.Drawing.Point(3, 3);
            this.chkPosition.Name = "chkPosition";
            this.chkPosition.Size = new System.Drawing.Size(63, 17);
            this.chkPosition.TabIndex = 4;
            this.chkPosition.Text = "Position";
            this.chkPosition.UseVisualStyleBackColor = true;
            // 
            // chkMoving
            // 
            this.chkMoving.AutoSize = true;
            this.chkMoving.Checked = true;
            this.chkMoving.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMoving.Location = new System.Drawing.Point(92, 3);
            this.chkMoving.Name = "chkMoving";
            this.chkMoving.Size = new System.Drawing.Size(61, 17);
            this.chkMoving.TabIndex = 5;
            this.chkMoving.Text = "Moving";
            this.chkMoving.UseVisualStyleBackColor = true;
            // 
            // btnDisable
            // 
            this.btnDisable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDisable.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDisable.ForeColor = System.Drawing.Color.Black;
            this.btnDisable.Location = new System.Drawing.Point(218, 5);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(67, 22);
            this.btnDisable.TabIndex = 8;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = false;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClear.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(218, 37);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(67, 22);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.FilterWheelSim.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(302, 3);
            this.picASCOM.Name = "picASCOM";
            this.tableLayoutPanel1.SetRowSpan(this.picASCOM, 2);
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 10;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.picASCOM_Click);
            // 
            // txtTraffic
            // 
            this.txtTraffic.BackColor = System.Drawing.SystemColors.Info;
            this.txtTraffic.CausesValidation = false;
            this.txtTraffic.Location = new System.Drawing.Point(12, 81);
            this.txtTraffic.Multiline = true;
            this.txtTraffic.Name = "txtTraffic";
            this.txtTraffic.ReadOnly = true;
            this.txtTraffic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTraffic.Size = new System.Drawing.Size(347, 313);
            this.txtTraffic.TabIndex = 4;
            this.txtTraffic.Text = "Hello";
            // 
            // frmTraffic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(374, 404);
            this.Controls.Add(this.txtTraffic);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTraffic";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ASCOM API Calls";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.CheckBox chkPosition;
        internal System.Windows.Forms.CheckBox chkMoving;
        internal System.Windows.Forms.CheckBox chkName;
        internal System.Windows.Forms.CheckBox chkOther;
        internal System.Windows.Forms.Button btnDisable;
        internal System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.PictureBox picASCOM;
        internal System.Windows.Forms.TextBox txtTraffic;
    }
}