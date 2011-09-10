using System;

namespace ASCOM
{
    /// <summary>
    ///   An attribute for declaratively associating an assembly, class or property with an 
    ///   ASCOM device ID (and optionally, a display name).
    /// </summary>
    /// <remarks>
    ///   This attribute is intended for use in two main situations:
    ///   <list type = "number">
    ///     <item>
    ///       <term>
    ///         Settings management and integration with Visual Studio designers
    ///       </term>
    ///       <description>
    ///         When this attribute is placed on the driver's <c>Properties.Settings</c> class, it  propagates
    ///         down to each of the settings properties. When the setting is passed to the 
    ///         <c>ASCOM.SettingsProvider</c> class at runtime, the settings provider looks for this attribute
    ///         to determine which settings hive to save the value in when it is passed to 
    ///         <see cref = "T:ASCOM.Utilities.Profile" />.
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <term>
    ///         Deployment
    ///       </term>
    ///       <description>
    ///         The values in this attribute could be used by an installer custom action to perform
    ///         ASCOM registration during setup. Historically this has been handled programmatically,
    ///         but there are trends towards a more declarative approach to deployment (for example
    ///         WiX, Windows Installer Xml). It is expected that such an installer may need to obtain
    ///         registration data by reflecting on the assemblies being installed. Placing this attribute
    ///         at the assembly level will assist in this situation.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Property, Inherited = true,
        AllowMultiple = false)]
    public sealed class DeviceIdAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DeviceIdAttribute" /> class.
        /// </summary>
        /// <param name = "deviceId">The ASCOM device ID (aka COM ProgID) to be associated with the class.</param>
        /// <remarks>
        ///   <para>
        ///     Recommended usage is:
        ///     <example>
        ///       [DeviceId("ASCOM.SuperDuper.Telescope", DeviceName="SuperDuper Deluxe")]
        ///     </example>
        ///   </para>
        ///   <para>
        ///     In the event that the DeviceName optional parameter is not set, it will return the DeviceId.
        ///   </para>
        /// </remarks>
        public DeviceIdAttribute(string deviceId)
        {
            DeviceId = deviceId;
            // DeviceName defaults to be the same as the ID, unless the user changes it later. This ensures that there is
            // always *something* available for the Chooser to use as the display name, should the user neglect to set it.
            DeviceName = deviceId;
        }

        /// <summary>
        ///   Gets the ASCOM DeviceID, also known as the COM ProgID.
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        ///   Gets or sets the display name of the device. This would be the short display name, as displayed in the ASCOM Chooser.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; set; }
    }
}