namespace CameraTest
{
    partial class ImageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSlider = new System.Windows.Forms.Panel();
            this.numMinimum = new System.Windows.Forms.NumericUpDown();
            this.numMaximum = new System.Windows.Forms.NumericUpDown();
            this.Gamma = new System.Windows.Forms.NumericUpDown();
            this.pnlHistogram = new System.Windows.Forms.Panel();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.logHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gamma)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSlider
            // 
            this.pnlSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSlider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSlider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSlider.Location = new System.Drawing.Point(36, 81);
            this.pnlSlider.MaximumSize = new System.Drawing.Size(555, 20);
            this.pnlSlider.MinimumSize = new System.Drawing.Size(4, 5);
            this.pnlSlider.Name = "pnlSlider";
            this.pnlSlider.Size = new System.Drawing.Size(91, 10);
            this.pnlSlider.TabIndex = 1;
            this.pnlSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSlider_MouseDown);
            this.pnlSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSlider_MouseMove);
            this.pnlSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSlider_MouseUp);
            // 
            // numMinimum
            // 
            this.numMinimum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numMinimum.Location = new System.Drawing.Point(0, 92);
            this.numMinimum.Name = "numMinimum";
            this.numMinimum.Size = new System.Drawing.Size(51, 20);
            this.numMinimum.TabIndex = 2;
            this.numMinimum.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numMinimum.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numMinimum.ValueChanged += new System.EventHandler(this.numMinimum_ValueChanged);
            this.numMinimum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDown_KeyUpDown);
            this.numMinimum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numUpDown_KeyUpDown);
            // 
            // numMaximum
            // 
            this.numMaximum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numMaximum.Location = new System.Drawing.Point(105, 92);
            this.numMaximum.Name = "numMaximum";
            this.numMaximum.Size = new System.Drawing.Size(51, 20);
            this.numMaximum.TabIndex = 3;
            this.numMaximum.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numMaximum.ValueChanged += new System.EventHandler(this.numMaximum_ValueChanged);
            this.numMaximum.Scroll += new System.Windows.Forms.ScrollEventHandler(this.numMaximum_Scroll);
            this.numMaximum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDown_KeyUpDown);
            this.numMaximum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numUpDown_KeyUpDown);
            // 
            // Gamma
            // 
            this.Gamma.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Gamma.DecimalPlaces = 1;
            this.Gamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Gamma.Location = new System.Drawing.Point(58, 92);
            this.Gamma.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.Gamma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Gamma.Name = "Gamma";
            this.Gamma.Size = new System.Drawing.Size(40, 20);
            this.Gamma.TabIndex = 4;
            this.Gamma.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.Gamma.ValueChanged += new System.EventHandler(this.Gamma_ValueChanged);
            // 
            // pnlHistogram
            // 
            this.pnlHistogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHistogram.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlHistogram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHistogram.ContextMenuStrip = this.contextMenuStrip1;
            this.pnlHistogram.Location = new System.Drawing.Point(0, 0);
            this.pnlHistogram.Name = "pnlHistogram";
            this.pnlHistogram.Size = new System.Drawing.Size(156, 81);
            this.pnlHistogram.TabIndex = 6;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logHistogramToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 48);
            // 
            // logHistogramToolStripMenuItem
            // 
            this.logHistogramToolStripMenuItem.CheckOnClick = true;
            this.logHistogramToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.logHistogramToolStripMenuItem.Name = "logHistogramToolStripMenuItem";
            this.logHistogramToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.logHistogramToolStripMenuItem.Text = "Log Histogram";
            this.logHistogramToolStripMenuItem.ToolTipText = "Check to use log scale for the histogram";
            this.logHistogramToolStripMenuItem.CheckedChanged += new System.EventHandler(this.logHistogramToolStripMenuItem_CheckedChanged);
            // 
            // ImageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.pnlHistogram);
            this.Controls.Add(this.Gamma);
            this.Controls.Add(this.numMaximum);
            this.Controls.Add(this.numMinimum);
            this.Controls.Add(this.pnlSlider);
            this.Name = "ImageControl";
            this.Size = new System.Drawing.Size(156, 112);
            this.Load += new System.EventHandler(this.PictSlider_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gamma)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSlider;
        private System.Windows.Forms.NumericUpDown numMinimum;
        private System.Windows.Forms.NumericUpDown numMaximum;
        private System.Windows.Forms.Panel pnlHistogram;
        internal System.Windows.Forms.NumericUpDown Gamma;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logHistogramToolStripMenuItem;
    }
}
