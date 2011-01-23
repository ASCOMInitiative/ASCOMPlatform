using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace ASCOM.Controls
{
    /// <summary>
    ///   Enumerations for Analog or Bar type meter for the switch drivers.
    /// </summary>
    /// 
    public enum MeterScale
    {
        ///<summary>
        /// used to select an analog type visual meter
        ///</summary>
        Analog,
        ///<summary>
        /// used to select an bar type visual meter
        ///</summary>
        Log10
    } ;

    ///<summary>
    /// usercontrol class for the design of the meter
    ///</summary>
    [Designer(typeof (VuMeterDesigner))]
    [ToolboxBitmap(@"Vu_Meter.bmp")]
    public class VuMeter : UserControl
    {
        #region priviate constants

        private bool _analogDialRegionOnly;
        private Color _borderColor = Color.DimGray;
        private int _currentLevel;
        private const double degHigh = Math.PI*1.2;
        private const double degLow = Math.PI*0.8;
        private Color _dialBackColor = Color.White;
        private Color _dialNeedle = Color.Black;
        private Color _dialPeak = Color.Red;
        private string[] _dialText = {"-40", "-20", "-10", "-5", "0", "+6"};
        private Color _dialTextHigh = Color.Black;
        private Color _dialTextLow = Color.Red;
        private Color _dialTextNeutral = Color.DarkGreen;
        private MeterScale _formType = MeterScale.Log10;
        private Size _led = new Size(6, 14);
        private Color _ledColorOff1 = Color.DarkGreen, _ledColorOff2 = Color.Olive, _ledColorOff3 = Color.Maroon;
        private Color _ledColorOn1 = Color.LimeGreen, _ledColorOn2 = Color.Yellow, _ledColorOn3 = Color.Red;
        private int _ledCount1 = 6, _ledCount2 = 6, _ledCount3 = 4;
        private int _ledSpacing = 3;
        private int _max = 65535;
        private bool _meterAnalog;
        private string _meterText = "VU";
        private const int min = 0;
        private int _peakHoldTime = 1000;
        private int _peakLevel;
        private bool _showDialText;
        private bool _showLedPeakInAnalog;
        private bool _showPeak = true;
        private bool _useLedLightInAnalog;
        private bool _vertical;
        private int _calcPeak;
        private int _calcValue;

        private readonly Timer _timer1;

        #endregion

        #region Constructors

        ///<summary>
        /// constructor method for a meter
        ///</summary>
        public VuMeter()
        {
            Name = "VuMeter";
            CalcSize();
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            _timer1 = new Timer {Interval = _peakHoldTime, Enabled = false};
            _timer1.Tick += Timer1Tick;
        }

        #endregion

        #region public properties

        ///<summary>
        /// Show textvalues in dial
        ///</summary>
        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool UseLedLight
        {
            get { return _useLedLightInAnalog; }
            set
            {
                _useLedLightInAnalog = value;
                Invalidate();
            }
        }


        ///<summary>
        /// Show textvalues in dial
        ///</summary>
        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool ShowLedPeak
        {
            get { return _showLedPeakInAnalog; }
            set
            {
                _showLedPeakInAnalog = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Analog meter layout
        ///</summary>
        [Category("Analog Meter")]
        [Description("Analog meter layout")]
        public bool AnalogMeter
        {
            get { return _meterAnalog; }
            set
            {
                if (value & !_meterAnalog) Size = new Size(100, 80);
                _meterAnalog = value;
                CalcSize();
                Invalidate();
            }
        }


        ///<summary>
        /// Text (max 10 letters
        ///</summary>
        [Category("Analog Meter")]
        [Description("Text (max 10 letters)")]
        public string VuText
        {
            get { return _meterText; }
            set
            {
                if (value.Length < 11) _meterText = value;
                Invalidate();
            }
        }


        ///<summary>
        /// Text in dial
        ///</summary>
        [Category("Analog Meter")]
        [Description("Text in dial")]
        public string[] TextInDial
        {
            get { return _dialText; }
            set
            {
                _dialText = value;
                Invalidate();
            }
        }


        ///<summary>
        /// Only show the Analog Dial Panel (Sets BackColor to DialBackColor so antialias won't look bad
        ///</summary>
        [Category("Analog Meter")]
        [Description("Only show the Analog Dial Panel (Sets BackColor to DialBackColor so antialias won't look bad)")]
        public bool ShowDialOnly
        {
            get { return _analogDialRegionOnly; }
            set
            {
                _analogDialRegionOnly = value;
                if (_analogDialRegionOnly) BackColor = _dialBackColor;
                Invalidate();
            }
        }

        ///<summary>
        /// Color on dial background
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on dial background")]
        public Color DialBackground
        {
            get { return _dialBackColor; }
            set
            {
                _dialBackColor = value;
                Invalidate();
            }
        }


        ///<summary>
        /// Color on boarder
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on border")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color on Value less than zero
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on Value < 0")]
        public Color DialTextNegative
        {
            get { return _dialTextLow; }
            set
            {
                _dialTextLow = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color on Value = 0
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on Value = 0")]
        public Color DialTextZero
        {
            get { return _dialTextNeutral; }
            set
            {
                _dialTextNeutral = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color on Value > 0
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on Value > 0")]
        public Color DialTextPositive
        {
            get { return _dialTextHigh; }
            set
            {
                _dialTextHigh = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color on needle
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on needle")]
        public Color NeedleColor
        {
            get { return _dialNeedle; }
            set
            {
                _dialNeedle = value;
                Invalidate();
            }
        }

        ///<summary>
        /// olor on Peak needle
        ///</summary>
        [Category("Analog Meter")]
        [Description("Color on Peak needle")]
        public Color PeakNeedleColor
        {
            get { return _dialPeak; }
            set
            {
                _dialPeak = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Display value in analog or logarithmic scale
        ///</summary>
        [Category("VU Meter")]
        [Description("Display value in analog or logarithmic scale")]
        public MeterScale MeterScale
        {
            get { return _formType; }
            set
            {
                _formType = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Led size (1 to 72 pixels)
        ///</summary>
        [Category("VU Meter")]
        [Description("Led size (1 to 72 pixels)")]
        public Size LedSize
        {
            get { return _led; }
            set
            {
                if (value.Height < 1) _led.Height = 1;
                else if (value.Height > 72) _led.Height = 72;
                else _led.Height = value.Height;

                if (value.Width < 1) _led.Width = 1;
                else if (value.Width > 72) _led.Width = 72;
                else _led.Width = value.Width;

                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Led spacing (0 to 72 pixels)
        ///</summary>
        [Category("VU Meter")]
        [Description("Led spacing (0 to 72 pixels)")]
        public int LedSpace
        {
            get { return _ledSpacing; }
            set
            {
                if (value < 0) _ledSpacing = 0;
                else if (value > 72) _ledSpacing = 72;
                else _ledSpacing = value;
                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Led bar is vertical
        ///</summary>
        [Category("VU Meter")]
        [Description("Led bar is vertical")]
        public bool VerticalBar
        {
            get { return _vertical; }
            set
            {
                _vertical = value;
                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Max value from total LedCount to 65535
        ///</summary>
        [Category("VU Meter")]
        [Description("Max value from total LedCount to 65535")]
        public int LevelMax
        {
            get { return _max; }
            set
            {
                if (value < (Led1Count + Led2Count + Led3Count)) _max = (Led1Count + Led2Count + Led3Count);
                else if (value > 65535) _max = 65535;
                else _max = value;

                Invalidate();
            }
        }

        ///<summary>
        /// The level shown (between Min and Max)
        ///</summary>
        [Category("VU Meter")]
        [Description("The level shown (between Min and Max)")]
        public int Level
        {
            get { return _currentLevel; }

            set
            {
                if (value != _currentLevel)
                {
                    if (value < min) _currentLevel = min;
                    else if (value > _max) _currentLevel = _max;
                    else _currentLevel = value;

                    if ((_currentLevel > _peakLevel) & (_showPeak | _showLedPeakInAnalog))
                    {
                        _peakLevel = _currentLevel;
                        _timer1.Stop();
                        _timer1.Start();
                    }
                    Invalidate();
                }
            }
        }

        ///<summary>
        /// How many mS to hold peak indicator (50 to 10000mS)
        ///</summary>
        [Category("VU Meter")]
        [Description("How many mS to hold peak indicator (50 to 10000mS)")]
        public int Peakms
        {
            get { return _peakHoldTime; }
            set
            {
                if (value < 50) _peakHoldTime = 50;
                else if (value > 10000) _peakHoldTime = 10000;
                else _peakHoldTime = value;
                _timer1.Interval = _peakHoldTime;
                Invalidate();
            }
        }

        ///<summary>
        /// Use peak indicator
        ///</summary>
        [Category("VU Meter")]
        [Description("Use peak indicator")]
        public bool PeakHold
        {
            get { return _showPeak; }
            set
            {
                _showPeak = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Peak Level
        ///</summary>
        [Category("VU Meter")]
        [Description("Current peak level")]
        public int PeakLevel
        {
            get { return _peakLevel; }
        }

        ///<summary>
        /// Show textvalues in dial
        ///</summary>
        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool ShowTextInDial
        {
            get { return _showDialText; }
            set
            {
                _showDialText = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Number of Leds in first area (0 to 32, default 6) Total Led count must be at least 1
        ///</summary>
        [Category("VU Meter")]
        [Description("Number of Leds in first area (0 to 32, default 6) Total Led count must be at least 1")]
        public int Led1Count
        {
            get { return _ledCount1; }

            set
            {
                if (value < 0) _ledCount1 = 0;
                else if (value > 32) _ledCount1 = 32;
                else _ledCount1 = value;
                if ((_ledCount1 + _ledCount2 + _ledCount3) < 1) _ledCount1 = 1;
                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Number of Leds in middle area (0 to 32, default 6) Total Led count must be at least 1
        ///</summary>
        [Category("VU Meter")]
        [Description("Number of Leds in middle area (0 to 32, default 6) Total Led count must be at least 1")]
        public int Led2Count
        {
            get { return _ledCount2; }

            set
            {
                if (value < 0) _ledCount2 = 0;
                else if (value > 32) _ledCount2 = 32;
                else _ledCount2 = value;
                if ((_ledCount1 + _ledCount2 + _ledCount3) < 1) _ledCount2 = 1;
                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Number of Leds in last area (0 to 32, default 4) Total Led count must be at least 1
        ///</summary>
        [Category("VU Meter")]
        [Description("Number of Leds in last area (0 to 32, default 4) Total Led count must be at least 1")]
        public int Led3Count
        {
            get { return _ledCount3; }

            set
            {
                if (value < 0) _ledCount3 = 0;
                else if (value > 32) _ledCount3 = 32;
                else _ledCount3 = value;
                if ((_ledCount1 + _ledCount2 + _ledCount3) < 1) _ledCount3 = 1;
                CalcSize();
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in first area (Led on)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in first area (Led on)")]
        public Color Led1ColorOn
        {
            get { return _ledColorOn1; }
            set
            {
                _ledColorOn1 = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in middle area (Led on)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in middle area (Led on)")]
        public Color Led2ColorOn
        {
            get { return _ledColorOn2; }
            set
            {
                _ledColorOn2 = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in last area (Led on)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in last area (Led on)")]
        public Color Led3ColorOn
        {
            get { return _ledColorOn3; }
            set
            {
                _ledColorOn3 = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in first area (Led off)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in first area (Led off)")]
        public Color Led1ColorOff
        {
            get { return _ledColorOff1; }
            set
            {
                _ledColorOff1 = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in middle area (Led off)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in middle area (Led off)")]
        public Color Led2ColorOff
        {
            get { return _ledColorOff2; }
            set
            {
                _ledColorOff2 = value;
                Invalidate();
            }
        }

        ///<summary>
        /// Color of Leds in last area (Led off)
        ///</summary>
        [Category("VU Meter - Colors")]
        [Description("Color of Leds in last area (Led off)")]
        public Color Led3ColorOff
        {
            get { return _ledColorOff3; }
            set
            {
                _ledColorOff3 = value;
                Invalidate();
            }
        }

        #endregion

        #region protected methods

        ///<summary>
        /// OnPaint
        ///</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_meterAnalog)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                DrawAnalogBorder(g);
                DrawAnalogDial(g);
            }
            else
            {
                Graphics g = e.Graphics;
                DrawBorder(g);
                DrawLeds(g);
            }
        }

        ///<summary>
        /// OnResize
        ///</summary>
        protected override void OnResize(EventArgs e)
        {
            if (_meterAnalog)
            {
                base.OnResize(e);
            }
            CalcSize();
            base.OnResize(e);
            Invalidate();
        }

        #endregion

        #region private methods

        private void Timer1Tick(object sender, EventArgs e)
        {
            _timer1.Stop();
            _peakLevel = _currentLevel;
            Invalidate();
            _timer1.Start();
        }

        private void CalcSize()
        {
            if (_meterAnalog)
            {
                Size = new Size(Width, (int) (Width*0.8));
            }
            else if (_vertical)
            {
                Size = new Size(_led.Width + _ledSpacing*2,
                                (_ledCount1 + _ledCount2 + _ledCount3)*(_led.Height + _ledSpacing) + _ledSpacing);
            }
            else
            {
                Size = new Size((_ledCount1 + _ledCount2 + _ledCount3)*(_led.Width + _ledSpacing) + _ledSpacing,
                                _led.Height + _ledSpacing*2);
            }
        }

        private void DrawAnalogDial(Graphics g)
        {
            //Add code to draw "LED:s" by color in Dial (Analog and LED)
            if (_useLedLightInAnalog)
            {
                if (_formType == MeterScale.Log10)
                {
                    _calcValue =
                        (int) (Math.Log10((double) _currentLevel/(_max/10) + 1)*(_ledCount1 + _ledCount2 + _ledCount3));
                    if (_showLedPeakInAnalog)
                        _calcPeak =
                            (int) (Math.Log10((double) _peakLevel/(_max/10) + 1)*(_ledCount1 + _ledCount2 + _ledCount3));
                }

                if (_formType == MeterScale.Analog)
                {
                    _calcValue = (int) (((double) _currentLevel/_max)*(_ledCount1 + _ledCount2 + _ledCount3) + 0.5);
                    if (_showLedPeakInAnalog)
                        _calcPeak = (int) (((double) _peakLevel/_max)*(_ledCount1 + _ledCount2 + _ledCount3) + 0.5);
                }

                Double degStep = (degHigh - degLow)/(_ledCount1 + _ledCount2 + _ledCount3 - 1);
                double i;
                double sinI, CosI;
                int lc = 0;
                var ledRadiusStart = (int) (Width*0.6);
                if (!ShowTextInDial) ledRadiusStart = (int) (Width*0.65);
                for (i = degHigh; i > degLow - degStep/2; i = i - degStep)
                {
                    Pen scalePen;
                    if ((lc < _calcValue) | (((lc + 1) == _calcPeak) & _showLedPeakInAnalog))
                    {
                        scalePen = new Pen(Led3ColorOn, _led.Width);
                        if (lc < _ledCount1 + _ledCount2) scalePen = new Pen(Led2ColorOn, _led.Width);
                        if (lc < _ledCount1) scalePen = new Pen(Led1ColorOn, _led.Width);
                    }
                    else
                    {
                        scalePen = new Pen(Led3ColorOff, _led.Width);
                        if (lc < _ledCount1 + _ledCount2) scalePen = new Pen(Led2ColorOff, _led.Width);
                        if (lc < _ledCount1) scalePen = new Pen(Led1ColorOff, _led.Width);
                    }

                    lc++;
                    sinI = Math.Sin(i);
                    CosI = Math.Cos(i);
                    g.DrawLine(scalePen, (int) ((ledRadiusStart - _led.Height)*sinI + Width/2),
                               (int) ((ledRadiusStart - _led.Height)*CosI + Height*0.9),
                               (int) (ledRadiusStart*sinI + Width/2), (int) (ledRadiusStart*CosI + Height*0.9));
                }
            }
            //End of code addition

            if (_formType == MeterScale.Log10)
            {
                _calcValue = (int) (Math.Log10((double) _currentLevel/(_max/10) + 1)*_max);
                if (_showPeak) _calcPeak = (int) (Math.Log10((double) _peakLevel/(_max/10) + 1)*_max);
            }

            if (_formType == MeterScale.Analog)
            {
                _calcValue = _currentLevel;
                if (_showPeak) _calcPeak = _peakLevel;
            }
            int dialRadiusLow = (int) (Width*0.3f), dialRadiusHigh = (int) (Width*0.65f);

            var dialPen = new Pen(_dialNeedle, Width*0.01f);
            double dialPos;
            if (_calcValue > 0) dialPos = degHigh - (((double) _calcValue/_max)*(degHigh - degLow));
            else dialPos = degHigh;
            Double sinD = Math.Sin(dialPos), cosD = Math.Cos(dialPos);
            g.DrawLine(dialPen, (int) (dialRadiusLow*sinD + Width*0.5),
                       (int) (dialRadiusLow*cosD + Height*0.9),
                       (int) (dialRadiusHigh*sinD + Width*0.5),
                       (int) (dialRadiusHigh*cosD + Height*0.9));

            if (_showPeak)
            {
                var peakPen = new Pen(_dialPeak, Width*0.01f);
                if (_calcPeak > 0) dialPos = degHigh - (((double) _calcPeak/_max)*(degHigh - degLow));
                else dialPos = degHigh;
                Double sinP = Math.Sin(dialPos), cosP = Math.Cos(dialPos);
                g.DrawLine(peakPen, (int) (dialRadiusLow*sinP + Width*0.5),
                           (int) (dialRadiusLow*cosP + Height*0.9),
                           (int) (dialRadiusHigh*sinP + Width*0.5),
                           (int) (dialRadiusHigh*cosP + Height*0.9));
            }
            dialPen.Dispose();
        }

        private void DrawLeds(Graphics g)
        {
            if (_formType == MeterScale.Log10)
            {
                _calcValue = (int) (Math.Log10((double) _currentLevel/(_max/10) + 1)*(_ledCount1 + _ledCount2 + _ledCount3));
                if (_showPeak)
                    _calcPeak = (int) (Math.Log10((double) _peakLevel/(_max/10) + 1)*(_ledCount1 + _ledCount2 + _ledCount3));
            }

            if (_formType == MeterScale.Analog)
            {
                _calcValue = (int) (((double) _currentLevel/_max)*(_ledCount1 + _ledCount2 + _ledCount3) + 0.5);
                if (_showPeak) _calcPeak = (int) (((double) _peakLevel/_max)*(_ledCount1 + _ledCount2 + _ledCount3) + 0.5);
            }


            for (int i = 0; i < (_ledCount1 + _ledCount2 + _ledCount3); i++)
            {
                if (_vertical)
                {
                    var current = new Rectangle(ClientRectangle.X + _ledSpacing,
                                                ClientRectangle.Height - ((i + 1)*(_led.Height + _ledSpacing)),
                                                _led.Width, _led.Height);

                    if ((i < _calcValue) | (((i + 1) == _calcPeak) & _showPeak))
                    {
                        if (i < _ledCount1)
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn1), current);
                        }
                        else if (i < (_ledCount1 + _ledCount2))
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn3), current);
                        }
                    }
                    else
                    {
                        if (i < _ledCount1)
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff1), current);
                        }
                        else if (i < (_ledCount1 + _ledCount2))
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff3), current);
                        }
                    }
                }
                else
                {
                    var current = new Rectangle(ClientRectangle.X + (i*(_led.Width + _ledSpacing)) + _ledSpacing,
                                                ClientRectangle.Y + _ledSpacing, _led.Width, _led.Height);

                    if ((i) < _calcValue | (((i + 1) == _calcPeak) & _showPeak))
                    {
                        if (i < _ledCount1)
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn1), current);
                        }
                        else if (i < (_ledCount1 + _ledCount2))
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOn3), current);
                        }
                    }
                    else
                    {
                        if (i < _ledCount1)
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff1), current);
                        }
                        else if (i < (_ledCount1 + _ledCount2))
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(_ledColorOff3), current);
                        }
                    }
                }
            }
        }

        private void DrawBorder(Graphics g)
        {
            var border = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width,
                                       ClientRectangle.Height);
            g.FillRectangle(new SolidBrush(BackColor), border);
        }

        private void DrawAnalogBorder(Graphics g)
        {
            if (!_analogDialRegionOnly) g.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);

            double degStep = (degHigh*1.05 - degLow/1.05)/19;
            double i = degHigh*1.05;
            double sinI, cosI;

            var curvePoints = new PointF[40];
            for (int cp = 0; cp < 20; cp++)
            {
                i = i - degStep;
                sinI = Math.Sin(i);
                cosI = Math.Cos(i);
                curvePoints[cp] = new PointF((float) (sinI*Width*0.7 + Width/2), (float) (cosI*Width*0.7 + Height*0.9));
                curvePoints[38 - cp] = new PointF((float) (sinI*Width*0.3 + Width/2),
                                                  (float) (cosI*Width*0.3 + Height*0.9));
            }
            curvePoints[39] = curvePoints[0];
            var dialPath = new GraphicsPath();
            if (_analogDialRegionOnly) dialPath.AddPolygon(curvePoints);
            else dialPath.AddRectangle(new Rectangle(0, 0, Width, Height));
            Region = new Region(dialPath);
            g.FillPolygon(new SolidBrush(_dialBackColor), curvePoints);

            // Test moving this block
            var ledRadiusStart = (int) (Width*0.6);
            if (!_useLedLightInAnalog)
            {
                degStep = (degHigh - degLow)/(_ledCount1 + _ledCount2 + _ledCount3 - 1);
                int lc = 0;
                if (!ShowTextInDial) ledRadiusStart = (int) (Width*0.65);
                for (i = degHigh; i > degLow - degStep/2; i = i - degStep)
                {
                    //Graphics scale = g.Graphics;
                    var scalePen = new Pen(Led3ColorOn, _led.Width);
                    if (lc < _ledCount1 + _ledCount2) scalePen = new Pen(Led2ColorOn, _led.Width);
                    if (lc < _ledCount1) scalePen = new Pen(Led1ColorOn, _led.Width);
                    lc++;
                    sinI = Math.Sin(i);
                    cosI = Math.Cos(i);
                    g.DrawLine(scalePen, (int) ((ledRadiusStart - _led.Height)*sinI + Width/2),
                               (int) ((ledRadiusStart - _led.Height)*cosI + Height*0.9),
                               (int) (ledRadiusStart*sinI + Width/2), (int) (ledRadiusStart*cosI + Height*0.9));
                    scalePen.Dispose();
                }
            }
            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            float meterFontSize = Font.SizeInPoints;
            if (Width > 0) meterFontSize = meterFontSize*(Width/100f);
            if (meterFontSize < 4) meterFontSize = 4;
            if (meterFontSize > 72) meterFontSize = 72;
            var meterFont = new Font(Font.FontFamily, meterFontSize);
            g.DrawString(_meterText, meterFont, new SolidBrush(_dialTextHigh), Width / 2, Height * 0.43f, format);

            var textRadiusStart = (int) (Width*0.64);
            if (_showDialText)
            {
                var dialTextStep = (degHigh - degLow)/(_dialText.Length - 1);
                int dt = 0;
                meterFontSize = meterFontSize*0.6f;
                for (i = degHigh; i > degLow - dialTextStep/2; i = i - dialTextStep)
                {
                    //Graphics scale = g.Graphics;
                    Brush dtColor = new SolidBrush(_dialTextHigh);
                    var dtformat = new StringFormat
                                       {
                                           Alignment = StringAlignment.Center,
                                           LineAlignment = StringAlignment.Center
                                       };
                    try
                    {
                        if (int.Parse(_dialText[dt]) < 0) dtColor = new SolidBrush(_dialTextLow);
                        if (int.Parse(_dialText[dt]) == 0) dtColor = new SolidBrush(_dialTextNeutral);
                    }
                    catch
                    {
                        dtColor = new SolidBrush(_dialTextHigh);
                    }
                    var dtfont = new Font(Font.FontFamily, meterFontSize);
                    sinI = Math.Sin(i);
                    cosI = Math.Cos(i);
                    g.DrawString(_dialText[dt++], dtfont, dtColor, (int) (textRadiusStart*sinI + Width/2),
                                 (int) (textRadiusStart*cosI + Height*0.9), dtformat);
                }
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            ResumeLayout(false);
        }

        #endregion
    }

    internal class VuMeterDesigner : ControlDesigner
    {
        protected override void PostFilterProperties(IDictionary properties)
        {
            properties.Remove("AccessibleDescription");
            properties.Remove("AccessibleName");
            properties.Remove("AccessibleRole");
            properties.Remove("BackgroundImage");
            //properties.Remove("BackgroundImageLayout");
            properties.Remove("BorderStyle");
            properties.Remove("Cursor");
            properties.Remove("RightToLeft");
            properties.Remove("UseWaitCursor");
            properties.Remove("AllowDrop");
            properties.Remove("AutoValidate");
            properties.Remove("ContextMenuStrip");
            properties.Remove("Enabled");
            properties.Remove("ImeMode");
            //properties.Remove("TabIndex"); // Don't remove this one or the designer will break
            properties.Remove("TabStop");
            //properties.Remove("Visible");
            properties.Remove("ApplicationSettings");
            properties.Remove("DataBindings");
            properties.Remove("Tag");
            properties.Remove("GenerateMember");
            properties.Remove("Locked");
            //properties.Remove("Modifiers");
            properties.Remove("CausesValidation");
            properties.Remove("Anchor");
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
            //properties.Remove("Location");
            properties.Remove("Dock");
            properties.Remove("Margin");
            properties.Remove("MaximumSize");
            properties.Remove("MinimumSize");
            properties.Remove("Padding");
            //properties.Remove("Size");
            properties.Remove("DockPadding");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("AutoScroll");
            properties.Remove("ForeColor");
            //properties.Remove("BackColor");
            properties.Remove("Text");
            //properties.Remove("Font");
        }
    }
}