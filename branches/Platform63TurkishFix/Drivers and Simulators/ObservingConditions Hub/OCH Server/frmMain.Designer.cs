using System;

namespace ASCOM.Simulator
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnShutDown = new System.Windows.Forms.Button();
            this.lblNumberOfConnections = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnShutDown
            // 
            this.btnShutDown.Location = new System.Drawing.Point(440, 191);
            this.btnShutDown.Name = "btnShutDown";
            this.btnShutDown.Size = new System.Drawing.Size(75, 23);
            this.btnShutDown.TabIndex = 1;
            this.btnShutDown.Text = "ShutDown";
            this.btnShutDown.UseVisualStyleBackColor = true;
            this.btnShutDown.Click += new System.EventHandler(this.btnShutDown_Click);
            // 
            // lblNumberOfConnections
            // 
            this.lblNumberOfConnections.AutoSize = true;
            this.lblNumberOfConnections.Location = new System.Drawing.Point(77, 102);
            this.lblNumberOfConnections.Name = "lblNumberOfConnections";
            this.lblNumberOfConnections.Size = new System.Drawing.Size(129, 13);
            this.lblNumberOfConnections.TabIndex = 2;
            this.lblNumberOfConnections.Text = "Number of connections: 0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 226);
            this.Controls.Add(this.lblNumberOfConnections);
            this.Controls.Add(this.btnShutDown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Observing Conditions Hub";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnShutDown;
        private System.Windows.Forms.Label lblNumberOfConnections;

    }
}

