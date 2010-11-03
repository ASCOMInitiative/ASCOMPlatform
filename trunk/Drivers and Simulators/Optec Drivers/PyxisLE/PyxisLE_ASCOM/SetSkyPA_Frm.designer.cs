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
            this.Degree_TB = new System.Windows.Forms.TextBox();
            this.Min_TB = new System.Windows.Forms.TextBox();
            this.Sec_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Sky Position Angle:";
            // 
            // CurrentSkyPA_LBL
            // 
            this.CurrentSkyPA_LBL.AutoSize = true;
            this.CurrentSkyPA_LBL.Location = new System.Drawing.Point(156, 26);
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
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(18, 136);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(414, 59);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // OK_BTN
            // 
            this.OK_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_BTN.Location = new System.Drawing.Point(357, 197);
            this.OK_BTN.Name = "OK_BTN";
            this.OK_BTN.Size = new System.Drawing.Size(75, 23);
            this.OK_BTN.TabIndex = 3;
            this.OK_BTN.Text = "OK";
            this.OK_BTN.UseVisualStyleBackColor = true;
            this.OK_BTN.Click += new System.EventHandler(this.OK_BTN_Click);
            // 
            // Cancel_BTN
            // 
            this.Cancel_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_BTN.Location = new System.Drawing.Point(229, 197);
            this.Cancel_BTN.Name = "Cancel_BTN";
            this.Cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BTN.TabIndex = 4;
            this.Cancel_BTN.Text = "Cancel";
            this.Cancel_BTN.UseVisualStyleBackColor = true;
            this.Cancel_BTN.Click += new System.EventHandler(this.Cancel_BTN_Click);
            // 
            // NewValue_TB
            // 
            this.NewValue_TB.CausesValidation = false;
            this.NewValue_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewValue_TB.Location = new System.Drawing.Point(292, 99);
            this.NewValue_TB.Name = "NewValue_TB";
            this.NewValue_TB.ReadOnly = true;
            this.NewValue_TB.Size = new System.Drawing.Size(75, 31);
            this.NewValue_TB.TabIndex = 0;
            this.NewValue_TB.TabStop = false;
            this.NewValue_TB.Text = "000.00";
            this.NewValue_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewValue_TB_KeyPress);
            this.NewValue_TB.Validating += new System.ComponentModel.CancelEventHandler(this.NewValue_TB_Validating);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Degree_TB
            // 
            this.Degree_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.Degree_TB.Location = new System.Drawing.Point(262, 57);
            this.Degree_TB.Name = "Degree_TB";
            this.Degree_TB.Size = new System.Drawing.Size(57, 31);
            this.Degree_TB.TabIndex = 0;
            this.Degree_TB.Text = "000";
            this.Degree_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Degree_TB.TextChanged += new System.EventHandler(this.Degree_TB_TextChanged);
            this.Degree_TB.Enter += new System.EventHandler(this.Degree_TB_Enter);
            this.Degree_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Degree_TB_KeyPress);
            // 
            // Min_TB
            // 
            this.Min_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.Min_TB.Location = new System.Drawing.Point(335, 57);
            this.Min_TB.Name = "Min_TB";
            this.Min_TB.Size = new System.Drawing.Size(31, 31);
            this.Min_TB.TabIndex = 1;
            this.Min_TB.Text = "00";
            this.Min_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Min_TB.TextChanged += new System.EventHandler(this.Degree_TB_TextChanged);
            this.Min_TB.Enter += new System.EventHandler(this.Degree_TB_Enter);
            this.Min_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Degree_TB_KeyPress);
            // 
            // Sec_TB
            // 
            this.Sec_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.Sec_TB.Location = new System.Drawing.Point(383, 57);
            this.Sec_TB.Name = "Sec_TB";
            this.Sec_TB.Size = new System.Drawing.Size(31, 31);
            this.Sec_TB.TabIndex = 2;
            this.Sec_TB.Text = "00";
            this.Sec_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sec_TB.TextChanged += new System.EventHandler(this.Degree_TB_TextChanged);
            this.Sec_TB.Enter += new System.EventHandler(this.Degree_TB_Enter);
            this.Sec_TB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Degree_TB_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.label4.Location = new System.Drawing.Point(315, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "°";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.label5.Location = new System.Drawing.Point(365, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "\'";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.label6.Location = new System.Drawing.Point(414, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "\"";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.label3.Location = new System.Drawing.Point(367, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "°";
            // 
            // SetSkyPA_Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 232);
            this.Controls.Add(this.Sec_TB);
            this.Controls.Add(this.Min_TB);
            this.Controls.Add(this.Degree_TB);
            this.Controls.Add(this.NewValue_TB);
            this.Controls.Add(this.Cancel_BTN);
            this.Controls.Add(this.OK_BTN);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentSkyPA_LBL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.TextBox Sec_TB;
        private System.Windows.Forms.TextBox Min_TB;
        private System.Windows.Forms.TextBox Degree_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
    }
}