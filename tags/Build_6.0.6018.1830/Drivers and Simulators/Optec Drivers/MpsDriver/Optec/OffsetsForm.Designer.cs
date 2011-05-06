namespace ASCOM.Optec
{
    partial class OffsetsForm
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
            this.RightAscension_NUD = new System.Windows.Forms.NumericUpDown();
            this.PortNumber_CB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Declination_NUD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Rotation_NUD = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.PortName_TB = new System.Windows.Forms.TextBox();
            this.OK_Btn = new System.Windows.Forms.Button();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.FocusOffset_NUD = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RightAscension_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Declination_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rotation_NUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FocusOffset_NUD)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Offsets:";
            // 
            // RightAscension_NUD
            // 
            this.RightAscension_NUD.DecimalPlaces = 1;
            this.RightAscension_NUD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RightAscension_NUD.Location = new System.Drawing.Point(117, 110);
            this.RightAscension_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.RightAscension_NUD.Name = "RightAscension_NUD";
            this.RightAscension_NUD.Size = new System.Drawing.Size(86, 20);
            this.RightAscension_NUD.TabIndex = 1;
            // 
            // PortNumber_CB
            // 
            this.PortNumber_CB.FormattingEnabled = true;
            this.PortNumber_CB.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.PortNumber_CB.Location = new System.Drawing.Point(203, 20);
            this.PortNumber_CB.Name = "PortNumber_CB";
            this.PortNumber_CB.Size = new System.Drawing.Size(33, 21);
            this.PortNumber_CB.TabIndex = 2;
            this.PortNumber_CB.Text = "1";
            this.PortNumber_CB.SelectedIndexChanged += new System.EventHandler(this.PortNumber_CB_SelectedIndexChanged);
            this.PortNumber_CB.Click += new System.EventHandler(this.PortNumber_CB_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select the Port you wish to configure:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Right Ascension";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Declination";
            // 
            // Declination_NUD
            // 
            this.Declination_NUD.DecimalPlaces = 1;
            this.Declination_NUD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Declination_NUD.Location = new System.Drawing.Point(117, 136);
            this.Declination_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Declination_NUD.Name = "Declination_NUD";
            this.Declination_NUD.Size = new System.Drawing.Size(86, 20);
            this.Declination_NUD.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Port Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Rotation";
            // 
            // Rotation_NUD
            // 
            this.Rotation_NUD.DecimalPlaces = 1;
            this.Rotation_NUD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Rotation_NUD.Location = new System.Drawing.Point(117, 162);
            this.Rotation_NUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Rotation_NUD.Name = "Rotation_NUD";
            this.Rotation_NUD.Size = new System.Drawing.Size(86, 20);
            this.Rotation_NUD.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(204, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Degrees";
            // 
            // PortName_TB
            // 
            this.PortName_TB.Location = new System.Drawing.Point(86, 54);
            this.PortName_TB.Name = "PortName_TB";
            this.PortName_TB.Size = new System.Drawing.Size(117, 20);
            this.PortName_TB.TabIndex = 11;
            // 
            // OK_Btn
            // 
            this.OK_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Btn.Location = new System.Drawing.Point(193, 214);
            this.OK_Btn.Name = "OK_Btn";
            this.OK_Btn.Size = new System.Drawing.Size(75, 23);
            this.OK_Btn.TabIndex = 12;
            this.OK_Btn.Text = "OK";
            this.OK_Btn.UseVisualStyleBackColor = true;
            this.OK_Btn.Click += new System.EventHandler(this.OK_Btn_Click);
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Btn.Location = new System.Drawing.Point(30, 222);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 13;
            this.Cancel_Btn.Text = "Cancel";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Focus";
            // 
            // FocusOffset_NUD
            // 
            this.FocusOffset_NUD.Location = new System.Drawing.Point(117, 188);
            this.FocusOffset_NUD.Maximum = new decimal(new int[] {
            7000,
            0,
            0,
            0});
            this.FocusOffset_NUD.Name = "FocusOffset_NUD";
            this.FocusOffset_NUD.Size = new System.Drawing.Size(86, 20);
            this.FocusOffset_NUD.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(204, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Motor Counts";
            // 
            // OffsetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 257);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FocusOffset_NUD);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.OK_Btn);
            this.Controls.Add(this.PortName_TB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Rotation_NUD);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Declination_NUD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PortNumber_CB);
            this.Controls.Add(this.RightAscension_NUD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Name = "OffsetsForm";
            this.Text = "Setup Offsets";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OffsetsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RightAscension_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Declination_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rotation_NUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FocusOffset_NUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button OK_Btn;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.NumericUpDown RightAscension_NUD;
        private System.Windows.Forms.ComboBox PortNumber_CB;
        private System.Windows.Forms.NumericUpDown Declination_NUD;
        private System.Windows.Forms.NumericUpDown Rotation_NUD;
        private System.Windows.Forms.TextBox PortName_TB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown FocusOffset_NUD;
        private System.Windows.Forms.Label label9;
    }
}