using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class CheckedMessageBox : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckedMessageBox));
            BtnOk = new Button();
            BtnOk.Click += new EventHandler(BtnOk_Click);
            Label1 = new Label();
            ChkDoNotShowAgain = new CheckBox();
            ChkDoNotShowAgain.CheckedChanged += new EventHandler(ChkDoNotShowAgain_CheckedChanged);
            Label2 = new Label();
            Panel1 = new Panel();
            Label3 = new Label();
            Label4 = new Label();
            Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnOk
            // 
            BtnOk.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnOk.Location = new Point(295, 12);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 0;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label1.Location = new Point(12, 23);
            Label1.Name = "Label1";
            Label1.Size = new Size(342, 13);
            Label1.TabIndex = 1;
            Label1.Text = "When you press OK on this dialogue you will be asked for";
            // 
            // ChkDoNotShowAgain
            // 
            ChkDoNotShowAgain.AutoSize = true;
            ChkDoNotShowAgain.BackColor = SystemColors.Control;
            ChkDoNotShowAgain.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChkDoNotShowAgain.Location = new Point(21, 16);
            ChkDoNotShowAgain.Name = "ChkDoNotShowAgain";
            ChkDoNotShowAgain.Size = new Size(194, 17);
            ChkDoNotShowAgain.TabIndex = 2;
            ChkDoNotShowAgain.Text = "Do not show Alpaca messages again";
            ChkDoNotShowAgain.UseVisualStyleBackColor = false;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label2.Location = new Point(12, 36);
            Label2.Name = "Label2";
            Label2.Size = new Size(155, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Administrator approval to create the new Alpaca Dynamic driver.";
            // 
            // Panel1
            // 
            Panel1.BackColor = SystemColors.Control;
            Panel1.Controls.Add(BtnOk);
            Panel1.Controls.Add(ChkDoNotShowAgain);
            Panel1.Location = new Point(-9, 115);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(403, 45);
            Panel1.TabIndex = 4;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label3.Location = new Point(12, 68);
            Label3.Name = "Label3";
            Label3.Size = new Size(346, 13);
            Label3.TabIndex = 5;
            Label3.Text = "After the driver is created, the Chooser dialogue will be presented";
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Label4.Location = new Point(12, 81);
            Label4.Name = "Label4";
            Label4.Size = new Size(352, 13);
            Label4.TabIndex = 6;
            Label4.Text = "again and the driver can be selected as normal with the OK button.";
            // 
            // CheckedMessageBox
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(373, 160);
            Controls.Add(Label4);
            Controls.Add(Label3);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(Panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CheckedMessageBox";
            Text = "New Alpaca Device Selected";
            Panel1.ResumeLayout(false);
            Panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        internal Button BtnOk;
        internal Label Label1;
        internal CheckBox ChkDoNotShowAgain;
        internal Label Label2;
        internal Panel Panel1;
        internal Label Label3;
        internal Label Label4;
    }
}