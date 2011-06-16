namespace Optec_TCF_S_Focuser
{
    partial class AddOffsetForm
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
            this.AddBtn = new System.Windows.Forms.Button();
            this.CancelBTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTB = new System.Windows.Forms.TextBox();
            this.Offset_NUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.Prompt_TB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Offset_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AddBtn.Enabled = false;
            this.AddBtn.Location = new System.Drawing.Point(200, 158);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 2;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // CancelBTN
            // 
            this.CancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBTN.Location = new System.Drawing.Point(91, 158);
            this.CancelBTN.Name = "CancelBTN";
            this.CancelBTN.Size = new System.Drawing.Size(75, 23);
            this.CancelBTN.TabIndex = 3;
            this.CancelBTN.Text = "Cancel";
            this.CancelBTN.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Offset:";
            // 
            // nameTB
            // 
            this.nameTB.Location = new System.Drawing.Point(58, 69);
            this.nameTB.Name = "nameTB";
            this.nameTB.Size = new System.Drawing.Size(138, 20);
            this.nameTB.TabIndex = 0;
            this.nameTB.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // Offset_NUD
            // 
            this.Offset_NUD.Location = new System.Drawing.Point(58, 112);
            this.Offset_NUD.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.Offset_NUD.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.Offset_NUD.Name = "Offset_NUD";
            this.Offset_NUD.Size = new System.Drawing.Size(138, 20);
            this.Offset_NUD.TabIndex = 1;
            this.Offset_NUD.Enter += new System.EventHandler(this.Offset_NUD_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(200, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Steps";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Prompt_TB
            // 
            this.Prompt_TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Prompt_TB.Location = new System.Drawing.Point(12, 12);
            this.Prompt_TB.Multiline = true;
            this.Prompt_TB.Name = "Prompt_TB";
            this.Prompt_TB.ReadOnly = true;
            this.Prompt_TB.Size = new System.Drawing.Size(263, 43);
            this.Prompt_TB.TabIndex = 7;
            this.Prompt_TB.TabStop = false;
            this.Prompt_TB.Text = "Enter name and value...";
            // 
            // AddOffsetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 189);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Offset_NUD);
            this.Controls.Add(this.nameTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CancelBTN);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.Prompt_TB);
            this.Icon = global::Optec_TCF_S_Focuser.Properties.Resources.TCF_S_2010;
            this.Name = "AddOffsetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddOffsetForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddFilterOffsetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Offset_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button CancelBTN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTB;
        private System.Windows.Forms.NumericUpDown Offset_NUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox Prompt_TB;
    }
}