using System;
using System.Drawing;
using System.Windows.Forms;

namespace CameraTest
{
    public partial class ImageControl : UserControl
    {
        #region constructor
        internal ImageControl()
        {        
            InitializeComponent();
            SetColourScale();
            ToolTip.SetToolTip(this.numMaximum, "Maximum value");
            ToolTip.SetToolTip(this.numMinimum, "Minimum value");
            ToolTip.SetToolTip(this.pnlSlider, "Left: adjust black level, Middle: move scale, Right; Adjust White level");
            ToolTip.SetToolTip(this.Gamma, "Set Gamma");
            ToolTip.SetToolTip(pnlHistogram, "Histogram Display");
            ctrlIncrement = 5;
            shiftIncrement = 50;
        }
        #endregion

        #region Change Event
        /// <summary>
        /// This event is raised when the values or gamma are changed
        /// </summary>
        internal event ChangeHandler Change;
        //internal ImageControl sender;
        //internal EventArgs e = null;
        internal delegate void ChangeHandler(object sender, EventArgs e);
        #endregion

        #region internal interface
        /// <summary>
        /// Maximum value for control
        /// </summary>
        public decimal Maximum
        {
            get { return numMaximum.Maximum; }
            set
            {
                numMaximum.Maximum = numMinimum.Maximum = value;
                SetSliders();
            }
        }

        /// <summary>
        /// Minimum value for control
        /// </summary>
        public decimal Minimum
        {
            get { return numMinimum.Minimum; }
            set
            {
                numMaximum.Minimum = numMinimum.Minimum = value;
                SetSliders();
            }
        }

        /// <summary>
        /// left hand (minimum) value of slider
        /// </summary>
        public decimal MinValue
        {
            get { return numMinimum.Value; }
            set
            {
                numMinimum.Value = value;
                SetSliders();
            }
        }
	
        /// <summary>
        /// right hand (maximum) value of slider
        /// </summary>
        public decimal MaxValue
        {
            get { return numMaximum.Value; }
            set
            {
                numMaximum.Value = value;
                SetSliders();
            }
        }

        /// <summary>
        /// number of decimal places
        /// </summary>
        public int DecimalPlaces
        {
            get { return numMaximum.DecimalPlaces; }
            set { numMaximum.DecimalPlaces = numMinimum.DecimalPlaces = value; }
        }

        /// <summary>
        /// Increment
        /// </summary>
        public decimal Increment
        {
            get { return numMinimum.Increment; }
            set { numMinimum.Increment = numMaximum.Increment = value; }
        }

        private decimal shiftIncrement;
        /// <summary>
        /// Increment used when the shift key is down
        /// </summary>
        public decimal ShiftIncrement
        {
            get { return shiftIncrement; }
            set { shiftIncrement = value; }
        }

        private decimal ctrlIncrement;
        /// <summary>
        /// Increment used when the control key is down
        /// </summary>
        public decimal CtrlIncrement
        {
            get { return ctrlIncrement; }
            set { ctrlIncrement = value; }
        }

        internal void Histogram(int[] hist)
        {
            if (hist == null)
                return;
            hist.CopyTo(histogram, 0);
            DrawHistogram();
        }
        #endregion

        #region private functions
        private int pnlLoc;
        private int pnlPos;

        private void pnlSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            pnlLoc = e.X;   // save current mouse position
            pnlPos = e.X * 3 / pnlSlider.Width; // 0 to 2 gives position within slider
        }

        private void pnlSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int dx = e.X - pnlLoc;
            int lp = pnlSlider.Left;
            int rp = this.Width - (pnlSlider.Left + pnlSlider.Width); // distance of rhs of slider from right of range

