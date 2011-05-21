using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// Defines an Attribute that is used to decorate classes that are to be served up by LocalServer.
	/// </summary>
    [global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ServedClassNameAttribute : Attribute
    {
        string servedClassName;
		/// <summary>
		/// Initializes an instance of a <see cref="ServedClassNameAttribute"/>
		/// </summary>
		/// <param name="servedClassName">The name of the class as it is to appear in the ASCOM Chooser.</param>
        public ServedClassNameAttribute(string servedClassName)
        {
            this.servedClassName = servedClassName;
        }
		/// <summary>
		/// Gets or sets the display name of the served class. This value is used to
		/// display the class name in the ASCOM Chooser.
		/// </summary>
        public string ServedClassName
        {
            get { return servedClassName; }
            set { servedClassName = value; }
        }
    }
}
