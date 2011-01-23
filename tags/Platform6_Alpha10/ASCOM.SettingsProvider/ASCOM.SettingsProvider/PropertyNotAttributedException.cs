using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// Exception thrown when teh SettingsProvider finds a setting that does not have a DeviceID attribute.
	/// </summary>
	[Serializable]
	public class PropertyNotAttributedException : DriverException
	{
		/// <summary>
		/// Gets or sets the name of the property that caused teh exception.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName { get; private set; }
		// constructors...
		#region PropertyNotAttributedException(string message)
		/// <summary>
		/// Constructs a new PropertyNotAttributedException.
		/// </summary>
        /// <param name="propertyName">The property name</param>
		public PropertyNotAttributedException(string propertyName)
			: base(String.Format("Setting [{0}] was missing a [DeviceIdAttribute].", propertyName), ErrorCodes.SettingsProviderError)
		{
			PropertyName = propertyName;
		}
		#endregion
	}
}