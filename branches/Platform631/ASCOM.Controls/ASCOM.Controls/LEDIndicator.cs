using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Controls
{
    /// <summary>
    ///   Provides a status indicator modeled on a bi-colour red/green LED lamp.
    ///   The lamp can be red or green and  (traffic light colours) and
    ///   can be steady or can flash with a choice of different cadences.
    /// </summary>
    [DefaultProperty("LabelText")]
    public sealed class LedIndicator : UserControl, ICadencedControl
    {
        #region Non-designer fields and properties
        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly Container components;

        /// <summary>
        ///   Records the current cadence state of the control.
        ///   Used to short-cut display updates when they are unnecessary.
        /// </summary>
        private bool active;

        /// <summary>
        ///   When True, the LED indicator reflects the state of the Red, Green and Cadence settings.
        ///   When False, the LED appears inactive (steady off).
        /// </summary>
        private bool bPowerOn = true;

        /// <summary>
        ///   True when the instance has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///   Internal control used to display the LED's text label.
        /// </summary>
        private Label ledLabel;

        /// <summary>
        ///   Internal panel control that is used to display the LED's colour.
        /// </summary>
        private Panel ledPanel;
        #endregion

        #region Designer Properties

        /// <summary>
        ///   Gets or sets the LED's status (which controls its display colour).
        /// </summary>
        [Category("LED Properties"), DefaultValue(TrafficLight.Green)]
        [Description(
            "Sets the LEDs status which controls the display colour. LEDs support 'traffic light' colours where Green represents normal status, Yellow represents a warning condition and Red represents an error."
            )]
        public TrafficLight Status { get; set; }

        /// <summary>
        ///   Sets the text displayed alongside the indicator
        /// </summary>
        [Category("LED Properties")]
        [Description("Sets the text that appears next to the LED indicator.")]
        public string LabelText
        {
            get { return ledLabel.Text; }
            set { ledLabel.Text = value; }
        }

        /// <summary>
        ///   Sets or reads the 'power status' of the LED
        ///   When the LED is Enabled, it reflects the current colour settings and cadence.
        ///   When disabled, the LED appears off and cadencing is disabled.
        /// </summary>
        [Category("LED Properties")]
        [DefaultValue(true)]
        [Description(
            "Simulates a power switch. When disabled (false), the LED always has the 'off' appearance regardless of colour or cadence settings."
            )]
        public new bool Enabled
        {
            get { return bPowerOn; }
            set
            {
                if (disposed) throw new ObjectDisposedException("LedIndicator");
                bPowerOn = value;
                if (value)
                    StartCadenceUpdates();
                else
                    StopCadenceUpdates();
            }
        }

        /// <summary>
        ///   Gets or sets the LED cadence bitmap.
        ///   If the cadence has changed and is non-steady and the LED is enabled, then the cadence timer is started.
        /// </summary>
        /// <remarks>
        ///   Implements the <see cref = "ICadencedControl.Cadence" /> property.
        /// </remarks>
        [Category("LED Properties")]
        [DefaultValue(CadencePattern.SteadyOn)]
        [Description(
            "Sets the cadence (blinking pattern) of the LED indicator. Available cadences range from SteadyOff, through a number of alternating patterns of various urgency, to SteadyOn."
            )]
        public CadencePattern Cadence { get; set; }

        #endregion

        #region Construction, Destruction and Disposal

        /// <summary>
        ///   Default constructor for a new LEDIndicator object. Performs the default processing required
        ///   by the designer.
        /// </summary>
        public LedIndicator()
        {
            disposed = false;
            components = null;
            InitializeComponent();  // This call is required by the Windows.Forms Form Designer.
            Cadence = CadencePattern.SteadyOn;
            active = false;
            StartCadenceUpdates();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    StopCadenceUpdates();
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (ledPanel != null)
                    {
                        ledPanel.Dispose();
                        ledPanel = null;
                    }
                    if (ledLabel != null)
                    {
                        ledLabel.Dispose();
                        ledLabel = null;
                    }
                }
                disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        ///   Required method for Designer support - do not modify 
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ledPanel = new System.Windows.Forms.Panel();
            this.ledLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ledPanel
            // 
            this.ledPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ledPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledPanel.CausesValidation = false;
            this.ledPanel.Location = new System.Drawing.Point(0, 4);
            this.ledPanel.Name = "ledPanel";
            this.ledPanel.Size = new System.Drawing.Size(16, 8);
            this.ledPanel.TabIndex = 0;
            // 
            // ledLabel
            // 
            this.ledLabel.Anchor = ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                      | System.Windows.Forms.AnchorStyles.Right)));
            this.ledLabel.AutoSize = true;
            this.ledLabel.CausesValidation = false;
            this.ledLabel.Location = new System.Drawing.Point(24, 0);
            this.ledLabel.Name = "ledLabel";
            this.ledLabel.Size = new System.Drawing.Size(28, 13);
            this.ledLabel.TabIndex = 1;
            this.ledLabel.Text = "LED";
            this.ledLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LEDIndicator
            // 
            this.Controls.Add(this.ledLabel);
            this.Controls.Add(this.ledPanel);
            this.Name = "LedIndicator";
            this.Size = new System.Drawing.Size(56, 16);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region ICadencedControl Members

        /// <summary>
        ///   Refreshes the LED display, taking account of the power,
        ///   colour and cadence settings.
        /// </summary>
        /// <param name = "newstate">The new state of the control's appearance ('on' or 'off').</param>
        /// <remarks>
        ///   Implements the <see cref = "ICadencedControl.CadenceUpdate" /> method.
        ///   The <see cref = "CadenceManager" /> always calls this method on the GUI thread.
        /// </remarks>
        public void CadenceUpdate(bool newstate)
        {
            if (newstate == active) return; // Don't do anything if state hasn't changed.
            if (disposed) throw new ObjectDisposedException("Attempt to update a control after it has been disposed.");
            active = newstate; // Remember the new state for next time.
            if (newstate == false || Enabled == false) // Render the 'power off' appearance
                RenderOffAppearance();
            else
            {
                RenderOnAppearance();
            }
        }

        /// <summary>
        /// Renders the 'power off' appearance of the LED indicator.
        /// </summary>
        private void RenderOffAppearance()
        {
            SetColour(Color.WhiteSmoke);
        }

        /// <summary>
        /// Renders the 'power on' appearance of the LED indicator. The exact appearance depends on the <see cref="Status"/> property.
        /// </summary>
        private void RenderOnAppearance()
        {
            switch (Status) // Render the 'power on' appearance according to status.
            {
                case TrafficLight.Green:
                    SetColour(Color.LightGreen);
                    break;
                case TrafficLight.Yellow:
                    SetColour(Color.Orange);
                    break;
                case TrafficLight.Red:
                    SetColour(Color.Red);
                    break;
            }
        }

        #endregion

        /// <summary>
        ///   Sets the colour of the LED.
        ///   If the colour is changed, then the LED's panel control is invalidated to force a re-draw.
        /// </summary>
        /// <param name = "newColour">The new led colour.</param>
        private void SetColour(Color newColour)
        {
            if (newColour != ledPanel.BackColor)
            {
                ledPanel.BackColor = newColour;
                ledPanel.Invalidate();
            }
        }


        /// <summary>
        ///   Unregister from the <see cref = "CadenceManager" />.
        /// </summary>
        private void StopCadenceUpdates()
        {
            CadenceManager.Instance.Remove(this);
            //RenderOffAppearance();
        }

        /// <summary>
        ///   Register with the <see cref = "CadenceManager" />.
        /// </summary>
        private void StartCadenceUpdates()
        {
            CadenceManager.Instance.Add(this);
        }
    }
}