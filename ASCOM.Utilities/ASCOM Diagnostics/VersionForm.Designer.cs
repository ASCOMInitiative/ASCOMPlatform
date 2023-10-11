using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Utilities
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class VersionForm : Form
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
#pragma warning disable CS0649 // Field 'VersionForm.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Field 'VersionForm.components' is never assigned to, and will always have its default value null

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionForm));
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.NameLbl = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.LblBuildSha = new System.Windows.Forms.Label();
            this.BuildSha = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("PictureBox1.InitialImage")));
            this.PictureBox1.Location = new System.Drawing.Point(12, 12);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(77, 74);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLbl.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.NameLbl.Location = new System.Drawing.Point(8, 124);
            this.NameLbl.MinimumSize = new System.Drawing.Size(550, 0);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(550, 22);
            this.NameLbl.TabIndex = 4;
            this.NameLbl.Text = "Name";
            this.NameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Version.Location = new System.Drawing.Point(8, 154);
            this.Version.MinimumSize = new System.Drawing.Size(550, 0);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(550, 18);
            this.Version.TabIndex = 5;
            this.Version.Text = "Version";
            this.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblBuildSha
            // 
            this.LblBuildSha.AutoSize = true;
            this.LblBuildSha.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBuildSha.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.LblBuildSha.Location = new System.Drawing.Point(92, 267);
            this.LblBuildSha.Name = "LblBuildSha";
            this.LblBuildSha.Size = new System.Drawing.Size(73, 16);
            this.LblBuildSha.TabIndex = 6;
            this.LblBuildSha.Text = "Build SHA:";
            // 
            // BuildSha
            // 
            this.BuildSha.AutoSize = true;
            this.BuildSha.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuildSha.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BuildSha.Location = new System.Drawing.Point(167, 267);
            this.BuildSha.Name = "BuildSha";
            this.BuildSha.Size = new System.Drawing.Size(306, 16);
            this.BuildSha.TabIndex = 7;
            this.BuildSha.Text = "a6231f4c20c7a241acf288d1655c65bf7adcaabf";
            // 
            // VersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 295);
            this.Controls.Add(this.BuildSha);
            this.Controls.Add(this.LblBuildSha);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.PictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VersionForm";
            this.Text = "ASCOM Platfom Version";
            this.Load += new System.EventHandler(this.VersionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal PictureBox PictureBox1;
        internal Label NameLbl;
        internal Label Version;
        internal Label LblBuildSha;
        internal Label BuildSha;
    }
}