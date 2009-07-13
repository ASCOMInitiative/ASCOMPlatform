namespace ASCOM.FilterWheelSim
{
    partial class frmSetupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupDialog));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmbTime = new System.Windows.Forms.ComboBox();
            this.cmbSlots = new System.Windows.Forms.ComboBox();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chkImplementsOffsets = new System.Windows.Forms.CheckBox();
            this.chkImplementsNames = new System.Windows.Forms.CheckBox();
            this.chkPreemptMoves = new System.Windows.Forms.CheckBox();
            this.lblDriverInfo = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.GroupBox2.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(230, 78);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Filter Wheel Settings";
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(3, 33);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(129, 13);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Time between slots (secs)";
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 6);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(102, 13);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "Number of filter slots";
            // 
            // cmbTime
            // 
            this.cmbTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmbTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5"});
            this.cmbTime.Location = new System.Drawing.Point(172, 29);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.Size = new System.Drawing.Size(43, 21);
            this.cmbTime.TabIndex = 1;
            this.cmbTime.Validating += new System.ComponentModel.CancelEventHandler(this.cmbTime_Validating);
            this.cmbTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbTime_KeyPress);
            // 
            // cmbSlots
            // 
            this.cmbSlots.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmbSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbSlots.FormattingEnabled = true;
            this.cmbSlots.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbSlots.Location = new System.Drawing.Point(171, 3);
            this.cmbSlots.Name = "cmbSlots";
            this.cmbSlots.Size = new System.Drawing.Size(44, 21);
            this.cmbSlots.TabIndex = 0;
            this.cmbSlots.Validating += new System.ComponentModel.CancelEventHandler(this.cmbSlots_Validating);
            this.cmbSlots.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbSlots_KeyPress);
            this.cmbSlots.SelectedValueChanged += new System.EventHandler(this.cmbSlots_SelectedValueChanged);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.FilterWheelSim.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(248, 18);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 4;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.picASCOM_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.TableLayoutPanel2);
            this.GroupBox2.Controls.Add(this.Label5);
            this.GroupBox2.Controls.Add(this.Label4);
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.ForeColor = System.Drawing.Color.White;
            this.GroupBox2.Location = new System.Drawing.Point(12, 96);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(284, 245);
            this.GroupBox2.TabIndex = 5;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Filter Setup";
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.ColumnCount = 4;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel2.Controls.Add(this.Label13, 0, 7);
            this.TableLayoutPanel2.Controls.Add(this.Label12, 0, 6);
            this.TableLayoutPanel2.Controls.Add(this.Label11, 0, 5);
            this.TableLayoutPanel2.Controls.Add(this.Label10, 0, 4);
            this.TableLayoutPanel2.Controls.Add(this.Label9, 0, 3);
            this.TableLayoutPanel2.Controls.Add(this.Label8, 0, 2);
            this.TableLayoutPanel2.Controls.Add(this.Label6, 0, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label7, 0, 1);
            this.TableLayoutPanel2.Location = new System.Drawing.Point(6, 32);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 8;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(272, 207);
            this.TableLayoutPanel2.TabIndex = 9;
            // 
            // Label13
            // 
            this.Label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(3, 188);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(37, 13);
            this.Label13.TabIndex = 6;
            this.Label13.Text = "Slot 7:";
            // 
            // Label12
            // 
            this.Label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(3, 162);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(37, 13);
            this.Label12.TabIndex = 6;
            this.Label12.Text = "Slot 6:";
            // 
            // Label11
            // 
            this.Label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(3, 136);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(37, 13);
            this.Label11.TabIndex = 6;
            this.Label11.Text = "Slot 5:";
            // 
            // Label10
            // 
            this.Label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(3, 110);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(37, 13);
            this.Label10.TabIndex = 6;
            this.Label10.Text = "Slot 4:";
            // 
            // Label9
            // 
            this.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(3, 84);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(37, 13);
            this.Label9.TabIndex = 6;
            this.Label9.Text = "Slot 3:";
            // 
            // Label8
            // 
            this.Label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(3, 58);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(37, 13);
            this.Label8.TabIndex = 6;
            this.Label8.Text = "Slot 2:";
            // 
            // Label6
            // 
            this.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(3, 6);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(37, 13);
            this.Label6.TabIndex = 6;
            this.Label6.Text = "Slot 0:";
            // 
            // Label7
            // 
            this.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(3, 32);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(37, 13);
            this.Label7.TabIndex = 6;
            this.Label7.Text = "Slot 1:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(244, 16);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(37, 13);
            this.Label5.TabIndex = 5;
            this.Label5.Text = "Colour";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(175, 16);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(65, 13);
            this.Label4.TabIndex = 4;
            this.Label4.Text = "Focus offset";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(49, 16);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(58, 13);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "Filter name";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel3.Controls.Add(this.chkImplementsOffsets, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.chkImplementsNames, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkPreemptMoves, 0, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 347);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(170, 74);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // chkImplementsOffsets
            // 
            this.chkImplementsOffsets.AutoSize = true;
            this.chkImplementsOffsets.Location = new System.Drawing.Point(3, 27);
            this.chkImplementsOffsets.Name = "chkImplementsOffsets";
            this.chkImplementsOffsets.Size = new System.Drawing.Size(147, 17);
            this.chkImplementsOffsets.TabIndex = 7;
            this.chkImplementsOffsets.Text = "Implements Focus Offsets";
            this.chkImplementsOffsets.UseVisualStyleBackColor = true;
            this.chkImplementsOffsets.CheckedChanged += new System.EventHandler(this.chkImplementsOffsets_CheckedChanged);
            // 
            // chkImplementsNames
            // 
            this.chkImplementsNames.AutoSize = true;
            this.chkImplementsNames.Location = new System.Drawing.Point(3, 3);
            this.chkImplementsNames.Name = "chkImplementsNames";
            this.chkImplementsNames.Size = new System.Drawing.Size(140, 17);
            this.chkImplementsNames.TabIndex = 6;
            this.chkImplementsNames.Text = "Implements Filter Names";
            this.chkImplementsNames.UseVisualStyleBackColor = true;
            this.chkImplementsNames.CheckedChanged += new System.EventHandler(this.chkImplementsNames_CheckedChanged);
            // 
            // chkPreemptMoves
            // 
            this.chkPreemptMoves.AutoSize = true;
            this.chkPreemptMoves.Location = new System.Drawing.Point(3, 51);
            this.chkPreemptMoves.Name = "chkPreemptMoves";
            this.chkPreemptMoves.Size = new System.Drawing.Size(157, 17);
            this.chkPreemptMoves.TabIndex = 10;
            this.chkPreemptMoves.Text = "Allow Pre-emption of Moves";
            this.chkPreemptMoves.UseVisualStyleBackColor = true;
            // 
            // lblDriverInfo
            // 
            this.lblDriverInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDriverInfo.AutoSize = true;
            this.lblDriverInfo.ForeColor = System.Drawing.Color.LemonChiffon;
            this.lblDriverInfo.Location = new System.Drawing.Point(9, 427);
            this.lblDriverInfo.Name = "lblDriverInfo";
            this.lblDriverInfo.Size = new System.Drawing.Size(90, 13);
            this.lblDriverInfo.TabIndex = 9;
            this.lblDriverInfo.Text = "<runtime version>";
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OK_Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OK_Button.Location = new System.Drawing.Point(38, 3);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(67, 31);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = false;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Cancel_Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Cancel_Button.Location = new System.Drawing.Point(38, 40);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(67, 31);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = false;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Cancel_Button, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(188, 347);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(108, 74);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.tableLayoutPanel4.Controls.Add(this.cmbTime, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.Label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.cmbSlots, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.Label1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(18, 31);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(218, 53);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // frmSetupDialog
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(307, 446);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblDriverInfo);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.GroupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetupDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Filter Wheel Simulator Setup";
            this.Shown += new System.EventHandler(this.frmSetupDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cmbTime;
        internal System.Windows.Forms.ComboBox cmbSlots;
        internal System.Windows.Forms.PictureBox picASCOM;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        internal System.Windows.Forms.CheckBox chkImplementsNames;
        internal System.Windows.Forms.CheckBox chkImplementsOffsets;
        private System.Windows.Forms.CheckBox chkPreemptMoves;
        internal System.Windows.Forms.Label lblDriverInfo;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}