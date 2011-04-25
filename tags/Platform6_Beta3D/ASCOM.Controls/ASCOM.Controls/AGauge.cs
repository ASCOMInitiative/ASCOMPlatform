using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ASCOM.Controls
{
    ///<summary>
    /// Copyright (C) 2007 A.J.Bauer
    ///
    /// This software is provided as-is, without any express or implied
    ///  warranty.  In no event will the authors be held liable for any damages
    ///  arising from the use of this software.
    ///
    ///  Permission is granted to anyone to use this software for any purpose,
    ///  including commercial applications, and to alter it and redistribute it
    ///  freely, subject to the following restrictions:
    ///
    ///  1. The origin of this software must not be misrepresented; you must not
    ///     claim that you wrote the original software. if you use this software
    ///     in a product, an acknowledgment in the product documentation would be
    ///     appreciated but is not required.
    ///  2. Altered source versions must be plainly marked as such, and must not be
    ///     misrepresented as being the original software.
    ///  3. This notice may not be removed or altered from any source distribution.
    ///</summary>
    [ToolboxBitmapAttribute(typeof (AGauge), "AGauge.bmp"),
     DefaultEvent("ValueInRangeChanged"),
     Description("Displays a value on an analog gauge. Raises an event if the value enters one of the definable ranges."
         )]
    public partial class AGauge : Control
    {
        #region enum, var, delegate, event

        #region Delegates

        ///<summary>
        /// ValueInRangeChangedDelegate
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        public delegate void ValueInRangeChangedDelegate(Object sender, ValueInRangeChangedEventArgs e);

        #endregion

        ///<summary>
        /// Needle colors
        ///</summary>
        public enum NeedleColorEnum
        {
            ///<summary>
            /// grey
            ///</summary>
            Gray = 0,
            ///<summary>
            /// red
            ///</summary>
            Red = 1,
            ///<summary>
            /// green
            ///</summary>
            Green = 2,
            ///<summary>
            /// blue
            ///</summary>
            Blue = 3,
            ///<summary>
            /// yellow
            ///</summary>
            Yellow = 4,
            ///<summary>
            /// violet
            ///</summary>
            Violet = 5,
            ///<summary>
            /// magenta
            ///</summary>
            Magenta = 6
        } ;

/*
        private const Byte zero = 0;
*/
        private const Byte numofcaps = 5;
        private const Byte numofranges = 5;
        private readonly String[] _mCapText = {"", "", "", "", ""};
        private readonly Boolean[] _mValueIsInRange = {false, false, false, false, false};
        private Boolean _drawGaugeBackground = true;

        private Single _fontBoundY1;
        private Single _fontBoundY2;
        private Bitmap _gaugeBitmap;

        private Color _mBaseArcColor = Color.Gray;
        private Int32 _mBaseArcRadius = 80;
        private Int32 _mBaseArcStart = 135;
        private Int32 _mBaseArcSweep = 270;
        private Int32 _mBaseArcWidth = 2;
        private Byte _mCapIdx = 1;
        private readonly Color[] _mCapColor = { Color.Black, Color.Black, Color.Black, Color.Black, Color.Black };
        private Point[] _mCapPosition = {
                                            new Point(10, 10), new Point(10, 10), new Point(10, 10), new Point(10, 10),
                                            new Point(10, 10)
                                        };

        private Point _mCenter = new Point(100, 100);
        private Single _mMaxValue = 400;
        private Single _mMinValue = -100;
        private NeedleColorEnum _mNeedleColor1 = NeedleColorEnum.Gray;
        private Color _mNeedleColor2 = Color.DimGray;
        private Int32 _mNeedleRadius = 80;
        private Int32 _mNeedleType;
        private Int32 _mNeedleWidth = 2;

        private Color[] _mRangeColor = {
                                           Color.LightGreen, Color.Red, Color.FromKnownColor(KnownColor.Control),
                                           Color.FromKnownColor(KnownColor.Control),
                                           Color.FromKnownColor(KnownColor.Control)
                                       };

        private Boolean[] _mRangeEnabled = {true, true, false, false, false};
        private Single[] _mRangeEndValue = {300.0f, 400.0f, 0.0f, 0.0f, 0.0f};
        private Byte _mRangeIdx;
        private Int32[] _mRangeInnerRadius = {70, 70, 70, 70, 70};
        private Int32[] _mRangeOuterRadius = {80, 80, 80, 80, 80};
        private Single[] _mRangeStartValue = {-100.0f, 300.0f, 0.0f, 0.0f, 0.0f};

        private Color _mScaleLinesInterColor = Color.Black;
        private Int32 _mScaleLinesInterInnerRadius = 73;
        private Int32 _mScaleLinesInterOuterRadius = 80;
        private Int32 _mScaleLinesInterWidth = 1;

        private Color _mScaleLinesMajorColor = Color.Black;
        private Int32 _mScaleLinesMajorInnerRadius = 70;
        private Int32 _mScaleLinesMajorOuterRadius = 80;
        private Single _mScaleLinesMajorStepValue = 50.0f;
        private Int32 _mScaleLinesMajorWidth = 2;
        private Color _mScaleLinesMinorColor = Color.Gray;
        private Int32 _mScaleLinesMinorInnerRadius = 75;
        private Int32 _mScaleLinesMinorNumOf = 9;
        private Int32 _mScaleLinesMinorOuterRadius = 80;
        private Int32 _mScaleLinesMinorWidth = 1;

        private Color _mScaleNumbersColor = Color.Black;
        private String _mScaleNumbersFormat;
        private Int32 _mScaleNumbersRadius = 95;
        private Int32 _mScaleNumbersRotation;
        private Int32 _mScaleNumbersStartScaleLine;
        private Int32 _mScaleNumbersStepScaleLines = 1;
        private Single _mValue;

        ///<summary>
        /// This event is raised if the value falls into a defined range.
        ///</summary>
        [Description("This event is raised if the value falls into a defined range.")]
        public event ValueInRangeChangedDelegate ValueInRangeChanged;

        ///<summary>
        /// This class is raised if the value falls into a defined range.
        ///</summary>
        public class ValueInRangeChangedEventArgs : EventArgs
        {
            ///<summary>
            /// ValueInRange
            ///</summary>
            private Int32 _valueInRange;

            ///<summary>
            /// ValueInRangeChangedEventArgs
            ///</summary>
            ///<param name="valueInRange"></param>
            public ValueInRangeChangedEventArgs(Int32 valueInRange)
            {
                _valueInRange = valueInRange;
            }
        }

        #endregion

        #region hidden , overridden inherited properties

        ///<summary>
        /// AllowDrop
        ///</summary>
        public new Boolean AllowDrop
        {
            get { return false; }
        }

        ///<summary>
        /// AutoSize
        ///</summary>
        public new Boolean AutoSize
        {
            get { return false; }
        }

        ///<summary>
        /// ForeColor
        ///</summary>
        public new Boolean ForeColor
        {
            get { return false; }
        }

        ///<summary>
        /// ImeMode
        ///</summary>
        public new Boolean ImeMode
        {
            get { return false; }
        }


        ///<summary>
        /// BackColor
        ///</summary>
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// Font
        ///</summary>
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// BackgroundImageLayout
        ///</summary>
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set
            {
                base.BackgroundImageLayout = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        #endregion

        ///<summary>
        /// constructor
        ///</summary>
        public AGauge()
        {
            CapColors = new Color[] {Color.Black, Color.Black, Color.Black, Color.Black, Color.Black};
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #region properties

        ///<summary>
        /// The value
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The value.")]
        public Single Value
        {
            get { return _mValue; }
            set
            {
                if (_mValue == value) return;
                _mValue = Math.Min(Math.Max(value, _mMinValue), _mMaxValue);

                if (!DesignMode)
                {
                }
                else
                {
                    _drawGaugeBackground = true;
                }

                for (var counter = 0; counter < numofranges - 1; counter++)
                {
                    if ((_mRangeStartValue[counter] > _mValue) || (_mValue > _mRangeEndValue[counter]) ||
                        (!_mRangeEnabled[counter]))
                    {
                        _mValueIsInRange[counter] = false;
                    }
                    else
                    {
                        if (_mValueIsInRange[counter]) continue;
                        if (ValueInRangeChanged == null) continue;
                        ValueInRangeChanged(this, new ValueInRangeChangedEventArgs(counter));
                    }
                }
                Refresh();
            }
        }

        ///<summary>
        /// The caption index. set this to a value of 0 up to 4 to change the corresponding caption's properties.
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         RefreshProperties(RefreshProperties.All),
         Description(
             "The caption index. set this to a value of 0 up to 4 to change the corresponding caption's properties.")]
        public Byte CapIdx
        {
            get { return _mCapIdx; }
            set
            {
                if ((_mCapIdx == value) || (0 > value) || (value >= 5)) return;
                _mCapIdx = value;
            }
        }

        ///<summary>
        /// CapColor
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the caption text.")]
        private Color CapColor
        {
            get { return _mCapColor[_mCapIdx]; }
            set
            {
                if (_mCapColor[_mCapIdx] == value) return;
                _mCapColor[_mCapIdx] = value;
                CapColors = _mCapColor;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// CapColors
        ///</summary>
        [Browsable(false)]
        public Color[] CapColors { get; set; }

        ///<summary>
        /// The text of the caption
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The text of the caption.")]
        public String CapText
        {
            get { return _mCapText[_mCapIdx]; }
            set
            {
                if (_mCapText[_mCapIdx] == value) return;
                _mCapText[_mCapIdx] = value;
                CapsText = _mCapText;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// CapsText
        ///</summary>
        [Browsable(false)]
        public String[] CapsText
        {
            get { return _mCapText; }
            set
            {
                for (Int32 counter = 0; counter < 5; counter++)
                {
                    _mCapText[counter] = value[counter];
                }
            }
        }

        ///<summary>
        /// The position of the caption
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The position of the caption.")]
        public Point CapPosition
        {
            get { return _mCapPosition[_mCapIdx]; }
            set
            {
                if (_mCapPosition[_mCapIdx] == value) return;
                _mCapPosition[_mCapIdx] = value;
                CapsPosition = _mCapPosition;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// CapsPosition
        ///</summary>
        [Browsable(false)]
        public Point[] CapsPosition
        {
            get { return _mCapPosition; }
            set { _mCapPosition = value; }
        }

        ///<summary>
        /// The center of the gauge (in the control's client area)
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The center of the gauge (in the control's client area).")]
        public Point Center
        {
            get { return _mCenter; }
            set
            {
                if (_mCenter == value) return;
                _mCenter = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The minimum value to show on the scale
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The minimum value to show on the scale.")]
        public Single MinValue
        {
            get { return _mMinValue; }
            set
            {
                if ((_mMinValue == value) || (value >= _mMaxValue)) return;
                _mMinValue = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The maximum value to show on the scale
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The maximum value to show on the scale.")]
        public Single MaxValue
        {
            get { return _mMaxValue; }
            set
            {
                if ((_mMaxValue == value) || (value <= _mMinValue)) return;
                _mMaxValue = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The color of the base arc
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the base arc.")]
        public Color BaseArcColor
        {
            get { return _mBaseArcColor; }
            set
            {
                if (_mBaseArcColor == value) return;
                _mBaseArcColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The radius of the base arc
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The radius of the base arc.")]
        public Int32 BaseArcRadius
        {
            get { return _mBaseArcRadius; }
            set
            {
                if (_mBaseArcRadius == value) return;
                _mBaseArcRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The start angle of the base arc
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The start angle of the base arc.")]
        public Int32 BaseArcStart
        {
            get { return _mBaseArcStart; }
            set
            {
                if (_mBaseArcStart == value) return;
                _mBaseArcStart = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The sweep angle of the base arc
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The sweep angle of the base arc.")]
        public Int32 BaseArcSweep
        {
            get { return _mBaseArcSweep; }
            set
            {
                if (_mBaseArcSweep == value) return;
                _mBaseArcSweep = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The width of the base arc
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The width of the base arc.")]
        public Int32 BaseArcWidth
        {
            get { return _mBaseArcWidth; }
            set
            {
                if (_mBaseArcWidth == value) return;
                _mBaseArcWidth = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The color of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The color of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines."
             )]
        public Color ScaleLinesInterColor
        {
            get { return _mScaleLinesInterColor; }
            set
            {
                if (_mScaleLinesInterColor == value) return;
                _mScaleLinesInterColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The inner radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The inner radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines."
             )]
        public Int32 ScaleLinesInterInnerRadius
        {
            get { return _mScaleLinesInterInnerRadius; }
            set
            {
                if (_mScaleLinesInterInnerRadius == value) return;
                _mScaleLinesInterInnerRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The outer radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The outer radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines."
             )]
        public Int32 ScaleLinesInterOuterRadius
        {
            get { return _mScaleLinesInterOuterRadius; }
            set
            {
                if (_mScaleLinesInterOuterRadius == value) return;
                _mScaleLinesInterOuterRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The width of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The width of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines."
             )]
        public Int32 ScaleLinesInterWidth
        {
            get { return _mScaleLinesInterWidth; }
            set
            {
                if (_mScaleLinesInterWidth == value) return;
                _mScaleLinesInterWidth = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The number of minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The number of minor scale lines.")]
        public Int32 ScaleLinesMinorNumOf
        {
            get { return _mScaleLinesMinorNumOf; }
            set
            {
                if (_mScaleLinesMinorNumOf == value) return;
                _mScaleLinesMinorNumOf = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The color of the minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the minor scale lines.")]
        public Color ScaleLinesMinorColor
        {
            get { return _mScaleLinesMinorColor; }
            set
            {
                if (_mScaleLinesMinorColor == value) return;
                _mScaleLinesMinorColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The inner radius of the minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The inner radius of the minor scale lines.")]
        public Int32 ScaleLinesMinorInnerRadius
        {
            get { return _mScaleLinesMinorInnerRadius; }
            set
            {
                if (_mScaleLinesMinorInnerRadius == value) return;
                _mScaleLinesMinorInnerRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The outer radius of the minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The outer radius of the minor scale lines.")]
        public Int32 ScaleLinesMinorOuterRadius
        {
            get { return _mScaleLinesMinorOuterRadius; }
            set
            {
                if (_mScaleLinesMinorOuterRadius == value) return;
                _mScaleLinesMinorOuterRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The width of the minor scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The width of the minor scale lines.")]
        public Int32 ScaleLinesMinorWidth
        {
            get { return _mScaleLinesMinorWidth; }
            set
            {
                if (_mScaleLinesMinorWidth == value) return;
                _mScaleLinesMinorWidth = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The step value of the major scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The step value of the major scale lines.")]
        public Single ScaleLinesMajorStepValue
        {
            get { return _mScaleLinesMajorStepValue; }
            set
            {
                if ((_mScaleLinesMajorStepValue == value) || (value <= 0)) return;
                _mScaleLinesMajorStepValue = Math.Max(Math.Min(value, _mMaxValue), _mMinValue);
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The color of the major scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the major scale lines.")]
        public Color ScaleLinesMajorColor
        {
            get { return _mScaleLinesMajorColor; }
            set
            {
                if (_mScaleLinesMajorColor == value) return;
                _mScaleLinesMajorColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The inner radius of the major scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The inner radius of the major scale lines.")]
        public Int32 ScaleLinesMajorInnerRadius
        {
            get { return _mScaleLinesMajorInnerRadius; }
            set
            {
                if (_mScaleLinesMajorInnerRadius == value) return;
                _mScaleLinesMajorInnerRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The outer radius of the major scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The outer radius of the major scale lines.")]
        public Int32 ScaleLinesMajorOuterRadius
        {
            get { return _mScaleLinesMajorOuterRadius; }
            set
            {
                if (_mScaleLinesMajorOuterRadius == value) return;
                _mScaleLinesMajorOuterRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The width of the major scale lines
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The width of the major scale lines.")]
        public Int32 ScaleLinesMajorWidth
        {
            get { return _mScaleLinesMajorWidth; }
            set
            {
                if (_mScaleLinesMajorWidth == value) return;
                _mScaleLinesMajorWidth = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The range index. set this to a value of 0 up to 4 to change the corresponding range's properties
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         RefreshProperties(RefreshProperties.All),
         Description("The range index. set this to a value of 0 up to 4 to change the corresponding range's properties."
             )]
        public Byte RangeIdx
        {
            get { return _mRangeIdx; }
            set
            {
                if ((_mRangeIdx == value) || (0 > value) || (value >= numofranges)) return;
                _mRangeIdx = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// Enables or disables the range selected by Range_Idx
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("Enables or disables the range selected by Range_Idx.")]
        public Boolean RangeEnabled
        {
            get { return _mRangeEnabled[_mRangeIdx]; }
            set
            {
                if (_mRangeEnabled[_mRangeIdx] == value) return;
                _mRangeEnabled[_mRangeIdx] = value;
                RangesEnabled = _mRangeEnabled;
                _drawGaugeBackground = true;
                Refresh();
            }
        }


        ///<summary>
        /// RangesEnabled
        ///</summary>
        [Browsable(false)]
        public Boolean[] RangesEnabled
        {
            get { return _mRangeEnabled; }
            set { _mRangeEnabled = value; }
        }

        ///<summary>
        /// The color of the range
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the range.")]
        public Color RangeColor
        {
            get { return _mRangeColor[_mRangeIdx]; }
            set
            {
                if (_mRangeColor[_mRangeIdx] == value) return;
                _mRangeColor[_mRangeIdx] = value;
                RangesColor = _mRangeColor;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// RangesColor
        ///</summary>
        [Browsable(false)]
        public Color[] RangesColor
        {
            get { return _mRangeColor; }
            set { _mRangeColor = value; }
        }

        ///<summary>
        /// The start value of the range, must be less than RangeEndValue
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The start value of the range, must be less than RangeEndValue.")]
        public Single RangeStartValue
        {
            get { return _mRangeStartValue[_mRangeIdx]; }
            set
            {
                if ((_mRangeStartValue[_mRangeIdx] == value) || (value >= _mRangeEndValue[_mRangeIdx])) return;
                _mRangeStartValue[_mRangeIdx] = value;
                RangesStartValue = _mRangeStartValue;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// RangesStartValue
        ///</summary>
        [Browsable(false)]
        public Single[] RangesStartValue
        {
            get { return _mRangeStartValue; }
            set { _mRangeStartValue = value; }
        }

        ///<summary>
        /// The end value of the range. Must be greater than RangeStartValue
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The end value of the range. Must be greater than RangeStartValue.")]
        public Single RangeEndValue
        {
            get { return _mRangeEndValue[_mRangeIdx]; }
            set
            {
                if ((_mRangeEndValue[_mRangeIdx] != value)
                    && (_mRangeStartValue[_mRangeIdx] < value))
                {
                    _mRangeEndValue[_mRangeIdx] = value;
                    RangesEndValue = _mRangeEndValue;
                    _drawGaugeBackground = true;
                    Refresh();
                }
            }
        }

        ///<summary>
        /// RangesEndValue
        ///</summary>
        [Browsable(false)]
        public Single[] RangesEndValue
        {
            get { return _mRangeEndValue; }
            set { _mRangeEndValue = value; }
        }

        ///<summary>
        /// The inner radius of the range
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The inner radius of the range.")]
        public Int32 RangeInnerRadius
        {
            get { return _mRangeInnerRadius[_mRangeIdx]; }
            set
            {
                if (_mRangeInnerRadius[_mRangeIdx] == value) return;
                _mRangeInnerRadius[_mRangeIdx] = value;
                RangesInnerRadius = _mRangeInnerRadius;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// RangesInnerRadius
        ///</summary>
        [Browsable(false)]
        public Int32[] RangesInnerRadius
        {
            get { return _mRangeInnerRadius; }
            set { _mRangeInnerRadius = value; }
        }

        ///<summary>
        /// The inner radius of the range
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The inner radius of the range.")]
        public Int32 RangeOuterRadius
        {
            get { return _mRangeOuterRadius[_mRangeIdx]; }
            set
            {
                if (_mRangeOuterRadius[_mRangeIdx] == value) return;
                _mRangeOuterRadius[_mRangeIdx] = value;
                RangesOuterRadius = _mRangeOuterRadius;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// RangesOuterRadius
        ///</summary>
        [Browsable(false)]
        public Int32[] RangesOuterRadius
        {
            get { return _mRangeOuterRadius; }
            set { _mRangeOuterRadius = value; }
        }

        ///<summary>
        /// The radius of the scale numbers
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The radius of the scale numbers.")]
        public Int32 ScaleNumbersRadius
        {
            get { return _mScaleNumbersRadius; }
            set
            {
                if (_mScaleNumbersRadius == value) return;
                _mScaleNumbersRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The color of the scale numbers
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The color of the scale numbers.")]
        public Color ScaleNumbersColor
        {
            get { return _mScaleNumbersColor; }
            set
            {
                if (_mScaleNumbersColor == value) return;
                _mScaleNumbersColor = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The format of the scale numbers
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The format of the scale numbers.")]
        public String ScaleNumbersFormat
        {
            get { return _mScaleNumbersFormat; }
            set
            {
                if (_mScaleNumbersFormat == value) return;
                _mScaleNumbersFormat = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The number of the scale line to start writing numbers next to
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The number of the scale line to start writing numbers next to.")]
        public Int32 ScaleNumbersStartScaleLine
        {
            get { return _mScaleNumbersStartScaleLine; }
            set
            {
                if (_mScaleNumbersStartScaleLine == value) return;
                _mScaleNumbersStartScaleLine = Math.Max(value, 1);
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The number of scale line steps for writing numbers
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The number of scale line steps for writing numbers.")]
        public Int32 ScaleNumbersStepScaleLines
        {
            get { return _mScaleNumbersStepScaleLines; }
            set
            {
                if (_mScaleNumbersStepScaleLines == value) return;
                _mScaleNumbersStepScaleLines = Math.Max(value, 1);
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The angle relative to the tangent of the base arc at a scale line that is used to rotate numbers. set to 0 for no rotation or e.g. set to 90
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The angle relative to the tangent of the base arc at a scale line that is used to rotate numbers. set to 0 for no rotation or e.g. set to 90."
             )]
        public Int32 ScaleNumbersRotation
        {
            get { return _mScaleNumbersRotation; }
            set
            {
                if (_mScaleNumbersRotation == value) return;
                _mScaleNumbersRotation = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The type of the needle, currently only type 0 and 1 are supported. Type 0 looks nicers but if you experience performance problems you might consider using type 1
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description(
             "The type of the needle, currently only type 0 and 1 are supported. Type 0 looks nicers but if you experience performance problems you might consider using type 1."
             )]
        public Int32 NeedleType
        {
            get { return _mNeedleType; }
            set
            {
                if (_mNeedleType == value) return;
                _mNeedleType = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The radius of the needle
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The radius of the needle.")]
        public Int32 NeedleRadius
        {
            get { return _mNeedleRadius; }
            set
            {
                if (_mNeedleRadius == value) return;
                _mNeedleRadius = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The first color of the needle
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The first color of the needle.")]
        public NeedleColorEnum NeedleColor1
        {
            get { return _mNeedleColor1; }
            set
            {
                if (_mNeedleColor1 == value) return;
                _mNeedleColor1 = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The second color of the needle
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The second color of the needle.")]
        public Color NeedleColor2
        {
            get { return _mNeedleColor2; }
            set
            {
                if (_mNeedleColor2 == value) return;
                _mNeedleColor2 = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        ///<summary>
        /// The width of the needle
        ///</summary>
        [Browsable(true),
         Category("AGauge"),
         Description("The width of the needle.")]
        public Int32 NeedleWidth
        {
            get { return _mNeedleWidth; }
            set
            {
                if (_mNeedleWidth == value) return;
                _mNeedleWidth = value;
                _drawGaugeBackground = true;
                Refresh();
            }
        }

        #endregion

        #region helper

        private void FindFontBounds()
        {
            //find upper and lower bounds for numeric characters
            Int32 c2;
            Graphics g;
            var backBrush = new SolidBrush(Color.White);
            var foreBrush = new SolidBrush(Color.Black);

            var b = new Bitmap(5, 5);
            g = Graphics.FromImage(b);
            var boundingBox = g.MeasureString("0123456789", Font, -1, StringFormat.GenericTypographic);
            b = new Bitmap((Int32) (boundingBox.Width), (Int32) (boundingBox.Height));
            g = Graphics.FromImage(b);
            g.FillRectangle(backBrush, 0.0F, 0.0F, boundingBox.Width, boundingBox.Height);
            g.DrawString("0123456789", Font, foreBrush, 0.0F, 0.0F, StringFormat.GenericTypographic);

            _fontBoundY1 = 0;
            _fontBoundY2 = 0;
            var c1 = 0;
            var boundfound = false;
            while ((c1 < b.Height) && (!boundfound))
            {
                c2 = 0;
                while ((c2 < b.Width) && (!boundfound))
                {
                    if (b.GetPixel(c2, c1) != backBrush.Color)
                    {
                        _fontBoundY1 = c1;
                        boundfound = true;
                    }
                    c2++;
                }
                c1++;
            }

            c1 = b.Height - 1;
            boundfound = false;
            while ((0 < c1) && (!boundfound))
            {
                c2 = 0;
                while ((c2 < b.Width) && (!boundfound))
                {
                    if (b.GetPixel(c2, c1) != backBrush.Color)
                    {
                        _fontBoundY2 = c1;
                        boundfound = true;
                    }
                    c2++;
                }
                c1--;
            }
        }

        #endregion

        #region base member overrides

        ///<summary>
        /// OnPaintBackground
        ///</summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        ///<summary>
        /// OnPaint
        ///</summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if ((Width < 10) || (Height < 10))
            {
                return;
            }

            Single countValue = 0;
            if (_drawGaugeBackground)
            {
                _drawGaugeBackground = false;

                FindFontBounds();

                _gaugeBitmap = new Bitmap(Width, Height, pe.Graphics);
                Graphics ggr = Graphics.FromImage(_gaugeBitmap);
                ggr.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

                if (BackgroundImage != null)
                {
                    switch (BackgroundImageLayout)
                    {
                        case ImageLayout.Center:
                            ggr.DrawImageUnscaled(BackgroundImage, Width/2 - BackgroundImage.Width/2,
                                                  Height/2 - BackgroundImage.Height/2);
                            break;
                        case ImageLayout.None:
                            ggr.DrawImageUnscaled(BackgroundImage, 0, 0);
                            break;
                        case ImageLayout.Stretch:
                            ggr.DrawImage(BackgroundImage, 0, 0, Width, Height);
                            break;
                        case ImageLayout.Tile:
                            Int32 pixelOffsetX = 0;
                            while (pixelOffsetX < Width)
                            {
                                var pixelOffsetY = 0;
                                while (pixelOffsetY < Height)
                                {
                                    ggr.DrawImageUnscaled(BackgroundImage, pixelOffsetX, pixelOffsetY);
                                    pixelOffsetY += BackgroundImage.Height;
                                }
                                pixelOffsetX += BackgroundImage.Width;
                            }
                            break;
                        case ImageLayout.Zoom:
                            if ((BackgroundImage.Width/Width) < (Single) (BackgroundImage.Height/Height))
                            {
                                ggr.DrawImage(BackgroundImage, 0, 0, Height, Height);
                            }
                            else
                            {
                                ggr.DrawImage(BackgroundImage, 0, 0, Width, Width);
                            }
                            break;
                    }
                }

                ggr.SmoothingMode = SmoothingMode.HighQuality;
                ggr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var gp = new GraphicsPath();
                for (var counter = 0; counter < numofranges; counter++)
                {
                    if (_mRangeEndValue[counter] <= _mRangeStartValue[counter] || !_mRangeEnabled[counter]) continue;
                    var rangeStartAngle = _mBaseArcStart +
                                             (_mRangeStartValue[counter] - _mMinValue)*_mBaseArcSweep/
                                             (_mMaxValue - _mMinValue);
                    var rangeSweepAngle = (_mRangeEndValue[counter] - _mRangeStartValue[counter])*_mBaseArcSweep/
                                             (_mMaxValue - _mMinValue);
                    gp.Reset();
                    gp.AddPie(
                        new Rectangle(_mCenter.X - _mRangeOuterRadius[counter],
                                      _mCenter.Y - _mRangeOuterRadius[counter], 2*_mRangeOuterRadius[counter],
                                      2*_mRangeOuterRadius[counter]), rangeStartAngle, rangeSweepAngle);
                    gp.Reverse();
                    gp.AddPie(
                        new Rectangle(_mCenter.X - _mRangeInnerRadius[counter],
                                      _mCenter.Y - _mRangeInnerRadius[counter], 2*_mRangeInnerRadius[counter],
                                      2*_mRangeInnerRadius[counter]), rangeStartAngle, rangeSweepAngle);
                    gp.Reverse();
                    ggr.SetClip(gp);
                    ggr.FillPie(new SolidBrush(_mRangeColor[counter]),
                                new Rectangle(_mCenter.X - _mRangeOuterRadius[counter],
                                              _mCenter.Y - _mRangeOuterRadius[counter],
                                              2*_mRangeOuterRadius[counter], 2*_mRangeOuterRadius[counter]),
                                rangeStartAngle, rangeSweepAngle);
                }

                ggr.SetClip(ClientRectangle);
                if (_mBaseArcRadius > 0)
                {
                    ggr.DrawArc(new Pen(_mBaseArcColor, _mBaseArcWidth),
                                new Rectangle(_mCenter.X - _mBaseArcRadius, _mCenter.Y - _mBaseArcRadius,
                                              2*_mBaseArcRadius, 2*_mBaseArcRadius), _mBaseArcStart, _mBaseArcSweep);
                }

                SizeF boundingBox;
                var counter1 = 0;
                while (countValue <= (_mMaxValue - _mMinValue))
                {
                    var valueText = (_mMinValue + countValue).ToString(_mScaleNumbersFormat);
                    ggr.ResetTransform();
                    boundingBox = ggr.MeasureString(valueText, Font, -1, StringFormat.GenericTypographic);

                    gp.Reset();
                    gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMajorOuterRadius,
                                                _mCenter.Y - _mScaleLinesMajorOuterRadius,
                                                2*_mScaleLinesMajorOuterRadius, 2*_mScaleLinesMajorOuterRadius));
                    gp.Reverse();
                    gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMajorInnerRadius,
                                                _mCenter.Y - _mScaleLinesMajorInnerRadius,
                                                2*_mScaleLinesMajorInnerRadius, 2*_mScaleLinesMajorInnerRadius));
                    gp.Reverse();
                    ggr.SetClip(gp);

                    ggr.DrawLine(new Pen(_mScaleLinesMajorColor, _mScaleLinesMajorWidth),
                                 (Center.X),
                                 (Center.Y),
                                 (Single)
                                 (Center.X +
                                  2*_mScaleLinesMajorOuterRadius*
                                  Math.Cos((_mBaseArcStart + countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue))*
                                           Math.PI/180.0)),
                                 (Single)
                                 (Center.Y +
                                  2*_mScaleLinesMajorOuterRadius*
                                  Math.Sin((_mBaseArcStart + countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue))*
                                           Math.PI/180.0)));

                    gp.Reset();
                    gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMinorOuterRadius,
                                                _mCenter.Y - _mScaleLinesMinorOuterRadius,
                                                2*_mScaleLinesMinorOuterRadius, 2*_mScaleLinesMinorOuterRadius));
                    gp.Reverse();
                    gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMinorInnerRadius,
                                                _mCenter.Y - _mScaleLinesMinorInnerRadius,
                                                2*_mScaleLinesMinorInnerRadius, 2*_mScaleLinesMinorInnerRadius));
                    gp.Reverse();
                    ggr.SetClip(gp);

                    if (countValue < (_mMaxValue - _mMinValue))
                    {
                        for (var counter2 = 1; counter2 <= _mScaleLinesMinorNumOf; counter2++)
                        {
                            if (((_mScaleLinesMinorNumOf%2) == 1) && ((_mScaleLinesMinorNumOf/2) + 1 == counter2))
                            {
                                gp.Reset();
                                gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesInterOuterRadius,
                                                            _mCenter.Y - _mScaleLinesInterOuterRadius,
                                                            2*_mScaleLinesInterOuterRadius,
                                                            2*_mScaleLinesInterOuterRadius));
                                gp.Reverse();
                                gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesInterInnerRadius,
                                                            _mCenter.Y - _mScaleLinesInterInnerRadius,
                                                            2*_mScaleLinesInterInnerRadius,
                                                            2*_mScaleLinesInterInnerRadius));
                                gp.Reverse();
                                ggr.SetClip(gp);

                                ggr.DrawLine(new Pen(_mScaleLinesInterColor, _mScaleLinesInterWidth),
                                             (Center.X),
                                             (Center.Y),
                                             (Single)
                                             (Center.X +
                                              2*_mScaleLinesInterOuterRadius*
                                              Math.Cos((_mBaseArcStart +
                                                        countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue) +
                                                        counter2*_mBaseArcSweep/
                                                        ((((_mMaxValue - _mMinValue)/_mScaleLinesMajorStepValue))*
                                                         (_mScaleLinesMinorNumOf + 1)))*Math.PI/180.0)),
                                             (Single)
                                             (Center.Y +
                                              2*_mScaleLinesInterOuterRadius*
                                              Math.Sin((_mBaseArcStart +
                                                        countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue) +
                                                        counter2*_mBaseArcSweep/
                                                        ((((_mMaxValue - _mMinValue)/_mScaleLinesMajorStepValue))*
                                                         (_mScaleLinesMinorNumOf + 1)))*Math.PI/180.0)));

                                gp.Reset();
                                gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMinorOuterRadius,
                                                            _mCenter.Y - _mScaleLinesMinorOuterRadius,
                                                            2*_mScaleLinesMinorOuterRadius,
                                                            2*_mScaleLinesMinorOuterRadius));
                                gp.Reverse();
                                gp.AddEllipse(new Rectangle(_mCenter.X - _mScaleLinesMinorInnerRadius,
                                                            _mCenter.Y - _mScaleLinesMinorInnerRadius,
                                                            2*_mScaleLinesMinorInnerRadius,
                                                            2*_mScaleLinesMinorInnerRadius));
                                gp.Reverse();
                                ggr.SetClip(gp);
                            }
                            else
                            {
                                ggr.DrawLine(new Pen(_mScaleLinesMinorColor, _mScaleLinesMinorWidth),
                                             (Center.X),
                                             (Center.Y),
                                             (Single)
                                             (Center.X +
                                              2*_mScaleLinesMinorOuterRadius*
                                              Math.Cos((_mBaseArcStart +
                                                        countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue) +
                                                        counter2*_mBaseArcSweep/
                                                        ((((_mMaxValue - _mMinValue)/_mScaleLinesMajorStepValue))*
                                                         (_mScaleLinesMinorNumOf + 1)))*Math.PI/180.0)),
                                             (Single)
                                             (Center.Y +
                                              2*_mScaleLinesMinorOuterRadius*
                                              Math.Sin((_mBaseArcStart +
                                                        countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue) +
                                                        counter2*_mBaseArcSweep/
                                                        ((((_mMaxValue - _mMinValue)/_mScaleLinesMajorStepValue))*
                                                         (_mScaleLinesMinorNumOf + 1)))*Math.PI/180.0)));
                            }
                        }
                    }

                    ggr.SetClip(ClientRectangle);

                    if (_mScaleNumbersRotation != 0)
                    {
                        ggr.TextRenderingHint = TextRenderingHint.AntiAlias;
                        ggr.RotateTransform(90.0F + _mBaseArcStart + countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue));
                    }

                    ggr.TranslateTransform(
                        (Single)
                        (Center.X +
                         _mScaleNumbersRadius*
                         Math.Cos((_mBaseArcStart + countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue))*Math.PI/180.0f)),
                        (Single)
                        (Center.Y +
                         _mScaleNumbersRadius*
                         Math.Sin((_mBaseArcStart + countValue*_mBaseArcSweep/(_mMaxValue - _mMinValue))*Math.PI/180.0f)),
                        MatrixOrder.Append);


                    if (counter1 >= ScaleNumbersStartScaleLine - 1)
                    {
                        ggr.DrawString(valueText, Font, new SolidBrush(_mScaleNumbersColor), -boundingBox.Width/2,
                                       -_fontBoundY1 - (_fontBoundY2 - _fontBoundY1 + 1)/2, StringFormat.GenericTypographic);
                    }

                    countValue += _mScaleLinesMajorStepValue;
                    counter1 ++;
                }

                ggr.ResetTransform();
                ggr.SetClip(ClientRectangle);

                if (_mScaleNumbersRotation != 0)
                {
                    ggr.TextRenderingHint = TextRenderingHint.SystemDefault;
                }

                for (var counter = 0; counter < numofcaps; counter++)
                {
                    if (_mCapText[counter] != "")
                    {
                        ggr.DrawString(_mCapText[counter], Font, new SolidBrush(CapColors[counter]),
                                       _mCapPosition[counter].X, _mCapPosition[counter].Y,
                                       StringFormat.GenericTypographic);
                    }
                }
            }

            pe.Graphics.DrawImageUnscaled(_gaugeBitmap, 0, 0);
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Single brushAngle =
                (Int32) (_mBaseArcStart + (_mValue - _mMinValue)*_mBaseArcSweep/(_mMaxValue - _mMinValue))%360;
            var needleAngle = brushAngle*Math.PI/180;

            switch (_mNeedleType)
            {
                case 0:
                    var points = new PointF[3];
                    var brush1 = Brushes.White;
                    var brush2 = Brushes.White;
                    var brush3 = Brushes.White;
                    var brush4 = Brushes.White;

                    var subcol = (Int32) (((brushAngle + 225)%180)*100/180);
                    var subcol2 = (Int32) (((brushAngle + 135)%180)*100/180);

                    pe.Graphics.FillEllipse(new SolidBrush(_mNeedleColor2), Center.X - _mNeedleWidth*3,
                                            Center.Y - _mNeedleWidth*3, _mNeedleWidth*6, _mNeedleWidth*6);
                    switch (_mNeedleColor1)
                    {
                        case NeedleColorEnum.Gray:
                            brush1 = new SolidBrush(Color.FromArgb(80 + subcol, 80 + subcol, 80 + subcol));
                            brush2 = new SolidBrush(Color.FromArgb(180 - subcol, 180 - subcol, 180 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(80 + subcol2, 80 + subcol2, 80 + subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(180 - subcol2, 180 - subcol2, 180 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Gray, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Red:
                            brush1 = new SolidBrush(Color.FromArgb(145 + subcol, subcol, subcol));
                            brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 100 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, subcol2, subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 100 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Red, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Green:
                            brush1 = new SolidBrush(Color.FromArgb(subcol, 145 + subcol, subcol));
                            brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 100 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 100 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Green, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Blue:
                            brush1 = new SolidBrush(Color.FromArgb(subcol, subcol, 145 + subcol));
                            brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 100 - subcol, 245 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(subcol2, subcol2, 145 + subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 100 - subcol2, 245 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Blue, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Magenta:
                            brush1 = new SolidBrush(Color.FromArgb(subcol, 145 + subcol, 145 + subcol));
                            brush2 = new SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 245 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, 145 + subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 245 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Magenta, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Violet:
                            brush1 = new SolidBrush(Color.FromArgb(145 + subcol, subcol, 145 + subcol));
                            brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 245 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, subcol2, 145 + subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 245 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Violet, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                        case NeedleColorEnum.Yellow:
                            brush1 = new SolidBrush(Color.FromArgb(145 + subcol, 145 + subcol, subcol));
                            brush2 = new SolidBrush(Color.FromArgb(245 - subcol, 245 - subcol, 100 - subcol));
                            brush3 = new SolidBrush(Color.FromArgb(145 + subcol2, 145 + subcol2, subcol2));
                            brush4 = new SolidBrush(Color.FromArgb(245 - subcol2, 245 - subcol2, 100 - subcol2));
                            pe.Graphics.DrawEllipse(Pens.Violet, Center.X - _mNeedleWidth*3, Center.Y - _mNeedleWidth*3,
                                                    _mNeedleWidth*6, _mNeedleWidth*6);
                            break;
                    }

                    if (Math.Floor((Single) (((brushAngle + 225)%360)/180.0)) == 0)
                    {
                        var brushBucket = brush1;
                        brush1 = brush2;
                        brush2 = brushBucket;
                    }

                    if (Math.Floor((Single) (((brushAngle + 135)%360)/180.0)) == 0)
                    {
                        brush4 = brush3;
                    }

                    points[0].X = (Single) (Center.X + _mNeedleRadius*Math.Cos(needleAngle));
                    points[0].Y = (Single) (Center.Y + _mNeedleRadius*Math.Sin(needleAngle));
                    points[1].X = (Single) (Center.X - _mNeedleRadius/20*Math.Cos(needleAngle));
                    points[1].Y = (Single) (Center.Y - _mNeedleRadius/20*Math.Sin(needleAngle));
                    points[2].X =
                        (Single)
                        (Center.X - _mNeedleRadius/5*Math.Cos(needleAngle) +
                         _mNeedleWidth*2*Math.Cos(needleAngle + Math.PI/2));
                    points[2].Y =
                        (Single)
                        (Center.Y - _mNeedleRadius/5*Math.Sin(needleAngle) +
                         _mNeedleWidth*2*Math.Sin(needleAngle + Math.PI/2));
                    pe.Graphics.FillPolygon(brush1, points);

                    points[2].X =
                        (Single)
                        (Center.X - _mNeedleRadius/5*Math.Cos(needleAngle) +
                         _mNeedleWidth*2*Math.Cos(needleAngle - Math.PI/2));
                    points[2].Y =
                        (Single)
                        (Center.Y - _mNeedleRadius/5*Math.Sin(needleAngle) +
                         _mNeedleWidth*2*Math.Sin(needleAngle - Math.PI/2));
                    pe.Graphics.FillPolygon(brush2, points);

                    points[0].X = (Single) (Center.X - (_mNeedleRadius/20 - 1)*Math.Cos(needleAngle));
                    points[0].Y = (Single) (Center.Y - (_mNeedleRadius/20 - 1)*Math.Sin(needleAngle));
                    points[1].X =
                        (Single)
                        (Center.X - _mNeedleRadius/5*Math.Cos(needleAngle) +
                         _mNeedleWidth*2*Math.Cos(needleAngle + Math.PI/2));
                    points[1].Y =
                        (Single)
                        (Center.Y - _mNeedleRadius/5*Math.Sin(needleAngle) +
                         _mNeedleWidth*2*Math.Sin(needleAngle + Math.PI/2));
                    points[2].X =
                        (Single)
                        (Center.X - _mNeedleRadius/5*Math.Cos(needleAngle) +
                         _mNeedleWidth*2*Math.Cos(needleAngle - Math.PI/2));
                    points[2].Y =
                        (Single)
                        (Center.Y - _mNeedleRadius/5*Math.Sin(needleAngle) +
                         _mNeedleWidth*2*Math.Sin(needleAngle - Math.PI/2));
                    pe.Graphics.FillPolygon(brush4, points);

                    points[0].X = (Single) (Center.X - _mNeedleRadius/20*Math.Cos(needleAngle));
                    points[0].Y = (Single) (Center.Y - _mNeedleRadius/20*Math.Sin(needleAngle));
                    points[1].X = (Single) (Center.X + _mNeedleRadius*Math.Cos(needleAngle));
                    points[1].Y = (Single) (Center.Y + _mNeedleRadius*Math.Sin(needleAngle));

                    pe.Graphics.DrawLine(new Pen(_mNeedleColor2), Center.X, Center.Y, points[0].X, points[0].Y);
                    pe.Graphics.DrawLine(new Pen(_mNeedleColor2), Center.X, Center.Y, points[1].X, points[1].Y);
                    break;
                case 1:
                    var startPoint = new Point((Int32) (Center.X - _mNeedleRadius/8*Math.Cos(needleAngle)),
                                               (Int32) (Center.Y - _mNeedleRadius/8*Math.Sin(needleAngle)));
                    var endPoint = new Point((Int32) (Center.X + _mNeedleRadius*Math.Cos(needleAngle)),
                                             (Int32) (Center.Y + _mNeedleRadius*Math.Sin(needleAngle)));

                    pe.Graphics.FillEllipse(new SolidBrush(_mNeedleColor2), Center.X - _mNeedleWidth*3,
                                            Center.Y - _mNeedleWidth*3, _mNeedleWidth*6, _mNeedleWidth*6);

                    switch (_mNeedleColor1)
                    {
                        case NeedleColorEnum.Gray:
                            pe.Graphics.DrawLine(new Pen(Color.DarkGray, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.DarkGray, _mNeedleWidth), Center.X, Center.Y,
                                                 startPoint.X, startPoint.Y);
                            break;
                        case NeedleColorEnum.Red:
                            pe.Graphics.DrawLine(new Pen(Color.Red, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Red, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                        case NeedleColorEnum.Green:
                            pe.Graphics.DrawLine(new Pen(Color.Green, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Green, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                        case NeedleColorEnum.Blue:
                            pe.Graphics.DrawLine(new Pen(Color.Blue, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Blue, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                        case NeedleColorEnum.Magenta:
                            pe.Graphics.DrawLine(new Pen(Color.Magenta, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Magenta, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                        case NeedleColorEnum.Violet:
                            pe.Graphics.DrawLine(new Pen(Color.Violet, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Violet, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                        case NeedleColorEnum.Yellow:
                            pe.Graphics.DrawLine(new Pen(Color.Yellow, _mNeedleWidth), Center.X, Center.Y, endPoint.X,
                                                 endPoint.Y);
                            pe.Graphics.DrawLine(new Pen(Color.Yellow, _mNeedleWidth), Center.X, Center.Y, startPoint.X,
                                                 startPoint.Y);
                            break;
                    }
                    break;
            }
        }

        ///<summary>
        /// OnResize
        ///</summary>
        protected override void OnResize(EventArgs e)
        {
            _drawGaugeBackground = true;
            Refresh();
        }

        #endregion
    }
}