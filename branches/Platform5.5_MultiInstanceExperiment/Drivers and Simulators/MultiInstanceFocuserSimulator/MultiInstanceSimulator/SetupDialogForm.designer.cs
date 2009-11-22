namespace ASCOM.MultiInstanceSimulator
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMaxIncrement = new System.Windows.Forms.TextBox();
            this.textBoxMaxStep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonAbsolute = new System.Windows.Forms.RadioButton();
            this.radioButtonRelative = new System.Windows.Forms.RadioButton();
            this.textBoxStepSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxTempCompAvailable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(113, 183);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(113, 213);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Construct your driver\'s setup dialog here.";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.ErrorImage = null;
            this.picASCOM.Image = global::ASCOM.MultiInstanceSimulator.Properties.Resources.ASCOM;
            this.picASCOM.InitialImage = null;
            this.picASCOM.Location = new System.Drawing.Point(124, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max Increment";
            // 
            // textBoxMaxIncrement
            // 
            this.textBoxMaxIncrement.Location = new System.Drawing.Point(85, 84);
            this.textBoxMaxIncrement.Name = "textBoxMaxIncrement";
            this.textBoxMaxIncrement.Size = new System.Drawing.Size(90, 20);
            this.textBoxMaxIncrement.TabIndex = 5;
            // 
            // textBoxMaxStep
            // 
            this.textBoxMaxStep.Location = new System.Drawing.Point(85, 110);
            this.textBoxMaxStep.Name = "textBoxMaxStep";
            this.textBoxMaxStep.Size = new System.Drawing.Size(90, 20);
            this.textBoxMaxStep.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Max Step";
            // 
            // radioButtonAbsolute
            // 
            this.radioButtonAbsolute.AutoSize = true;
            this.radioButtonAbsolute.Location = new System.Drawing.Point(12, 43);
            this.radioButtonAbsolute.Name = "radioButtonAbsolute";
            this.radioButtonAbsolute.Size = new System.Drawing.Size(66, 17);
            this.radioButtonAbsolute.TabIndex = 8;
            this.radioButtonAbsolute.TabStop = true;
            this.radioButtonAbsolute.Text = "Absolute";
            this.radioButtonAbsolute.UseVisualStyleBackColor = true;
            // 
            // radioButtonRelative
            // 
            this.radioButtonRelative.AutoSize = true;
            this.radioButtonRelative.Location = new System.Drawing.Point(12, 66);
            this.radioButtonRelative.Name = "radioButtonRelative";
            this.radioButtonRelative.Size = new System.Drawing.Size(64, 17);
            this.radioButtonRelative.TabIndex = 9;
            this.radioButtonRelative.TabStop = true;
            this.radioButtonRelative.Text = "Relative";
            this.radioButtonRelative.UseVisualStyleBackColor = true;
            // 
            // textBoxStepSize
            // 
            this.textBoxStepSize.Location = new System.Drawing.Point(85, 136);
            this.textBoxStepSize.Name = "textBoxStepSize";
            this.textBoxStepSize.Size = new System.Drawing.Size(90, 20);
            this.textBoxStepSize.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Step Size";
            // 
            // checkBoxTempCompAvailable
            // 
            this.checkBoxTempCompAvailable.AutoSize = true;
            this.checkBoxTempCompAvailable.Location = new System.Drawing.Point(10, 162);
            this.checkBoxTempCompAvailable.Name = "checkBoxTempCompAvailable";
            this.checkBoxTempCompAvailable.Size = new System.Drawing.Size(129, 17);
            this.checkBoxTempCompAvailable.TabIndex = 12;
            this.checkBoxTempCompAvailable.Text = "Temp Comp Available";
            this.checkBoxTempCompAvailable.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 250);
            this.Controls.Add(this.checkBoxTempCompAvailable);
            this.Controls.Add(this.textBoxStepSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButtonRelative);
            this.Controls.Add(this.radioButtonAbsolute);
            this.Controls.Add(this.textBoxMaxStep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMaxIncrement);
            this.Controls.Add(this.label2);
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
            this.Text = "MultiInstanceSimulator Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMaxIncrement;
        private System.Windows.Forms.TextBox textBoxMaxStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonAbsolute;
        private System.Windows.Forms.RadioButton radioButtonRelative;
        private System.Windows.Forms.TextBox textBoxStepSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxTempCompAvailable;
    }
}