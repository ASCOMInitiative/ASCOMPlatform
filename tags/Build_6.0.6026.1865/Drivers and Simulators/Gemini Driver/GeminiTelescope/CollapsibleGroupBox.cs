using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Indigo
{
    /// <summary>
    /// GroupBox control that provides functionality to 
    /// allow it to be collapsed. 
    /// 
    /// Borrowed from: http://www.codeproject.com/KB/miscctrl/XPCollapsGroupBox.aspx
    /// adapted for Gemini Driver by [pk] 06-OCT-2009 -- changes in initial state, button images, text painting
    /// 
    /// </summary>
    [ToolboxBitmap(typeof(CollapsibleGroupBox))]
    public partial class CollapsibleGroupBox : GroupBox
    {
        #region Fields

        private Rectangle m_toggleRect = new Rectangle(8, 2, 11, 11);
        private Boolean m_collapsed = false;
        private Boolean m_bResizingFromCollapse = false;

        private const int m_collapsedHeight = 20;
        private Size m_FullSize = Size.Empty;

        #endregion

        #region Events & Delegates

        /// <summary>Fired when the Collapse Toggle button is pressed</summary>
        public delegate void CollapseBoxClickedEventHandler(object sender);
        public event CollapseBoxClickedEventHandler CollapseBoxClickedEvent;

        #endregion

        #region Constructor

        public CollapsibleGroupBox()
        {
            InitializeComponent();
            m_FullSize = this.Size;
        }

        #endregion

        #region Public Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FullHeight
        {
            get { return m_FullSize.Height; }
        }

        public Size FullSize
        {
            get { return m_FullSize; }
            set { m_FullSize = value; }
        }

        [DefaultValue(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCollapsed
        {
            get { return m_collapsed; }
            set
            {
                if (value != m_collapsed)
                {
                    m_collapsed = value;

                    if (!value)
                        // Expand
                        this.Size = m_FullSize;
                    else
                    {
                        // Collapse
                        m_bResizingFromCollapse = true;
                        this.Height = m_collapsedHeight;
                        m_bResizingFromCollapse = false;
                    }

                    foreach (Control c in Controls)
                        c.Visible = !value;

                    Invalidate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CollapsedHeight
        {
            get { return m_collapsedHeight; }
        }

        #endregion

        #region Overrides

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_toggleRect.Contains(e.Location))
                ToggleCollapsed();
            else
                base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            HandleResize();
            try
            {
                DrawGroupBox(e.Graphics);
                DrawToggleButton(e.Graphics);
            }
            catch { }
        }

        #endregion

        #region Implimentation

        void DrawGroupBox(Graphics g)
        {
            // Get windows to draw the GroupBox
            Rectangle bounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y + 6, ClientRectangle.Width, ClientRectangle.Height - 6);
            GroupBoxRenderer.DrawGroupBox(g, bounds, (Enabled && !IsCollapsed)? GroupBoxState.Normal : GroupBoxState.Disabled);

            // Text Formating positioning & Size
            StringFormat sf = new StringFormat();
            int i_textPos = (bounds.X + 8) + m_toggleRect.Width + 2;

            SizeF sz = g.MeasureString(Text, this.Font);
            int i_textSize = (int)sz.Width;
            i_textSize = i_textSize < 1 ? 1 : i_textSize;
            int i_endPos = i_textPos + i_textSize + 1;
            int i_textHeight = (int)sz.Height;

            // Draw a line to cover the GroupBox border where the text will sit
//            g.DrawLine(SystemPens.Control, i_textPos, bounds.Y, i_endPos, bounds.Y);

            // Draw the GroupBox text
            using (SolidBrush drawBrush = new SolidBrush(this.ForeColor))
            using (SolidBrush bkBrush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(bkBrush, i_textPos, 0, i_textSize, i_textHeight);
                g.DrawString(Text, this.Font, drawBrush, i_textPos, 0);
            }
        }

        void DrawToggleButton(Graphics g)
        {
            if(IsCollapsed)
                g.DrawImage(ASCOM.GeminiTelescope.Properties.Resources.plus, m_toggleRect);
            else
                g.DrawImage(ASCOM.GeminiTelescope.Properties.Resources.minus, m_toggleRect);
        }

        void ToggleCollapsed()
        {
            IsCollapsed = !IsCollapsed;

            if (CollapseBoxClickedEvent != null)
                CollapseBoxClickedEvent(this);
        }

        void HandleResize()
        {
            if (!m_bResizingFromCollapse && !m_collapsed)
                m_FullSize = this.Size;
        }

        #endregion
    }
}