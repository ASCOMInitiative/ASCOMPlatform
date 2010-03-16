namespace ASCOM.OptecTCF_S
{
    partial class EndPointForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndPointForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.StartPointDateTime_TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StartPtTemp_TB = new System.Windows.Forms.TextBox();
            this.StartPointPos_TB = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Temp_TB = new System.Windows.Forms.TextBox();
            this.Pos_TB = new System.Windows.Forms.TextBox();
            this.CapStPt_BTN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.StartPointDateTime_TB);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.StartPtTemp_TB);
            this.groupBox2.Controls.Add(this.StartPointPos_TB);
            this.groupBox2.Location = new System.Drawing.Point(32, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 125);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start Point";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Captured:";
            // 
            // StartPointDateTime_TB
            // 
            this.StartPointDateTime_TB.Location = new System.Drawing.Point(95, 80);
            this.StartPointDateTime_TB.Multiline = true;
            this.StartPointDateTime_TB.Name = "StartPointDateTime_TB";
            this.StartPointDateTime_TB.ReadOnly = true;
            this.StartPointDateTime_TB.Size = new System.Drawing.Size(86, 34);
            this.StartPointDateTime_TB.TabIndex = 8;
            this.StartPointDateTime_TB.Text = "Time...\r\nDate...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Focus Position:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Temperature:";
            // 
            // StartPtTemp_TB
            // 
            this.StartPtTemp_TB.Location = new System.Drawing.Point(95, 49);
            this.StartPtTemp_TB.Name = "StartPtTemp_TB";
            this.StartPtTemp_TB.ReadOnly = true;
            this.StartPtTemp_TB.Size = new System.Drawing.Size(86, 20);
            this.StartPtTemp_TB.TabIndex = 6;
            this.StartPtTemp_TB.Text = "999999";
            // 
            // StartPointPos_TB
            // 
            this.StartPointPos_TB.Location = new System.Drawing.Point(95, 21);
            this.StartPointPos_TB.Name = "StartPointPos_TB";
            this.StartPointPos_TB.ReadOnly = true;
            this.StartPointPos_TB.Size = new System.Drawing.Size(86, 20);
            this.StartPointPos_TB.TabIndex = 5;
            this.StartPointPos_TB.Text = "00000";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Temp_TB);
            this.groupBox1.Controls.Add(this.Pos_TB);
            this.groupBox1.Controls.Add(this.CapStPt_BTN);
            this.groupBox1.Location = new System.Drawing.Point(32, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 118);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Conditions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Focus Position:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Temperature:";
            // 
            // Temp_TB
            // 
            this.Temp_TB.Location = new System.Drawing.Point(95, 53);
            this.Temp_TB.Name = "Temp_TB";
            this.Temp_TB.ReadOnly = true;
            this.Temp_TB.Size = new System.Drawing.Size(86, 20);
            this.Temp_TB.TabIndex = 6;
            this.Temp_TB.Text = "999999";
            // 
            // Pos_TB
            // 
            this.Pos_TB.Location = new System.Drawing.Point(95, 25);
            this.Pos_TB.Name = "Pos_TB";
            this.Pos_TB.ReadOnly = true;
            this.Pos_TB.Size = new System.Drawing.Size(86, 20);
            this.Pos_TB.TabIndex = 5;
            this.Pos_TB.Text = "00000";
            // 
            // CapStPt_BTN
            // 
            this.CapStPt_BTN.Location = new System.Drawing.Point(37, 79);
            this.CapStPt_BTN.Name = "CapStPt_BTN";
            this.CapStPt_BTN.Size = new System.Drawing.Size(127, 26);
            this.CapStPt_BTN.TabIndex = 13;
            this.CapStPt_BTN.Text = "Capture End Point";
            this.CapStPt_BTN.UseVisualStyleBackColor = true;
            this.CapStPt_BTN.Click += new System.EventHandler(this.CapStPt_BTN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 18);
            this.label4.TabIndex = 15;
            this.label4.Text = "Set End Point for Calculation";
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.Location = new System.Drawing.Point(174, 318);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 14;
            this.Cancel_Btn.Text = "Close";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Btn_Click);
            // 
            // EndPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 353);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Cancel_Btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EndPointForm";
            this.Text = "Temperature Compensation";
            this.Load += new System.EventHandler(this.EndPointForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox StartPointDateTime_TB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox StartPtTemp_TB;
        private System.Windows.Forms.TextBox StartPointPos_TB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Temp_TB;
        private System.Windows.Forms.TextBox Pos_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Button CapStPt_BTN;
    }
}