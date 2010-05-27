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
	/// Cadences are based on 32-bit unsigned integers, such that the ordinal value
	/// of each item represents a bit mask that can be used directly in an update routine.
	/// </summary>
	public enum CadencePattern : uint
	{
		/// <summary>
		/// Permanently off, 
		/// appropriate for indication of a non-critical inactive state.
		/// </summary>
		SteadyOff = 0x00000000,

		/// <summary>
		/// Permanently on,
		/// appropriate for indication of a non-critical active state.
		/// </summary>
		SteadyOn = 0xFFFFFFFF,

		/// <summary>
		/// Fast blink,
		/// appropriate for indicating a state of hightened but non-critical alert.
		/// Usage example: during movement of robotic equipment.
		/// </summary>
		BlinkFast = 0xF0F0F0F0,

		/// <summary>
		/// Slow blink,
		/// appropriate for non-critical persistent conditions.
		/// Usage example: image exposure in progress.
		/// </summary>
		BlinkSlow = 0xFF00FF00,
		
		/// <summary>
		/// Very fast blink,
		/// appropriate for drawing attention to urgent conditions that require operator intervention.
		/// Usage example: Rain detected
		/// </summary>
		BlinkAlarm = 0xCCCCCCCC,

		/// <summary>
		/// Strobe is mostly off but with an occasional short blip on,
		/// appropriate for indicating non-critical ongoing steady idle state.
		/// </summary>
		Strobe = 0x00000001,

		/// <summary>
		/// Wink (mostly on with occasional short wink-off),
		/// appropriate for indicating non-critical ongoing steady active state.
		/// </summary>
		Wink = 0xFFFFFFFE
	}
}
