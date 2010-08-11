namespace PyxisLE_Control
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Test_BTN = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReqMove_BTN = new System.Windows.Forms.Button();
            this.HomeBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.GoToRel_BTN = new System.Windows.Forms.Button();
            this.Halt_BTN = new System.Windows.Forms.Button();
            this.ReverseCB = new System.Windows.Forms.CheckBox();
            this.RTL_CB = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Output Window:"});
            this.listBox1.Location = new System.Drawing.Point(12, 26);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(589, 186);
            this.listBox1.TabIndex = 0;
            // 
            // Test_BTN
            // 
            this.Test_BTN.Location = new System.Drawing.Point(12, 229);
            this.Test_BTN.Name = "Test_BTN";
            this.Test_BTN.Size = new System.Drawing.Size(158, 23);
            this.Test_BTN.TabIndex = 1;
            this.Test_BTN.Text = "Get Attached Device Info";
            this.Test_BTN.UseVisualStyleBackColor = true;
            this.Test_BTN.Click += new System.EventHandler(this.GetInfo_BTN_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 323);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MoveTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Absolute Position:";
            // 
            // ReqMove_BTN
            // 
            this.ReqMove_BTN.Location = new System.Drawing.Point(213, 321);
            this.ReqMove_BTN.Name = "ReqMove_BTN";
            this.ReqMove_BTN.Size = new System.Drawing.Size(64, 23);
            this.ReqMove_BTN.TabIndex = 3;
            this.ReqMove_BTN.Text = "Go To";
            this.ReqMove_BTN.UseVisualStyleBackColor = true;
            this.ReqMove_BTN.Click += new System.EventHandler(this.ReqMove_BTN_Click);
            // 
            // HomeBtn
            // 
            this.HomeBtn.Location = new System.Drawing.Point(15, 272);
            this.HomeBtn.Name = "HomeBtn";
            this.HomeBtn.Size = new System.Drawing.Size(95, 23);
            this.HomeBtn.TabIndex = 5;
            this.HomeBtn.Text = "Home Device";
            this.HomeBtn.UseVisualStyleBackColor = true;
            this.HomeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Enter Relative Distance:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(137, 366);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MoveTextBoxRelative_KeyPress);
            // 
            // GoToRel_BTN
            // 
            this.GoToRel_BTN.Location = new System.Drawing.Point(213, 364);
            this.GoToRel_BTN.Name = "GoToRel_BTN";
            this.GoToRel_BTN.Size = new System.Drawing.Size(64, 23);
            this.GoToRel_BTN.TabIndex = 7;
            this.GoToRel_BTN.Text = "Go";
            this.GoToRel_BTN.UseVisualStyleBackColor = true;
            this.GoToRel_BTN.Click += new System.EventHandler(this.GoToRel_BTN_Click);
            // 
            // Halt_BTN
            // 
            this.Halt_BTN.Location = new System.Drawing.Point(324, 326);
            this.Halt_BTN.Name = "Halt_BTN";
            this.Halt_BTN.Size = new System.Drawing.Size(65, 56);
            this.Halt_BTN.TabIndex = 8;
            this.Halt_BTN.Text = "Halt Move";
            this.Halt_BTN.UseVisualStyleBackColor = true;
            this.Halt_BTN.Click += new System.EventHandler(this.Halt_BTN_Click);
            // 
            // ReverseCB
            // 
            this.ReverseCB.AutoSize = true;
            this.ReverseCB.Location = new System.Drawing.Point(15, 401);
            this.ReverseCB.Name = "ReverseCB";
            this.ReverseCB.Size = new System.Drawing.Size(66, 17);
            this.ReverseCB.TabIndex = 9;
            this.ReverseCB.Text = "Reverse";
            this.ReverseCB.UseVisualStyleBackColor = true;
            this.ReverseCB.CheckedChanged += new System.EventHandler(this.ReverseCB_CheckedChanged);
            // 
            // RTL_CB
            // 
            this.RTL_CB.AutoSize = true;
            this.RTL_CB.Location = new System.Drawing.Point(15, 424);
            this.RTL_CB.Name = "RTL_CB";
            this.RTL_CB.Size = new System.Drawing.Size(180, 17);
            this.RTL_CB.TabIndex = 9;
            this.RTL_CB.Text = "Return to last position on Home?";
            this.RTL_CB.UseVisualStyleBackColor = true;
            this.RTL_CB.CheckedChanged += new System.EventHandler(this.RTL_CB_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-23, -45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(443, 290);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(158, 151);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            this.pictureBox2.Click += new System.EventHandler(this.Pic_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 459);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.RTL_CB);
            this.Controls.Add(this.ReverseCB);
            this.Controls.Add(this.Halt_BTN);
            this.Controls.Add(this.GoToRel_BTN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HomeBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReqMove_BTN);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Test_BTN);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Rotator Control Panel";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Test_BTN;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReqMove_BTN;
        private System.Windows.Forms.Button HomeBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button GoToRel_BTN;
        private System.Windows.Forms.Button Halt_BTN;
        private System.Windows.Forms.CheckBox ReverseCB;
        private System.Windows.Forms.CheckBox RTL_CB;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

