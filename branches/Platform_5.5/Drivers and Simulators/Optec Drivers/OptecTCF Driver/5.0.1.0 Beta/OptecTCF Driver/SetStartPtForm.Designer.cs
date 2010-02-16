namespace ASCOM.OptecTCF_Driver
{
    partial class SetStartPtForm
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
            this.CapStPt_BTN = new System.Windows.Forms.Button();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Pos_TB = new System.Windows.Forms.TextBox();
            this.Temp_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CapStPt_BTN
            // 
            this.CapStPt_BTN.Location = new System.Drawing.Point(37, 84);
            this.CapStPt_BTN.Name = "CapStPt_BTN";
            this.CapStPt_BTN.Size = new System.Drawing.Size(127, 26);
            this.CapStPt_BTN.TabIndex = 0;
            this.CapStPt_BTN.Text = "Capture Start Point";
            this.CapStPt_BTN.UseVisualStyleBackColor = true;
            this.CapStPt_BTN.Click += new System.EventHandler(this.CapStPt_BTN_Click);
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.Location = new System.Drawing.Point(178, 186);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 1;
            this.Cancel_Btn.Text = "Close";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Btn_Click);
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
            // Pos_TB
            // 
            this.Pos_TB.Location = new System.Drawing.Point(95, 25);
            this.Pos_TB.Name = "Pos_TB";
            this.Pos_TB.ReadOnly = true;
            this.Pos_TB.Size = new System.Drawing.Size(86, 20);
            this.Pos_TB.TabIndex = 5;
            this.Pos_TB.Text = "00000";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Set Start Point for Calculation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Temp_TB);
            this.groupBox1.Controls.Add(this.CapStPt_BTN);
            this.groupBox1.Controls.Add(this.Pos_TB);
            this.groupBox1.Location = new System.Drawing.Point(31, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 121);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Conditions";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // SetStartPtForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 217);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Cancel_Btn);
            this.Name = "SetStartPtForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calculate Temp Constant";
            this.Load += new System.EventHandler(this.SetStartPtForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CapStPt_BTN;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Pos_TB;
        private System.Windows.Forms.TextBox Temp_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}