namespace ASCOM.GeminiTelescope
{
    partial class frmJoystickConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJoystickConfig));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbAxisDEC = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAxisRA = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonGrid = new System.Windows.Forms.DataGridView();
            this.ButtonNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Assignment = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSensitivity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.rbFixed = new System.Windows.Forms.RadioButton();
            this.rbAnalog = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ckFlipRA = new System.Windows.Forms.CheckBox();
            this.ckFlipDec = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonGrid)).BeginInit();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSensitivity)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.GroupBox1, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 515);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ckFlipDec);
            this.groupBox3.Controls.Add(this.ckFlipRA);
            this.groupBox3.Controls.Add(this.cmbAxisDEC);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cmbAxisRA);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(389, 84);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controller Selection";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // cmbAxisDEC
            // 
            this.cmbAxisDEC.BackColor = System.Drawing.Color.Black;
            this.cmbAxisDEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAxisDEC.ForeColor = System.Drawing.Color.White;
            this.cmbAxisDEC.FormattingEnabled = true;
            this.cmbAxisDEC.Location = new System.Drawing.Point(237, 47);
            this.cmbAxisDEC.Name = "cmbAxisDEC";
            this.cmbAxisDEC.Size = new System.Drawing.Size(92, 21);
            this.cmbAxisDEC.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Select controller axis to move Gemini in DEC:";
            // 
            // cmbAxisRA
            // 
            this.cmbAxisRA.BackColor = System.Drawing.Color.Black;
            this.cmbAxisRA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAxisRA.ForeColor = System.Drawing.Color.White;
            this.cmbAxisRA.FormattingEnabled = true;
            this.cmbAxisRA.Location = new System.Drawing.Point(237, 21);
            this.cmbAxisRA.Name = "cmbAxisRA";
            this.cmbAxisRA.Size = new System.Drawing.Size(92, 21);
            this.cmbAxisRA.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select controller axis to move Gemini in RA:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(3, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(389, 308);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Joystick Button Assignment";
            // 
            // ButtonGrid
            // 
            this.ButtonGrid.AllowUserToAddRows = false;
            this.ButtonGrid.AllowUserToDeleteRows = false;
            this.ButtonGrid.AllowUserToResizeColumns = false;
            this.ButtonGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            this.ButtonGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ButtonGrid.BackgroundColor = System.Drawing.Color.Black;
            this.ButtonGrid.CausesValidation = false;
            this.ButtonGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.ButtonGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ButtonGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ButtonGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ButtonGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ButtonNumber,
            this.Assignment});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ButtonGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.ButtonGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ButtonGrid.EnableHeadersVisualStyles = false;
            this.ButtonGrid.Location = new System.Drawing.Point(3, 16);
            this.ButtonGrid.MultiSelect = false;
            this.ButtonGrid.Name = "ButtonGrid";
            this.ButtonGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            this.ButtonGrid.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.ButtonGrid.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            this.ButtonGrid.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.ButtonGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ButtonGrid.ShowCellErrors = false;
            this.ButtonGrid.ShowEditingIcon = false;
            this.ButtonGrid.ShowRowErrors = false;
            this.ButtonGrid.Size = new System.Drawing.Size(383, 289);
            this.ButtonGrid.TabIndex = 0;
            this.ButtonGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ButtonGrid_CellClick);
            // 
            // ButtonNumber
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            this.ButtonNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.ButtonNumber.Frozen = true;
            this.ButtonNumber.HeaderText = "ButtonNumber";
            this.ButtonNumber.Name = "ButtonNumber";
            // 
            // Assignment
            // 
            this.Assignment.AutoComplete = false;
            this.Assignment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            this.Assignment.DefaultCellStyle = dataGridViewCellStyle4;
            this.Assignment.HeaderText = "Button Command Assignment";
            this.Assignment.Name = "Assignment";
            this.Assignment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Assignment.Sorted = true;
            this.Assignment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.txtSensitivity);
            this.GroupBox1.Controls.Add(this.label4);
            this.GroupBox1.Controls.Add(this.rbFixed);
            this.GroupBox1.Controls.Add(this.rbAnalog);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox1.ForeColor = System.Drawing.Color.White;
            this.GroupBox1.Location = new System.Drawing.Point(3, 93);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(389, 105);
            this.GroupBox1.TabIndex = 9;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Joystick Mode";
            // 
            // txtSensitivity
            // 
            this.txtSensitivity.DecimalPlaces = 2;
            this.txtSensitivity.Location = new System.Drawing.Point(237, 45);
            this.txtSensitivity.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.txtSensitivity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtSensitivity.Name = "txtSensitivity";
            this.txtSensitivity.Size = new System.Drawing.Size(76, 20);
            this.txtSensitivity.TabIndex = 3;
            this.txtSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSensitivity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Sensitivity Adjustment (0.1% to 200%):";
            // 
            // rbFixed
            // 
            this.rbFixed.AutoSize = true;
            this.rbFixed.Location = new System.Drawing.Point(23, 75);
            this.rbFixed.Name = "rbFixed";
            this.rbFixed.Size = new System.Drawing.Size(84, 17);
            this.rbFixed.TabIndex = 1;
            this.rbFixed.TabStop = true;
            this.rbFixed.Text = "Fixed Speed";
            this.rbFixed.UseVisualStyleBackColor = true;
            // 
            // rbAnalog
            // 
            this.rbAnalog.AutoSize = true;
            this.rbAnalog.Location = new System.Drawing.Point(23, 24);
            this.rbAnalog.Name = "rbAnalog";
            this.rbAnalog.Size = new System.Drawing.Size(258, 17);
            this.rbAnalog.TabIndex = 0;
            this.rbAnalog.TabStop = true;
            this.rbAnalog.Text = "Analog with Variable Speed (Guide, Center, Slew)";
            this.rbAnalog.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(412, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 22);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(412, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(66, 22);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(412, 220);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(66, 22);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 518);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "HINT: Press a button on the joystick to see it highlited in the list above.";
            // 
            // ckFlipRA
            // 
            this.ckFlipRA.AutoSize = true;
            this.ckFlipRA.Location = new System.Drawing.Point(336, 24);
            this.ckFlipRA.Name = "ckFlipRA";
            this.ckFlipRA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckFlipRA.Size = new System.Drawing.Size(42, 17);
            this.ckFlipRA.TabIndex = 4;
            this.ckFlipRA.Text = "Flip";
            this.ckFlipRA.UseVisualStyleBackColor = true;
            // 
            // ckFlipDec
            // 
            this.ckFlipDec.AutoSize = true;
            this.ckFlipDec.Location = new System.Drawing.Point(336, 50);
            this.ckFlipDec.Name = "ckFlipDec";
            this.ckFlipDec.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckFlipDec.Size = new System.Drawing.Size(42, 17);
            this.ckFlipDec.TabIndex = 5;
            this.ckFlipDec.Text = "Flip";
            this.ckFlipDec.UseVisualStyleBackColor = true;
            // 
            // frmJoystickConfig
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(490, 540);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmJoystickConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gemini Joystick Configuration";
            this.Load += new System.EventHandler(this.frmJoystickConfig_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonGrid)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSensitivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton rbFixed;
        private System.Windows.Forms.RadioButton rbAnalog;
        internal System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView ButtonGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ButtonNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn Assignment;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbAxisRA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAxisDEC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtSensitivity;
        private System.Windows.Forms.CheckBox ckFlipDec;
        private System.Windows.Forms.CheckBox ckFlipRA;


    }
}