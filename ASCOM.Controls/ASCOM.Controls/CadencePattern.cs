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
	/// Cadence patterns for blinking LEDs.
	/// </summary>
	public enum CadencePattern
	{
		/// <summary>
		/// LED permanently on (when enabled). Appropriate for indication of simple on/off status.
		/// </summary>
		SteadyOn = 0,
		/// <summary>
		/// LED permanently off, even when enabled. Appropriate for indication of simple on/off status.
		/// </summary>
		SteadyOff = 1,
		/// <summary>
		/// Fast blink, appropriate for non-critical ongoing change of state (eg Slewing).
		/// </summary>
		BlinkFast = 2,
		/// <summary>
		/// Slow blink, appropriate for non-critical persistent conditions (e.g. image exposure in progress).
		/// </summary>
		BlinkSlow = 3,
		/// <summary>
		/// Very fast blink, appropriate for drawing attention to urgent conditions that require operator intervention.
		/// </summary>
		BlinkAlarm = 4,
		/// <summary>
		/// Wink (mostly on with occasional short wink-off), appropriate for indicating non-critical ongoing steady state.
		/// </summary>
		Wink = 5
	}
}
