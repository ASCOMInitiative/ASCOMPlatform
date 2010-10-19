namespace ASCOM.GeminiTelescope
{
    partial class frmOpticsSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpticsSetup));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.listBoxOptics = new System.Windows.Forms.ListBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxFocalLength = new System.Windows.Forms.TextBox();
            this.textBoxAperture = new System.Windows.Forms.TextBox();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.radioButtonmillimeters = new System.Windows.Forms.RadioButton();
            this.radioButtonInches = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Black;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Location = new System.Drawing.Point(207, 208);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Black;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Location = new System.Drawing.Point(207, 177);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            // 
            // listBoxOptics
            // 
            this.listBoxOptics.FormattingEnabled = true;
            this.listBoxOptics.Location = new System.Drawing.Point(12, 138);
            this.listBoxOptics.Name = "listBoxOptics";
            this.listBoxOptics.Size = new System.Drawing.Size(189, 95);
            this.listBoxOptics.TabIndex = 7;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(81, 19);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // textBoxFocalLength
            // 
            this.textBoxFocalLength.Location = new System.Drawing.Point(81, 45);
            this.textBoxFocalLength.Name = "textBoxFocalLength";
            this.textBoxFocalLength.Size = new System.Drawing.Size(100, 20);
            this.textBoxFocalLength.TabIndex = 9;
            this.textBoxFocalLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFocalLength_KeyPress);
            // 
            // textBoxAperture
            // 
            this.textBoxAperture.Location = new System.Drawing.Point(81, 71);
            this.textBoxAperture.Name = "textBoxAperture";
            this.textBoxAperture.Size = new System.Drawing.Size(100, 20);
            this.textBoxAperture.TabIndex = 10;
            this.textBoxAperture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAperture_KeyPress);
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.BackColor = System.Drawing.Color.Transparent;
            this.labelLatitude.ForeColor = System.Drawing.Color.White;
            this.labelLatitude.Location = new System.Drawing.Point(6, 22);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(38, 13);
            this.labelLatitude.TabIndex = 11;
            this.labelLatitude.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Focal Length:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Aperture:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.Black;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.ForeColor = System.Drawing.Color.White;
            this.buttonAdd.Location = new System.Drawing.Point(207, 74);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(59, 25);
            this.buttonAdd.TabIndex = 28;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.BackColor = System.Drawing.Color.Black;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.ForeColor = System.Drawing.Color.White;
            this.buttonRemove.Location = new System.Drawing.Point(207, 105);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(59, 25);
            this.buttonRemove.TabIndex = 29;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // radioButtonmillimeters
            // 
            this.radioButtonmillimeters.AutoSize = true;
            this.radioButtonmillimeters.Checked = true;
            this.radioButtonmillimeters.ForeColor = System.Drawing.Color.White;
            this.radioButtonmillimeters.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButtonmillimeters.Location = new System.Drawing.Point(81, 97);
            this.radioButtonmillimeters.Name = "radioButtonmillimeters";
            this.radioButtonmillimeters.Size = new System.Drawing.Size(72, 17);
            this.radioButtonmillimeters.TabIndex = 31;
            this.radioButtonmillimeters.TabStop = true;
            this.radioButtonmillimeters.Text = "millimeters";
            this.radioButtonmillimeters.UseVisualStyleBackColor = true;
            // 
            // radioButtonInches
            // 
            this.radioButtonInches.AutoSize = true;
            this.radioButtonInches.ForeColor = System.Drawing.Color.White;
            this.radioButtonInches.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButtonInches.Location = new System.Drawing.Point(9, 97);
            this.radioButtonInches.Name = "radioButtonInches";
            this.radioButtonInches.Size = new System.Drawing.Size(56, 17);
            this.radioButtonInches.TabIndex = 30;
            this.radioButtonInches.Text = "inches";
            this.radioButtonInches.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.radioButtonInches);
            this.groupBox1.Controls.Add(this.radioButtonmillimeters);
            this.groupBox1.Controls.Add(this.textBoxFocalLength);
            this.groupBox1.Controls.Add(this.textBoxAperture);
            this.groupBox1.Controls.Add(this.labelLatitude);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 122);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optics";
            // 
            // frmOpticsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(274, 240);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxOptics);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpticsSetup";
            this.Text = "Optics";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ListBox listBoxOptics;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxFocalLength;
        private System.Windows.Forms.TextBox textBoxAperture;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.RadioButton radioButtonmillimeters;
        private System.Windows.Forms.RadioButton radioButtonInches;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}