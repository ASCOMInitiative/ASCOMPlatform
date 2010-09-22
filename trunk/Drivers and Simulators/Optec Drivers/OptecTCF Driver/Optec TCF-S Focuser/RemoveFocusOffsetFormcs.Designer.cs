namespace Optec_TCF_S_Focuser
{
    partial class RemoveFocusOffsetFormcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveFocusOffsetFormcs));
            this.CancelBTN = new System.Windows.Forms.Button();
            this.Items_CB = new System.Windows.Forms.CheckedListBox();
            this.Remove_BTN = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CancelBTN
            // 
            this.CancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBTN.Location = new System.Drawing.Point(204, 238);
            this.CancelBTN.Name = "CancelBTN";
            this.CancelBTN.Size = new System.Drawing.Size(75, 23);
            this.CancelBTN.TabIndex = 4;
            this.CancelBTN.Text = "Close";
            this.CancelBTN.UseVisualStyleBackColor = true;
            // 
            // Items_CB
            // 
            this.Items_CB.CheckOnClick = true;
            this.Items_CB.Location = new System.Drawing.Point(14, 45);
            this.Items_CB.Name = "Items_CB";
            this.Items_CB.Size = new System.Drawing.Size(265, 139);
            this.Items_CB.TabIndex = 6;
            // 
            // Remove_BTN
            // 
            this.Remove_BTN.Location = new System.Drawing.Point(14, 190);
            this.Remove_BTN.Name = "Remove_BTN";
            this.Remove_BTN.Size = new System.Drawing.Size(75, 23);
            this.Remove_BTN.TabIndex = 7;
            this.Remove_BTN.Text = "Remove";
            this.Remove_BTN.UseVisualStyleBackColor = true;
            this.Remove_BTN.Click += new System.EventHandler(this.Remove_BTN_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(14, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(265, 30);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "Click an item to select it. Click \'Remove\' to delete all checked focus offsets.\r\n" +
                "";
            // 
            // RemoveFocusOffsetFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 273);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Remove_BTN);
            this.Controls.Add(this.Items_CB);
            this.Controls.Add(this.CancelBTN);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoveFocusOffsetFormcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remove Filter Focus Offsets";
            this.Load += new System.EventHandler(this.RemoveFocusOffsetFormcs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBTN;
        private System.Windows.Forms.CheckedListBox Items_CB;
        private System.Windows.Forms.Button Remove_BTN;
        private System.Windows.Forms.TextBox textBox1;
    }
}