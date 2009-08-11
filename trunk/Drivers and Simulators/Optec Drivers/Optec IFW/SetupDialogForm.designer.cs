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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.TestConnect_Btn = new System.Windows.Forms.Button();
            this.DissConnBtn = new System.Windows.Forms.Button();
            this.HomeBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.WheelId_TB = new System.Windows.Forms.TextBox();
            this.ReadNames_Btn = new System.Windows.Forms.Button();
            this.FilterNames_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckConn_Btn = new System.Windows.Forms.Button();
            this.ConnStatus_TB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GoTo_Btn = new System.Windows.Forms.Button();
            this.GoToPos_CB = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BackOffset_TB = new System.Windows.Forms.TextBox();
            this.NextOffset_TB = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.IFW3_RB = new System.Windows.Forms.RadioButton();
            this.IFW_RB = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.Connect_BTN = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SaveData_Btn = new System.Windows.Forms.Button();
            this.ComPort_Picker = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Filter1Name_TB = new System.Windows.Forms.TextBox();
            this.Filter2Name_TB = new System.Windows.Forms.TextBox();
            this.Filter3Name_TB = new System.Windows.Forms.TextBox();
            this.Filter4Name_TB = new System.Windows.Forms.TextBox();
            this.Filter5Name_TB = new System.Windows.Forms.TextBox();
            this.Filter6Name_TB = new System.Windows.Forms.TextBox();
            this.Filter7Name_TB = new System.Windows.Forms.TextBox();
            this.Filter8Name_TB = new System.Windows.Forms.TextBox();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComPort_Picker)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(584, 434);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(584, 464);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Optec_IFW.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(462, 35);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // TestConnect_Btn
            // 
            this.TestConnect_Btn.Location = new System.Drawing.Point(16, 86);
            this.TestConnect_Btn.Name = "TestConnect_Btn";
            this.TestConnect_Btn.Size = new System.Drawing.Size(100, 23);
            this.TestConnect_Btn.TabIndex = 4;
            this.TestConnect_Btn.Text = "Connect";
            this.TestConnect_Btn.UseVisualStyleBackColor = true;
            this.TestConnect_Btn.Click += new System.EventHandler(this.TestConnect_Btn_Click);
            // 
            // DissConnBtn
            // 
            this.DissConnBtn.Location = new System.Drawing.Point(16, 265);
            this.DissConnBtn.Name = "DissConnBtn";
            this.DissConnBtn.Size = new System.Drawing.Size(100, 23);
            this.DissConnBtn.TabIndex = 5;
            this.DissConnBtn.Text = "Disconnect";
            this.DissConnBtn.UseVisualStyleBackColor = true;
            this.DissConnBtn.Click += new System.EventHandler(this.DissConnBtn_Click);
            // 
            // HomeBtn
            // 
            this.HomeBtn.Location = new System.Drawing.Point(16, 144);
            this.HomeBtn.Name = "HomeBtn";
            this.HomeBtn.Size = new System.Drawing.Size(100, 23);
            this.HomeBtn.TabIndex = 6;
            this.HomeBtn.Text = "Home Device";
            this.HomeBtn.UseVisualStyleBackColor = true;
            this.HomeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Wheel ID:";
            // 
            // WheelId_TB
            // 
            this.WheelId_TB.Location = new System.Drawing.Point(204, 141);
            this.WheelId_TB.Name = "WheelId_TB";
            this.WheelId_TB.Size = new System.Drawing.Size(26, 20);
            this.WheelId_TB.TabIndex = 8;
            // 
            // ReadNames_Btn
            // 
            this.ReadNames_Btn.Location = new System.Drawing.Point(16, 173);
            this.ReadNames_Btn.Name = "ReadNames_Btn";
            this.ReadNames_Btn.Size = new System.Drawing.Size(100, 23);
            this.ReadNames_Btn.TabIndex = 9;
            this.ReadNames_Btn.Text = "Read Names";
            this.ReadNames_Btn.UseVisualStyleBackColor = true;
            this.ReadNames_Btn.Click += new System.EventHandler(this.ReadNames_Btn_Click);
            // 
            // FilterNames_TB
            // 
            this.FilterNames_TB.Location = new System.Drawing.Point(204, 171);
            this.FilterNames_TB.Name = "FilterNames_TB";
            this.FilterNames_TB.Size = new System.Drawing.Size(303, 20);
            this.FilterNames_TB.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Filter Names:";
            // 
            // CheckConn_Btn
            // 
            this.CheckConn_Btn.Location = new System.Drawing.Point(16, 115);
            this.CheckConn_Btn.Name = "CheckConn_Btn";
            this.CheckConn_Btn.Size = new System.Drawing.Size(100, 23);
            this.CheckConn_Btn.TabIndex = 12;
            this.CheckConn_Btn.Text = "Check Connect";
            this.CheckConn_Btn.UseVisualStyleBackColor = true;
            this.CheckConn_Btn.Click += new System.EventHandler(this.CheckConn_Btn_Click);
            // 
            // ConnStatus_TB
            // 
            this.ConnStatus_TB.Location = new System.Drawing.Point(204, 115);
            this.ConnStatus_TB.Name = "ConnStatus_TB";
            this.ConnStatus_TB.Size = new System.Drawing.Size(39, 20);
            this.ConnStatus_TB.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Yes/No:";
            // 
            // GoTo_Btn
            // 
            this.GoTo_Btn.Location = new System.Drawing.Point(16, 202);
            this.GoTo_Btn.Name = "GoTo_Btn";
            this.GoTo_Btn.Size = new System.Drawing.Size(100, 23);
            this.GoTo_Btn.TabIndex = 15;
            this.GoTo_Btn.Text = "Go To Filter";
            this.GoTo_Btn.UseVisualStyleBackColor = true;
            this.GoTo_Btn.Click += new System.EventHandler(this.GoTo_Btn_Click);
            // 
            // GoToPos_CB
            // 
            this.GoToPos_CB.FormattingEnabled = true;
            this.GoToPos_CB.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.GoToPos_CB.Location = new System.Drawing.Point(123, 203);
            this.GoToPos_CB.Name = "GoToPos_CB";
            this.GoToPos_CB.Size = new System.Drawing.Size(36, 21);
            this.GoToPos_CB.TabIndex = 16;
            this.GoToPos_CB.Text = "1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(566, 481);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.BackOffset_TB);
            this.tabPage1.Controls.Add(this.NextOffset_TB);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.IFW3_RB);
            this.tabPage1.Controls.Add(this.IFW_RB);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.Connect_BTN);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.SaveData_Btn);
            this.tabPage1.Controls.Add(this.ComPort_Picker);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(558, 455);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Device Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BackOffset_TB
            // 
            this.BackOffset_TB.Location = new System.Drawing.Point(298, 415);
            this.BackOffset_TB.Name = "BackOffset_TB";
            this.BackOffset_TB.Size = new System.Drawing.Size(65, 20);
            this.BackOffset_TB.TabIndex = 41;
            // 
            // NextOffset_TB
            // 
            this.NextOffset_TB.Location = new System.Drawing.Point(120, 415);
            this.NextOffset_TB.Name = "NextOffset_TB";
            this.NextOffset_TB.Size = new System.Drawing.Size(65, 20);
            this.NextOffset_TB.TabIndex = 40;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(216, 418);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(76, 13);
            this.label20.TabIndex = 39;
            this.label20.Text = "\"Back\" Offset:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(41, 418);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 13);
            this.label19.TabIndex = 38;
            this.label19.Text = "\"Next\" Offset:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(17, 387);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(338, 20);
            this.label18.TabIndex = 37;
            this.label18.Text = "Configure the wheel centering constants:";
            // 
            // IFW3_RB
            // 
            this.IFW3_RB.AutoSize = true;
            this.IFW3_RB.Location = new System.Drawing.Point(459, 54);
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
            this.IFW_RB.Location = new System.Drawing.Point(378, 54);
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
            this.label17.Location = new System.Drawing.Point(17, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(319, 20);
            this.label17.TabIndex = 34;
            this.label17.Text = "Select which filter wheel you are using:";
            // 
            // Connect_BTN
            // 
            this.Connect_BTN.AllowDrop = true;
            this.Connect_BTN.Location = new System.Drawing.Point(369, 161);
            this.Connect_BTN.Name = "Connect_BTN";
            this.Connect_BTN.Size = new System.Drawing.Size(93, 93);
            this.Connect_BTN.TabIndex = 33;
            this.Connect_BTN.Text = "Connect";
            this.Connect_BTN.UseVisualStyleBackColor = true;
            this.Connect_BTN.Click += new System.EventHandler(this.Connect_BTN_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(266, 119);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 32;
            this.label16.Text = "Offset";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(185, 119);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(130, 346);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Filter 9:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(130, 320);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Filter 8:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(130, 294);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Filter 7:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(130, 268);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Filter 6:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(130, 242);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Filter 5:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(130, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Filter 4:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(130, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Filter 3:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(130, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Filter 2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(130, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Filter 1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(314, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Setup the filters for the current wheel:";
            // 
            // SaveData_Btn
            // 
            this.SaveData_Btn.Location = new System.Drawing.Point(472, 387);
            this.SaveData_Btn.Name = "SaveData_Btn";
            this.SaveData_Btn.Size = new System.Drawing.Size(80, 28);
            this.SaveData_Btn.TabIndex = 2;
            this.SaveData_Btn.Text = "Save Data";
            this.SaveData_Btn.UseVisualStyleBackColor = true;
            this.SaveData_Btn.Click += new System.EventHandler(this.SaveData_Btn_Click);
            // 
            // ComPort_Picker
            // 
            this.ComPort_Picker.Location = new System.Drawing.Point(378, 16);
            this.ComPort_Picker.Name = "ComPort_Picker";
            this.ComPort_Picker.Size = new System.Drawing.Size(42, 20);
            this.ComPort_Picker.TabIndex = 1;
            this.ComPort_Picker.ValueChanged += new System.EventHandler(this.ComPort_Picker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a COM port to use for this device:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Filter1Name_TB);
            this.panel1.Controls.Add(this.Filter2Name_TB);
            this.panel1.Controls.Add(this.Filter3Name_TB);
            this.panel1.Controls.Add(this.Filter4Name_TB);
            this.panel1.Controls.Add(this.Filter5Name_TB);
            this.panel1.Controls.Add(this.Filter6Name_TB);
            this.panel1.Controls.Add(this.Filter7Name_TB);
            this.panel1.Controls.Add(this.Filter8Name_TB);
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
            this.panel1.Location = new System.Drawing.Point(174, 127);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 250);
            this.panel1.TabIndex = 42;
            // 
            // Filter1Name_TB
            // 
            this.Filter1Name_TB.Enabled = false;
            this.Filter1Name_TB.Location = new System.Drawing.Point(3, 7);
            this.Filter1Name_TB.MaxLength = 8;
            this.Filter1Name_TB.Name = "Filter1Name_TB";
            this.Filter1Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter1Name_TB.TabIndex = 13;
            // 
            // Filter2Name_TB
            // 
            this.Filter2Name_TB.Enabled = false;
            this.Filter2Name_TB.Location = new System.Drawing.Point(3, 33);
            this.Filter2Name_TB.MaxLength = 8;
            this.Filter2Name_TB.Name = "Filter2Name_TB";
            this.Filter2Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter2Name_TB.TabIndex = 14;
            // 
            // Filter3Name_TB
            // 
            this.Filter3Name_TB.Enabled = false;
            this.Filter3Name_TB.Location = new System.Drawing.Point(3, 59);
            this.Filter3Name_TB.MaxLength = 8;
            this.Filter3Name_TB.Name = "Filter3Name_TB";
            this.Filter3Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter3Name_TB.TabIndex = 15;
            // 
            // Filter4Name_TB
            // 
            this.Filter4Name_TB.AcceptsReturn = true;
            this.Filter4Name_TB.Enabled = false;
            this.Filter4Name_TB.Location = new System.Drawing.Point(3, 85);
            this.Filter4Name_TB.MaxLength = 8;
            this.Filter4Name_TB.Name = "Filter4Name_TB";
            this.Filter4Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter4Name_TB.TabIndex = 16;
            // 
            // Filter5Name_TB
            // 
            this.Filter5Name_TB.Enabled = false;
            this.Filter5Name_TB.Location = new System.Drawing.Point(3, 111);
            this.Filter5Name_TB.MaxLength = 8;
            this.Filter5Name_TB.Name = "Filter5Name_TB";
            this.Filter5Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter5Name_TB.TabIndex = 17;
            // 
            // Filter6Name_TB
            // 
            this.Filter6Name_TB.Enabled = false;
            this.Filter6Name_TB.Location = new System.Drawing.Point(3, 137);
            this.Filter6Name_TB.MaxLength = 8;
            this.Filter6Name_TB.Name = "Filter6Name_TB";
            this.Filter6Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter6Name_TB.TabIndex = 18;
            // 
            // Filter7Name_TB
            // 
            this.Filter7Name_TB.Enabled = false;
            this.Filter7Name_TB.Location = new System.Drawing.Point(3, 163);
            this.Filter7Name_TB.MaxLength = 8;
            this.Filter7Name_TB.Name = "Filter7Name_TB";
            this.Filter7Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter7Name_TB.TabIndex = 19;
            // 
            // Filter8Name_TB
            // 
            this.Filter8Name_TB.Enabled = false;
            this.Filter8Name_TB.Location = new System.Drawing.Point(3, 189);
            this.Filter8Name_TB.MaxLength = 8;
            this.Filter8Name_TB.Name = "Filter8Name_TB";
            this.Filter8Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter8Name_TB.TabIndex = 20;
            // 
            // Filter9Name_TB
            // 
            this.Filter9Name_TB.Enabled = false;
            this.Filter9Name_TB.Location = new System.Drawing.Point(3, 215);
            this.Filter9Name_TB.MaxLength = 8;
            this.Filter9Name_TB.Name = "Filter9Name_TB";
            this.Filter9Name_TB.Size = new System.Drawing.Size(54, 20);
            this.Filter9Name_TB.TabIndex = 21;
            // 
            // F1Offset_TB
            // 
            this.F1Offset_TB.Enabled = false;
            this.F1Offset_TB.Location = new System.Drawing.Point(84, 7);
            this.F1Offset_TB.MaxLength = 8;
            this.F1Offset_TB.Name = "F1Offset_TB";
            this.F1Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F1Offset_TB.TabIndex = 22;
            // 
            // F2Offset_TB
            // 
            this.F2Offset_TB.Enabled = false;
            this.F2Offset_TB.Location = new System.Drawing.Point(84, 33);
            this.F2Offset_TB.MaxLength = 8;
            this.F2Offset_TB.Name = "F2Offset_TB";
            this.F2Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F2Offset_TB.TabIndex = 23;
            // 
            // F9Offset_TB
            // 
            this.F9Offset_TB.Enabled = false;
            this.F9Offset_TB.Location = new System.Drawing.Point(84, 215);
            this.F9Offset_TB.MaxLength = 8;
            this.F9Offset_TB.Name = "F9Offset_TB";
            this.F9Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F9Offset_TB.TabIndex = 30;
            // 
            // F3Offset_TB
            // 
            this.F3Offset_TB.Enabled = false;
            this.F3Offset_TB.Location = new System.Drawing.Point(84, 59);
            this.F3Offset_TB.MaxLength = 8;
            this.F3Offset_TB.Name = "F3Offset_TB";
            this.F3Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F3Offset_TB.TabIndex = 24;
            // 
            // F8Offset_TB
            // 
            this.F8Offset_TB.Enabled = false;
            this.F8Offset_TB.Location = new System.Drawing.Point(84, 189);
            this.F8Offset_TB.MaxLength = 8;
            this.F8Offset_TB.Name = "F8Offset_TB";
            this.F8Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F8Offset_TB.TabIndex = 29;
            // 
            // F4Offset_TB
            // 
            this.F4Offset_TB.Enabled = false;
            this.F4Offset_TB.Location = new System.Drawing.Point(84, 85);
            this.F4Offset_TB.MaxLength = 8;
            this.F4Offset_TB.Name = "F4Offset_TB";
            this.F4Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F4Offset_TB.TabIndex = 25;
            // 
            // F7Offset_TB
            // 
            this.F7Offset_TB.Enabled = false;
            this.F7Offset_TB.Location = new System.Drawing.Point(84, 163);
            this.F7Offset_TB.MaxLength = 8;
            this.F7Offset_TB.Name = "F7Offset_TB";
            this.F7Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F7Offset_TB.TabIndex = 28;
            // 
            // F5Offset_TB
            // 
            this.F5Offset_TB.Enabled = false;
            this.F5Offset_TB.Location = new System.Drawing.Point(84, 111);
            this.F5Offset_TB.MaxLength = 8;
            this.F5Offset_TB.Name = "F5Offset_TB";
            this.F5Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F5Offset_TB.TabIndex = 26;
            // 
            // F6Offset_TB
            // 
            this.F6Offset_TB.Enabled = false;
            this.F6Offset_TB.Location = new System.Drawing.Point(84, 137);
            this.F6Offset_TB.MaxLength = 8;
            this.F6Offset_TB.Name = "F6Offset_TB";
            this.F6Offset_TB.Size = new System.Drawing.Size(54, 20);
            this.F6Offset_TB.TabIndex = 27;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TestConnect_Btn);
            this.tabPage2.Controls.Add(this.GoToPos_CB);
            this.tabPage2.Controls.Add(this.GoTo_Btn);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.picASCOM);
            this.tabPage2.Controls.Add(this.ConnStatus_TB);
            this.tabPage2.Controls.Add(this.DissConnBtn);
            this.tabPage2.Controls.Add(this.CheckConn_Btn);
            this.tabPage2.Controls.Add(this.HomeBtn);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.FilterNames_TB);
            this.tabPage2.Controls.Add(this.WheelId_TB);
            this.tabPage2.Controls.Add(this.ReadNames_Btn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(558, 455);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Device Trouble Shooting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 492);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optec_IFW Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComPort_Picker)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Button TestConnect_Btn;
        private System.Windows.Forms.Button DissConnBtn;
        private System.Windows.Forms.Button HomeBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WheelId_TB;
        private System.Windows.Forms.Button ReadNames_Btn;
        private System.Windows.Forms.TextBox FilterNames_TB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CheckConn_Btn;
        private System.Windows.Forms.TextBox ConnStatus_TB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button GoTo_Btn;
        private System.Windows.Forms.ComboBox GoToPos_CB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button SaveData_Btn;
        private System.Windows.Forms.NumericUpDown ComPort_Picker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Filter9Name_TB;
        private System.Windows.Forms.TextBox Filter8Name_TB;
        private System.Windows.Forms.TextBox Filter7Name_TB;
        private System.Windows.Forms.TextBox Filter6Name_TB;
        private System.Windows.Forms.TextBox Filter5Name_TB;
        private System.Windows.Forms.TextBox Filter4Name_TB;
        private System.Windows.Forms.TextBox Filter3Name_TB;
        private System.Windows.Forms.TextBox Filter2Name_TB;
        private System.Windows.Forms.TextBox Filter1Name_TB;
        private System.Windows.Forms.TextBox F1Offset_TB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox F9Offset_TB;
        private System.Windows.Forms.TextBox F8Offset_TB;
        private System.Windows.Forms.TextBox F7Offset_TB;
        private System.Windows.Forms.TextBox F6Offset_TB;
        private System.Windows.Forms.TextBox F5Offset_TB;
        private System.Windows.Forms.TextBox F4Offset_TB;
        private System.Windows.Forms.TextBox F3Offset_TB;
        private System.Windows.Forms.TextBox F2Offset_TB;
        private System.Windows.Forms.Button Connect_BTN;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton IFW3_RB;
        private System.Windows.Forms.RadioButton IFW_RB;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox BackOffset_TB;
        private System.Windows.Forms.TextBox NextOffset_TB;
        private System.Windows.Forms.Panel panel1;
    }
}