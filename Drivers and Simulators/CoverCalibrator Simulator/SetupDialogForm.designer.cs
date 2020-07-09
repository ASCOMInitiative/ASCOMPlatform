namespace ASCOM.Simulator
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
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.NumCoverOpeningTime = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NumCalibratorStablisationTime = new System.Windows.Forms.NumericUpDown();
            this.NumMaxBrightness = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.CmbCalibratorInitialisationState = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CmbCoverInitialisationState = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LblSynchBehaviourTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoverOpeningTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCalibratorStablisationTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxBrightness)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(454, 277);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.CmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(454, 307);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "ASCOM CoverCalibrator Simulator";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(458, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // chkTrace
            // 
            this.chkTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(451, 176);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // NumCoverOpeningTime
            // 
            this.NumCoverOpeningTime.DecimalPlaces = 1;
            this.NumCoverOpeningTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumCoverOpeningTime.Location = new System.Drawing.Point(13, 15);
            this.NumCoverOpeningTime.Name = "NumCoverOpeningTime";
            this.NumCoverOpeningTime.Size = new System.Drawing.Size(120, 20);
            this.NumCoverOpeningTime.TabIndex = 7;
            this.NumCoverOpeningTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cover opening time (0.0 to 100.0 seconds)*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(139, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Calibrator stabilisation time (0.0 to 100.0 seconds)*";
            // 
            // NumCalibratorStablisationTime
            // 
            this.NumCalibratorStablisationTime.DecimalPlaces = 1;
            this.NumCalibratorStablisationTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumCalibratorStablisationTime.Location = new System.Drawing.Point(13, 40);
            this.NumCalibratorStablisationTime.Name = "NumCalibratorStablisationTime";
            this.NumCalibratorStablisationTime.Size = new System.Drawing.Size(120, 20);
            this.NumCalibratorStablisationTime.TabIndex = 9;
            this.NumCalibratorStablisationTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumMaxBrightness
            // 
            this.NumMaxBrightness.Location = new System.Drawing.Point(13, 14);
            this.NumMaxBrightness.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.NumMaxBrightness.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMaxBrightness.Name = "NumMaxBrightness";
            this.NumMaxBrightness.Size = new System.Drawing.Size(120, 20);
            this.NumMaxBrightness.TabIndex = 11;
            this.NumMaxBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumMaxBrightness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Calibrator maximum brightness (1 to 2147483647)";
            // 
            // CmbCalibratorInitialisationState
            // 
            this.CmbCalibratorInitialisationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCalibratorInitialisationState.FormattingEnabled = true;
            this.CmbCalibratorInitialisationState.Items.AddRange(new object[] {
            "NotPresent",
            "Off",
            "NotReady",
            "Ready",
            "Unknown",
            "Error"});
            this.CmbCalibratorInitialisationState.Location = new System.Drawing.Point(13, 66);
            this.CmbCalibratorInitialisationState.Name = "CmbCalibratorInitialisationState";
            this.CmbCalibratorInitialisationState.Size = new System.Drawing.Size(121, 21);
            this.CmbCalibratorInitialisationState.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Calibrator initialisation state";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Cover initialisation state";
            // 
            // CmbCoverInitialisationState
            // 
            this.CmbCoverInitialisationState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCoverInitialisationState.FormattingEnabled = true;
            this.CmbCoverInitialisationState.Items.AddRange(new object[] {
            "NotPresent",
            "Closed",
            "Moving",
            "Open",
            "Unknown",
            "Error"});
            this.CmbCoverInitialisationState.Location = new System.Drawing.Point(13, 41);
            this.CmbCoverInitialisationState.Name = "CmbCoverInitialisationState";
            this.CmbCoverInitialisationState.Size = new System.Drawing.Size(121, 21);
            this.CmbCoverInitialisationState.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.NumMaxBrightness);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.NumCalibratorStablisationTime);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CmbCalibratorInitialisationState);
            this.panel1.Location = new System.Drawing.Point(37, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 101);
            this.panel1.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.NumCoverOpeningTime);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.CmbCoverInitialisationState);
            this.panel2.Location = new System.Drawing.Point(37, 194);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(396, 78);
            this.panel2.TabIndex = 18;
            // 
            // LblSynchBehaviourTime
            // 
            this.LblSynchBehaviourTime.AutoSize = true;
            this.LblSynchBehaviourTime.Location = new System.Drawing.Point(15, 298);
            this.LblSynchBehaviourTime.Name = "LblSynchBehaviourTime";
            this.LblSynchBehaviourTime.Size = new System.Drawing.Size(418, 13);
            this.LblSynchBehaviourTime.TabIndex = 19;
            this.LblSynchBehaviourTime.Text = "* Methods will be synchronous from 0.0 and 0.5 seconds and asynchronous above thi" +
    "s.";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 340);
            this.Controls.Add(this.LblSynchBehaviourTime);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASCOM CoverCalibrator Simulator Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCoverOpeningTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumCalibratorStablisationTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMaxBrightness)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.NumericUpDown NumCoverOpeningTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumCalibratorStablisationTime;
        private System.Windows.Forms.NumericUpDown NumMaxBrightness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CmbCalibratorInitialisationState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CmbCoverInitialisationState;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label LblSynchBehaviourTime;
    }
}