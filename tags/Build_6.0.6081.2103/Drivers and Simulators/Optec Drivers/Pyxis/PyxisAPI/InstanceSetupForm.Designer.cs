namespace PyxisAPI
{
    partial class InstanceSetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstanceSetupForm));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editBtn = new System.Windows.Forms.Button();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.newNameTb = new System.Windows.Forms.TextBox();
            this.addNowBtn = new System.Windows.Forms.Button();
            this.editNameTb = new System.Windows.Forms.TextBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.SetupDialogBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectInstanceBtn = new System.Windows.Forms.Button();
            this.CurrentInstanceLBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 85);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(213, 134);
            this.listBox1.TabIndex = 0;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(12, 225);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 1;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 53);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Available Instances:";
            // 
            // editBtn
            // 
            this.editBtn.Location = new System.Drawing.Point(12, 254);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(75, 23);
            this.editBtn.TabIndex = 4;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Location = new System.Drawing.Point(12, 283);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(75, 23);
            this.RemoveBtn.TabIndex = 5;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.UseVisualStyleBackColor = true;
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(268, 302);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 6;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.oKBtn_Click);
            // 
            // newNameTb
            // 
            this.newNameTb.Location = new System.Drawing.Point(93, 227);
            this.newNameTb.Name = "newNameTb";
            this.newNameTb.Size = new System.Drawing.Size(150, 20);
            this.newNameTb.TabIndex = 7;
            this.newNameTb.Visible = false;
            this.newNameTb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.newNameTb_KeyPress);
            // 
            // addNowBtn
            // 
            this.addNowBtn.Location = new System.Drawing.Point(249, 225);
            this.addNowBtn.Name = "addNowBtn";
            this.addNowBtn.Size = new System.Drawing.Size(62, 23);
            this.addNowBtn.TabIndex = 8;
            this.addNowBtn.Text = "Add Now";
            this.addNowBtn.UseVisualStyleBackColor = true;
            this.addNowBtn.Visible = false;
            this.addNowBtn.Click += new System.EventHandler(this.addNowBtn_Click);
            // 
            // editNameTb
            // 
            this.editNameTb.Location = new System.Drawing.Point(93, 256);
            this.editNameTb.Name = "editNameTb";
            this.editNameTb.Size = new System.Drawing.Size(150, 20);
            this.editNameTb.TabIndex = 9;
            this.editNameTb.Visible = false;
            this.editNameTb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editNameTb_KeyPress);
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(249, 254);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(62, 23);
            this.updateBtn.TabIndex = 10;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Visible = false;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // SetupDialogBtn
            // 
            this.SetupDialogBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetupDialogBtn.Location = new System.Drawing.Point(150, 302);
            this.SetupDialogBtn.Name = "SetupDialogBtn";
            this.SetupDialogBtn.Size = new System.Drawing.Size(110, 23);
            this.SetupDialogBtn.TabIndex = 6;
            this.SetupDialogBtn.Text = "Setup Driver...";
            this.SetupDialogBtn.UseVisualStyleBackColor = true;
            this.SetupDialogBtn.Visible = false;
            this.SetupDialogBtn.Click += new System.EventHandler(this.SetupDialogBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Current Instance:";
            // 
            // SelectInstanceBtn
            // 
            this.SelectInstanceBtn.Location = new System.Drawing.Point(238, 141);
            this.SelectInstanceBtn.Name = "SelectInstanceBtn";
            this.SelectInstanceBtn.Size = new System.Drawing.Size(100, 39);
            this.SelectInstanceBtn.TabIndex = 13;
            this.SelectInstanceBtn.Text = "Use Selected Instance";
            this.SelectInstanceBtn.UseVisualStyleBackColor = true;
            this.SelectInstanceBtn.Click += new System.EventHandler(this.SelectInstanceBtn_Click);
            // 
            // CurrentInstanceLBL
            // 
            this.CurrentInstanceLBL.AutoEllipsis = true;
            this.CurrentInstanceLBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CurrentInstanceLBL.Location = new System.Drawing.Point(235, 111);
            this.CurrentInstanceLBL.Name = "CurrentInstanceLBL";
            this.CurrentInstanceLBL.Size = new System.Drawing.Size(100, 20);
            this.CurrentInstanceLBL.TabIndex = 14;
            this.CurrentInstanceLBL.Text = "label4";
            // 
            // InstanceSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 334);
            this.Controls.Add(this.CurrentInstanceLBL);
            this.Controls.Add(this.SelectInstanceBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.editNameTb);
            this.Controls.Add(this.addNowBtn);
            this.Controls.Add(this.newNameTb);
            this.Controls.Add(this.SetupDialogBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.listBox1);
            this.Name = "InstanceSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instance Setup Form";
            this.Load += new System.EventHandler(this.InstanceSetupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Button RemoveBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.TextBox newNameTb;
        private System.Windows.Forms.Button addNowBtn;
        private System.Windows.Forms.TextBox editNameTb;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Button SetupDialogBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SelectInstanceBtn;
        private System.Windows.Forms.Label CurrentInstanceLBL;
    }
}