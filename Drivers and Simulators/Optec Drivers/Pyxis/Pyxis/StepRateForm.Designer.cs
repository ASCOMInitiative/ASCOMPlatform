namespace ASCOM.Pyxis
{
    partial class StepRateForm
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
            this.CurrentRateLBL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Rate_NUD = new System.Windows.Forms.NumericUpDown();
            this.SetBTN = new System.Windows.Forms.Button();
            this.Cancel_BTN = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Rate_NUD)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Changing the step rate will affect the rotation speed of the Pyxis. Please note t" +
                "hat faster step rates may result in reduced torque.\r\n\r\n\r\n";
            // 
            // CurrentRateLBL
            // 
            this.CurrentRateLBL.AutoSize = true;
            this.CurrentRateLBL.Location = new System.Drawing.Point(12, 59);
            this.CurrentRateLBL.Name = "CurrentRateLBL";
            this.CurrentRateLBL.Size = new System.Drawing.Size(73, 13);
            this.CurrentRateLBL.TabIndex = 1;
            this.CurrentRateLBL.Text = "Current Rate: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select a new Step Rate:";
            // 
            // Rate_NUD
            // 
            this.Rate_NUD.Location = new System.Drawing.Point(140, 76);
            this.Rate_NUD.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.Rate_NUD.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.Rate_NUD.Name = "Rate_NUD";
            this.Rate_NUD.Size = new System.Drawing.Size(44, 20);
            this.Rate_NUD.TabIndex = 3;
            this.Rate_NUD.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // SetBTN
            // 
            this.SetBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SetBTN.Location = new System.Drawing.Point(201, 75);
            this.SetBTN.Name = "SetBTN";
            this.SetBTN.Size = new System.Drawing.Size(51, 23);
            this.SetBTN.TabIndex = 4;
            this.SetBTN.Text = "Set";
            this.SetBTN.UseVisualStyleBackColor = true;
            this.SetBTN.Click += new System.EventHandler(this.SetBTN_Click);
            // 
            // Cancel_BTN
            // 
            this.Cancel_BTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_BTN.Location = new System.Drawing.Point(189, 112);
            this.Cancel_BTN.Name = "Cancel_BTN";
            this.Cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.Cancel_BTN.TabIndex = 5;
            this.Cancel_BTN.Text = "Cancel";
            this.Cancel_BTN.UseVisualStyleBackColor = true;
            // 
            // StepRateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 142);
            this.Controls.Add(this.Cancel_BTN);
            this.Controls.Add(this.SetBTN);
            this.Controls.Add(this.Rate_NUD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentRateLBL);
            this.Controls.Add(this.label1);
            this.Name = "StepRateForm";
            this.Text = "Change Step Rate";
            this.Load += new System.EventHandler(this.StepRateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Rate_NUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurrentRateLBL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Rate_NUD;
        private System.Windows.Forms.Button SetBTN;
        private System.Windows.Forms.Button Cancel_BTN;
    }
}