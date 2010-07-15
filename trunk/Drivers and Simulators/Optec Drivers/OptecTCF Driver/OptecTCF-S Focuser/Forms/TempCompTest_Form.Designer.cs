namespace ASCOM.OptecTCF_S
{
    partial class TempCompTest_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempCompTest_Form));
            this.label1 = new System.Windows.Forms.Label();
            this.duration_NUD = new System.Windows.Forms.NumericUpDown();
            this.Start_Btn = new System.Windows.Forms.Button();
            this.Stop_Btn = new System.Windows.Forms.Button();
            this.Close_Btn = new System.Windows.Forms.Button();
            this.Clear_Btn = new System.Windows.Forms.Button();
            this.Output_LB = new System.Windows.Forms.ListBox();
            this.TestTimer = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.duration_NUD)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the duration of the test (Seconds):";
            // 
            // duration_NUD
            // 
            this.duration_NUD.Location = new System.Drawing.Point(238, 8);
            this.duration_NUD.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.duration_NUD.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.duration_NUD.Name = "duration_NUD";
            this.duration_NUD.Size = new System.Drawing.Size(64, 20);
            this.duration_NUD.TabIndex = 1;
            this.duration_NUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Start_Btn
            // 
            this.Start_Btn.Location = new System.Drawing.Point(15, 27);
            this.Start_Btn.Name = "Start_Btn";
            this.Start_Btn.Size = new System.Drawing.Size(75, 23);
            this.Start_Btn.TabIndex = 2;
            this.Start_Btn.Text = "Start Test";
            this.Start_Btn.UseVisualStyleBackColor = true;
            this.Start_Btn.Click += new System.EventHandler(this.Start_Btn_Click);
            // 
            // Stop_Btn
            // 
            this.Stop_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Stop_Btn.Enabled = false;
            this.Stop_Btn.Location = new System.Drawing.Point(15, 223);
            this.Stop_Btn.Name = "Stop_Btn";
            this.Stop_Btn.Size = new System.Drawing.Size(75, 23);
            this.Stop_Btn.TabIndex = 3;
            this.Stop_Btn.Text = "Stop Test";
            this.Stop_Btn.UseVisualStyleBackColor = true;
            this.Stop_Btn.Click += new System.EventHandler(this.Stop_Btn_Click);
            // 
            // Close_Btn
            // 
            this.Close_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_Btn.Location = new System.Drawing.Point(227, 223);
            this.Close_Btn.Name = "Close_Btn";
            this.Close_Btn.Size = new System.Drawing.Size(75, 23);
            this.Close_Btn.TabIndex = 4;
            this.Close_Btn.Text = "Close";
            this.Close_Btn.UseVisualStyleBackColor = true;
            this.Close_Btn.Click += new System.EventHandler(this.Close_Btn_Click);
            // 
            // Clear_Btn
            // 
            this.Clear_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Clear_Btn.Location = new System.Drawing.Point(121, 223);
            this.Clear_Btn.MinimumSize = new System.Drawing.Size(0, 20);
            this.Clear_Btn.Name = "Clear_Btn";
            this.Clear_Btn.Size = new System.Drawing.Size(75, 23);
            this.Clear_Btn.TabIndex = 5;
            this.Clear_Btn.Text = "Clear Data";
            this.Clear_Btn.UseVisualStyleBackColor = true;
            this.Clear_Btn.Click += new System.EventHandler(this.clearData);
            // 
            // Output_LB
            // 
            this.Output_LB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_LB.FormattingEnabled = true;
            this.Output_LB.Location = new System.Drawing.Point(15, 56);
            this.Output_LB.Name = "Output_LB";
            this.Output_LB.Size = new System.Drawing.Size(287, 160);
            this.Output_LB.TabIndex = 6;
            // 
            // TestTimer
            // 
            this.TestTimer.Tick += new System.EventHandler(this.TestTimer_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(105, 31);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(91, 16);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Visible = false;
            // 
            // TempCompTest_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 258);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Output_LB);
            this.Controls.Add(this.Clear_Btn);
            this.Controls.Add(this.Close_Btn);
            this.Controls.Add(this.Stop_Btn);
            this.Controls.Add(this.Start_Btn);
            this.Controls.Add(this.duration_NUD);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(16, 290);
            this.Name = "TempCompTest_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Temperature Compensation Test";
            this.Load += new System.EventHandler(this.TempCompTest_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.duration_NUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown duration_NUD;
        private System.Windows.Forms.Button Start_Btn;
        private System.Windows.Forms.Button Stop_Btn;
        private System.Windows.Forms.Button Close_Btn;
        private System.Windows.Forms.Button Clear_Btn;
        private System.Windows.Forms.ListBox Output_LB;
        private System.Windows.Forms.Timer TestTimer;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}