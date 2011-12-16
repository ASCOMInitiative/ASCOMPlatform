using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ASCOM.Controls
{
    /// <summary>
    ///   Gauge Control - A Windows User Control.
    ///   Author  : Ambalavanar Thirugnanam
    ///   Date    : 24th August 2007
    ///   email   : ambalavanar.thiru@gmail.com
    ///   This is control is for free. You can use for any commercial or non-commercial purposes.
    ///   [Please do no remove this header when using this control in your application.]
    /// </summary>
    public sealed class AMeter : UserControl
    {
        #region Private Attributes

        private readonly int _x;
        private readonly int _y;
        private float _maxValue;
        private float _minValue;
        private float _threshold;
        private Image _backgroundImg;
        private float _currentValue;
        private Color _dialColor = Color.Lavender;
        private string _dialText;
        private bool _enableTransparentBackground;
        private const float fromAngle = 135F;
        private float _glossinessAlpha = 25;
        private int _height;
        private int _noOfDivisions;
        private int _noOfSubDivisions;
        private int _oldHeight;
        private int _oldWidth;
        private float _recommendedValue;
        private Rectangle _rectImg;
        private bool _requiresRedraw;
        private const float toAngle = 405F;
        private int _width;

        #endregion

        #region Constructor

        /// <summary>
        ///   Date    : 24th August 2007
        ///   This is control is for free. You can use for any commercial or non-commercial purposes.
        ///   [Please do no remove this header when using this control in your application.]
        /// </summary>
        public AMeter()
        {
            _x = 5;
            _y = 5;
            _width = Width - 10;
            _height = Height - 10;
            _noOfDivisions = 10;
            _noOfSubDivisions = 3;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
            Resize += AquaGaugeResize;
            _requiresRedraw = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Mininum value on the scale
        /// </summary>
        [DefaultValue(0)]
        [Description("Mininum value on the scale")]
        public float MinValue
        {
            get { return _minValue; }
            set
            {
                if (value < _maxValue)
                {
                    _minValue = value;
                    if (_currentValue < _minValue)
                        _currentValue = _minValue;
                    if (_recommendedValue < _minValue)
                        _recommendedValue = _minValue;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Maximum value on the scale
        /// </summary>
        [DefaultValue(100)]
        [Description("Maximum value on the scale")]
        public float MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value > _minValue)
                {
                    _maxValue = value;
                    if (_currentValue > _maxValue)
                        _currentValue = _maxValue;
                    if (_recommendedValue > _maxValue)
                        _recommendedValue = _maxValue;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        /// </summary>
        [DefaultValue(25)]
        [Description("Gets or Sets the Threshold area from the Recommended Value. (1-99%)")]
        public float ThresholdPercent
        {
            get { return _threshold; }
            set
            {
                if (value > 0 && value < 100)
                {
                    _threshold = value;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Threshold value from which green area will be marked.
        /// </summary>
        [DefaultValue(25)]
        [Description("Threshold value from which green area will be marked.")]
        public float RecommendedValue
        {
            get { return _recommendedValue; }
            set
            {
                if (value > _minValue && value < _maxValue)
                {
                    _recommendedValue = value;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Value where the pointer will point to.
        /// </summary>
        [DefaultValue(0)]
        [Description("Value where the pointer will point to.")]
        public float Value
        {
            get { return _currentValue; }
            set
            {
                if (value < _minValue || value > _maxValue) return;
                _currentValue = value;
                Refresh();
            }
        }

        /// <summary>
        ///   Background color of the dial
        /// </summary>
        [Description("Background color of the dial")]
        public Color DialColor
        {
            get { return _dialColor; }
            set
            {
                _dialColor = value;
                _requiresRedraw = true;
                Invalidate();
            }
        }

        /// <summary>
        ///   Glossiness strength. Range: 0-100
        /// </summary>
        [DefaultValue(72)]
        [Description("Glossiness strength. Range: 0-100")]
        public float Glossiness
        {
            get { return (_glossinessAlpha*100)/220; }
            set
            {
                float val = value;
                if (val > 100)
                    value = 100;
                if (val < 0)
                    value = 0;
                _glossinessAlpha = (value*220)/100;
                Refresh();
            }
        }

        /// <summary>
        ///   Get or Sets the number of Divisions in the dial scale.
        /// </summary>
        [DefaultValue(10)]
        [Description("Get or Sets the number of Divisions in the dial scale.")]
        public int NoOfDivisions
        {
            get { return _noOfDivisions; }
            set
            {
                if (value > 1 && value < 25)
                {
                    _noOfDivisions = value;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Gets or Sets the number of Sub Divisions in the scale per Division.
        /// </summary>
        [DefaultValue(3)]
        [Description("Gets or Sets the number of Sub Divisions in the scale per Division.")]
        public int NoOfSubDivisions
        {
            get { return _noOfSubDivisions; }
            set
            {
                if (value > 0 && value <= 10)
                {
                    _noOfSubDivisions = value;
                    _requiresRedraw = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///   Gets or Sets the Text to be displayed in the dial
        /// </summary>
        [Description("Gets or Sets the Text to be displayed in the dial")]
        public string DialText
        {
            get { return _dialText; }
            set
            {
                _dialText = value;
                _requiresRedraw = true;
                Invalidate();
            }
        }

        /// <summary>
        ///   Enables or Disables Transparent Background color.
        ///   Note: Enabling this will reduce the performance and may make the control flicker.
        /// </summary>
        [DefaultValue(false)]
        [Description(
            "Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker."
            )]
        public bool EnableTransparentBackground
        {
            get { return _enableTransparentBackground; }
            set
            {
                _enableTransparentBackground = value;
                SetStyle(ControlStyles.OptimizedDoubleBuffer, !_enableTransparentBackground);
                _requiresRedraw = true;
                Refresh();
            }
        }

        #endregion

        #region Overriden Control methods

        /// <summary>
        ///   Enables CreateParams
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        /// <summary>
        ///   Draws the pointer.
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _width = Width - _x*2;
            _height = Height - _y*2;
            DrawPointer(e.Graphics, ((_width)/2) + _x, ((_height)/2) + _y);
        }

        /// <summary>
        ///   Draws the dial background.
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!_enableTransparentBackground)
            {
                base.OnPaintBackground(e);
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, Width, Height));
            if (_backgroundImg == null || _requiresRedraw)
            {
                _backgroundImg = new Bitmap(Width, Height);
                var g = Graphics.FromImage(_backgroundImg);
                g.SmoothingMode = SmoothingMode.HighQuality;
                _width = Width - _x*2;
                _height = Height - _y*2;
                _rectImg = new Rectangle(_x, _y, _width, _height);

                //Draw background color
                Brush backGroundBrush = new SolidBrush(Color.FromArgb(120, _dialColor));
                if (_enableTransparentBackground && Parent != null)
                {
                    float gg = _width/60;
                    //g.FillEllipse(new SolidBrush(this.Parent.BackColor), -gg, -gg, this.Width+gg*2, this.Height+gg*2);
                }
                g.FillEllipse(backGroundBrush, _x, _y, _width, _height);

                //Draw Rim
                var outlineBrush = new SolidBrush(Color.FromArgb(100, Color.SlateGray));
                var outline = new Pen(outlineBrush, (float) (_width*.03));
                g.DrawEllipse(outline, _rectImg);
                var darkRim = new Pen(Color.SlateGray);
                g.DrawEllipse(darkRim, _x, _y, _width, _height);

                //Draw Callibration
                DrawCalibration(g, _rectImg, ((_width)/2) + _x, ((_height)/2) + _y);

                //Draw Colored Rim
                var colorPen = new Pen(Color.FromArgb(190, Color.Gainsboro), Width/40);
                var blackPen = new Pen(Color.FromArgb(250, Color.Black), Width/200);
                var gap = (int) (Width*0.03F);
                var rectg = new Rectangle(_rectImg.X + gap, _rectImg.Y + gap, _rectImg.Width - gap*2,
                                          _rectImg.Height - gap*2);
                g.DrawArc(colorPen, rectg, 135, 270);

                //Draw Threshold
                colorPen = new Pen(Color.FromArgb(200, Color.LawnGreen), Width/50);
                rectg = new Rectangle(_rectImg.X + gap, _rectImg.Y + gap, _rectImg.Width - gap*2, _rectImg.Height - gap*2);
                var val = MaxValue - MinValue;
                val = (100*(_recommendedValue - MinValue))/val;
                val = ((toAngle - fromAngle)*val)/100;
                val += fromAngle;
                var stAngle = val - ((270*_threshold)/200);
                if (stAngle <= 135) stAngle = 135;
                var sweepAngle = ((270*_threshold)/100);
                if (stAngle + sweepAngle > 405) sweepAngle = 405 - stAngle;
                g.DrawArc(colorPen, rectg, stAngle, sweepAngle);

                //Draw Digital Value
                var digiRect = new RectangleF(Width/2F - _width/5F,
                                              _height/1.2F, _width/2.5F,
                                              Height/9F);
                var digiFRect = new RectangleF(Width/2 - _width/7, (int) (_height/1.18),
                                               _width/4, Height/12);
                g.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Gray)), digiRect);
                DisplayNumber(g, _currentValue, digiFRect);

                var textSize = g.MeasureString(_dialText, Font);
                var digiFRectText = new RectangleF(Width/2 - textSize.Width/2, (int) (_height/1.5),
                                                   textSize.Width, textSize.Height);
                g.DrawString(_dialText, Font, new SolidBrush(ForeColor), digiFRectText);
                _requiresRedraw = false;
            }
            e.Graphics.DrawImage(_backgroundImg, _rectImg);
        }

        #endregion

        #region Private methods

        /// <summary>
        ///   Draws the Pointer.
        /// </summary>
        /// <param name = "gr"></param>
        /// <param name = "cx"></param>
        /// <param name = "cy"></param>
        private void DrawPointer(Graphics gr, int cx, int cy)
        {
            float radius = Width/2 - (Width*.12F);
            float val = MaxValue - MinValue;

            Image img = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            val = (100*(_currentValue - MinValue))/val;
            val = ((toAngle - fromAngle)*val)/100;
            val += fromAngle;

            float angle = GetRadian(val);

            var pts = new PointF[5];

            pts[0].X = (float) (cx + radius*Math.Cos(angle));
            pts[0].Y = (float) (cy + radius*Math.Sin(angle));

            pts[4].X = (float) (cx + radius*Math.Cos(angle - 0.02));
            pts[4].Y = (float) (cy + radius*Math.Sin(angle - 0.02));

            angle = GetRadian((val + 20));
            pts[1].X = (float) (cx + (Width*.09F)*Math.Cos(angle));
            pts[1].Y = (float) (cy + (Width*.09F)*Math.Sin(angle));

            pts[2].X = cx;
            pts[2].Y = cy;

            angle = GetRadian((val - 20));
            pts[3].X = (float) (cx + (Width*.09F)*Math.Cos(angle));
            pts[3].Y = (float) (cy + (Width*.09F)*Math.Sin(angle));

            Brush pointer = new SolidBrush(Color.Black);
            g.FillPolygon(pointer, pts);

            var shinePts = new PointF[3];
            angle = GetRadian(val);
            shinePts[0].X = (float) (cx + radius*Math.Cos(angle));
            shinePts[0].Y = (float) (cy + radius*Math.Sin(angle));

            angle = GetRadian(val + 20);
            shinePts[1].X = (float) (cx + (Width*.09F)*Math.Cos(angle));
            shinePts[1].Y = (float) (cy + (Width*.09F)*Math.Sin(angle));

            shinePts[2].X = cx;
            shinePts[2].Y = cy;

            var gpointer = new LinearGradientBrush(shinePts[0], shinePts[2], Color.SlateGray,
                                                   Color.Black);
            g.FillPolygon(gpointer, shinePts);

            var rect = new Rectangle(_x, _y, _width, _height);
            DrawCenterPoint(g, rect, ((_width)/2) + _x, ((_height)/2) + _y);

            DrawGloss(g);

            gr.DrawImage(img, 0, 0);
        }

        /// <summary>
        ///   Draws the glossiness.
        /// </summary>
        /// <param name = "g"></param>
        private void DrawGloss(Graphics g)
        {
            var glossRect = new RectangleF(
                _x + (float) (_width*0.10),
                _y + (float) (_height*0.07),
                (float) (_width*0.80),
                (float) (_height*0.7));
            var gradientBrush =
                new LinearGradientBrush(glossRect,
                                        Color.FromArgb((int) _glossinessAlpha, Color.White),
                                        Color.Transparent,
                                        LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);

            //TODO: Gradient from bottom
            glossRect = new RectangleF(
                _x + (float) (_width*0.25),
                _y + (float) (_height*0.77),
                (float) (_width*0.50),
                (float) (_height*0.2));
            var gloss = (int) (_glossinessAlpha/3);
            gradientBrush =
                new LinearGradientBrush(glossRect,
                                        Color.Transparent, Color.FromArgb(gloss, BackColor),
                                        LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);
        }

        /// <summary>
        ///   Draws the center point.
        /// </summary>
        /// <param name = "g"></param>
        /// <param name = "rect"></param>
        /// <param name = "cX"></param>
        /// <param name = "cY"></param>
        private void DrawCenterPoint(Graphics g, Rectangle rect, int cX, int cY)
        {
            float shift = Width/5;
            var rectangle = new RectangleF(cX - (shift/2), cY - (shift/2), shift, shift);
            var brush = new LinearGradientBrush(rect, Color.Black, Color.FromArgb(100, _dialColor),
                                                LinearGradientMode.Vertical);
            g.FillEllipse(brush, rectangle);

            shift = Width/7;
            rectangle = new RectangleF(cX - (shift/2), cY - (shift/2), shift, shift);
            brush = new LinearGradientBrush(rect, Color.SlateGray, Color.Black, LinearGradientMode.ForwardDiagonal);
            g.FillEllipse(brush, rectangle);
        }

        /// <summary>
        ///   Draws the Ruler
        /// </summary>
        /// <param name = "g"></param>
        /// <param name = "rect"></param>
        /// <param name = "cX"></param>
        /// <param name = "cY"></param>
        private void DrawCalibration(Graphics g, Rectangle rect, int cX, int cY)
        {
            int noOfParts = _noOfDivisions + 1;
            int noOfIntermediates = _noOfSubDivisions;
            float currentAngle = GetRadian(fromAngle);
            var gap = (int) (Width*0.01F);
            float shift = Width/25;
            var rectangle = new Rectangle(rect.Left + gap, rect.Top + gap, rect.Width - gap, rect.Height - gap);

            float radius = rectangle.Width/2 - gap*5;
            const float totalAngle = toAngle - fromAngle;
            var incr = GetRadian(((totalAngle)/((noOfParts - 1)*(noOfIntermediates + 1))));

            var thickPen = new Pen(Color.Black, Width/50);
            var thinPen = new Pen(Color.Black, Width/100);
            float rulerValue = MinValue;
            for (int i = 0; i <= noOfParts; i++)
            {
                //Draw Thick Line
                var x = (float) (cX + radius*Math.Cos(currentAngle));
                var y = (float) (cY + radius*Math.Sin(currentAngle));
                var x1 = (float) (cX + (radius - Width/20)*Math.Cos(currentAngle));
                var y1 = (float) (cY + (radius - Width/20)*Math.Sin(currentAngle));
                g.DrawLine(thickPen, x, y, x1, y1);

                //Draw Strings
                var format = new StringFormat();
                var tx = (float) (cX + (radius - Width/10)*Math.Cos(currentAngle));
                var ty = (float) (cY - shift + (radius - Width/10)*Math.Sin(currentAngle));
                Brush stringPen = new SolidBrush(ForeColor);
                var strFormat = new StringFormat(StringFormatFlags.NoClip);
                strFormat.Alignment = StringAlignment.Center;
                var f = new Font(Font.FontFamily, (Width/23), Font.Style);
                g.DrawString(rulerValue + "", f, stringPen, new PointF(tx, ty), strFormat);
                rulerValue += ((MaxValue - MinValue)/(noOfParts - 1));
                rulerValue = (float) Math.Round(rulerValue, 2);

                //currentAngle += incr;
                if (i == noOfParts - 1)
                    break;
                for (int j = 0; j <= noOfIntermediates; j++)
                {
                    //Draw thin lines 
                    currentAngle += incr;
                    x = (float) (cX + radius*Math.Cos(currentAngle));
                    y = (float) (cY + radius*Math.Sin(currentAngle));
                    x1 = (float) (cX + (radius - Width/50)*Math.Cos(currentAngle));
                    y1 = (float) (cY + (radius - Width/50)*Math.Sin(currentAngle));
                    g.DrawLine(thinPen, x, y, x1, y1);
                }
            }
        }

        /// <summary>
        ///   Converts the given degree to radian.
        /// </summary>
        /// <param name = "theta"></param>
        /// <returns></returns>
        public float GetRadian(float theta)
        {
            return theta*(float) Math.PI/180F;
        }

        /// <summary>
        ///   Displays the given number in the 7-Segement format.
        /// </summary>
        /// <param name = "g"></param>
        /// <param name = "number"></param>
        /// <param name = "drect"></param>
        private void DisplayNumber(Graphics g, float number, RectangleF drect)
        {
            try
            {
                var num = number.ToString("000.00");
                num.PadLeft(3, '0');
                float shift = 0;
                if (number < 0)
                {
                    shift -= _width/17;
                }
                var chars = num.ToCharArray();
                for (var i = 0; i < chars.Length; i++)
                {
                    var c = chars[i];
                    bool drawDps;
                    if (i < chars.Length - 1 && chars[i + 1] == '.')
                        drawDps = true;
                    else
                        drawDps = false;
                    if (c != '.')
                    {
                        if (c == '-')
                        {
                            DrawDigit(g, -1, new PointF(drect.X + shift, drect.Y), drawDps, drect.Height);
                        }
                        else
                        {
                            DrawDigit(g, Int16.Parse(c.ToString()), new PointF(drect.X + shift, drect.Y), drawDps,
                                      drect.Height);
                        }
                        shift += 15*_width/250;
                    }
                    else
                    {
                        shift += 2*_width/250;
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///   Draws a digit in 7-Segement format.
        /// </summary>
        /// <param name = "g"></param>
        /// <param name = "number"></param>
        /// <param name = "position"></param>
        /// <param name = "dp"></param>
        /// <param name = "height"></param>
        private void DrawDigit(Graphics g, int number, PointF position, bool dp, float height)
        {
            float width = 10F*height/13;

            var outline = new Pen(Color.FromArgb(40, _dialColor));
            var fillPen = new Pen(Color.Black);

            #region Form Polygon Points

            //Segment A
            var segmentA = new PointF[5];
            segmentA[0] = segmentA[4] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(1F, height));
            segmentA[1] = new PointF(position.X + GetX(10, width), position.Y + GetY(1F, height));
            segmentA[2] = new PointF(position.X + GetX(8.8F, width), position.Y + GetY(2F, height));
            segmentA[3] = new PointF(position.X + GetX(3.8F, width), position.Y + GetY(2F, height));

            //Segment B
            var segmentB = new PointF[5];
            segmentB[0] = segmentB[4] = new PointF(position.X + GetX(10, width), position.Y + GetY(1.4F, height));
            segmentB[1] = new PointF(position.X + GetX(9.3F, width), position.Y + GetY(6.8F, height));
            segmentB[2] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(6.4F, height));
            segmentB[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(2.2F, height));

            //Segment C
            var segmentC = new PointF[5];
            segmentC[0] = segmentC[4] = new PointF(position.X + GetX(9.2F, width), position.Y + GetY(7.2F, height));
            segmentC[1] = new PointF(position.X + GetX(8.7F, width), position.Y + GetY(12.7F, height));
            segmentC[2] = new PointF(position.X + GetX(7.6F, width), position.Y + GetY(11.9F, height));
            segmentC[3] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.7F, height));

            //Segment D
            var segmentD = new PointF[5];
            segmentD[0] = segmentD[4] = new PointF(position.X + GetX(7.4F, width), position.Y + GetY(12.1F, height));
            segmentD[1] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(13F, height));
            segmentD[2] = new PointF(position.X + GetX(1.3F, width), position.Y + GetY(13F, height));
            segmentD[3] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(12.1F, height));

            //Segment E
            var segmentE = new PointF[5];
            segmentE[0] = segmentE[4] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(11.8F, height));
            segmentE[1] = new PointF(position.X + GetX(1F, width), position.Y + GetY(12.7F, height));
            segmentE[2] = new PointF(position.X + GetX(1.7F, width), position.Y + GetY(7.2F, height));
            segmentE[3] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(7.7F, height));

            //Segment F
            var segmentF = new PointF[5];
            segmentF[0] = segmentF[4] = new PointF(position.X + GetX(3F, width), position.Y + GetY(6.4F, height));
            segmentF[1] = new PointF(position.X + GetX(1.8F, width), position.Y + GetY(6.8F, height));
            segmentF[2] = new PointF(position.X + GetX(2.6F, width), position.Y + GetY(1.3F, height));
            segmentF[3] = new PointF(position.X + GetX(3.6F, width), position.Y + GetY(2.2F, height));

            //Segment G
            var segmentG = new PointF[7];
            segmentG[0] = segmentG[6] = new PointF(position.X + GetX(2F, width), position.Y + GetY(7F, height));
            segmentG[1] = new PointF(position.X + GetX(3.1F, width), position.Y + GetY(6.5F, height));
            segmentG[2] = new PointF(position.X + GetX(8.3F, width), position.Y + GetY(6.5F, height));
            segmentG[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(7F, height));
            segmentG[4] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.5F, height));
            segmentG[5] = new PointF(position.X + GetX(2.9F, width), position.Y + GetY(7.5F, height));

            //Segment DP

            #endregion

            #region Draw Segments Outline

            g.FillPolygon(outline.Brush, segmentA);
            g.FillPolygon(outline.Brush, segmentB);
            g.FillPolygon(outline.Brush, segmentC);
            g.FillPolygon(outline.Brush, segmentD);
            g.FillPolygon(outline.Brush, segmentE);
            g.FillPolygon(outline.Brush, segmentF);
            g.FillPolygon(outline.Brush, segmentG);

            #endregion

            #region Fill Segments

            //Fill SegmentA
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentA);
            }

            //Fill SegmentB
            if (IsNumberAvailable(number, 0, 1, 2, 3, 4, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentB);
            }

            //Fill SegmentC
            if (IsNumberAvailable(number, 0, 1, 3, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentC);
            }

            //Fill SegmentD
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentD);
            }

            //Fill SegmentE
            if (IsNumberAvailable(number, 0, 2, 6, 8))
            {
                g.FillPolygon(fillPen.Brush, segmentE);
            }

            //Fill SegmentF
            if (IsNumberAvailable(number, 0, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentF);
            }

            //Fill SegmentG
            if (IsNumberAvailable(number, 2, 3, 4, 5, 6, 8, 9, -1))
            {
                g.FillPolygon(fillPen.Brush, segmentG);
            }

            #endregion

            //Draw decimal point
            if (dp)
            {
                g.FillEllipse(fillPen.Brush, new RectangleF(
                                                 position.X + GetX(10F, width),
                                                 position.Y + GetY(12F, height),
                                                 width/7,
                                                 width/7));
            }
        }

        /// <summary>
        ///   Gets Relative X for the given width to draw digit
        /// </summary>
        /// <param name = "x"></param>
        /// <param name = "width"></param>
        /// <returns></returns>
        private static float GetX(float x, float width)
        {
            return x*width/12;
        }

        /// <summary>
        ///   Gets relative Y for the given height to draw digit
        /// </summary>
        /// <param name = "y"></param>
        /// <param name = "height"></param>
        /// <returns></returns>
        private static float GetY(float y, float height)
        {
            return y*height/15;
        }

        /// <summary>
        ///   Returns true if a given number is available in the given list.
        /// </summary>
        /// <param name = "number"></param>
        /// <param name = "listOfNumbers"></param>
        /// <returns></returns>
        private static bool IsNumberAvailable(int number, params int[] listOfNumbers)
        {
            if (listOfNumbers.Length > 0)
            {
                return listOfNumbers.Any(i => i == number);
            }
            return false;
        }

        /// <summary>
        ///   Restricts the size to make sure the height and width are always same.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void AquaGaugeResize(object sender, EventArgs e)
        {
            if (Width < 136)
            {
                Width = 136;
            }
            if (_oldWidth != Width)
            {
                Height = Width;
                _oldHeight = Width;
            }
            if (_oldHeight != Height)
            {
                Width = Height;
                _oldWidth = Width;
            }
        }

        #endregion
    }
}