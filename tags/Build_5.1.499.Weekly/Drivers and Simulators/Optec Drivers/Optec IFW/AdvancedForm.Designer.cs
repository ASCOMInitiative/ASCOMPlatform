namespace ASCOM.Optec_IFW
{
    partial class AdvancedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedForm));
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.v = new System.Windows.Forms.Button();
            this.Ad_Cancel_Btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Ad_CmdPicker_Cb = new System.Windows.Forms.ComboBox();
            this.Ad_Save = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Ad_CmdToSend = new System.Windows.Forms.TextBox();
            this.Ad_Send_Btn = new System.Windows.Forms.Button();
            this.Ad_ComPortLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ReadTimeout_Picker = new System.Windows.Forms.NumericUpDown();
            this.NextValue_Picker = new System.Windows.Forms.NumericUpDown();
            this.BackValue_Picker = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ReadTimeout_Picker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextValue_Picker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackValue_Picker)).BeginInit();
            this.SuspendLayout();
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(41, 57);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 42;
            this.label19.Text = "\"NEXT\" Offset:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(194, 57);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 13);
            this.label20.TabIndex = 43;
            this.label20.Text = "\"BACK\" Offset:";
            // 
            // v
            // 
            this.v.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.v.Location = new System.Drawing.Point(462, 297);
            this.v.Name = "v";
            this.v.Size = new System.Drawing.Size(75, 23);
            this.v.TabIndex = 46;
            this.v.Text = "OK";
            this.v.UseVisualStyleBackColor = true;
            this.v.Click += new System.EventHandler(this.v_Click);
            // 
            // Ad_Cancel_Btn
            // 
            this.Ad_Cancel_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ad_Cancel_Btn.Location = new System.Drawing.Point(359, 297);
            this.Ad_Cancel_Btn.Name = "Ad_Cancel_Btn";
            this.Ad_Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Ad_Cancel_Btn.TabIndex = 47;
            this.Ad_Cancel_Btn.Text = "Cancel";
            this.Ad_Cancel_Btn.UseVisualStyleBackColor = true;
            this.Ad_Cancel_Btn.Click += new System.EventHandler(this.Ad_Cancel_Btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 20);
            this.label3.TabIndex = 48;
            this.label3.Text = "Enter Centering Constants:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 20);
            this.label1.TabIndex = 49;
            this.label1.Text = "Serial Com Debug:";
            // 
            // Ad_CmdPicker_Cb
            // 
            this.Ad_CmdPicker_Cb.FormattingEnabled = true;
            this.Ad_CmdPicker_Cb.Items.AddRange(new object[] {
            "WSMODE - Initialize Connection, Enter Main Program Loop - No Parameters - Returns" +
                " \"!\"",
            "WHOME* - Home Device - No Parameters - Returns Wheel ID",
            "WIDENT - Get the Wheel Identity - No Parameters - Returns Wheel ID",
            "WFILTR - Get the current Filter Position - No Parameters - Returns Filter Number",
            "WGOTO* - Go to specified filter position - Paramater: x = position - Returns \"*\"",
            "WREAD* - Read the Stored Filter Names - No Parameters - Returns Filter Names",
            "WLOAD - Store Filter Names to EEPROM - Parameter: WheelID + * + 8 char per filter" +
                " - Returns \"!\" ",
            "WEXITS - Exit serial loop and return to manual operation - No Paramaters - Return" +
                "s \"END\""});
            this.Ad_CmdPicker_Cb.Location = new System.Drawing.Point(12, 253);
            this.Ad_CmdPicker_Cb.Name = "Ad_CmdPicker_Cb";
            this.Ad_CmdPicker_Cb.Size = new System.Drawing.Size(525, 21);
            this.Ad_CmdPicker_Cb.TabIndex = 54;
            this.Ad_CmdPicker_Cb.Text = "Command Reference...";
            this.Ad_CmdPicker_Cb.SelectedIndexChanged += new System.EventHandler(this.Ad_CmdPicker_Cb_SelectedIndexChanged);
            // 
            // Ad_Save
            // 
            this.Ad_Save.Location = new System.Drawing.Point(360, 52);
            this.Ad_Save.Name = "Ad_Save";
            this.Ad_Save.Size = new System.Drawing.Size(109, 23);
            this.Ad_Save.TabIndex = 56;
            this.Ad_Save.Text = "Save";
            this.Ad_Save.UseVisualStyleBackColor = true;
            this.Ad_Save.Click += new System.EventHandler(this.Ad_Save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-21, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(646, 52);
            this.label4.TabIndex = 57;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Command To Send:";
            // 
            // Ad_CmdToSend
            // 
            this.Ad_CmdToSend.Location = new System.Drawing.Point(145, 185);
            this.Ad_CmdToSend.Name = "Ad_CmdToSend";
            this.Ad_CmdToSend.Size = new System.Drawing.Size(180, 20);
            this.Ad_CmdToSend.TabIndex = 51;
            // 
            // Ad_Send_Btn
            // 
            this.Ad_Send_Btn.Location = new System.Drawing.Point(347, 182);
            this.Ad_Send_Btn.Name = "Ad_Send_Btn";
            this.Ad_Send_Btn.Size = new System.Drawing.Size(75, 23);
            this.Ad_Send_Btn.TabIndex = 52;
            this.Ad_Send_Btn.Text = "Send";
            this.Ad_Send_Btn.UseVisualStyleBackColor = true;
            this.Ad_Send_Btn.Click += new System.EventHandler(this.Ad_Send_Btn_Click);
            // 
            // Ad_ComPortLabel
            // 
            this.Ad_ComPortLabel.AutoSize = true;
            this.Ad_ComPortLabel.Location = new System.Drawing.Point(36, 161);
            this.Ad_ComPortLabel.Name = "Ad_ComPortLabel";
            this.Ad_ComPortLabel.Size = new System.Drawing.Size(33, 13);
            this.Ad_ComPortLabel.TabIndex = 55;
            this.Ad_ComPortLabel.Text = "Note:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "Receive Timout (ms):";
            // 
            // ReadTimeout_Picker
            // 
            this.ReadTimeout_Picker.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReadTimeout_Picker.Location = new System.Drawing.Point(150, 212);
            this.ReadTimeout_Picker.Maximum = new decimal(new int[] {
            120000,
            0,
            0,
            0});
            this.ReadTimeout_Picker.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.ReadTimeout_Picker.Name = "ReadTimeout_Picker";
            this.ReadTimeout_Picker.Size = new System.Drawing.Size(63, 20);
            this.ReadTimeout_Picker.TabIndex = 59;
            this.ReadTimeout_Picker.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // NextValue_Picker
            // 
            this.NextValue_Picker.Location = new System.Drawing.Point(123, 55);
            this.NextValue_Picker.Maximum = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.NextValue_Picker.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NextValue_Picker.Name = "NextValue_Picker";
            this.NextValue_Picker.Size = new System.Drawing.Size(47, 20);
            this.NextValue_Picker.TabIndex = 60;
            this.NextValue_Picker.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.NextValue_Picker.ValueChanged += new System.EventHandler(this.NextValue_Picker_ValueChanged);
            // 
            // BackValue_Picker
            // 
            this.BackValue_Picker.Location = new System.Drawing.Point(276, 55);
            this.BackValue_Picker.Maximum = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.BackValue_Picker.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.BackValue_Picker.Name = "BackValue_Picker";
            this.BackValue_Picker.Size = new System.Drawing.Size(47, 20);
            this.BackValue_Picker.TabIndex = 61;
            this.BackValue_Picker.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.BackValue_Picker.ValueChanged += new System.EventHandler(this.BackValue_Picker_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(311, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "Note: You MUST send WSMODE first to establish a connection.";
            // 
            // AdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(549, 332);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BackValue_Picker);
            this.Controls.Add(this.NextValue_Picker);
            this.Controls.Add(this.ReadTimeout_Picker);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Ad_Save);
            this.Controls.Add(this.Ad_ComPortLabel);
            this.Controls.Add(this.Ad_CmdPicker_Cb);
            this.Controls.Add(this.Ad_Send_Btn);
            this.Controls.Add(this.Ad_CmdToSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Ad_Cancel_Btn);
            this.Controls.Add(this.v);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label4);
            this.Name = "AdvancedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AdvancedForm";
            this.Load += new System.EventHandler(this.AdvancedForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReadTimeout_Picker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextValue_Picker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackValue_Picker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button v;
        private System.Windows.Forms.Button Ad_Cancel_Btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Ad_CmdPicker_Cb;
        private System.Windows.Forms.Button Ad_Save;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Ad_CmdToSend;
        private System.Windows.Forms.Button Ad_Send_Btn;
        private System.Windows.Forms.Label Ad_ComPortLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ReadTimeout_Picker;
        private System.Windows.Forms.NumericUpDown NextValue_Picker;
        private System.Windows.Forms.NumericUpDown BackValue_Picker;
        private System.Windows.Forms.Label label6;
    }
}