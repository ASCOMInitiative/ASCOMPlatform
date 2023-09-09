using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RemoveASCOM
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            btnExit = new Button();
            btnExit.Click += new EventHandler(btnExit_Click);
            btnRemove = new Button();
            btnRemove.Click += new EventHandler(btnRemove_Click);
            txtWarning = new TextBox();
            lblResult = new Label();
            LblAction = new Label();
            cmbRemoveMode = new ComboBox();
            cmbRemoveMode.SelectedIndexChanged += new EventHandler(cmbRemoveMode_SelectedIndexChanged);
            Label1 = new Label();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.DialogResult = DialogResult.Cancel;
            btnExit.Location = new Point(623, 437);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 23);
            btnExit.TabIndex = 1;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            btnRemove.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemove.Location = new Point(227, 242);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(260, 76);
            btnRemove.TabIndex = 2;
            btnRemove.Text = "Remove ASCOM";
            btnRemove.UseVisualStyleBackColor = true;
            // 
            // txtWarning
            // 
            txtWarning.BackColor = Color.WhiteSmoke;
            txtWarning.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtWarning.ForeColor = SystemColors.Window;
            txtWarning.Location = new Point(12, 28);
            txtWarning.Multiline = true;
            txtWarning.Name = "txtWarning";
            txtWarning.ReadOnly = true;
            txtWarning.Size = new Size(686, 120);
            txtWarning.TabIndex = 3;
            txtWarning.TextAlign = HorizontalAlignment.Center;
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Location = new Point(9, 418);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(51, 13);
            lblResult.TabIndex = 4;
            lblResult.Text = "LblStatus";
            // 
            // LblAction
            // 
            LblAction.AutoSize = true;
            LblAction.Location = new Point(9, 442);
            LblAction.Name = "LblAction";
            LblAction.Size = new Size(51, 13);
            LblAction.TabIndex = 5;
            LblAction.Text = "LblAction";
            // 
            // cmbRemoveMode
            // 
            cmbRemoveMode.FormattingEnabled = true;
            cmbRemoveMode.Location = new Point(235, 358);
            cmbRemoveMode.Name = "cmbRemoveMode";
            cmbRemoveMode.Size = new Size(241, 21);
            cmbRemoveMode.TabIndex = 6;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(146, 361);
            Label1.Name = "Label1";
            Label1.Size = new Size(83, 13);
            Label1.TabIndex = 7;
            Label1.Text = "Removal Option";
            // 
            // Form1
            // 
            AcceptButton = btnExit;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnExit;
            ClientSize = new Size(710, 472);
            Controls.Add(Label1);
            Controls.Add(cmbRemoveMode);
            Controls.Add(LblAction);
            Controls.Add(lblResult);
            Controls.Add(txtWarning);
            Controls.Add(btnRemove);
            Controls.Add(btnExit);
            Name = "Form1";
            Text = "Platform Recovery Tool";
            Load += new EventHandler(Form1_Load);
            FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            ResumeLayout(false);
            PerformLayout();

        }
        internal Button btnExit;
        private Button btnRemove;
        internal TextBox txtWarning;
        internal Label lblResult;
        internal Label LblAction;
        internal ComboBox cmbRemoveMode;
        internal Label Label1;

    }
}