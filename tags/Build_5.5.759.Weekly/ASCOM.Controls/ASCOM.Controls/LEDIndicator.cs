using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace ASCOM.Controls
	{

	/// <summary>
	/// Provides an indicator lamp similar in appearance to a rectangular LED.
	/// The lamp can be red, yellow or green (traffic light colours) and
	/// can be steady or can flash with a choice of different cadences.
	/// </summary>
	[DefaultProperty("LabelText")]
	public class LEDIndicator : System.Windows.Forms.UserControl
		{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static ulong[] CadenceBitmap = { 0xffffffffffffffff, 0x0000000000000000, 0xf0f0f0f0f0f0f0f0, 
												 0xffff0000ffff0000, 0x3333333333333333, 0xfffffffffffffff0 };

		/// <summary>
		/// Default constructor for a new LEDIndicator object. Performs the default processing required
		/// by the designer.
		/// </summary>
		public LEDIndicator()
			{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
			{
			if (disposing)
				{
				if (components != null)
					{
					components.Dispose();
					}
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
			this.panelLED = new System.Windows.Forms.Panel();
			this.labelLED = new System.Windows.Forms.Label();
			this.timerCadence = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timerCadence)).BeginInit();
			this.SuspendLayout();
			// 
			// panelLED
			// 
			this.panelLED.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panelLED.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelLED.CausesValidation = false;
			this.panelLED.Location = new System.Drawing.Point(0, 4);
			this.panelLED.Name = "panelLED";
			this.panelLED.Size = new System.Drawing.Size(16, 8);
			this.panelLED.TabIndex = 0;
			// 
			// labelLED
			// 
			this.labelLED.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelLED.AutoSize = true;
			this.labelLED.CausesValidation = false;
			this.labelLED.Location = new System.Drawing.Point(24, 0);
			this.labelLED.Name = "labelLED";
			this.labelLED.Size = new System.Drawing.Size(26, 16);
			this.labelLED.TabIndex = 1;
			this.labelLED.Text = "LED";
			this.labelLED.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timerCadence
			// 
			this.timerCadence.SynchronizingObject = this;
			this.timerCadence.Elapsed += new System.Timers.ElapsedEventHandler(this.timerCadence_Elapsed);
			// 
			// LEDIndicator
			// 
			this.Controls.Add(this.labelLED);
			this.Controls.Add(this.panelLED);
			this.Name = "LEDIndicator";
			this.Size = new System.Drawing.Size(56, 16);
			((System.ComponentModel.ISupportInitialize)(this.timerCadence)).EndInit();
			this.ResumeLayout(false);

			}
		#endregion

		/// <summary>
		/// True if the RED portion of the LED is active
		/// </summary>
		private bool bRedOn;
		/// <summary>
		/// True if the green portion of the LED is on
		/// </summary>
		private bool bGreenOn;
		/// <summary>
		/// When True, the LED indicator reflects the state of the Red, Green and Cadence settings.
		/// When False, the LED appears inactive (steady off).
		/// </summary>
		private bool bPowerOn = true;
		/// <summary>
		/// While bPowerOn is true, the cadence bit controls whether the LED is on or off.
		/// For the LED to be On, bPowerOn and bCadenceBit must both be true, and at least one of
		/// bRedOn and bGreenOn must be true.
		/// </summary>
		private bool bCadenceBit;
		/// <summary>
		/// 64-bit cadence bitmap that determines the duty cycle of the LED.
		/// Defaults to steady on cadence (not flashing).
		/// </summary>
		private CadencePattern eCadence = CadencePattern.SteadyOn;
		/// <summary>
		/// Working copy of the cadence bitmap, used as a shift register.
		/// </summary>
		private UInt64 i64ShiftReg = 0xFFFFFFFFFFFFFFFF;
		/// <summary>
		/// Counter that keeps track of the position of the shift register.
		/// Counts down from 64 and when it decrements to zero, the shift register
		/// is reloaded from the cadence bitmap.
		/// </summary>
		private int nBit = 64;
		/// <summary>
		/// Internal panel control that is used to display the LED's colour.
		/// </summary>
		private System.Windows.Forms.Panel panelLED;
		/// <summary>
		/// Internal control used to display the LED's text label.
		/// </summary>
		private System.Windows.Forms.Label labelLED;
		/// <summary>
		/// Internal timer object used to time the flashing cadence of the LED.
		/// </summary>
		private System.Timers.Timer timerCadence;
		/// <summary>
		/// The number of system clock ticks per cadence bit.
		/// Determines the flashing speed of an LED indicator.
		/// Defaults to 500ms per bit.
		/// </summary>
		private uint uiTicksPerCadenceBit = 100;

		/// <summary>
		/// Gets or sets the red component of the LED
		/// </summary>
		[Category("LED Properties")]
		[DefaultValue(false)]
		public bool Red
			{
			get
				{
				return bRedOn;
				}
			set
				{
				bool bPrevious = bRedOn;
				bRedOn = value;
				ConfigureCadence(bRedOn, bPrevious);
				}
			}

		/// <summary>
		/// Gets or sets the Green component of the LED
		/// </summary>
		[Category("LED Properties")]
		[DefaultValue(true)]
		public bool Green
			{
			get
				{
				return bGreenOn;
				}
			set
				{
				bool bPrevious = bGreenOn;
				bGreenOn = value;
				ConfigureCadence(bGreenOn, bPrevious);
				}
			}

		/// <summary>
		/// Refreshes the LED display, taking account of the power,
		/// colour and cadence settings.
		/// </summary>
		private void RefreshLED()
			{
			Color clrLED;
			if (bPowerOn && bCadenceBit)
				{
				if (bRedOn)
					{
					if (bGreenOn)
						{
						clrLED = Color.Orange;
						}
					else
						{
						clrLED = Color.Red;
						}
					}
				else
					{
					if (bGreenOn)
						{
						clrLED = Color.LightGreen;
						}
					else
						{
						clrLED = Color.WhiteSmoke;
						}
					}
				}
			else
				{
				clrLED = Color.WhiteSmoke;
				}

			if (clrLED != this.panelLED.BackColor)
				{
				this.SetColour(clrLED);
				}
			}
		/// <summary>
		/// Defines the delegate used to set the LED's display colour.
		/// This is used to update the control in a thread-safe manner.
		/// </summary>
		/// <param name="ledColour">The desired LED colour.</param>
		private delegate void SetColourDelegate(Color ledColour);
		/// <summary>
		/// Sets the colour of the LED. This method is thread-safe.
		/// This is intended primarily for use in the Tick event of the
		/// cadence timer.
		/// </summary>
		/// <param name="ledColour">The led colour.</param>
		private void SetColour(Color ledColour)
			{
			if (this.panelLED.InvokeRequired)
				{
				this.Invoke(new SetColourDelegate(SetColour), new object[] { ledColour });
				}
			else if (ledColour != this.panelLED.BackColor)
				{
				this.panelLED.BackColor = ledColour;
				}
			}
		/// <summary>
		/// Updates the LED on/off status based on the specified cadence pattern.
		/// If the cadence has been changed to a continuous pattern, then the timer is disabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerCadence_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
			if (eCadence == CadencePattern.SteadyOff)
				{
				DisableCadence();
				bCadenceBit = false;
				}
			else if (eCadence == CadencePattern.SteadyOn)
				{
				DisableCadence();
				bCadenceBit = true;
				}
			else
				{

				bCadenceBit = ((i64ShiftReg & 0x8000000000000000) == 0x8000000000000000);	// Set the LED on/off control bit
				if (--nBit == 0)									// Decrement and test bit counter
					{
					ReloadCadenceShiftRegister();
					}
				else
					{
					i64ShiftReg <<= 1;								// Sift the shift register 1 bit left.
					i64ShiftReg |= (bCadenceBit ? 1UL : 0UL);
					}
				}
			RefreshLED();
			}

		/// <summary>
		/// Sets the text displayed alongside the indicator
		/// </summary>
		[Category("LED Properties")]
		public string LabelText
			{
			get
				{
				return this.labelLED.Text;
				}
			set
				{
				this.labelLED.Text = value;
				}
			}

		/// <summary>
		/// Sets or reads the 'power status' of the LED
		/// When the LED is Enabled, it reflects the current colour settings and cadence.
		/// When disabled, the LED appears off and cadencing is disabled.
		/// </summary>
		[Category("LED Properties")]
		public new bool Enabled
			{
			get
				{
				return bPowerOn;
				}
			set
				{
				bool bPreviousState = bPowerOn;		// Used to detect a state change
				bPowerOn = value;
				ConfigureCadence(bPowerOn, bPreviousState);
				}
			}

		/// <summary>
		/// Compares the two objects for inequality (change of state).
		/// If there is a difference, the LED cadence needs to be configured
		/// and the display refreshed.
		/// </summary>
		/// <param name="oOld">Old object value</param>
		/// <param name="oNew">New object value</param>
		private void ConfigureCadence(object oOld, object oNew)
			{
			// Uncomment the following lines to disable cadence display
			// while in the forms designer.
			//if (this.DesignMode)
			//    {
			//    DisableCadence();
			//    RefreshLED();
			//    return;
			//    }

			if (oOld != oNew)		// Only update LED status on change of state
				{
				if (bPowerOn)
					{
					if (eCadence == CadencePattern.SteadyOn || eCadence == CadencePattern.SteadyOff)
						{
						DisableCadence();
						}
					else
						{
						EnableCadence();
						}
					}
				else
					{
					DisableCadence();
					}
				RefreshLED();
				}
			}

		/// <summary>
		/// LED cadence mask for steady-on
		/// </summary>
		public const UInt64 cCadenceSteadyOn = 0xFFFFFFFFFFFFFFFF;
		/// <summary>
		/// LED cadence mask for steady-off
		/// </summary>
		public const UInt64 cCadenceSteadyOff = 0x0000000000000000;
		/// <summary>
		/// LED cadence mask for slow blink
		/// </summary>
		public const UInt64 cCadenceBlinkSlow = 0xFFFFFFFF00000000;
		/// <summary>
		/// LED cadence mask for fast blink
		/// </summary>
		public const UInt64 cCadenceBlinkFast = 0xF0F0F0F0F0F0F0F0;

		/// <summary>
		/// Configures and starts the cadence timer
		/// </summary>
		private void EnableCadence()
			{
			ReloadCadenceShiftRegister();
			this.timerCadence.Interval = (double)uiTicksPerCadenceBit;
			this.timerCadence.AutoReset = true;
			this.timerCadence.Start();	// anable cadence timer
			}
		/// <summary>
		/// Reloads the cadence shift register based on the user-selected cadence pattern
		/// and resets the shift register bit counter.
		/// </summary>
		private void ReloadCadenceShiftRegister()
			{
			nBit = 64;
			i64ShiftReg = CadenceBitmap[(int)eCadence];
			}

		/// <summary>
		/// Stops the cadence timer, effectively disabling cadencing.
		/// </summary>
		private void DisableCadence()
			{
			this.timerCadence.Stop();
			if (eCadence == CadencePattern.SteadyOn)
				{
				bCadenceBit = true;
				}
			else
				{
				bCadenceBit = false;
				}
			}

		/// <summary>
		/// Gets or sets the LED cadence bitmap.
		/// If the cadence has changed and is non-steady and the LED is enabled, then the cadence timer is started.
		/// </summary>
		[Category("LED Properties")]
		public CadencePattern Cadence
			{
			get
				{
				return eCadence;
				}
			set
				{
				CadencePattern eOldCadence = eCadence;
				eCadence = value;
				if (eCadence != eOldCadence)	// Change of cadence value?
					{
					if (this.Enabled)				// indicator is enabled
						{
						if (Cadence != CadencePattern.SteadyOff && eCadence != CadencePattern.SteadyOn)	// Cadence is non-steady
							{
							EnableCadence();
							}
						}
					}
				}
			}
		}
	}
