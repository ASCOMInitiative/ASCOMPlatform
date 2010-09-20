namespace ASCOM.PyxisLE_ASCOM
{
    partial class SetSkyPA_Frm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetSkyPA_Frm));
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentSkyPA_LBL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OK_BTN = new System.Windows.Forms.Button();
            this.Cancel_BTN = new System.Windows.Forms.Button();
            this.NewValue_TB = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Sky Position Angle:";
            // 
            // CurrentSkyPA_LBL
            // 
            this.CurrentSkyPA_LBL.AutoSize = true;
            this.CurrentSkyPA_LBL.Location = new System.Drawing.Point(156, 16);
            this.CurrentSkyPA_LBL.Name = "CurrentSkyPA_LBL";
            this.CurrentSkyPA_LBL.Size = new System.Drawing.Size(78, 13);
            this.CurrentSkyPA_LBL.TabIndex = 2;
            this.CurrentSkyPA_LBL.Text = "Current Angle °";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "New Sky Position Angle:";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(15, 115);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(349, 41);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Note: Changing the Sky Position Angle will not move the rotator.  This is used to" +
                " \"tell\" the focuser that it is currently at the specifiec position angle. ";
            // 
            // OK_BTN
            // 
            this.OK_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_BTN.Location = new System.Drawing.Point(307, 167);
            this.OK_BTN.Name = "OK_BTN";
            this.OK_BTN.Size = new System.Drawing.Size(75, 23);
            this.OK_BTN.TabIndex = 1;
            this.OK_BTN.Text = "OK";
            this.OK_BTN.UseVisualStyleBackColor = true;
            this.OK_BTN.Click += new System.EventHandler(this.OK_BTN_Click);
            // 
            // Cancel_BTN
            // 
            this.Cancel_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_BTN.Location = new System.Drawing.Point(179, 167);
            this.Cancel_BTN.Name = "Cancel_BTN";
            this.Cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BTN.TabIndex = 2;
            this.Cancel_BTN.Text = "Cancel";
            this.Cancel_BTN.UseVisualStyleBackColor = true;
            this.Cancel_BTN.Click += new System.EventHandler(this.Cancel_BTN_Click);
            // 
            // NewValue_TB
            // 
            this.NewValue_TB.CausesValidation = false;
            this.NewValue_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewValue_TB.Location = new System.Drawing.Point(265, 57);
            this.NewValue_TB.Name = "NewValue_TB";
            this.NewValue_TB.Size = new System.Drawing.Size(96, 31);
            this.NewValue_TB.TabIndex = 0;
            this.NewValue_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewValue_TB_KeyPress);
            this.NewValue_TB.Validating += new System.ComponentModel.CancelEventHandler(this.NewValue_TB_Validating);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SetSkyPA_Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 202);
            this.Controls.Add(this.NewValue_TB);
            this.Controls.Add(this.Cancel_BTN);
            this.Controls.Add(this.OK_BTN);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentSkyPA_LBL);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetSkyPA_Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Sky Position Angle";
            this.Load += new System.EventHandler(this.SetSkyPA_Frm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurrentSkyPA_LBL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button OK_BTN;
        private System.Windows.Forms.Button Cancel_BTN;
        private System.Windows.Forms.TextBox NewValue_TB;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}