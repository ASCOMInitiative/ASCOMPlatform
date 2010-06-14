namespace ASCOM.GeminiTelescope
{
    partial class frmRA_DEC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRA_DEC));
            this.label1 = new System.Windows.Forms.Label();
            this.btnGoto = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRA = new System.Windows.Forms.MaskedTextBox();
            this.txtDEC = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtObject = new System.Windows.Forms.ComboBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnAddAlign = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkJ2000 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbCatalog = new System.Windows.Forms.Label();
            this.pbCatalog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // btnGoto
            // 
            this.btnGoto.AccessibleDescription = null;
            this.btnGoto.AccessibleName = null;
            resources.ApplyResources(this.btnGoto, "btnGoto");
            this.btnGoto.BackgroundImage = null;
            this.btnGoto.Font = null;
            this.btnGoto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.UseVisualStyleBackColor = true;
            this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // txtRA
            // 
            this.txtRA.AccessibleDescription = null;
            this.txtRA.AccessibleName = null;
            resources.ApplyResources(this.txtRA, "txtRA");
            this.txtRA.BackColor = System.Drawing.Color.Black;
            this.txtRA.BackgroundImage = null;
            this.txtRA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRA.ForeColor = System.Drawing.Color.White;
            this.txtRA.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.txtRA.Name = "txtRA";
            this.txtRA.Enter += new System.EventHandler(this.txtRA_Enter);
            // 
            // txtDEC
            // 
            this.txtDEC.AccessibleDescription = null;
            this.txtDEC.AccessibleName = null;
            resources.ApplyResources(this.txtDEC, "txtDEC");
            this.txtDEC.BackColor = System.Drawing.Color.Black;
            this.txtDEC.BackgroundImage = null;
            this.txtDEC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDEC.ForeColor = System.Drawing.Color.White;
            this.txtDEC.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.txtDEC.Name = "txtDEC";
            this.txtDEC.Enter += new System.EventHandler(this.txtDEC_Enter);
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // txtObject
            // 
            this.txtObject.AccessibleDescription = null;
            this.txtObject.AccessibleName = null;
            resources.ApplyResources(this.txtObject, "txtObject");
            this.txtObject.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txtObject.BackColor = System.Drawing.Color.Black;
            this.txtObject.BackgroundImage = null;
            this.txtObject.Font = null;
            this.txtObject.ForeColor = System.Drawing.Color.White;
            this.txtObject.FormattingEnabled = true;
            this.txtObject.Name = "txtObject";
            this.txtObject.SelectedIndexChanged += new System.EventHandler(this.txtObject_SelectedIndexChanged);
            this.txtObject.TextChanged += new System.EventHandler(this.txtObject_TextChanged);
            // 
            // btnSync
            // 
            this.btnSync.AccessibleDescription = null;
            this.btnSync.AccessibleName = null;
            resources.ApplyResources(this.btnSync, "btnSync");
            this.btnSync.BackgroundImage = null;
            this.btnSync.Font = null;
            this.btnSync.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSync.Name = "btnSync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // btnAddAlign
            // 
            this.btnAddAlign.AccessibleDescription = null;
            this.btnAddAlign.AccessibleName = null;
            resources.ApplyResources(this.btnAddAlign, "btnAddAlign");
            this.btnAddAlign.BackgroundImage = null;
            this.btnAddAlign.Font = null;
            this.btnAddAlign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddAlign.Name = "btnAddAlign";
            this.btnAddAlign.UseVisualStyleBackColor = true;
            this.btnAddAlign.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleDescription = null;
            this.btnExit.AccessibleName = null;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.BackgroundImage = null;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = null;
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkJ2000
            // 
            this.chkJ2000.AccessibleDescription = null;
            this.chkJ2000.AccessibleName = null;
            resources.ApplyResources(this.chkJ2000, "chkJ2000");
            this.chkJ2000.BackgroundImage = null;
            this.chkJ2000.Font = null;
            this.chkJ2000.ForeColor = System.Drawing.Color.White;
            this.chkJ2000.Name = "chkJ2000";
            this.chkJ2000.UseVisualStyleBackColor = true;
            this.chkJ2000.CheckedChanged += new System.EventHandler(this.chkJ2000_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // lbCatalog
            // 
            this.lbCatalog.AccessibleDescription = null;
            this.lbCatalog.AccessibleName = null;
            resources.ApplyResources(this.lbCatalog, "lbCatalog");
            this.lbCatalog.Font = null;
            this.lbCatalog.Name = "lbCatalog";
            // 
            // pbCatalog
            // 
            this.pbCatalog.AccessibleDescription = null;
            this.pbCatalog.AccessibleName = null;
            resources.ApplyResources(this.pbCatalog, "pbCatalog");
            this.pbCatalog.BackgroundImage = null;
            this.pbCatalog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.pbCatalog.Font = null;
            this.pbCatalog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pbCatalog.Name = "pbCatalog";
            this.pbCatalog.UseVisualStyleBackColor = true;
            this.pbCatalog.Click += new System.EventHandler(this.pbCatalog_Click);
            // 
            // frmRA_DEC
            // 
            this.AcceptButton = this.btnGoto;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = null;
            this.CancelButton = this.btnExit;
            this.Controls.Add(this.pbCatalog);
            this.Controls.Add(this.lbCatalog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkJ2000);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAddAlign);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.txtObject);
            this.Controls.Add(this.txtDEC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGoto);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRA_DEC";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmRA_DEC_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRA_DEC_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGoto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtRA;
        private System.Windows.Forms.MaskedTextBox txtDEC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox txtObject;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnAddAlign;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox chkJ2000;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbCatalog;
        private System.Windows.Forms.Button pbCatalog;
    }
}