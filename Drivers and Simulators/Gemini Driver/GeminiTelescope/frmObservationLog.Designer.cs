namespace ASCOM.GeminiTelescope
{
    partial class frmObservationLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvLog = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbFromGemini = new System.Windows.Forms.Button();
            this.pbToGemini = new System.Windows.Forms.Button();
            this.pbToFile = new System.Windows.Forms.Button();
            this.pbFromFile = new System.Windows.Forms.Button();
            this.pbExit = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.08943F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.91057F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(714, 439);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.groupBox2.Controls.Add(this.gvLog);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(10, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(10, 15, 3, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(580, 414);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Observation Log Entries";
            // 
            // gvLog
            // 
            this.gvLog.AllowUserToAddRows = false;
            this.gvLog.AllowUserToDeleteRows = false;
            this.gvLog.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            this.gvLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.gvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvLog.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.gvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvLog.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvLog.Location = new System.Drawing.Point(10, 23);
            this.gvLog.Name = "gvLog";
            this.gvLog.ReadOnly = true;
            this.gvLog.RowHeadersVisible = false;
            this.gvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvLog.ShowCellErrors = false;
            this.gvLog.ShowEditingIcon = false;
            this.gvLog.ShowRowErrors = false;
            this.gvLog.Size = new System.Drawing.Size(560, 381);
            this.gvLog.TabIndex = 0;
            this.gvLog.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvLog_ColumnHeaderMouseClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.panel1.Controls.Add(this.pbFromGemini);
            this.panel1.Controls.Add(this.pbToGemini);
            this.panel1.Controls.Add(this.pbToFile);
            this.panel1.Controls.Add(this.pbFromFile);
            this.panel1.Controls.Add(this.pbExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(596, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(115, 433);
            this.panel1.TabIndex = 2;
            // 
            // pbFromGemini
            // 
            this.pbFromGemini.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbFromGemini.ForeColor = System.Drawing.Color.White;
            this.pbFromGemini.Location = new System.Drawing.Point(12, 23);
            this.pbFromGemini.Name = "pbFromGemini";
            this.pbFromGemini.Size = new System.Drawing.Size(75, 23);
            this.pbFromGemini.TabIndex = 13;
            this.pbFromGemini.Text = "From Gemini";
            this.pbFromGemini.UseVisualStyleBackColor = true;
            this.pbFromGemini.Click += new System.EventHandler(this.pbFromGemini_Click);
            // 
            // pbToGemini
            // 
            this.pbToGemini.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbToGemini.ForeColor = System.Drawing.Color.White;
            this.pbToGemini.Location = new System.Drawing.Point(12, 52);
            this.pbToGemini.Name = "pbToGemini";
            this.pbToGemini.Size = new System.Drawing.Size(75, 23);
            this.pbToGemini.TabIndex = 14;
            this.pbToGemini.Text = "Clear Gemini";
            this.pbToGemini.UseVisualStyleBackColor = true;
            this.pbToGemini.Click += new System.EventHandler(this.pbToGemini_Click);
            // 
            // pbToFile
            // 
            this.pbToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbToFile.ForeColor = System.Drawing.Color.White;
            this.pbToFile.Location = new System.Drawing.Point(12, 111);
            this.pbToFile.Name = "pbToFile";
            this.pbToFile.Size = new System.Drawing.Size(75, 23);
            this.pbToFile.TabIndex = 12;
            this.pbToFile.Text = "To File...";
            this.pbToFile.UseVisualStyleBackColor = true;
            this.pbToFile.Click += new System.EventHandler(this.pbToFile_Click);
            // 
            // pbFromFile
            // 
            this.pbFromFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbFromFile.ForeColor = System.Drawing.Color.White;
            this.pbFromFile.Location = new System.Drawing.Point(12, 139);
            this.pbFromFile.Name = "pbFromFile";
            this.pbFromFile.Size = new System.Drawing.Size(75, 23);
            this.pbFromFile.TabIndex = 11;
            this.pbFromFile.Text = "From File...";
            this.pbFromFile.UseVisualStyleBackColor = true;
            this.pbFromFile.Visible = false;
            // 
            // pbExit
            // 
            this.pbExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbExit.ForeColor = System.Drawing.Color.White;
            this.pbExit.Location = new System.Drawing.Point(12, 176);
            this.pbExit.Name = "pbExit";
            this.pbExit.Size = new System.Drawing.Size(75, 23);
            this.pbExit.TabIndex = 7;
            this.pbExit.Text = "Exit";
            this.pbExit.UseVisualStyleBackColor = true;
            this.pbExit.Click += new System.EventHandler(this.pbExit_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.Filter = "Text Files|*.txt";
            this.saveFileDialog1.Title = "Save Gemini Observation Log";
            // 
            // frmObservationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(714, 439);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmObservationLog";
            this.Text = "Gemini Observation Log";
            this.Load += new System.EventHandler(this.frmObservationLog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView gvLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button pbExit;
        private System.Windows.Forms.Button pbToFile;
        private System.Windows.Forms.Button pbFromFile;
        private System.Windows.Forms.Button pbFromGemini;
        private System.Windows.Forms.Button pbToGemini;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}