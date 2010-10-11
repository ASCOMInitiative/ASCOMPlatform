using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// An attribute for declaratively associating an assembly, class or property with an ASCOM device.
	/// </summary>
	/// <remarks>
    /// This attribute is intended for use int two main situations:
    /// <list type="numbered">
    /// <item>Settings management and integration with Visual Studio designers</item>
    /// <description>
    /// When this attribute is placed on the driver's <c>Properties.Settings</c> class, it  propagates
    /// down to each of the settings properties. When the setting is passed to the 
    /// <c>ASCOM.SettingsProvider</c> class at runtime, the settings provider looks for this attribute
    /// to determine which settings hive to save the value in when it is passed to 
    /// <see cref="T:ASCOM.Utilities.Profile"/>.
    /// </description>
    /// <item>Deployment</item>
    /// <description>
    /// The values in this attribute could be used by an installer custom action to perform
    /// ASCOM registration during setup. Historically this has been handled programmatically,
    /// but there are trends towards a more declarative approach to deployment (for example
    /// WiX, Windows Installer Xml). It is expected that such an installer may need to obtain
    /// registration data by reflecting on the assemblies being installed. Placing this attribute
    /// at the assembly level will assist in this situation.
    /// </description>
    /// </list>
	/// </remarks>
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
		/// Gets the ASCOM DeviceID, also known as the COM ProgID.
		/// </summary>
		public string DeviceId { get; private set; }

        /// <summary>
        /// Gets or sets the display name of the device. This would be the short display name, as displayed in the ASCOM Chooser.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; set; }
	}
}
