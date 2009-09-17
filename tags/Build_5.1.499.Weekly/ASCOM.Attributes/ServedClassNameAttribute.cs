using System;
using System.Collections.Generic;
using System.Text;


namespace ASCOM
{
	/// <summary>
	/// An attribute that confers a 'friendly name' on an assembly and marks it as loadable by LocalServer.
	/// The 'friendly name' is used by the ASCOM LocalServer to register the class with the ASCOM Chooser.
	/// The 'friendly name' is what gets displayed to the user in the driver selection combo box.
	/// This attribute is also used by the LocalServer to filter the assemblies that it will
	/// attempt to load at runtime. LocalServer will only load assemblies bearing this attribute.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class ServedClassNameAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the 'friendly name' of the served class, as registered with the ASCOM Chooser.
		/// </summary>
		/// <value>The 'friendly name' of the served class.</value>
		public string DisplayName { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="ServedClassNameAttribute"/> class.
		/// </summary>
		/// <param name="servedClassName">The 'friendly name' of the served class.</param>
		public ServedClassNameAttribute(string servedClassName)
		{
			DisplayName = servedClassName;
		}
	}
}
