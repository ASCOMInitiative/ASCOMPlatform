namespace TestHarness
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgID1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ProgID2 = new System.Windows.Forms.Label();
            this.Connect1BTn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Connect2BTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnLabel1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ConnLabel2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TempLabel1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TempLabel2 = new System.Windows.Forms.Label();
            this.Move1 = new System.Windows.Forms.Button();
            this.Move2 = new System.Windows.Forms.Button();
            this.newPosTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CurrentPosLBL1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.CurrentPosLabel2 = new System.Windows.Forms.Label();
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.IsMoving1RB = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IsMoving2RB = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Focuser1:";
            // 
            // ProgID1
            // 
            this.ProgID1.AutoSize = true;
            this.ProgID1.Location = new System.Drawing.Point(72, 9);
            this.ProgID1.Name = "ProgID1";
            this.ProgID1.Size = new System.Drawing.Size(35, 13);
            this.ProgID1.TabIndex = 0;
            this.ProgID1.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(310, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Focuser 2:";
            // 
            // ProgID2
            // 
            this.ProgID2.AutoSize = true;
            this.ProgID2.Location = new System.Drawing.Point(373, 9);
            this.ProgID2.Name = "ProgID2";
            this.ProgID2.Size = new System.Drawing.Size(35, 13);
            this.ProgID2.TabIndex = 0;
            this.ProgID2.Text = "label1";
            // 
            // Connect1BTn
            // 
            this.Connect1BTn.Location = new System.Drawing.Point(15, 43);
            this.Connect1BTn.Name = "Connect1BTn";
            this.Connect1BTn.Size = new System.Drawing.Size(75, 23);
            this.Connect1BTn.TabIndex = 1;
            this.Connect1BTn.Text = "Choose1";
            this.Connect1BTn.UseVisualStyleBackColor = true;
            this.Connect1BTn.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(313, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Choose 2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 72);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Connect1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Connect2BTN
            // 
            this.Connect2BTN.Location = new System.Drawing.Point(313, 72);
            this.Connect2BTN.Name = "Connect2BTN";
            this.Connect2BTN.Size = new System.Drawing.Size(75, 23);
            this.Connect2BTN.TabIndex = 4;
            this.Connect2BTN.Text = "Connect2";
            this.Connect2BTN.UseVisualStyleBackColor = true;
            this.Connect2BTN.Click += new System.EventHandler(this.Connect2BTN_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connected:";
            // 
            // ConnLabel1
            // 
            this.ConnLabel1.AutoSize = true;
            this.ConnLabel1.Location = new System.Drawing.Point(80, 122);
            this.ConnLabel1.Name = "ConnLabel1";
            this.ConnLabel1.Size = new System.Drawing.Size(35, 13);
            this.ConnLabel1.TabIndex = 7;
            this.ConnLabel1.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Connected:";
            // 
            // ConnLabel2
            // 
            this.ConnLabel2.AutoSize = true;
            this.ConnLabel2.Location = new System.Drawing.Point(378, 122);
            this.ConnLabel2.Name = "ConnLabel2";
            this.ConnLabel2.Size = new System.Drawing.Size(35, 13);
            this.ConnLabel2.TabIndex = 7;
            this.ConnLabel2.Text = "label4";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Disconnect1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(313, 96);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Disconnect2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Temp";
            // 
            // TempLabel1
            // 
            this.TempLabel1.AutoSize = true;
            this.TempLabel1.Location = new System.Drawing.Point(80, 139);
            this.TempLabel1.Name = "TempLabel1";
            this.TempLabel1.Size = new System.Drawing.Size(13, 13);
            this.TempLabel1.TabIndex = 8;
            this.TempLabel1.Text = "?";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(310, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Temp";
            // 
            // TempLabel2
            // 
            this.TempLabel2.AutoSize = true;
            this.TempLabel2.Location = new System.Drawing.Point(375, 139);
            this.TempLabel2.Name = "TempLabel2";
            this.TempLabel2.Size = new System.Drawing.Size(13, 13);
            this.TempLabel2.TabIndex = 8;
            this.TempLabel2.Text = "?";
            // 
            // Move1
            // 
            this.Move1.Location = new System.Drawing.Point(18, 198);
            this.Move1.Name = "Move1";
            this.Move1.Size = new System.Drawing.Size(75, 23);
            this.Move1.TabIndex = 9;
            this.Move1.Text = "Move1";
            this.Move1.UseVisualStyleBackColor = true;
            this.Move1.Click += new System.EventHandler(this.Move1_Click);
            // 
            // Move2
            // 
            this.Move2.Location = new System.Drawing.Point(313, 198);
            this.Move2.Name = "Move2";
            this.Move2.Size = new System.Drawing.Size(75, 23);
            this.Move2.TabIndex = 9;
            this.Move2.Text = "Move2";
            this.Move2.UseVisualStyleBackColor = true;
            this.Move2.Click += new System.EventHandler(this.Move2_Click);
            // 
            // newPosTB
            // 
            this.newPosTB.Location = new System.Drawing.Point(159, 200);
            this.newPosTB.Name = "newPosTB";
            this.newPosTB.Size = new System.Drawing.Size(100, 20);
            this.newPosTB.TabIndex = 10;
            this.newPosTB.Text = "100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Current Pos:";
            // 
            // CurrentPosLBL1
            // 
            this.CurrentPosLBL1.AutoSize = true;
            this.CurrentPosLBL1.Location = new System.Drawing.Point(80, 224);
            this.CurrentPosLBL1.Name = "CurrentPosLBL1";
            this.CurrentPosLBL1.Size = new System.Drawing.Size(13, 13);
            this.CurrentPosLBL1.TabIndex = 12;
            this.CurrentPosLBL1.Text = "?";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(310, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Current Pos:";
            // 
            // CurrentPosLabel2
            // 
            this.CurrentPosLabel2.AutoSize = true;
            this.CurrentPosLabel2.Location = new System.Drawing.Point(375, 224);
            this.CurrentPosLabel2.Name = "CurrentPosLabel2";
            this.CurrentPosLabel2.Size = new System.Drawing.Size(13, 13);
            this.CurrentPosLabel2.TabIndex = 12;
            this.CurrentPosLabel2.Text = "?";
            // 
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Enabled = true;
            this.statusUpdateTimer.Interval = 250;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.statusUpdateTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IsMoving1RB);
            this.panel1.Location = new System.Drawing.Point(18, 240);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 48);
            this.panel1.TabIndex = 13;
            // 
            // IsMoving1RB
            // 
            this.IsMoving1RB.AutoSize = true;
            this.IsMoving1RB.Location = new System.Drawing.Point(12, 15);
            this.IsMoving1RB.Name = "IsMoving1RB";
            this.IsMoving1RB.Size = new System.Drawing.Size(71, 17);
            this.IsMoving1RB.TabIndex = 0;
            this.IsMoving1RB.TabStop = true;
            this.IsMoving1RB.Text = "Is Moving";
            this.IsMoving1RB.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.IsMoving2RB);
            this.panel2.Location = new System.Drawing.Point(313, 240);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(114, 48);
            this.panel2.TabIndex = 13;
            // 
            // IsMoving2RB
            // 
            this.IsMoving2RB.AutoSize = true;
            this.IsMoving2RB.Location = new System.Drawing.Point(12, 15);
            this.IsMoving2RB.Name = "IsMoving2RB";
            this.IsMoving2RB.Size = new System.Drawing.Size(71, 17);
            this.IsMoving2RB.TabIndex = 0;
            this.IsMoving2RB.TabStop = true;
            this.IsMoving2RB.Text = "Is Moving";
            this.IsMoving2RB.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 300);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CurrentPosLabel2);
            this.Controls.Add(this.CurrentPosLBL1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.newPosTB);
            this.Controls.Add(this.Move2);
            this.Controls.Add(this.Move1);
            this.Controls.Add(this.TempLabel2);
            this.Controls.Add(this.TempLabel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ConnLabel2);
            this.Controls.Add(this.ConnLabel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Connect2BTN);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Connect1BTn);
            this.Controls.Add(this.ProgID2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProgID1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Focusers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ProgID1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ProgID2;
        private System.Windows.Forms.Button Connect1BTn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Connect2BTN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ConnLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ConnLabel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label TempLabel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label TempLabel2;
        private System.Windows.Forms.Button Move1;
        private System.Windows.Forms.Button Move2;
        private System.Windows.Forms.TextBox newPosTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label CurrentPosLBL1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label CurrentPosLabel2;
        private System.Windows.Forms.Timer statusUpdateTimer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton IsMoving1RB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton IsMoving2RB;
    }
}

