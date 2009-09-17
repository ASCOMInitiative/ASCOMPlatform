namespace ASCOM.FocuserSimulator
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioRelative = new System.Windows.Forms.RadioButton();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.IsTempCompAvailable = new System.Windows.Forms.CheckBox();
            this.IsTempComp = new System.Windows.Forms.CheckBox();
            this.SetPosButton = new System.Windows.Forms.Button();
            this.LabelPosition = new System.Windows.Forms.Label();
            this.TextPosition = new System.Windows.Forms.TextBox();
            this.RadioAbsolute = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(277, 92);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(277, 122);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SetPosButton);
            this.groupBox1.Controls.Add(this.LabelPosition);
            this.groupBox1.Controls.Add(this.TextPosition);
            this.groupBox1.Controls.Add(this.RadioRelative);
            this.groupBox1.Controls.Add(this.RadioAbsolute);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // RadioRelative
            // 
            this.RadioRelative.AutoSize = true;
            this.RadioRelative.Checked = true;
            this.RadioRelative.Location = new System.Drawing.Point(6, 42);
            this.RadioRelative.Name = "RadioRelative";
            this.RadioRelative.Size = new System.Drawing.Size(64, 17);
            this.RadioRelative.TabIndex = 1;
            this.RadioRelative.TabStop = true;
            this.RadioRelative.Text = "Relative";
            this.RadioRelative.UseVisualStyleBackColor = true;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.FocuserSimulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(288, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.IsTempCompAvailable);
            this.groupBox2.Controls.Add(this.IsTempComp);
            this.groupBox2.Location = new System.Drawing.Point(12, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 100);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temperature";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown3);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numericUpDown2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(237, 100);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "StepSize";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max Increment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Max Step";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxStep", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown3.Location = new System.Drawing.Point(89, 67);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown3.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxStep;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sMaxIncrement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown2.Location = new System.Drawing.Point(89, 41);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sMaxIncrement;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sStepSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.Location = new System.Drawing.Point(89, 15);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(47, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sStepSize;
            // 
            // IsTempCompAvailable
            // 
            this.IsTempCompAvailable.AutoSize = true;
            this.IsTempCompAvailable.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempCompAvailable;
            this.IsTempCompAvailable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempCompAvailable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTempCompAvailable.Location = new System.Drawing.Point(6, 54);
            this.IsTempCompAvailable.Name = "IsTempCompAvailable";
            this.IsTempCompAvailable.Size = new System.Drawing.Size(132, 17);
            this.IsTempCompAvailable.TabIndex = 1;
            this.IsTempCompAvailable.Text = "Compensation enabled";
            this.IsTempCompAvailable.UseVisualStyleBackColor = true;
            // 
            // IsTempComp
            // 
            this.IsTempComp.AutoSize = true;
            this.IsTempComp.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sTempComp;
            this.IsTempComp.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sTempComp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IsTempComp.Location = new System.Drawing.Point(6, 77);
            this.IsTempComp.Name = "IsTempComp";
            this.IsTempComp.Size = new System.Drawing.Size(117, 17);
            this.IsTempComp.TabIndex = 0;
            this.IsTempComp.Text = "Compensation state";
            this.IsTempComp.UseVisualStyleBackColor = true;
            // 
            // SetPosButton
            // 
            this.SetPosButton.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SetPosButton.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.SetPosButton.Location = new System.Drawing.Point(178, 44);
            this.SetPosButton.Name = "SetPosButton";
            this.SetPosButton.Size = new System.Drawing.Size(50, 23);
            this.SetPosButton.TabIndex = 4;
            this.SetPosButton.Text = "Set";
            this.SetPosButton.UseVisualStyleBackColor = true;
            // 
            // LabelPosition
            // 
            this.LabelPosition.AutoSize = true;
            this.LabelPosition.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LabelPosition.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.LabelPosition.Location = new System.Drawing.Point(128, 21);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Size = new System.Drawing.Size(44, 13);
            this.LabelPosition.TabIndex = 3;
            this.LabelPosition.Text = "Position";
            // 
            // TextPosition
            // 
            this.TextPosition.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TextPosition.Enabled = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.TextPosition.Location = new System.Drawing.Point(178, 18);
            this.TextPosition.Name = "TextPosition";
            this.TextPosition.Size = new System.Drawing.Size(50, 20);
            this.TextPosition.TabIndex = 2;
            this.TextPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RadioAbsolute
            // 
            this.RadioAbsolute.AutoSize = true;
            this.RadioAbsolute.Checked = global::ASCOM.FocuserSimulator.Properties.Settings.Default.sAbsolute;
            this.RadioAbsolute.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FocuserSimulator.Properties.Settings.Default, "sAbsolute", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RadioAbsolute.Location = new System.Drawing.Point(6, 19);
            this.RadioAbsolute.Name = "RadioAbsolute";
            this.RadioAbsolute.Size = new System.Drawing.Size(66, 17);
            this.RadioAbsolute.TabIndex = 0;
            this.RadioAbsolute.Text = "Absolute";
            this.RadioAbsolute.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(79, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(44, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "17.5°";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Temperature";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 315);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FocuserSimulator Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioRelative;
        private System.Windows.Forms.RadioButton RadioAbsolute;
        private System.Windows.Forms.TextBox TextPosition;
        private System.Windows.Forms.Label LabelPosition;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox IsTempComp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button SetPosButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox IsTempCompAvailable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
    }
}