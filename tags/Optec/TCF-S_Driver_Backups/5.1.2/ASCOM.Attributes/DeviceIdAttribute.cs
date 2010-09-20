using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// An attribute for specifying an associated ASCOM DeviceID (aka COM ProgID).
	/// This is intended primarily for use with the ASCOM.SettingsProvider class.
	/// This attribute is placed on the driver's <c>Properties.Settings</c> class, which propagates
	/// down to each of the settings properties. When the setting is passed to the
	/// ASCOM.SettingsProvider class at runtime, the settings provider looks for this attribute
	/// to determine which settings hive to save the value in when it is passed to 
	/// <see cref="T:ASCOM.Utilities.Profile"/>.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class DeviceIdAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceIdAttribute"/> class.
		/// </summary>
		/// <param name="deviceId">The ASCOM device ID (aka COM ProgID) to be associated with the class.</param>
		public DeviceIdAttribute(string deviceId)
		{
			this.DeviceId = deviceId;
		}

		/// <summary>
		/// Gets the ASCOM DeviceID that the attribute is associated with.
		/// </summary>
		public string DeviceId { get; private set; }
	}
}
