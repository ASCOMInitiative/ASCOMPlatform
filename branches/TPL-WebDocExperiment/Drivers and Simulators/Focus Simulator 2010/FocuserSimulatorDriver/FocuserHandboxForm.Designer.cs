namespace ASCOM.Simulator
{
    partial class FocuserHandboxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocuserHandboxForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMoveIn = new System.Windows.Forms.Button();
            this.btnMoveOut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPositionDisplay = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTempDisplay = new System.Windows.Forms.Label();
            this.btnShowSetupDialog = new System.Windows.Forms.Button();
            this.chkTempCompEnabled = new System.Windows.Forms.CheckBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.txtGoTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GoToErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoToErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.Simulator.Properties.Resources.ASCOM;
            this.pictureBox1.Location = new System.Drawing.Point(267, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 59);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnMoveIn
            // 
            this.btnMoveIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMoveIn.Location = new System.Drawing.Point(172, 43);
            this.btnMoveIn.Name = "btnMoveIn";
            this.btnMoveIn.Size = new System.Drawing.Size(37, 23);
            this.btnMoveIn.TabIndex = 1;
            this.btnMoveIn.Text = "In";
            this.btnMoveIn.UseVisualStyleBackColor = true;
            // 
            // btnMoveOut
            // 
            this.btnMoveOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMoveOut.Location = new System.Drawing.Point(215, 43);
            this.btnMoveOut.Name = "btnMoveOut";
            this.btnMoveOut.Size = new System.Drawing.Size(39, 23);
            this.btnMoveOut.TabIndex = 2;
            this.btnMoveOut.Text = "Out";
            this.btnMoveOut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(22, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Position:";
            // 
            // lblPositionDisplay
            // 
            this.lblPositionDisplay.AutoSize = true;
            this.lblPositionDisplay.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPositionDisplay.ForeColor = System.Drawing.Color.Red;
            this.lblPositionDisplay.Location = new System.Drawing.Point(99, 48);
            this.lblPositionDisplay.Name = "lblPositionDisplay";
            this.lblPositionDisplay.Size = new System.Drawing.Size(48, 18);
            this.lblPositionDisplay.TabIndex = 4;
            this.lblPositionDisplay.Text = "00000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(22, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tempature C:";
            // 
            // lblTempDisplay
            // 
            this.lblTempDisplay.AutoSize = true;
            this.lblTempDisplay.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempDisplay.ForeColor = System.Drawing.Color.Red;
            this.lblTempDisplay.Location = new System.Drawing.Point(99, 21);
            this.lblTempDisplay.Name = "lblTempDisplay";
            this.lblTempDisplay.Size = new System.Drawing.Size(48, 18);
            this.lblTempDisplay.TabIndex = 6;
            this.lblTempDisplay.Text = "00000";
            // 
            // btnShowSetupDialog
            // 
            this.btnShowSetupDialog.BackColor = System.Drawing.Color.Black;
            this.btnShowSetupDialog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnShowSetupDialog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnShowSetupDialog.Location = new System.Drawing.Point(241, 137);
            this.btnShowSetupDialog.Name = "btnShowSetupDialog";
            this.btnShowSetupDialog.Size = new System.Drawing.Size(82, 23);
            this.btnShowSetupDialog.TabIndex = 7;
            this.btnShowSetupDialog.Text = "Configuration";
            this.btnShowSetupDialog.UseVisualStyleBackColor = true;
            this.btnShowSetupDialog.Click += new System.EventHandler(this.btnShowSetupDialog_Click);
            // 
            // chkTempCompEnabled
            // 
            this.chkTempCompEnabled.AutoSize = true;
            this.chkTempCompEnabled.ForeColor = System.Drawing.Color.White;
            this.chkTempCompEnabled.Location = new System.Drawing.Point(25, 117);
            this.chkTempCompEnabled.Name = "chkTempCompEnabled";
            this.chkTempCompEnabled.Size = new System.Drawing.Size(147, 17);
            this.chkTempCompEnabled.TabIndex = 9;
            this.chkTempCompEnabled.Text = "Tempature Compensation";
            this.chkTempCompEnabled.UseVisualStyleBackColor = true;
            this.chkTempCompEnabled.CheckedChanged += new System.EventHandler(this.chkTempCompEnabled_CheckStateChanged);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(22, 149);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(35, 13);
            this.lblVersion.TabIndex = 11;
            this.lblVersion.Text = "label5";
            // 
            // btnGoTo
            // 
            this.btnGoTo.Location = new System.Drawing.Point(172, 79);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(48, 23);
            this.btnGoTo.TabIndex = 12;
            this.btnGoTo.Text = "GoTo";
            this.btnGoTo.UseVisualStyleBackColor = true;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
            // 
            // txtGoTo
            // 
            this.txtGoTo.Location = new System.Drawing.Point(76, 81);
            this.txtGoTo.Name = "txtGoTo";
            this.txtGoTo.Size = new System.Drawing.Size(71, 20);
            this.txtGoTo.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(22, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Target:";
            // 
            // GoToErrorProvider
            // 
            this.GoToErrorProvider.ContainerControl = this;
            // 
            // FocuserHandboxForm
            // 
            this.AcceptButton = this.btnGoTo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(335, 172);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGoTo);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.chkTempCompEnabled);
            this.Controls.Add(this.btnShowSetupDialog);
            this.Controls.Add(this.lblTempDisplay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPositionDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMoveOut);
            this.Controls.Add(this.btnMoveIn);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FocuserHandboxForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Focuser Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoToErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMoveIn;
        private System.Windows.Forms.Button btnMoveOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPositionDisplay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTempDisplay;
        private System.Windows.Forms.CheckBox chkTempCompEnabled;
        private System.Windows.Forms.Button btnShowSetupDialog;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.TextBox txtGoTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider GoToErrorProvider;

    }
}

