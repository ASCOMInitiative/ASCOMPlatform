namespace ASCOM.Optec
{
    partial class COMPortForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.COMPort_CB = new System.Windows.Forms.ComboBox();
            this.OK_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select a COM port from the list of available ports:";
            // 
            // COMPort_CB
            // 
            this.COMPort_CB.FormattingEnabled = true;
            this.COMPort_CB.Location = new System.Drawing.Point(304, 32);
            this.COMPort_CB.Name = "COMPort_CB";
            this.COMPort_CB.Size = new System.Drawing.Size(60, 21);
            this.COMPort_CB.Sorted = true;
            this.COMPort_CB.TabIndex = 1;
            this.COMPort_CB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.COMPort_CB_KeyPress);
            // 
            // OK_Btn
            // 
            this.OK_Btn.Location = new System.Drawing.Point(304, 88);
            this.OK_Btn.Name = "OK_Btn";
            this.OK_Btn.Size = new System.Drawing.Size(75, 23);
            this.OK_Btn.TabIndex = 2;
            this.OK_Btn.TabStop = false;
            this.OK_Btn.Text = "OK";
            this.OK_Btn.UseVisualStyleBackColor = true;
            this.OK_Btn.Click += new System.EventHandler(this.OK_Btn_Click);
            // 
            // COMPortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 123);
            this.Controls.Add(this.OK_Btn);
            this.Controls.Add(this.COMPort_CB);
            this.Controls.Add(this.label1);
            this.Name = "COMPortForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COM Port Setup";
            this.Load += new System.EventHandler(this.COMPortForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox COMPort_CB;
        private System.Windows.Forms.Button OK_Btn;
    }
}