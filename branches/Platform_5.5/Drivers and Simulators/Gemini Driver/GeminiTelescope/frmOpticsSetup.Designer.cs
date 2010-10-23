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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxObstruction = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Black;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.White;
            this.cmdCancel.Location = new System.Drawing.Point(207, 254);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 18;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Black;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdOK.ForeColor = System.Drawing.Color.White;
            this.cmdOK.Location = new System.Drawing.Point(207, 224);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 17;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            // 
            // listBoxOptics
            // 
            this.listBoxOptics.FormattingEnabled = true;
            this.listBoxOptics.Location = new System.Drawing.Point(12, 184);
            this.listBoxOptics.Name = "listBoxOptics";
            this.listBoxOptics.Size = new System.Drawing.Size(189, 95);
            this.listBoxOptics.TabIndex = 16;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(94, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(86, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // textBoxFocalLength
            // 
            this.textBoxFocalLength.Location = new System.Drawing.Point(94, 33);
            this.textBoxFocalLength.Name = "textBoxFocalLength";
            this.textBoxFocalLength.Size = new System.Drawing.Size(86, 20);
            this.textBoxFocalLength.TabIndex = 9;
            this.textBoxFocalLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFocalLength_KeyPress);
            // 
            // textBoxAperture
            // 
            this.textBoxAperture.Location = new System.Drawing.Point(94, 63);
            this.textBoxAperture.Name = "textBoxAperture";
            this.textBoxAperture.Size = new System.Drawing.Size(86, 20);
            this.textBoxAperture.TabIndex = 10;
            this.textBoxAperture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAperture_KeyPress);
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.BackColor = System.Drawing.Color.Transparent;
            this.labelLatitude.ForeColor = System.Drawing.Color.White;
            this.labelLatitude.Location = new System.Drawing.Point(3, 0);
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
            this.label1.Location = new System.Drawing.Point(3, 30);
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
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 26);
            this.label2.TabIndex = 13;
            this.label2.Text = "Aperture\r\nDiameter:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.Black;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.ForeColor = System.Drawing.Color.White;
            this.buttonAdd.Location = new System.Drawing.Point(207, 122);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(59, 25);
            this.buttonAdd.TabIndex = 14;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.BackColor = System.Drawing.Color.Black;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.ForeColor = System.Drawing.Color.White;
            this.buttonRemove.Location = new System.Drawing.Point(207, 153);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(59, 25);
            this.buttonRemove.TabIndex = 15;
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
            this.radioButtonmillimeters.Location = new System.Drawing.Point(94, 123);
            this.radioButtonmillimeters.Name = "radioButtonmillimeters";
            this.radioButtonmillimeters.Size = new System.Drawing.Size(72, 17);
            this.radioButtonmillimeters.TabIndex = 13;
            this.radioButtonmillimeters.TabStop = true;
            this.radioButtonmillimeters.Text = "millimeters";
            this.radioButtonmillimeters.UseVisualStyleBackColor = true;
            // 
            // radioButtonInches
            // 
            this.radioButtonInches.AutoSize = true;
            this.radioButtonInches.ForeColor = System.Drawing.Color.White;
            this.radioButtonInches.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButtonInches.Location = new System.Drawing.Point(3, 123);
            this.radioButtonInches.Name = "radioButtonInches";
            this.radioButtonInches.Size = new System.Drawing.Size(56, 17);
            this.radioButtonInches.TabIndex = 12;
            this.radioButtonInches.Text = "inches";
            this.radioButtonInches.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 170);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optics";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxObstruction, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAperture, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonInches, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonmillimeters, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFocalLength, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLatitude, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(183, 151);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxObstruction
            // 
            this.textBoxObstruction.Location = new System.Drawing.Point(94, 93);
            this.textBoxObstruction.Name = "textBoxObstruction";
            this.textBoxObstruction.Size = new System.Drawing.Size(86, 20);
            this.textBoxObstruction.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 26);
            this.label3.TabIndex = 33;
            this.label3.Text = "Obstruction\r\nDiameter:";
            // 
            // frmOpticsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(277, 291);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxObstruction;
        private System.Windows.Forms.Label label3;
    }
}