            switch (pnlPos)
            {
                case (0):
                    // left side, move left edge
                    if (lp >= -dx)
                    {
                        lp += dx;
                        numMinimum.Value = numMinimum.Minimum + lp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                    }
                    break;
                case (1):
                    // middle, move the lot
                    if (dx >= 0)
                    {
                        // move left, increasing values, so check the right edge
                        if (rp >= dx)
                        {
                            lp += dx;
                            rp -= dx;
                            numMinimum.Value = numMinimum.Minimum + lp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                            numMaximum.Value = numMaximum.Maximum - rp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                        }
                    }
                    else
                    {
                        // move right, check the left edge
                        if (lp >= -dx)
                        {
                            lp += dx;
                            rp -= dx;
                            numMinimum.Value = numMinimum.Minimum + lp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                            numMaximum.Value = numMaximum.Maximum - rp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                        }
                    }
                    break;
                case (2):
                    // right side, move right edge
                    if (rp >= dx)
                    {
                        rp -= dx;
                        numMaximum.Value = numMaximum.Maximum - rp * (numMaximum.Maximum - numMinimum.Minimum) / this.Width;
                    }
                    pnlLoc = e.X;
                    break;
            }
            pnlSlider.Left = lp;
            pnlSlider.Width = this.Width - (rp + lp);
        }

        private void SetSliders()
        {
            if (numMaximum.Maximum <= numMinimum.Minimum) return;
            pnlSlider.Left = (int)(this.Width * (numMinimum.Value - numMinimum.Minimum) / (numMaximum.Maximum - numMinimum.Minimum));
            pnlSlider.Width = (int)(this.Width * (numMaximum.Value - numMinimum.Value) / (numMaximum.Maximum - numMinimum.Minimum));
        }

        private void PictSlider_Load(object sender, EventArgs e)
        {
            //Gamma.Left = this.Width / 2;
            //lblGamma.Left = Gamma.Left - lblGamma.Width;
        }

        private readonly Bitmap bmp = new Bitmap(256, 1, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        /// <summary>
        /// Set the colour scale, allowing for the gamma setting
        /// </summary>
        private void SetColourScale()
        {
            for (int i = 0; i < 256; i++)
            {
                double v = i / 255.0;
                v = Math.Pow(v, (double)Gamma.Value);
                var j = (int)(v * 255);
                bmp.SetPixel(i, 0, Color.FromArgb(j, j, j));
            }
            pnlSlider.BackgroundImage = bmp;
            pnlSlider.Invalidate();
        }
        private readonly int[] histogram = new int[256];
        private int max;
        private bool logScale;

        private void DrawHistogram()
        {
            // test histogram
            max = pnlHistogram.Height;
            //histogram = new int[256];
            for (int i = 0; i < 256; i++)
            {
                //histogram[i] = i/5;
                if (max < histogram[i]) max = histogram[i];
            }

            Graphics g = pnlHistogram.CreateGraphics();
            Color colour = Color.Black;
            using (var sb = new SolidBrush(colour))
            {
                using (var myPen = new Pen(sb, 1))
                {
                    //The width of the pen is given by the XUnit for the control.
                    int h = pnlHistogram.Height, w = pnlHistogram.Width;
                    float sy = (float)h / max;
                    float sx = (float)w / 255;
                    if (logScale) sy = (float)(h / Math.Log10(max));
                    for (int i = 0; i < histogram.Length; i++)
                    {
                        var x = (int)(i * sx);
                        // set the colour
                        if (x < pnlSlider.Left)
                        {
                            myPen.Color = Color.Black;
                        }
                        else if (x > pnlSlider.Left + pnlSlider.Width)
                        {
                            myPen.Color = Color.White;
                        }
                        else
                        {
                            double v = (double)(x - pnlSlider.Left) / pnlSlider.Width;
                            v = Math.Pow(v, (double)Gamma.Value);
                            var j = (int)(v * 255);
                            myPen.Color = Color.FromArgb(j, j, j);
                        }

                        //We draw each line
                        int y = h - (int)(histogram[i] * sy);
                        if (logScale)
                            y = h - (int)(Math.Log10(histogram[i] + 1) * sy);
                        g.DrawLine(myPen, x, h, x, y);
                    }
                }
            }
        }

        private void Gamma_ValueChanged(object sender, EventArgs e)
        {
            SetColourScale();
            DrawHistogram();
            this.Change(this, e);
        }

        private void numMaximum_ValueChanged(object sender, EventArgs e)
        {
            if (numMaximum.Maximum <= numMinimum.Minimum) return;
            pnlSlider.Width = (int)(this.Width * (numMaximum.Value - numMinimum.Value) / (numMaximum.Maximum - numMinimum.Minimum));
            pnlSlider.Left = (int)(this.Width * numMinimum.Value / (numMaximum.Maximum - numMinimum.Minimum));
            //Change(this, e);
        }

        private void numMinimum_ValueChanged(object sender, EventArgs e)
        {
            if (numMaximum.Maximum <= numMinimum.Minimum) return;
            pnlSlider.Left = (int)(this.Width * numMinimum.Value / (numMaximum.Maximum - numMinimum.Minimum));
            pnlSlider.Width = (int)(this.Width * (numMaximum.Value - numMinimum.Value) / (numMaximum.Maximum - numMinimum.Minimum));
            //Change(this, e);
        }

        /// <summary>
        /// handles the key up and down for both numeric controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numUpDown_KeyUpDown(object sender, KeyEventArgs e)
        {
            var nud = sender as NumericUpDown;
            if (nud == null) throw new ArgumentNullException("sender");
            switch (e.Modifiers)
            {

                case Keys.Control:
                    nud.Increment = ctrlIncrement;
                    break;
                case Keys.ControlKey:
                    break;
                case Keys.Shift:
                    nud.Increment = shiftIncrement;
                    break;
	        }
        }

        private void pnlSlider_MouseUp(object sender, MouseEventArgs e)
        {
            this.DrawHistogram();
            this.Change(this, e);
        }

        private void numMaximum_Scroll(object sender, ScrollEventArgs e)
        {
            this.DrawHistogram();
            this.Change(this, e);
        }

        private void logHistogramToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            logScale = logHistogramToolStripMenuItem.Checked;
            this.DrawHistogram();
        }
        #endregion
    }
}
