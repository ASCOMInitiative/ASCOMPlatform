using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Serial form class
    /// </summary>
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class SerialForm : Form
    {
        /// <summary>
        /// Form overrides dispose to clean up the component list.
        /// </summary>
        /// <param name="disposing"></param>
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
#pragma warning disable CS0649 // Field 'SerialForm.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Field 'SerialForm.components' is never assigned to, and will always have its default value null

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialForm));
            lstSerialASCOM = new ListBox();
            btnSerialExit = new Button();
            btnSerialExit.Click += new EventHandler(btnSerialExit_Click);
            lblSerial = new Label();
            SuspendLayout();
            // 
            // lstSerialASCOM
            // 
            lstSerialASCOM.FormattingEnabled = true;
            lstSerialASCOM.Location = new Point(15, 25);
            lstSerialASCOM.Name = "lstSerialASCOM";
            lstSerialASCOM.Size = new Size(295, 264);
            lstSerialASCOM.TabIndex = 0;
            // 
            // btnSerialExit
            // 
            btnSerialExit.DialogResult = DialogResult.Cancel;
            btnSerialExit.Location = new Point(331, 266);
            btnSerialExit.Name = "btnSerialExit";
            btnSerialExit.Size = new Size(75, 23);
            btnSerialExit.TabIndex = 1;
            btnSerialExit.Text = "Exit";
            btnSerialExit.UseVisualStyleBackColor = true;
            // 
            // lblSerial
            // 
            lblSerial.AutoSize = true;
            lblSerial.Location = new Point(12, 9);
            lblSerial.Name = "lblSerial";
            lblSerial.Size = new Size(143, 13);
            lblSerial.TabIndex = 2;
            lblSerial.Text = "COM Ports visible to ASCOM";
            // 
            // SerialForm
            // 
            AcceptButton = btnSerialExit;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnSerialExit;
            ClientSize = new Size(424, 306);
            Controls.Add(lblSerial);
            Controls.Add(btnSerialExit);
            Controls.Add(lstSerialASCOM);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SerialForm";
            Text = "COM Ports";
            Load += new EventHandler(SerialForm_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal ListBox lstSerialASCOM;
        internal Button btnSerialExit;
        internal Label lblSerial;
    }
}