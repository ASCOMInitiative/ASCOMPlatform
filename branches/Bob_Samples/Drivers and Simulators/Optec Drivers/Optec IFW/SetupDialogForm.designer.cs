namespace ASCOM.Optec_IFW
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.IFW3_RB = new System.Windows.Forms.RadioButton();
            this.IFW_RB = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.Connect_BTN = new System.Windows.Forms.Button();
            this.SaveData_Btn = new System.Windows.Forms.Button();
            this.ComPort_Picker = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Filter1Name_TB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Filter2Name_TB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Filter3Name_TB = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Filter4Name_TB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Filter5Name_TB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.Filter6Name_TB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Filter7Name_TB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Filter8Name_TB = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.Filter9Name_TB = new System.Windows.Forms.TextBox();
            this.F1Offset_TB = new System.Windows.Forms.TextBox();
            this.F2Offset_TB = new System.Windows.Forms.TextBox();
            this.F9Offset_TB = new System.Windows.Forms.TextBox();
            this.F3Offset_TB = new System.Windows.Forms.TextBox();
            this.F8Offset_TB = new System.Windows.Forms.TextBox();
            this.F4Offset_TB = new System.Windows.Forms.TextBox();
            this.F7Offset_TB = new System.Windows.Forms.TextBox();
            this.F5Offset_TB = new System.Windows.Forms.TextBox();
            this.F6Offset_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AdvancedButton = new System.Windows.Forms.Button();
            this.Home_Btn = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ComPort_Picker)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(405, 332);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // IFW3_RB
            // 
            this.IFW3_RB.AutoSize = true;
            this.IFW3_RB.Location = new System.Drawing.Point(423, 62);
            this.IFW3_RB.Name = "IFW3_RB";
            this.IFW3_RB.Size = new System.Drawing.Size(51, 17);
            this.IFW3_RB.TabIndex = 36;
            this.IFW3_RB.Text = "IFW3";
            this.IFW3_RB.UseVisualStyleBackColor = true;
            this.IFW3_RB.CheckedChanged += new System.EventHandler(this.IFW3_RB_CheckedChanged);
            // 
            // IFW_RB
            // 
            this.IFW_RB.AutoSize = true;
            this.IFW_RB.Location = new System.Drawing.Point(359, 62);
            this.IFW_RB.Name = "IFW_RB";
            this.IFW_RB.Size = new System.Drawing.Size(45, 17);
            this.IFW_RB.TabIndex = 35;
            this.IFW_RB.Text = "IFW";
            this.IFW_RB.UseVisualStyleBackColor = true;
            this.IFW_RB.CheckedChanged += new System.EventHandler(this.IFW_RB_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(12, 60);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(319, 20);
            this.label17.TabIndex = 34;
            this.label17.Text = "Select which filter wheel you are using:";
            // 
            // Connect_BTN
            // 
            this.Connect_BTN.AllowDrop = true;
            this.Connect_BTN.Location = new System.Drawing.Point(328, 100);
            this.Connect_BTN.Name = "Connect_BTN";
            this.Connect_BTN.Size = new System.Drawing.Size(93, 32);
            this.Connect_BTN.TabIndex = 33;
            this.Connect_BTN.Text = "Connect";
            this.Connect_BTN.UseVisualStyleBackColor = true;
            this.Connect_BTN.Click += new System.EventHandler(this.Connect_BTN_Click);
            // 
            // SaveData_Btn
            // 
            this.SaveData_Btn.Enabled = false;
            this.SaveData_Btn.Location = new System.Drawing.Point(233, 280);
            this.SaveData_Btn.Name = "SaveData_Btn";
            this.SaveData_Btn.Size = new System.Drawing.Size(93, 26);
            this.SaveData_Btn.TabIndex = 2;
            this.SaveData_Btn.Text = "Save Data";
            this.SaveData_Btn.UseVisualStyleBackColor = true;
            this.SaveData_Btn.Click += new System.EventHandler(this.SaveData_Btn_Click);
            // 
            // ComPort_Picker
            // 
            this.ComPort_Picker.Location = new System.Drawing.Point(359, 24);
            this.ComPort_Picker.Name = "ComPort_Picker";
            this.ComPort_Picker.Size = new System.Drawing.Size(42, 20);
            this.ComPort_Picker.TabIndex = 1;
            this.ComPort_Picker.ValueChanged += new System.EventHandler(this.ComPort_Picker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a COM port to use for this device:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(375, 239);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(93, 79);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "Note: You must connect the device and press Save Data before continuing.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.Filter1Name_TB);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.Filter2Name_TB);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.Filter3Name_TB);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.Filter4Name_TB);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.Filter5Name_TB);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.Filter6Name_TB);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.Filter7Name_TB);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Filter8Name_TB);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.Filter9Name_TB);
            this.panel1.Controls.Add(this.F1Offset_TB);
            this.panel1.Controls.Add(this.F2Offset_TB);
            this.panel1.Controls.Add(this.F9Offset_TB);
            this.panel1.Controls.Add(this.F3Offset_TB);
            this.panel1.Controls.Add(this.F8Offset_TB);
            this.panel1.Controls.Add(this.F4Offset_TB);
            this.panel1.Controls.Add(this.F7Offset_TB);
            this.panel1.Controls.Add(this.F5Offset_TB);
            this.panel1.Controls.Add(this.F6Offset_TB);
            this.panel1.Location = new System.Drawing.Point(16, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 282);
            this.panel1.TabIndex = 42;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Filter 6:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(75, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Filter 5:";
            // 
            // Filter1Name_TB
            // 
            this.Filter1Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter1Name_TB.Enabled = false;
            this.Filter1Name_TB.Location = new System.Drawing.Point(58, 38);
            this.Filter1Name_TB.MaxLength = 8;
            this.Filter1Name_TB.Name = "Filter1Name_TB";
            this.Filter1Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter1Name_TB.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Filter 7:";
            // 
            // Filter2Name_TB
            // 
            this.Filter2Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter2Name_TB.Enabled = false;
            this.Filter2Name_TB.Location = new System.Drawing.Point(58, 64);
            this.Filter2Name_TB.MaxLength = 8;
            this.Filter2Name_TB.Name = "Filter2Name_TB";
            this.Filter2Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter2Name_TB.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Filter 4:";
            // 
            // Filter3Name_TB
            // 
            this.Filter3Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter3Name_TB.Enabled = false;
            this.Filter3Name_TB.Location = new System.Drawing.Point(58, 90);
            this.Filter3Name_TB.MaxLength = 8;
            this.Filter3Name_TB.Name = "Filter3Name_TB";
            this.Filter3Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter3Name_TB.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 223);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Filter 8:";
            // 
            // Filter4Name_TB
            // 
            this.Filter4Name_TB.AcceptsReturn = true;
            this.Filter4Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter4Name_TB.Enabled = false;
            this.Filter4Name_TB.Location = new System.Drawing.Point(58, 116);
            this.Filter4Name_TB.MaxLength = 8;
            this.Filter4Name_TB.Name = "Filter4Name_TB";
            this.Filter4Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter4Name_TB.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Filter 3:";
            // 
            // Filter5Name_TB
            // 
            this.Filter5Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter5Name_TB.Enabled = false;
            this.Filter5Name_TB.Location = new System.Drawing.Point(58, 142);
            this.Filter5Name_TB.MaxLength = 8;
            this.Filter5Name_TB.Name = "Filter5Name_TB";
            this.Filter5Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter5Name_TB.TabIndex = 17;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 249);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Filter 9:";
            // 
            // Filter6Name_TB
            // 
            this.Filter6Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter6Name_TB.Enabled = false;
            this.Filter6Name_TB.Location = new System.Drawing.Point(58, 168);
            this.Filter6Name_TB.MaxLength = 8;
            this.Filter6Name_TB.Name = "Filter6Name_TB";
            this.Filter6Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter6Name_TB.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Filter 2:";
            // 
            // Filter7Name_TB
            // 
            this.Filter7Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter7Name_TB.Enabled = false;
            this.Filter7Name_TB.Location = new System.Drawing.Point(58, 194);
            this.Filter7Name_TB.MaxLength = 8;
            this.Filter7Name_TB.Name = "Filter7Name_TB";
            this.Filter7Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter7Name_TB.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Filter 1:";
            // 
            // Filter8Name_TB
            // 
            this.Filter8Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter8Name_TB.Enabled = false;
            this.Filter8Name_TB.Location = new System.Drawing.Point(58, 220);
            this.Filter8Name_TB.MaxLength = 8;
            this.Filter8Name_TB.Name = "Filter8Name_TB";
            this.Filter8Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter8Name_TB.TabIndex = 20;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(154, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 32;
            this.label16.Text = "Offset";
            // 
            // Filter9Name_TB
            // 
            this.Filter9Name_TB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Filter9Name_TB.Enabled = false;
            this.Filter9Name_TB.Location = new System.Drawing.Point(58, 246);
            this.Filter9Name_TB.MaxLength = 8;
            this.Filter9Name_TB.Name = "Filter9Name_TB";
            this.Filter9Name_TB.Size = new System.Drawing.Size(65, 20);
            this.Filter9Name_TB.TabIndex = 21;
            // 
            // F1Offset_TB
            // 
            this.F1Offset_TB.Enabled = false;
            this.F1Offset_TB.Location = new System.Drawing.Point(139, 38);
            this.F1Offset_TB.MaxLength = 8;
            this.F1Offset_TB.Name = "F1Offset_TB";
            this.F1Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F1Offset_TB.TabIndex = 22;
            // 
            // F2Offset_TB
            // 
            this.F2Offset_TB.Enabled = false;
            this.F2Offset_TB.Location = new System.Drawing.Point(139, 64);
            this.F2Offset_TB.MaxLength = 8;
            this.F2Offset_TB.Name = "F2Offset_TB";
            this.F2Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F2Offset_TB.TabIndex = 23;
            // 
            // F9Offset_TB
            // 
            this.F9Offset_TB.Enabled = false;
            this.F9Offset_TB.Location = new System.Drawing.Point(139, 246);
            this.F9Offset_TB.MaxLength = 8;
            this.F9Offset_TB.Name = "F9Offset_TB";
            this.F9Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F9Offset_TB.TabIndex = 30;
            // 
            // F3Offset_TB
            // 
            this.F3Offset_TB.Enabled = false;
            this.F3Offset_TB.Location = new System.Drawing.Point(139, 90);
            this.F3Offset_TB.MaxLength = 8;
            this.F3Offset_TB.Name = "F3Offset_TB";
            this.F3Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F3Offset_TB.TabIndex = 24;
            // 
            // F8Offset_TB
            // 
            this.F8Offset_TB.Enabled = false;
            this.F8Offset_TB.Location = new System.Drawing.Point(139, 220);
            this.F8Offset_TB.MaxLength = 8;
            this.F8Offset_TB.Name = "F8Offset_TB";
            this.F8Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F8Offset_TB.TabIndex = 29;
            // 
            // F4Offset_TB
            // 
            this.F4Offset_TB.Enabled = false;
            this.F4Offset_TB.Location = new System.Drawing.Point(139, 116);
            this.F4Offset_TB.MaxLength = 8;
            this.F4Offset_TB.Name = "F4Offset_TB";
            this.F4Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F4Offset_TB.TabIndex = 25;
            // 
            // F7Offset_TB
            // 
            this.F7Offset_TB.Enabled = false;
            this.F7Offset_TB.Location = new System.Drawing.Point(139, 194);
            this.F7Offset_TB.MaxLength = 8;
            this.F7Offset_TB.Name = "F7Offset_TB";
            this.F7Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F7Offset_TB.TabIndex = 28;
            // 
            // F5Offset_TB
            // 
            this.F5Offset_TB.Enabled = false;
            this.F5Offset_TB.Location = new System.Drawing.Point(139, 142);
            this.F5Offset_TB.MaxLength = 8;
            this.F5Offset_TB.Name = "F5Offset_TB";
            this.F5Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F5Offset_TB.TabIndex = 26;
            // 
            // F6Offset_TB
            // 
            this.F6Offset_TB.Enabled = false;
            this.F6Offset_TB.Location = new System.Drawing.Point(139, 168);
            this.F6Offset_TB.MaxLength = 8;
            this.F6Offset_TB.Name = "F6Offset_TB";
            this.F6Offset_TB.Size = new System.Drawing.Size(63, 20);
            this.F6Offset_TB.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 20);
            this.label2.TabIndex = 43;
            this.label2.Text = "Filter Names and Offsets:";
            // 
            // AdvancedButton
            // 
            this.AdvancedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedButton.Location = new System.Drawing.Point(303, 373);
            this.AdvancedButton.Name = "AdvancedButton";
            this.AdvancedButton.Size = new System.Drawing.Size(75, 25);
            this.AdvancedButton.TabIndex = 45;
            this.AdvancedButton.Text = "Advanced";
            this.AdvancedButton.UseVisualStyleBackColor = true;
            this.AdvancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
            // 
            // Home_Btn
            // 
            this.Home_Btn.Enabled = false;
            this.Home_Btn.Location = new System.Drawing.Point(233, 321);
            this.Home_Btn.Name = "Home_Btn";
            this.Home_Btn.Size = new System.Drawing.Size(93, 26);
            this.Home_Btn.TabIndex = 46;
            this.Home_Btn.Text = "Home Device";
            this.Home_Btn.UseVisualStyleBackColor = true;
            this.Home_Btn.Click += new System.EventHandler(this.Home_Btn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::ASCOM.Optec_IFW.Properties.Resources.ASCOM;
            this.pictureBox2.Location = new System.Drawing.Point(427, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 56);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ASCOM.Optec_IFW.Properties.Resources.Optec_Logo_medium_png;
            this.pictureBox1.Location = new System.Drawing.Point(242, 135);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(198, 99);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(405, 373);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(476, 407);
            this.Controls.Add(this.Home_Btn);
            this.Controls.Add(this.AdvancedButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IFW3_RB);
            this.Controls.Add(this.IFW_RB);
            this.Controls.Add(this.ComPort_Picker);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.SaveData_Btn);
            this.Controls.Add(this.Connect_BTN);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec_IFW Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ComPort_Picker)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button SaveData_Btn;
        private System.Windows.Forms.NumericUpDown ComPort_Picker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Connect_BTN;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton IFW3_RB;
        private System.Windows.Forms.RadioButton IFW_RB;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Filter1Name_TB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Filter2Name_TB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Filter3Name_TB;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox Filter4Name_TB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Filter5Name_TB;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox Filter6Name_TB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Filter7Name_TB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Filter8Name_TB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox Filter9Name_TB;
        private System.Windows.Forms.TextBox F1Offset_TB;
        private System.Windows.Forms.TextBox F2Offset_TB;
        private System.Windows.Forms.TextBox F9Offset_TB;
        private System.Windows.Forms.TextBox F3Offset_TB;
        private System.Windows.Forms.TextBox F8Offset_TB;
        private System.Windows.Forms.TextBox F4Offset_TB;
        private System.Windows.Forms.TextBox F7Offset_TB;
        private System.Windows.Forms.TextBox F5Offset_TB;
        private System.Windows.Forms.TextBox F6Offset_TB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AdvancedButton;
        private System.Windows.Forms.Button Home_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button cmdCancel;
    }
}