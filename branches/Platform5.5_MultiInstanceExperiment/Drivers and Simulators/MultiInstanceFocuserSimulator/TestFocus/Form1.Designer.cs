namespace TestFocus
{
    partial class FormFocuser
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonIn = new System.Windows.Forms.Button();
            this.buttonOut = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.buttonSetPosition = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.comboBoxSteps = new System.Windows.Forms.ComboBox();
            this.comboBoxPositions = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxTarget = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDriverName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(95, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 21);
            this.button2.TabIndex = 5;
            this.button2.Text = "Connect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = "Choose";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Position";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Steps";
            // 
            // buttonIn
            // 
            this.buttonIn.Location = new System.Drawing.Point(8, 110);
            this.buttonIn.Name = "buttonIn";
            this.buttonIn.Size = new System.Drawing.Size(39, 21);
            this.buttonIn.TabIndex = 12;
            this.buttonIn.Text = "In";
            this.buttonIn.UseVisualStyleBackColor = true;
            this.buttonIn.Click += new System.EventHandler(this.buttonIn_Click);
            // 
            // buttonOut
            // 
            this.buttonOut.Location = new System.Drawing.Point(133, 110);
            this.buttonOut.Name = "buttonOut";
            this.buttonOut.Size = new System.Drawing.Size(39, 21);
            this.buttonOut.TabIndex = 13;
            this.buttonOut.Text = "Out";
            this.buttonOut.UseVisualStyleBackColor = true;
            this.buttonOut.Click += new System.EventHandler(this.buttonOut_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(56, 68);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.Size = new System.Drawing.Size(77, 20);
            this.textBoxPosition.TabIndex = 15;
            this.textBoxPosition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPosition_KeyPress);
            // 
            // buttonSetPosition
            // 
            this.buttonSetPosition.Location = new System.Drawing.Point(49, 171);
            this.buttonSetPosition.Name = "buttonSetPosition";
            this.buttonSetPosition.Size = new System.Drawing.Size(39, 21);
            this.buttonSetPosition.TabIndex = 16;
            this.buttonSetPosition.Text = "Set";
            this.buttonSetPosition.UseVisualStyleBackColor = true;
            this.buttonSetPosition.Click += new System.EventHandler(this.buttonSetPosition_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(94, 171);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(39, 21);
            this.buttonStop.TabIndex = 17;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // comboBoxSteps
            // 
            this.comboBoxSteps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSteps.FormattingEnabled = true;
            this.comboBoxSteps.Items.AddRange(new object[] {
            "1",
            "2",
            "5",
            "10",
            "20",
            "50",
            "100",
            "200",
            "500",
            "1000",
            "2000"});
            this.comboBoxSteps.Location = new System.Drawing.Point(56, 110);
            this.comboBoxSteps.Name = "comboBoxSteps";
            this.comboBoxSteps.Size = new System.Drawing.Size(71, 21);
            this.comboBoxSteps.TabIndex = 19;
            // 
            // comboBoxPositions
            // 
            this.comboBoxPositions.FormattingEnabled = true;
            this.comboBoxPositions.Location = new System.Drawing.Point(8, 202);
            this.comboBoxPositions.Name = "comboBoxPositions";
            this.comboBoxPositions.Size = new System.Drawing.Size(163, 21);
            this.comboBoxPositions.TabIndex = 20;
            this.comboBoxPositions.SelectedValueChanged += new System.EventHandler(this.comboBoxPositions_SelectedValueChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(8, 229);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(50, 21);
            this.buttonAdd.TabIndex = 21;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(115, 229);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(57, 21);
            this.buttonRemove.TabIndex = 22;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.Location = new System.Drawing.Point(49, 145);
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(84, 20);
            this.textBoxTarget.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Target";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Name";
            // 
            // textBoxDriverName
            // 
            this.textBoxDriverName.Location = new System.Drawing.Point(54, 39);
            this.textBoxDriverName.Name = "textBoxDriverName";
            this.textBoxDriverName.Size = new System.Drawing.Size(116, 20);
            this.textBoxDriverName.TabIndex = 26;
            // 
            // FormFocuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 262);
            this.Controls.Add(this.textBoxDriverName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.comboBoxPositions);
            this.Controls.Add(this.comboBoxSteps);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonSetPosition);
            this.Controls.Add(this.textBoxPosition);
            this.Controls.Add(this.buttonOut);
            this.Controls.Add(this.buttonIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "FormFocuser";
            this.Text = "Focuser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonIn;
        private System.Windows.Forms.Button buttonOut;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Button buttonSetPosition;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ComboBox comboBoxSteps;
        private System.Windows.Forms.ComboBox comboBoxPositions;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TextBox textBoxTarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDriverName;
    }
}

