namespace ASCOM.GeminiTelescope
{
    partial class frmUserCatalog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserCatalog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbCatalogs = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvAllObjects = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvGeminiCatalog = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbSendtObject = new System.Windows.Forms.Button();
            this.pbAddAll = new System.Windows.Forms.Button();
            this.pbAdd = new System.Windows.Forms.Button();
            this.btnGoto = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnAddAlign = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbToFile = new System.Windows.Forms.Button();
            this.pbFromFile = new System.Windows.Forms.Button();
            this.pbClear = new System.Windows.Forms.Button();
            this.pbFromGemini = new System.Windows.Forms.Button();
            this.pbDelete = new System.Windows.Forms.Button();
            this.pbToGemini = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pbUnselAll = new System.Windows.Forms.Button();
            this.pbSelAll = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pbClearSearch = new System.Windows.Forms.Button();
            this.textSearch = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAllObjects)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvGeminiCatalog)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 311F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(681, 664);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbCatalogs);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 159);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Catalogs";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lbCatalogs
            // 
            this.lbCatalogs.CheckOnClick = true;
            this.lbCatalogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCatalogs.FormattingEnabled = true;
            this.lbCatalogs.Location = new System.Drawing.Point(3, 16);
            this.lbCatalogs.Name = "lbCatalogs";
            this.lbCatalogs.Size = new System.Drawing.Size(263, 139);
            this.lbCatalogs.Sorted = true;
            this.lbCatalogs.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pbExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(586, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(95, 165);
            this.panel1.TabIndex = 0;
            // 
            // pbExit
            // 
            this.pbExit.Location = new System.Drawing.Point(6, 19);
            this.pbExit.Name = "pbExit";
            this.pbExit.Size = new System.Drawing.Size(75, 23);
            this.pbExit.TabIndex = 5;
            this.pbExit.Text = "Exit";
            this.pbExit.UseVisualStyleBackColor = true;
            this.pbExit.Click += new System.EventHandler(this.pbExit_Click);
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.gvAllObjects);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 243);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Objects in selected Catalogs";
            // 
            // gvAllObjects
            // 
            this.gvAllObjects.AllowUserToAddRows = false;
            this.gvAllObjects.AllowUserToDeleteRows = false;
            this.gvAllObjects.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvAllObjects.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvAllObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvAllObjects.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gvAllObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAllObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAllObjects.Location = new System.Drawing.Point(3, 16);
            this.gvAllObjects.Name = "gvAllObjects";
            this.gvAllObjects.ReadOnly = true;
            this.gvAllObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAllObjects.ShowCellErrors = false;
            this.gvAllObjects.ShowEditingIcon = false;
            this.gvAllObjects.ShowRowErrors = false;
            this.gvAllObjects.Size = new System.Drawing.Size(574, 224);
            this.gvAllObjects.TabIndex = 0;
            this.gvAllObjects.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvAllObjects_ColumnHeaderMouseClick);
            this.gvAllObjects.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvAllObjects_CellMouseDoubleClick);
            this.gvAllObjects.SelectionChanged += new System.EventHandler(this.gvAllObjects_SelectionChanged);
            this.gvAllObjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAllObjects_CellContentClick);
            // 
            // groupBox3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.gvGeminiCatalog);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 417);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(580, 244);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Objects in Gemini User Calatog";
            // 
            // gvGeminiCatalog
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvGeminiCatalog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gvGeminiCatalog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvGeminiCatalog.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gvGeminiCatalog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvGeminiCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvGeminiCatalog.Location = new System.Drawing.Point(3, 16);
            this.gvGeminiCatalog.Name = "gvGeminiCatalog";
            this.gvGeminiCatalog.ReadOnly = true;
            this.gvGeminiCatalog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvGeminiCatalog.Size = new System.Drawing.Size(574, 225);
            this.gvGeminiCatalog.TabIndex = 1;
            this.gvGeminiCatalog.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvGeminiCatalog_ColumnHeaderMouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbSendtObject);
            this.panel2.Controls.Add(this.pbAddAll);
            this.panel2.Controls.Add(this.pbAdd);
            this.panel2.Controls.Add(this.btnGoto);
            this.panel2.Controls.Add(this.btnSync);
            this.panel2.Controls.Add(this.btnAddAlign);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(589, 168);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(89, 243);
            this.panel2.TabIndex = 1;
            // 
            // pbSendtObject
            // 
            this.pbSendtObject.Location = new System.Drawing.Point(6, 127);
            this.pbSendtObject.Name = "pbSendtObject";
            this.pbSendtObject.Size = new System.Drawing.Size(75, 23);
            this.pbSendtObject.TabIndex = 6;
            this.pbSendtObject.Text = "Set Object";
            this.pbSendtObject.UseVisualStyleBackColor = true;
            this.pbSendtObject.Click += new System.EventHandler(this.pbSendtObject_Click);
            // 
            // pbAddAll
            // 
            this.pbAddAll.Location = new System.Drawing.Point(6, 45);
            this.pbAddAll.Name = "pbAddAll";
            this.pbAddAll.Size = new System.Drawing.Size(75, 23);
            this.pbAddAll.TabIndex = 5;
            this.pbAddAll.Text = "Add All";
            this.pbAddAll.UseVisualStyleBackColor = true;
            this.pbAddAll.Visible = false;
            // 
            // pbAdd
            // 
            this.pbAdd.Location = new System.Drawing.Point(6, 16);
            this.pbAdd.Name = "pbAdd";
            this.pbAdd.Size = new System.Drawing.Size(75, 23);
            this.pbAdd.TabIndex = 4;
            this.pbAdd.Text = "Add";
            this.pbAdd.UseVisualStyleBackColor = true;
            this.pbAdd.Click += new System.EventHandler(this.pbAdd_Click);
            // 
            // btnGoto
            // 
            this.btnGoto.Location = new System.Drawing.Point(6, 98);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(75, 23);
            this.btnGoto.TabIndex = 1;
            this.btnGoto.Text = "Go To";
            this.btnGoto.UseVisualStyleBackColor = true;
            this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(6, 189);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(75, 23);
            this.btnSync.TabIndex = 2;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // btnAddAlign
            // 
            this.btnAddAlign.Location = new System.Drawing.Point(6, 218);
            this.btnAddAlign.Name = "btnAddAlign";
            this.btnAddAlign.Size = new System.Drawing.Size(75, 23);
            this.btnAddAlign.TabIndex = 3;
            this.btnAddAlign.Text = "Add\'l Align";
            this.btnAddAlign.UseVisualStyleBackColor = true;
            this.btnAddAlign.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pbToFile);
            this.panel3.Controls.Add(this.pbFromFile);
            this.panel3.Controls.Add(this.pbClear);
            this.panel3.Controls.Add(this.pbFromGemini);
            this.panel3.Controls.Add(this.pbDelete);
            this.panel3.Controls.Add(this.pbToGemini);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(589, 417);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(89, 244);
            this.panel3.TabIndex = 2;
            // 
            // pbToFile
            // 
            this.pbToFile.Location = new System.Drawing.Point(6, 140);
            this.pbToFile.Name = "pbToFile";
            this.pbToFile.Size = new System.Drawing.Size(75, 23);
            this.pbToFile.TabIndex = 10;
            this.pbToFile.Text = "To File...";
            this.pbToFile.UseVisualStyleBackColor = true;
            this.pbToFile.Click += new System.EventHandler(this.pbToFile_Click);
            // 
            // pbFromFile
            // 
            this.pbFromFile.Location = new System.Drawing.Point(6, 111);
            this.pbFromFile.Name = "pbFromFile";
            this.pbFromFile.Size = new System.Drawing.Size(75, 23);
            this.pbFromFile.TabIndex = 9;
            this.pbFromFile.Text = "From File...";
            this.pbFromFile.UseVisualStyleBackColor = true;
            this.pbFromFile.Click += new System.EventHandler(this.pbFromFile_Click);
            // 
            // pbClear
            // 
            this.pbClear.Location = new System.Drawing.Point(6, 215);
            this.pbClear.Name = "pbClear";
            this.pbClear.Size = new System.Drawing.Size(75, 23);
            this.pbClear.TabIndex = 8;
            this.pbClear.Text = "Remove All";
            this.pbClear.UseVisualStyleBackColor = true;
            this.pbClear.Click += new System.EventHandler(this.pbClear_Click);
            // 
            // pbFromGemini
            // 
            this.pbFromGemini.Location = new System.Drawing.Point(6, 16);
            this.pbFromGemini.Name = "pbFromGemini";
            this.pbFromGemini.Size = new System.Drawing.Size(75, 23);
            this.pbFromGemini.TabIndex = 6;
            this.pbFromGemini.Text = "From Gemini";
            this.pbFromGemini.UseVisualStyleBackColor = true;
            this.pbFromGemini.Click += new System.EventHandler(this.pbFromGemini_Click);
            // 
            // pbDelete
            // 
            this.pbDelete.Location = new System.Drawing.Point(6, 186);
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.Size = new System.Drawing.Size(75, 23);
            this.pbDelete.TabIndex = 5;
            this.pbDelete.Text = "Remove";
            this.pbDelete.UseVisualStyleBackColor = true;
            this.pbDelete.Click += new System.EventHandler(this.pbDelete_Click);
            // 
            // pbToGemini
            // 
            this.pbToGemini.Location = new System.Drawing.Point(6, 45);
            this.pbToGemini.Name = "pbToGemini";
            this.pbToGemini.Size = new System.Drawing.Size(75, 23);
            this.pbToGemini.TabIndex = 7;
            this.pbToGemini.Text = "To Gemini";
            this.pbToGemini.UseVisualStyleBackColor = true;
            this.pbToGemini.Click += new System.EventHandler(this.pbToGemini_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pbUnselAll);
            this.panel4.Controls.Add(this.pbSelAll);
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(278, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(305, 159);
            this.panel4.TabIndex = 3;
            // 
            // pbUnselAll
            // 
            this.pbUnselAll.Location = new System.Drawing.Point(3, 45);
            this.pbUnselAll.Name = "pbUnselAll";
            this.pbUnselAll.Size = new System.Drawing.Size(75, 23);
            this.pbUnselAll.TabIndex = 7;
            this.pbUnselAll.Text = "Unselect All";
            this.pbUnselAll.UseVisualStyleBackColor = true;
            this.pbUnselAll.Click += new System.EventHandler(this.pbUnselAll_Click);
            // 
            // pbSelAll
            // 
            this.pbSelAll.Location = new System.Drawing.Point(3, 16);
            this.pbSelAll.Name = "pbSelAll";
            this.pbSelAll.Size = new System.Drawing.Size(75, 23);
            this.pbSelAll.TabIndex = 6;
            this.pbSelAll.Text = "Select All";
            this.pbSelAll.UseVisualStyleBackColor = true;
            this.pbSelAll.Click += new System.EventHandler(this.pbSelAll_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.pbClearSearch);
            this.groupBox4.Controls.Add(this.textSearch);
            this.groupBox4.Location = new System.Drawing.Point(3, 104);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(299, 51);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Search";
            // 
            // pbClearSearch
            // 
            this.pbClearSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbClearSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbClearSearch.Location = new System.Drawing.Point(261, 19);
            this.pbClearSearch.Name = "pbClearSearch";
            this.pbClearSearch.Size = new System.Drawing.Size(25, 20);
            this.pbClearSearch.TabIndex = 1;
            this.pbClearSearch.Text = "X";
            this.pbClearSearch.UseVisualStyleBackColor = true;
            this.pbClearSearch.Click += new System.EventHandler(this.pbClearSearch_Click);
            // 
            // textSearch
            // 
            this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textSearch.Location = new System.Drawing.Point(6, 19);
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(255, 20);
            this.textSearch.TabIndex = 0;
            this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "guc";
            this.saveFileDialog1.FileName = "User Objects";
            this.saveFileDialog1.Filter = "Gemini Catalog Files|*.guc|All Files|*.*";
            this.saveFileDialog1.Title = "Save Catalog in a File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "guc";
            this.openFileDialog1.Filter = "Gemini Catalog Files|*.guc|All Files|*.*";
            this.openFileDialog1.Title = "Load Catalog from a File";
            // 
            // frmUserCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 664);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUserCatalog";
            this.Text = "Object Catalog Manager";
            this.Load += new System.EventHandler(this.frmUserCatalog_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUserCatalog_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAllObjects)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvGeminiCatalog)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lbCatalogs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnGoto;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnAddAlign;
        private System.Windows.Forms.Button pbAdd;
        private System.Windows.Forms.Button pbDelete;
        private System.Windows.Forms.Button pbFromGemini;
        private System.Windows.Forms.Button pbToGemini;
        private System.Windows.Forms.Button pbClear;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView gvAllObjects;
        private System.Windows.Forms.DataGridView gvGeminiCatalog;
        private System.Windows.Forms.Button pbExit;
        private System.Windows.Forms.Button pbAddAll;
        private System.Windows.Forms.Button pbToFile;
        private System.Windows.Forms.Button pbFromFile;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button pbUnselAll;
        private System.Windows.Forms.Button pbSelAll;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button pbClearSearch;
        private System.Windows.Forms.TextBox textSearch;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button pbSendtObject;
    }
}